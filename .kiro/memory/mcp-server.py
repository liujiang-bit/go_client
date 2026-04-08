#!/usr/bin/env python3
"""
kiro-mem MCP Server — 3-layer progressive memory search
Wraps kiro-mem.py's SQLite+FTS5 backend as MCP tools.

3-Layer Workflow (10x token savings):
  1. mem_search   — FTS5 index → compact table (~50-100 tokens/result)
  2. mem_timeline — Context around anchor point (~100-200 tokens/result)
  3. mem_get      — Full details by IDs (~500-1000 tokens/result)

Write tools:
  mem_store    — Store new observation
  mem_summary  — Store/get session summaries

Utility:
  mem_stats    — Database statistics
"""
import sys, os, json, sqlite3, math, traceback, threading, socket
from datetime import datetime, timezone
from pathlib import Path
from http.server import HTTPServer, BaseHTTPRequestHandler
from urllib.parse import urlparse, parse_qs

# ── MCP protocol helpers (JSON-RPC over stdio) ──────────────

def _write_msg(data):
    """Write a JSON-RPC message as a single JSON line.
    Kiro MCP uses raw JSON lines (no Content-Length framing).
    """
    body = json.dumps(data, ensure_ascii=False) + "\n"
    sys.stdout.buffer.write(body.encode("utf-8"))
    sys.stdout.buffer.flush()

def send_response(id, result):
    _write_msg({"jsonrpc": "2.0", "id": id, "result": result})

def send_error(id, code, message):
    _write_msg({"jsonrpc": "2.0", "id": id, "error": {"code": code, "message": message}})

def read_message():
    """Read a JSON-RPC message from stdin.
    Supports raw JSON lines (Kiro) and Content-Length framing (standard MCP).
    """
    while True:
        raw = sys.stdin.buffer.readline()
        if not raw:
            return None  # EOF
        line = raw.decode("utf-8", errors="replace").strip()
        if not line:
            continue
        
        # Raw JSON line mode (Kiro's protocol)
        if line.startswith("{"):
            try:
                return json.loads(line)
            except json.JSONDecodeError:
                pass
        
        # Content-Length framing mode (standard MCP)
        if "content-length" in line.lower():
            headers = {}
            key, val = line.split(":", 1)
            headers[key.strip().lower()] = val.strip()
            while True:
                raw2 = sys.stdin.buffer.readline()
                if not raw2:
                    return None
                h = raw2.decode("utf-8", errors="replace").strip()
                if h == "":
                    break
                if ":" in h:
                    k, v = h.split(":", 1)
                    headers[k.strip().lower()] = v.strip()
            length = int(headers.get("content-length", 0))
            if length == 0:
                continue
            body = sys.stdin.buffer.read(length)
            try:
                return json.loads(body.decode("utf-8"))
            except json.JSONDecodeError:
                continue
    body = buf.read(length)
    return json.loads(body.decode("utf-8"))


# ── Database layer (reuse kiro-mem.py schema) ────────────────

DB_DIR = Path(__file__).parent
DB_PATH = DB_DIR / "kiro-mem.db"
CHARS_PER_TOKEN = 4

TYPE_ICONS = {
    "discovery": "🔵", "bugfix": "🔴", "feature": "🟣",
    "decision": "⚖️", "change": "✅", "refactor": "🔄",
}

def get_db():
    db = sqlite3.connect(str(DB_PATH))
    db.row_factory = sqlite3.Row
    db.execute("PRAGMA journal_mode=WAL")
    db.execute("PRAGMA foreign_keys=ON")
    return db

def ensure_db():
    """Auto-init DB if it doesn't exist."""
    if DB_PATH.exists():
        return
    import importlib.util
    spec = importlib.util.spec_from_file_location("kiro_mem", str(DB_DIR / "kiro-mem.py"))
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    db = get_db()
    db.executescript(mod.SCHEMA_SQL)
    db.execute("INSERT OR REPLACE INTO schema_version(version) VALUES(?)", (mod.SCHEMA_VERSION,))
    db.commit()
    db.close()
    log("Database initialized")

def estimate_tokens(text):
    if not text: return 0
    return math.ceil(len(text) / CHARS_PER_TOKEN)

def fmt_time(created_at):
    if not created_at: return ""
    return created_at[5:16].replace("T", " ")

def fmt_date(created_at):
    if not created_at: return ""
    return created_at[:10]

def date_to_epoch(date_str):
    dt = datetime.strptime(date_str, "%Y-%m-%d")
    return int(dt.replace(tzinfo=timezone.utc).timestamp())

def log(msg):
    sys.stderr.write(f"[kiro-mem] {msg}\n")
    sys.stderr.flush()


# ── Tool implementations ─────────────────────────────────────

def tool_search(args):
    """Layer 1: FTS5 search → compact index (~50-100 tokens/result)
    
    Returns a table of ID/time/type/title — enough to decide which
    observations to drill into with mem_timeline or mem_get.
    """
    db = get_db()
    query = args.get("query")
    limit = min(args.get("limit", 20), 100)
    obs_type = args.get("type")
    project = args.get("project")
    concept = args.get("concept")
    date_start = args.get("date_start")
    date_end = args.get("date_end")

    params = []
    where_parts = []
    if obs_type:
        types = [t.strip() for t in obs_type.split(",")]
        where_parts.append(f"o.type IN ({','.join('?' for _ in types)})")
        params.extend(types)
    if project:
        where_parts.append("o.project = ?")
        params.append(project)
    if date_start:
        where_parts.append("o.created_at_epoch >= ?")
        params.append(date_to_epoch(date_start))
    if date_end:
        where_parts.append("o.created_at_epoch <= ?")
        params.append(date_to_epoch(date_end))
    if concept:
        for c in [x.strip() for x in concept.split(",")]:
            where_parts.append("o.concepts LIKE ?")
            params.append(f'%"{c}"%')

    where_sql = (" AND " + " AND ".join(where_parts)) if where_parts else ""

    if query:
        # Sanitize FTS5 query: wrap bare terms in quotes if needed
        fts_query = query
        sql = f"""
            SELECT o.id, o.type, o.title, o.summary, o.files,
                   o.created_at, o.created_at_epoch
            FROM observations_fts fts
            JOIN observations o ON o.id = fts.rowid
            WHERE observations_fts MATCH ?{where_sql}
            ORDER BY fts.rank LIMIT ?
        """
        all_params = [fts_query] + params + [limit]
    else:
        if where_parts:
            where_sql = "WHERE " + " AND ".join(where_parts)
        else:
            where_sql = ""
        sql = f"""
            SELECT o.id, o.type, o.title, o.summary, o.files,
                   o.created_at, o.created_at_epoch
            FROM observations o {where_sql}
            ORDER BY o.created_at_epoch DESC LIMIT ?
        """
        all_params = params + [limit]

    try:
        rows = db.execute(sql, all_params).fetchall()
    except sqlite3.OperationalError as e:
        # FTS5 query syntax error — retry with quoted query
        if query and "fts5" in str(e).lower():
            fts_query = f'"{query}"'
            all_params[0] = fts_query
            rows = db.execute(sql, all_params).fetchall()
        else:
            raise
    finally:
        db.close()

    total = len(rows)
    lines = [f"Found {total} result(s)" + (f' for "{query}"' if query else "")]
    if total == 0:
        lines.append("\nTip: try broader terms, or omit query to see recent observations.")
        return "\n".join(lines)

    lines.append("")

    # Group by date for readability
    by_date = {}
    for r in rows:
        day = fmt_date(r["created_at"])
        by_date.setdefault(day, []).append(r)

    for day, day_rows in by_date.items():
        lines.append(f"### {day}")
        lines.append("")
        lines.append("| ID | Time | T | Title | ~Tok |")
        lines.append("|----|------|---|-------|------|")
        for r in day_rows:
            icon = TYPE_ICONS.get(r["type"], "?")
            t = fmt_time(r["created_at"])
            tokens = estimate_tokens((r["title"] or "") + (r["summary"] or ""))
            lines.append(f"| #{r['id']} | {t} | {icon} | {r['title']} | ~{tokens} |")
        lines.append("")

    lines.append(f"→ Use `mem_timeline(anchor=ID)` for context, `mem_get(ids=[...])` for full details")
    return "\n".join(lines)


def tool_timeline(args):
    """Layer 2: Context around an anchor point."""
    db = get_db()
    anchor = args.get("anchor")
    query = args.get("query")
    before = min(args.get("before", 5), 30)
    after = min(args.get("after", 5), 30)
    project = args.get("project")

    proj_filter = "AND project = ?" if project else ""
    proj_params = [project] if project else []

    anchor_id = None
    anchor_epoch = None

    if anchor:
        anchor_id = int(anchor)
        row = db.execute("SELECT created_at_epoch FROM observations WHERE id=?",
                         (anchor_id,)).fetchone()
        if not row:
            db.close()
            return f"Observation #{anchor_id} not found"
        anchor_epoch = row["created_at_epoch"]
    elif query:
        try:
            fts_q = query
            row = db.execute(
                "SELECT o.id, o.created_at_epoch "
                "FROM observations_fts fts "
                "JOIN observations o ON o.id = fts.rowid "
                "WHERE observations_fts MATCH ? "
                "ORDER BY fts.rank LIMIT 1", (fts_q,)).fetchone()
        except sqlite3.OperationalError:
            fts_q = f'"{query}"'
            row = db.execute(
                "SELECT o.id, o.created_at_epoch "
                "FROM observations_fts fts "
                "JOIN observations o ON o.id = fts.rowid "
                "WHERE observations_fts MATCH ? "
                "ORDER BY fts.rank LIMIT 1", (fts_q,)).fetchone()
        if not row:
            db.close()
            return f'No observations matching "{query}"'
        anchor_id = row["id"]
        anchor_epoch = row["created_at_epoch"]
    else:
        db.close()
        return "Error: provide anchor (ID) or query (text)"

    # Get boundary epochs via ID-based window
    before_rows = db.execute(
        f"SELECT created_at_epoch FROM observations "
        f"WHERE id <= ? {proj_filter} ORDER BY id DESC LIMIT ?",
        [anchor_id] + proj_params + [before + 1]).fetchall()

    after_rows = db.execute(
        f"SELECT created_at_epoch FROM observations "
        f"WHERE id >= ? {proj_filter} ORDER BY id ASC LIMIT ?",
        [anchor_id] + proj_params + [after + 1]).fetchall()

    start_epoch = before_rows[-1]["created_at_epoch"] if before_rows else anchor_epoch
    end_epoch = after_rows[-1]["created_at_epoch"] if after_rows else anchor_epoch

    # Fetch all records in window
    obs = db.execute(
        f"SELECT id, type, title, summary, files, created_at, created_at_epoch "
        f"FROM observations "
        f"WHERE created_at_epoch >= ? AND created_at_epoch <= ? {proj_filter} "
        f"ORDER BY created_at_epoch ASC",
        [start_epoch, end_epoch] + proj_params).fetchall()

    sums = db.execute(
        f"SELECT id, user_request, completed, next_steps, created_at, created_at_epoch "
        f"FROM summaries "
        f"WHERE created_at_epoch >= ? AND created_at_epoch <= ? {proj_filter} "
        f"ORDER BY created_at_epoch ASC",
        [start_epoch, end_epoch] + proj_params).fetchall()
    db.close()

    # Merge and sort chronologically
    items = []
    for r in obs:
        items.append(("obs", dict(r)))
    for r in sums:
        items.append(("sum", dict(r)))
    items.sort(key=lambda x: x[1].get("created_at_epoch", 0))

    # Format output
    lines = []
    if query:
        lines.append(f'# Timeline for "{query}"')
        lines.append(f"Anchor: #{anchor_id}")
    else:
        lines.append(f"# Timeline around #{anchor_id}")
    lines.append(f"Window: {before} before / {after} after | Items: {len(items)}")
    lines.append("")

    by_date = {}
    for kind, data in items:
        day = fmt_date(data.get("created_at", ""))
        by_date.setdefault(day, []).append((kind, data))

    for day, day_items in by_date.items():
        lines.append(f"### {day}")
        lines.append("")
        has_obs_table = False
        for kind, data in day_items:
            if kind == "sum":
                if has_obs_table:
                    lines.append("")
                    has_obs_table = False
                req = data.get("user_request") or "Session summary"
                lines.append(f"  S#{data['id']} {req}")
                if data.get("completed"):
                    lines.append(f"  Completed: {data['completed']}")
                lines.append("")
            else:
                if not has_obs_table:
                    lines.append("| ID | Time | T | Title | Summary | ~Tok |")
                    lines.append("|----|------|---|-------|---------|------|")
                    has_obs_table = True
                icon = TYPE_ICONS.get(data["type"], "?")
                t = fmt_time(data["created_at"])
                summary_preview = (data.get("summary") or "")[:80]
                if len(data.get("summary") or "") > 80:
                    summary_preview += "..."
                tokens = estimate_tokens((data.get("title") or "") + (data.get("summary") or ""))
                marker = " **ANCHOR**" if data["id"] == anchor_id else ""
                lines.append(f"| #{data['id']} | {t} | {icon} | {data['title']}{marker} | {summary_preview} | ~{tokens} |")
        if has_obs_table:
            lines.append("")

    lines.append(f"-> Use `mem_get(ids=[...])` for full details of specific observations")
    return "\n".join(lines)


def tool_get(args):
    """Layer 3: Full details by IDs (~500-1000 tokens/result)"""
    db = get_db()
    ids = args.get("ids", [])
    if isinstance(ids, str):
        ids = [int(x.strip()) for x in ids.split(",")]
    ids = [int(x) for x in ids]
    if not ids:
        db.close()
        return "Error: provide ids array, e.g. ids=[1,2,3]"
    if len(ids) > 50:
        db.close()
        return "Error: max 50 IDs per request. Filter with mem_search first."
    placeholders = ",".join("?" for _ in ids)
    rows = db.execute(
        f"SELECT * FROM observations WHERE id IN ({placeholders}) "
        f"ORDER BY created_at_epoch DESC", ids).fetchall()
    db.close()

    if not rows:
        return f"No observations found for IDs: {ids}"

    lines = []
    for r in rows:
        d = dict(r)
        icon = TYPE_ICONS.get(d["type"], "?")
        lines.append(f"## [{icon} #{d['id']}] {d['title']}")
        lines.append(f"- Type: {d['type']}")
        lines.append(f"- Time: {d['created_at']}")
        if d.get("project"):
            lines.append(f"- Project: {d['project']}")
        if d.get("files"):
            try:
                files = json.loads(d["files"])
                lines.append(f"- Files: {', '.join(files)}")
            except Exception:
                lines.append(f"- Files: {d['files']}")
        if d.get("concepts"):
            try:
                concepts = json.loads(d["concepts"])
                lines.append(f"- Concepts: {', '.join(concepts)}")
            except Exception:
                lines.append(f"- Concepts: {d['concepts']}")
        lines.append("")
        if d.get("summary"):
            lines.append(f"Summary: {d['summary']}")
            lines.append("")
        if d.get("detail"):
            lines.append(f"Detail: {d['detail']}")
            lines.append("")
        lines.append("---")
        lines.append("")
    return "\n".join(lines)

def tool_store(args):
    """Store a new observation."""
    db = get_db()
    title = args.get("title")
    obs_type = args.get("type", "discovery")
    if not title:
        db.close()
        return json.dumps({"ok": False, "error": "title is required"})

    files = args.get("files", "")
    if isinstance(files, list):
        files_json = json.dumps(files)
    elif files:
        files_json = json.dumps([f.strip() for f in files.split(",") if f.strip()])
    else:
        files_json = "[]"

    concepts = args.get("concepts", "")
    if isinstance(concepts, list):
        concepts_json = json.dumps(concepts)
    elif concepts:
        concepts_json = json.dumps([c.strip() for c in concepts.split(",") if c.strip()])
    else:
        concepts_json = "[]"

    cur = db.execute(
        "INSERT INTO observations "
        "(session_id,project,type,title,summary,detail,files,concepts) "
        "VALUES (?,?,?,?,?,?,?,?)",
        (args.get("session_id", ""), args.get("project", ""),
         obs_type, title, args.get("summary", ""),
         args.get("detail", ""), files_json, concepts_json))
    obs_id = cur.lastrowid
    db.commit()
    db.close()
    return json.dumps({"ok": True, "id": obs_id})

def tool_summary(args):
    """Store or get session summaries."""
    db = get_db()
    action = args.get("action", "get")
    if action == "store":
        cur = db.execute(
            "INSERT INTO summaries "
            "(session_id,project,user_request,investigated,"
            "learned,completed,next_steps) "
            "VALUES (?,?,?,?,?,?,?)",
            (args.get("session_id", ""), args.get("project", ""),
             args.get("user_request", ""), args.get("investigated", ""),
             args.get("learned", ""), args.get("completed", ""),
             args.get("next_steps", "")))
        db.commit()
        result = json.dumps({"ok": True, "id": cur.lastrowid})
        db.close()
        return result
    else:
        limit = min(args.get("limit", 1), 20)
        project = args.get("project")
        if project:
            rows = db.execute(
                "SELECT * FROM summaries WHERE project=? "
                "ORDER BY created_at_epoch DESC LIMIT ?",
                (project, limit)).fetchall()
        else:
            rows = db.execute(
                "SELECT * FROM summaries "
                "ORDER BY created_at_epoch DESC LIMIT ?",
                (limit,)).fetchall()
        db.close()
        lines = []
        for r in rows:
            d = dict(r)
            lines.append(f"## Session #{d['id']} ({d['created_at']})")
            for key, label in [
                ("user_request", "Request"),
                ("investigated", "Investigated"),
                ("learned", "Learned"),
                ("completed", "Completed"),
                ("next_steps", "Next"),
            ]:
                if d.get(key):
                    lines.append(f"{label}: {d[key]}")
            lines.append("")
        return "\n".join(lines) if lines else "No summaries found."

def tool_stats(_args):
    """Database statistics."""
    db = get_db()
    total = db.execute("SELECT COUNT(*) c FROM observations").fetchone()["c"]
    by_type = db.execute(
        "SELECT type, COUNT(*) c FROM observations GROUP BY type").fetchall()
    sessions = db.execute("SELECT COUNT(*) c FROM summaries").fetchone()["c"]
    latest = db.execute(
        "SELECT created_at FROM observations "
        "ORDER BY created_at_epoch DESC LIMIT 1").fetchone()
    earliest = db.execute(
        "SELECT created_at FROM observations "
        "ORDER BY created_at_epoch ASC LIMIT 1").fetchone()
    db.close()
    types = {r["type"]: r["c"] for r in by_type}
    result = {
        "total_observations": total,
        "by_type": types,
        "total_summaries": sessions,
        "earliest": earliest["created_at"] if earliest else None,
        "latest": latest["created_at"] if latest else None,
    }
    return json.dumps(result, ensure_ascii=False)


# ── Embedded Web Viewer HTTP Server ──────────────────────────

VIEWER_HTML = DB_DIR / "viewer.html"
VIEWER_PORT = 8765
_viewer_server = None
_viewer_thread = None

def _find_free_port(start=8765, end=8785):
    """Find a free port in range."""
    for port in range(start, end):
        try:
            with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
                s.bind(("127.0.0.1", port))
                return port
        except OSError:
            continue
    return None

class _ViewerHandler(BaseHTTPRequestHandler):
    def log_message(self, fmt, *args):
        pass

    def _json_resp(self, data, status=200):
        body = json.dumps(data, ensure_ascii=False).encode("utf-8")
        self.send_response(status)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(body)))
        self.send_header("Access-Control-Allow-Origin", "*")
        self.end_headers()
        self.wfile.write(body)

    def _serve_html(self):
        if VIEWER_HTML.exists():
            body = VIEWER_HTML.read_bytes()
            self.send_response(200)
            self.send_header("Content-Type", "text/html; charset=utf-8")
            self.send_header("Content-Length", str(len(body)))
            self.end_headers()
            self.wfile.write(body)
        else:
            self._json_resp({"error": "viewer.html not found"}, 404)

    def _api_observations(self, params):
        db = get_db()
        query = params.get("q", [None])[0]
        obs_type = params.get("type", [None])[0]
        project = params.get("project", [None])[0]
        limit = int(params.get("limit", ["50"])[0])
        offset = int(params.get("offset", ["0"])[0])
        sql_params, where_parts = [], []
        if obs_type:
            types = [t.strip() for t in obs_type.split(",")]
            where_parts.append(f"o.type IN ({','.join('?' for _ in types)})")
            sql_params.extend(types)
        if project:
            where_parts.append("o.project = ?")
            sql_params.append(project)
        if query:
            where_sql = (" AND " + " AND ".join(where_parts)) if where_parts else ""
            sql = f"SELECT o.* FROM observations_fts fts JOIN observations o ON o.id=fts.rowid WHERE observations_fts MATCH ?{where_sql} ORDER BY fts.rank LIMIT ? OFFSET ?"
            all_p = [query] + sql_params + [limit, offset]
        else:
            where_sql = ("WHERE " + " AND ".join(where_parts)) if where_parts else ""
            sql = f"SELECT o.* FROM observations o {where_sql} ORDER BY o.created_at_epoch DESC LIMIT ? OFFSET ?"
            all_p = sql_params + [limit, offset]
        try:
            rows = db.execute(sql, all_p).fetchall()
        except sqlite3.OperationalError:
            if query:
                all_p[0] = f'"{query}"'
                rows = db.execute(sql, all_p).fetchall()
            else:
                raise
        db.close()
        results = []
        for r in rows:
            d = dict(r)
            for f in ("files", "concepts"):
                if d.get(f):
                    try: d[f] = json.loads(d[f])
                    except: pass
            results.append(d)
        return results

    def _api_summaries(self, params):
        db = get_db()
        limit = int(params.get("limit", ["20"])[0])
        offset = int(params.get("offset", ["0"])[0])
        rows = db.execute("SELECT * FROM summaries ORDER BY created_at_epoch DESC LIMIT ? OFFSET ?", (limit, offset)).fetchall()
        db.close()
        return [dict(r) for r in rows]

    def do_GET(self):
        parsed = urlparse(self.path)
        path = parsed.path
        params = parse_qs(parsed.query)
        if path == "/" or path == "/index.html":
            self._serve_html()
        elif path == "/api/stats":
            self._json_resp(json.loads(tool_stats({})))
        elif path == "/api/observations":
            self._json_resp(self._api_observations(params))
        elif path == "/api/summaries":
            self._json_resp(self._api_summaries(params))
        elif path == "/api/projects":
            db = get_db()
            rows = db.execute("SELECT DISTINCT project FROM observations WHERE project!='' ORDER BY project").fetchall()
            db.close()
            self._json_resp([r["project"] for r in rows])
        elif path == "/api/types":
            db = get_db()
            rows = db.execute("SELECT type, COUNT(*) c FROM observations GROUP BY type ORDER BY c DESC").fetchall()
            summary_count = db.execute("SELECT COUNT(*) c FROM summaries").fetchone()["c"]
            db.close()
            db_counts = {r["type"]: r["c"] for r in rows}
            ALL_TYPES = ["discovery", "bugfix", "feature", "decision", "change", "refactor"]
            results = []
            for t in ALL_TYPES:
                results.append({"type": t, "count": db_counts.get(t, 0), "icon": TYPE_ICONS.get(t, "?")})
            results.append({"type": "summary", "count": summary_count, "icon": "🎯"})
            self._json_resp(results)
        elif path.startswith("/api/observation/"):
            try:
                oid = int(path.split("/")[-1])
                db = get_db()
                row = db.execute("SELECT * FROM observations WHERE id=?", (oid,)).fetchone()
                db.close()
                if row:
                    d = dict(row)
                    for f in ("files", "concepts"):
                        if d.get(f):
                            try: d[f] = json.loads(d[f])
                            except: pass
                    self._json_resp(d)
                else:
                    self._json_resp({"error": "not found"}, 404)
            except ValueError:
                self._json_resp({"error": "invalid id"}, 400)
        else:
            self._json_resp({"error": "not found"}, 404)


def start_viewer_server():
    """Start web viewer HTTP server in a daemon thread."""
    global _viewer_server, _viewer_thread, VIEWER_PORT
    if _viewer_thread and _viewer_thread.is_alive():
        return VIEWER_PORT
    port = _find_free_port(VIEWER_PORT, VIEWER_PORT + 20)
    if not port:
        log("Web viewer: no free port found")
        return None
    VIEWER_PORT = port
    try:
        _viewer_server = HTTPServer(("127.0.0.1", port), _ViewerHandler)
        _viewer_thread = threading.Thread(target=_viewer_server.serve_forever, daemon=True)
        _viewer_thread.start()
        log(f"Web viewer started at http://127.0.0.1:{port}")
        return port
    except Exception as e:
        log(f"Web viewer failed to start: {e}")
        return None


def tool_viewer(args):
    """Open or check status of the web viewer."""
    action = args.get("action", "status")
    global _viewer_server, _viewer_thread
    if action == "open":
        import webbrowser
        if _viewer_thread and _viewer_thread.is_alive():
            url = f"http://127.0.0.1:{VIEWER_PORT}"
            webbrowser.open(url)
            return f"Opened {url} in browser"
        else:
            port = start_viewer_server()
            if port:
                url = f"http://127.0.0.1:{port}"
                webbrowser.open(url)
                return f"Started and opened {url}"
            return "Failed to start web viewer"
    else:
        if _viewer_thread and _viewer_thread.is_alive():
            return json.dumps({"running": True, "url": f"http://127.0.0.1:{VIEWER_PORT}"})
        return json.dumps({"running": False})


# ── MCP Tool definitions ─────────────────────────────────────

TOOLS = [
    {
        "name": "mem_search",
        "description": (
            "Layer 1 (START HERE): FTS5 full-text search. "
            "Returns compact index table with ID/time/type/title (~50-100 tokens/result). "
            "Supports AND/OR/NOT in query. Filter by type, project, concept, date range. "
            "ALWAYS use this first, then mem_timeline or mem_get for details."
        ),
        "inputSchema": {
            "type": "object",
            "properties": {
                "query": {"type": "string", "description": "FTS5 search (AND/OR/NOT). Omit for recent."},
                "type": {"type": "string", "description": "Filter: discovery,bugfix,feature,decision,change,refactor"},
                "project": {"type": "string", "description": "Filter by project name"},
                "concept": {"type": "string", "description": "Filter by concept tag (comma-separated)"},
                "date_start": {"type": "string", "description": "Start date YYYY-MM-DD"},
                "date_end": {"type": "string", "description": "End date YYYY-MM-DD"},
                "limit": {"type": "integer", "description": "Max results (default 20, max 100)"}
            }
        }
    },
    {
        "name": "mem_timeline",
        "description": (
            "Layer 2: Chronological context around an anchor observation. "
            "Shows observations + session summaries in time window with summary previews. "
            "Use after mem_search to understand what was happening around a result."
        ),
        "inputSchema": {
            "type": "object",
            "properties": {
                "anchor": {"type": "integer", "description": "Observation ID to center around"},
                "query": {"type": "string", "description": "Auto-find anchor via FTS5 (alternative to anchor)"},
                "before": {"type": "integer", "description": "Items before anchor (default 5, max 30)"},
                "after": {"type": "integer", "description": "Items after anchor (default 5, max 30)"},
                "project": {"type": "string", "description": "Filter by project"}
            }
        }
    },
    {
        "name": "mem_get",
        "description": (
            "Layer 3: Full details by IDs (~500-1000 tokens/result). "
            "Returns complete observation with summary, detail, files, concepts. "
            "ONLY use after filtering with mem_search/mem_timeline. Max 50 IDs."
        ),
        "inputSchema": {
            "type": "object",
            "properties": {
                "ids": {
                    "type": "array",
                    "items": {"type": "integer"},
                    "description": "Observation IDs to fetch (max 50)"
                }
            },
            "required": ["ids"]
        }
    },
    {
        "name": "mem_store",
        "description": (
            "Store a new observation. "
            "Types: discovery, bugfix, feature, decision, change, refactor."
        ),
        "inputSchema": {
            "type": "object",
            "properties": {
                "type": {"type": "string", "enum": ["discovery","bugfix","feature","decision","change","refactor"]},
                "title": {"type": "string", "description": "Short title (required)"},
                "summary": {"type": "string", "description": "Brief summary"},
                "detail": {"type": "string", "description": "Full details"},
                "files": {"type": "array", "items": {"type": "string"}, "description": "Related file paths"},
                "concepts": {"type": "array", "items": {"type": "string"}, "description": "Concept tags"},
                "project": {"type": "string"},
                "session_id": {"type": "string"}
            },
            "required": ["type", "title"]
        }
    },
    {
        "name": "mem_summary",
        "description": "Store or retrieve session summaries.",
        "inputSchema": {
            "type": "object",
            "properties": {
                "action": {"type": "string", "enum": ["store", "get"]},
                "session_id": {"type": "string"},
                "project": {"type": "string"},
                "user_request": {"type": "string"},
                "investigated": {"type": "string"},
                "learned": {"type": "string"},
                "completed": {"type": "string"},
                "next_steps": {"type": "string"},
                "limit": {"type": "integer", "description": "For get: max summaries (default 1)"}
            }
        }
    },
    {
        "name": "mem_stats",
        "description": "Database statistics: total observations, by type, summaries, date range.",
        "inputSchema": {"type": "object", "properties": {}}
    },
    {
        "name": "mem_viewer",
        "description": (
            "Web viewer for browsing memories in browser. "
            "action='status' to check if running, action='open' to open in browser."
        ),
        "inputSchema": {
            "type": "object",
            "properties": {
                "action": {"type": "string", "enum": ["status", "open"],
                           "description": "status=check running state, open=launch in browser"}
            }
        }
    }
]

TOOL_HANDLERS = {
    "mem_search": tool_search,
    "mem_timeline": tool_timeline,
    "mem_get": tool_get,
    "mem_store": tool_store,
    "mem_summary": tool_summary,
    "mem_stats": tool_stats,
    "mem_viewer": tool_viewer,
}


# ── MCP Server main loop ─────────────────────────────────────

SERVER_INFO = {
    "name": "kiro-mem",
    "version": "2.1.0",
}

def handle_request(msg):
    """Handle a single JSON-RPC request."""
    method = msg.get("method")
    id = msg.get("id")
    params = msg.get("params", {})

    if method == "initialize":
        send_response(id, {
            "protocolVersion": "2024-11-05",
            "capabilities": {"tools": {}},
            "serverInfo": SERVER_INFO,
        })
    elif method == "notifications/initialized":
        pass  # no response needed
    elif method == "tools/list":
        send_response(id, {"tools": TOOLS})
    elif method == "tools/call":
        tool_name = params.get("name")
        arguments = params.get("arguments", {})
        handler = TOOL_HANDLERS.get(tool_name)
        if not handler:
            send_response(id, {
                "content": [{"type": "text", "text": f"Unknown tool: {tool_name}"}],
                "isError": True
            })
        else:
            try:
                result_text = handler(arguments)
                send_response(id, {
                    "content": [{"type": "text", "text": result_text}]
                })
            except Exception as e:
                log(f"Tool error [{tool_name}]: {traceback.format_exc()}")
                send_response(id, {
                    "content": [{"type": "text", "text": f"Error in {tool_name}: {e}"}],
                    "isError": True
                })
    elif method == "ping":
        send_response(id, {})
    else:
        if id is not None:
            send_error(id, -32601, f"Method not found: {method}")

def main():
    """MCP stdio server main loop."""
    ensure_db()
    log(f"kiro-mem MCP server v{SERVER_INFO['version']} started (db: {DB_PATH})")

    # Auto-start web viewer in background
    port = start_viewer_server()
    if port:
        log(f"Web viewer: http://127.0.0.1:{port}")

    while True:
        try:
            msg = read_message()
            if msg is None:
                break
            handle_request(msg)
        except json.JSONDecodeError as e:
            log(f"JSON parse error: {e}")
        except Exception as e:
            log(f"Unhandled error: {traceback.format_exc()}")


if __name__ == "__main__":
    main()

#!/usr/bin/env python3
"""
kiro-mem Web Viewer — 轻量级记忆浏览器
Python HTTP 服务器 + 单文件 HTML 前端，直接查询 SQLite 数据库。

用法:
  python .kiro/memory/web-viewer.py [--port 8765]
  然后浏览器打开 http://localhost:8765
"""
import sys, os, json, sqlite3, math, argparse, webbrowser, mimetypes
from http.server import HTTPServer, BaseHTTPRequestHandler
from urllib.parse import urlparse, parse_qs
from pathlib import Path
from datetime import datetime, timezone

DB_DIR = Path(__file__).parent
DB_PATH = DB_DIR / "kiro-mem.db"
VIEWER_HTML = DB_DIR / "viewer.html"

TYPE_ICONS = {
    "discovery": "🔵", "bugfix": "🔴", "feature": "🟣",
    "decision": "⚖️", "change": "✅", "refactor": "🔄",
}

def get_db():
    db = sqlite3.connect(str(DB_PATH))
    db.row_factory = sqlite3.Row
    db.execute("PRAGMA journal_mode=WAL")
    return db

def date_to_epoch(date_str):
    dt = datetime.strptime(date_str, "%Y-%m-%d")
    return int(dt.replace(tzinfo=timezone.utc).timestamp())


# ── API handlers ──────────────────────────────────────────────

def api_stats():
    db = get_db()
    total = db.execute("SELECT COUNT(*) c FROM observations").fetchone()["c"]
    by_type = db.execute(
        "SELECT type, COUNT(*) c FROM observations GROUP BY type").fetchall()
    summaries = db.execute("SELECT COUNT(*) c FROM summaries").fetchone()["c"]
    latest = db.execute(
        "SELECT created_at FROM observations ORDER BY created_at_epoch DESC LIMIT 1").fetchone()
    earliest = db.execute(
        "SELECT created_at FROM observations ORDER BY created_at_epoch ASC LIMIT 1").fetchone()
    db.close()
    return {
        "total_observations": total,
        "by_type": {r["type"]: r["c"] for r in by_type},
        "total_summaries": summaries,
        "earliest": earliest["created_at"] if earliest else None,
        "latest": latest["created_at"] if latest else None,
    }

def api_observations(params):
    db = get_db()
    query = params.get("q", [None])[0]
    obs_type = params.get("type", [None])[0]
    project = params.get("project", [None])[0]
    limit = int(params.get("limit", ["50"])[0])
    offset = int(params.get("offset", ["0"])[0])

    sql_params = []
    where_parts = []
    if obs_type:
        types = [t.strip() for t in obs_type.split(",")]
        where_parts.append(f"o.type IN ({','.join('?' for _ in types)})")
        sql_params.extend(types)
    if project:
        where_parts.append("o.project = ?")
        sql_params.append(project)

    if query:
        where_sql = (" AND " + " AND ".join(where_parts)) if where_parts else ""
        sql = f"""
            SELECT o.* FROM observations_fts fts
            JOIN observations o ON o.id = fts.rowid
            WHERE observations_fts MATCH ?{where_sql}
            ORDER BY fts.rank LIMIT ? OFFSET ?
        """
        all_params = [query] + sql_params + [limit, offset]
    else:
        where_sql = ("WHERE " + " AND ".join(where_parts)) if where_parts else ""
        sql = f"""
            SELECT o.* FROM observations o {where_sql}
            ORDER BY o.created_at_epoch DESC LIMIT ? OFFSET ?
        """
        all_params = sql_params + [limit, offset]

    try:
        rows = db.execute(sql, all_params).fetchall()
    except sqlite3.OperationalError:
        if query:
            all_params[0] = f'"{query}"'
            rows = db.execute(sql, all_params).fetchall()
        else:
            raise
    db.close()

    results = []
    for r in rows:
        d = dict(r)
        for field in ("files", "concepts"):
            if d.get(field):
                try: d[field] = json.loads(d[field])
                except: pass
        results.append(d)
    return results

def api_summaries(params):
    db = get_db()
    limit = int(params.get("limit", ["20"])[0])
    offset = int(params.get("offset", ["0"])[0])
    rows = db.execute(
        "SELECT * FROM summaries ORDER BY created_at_epoch DESC LIMIT ? OFFSET ?",
        (limit, offset)).fetchall()
    db.close()
    return [dict(r) for r in rows]

def api_observation_detail(obs_id):
    db = get_db()
    row = db.execute("SELECT * FROM observations WHERE id=?", (obs_id,)).fetchone()
    db.close()
    if not row:
        return None
    d = dict(row)
    for field in ("files", "concepts"):
        if d.get(field):
            try: d[field] = json.loads(d[field])
            except: pass
    return d

def api_projects():
    db = get_db()
    rows = db.execute(
        "SELECT DISTINCT project FROM observations WHERE project != '' ORDER BY project"
    ).fetchall()
    db.close()
    return [r["project"] for r in rows]

def api_types():
    db = get_db()
    rows = db.execute(
        "SELECT type, COUNT(*) c FROM observations GROUP BY type ORDER BY c DESC"
    ).fetchall()
    summary_count = db.execute("SELECT COUNT(*) c FROM summaries").fetchone()["c"]
    db.close()

    # Build counts from DB
    db_counts = {r["type"]: r["c"] for r in rows}

    # Always show all 6 types in fixed order, even if count is 0
    ALL_TYPES = ["discovery", "bugfix", "feature", "decision", "change", "refactor"]
    results = []
    for t in ALL_TYPES:
        results.append({
            "type": t,
            "count": db_counts.get(t, 0),
            "icon": TYPE_ICONS.get(t, "?"),
        })

    # Append summary as a virtual type
    results.append({
        "type": "summary",
        "count": summary_count,
        "icon": "🎯",
    })

    return results


# ── HTTP Server ───────────────────────────────────────────────

class ViewerHandler(BaseHTTPRequestHandler):
    def log_message(self, format, *args):
        pass  # suppress default logging

    def _json_response(self, data, status=200):
        body = json.dumps(data, ensure_ascii=False).encode("utf-8")
        self.send_response(status)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(body)))
        self.send_header("Access-Control-Allow-Origin", "*")
        self.end_headers()
        self.wfile.write(body)

    def do_GET(self):
        parsed = urlparse(self.path)
        path = parsed.path
        params = parse_qs(parsed.query)

        # API routes
        if path == "/api/stats":
            self._json_response(api_stats())
        elif path == "/api/observations":
            self._json_response(api_observations(params))
        elif path == "/api/summaries":
            self._json_response(api_summaries(params))
        elif path == "/api/projects":
            self._json_response(api_projects())
        elif path == "/api/types":
            self._json_response(api_types())
        elif path.startswith("/api/observation/"):
            try:
                obs_id = int(path.split("/")[-1])
                result = api_observation_detail(obs_id)
                if result:
                    self._json_response(result)
                else:
                    self._json_response({"error": "not found"}, 404)
            except ValueError:
                self._json_response({"error": "invalid id"}, 400)
        elif path == "/" or path == "/index.html":
            # Serve viewer HTML
            html_path = VIEWER_HTML
            if html_path.exists():
                body = html_path.read_bytes()
                self.send_response(200)
                self.send_header("Content-Type", "text/html; charset=utf-8")
                self.send_header("Content-Length", str(len(body)))
                self.end_headers()
                self.wfile.write(body)
            else:
                self._json_response({"error": "viewer.html not found"}, 404)
        else:
            self._json_response({"error": "not found"}, 404)


def main():
    parser = argparse.ArgumentParser(description="kiro-mem Web Viewer")
    parser.add_argument("--port", type=int, default=8765, help="Port (default: 8765)")
    parser.add_argument("--no-open", action="store_true", help="Don't auto-open browser")
    args = parser.parse_args()

    if not DB_PATH.exists():
        print(f"Error: Database not found at {DB_PATH}")
        print("Run: python .kiro/memory/kiro-mem.py init")
        sys.exit(1)

    server = HTTPServer(("127.0.0.1", args.port), ViewerHandler)
    url = f"http://127.0.0.1:{args.port}"
    print(f"🧠 kiro-mem viewer running at {url}")
    print(f"📁 Database: {DB_PATH}")
    print("Press Ctrl+C to stop")

    if not args.no_open:
        webbrowser.open(url)

    try:
        server.serve_forever()
    except KeyboardInterrupt:
        print("\nStopped.")
        server.server_close()


if __name__ == "__main__":
    main()

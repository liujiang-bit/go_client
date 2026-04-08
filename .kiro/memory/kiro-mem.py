#!/usr/bin/env python3
"""
kiro-mem: Kiro persistent memory CLI
SQLite + FTS5 full-text search with 3-layer progressive search

Commands:
  init       Initialize database
  store      Store an observation
  search     Layer 1: FTS5 search -> compact index (~50-100 tokens/result)
  timeline   Layer 2: Context around an anchor point
  get        Layer 3: Full details by IDs (~500-1000 tokens/result)
  summary    Store/get session summaries
  context    Generate context injection text
  stats      Show statistics
"""
import sys, os, json, sqlite3, argparse, math
from datetime import datetime, timezone
from pathlib import Path

DB_DIR = Path(__file__).parent
DB_PATH = DB_DIR / "kiro-mem.db"
SCHEMA_VERSION = 2
CHARS_PER_TOKEN = 4

TYPE_ICONS = {
    "discovery": "🔵", "bugfix": "🔴", "feature": "🟣",
    "decision": "⚖️", "change": "✅", "refactor": "🔄",
}

SCHEMA_SQL = """
CREATE TABLE IF NOT EXISTS schema_version (version INTEGER PRIMARY KEY);
CREATE TABLE IF NOT EXISTS sessions (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    session_id TEXT UNIQUE NOT NULL,
    project TEXT NOT NULL DEFAULT '',
    user_request TEXT,
    started_at TEXT NOT NULL DEFAULT (datetime('now')),
    started_at_epoch INTEGER NOT NULL DEFAULT (strftime('%s','now')),
    completed_at TEXT, completed_at_epoch INTEGER,
    status TEXT NOT NULL DEFAULT 'active'
        CHECK (status IN ('active','completed','failed'))
);
CREATE TABLE IF NOT EXISTS observations (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    session_id TEXT,
    project TEXT NOT NULL DEFAULT '',
    type TEXT NOT NULL DEFAULT 'discovery'
        CHECK (type IN ('discovery','bugfix','feature',
                        'decision','change','refactor')),
    title TEXT NOT NULL,
    summary TEXT, detail TEXT,
    files TEXT, concepts TEXT,
    created_at TEXT NOT NULL DEFAULT (datetime('now')),
    created_at_epoch INTEGER NOT NULL DEFAULT (strftime('%s','now'))
);
CREATE TABLE IF NOT EXISTS summaries (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    session_id TEXT,
    project TEXT NOT NULL DEFAULT '',
    user_request TEXT, investigated TEXT,
    learned TEXT, completed TEXT, next_steps TEXT,
    created_at TEXT NOT NULL DEFAULT (datetime('now')),
    created_at_epoch INTEGER NOT NULL DEFAULT (strftime('%s','now'))
);
CREATE VIRTUAL TABLE IF NOT EXISTS observations_fts USING fts5(
    title, summary, detail, files, concepts,
    content='observations', content_rowid='id',
    tokenize='unicode61'
);
CREATE TRIGGER IF NOT EXISTS observations_ai AFTER INSERT ON observations BEGIN
    INSERT INTO observations_fts(rowid,title,summary,detail,files,concepts)
    VALUES (new.id,new.title,new.summary,new.detail,new.files,new.concepts);
END;
CREATE TRIGGER IF NOT EXISTS observations_ad AFTER DELETE ON observations BEGIN
    INSERT INTO observations_fts(observations_fts,rowid,title,summary,detail,files,concepts)
    VALUES ('delete',old.id,old.title,old.summary,old.detail,old.files,old.concepts);
END;
CREATE TRIGGER IF NOT EXISTS observations_au AFTER UPDATE ON observations BEGIN
    INSERT INTO observations_fts(observations_fts,rowid,title,summary,detail,files,concepts)
    VALUES ('delete',old.id,old.title,old.summary,old.detail,old.files,old.concepts);
    INSERT INTO observations_fts(rowid,title,summary,detail,files,concepts)
    VALUES (new.id,new.title,new.summary,new.detail,new.files,new.concepts);
END;
CREATE VIRTUAL TABLE IF NOT EXISTS summaries_fts USING fts5(
    user_request, investigated, learned, completed, next_steps,
    content='summaries', content_rowid='id',
    tokenize='unicode61'
);
CREATE TRIGGER IF NOT EXISTS summaries_ai AFTER INSERT ON summaries BEGIN
    INSERT INTO summaries_fts(rowid,user_request,investigated,learned,completed,next_steps)
    VALUES (new.id,new.user_request,new.investigated,new.learned,new.completed,new.next_steps);
END;
CREATE TRIGGER IF NOT EXISTS summaries_ad AFTER DELETE ON summaries BEGIN
    INSERT INTO summaries_fts(summaries_fts,rowid,user_request,investigated,learned,completed,next_steps)
    VALUES ('delete',old.id,old.user_request,old.investigated,old.learned,old.completed,old.next_steps);
END;
CREATE INDEX IF NOT EXISTS idx_obs_project ON observations(project);
CREATE INDEX IF NOT EXISTS idx_obs_type ON observations(type);
CREATE INDEX IF NOT EXISTS idx_obs_epoch ON observations(created_at_epoch DESC);
CREATE INDEX IF NOT EXISTS idx_obs_session ON observations(session_id);
CREATE INDEX IF NOT EXISTS idx_sum_project ON summaries(project);
CREATE INDEX IF NOT EXISTS idx_sum_epoch ON summaries(created_at_epoch DESC);
"""


# ============================================================
# Helpers
# ============================================================

def get_db() -> sqlite3.Connection:
    db = sqlite3.connect(str(DB_PATH))
    db.row_factory = sqlite3.Row
    db.execute("PRAGMA journal_mode=WAL")
    db.execute("PRAGMA foreign_keys=ON")
    return db

def ensure_db():
    if not DB_PATH.exists():
        cmd_init(None)

def estimate_tokens(text: str) -> int:
    if not text: return 0
    return math.ceil(len(text) / CHARS_PER_TOKEN)

def fmt_time(created_at: str) -> str:
    if not created_at: return ""
    return created_at[5:16].replace("T", " ")

def fmt_date(created_at: str) -> str:
    if not created_at: return ""
    return created_at[:10]

def first_file(files_json: str) -> str:
    if not files_json: return ""
    try:
        fl = json.loads(files_json)
        if fl: return os.path.basename(fl[0])
    except: pass
    return ""

def date_to_epoch(date_str: str) -> int:
    dt = datetime.strptime(date_str, "%Y-%m-%d")
    return int(dt.replace(tzinfo=timezone.utc).timestamp())


# ============================================================
# init
# ============================================================

def cmd_init(_args):
    db = get_db()
    db.executescript(SCHEMA_SQL)
    db.execute("INSERT OR REPLACE INTO schema_version(version) VALUES(?)",
               (SCHEMA_VERSION,))
    db.commit()
    db.close()
    print(json.dumps({"ok": True, "db": str(DB_PATH)}))

# ============================================================
# store: 存储观察记录
# ============================================================

def cmd_store(args):
    ensure_db()
    db = get_db()
    files_json = json.dumps([f.strip() for f in args.files.split(",") if f.strip()]) if args.files else "[]"
    concepts_json = json.dumps([c.strip() for c in args.concepts.split(",") if c.strip()]) if args.concepts else "[]"
    cur = db.execute(
        """INSERT INTO observations
           (session_id,project,type,title,summary,detail,files,concepts)
           VALUES (?,?,?,?,?,?,?,?)""",
        (args.session_id or "", args.project or "",
         args.type, args.title, args.summary or "",
         args.detail or "", files_json, concepts_json))
    obs_id = cur.lastrowid
    db.commit()
    db.close()
    print(json.dumps({"ok": True, "id": obs_id}))


# ============================================================
# LAYER 1: search — compact index (~50-100 tokens/result)
# Returns: ID, time, type icon, title, estimated read tokens
# Token-efficient: lets you filter before fetching full details
# ============================================================

def cmd_search(args):
    ensure_db()
    db = get_db()
    query = args.query
    limit = args.limit or 20
    obs_type = args.type
    project = args.project
    concept = args.concept

    # Build filter conditions
    params = []
    where_parts = []
    if obs_type:
        types = [t.strip() for t in obs_type.split(",")]
        where_parts.append(f"o.type IN ({','.join('?' for _ in types)})")
        params.extend(types)
    if project:
        where_parts.append("o.project = ?")
        params.append(project)
    if args.date_start:
        where_parts.append("o.created_at_epoch >= ?")
        params.append(date_to_epoch(args.date_start))
    if args.date_end:
        where_parts.append("o.created_at_epoch <= ?")
        params.append(date_to_epoch(args.date_end))
    if concept:
        concepts = [c.strip() for c in concept.split(",")]
        for c in concepts:
            where_parts.append("o.concepts LIKE ?")
            params.append(f'%"{c}"%')

    where_sql = (" AND " + " AND ".join(where_parts)) if where_parts else ""

    if query:
        # FTS5 full-text search
        sql = f"""
            SELECT o.id, o.type, o.title, o.summary, o.files,
                   o.created_at, o.created_at_epoch
            FROM observations_fts fts
            JOIN observations o ON o.id = fts.rowid
            WHERE observations_fts MATCH ?{where_sql}
            ORDER BY fts.rank
            LIMIT ?
        """
        all_params = [query] + params + [limit]
    else:
        # Filter-only (no query text)
        if not where_parts:
            # Default: recent observations
            where_sql = ""
        else:
            where_sql = "WHERE " + " AND ".join(where_parts)
        sql = f"""
            SELECT o.id, o.type, o.title, o.summary, o.files,
                   o.created_at, o.created_at_epoch
            FROM observations o
            {where_sql}
            ORDER BY o.created_at_epoch DESC
            LIMIT ?
        """
        all_params = params + [limit]

    rows = db.execute(sql, all_params).fetchall()
    db.close()

    # Format as compact index table (like claude-mem)
    total = len(rows)
    lines = []
    lines.append(f"Found {total} result(s)" + (f' matching "{query}"' if query else ""))
    lines.append("")

    # Group by date
    by_date = {}
    for r in rows:
        day = fmt_date(r["created_at"])
        by_date.setdefault(day, []).append(r)

    for day, day_rows in by_date.items():
        lines.append(f"### {day}")
        lines.append("")
        lines.append("| ID | Time | T | Title | ~Tokens |")
        lines.append("|----|------|---|-------|---------|")
        last_time = ""
        for r in day_rows:
            icon = TYPE_ICONS.get(r["type"], "?")
            t = fmt_time(r["created_at"])
            time_display = t if t != last_time else '"'
            last_time = t
            tokens = estimate_tokens((r["title"] or "") + (r["summary"] or ""))
            lines.append(f"| #{r['id']} | {time_display} | {icon} | {r['title']} | ~{tokens} |")
        lines.append("")

    print("\n".join(lines))


# ============================================================
# LAYER 2: timeline — context around an anchor point
# Shows observations + summaries in chronological order
# around a specific observation ID or query match
# ============================================================

def cmd_timeline(args):
    ensure_db()
    db = get_db()
    anchor = args.anchor
    query = args.query
    before = args.before or 5
    after = args.after or 5
    project = args.project

    proj_filter = "AND project = ?" if project else ""
    proj_params = [project] if project else []

    anchor_id = None
    anchor_epoch = None

    if anchor:
        # Direct anchor by observation ID
        anchor_id = int(anchor)
        row = db.execute("SELECT created_at_epoch FROM observations WHERE id=?",
                         (anchor_id,)).fetchone()
        if not row:
            print(f"Observation #{anchor_id} not found")
            db.close()
            return
        anchor_epoch = row["created_at_epoch"]
    elif query:
        # Find anchor via FTS5 search (best match)
        row = db.execute("""
            SELECT o.id, o.created_at_epoch
            FROM observations_fts fts
            JOIN observations o ON o.id = fts.rowid
            ORDER BY fts.rank LIMIT 1
        """).fetchone()
        if not row:
            print(f'No observations matching "{query}"')
            db.close()
            return
        anchor_id = row["id"]
        anchor_epoch = row["created_at_epoch"]
    else:
        print("Error: provide --anchor ID or --query text")
        db.close()
        return

    # Get boundary epochs
    before_rows = db.execute(f"""
        SELECT created_at_epoch FROM observations
        WHERE id <= ? {proj_filter}
        ORDER BY id DESC LIMIT ?
    """, [anchor_id] + proj_params + [before + 1]).fetchall()

    after_rows = db.execute(f"""
        SELECT created_at_epoch FROM observations
        WHERE id >= ? {proj_filter}
        ORDER BY id ASC LIMIT ?
    """, [anchor_id] + proj_params + [after + 1]).fetchall()

    start_epoch = before_rows[-1]["created_at_epoch"] if before_rows else anchor_epoch
    end_epoch = after_rows[-1]["created_at_epoch"] if after_rows else anchor_epoch

    # Fetch all records in window
    obs = db.execute(f"""
        SELECT id, type, title, summary, files, created_at, created_at_epoch
        FROM observations
        WHERE created_at_epoch >= ? AND created_at_epoch <= ? {proj_filter}
        ORDER BY created_at_epoch ASC
    """, [start_epoch, end_epoch] + proj_params).fetchall()

    sums = db.execute(f"""
        SELECT id, user_request, completed, next_steps, created_at, created_at_epoch
        FROM summaries
        WHERE created_at_epoch >= ? AND created_at_epoch <= ? {proj_filter}
        ORDER BY created_at_epoch ASC
    """, [start_epoch, end_epoch] + proj_params).fetchall()

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
        lines.append(f'# Timeline for query: "{query}"')
        lines.append(f"**Anchor:** Observation #{anchor_id}")
    else:
        lines.append(f"# Timeline around #{anchor_id}")
    lines.append(f"**Window:** {before} before -> {after} after | **Items:** {len(items)}")
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
                lines.append(f"**🎯 #S{data['id']}** {req}")
                if data.get("completed"):
                    lines.append(f"  Completed: {data['completed']}")
                lines.append("")
            else:
                if not has_obs_table:
                    lines.append("| ID | Time | T | Title | ~Tokens |")
                    lines.append("|----|------|---|-------|---------|")
                    has_obs_table = True
                icon = TYPE_ICONS.get(data["type"], "?")
                t = fmt_time(data["created_at"])
                tokens = estimate_tokens((data.get("title") or "") + (data.get("summary") or ""))
                marker = " <- **ANCHOR**" if data["id"] == anchor_id else ""
                lines.append(f"| #{data['id']} | {t} | {icon} | {data['title']}{marker} | ~{tokens} |")
        if has_obs_table:
            lines.append("")

    print("\n".join(lines))


# ============================================================
# LAYER 3: get — full details by IDs (~500-1000 tokens/result)
# Only fetch after filtering with search + timeline
# ============================================================

def cmd_get(args):
    ensure_db()
    db = get_db()
    ids = [int(x.strip()) for x in args.ids.split(",")]
    placeholders = ",".join("?" for _ in ids)
    rows = db.execute(f"""
        SELECT * FROM observations WHERE id IN ({placeholders})
        ORDER BY created_at_epoch DESC
    """, ids).fetchall()
    db.close()

    results = []
    for r in rows:
        d = dict(r)
        # Parse JSON fields for readability
        for field in ("files", "concepts"):
            if d.get(field):
                try: d[field] = json.loads(d[field])
                except: pass
        results.append(d)

    # Output as readable markdown (not raw JSON)
    for r in results:
        icon = TYPE_ICONS.get(r["type"], "?")
        print(f"## [{icon} #{r['id']}] {r['title']}")
        print(f"- **Type**: {r['type']}")
        print(f"- **Time**: {r['created_at']}")
        if r.get("files"):
            files = r["files"] if isinstance(r["files"], list) else [r["files"]]
            print(f"- **Files**: {', '.join(files)}")
        if r.get("concepts"):
            concepts = r["concepts"] if isinstance(r["concepts"], list) else [r["concepts"]]
            print(f"- **Concepts**: {', '.join(concepts)}")
        print("")
        if r.get("summary"):
            print(f"**Summary:** {r['summary']}")
            print("")
        if r.get("detail"):
            print(f"**Detail:** {r['detail']}")
            print("")
        print("---")
        print("")


# ============================================================
# summary, context, stats
# ============================================================

def cmd_summary(args):
    ensure_db()
    db = get_db()
    if args.action == "store":
        cur = db.execute(
            """INSERT INTO summaries
               (session_id,project,user_request,investigated,
                learned,completed,next_steps)
               VALUES (?,?,?,?,?,?,?)""",
            (args.session_id or "", args.project or "",
             args.user_request or "", args.investigated or "",
             args.learned or "", args.completed or "",
             args.next_steps or ""))
        db.commit()
        print(json.dumps({"ok": True, "id": cur.lastrowid}))
    else:
        limit = args.limit or 1
        rows = db.execute(
            "SELECT * FROM summaries ORDER BY created_at_epoch DESC LIMIT ?",
            (limit,)).fetchall()
        for r in rows:
            d = dict(r)
            print(f"## Session Summary #{d['id']} ({d['created_at']})")
            if d.get("user_request"):
                print(f"**Request:** {d['user_request']}")
            if d.get("investigated"):
                print(f"**Investigated:** {d['investigated']}")
            if d.get("learned"):
                print(f"**Learned:** {d['learned']}")
            if d.get("completed"):
                print(f"**Completed:** {d['completed']}")
            if d.get("next_steps"):
                print(f"**Next:** {d['next_steps']}")
            print("")
    db.close()

def cmd_context(args):
    ensure_db()
    db = get_db()
    limit = args.limit or 15
    params = []
    where = ""
    if args.project:
        where = "WHERE project = ?"
        params.append(args.project)
    params.append(limit)

    obs = db.execute(f"""
        SELECT id, type, title, summary, files, created_at
        FROM observations {where}
        ORDER BY created_at_epoch DESC LIMIT ?
    """, params).fetchall()

    sums = db.execute(
        "SELECT * FROM summaries ORDER BY created_at_epoch DESC LIMIT 1"
    ).fetchall()
    db.close()

    out = ["<kiro-memory>", "# Recent Activity", ""]
    if obs:
        out.append("| ID | Time | T | Title |")
        out.append("|----|------|---|-------|")
        for r in obs:
            icon = TYPE_ICONS.get(r["type"], "?")
            ts = fmt_time(r["created_at"])
            out.append(f"| #{r['id']} | {ts} | {icon} | {r['title']} |")
    else:
        out.append("(No observations yet)")

    if sums:
        s = dict(sums[0])
        out.append("")
        out.append("## Last Session")
        if s.get("completed"):
            out.append(f"Completed: {s['completed']}")
        if s.get("next_steps"):
            out.append(f"Next: {s['next_steps']}")

    out.append("")
    out.append("## Search Commands")
    out.append("```")
    out.append('python .kiro/memory/kiro-mem.py search "keyword"')
    out.append("python .kiro/memory/kiro-mem.py timeline --anchor ID")
    out.append('python .kiro/memory/kiro-mem.py get --ids "1,2,3"')
    out.append("```")
    out.append("</kiro-memory>")
    print("\n".join(out))

def cmd_stats(_args):
    ensure_db()
    db = get_db()
    total = db.execute("SELECT COUNT(*) c FROM observations").fetchone()["c"]
    by_type = db.execute(
        "SELECT type, COUNT(*) c FROM observations GROUP BY type"
    ).fetchall()
    sessions = db.execute("SELECT COUNT(*) c FROM summaries").fetchone()["c"]
    db.close()
    types = {r["type"]: r["c"] for r in by_type}
    print(json.dumps({"ok": True, "total_observations": total,
                       "by_type": types, "total_summaries": sessions},
                      ensure_ascii=False))


# ============================================================
# CLI parser
# ============================================================

def build_parser():
    p = argparse.ArgumentParser(prog="kiro-mem")
    sub = p.add_subparsers(dest="command", required=True)

    sub.add_parser("init")

    s = sub.add_parser("store")
    s.add_argument("--type", required=True, choices=list(TYPE_ICONS.keys()))
    s.add_argument("--title", required=True)
    s.add_argument("--summary", default="")
    s.add_argument("--detail", default="")
    s.add_argument("--files", default="")
    s.add_argument("--concepts", default="")
    s.add_argument("--project", default="")
    s.add_argument("--session-id", default="")

    s = sub.add_parser("search")
    s.add_argument("query", nargs="?", default=None)
    s.add_argument("--type", default=None, help="Filter: bugfix,feature,discovery...")
    s.add_argument("--project", default=None)
    s.add_argument("--concept", default=None, help="Filter by concept tag")
    s.add_argument("--date-start", default=None, help="YYYY-MM-DD")
    s.add_argument("--date-end", default=None, help="YYYY-MM-DD")
    s.add_argument("--limit", type=int, default=20)

    s = sub.add_parser("timeline")
    s.add_argument("--anchor", default=None, help="Observation ID to center around")
    s.add_argument("--query", default=None, help="Find anchor via search")
    s.add_argument("--before", type=int, default=5)
    s.add_argument("--after", type=int, default=5)
    s.add_argument("--project", default=None)

    s = sub.add_parser("get")
    s.add_argument("--ids", required=True, help="Comma-separated IDs")

    s = sub.add_parser("summary")
    s.add_argument("action", choices=["store", "get"])
    s.add_argument("--session-id", default="")
    s.add_argument("--project", default="")
    s.add_argument("--user-request", default="")
    s.add_argument("--investigated", default="")
    s.add_argument("--learned", default="")
    s.add_argument("--completed", default="")
    s.add_argument("--next-steps", default="")
    s.add_argument("--limit", type=int, default=1)

    s = sub.add_parser("context")
    s.add_argument("--project", default=None)
    s.add_argument("--limit", type=int, default=15)

    sub.add_parser("stats")
    return p

COMMANDS = {
    "init": cmd_init, "store": cmd_store,
    "search": cmd_search, "timeline": cmd_timeline,
    "get": cmd_get, "summary": cmd_summary,
    "context": cmd_context, "stats": cmd_stats,
}

if __name__ == "__main__":
    parser = build_parser()
    args = parser.parse_args()
    COMMANDS[args.command](args)

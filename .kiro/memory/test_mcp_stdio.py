#!/usr/bin/env python3
"""Test MCP server stdio protocol — supports both Content-Length and raw JSON lines."""
import subprocess, json, sys, os

server = subprocess.Popen(
    [sys.executable, os.path.join(os.path.dirname(os.path.abspath(__file__)), "mcp-server.py")],
    stdin=subprocess.PIPE, stdout=subprocess.PIPE, stderr=subprocess.PIPE
)

def send_msg(obj):
    """Send as raw JSON line (matching Kiro's behavior)."""
    line = json.dumps(obj) + "\n"
    server.stdin.write(line.encode("utf-8"))
    server.stdin.flush()

def read_msg():
    """Read response — try raw JSON line first, fall back to Content-Length."""
    while True:
        raw = server.stdout.readline()
        if not raw:
            return None
        line = raw.decode("utf-8").strip()
        if not line:
            continue
        # Raw JSON line
        if line.startswith("{"):
            return json.loads(line)
        # Content-Length header
        if "content-length" in line.lower():
            length = int(line.split(":")[1].strip())
            server.stdout.readline()  # empty line
            body = server.stdout.read(length)
            return json.loads(body.decode("utf-8"))

# Test 1: initialize
send_msg({
    "jsonrpc": "2.0", "id": 1, "method": "initialize",
    "params": {"protocolVersion": "2024-11-05", "capabilities": {},
               "clientInfo": {"name": "test", "version": "1.0"}}
})
resp = read_msg()
print(f"initialize: {json.dumps(resp, indent=2)}")
assert resp["result"]["serverInfo"]["name"] == "kiro-mem"

# Test 2: tools/list
send_msg({"jsonrpc": "2.0", "id": 2, "method": "tools/list", "params": {}})
resp = read_msg()
tools = [t["name"] for t in resp["result"]["tools"]]
print(f"tools/list: {tools}")
assert "mem_search" in tools

# Test 3: tools/call mem_stats
send_msg({
    "jsonrpc": "2.0", "id": 3, "method": "tools/call",
    "params": {"name": "mem_stats", "arguments": {}}
})
resp = read_msg()
print(f"mem_stats: {resp}")
assert not resp["result"].get("isError")

# Cleanup
server.stdin.close()
stderr_out = server.stderr.read().decode("utf-8", errors="replace")
server.wait()
print(f"\nstderr:\n{stderr_out}")
print(f"exit code: {server.returncode}")
print("\nAll tests passed.")

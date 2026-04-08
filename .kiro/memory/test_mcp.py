#!/usr/bin/env python3
"""Quick test for MCP server tool functions — imports module properly."""
import sys, os, importlib.util

# Load mcp-server module properly (avoids exec() encoding issues)
mod_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), "mcp-server.py")
spec = importlib.util.spec_from_file_location("mcp_server", mod_path)
mod = importlib.util.module_from_spec(spec)
spec.loader.exec_module(mod)

print("=== Stats ===")
print(mod.tool_stats({}))
print()

print("=== Layer 1: Search (recent, no query) ===")
print(mod.tool_search({"limit": 5}))
print()

print("=== Layer 1: Search (FTS5 query) ===")
print(mod.tool_search({"query": "memory"}))
print()

print("=== Layer 2: Timeline (anchor=1) ===")
print(mod.tool_timeline({"anchor": 1, "before": 3, "after": 3}))
print()

print("=== Layer 3: Get full details [1, 2] ===")
print(mod.tool_get({"ids": [1, 2]}))
print()

print("=== Layer 2: Timeline (query search) ===")
print(mod.tool_timeline({"query": "memory"}))
print()

print("=== Edge: Search with type filter ===")
print(mod.tool_search({"type": "bugfix", "limit": 3}))
print()

print("=== Edge: Get with empty IDs ===")
print(mod.tool_get({"ids": []}))
print()

print("All tests passed.")

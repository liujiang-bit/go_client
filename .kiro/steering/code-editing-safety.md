---
inclusion: always
---

# 代码编辑安全规则

## 子代理执行后的 tasks.md 状态更新

当委托子代理（general-task-execution 等）执行 spec 任务后，子代理通常会自动更新 tasks.md 中的 checkbox 状态。
在尝试用 strReplace 更新 tasks.md 状态前，先用 grepSearch 确认实际文本内容。
如果子代理已将 `[ ]` 改为 `[x]` 或 `[-]`，则跳过 strReplace，避免匹配失败。

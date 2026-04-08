---
name: kiro-memory
description: "Kiro 持久记忆技能。自动捕获会话中的观察（发现、Bug修复、功能实现、决策、变更、重构），压缩为结构化记录存储在 .kiro/memory/ 目录。支持跨会话上下文延续。"
---

# Kiro 持久记忆技能

跨会话持久记忆系统。自动捕获有价值的观察，压缩存储，在新会话中提供历史上下文。

## 快速参考

| 操作 | 说明 |
|------|------|
| 记录观察 | 追加到 `.kiro/memory/observations/YYYY-MM-DD.md` |
| 更新时间线 | 更新 `.kiro/memory/timeline.md`（保持最近 30 条） |
| 更新索引 | 更新 `.kiro/memory/index.md` |
| 生成摘要 | 覆盖 `.kiro/memory/summaries/latest.md` |
| 查询历史 | 先看 timeline.md → 再看具体日期文件 |

## 观察类型

| 类型 | 图标 | 触发场景 |
|------|------|---------|
| `discovery` | 🔵 | 发现代码结构、API 行为、项目模式 |
| `bugfix` | 🔴 | 修复了 Bug，记录根因和解决方案 |
| `feature` | 🟣 | 实现了新功能或修改了现有功能 |
| `decision` | ⚖️ | 做了架构或设计决策 |
| `change` | ✅ | 重要的代码变更 |
| `refactor` | 🔄 | 重构操作 |

## 记录观察的完整流程

### 1. 确定是否值得记录

值得记录：
- 修复了非显而易见的 Bug
- 发现了项目特定的模式或约定
- 实现了新功能
- 做了需要记住理由的决策
- 进行了影响多个文件的变更

不值得记录：
- 简单格式调整
- 纯问答，无实际操作
- 已记录过的重复内容

### 2. 生成观察 ID

格式：`OBS-XXXX`

生成步骤：
1. 读取 `.kiro/memory/index.md`
2. 找到当前最大 ID 数字
3. +1 作为新 ID
4. 如果 index.md 不存在或为空，从 `OBS-0001` 开始

### 3. 写入观察记录

追加到 `.kiro/memory/observations/YYYY-MM-DD.md`（当天日期）：

```markdown
### [OBS-XXXX] 🔵 标题

- **时间**: YYYY-MM-DD HH:mm
- **类型**: discovery
- **文件**: path/to/file1.lua, path/to/file2.lua
- **概念**: scrollview, cell-recycling, ui-framework

**摘要**: 一句话描述

**详情**: 
更详细的上下文（可选，仅在信息量大时添加）

---
```

### 4. 更新时间线

在 `.kiro/memory/timeline.md` 的表格顶部插入新条目，保持总数不超过 30 条（删除最旧的）。

### 5. 更新索引

在 `.kiro/memory/index.md` 对应类型分组下添加条目。

## 生成会话摘要

在会话即将结束时（agentStop hook 触发），生成摘要：

```markdown
# 会话摘要

- **日期**: YYYY-MM-DD
- **观察数**: N 条新观察

## 用户请求
简述用户本次会话的主要目标

## 调查发现
列出调查过程中发现的关键信息

## 学到的知识
本次会话中获得的新认知

## 完成的工作
实际完成了什么（简洁列表）

## 后续步骤
建议的下一步操作（如果有）
```

覆盖写入 `.kiro/memory/summaries/latest.md`。

## 查询历史记忆

当用户问"之前做过什么"、"上次怎么解决的"等问题时：

1. 先读 `.kiro/memory/timeline.md` 获取最近活动概览
2. 根据时间线中的信息，读取具体日期的观察文件
3. 读 `.kiro/memory/summaries/latest.md` 了解上次会话上下文
4. 如需按类型查找，读 `.kiro/memory/index.md`

## 文件模板

### observations/YYYY-MM-DD.md（新建时）

```markdown
# 观察记录 - YYYY-MM-DD

---

```

### timeline.md（新建时）

```markdown
# 最近活动

| ID | 时间 | 类型 | 标题 | 文件 |
|----|------|------|------|------|

```

### index.md（新建时）

```markdown
# 记忆索引

## 🔵 Discovery

## 🔴 Bugfix

## 🟣 Feature

## ⚖️ Decision

## ✅ Change

## 🔄 Refactor

```

### summaries/latest.md（新建时）

```markdown
# 会话摘要

（尚无历史会话）
```

## 最佳实践

1. **即时记录** — 操作完成后立即记录，上下文最新鲜
2. **标题要有信息量** — 未来扫描时间线时能快速理解
3. **概念标签要一致** — 同一概念用同一标签（如 `scrollview` 不要写成 `scroll-view`）
4. **文件路径要准确** — 方便后续定位
5. **摘要要简洁** — 一句话说清楚发生了什么
6. **详情按需添加** — 只在信息量大、解决方案复杂时添加详情

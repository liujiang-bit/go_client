---
name: self-improving-agent
description: "Kiro 自我学习纠错技能。在以下情况触发：(1) 命令或操作失败 (2) 用户纠正错误 (3) 发现知识过时 (4) 找到更好的方案 (5) 用户请求不存在的功能。自动记录教训到 .learnings/ 目录，高价值经验晋升到 steering 文件。"
inclusion: auto
---

# Kiro 自我学习纠错技能

在开发过程中自动捕获错误、纠正和经验教训，记录到 `.kiro/.learnings/` 目录。高价值的经验会晋升为 steering 规则，让 Kiro 在后续会话中不再犯同样的错误。

## 快速参考

| 场景 | 操作 |
|------|------|
| 命令/操作失败 | 记录到 `.kiro/.learnings/ERRORS.md` |
| 用户纠正你 | 记录到 `.kiro/.learnings/LEARNINGS.md`，分类 `correction` |
| 用户要求不存在的功能 | 记录到 `.kiro/.learnings/FEATURE_REQUESTS.md` |
| 发现知识过时 | 记录到 `.kiro/.learnings/LEARNINGS.md`，分类 `knowledge_gap` |
| 找到更好方案 | 记录到 `.kiro/.learnings/LEARNINGS.md`，分类 `best_practice` |
| 反复出现的问题 | 用 `See Also` 关联，考虑提升优先级 |
| 广泛适用的经验 | 晋升到 `.kiro/steering/` 目录下的 steering 文件 |

## 检测触发条件

自动识别以下信号并记录：

**纠正类**（→ LEARNINGS.md，`correction`）：
- 用户说"不对"、"错了"、"应该是..."、"实际上..."
- 你的方案被用户否定或修改

**知识缺口**（→ LEARNINGS.md，`knowledge_gap`）：
- 用户提供了你不知道的项目信息
- 你引用的文档/API 已过时
- 实际行为与你的理解不符

**错误类**（→ ERRORS.md）：
- 命令返回非零退出码
- 异常或堆栈跟踪
- 编译/类型检查失败
- 超时或连接失败

**功能请求**（→ FEATURE_REQUESTS.md）：
- "能不能..."、"有没有办法..."、"为什么不能..."

## 记录格式

### 学习条目（LEARNINGS.md）

```markdown
## [LRN-YYYYMMDD-XXX] category

**记录时间**: YYYY-MM-DD HH:mm
**优先级**: low | medium | high | critical
**状态**: pending | resolved | promoted
**领域**: lua | csharp | proto | ui | config | infra

### 摘要
一句话描述学到了什么

### 详情
完整上下文：发生了什么，哪里错了，正确做法是什么

### 建议操作
具体的修复或改进措施

### 元数据
- 来源: conversation | error | user_feedback
- 相关文件: path/to/file
- 标签: tag1, tag2
- 关联: LRN-20260101-001

---
```

### 错误条目（ERRORS.md）

```markdown
## [ERR-YYYYMMDD-XXX] error_name

**记录时间**: YYYY-MM-DD HH:mm
**优先级**: high
**状态**: pending
**领域**: lua | csharp | proto | ui | config | infra

### 摘要
简述什么失败了

### 错误信息
```
实际错误输出
```

### 上下文
- 执行的命令/操作
- 输入参数
- 环境信息

### 建议修复
如果能确定，写出解决方案

### 元数据
- 可复现: yes | no | unknown
- 相关文件: path/to/file

---
```

### 功能请求条目（FEATURE_REQUESTS.md）

```markdown
## [FEAT-YYYYMMDD-XXX] capability_name

**记录时间**: YYYY-MM-DD HH:mm
**优先级**: medium
**状态**: pending
**领域**: lua | csharp | proto | ui | config | infra

### 请求的功能
用户想做什么

### 用户场景
为什么需要，解决什么问题

### 建议实现
如何实现，可以扩展什么

---
```

## ID 生成规则

格式: `TYPE-YYYYMMDD-XXX`
- TYPE: `LRN`（学习）、`ERR`（错误）、`FEAT`（功能请求）
- YYYYMMDD: 当前日期
- XXX: 三位序号（001, 002...）

## 晋升到 Steering

当一个经验教训被证明广泛适用时，将其晋升为 steering 规则：

### 晋升条件（满足任一）
- 同类问题出现 2 次以上（有 `关联` 链接）
- 适用于多个文件/功能模块
- 任何开发者（人或 AI）都应该知道的知识
- 能防止反复犯错

### 晋升流程
1. 将经验提炼为简洁的规则
2. 添加到 `.kiro/steering/` 下合适的 steering 文件
3. 更新原始条目状态为 `promoted`，添加 `**晋升到**: steering/xxx.md`

### 晋升示例

**原始学习**（详细）：
> 项目中 proto 文件修改后需要同步更新 EmmyLua 注释，否则 Lua 代码的类型提示会过时。

**晋升到 steering**（简洁）：
```markdown
## Proto 同步规则
- 修改 .proto 文件后，必须同步更新对应的 EmmyLua 类型注释
```

## 定期回顾

在以下时机回顾 `.kiro/.learnings/`：
- 开始新的重大任务前
- 完成一个功能后
- 在有过往教训的领域工作时

## 最佳实践

1. **立即记录** — 上下文最新鲜的时候记
2. **具体明确** — 未来的会话需要快速理解
3. **包含复现步骤** — 尤其是错误类
4. **关联相关文件** — 方便后续修复
5. **建议具体修复** — 不要只写"需要调查"
6. **积极晋升** — 有疑问就加到 steering

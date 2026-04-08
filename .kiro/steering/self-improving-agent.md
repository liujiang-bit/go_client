---
description: "自我学习纠错规则：错误记录、经验晋升、学习日志"
inclusion: auto
---

# 自我学习纠错规则

你具备自我学习和纠错能力。在每次交互中，注意以下信号并自动记录教训。

## 触发条件

完成任务后，评估是否产生了可提取的知识：
- 通过调查发现了非显而易见的解决方案？
- 发现了意外行为的变通方法？
- 学到了项目特定的模式？
- 错误需要调试才能解决？
- 用户纠正了你的错误？

## 操作规则

1. **检测到错误/失败时**：记录到 `.kiro/.learnings/ERRORS.md`
2. **被用户纠正时**：记录到 `.kiro/.learnings/LEARNINGS.md`，分类为 `correction`
3. **发现知识过时时**：记录到 `.kiro/.learnings/LEARNINGS.md`，分类为 `knowledge_gap`
4. **找到更好方案时**：记录到 `.kiro/.learnings/LEARNINGS.md`，分类为 `best_practice`
5. **用户请求不存在的功能时**：记录到 `.kiro/.learnings/FEATURE_REQUESTS.md`

## 记录格式

使用 `.kiro/skills/self-improving-agent/SKILL.md` 中定义的标准格式。

## 晋升规则

当一个教训满足以下任一条件时，将其晋升为 steering 规则：
- 同类问题出现 2 次以上
- 适用于多个文件/功能
- 能防止反复犯错
- 任何开发者都应该知道

晋升方式：在 `.kiro/steering/code-editing-safety.md` 中创建或更新对应的规则。晋升是原子操作，必须同时完成：(1) 创建/更新 steering 文件中的规则 (2) 更新 learning 状态为 `promoted` (3) 填写正确的 `**晋升到**` 路径。

## 回顾提醒

开始重大任务前，先快速浏览 `.kiro/.learnings/` 中的待处理条目，避免重蹈覆辙。

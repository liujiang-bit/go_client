---
inclusion: manual
---

# Sync Bugfix to Design

当 bugfix.md 中的所有修复任务完成后，将修复内容同步更新到同一 Spec 的 design.md 中。

## 触发时机

用户明确要求将 bugfix 修正同步到 design，或 bugfix 任务全部完成后。

## 执行步骤

### 1. 读取 bugfix.md

读取当前 Spec 目录下的 `bugfix.md`，提取：
- 每个 Bug 的**根因**（Root Cause）
- 每个 Bug 的**修复方案**（Fix）：函数签名变更、逻辑变更、新增字段/方法
- 修复后的**正确行为描述**

### 2. 读取 design.md

读取同目录下的 `design.md`，识别需要更新的章节：
- **节点路径表**：ComponentDefine 中绑定的 UI 节点路径或组件类型有变化时更新
- **关键字段表**：新增或修改了字段时更新
- **对外接口**：函数签名变更时更新
- **核心流程**（初始化流程、数据刷新流程、模式切换流程等）：流程步骤有增删时更新
- **数据处理逻辑**（ApplyFilter、ApplySort、GetBattleState 等伪代码）：逻辑变更时更新
- **注意事项 / 备注**：行为描述与实现不符时更新

### 3. 对比差异，定位需更新的位置

对每个 Bug 修复，逐一判断 design.md 中哪些描述与修复后的实现不符，列出需要修改的具体段落。

### 4. 更新 design.md

只修改与 bugfix 相关的内容，不改动无关章节。更新原则：
- **精确替换**：只改有误的描述，保留正确内容
- **保持风格**：沿用 design.md 已有的表格、代码块、流程图格式
- **不删除 Bugfix 章节**：design.md 末尾的 `# Bugfix 设计` 章节保留，作为历史记录
- **同步伪代码**：若 design.md 中有函数伪代码（如 `ApplyFilter`、`GetBattleState`），同步更新为修复后的签名和逻辑

### 5. 确认完成

告知用户哪些章节已更新，哪些无需变动。

## 注意事项

- 只处理**同一 Spec 目录**下的 bugfix.md 和 design.md（路径：`.kiro/specs/{feature-name}/`）
- 若 bugfix.md 不存在，告知用户并停止
- 若某个 Bug 修复只影响实现细节、不影响设计描述，可跳过该 Bug
- 更新后不需要重新生成 tasks.md

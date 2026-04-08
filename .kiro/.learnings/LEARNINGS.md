# 学习记录

## [2026-03-18] Steering fileMatchPattern 不应设置过宽
- 分类: `correction`
- 状态: `active`
- 描述: 创建 `ui-list.md` steering 时将 `fileMatchPattern` 设为 `**/*.lua`，导致读取任何 Lua 文件都会注入该 steering，污染无关上下文。用户指出了这个问题。
- 教训: UI 组件用法类 steering 应使用 `fileMatch` 配合 `**/UI/**/*.lua`，而非 `**/*.lua`（太宽）或 `manual`（用户不想手动引用）。原则是找到最小合理匹配范围：UI 组件规范限定到 UI 目录，DataCenter 规范限定到 DataCenter 目录。

## [2026-03-18] 时间单位混用：GetServerSeconds() 秒 vs Manager 返回毫秒
- 分类: `bugfix`
- 状态: `active`
- 文件: `Assets/Main/LuaScripts/UI/MainUI_SU/Component/UIPVEMainBottom/UIPVEMainBottom.lua`
- 描述: `UpdateGovernmentPrepareTime` 中 `curTime`（秒）与 `endTime`（毫秒）直接比较 `curTime >= endTime`，导致过期判断永远不成立。倒计时显示部分用了 `endTime / 1000 - curTime` 是正确的，但过期退出条件漏了 `/1000`。
- 教训: 项目中 `UITimeManager:GetServerSeconds()` 返回秒，而很多 Manager 内部用毫秒（`appointTime`、`k9` 都是毫秒级）。比较时务必统一单位，优先在比较处显式转换 `endTime / 1000`。

## 2026-03-18 | GovernmentManager:GetPrepareTime 的 index 参数与列表顺序无关

- **分类**: correction
- **场景**: 在 endTimeFunc 中调用 `GetPrepareTime(positionId, 1)` 前对 prepareList 按 order 排序
- **纠正**: 用户指出排序没有意义，因为 `GetPrepareTime` 的第二个参数 index 是固定传 1，表示第一个排队位置的预计时间，计算逻辑是 `appointTime + k9 * index`，与列表的实际排列顺序无关
- **教训**: 调用接口前应仔细阅读其实现逻辑，确认参数含义。`GetPrepareTime` 基于 appointTime 和配置常量计算，不依赖 prepareList 的排序状态，只需判断列表非空即可
- **状态**: active

## [2026-04-02] Docker Desktop 新版无 "Create from Compose file" 菜单且不支持拖拽启动 Compose
- 分类: `knowledge_gap`
- 状态: `active`
- 描述: 新版 Docker Desktop（Personal 版）既没有 "Create from Compose file" 菜单，拖拽 docker-compose.yml 到窗口也没有反应。Docker Desktop GUI 目前无法直接导入并首次启动 Compose 项目，首次启动仍需命令行 `docker-compose up -d --build`，之后的停止/重启/日志查看可在 GUI 中完成。
- 教训: Docker Desktop 的 GUI 定位是容器管理工具（查看、停止、重启、日志），不是 Compose 项目的启动入口。写文档时不要承诺"纯 GUI 零命令行"，应如实说明首次启动需要一条命令，后续管理在 GUI 完成。

## 2026-04-07 | 业务模块实现需要深入对照 Lua 源码，避免功能遗漏

- **分类**: best_practice
- **场景**: 实现聊天系统模块时，初始版本只实现了基础的消息收发、禁言、跑马灯，但对比 Lua 源码后发现遗漏了 6 个重要功能：消息持久化（ChatSave）、联盟频道动态管理、私聊记录持久化、玩家上下线管理（AgentMgr）、发送频率限制、消息推送队列
- **教训**: 翻译 Lua 服务器到 Go 时，每个模块实现前应先完整阅读 Lua 源码目录下的所有文件（不仅是 handler，还包括 service/proxy/mgr 等），列出完整功能清单后再实现。特别是独立服务（chat_server、battle_server）有自己的 Agent/Gate/Save 等子模块，容易被忽略。
- **适用范围**: 所有从 Lua 翻译到 Go 的业务模块
- **状态**: active

# Implementation Plan: Lua to Go Server

## Overview

将基于 Skynet 框架的 Lua 游戏服务器完整翻译为 Go 语言实现。按照从基础设施 → 核心协议 → 数据层 → 网络层 → 业务逻辑 → 辅助系统的顺序，逐步构建功能等价的 Go 服务器。实现语言为 Go，使用 rapid 进行属性测试。

## Tasks

- [x] 1. 项目初始化与基础设施搭建
  - [x] 1.1 创建 go_server 项目结构和 Go Module 初始化
    - 创建 `go_server/` 目录及子目录：`cmd/`, `internal/`, `pkg/`, `configs/`, `docs/`, `scripts/`, `deployments/`
    - 初始化 `go.mod`（module 名 `go_server`，Go 1.21+）
    - 创建 Makefile，支持 `make build`、`make test`、`make docker`
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5_

  - [x] 1.2 创建错误码与枚举定义（pkg/errcode, pkg/enum）
    - 翻译 `common/errorcode/ErrorCode.lua` 中所有错误码为 Go 常量
    - 翻译所有业务枚举定义（ActivityEnum, ArmyEnum, BattleEnum 等）
    - 实现属性定义（AttrDef）和各模块常量定义
    - _Requirements: 70.1, 70.2, 70.3, 70.4_

  - [x] 1.3 实现日志框架（pkg/logger）
    - 基于 zerolog 实现分级日志（DEBUG/INFO/WARNING/ERROR）
    - 支持文件输出、按日期轮转、JSON 格式
    - 包含时间戳、日志级别、源文件位置等上下文信息
    - _Requirements: 26.1, 26.2, 26.3, 26.4_

  - [x] 1.4 实现服务器配置管理（pkg/config 的配置文件解析部分）
    - 实现 YAML 配置文件解析，兼容 Lua_Server 的 etc/*.conf 格式
    - 支持环境变量占位符替换（$SERVER_ID 等）
    - 为每个服务节点创建独立配置文件（login.yaml, game.yaml 等）
    - _Requirements: 65.1, 65.2, 65.3_

- [x] 2. Sproto 协议编解码器实现（pkg/sproto）
  - [x] 2.1 实现 Sproto Schema 解析器
    - 解析 `.sproto` 文件文本为内部类型定义结构
    - 支持 integer、string、boolean、binary、嵌套结构体、数组、map 类型
    - 支持 package 头（type + session 字段）
    - _Requirements: 5.1, 5.4, 5.5_

  - [x] 2.2 实现 Sproto Encode/Decode 核心编解码
    - 实现 Go struct/map → Sproto binary 编码
    - 实现 Sproto binary → Go map 解码
    - 实现 Pack（pencode）和 Unpack（pdecode）压缩/解压
    - _Requirements: 5.1, 5.5, 33.4, 33.6, 33.7_

  - [ ]* 2.3 Write property test: Sproto 编解码往返（Property 1）
    - **Property 1: Sproto 编解码往返**
    - 对任意有效 Sproto 消息，Encode → Decode 应产生等价对象；Pack → Unpack 应产生相同二进制
    - **Validates: Requirements 5.1, 5.4, 5.5, 5.7, 33.6**

  - [x] 2.4 实现 Sproto RPC Host/Attach 模式
    - 实现 Host：Dispatch 解析客户端请求，PackResponse 打包响应
    - 实现 Attach：PackRequest 打包服务器推送
    - 支持 request-response 和 server-push 两种通信模式
    - _Requirements: 5.2_

  - [ ]* 2.5 Write property test: Sproto RPC Host/Attach 往返（Property 2）
    - **Property 2: Sproto RPC Host/Attach 往返**
    - Attach 打包请求后 Host 应正确解析出协议名、session ID 和请求数据
    - **Validates: Requirements 5.2**

  - [x] 2.6 实现 GateMessage 封装/解封装
    - 实现 GateMessage 的 pack/unpack，支持单次请求包含多个子消息
    - _Requirements: 5.3_

  - [ ]* 2.7 Write property test: GateMessage 封装/解封装往返（Property 3）
    - **Property 3: GateMessage 封装/解封装往返**
    - 封装后再解封装应产生与原始子消息列表等价的结果
    - **Validates: Requirements 5.3**

  - [x] 2.8 实现 Sproto Pretty Printer
    - 将 Sproto 二进制数据格式化为可读文本输出（调试日志用）
    - _Requirements: 5.6_

  - [ ]* 2.9 Write property test: Sproto Pretty Printer 信息完整性（Property 21）
    - **Property 21: Sproto Pretty Printer 信息完整性**
    - Pretty Printer 输出应包含消息中所有字段的名称和值
    - **Validates: Requirements 5.6**

- [x] 3. 加密库实现（pkg/crypt）
  - [x] 3.1 实现 DH 密钥交换（DHExchange, DHSecret）
    - 兼容 Skynet crypt 库的 dhexchange/dhsecret 实现
    - _Requirements: 6.1_

  - [ ]* 3.2 Write property test: DH 密钥交换共享密钥一致性（Property 5）
    - **Property 5: DH 密钥交换共享密钥一致性**
    - DHSecret(a, B) == DHSecret(b, A)
    - **Validates: Requirements 3.2, 6.1**

  - [x] 3.3 实现 DES 加密解密（DESEncode, DESDecode, DESCodec）
    - 兼容 Skynet crypt 库的 desencode/desdecode，8 字节密钥
    - 实现有状态的 DESCodec 用于 Gate 连接
    - _Requirements: 6.2_

  - [ ]* 3.4 Write property test: DES 加解密往返（Property 4）
    - **Property 4: DES 加解密往返**
    - DESEncode → DESDecode 应产生与原始明文相同的数据
    - **Validates: Requirements 3.4, 4.5, 6.2, 6.5**

  - [x] 3.5 实现 HMAC 签名（HMAC64, HashKey）
    - 兼容 Skynet crypt 库的 hmac64/hashkey
    - _Requirements: 6.3_

  - [ ]* 3.6 Write property test: HMAC 签名确定性（Property 6）
    - **Property 6: HMAC 签名确定性**
    - 相同输入产生相同输出
    - **Validates: Requirements 3.3, 4.2, 6.3**

  - [x] 3.7 实现 Base64 编解码
    - 兼容 Skynet crypt 库的 base64encode/base64decode
    - _Requirements: 6.4_

  - [ ]* 3.8 Write property test: Base64 编解码往返（Property 7）
    - **Property 7: Base64 编解码往返**
    - Base64Encode → Base64Decode 应产生与原始字节序列相同的数据
    - **Validates: Requirements 6.4**

- [x] 4. Checkpoint - 核心协议与加密库验证
  - Ensure all tests pass, ask the user if questions arise.

- [x] 5. 网络层实现（pkg/network）
  - [x] 5.1 实现 TCP Server 和 Connection
    - 实现 2 字节大端序包头协议的 TCP 服务器
    - 实现 ConnectionHandler 接口（OnConnect, OnMessage, OnDisconnect）
    - 实现 Connection 的 ReadPacket/WritePacket
    - 支持 DESCodec 集成（握手后设置加解密）
    - _Requirements: 4.1, 4.3, 4.7_

  - [ ]* 5.2 Write property test: 网络包编解码往返（Property 8）
    - **Property 8: 网络包编解码往返**
    - 2 字节大端序包头编码后再解码，应正确还原消息体、game_session 和压缩标志
    - **Validates: Requirements 4.3, 4.4**

  - [x] 5.3 实现定时器框架（pkg/timer）
    - 实现 time.AfterFunc + 时间轮定时器
    - 支持一次性定时器和循环定时器
    - _Requirements: 50.4_

- [ ] 6. 数据层实现 - Entity 框架与数据库
  - [x] 6.1 创建 MySQL 初始化脚本（deployments/sql/init.sql）
    - 创建所有 Common 类型表（c_pkid, c_account, c_map_object 等 22 张表）
    - 创建所有 Role 类型表（d_role, d_user, d_building 等 11 张表）
    - 创建所有排行榜表（c_role_power 等 17 张表）
    - 创建所有联盟子排行榜表（c_guild_role_power 等 9 张表）
    - 统一三列结构：主键列 + value(LONGTEXT) + json(LONGTEXT)
    - _Requirements: 31.1, 31.2, 31.3, 31.4, 31.5, 31.6, 31.7, 32.1_

  - [ ] 6.2 实现数据库迁移框架（deployments/sql/migrations/）
    - 实现版本管理机制，按版本号存放增量迁移脚本
    - 启动时自动执行未应用的迁移脚本
    - _Requirements: 32.4, 32.5_

  - [x] 6.3 实现 Entity 数据管理框架核心（pkg/entity）
    - 实现 TableConfig 定义和四种数据类型分类（CONFIG/COMMON/USER/ROLE）
    - 实现 EntityImpl 核心：MySQL 连接池 + Redis 连接 + Sproto 编解码
    - 实现三层数据访问：内存 → Redis(HGET/HSET) → MySQL
    - 实现 CRUD 操作：Load, Add, Update, Delete
    - _Requirements: 24.1, 24.2, 24.3, 24.4, 24.7, 34.1, 34.5, 34.6, 34.7_

  - [x] 6.4 实现 SingleEntity（Common 类型数据管理）
    - 启动时全量加载到内存
    - 支持 Add, Delete, Update, Get, Set 操作
    - 变更时同步写入 MySQL 和 Redis
    - 支持 lockFlag 原子递增模式
    - _Requirements: 34.2, 38.7_

  - [x] 6.5 实现 MultiEntity（Role 类型子表数据管理）
    - 支持按子索引（buildingIndex, itemIndex 等）的增删改查
    - 实现按需加载和卸载
    - _Requirements: 34.3, 34.4_

  - [x] 6.6 实现 EntityLoad 批量操作模块
    - 实现 loadRole（加载角色全部数据表）
    - 实现 saveRole（保存角色全部数据表）
    - 实现 unLoadRole（卸载角色数据）
    - 实现 deleteRole（删除角色全部数据）
    - 支持 d_user 表的 noLoad 标记
    - _Requirements: 34.8, 34.9, 24.5_

  - [x] 6.7 实现主键 ID 生成器（PkIdMgr）
    - 基于 Redis INCR 的原子性 ID 生成
    - 管理 pkidkey, petkey, itemkey, systemmailkey, chatUniqueIndexkey
    - 启动时从 c_pkid 加载，每 5 秒持久化
    - 启动时扫描所有表 MAX(key) 写入 Redis
    - _Requirements: 35.1, 35.2, 35.3, 35.4, 35.5, 32.3_

  - [ ]* 6.8 Write property test: 主键 ID 唯一性与单调递增（Property 15）
    - **Property 15: 主键 ID 唯一性与单调递增**
    - 连续 N 次生成的 ID 应两两不同且严格单调递增
    - **Validates: Requirements 35.1, 35.2**

  - [x] 6.9 实现 Redis 缓存策略（pkg/entity 的 Redis 部分）
    - 实现 Redis Hash 缓存 Entity 数据
    - 实现 Redis String 存储主键 ID 和表最大 ID
    - 实现 Redis Pipeline 批量操作
    - 支持多 Redis 实例配置
    - db_server 启动时 FLUSHDB 并从 MySQL 重新加载
    - _Requirements: 36.1, 36.2, 36.3, 36.7, 36.8, 36.9_

  - [x] 6.10 实现排行榜管理器（pkg/rank）
    - 实现 Redis Sorted Set 操作：ZADD, ZREVRANGE, ZRANK, ZREM, ZCARD, ZSCORE
    - 实现 Redis + MySQL 双写机制
    - 启动时从 MySQL 加载到 Redis
    - 实现排行榜记录上限管理
    - 支持联盟子排行榜和最强执政官特殊格式
    - 维护 copy 副本用于排名变化展示
    - _Requirements: 23.1, 23.2, 23.3, 36.4, 36.5, 37.1, 37.2, 37.3, 37.4, 37.5, 37.6, 37.7_

  - [ ]* 6.11 Write property test: 排行榜有序性（Property 14）
    - **Property 14: 排行榜有序性**
    - 排行榜查询结果应始终按分数从高到低排序
    - **Validates: Requirements 23.1, 23.2**

  - [ ] 6.12 实现 Db.sproto 和 Common.sproto 数据结构编解码
    - 翻译 Db.sproto 中所有数据结构为 Go struct
    - 翻译 Common.sproto 中所有共享数据结构为 Go struct
    - 实现 Base64 + Sproto pencode/pdecode 的完整序列化流程
    - 支持 debug 模式下 JSON 列写入（alljson/nojson 标记）
    - _Requirements: 33.1, 33.2, 33.3, 33.4, 33.5, 33.7_

  - [ ] 6.13 实现数据库事务与一致性保障
    - 实现角色创建/删除的原子性事务
    - 实现联盟创建/解散的原子性事务
    - 实现 Redis-MySQL 最终一致性（先 Redis 后 MySQL，失败重试队列）
    - 实现在线角色数据每 5 分钟自动保存
    - 实现脏标记机制避免无效写入
    - _Requirements: 38.1, 38.2, 38.3, 38.4, 38.5, 38.6, 40.10_

  - [ ] 6.14 实现数据库性能优化
    - 主键索引和唯一索引（c_account.iggid, c_role_name.name 等）
    - Common 数据分页加载（LIMIT offset, 2000）
    - Sproto 编解码内存池（sync.Pool）
    - goroutine 池限制并发数据库操作
    - _Requirements: 40.1, 40.2, 40.3, 40.4, 40.5, 40.6, 40.7, 40.8, 40.9_

- [ ] 7. Checkpoint - 数据层验证
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 8. 配置表加载器实现（pkg/config）
  - [ ] 8.1 实现配置表加载器（ConfigLoader）
    - 加载 `common/config/gen/` 目录下的配置数据文件
    - 支持按 ID 查询和全表遍历 API
    - 支持热重载
    - _Requirements: 25.1, 25.2, 25.3, 25.4_

  - [ ]* 8.2 Write property test: 配置表加载完整性（Property 16）
    - **Property 16: 配置表加载完整性**
    - Get(id) 查询结果应与源文件对应记录一致，Range 遍历应覆盖所有记录
    - **Validates: Requirements 25.1, 25.2, 25.3**

  - [ ] 8.3 实现系统配置表管理（c_system）和推荐服务器管理（c_recommend）
    - 运行时可修改的系统参数
    - 各地区推荐服务器信息
    - _Requirements: 65.4, 65.5_

- [ ] 9. Login Service 实现（cmd/login, internal/login）
  - [x] 9.1 实现 Login Service 入口和 TCP 监听
    - 创建 `cmd/login/main.go` 服务入口
    - 监听 TCP 端口，接受 Unity 客户端连接
    - 实现独立 MySQL/Redis 连接（loginmysqlip 配置，Redis db4）
    - 实现连接超时管理（10 秒未完成认证断开）
    - _Requirements: 3.1, 3.8, 39.1, 39.2, 39.5, 4.9_

  - [x] 9.2 实现 DH 握手与认证流程
    - 发送 8 字节 Base64 challenge → 接收 DH 公钥 → 计算共享密钥
    - 验证 HMAC 签名（hmac64(challenge, secret)）
    - DES 解密 Token，解析 iggid/accessToken/platform/language/clientaddr/selectGameNode
    - 验证账号信息，支持新账号自动注册
    - _Requirements: 3.2, 3.3, 3.4, 3.5, 39.3, 39.4_

  - [x] 9.3 实现登录响应格式化和错误处理
    - 格式化成功响应：`200 base64(uid)@base64(servername)#base64(subid)@...`
    - 实现错误码返回（400/401/403/406/407/408）
    - 通过 gRPC 调用 Game Service 的 Login 接口
    - _Requirements: 3.6, 3.7_

  - [ ]* 9.4 Write property test: 登录响应格式正确性（Property 9）
    - **Property 9: 登录响应格式正确性**
    - 格式化后的响应中每个 Base64 字段解码后应得到原始参数值
    - **Validates: Requirements 3.6**

- [-] 10. Gate Service 与 Agent 实现（internal/game/gate, internal/game/agent）
  - [x] 10.1 实现 Gate 网关核心
    - 创建 Gate 结构体，集成 TCP Server、Sproto Host/Attach
    - 实现握手认证（解析 `base64(uid)@base64(servername)#base64(subid):index:base64(hmac)`）
    - 实现请求处理：DES 解密 → 解包 GateMessage → 分发到 Agent
    - 实现 Login/Logout gRPC 接口供 Login Service 调用
    - _Requirements: 4.1, 4.2, 4.4, 4.5, 4.6, 4.8_

  - [x] 10.2 实现 Agent 玩家会话管理
    - 实现三态状态机：PRELOGIN → OK → AFK
    - 实现请求队列保证顺序处理
    - 实现推送消息队列和批量发送
    - 实现离线定时器和登出流程
    - 实现重复登录踢出（System_KickConnect）
    - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 7.6, 7.7, 7.8_

  - [ ]* 10.3 Write property test: Agent 状态机转换正确性（Property 10）
    - **Property 10: Agent 状态机转换正确性**
    - 状态转换应满足合法路径，不存在非法转换
    - **Validates: Requirements 7.1, 7.2, 7.3, 7.5**

  - [ ]* 10.4 Write property test: Agent 请求队列顺序保证（Property 11）
    - **Property 11: Agent 请求队列顺序保证**
    - 请求应按入队顺序处理，推送消息按入队顺序发送
    - **Validates: Requirements 7.7, 7.8**

- [ ] 11. Checkpoint - 登录与网关流程验证
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 12. Game Service 入口与 gRPC 服务定义
  - [x] 12.1 创建 Game Service 入口（cmd/game/main.go）
    - 初始化 Gate、Agent 管理器、配置加载、数据库连接
    - 注册 gRPC 内部服务（GameInternal）
    - 实现优雅关闭：遍历所有在线 Agent 执行 saveRole
    - _Requirements: 29.4, 38.6_

  - [ ] 12.2 定义所有 gRPC 内部服务 Proto 文件
    - 定义 GameInternal、ChatInternal、BattleInternal、DBInternal、LogInternal、MonitorInternal 服务接口
    - 生成 Go gRPC 代码
    - _Requirements: 54.1, 54.5_

  - [ ] 12.3 实现协议注册与分发框架（pkg/protocol）
    - 翻译 Protocol.sproto 中所有协议定义
    - 实现协议名 → 处理函数的注册和分发机制
    - _Requirements: 5.1, 5.2_

- [x] 13. 角色管理模块（internal/game/role）
  - [x] 13.1 实现角色列表查询和创建
    - Role_GetRoleList：返回账号下所有角色基本信息
    - Role_CreateRole：验证名称唯一性、国家有效性，创建角色并分配初始资源/建筑/英雄
    - _Requirements: 8.1, 8.2_

  - [x] 13.2 实现角色登录和数据推送
    - Role_RoleLogin：加载角色全部数据，推送完整角色信息，返回聊天服务器连接信息
    - 实现心跳机制（Role_Heart）
    - _Requirements: 8.3, 8.4_

  - [x] 13.3 实现角色基础操作
    - Role_ModifyName（改名）、Role_SetRoleHead（头像）、Role_ChangeCivilization（文明）
    - Role_NoviceGuideStep（新手引导）、Role_ReportSelf（设备信息上报）
    - _Requirements: 8.5, 8.6_

  - [x] 13.4 实现跨天/跨周/跨月定时重置（RoleTimerLogic）
    - crossDay：每日属性重置、每日任务、白银宝箱、活动、VIP 奖励等
    - crossWeek/crossMonth：每周/每月充值礼包重置
    - 角色定时器框架（一次性/循环/跨天定时器）
    - 登出时清理 isLogoutDelete 定时器
    - _Requirements: 50.1, 50.2, 50.3, 50.4, 50.5, 50.6_

- [-] 14. 建筑系统模块（internal/game/building）
  - [x] 14.1 实现建筑创建和升级
    - Build_CreateBuilding：验证类型、位置、资源，创建建筑并启动建造队列
    - Build_UpGradeBuilding：验证前置条件，启动升级队列定时器
    - 建造/升级定时器到期自动完成
    - _Requirements: 9.1, 9.2, 9.3_

  - [x] 14.2 实现建筑队列和加速功能
    - 主队列和第二队列管理
    - Role_SpeedUp：加速道具/宝石缩短建造/升级/训练/研究/治疗时间
    - Build_GetBuildResources：资源建筑产出计算
    - Build_Tavern：酒馆召唤（银/金箱子）
    - _Requirements: 9.4, 9.5, 9.6, 9.7_

  - [x] 14.3 实现城墙系统
    - 城墙耐久管理、燃烧状态（startBurnWall）
    - 城墙维修（resetWallTime）、wallHpNotify 通知
    - 战争狂热 Buff（warCrazy）
    - _Requirements: 58.1, 58.2, 58.3, 58.4, 58.5_

- [-] 15. 军队与士兵模块（internal/game/army）
  - [x] 15.1 实现士兵训练和军队创建
    - Role_TrainArmy：验证兵营等级、资源，启动训练队列
    - Role_AwardArmy：训练完成加入士兵池
    - Role_CreateArmy：验证英雄/士兵可用性，创建军队
    - _Requirements: 10.1, 10.2, 10.3_

  - [x] 15.2 实现军队行军和士兵管理
    - Map_March：行军路径计算、到达时间、行军状态管理
    - Role_Treatment：士兵治疗
    - Role_DisbandArmy：士兵解散和晋升
    - _Requirements: 10.4, 10.5, 10.6_

  - [x] 15.3 实现士兵锁与医院互斥管理
    - SoldierLockMgr：addSoldiersInLock/subSoldiersInLock 互斥保护
    - SeriousInjureMgr：伤兵增加/治疗互斥
    - ServiceBusyCheckMgr：高负载限流
    - _Requirements: 67.1, 67.2, 67.3_

  - [ ]* 15.4 Write property test: 士兵锁互斥性（Property 20）
    - **Property 20: 士兵锁互斥性**
    - 并发操作后士兵数量应等于初始数量加上所有增减量
    - **Validates: Requirements 67.1, 67.2**

  - [x] 15.5 实现军队行军回调与追击系统
    - ArmyMarchCallback：根据 targetType 执行到达逻辑
    - ArmyFollowUpLogic：目标移动时自动调整路径
    - ArmyWalkLogic：实时位置更新和 AOI 同步
    - AttackAroundPosLogic/AttackAroundPosMgr
    - ScoutFollowUpLogic
    - _Requirements: 68.1, 68.2, 68.3, 68.4, 68.5_

- [ ] 16. Checkpoint - 核心游戏逻辑验证
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 17. 大世界地图模块（internal/game/map）
  - [ ] 17.1 实现 AOI 兴趣区域管理（pkg/aoi）
    - Map_Move：视野移动时推送视野范围内地图对象
    - 地图对象管理：资源点、野蛮人、玩家城市、联盟建筑的创建/刷新/删除
    - _Requirements: 11.1, 11.2_

  - [ ] 17.2 实现地图搜索和斥候系统
    - Map_SearchResource：搜索指定类型和等级的资源点
    - Map_SearchBarbarian：搜索指定等级的野蛮人
    - Map_Scouts/Map_ScoutsBack：斥候探索和侦查
    - 迷雾系统（DenseFogInfo）
    - _Requirements: 11.3, 11.4, 11.5, 11.6_

  - [ ] 17.3 实现迁城和书签系统
    - Map_MoveCity：新手迁城、领土迁城、定点迁城、随机迁城
    - Map_AddMarker/Map_ModifyMarker/Map_DeleteMarker：个人和联盟书签
    - _Requirements: 11.7, 11.8_

  - [ ] 17.4 实现 NavMesh 寻路（pkg/navmesh）
    - NavMesh 导航网格加载器（.bin 格式）
    - NavMesh 寻路查询接口
    - NavMeshObstracleMgr：动态障碍物管理
    - checkPosIdle：位置空闲检测
    - _Requirements: 42.1, 42.2, 42.3, 42.7_

  - [ ]* 17.5 Write property test: NavMesh 寻路路径有效性（Property 17）
    - **Property 17: NavMesh 寻路路径有效性**
    - 路径上每个点在可行走区域内，路径连续，起终点正确
    - **Validates: Requirements 42.1, 42.2, 42.4**

  - [ ] 17.6 实现 AStar 路径计算（pkg/astar）
    - AStarMgr：大世界军队行军路径规划
    - CheckPointAStarMgr：关卡地图路径计算
    - PVENavMeshMapMgr：远征 PVE 独立导航网格
    - _Requirements: 42.4, 42.5, 42.6_

  - [ ] 17.7 实现地图省份与区域管理
    - MapProvinceLogic：坐标 → 省份计算
    - MapLevelMgr：等级区域管理
    - MapObjectRefreshMgr：按分组和瓦片周期性刷新
    - MapCityMgr：城市数量管理
    - modifyFullProvice：省份满员管理
    - _Requirements: 57.1, 57.2, 57.3, 57.4, 57.5_

  - [ ]* 17.8 Write property test: 地图省份计算确定性（Property 18）
    - **Property 18: 地图省份计算确定性**
    - 相同坐标始终返回相同省份，省份 ID 在有效范围内
    - **Validates: Requirements 57.1**

  - [ ] 17.9 实现地图固定点与村庄山洞
    - MapFixPointMgr：根据配置放置固定探索点
    - 村庄山洞探索逻辑和探索发现报告邮件
    - _Requirements: 64.1, 64.2, 64.3_

- [x] 18. 英雄系统模块（internal/game/hero）
  - [x] 18.1 实现英雄数据管理和成长系统
    - 英雄等级、星级、经验、技能、天赋树、装备属性管理
    - Hero_HeroInfo：推送完整英雄数据
    - 英雄升级、升星、技能升级
    - 天赋树系统（多页配置和切换）
    - _Requirements: 12.1, 12.2, 12.3, 12.4_

  - [x] 18.2 实现装备系统
    - 装备数据管理（subType 分类）
    - 装备属性计算（s_Equip, s_EquipAtt）
    - 装备合成（s_EquipCompose, s_EquipMaterial）
    - 装备穿戴/卸下，不可叠加逻辑
    - _Requirements: 12.5, 59.1, 59.2, 59.3, 59.4, 59.5_

- [x] 19. 道具系统模块（internal/game/item）
  - [x] 19.1 实现道具背包管理
    - 道具增加、删除、叠加
    - Item_ItemInfo 推送通知
    - Role_BuyResource：宝石购买资源
    - _Requirements: 14.1, 14.2, 14.4_

  - [x] 19.2 实现道具使用和脚本系统
    - Item_ItemUse：根据道具类型执行效果
    - ItemScript：资源增加、加速、Buff、经验、士兵、宝箱、头像、迁城、护盾等
    - getItemPackage：奖励组系统
    - s_ItemRewardChoice：选择奖励系统
    - _Requirements: 14.3, 73.1, 73.2, 73.3, 73.4_

- [x] 20. 科技研究模块（internal/game/technology）
  - [x] 20.1 实现科技研究系统
    - Role_TechnologyLevelUp：验证前置条件，启动研究队列
    - 研究完成推送 Technology_ResearchTechnology
    - 科技属性加成计算（建造/训练/采集/战斗速度等）
    - _Requirements: 13.1, 13.2, 13.3_

- [x] 21. 邮件系统模块（internal/game/email）
  - [x] 21.1 实现邮件收发系统
    - 支持系统邮件、战斗报告、联盟邮件、玩家邮件
    - Email_EmailList 推送通知
    - Email_TakeEnclosure（附件领取）、Email_CollectEmail（收藏）、Email_DeleteEmail（删除）
    - 系统邮件定时发送机制
    - 战斗报告生成和存储（BattleReportEx）
    - _Requirements: 15.1, 15.2, 15.3, 15.4, 15.5_

- [x] 22. 联盟系统模块（internal/game/guild）
  - [x] 22.1 实现联盟基础操作
    - Guild_CreateGuild（创建）、解散、Guild_ApplyJoinGuild（加入申请）、邀请、退出
    - 成员管理：职位任命、权限控制、踢出
    - _Requirements: 16.1, 16.2_

  - [x] 22.2 实现联盟科技、建筑和领地
    - Guild_GuildTechnologies：科技研究和捐献
    - 联盟建筑系统（旗帜、资源中心等）
    - 联盟领地系统（领地范围、收益计算）
    - _Requirements: 16.3, 16.4, 16.5, 74.1, 74.2, 74.3, 74.4_

  - [x] 22.3 实现联盟礼物、求助和留言板
    - 联盟礼物发放和领取
    - 建造/研究求助和帮助
    - GuildMessageBoardMgr：留言板发布/查看/删除，数量上限控制
    - _Requirements: 16.6, 16.7, 62.1, 62.2, 62.3_

  - [x] 22.4 实现联盟建筑初始化与索引管理
    - GuildBuildInitMgr：联盟创建时初始化默认建筑
    - GuildBuildIndexMgr：建筑 ID → 地图对象映射
    - GuildResourcePointIndexMgr：领地内资源点追踪
    - SceneGuildBuildMgr/SceneGuildResourcePointMgr
    - GuildAttrMgr：联盟属性加成计算
    - GuildTimerMgr：联盟定时任务
    - _Requirements: 69.1, 69.2, 69.3, 69.4, 69.5, 69.6_

  - [x] 22.5 实现联盟推荐与搜索
    - GuildRecommendMgr：推荐列表生成
    - 联盟名称/简称模糊搜索
    - GuildIndexMgr：快速查找索引
    - _Requirements: 63.1, 63.2, 63.3_

- [x] 23. 聊天系统模块（cmd/chat, internal/chat）
  - [x] 23.1 实现 Chat Service 独立服务
    - 创建 `cmd/chat/main.go` 服务入口
    - 注册 ChatInternal gRPC 服务
    - 实现频道管理（ChatChannel/ChatChannelEntity）
    - 支持世界频道、联盟频道、私聊频道
    - _Requirements: 17.1, 17.2, 71.1_

  - [x] 23.2 实现聊天消息收发和持久化
    - Chat_SendMsg：消息广播到频道所有在线玩家
    - Chat_PushMsg：推送消息给客户端
    - ChatSave：消息持久化到 c_chat/c_chat_guild 表
    - 消息数量上限控制（saveStorageNum）
    - 屏蔽词过滤（s_ChatBlock）
    - 启动时从数据库加载历史记录
    - _Requirements: 17.3, 17.4, 71.2, 71.3, 71.4, 71.5_

  - [x] 23.3 实现私聊、跑马灯和禁言
    - Chat_Msg2GSQueryPrivateChatLst/Chat_Msg2GSQueryPrivateChatByRid：私聊历史查询
    - Chat_MarqueeNotify：全服公告推送
    - 禁言功能
    - _Requirements: 17.5, 17.6, 17.7_

  - [ ]* 23.4 Write property test: 聊天消息频道路由正确性（Property 12）
    - **Property 12: 聊天消息频道路由正确性**
    - 消息路由到正确频道，频道成员收到消息，非成员不收到
    - **Validates: Requirements 17.2, 17.3, 17.4**

  - [ ]* 23.5 Write property test: 禁言功能有效性（Property 13）
    - **Property 13: 禁言功能有效性**
    - 禁言期间消息被拒绝，到期后可正常发送
    - **Validates: Requirements 17.7**

- [ ] 24. Checkpoint - 社交与通信系统验证
  - Ensure all tests pass, ask the user if questions arise.

- [x] 25. 战斗系统模块（cmd/battle, internal/battle_svc, internal/game/battle）
  - [x] 25.1 实现 Battle Service 独立服务
    - 创建 `cmd/battle/main.go` 服务入口
    - 注册 BattleInternal gRPC 服务
    - BattleLoop：管理多个并发战斗实例生命周期
    - BattleSceneMgr：双方部队状态、回合数、战斗结果
    - BattleIndexMgr：战斗唯一索引分配和查询
    - BattleConfigData：加载战斗配置数据
    - _Requirements: 55.1, 55.2, 55.5, 55.7, 55.8_

  - [x] 25.2 实现战斗引擎核心
    - 回合制战斗计算引擎
    - BattleBuff：增益/减益/持续伤害/持续治疗
    - BattleSkill：技能效果执行（s_SkillBattle, s_SkillStatus）
    - BattleStatus：部队加入/退出/士兵扣减
    - _Requirements: 18.1, 55.3, 55.4, 55.6_

  - [x] 25.3 实现战斗属性和损失计算
    - BattleAttrLogic：综合英雄属性、科技、装备、城市 Buff
    - BattleLosePowerLogic：轻伤/重伤/死亡士兵分配
    - BattleReport：每回合伤害/治疗/技能/Buff 信息
    - _Requirements: 18.2, 18.3, 18.4_

  - [ ] 25.4 实现集结攻击和城市增援
    - RallyLogic：多玩家联合攻击同一目标
    - CityReinforceLogic：联盟成员向城市派遣增援
    - EarlyWarningLogic：被攻击时推送预警通知
    - RepatriationLogic：增援部队遣返（互斥锁保护）
    - _Requirements: 18.5, 18.6, 18.7, 47.1, 47.2, 47.3, 47.4, 47.5_

  - [ ] 25.5 实现城市掠夺系统
    - CityPlunderMgr：计算可掠夺资源数量
    - 掠夺比例计算（s_ResourcesPlunderLoss）
    - 攻击方胜利后资源转移
    - 战斗报告记录掠夺明细
    - _Requirements: 66.1, 66.2, 66.3, 66.4_

- [x] 26. 资源与经济模块（internal/game/role 扩展）
  - [x] 26.1 实现资源管理和产出计算
    - 四种基础资源（食物/木材/石料/金币）+ 宝石管理
    - 资源产出计算（建筑等级 × 时间差）
    - 资源采集系统（军队到达后按速度获取）
    - _Requirements: 19.1, 19.2, 19.3_

  - [x] 26.2 实现资源运输和城市 Buff
    - Transport：联盟成员间资源援助
    - Role_AddBuff：增益效果添加和过期管理
    - MysteryStore：驿站商店定时刷新和购买
    - _Requirements: 19.4, 19.5, 19.6_

- [x] 27. 任务系统模块（internal/game/task）
  - [x] 27.1 实现任务系统
    - 主线/支线/每日/章节四种任务类型
    - TaskStatistics：根据玩家行为自动更新进度
    - 任务完成通知推送
    - 任务奖励发放（邮件或直接发放）
    - _Requirements: 20.1, 20.2, 20.3, 20.4_

- [x] 28. 活动系统模块（internal/game/activity）
  - [x] 28.1 实现活动系统
    - 活动时间管理（配置表控制开启/关闭）
    - 多种活动类型：积分、排名、兑换、转盘等
    - Activity_Reward（奖励领取）、Activity_Rank（排行榜查询）
    - Activity_ScheduleInfo（进度追踪）
    - _Requirements: 21.1, 21.2, 21.3, 21.4_

  - [ ] 28.2 实现地狱活动系统
    - HellActivityProxy：活动数据管理（c_hallActivity）
    - 开启/进行/结束三阶段状态管理
    - 地狱活动排行榜（c_hell_activity_rank）
    - 结束时按排名发放奖励
    - _Requirements: 60.1, 60.2, 60.3, 60.4_

- [x] 29. 充值与商城模块（internal/game/recharge）
  - [x] 29.1 实现充值和商城系统
    - Recharge_RechargeInfo：充值回调处理和商品发放
    - 多种商城类型：每日特惠、限时礼包、VIP 商店、崛起之路
    - VIP 系统（经验 → 等级 → 特权）
    - 成长基金、首充奖励等一次性商品
    - _Requirements: 22.1, 22.2, 22.3, 22.4_

- [x] 30. 纪念碑与圣地系统（internal/game/monument, internal/game/holyland）
  - [x] 30.1 实现纪念碑（文明进化）系统
    - MonumentMgr：纪念碑阶段和进度管理
    - MonumentLogic:setSchedule：多种进度类型统计
    - 阶段自动结束和下一阶段开启（monumentEnd）
    - getMonumentReward：个人/联盟/排名奖励发放
    - fixData：进度数据修正
    - Monument_End 通知和关联逻辑（圣地解锁、迷雾全开）
    - pushMonument：登录时推送纪念碑信息
    - _Requirements: 43.1, 43.2, 43.3, 43.4, 43.5, 43.6, 43.7_

  - [x] 30.2 实现圣地系统
    - HolyLandMgr：圣地占领状态、类型分类
    - reinforceHolyLand：圣地增援
    - SceneHolyLandMgr：驻守队长选举、部队进出、容量上限
    - HolyLandGuardMgr：守护者 NPC 定时刷新
    - GuildHolyLandMgr：联盟圣地追踪和重复占领检查
    - 圣地被攻破时遣返驻守部队
    - _Requirements: 44.1, 44.2, 44.3, 44.4, 44.5, 44.6, 44.7, 44.8_

- [x] 31. 野蛮人城寨与城市隐藏系统
  - [x] 31.1 实现野蛮人城寨系统
    - MonsterCityMgr：按瓦片区域管理城寨
    - monsterCityRefresh：根据纪念碑进度刷新
    - monsterCityTimeOut：超时删除
    - addMonsterCityRefreshTimer：周期性刷新定时器
    - 启动时初始化服务瓦片索引
    - _Requirements: 45.1, 45.2, 45.3, 45.4, 45.5, 45.6_

  - [x] 31.2 实现城市隐藏与回收系统
    - CityHideMgr：离线玩家城市追踪
    - checkCityHide：根据配置判断隐藏
    - 隐藏操作：解散部队、召回斥候/运输队、退出战斗、离开地图
    - 低等级角色自动退出联盟
    - 重新登录时随机分配位置
    - 登出时添加到检查队列
    - _Requirements: 46.1, 46.2, 46.3, 46.4, 46.5, 46.6_

- [x] 32. 符文与召唤怪物系统
  - [x] 32.1 实现符文系统
    - RuneMgr：按瓦片区域管理符文
    - SceneRuneMgr：地图显示和交互
    - 符文刷新机制（守护者刷新时同步）
    - 拾取效果应用
    - _Requirements: 48.1, 48.2, 48.3, 48.4_

  - [x] 32.2 实现召唤怪物系统
    - MonsterSummonMgr：道具/事件召唤特殊怪物
    - MonsterFollowUpLogic：怪物追击
    - MonsterPartolLogic：怪物巡逻
    - NpcScript：配置驱动行为模式
    - _Requirements: 49.1, 49.2, 49.3, 49.4_

- [x] 33. 远征系统模块（internal/game/expedition）
  - [x] 33.1 实现远征系统
    - 远征地图管理和关卡通关
    - 远征商店（远征币购买道具）
    - 远征 PVE 战斗（独立 NavMesh 地图）
    - _Requirements: 27.1, 27.2, 27.3_

- [x] 34. 国王与角色移民系统
  - [x] 34.1 实现国王系统
    - c_king 表数据管理
    - 国王特权（全服公告、封号/解封、称号授予）
    - 任期结束自动清除和新竞选
    - _Requirements: 61.1, 61.2, 61.3_

  - [x] 34.2 实现角色移民系统
    - RoleImmigrateMgr：角色跨服迁移
    - 移民条件验证
    - 数据迁移流程（保存→删除→更新 gameNode）
    - 城市从源服移除，目标服重新放置
    - _Requirements: 51.1, 51.2, 51.3, 51.4_

- [ ] 35. Checkpoint - 业务逻辑模块验证
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 36. 辅助服务实现
  - [ ] 36.1 实现 DB Service（cmd/db, internal/db）
    - 创建 `cmd/db/main.go` 服务入口
    - 注册 DBInternal gRPC 服务（Load/Save/Delete）
    - 启动时 FLUSHDB 并从 MySQL 加载 Common 数据和排行榜到 Redis
    - _Requirements: 24.1, 24.2, 36.8_

  - [ ] 36.2 实现 Log Service（cmd/log, internal/log_svc）
    - 创建 `cmd/log/main.go` 服务入口
    - 注册 LogInternal gRPC 服务
    - LogImpl：日志写入文件或数据库
    - LogLogic：记录所有运营事件类型（roleCreate/roleLogin/roleLogout 等 15+ 种）
    - getRoleLogRequireInfo：角色详细信息快照（40+ 字段）
    - LogTimer：定时汇总在线人数
    - _Requirements: 56.1, 56.2, 56.3, 56.4, 56.5, 56.6_

  - [ ] 36.3 实现 Center Service（cmd/center, internal/center）
    - 创建 `cmd/center/main.go` 服务入口
    - 跨服联盟查询、服务器列表管理、全局配置分发
    - 服务节点发现机制
    - _Requirements: 54.2, 54.3_

  - [ ] 36.4 实现 Monitor Service（cmd/monitor, internal/monitor）
    - 创建 `cmd/monitor/main.go` 服务入口
    - HTTP API：查询服务器状态（在线人数、内存、goroutine 数量）
    - Web 管理接口：`/api/lists.php`（服务器列表）、`/api/login.php`（账号验证）
    - 注册 MonitorInternal gRPC 服务（ReloadConfig/CloseCluster/GetStatus）
    - 集群关闭流程协调（restartCluster/closeCluster）
    - 发布/订阅模式配置同步
    - _Requirements: 29.1, 29.2, 29.3, 29.4, 54.6_

  - [ ] 36.5 实现 Push Service（cmd/push, internal/push）
    - 创建 `cmd/push/main.go` 服务入口
    - AuthToken：推送令牌认证
    - PushMgr：推送消息管理（s_PushMessageData, s_PushMessageGroup）
    - AlarmMgr：紧急事件即时推送
    - MessagePush：对接第三方推送服务（FCM/APNs）
    - Role_SettingPush：推送设置管理
    - _Requirements: 28.1, 28.2, 28.3, 72.1, 72.2, 72.3, 72.4_

  - [ ] 36.6 实现多线路服务管理
    - MultiSnax：同一服务类型多实例支持
    - 索引分配负载
    - _Requirements: 54.4_

- [x] 37. GM/PM 命令与热更新系统
  - [x] 37.1 实现 GM/PM 命令系统
    - PMLogic：仅 debug 模式启用
    - 支持所有 GM 命令类别（modifyAttr/addItem/addSoldiers 等 20+ 种）
    - WebCmd.pmCmd：HTTP 接口接收，JSONP 格式返回
    - 执行后自动同步数据给客户端
    - _Requirements: 52.1, 52.2, 52.3, 52.4_

  - [ ] 37.2 实现配置热重载和服务器热更新
    - reloadConfig：Monitor 广播通知所有节点重新加载配置
    - hotfix：运行时替换指定模块逻辑
    - MonitorPublish/MonitorSubscribe：集群级配置同步
    - Web 接口触发
    - _Requirements: 53.1, 53.2, 53.3, 53.4_

- [x] 38. GC 与内存管理（pkg 层扩展）
  - [x] 38.1 实现 GC 管理和内存监控
    - 定期内存回收策略
    - HTTP API 暴露内存使用量、goroutine 数量等指标
    - sync.Pool 对象池复用（Sproto 缓冲区、消息对象）
    - _Requirements: 75.1, 75.2, 75.3_

- [ ] 39. Docker 容器化部署
  - [x] 39.1 创建 Dockerfile（多阶段构建）
    - 构建阶段：Go 编译所有服务二进制
    - 运行阶段：基于 Alpine Linux 最小镜像
    - _Requirements: 2.5_

  - [x] 39.2 创建 Docker Compose 编排配置
    - 定义所有服务节点容器（login, game, chat, battle, db, log, center, monitor, push）
    - 定义 MySQL、Redis 依赖容器（健康检查）
    - 按依赖顺序启动，环境变量配置覆盖
    - 数据库初始化脚本自动执行（docker-entrypoint-initdb.d）
    - MySQL 数据卷持久化，Redis RDB+AOF 持久化
    - 30 秒内完成初始化
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.6, 2.7, 41.1, 41.5_

  - [ ]* 39.3 Write property test: 服务器配置解析往返（Property 19）
    - **Property 19: 服务器配置解析往返**
    - 解析后的配置对象应包含所有配置项，环境变量占位符正确替换
    - **Validates: Requirements 65.1, 65.2, 65.3**

- [x] 40. 数据库备份与恢复脚本
  - [x] 40.1 创建备份和恢复脚本
    - `scripts/backup.sh`：mysqldump 全量备份，支持保留天数配置
    - `scripts/restore.sh`：从备份文件恢复
    - _Requirements: 41.2, 41.3, 41.4_

- [x] 41. HTML 服务器文档（go_server/docs/）
  - [x] 41.1 创建 HTML 格式服务器文档
    - 架构概览（架构图、服务节点职责和通信关系）
    - 服务节点说明
    - 启动流程（docker-compose up → 服务就绪完整流程）
    - 配置说明
    - 协议说明（Sproto 格式、握手流程图、消息收发流程图）
    - 编码规范（Go 编码规范、命名约定、错误处理、日志规范）
    - 部署指南（Docker 部署步骤、环境变量、数据库初始化）
    - API 文档
    - 响应式 HTML 布局，导航目录和代码高亮
    - _Requirements: 30.1, 30.2, 30.3, 30.4, 30.5, 30.6, 30.7, 30.8_

- [ ] 42. Final checkpoint - 全部功能验证
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional and can be skipped for faster MVP
- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation
- Property tests validate universal correctness properties from the design document (21 properties total)
- Unit tests validate specific examples and edge cases
- 属性测试使用 [rapid](https://github.com/flyingmutant/rapid) 库，每个属性至少 100 次迭代
- 标签格式：`Feature: lua-to-go-server, Property {number}: {property_text}`

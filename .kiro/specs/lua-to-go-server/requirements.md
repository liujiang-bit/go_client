# 需求文档：Lua 服务器翻译为 Go 语言实现

## 简介

将现有基于 Skynet 框架的 Lua 游戏服务器（rok_server）完整翻译为符合 Go 语言开发习惯的新服务器实现。新服务器需在 `go_server` 目录中独立构建，使用 Docker 管理部署，并能与现有 Unity C# 客户端（使用 Sproto 协议、DES 加密、DH 密钥交换握手）完全对接，实现即启即用。同时需编写完善的 HTML 格式服务器文档。

## 术语表

- **Go_Server**: 新建的 Go 语言游戏服务器，位于 `go_server` 目录
- **Lua_Server**: 现有基于 Skynet 框架的 Lua 服务器，位于 `rok_server` 目录
- **Unity_Client**: 现有 Unity C# SLG 游戏客户端，使用 PureMVC + ILRuntime + Sproto
- **Sproto**: 项目使用的二进制协议序列化格式（非 Protobuf），定义在 `.sproto` 文件中
- **Skynet**: Lua_Server 使用的 Actor 模型服务器框架
- **Login_Service**: 负责账号验证、DH 密钥交换、Token 认证的登录服务
- **Game_Service**: 负责游戏核心逻辑处理的游戏服务
- **Chat_Service**: 负责聊天消息收发的聊天服务
- **Gate_Service**: 负责客户端 TCP 连接管理、协议分发的网关服务
- **Agent**: 玩家会话代理，管理单个玩家的连接状态和消息分发
- **DH_Handshake**: Diffie-Hellman 密钥交换握手流程，用于建立加密通信
- **DES_Codec**: DES 加密解密编解码器，用于通信数据加密
- **GateMessage**: Sproto 协议的外层包装消息，包含多个子消息的批量传输容器
- **Docker_Compose**: Docker 编排工具，用于一键启动所有服务器节点和依赖服务
- **Config_Loader**: 配置表加载器，负责读取策划配置数据（CSV/二进制格式）
- **NavMesh**: 导航网格，用于大世界地图寻路
- **AOI**: Area of Interest，兴趣区域管理，用于大世界视野同步
- **EntityImpl**: 数据管理核心实现，负责 Sproto 序列化/反序列化、MySQL CRUD、Redis 缓存的统一封装
- **EntityLoad**: 数据加载模块，提供角色数据的批量加载、保存、卸载、删除操作
- **SingleEntity**: 单行实体模式，用于 Common 类型数据，启动时全量加载到内存
- **MultiEntity**: 多行实体模式，用于 Role 类型的子表数据（如建筑、道具），支持按子索引增删改查
- **PkIdMgr**: 主键 ID 管理器，基于 Redis INCR 命令实现原子性全局唯一 ID 生成
- **RankMgr**: 排行榜管理器，使用 Redis Sorted Set 实现实时排名，同时持久化到 MySQL
- **Db.sproto**: 数据库序列化协议定义文件，定义所有数据表的 Sproto 结构
- **Common.sproto**: 公共协议结构定义文件，定义 PosInfo、SoldierInfo 等共享数据类型
- **Battle_Service**: 独立战斗计算服务，负责回合制战斗的 Buff、技能、伤害计算
- **Log_Service**: 独立运营日志服务，负责接收和存储各类运营日志数据
- **Push_Service**: 独立推送通知服务，负责向离线玩家发送推送消息
- **Center_Service**: 集群中心节点服务，负责跨服联盟查询、服务器列表管理
- **Monitor_Service**: 监控管理服务，负责集群状态监控、配置重载、服务器重启协调
- **MonumentMgr**: 纪念碑事件管理器，管理服务器文明进化阶段和进度
- **HolyLandMgr**: 圣地管理器，管理圣地占领状态、增援部队和守护者
- **MonsterCityMgr**: 野蛮人城寨管理器，管理城寨的刷新、存活和删除
- **CityHideMgr**: 城市隐藏管理器，管理离线玩家城市的自动隐藏和恢复
- **RuneMgr**: 符文管理器，管理大世界地图上符文的刷新和拾取
- **ItemScript**: 道具脚本系统，根据道具类型执行对应的使用效果
- **MapProvinceLogic**: 地图省份逻辑，根据坐标计算所在省份和区域

## 需求

### 需求 1：项目结构与构建系统

**用户故事：** 作为开发者，我希望 Go 服务器有清晰的项目结构和构建系统，以便于开发维护和部署。

#### 验收标准

1. THE Go_Server SHALL 在项目根目录下的 `go_server` 目录中独立存在，不修改 Lua_Server 和现有 Go 服务器（Server 目录）的任何代码
2. THE Go_Server SHALL 使用 Go Modules 管理依赖，go.mod 中的 module 名称为 `go_server`
3. THE Go_Server SHALL 采用如下目录结构：`cmd/`（各服务入口）、`internal/`（内部包）、`pkg/`（可复用包）、`configs/`（配置文件）、`docs/`（HTML 文档）、`scripts/`（脚本）、`deployments/`（Docker 相关文件）
4. THE Go_Server SHALL 使用 Go 1.21 或更高版本编译
5. THE Go_Server SHALL 提供 Makefile，支持 `make build`、`make test`、`make docker` 命令

### 需求 2：Docker 容器化部署

**用户故事：** 作为运维人员，我希望使用 Docker 一键启动整个服务器集群，以便快速部署和测试。

#### 验收标准

1. THE Docker_Compose SHALL 定义所有服务节点（Login_Service、Game_Service、Chat_Service、数据库服务）的编排配置
2. THE Docker_Compose SHALL 包含 MySQL、Redis 依赖服务的容器定义，并配置健康检查
3. WHEN 执行 `docker-compose up` 命令时，THE Docker_Compose SHALL 按依赖顺序启动所有服务，MySQL 和 Redis 先于游戏服务启动
4. THE Docker_Compose SHALL 提供数据库初始化脚本的自动执行机制，在首次启动时创建所需的数据库表结构
5. THE Go_Server SHALL 提供多阶段构建的 Dockerfile，最终镜像基于 Alpine Linux 以减小体积
6. WHEN 所有容器启动完成后，THE Go_Server SHALL 在 30 秒内完成初始化并开始接受客户端连接
7. THE Docker_Compose SHALL 通过环境变量支持配置覆盖，包括数据库地址、端口、密码等

### 需求 3：登录认证服务（Login_Service）

**用户故事：** 作为玩家，我希望通过 Unity 客户端登录服务器，以便开始游戏。

#### 验收标准

1. THE Login_Service SHALL 监听 TCP 端口，接受 Unity_Client 的连接请求
2. WHEN Unity_Client 连接时，THE Login_Service SHALL 执行完整的 DH_Handshake 流程：发送 8 字节 Base64 编码的随机 challenge，接收客户端 DH 公钥，计算共享密钥
3. WHEN DH_Handshake 完成后，THE Login_Service SHALL 验证客户端发送的 HMAC 签名（hmac64(challenge, secret)）
4. WHEN HMAC 验证通过后，THE Login_Service SHALL 使用 DES 密钥解密客户端 Token，解析出 iggid、accessToken、platform、language、clientaddr、selectGameNode 六个字段
5. THE Login_Service SHALL 验证账号信息（iggid + accessToken），支持新账号自动注册和已有账号登录
6. WHEN 认证成功时，THE Login_Service SHALL 返回 `200 base64(uid)@base64(servername)#base64(subid)@base64(connectip)@base64(connectport)@base64(connectrealip)@base64(gamenode)@base64(uid)` 格式的响应
7. IF 认证失败，THEN THE Login_Service SHALL 返回对应错误码：400（Bad Request）、401（Unauthorized）、403（Forbidden）、406（Not Acceptable）、407（User Ban）、408（Token Expire）
8. THE Login_Service SHALL 支持多实例并发登录处理，单实例支持至少 20 个并发认证连接

### 需求 4：游戏网关服务（Gate_Service）

**用户故事：** 作为玩家，我希望在登录认证后能无缝连接到游戏服务器，以便进行游戏操作。

#### 验收标准

1. THE Gate_Service SHALL 监听 TCP 端口，接受经过 Login_Service 认证后重定向的客户端连接
2. WHEN 客户端发送重定向认证包（格式：`base64(uid)@base64(servername)#base64(subid):index:base64(hmac)`）时，THE Gate_Service SHALL 验证 HMAC 签名并返回 `200 OK` 或对应错误码
3. THE Gate_Service SHALL 使用 2 字节大端序包头表示包体长度的协议格式
4. WHEN 收到客户端请求包时，THE Gate_Service SHALL 解析包尾的 4 字节 game_session 和 1 字节压缩标志
5. THE Gate_Service SHALL 使用 DES_Codec 对收到的数据包进行解密，对发送的数据包进行加密
6. THE Gate_Service SHALL 将解密后的 Sproto 数据分发给对应的 Agent 处理
7. THE Gate_Service SHALL 支持至少 1024 个并发客户端连接
8. WHEN 客户端断开连接时，THE Gate_Service SHALL 通知对应的 Agent 进入 AFK（离线）状态
9. IF 客户端在 10 秒内未完成认证，THEN THE Gate_Service SHALL 主动断开该连接

### 需求 5：Sproto 协议编解码

**用户故事：** 作为开发者，我希望 Go 服务器能正确编解码 Sproto 协议，以便与 Unity 客户端正常通信。

#### 验收标准

1. THE Go_Server SHALL 实现完整的 Sproto 二进制协议编解码器，兼容 `rok_server/work/common/protocol/Protocol.sproto` 中定义的所有消息类型
2. THE Go_Server SHALL 实现 Sproto RPC 的 host/attach 模式，支持 request-response 和 server-push 两种通信模式
3. THE Go_Server SHALL 实现 GateMessage 的封装和解封装，支持单次请求中包含多个子消息的批量处理
4. THE Go_Server SHALL 正确处理 Sproto 的 package 头（type + session 字段）
5. THE Go_Server SHALL 支持 Sproto 中定义的所有数据类型：integer、string、boolean、binary、以及嵌套结构体和数组
6. THE Go_Server SHALL 提供 Sproto 协议的 Pretty_Printer，能将 Sproto 二进制数据格式化为可读的文本输出（用于调试日志）
7. FOR ALL 有效的 Sproto 消息，编码后再解码 SHALL 产生与原始消息等价的对象（往返属性）

### 需求 6：DES 加密与 DH 密钥交换

**用户故事：** 作为开发者，我希望 Go 服务器的加密实现与 Lua 服务器完全一致，以便客户端无需修改即可连接。

#### 验收标准

1. THE Go_Server SHALL 实现与 Skynet crypt 库完全兼容的 DH 密钥交换算法（dhexchange、dhsecret）
2. THE Go_Server SHALL 实现与 Skynet crypt 库完全兼容的 DES 加密解密（desencode、desdecode），使用 8 字节密钥
3. THE Go_Server SHALL 实现与 Skynet crypt 库完全兼容的 HMAC 签名（hmac64、hashkey）
4. THE Go_Server SHALL 实现 Base64 编解码，兼容 Skynet crypt 库的 base64encode/base64decode
5. FOR ALL 随机生成的 8 字节密钥和任意长度的明文数据，DES 加密后再解密 SHALL 产生与原始数据相同的结果（往返属性）

### 需求 7：玩家会话管理（Agent）

**用户故事：** 作为玩家，我希望服务器能正确管理我的在线状态，以便断线重连时不丢失游戏进度。

#### 验收标准

1. THE Agent SHALL 管理玩家的连接状态，包括：PRELOGIN（预登录）、OK（在线）、AFK（离线等待）三种状态
2. WHEN 玩家连接认证成功时，THE Agent SHALL 将状态从 PRELOGIN 转为 OK
3. WHEN 玩家断开连接时，THE Agent SHALL 将状态转为 AFK，并启动离线定时器
4. WHEN 离线定时器到期时，THE Agent SHALL 执行玩家登出流程，保存并卸载玩家数据
5. WHEN 玩家在 AFK 状态下重新连接时，THE Agent SHALL 恢复 OK 状态，取消离线定时器
6. WHEN 同一账号重复登录时，THE Agent SHALL 踢出旧连接，向旧客户端发送 System_KickConnect 通知
7. THE Agent SHALL 维护推送消息队列，定时将累积的服务器推送消息批量发送给客户端
8. THE Agent SHALL 使用请求队列保证同一玩家的请求按序处理，避免并发数据竞争

### 需求 8：角色管理模块

**用户故事：** 作为玩家，我希望能创建角色、选择角色并登录游戏，以便开始游戏体验。

#### 验收标准

1. WHEN 玩家请求角色列表（Role_GetRoleList）时，THE Game_Service SHALL 返回该账号下所有角色的基本信息
2. WHEN 玩家创建角色（Role_CreateRole）时，THE Game_Service SHALL 验证名称唯一性、国家有效性，创建角色并分配初始资源、建筑、英雄
3. WHEN 玩家登录角色（Role_RoleLogin）时，THE Game_Service SHALL 加载角色全部数据，推送完整的角色信息给客户端，并返回聊天服务器连接信息
4. THE Game_Service SHALL 实现心跳机制（Role_Heart），响应客户端心跳请求并返回服务器时间
5. THE Game_Service SHALL 实现角色改名（Role_ModifyName）、修改头像（Role_SetRoleHead）、更换文明（Role_ChangeCivilization）等基础操作
6. THE Game_Service SHALL 实现新手引导步骤记录（Role_NoviceGuideStep）和客户端设备信息上报（Role_ReportSelf）

### 需求 9：建筑系统模块

**用户故事：** 作为玩家，我希望能建造和升级城内建筑，以便发展我的城市。

#### 验收标准

1. WHEN 玩家请求创建建筑（Build_CreateBuilding）时，THE Game_Service SHALL 验证建筑类型、位置合法性、资源是否充足，创建建筑并启动建造队列
2. WHEN 玩家请求升级建筑（Build_UpGradeBuilding）时，THE Game_Service SHALL 验证前置条件（市政厅等级、资源），启动升级队列定时器
3. WHEN 建造/升级定时器到期时，THE Game_Service SHALL 自动完成建筑建造/升级，更新建筑等级并推送通知给客户端
4. THE Game_Service SHALL 实现建筑队列管理，支持主队列和第二队列（通过道具或宝石解锁）
5. THE Game_Service SHALL 实现加速功能（Role_SpeedUp），支持使用加速道具或宝石缩短建造/升级/训练/研究/治疗时间
6. THE Game_Service SHALL 实现资源建筑的产出计算（Build_GetBuildResources），根据建筑等级和时间差计算累积资源
7. THE Game_Service SHALL 实现酒馆召唤（Build_Tavern），支持银箱子和金箱子的免费和付费抽取

### 需求 10：军队与士兵模块

**用户故事：** 作为玩家，我希望能训练士兵和组建军队，以便进行战斗和采集。

#### 验收标准

1. WHEN 玩家请求训练士兵（Role_TrainArmy）时，THE Game_Service SHALL 验证兵营等级、资源消耗，启动训练队列
2. WHEN 训练完成时，THE Game_Service SHALL 通过 Role_AwardArmy 将士兵加入玩家的士兵池
3. WHEN 玩家创建军队（Role_CreateArmy）时，THE Game_Service SHALL 验证英雄和士兵可用性，创建军队并根据 targetType 执行对应行为（行军、攻击、采集）
4. THE Game_Service SHALL 实现军队行军系统（Map_March），支持行军路径计算、到达时间计算、行军状态管理
5. THE Game_Service SHALL 实现士兵治疗系统（Role_Treatment），支持治疗重伤士兵
6. THE Game_Service SHALL 实现士兵解散（Role_DisbandArmy）和士兵晋升功能

### 需求 11：大世界地图模块

**用户故事：** 作为玩家，我希望能在大世界地图上探索、采集和战斗，以便获取资源和扩展势力。

#### 验收标准

1. THE Game_Service SHALL 实现 AOI 兴趣区域管理，当玩家移动视野（Map_Move）时推送视野范围内的地图对象信息
2. THE Game_Service SHALL 实现地图对象管理，包括资源点、野蛮人、玩家城市、联盟建筑等对象的创建、刷新和删除
3. WHEN 玩家请求搜索资源（Map_SearchResource）时，THE Game_Service SHALL 返回指定类型和等级的资源点坐标列表
4. WHEN 玩家请求搜索野蛮人（Map_SearchBarbarian）时，THE Game_Service SHALL 返回指定等级的野蛮人坐标列表
5. THE Game_Service SHALL 实现斥候系统（Map_Scouts、Map_ScoutsBack），支持斥候探索和侦查
6. THE Game_Service SHALL 实现迷雾系统，管理玩家的地图迷雾探索状态（DenseFogInfo）
7. THE Game_Service SHALL 实现迁城功能（Map_MoveCity），支持新手迁城、领土迁城、定点迁城、随机迁城
8. THE Game_Service SHALL 实现地图书签系统（Map_AddMarker、Map_ModifyMarker、Map_DeleteMarker），支持个人和联盟书签

### 需求 12：英雄系统模块

**用户故事：** 作为玩家，我希望能管理和培养英雄，以便提升军队战斗力。

#### 验收标准

1. THE Game_Service SHALL 实现英雄数据管理，包括英雄等级、星级、经验、技能、天赋树、装备等属性
2. WHEN 客户端请求英雄信息（Hero_HeroInfo）时，THE Game_Service SHALL 推送完整的英雄数据
3. THE Game_Service SHALL 实现英雄升级、升星、技能升级等成长系统
4. THE Game_Service SHALL 实现英雄天赋树系统，支持多页天赋配置和切换
5. THE Game_Service SHALL 实现英雄装备系统，支持装备穿戴和卸下

### 需求 13：科技研究模块

**用户故事：** 作为玩家，我希望能研究科技提升各项属性，以便增强城市和军队实力。

#### 验收标准

1. WHEN 玩家请求科技升级（Role_TechnologyLevelUp）时，THE Game_Service SHALL 验证前置条件（学院等级、资源），启动研究队列
2. WHEN 研究完成时，THE Game_Service SHALL 更新科技等级并推送 Technology_ResearchTechnology 通知
3. THE Game_Service SHALL 实现科技属性加成计算，科技效果应用到对应的游戏系统（建造速度、训练速度、采集速度、战斗属性等）

### 需求 14：道具系统模块

**用户故事：** 作为玩家，我希望能获取和使用道具，以便辅助游戏进程。

#### 验收标准

1. THE Game_Service SHALL 实现道具背包管理，支持道具的增加、删除、叠加
2. WHEN 道具变化时，THE Game_Service SHALL 推送 Item_ItemInfo 通知给客户端
3. THE Game_Service SHALL 实现道具使用功能（Item_ItemUse），根据道具类型执行对应效果（加速、资源、增益等）
4. THE Game_Service SHALL 实现宝石购买资源功能（Role_BuyResource）

### 需求 15：邮件系统模块

**用户故事：** 作为玩家，我希望能收发邮件，以便获取系统通知和战斗报告。

#### 验收标准

1. THE Game_Service SHALL 实现邮件收发系统，支持系统邮件、战斗报告邮件、联盟邮件、玩家邮件等类型
2. WHEN 有新邮件时，THE Game_Service SHALL 推送 Email_EmailList 通知给客户端
3. THE Game_Service SHALL 实现邮件附件领取（Email_TakeEnclosure）、邮件收藏（Email_CollectEmail）、邮件删除（Email_DeleteEmail）
4. THE Game_Service SHALL 实现系统邮件定时发送机制，根据配置表在特定条件下自动发送系统邮件
5. THE Game_Service SHALL 实现战斗报告的生成和存储，包含详细的战斗数据（BattleReportEx）

### 需求 16：联盟系统模块

**用户故事：** 作为玩家，我希望能创建或加入联盟，以便与其他玩家协作。

#### 验收标准

1. THE Game_Service SHALL 实现联盟的创建（Guild_CreateGuild）、解散、加入申请（Guild_ApplyJoinGuild）、邀请、退出等基础操作
2. THE Game_Service SHALL 实现联盟成员管理，包括职位任命、权限控制、成员踢出
3. THE Game_Service SHALL 实现联盟科技研究和捐献系统（Guild_GuildTechnologies）
4. THE Game_Service SHALL 实现联盟建筑系统，支持联盟旗帜、资源中心等建筑的建造和管理
5. THE Game_Service SHALL 实现联盟领地系统，管理联盟领土范围和领地收益
6. THE Game_Service SHALL 实现联盟礼物系统，支持联盟礼物的发放和领取
7. THE Game_Service SHALL 实现联盟求助系统，支持建造/研究求助和帮助

### 需求 17：聊天系统模块

**用户故事：** 作为玩家，我希望能与其他玩家聊天，以便社交互动。

#### 验收标准

1. THE Chat_Service SHALL 作为独立服务运行，通过内部 RPC 与 Game_Service 通信
2. THE Chat_Service SHALL 支持世界频道、联盟频道、私聊频道等多种聊天频道
3. WHEN 玩家发送聊天消息（Chat_SendMsg）时，THE Chat_Service SHALL 将消息广播给对应频道的所有在线玩家
4. THE Chat_Service SHALL 推送聊天消息（Chat_PushMsg）给客户端，包含发送者信息、频道类型、消息内容、时间戳
5. THE Chat_Service SHALL 实现私聊历史记录查询（Chat_Msg2GSQueryPrivateChatLst、Chat_Msg2GSQueryPrivateChatByRid）
6. THE Chat_Service SHALL 实现跑马灯系统（Chat_MarqueeNotify），支持全服公告推送
7. THE Chat_Service SHALL 实现禁言功能，被禁言的玩家在禁言期间无法发送消息

### 需求 18：战斗系统模块

**用户故事：** 作为玩家，我希望能与野蛮人和其他玩家战斗，以便获取资源和提升实力。

#### 验收标准

1. THE Game_Service SHALL 实现回合制战斗计算引擎，根据双方部队属性、英雄技能、科技加成计算战斗结果
2. THE Game_Service SHALL 实现战斗属性计算（BattleAttrLogic），综合英雄属性、科技加成、装备加成、城市 Buff 等
3. THE Game_Service SHALL 实现战斗损失计算（BattleLosePowerLogic），包括轻伤、重伤、死亡的士兵分配
4. THE Game_Service SHALL 实现战斗报告生成（BattleReport），包含每回合的伤害、治疗、技能、Buff 信息
5. THE Game_Service SHALL 实现集结攻击系统（RallyLogic），支持多个玩家联合攻击同一目标
6. THE Game_Service SHALL 实现城市增援系统（CityReinforceLogic），支持联盟成员向城市派遣增援部队
7. THE Game_Service SHALL 实现预警系统（EarlyWarningLogic），当玩家城市被攻击时推送预警通知

### 需求 19：资源与经济模块

**用户故事：** 作为玩家，我希望能管理城市资源和进行经济活动，以便支撑城市发展。

#### 验收标准

1. THE Game_Service SHALL 实现四种基础资源（食物、木材、石料、金币）和宝石的管理
2. THE Game_Service SHALL 实现资源产出计算，根据资源建筑等级和时间差计算累积产出
3. THE Game_Service SHALL 实现资源采集系统，军队到达资源点后按采集速度获取资源
4. THE Game_Service SHALL 实现资源运输系统（Transport），支持联盟成员间的资源援助
5. THE Game_Service SHALL 实现城市 Buff 系统（Role_AddBuff），支持各类增益效果的添加和过期管理
6. THE Game_Service SHALL 实现驿站商店系统（MysteryStore），支持定时刷新和购买

### 需求 20：任务系统模块

**用户故事：** 作为玩家，我希望能完成各类任务获取奖励，以便引导游戏进程。

#### 验收标准

1. THE Game_Service SHALL 实现主线任务、支线任务、每日任务、章节任务四种任务类型
2. THE Game_Service SHALL 实现任务进度统计（TaskStatistics），根据玩家行为自动更新任务进度
3. WHEN 任务完成条件满足时，THE Game_Service SHALL 推送任务完成通知给客户端
4. THE Game_Service SHALL 实现任务奖励发放，通过邮件或直接发放奖励

### 需求 21：活动系统模块

**用户故事：** 作为玩家，我希望能参与各类限时活动，以便获取额外奖励。

#### 验收标准

1. THE Game_Service SHALL 实现活动时间管理，根据配置表控制活动的开启和关闭
2. THE Game_Service SHALL 实现多种活动类型：积分活动、排名活动、兑换活动、转盘活动等
3. THE Game_Service SHALL 实现活动奖励领取（Activity_Reward）和排行榜查询（Activity_Rank）
4. THE Game_Service SHALL 实现活动进度追踪（Activity_ScheduleInfo），根据玩家行为更新活动进度

### 需求 22：充值与商城模块

**用户故事：** 作为玩家，我希望能购买游戏内商品，以便加速游戏进程。

#### 验收标准

1. THE Game_Service SHALL 实现充值回调处理（Recharge_RechargeInfo），验证充值订单并发放对应商品
2. THE Game_Service SHALL 实现多种商城类型：每日特惠、限时礼包、VIP 商店、崛起之路等
3. THE Game_Service SHALL 实现 VIP 系统，根据 VIP 经验计算 VIP 等级和对应特权
4. THE Game_Service SHALL 实现成长基金、首充奖励等一次性购买商品

### 需求 23：排行榜系统模块

**用户故事：** 作为玩家，我希望能查看各类排行榜，以便了解自己在服务器中的排名。

#### 验收标准

1. THE Game_Service SHALL 实现多种排行榜：个人战力、个人击杀、联盟战力、联盟击杀、资源采集等
2. THE Game_Service SHALL 使用有序集合（Sorted Set）数据结构维护排行榜，支持高效的排名查询和更新
3. WHEN 玩家相关数值变化时，THE Game_Service SHALL 异步更新对应排行榜

### 需求 24：数据持久化模块（总体架构）

**用户故事：** 作为开发者，我希望服务器能可靠地存储和加载游戏数据，以便保障玩家数据安全。

#### 验收标准

1. THE Go_Server SHALL 使用 MySQL 作为主要持久化存储，存储账号信息、角色数据、联盟数据、地图对象等
2. THE Go_Server SHALL 使用 Redis 作为缓存和排行榜存储，支持多个 Redis 数据库实例（db_server 使用 db0、game_server 使用 db2、login_server 使用 db4）
3. THE Go_Server SHALL 实现数据库连接池管理，MySQL 连接池大小至少为 10（与 Lua_Server 的 dbagentnum=10 一致）
4. THE Go_Server SHALL 使用 Sproto 格式序列化角色数据存入数据库（与 Lua_Server 的 Db.sproto 定义兼容），数据以 Base64 编码后存储在 MySQL 的 value 列中
5. THE Go_Server SHALL 实现角色数据的按需加载和定时保存机制
6. IF 数据库操作失败，THEN THE Go_Server SHALL 记录错误日志并进行重试，重试 3 次后报告错误
7. THE Go_Server SHALL 实现三层数据访问架构：内存缓存层（进程内 map）→ Redis 缓存层 → MySQL 持久化层，与 Lua_Server 的 EntityImpl 架构一致
8. THE Go_Server SHALL 支持 MySQL 和 MongoDB 两种数据库后端的抽象接口（通过配置 dbtype 切换），当前实现 MySQL 后端

### 需求 31：MySQL 数据库表结构定义

**用户故事：** 作为开发者，我希望 Go 服务器的数据库表结构与 Lua 服务器完全兼容，以便支持数据迁移和共用数据库。

#### 验收标准

1. THE Go_Server SHALL 创建以下 Common 类型数据表（全局共享数据），每张表采用 key-value 结构（主键列 + value 列 + json 列）：
   - `c_pkid`：主键 ID 分配表（key=id, value=Sproto 编码数据）
   - `c_account`：玩家账号信息表（key=iggid(string), value=Sproto 编码数据），存储 iggid、accessToken、gameNode、uid、ban、silence 等字段
   - `c_recommend`：推荐服务器信息表（key=serverNode）
   - `c_map_object`：地图对象表（key=id），存储资源点、野蛮人、玩家城市等地图对象
   - `c_refresh`：刷新信息表（key=id），存储野蛮人和资源点的下次刷新时间
   - `c_activity`：活动时间表（key=id），存储活动开始/结束时间
   - `c_guild`：联盟信息表（key=guildId），存储联盟完整数据
   - `c_guild_name`：联盟名称唯一性表（key=name）
   - `c_guild_abbname`：联盟简称唯一性表（key=abbreviationName）
   - `c_role_name`：角色名称唯一性表（key=name），存储 name、gameNode、rid
   - `c_guild_building`：联盟建筑表（key=guildId, value=Sproto 编码数据，子属性 buildInfo，子索引 buildIndex）
   - `c_email_content`：邮件内容表（key=emailIndex），存储邮件详细内容和附件
   - `c_chat`：聊天信息表（key=channelType），存储各频道聊天记录
   - `c_chat_guild`：联盟聊天信息表（key=guildId）
   - `c_monument`：纪念碑信息表（key=id）
   - `c_expeditionShop`：远征商店信息表（key=id）
   - `c_hallActivity`：地狱活动信息表（key=activityId）
   - `c_recharge`：充值信息表（key=id）
   - `c_system`：系统配置表（key=id）
   - `c_systemmail`：系统邮件表（key=id, nojson=true）
   - `c_king`：国王信息表（key=id）
   - `c_holy_land`：圣地信息表（key=id）
2. THE Go_Server SHALL 创建以下排行榜 MySQL 持久化表（均为 key-value 结构，key=rid 或 guildId，value=Sproto 编码的 score+lastScore）：
   - `c_role_power`：个人战力排行榜
   - `c_townhall`：市政厅等级排行榜
   - `c_role_kill`：个人击杀排行榜
   - `c_role_collect_res`：个人采集排行榜
   - `c_combat_first`：战力至上排行榜
   - `c_rise_up`：拔地而起排行榜
   - `c_reserve`：战略储备排行榜
   - `c_kill_type`：最强执政官排行榜
   - `c_kill_type_history`：历届执政官历史信息
   - `c_expedition`：远征排行榜
   - `c_tribe_king`：部落之王排行榜
   - `c_fight_horn`：战争号角个人排行榜
   - `c_fight_horn_alliance`：战争号角联盟排行榜
   - `c_alliance_power`：联盟战力排行榜
   - `c_alliance_kill`：联盟击杀排行榜
   - `c_alliance_flag`：联盟旗帜排行榜
   - `c_hell_activity_rank`：地狱活动排行榜
3. THE Go_Server SHALL 创建以下联盟子排行榜表（key=guildId，value=Sproto 编码数据，子属性按 rid 索引）：
   - `c_guild_role_power`：联盟成员战力排行
   - `c_guild_role_kill`：联盟成员击杀排行
   - `c_guild_role_donate`：联盟成员捐献排行
   - `c_guild_role_build`：联盟成员建造排行
   - `c_guild_role_help`：联盟成员帮助排行
   - `c_guild_resource_help`：联盟成员资源帮助排行
   - `c_guild_shop`：联盟商店信息
   - `c_guild_message_board`：联盟留言板（子属性 messageInfo，子索引 messageIndex）
   - `c_guild_gift`：联盟礼物（子属性 giftInfo，子索引 giftIndex）
4. THE Go_Server SHALL 创建以下 Role 类型数据表（按角色 rid 分片的玩家数据），每张表采用 key(rid)-value 结构：
   - `d_role`：角色主属性表（key=rid, alljson=true），存储角色全部属性（超过 140 个字段），包括资源、科技、队列、活动、VIP、充值等
   - `d_user`：用户角色映射表（key=uid），存储 uid 到角色列表的映射（noLoad=true，不随角色加载）
   - `d_building`：角色建筑表（key=rid，子属性 buildInfo，子索引 buildingIndex）
   - `d_item`：角色道具表（key=rid，子属性 itemInfo，子索引 itemIndex）
   - `d_email`：角色邮件表（key=rid，子属性 emailInfo，子索引 emailIndex）
   - `d_hero`：角色英雄表（key=rid，子属性 heroInfo，子索引 heroId）
   - `d_army`：角色军队表（key=rid，子属性 armyInfo，子索引 armyIndex）
   - `d_scouts`：角色斥候表（key=rid，子属性 scoutsInfo，子索引 scoutsIndex）
   - `d_task`：角色任务表（key=rid，子属性 taskInfo，子索引 taskId）
   - `d_transport`：角色运输车表（key=rid，子属性 transportInfo，子索引 transportIndex）
   - `d_chat`：角色私聊表（key=rid，子属性 privateChatInfo，子索引 rid）
5. THE Go_Server SHALL 为所有数据表统一使用三列结构：主键列（类型根据 key 为 VARCHAR(255) 或 BIGINT）、`value` 列（LONGTEXT，存储 Base64 编码的 Sproto 序列化数据）、`json` 列（LONGTEXT，存储 JSON 格式的可读数据，debug 模式下写入）
6. THE Go_Server SHALL 在 MySQL 连接建立时执行 `SET CHARSET utf8` 和 `SET sql_mode = 'STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION'`
7. THE Go_Server SHALL 设置 MySQL 最大包大小为 64MB（max_packet_size = 1024 * 1024 * 64），以支持大型 Sproto 序列化数据

### 需求 32：数据库初始化与迁移

**用户故事：** 作为运维人员，我希望数据库能自动初始化和迁移，以便快速部署新服务器。

#### 验收标准

1. THE Go_Server SHALL 提供 SQL 初始化脚本（位于 `go_server/deployments/sql/init.sql`），包含所有数据表的 CREATE TABLE IF NOT EXISTS 语句
2. WHEN 服务器首次启动时，THE Go_Server SHALL 自动检测数据库表是否存在，不存在则自动创建
3. THE Go_Server SHALL 在初始化时为 c_pkid 表写入默认的主键 ID 种子值，种子值按 serverId 计算：pkid = serverId * 10000000，petId = serverId * 1000000000000，itemId = serverId * 1000000000000，systemMailId = serverId * 1000000000000，chatUniqueIndexId = serverId * 100000000000000
4. THE Go_Server SHALL 提供数据库版本管理机制，在 `go_server/deployments/sql/migrations/` 目录下按版本号存放增量迁移脚本
5. THE Go_Server SHALL 在启动时自动执行未应用的迁移脚本，并记录已应用的迁移版本
6. THE Docker_Compose SHALL 在 MySQL 容器首次启动时自动执行 init.sql 初始化脚本（通过 docker-entrypoint-initdb.d 机制）

### 需求 33：数据序列化与反序列化（Db.sproto 兼容）

**用户故事：** 作为开发者，我希望 Go 服务器的数据序列化格式与 Lua 服务器完全兼容，以便支持数据库共用和平滑迁移。

#### 验收标准

1. THE Go_Server SHALL 实现 Db.sproto 中定义的所有数据结构的 Sproto 编解码，包括但不限于：c_pkid、c_account、c_recommend、d_role（超过 140 个字段）、d_building、d_item、d_hero、d_email、d_army、d_scouts、d_task、d_transport、d_chat、c_guild（超过 45 个字段）、c_map_object、c_email_content、c_guild_building 等
2. THE Go_Server SHALL 实现 Common.sproto 中定义的所有共享数据结构的 Sproto 编解码，包括：PosInfo、SoldierInfo、CollectSpeedInfo、MarchTargetArg、QueueInfo、ArmyInfo、BuildingInfo、ItemInfo、HeroInfo、EmailInfo、TransportInfo、BattleReportEx 等
3. THE Go_Server SHALL 实现 Sproto 数据的 Base64 编码和解码，与 Lua_Server 的 crypt.base64encode/base64decode 完全兼容
4. THE Go_Server SHALL 实现 Sproto pencode（打包编码）和 pdecode（打包解码）操作，与 Lua_Server 的 sprotoloader 兼容
5. THE Go_Server SHALL 在 debug 模式下将数据同时以 JSON 格式写入 json 列（d_role 表始终写入 JSON，即 alljson=true；c_systemmail 表不写入 JSON，即 nojson=true）
6. FOR ALL 有效的 Db.sproto 数据结构，Sproto 编码后再解码 SHALL 产生与原始数据等价的对象（往返属性）
7. THE Go_Server SHALL 正确处理 Sproto 中的嵌套结构体、数组类型（*Type）、带索引的 map 类型（*Type(key)）和内嵌类型定义（如 QueueInfo.Items、Activity.ScheduleInfo.DataInfo）

### 需求 34：Entity 数据管理框架

**用户故事：** 作为开发者，我希望 Go 服务器有统一的数据管理框架，以便规范化数据的增删改查操作。

#### 验收标准

1. THE Go_Server SHALL 实现 Entity 数据管理框架，支持四种数据类型分类：CONFIG（配置数据，只读）、COMMON（全局共享数据）、USER（用户数据）、ROLE（角色数据）
2. THE Go_Server SHALL 为 COMMON 类型数据实现 SingleEntity 模式：启动时全量加载到内存，支持 Add、Delete、Update、Get、Set 操作，变更时同步写入 MySQL
3. THE Go_Server SHALL 为 ROLE 类型数据实现按需加载模式：角色登录时从 MySQL 加载到内存和 Redis，角色登出时从内存卸载
4. THE Go_Server SHALL 为 ROLE 类型数据实现 MultiEntity 模式（用于 d_building、d_item、d_email、d_hero、d_army、d_scouts、d_task、d_transport、d_chat），支持按子索引的增删改查
5. THE Go_Server SHALL 实现数据加载流程：先查 Redis 缓存（HGET），缓存未命中则查 MySQL，查到后写入 Redis 缓存（HSET）
6. THE Go_Server SHALL 实现数据更新流程：更新内存 → 删除旧 Redis 缓存 → 写入新 Redis 缓存 → 更新 MySQL
7. THE Go_Server SHALL 实现数据删除流程：删除 MySQL 记录 → 删除 Redis 缓存 → 删除内存记录
8. THE Go_Server SHALL 实现 EntityLoad 模块，提供 loadRole（加载角色全部数据表）、saveRole（保存角色全部数据表）、unLoadRole（卸载角色数据）、deleteRole（删除角色全部数据）的批量操作
9. THE Go_Server SHALL 支持 d_user 表的 noLoad 标记，该表不随角色登录自动加载，仅在需要时按需查询

### 需求 35：主键 ID 生成机制

**用户故事：** 作为开发者，我希望服务器能高效生成全局唯一的主键 ID，以便避免数据冲突。

#### 验收标准

1. THE Go_Server SHALL 实现基于 Redis INCR 命令的原子性主键 ID 生成器（PkIdMgr），与 Lua_Server 的 PkIdMgr 兼容
2. THE Go_Server SHALL 管理以下主键 ID 序列：pkidkey（玩家 UID，种子=serverId*10000000）、petkey（宠物 ID）、itemkey（道具唯一索引）、systemmailkey（系统邮件 ID）、chatUniqueIndexkey（聊天消息唯一索引）
3. THE Go_Server SHALL 在启动时从 c_pkid 表加载已有的主键 ID 值到 Redis，若不存在则使用默认种子值初始化
4. THE Go_Server SHALL 每 5 秒将 Redis 中的当前主键 ID 值持久化到 c_pkid 表（防止 Redis 重启丢失）
5. THE Go_Server SHALL 在启动时扫描所有数据表，将每张表的 MAX(key) 值写入 Redis（格式：`表名:主键列名`），用于 Entity 框架的 newId 操作

### 需求 36：Redis 缓存策略与数据结构

**用户故事：** 作为开发者，我希望 Redis 缓存策略与 Lua 服务器一致，以便保证数据一致性和查询性能。

#### 验收标准

1. THE Go_Server SHALL 使用 Redis Hash 结构缓存 Entity 数据：key 为表名（如 d_role、d_building），field 为主键值（如 rid），value 为 Base64 编码的 Sproto 序列化数据
2. THE Go_Server SHALL 使用 Redis String 结构存储主键 ID 序列：key 为序列名（如 pkidkey、petkey），value 为当前 ID 值
3. THE Go_Server SHALL 使用 Redis String 结构存储表最大 ID：key 格式为 `表名:主键列名`（如 `d_role:rid`），value 为当前最大 ID
4. THE Go_Server SHALL 使用 Redis Sorted Set 结构存储排行榜数据：key 为排行榜名称（如 role_power、alliance_power），member 为 rid 或 guildId，score 为排名分数
5. THE Go_Server SHALL 为每个排行榜维护一个 copy 副本（key 格式：`排行榜名_copy`），用于记录上次排名变化
6. THE Go_Server SHALL 使用 Redis String 结构存储游服角色数量：key 格式为 `gameRoleCount_游服名`
7. THE Go_Server SHALL 支持 Redis Pipeline 批量操作，减少网络往返次数
8. THE Go_Server SHALL 在 db_server 启动时执行 FLUSHDB 清空 Redis 缓存，然后从 MySQL 重新加载数据到 Redis
9. THE Go_Server SHALL 支持配置多个 Redis 实例（通过 redisnum 配置），不同实例使用不同端口（基础端口 + 实例索引 - 1）

### 需求 37：排行榜数据库操作

**用户故事：** 作为开发者，我希望排行榜系统能高效地进行排名查询和更新，以便支持大量玩家的实时排名。

#### 验收标准

1. THE Go_Server SHALL 实现排行榜的 Redis + MySQL 双写机制：排名数据实时写入 Redis Sorted Set，同时异步持久化到对应的 MySQL 排行榜表
2. THE Go_Server SHALL 实现以下排行榜 Redis 操作：ZADD（更新分数）、ZREVRANGE（查询排名，从高到低）、ZRANK/ZREVRANK（查询单个成员排名）、ZREM（删除成员）、ZCARD（获取总数）、ZSCORE（查询分数）
3. THE Go_Server SHALL 在服务器启动时从 MySQL 排行榜表加载数据到 Redis Sorted Set（通过 RankMgr.Init）
4. THE Go_Server SHALL 实现排行榜记录上限管理：根据 s_Leaderboard 配置表的 recordLimit 字段，超出上限的记录自动从 Redis 和 MySQL 中删除
5. THE Go_Server SHALL 支持联盟子排行榜，key 格式为 `排行榜类型_联盟ID`（如 `guild_role_power_12345`）
6. THE Go_Server SHALL 实现最强执政官排行榜的特殊格式：member 格式为 `rid_类型`（如 `10000001_1`），支持按类型分别排名
7. WHEN 玩家相关数值变化时，THE Go_Server SHALL 通过 RankMgr 异步更新对应排行榜，记录 lastScore（上次排名）用于排名变化展示

### 需求 38：数据库事务与一致性保障

**用户故事：** 作为开发者，我希望关键数据操作具有事务保障，以便防止数据不一致。

#### 验收标准

1. THE Go_Server SHALL 实现角色创建的原子性操作：创建 d_role、d_building、d_item、d_hero、d_army 等多张表的记录在同一事务中完成，任一失败则全部回滚
2. THE Go_Server SHALL 实现角色删除的原子性操作：删除角色所有相关表数据在同一事务中完成
3. THE Go_Server SHALL 实现联盟创建/解散的原子性操作：联盟信息表、联盟名称表、联盟简称表的操作在同一事务中完成
4. THE Go_Server SHALL 实现 Redis 和 MySQL 的最终一致性：先更新 Redis 缓存，再写入 MySQL；若 MySQL 写入失败，记录错误日志并加入重试队列
5. THE Go_Server SHALL 实现角色数据定时保存机制：在线角色数据每 5 分钟自动保存一次到 MySQL
6. THE Go_Server SHALL 实现优雅关闭时的数据保存：收到关闭信号后，遍历所有在线角色执行 saveRole 操作，确保数据不丢失
7. THE Go_Server SHALL 实现 CommonSingleEntity 的 Update 操作支持 lockFlag 原子递增模式：当 lockFlag=true 时，对指定字段执行原子加操作而非覆盖操作

### 需求 39：Login 数据库独立连接

**用户故事：** 作为开发者，我希望登录服务器有独立的数据库连接，以便登录认证不受游戏服务器数据库负载影响。

#### 验收标准

1. THE Login_Service SHALL 使用独立的 MySQL 连接配置（loginmysqlip、loginmysqlport、loginmysqldb、loginmysqluser、loginmysqlpwd），与 Game_Service 的数据库连接分离
2. THE Login_Service SHALL 使用独立的 Redis 数据库实例（db4），与 db_server（db0）和 game_server（db2）隔离
3. THE Login_Service SHALL 直接访问 c_account 表进行账号验证，无需通过 db_server 中转
4. THE Login_Service SHALL 访问 d_user 表查询用户角色列表，支持跨服角色查询
5. THE Login_Service SHALL 实现 MySQL 连接断线自动重连机制，与 Lua_Server 的 MysqlAgent 重试逻辑一致

### 需求 40：数据库性能优化

**用户故事：** 作为开发者，我希望数据库操作具有良好的性能，以便支持大量玩家同时在线。

#### 验收标准

1. THE Go_Server SHALL 对所有数据表的主键列建立主键索引（PRIMARY KEY）
2. THE Go_Server SHALL 对 c_account 表的 iggid 列建立唯一索引
3. THE Go_Server SHALL 对 c_role_name 表的 name 列建立唯一索引
4. THE Go_Server SHALL 对 c_guild_name 表的 name 列建立唯一索引
5. THE Go_Server SHALL 对 c_guild_abbname 表的 abbreviationName 列建立唯一索引
6. THE Go_Server SHALL 实现 MySQL 批量加载优化：Common 类型数据使用 `SELECT * FROM table LIMIT offset, 2000` 分页加载，避免一次性加载过大结果集
7. THE Go_Server SHALL 实现 Redis Pipeline 批量操作：排行榜初始化时使用 Pipeline 批量 ZADD，减少网络往返
8. THE Go_Server SHALL 实现 Sproto 编解码的内存池优化：复用编解码缓冲区，减少 GC 压力
9. THE Go_Server SHALL 实现数据库操作的 goroutine 池：限制并发数据库操作数量，防止连接池耗尽
10. THE Go_Server SHALL 对角色数据实现脏标记机制：仅在数据发生变化时才执行 MySQL 写入操作，避免无效写入

### 需求 41：数据库备份与恢复

**用户故事：** 作为运维人员，我希望能定期备份数据库并在需要时恢复，以便保障数据安全。

#### 验收标准

1. THE Docker_Compose SHALL 配置 MySQL 数据卷持久化，确保容器重启不丢失数据
2. THE Go_Server SHALL 提供数据库备份脚本（位于 `go_server/scripts/backup.sh`），支持 mysqldump 全量备份
3. THE Go_Server SHALL 提供数据库恢复脚本（位于 `go_server/scripts/restore.sh`），支持从备份文件恢复
4. THE Go_Server SHALL 在备份脚本中支持配置备份保留天数，自动清理过期备份文件
5. THE Docker_Compose SHALL 配置 Redis 持久化策略（RDB + AOF），确保 Redis 重启后排行榜数据可恢复
6. IF Redis 重启后数据丢失，THEN THE Go_Server SHALL 在启动时自动从 MySQL 重新加载排行榜数据到 Redis（通过 RankMgr.Init 流程）

### 需求 25：配置表加载模块

**用户故事：** 作为开发者，我希望 Go 服务器能加载策划配置表，以便驱动游戏逻辑。

#### 验收标准

1. THE Config_Loader SHALL 加载 Lua_Server 中 `common/config/gen/` 目录下的所有配置数据文件
2. THE Config_Loader SHALL 支持在服务器启动时一次性加载所有配置表到内存
3. THE Config_Loader SHALL 提供按 ID 查询单条记录和遍历全表的 API
4. THE Config_Loader SHALL 支持热重载配置表，无需重启服务器即可更新配置

### 需求 26：日志系统模块

**用户故事：** 作为运维人员，我希望服务器有完善的日志系统，以便排查问题和监控运行状态。

#### 验收标准

1. THE Go_Server SHALL 实现分级日志系统，支持 DEBUG、INFO、WARNING、ERROR 四个级别
2. THE Go_Server SHALL 将日志输出到文件，按日期自动轮转
3. THE Go_Server SHALL 在日志中包含时间戳、日志级别、源文件位置、goroutine ID 等上下文信息
4. THE Go_Server SHALL 实现结构化日志，支持 JSON 格式输出（用于日志收集系统）

### 需求 27：远征系统模块

**用户故事：** 作为玩家，我希望能参与远征玩法，以便获取远征币和通关奖励。

#### 验收标准

1. THE Game_Service SHALL 实现远征地图管理，支持远征关卡的进入和通关
2. THE Game_Service SHALL 实现远征商店系统，支持使用远征币购买道具
3. THE Game_Service SHALL 实现远征 PVE 战斗，使用独立的 NavMesh 地图进行寻路

### 需求 28：推送通知模块

**用户故事：** 作为玩家，我希望在离线时能收到重要游戏事件的推送通知。

#### 验收标准

1. THE Go_Server SHALL 实现推送消息管理，根据玩家的推送设置（Role_SettingPush）决定是否发送推送
2. THE Go_Server SHALL 实现推送消息队列，在玩家离线时将待推送消息存入队列
3. THE Go_Server SHALL 支持多种推送事件类型：建造完成、训练完成、被攻击、邮件等

### 需求 29：监控与管理模块

**用户故事：** 作为运维人员，我希望能监控服务器运行状态和执行管理操作。

#### 验收标准

1. THE Go_Server SHALL 提供 HTTP API 接口，用于查询服务器状态（在线人数、内存使用、goroutine 数量等）
2. THE Go_Server SHALL 提供 Web 管理接口，支持服务器列表查询（供客户端 LoginMediator 调用的 `/api/lists.php` 接口）
3. THE Go_Server SHALL 提供账号登录验证接口（供客户端 LoginMediator 调用的 `/api/login.php` 接口）
4. THE Go_Server SHALL 实现优雅关闭机制，收到关闭信号后先保存所有在线玩家数据，再关闭服务

### 需求 30：HTML 服务器文档

**用户故事：** 作为开发者，我希望有完善的服务器文档，以便快速理解和维护服务器代码。

#### 验收标准

1. THE Go_Server SHALL 在 `go_server/docs/` 目录下提供 HTML 格式的服务器文档
2. THE 文档 SHALL 包含以下章节：架构概览、服务节点说明、启动流程、配置说明、协议说明、编码规范、部署指南、API 文档
3. THE 架构概览文档 SHALL 包含服务器整体架构图、各服务节点的职责和通信关系
4. THE 启动流程文档 SHALL 详细描述从 `docker-compose up` 到服务器就绪的完整启动流程
5. THE 编码规范文档 SHALL 包含 Go 语言编码规范、项目命名约定、错误处理规范、日志规范
6. THE 协议说明文档 SHALL 包含 Sproto 协议格式说明、握手流程图、消息收发流程图
7. THE 部署指南文档 SHALL 包含 Docker 部署步骤、环境变量配置说明、数据库初始化步骤
8. THE 文档 SHALL 使用响应式 HTML 布局，支持在浏览器中直接查看，包含导航目录和代码高亮

### 需求 42：NavMesh 寻路与 AStar 路径计算模块

**用户故事：** 作为开发者，我希望 Go 服务器能实现与 Lua 服务器一致的寻路系统，以便军队行军和地图对象移动使用正确的路径。

#### 验收标准

1. THE Go_Server SHALL 实现 NavMesh 导航网格加载器，读取 `common/mapmesh/` 目录下的 `.bin` 格式导航网格文件（map_4_Building_NavMesh.bin、map_4_Walkable_NavMesh.bin）
2. THE Go_Server SHALL 实现 NavMesh 寻路查询接口，支持给定起点和终点计算可行走路径
3. THE Go_Server SHALL 实现 NavMesh 障碍物管理（NavMeshObstracleMgr），支持动态添加和删除障碍物（城市、联盟建筑等占位）
4. THE Go_Server SHALL 实现 AStar 路径计算（AStarMgr），用于大世界地图上的军队行军路径规划
5. THE Go_Server SHALL 实现 CheckPoint AStar 寻路（CheckPointAStarMgr），用于关卡地图（远征等）的路径计算
6. THE Go_Server SHALL 实现 PVE NavMesh 地图管理（PVENavMeshMapMgr），支持远征等 PVE 玩法的独立导航网格
7. THE Go_Server SHALL 实现位置空闲检测（checkPosIdle），验证指定坐标和半径范围内是否有其他地图对象占用

### 需求 43：纪念碑（文明进化）系统模块

**用户故事：** 作为玩家，我希望能参与纪念碑事件推动服务器文明进化，以便解锁新内容和获取奖励。

#### 验收标准

1. THE Game_Service SHALL 实现纪念碑事件管理（MonumentMgr），维护当前服务器的纪念碑阶段（step）和进度
2. THE Game_Service SHALL 实现纪念碑进度统计（MonumentLogic:setSchedule），支持多种进度类型：迷雾探索、击杀野蛮人、联盟成员数、联盟旗帜数、圣所占领、联盟击杀城寨、联盟战力、城市等级等
3. WHEN 纪念碑事件进度达到目标值时，THE Game_Service SHALL 自动结束当前阶段并开启下一阶段（monumentEnd）
4. THE Game_Service SHALL 实现纪念碑奖励领取（getMonumentReward），支持个人奖励、联盟奖励、联盟排名奖励三种发放方式
5. THE Game_Service SHALL 实现纪念碑数据修正机制（fixData），根据 s_EvolutionGoalFillData 配置定时补正进度
6. WHEN 纪念碑事件结束时，THE Game_Service SHALL 推送 Monument_End 通知给所有在线玩家，并触发圣地解锁和迷雾全开等关联逻辑
7. THE Game_Service SHALL 在角色登录时推送纪念碑信息（pushMonument），包含各阶段进度、奖励状态、联盟排名

### 需求 44：圣地系统模块

**用户故事：** 作为玩家，我希望联盟能争夺和占领圣地，以便获取圣地加成和领地收益。

#### 验收标准

1. THE Game_Service SHALL 实现圣地数据管理（HolyLandMgr），存储圣地占领状态、占领联盟、增援部队等信息（c_holy_land 表）
2. THE Game_Service SHALL 实现圣地类型分类：圣所（Sanctuary）、圣坛（Altar）、圣祠（Holy Shrine）、神庙（Temple）、关卡（Checkpoint Level 1-3）
3. THE Game_Service SHALL 实现圣地增援系统（reinforceHolyLand），支持联盟成员向圣地派遣部队驻守
4. THE Game_Service SHALL 实现圣地驻守部队管理（SceneHolyLandMgr），包括驻守队长选举、部队进出、容量上限检查
5. WHEN 纪念碑事件解锁圣地时，THE Game_Service SHALL 更新圣地状态并为所有在线玩家开启圣地附近迷雾
6. THE Game_Service SHALL 实现圣地守护者系统（HolyLandGuardMgr），根据 guardianBornTime 配置定时刷新守护者 NPC
7. THE Game_Service SHALL 实现联盟圣地管理（GuildHolyLandMgr），追踪每个联盟占领的圣地列表，检查同类型圣地重复占领
8. WHEN 圣地被攻破时，THE Game_Service SHALL 遣返所有驻守部队并更新圣地占领状态

### 需求 45：野蛮人城寨系统模块

**用户故事：** 作为玩家，我希望能在大世界地图上攻击野蛮人城寨，以便获取高级奖励和推动纪念碑进度。

#### 验收标准

1. THE Game_Service SHALL 实现野蛮人城寨管理（MonsterCityMgr），按瓦片区域管理城寨的刷新、存活和删除
2. THE Game_Service SHALL 实现野蛮人城寨刷新逻辑（monsterCityRefresh），根据纪念碑进度决定最大城寨等级，按区域等级和配置数量刷新
3. THE Game_Service SHALL 实现野蛮人城寨超时删除机制（monsterCityTimeOut），城寨存活时间由 s_Monster 配置的 showTime 决定
4. WHEN 野蛮人城寨被击败时，THE Game_Service SHALL 删除城寨地图对象并更新纪念碑进度
5. THE Game_Service SHALL 实现城寨刷新定时器（addMonsterCityRefreshTimer），根据 fortressFreshTimeGap 配置周期性刷新
6. THE Game_Service SHALL 在服务器启动时初始化城寨服务瓦片索引，并执行首次刷新

### 需求 46：城市隐藏与回收系统模块

**用户故事：** 作为运维人员，我希望长期离线玩家的城市能自动从地图上隐藏，以便释放地图空间给活跃玩家。

#### 验收标准

1. THE Game_Service SHALL 实现城市隐藏管理（CityHideMgr），追踪离线玩家的城市信息和离线时间
2. THE Game_Service SHALL 实现城市隐藏检查逻辑（checkCityHide），根据 s_CityHideData 配置的 hideCityTime 判断是否需要隐藏城市
3. WHEN 城市被隐藏时，THE Game_Service SHALL 执行以下操作：解散所有部队、召回斥候、召回运输队、退出战斗、城市离开地图、通知联盟成员
4. WHEN 角色等级低于 hideCityExitAlliance 配置值时，THE Game_Service SHALL 在隐藏城市前自动将角色退出联盟（盟主除外）
5. WHEN 被隐藏城市的玩家重新登录时，THE Game_Service SHALL 在原省份随机分配空闲位置重新放置城市，开启附近迷雾
6. THE Game_Service SHALL 在角色登出时将角色添加到城市隐藏检查队列，新手引导未完成的角色立即隐藏城市

### 需求 47：城市增援遣返系统模块

**用户故事：** 作为玩家，我希望能遣返驻守在我城市中的联盟成员部队，以便管理城市防御。

#### 验收标准

1. THE Game_Service SHALL 实现城市增援遣返逻辑（RepatriationLogic），支持从城市遣返指定联盟成员的增援部队
2. WHEN 遣返增援部队时，THE Game_Service SHALL 验证部队是否已到达，未到达的部队返回错误码 RALLY_REPATRIATION_REINFORCE_FAIL
3. THE Game_Service SHALL 实现遣返操作的互斥锁（tryLock/unLock），防止同一城市的并发遣返操作导致数据竞争
4. WHEN 城市正在战斗中遣返部队时，THE Game_Service SHALL 通知战斗服务器扣除对应士兵并退出战斗
5. THE Game_Service SHALL 在遣返完成后更新城市增援列表（reinforces）并同步给客户端

### 需求 48：符文系统模块

**用户故事：** 作为玩家，我希望能在大世界地图上拾取符文，以便获取临时增益效果。

#### 验收标准

1. THE Game_Service SHALL 实现符文管理（RuneMgr），按瓦片区域管理符文的刷新和拾取
2. THE Game_Service SHALL 实现符文场景管理（SceneRuneMgr），管理符文在地图上的显示和交互
3. THE Game_Service SHALL 实现符文刷新机制，在守护者刷新时同步清空并重新生成符文
4. WHEN 玩家拾取符文时，THE Game_Service SHALL 将符文效果应用到玩家角色并从地图上移除符文对象

### 需求 49：召唤怪物系统模块

**用户故事：** 作为玩家，我希望能在大世界地图上遇到召唤怪物，以便获取特殊战斗体验和奖励。

#### 验收标准

1. THE Game_Service SHALL 实现召唤怪物管理（MonsterSummonMgr），支持通过道具或事件在地图上召唤特殊怪物
2. THE Game_Service SHALL 实现怪物追击逻辑（MonsterFollowUpLogic），支持怪物主动追击玩家部队
3. THE Game_Service SHALL 实现怪物巡逻逻辑（MonsterPartolLogic），支持怪物在指定区域内巡逻移动
4. THE Game_Service SHALL 实现 NPC 脚本系统（NpcScript），支持通过配置驱动怪物的行为模式

### 需求 50：跨天/跨周/跨月定时重置系统

**用户故事：** 作为开发者，我希望服务器能在跨天、跨周、跨月时自动重置相关数据，以便支持每日任务、每周商店等周期性玩法。

#### 验收标准

1. THE Game_Service SHALL 实现角色跨天逻辑（RoleTimerLogic:crossDay），在每日零点或角色登录时执行以下重置：每日角色属性、每日任务、白银宝箱免费次数、活动信息、神秘商人、VIP 每日奖励、活动开启检查、活动进度重置、礼包购买记录、远征商店、远征信息
2. THE Game_Service SHALL 实现角色跨周逻辑（crossWeek），重置每周充值礼包购买记录和 VIP 商店
3. THE Game_Service SHALL 实现角色跨月逻辑（crossMonth），重置每月充值礼包购买记录
4. THE Game_Service SHALL 实现角色定时器框架（RoleTimerLogic），支持按角色维度的定时器管理，包括一次性定时器、循环定时器、跨天定时器
5. THE Game_Service SHALL 在角色登出时清理标记为 isLogoutDelete 的角色定时器
6. THE Game_Service SHALL 在在线跨天时发送联盟不活跃成员邮件通知

### 需求 51：角色移民系统模块

**用户故事：** 作为玩家，我希望能将角色迁移到其他服务器，以便与朋友一起游戏。

#### 验收标准

1. THE Game_Service SHALL 实现角色移民管理（RoleImmigrateMgr），支持角色从当前游服迁移到目标游服
2. WHEN 角色发起移民请求时，THE Game_Service SHALL 验证移民条件（等级、联盟状态、部队状态等）
3. THE Game_Service SHALL 实现移民数据迁移流程：保存角色数据到目标服务器数据库、删除源服务器角色数据、更新账号表的 gameNode 字段
4. THE Game_Service SHALL 在移民完成后将角色城市从源服务器地图上移除，并在目标服务器地图上重新放置

### 需求 52：GM/PM 命令系统模块

**用户故事：** 作为运维人员，我希望能通过 GM 命令修改游戏数据，以便进行测试和问题排查。

#### 验收标准

1. THE Go_Server SHALL 实现 PM 命令系统（PMLogic），仅在 debug 模式下启用
2. THE Go_Server SHALL 支持以下 GM 命令类别：角色属性修改（modifyAttr）、道具增加（addItem）、士兵增加（addSoldiers）、伤兵增加（addHosptial）、系统邮件发送（addSystemEmail）、英雄增加（addHero）、城市迁移（cityMove）、建筑升级（upGradeBuliding）、科技升级（researchTechnology）、部队解散（disbandArmy）、任务进度修改（addTaskStatisticsSum）、迷雾解锁（scoutDenseFog/scoutAllDenseFog）、Buff 添加/移除（addBuff/removeCityBuff）、联盟操作（disbandGuild/modifyGuildAttr）、充值模拟（buyDenar）、圣地占领（occupyHolyLand）、纪念碑操作（monumentEnd）、符文刷新（addRuneInfo）、跑马灯发送（sendMarquee）、移民（immigrate）等
3. THE Go_Server SHALL 通过 Web HTTP 接口（WebCmd.pmCmd）接收 GM 命令，返回 JSONP 格式结果
4. THE Go_Server SHALL 在 GM 命令执行后自动同步修改后的数据给客户端

### 需求 53：服务器热更新与配置重载模块

**用户故事：** 作为运维人员，我希望能在不停服的情况下更新服务器逻辑和配置，以便快速修复线上问题。

#### 验收标准

1. THE Go_Server SHALL 实现配置热重载机制（reloadConfig），通过 Monitor_Service 广播通知所有服务节点重新加载配置表
2. THE Go_Server SHALL 实现服务器热更新机制（hotfix），支持在运行时替换指定模块的逻辑代码
3. THE Go_Server SHALL 通过 Monitor_Service 的发布/订阅模式（MonitorPublish/MonitorSubscribe）实现集群级别的配置同步
4. THE Go_Server SHALL 支持通过 Web 接口触发热更新和配置重载操作

### 需求 54：服务器集群通信与节点管理模块

**用户故事：** 作为开发者，我希望各服务节点之间能可靠通信，以便实现分布式游戏服务架构。

#### 验收标准

1. THE Go_Server SHALL 实现服务节点间的 RPC 通信机制，替代 Lua_Server 的 Skynet Cluster RPC，支持 rpcCall（同步调用）和 rpcSend（异步发送）两种模式
2. THE Go_Server SHALL 实现 Center_Service 作为集群中心节点，负责跨服联盟查询、服务器列表管理、全局配置分发
3. THE Go_Server SHALL 实现服务节点发现机制，各节点通过配置文件（etc/*.conf）获取其他节点的地址和端口
4. THE Go_Server SHALL 实现多线路（MultiSnax）服务管理，支持同一服务类型启动多个实例（如多个地图瓦片服务），通过索引分配负载
5. THE Go_Server SHALL 实现跨服通信，支持 Game_Service 与 Chat_Service、Battle_Service、Log_Service、Center_Service 之间的 RPC 调用
6. THE Go_Server SHALL 实现集群关闭流程（restartCluster/closeCluster），通过 Monitor_Service 协调所有节点的有序关闭

### 需求 55：独立战斗服务（Battle_Service）

**用户故事：** 作为开发者，我希望战斗计算在独立服务中执行，以便不阻塞游戏主逻辑并支持水平扩展。

#### 验收标准

1. THE Battle_Service SHALL 作为独立进程运行，通过内部 RPC 与 Game_Service 通信
2. THE Battle_Service SHALL 实现战斗循环引擎（BattleLoop），管理多个并发战斗实例的生命周期
3. THE Battle_Service SHALL 实现战斗 Buff 系统（BattleBuff），支持增益、减益、持续伤害、持续治疗等 Buff 类型
4. THE Battle_Service SHALL 实现战斗技能系统（BattleSkill），根据英雄技能配置（s_SkillBattle、s_SkillStatus）执行技能效果
5. THE Battle_Service SHALL 实现战斗场景管理（BattleSceneMgr），维护战斗中的双方部队状态、回合数、战斗结果
6. THE Battle_Service SHALL 实现战斗状态管理（BattleStatus），支持战斗中的部队加入、退出、士兵扣减等动态操作
7. THE Battle_Service SHALL 实现战斗索引管理（BattleIndexMgr），为每场战斗分配唯一索引，支持通过索引查询战斗状态
8. THE Battle_Service SHALL 加载独立的战斗配置数据（BattleConfigData），包括兵种属性、技能效果、Buff 参数等

### 需求 56：运营日志服务（Log_Service）

**用户故事：** 作为运营人员，我希望服务器能记录详细的运营日志，以便进行数据分析和用户行为追踪。

#### 验收标准

1. THE Log_Service SHALL 作为独立进程运行，接收 Game_Service 通过 RPC 发送的日志数据
2. THE Log_Service SHALL 实现日志写入（LogImpl），支持将运营日志写入文件或数据库（根据 logtype 配置）
3. THE Go_Server SHALL 实现运营日志逻辑（LogLogic），记录以下事件类型：角色创建（roleCreate）、角色登录（roleLogin）、角色登出（roleLogout）、建筑创建（buildCreate）、士兵增减（armsChange）、新手引导（roleGuide）、任务完成（roleTask）、货币增减（currencyChange）、道具增减（itemChange）、部队行军（troopsMarch）、联盟操作（roleGuild）、圣地占领（holyLandOccupy）、充值（roleRecharge）、纪念碑进化（roleEvolution）、联盟建筑（guildBuild）
4. THE Go_Server SHALL 在每条运营日志中包含标准字段：时间戳、IGGID、服务器名称、开服时间
5. THE Go_Server SHALL 实现角色详细信息快照（getRoleLogRequireInfo），包含 40+ 个字段用于日志记录
6. THE Log_Service SHALL 实现日志定时器（LogTimer），支持定时汇总和上报服务器在线人数

### 需求 57：地图省份与区域管理模块

**用户故事：** 作为开发者，我希望大世界地图按省份和区域划分，以便实现区域化的资源刷新和城市分配。

#### 验收标准

1. THE Game_Service SHALL 实现地图省份逻辑（MapProvinceLogic），根据坐标计算所在省份（通过 s_MapZoneSF 配置表）
2. THE Game_Service SHALL 实现地图等级区域管理（MapLevelMgr），维护不同等级区域的野蛮人和资源点刷新规则
3. THE Game_Service SHALL 实现地图对象刷新管理（MapObjectRefreshMgr），按分组和瓦片区域周期性刷新野蛮人和资源点
4. THE Game_Service SHALL 实现地图城市数量管理（MapCityMgr），追踪当前地图上的城市总数，控制城市隐藏回收的触发条件
5. THE Game_Service SHALL 实现省份满员管理（modifyFullProvice），追踪已满和未满的省份，用于新城市放置时的省份选择

### 需求 58：城墙系统模块

**用户故事：** 作为玩家，我希望城墙能保护我的城市，城墙被攻击时会损耗耐久并可能燃烧。

#### 验收标准

1. THE Game_Service SHALL 实现城墙耐久管理，城墙作为建筑类型（WALL）存在于角色建筑列表中
2. WHEN 城墙耐久降为零时，THE Game_Service SHALL 触发城墙燃烧状态（startBurnWall），城墙燃烧期间城市防御力下降
3. THE Game_Service SHALL 实现城墙维修机制（resetWallTime），城墙燃烧结束后自动恢复耐久
4. THE Game_Service SHALL 在城墙耐久变化时推送 wallHpNotify 通知给客户端
5. THE Game_Service SHALL 实现城墙相关的战争狂热 Buff（warCrazy），被攻击后触发战争狂热状态

### 需求 59：装备系统模块

**用户故事：** 作为玩家，我希望能为英雄制作和穿戴装备，以便提升英雄战斗属性。

#### 验收标准

1. THE Game_Service SHALL 实现装备数据管理，装备作为特殊道具存储在道具背包中（subType 为 ARMS、HELMET、BREASTPLATE、GLOVES、PANTS、ACCESSORIES、SHOES）
2. THE Game_Service SHALL 实现装备属性计算，根据 s_Equip 和 s_EquipAtt 配置表计算装备提供的属性加成
3. THE Game_Service SHALL 实现装备合成系统，根据 s_EquipCompose 和 s_EquipMaterial 配置表支持装备材料合成
4. THE Game_Service SHALL 实现装备穿戴和卸下操作，装备穿戴后更新英雄属性并重新计算战斗力
5. THE Game_Service SHALL 对装备类道具实现不可叠加逻辑，每件装备占用独立的道具索引

### 需求 60：地狱活动系统模块

**用户故事：** 作为玩家，我希望能参与地狱活动获取高级奖励，以便加速角色成长。

#### 验收标准

1. THE Game_Service SHALL 实现地狱活动管理（HellActivityProxy），存储活动数据到 c_hallActivity 表
2. THE Game_Service SHALL 实现地狱活动的开启、进行、结束三个阶段的状态管理
3. THE Game_Service SHALL 实现地狱活动排行榜（c_hell_activity_rank），记录玩家在地狱活动中的积分和排名
4. THE Game_Service SHALL 在地狱活动结束时根据排名发放奖励

### 需求 61：国王系统模块

**用户故事：** 作为玩家，我希望服务器有国王竞选和管理系统，以便最强联盟的领袖能成为国王并行使特权。

#### 验收标准

1. THE Game_Service SHALL 实现国王数据管理（c_king 表），存储当前国王信息、任期、特权状态
2. THE Game_Service SHALL 实现国王特权功能，包括全服公告、封号/解封、称号授予等
3. THE Game_Service SHALL 在国王任期结束时自动清除国王状态并触发新一轮竞选

### 需求 62：联盟留言板系统模块

**用户故事：** 作为联盟成员，我希望能在联盟留言板上发布和查看消息，以便联盟内部沟通。

#### 验收标准

1. THE Game_Service SHALL 实现联盟留言板管理（GuildMessageBoardMgr），数据存储在 c_guild_message_board 表中
2. THE Game_Service SHALL 支持留言的发布、查看和删除操作，留言按 messageIndex 索引
3. THE Game_Service SHALL 实现留言数量上限控制，超出上限时自动删除最早的留言

### 需求 63：联盟推荐与搜索模块

**用户故事：** 作为玩家，我希望能搜索和浏览推荐联盟，以便找到合适的联盟加入。

#### 验收标准

1. THE Game_Service SHALL 实现联盟推荐管理（GuildRecommendMgr），根据联盟战力、成员数等条件生成推荐列表
2. THE Game_Service SHALL 实现联盟搜索功能，支持按联盟名称或简称模糊搜索
3. THE Game_Service SHALL 实现联盟索引管理（GuildIndexMgr），维护联盟 ID 到联盟信息的快速查找索引

### 需求 64：地图固定点与村庄山洞模块

**用户故事：** 作为玩家，我希望能在大世界地图上探索村庄和山洞，以便获取探索奖励。

#### 验收标准

1. THE Game_Service SHALL 实现地图固定点管理（MapFixPointMgr），根据 s_MapFixPoint 配置表在地图上放置固定的探索点
2. THE Game_Service SHALL 实现村庄山洞探索逻辑，玩家派遣斥候到达后触发探索事件并发送探索发现邮件
3. THE Game_Service SHALL 实现探索发现报告（DISCOVER_REPORT 类型邮件），包含探索坐标、固定点 ID、圣地类型等信息

### 需求 65：服务器配置管理模块

**用户故事：** 作为运维人员，我希望服务器有统一的配置管理，以便灵活调整各服务节点的运行参数。

#### 验收标准

1. THE Go_Server SHALL 实现配置文件解析，兼容 Lua_Server 的 etc/*.conf 配置格式，支持以下配置项：服务器 ID（serverid）、集群节点名（clusternode）、集群 IP/端口、Web IP/端口、Debug 端口、日志类型、工作线程数等
2. THE Go_Server SHALL 实现各服务节点的独立配置：login.conf（登录服务配置）、game.conf（游戏服务配置）、chat.conf（聊天服务配置）、db.conf（数据库服务配置）、battle.conf（战斗服务配置）、center.conf（中心服务配置）、log.conf（日志服务配置）、monitor.conf（监控服务配置）、push.conf（推送服务配置）
3. THE Go_Server SHALL 支持通过环境变量覆盖配置文件中的占位符（$SERVER_ID、$CLUSTER_NODE、$CLUSTER_IP、$CLUSTER_PORT 等）
4. THE Go_Server SHALL 实现系统配置表管理（c_system），存储运行时可修改的系统参数
5. THE Go_Server SHALL 实现推荐服务器管理（c_recommend），存储各地区的推荐服务器信息供客户端登录时使用

### 需求 66：城市掠夺系统模块

**用户故事：** 作为玩家，我希望攻击其他玩家城市时能掠夺资源，以便通过战斗获取经济收益。

#### 验收标准

1. THE Game_Service SHALL 实现城市掠夺管理（CityPlunderMgr），计算攻击方可掠夺的资源数量
2. THE Game_Service SHALL 根据 s_ResourcesPlunderLoss 配置表计算掠夺比例，受攻击方仓库保护量、资源总量、攻击方负载能力等因素影响
3. WHEN 城市战斗结束且攻击方胜利时，THE Game_Service SHALL 从防守方扣除被掠夺资源并添加到攻击方军队负载中
4. THE Game_Service SHALL 在战斗报告中记录掠夺的资源明细

### 需求 67：士兵锁与医院互斥管理模块

**用户故事：** 作为开发者，我希望士兵增减和伤兵操作有互斥保护，以便防止并发操作导致数据不一致。

#### 验收标准

1. THE Game_Service SHALL 实现士兵锁管理（SoldierLockMgr），对同一角色的士兵增加（addSoldiersInLock）和减少（subSoldiersInLock）操作进行互斥保护
2. THE Game_Service SHALL 实现伤兵互斥管理（SeriousInjureMgr），对同一角色的伤兵增加（addSeriousInLock）和治疗（subSeriousInLock）操作进行互斥保护
3. THE Game_Service SHALL 实现服务繁忙检查（ServiceBusyCheckMgr），在高负载时限制并发请求数量

### 需求 68：军队行军回调与追击系统模块

**用户故事：** 作为开发者，我希望军队到达目标后能自动执行对应的回调逻辑，以便实现完整的行军-到达-执行流程。

#### 验收标准

1. THE Game_Service SHALL 实现军队行军回调系统（ArmyMarchCallback），根据军队的 targetType 在到达目标后执行对应逻辑：攻击城市、采集资源、增援联盟建筑、增援圣地、侦查、运输等
2. THE Game_Service SHALL 实现军队追击逻辑（ArmyFollowUpLogic），当目标移动时军队自动调整行军路径
3. THE Game_Service SHALL 实现军队行走逻辑（ArmyWalkLogic），管理军队在地图上的实时位置更新和 AOI 视野同步
4. THE Game_Service SHALL 实现攻击周围坐标逻辑（AttackAroundPosLogic/AttackAroundPosMgr），支持对指定坐标周围的目标发起攻击
5. THE Game_Service SHALL 实现斥候追击逻辑（ScoutFollowUpLogic），斥候到达目标后执行侦查并返回报告

### 需求 69：联盟建筑初始化与索引管理模块

**用户故事：** 作为开发者，我希望联盟建筑有完善的初始化和索引管理，以便支持联盟旗帜、资源中心等建筑的高效查询。

#### 验收标准

1. THE Game_Service SHALL 实现联盟建筑初始化管理（GuildBuildInitMgr），在联盟创建时初始化默认联盟建筑
2. THE Game_Service SHALL 实现联盟建筑索引管理（GuildBuildIndexMgr），维护联盟建筑 ID 到地图对象的映射关系
3. THE Game_Service SHALL 实现联盟资源点索引管理（GuildResourcePointIndexMgr），追踪联盟领地内的资源点
4. THE Game_Service SHALL 实现联盟建筑场景管理（SceneGuildBuildMgr、SceneGuildResourcePointMgr），管理联盟建筑在地图上的显示和交互
5. THE Game_Service SHALL 实现联盟属性管理（GuildAttrMgr），计算联盟科技、建筑等提供的属性加成
6. THE Game_Service SHALL 实现联盟定时器管理（GuildTimerMgr），管理联盟建筑建造、科技研究等定时任务

### 需求 70：错误码与枚举定义模块

**用户故事：** 作为开发者，我希望 Go 服务器有完整的错误码和枚举定义，以便与客户端错误处理逻辑一致。

#### 验收标准

1. THE Go_Server SHALL 实现与 Lua_Server 完全一致的错误码定义（ErrorCode），兼容 `common/errorcode/ErrorCode.lua` 中定义的所有错误码
2. THE Go_Server SHALL 实现与 Lua_Server 完全一致的枚举定义，包括但不限于：ActivityEnum、ArmyEnum、BattleEnum、BuildingEnum、ChatEnum、EmailEnum、ExpeditionEnum、GuildEnum、HeroEnum、HolyLandEnum、ItemEnum、LogEnum、MapEnum、MonsterEnum、MonumentEnum、RallyEnum、RankEnum、RechargeEnum、ResourceEnum、RoleEnum、ScoutEnum、SystemEnum、TaskEnum、WebEnum
3. THE Go_Server SHALL 实现属性定义（AttrDef），包含战斗属性的默认值和属性名称映射
4. THE Go_Server SHALL 实现各模块的常量定义（ArmyDef、BattleDef、BuildDef、EarlyWarningDef、EmailDef、GuildBuildDef、GuildDef、GuildGiftDef、HeroDef、HolyLandDef、ItemDef、MonsterCityDef、MonsterDef、RallyDef、ResourceDef、RoleDef、TransportDef）

### 需求 71：聊天频道与消息持久化模块

**用户故事：** 作为开发者，我希望聊天服务有完善的频道管理和消息持久化，以便支持历史消息查询和服务器重启后恢复。

#### 验收标准

1. THE Chat_Service SHALL 实现聊天频道管理（ChatChannel/ChatChannelEntity），为每个频道维护独立的消息队列和在线用户列表
2. THE Chat_Service SHALL 实现聊天消息持久化（ChatSave），将各频道聊天记录存储到 c_chat 表（世界频道）和 c_chat_guild 表（联盟频道）
3. THE Chat_Service SHALL 实现聊天消息数量上限控制，根据 s_ChatChannel 配置的 saveStorageNum 字段限制每个频道的历史消息数量
4. THE Chat_Service SHALL 实现聊天屏蔽词过滤，根据 s_ChatBlock 配置表过滤敏感词汇
5. THE Chat_Service SHALL 在服务器启动时从数据库加载历史聊天记录到内存

### 需求 72：推送服务认证与消息管理（Push_Service）

**用户故事：** 作为开发者，我希望推送服务有独立的认证机制和消息管理，以便可靠地向离线玩家发送推送通知。

#### 验收标准

1. THE Push_Service SHALL 作为独立进程运行，实现推送令牌认证（AuthToken），管理玩家设备的推送令牌
2. THE Push_Service SHALL 实现推送消息管理（PushMgr），根据 s_PushMessageData 和 s_PushMessageGroup 配置表管理推送消息模板
3. THE Push_Service SHALL 实现告警管理（AlarmMgr），支持被攻击、建造完成等紧急事件的即时推送
4. THE Push_Service SHALL 实现推送消息代理（MessagePush），与第三方推送服务（如 FCM、APNs）对接发送推送通知

### 需求 73：道具脚本与使用效果系统

**用户故事：** 作为开发者，我希望道具使用效果通过脚本驱动，以便灵活配置不同道具的使用逻辑。

#### 验收标准

1. THE Game_Service SHALL 实现道具脚本系统（ItemScript），根据道具类型和子类型执行对应的使用效果
2. THE Game_Service SHALL 支持以下道具使用效果类型：资源增加、加速道具、城市 Buff、英雄经验、士兵增加、宝箱开启、头像解锁、迁城道具、和平护盾、战争狂热移除、行动力恢复、选择奖励等
3. THE Game_Service SHALL 实现道具奖励组系统（getItemPackage），根据 s_ItemPackage 配置表发放随机或固定奖励
4. THE Game_Service SHALL 实现选择奖励系统（s_ItemRewardChoice），支持玩家从多个奖励中选择一个

### 需求 74：联盟领地系统模块

**用户故事：** 作为联盟成员，我希望联盟能占领和管理领地，以便获取领地内的资源收益。

#### 验收标准

1. THE Game_Service SHALL 实现联盟领地管理（GuildTerritoryLogic/GuildTerritoryMgr/TerritoryMgr），计算联盟旗帜覆盖的领地范围
2. THE Game_Service SHALL 实现领地地块管理，追踪每个地块的归属联盟，支持领地重叠和争夺
3. THE Game_Service SHALL 实现领地收益计算，联盟成员在联盟领地内采集资源时获得额外加成
4. THE Game_Service SHALL 在联盟旗帜建造或拆除时重新计算联盟领地范围

### 需求 75：GC 与内存管理模块

**用户故事：** 作为开发者，我希望服务器有合理的内存管理策略，以便保证长时间运行的稳定性。

#### 验收标准

1. THE Go_Server SHALL 实现 GC 管理策略（对应 Lua_Server 的 GCMgr），定期执行内存回收以防止内存泄漏
2. THE Go_Server SHALL 实现内存使用监控，通过 HTTP API 暴露当前内存使用量、goroutine 数量等运行时指标
3. THE Go_Server SHALL 实现对象池复用机制，对高频创建的对象（如 Sproto 编解码缓冲区、消息对象）使用 sync.Pool 复用
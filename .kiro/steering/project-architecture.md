项目架构概览

Unity C# SLG 游戏项目，使用以下技术栈：

框架
- PureMVC: MVC 架构，Notification 驱动通信
- ILRuntime: C# 热更新方案（Hotfix 程序集）
- Sproto: 网络协议序列化（非 Protobuf）
- Addressable: 资源管理和热更新
- Skyunion: 底层框架（插件管理、服务接口）

代码组织
```
Assets/Scripts/
├── Client/          -- 客户端原生层（地图、LOD、天气、HUD 等 C# MonoBehaviour）
├── Hotfix/          -- 热更新层（ILRuntime 加载，核心业务逻辑）
│   ├── Common/PureMVC/  -- PureMVC 框架源码
│   ├── Config/          -- 配置表自动生成的 C# 类（XxxConfig.cs）
│   ├── Manager/         -- 全局管理器（GameMode、ServerTime 等）
│   ├── MVC/
│   │   ├── UI.cs        -- 所有 UIInfo 注册
│   │   ├── RS.cs        -- 资源路径常量
│   │   ├── CMD/         -- Command 层（网络请求、业务命令）
│   │   ├── Proxy/       -- 数据层（数据模型管理）
│   │   ├── View_Mediator/ -- 视图层（View + Mediator）
│   │   └── Base/        -- MVC 基类
│   └── Protocol/        -- Sproto 协议定义
├── ILRTBind/        -- ILRuntime CLR 绑定
├── IFix/            -- IFix 热修复
├── Native/          -- 原生插件层
└── Game/            -- 应用入口（ClientApp.cs）
```

PureMVC 通信模式
- View → Mediator: UI 事件回调
- Mediator → Command: SendNotification 触发命令
- Command → Proxy: 读写数据
- Proxy → Mediator: SendNotification 通知数据变化
- Mediator → View: 更新 UI 显示

协议系统
- 协议定义: Assets/Protocol/Protocol.sproto
- 协议工具: tool/sprotodump.lua（生成 C#/Go/SPB）
- 协议实体: Assets/Scripts/Hotfix/Protocol/SprotoEntity.cs

配置表
- 自动生成的 Config 类在 Assets/Scripts/Hotfix/Config/
- 命名规则: XxxConfig.cs（如 HeroConfig.cs、ItemConfig.cs）

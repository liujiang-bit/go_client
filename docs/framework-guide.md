# 项目框架全解析 —— 从启动到业务的完整指南

> 本文档基于源码分析，帮助你一步一步理解整个工程的架构和代码结构。

---

## 目录

1. [项目概览](#1-项目概览)
2. [技术栈](#2-技术栈)
3. [目录结构](#3-目录结构)
4. [启动流程](#4-启动流程)
5. [PureMVC 框架核心](#5-puremvc-框架核心)
6. [AppFacade —— 中央枢纽](#6-appfacade--中央枢纽)
7. [Notification 通信机制](#7-notification-通信机制)
8. [MVC 三层详解](#8-mvc-三层详解)
9. [UI 系统](#9-ui-系统)
10. [网络层](#10-网络层)
11. [全局管理器](#11-全局管理器)
12. [配置表系统](#12-配置表系统)
13. [资源管理](#13-资源管理)
14. [常见开发流程](#14-常见开发流程)

---

## 1. 项目概览

这是一个 **Unity C# SLG（策略类）游戏**项目，类似《万国觉醒》。
玩家在大世界地图上建造城市、训练军队、研究科技、组建联盟、参与战斗。

核心特点：
- 使用 **PureMVC** 作为 MVC 架构，所有模块间通过 **Notification（通知）** 解耦通信
- 使用 **ILRuntime** 实现 C# 热更新，业务逻辑可以不发版本就更新
- 使用 **Sproto** 作为网络协议（不是 Protobuf），带 DES 加密和握手认证

---

## 2. 技术栈

| 技术 | 用途 | 说明 |
|------|------|------|
| **PureMVC** | MVC 架构 | Notification 驱动的经典 MVC，Facade 单例管理全局 |
| **ILRuntime** | C# 热更新 | 将 Hotfix 程序集以 DLL 形式加载，支持不发版更新业务逻辑 |
| **Sproto** | 网络协议 | 类似 Protobuf 的轻量序列化方案，配合 DES 加密 |
| **Addressable** | 资源管理 | Unity 官方资源管理方案，支持热更新资源 |
| **Skyunion** | 底层框架 | 提供插件管理、服务接口（UI、音频、网络、资源、日志等） |
| **IFix** | 热修复 | 用于紧急 Bug 修复的补丁方案 |

---

## 3. 目录结构

```
Assets/Scripts/
├── Game/                    ← 应用入口
│   └── ClientApp.cs         ← Unity MonoBehaviour，程序最先执行的地方
│
├── Client/                  ← 客户端原生层（不可热更新）
│   ├── ClientPlugin.cs      ← 客户端插件，注册原生服务
│   ├── Map/                 ← 大世界地图渲染、LOD、瓦片
│   ├── Weather/             ← 天气系统
│   └── HUD/                 ← 3D 场景中的 UI 标签
│
├── Hotfix/                  ← 热更新层（核心业务逻辑，通过 ILRuntime 加载）
│   ├── Game.cs              ← 热更新入口，被 ILRuntime 调用
│   ├── Common/PureMVC/      ← PureMVC 框架源码
│   ├── Config/              ← 配置表自动生成的 C# 类
│   ├── Manager/             ← 全局管理器（单例）
│   ├── Protocol/            ← Sproto 协议实体（自动生成）
│   └── MVC/                 ← 业务 MVC 代码
│       ├── Base/            ← MVC 基类（AppFacade, GameMediator, GameProxy, GameCmd）
│       ├── CMD/             ← Command 层（处理通知、发起网络请求）
│       ├── Proxy/           ← Proxy 层（数据管理）
│       ├── View_Mediator/   ← View + Mediator 层（UI 界面）
│       ├── UI.cs            ← 所有 UIInfo 注册定义
│       ├── RS.cs            ← 资源路径常量
│       └── CMD/CmdConstant.cs ← 所有 Notification 名称常量
│
├── ILRTBind/                ← ILRuntime CLR 绑定代码
├── IFix/                    ← IFix 热修复
└── Native/                  ← 原生插件层
```

---

## 4. 启动流程

理解启动流程是理解整个框架的第一步。从 Unity 场景加载开始，到玩家看到登录界面，经历了以下步骤：

```
Unity 场景加载
    │
    ▼
ClientApp.Awake()              ← MonoBehaviour 生命周期
    │
    ▼
ClientApp.OnAddPlugin()        ← 注册三个插件
    │  ├── CorePlugin          ← 核心服务（UI、音频、资源、网络等）
    │  ├── ClientPlugin        ← 客户端原生功能（地图、天气等）
    │  └── NativePlugin        ← 原生平台插件
    │
    ▼
Skyunion 插件系统初始化         ← 各插件的 Install() 被调用
    │
    ▼
ClientApp.OnInitialized()      ← 所有插件初始化完成
    │  ├── ILRuntime 绑定初始化 (ILRTBind.Init)
    │  └── 语言设置
    │
    ▼
ILRuntime 加载 Hotfix.dll      ← Skyunion 框架自动完成
    │
    ▼
Hotfix.Game.Initialize()       ← 热更新入口被调用
    │  └── AppFacade.GetInstance().StartUp()
    │
    ▼
AppFacade 构造函数              ← 首次 GetInstance 触发
    │  ├── InitializeModel()   ← 创建 Model 单例
    │  ├── InitializeController() ← 创建 Controller 单例 + 注册所有 Command
    │  └── InitializeView()    ← 创建 View 单例
    │
    ▼
AppFacade.StartUp()
    │  └── SendNotification("AppFacade.StartUp")
    │
    ▼
StartUpCommand.Execute()       ← 启动命令被执行
    ├── 初始化调试工具（Debug 模式）
    ├── 初始化主线程调度器 (Dispatcher)
    ├── 初始化画质设置
    ├── 加载音量设置
    ├── 加载缓存语言
    ├── 替换 IGGSDK 弹窗接口
    └── 判断语言 → 弹出语言选择 或 SendNotification(ReloadGame)
```

关键理解点：
- `ClientApp` 是 Unity 原生层的入口，负责插件注册
- `Hotfix.Game` 是热更新层的入口，负责启动 PureMVC
- `AppFacade` 的构造函数中完成了所有 Command 的注册（这是整个系统的"接线板"）
- `StartUpCommand` 是第一个被执行的 Command，负责初始化各种基础设施

---

## 5. PureMVC 框架核心

PureMVC 是一个经典的 MVC 框架，本项目使用的是 C# Standard 版本。理解它的四个核心角色：

### 5.1 Facade（门面）

**文件**: `Assets/Scripts/Hotfix/Common/PureMVC/Patterns/Facade/Facade.cs`

Facade 是整个 PureMVC 的入口，是一个单例。它持有三个核心组件的引用：

```csharp
protected IController controller;  // 管理 Command
protected IModel model;            // 管理 Proxy
protected IView view;              // 管理 Mediator + Observer
```

Facade 提供的核心方法：
- `RegisterCommand(name, factory)` — 注册通知名到 Command 的映射
- `RegisterProxy(proxy)` — 注册数据代理
- `RegisterMediator(mediator)` — 注册 UI 中介者
- `SendNotification(name, body, type)` — 发送通知（驱动整个系统运转的核心方法）

### 5.2 Controller（控制器）

**文件**: `Assets/Scripts/Hotfix/Common/PureMVC/Core/Controller.cs`

Controller 维护一个 `ConcurrentDictionary<string, Func<ICommand>>`，即通知名 → Command 工厂的映射。

当收到通知时：
1. 根据通知名查找对应的 Command 工厂
2. 创建 Command 实例（每次都是新实例）
3. 调用 `command.Execute(notification)`

### 5.3 Model（模型）

**文件**: `Assets/Scripts/Hotfix/Common/PureMVC/Core/Model.cs`

Model 维护一个 `ConcurrentDictionary<string, IProxy>`，即名称 → Proxy 的映射。
Proxy 注册后会调用 `OnRegister()`，移除时调用 `OnRemove()`。

### 5.4 View（视图管理）

**文件**: `Assets/Scripts/Hotfix/Common/PureMVC/Core/View.cs`

View 维护两个字典：
- `mediatorMap`: 名称 → Mediator 的映射
- `observerMap`: 通知名 → Observer 列表的映射

当 Mediator 注册时，View 会：
1. 调用 `mediator.ListNotificationInterests()` 获取它关心的通知列表
2. 为每个通知创建 Observer，绑定到 `mediator.HandleNotification`
3. 调用 `mediator.OnRegister()`

当 `SendNotification` 被调用时，View 遍历对应通知的 Observer 列表，逐个调用。

---

## 6. AppFacade —— 中央枢纽

**文件**: `Assets/Scripts/Hotfix/MVC/Base/AppFacade.cs`

AppFacade 继承自 PureMVC 的 Facade，是本项目的核心中枢。它做了三件关键的事：

### 6.1 注册所有 Command

在 `InitializeController()` 中，AppFacade 注册了项目中所有的 Command 映射。这些映射分为两类：

**客户端通知 → Command**（由 CmdConstant 定义）：
```csharp
RegisterCommand(CmdConstant.ReloadGame, () => new ReloadGameCMD());
RegisterCommand(CmdConstant.SpeedUp, () => new SpeedUpCMD());
RegisterCommand(CmdConstant.OpenUI, () => new OpenUICMD());
```

**服务器协议 → Command**（由 Sproto TagName 定义）：
```csharp
RegisterCommand(Hero_HeroInfo.TagName, () => new HeroCmd());
RegisterCommand(Email_GetEmails.TagName, () => new EmailCMD());
RegisterCommand(Chat_PushMsg.TagName, () => new ChatCMD());
```

这意味着：当服务器推送一个协议消息时，框架会自动找到对应的 Command 来处理。

### 6.2 管理 Mediator 生命周期

AppFacade 维护了三个 Mediator 字典：
- `m_viewMap`: ViewBinder → Mediator（通过 Unity GameObject 上的 ViewBinder 组件关联）
- `m_MediatorMap`: 名称 → Mediator（快速查找）
- `m_globalMap`: 全局 Mediator（不随 UI 销毁的长生命周期 Mediator）

### 6.3 封装网络发送

```csharp
public void SendSproto(SprotoTypeBase obj)
{
    var net = RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
    net.SendSproto(obj);
}
```

任何地方都可以通过 `AppFacade.GetInstance().SendSproto(obj)` 发送网络请求。

### 6.4 UI 事件监听

```csharp
CoreUtils.uiManager.AddShowUIListener(OnShowUI);
CoreUtils.uiManager.AddCloseUIListener(OnCloseUI);
```

当 UI 打开/关闭时，AppFacade 会发送 `CmdConstant.OnShowUI` / `CmdConstant.OnCloseUI` 通知。

---

## 7. Notification 通信机制

Notification（通知）是 PureMVC 中所有模块间通信的唯一方式。理解它就理解了整个系统的运转方式。

### 7.1 通知的两个来源

**来源一：客户端内部通知**

定义在 `CmdConstant.cs` 中，600+ 个常量字符串：

```csharp
public const string buildQueueChange = "buildQueueChange";     // 建造队列变化
public const string UpdatePlayerPower = "UpdatePlayerPower";   // 更新战力
public const string OpenUI = "OpenUI";                         // 打开 UI
public const string SystemDayChange = "SystemDayChange";       // 系统日期变化
public const string GameModeChanged = "GameModeChanged";       // 游戏模式切换
```

**来源二：服务器协议通知**

由 Sproto 自动生成的协议类的 `TagName` 属性：

```csharp
Hero_HeroInfo.TagName      // 英雄信息协议
Email_GetEmails.TagName    // 获取邮件协议
Chat_PushMsg.TagName       // 聊天推送协议
Map_ObjectInfo.TagName     // 地图对象信息协议
```

### 7.2 通知的流转路径

```
发送方                          接收方
──────                          ──────
任意位置调用:                    
SendNotification(name, body)    
        │                       
        ▼                       
    View.NotifyObservers()      
        │                       
        ├──→ Controller         → 查找并执行对应的 Command
        │                       
        ├──→ Mediator A         → 如果 ListNotificationInterests() 包含该通知
        │                       
        ├──→ Mediator B         → 同上
        │                       
        └──→ SubViewManager     → AppFacade 重写了 SendNotification，额外通知子视图
```

### 7.3 通知的三个参数

```csharp
SendNotification(
    string notificationName,  // 通知名（必需）
    object body = null,       // 携带数据（可选，任意类型）
    string type = null        // 类型标记（可选，用于区分同名通知的不同场景）
);
```

---

## 8. MVC 三层详解

### 8.1 Command 层（命令）

**基类**: `GameCmd` → `SimpleCommand`
**目录**: `Assets/Scripts/Hotfix/MVC/CMD/`

Command 是"一次性"的处理器。每次通知触发时，Controller 会创建一个新的 Command 实例，调用 `Execute(notification)` 后即丢弃。

Command 的典型职责：
- 处理服务器协议响应，解析数据并存入 Proxy
- 执行业务逻辑（如加速、奖励领取）
- 转发通知给其他模块

```csharp
// 示例：HeroCmd 处理英雄相关通知
public class HeroCmd : GameCmd
{
    public override void Execute(INotification notification)
    {
        switch (notification.Name)
        {
            case Hero_HeroInfo.TagName:
                // 解析服务器返回的英雄数据
                var data = notification.Body as Hero_HeroInfo.response;
                var proxy = AppFacade.GetInstance().RetrieveProxy("HeroProxy") as HeroProxy;
                proxy.UpdateHeroInfo(data);
                // 通知 UI 刷新
                SendNotification(CmdConstant.UpdateHero);
                break;
        }
    }
}
```

一个 Command 类可以处理多个通知（通过 switch-case 区分 `notification.Name`）。
在 AppFacade 中，多个通知名可以映射到同一个 Command 类。

### 8.2 Proxy 层（数据代理）

**基类**: `GameProxy` → `Proxy`
**目录**: `Assets/Scripts/Hotfix/MVC/Proxy/`

Proxy 负责管理数据。它是长生命周期的单例，注册后一直存在。

```csharp
public class GameProxy : Proxy
{
    public GameProxy(string proxyName) : base(proxyName, null) { }
}
```

Proxy 的典型用法：
- 存储从服务器获取的数据
- 提供数据查询接口给 Mediator 和 Command
- 数据变化时通过 `SendNotification` 通知外部

关键 Proxy：
- `NetProxy` — 网络连接管理，Sproto 收发
- `PlayerProxy` — 玩家基础数据
- `DataProxy` — 通用数据存储

获取 Proxy 的方式：
```csharp
var proxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
```

### 8.3 View + Mediator 层（视图）

**基类**: `GameMediator` → `Mediator`
**目录**: `Assets/Scripts/Hotfix/MVC/View_Mediator/`

这是 PureMVC 中最复杂的一层。每个 UI 界面由两个类组成：

- **View**: 纯 UI 逻辑，持有 UI 控件引用，处理显示
- **Mediator**: 中介者，连接 View 和 PureMVC 系统

#### GameMediator 生命周期

```csharp
public class GameMediator : Mediator
{
    // 注册时自动调用（相当于 Init）
    public override void OnRegister()
    {
        InitData();      // 1. 初始化数据（获取 Proxy 引用等）
        BindUIData();    // 2. 绑定数据到 UI（初始显示）
        BindUIEvent();   // 3. 绑定 UI 事件（按钮点击等）
    }

    // 移除时调用（相当于 Destroy）
    public override void OnRemove() { }

    // 声明关心的通知列表
    public override string[] ListNotificationInterests() { }

    // 处理收到的通知
    public override void HandleNotification(INotification notification) { }

    // 可选：每帧更新（需要 IsOpenUpdate = true）
    public virtual void Update() { }

    // UI 打开动画结束后调用
    public virtual void OpenAniEnd() { }

    // 窗口获得焦点
    public virtual void WinFocus() { }

    // 窗口关闭
    public virtual void WinClose() { }
}
```

#### Mediator 的两种类型

**UI Mediator**（随界面生命周期）：
- 界面打开时创建，关闭时销毁
- 通过 `UIInfo` 注册，由 Skyunion UI 管理器自动管理

**Global Mediator**（全局长生命周期）：
- 不依赖 UI 界面，常驻内存
- 通过 `GlobalBehaviourManger` 注册
- 可以接收 Update 回调
- 适用于：地图逻辑、部队行军、聊天监听等

```csharp
// 注册全局 Mediator
GlobalBehaviourManger.Instance.AddGlobalMeditor<MapMediator>(true);  // true = 需要 Update

// 移除全局 Mediator
GlobalBehaviourManger.Instance.RemoveGlobalMediator(MapMediator.NameMediator);
```

---

## 9. UI 系统

**文件**: `Assets/Scripts/Hotfix/MVC/UI.cs`

UI 系统是 Skyunion 框架提供的，PureMVC 通过 Mediator 与之对接。

### 9.1 UIViewInfo（视图模板）

UIViewInfo 定义了一类 UI 的通用行为：

```csharp
new UIViewInfo(UIViewType, UILayer, UIAddMode, UICloseMode)
```

| 参数 | 说明 | 常用值 |
|------|------|--------|
| UIViewType | 视图类型 | `FullView`（全屏）、`Window`（弹窗）、`hud`（HUD） |
| UILayer | 渲染层级 | `HUDLayer`、`FullViewLayer`、`WindowLayer`、`WindowPopLayer`、`GuideLayer` 等 |
| UIAddMode | 添加方式 | `Stack`（入栈）、`Replace`（替换当前） |
| UICloseMode | 关闭方式 | `PopWin`（弹出销毁）、`Hide`（隐藏不销毁）、`PopAll`（关闭所有） |

项目中预定义了多种模板：

```csharp
s_fullWindow   // 全屏界面，Replace 模式
s_popWin       // 弹出窗口，Stack 模式
s_popWinPop    // 弹窗上的弹窗（更高层级）
s_popWinHide   // 弹窗，关闭时隐藏（不销毁）
s_guide        // 引导层
s_loading      // 加载界面
```

### 9.2 UIInfo（界面注册）

每个具体界面都需要注册一个 UIInfo：

```csharp
new UIInfo(viewName, viewType, viewInfo, maskStatus, subViews, sortOrder, groupId, needPrewarm, needAnim)
```

| 参数 | 说明 |
|------|------|
| viewName | 界面名称（对应 Prefab 名） |
| viewType | View 类的 Type |
| viewInfo | 使用哪个 UIViewInfo 模板 |
| maskStatus | 遮罩行为 |
| subViews | 附属子界面数组 |
| sortOrder | 排序优先级 |

EnumMaskStatus 遮罩类型：
- `kNone` — 无遮罩
- `kOnlyShow` — 仅显示遮罩（不可点击关闭）
- `kTouchClose` — 点击遮罩关闭
- `kTouchCloseAlpha` — 透明遮罩，点击关闭
- `kNoMaskNoTouch` — 无遮罩无触摸拦截

### 9.3 打开/关闭 UI

```csharp
// 打开界面
CoreUtils.uiManager.ShowUI(UI.s_bagInfo, callback, param1, param2);

// 关闭界面
CoreUtils.uiManager.CloseUI(UI.s_bagInfo);
```

---

## 10. 网络层

**文件**: `Assets/Scripts/Hotfix/MVC/Proxy/NetProxy.cs`

网络层是整个游戏与服务器通信的基础。

### 10.1 架构概览

```
AppFacade.SendSproto(obj)
        │
        ▼
    NetProxy.SendSproto(obj)
        │
        ▼
    SprotoSocketAp.SendSproto(obj)
        │  ├── Sproto 序列化
        │  ├── DES 加密
        │  └── 添加包头（2字节长度 + 4字节 GameSession + 1字节压缩标记）
        │
        ▼
    INetClient.Send()  ← Skyunion 底层网络服务
```

### 10.2 连接认证流程（DH 密钥交换）

```
1. 客户端连接服务器 TCP
2. 服务器 → 客户端: base64(8字节随机 challenge)
3. 客户端 → 服务器: base64(8字节 client_key)
4. 服务器 → 客户端: base64(DH-Exchange(server_key))
5. 双方计算共享密钥: secret = DH-Secret(client_key/server_key)
6. 客户端 → 服务器: base64(HMAC(challenge, secret))
7. 客户端 → 服务器: DES(secret, base64(token))
8. 服务器验证 token，返回 uid + 游戏服务器地址
9. 客户端重定向到游戏服务器，用 uid 再次认证
```

认证状态枚举：
```csharp
enum ELoginState
{
    EAuth1,                    // 初始认证阶段1
    EAuth2,                    // 认证阶段2
    EAuth3,                    // 认证阶段3
    EAuthOK,                   // 认证成功
    ERedirectionGameServer,    // 重定向到游戏服务器
    EGameServerAuthOK,         // 游戏服务器认证成功
}
```

### 10.3 协议收发

**发送请求**：
```csharp
// 构造协议对象
var req = new Hero_HeroInfo.request();
req.heroId = 1001;

// 发送
AppFacade.GetInstance().SendSproto(req);
```

**接收响应**：
服务器响应到达后，`SprotoSocketAp.OnReciveSproto()` 解析协议，然后通过 `SendNotification(TagName, responseBody)` 分发。
由于 AppFacade 已经注册了 `TagName → Command` 的映射，对应的 Command 会自动被执行。

### 10.4 心跳机制

```csharp
// 心跳协议: Role_Heart
// 发送间隔: 定时器驱动
// 作用: 保持连接 + 同步服务器时间
// 如果心跳误差超过 1 秒，会触发重新同步
```

---

## 11. 全局管理器

项目中有多个全局单例管理器，它们不属于 MVC 的任何一层，但为整个系统提供基础服务。

### 11.1 GlobalBehaviourManger

**文件**: `Assets/Scripts/Hotfix/Manager/GlobalBehaviourManager.cs`

管理全局 Mediator 的生命周期。它创建一个 `BehaviourBinder`（MonoBehaviour），将 Mediator 的 Update/FixedUpdate/LateUpdate 挂载到 Unity 的更新循环中。

```csharp
// 添加全局 Mediator（带 Update 回调）
GlobalBehaviourManger.Instance.AddGlobalMeditor<MapMediator>(true);

// 获取
var mediator = GlobalBehaviourManger.Instance.GetGlobalMediator("MapMediator");

// 移除
GlobalBehaviourManger.Instance.RemoveGlobalMediator("MapMediator");

// 添加自定义 Update 监听
GlobalBehaviourManger.Instance.AddUpdateListener(MyUpdateFunc);
```

### 11.2 ServerTimeModule

**文件**: `Assets/Scripts/Hotfix/Manager/ServerTimeModule.cs`

服务器时间同步模块。通过心跳协议 `Role_Heart` 校准本地时间与服务器时间的偏差。

核心原理：
```
Lose_Time = serverTime + Ping - clientTime
本地服务器时间 = 本地 Ticks / 10000 + Lose_Time
```

常用 API：
```csharp
// 获取服务器时间（秒）
long seconds = ServerTimeModule.Instance.GetServerTime();

// 获取服务器时间（毫秒）
long millis = ServerTimeModule.Instance.GetServerTimeMilli();

// 获取当前服务器日期时间（考虑时区）
DateTime dt = ServerTimeModule.Instance.GetCurrServerDateTime();

// 距离今日零点的剩余秒数
long remain = ServerTimeModule.Instance.GetDistanceZeroTime();

// 距离下周日零点的秒数
long nextWeek = ServerTimeModule.Instance.GetNextSundayTime();
```

### 11.3 GameModeManager

**文件**: `Assets/Scripts/Hotfix/Manager/GameModeManager.cs`

管理游戏的三种视角模式：

```csharp
enum GameModeType
{
    World,       // 大世界地图
    Expedition,  // 远征视角
    Citylayout,  // 城市布局视角
}

// 切换模式
GameModeManager.Instance.ChangeMode(GameModeType.World);

// 获取当前模式
var mode = GameModeManager.Instance.CurGameMode;
```

切换模式时会发送 `CmdConstant.GameModeChanged` 通知，各 Mediator 可以监听并做出响应。

### 11.4 Skyunion 服务接口（CoreUtils）

Skyunion 框架通过 `CoreUtils` 提供全局服务访问：

```csharp
CoreUtils.uiManager      // UI 管理器（ShowUI、CloseUI）
CoreUtils.audioService    // 音频服务（BGM、SFX）
CoreUtils.assetService    // 资源服务（Instantiate、LoadAsset）
CoreUtils.logService      // 日志服务
CoreUtils.dataService     // 数据服务（配置表查询）
CoreUtils.hotService      // 热更新服务（ILRuntime）
```

---

## 12. 配置表系统

**目录**: `Assets/Scripts/Hotfix/Config/`

策划在 Excel 中编辑数据表，通过工具自动生成 C# 类。

### 12.1 生成的 Config 类

每个配置表对应一个 `XxxConfig.cs` 或 `XxxDefine.cs`，例如：
- `HeroConfig.cs` — 英雄配置
- `ItemConfig.cs` — 道具配置
- `BuildingConfig.cs` — 建筑配置
- `LanguageSetDefine.cs` — 语言设置

### 12.2 查询配置

通过 Skyunion 的 `dataService` 查询：

```csharp
// 查询所有记录
var records = CoreUtils.dataService.QueryRecords<LanguageSetDefine>();

// 遍历
for (int i = 0; i < records.Count; i++)
{
    var config = records[i];
    // 使用 config.ID, config.name 等字段
}
```

### 12.3 资源路径常量（RS.cs）

**文件**: `Assets/Scripts/Hotfix/MVC/RS.cs`

RS 类集中定义了代码中使用的所有资源路径字符串：

```csharp
public class RS
{
    // 通用 Tip 资源
    public const string Tip_Up = "UI_Common_UpTips";
    public const string Tip_Mid = "UI_Common_MidTips";

    // Alert 资源
    public const string Alert = "UI_Common_Alert";

    // 品质背景数组
    public static string[] ItemQualityBg = new[] { "item0[img_item_bg1]", ... };

    // 音量 Key
    public const string BGMVolume = "BGMVolume";
    public const string SfxVolume = "SfxVolume";
}
```

---

## 13. 资源管理

项目使用 Unity Addressable 系统管理资源。

### 13.1 Addressable 分组

在 `Assets/AddressableAssetsData/AssetGroups/` 下定义了 100+ 个资源分组：
- `Barbarian.asset` — 野蛮人资源
- `CityLevel1~5.asset` — 城市各等级资源
- `Effect.asset` — 特效资源
- `Hero_img.asset` — 英雄图片
- `Hero_SD.asset` — 英雄 SD 模型
- `Emoji.asset` — 表情资源

### 13.2 资源加载

通过 Skyunion 的 `assetService` 加载：

```csharp
// 实例化 Prefab
CoreUtils.assetService.Instantiate("PrefabName", (gameObject) => {
    // 使用 gameObject
});

// 加载资源（Sprite、Texture 等）
// 通过 Addressable API
```

### 13.3 AssetBundle 构建

`AssetBundles/` 目录下有 Android 和 Windows 两个平台的构建输出目录。

---

## 14. 常见开发流程

### 14.1 新增一个 UI 界面

```
步骤 1: 在 Unity 中制作 Prefab
步骤 2: 创建 View 类（继承框架 View 基类）
步骤 3: 创建 Mediator 类（继承 GameMediator）
步骤 4: 在 UI.cs 中注册 UIInfo
步骤 5: 在需要的地方调用 CoreUtils.uiManager.ShowUI()
```

Mediator 模板：
```csharp
public class MyFeatureMediator : GameMediator
{
    public new const string NAME = "MyFeatureMediator";

    public MyFeatureMediator(object viewComponent)
        : base(NAME, viewComponent) { }

    protected override void InitData()
    {
        // 获取 Proxy 引用
    }

    protected override void BindUIData()
    {
        // 初始化 UI 显示
    }

    protected override void BindUIEvent()
    {
        // 绑定按钮点击等事件
    }

    public override string[] ListNotificationInterests()
    {
        return new string[]
        {
            CmdConstant.SomeEvent,
            SomeProtocol.TagName,
        };
    }

    public override void HandleNotification(INotification notification)
    {
        switch (notification.Name)
        {
            case CmdConstant.SomeEvent:
                // 刷新 UI
                break;
        }
    }
}
```

### 14.2 新增一个网络协议处理

```
步骤 1: 在 Protocol.sproto 中定义协议
步骤 2: 运行 sprotodump 生成 C# 实体类（Sproto.cs）
步骤 3: 在 AppFacade.InitializeController() 中注册 Command 映射
步骤 4: 在对应的 Command 中处理协议响应
步骤 5: 更新 Proxy 数据，发送 Notification 通知 UI
```

### 14.3 新增一个全局管理器

```
步骤 1: 创建类，继承 TSingleton<T>
步骤 2: 如果需要 Update，通过 GlobalBehaviourManger 注册
步骤 3: 在 Hotfix.Game.DestroyInstance() 中添加清理调用
```

### 14.4 数据流完整示例（以"领取邮件附件"为例）

```
1. 用户点击"领取"按钮
   └→ View 触发回调 → Mediator.OnClickCollect()

2. Mediator 发送网络请求
   └→ var req = new Email_TakeEnclosure.request();
      req.emailId = id;
      AppFacade.GetInstance().SendSproto(req);

3. 服务器处理后返回响应
   └→ SprotoSocketAp 接收并解析
      └→ SendNotification(Email_TakeEnclosure.TagName, response)

4. Controller 找到对应 Command
   └→ EmailCMD.Execute() 被调用
      └→ 解析 response，更新 EmailProxy 数据
      └→ SendNotification(CmdConstant.EmailDataChanged)

5. Mediator 收到通知
   └→ HandleNotification() 中匹配 EmailDataChanged
      └→ 刷新 UI 列表显示
```

---

## 附录：关键文件速查表

| 文件 | 作用 |
|------|------|
| `Assets/Scripts/Game/ClientApp.cs` | 应用入口，注册插件 |
| `Assets/Scripts/Hotfix/Game.cs` | 热更新入口，启动 AppFacade |
| `Assets/Scripts/Hotfix/MVC/Base/AppFacade.cs` | 中央枢纽，注册所有 Command |
| `Assets/Scripts/Hotfix/MVC/Base/GameMediator.cs` | Mediator 基类 |
| `Assets/Scripts/Hotfix/MVC/Base/GameProxy.cs` | Proxy 基类 |
| `Assets/Scripts/Hotfix/MVC/Base/GameCmd.cs` | Command 基类 |
| `Assets/Scripts/Hotfix/MVC/UI.cs` | 所有 UIInfo 注册 |
| `Assets/Scripts/Hotfix/MVC/RS.cs` | 资源路径常量 |
| `Assets/Scripts/Hotfix/MVC/CMD/CmdConstant.cs` | 通知名称常量（600+） |
| `Assets/Scripts/Hotfix/MVC/CMD/StartUpCommand.cs` | 启动命令 |
| `Assets/Scripts/Hotfix/MVC/Proxy/NetProxy.cs` | 网络层（Sproto + DES） |
| `Assets/Scripts/Hotfix/Manager/ServerTimeModule.cs` | 服务器时间同步 |
| `Assets/Scripts/Hotfix/Manager/GameModeManager.cs` | 游戏模式管理 |
| `Assets/Scripts/Hotfix/Manager/GlobalBehaviourManager.cs` | 全局 Mediator 管理 |
| `Assets/Scripts/Hotfix/Common/PureMVC/` | PureMVC 框架源码 |
| `Assets/Scripts/Hotfix/Protocol/Sproto.cs` | 协议实体（自动生成） |
| `Assets/Scripts/Hotfix/Config/` | 配置表 C# 类（自动生成） |

---

## 附录：核心类关系图

```
                          ┌─────────────────────────────────┐
                          │          AppFacade               │
                          │  (Singleton, 中央枢纽)           │
                          │                                  │
                          │  ┌───────────┐ ┌──────────────┐ │
                          │  │ Controller │ │    Model     │ │
                          │  │            │ │              │ │
                          │  │ Command Map│ │  Proxy Map   │ │
                          │  └─────┬──────┘ └──────┬───────┘ │
                          │        │               │         │
                          │  ┌─────┴───────────────┴──────┐  │
                          │  │           View              │  │
                          │  │  Observer Map + Mediator Map│  │
                          │  └─────────────────────────────┘  │
                          └──────────────┬──────────────────┘
                                         │
                    SendNotification(name, body)
                                         │
                 ┌───────────────────────┼───────────────────────┐
                 │                       │                       │
                 ▼                       ▼                       ▼
          ┌──────────┐           ┌──────────────┐        ┌──────────────┐
          │ Command  │           │  Mediator A  │        │  Mediator B  │
          │ (一次性)  │           │  (UI 界面)    │        │  (全局)      │
          │          │           │              │        │              │
          │ Execute()│           │ HandleNotif()│        │ HandleNotif()│
          └────┬─────┘           └──────┬───────┘        └──────────────┘
               │                        │
               ▼                        ▼
          ┌──────────┐           ┌──────────────┐
          │  Proxy   │           │    View      │
          │ (数据层)  │           │  (UI 控件)   │
          └──────────┘           └──────────────┘
```

---

## 建议阅读顺序

如果你是第一次接触这个项目，建议按以下顺序阅读源码：

1. **`ClientApp.cs`** → 理解程序入口和插件系统
2. **`Game.cs`** → 理解热更新入口
3. **`Facade.cs`** → 理解 PureMVC 核心机制
4. **`AppFacade.cs`** → 理解项目如何使用 PureMVC（重点看 InitializeController）
5. **`StartUpCommand.cs`** → 理解启动流程
6. **`GameMediator.cs`** → 理解 UI 界面的生命周期
7. **`CmdConstant.cs`** → 浏览通知常量，了解系统有哪些事件
8. **`UI.cs`** → 浏览所有注册的界面，了解项目功能范围
9. **`NetProxy.cs`** → 理解网络通信机制
10. **`ServerTimeModule.cs`** → 理解时间同步
11. 选择一个具体业务模块（如邮件 Email），从 Command → Proxy → Mediator 完整跟一遍

---
inclusion: fileMatch
fileMatchPattern: '**/View_Mediator/**/*.cs'
---

UI View 生命周期管理 (PureMVC)

当创建新 UI 界面或需要理解 View/Mediator 生命周期时使用。

项目架构
本项目使用 PureMVC 框架，UI 由 View + Mediator 组成，通过 Notification 通信。

目录结构
Assets/Scripts/Hotfix/MVC/
├── View_Mediator/功能名/
│   ├── XxxView.cs          -- 视图层（继承 GameView，绑定 UI 组件）
│   └── XxxMediator.cs      -- 中介层（继承 Mediator，处理通知和业务逻辑）
├── CMD/XxxCMD.cs            -- 命令层（处理网络请求和复杂业务）
├── Proxy/XxxProxy.cs        -- 数据层（继承 Proxy，管理数据模型）
└── Base/                    -- 基类定义

UI 注册
在 Assets/Scripts/Hotfix/MVC/UI.cs 中注册 UIInfo：
```csharp
public static UIInfo s_myWindow = new UIInfo(
    MyView.VIEW_NAME,        // 视图名称
    typeof(MyView),          // 视图类型
    s_popWin,                // UIViewInfo（窗口类型）
    EnumMaskStatus.kTouchClose  // 遮罩行为
);
```

UIViewInfo 常用类型
- s_fullWindow: 全屏窗口（Replace 模式）
- s_popWin: 弹出窗口（Stack 模式，点击关闭）
- s_popWinPop: 弹窗上的弹窗
- s_fullViewMenu: 全屏菜单（Hide 模式）
- s_hudView: HUD 层

EnumMaskStatus 遮罩类型
- kNone: 无遮罩
- kOnlyShow: 仅显示遮罩
- kTouchClose: 点击遮罩关闭
- kTouchCloseAlpha: 透明遮罩点击关闭
- kNoMaskNoTouch: 无遮罩无触摸

打开/关闭窗口
```csharp
// 打开
AppFacade.GetInstance().SendNotification(CmdConstant.OpenUI, UI.s_myWindow);
// 关闭
AppFacade.GetInstance().SendNotification(CmdConstant.CloseUI, UI.s_myWindow);
```

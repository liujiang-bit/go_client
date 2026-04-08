---
inclusion: manual
---

添加新活动适配指南 (PureMVC)

当需要添加新活动时使用。

1. UI 注册

- Assets/Scripts/Hotfix/MVC/UI.cs
  - 添加 UIInfo 静态字段，指定 View 类型和窗口模式
  ```csharp
  public static UIInfo s_myActivity = new UIInfo(
      MyActivityView.VIEW_NAME, typeof(MyActivityView),
      s_popWin, EnumMaskStatus.kTouchClose);
  ```

2. View + Mediator

- Assets/Scripts/Hotfix/MVC/View_Mediator/Activity/
  - 新建 MyActivityView.cs（继承 GameView）
  - 新建 MyActivityMediator.cs（继承 Mediator）
  - Mediator 在 ListNotificationInterests 中注册关心的通知
  - Mediator 在 HandleNotification 中处理通知

3. Command

- Assets/Scripts/Hotfix/MVC/CMD/ActivityCMD.cs
  - 添加活动相关的命令处理
  - 或新建独立的 CMD 文件

4. Proxy 数据层

- Assets/Scripts/Hotfix/MVC/Proxy/ActivityProxy.cs
  - 在现有 ActivityProxy 中扩展，或新建独立 Proxy
  - Proxy 负责管理活动数据和网络请求

5. 配置表

- Assets/Scripts/Hotfix/Config/
  - 如有新配置表，添加对应的 XxxConfig.cs

6. 资源常量

- Assets/Scripts/Hotfix/MVC/RS.cs
  - 添加活动相关的资源路径常量

注意事项
- 参考现有活动实现（View_Mediator/Activity/ 目录）
- 通过 Notification 通信，避免直接引用
- 活动数据通过 Proxy 管理，View 只负责显示

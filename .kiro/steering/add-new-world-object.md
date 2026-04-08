---
inclusion: manual
---

添加新世界物体适配指南

当需要在大世界地图添加新的交互物体时使用。

1. 世界物体数据

- Assets/Scripts/Hotfix/MVC/Proxy/WorldMapObjectProxy.cs
  - 管理世界地图物体数据
- Assets/Scripts/Hotfix/Common/WorldObjs/
  - 世界物体相关的通用逻辑

2. UI 弹窗

- Assets/Scripts/Hotfix/MVC/View_Mediator/UIWorldHud/
  - 世界 HUD 相关视图
- Assets/Scripts/Hotfix/MVC/View_Mediator/UIWorldObjectPlayer/
  - 世界物体玩家交互视图
- 参考 UI_Pop_WorldObjectInfoView 等已有弹窗实现

3. 地图逻辑

- Assets/Scripts/Client/MapManager/
  - 地图管理器（C# 原生层）
- Assets/Scripts/Client/TileCollideManager/
  - 地格碰撞管理

4. 配置

- Assets/Scripts/Hotfix/Config/MapBuildingDataConfig.cs
  - 地图建筑数据配置
- Assets/Scripts/Hotfix/Config/MapItemTypeConfig.cs
  - 地图物品类型配置
- Assets/Scripts/Hotfix/Config/MapFixPointConfig.cs
  - 地图固定点配置

5. UI 注册

在 UI.cs 中添加弹窗 UIInfo：
```csharp
public static UIInfo s_Pop_MyWorldObj = new UIInfo(
    UI_Pop_MyWorldObjView.VIEW_NAME,
    typeof(UI_Pop_MyWorldObjView),
    s_popWinPop,
    EnumMaskStatus.kTouchCloseAlpha);
```

注意事项
- 世界物体的渲染在 Client 层（C# MonoBehaviour），业务逻辑在 Hotfix 层
- 通过 PureMVC Notification 连接两层
- 参考 MapObjCmd.cs 中的地图物体命令处理

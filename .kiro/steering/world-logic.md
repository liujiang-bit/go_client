世界业务逻辑
大世界地图上的业务逻辑。

跳转到世界坐标
GoToUtil.GotoWorldPos(pos, zoom, time, onComplete, serverId, worldId, forceBackToSelfServer)
- pos: Vector3 世界坐标，通常由 TileCoordWorld.TileIndexToWorld(pointId, ForceChangeScene.World) 转换
- zoom: 镜头缩放，常用 World_InitZoom
- time: 移动时长(秒)，可传 nil 使用默认值，精确定位可用 LookAtFocusTime
- onComplete: 到达后回调，可传 nil
- serverId/worldId: 跨服跳转时传入，本服传 nil

关闭所有窗口后跳转（从活动界面跳转时必须先调用）
GoToUtil.CloseAllWindows()
GoToUtil.GotoWorldPos(pos, World_InitZoom)

跳转到玩家主城（无目标坐标时的兜底）
local mainCityPos = TileCoordWorld.TileIndexToWorld(LuaEntry.Player:GetMainWorldPos())
GoToUtil.GotoWorldPos(mainCityPos, World_InitZoom)

跳转到指定 pointId（封装方法）
GoToUtil.GotoWorldPointId(pointId, zoom, time, onComplete)

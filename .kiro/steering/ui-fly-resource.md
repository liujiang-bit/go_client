飞行资源动画

当需要实现资源飞行效果时使用。

UIUtil.DoFlyCustom
UIUtil.DoFlyCustom(icon, content, num, srcPos, destPos, width, height, callback, model, minRange, maxRange, parentNode)

参数说明:
- icon: 图标路径(string)
- content: 内容文本(string, 可选)
- num: 数量(number)
- srcPos: 起始位置(Vector3或Transform)
- destPos: 目标位置(Vector3或Transform) - 必需
- width/height: 可选
- callback: 完成回调(function, 可选)

示例
local iconPath = DataCenter.ResourceManager:GetResourceIconByType(resType)
local srcPos = self.flyBtn.transform.position
local destPos = self.resourceIcon.transform.position
UIUtil.DoFlyCustom(iconPath, nil, 1, srcPos, destPos, nil, nil, function()
    Logger.Log("Fly complete")
end)

UIUtil.DoFly
UIUtil.DoFly(rewardType, num, icon, srcPos, destPos, width, height, callback, useTextFormat, moveTime, timeDelta, uuid, parentNode)
用于标准资源类型飞行，会自动处理资源图标路径。

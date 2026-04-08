本地数据存储 (Setting)

用于在本地持久化玩家的设置、状态或临时信息。项目区分了"账号私有"和"全局共享"两种存储方式。

1. 账号私有存储 (Private)
自动处理 UID，不同账号登录时数据隔离。适用于：今日不再提示、新手引导进度、个人偏好设置。
Setting:GetPrivateInt(key, defaultValue)
Setting:GetPrivateBool(key, defaultValue)
Setting:GetPrivateString(key, defaultValue)
Setting:SetPrivateInt(key, value)
Setting:SetPrivateBool(key, value)
Setting:SetPrivateString(key, value)

2. 全局共享存储 (Public)
不拼接 UID，所有账号共享。适用于：多语言设置、画质等级、服务器端口、记住账号。
Setting:GetPublicInt(key, defaultValue)
Setting:GetPublicBool(key, defaultValue)
Setting:GetPublicString(key, defaultValue)
Setting:SetPublicInt(key, value)
Setting:SetPublicBool(key, value)
Setting:SetPublicString(key, value)

Key 的定义规范
通常在 Assets/Main/LuaScripts/Global/EnumType.lua 的 SettingKeys 表中定义。
临时的 key 可以直接写字符串，Private 方法内部会自动处理 UID。

常见用法
检查今日是否首次打开:
local key = "Activity_First_Open_Flag"
if Setting:GetPrivateBool(key, true) then
    Setting:SetPrivateBool(key, false)
end

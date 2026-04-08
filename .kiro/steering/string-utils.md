字符串常见操作

定义见 Assets/Main/LuaScripts/Common/StringUtil.lua。

一、格式化数字（UI 显示用）
千位分隔符: string.GetFormattedSeperatorNum(n)
K/M/G 缩写: string.GetFormattedStr(value, isBuildUpgrade)
保留一位小数: string.GetFloatStr(value)
不四舍五入: string.GetFloatStr_NotRound(value)
百分比(一位小数): string.GetFormattedPercentStr(value) -- 传入0~1
百分比(两位小数): string.GetFormattedPercentStrSpecial(value)
智能整数/小数: UIUtil.FormatSmartNumber(num)

二、解析：字符串转数组/表
整型数组: string.string2array_i(str, sep)
字符串数组: string.string2array_s(str, sep)
浮点数组: string.string2array_f(str, sep)
键值对: string.string2table_ii(str, sep1, sep2) 及 _ss/_si/_is/_sf/_if 变体

三、空值判断
string.IsNullOrEmpty(value) -- nil 或 "" 返回 true

四、富文本颜色（TMP）
CommonUtil.GetColorName(color, str) -- color 如 "#ff9000" 或项目常量

五、性能红线
禁止在 Update、列表的 OnRefresh/SetData 或高频回调里做大量 string.split。
配置或服务器下发的字符串应在进入界面或刷新前解析成 table/array 缓存。
禁止在循环中频繁调用 string.split；应优先使用 string.string2array_i 等一次性解析。

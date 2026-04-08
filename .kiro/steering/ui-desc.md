文本提示

当需要显示道具提示或文字提示时使用。

=== 道具提示 ===
local param = {}
param.itemId = itemId
param.alignObject = self.btn
UIManager:GetInstance():OpenWindow(UIWindowNames.UIItemTips, OpenWinAnimTrue, param)

=== 文字提示 ===
local UIHeroTipView = require "UI.UIHero2.UIHeroTip.View.UIHeroTipView"
local scaleFactor = UIManager:GetInstance():GetScaleFactor()
local position = self.btn.transform.position + Vector3.New(0, 20, 0) * scaleFactor
local param = {}
param.content = Localization:GetString(textId, ...)  -- 必需
param.dir = 1                                         -- 1=上方，2=下方，3=左方，4=右方
param.defWidth = 300                                  -- 可选，默认300
param.pivot = 0.5                                     -- 可选，默认0.5
param.position = position                             -- 必需
UIManager:GetInstance():OpenWindow(UIWindowNames.UIHeroTip, { anim = false }, param)

玩家头像组件

当需要显示玩家头像时使用。

local PlayerHead = require "UI.UICommonIcon.UICommonPlayerHead"

-- 组件定义
self.playerHead = self:AddComponent(PlayerHead, 'path/to/UIPlayerHead')
self.playerClick = self:AddComponent(UIButton, "path/to/UIPlayerHead")
self.playerClick:SetOnClick(function()
    SUSoundUtil.PlayEffect(SoundAssets.Music_Effect_Button)
    self:OnClickHead()
end)

-- 头像赋值
local headBgSpine = DataCenter.DecorationDataManager:GetHeadFrameSpine(
    tonumber(param.headSkinId),
    param.headSkinET,
    nil
)
local headParam = {
    uid = param.ownerUid,
    pic = param.pic,
    picVer = param.picVer,
    headBgSpine = headBgSpine,
}
self.playerHead:ReInit(headParam)
self.playerHead:SetActive(true)

-- 点击处理
function UIExampleView:OnClickHead()
    local param = self.playerData
    if param and param.uid then
        if param.uid == LuaEntry.Player.uid then
            UIManager:GetInstance():OpenWindow(UIWindowNames.UIPlayerInfo, LuaEntry.Player.uid)
        else
            UIManager:GetInstance():OpenWindow(
                UIWindowNames.UIOtherPlayerInfo,
                { anim = true, hideTop = true },
                param.uid
            )
        end
    end
end

注意: 如果 ReInit 时设置了 canClick = true，组件内部会自动处理点击事件，无需额外添加按钮组件。

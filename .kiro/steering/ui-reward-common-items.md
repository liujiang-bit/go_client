奖励弹窗和通用道具展示

当需要显示奖励弹窗或展示道具时使用。

=== 奖励弹窗 ===
DataCenter.RewardManager:ShowCommonReward(rewardData)

reward结构:
[{
type = RewardType.Goods,
value = {
    itemId = tonumber(cardId),
    rewardAdd = tonumber(count) or 0
},...]

=== 通用道具组件 UICommonResItem ===
local UICommonResItem = require "UI.UICommonResItem.UICommonResItem"

function UIRewardList:OnRewardItemMoveIn(itemObj, index)
    local reward = self.rewardList[index]
    if not reward then return end
    local rewardItem = self.scroll_view:AddComponent(UICommonResItem, itemObj)
    if rewardItem then
        local param = {
            count = reward.count,
            itemId = reward.itemId,
            rewardType = reward.rewardType
        }
        rewardItem:ReInit(param)
    end
end

=== 本地配置转换为奖励数据 ===
解析奖励字符串(格式: "id,type,num;id,type,num"):
function ParseRewardStr(rewardStr)
    local rewardList = {}
    if rewardStr ~= nil and rewardStr ~= "" then
        local rewardArr = string.split(rewardStr, ";")
        for k, v in pairs(rewardArr) do
            local strVec = string.split(v, ",")
            if strVec ~= nil and #strVec >= 3 then
                local item = {}
                item.type = tonumber(strVec[2])
                item.value = {
                    id = tonumber(strVec[1]),
                    num = tonumber(strVec[3]),
                }
                if strVec[4] then
                    item.probability = strVec[4]
                end
                table.insert(rewardList, item)
            end
        end
    end
    return DataCenter.RewardManager:ReturnRewardParamForMessage(rewardList)
end

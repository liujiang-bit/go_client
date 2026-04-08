通用对话框和提示

飘字、二次确认、今日不再提示、规则介绍弹窗。

=== 飘字 ===
UIUtil.ShowTips("文本提示")
UIUtil.ShowTipsId(errorCode)
UIUtil.ShowTipsIdParams(textId, param1, param2)

=== 二次确认弹窗 ===
local param = {
    contentText = Localization:GetString(textId),
    btnNum = 2,
    text1 = GameDialogDefine.CONFIRM,
    text2 = GameDialogDefine.CANCEL,
    sureAction = function() end,
    cancelActon = function() end,
}
UIUtil.ShowSecondMessageWithParam(param)

=== 带"今日不再提示"的二次确认 ===
local todayType = TodayNoSecondConfirmType.XXX
if not DataCenter.SecondConfirmManager:GetTodayCanShowSecondConfirm(todayType) then
    self:DoAction()
    return
end
local param = {
    contentText = Localization:GetString(textId),
    btnNum = 2,
    text1 = GameDialogDefine.CONFIRM,
    text2 = GameDialogDefine.CANCEL,
    sureAction = function() self:DoAction() end,
    cancelActon = function() end,
    toggleAction = function(isNoShow)
        DataCenter.SecondConfirmManager:SetTodayNoShowSecondConfirm(todayType, isNoShow)
    end,
    toggleText = Localization:GetString(GameDialogDefine.TODAY_NO_SHOW),
}
UIUtil.ShowSecondMessageWithParam(param)

=== 规则说明弹窗 ===
UIUtil.ShowIntroId(titleId, subTitleId, introId)
- titleId: 标题多语言ID
- subTitleId: 副标题多语言ID，一般是100547
- introId: 介绍内容多语言ID

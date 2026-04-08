UI置灰控制 (UIGray)

当需要将UI按钮、图片或整个界面设置为置灰状态时使用。

核心 API
local UIGray = CS.UIGray
UIGray.SetGray(transform, isGray, canClick, recursive)

参数说明
- transform: 目标UI对象的变换组件
- isGray: true 置灰，false 恢复
- canClick (可选): 置灰后是否仍可点击，默认 true
- recursive (可选): 是否递归处理子节点，默认 false

常见用法
1. 基础置灰（不可点击）
UIGray.SetGray(self.btn_exchange.transform, true, false)

2. 仅视觉置灰（保持点击响应以弹出 Tips）
UIGray.SetGray(self.add_btn.transform, true, true)

3. 在 Refresh 中使用
local UIGray = CS.UIGray
function UIExampleView:Refresh()
    local isUnlock = self.data.level >= self.unlockLevel
    UIGray.SetGray(self.btn_enter.transform, not isUnlock, isUnlock)
end

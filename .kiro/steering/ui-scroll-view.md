UIScrollView列表

当预制体中使用了 UIScrollView 组件时，按此模板编写代码。

核心API:
SetTotalCount(count) - 设置数量
RefillCells() - 刷新
SetOnItemMoveIn(callback) - 进入回调
SetOnItemMoveOut(callback) - 离开回调

View示例:
self.scrollView = self:AddComponent(UIScrollView, "ScrollView")
self.scrollView:SetOnItemMoveIn(function(itemObj, index)
    itemObj.name = "ListItem_" .. tostring(index)
    local item = self.scrollView:AddComponent(UIExampleItem, itemObj)
    item:SetData(self.listData[index])
end)
self.scrollView:SetOnItemMoveOut(function(itemObj, index)
    if itemObj and itemObj.name then
        self.scrollView:RemoveComponent(itemObj.name, UIExampleItem)
    end
end)

刷新: self.scrollView:SetTotalCount(#data); self.scrollView:RefillCells()
清理(OnDestroy): self.scrollView:ClearCells(); self.scrollView:RemoveComponents(UIExampleItem)

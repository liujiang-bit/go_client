LoopListView2列表

当预制体中使用了 LoopListView2 组件时，按此模板编写代码。

初始化
self.loopListView = self:AddComponent(UILoopListView2, "LoopListView")
self.loopListView:InitListView(#self.dataList, function(listView, index)
    local item = listView:NewListViewItem("ItemPrefabName")
    local component = item:AddComponent(UIExampleItem, "")
    component:SetData(self.dataList[index])
    return item
end)

刷新:
self.loopListView:SetListItemCount(#self.dataList, false)
self.loopListView:RefreshAllShownItem()

如果Item是自适应高度，则需要在SetData后调用
CS.UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(self.rectTransform)

如果是可折叠的列表，则在size变化(折叠或者展开后)调用 :OnItemSizeChanged(itemIndex)

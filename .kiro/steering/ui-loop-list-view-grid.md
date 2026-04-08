Grid网格列表

当预制体中使用了 LoopListView2 且需要网格布局时，按此 Row+Item 模式编写代码。

常量: local ColMax = 3

数据组织:
function UIGridView:OrganizeDataToRows()
    self.rowData = {}
    local currentRow = {}
    for i, data in ipairs(self.originData) do
        table.insert(currentRow, data)
        if #currentRow >= ColMax or i == #self.originData then
            table.insert(self.rowData, {items = currentRow})
            currentRow = {}
        end
    end
end

初始化:
self:OrganizeDataToRows()
self.loopListView:InitListView(#self.rowData, function(listView, index)
    local item = listView:NewListViewItem("GridRow")
    local component = item:AddComponent(UIGridRow, "")
    component:SetRowData(self.rowData[index], ColMax)
    return item
end)

Row组件:
function UIGridRow:SetRowData(rowData, colMax)
    for i = 1, colMax do
        local item = self:AddComponent(UIGridItem, "listObj" .. i)
        if rowData.items[i] then
            item:SetData(rowData.items[i])
            item:SetActive(true)
        else
            item:SetActive(false)
        end
    end
end

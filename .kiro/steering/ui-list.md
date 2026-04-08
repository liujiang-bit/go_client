---
inclusion: fileMatch
fileMatchPattern: "**/UI/**/*.lua"
---

UIList动态列表

当预制体中需要动态创建子项列表（异步实例化Prefab）时，按此模板编写代码。
UIList 适用于数量不多、无需虚拟化回收的列表场景。大量数据请使用 UIScrollView 或 LoopListView2。

核心API:
SetData(prefabPath, componentType, dataList, params, initFunc) - 设置列表数据并异步创建
ClearList() - 清空列表
GetItemCount() - 获取当前项数量
GetItemByIndex(index) - 获取指定索引的项

View中使用:
```lua
local UIExampleItem = require "UI.UIExample.Component.UIExampleItem"

self.list = self:AddComponent(UIList, "Content")

-- 刷新列表
function UIExampleView:Refresh()
    local dataList = self:GetDataList()
    self.list:SetData(UIAssets.ExampleItemPrefab, UIExampleItem, dataList)
end
```

带额外参数:
```lua
-- params 会作为第二个参数传给 Item 的 ReInit
self.list:SetData(UIAssets.ExampleItemPrefab, UIExampleItem, dataList, extraParams)
```

使用自定义初始化方法（替代默认的 ReInit）:
```lua
self.list:SetData(UIAssets.ExampleItemPrefab, UIExampleItem, dataList, params, "CustomInit")
```

Item组件模板（必须实现 ReInit 方法）:
```lua
local UIExampleItem = BaseClass("UIExampleItem", UIBaseContainer)
local base = UIBaseContainer

function UIExampleItem:OnCreate()
    base.OnCreate(self)
    self.nameText = self:AddComponent(UITextMeshProUGUIEx, "Text_Name")
end

function UIExampleItem:OnDestroy()
    base.OnDestroy(self)
end

--- @param data table 单项数据（dataList中的元素）
--- @param params table|nil 额外参数（SetData的第4个参数）
--- @param index number 当前索引
function UIExampleItem:ReInit(data, params, index)
    if not data then return end
    self.nameText:SetText(data.name or "")
end

return UIExampleItem
```

嵌套UIList（列表中的列表）:
```lua
-- 外层Item中也可以使用UIList
function UIOuterItem:ReInit(classData)
    self.subList:SetData(UIAssets.SubItemPrefab, UISubItem, classData.items)
end
```

注意事项:
- UIList 在 OnCreate 时会自动清除容器下的所有子节点
- SetData 前会自动调用 ClearList，无需手动清理
- UIList 在 OnDestroy 时会自动清理，无需额外处理
- Item 组件默认调用 ReInit(data, params, index)，确保 Item 实现该方法
- 如需刷新列表，直接再次调用 SetData 即可

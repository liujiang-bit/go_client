策划配置表读取

当需要读取策划配置时使用。策划配置表是本地Excel表格转换的Lua数据，存储在 LuaDatatable 目录下。

新增配置表
新增配置表需要在 Assets/Main/LuaScripts/Global/EnumType.lua 的 TableName 枚举中添加：
TableName = {
    YourTableName = "your_table_name",
}

推荐方式：直接读取

获取单个值（最优先，性能最好）
local value = GetTableData(TableName.YourTable, id, "attr_name")
local num = GetTableNumber(TableName.YourTable, id, "attr_name")
local str = GetTableString(TableName.YourTable, id, "attr_name")

注意: 禁止在循环中解析字符串，如 string.split(GetTableData(...), ",") 这种写法。

获取一行数据（需要多个列时）
local lineData = LocalController:instance():getLine(TableName.YourTable, id)
if lineData then
    local value1 = lineData:getValue("attr1")
    local value2 = lineData:getIntValue("attr2")
    local value3 = lineData:getStrValue("attr3")
end

遍历整个表格
LocalController:instance():visitTable(TableName.YourTable, function(id, lineData)
    local value = lineData:getValue("attr")
end)

特殊情况：Template预处理方式
仅当表格需要复杂解析、一次性预处理时才使用。

Template类
local {Name}Template = BaseClass("{Name}Template")
function {Name}Template:InitData(row)
    if not row then return end
    self.id = tonumber(row:getValue("id")) or 0
    self.name = row:getStrValue("name") or ""
    local costStr = row:getStrValue("cost")
    if not string.IsNullOrEmpty(costStr) then
        self.costArray = string.string2array_i(costStr, '|')
    end
end

TemplateManager类
local {Name}TemplateManager = BaseClass("{Name}TemplateManager")
function {Name}TemplateManager:__init()
    self.allTemplate = {}
    self.isInit = false
end
function {Name}TemplateManager:InitAllTemplate()
    if self.isInit then return end
    self.isInit = true
    LocalController:instance():visitTable(TableName.{Name}, function(id, lineData)
        local template = {Name}Template.New()
        template:InitData(lineData)
        self.allTemplate[tonumber(id)] = template
    end)
end
function {Name}TemplateManager:GetTemplate(id)
    self:InitAllTemplate()
    return self.allTemplate[tonumber(id)]
end

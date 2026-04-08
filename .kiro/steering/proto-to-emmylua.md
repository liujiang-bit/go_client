---
inclusion: manual
---

# Proto to EmmyLua 类型声明转换

将 `Assets/Data/ProtoFile/` 下的 protobuf `.proto` 文件转换为 EmmyLua 类型注解，输出到 `Assets/Main/LuaScripts/Util/ProtobufToLua.lua`。

## 触发时机

用户要求将 proto 文件转换为 EmmyLua 类型声明，或要求更新 ProtobufToLua.lua。

## 输入

- Proto 源目录：`Assets/Data/ProtoFile/*.proto`
- 输出文件：`Assets/Main/LuaScripts/Util/ProtobufToLua.lua`
- 参考格式：`UIDragonSelectMemberAllinOneView.lua` 中的 `FilterCondition` 类型声明

## 转换规则

### 1. message → `---@class`

每个 `message MessageName` 转换为：

```lua
---@class MessageName
```

### 2. 字段 → `---@field`

每个字段按以下格式输出：

```lua
---@field fieldName luaType 注释
```

### 3. 类型映射

| Proto 类型 | Lua/EmmyLua 类型 |
|-----------|-----------------|
| `int32`, `int64`, `uint32`, `uint64`, `float`, `double` | `number` |
| `string` | `string` |
| `bool` | `boolean` |
| `bytes` | `string` |
| `repeated T` | `T[]` |
| `google.protobuf.Int32Value` | `Int32Value` （wrapper，Lua 中为 `{value=number}`） |
| `google.protobuf.Int64Value` | `Int64Value` |
| `google.protobuf.StringValue` | `StringValue` |
| `google.protobuf.BoolValue` | `BoolValue` |
| `google.protobuf.FloatValue` | `FloatValue` |
| `google.protobuf.DoubleValue` | `DoubleValue` |
| 其他 message 类型 | 直接使用 message 名称 |

### 4. Wrapper 类型声明

在文件头部声明所有用到的 google.protobuf wrapper 类型：

```lua
---@class Int32Value
---@field value number

---@class Int64Value
---@field value number

---@class StringValue
---@field value string

---@class BoolValue
---@field value boolean
```

只声明 proto 文件中实际引用到的 wrapper 类型。

### 5. oneof 处理

`oneof` 中的每个字段都作为可选字段输出（EmmyLua 不支持 union，全部列出即可）。

### 6. enum 处理

`enum EnumName` 转换为注释块，列出所有枚举值：

```lua
--- EnumName 枚举
--- VALUE_A = 0
--- VALUE_B = 1
```

### 7. 注释保留

proto 中的行尾注释（`// 注释`）保留为 `---@field` 的尾部注释。

## 输出格式

```lua
--- ============================================================
--- 本文件由 Proto 定义自动生成，请勿手动修改
--- 源文件目录：Assets/Data/ProtoFile/
--- ============================================================

-- ============================================================
-- google.protobuf Wrapper 类型
-- ============================================================

---@class Int32Value
---@field value number

-- ============================================================
-- FileName.proto
-- ============================================================

---@class MessageName
---@field fieldA number  字段A注释
---@field fieldB string  字段B注释
---@field items  ItemType[]  列表字段

---@class ItemType
---@field id    number  ID
---@field name  string  名称
```

## 执行步骤

1. 读取 `Assets/Data/ProtoFile/` 下所有 `.proto` 文件（排除 `.meta`）
2. 解析每个 proto 文件中的 `message`、`enum`、`oneof` 定义
3. 收集所有引用到的 wrapper 类型
4. 按上述规则生成 EmmyLua 注解
5. 按 proto 文件名分组，写入 `Assets/Main/LuaScripts/Util/ProtobufToLua.lua`
6. 同一 proto 文件内的 message 按原始定义顺序排列

## 注意事项

- 若 `ProtobufToLua.lua` 已存在，全量覆盖重新生成
- `Wrappers.proto` 本身不需要单独输出为一个分组，其类型作为公共 wrapper 声明在文件头部
- `repeated` 嵌套 message 类型使用 `MessageName[]` 格式
- `map<K,V>` 类型（如果有）转换为 `table<K,V>`
- 保持字段顺序与 proto 定义一致

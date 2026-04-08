---
inclusion: fileMatch
fileMatchPattern: '**/DataCenter/**/*.lua'
---

DataCenter Manager 规范

当需要新建或迁移 DataCenter Manager 时使用。

继承与基类
- 所有 DataCenter Manager 必须继承 ListenerHandler（路径: Assets/Main/LuaScripts/Global/ListenerHandler.lua）
- 禁止继承 Singleton，DataCenter 框架自行管理实例生命周期
- 禁止自己手动实现事件监听闭包管理（手动创建 self._xxxSignal 包装回调）

类定义模板
```lua
---@class MyManager : ListenerHandler
local MyManager = BaseClass("MyManager", ListenerHandler)
```

事件监听
使用基类 ListenerHandler 提供的 self:AddListener / self:RemoveListener：
```lua
function MyManager:Startup()
    self:AddListener(EventId.SomeEvent, self.OnSomeEvent)
end
```
基类 __delete 会自动调用 RemoveAllListeners 清理所有监听，无需手动管理。

禁止写法：
```lua
-- 禁止：手动创建闭包管理事件
self._signal = function(data) self:OnEvent(data) end
EventManager:GetInstance():AddListener(EventId.XXX, self._signal)
```

函数定义风格
使用 function M:method() 风格，禁止 local function + 尾部赋值的方式：
```lua
-- 正确
function MyManager:OnSomeEvent(data)
end

-- 禁止
local function OnSomeEvent(self, data)
end
MyManager.OnSomeEvent = OnSomeEvent
```

注册到 DataCenter
在 Assets/Main/LuaScripts/DataCenter/DataCenter.lua 的 Managers 表中添加：
```lua
MyManager = "DataCenter.MyModule.MyManager",
```

生命周期
- __init: 先调用 ListenerHandler.__init(self)，再初始化数据字段
- Startup: 注册事件监听（在 LuaEntry 中调用）
- __delete: 先清理业务数据，再调用 ListenerHandler.__delete(self)（基类清理监听）

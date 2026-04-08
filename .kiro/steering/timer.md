定时器与时间

一、通用定时器
适用于非 UI 逻辑或需要跨界面存在的长生命周期任务。必须手动管理生命周期。

获取定时器
local timer = TimerManager:GetInstance():GetTimer(interval, callback, obj, one_shot, use_frame, unscaled)
timer:Start()

延迟调用:
local timer = TimerManager:GetInstance():DelayInvoke(callback, delayTime, userdata)

生命周期管理:
if self.timer then
    self.timer:Stop()
    self.timer = nil
end

二、UI 组件封装定时器(UIBaseContainer) [推荐]
继承自 UIBaseContainer 的类应优先使用封装方法。框架会在 OnDestroy 时自动清理。

一次性延迟
self:DelayInvoke(function() end, 0.5)

循环/托管定时器
self:AddTimerAction(self.OnTimerTick, 1.0, true)

三、时间获取与格式化(UITimeManager)
local curTime = UITimeManager:GetInstance():GetServerTime()    -- 毫秒
local curSec  = UITimeManager:GetInstance():GetServerSeconds() -- 秒
注意：严禁使用 os.time()

常用格式化
1. MilliSecondToFmtString: 输入毫秒，输出 1d 05:30:10 或 05:30:10
2. GetCountdownFmtString: 输入秒，动态格式（>1天: 01d 05h; <1天: 05h 30m; <1小时: 30m 15s）
3. SecondToFmtStringWithoutDay: 输入秒，始终输出 HH:mm:ss

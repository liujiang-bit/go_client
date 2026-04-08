# Errors Log

## 2026-03-17 | strReplace oldStr 不精确导致代码损坏

- **场景**: 批量在多处 `BattleType.PARADE_FLOAT` 旁添加 `BattleType.PARADE_FLOAT_PRIEST`
- **错误**: 对 `MailShowTemplateManager.lua` 第二处替换时，`oldStr` 包含了不够精确的上下文，导致匹配到错误的范围，把中间几行代码（`PARADE_FLOAT)` 闭合括号、`and seasonId > 1`、`then`、`local iconName = ...`）全部删除。
- **根因**: 同一文件中有两处几乎相同的代码块（icon_win 和 icon_lose），第一次替换成功后，第二次的 `oldStr` 没有包含足够的区分上下文（如 `icon_lose`），且 `newStr` 写得不完整。
- **修复**: 用第二次 strReplace 补回了被删除的行。
- **教训**: 同一文件中有多处相似代码时，`strReplace` 的 `oldStr` 必须包含足够多的上下文来唯一定位目标位置，且 `newStr` 必须包含完整的替换内容。建议先读取精确行号再做替换，或在 oldStr 中包含前后更多不同的行作为锚点。
- **严重程度**: medium
- **状态**: resolved

## 2026-03-17 | PowerShell Rename-Item 对含 `@` 的路径报错

- **场景**: 尝试用 `Rename-Item "Packages/com.unity.ugui@1.0.0" "Packages/com.unity.ugui"` 重命名目录
- **错误**: `Rename-Item : 无法重命名指定的目标，因为该目标表示路径或设备名称。`
- **根因**: PowerShell 的 `Rename-Item` 第二个参数 `-NewName` 只接受新名称（不含路径），不接受完整目标路径。传入 `"Packages/com.unity.ugui"` 被解析为路径而非名称。
- **修复**: 改用 `Move-Item "Packages/com.unity.ugui@1.0.0" "Packages/com.unity.ugui"` 成功。
- **教训**: PowerShell `Rename-Item` 的第二个参数是纯名称（如 `"com.unity.ugui"`），不能包含路径分隔符。如果需要同时改名和移动，用 `Move-Item`。
- **严重程度**: low
- **状态**: resolved

## 2026-03-17 | PowerShell Set-Content 破坏 UTF-8 文件中的中文字符

- **场景**: 用 `(Get-Content file) -replace ... | Set-Content file` 批量替换 C# 文件中的 `Debug.Log` 为 `Debug.LogError`
- **错误**: 文件中的中文注释被破坏（如 `日志` 变成 `日�?`），导致注释行与下一行代码合并，编译报错 `CS0103: The name 'debugLog' does not exist in the current context`
- **根因**: PowerShell 的 `Set-Content` 默认使用系统编码（Windows 中文系统为 GBK/GB2312），而源文件是 UTF-8 编码。中文字符在编码转换中被截断/损坏，注释行的换行符丢失。
- **修复**: 用 `strReplace` 将被破坏的中文注释替换为英文注释。
- **教训**: 在包含非 ASCII 字符（中文等）的文件上，避免使用 PowerShell 的 `Get-Content | Set-Content` 管道。应改用 `strReplace` 工具逐个替换，或在 PowerShell 中显式指定编码：`Set-Content -Encoding UTF8`。
- **严重程度**: medium
- **状态**: resolved

## 2026-03-18 | strReplace oldStr 跨越多个代码块导致中间内容被吞掉

- **场景**: 修改 UIWorldBuffView.lua 中 endTimeFunc 分支的条件判断，将 `> 0` 改为 `>= 0`
- **错误**: oldStr 只包含了要修改的那一行和紧接的几行，但 newStr 只写了修改后的那一行，没有包含 oldStr 中其余被匹配到的行（`self.param.endTime = endTime` / `self:AddTimerAction(...)` / `elseif` 分支等）。替换后这些行全部丢失，代码结构被破坏（缩进错乱、分支缺失）。
- **根因**: 想只改一行条件，但 oldStr 为了唯一匹配而包含了多行上下文，newStr 却没有完整保留这些上下文行。
- **修复**: 用第二次 strReplace 传入完整的代码块重新修复。
- **教训**: 当只需修改一行时，oldStr 应尽量精简到只包含该行及最少的唯一上下文；如果 oldStr 不得不包含多行，newStr 必须完整包含所有这些行（只修改目标行，其余原样保留）。替换后应立即 readFile 验证结果。
- **严重程度**: medium
- **状态**: resolved

## 2026-03-19 | executePwsh cwd 参数在含中文路径的工作区中不生效

- **场景**: 在 `go_server` 子目录中执行 `go build`，通过 `cwd: "go_server"` 参数指定工作目录
- **错误**: `go: go.mod file not found in current directory or any parent directory`。shell 实际仍在工作区根目录执行，没有切换到 `go_server/`。
- **根因**: 工作区路径包含中文字符（`D:\BaiduNetdiskDownload\wgjx\rok万国觉醒源码\ROK2\ROK2\ROK\Client`），`executePwsh` 的 `cwd` 参数在这种路径下可能无法正确拼接或切换目录。
- **修复**: 不使用 `cwd` 参数，改为在命令中直接引用子目录路径，如 `go build -C go_server ./cmd/login` 或使用 PowerShell 的 `Push-Location`/`Pop-Location`。
- **教训**: 当工作区路径含非 ASCII 字符时，`executePwsh` 的 `cwd` 参数可能不可靠。应在命令本身中处理目录切换（如 Go 1.21+ 的 `-C` 标志，或 `Push-Location`）。
- **严重程度**: medium
- **状态**: resolved

## 2026-03-25 | strReplace 更新 tasks.md 状态时因子代理已修改而匹配失败（重复出现）

- **场景**: 委托子代理（general-task-execution）执行 spec 任务后，尝试用 strReplace 更新 tasks.md 中的 checkbox 状态（`[ ]` → `[x]`）
- **错误**: `String '...' not found in .kiro/specs/lua-to-go-server/tasks.md`，多次出现（任务 3、5、6 的状态更新）
- **根因**: 子代理在执行任务时已经自动将 tasks.md 中对应任务的 checkbox 从 `[ ]` 改为 `[x]` 或 `[-]`。当主代理随后尝试用 strReplace 做同样的更新时，oldStr 中的 `[ ]` 已不存在于文件中，导致匹配失败。
- **修复**: 每次都需要先用 grepSearch 确认文件中的实际文本，再决定是否需要 strReplace。
- **教训**: 委托子代理执行 spec 任务后，不要假设 tasks.md 的 checkbox 状态未变。应先用 grepSearch 检查实际状态，如果子代理已更新则跳过 strReplace。更好的做法是：信任子代理的状态更新，只在子代理未更新时才手动修改。
- **严重程度**: low（不影响功能，只是浪费一次工具调用）
- **状态**: resolved
- **出现次数**: 4+（任务 2.8、3、5、6.1 的状态更新均遇到）

## 2026-04-01 | invokeSubAgent 缺少 prompt 参数导致调用失败

- **场景**: 批量执行任务 25/28/29/30 时调用 invokeSubAgent
- **错误**: `Provided input does not match the required schema: Expected string, received null` — prompt 参数为 null
- **根因**: 在准备 readFile 调用的同时并行调用了 invokeSubAgent，但忘记填写 prompt 参数
- **修复**: 重新调用 invokeSubAgent 并提供完整的 prompt
- **教训**: invokeSubAgent 的 prompt 是必填参数，不能在并行调用中遗漏。当同时准备多个工具调用时，确保每个调用的必填参数都已填写。
- **严重程度**: low
- **状态**: resolved

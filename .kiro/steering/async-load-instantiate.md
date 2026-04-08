资源加载与Prefab实例化

当需要异步加载资源或实例化Prefab时使用。

一、Prefab 实例化(ResourceManager:InstantiateAsync)

self.loadRequest = ResourceManager:InstantiateAsync(prefabPath)
self.loadRequest:completed('+', function(request)
    if request.isError then return end
    local go = request.gameObject
    go.transform:SetParent(self.transform)
    self:OnResourceLoaded(go)
end)

清理：
if self.loadRequest then
    self.loadRequest:Destroy()
    self.loadRequest = nil
end

UIBaseComponent 封装（推荐）
local request = self:GameObjectInstantiateAsync(prefabPath, function(request)
    if request.isError then return end
    local go = request.gameObject
    go:SetActive(true)
end, parentTransform)

二、Asset 直接加载 (Resource:LoadAssetAsync)
用于加载 Sprite、Texture、Material、TextAsset 等原始资源，无需实例化。

local Resource = CS.GameEntry.Resource
local req = Resource:LoadAssetAsync(iconPath, typeof(CS.UnityEngine.Sprite))
req:completed('+', function()
    if not req.isError and req.asset then
        image.sprite = req.asset
    end
end)
self.textureRequest = req

清理：
if self.textureRequest then
    self.textureRequest:Release()
    self.textureRequest = nil
end

在 OnDestroy 前必须 Release，否则引用计数无法释放。

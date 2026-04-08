//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      资源加载服务类，提供文件的同步异步加载，提供Unity AB的同步异步加载和资源的声明周期。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Plugins.XAsset;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement;

namespace Skyunion
{
    class AddressAssetService : BaseAssetService
    {
        public override void Init()
        {
            CoreUtils.logService.WaitInitAsync(()=>
            {
#if !UNITY_EDITOR
                string runtimeSettingsFilename = VersionUtil.HotfixAddressablesPath + "/settings.json";
                CoreUtils.logService.Info(runtimeSettingsFilename, Color.green);
                if (File.Exists(runtimeSettingsFilename))
                {
                    PlayerPrefs.SetString(Addressables.kAddressablesRuntimeDataPath, runtimeSettingsFilename);
                }
                else
                {
                    // 如果找不到对应的热更新目录就删除掉Runtime， 前提是原来设定的Runtime必须是之前设置的目录
                    var runtimePath = PlayerPrefs.GetString(Addressables.kAddressablesRuntimeDataPath, "");
                    if (runtimePath.Contains(Application.persistentDataPath))
                    {
                        PlayerPrefs.DeleteKey(Addressables.kAddressablesRuntimeDataPath);
                    }
                }
#endif
                Debug.Log("start init Addressables"+Time.realtimeSinceStartup);
                Addressables.InitializeAsync(true).Completed += AddressAssetService_Completed;
            });
        }

        private void AddressAssetService_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.AddressableAssets.ResourceLocators.IResourceLocator> obj)
        {
            Debug.Log("end init Addressables"+Time.realtimeSinceStartup);
            mGameObject = new GameObject("AddressAssetService");
            mGameObject.AddComponent<ObjectPoolHandler>();
            //GameObject.DontDestroyOnLoad(mGameObject);
            ResourceManager.ExceptionHandler = null;
            OnInitialized();
        }

        public override IAsset LoadAssetAsync<T>(string assetName, Action<IAsset> completed)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                Debug.LogErrorFormat("LoadAssetAsync param assetName is null");
                return null;
            }
            var handdler = Addressables.LoadAssetAsync<T>(assetName);
            if(handdler.IsDone && handdler.Result == null)
            {
                handdler = Addressables.LoadAssetAsync<T>("ErrorPrefab");
            }
            var newAsset = new AddressAsset<T>(handdler, assetName, m_unload_unused_asset_time);
            handdler.Completed += (AsyncOperationHandle<T> obj)=>
            {

                if (mIsInitialized==false)
                {
                    return;
                }
                
                // 由于特效Animator之类的需要在Update里面初始化，不然特效会出现第一帧错误
                if (typeof(T) == typeof(GameObject))
                {
                    LoadCallbackInUpdate(completed, newAsset);
                }
                else
                {
                    completed?.Invoke(newAsset);
                }
            };
            return newAsset;
        }
        public override IAsset LoadSceneAssetAsync(string assetName, bool addictive, Action<IAsset> completed)
        {
            var handler = Addressables.LoadSceneAsync(assetName, addictive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            var newAsset = new AddressSceneAsset(handler, assetName, m_unload_unused_asset_time);
            handler.Completed += (AsyncOperationHandle<SceneInstance> obj) =>
            {
                completed?.Invoke(newAsset);
            };
            return newAsset;
        }
        public override void Instantiate(string assetName, Action<GameObject> completed)
        {
            // 已经知道是 回收对象了。直接调用回收池
            if (mObjectHasPool.ContainsKey(assetName))
            {
                ObjectPoolMgr.GetInstance().GetPool(assetName).Spawn(assetName, completed);
                return;
            }
            if (assetName == null || assetName.Equals(string.Empty))
            {
                Debug.LogError("Instantiate assetName is null");
                return;
            }

            LoadAssetAsync<GameObject>(assetName, (IAsset asset) =>
            {
                GameObject gameObject;
                if (asset.asset() == null)
                {
                    Debug.LogError("no find "+assetName);
                    gameObject = new GameObject(assetName);
                    completed?.Invoke(gameObject);
                    return;
                }
                else
                {
                    gameObject = asset.asset() as GameObject;
                }
                ObjectPoolItem component = gameObject.GetComponent<ObjectPoolItem>();
                if (component != null)
                {
                    if(component.poolName == null || component.poolName.Equals(string.Empty))
                    {
                        component.poolName = assetName;
                    }
                    ObjectPoolMgr.GetInstance().GetPool(component.poolName).Spawn(component.poolName, completed);
                }
                else
                {
                    var @object = CoreUtils.assetService.Instantiate(gameObject);
                    asset.Attack(@object);
                    try
                    {
                        completed?.Invoke(@object);
                    }
                    catch(Exception ex)
                    {
                        Debug.LogError($"{assetName} Callback Error: {ex.ToString()}");
                    }
                }
            }, null);
        }
        public override void Destroy(GameObject gameObject)
        {
            // 容错
            if (gameObject == null)
                return;

            ObjectPoolItem component = gameObject.GetComponent<ObjectPoolItem>();
            if (component != null && component.poolName != null)
            {
                //Debug.Log($"{gameObject.name},{gameObject.GetInstanceID()}");
                ObjectPoolMgr.GetInstance().GetPool(component.poolName).DeSpawn(gameObject, component.poolName);
                if (!mObjectHasPool.ContainsKey(component.poolName))
                {
                    mObjectHasPool.Add(component.poolName, true);
                }
            }
            else
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
        public override void Destroy(GameObject gameObject, float fadeTime)
        {
            // 容错
            if (gameObject == null)
                return;
            // 临时不加延迟
            Destroy(gameObject);
        }

        public override void UnloadUnusedAssets()
        {
            ObjectPoolMgr.GetInstance().TryCleanPool();

            for (int i = 0; i < mAssets.Count; i++)
            {
                int status = mAssets[i].Process();
                if (status == 0)
                {
                    break;
                }
                else if (mAssets[i].Process() == 2)
                {
                    continue;
                }
                else if (status == 1)
                {
                    mAssets.RemoveAt(i);
                    i--;
                }
            }

            m_unload_unused_asset_timer = 0.0f;
            GC.Collect();
            Resources.UnloadUnusedAssets();
        }
    }
}

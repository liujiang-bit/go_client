//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      资源实现类，对 XAsset的 Asset进行绑定。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using log4net;
using log4net.Appender;
using log4net.Config;
using UnityEngine;
using UnityEngine.Networking;
using log4net.Util;
using System.Collections.Generic;
using Plugins.XAsset;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Skyunion
{
    class AddressAsset<T> : IAsset where T :UnityEngine.Object
    {
        private AsyncOperationHandle<T> mHanddler;
        private Queue<UnityEngine.Object> mAttackObjects = new Queue<UnityEngine.Object>();
        int mRefCount = 0;
        private string mAssetName;
        private float fReleaseTime = 0.0f;
        public AddressAsset(AsyncOperationHandle<T> handdler, string name, float fTime = 30.0f)
        {
            mHanddler = handdler;
            mAssetName = name;
            fReleaseTime = Time.realtimeSinceStartup + fTime;
        }

        public string assetName()
        {
            return mAssetName;
        }
        public UnityEngine.Object asset()
        {
            return mHanddler.Result;
        }
        public Scene scene()
        {
            return default(Scene);
        }
        public void UnloadScene()
        {
        }

        public void Attack(UnityEngine.Object obj)
        {
            mAttackObjects.Enqueue(obj);
            Retain();
        }

        public void Release()
        {
            mRefCount--;
            if (mRefCount <= 0)
            {
                mRefCount = 0;
                //CoreUtils.logService.Info($"Release Handdle:{mHanddler.Result.name}", Color.green);
                Addressables.Release(mHanddler);
                mAttackObjects.Clear();
            }
        }

        public void Retain()
        {
            mRefCount++;
        }


        public int Process()
        {
            if (Time.realtimeSinceStartup < fReleaseTime)
                return 0;

            // 目前先处理，只有绑定的才会处理自动销毁
            if (mAttackObjects.Count > 0)
            {
                if (mAttackObjects.Peek() == null)
                {
                    mAttackObjects.Dequeue();
                    Release();
                }
                if (mRefCount == 0)
                    return 1;

                return 2;
            }
            else
            {
                return 1;
            }
        }
    }
    class AddressSceneAsset : IAsset
    {
        private AsyncOperationHandle<SceneInstance> mHanddler;
        private Queue<UnityEngine.Object> mAttackObjects = new Queue<UnityEngine.Object>();
        int mRefCount = 0;
        private string mAssetName;
        private float fReleaseTime = 0.0f;
        public AddressSceneAsset(AsyncOperationHandle<SceneInstance> handdler, string name, float fTime)
        {
            mHanddler = handdler;
            mAssetName = name;
            fReleaseTime = Time.realtimeSinceStartup + fTime;
        }

        public string assetName()
        {
            return mAssetName;
        }
        public Scene scene()
        {
            return mHanddler.Result.Scene;
        }
        public void UnloadScene()
        {
            Addressables.UnloadSceneAsync(mHanddler);
        }
        public UnityEngine.Object asset()
        {
            return null;
        }

        public void Attack(UnityEngine.Object obj)
        {
            mAttackObjects.Enqueue(obj);
            Retain();
        }

        public void Release()
        {
            mRefCount--;
            if(mRefCount <= 0)
            {
                mRefCount = 0;
                Addressables.Release(mHanddler);
                mAttackObjects.Clear();
            }
        }

        public void Retain()
        {
            mRefCount++;
        }

        public int RefCount()
        {
            return mRefCount;
        }

        public int Process()
        {
            if (Time.realtimeSinceStartup < fReleaseTime)
                return 0;

            // 目前先处理，只有绑定的才会处理自动销毁
            if (mAttackObjects.Count > 0)
            {
                if (mAttackObjects.Peek() == null)
                {
                    mAttackObjects.Dequeue();
                    Release();
                }
                if (mRefCount == 0)
                    return 1;

                return 2;
            }
            else
            {
                return 1;
            }
        }
    }
}

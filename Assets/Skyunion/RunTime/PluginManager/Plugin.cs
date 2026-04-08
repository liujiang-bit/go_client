//
// GameApp.cs
// Create:
//      2019-10-29
// Description:
//      插件实现基类， 管理插件所支配的模块。
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Skyunion
{
    public class Plugin : Module, IPlugin
    {
        protected string mName;
        protected Dictionary<string, IModule> mModules = new Dictionary<string, IModule>();
        protected List<IModule> mModuleList = new List<IModule>();
        public void AddModule(string name, IModule module)
        {
            mPluginManager.AddModule(name, module);
            mModules.Add(name, module);
            mModuleList.Add(module);
        }
        public void AddModule<T1>(IModule module)
        {
            string strName = typeof(T1).ToString();
            mPluginManager.AddModule(strName, module);
            mModules.Add(strName, module);
            mModuleList.Add(module);
        }
        public void RemoveModule<T1>()
        {
            string strName = typeof(T1).ToString();
            IModule module;
            if (mModules.TryGetValue(strName, out module))
            {
                mPluginManager.RemoveModule(strName);
                mModules.Remove(strName);
                mModuleList.Remove(module);
            }
        }
        public Plugin(string name)
        {
            mName = name;
        }

        #region 实现 IModule
        public override void BeforeInit()
        {
            for(int i = 0; i < mModuleList.Count; i++)
            {
                var module = mModuleList[i];
                if (module != null)
                {
                    module.BeforeInit();
                }
            }
        }

        public override void Init()
        {
            if(mModules.Count == 0)
            {
                OnInitialized();
                return;
            }
            for (int i = 0; i < mModuleList.Count; i++)
            {
                var module = mModuleList[i];
                if (module != null)
                {
                    module.Init();
                    module.WaitInitAsync(() =>
                    {
                        mLoadedCount++;
                        if (mLoadedCount == mModules.Count)
                        {
                            OnInitialized();
                        }
                    });
                }
            }
        }

        public override void AfterInit()
        {
            for (int i = 0; i < mModuleList.Count; i++)
            {
                var module = mModuleList[i];
                if (module != null)
                {
                    module.AfterInit();
                }
            }
        }

        public override void Update()
        {
            for (int i = 0; i < mModuleList.Count; i++)
            {
                var module = mModuleList[i];
                if (module != null && module.Initialized())
                {
					module.Update();
                }
            }
        }

        public override void LateUpdate()
        {
            for (int i = 0; i < mModuleList.Count; i++)
            {
                var module = mModuleList[i];
                if (module != null && module.Initialized())
                {
                    module.LateUpdate();
                }
            }
        }

        public override void BeforeShut()
        {
            for (int i = 0; i < mModuleList.Count; i++)
            {
                var module = mModuleList[i];
                if (module != null)
                {
                    module.BeforeShut();
                }
            }
        }

        public override void Shut()
        {
            for (int i = 0; i < mModuleList.Count; i++)
            {
                var module = mModuleList[i];
                if (module != null)
                {
                    module.Shut();
                }
            }
        }
        #endregion 实现 IModule

        #region 实现 IPlugin
        public virtual string GetPluginName()
        {
            return mName;
        }

        public virtual void OnAddModule()
        {
        }

        public void Install()
        {
            OnAddModule();
        }
        public void Uninstall()
        {
            foreach (var name in mModules.Keys)
            {
                mPluginManager.RemoveModule(name);
            }
            mModules.Clear();
            mModuleList.Clear();
        }
        #endregion 实现 IPlugin
    };
}
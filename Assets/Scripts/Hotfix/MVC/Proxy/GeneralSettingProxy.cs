// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, 28 October 2020
// Update Time         :    Wednesday, 28 October 2020
// Class Description   :    GeneralSettingProxy
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Skyunion;
using System;

namespace Game {
    public class GeneralSettingProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "GeneralSettingProxy";


        private Dictionary<int, bool> m_generalSettingDIc = new Dictionary<int, bool>();
        private Dictionary<int, SettingPersonalityDefine> m_settingPersonalityDefineDic = new Dictionary<int, SettingPersonalityDefine>();
        private Dictionary<int,long> m_needRefreshDic = new Dictionary<int,long>();//需要刷新的个人设置
        private List<SettingPersonalityDefine> m_displayList = new List<SettingPersonalityDefine>();//需要显示的个人设置

        private Timer timer = null;
        #endregion

        // Use this for initialization
        public GeneralSettingProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" GeneralSettingProxy register");   
        }


        public override void OnRemove()
        {
            if (timer != null)
            {
                timer.Cancel();
                timer = null;
            }
            Debug.Log(" GeneralSettingProxy remove");
        }
        public void Init()
        {
            List<SettingPersonalityDefine> settingPersonalityDefines = CoreUtils.dataService.QueryRecords<SettingPersonalityDefine>();
            settingPersonalityDefines.ForEach((settingPersonalityDefine) => {
                m_settingPersonalityDefineDic[settingPersonalityDefine.ID] = settingPersonalityDefine;
                LoadGeneralSetting(settingPersonalityDefine);
            });
            SortGeneralSetting();

            timer = Timer.Register(5, () =>
            {
                RefreshgeneralSetting();
            }, null, true);
        }

        public void SortGeneralSetting()
        {
            m_displayList.Sort((SettingPersonalityDefine x, SettingPersonalityDefine y) => {
                return x.order.CompareTo(y.order);
            });
        }

        public void OpenGeneralSettingItem(int id)
        {
            m_generalSettingDIc[id] = true;
            string key = string.Format("GeneralSetting_{0}", id);
            PlayerPrefs.SetString(key, string.Format("{0}|{1}", ServerTimeModule.Instance.GetServerTime(), 1));

            SettingPersonalityDefine settingPersonalityDefine = CoreUtils.dataService.QueryRecord<SettingPersonalityDefine>(id);
            if (settingPersonalityDefine.open != 1 && settingPersonalityDefine.resetTiem > 0)
            {
                m_needRefreshDic[id] = ServerTimeModule.Instance.GetServerTime();
            }
        }

        public void CloseGeneralSettingItem(int id)
        {
            m_generalSettingDIc[id] = false;
            string key = string.Format("GeneralSetting_{0}", id);
            PlayerPrefs.SetString(key, string.Format("{0}|{1}", ServerTimeModule.Instance.GetServerTime(), 0));

            SettingPersonalityDefine settingPersonalityDefine = CoreUtils.dataService.QueryRecord<SettingPersonalityDefine>(id);
            if (settingPersonalityDefine.open != 0&& settingPersonalityDefine.resetTiem>0)
            {
                    m_needRefreshDic[id] = ServerTimeModule.Instance.GetServerTime();
            }
        }

        public bool GetGeneralSettingByID(int id)
        {
            bool open = false;
            if (m_generalSettingDIc.TryGetValue(id, out open))
            {

            }
            else
            {
                Debug.LogError("ERROR not find " + id);
            }
            return open;
        }
        public SettingPersonalityDefine GetSettingPersonalityDefine(int id)
        {
            SettingPersonalityDefine settingPersonalityDefine = null;
            m_settingPersonalityDefineDic.TryGetValue(id, out settingPersonalityDefine);

            return settingPersonalityDefine;
        }
        public List<GeneralSettingItemData> GetItemDatas()
        {
            List<GeneralSettingItemData> generalSettingItemDatas = new List<GeneralSettingItemData>();
            {
                GeneralSettingItemData generalSettingItemData = new GeneralSettingItemData(1);
                generalSettingItemDatas.Add(generalSettingItemData);
            }
            {
                GeneralSettingItemData generalSettingItemData = new GeneralSettingItemData(2);
                generalSettingItemDatas.Add(generalSettingItemData);
            }
            {
                m_displayList.ForEach((settingPersonalityDefine) =>
                {
                    GeneralSettingItemData generalSettingItemData = new GeneralSettingItemData(3, settingPersonalityDefine); 
                    generalSettingItemDatas.Add(generalSettingItemData);
                });
            }
            return generalSettingItemDatas;
        }

        public void RefreshgeneralSetting()
        {
            foreach (var temp in m_needRefreshDic )
            {
                SettingPersonalityDefine  settingPersonalityDefine = null;
                if (m_settingPersonalityDefineDic.TryGetValue(temp.Key, out settingPersonalityDefine))
                {
                    int resetTime = settingPersonalityDefine.resetTiem;
                  //  int resetTime =100;
                    if (ServerTimeModule.Instance.GetServerTime() - temp.Value > resetTime)
                    {
                        m_generalSettingDIc[temp.Key] = settingPersonalityDefine.open == 1;
                    }
                }
            }
        }
        private void LoadGeneralSetting(SettingPersonalityDefine settingPersonalityDefine)
        {
            bool save = false;
            string key = string.Format("GeneralSetting_{0}", settingPersonalityDefine.ID);
            string value = PlayerPrefs.GetString(key,"");
            string[] spit = value.Split('|');
            long saveServerTime = 0;
            int saveOpne = 0;
            bool open ;
            bool defaultOpen = settingPersonalityDefine.open == 1;
            if (spit.Length == 2)
            {
                if (long.TryParse(spit[0], out saveServerTime) && int.TryParse(spit[1], out saveOpne))
                {
                    save = true;
                }
            }
            if (save)
            {
                if (settingPersonalityDefine.resetTiem > 0)
                {
                    if (ServerTimeModule.Instance.GetServerTime() - saveServerTime > settingPersonalityDefine.resetTiem)
                    {
                        open = defaultOpen;
                    }
                    else
                    {
                        open = saveOpne == 1;
                        if (open != defaultOpen)
                        {
                            m_needRefreshDic[settingPersonalityDefine.ID] = saveServerTime;
                        }
                    }
                }
                else
                {
                    open = saveOpne == 1;
                }
            }
            else
            {
                open = defaultOpen;
            }
                m_generalSettingDIc[settingPersonalityDefine.ID] = open;
            if (settingPersonalityDefine.display == 1)
            {
                m_displayList.Add(settingPersonalityDefine);
            }
        }
    }
}
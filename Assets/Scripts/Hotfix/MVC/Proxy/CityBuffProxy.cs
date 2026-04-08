// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月9日
// Update Time         :    2020年1月9日
// Class Description   :    TaskProxy
// Copyright IGG All rights reserved.
// ===============================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;
using Client;
using Data;
using SprotoTemp;
using System;
using System.Linq;
using PureMVC.Interfaces;
using SprotoType;
using System.Text;
using Hotfix;

namespace Game
{
    public class CityBuffGroupData
    {
        public CityBuffGroupDefine groupDefine;
        public CityBuff cityBuff;
        public CityBuffDefine cityBuffDefine;
    }

    public class CityBuffProxy : GameProxy
    {

        #region Member
        public const string ProxyNAME = "CityManagerProxy";

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;
        private BagProxy m_bagProxy;

        private List<CityBuffDefine> m_cityBuffList = new List<CityBuffDefine>();
        private List<CityBuffGroupDefine> m_cityBuffGroupList = new List<CityBuffGroupDefine>();
        private List<CityBuffSeriesDefine> m_cityBuffSeriesList = new List<CityBuffSeriesDefine>();
        private Dictionary<int, CityBuffSeriesDefine> m_cityBuffSeriesDic = new Dictionary<int, CityBuffSeriesDefine>();
        private Dictionary<int, List<CityBuffGroupData>> m_cityBuffGroupDicBySeries = new Dictionary<int, List<CityBuffGroupData>>();
        private Dictionary<int, CityBuffGroupData> m_cityBuffGroupDic = new Dictionary<int, CityBuffGroupData>();
        private Dictionary<int, List<CityBuffDefine>> m_cityBuffDic = new Dictionary<int, List<CityBuffDefine>>();
        private bool m_type1Buff = false;
        private bool m_type2Buff = false;
        private string m_warFrenzyDes;
        private Timer timer_type1;//战争狂人计时器
        #endregion

        // Use this for initialization
        public CityBuffProxy(string proxyName)
            : base(proxyName)
        {

        }


        public override void OnRegister()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_cityBuildingProxy= AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            Debug.Log(" CityManagerProxy register");
        }


        public override void OnRemove()
        {
            Debug.Log(" CityManagerProxy remove");
            if (timer_type1 != null)
            {
                timer_type1.Cancel();
                timer_type1 = null;
            }
        }

        public List<CityBuffItemData> GetScrollItemList()
        {
            List<CityBuffItemData> scrollItems = new List<CityBuffItemData>();
            if (m_cityBuffGroupList.Count == 0)
            {
                InitData();
            }
            if (m_cityBuffSeriesList.Count == 0)
            {
                m_cityBuffSeriesList = CoreUtils.dataService.QueryRecords<CityBuffSeriesDefine>();
            }
            m_cityBuffSeriesList.ForEach((cityBuffSeries) =>
            {
                if (cityBuffSeries.show == 1)
                {
                    CityBuffItemData scrollItem = new CityBuffItemData(1, LanguageUtils.getText(cityBuffSeries.nameID));
                    scrollItems.Add(scrollItem);
                    if (m_cityBuffGroupDicBySeries.ContainsKey(cityBuffSeries.ID))
                    {
                        m_cityBuffGroupDicBySeries[cityBuffSeries.ID].ForEach((cityBuffGroup) =>
                        {
                            CityBuffItemData scrollItem1 = new CityBuffItemData(2, cityBuffGroup);
                            scrollItems.Add(scrollItem1);
                        });
                    }
                    if (!m_cityBuffSeriesDic.ContainsKey(cityBuffSeries.ID))
                    {
                        m_cityBuffSeriesDic[cityBuffSeries.ID] = cityBuffSeries;
                    }
                }

            });
            return scrollItems;
        }

        public List<CityBuffDefine> GetScrollItem1List(int group)
        {
            List<CityBuffDefine> cityBuffDefineList = null;
            if (m_cityBuffList.Count == 0)
            {
                m_cityBuffList = CoreUtils.dataService.QueryRecords<CityBuffDefine>();
            }
            m_cityBuffDic.Clear();
                m_cityBuffList.ForEach((cityBuff) =>
                {
                    if (!m_cityBuffDic.ContainsKey(cityBuff.group))
                    {
                        m_cityBuffDic[cityBuff.group] = new List<CityBuffDefine>();
                    }
                    else
                    { 
                        
                    }
                    ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(cityBuff.item);
                    if (itemDefine != null)
                    {
                        long num = m_bagProxy.GetItemNum(itemDefine.ID);
                        if (num != 0 || itemDefine.shortcutPrice != 0)
                        {
                            m_cityBuffDic[cityBuff.group].Add(cityBuff);
                        }
                    }
                });
            
            cityBuffDefineList = m_cityBuffDic[group];
            return cityBuffDefineList;
        }


        public List<BattleBuffDefine> GetScrollItem2List()
        {
            List<BattleBuffDefine> battleBuffDefine = CoreUtils.dataService.QueryRecords<BattleBuffDefine>();
            return battleBuffDefine;
        }
        public void InitData()
        {
            if (m_bagProxy == null)
            {
                m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            }
            if (m_playerAttributeProxy == null)
            {
                m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            }
            m_cityBuffGroupList = CoreUtils.dataService.QueryRecords<CityBuffGroupDefine>();
            m_cityBuffGroupList.ForEach((cityBuffGroup) =>
            {
                if (!m_cityBuffGroupDicBySeries.ContainsKey(cityBuffGroup.series))
                {
                    m_cityBuffGroupDicBySeries[cityBuffGroup.series] = new List<CityBuffGroupData>();
                }
                CityBuffGroupData groupData = new CityBuffGroupData();
                groupData.groupDefine = cityBuffGroup;
                m_cityBuffGroupDicBySeries[cityBuffGroup.series].Add(groupData);
                m_cityBuffGroupDic[cityBuffGroup.ID] = groupData;
            });
        }

        public void UpdateCityBuff()
        {
            if (m_cityBuffGroupList.Count == 0)
            {
                InitData();
            }
            m_type2Buff = false;
            m_type1Buff = false;
            m_playerProxy.CurrentRoleInfo.cityBuff.Values.ToList().ForEach((citybuff) =>
            {

                CityBuffGroupData cityBuffGroupData = null;
                CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)citybuff.id);
                if (cityBuffDefine != null)
                {
                    m_playerAttributeProxy.UpdateCityBuf(citybuff);
                    if (m_cityBuffGroupDic.TryGetValue((int)cityBuffDefine.group, out cityBuffGroupData))
                    {
                        if (citybuff.expiredTime != -2)
                        {
                            cityBuffGroupData.cityBuff = citybuff;
                            cityBuffGroupData.cityBuffDefine = cityBuffDefine;
                            if (cityBuffDefine.type == 1)
                            {
                                if (citybuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
                                {
                                    m_type1Buff = true;
                                }
                                else
                                {
                                    m_type1Buff = false;
                                }
                            }
                            else if (cityBuffDefine.type == 2)
                            {
                                m_type2Buff = true;
                            }
                            //          Debug.LogErrorFormat("{0},,,,{1},,,{2}", citybuff.id, cityBuffGroupData.cityBuff.expiredTime, ServerTimeModule.Instance.GetServerTime());
                        }
                        else
                        {
                            if (cityBuffGroupData.cityBuff != null && cityBuffGroupData.cityBuff.id == citybuff.id)
                            {
                                cityBuffGroupData.cityBuff = null;
                            }
                        }
                    }

                }
            });
        }
        public List<BuffListItemData> GetCityBuffList()
        {
            List<BuffListItemData> buffListItemDataList = new List<BuffListItemData>();
            m_playerProxy.CurrentRoleInfo.cityBuff.Values.ToList().ForEach((citybuff) =>
            {
                if (citybuff.expiredTime != -2)
                {
                    BuffListItemData buffListItemData = new BuffListItemData();
                    buffListItemData.cityBuff = citybuff;
                    buffListItemData.cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)buffListItemData.cityBuff.id);
                    buffListItemData.timer = null;
                    buffListItemDataList.Add(buffListItemData);
                }
            });
            return buffListItemDataList;
        }

        /// <summary>
        /// 是否处于战争狂热
        /// </summary>
        /// <returns></returns>
        public bool HasType1Buff()
        {
          return m_type1Buff;
        }
        

        /// <summary>
        /// 是否处于护盾
        /// </summary>
        /// <returns></returns>
        public bool HasType2Buff(long rid = 0)
        {
            if (rid == 0)
            {
                rid = m_playerProxy.CurrentRoleInfo.rid;
            }
                return m_cityBuildingProxy.HasType2Buff(rid);
        }

        /// <summary>
        /// 获取所有含特效的buff
        /// </summary>
        /// <returns></returns>
        public List<CityBuffDefine> GetEffectBuff(long rid = 0)
        {
            CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(rid);
            List<CityBuffDefine> buffList = new List<CityBuffDefine>();
            if (cityObjData != null)
            {
                if (cityObjData.mapObjectExtEntity!=null)
                    {
                    if (cityObjData.mapObjectExtEntity.cityBuff != null)
                    {
                        foreach (var cityBuff in cityObjData.mapObjectExtEntity.cityBuff.Values)
                        {
                            CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>((int)cityBuff.id);
                            if (cityBuff.expiredTime != -2)
                            {
                                if (!string.IsNullOrEmpty(cityBuffDefine.effect))
                                {
                                    buffList.Add(cityBuffDefine);
                                }
                            }
                        }
                    }
                }
            }
            return buffList;
        }

        public bool HasEffect(long rid, long buffID)
        {
            CityObjData cityObjData = m_cityBuildingProxy.GetCityObjData(rid);
            if (cityObjData != null)
            {
                if (cityObjData.mapObjectExtEntity.cityBuff != null)
                {
                    foreach (var cityBuff in cityObjData.mapObjectExtEntity.cityBuff.Values)
                    {
                        if (cityBuff.id == buffID && cityBuff.expiredTime != -2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 已经有同系列的buff
        /// </summary>
        /// <param name="cityBuffGroupData"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        public bool HasseriesBuff( CityBuffGroupData cityBuffGroupData ,out string tip)
        {
            bool b_series = false;
            int series = cityBuffGroupData.groupDefine.series;
            StringBuilder stringBuilder = new StringBuilder();
            CityBuffSeriesDefine cityBuffSeriesDefine = null;
            m_cityBuffSeriesDic.TryGetValue(series, out cityBuffSeriesDefine);
            if (cityBuffSeriesDefine != null)
            {
                if (cityBuffSeriesDefine.overlay == 1)
                {
                    List<CityBuffGroupData> cityBuffGroupDatas = m_cityBuffGroupDicBySeries[series];
                    for (int i = 0; i < cityBuffGroupDatas.Count; i++)
                    {
                        CityBuffGroupData  temp= cityBuffGroupDatas[i];
                        if (temp.cityBuff != null && temp.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
                        {
                            b_series = true;
                        }
                            if (i == 0)
                        {
                            stringBuilder.Append(LanguageUtils.getText(cityBuffGroupDatas[i].groupDefine.nameID));
                        }
                        else
                        {
                            stringBuilder.Append("/");
                            stringBuilder.Append(LanguageUtils.getText(cityBuffGroupDatas[i].groupDefine.nameID));
                        }
                    }
                }
            }
            tip = stringBuilder.ToString();
            return b_series;
        }

        public void SendUseItem(int buffid, int itemid, Action callback = null)
        {
            string tip = string.Empty;
            CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>(buffid);
            CityBuffGroupData cityBuffGroupData = null;
            if (m_cityBuffGroupDic.TryGetValue((int)cityBuffDefine.group, out cityBuffGroupData))
            {
                if (cityBuffGroupData.cityBuff != null && cityBuffGroupData.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
                {
                    Alert.CreateAlert(128010).SetRightButton(null, LanguageUtils.getText(192010))
                        .SetLeftButton(() =>
                    {
                        OkBlueCallBack(cityBuffDefine, itemid, callback);

                    }, LanguageUtils.getText(192009)
                    ).Show();
                }
                else
                {
                    if (HasseriesBuff(cityBuffGroupData, out tip))
                    {
                        Alert.CreateAlert(LanguageUtils.getTextFormat(128011, tip))
                            .SetRightButton(null, LanguageUtils.getText(192010))
                            .SetLeftButton(() =>
                              {
                                  OkBlueCallBack(cityBuffDefine, itemid, callback);
                              }, LanguageUtils.getText(192009))
                            .Show();

                    }
                    else
                    {
                        OkBlueCallBack(cityBuffDefine, itemid, callback);
                    }

                }
            }
        }

        public void OkBlueCallBack(CityBuffDefine citybuffid, int itemid, Action callback = null)
        {
            if (citybuffid.type == 2 && HasType1Buff())
            {
                Tip.CreateTip(128008).Show();
            }
            else
            {
                if (callback != null)
                {
                    callback();
                }
                else
                {
                    Role_AddBuff.request req = new Role_AddBuff.request();
                    req.buffId = citybuffid.ID;
                    req.itemId = itemid;
                    AppFacade.GetInstance().SendSproto(req);
                }
            }
        }
        public string GetWarFrenzyDesc()
        {
            if (string.IsNullOrEmpty(m_warFrenzyDes))
            {

                if (m_cityBuffList.Count == 0)
                {
                    m_cityBuffList = CoreUtils.dataService.QueryRecords<CityBuffDefine>();
                }
                m_cityBuffList.ForEach((cityBuff) =>
                {
                    if (cityBuff.type == 1)
                    {
                        m_warFrenzyDes = string.Format(LanguageUtils.getText(cityBuff.tag), cityBuff.tagData[0]);
                    }
                });
            }
            return m_warFrenzyDes;
        }
        /// <summary>
        /// 迁城按钮
        /// </summary>
        public void SendMoveCity()
        {
            if (WorldCamera.Instance().IsAutoMoving())
            {
                return;
            }
            if (CheckMoveCity(false))
            {
                CoreUtils.uiManager.ShowUI(UI.s_moveCity, null, 3);
            }
            else
            {
                Alert.CreateAlert(770100).SetRightButton().Show();
            }
        }
        /// <summary>
        /// 摄像头往目标点移动过程中禁止迁城,建造旗帜
        /// </summary>
        /// <returns></returns>
        public bool CheckGuildBuildCreatePre()
        {
            if (WorldCamera.Instance().IsAutoMoving())
            {
                return false;
            }
            return true;
        }
        public bool CheckMoveCity(bool showTip = true)
        {
            IPlayTroopCheckHandler playTroopCheckHandler = WorldMapLogicMgr.Instance.PlayTroopCheckHandler;
            if (playTroopCheckHandler.isHaveFight() || playTroopCheckHandler.isHaveRun())
            {
                if(showTip) Tip.CreateTip(LanguageUtils.getText(770000), Tip.TipStyle.Middle).Show();
                return false;
            }
            if (playTroopCheckHandler.isHaveStationing())
            {
                if (showTip) Tip.CreateTip(LanguageUtils.getText(770001), Tip.TipStyle.Middle).Show();
                return false;
            }
            if (playTroopCheckHandler.isHavePlayMarch())
            {
                if (showTip) Tip.CreateTip(LanguageUtils.getText(770002), Tip.TipStyle.Middle).Show();
                return false;
            }
            if (playTroopCheckHandler.isHaveRally())
            {
                if (showTip) Tip.CreateTip(LanguageUtils.getText(770002), Tip.TipStyle.Middle).Show();
                return false;
            }
            if (playTroopCheckHandler.isHaveCollect())
            {
                if (showTip) Tip.CreateTip(LanguageUtils.getText(770003), Tip.TipStyle.Middle).Show();
                return false;
            }
            if (playTroopCheckHandler.isHaveScoutMap())
            {
                if (showTip) Tip.CreateTip(LanguageUtils.getText(770004), Tip.TipStyle.Middle).Show();
                return false;
            }
            return true;
        }

        public string GetBattleBuffTime()
        {
            string s = string.Empty;
            int level = (int)m_playerProxy.CurrentRoleInfo.level;
            List<BattleBuffDefine> battleBuffDefine = CoreUtils.dataService.QueryRecords<BattleBuffDefine>();
            battleBuffDefine.ForEach((buffWarLevel) => {
                if (buffWarLevel.minLevel <= level && buffWarLevel.maxLevel >= level)
                {
                    CityBuffDefine cityBuffDefine = CoreUtils.dataService.QueryRecord<CityBuffDefine>(buffWarLevel.buff);
                    if (cityBuffDefine != null)
                    {
                       s = ClientUtils.FormatTimeCityBuff(cityBuffDefine.duration);
                    }
                }
            });
            return s;
        }
    }
}
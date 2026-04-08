// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月9日
// Update Time         :    2020年1月9日
// Class Description   :    UI_Win_ArmyConstMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using System;
using System.Linq;
using Data;

namespace Game {
    enum enumArmConstTab
    {
        /// <summary>
        /// 总兵力
        /// </summary>
        Total = 0,
        /// <summary>
        /// 城内
        /// </summary>
        inside = 1,
        /// <summary>
        /// 城外
        /// </summary>
        outside = 2,
    }
    public class ItemArmyConstData
    {
        public ArmyInfoEntity armyInfo; //部队信息
        public int prefab_index; //0,部队信息，1士兵信息
        public bool isSelected = false;
        public bool canSelected = false;
        public List<SoldierInfo> subItemData; //士兵信息
        public int dataType = 0;//数据来源 0部队信息，1 生动拼接
        public long mainHeroId = 0;
        public long deputyHeroId = 0;
        public long soldiers = 0;

    }
    public class UI_Win_ArmyConstMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ArmyConstMediator";

        private AllianceProxy m_allianceProxy;
        private TroopProxy m_troopProxy; 
        private SoldierProxy m_soldierProxy;
        private PlayerProxy m_playerProxy;
        private HospitalProxy m_hospitalProxy;
        

        private List<ItemArmyConstData> m_armysList = new List<ItemArmyConstData>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<long, string> m_ArmIconDic = new Dictionary<long, string>();

        private Dictionary<long, long> m_armyNumData = new Dictionary<long, long>();//总兵力

        private Dictionary<long, long> m_inArmyNumData = new Dictionary<long, long>();//城内部队
        private Dictionary<long, ArmsDefine> m_armsDefines = new Dictionary<long, ArmsDefine>();
        private long m_seriousInjured;//重伤兵
        private long m_minorSoldiers;//轻伤兵

        private long m_totalNum;//总兵力
        private long m_troopsTotalPower = 0; //部队总战力

        private enumArmConstTab m_curTab;
        private bool m_assetsReady = false;
        #endregion

        //IMediatorPlug needs
        public UI_Win_ArmyConstMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ArmyConstView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
          m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_soldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_hospitalProxy = AppFacade.GetInstance().RetrieveProxy(HospitalProxy.ProxyNAME) as HospitalProxy;
            
            List<ArmsDefine> armsDefines = CoreUtils.dataService.QueryRecords<ArmsDefine>();
            armsDefines.ForEach((armsDefine) => {
                m_armsDefines.Add(armsDefine.ID, armsDefine);
                m_ArmIconDic.Add(armsDefine.ID, armsDefine.icon);
            });
            int id = (int)m_playerProxy.GetCivilization();
            CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>(id);
            if (define != null)
            {
                m_ArmIconDic.Add(0, m_playerProxy.ConfigDefine.woundedSoldier[define.hospitalMark]);
            }
            m_curTab = enumArmConstTab.Total;
            refreshData(m_curTab);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.AddCloseEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyConst);
            });
            view.m_ck_incity_GameToggle.AddListener(OnBtnIncityClick);
            view.m_ck_outcity_GameToggle.AddListener(OnBtnOutcityClick);
            view.m_ck_total_GameToggle.AddListener(OnBtnTotalClick);
        }

        protected override void BindUIData()
        {
            InitView();
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                m_assetsReady = true;
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                funcTab.GetItemSize = GetItemSize;
                funcTab.GetItemPrefabName = GetItemPrefabName;

                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_armysList.Count);
            });
        }

        #endregion
        private float GetItemSize(ListView.ListItem item)
        {
            var data = m_armysList[item.index];

            if (data.prefab_index == 0)
            {
                return 84f;
            }
            else if (data.prefab_index == 1)
            {
                return 98f;
            }

            return 98f;
        }
        private string GetItemPrefabName(ListView.ListItem item)
        {
            var data = m_armysList[item.index];

            return view.m_sv_list_ListView.ItemPrefabDataList[data.prefab_index];
        }
        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            var data = m_armysList[scrollItem.index];
            if (data.prefab_index == 0) //UI_Item_WarMemberJoin
            {
                UI_Item_ArmyConst_SubView subView;
                if (scrollItem.data == null)
                {
                    subView = new UI_Item_ArmyConst_SubView(scrollItem.go.GetComponent<RectTransform>());
                    scrollItem.data = subView;
                }
                else
                {
                    subView = scrollItem.data as UI_Item_ArmyConst_SubView;
                }
                subView.InitData();
                if (data.dataType == 0)
                {
                    long soldierInfos = 0;
                    subView.SetCaptain1(data.armyInfo.mainHeroId);
                    subView.SetCaptain2(data.armyInfo.deputyHeroId);
                    subView.SetName();
                    data.armyInfo.soldiers.Values.ToList().ForEach((temp) => {
                        soldierInfos += temp.num;
                    });
                    data.armyInfo.minorSoldiers.Values.ToList().ForEach((temp) => {
                        soldierInfos += temp.num;
                    });
                    subView.soldiers(soldierInfos);
                    subView.SetSelect(data.isSelected, data.canSelected);
                    subView.RemoveItemEvent();
                    subView.AddItemEvent(()=> {
                        data.isSelected = !data.isSelected;
                        if (data.isSelected)
                        {
                           // Debug.LogError(scrollItem.index);
                            AddMember(scrollItem.index, data);
                            view.m_sv_list_ListView.FillContent(m_armysList.Count);
                        }
                        else
                        {
                         //   Debug.LogError(scrollItem.index);
                            RemoveMember(scrollItem.index, data);
                            view.m_sv_list_ListView.FillContent(m_armysList.Count);
                        }
                    });
                }
                else
                {
                    subView.SetCaptain1(data.mainHeroId);
                    subView.SetCaptain2(data.deputyHeroId);
                    subView.SetName();
                    subView.soldiers(data.soldiers);
                    subView.SetSelect(data.isSelected, data.canSelected);
                    subView.RemoveItemEvent();
                }
            }
            else if (data.prefab_index == 1) //UI_Item_WarMember  //
            {
                UI_Item_WarMenberDetialView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMenberDetialView>(scrollItem.go);
                UI_Item_SoldierHead_SubView[] subItems = new UI_Item_SoldierHead_SubView[]{itemView.m_UI_head1, itemView.m_UI_head2, itemView.m_UI_head3, itemView.m_UI_head4};

                var len = subItems.Length;
                for (int i = 0; i < len; i++)
                {
                    var subItem = subItems[i];
                    var subData = data.subItemData.Count - 1 >= i ? data.subItemData[i] : null;

                    subItem.gameObject.SetActive(subData != null);
                    if (subData != null)
                    {
                        subItem.SetSoldierInfo(GetArmyHeadIcon(subData.id), (int)subData.num);
                        subItem.HeadBtnAddOnClick((int)subData.id);    
                    }
                }
            }
        }
        public void AddMember(int index, ItemArmyConstData tag)
        {
            List<SoldierInfo> soldierInfos = new List<SoldierInfo>();
            soldierInfos = tag.armyInfo.soldiers.Values.ToList();
            long minorSoldier = 0;
            tag.armyInfo.minorSoldiers.Values.ToList().ForEach((data)=>{
                minorSoldier += data.num;
            });
                

            soldierInfos.Sort((SoldierInfo x, SoldierInfo y) =>
            {
                int re = 0;
                re = ((int)y.level).CompareTo((int)x.level);
                if (re == 0)
                {
                    return x.type.CompareTo(y.type);
                }
                return re;
            });
            SoldierInfo soldierInfo = new SoldierInfo();
            soldierInfo.id = 0;
            soldierInfo.num = minorSoldier;
            soldierInfos.Add(soldierInfo);

            for (int i = soldierInfos.Count - 1; i >= 0; i--)
            {
                //    Debug.LogError(keys[i]+" " +  m_armyNumData[keys[i]]);
                if (soldierInfos[i].num == 0)
                {
                    soldierInfos.RemoveAt(i);
                }
            }
            int len = soldierInfos.Count;
            for (int i = 0; i < len; i = i + 4)
            {
                ItemArmyConstData itemArmyConstData = new ItemArmyConstData();

                itemArmyConstData.prefab_index = 1; //兵种
                itemArmyConstData.subItemData = new List<SoldierInfo>();
                itemArmyConstData.mainHeroId = tag.armyInfo.mainHeroId;
                for (int j = i; j < i + 4; j++)
                {
                    if (j < len)
                    {
                        itemArmyConstData.subItemData.Add(soldierInfos[j]);
                    }
                }
                m_armysList.Insert(index + 1, itemArmyConstData);
                index++;
            }
        }
        public void RemoveMember(int index, ItemArmyConstData tag)
        {
            int startIndex = 0;
            long key = tag.mainHeroId;

            int count = 0;

            int len = m_armysList.Count;

            for (int i = index + 1; i < len; i++)
            {
                var item = m_armysList[i];
                long itemkey = m_armysList[i].mainHeroId;

                //  Debug.LogError(itemRid + " " + targetRid);
                if (itemkey == key)
                {
                    if (startIndex == 0)
                    {
                        startIndex = i;
                    }
                    count++;
                }
            }
             // Debug.LogErrorFormat("{0},{1}", startIndex,count);
            m_armysList.RemoveRange(startIndex, count);
        }
        private void refreshData(enumArmConstTab enumTab)
        {
            m_curTab = enumTab;
            var soldiers = m_playerProxy.GetInArmyInfo();//内城兵力
            var armies = m_troopProxy.GetArmys();//外城兵力
            var seriousInjureds = m_playerProxy.GetWoundedInfo();//重伤兵力
            m_armyNumData.Clear();
            m_inArmyNumData.Clear();
            m_armysList.Clear();
            m_seriousInjured = 0;//重伤兵数量
            m_minorSoldiers = 0;//伤兵数量
            long soldierNum = 0;//内城兵力
            m_totalNum = 0; ;//总兵力
            m_troopsTotalPower = m_soldierProxy.GetTroopsTotalPower();
            if (soldiers != null)
            {
                soldiers.Values.ToList().ForEach((data) =>
                {
                    m_armyNumData.Add(data.id, data.num);
                    m_inArmyNumData.Add(data.id, data.num);
                    soldierNum += data.num;
                });
            }
            if (seriousInjureds != null)
            {
                seriousInjureds.Values.ToList().ForEach((data) =>
                {
                    m_seriousInjured += data.num;
                });
            }
            if (armies != null)
            {
                foreach (var data in armies)
                {
                    if (data.mainHeroId > 0)
                    {
                        if (data.soldiers != null)
                        {
                            data.soldiers.Values.ToList().ForEach((soldier) =>
                                {
                                    if (m_armyNumData.ContainsKey(soldier.id))
                                    {
                                        m_armyNumData[soldier.id] = m_armyNumData[soldier.id] + soldier.num;
                                    }
                                    else
                                    {
                                        m_armyNumData.Add(soldier.id, soldier.num);
                                    }
                                });
                        }
                        if (data.minorSoldiers != null)
                        {
                            data.minorSoldiers.Values.ToList().ForEach((soldier) =>
                            {
                                m_minorSoldiers += soldier.num;
                            });
                        }
                    }
                }
            }
            m_armyNumData.Values.ToList().ForEach((data)=> {
                m_totalNum += data;
            });
            m_totalNum += m_minorSoldiers;
            switch (enumTab)
            {
                case enumArmConstTab.Total:
                    {
                        List<long> keys = m_armyNumData.Keys.ToList();
                        keys.Sort((long x, long y) =>
                        {
                            int re = 0;
                            ArmsDefine armsDefinesX = m_armsDefines[x];
                            ArmsDefine armsDefinesY = m_armsDefines[y];
                            int typeX = armsDefinesX.armsType;
                            int typeY = armsDefinesY.armsType;
                            int levelX = armsDefinesX.armsLv;
                            int levelY = armsDefinesY.armsLv;
                          //  Debug.LogErrorFormat("{0},,,{1},,,{2},,,{3},,,,{4},,,,{5}",x,y, typeX, typeY, levelX, levelY);
                            re = levelY.CompareTo(levelX);
                            if (re == 0)
                            {
                                return typeX.CompareTo(typeY);
                            }
                            return re;
                        });

                        keys.Add(0);
                        m_armyNumData.Add(0, m_minorSoldiers);
                        for (int i = keys.Count - 1; i >= 0; i--)
                        {
                               // Debug.LogError(keys[i]+" " +  m_armyNumData[keys[i]]);
                            if (m_armyNumData[keys[i]] == 0)
                            {
                                m_armyNumData.Remove(keys[i]);
                                keys.RemoveAt(i);
                            }
                        }
                        int len = 0;
                        len = keys.Count;
                        for (int i = 0; i < len; i = i + 4)
                        {
                            ItemArmyConstData itemArmyConstData = new ItemArmyConstData();

                            itemArmyConstData.prefab_index = 1; //兵种
                            itemArmyConstData.subItemData = new List<SoldierInfo>();
                            m_armysList.Add(itemArmyConstData);
                            for (int j = i; j < i + 4; j++)
                            {
                                long num = 0;
                                SoldierInfo soldierInfo = new SoldierInfo();
                                if (j < len )
                                {
                                    soldierInfo.id = keys[j];
                                    m_armyNumData.TryGetValue(keys[j], out num);
                                    soldierInfo.num = num;
                                    itemArmyConstData.subItemData.Add(soldierInfo);
                                }
                            }
                        }
                    }
                    break;
                case enumArmConstTab.inside:
                    {
                        List<long> keys = m_inArmyNumData.Keys.ToList();
                        keys.Sort((long x, long y) =>
                        {
                            int re = 0;
                            ArmsDefine armsDefinesX = m_armsDefines[x];
                            ArmsDefine armsDefinesY = m_armsDefines[y];
                            int typeX = armsDefinesX.armsType;
                            int typeY = armsDefinesY.armsType;
                            int levelX = armsDefinesX.armsLv;
                            int levelY = armsDefinesY.armsLv;
                            re = levelY.CompareTo(levelX);
                            if (re == 0)
                            {
                                return typeX.CompareTo(typeY);
                            }
                            return re;
                        });

                        for (int i = keys.Count - 1; i >= 0; i--)
                        {
                            if (m_inArmyNumData[keys[i]] == 0)
                            {
                                m_inArmyNumData.Remove(keys[i]);
                                keys.RemoveAt(i);
                            }
                        }
                        if (soldierNum != 0)
                        {
                            ItemArmyConstData itemArmyConstData = new ItemArmyConstData();
                            itemArmyConstData.dataType = 1;
                            itemArmyConstData.mainHeroId = m_playerProxy.CurrentRoleInfo.mainHeroId;
                            itemArmyConstData.prefab_index = 0;
                            itemArmyConstData.deputyHeroId = m_playerProxy.CurrentRoleInfo.deputyHeroId;
                            itemArmyConstData.soldiers = soldierNum;
                            m_armysList.Add(itemArmyConstData);
                        }
                        int len = 0;
                        len = keys.Count;
                        for (int i = 0; i < len; i = i + 4)
                        {
                            ItemArmyConstData itemArmyConstData = new ItemArmyConstData();

                            itemArmyConstData.prefab_index = 1; //兵种
                            itemArmyConstData.subItemData = new List<SoldierInfo>();
                            m_armysList.Add(itemArmyConstData);
                            for (int j = i; j < i + 4; j++)
                            {
                                long num = 0;
                                SoldierInfo soldierInfo = new SoldierInfo();
                                if (j < len)
                                {
                                    soldierInfo.id = keys[j];
                                    m_inArmyNumData.TryGetValue(keys[j], out num);
                                    soldierInfo.num = num;
                                    itemArmyConstData.subItemData.Add(soldierInfo);
                                }
                            }
                        }
                    }
                    break;
                case enumArmConstTab.outside:
                    {
                        armies.ForEach((data)=> {
                            ItemArmyConstData itemArmyConstData = new ItemArmyConstData();
                            itemArmyConstData.armyInfo = data;
                            itemArmyConstData.mainHeroId = data.mainHeroId;
                            itemArmyConstData.prefab_index = 0;
                            itemArmyConstData.isSelected = false;
                            itemArmyConstData.canSelected = true;
                            itemArmyConstData.soldiers = 0;
                            m_armysList.Add(itemArmyConstData);
                        });
                    }
                    break;
            }
        }
        private void OnBtnTotalClick()
        {
            if (m_assetsReady)
            {
                if (m_curTab != enumArmConstTab.Total)
                {
                    refreshData(enumArmConstTab.Total);
                    RefreshView();
                    view.m_sv_list_ListView.FillContent(m_armysList.Count);
                    view.m_sv_list_ListView.ForceRefresh();
                }
            }
        }
        private void OnBtnIncityClick()
        {
            if (m_assetsReady)
            {
                if (m_curTab != enumArmConstTab.inside)
                {
                    refreshData(enumArmConstTab.inside);
                    RefreshView();
                    view.m_sv_list_ListView.FillContent(m_armysList.Count);
                    view.m_sv_list_ListView.ForceRefresh();
                }
            }
        }
        private void OnBtnOutcityClick()
        {
            if (m_assetsReady)
            {
                if (m_curTab != enumArmConstTab.outside)
                {
                    refreshData(enumArmConstTab.outside);
                    RefreshView();
                    view.m_sv_list_ListView.FillContent(m_armysList.Count);
                    view.m_sv_list_ListView.ForceRefresh();
                }
            }
        }
        private void InitView()
        {
            RefreshView();
            switch (m_curTab)
            {
                case enumArmConstTab.Total:
                    view.m_ck_total_GameToggle.isOn = true;
                    break;
                case enumArmConstTab.inside:
                    view.m_ck_incity_GameToggle.isOn = true;
                    break;
                case enumArmConstTab.outside:
                    view.m_ck_outcity_GameToggle.isOn = true;
                    break;
            }
        }

        private void RefreshView()
        {
            view.m_lbl_notroops_LanguageText.gameObject.SetActive(m_armysList.Count == 0);
            view.m_sv_list_ListView.gameObject.SetActive(!(m_armysList.Count == 0));

            view.m_lbl_powar_num_LanguageText.text = m_troopsTotalPower.ToString("N0");
            view.m_lbl_total_num_LanguageText.text = m_totalNum.ToString("N0"); 
              view.m_lbl_troopsline_num_LanguageText.text =  LanguageUtils.getTextFormat(300001,m_troopProxy.GetArmys().Count, m_troopProxy.GetTroopDispatchNum());
             view.m_lbl_hospital_num_LanguageText.text =  LanguageUtils.getTextFormat(300001, m_seriousInjured.ToString("N0"), m_hospitalProxy.GetHospitalCapacity().ToString("N0")); 
        }
        private string  GetArmyHeadIcon(long id)
        {
            string icon = string.Empty;
            m_ArmIconDic.TryGetValue(id, out icon);
            return icon;
        }
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    UI_Win_ReinforcementsMediator
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
using Hotfix.Utils;
using UnityEngine.EventSystems;
using Hotfix;

namespace Game {

    public class UI_Win_ReinforcementsMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ReinforcementsMediator";

        private PlayerProxy m_playerProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private CityBuildingProxy m_CityBuildingProxy;
        private TroopProxy m_TroopProxy;
        private CityBuffProxy m_CityBuffProxy;
        private AllianceProxy m_allianceProxy;

        private List<ReinforceDetailItemData> m_reinforceList = new List<ReinforceDetailItemData>();
        private List<ReinforceRecordInfo> m_reinforceRecordInfoList = new List<ReinforceRecordInfo>();

        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private bool m_showHistroy = false;
        private int m_maxArmCount = 0;//最大援军数量
        private int m_curArmCount = 0;//当前援军数量
        private Timer m_timer;//计时器
        private long m_myobjectid;
        private List<long> m_reinforceIDList = new List<long>();//维护一个当前在界面上的援军id表

        #endregion

        //IMediatorPlug needs
        public UI_Win_ReinforcementsMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ReinforcementsView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_GetCityReinforceRecord.TagName,
                 Rally_RepatriationReinforce.TagName,
                 CmdConstant.ReinforcesChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ReinforcesChange:
                    m_curArmCount = 0;
                    List<long> reinforceIDList = new List<long>();//维护一个当前在界面上的援军id表
                    foreach (var reinforce in m_playerProxy.CurrentRoleInfo.reinforces.Values)
                    {
                        if (!m_reinforceIDList.Contains(reinforce.reinforceRid))
                        {
                            ReinforceDetailItemData reinforceDetailItemData = new ReinforceDetailItemData();
                            reinforceDetailItemData.prefab_index = 0;
                            reinforceDetailItemData.isSelected = false;
                            reinforceDetailItemData.LevelMember = reinforce;
                            reinforceDetailItemData.armyCount = m_allianceProxy.CountSoldiers(reinforce.soldiers);
                            m_reinforceList.Add(reinforceDetailItemData);
                        }
                        reinforceIDList.Add(reinforce.reinforceRid);
                    }
                    m_reinforceIDList = reinforceIDList;
                    for (int i = m_reinforceList.Count - 1; i >= 0; i--)
                    {
                        ReinforceDetailItemData reinforceDetailItemData = m_reinforceList[i];
                        if (!m_reinforceIDList.Contains(reinforceDetailItemData.LevelMember.reinforceRid))
                        {
                            m_reinforceList.RemoveAt(i);
                        }
                    }
                     view.m_sv_list_ListView.FillContent(m_reinforceList.Count);
                    RefreshRmptyListView();
                    break;
                case Rally_RepatriationReinforce.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;

                            switch ((ErrorCode)@error.errorCode)
                            {
                                case ErrorCode.RALLY_REINFORCE_CITY_FAIL:
                                case ErrorCode.RALLY_REPATRIATION_REINFORCE_FAIL:
                                    Tip.CreateTip(200564).Show();
                                    break;
                            }
                            }
                        else
                        {

                        }
                    }
                    break;
                case Role_GetCityReinforceRecord.TagName:
                    {
                        Role_GetCityReinforceRecord.response response = notification.Body as Role_GetCityReinforceRecord.response;
                        if (response != null)
                        {
                            m_reinforceRecordInfoList = response.reinforceRecord;
                            m_reinforceRecordInfoList.Sort((x, y) => (int)(y.arrivalTime - x.arrivalTime));
                            //Debug.LogErrorFormat("ReinforceRecordInfo.count{0}", response.reinforceRecord.Count);
                            ShowHistrtoy(true);
                            if (m_reinforceRecordInfoList == null || m_reinforceRecordInfoList.Count == 0)
                            {

                            }
                            else
                            {
                                ListView.FuncTab funcTab = new ListView.FuncTab();
                                funcTab.ItemEnter = ViewItemByIndexHistroy;

                                view.m_sv_list_histroy_ListView.SetInitData(m_assetDic, funcTab);
                                view.m_sv_list_histroy_ListView.FillContent(m_reinforceRecordInfoList.Count);
                            }
                        }
                    }
                    break;
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
            ClearTimer();
        }

        public override void PrewarmComplete(){
            
        }

        public override void Update()
        {
            if (view.m_pl_tip_Animator.gameObject.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    pointerEventData.position = Input.mousePosition;
                    List<RaycastResult> result = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, result);
                    if (result.Count > 0)
                    {
                        view.m_pl_tip_Animator.Play("Close");
                    }
                }
            }
        }        

        protected override void InitData()
        {
            IsOpenUpdate = true;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_CityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_CityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_myobjectid = m_worldMapObjectProxy.MyCityId;
            BuildingInfoEntity buildingInfoEntity = m_CityBuildingProxy.GetBuildingInfoByType((long)EnumCityBuildingType.AllianceCenter);
            if (buildingInfoEntity != null)
            {
                BuildingAllianceCenterDefine buildingAllianceCenterDefine = CoreUtils.dataService.QueryRecord<BuildingAllianceCenterDefine>((int)buildingInfoEntity.level);
                if (buildingAllianceCenterDefine!=null)
                {
                    m_maxArmCount = buildingAllianceCenterDefine.defCapacity;
                }
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.setCloseHandle(()=> {
                if (m_showHistroy)
                {
                    ShowHistrtoy(false);
                }
                else
                {
                    CoreUtils.uiManager.CloseUI(UI.s_reinforcements);
                }

            });
            view.m_UI_histroy.AddClickEvent(OnHistrtoyBtnClick);
            view.m_btn_icon_GameButton.onClick.AddListener(OnIconBtnClick);
            m_timer = Timer.Register(1, onTime, null, true);
        }

        protected override void BindUIData()
        {
            ShowHistrtoy(false);
            m_reinforceList.Clear();
            view.m_pb_rogressBar_GameSlider.minValue = 0;
            view.m_pb_rogressBar_GameSlider.maxValue = m_maxArmCount;
            m_curArmCount = 0;
            RefreshArmCountView();
            foreach (var reinforce in m_playerProxy.CurrentRoleInfo.reinforces.Values)
            {
                ReinforceDetailItemData reinforceDetailItemData = new ReinforceDetailItemData();
                reinforceDetailItemData.prefab_index = 0;
                reinforceDetailItemData.isSelected = false;
                reinforceDetailItemData.LevelMember = reinforce;
                reinforceDetailItemData.armyCount = m_allianceProxy.CountSoldiers(reinforce.soldiers);
                m_reinforceList.Add(reinforceDetailItemData);
                m_reinforceIDList.Add(reinforce.reinforceRid);
            }
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            m_preLoadRes.AddRange(view.m_sv_list_histroy_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                funcTab.GetItemSize = GetItemSize;
                funcTab.GetItemPrefabName = GetItemPrefabName;

                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_reinforceList.Count);

                RefreshRmptyListView();
            });
        }

        #endregion
        private void onTime()
        {
            m_curArmCount = 0;
            if (m_reinforceList.Count > 0)
            {
                for (int i = 0; i < m_reinforceList.Count; i++)
                {
                    var item = view.m_sv_list_ListView.GetItemByIndex(i);
                    if (item != null && item.go != null)
                    {
                        ViewItemByIndex(item);
                    }
                    if (m_reinforceList[i].prefab_index == 0)
                    {
                        if (m_reinforceList[i].LevelMember.arrivalTime - ServerTimeModule.Instance.GetServerTime() <=0)
                        {
                            m_curArmCount += (int)m_allianceProxy.CountSoldiers(m_playerProxy.CurrentRoleInfo.reinforces[m_reinforceList[i].LevelMember.reinforceRid].soldiers);
                        }
                    }
                }
            }
            RefreshArmCountView();
        }
        private string GetItemPrefabName(ListView.ListItem item)
        {
            {
                var data = m_reinforceList[item.index];
                return view.m_sv_list_ListView.ItemPrefabDataList[data.prefab_index];
            }
        }
        private float GetItemSize(ListView.ListItem item)
        {
            var data = m_reinforceList[item.index];

            if (data.prefab_index == 0)
            {
                return 120f;
            }
            else if (data.prefab_index == 1)
            {
                return 120f;
            }

            return 120f;
        }
        private void ViewItemByIndexHistroy(ListView.ListItem scrollItem)
        {
            var data = m_reinforceRecordInfoList[scrollItem.index];

            UI_Item_ReinforcementsHistroyView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ReinforcementsHistroyView>(scrollItem.go);

            itemView.m_lbl_count_LanguageText.text = data.armyCount.ToString("N0");
            itemView.m_lbl_playerName_LanguageText.text = data.name;
            itemView.m_UI_PlayerHead.LoadHeadCountry((int)data.headId);
            itemView.m_lbl_time_LanguageText.text = ServerTimeModule.Instance.ConverToServerDateTime(data.arrivalTime).ToString("yyyy/MM/dd HH:mm");
        }
        private void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            var data = m_reinforceList[scrollItem.index];
            if (data.prefab_index == 0)//UI_Item_WarMember  //
            {
                UI_Item_WarMemberView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberView>(scrollItem.go);

                var armyInfo = data.LevelMember;
                //----------------------------------------------------------
                itemView.m_pb_collect_GameSlider.maxValue = 1;
                itemView.m_pb_collect_GameSlider.minValue = 0;
                itemView.m_pb_collect_GameSlider.value = 1;
                itemView.m_btn_leader_GameButton.gameObject.SetActive(false);
                //----------------------------------------------------------
                int armyCount = 0;
                if (!m_playerProxy.CurrentRoleInfo.reinforces.ContainsKey(data.LevelMember.reinforceRid))
                {
                    return;
                }
                ReinforceArmyInfo reinforceArmyInfo = m_playerProxy.CurrentRoleInfo.reinforces[data.LevelMember.reinforceRid];
                itemView.m_UI_PlayerHead.LoadPlayerIcon((int)reinforceArmyInfo.headId, (int)reinforceArmyInfo.headFrameID);

                itemView.m_lbl_name_LanguageText.text = reinforceArmyInfo.name;


                itemView.m_UI_Captain1.gameObject.SetActive(armyInfo.mainHeroId > 0);

                string nameCp1 = String.Empty;
                string nameCp2 = String.Empty;

                if (armyInfo.mainHeroId > 0)
                {
                    nameCp1 = itemView.m_UI_Captain1.LoadHeadID(armyInfo.mainHeroId, armyInfo.mainHeroLevel);
                }

                itemView.m_UI_Captain2.gameObject.SetActive(armyInfo.deputyHeroId > 0);
                if (armyInfo.deputyHeroId > 2)
                {
                    nameCp2 = itemView.m_UI_Captain2.LoadHeadID(armyInfo.deputyHeroId, armyInfo.deputyHeroLevel);
                }

                //队伍信息
                itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730309, ClientUtils.FormatComma(m_allianceProxy.CountSoldiers(m_playerProxy.CurrentRoleInfo.reinforces[armyInfo.reinforceRid].soldiers)));

                itemView.m_lbl_captainName_LanguageText.text = armyInfo.deputyHeroId > 0
                    ? LanguageUtils.getTextFormat(300001, nameCp1, nameCp2)
                    : nameCp1;
                RefreshView(itemView, data);
                itemView.m_btn_back_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_back_GameButton.onClick.AddListener(() =>
                {
                    Alert.CreateAlert(LanguageUtils.getText(730310))
                     .SetRightButton(null, LanguageUtils.getText(300013))
                     .SetLeftButton(() =>
                     {
                         if (m_CityBuildingProxy.MyCityObjData.mapObjectExtEntity != null)
                         {
                             //遣返增援我的部队
                             Rally_RepatriationReinforce.request request = new Rally_RepatriationReinforce.request();
                             request.repatriationRid = data.LevelMember.reinforceRid;
                             request.repatriationArmyIndex = data.LevelMember.armyIndex;
                             request.fromObjectIndex = m_CityBuildingProxy.MyCityObjData.mapObjectExtEntity.objectId;
                             request.isSelfBack = false;
                             AppFacade.GetInstance().SendSproto(request);
                         }
                     }, LanguageUtils.getText(300014))
                     .Show();
                });

                itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);
                //加入队伍
                itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                {
                    //selected tag
                    data.isSelected = !data.isSelected;

                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);

                    if (data.isSelected)
                    {
                        AddMember(scrollItem.index, data);
                        view.m_sv_list_ListView.FillContent(m_reinforceList.Count);
                    }
                    else
                    {
                        RemoveMemberCity(scrollItem.index, data);
                        view.m_sv_list_ListView.FillContent(m_reinforceList.Count);
                    }

                });


            }
            else
            {
                UI_Item_WarMenberDetialView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMenberDetialView>(scrollItem.go);
                UI_Item_SoldierHead_SubView[] subItems = new UI_Item_SoldierHead_SubView[] { itemView.m_UI_head1, itemView.m_UI_head2, itemView.m_UI_head3, itemView.m_UI_head4 };


                var len = subItems.Length;
                for (int i = 0; i < len; i++)
                {
                    var subItem = subItems[i];
                    var subData = data.subItemData.Count - 1 >= i ? data.subItemData[i] : null;

                    subItem.gameObject.SetActive(subData != null);

                    if (subData != null)
                    {
                        int num = 0;
                        SoldierInfo soldierInfo = null;
                        if (m_playerProxy.CurrentRoleInfo.reinforces[data.LevelMember.reinforceRid].soldiers.TryGetValue(subData.id, out soldierInfo))
                        {
                            subItem.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int)subData.id), (int)soldierInfo.num);
                        }
                        else
                        {
                            subItem.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int)subData.id), 0);
                        }
                    }
                }
            }
        }
        private void RefreshView(UI_Item_WarMenberDetialView itemView, ReinforceDetailItemData data)
        {
            //long rid = 0;
            //rid = data.LevelMember.reinforceRid;
            //if (rid == 0)
            //{
            //    return;
            //}
            //Dictionary<long, BattleRemainSoldiers> battleRemainSoldiers = WorldMapLogicMgr.Instance.BattleRemainSoldiersHandler.GetBattleRemainSoldiers(m_myobjectid);
            //if (battleRemainSoldiers != null && battleRemainSoldiers.Count != 0)
            //{
            //    foreach (var battleRemainSoldier in battleRemainSoldiers.Values)
            //    {
            //        if (battleRemainSoldier.rid == rid)
            //        {
            //            //  Debug.LogErrorFormat("{0},,,{1},,,,{2},,,,{3}", itemView.m_UI_head1.soldierId, itemView.m_UI_head2.soldierId, itemView.m_UI_head3.soldierId, itemView.m_UI_head4.soldierId);
            //            bool Contains1, Contains2, Contains3, Contains4;
            //            Contains1 = Contains2 = Contains3 = Contains4 = false;
            //            foreach (var remainSoldier in battleRemainSoldier.remainSoldier.Values)
            //            {
            //                if (itemView.m_UI_head1.soldierId == remainSoldier.id)
            //                {
            //                    itemView.m_UI_head1.SetSoldierInfo(remainSoldier.num);
            //                    Contains1 = true;
            //                }
            //                if (itemView.m_UI_head2.soldierId == remainSoldier.id)
            //                {
            //                    itemView.m_UI_head2.SetSoldierInfo(remainSoldier.num);
            //                    Contains1 = true;
            //                }
            //                if (itemView.m_UI_head3.soldierId == remainSoldier.id)
            //                {
            //                    itemView.m_UI_head3.SetSoldierInfo(remainSoldier.num);
            //                    Contains1 = true;
            //                }
            //                if (itemView.m_UI_head4.soldierId == remainSoldier.id)
            //                {
            //                    itemView.m_UI_head4.SetSoldierInfo(remainSoldier.num);
            //                    Contains1 = true;
            //                }
            //            }
            //            if (!Contains1)
            //            {
            //                itemView.m_UI_head1.SetSoldierInfo(0);
            //                Contains1 = true;
            //            }
            //            if (!Contains2)
            //            {
            //                itemView.m_UI_head2.SetSoldierInfo(0);
            //            }
            //            if (!Contains3)
            //            {
            //                itemView.m_UI_head3.SetSoldierInfo(0);
            //            }
            //            if (!Contains4)
            //            {
            //                itemView.m_UI_head4.SetSoldierInfo(0);
            //            }

            //        }
            //    }
           // }
        }
        private void RefreshView(UI_Item_WarMemberView itemView, ReinforceDetailItemData data)
        {
            int leftTimeArrival = (int)(data.LevelMember.arrivalTime - ServerTimeModule.Instance.GetServerTime());
            TimeSpan m_formatTimeSpan;

            if (leftTimeArrival >= 0)
            {
                itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                itemView.m_pb_collect_GameSlider.gameObject.SetActive(true);
                itemView.m_lbl_state_LanguageText.gameObject.SetActive(false);
                m_formatTimeSpan = new TimeSpan(0, 0, (int)leftTimeArrival);
                itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(730308, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                
            }
            else
            {
                itemView.m_btn_back_GameButton.gameObject.SetActive(true); 
                itemView.m_pb_collect_GameSlider.gameObject.SetActive(false);
                itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
                itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(732076);
                if (!data.arrived)
                {
                    data.arrived = true;
                }
            }
        }
        /// <summary>
        /// 清理计时器
        /// </summary>
        private void ClearTimer()
        {
                if (m_timer != null)
                {
                m_timer.Cancel();
                m_timer = null;
                }
        }

        private void RefreshArmCountView()
        {
            view.m_pb_rogressBar_GameSlider.value = m_curArmCount;
            view.m_lbl_barText_LanguageText.text = LanguageUtils.getTextFormat(730318, ClientUtils.FormatComma(m_curArmCount),
                    ClientUtils.FormatComma(m_maxArmCount));
        }
        private void RefreshRmptyListView()
        {
            if (m_reinforceList.Count == 0)
            {
                view.m_lbl_Empty_LanguageText.gameObject.SetActive(true);
            }
            else
            {
                view.m_lbl_Empty_LanguageText.gameObject.SetActive(false);
            }
        }

        public void AddMember(int index, ReinforceDetailItemData tag)
        {
            List<SoldierInfo> soldierInfos = new List<SoldierInfo>();
            soldierInfos = m_playerProxy.CurrentRoleInfo.reinforces[tag.LevelMember.reinforceRid].soldiers.Values.ToList();
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
            int len = soldierInfos.Count;
            for (int i = 0; i < len; i = i + 4)
            {
                ReinforceDetailItemData itemWarDetialData = new ReinforceDetailItemData();
                itemWarDetialData.LevelMember = tag.LevelMember;
                itemWarDetialData.prefab_index = 1; //兵种
                itemWarDetialData.subItemData = new List<SoldierInfo>();
                for (int j = i; j < i + 4; j++)
                {
                    if (j < len)
                    {
                        itemWarDetialData.subItemData.Add(soldierInfos[j]);
                    }
                }
                m_reinforceList.Insert(index + 1, itemWarDetialData);
                index++;
            }
        }
        public void RemoveMemberCity(int index, ReinforceDetailItemData tag)
        {
            int startIndex = 0;

            int count = 0;

            int len = m_reinforceList.Count;

            for (int i = index + 1; i < len; i++)
            {
                var item = m_reinforceList[i];

                if (item.prefab_index == 0)
                {
                    break;
                }
                if (startIndex == 0)
                {
                    startIndex = i;
                }

                count++;
            }
            m_reinforceList.RemoveRange(startIndex, count);
        }

        public void OnIconBtnClick()
        {
            view.m_pl_tip_Animator.gameObject.SetActive(true);
            view.m_pl_tip_Animator.Play("Open");
        }
        public void OnHistrtoyBtnClick()
        {
            Role_GetCityReinforceRecord.request  request = new Role_GetCityReinforceRecord.request();
            AppFacade.GetInstance().SendSproto(request);

        }
        public void ShowHistrtoy(bool show)
        {
            if (show)
            {
                view.m_UI_Model_Window_Type3.m_lbl_title_LanguageText.text = LanguageUtils.getText(730311);
            }
            else
            {
                view.m_UI_Model_Window_Type3.m_lbl_title_LanguageText.text = LanguageUtils.getText(730304);
            }
            view.m_pl_histroy.gameObject.SetActive(show);
            view.m_pl_armyList.gameObject.SetActive(!show);
            m_showHistroy = show;

        }
    }
}
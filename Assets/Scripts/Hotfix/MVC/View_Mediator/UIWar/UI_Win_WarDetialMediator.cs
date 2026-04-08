// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_Win_WarDetialMediator
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
using System.Linq;
using System;
using Data;
using Hotfix;

namespace Game
{
    public class UI_Win_WarDetialMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_WarDetialMediator";


        private RallyTroopsProxy m_rallyTroopsProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private AllianceProxy m_allianceProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;

        private GuildInfoEntity guildInfoEntity;//联盟信息
        private ItemWarData m_itemWarData;//集结信息
        private Timer m_timer;//计时器

        private Timer m_timerWarDetial ;//item计时器

        List<ItemWarDetialData> m_itemWarDetialDatas = new List<ItemWarDetialData>();
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private bool m_assetsReady = false;
        #endregion

        //IMediatorPlug needs
        public UI_Win_WarDetialMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_WarDetialView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.RallyTroopChange,
                Rally_RallyBattleInfo.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.RallyTroopChange:
                    if (m_itemWarData.detialType == DetialType.rallyDetail)
                    {
                        m_itemWarDetialDatas = m_rallyTroopsProxy.getItemWarDetialDataListType1(m_itemWarData.rallyRid);
                        if (m_itemWarDetialDatas.Count == 0)
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_warDetial);
                            return;
                        }
                    }
                    else
                    {
                        RallyDetailEntity rallyDetailEntity = m_rallyTroopsProxy.GetRallyDetailEntity(m_itemWarData.rallyedDetailEntity.rallyedIndex, m_itemWarData.rallyDetailEntity.rallyRid);
                        if (rallyDetailEntity == null)
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_warDetial);
                            return;
                        }
                        else
                        {
                            m_itemWarDetialDatas = m_rallyTroopsProxy.getItemWarDetialDataListType2(m_itemWarData.rallyedDetailEntity.rallyedIndex);
                        }

                    }
                    view.m_sv_list_ListView.FillContent(m_itemWarDetialDatas.Count);
                    RefreshMesView();
                    break;

                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {

            m_rallyTroopsProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            guildInfoEntity = m_allianceProxy.GetAlliance();
            if (view.data is ItemWarData)
            {
                m_itemWarData = view.data as ItemWarData;
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.AddCloseEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_warDetial);
            });
            m_timerWarDetial = Timer.Register(1, onTime, null, true);
        }

        protected override void BindUIData()
        {
            view.m_UI_Model_Window_Type3.m_lbl_title_LanguageText.text = LanguageUtils.getText(730291);
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            InitTopItem(m_itemWarData);
            m_timer = Timer.Register(1, () =>
            {
                InitTopItem(m_itemWarData);
            }, null, true, true, view.vb);
            if (m_itemWarData.detialType == DetialType.rallyedDetail)
            {
                long type = m_itemWarData.rallyedDetailEntity.rallyedType;
                if (type == (long)RssType.City || type == (long)RssType.None)
                {
                    view.m_lbl_mes_LanguageText.text = LanguageUtils.getText(730301);
                }
                else if (type == (long)RssType.GuildFlag || type == (long)RssType.GuildFortress1 || type == (long)RssType.GuildFortress2 || type == (long)RssType.GuildCenter)
                {
                    //int alianceBuildingType = m_allianceProxy.GetBuildServerTypeToConfigType(type);
                    //var cconfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(alianceBuildingType);
                    //if (cconfig != null)
                    //{
                    //    view.m_lbl_mes_LanguageText.text = LanguageUtils.getTextFormat(730302, LanguageUtils.getText(cconfig.l_nameId));
                    //}
                }
                else if (type == (long)RssType.CheckPoint ||
                    type == (long)RssType.HolyLand ||
                    type == (long)RssType.Sanctuary ||
                    type == (long)RssType.Altar ||
                    type == (long)RssType.Shrine ||
                    type == (long)RssType.LostTemple ||
                    type == (long)RssType.Checkpoint_1 ||
                    type == (long)RssType.Checkpoint_2 ||
                    type == (long)RssType.Checkpoint_3)

                {

                    //StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)m_itemWarData.rallyedDetailEntity.rallyTargetHolyLandId);
                    //if (strongHoldDataDefine != null)
                    //{
                    //    StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>((int)strongHoldDataDefine.type);
                    //    if (strongHoldTypeDefine != null)
                    //    {
                    //        view.m_lbl_mes_LanguageText.text = LanguageUtils.getTextFormat(730302, LanguageUtils.getText(strongHoldTypeDefine.l_nameId));
                    //    }
                    //}
                }
                else
                {
                    view.m_lbl_mes_LanguageText.text = "(缺)被集结目标内没有增援部队";
                }

            }
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                m_assetsReady = true;
                if (m_itemWarData.detialType == DetialType.rallyDetail)
                {
                    m_itemWarDetialDatas = m_rallyTroopsProxy.getItemWarDetialDataListType1(m_itemWarData.rallyRid);
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ViewItemByIndex_rallyDetail;
                    funcTab.GetItemSize = GetItemSize;
                    funcTab.GetItemPrefabName = GetItemPrefabName;

                    view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_list_ListView.FillContent(m_itemWarDetialDatas.Count);
                }
                else
                {
                    m_itemWarDetialDatas = m_rallyTroopsProxy.getItemWarDetialDataListType2(m_itemWarData.rallyedDetailEntity.rallyedIndex);
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ViewItemByIndex_rallyedDetail;
                    funcTab.GetItemSize = GetItemSize;
                    funcTab.GetItemPrefabName = GetItemPrefabName;

                    view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_list_ListView.FillContent(m_itemWarDetialDatas.Count);
                    RefreshMesView();

                }
            });
        }

        public override void OnRemove()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
            if (m_timerWarDetial != null)
            {
                m_timerWarDetial.Cancel();
                m_timerWarDetial = null;
            }
        }


        #endregion
        public void CancelTimerList()
        {

        }
        private void onTime()
        {
            if (m_itemWarData.detialType == DetialType.rallyDetail)
            {
                if (m_itemWarDetialDatas.Count > 0)
                {
                    for (int i = 0; i < m_itemWarDetialDatas.Count; i++)
                    {
                        var item = view.m_sv_list_ListView.GetItemByIndex(i);
                        if (item != null && item.go != null)
                        {
                            ViewItemByIndex_rallyDetail(item);
                        }
                    }
                }
            }
            else if (m_itemWarData.detialType == DetialType.rallyedDetail)
            {
                if (m_itemWarDetialDatas.Count > 0)
                {
                    for (int i = 0; i < m_itemWarDetialDatas.Count; i++)
                    {
                        var item = view.m_sv_list_ListView.GetItemByIndex(i);
                        if (item != null && item.go != null)
                        {
                            ViewItemByIndex_rallyedDetail(item);
                        }
                    }
                }
            }
        }
        private float GetItemSize(ListView.ListItem item)
        {
            var data = m_itemWarDetialDatas[item.index];

            if (data.itemType == 0)
            {
                return 100;
            }
            else if (data.itemType == 1)
            {
                return 88;
            }
            else if (data.itemType == 2)
            {
                return 94;
            }
            else if (data.itemType == 3)
            {
                return 90;
            }
            else
            {
                Debug.LogError("not find type");
            }
            return 90;
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            var data = m_itemWarDetialDatas[item.index];

            return view.m_sv_list_ListView.ItemPrefabDataList[data.itemType];
        }
        void ViewItemByIndex_rallyDetail(ListView.ListItem scrollItem)
        {
            ItemWarDetialData itemData = m_itemWarDetialDatas[scrollItem.index];
            if (itemData.detialType == DetialType.rallyDetail)
            {
                if (itemData.itemType == 0)
                {

                }
                else if (itemData.itemType == 1)
                {
                    UI_Item_WarMemberJoinView itemView =
                         MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberJoinView>(scrollItem.go);
                    itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_warDetial);
                        CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                        CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                        m_rallyTroopsProxy.SendJoinOrReinforce(m_itemWarData);

                    });
                }
                else if (itemData.itemType == 2)
                {
                    UI_Item_WarMemberView itemView =
                      MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberView>(scrollItem.go);
                    //状态
                    if (m_itemWarData.rallyDetailEntity.rallyRid == m_playerProxy.CurrentRoleInfo.rid)
                    {
                        if (itemData.isCaptain)
                        {
                            if (itemData.isme)
                            {
                                itemView.m_btn_back_GameButton.gameObject.SetActive(false);

                            }
                            else
                            {
                                itemView.m_btn_back_GameButton.gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            itemView.m_btn_back_GameButton.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                    }
                    if (itemData.isCaptain)
                    {
                        itemView.m_btn_leader_GameButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        itemView.m_btn_leader_GameButton.gameObject.SetActive(false);
                    }
                    long mainHeroId = 0;
                    long deputyHeroId = 0;
                    long memberRid = 0;
                    long mainHerolevel = 0;
                    long deputyHerolevel = 0;
                    string name = "";
                    long headId = 0;
                    long headFrameId = 0;

                    if (itemData.datatype == 1)
                    {
                        memberRid = itemData.joinRallyDetail.joinRid;
                        mainHeroId = itemData.joinRallyDetail.joinMainHeroId;
                        deputyHeroId = itemData.joinRallyDetail.joinDeputyHeroId;
                        mainHerolevel = itemData.joinRallyDetail.joinMainHeroLevel;
                        deputyHerolevel = itemData.joinRallyDetail.joinDeputyHeroLevel;
                        name = itemData.joinRallyDetail.joinName;
                        headId = itemData.joinRallyDetail.joinHeadId;
                        headFrameId = itemData.joinRallyDetail.joinHeadFrameId;
                    }
                    else
                    {
                        memberRid = itemData.reinforceDetail.reinforceRid;
                        mainHeroId = itemData.reinforceDetail.mainHeroId;
                        deputyHeroId = itemData.reinforceDetail.deputyHeroId;
                        mainHerolevel = itemData.reinforceDetail.mainHeroLevel;
                        deputyHerolevel = itemData.reinforceDetail.deputyHeroLevel;
                        name = itemData.reinforceDetail.reinforceName;
                        headId = itemData.reinforceDetail.reinforceHeadId;
                        headFrameId = itemData.reinforceDetail.reinforceHeadFrameId;
                    }
                        itemView.m_UI_PlayerHead.LoadPlayerIcon((int)headId, (int)headFrameId);
                        itemView.m_lbl_name_LanguageText.text = name;
                        itemView.m_btn_back_GameButton.onClick.RemoveAllListeners();
                        itemView.m_btn_back_GameButton.onClick.AddListener(() =>
                        {
                            m_rallyTroopsProxy.SendReturnRally(memberRid, name);
                        });

                    string nameCp1 = String.Empty;
                    string nameCp2 = String.Empty;
                    itemView.m_UI_Captain1.gameObject.SetActive(mainHeroId > 0);
                    if (mainHeroId > 0)
                    {
                        nameCp1 = itemView.m_UI_Captain1.LoadHeadID(mainHeroId, mainHerolevel);
                    }

                    itemView.m_UI_Captain2.gameObject.SetActive(deputyHeroId > 0);
                    if (deputyHeroId > 0)
                    {
                        nameCp2 = itemView.m_UI_Captain2.LoadHeadID(deputyHeroId, deputyHerolevel);
                    }
                    itemView.m_lbl_captainName_LanguageText.text = deputyHeroId > 0 ? LanguageUtils.getTextFormat(300001, nameCp1, nameCp2) : nameCp1;

                    RefreshItemView(itemView, itemData);

                    itemView.m_btn_leader_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_leader_GameButton.onClick.AddListener(() =>
                    {
                        HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(4023);
                        var data1 = LanguageUtils.getTextFormat(define.l_typeID, LanguageUtils.getText(define.l_data1), LanguageUtils.getText(define.l_data2));
                        HelpTip.CreateTip(LanguageUtils.getTextFormat(define.l_typeID, data1), itemView.m_btn_leader_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).Show();
                    });

                    //扩展
                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!itemData.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(itemData.isSelected);

                    //加入队伍
                    itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                    {
                        //selected tag
                        itemData.isSelected = !itemData.isSelected;

                        itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!itemData.isSelected);
                        itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(itemData.isSelected);

                        if (itemData.isSelected)
                        {
                            AddMember(scrollItem.index, itemData);
                            view.m_sv_list_ListView.FillContent(m_itemWarDetialDatas.Count);
                        }
                        else
                        {
                            RemoveMember(scrollItem.index, itemData);
                            view.m_sv_list_ListView.FillContent(m_itemWarDetialDatas.Count);
                        }

                    });
                }
                else if (itemData.itemType == 3)
                {
                    UI_Item_WarMenberDetialView itemView =
                     MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMenberDetialView>(scrollItem.go);
                    UI_Item_SoldierHead_SubView[] subItems = new UI_Item_SoldierHead_SubView[] { itemView.m_UI_head1, itemView.m_UI_head2, itemView.m_UI_head3, itemView.m_UI_head4 };
                    long memberRid = 0;
                    if (itemData.datatype == 1)
                    {
                        memberRid = itemData.joinRallyDetail.joinRid;
                    }
                    else if (itemData.datatype == 2)
                    {
                        memberRid = itemData.reinforceDetail.reinforceRid;
                    }
                    var len = subItems.Length;
                    for (int i = 0; i < len; i++)
                    {
                        var subItem = subItems[i];
                        var subData = itemData.subItemData.Count - 1 >= i ? itemData.subItemData[i] : null;

                        subItem.gameObject.SetActive(subData != null);

                        if (subData != null)
                        {
                            subItem.soldierId = subData.id;
                            subItem.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int)subData.id), (int)subData.num);
                            subItem.HeadBtnAddOnClick((int)subData.id);
                        }

                    }
                    RefreshItemView(itemView, itemData);
                }
            }
        }
        void ViewItemByIndex_rallyedDetail(ListView.ListItem scrollItem)
        {
            ItemWarDetialData itemData = m_itemWarDetialDatas[scrollItem.index];
                if (itemData.itemType == 0)
                {
                    UI_Item_WarMemberCantView itemView =
MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberCantView>(scrollItem.go);
                    long type = m_itemWarData.rallyedDetailEntity.rallyedType;
                        itemView.m_lbl_text_LanguageText.text = LanguageUtils.getText(730276);
                }
                else if (itemData.itemType == 1)
                {
                    UI_Item_WarMemberJoinView itemView =
                         MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberJoinView>(scrollItem.go);
                itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_warDetial);
                        CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                        CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                        FightHelper.Instance.Reinfore(1, (int)m_itemWarData.rallyedDetailEntity.rallyedIndex, m_itemWarData.rallyedDetailEntity.rallyedIndex, m_itemWarData.rallyedDetailEntity.rallyedPos.x, m_itemWarData.rallyedDetailEntity.rallyedPos.y, false, m_itemWarData.rallyedDetailEntity.rallyedReinforceMax - m_rallyTroopsProxy.GetReinforceCount(m_itemWarData.rallyedDetailEntity.rallyedIndex));

                    });
                }
                else if (itemData.itemType == 2)
                {
                    UI_Item_WarMemberView itemView =
                      MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMemberView>(scrollItem.go);
                    //状态
                    if (itemData.isCaptain)
                    {
                        if (itemData.isme)
                        {
                            itemView.m_btn_back_GameButton.gameObject.SetActive(false);

                        }
                        else
                        {
                            itemView.m_btn_back_GameButton.gameObject.SetActive(true);
                        }
                        itemView.m_btn_leader_GameButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                        itemView.m_btn_leader_GameButton.gameObject.SetActive(false);

                    }


                long mainHeroId = 0;
                long deputyHeroId = 0;
                long memberRid = 0;
                long mainHerolevel = 0;
                long deputyHerolevel = 0;
                string name = "";
                long headId = 0;
                long headFrameId = 0;

                memberRid = itemData.reinforceDetail.reinforceRid;
                mainHeroId = itemData.reinforceDetail.mainHeroId;
                deputyHeroId = itemData.reinforceDetail.deputyHeroId;
                mainHerolevel = itemData.reinforceDetail.mainHeroLevel;
                deputyHerolevel = itemData.reinforceDetail.deputyHeroLevel;
                name = itemData.reinforceDetail.reinforceName;
                headId = itemData.reinforceDetail.reinforceHeadId;
                headFrameId = itemData.reinforceDetail.reinforceHeadFrameId;

                    itemView.m_UI_PlayerHead.LoadPlayerIcon((int)headId, (int)headFrameId);

                    itemView.m_lbl_name_LanguageText.text = name;
                string nameCp1 = String.Empty;
                    string nameCp2 = String.Empty;
                    itemView.m_UI_Captain1.gameObject.SetActive(mainHeroId > 0);
                    if (mainHeroId > 0)
                    {
                        nameCp1 = itemView.m_UI_Captain1.LoadHeadID(mainHeroId, mainHerolevel);
                    }

                    itemView.m_UI_Captain2.gameObject.SetActive(deputyHeroId > 0);
                    if (deputyHeroId > 0)
                    {
                        nameCp2 = itemView.m_UI_Captain2.LoadHeadID(deputyHeroId, deputyHerolevel);
                    }
                    itemView.m_lbl_captainName_LanguageText.text = deputyHeroId > 0 ? LanguageUtils.getTextFormat(300001, nameCp1, nameCp2) : nameCp1;

                    itemView.m_btn_leader_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_leader_GameButton.onClick.AddListener(() =>
                    {
                        //Tip.CreateTip("打开队长tip").Show();
                    });
                    //扩展
                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!itemData.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(itemData.isSelected);

                    //加入队伍
                    itemView.m_btn_Join_GameButton.onClick.RemoveAllListeners();
                    itemView.m_btn_Join_GameButton.onClick.AddListener(() =>
                    {
                        //selected tag
                        itemData.isSelected = !itemData.isSelected;

                        itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!itemData.isSelected);
                        itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(itemData.isSelected);

                        if (itemData.isSelected)
                        {
                            AddMember(scrollItem.index, itemData);
                            view.m_sv_list_ListView.FillContent(m_itemWarDetialDatas.Count);
                        }
                        else
                        {
                            RemoveMember(scrollItem.index, itemData);
                            view.m_sv_list_ListView.FillContent(m_itemWarDetialDatas.Count);
                        }

                    });
                    RefreshItemView(itemView, itemData);
                }
                else if (itemData.itemType == 3)
                {
                    UI_Item_WarMenberDetialView itemView =
                     MonoHelper.GetOrAddHotFixViewComponent<UI_Item_WarMenberDetialView>(scrollItem.go);
                    UI_Item_SoldierHead_SubView[] subItems = new UI_Item_SoldierHead_SubView[] { itemView.m_UI_head1, itemView.m_UI_head2, itemView.m_UI_head3, itemView.m_UI_head4 };


                    var len = subItems.Length;
                    for (int i = 0; i < len; i++)
                    {
                        var subItem = subItems[i];
                        var subData = itemData.subItemData.Count - 1 >= i ? itemData.subItemData[i] : null;

                        subItem.gameObject.SetActive(subData != null);

                        if (subData != null)
                        {
                            subItem.soldierId = subData.id;
       subItem.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int)subData.id), (int)subData.num);
                        }

                    }
                    RefreshItemView(itemView, itemData);
                }
        }

        public long GetSoldiersCount(ItemWarDetialData tag)
        {
            long count = 0;
            if (tag.detialType  == DetialType.rallyDetail)
            {
                if (tag.datatype == 1)
                {
                    if (tag.joinRallyDetail.joinSoldiers != null)
                    {
                        tag.joinRallyDetail.joinSoldiers.Values.ToList().ForEach((SoldierInfo) =>
                        {
                            count += SoldierInfo.num;
                        });
                    }
                }
                else
                {
                    if (tag.reinforceDetail.soldiers != null)
                    {
                        tag.reinforceDetail.soldiers.Values.ToList().ForEach((SoldierInfo) =>
                        {
                            count += SoldierInfo.num;
                        });
                    }
                }
            }
            else
            {
                if (tag.reinforceDetail != null)
                {
                    if (tag.reinforceDetail.soldiers != null)
                    {
                        tag.reinforceDetail.soldiers.Values.ToList().ForEach((SoldierInfo) =>
                        {
                            count += SoldierInfo.num;
                        });
                    }
                }
            }

            return count;
        }
        public void AddMember(int index, ItemWarDetialData tag)
        {
            List<SoldierInfo> soldierInfos = new List<SoldierInfo>();
            if (tag.detialType == DetialType.rallyDetail)
            {
                if (tag.datatype == 1)
                {
                    if (tag.joinRallyDetail != null&& tag.joinRallyDetail.joinSoldiers!=null)
                    {
                        soldierInfos = tag.joinRallyDetail.joinSoldiers.Values.ToList();
                    }
                }
                else
                {
                    if (tag.reinforceDetail != null && tag.reinforceDetail.soldiers!=null)
                    {
                        soldierInfos = tag.reinforceDetail.soldiers.Values.ToList();
                    }
                }
            }
            else if (tag.detialType == DetialType.rallyedDetail)
            {
                if (tag.reinforceDetail != null&& tag.reinforceDetail.soldiers!=null)
                {
                    soldierInfos = tag.reinforceDetail.soldiers.Values.ToList();
                }
            }
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
                ItemWarDetialData itemWarDetialData = new ItemWarDetialData();

                itemWarDetialData.itemType = 3; //兵种
                itemWarDetialData.joinRallyDetail = tag.joinRallyDetail;
                itemWarDetialData.reinforceDetail = tag.reinforceDetail;
                itemWarDetialData.datatype = tag.datatype;
                itemWarDetialData.subItemData = new List<SoldierInfo>();
                itemWarDetialData.detialType = tag.detialType;

                for (int j = i; j < i + 4; j++)
                {
                    if (j < len)
                    {
                        itemWarDetialData.subItemData.Add(soldierInfos[j]);
                    }
                }

                m_itemWarDetialDatas.Insert(index + 1, itemWarDetialData);
                index++;
            }
        }
        public void RemoveMember(int index, ItemWarDetialData tag)
        {
            int startIndex = 0;
            long key = 0;
            if (tag.detialType == DetialType.rallyDetail)
            {
                if (tag.datatype == 1)
                {
                    if (tag.joinRallyDetail != null)
                    {
                        key = tag.joinRallyDetail.joinRid;
                    }
                    else
                    {
                        Debug.Log("tag.joinRallyDetail == null");
                    }
                }
                else
                {
                    if (tag.reinforceDetail != null)
                    {
                        key = tag.reinforceDetail.reinforceRid;
                    }
                    else
                    {
                        Debug.Log("tag.reinforceDetail == null");
                    }
                }
            }
            else if (tag.detialType == DetialType.rallyedDetail)
            {
                if (tag.reinforceDetail != null)
                {
                    key = tag.reinforceDetail.reinforceRid * 10 + tag.reinforceDetail.armyIndex;
                }
            }
            int count = 0;

            int len = m_itemWarDetialDatas.Count;

            for (int i = index + 1; i < len; i++)
            {
                var item = m_itemWarDetialDatas[i];
                long itemkey = 0;
                if (tag.detialType == DetialType.rallyDetail)
                {
                    if (tag.datatype == 1)
                    {
                        if (item.joinRallyDetail != null)
                        {
                            itemkey = item.joinRallyDetail.joinRid;
                        }
                    }
                    else
                    {
                        if (item.reinforceDetail != null)
                        {
                            itemkey = item.reinforceDetail.reinforceRid;
                        }
                    }

                }
                else if (tag.detialType == DetialType.rallyedDetail)
                {
                    if (item.reinforceDetail != null)
                    {
                        itemkey = item.reinforceDetail.reinforceRid * 10 + item.reinforceDetail.armyIndex;
                    }
                }
                //  Debug.LogError(itemRid + " " + targetRid);
                if (itemkey == key)
                {
                    if (item.itemType == 3)
                    {
                        if (startIndex == 0)
                        {
                            startIndex = i;
                        }
                        count++;
                    }
                }
            }
            //  Debug.LogErrorFormat("{0},{1}", startIndex,count);
            m_itemWarDetialDatas.RemoveRange(startIndex, count);
        }
        //时间刷新
        public void RefreshItemView(UI_Item_WarMemberView itemView, ItemWarDetialData data)
        {
            if (data.itemType == 2)
            {

                if (data.detialType == DetialType.rallyDetail)
                {
                    if (data.datatype == 1)
                    {
                        long wait = data.joinRallyDetail.joinArrivalTime - ServerTimeModule.Instance.GetServerTime();//等待中
                        long leftTimeReady = m_itemWarData.rallyDetailEntity.rallyReadyTime - ServerTimeModule.Instance.GetServerTime();//准备中
                        long leftTimeWait = m_itemWarData.rallyDetailEntity.rallyWaitTime - ServerTimeModule.Instance.GetServerTime();//等待中
                        long leftTimeMarch = m_itemWarData.rallyDetailEntity.rallyMarchTime - ServerTimeModule.Instance.GetServerTime();//行军中
                                                                                                                                        //   Debug.LogErrorFormat("{0},,,,{1},,,,{2},,,,,,,{3},,,{4}", data.joinRallyDetail.joinArrivalTime, m_itemWarData.rallyDetailEntity.rallyReadyTime, m_itemWarData.rallyDetailEntity.rallyWaitTime, m_itemWarData.rallyDetailEntity.rallyMarchTime, data.joinRallyDetail.joinName);
                        TimeSpan m_formatTimeSpan;
                        if (leftTimeReady >= 0)
                        {
                            itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                            if (wait >= 0)
                            {
                                itemView.m_pb_collect_GameSlider.gameObject.SetActive(true);
                                itemView.m_lbl_state_LanguageText.gameObject.SetActive(false);
                                m_formatTimeSpan = new TimeSpan(0, 0, (int)wait);
                                itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(730293, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                            }
                            else
                            {
                                itemView.m_pb_collect_GameSlider.gameObject.SetActive(false);
                                itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
                                itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(181240);
                            }
                        }
                        else if (leftTimeWait >= 0)
                        {
                            itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                            if (wait >= 0)
                            {
                                itemView.m_pb_collect_GameSlider.gameObject.SetActive(true);
                                itemView.m_lbl_state_LanguageText.gameObject.SetActive(false);
                                m_formatTimeSpan = new TimeSpan(0, 0, (int)wait);
                                itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(730293, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                            }
                            else
                            {
                                itemView.m_pb_collect_GameSlider.gameObject.SetActive(false);
                                itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
                                itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(181240);
                            }

                        }
                        else if (leftTimeMarch >= 0)
                        {
                            itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                            itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                            itemView.m_pb_collect_GameSlider.gameObject.SetActive(true);
                            itemView.m_lbl_state_LanguageText.gameObject.SetActive(false);
                            m_formatTimeSpan = new TimeSpan(0, 0, (int)leftTimeMarch);
                            itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(730293, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                        }
                        else
                        {
                            itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                            itemView.m_pb_collect_GameSlider.gameObject.SetActive(false);
                            itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
                            itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(732063);
                            Dictionary<long, BattleRemainSoldiers> battleRemainSoldiers = WorldMapLogicMgr.Instance.BattleRemainSoldiersHandler.GetBattleRemainSoldiers(m_itemWarData.rallyDetailEntity.rallyObjectIndex);
                            if (battleRemainSoldiers != null && battleRemainSoldiers.Count != 0)
                            {
                                BattleRemainSoldiers battleRemainSoldier = null;
                                if (battleRemainSoldiers.TryGetValue(data.joinRallyDetail.joinRid, out battleRemainSoldier))
                                {
                                    itemView.m_lbl_armyCount_LanguageText.text = m_allianceProxy.CountSoldiers(battleRemainSoldier.remainSoldier).ToString("N0");
                                }
                                else
                                {
                                    itemView.m_lbl_armyCount_LanguageText.text = 0.ToString("N0");
                                }
                            }
                            else
                            {
                                itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                                if (battleRemainSoldiers == null)
                                {
                                    //  Debug.LogError("战斗没有在视野内");
                                }
                                else if (battleRemainSoldiers.Count == 0)
                                {
                                    //   Debug.LogError("战斗没有士兵变化数据");
                                }
                            }
                        }
                    }
                    else
                    {
                        long arrival = data.reinforceDetail.arrivalTime - ServerTimeModule.Instance.GetServerTime();//增援行军中
                        long leftTimeMarch = m_itemWarData.rallyDetailEntity.rallyMarchTime - ServerTimeModule.Instance.GetServerTime();//行军中
                                                                                                                                        //   Debug.LogErrorFormat("{0},,,,{1},,,,{2},,,,,,,{3},,,{4}", data.joinRallyDetail.joinArrivalTime, m_itemWarData.rallyDetailEntity.rallyReadyTime, m_itemWarData.rallyDetailEntity.rallyWaitTime, m_itemWarData.rallyDetailEntity.rallyMarchTime, data.joinRallyDetail.joinName);
                        TimeSpan m_formatTimeSpan;

                        if (arrival >= 0)
                        {
                            itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                            itemView.m_pb_collect_GameSlider.gameObject.SetActive(true);
                            itemView.m_lbl_state_LanguageText.gameObject.SetActive(false);
                            m_formatTimeSpan = new TimeSpan(0, 0, (int)arrival);
                            itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(730308, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                        }
                        else if (leftTimeMarch >= 0)
                        {
                            itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                            itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                            itemView.m_pb_collect_GameSlider.gameObject.SetActive(true);
                            itemView.m_lbl_state_LanguageText.gameObject.SetActive(false);
                            m_formatTimeSpan = new TimeSpan(0, 0, (int)leftTimeMarch);
                            itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(730293, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                        }
                        else
                        {
                            itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                            itemView.m_pb_collect_GameSlider.gameObject.SetActive(false);
                            itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
                            itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(732063);
                            Dictionary<long, BattleRemainSoldiers> battleRemainSoldiers = WorldMapLogicMgr.Instance.BattleRemainSoldiersHandler.GetBattleRemainSoldiers(m_itemWarData.rallyDetailEntity.rallyObjectIndex);
                            if (battleRemainSoldiers != null && battleRemainSoldiers.Count != 0)
                            {
                                BattleRemainSoldiers battleRemainSoldier = null;
                                if (battleRemainSoldiers.TryGetValue(data.reinforceDetail.reinforceRid, out battleRemainSoldier))
                                {
                                    itemView.m_lbl_armyCount_LanguageText.text = m_allianceProxy.CountSoldiers(battleRemainSoldier.remainSoldier).ToString("N0");
                                }
                                else
                                {
                                    itemView.m_lbl_armyCount_LanguageText.text = 0.ToString("N0");
                                }
                            }
                            else
                            {
                                itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                                if (battleRemainSoldiers == null)
                                {
                                    //  Debug.LogError("战斗没有在视野内");
                                }
                                else if (battleRemainSoldiers.Count == 0)
                                {
                                    //   Debug.LogError("战斗没有士兵变化数据");
                                }
                            }
                        }
                    }
                }
                else if (data.detialType == DetialType.rallyedDetail)
                {
                    long leftTimeWait = data.reinforceDetail.arrivalTime - ServerTimeModule.Instance.GetServerTime();//等待中
                    TimeSpan m_formatTimeSpan;
                    if (leftTimeWait > 0)
                    {
                        itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                        itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                        itemView.m_pb_collect_GameSlider.gameObject.SetActive(true);
                        itemView.m_lbl_state_LanguageText.gameObject.SetActive(false);
                        m_formatTimeSpan = new TimeSpan(0, 0, (int)leftTimeWait);
                        itemView.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(730308, m_formatTimeSpan.Hours.ToString("D2"), m_formatTimeSpan.Minutes.ToString("D2"), m_formatTimeSpan.Seconds.ToString("D2"));
                    }
                    else
                    {
                        itemView.m_btn_back_GameButton.gameObject.SetActive(false);
                        itemView.m_pb_collect_GameSlider.gameObject.SetActive(false);
                        itemView.m_lbl_state_LanguageText.gameObject.SetActive(true);
                        itemView.m_lbl_state_LanguageText.text = LanguageUtils.getText(181240);
                        Dictionary<long, BattleRemainSoldiers> battleRemainSoldiers = WorldMapLogicMgr.Instance.BattleRemainSoldiersHandler.GetBattleRemainSoldiers(m_itemWarData.rallyedDetailEntity.rallyedIndex);
                        if (battleRemainSoldiers != null && battleRemainSoldiers.Count != 0)
                        {
                            BattleRemainSoldiers battleRemainSoldier = null;
                            if (battleRemainSoldiers.TryGetValue(data.reinforceDetail.reinforceRid, out battleRemainSoldier))
                            {
                                itemView.m_lbl_armyCount_LanguageText.text = m_allianceProxy.CountSoldiers(battleRemainSoldier.remainSoldier).ToString("N0");
                            }
                            else
                            {
                                itemView.m_lbl_armyCount_LanguageText.text = 0.ToString("N0");
                            }
                        }
                        else
                        {
                            itemView.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(730292, GetSoldiersCount(data).ToString("N0"));
                            if (battleRemainSoldiers == null)
                            {
                                //  Debug.LogError("战斗没有在视野内");
                            }
                            else if (battleRemainSoldiers.Count == 0)
                            {
                                //   Debug.LogError("战斗没有士兵变化数据");
                            }
                        }
                    }
                }
            }
            else
            {
                
            }
        }
        //兵力刷新
        public void RefreshItemView(UI_Item_WarMenberDetialView itemView, ItemWarDetialData data)
        {
            long rid = 0;
            if (data.itemType == 3)
            {
                if (data.detialType == DetialType.rallyDetail)
                {
                    if (data.datatype == 1)
                    {
                        rid = data.joinRallyDetail.joinRid;
                    }
                    else if (data.datatype == 2)
                    {
                        rid = data.reinforceDetail.reinforceRid;
                    }

                }
                else if (data.detialType == DetialType.rallyedDetail)
                {
                    rid = data.reinforceDetail.reinforceRid;
                }
                if (rid == 0)
                {
                    return;
                }
                Dictionary<long, BattleRemainSoldiers> battleRemainSoldiers = WorldMapLogicMgr.Instance.BattleRemainSoldiersHandler.GetBattleRemainSoldiers(m_itemWarData.rallyDetailEntity.rallyObjectIndex);
                if (battleRemainSoldiers != null && battleRemainSoldiers.Count != 0)
                {
                    BattleRemainSoldiers battleRemainSoldier = null;
                    bool Contains1, Contains2, Contains3, Contains4;
                    Contains1 = Contains2 = Contains3 = Contains4 = false;
                    if (battleRemainSoldiers.TryGetValue(rid, out battleRemainSoldier))
                    {
                        foreach (var remainSoldier in battleRemainSoldier.remainSoldier.Values)
                        {
                            if (itemView.m_UI_head1.soldierId == remainSoldier.id)
                            {
                                itemView.m_UI_head1.SetSoldierInfo(remainSoldier.num);
                                Contains1 = true;
                            }
                            if (itemView.m_UI_head2.soldierId == remainSoldier.id)
                            {
                                itemView.m_UI_head2.SetSoldierInfo(remainSoldier.num);
                                Contains2 = true;
                            }
                            if (itemView.m_UI_head3.soldierId == remainSoldier.id)
                            {
                                itemView.m_UI_head3.SetSoldierInfo(remainSoldier.num);
                                Contains3 = true;
                            }
                            if (itemView.m_UI_head4.soldierId == remainSoldier.id)
                            {
                                itemView.m_UI_head4.SetSoldierInfo(remainSoldier.num);
                                Contains4 = true;
                            }
                        }
                    }
                    if (!Contains1)
                    {
                        itemView.m_UI_head1.SetSoldierInfo(0);
                    }
                    if (!Contains2)
                    {
                        itemView.m_UI_head2.SetSoldierInfo(0);
                    }
                    if (!Contains3)
                    {
                        itemView.m_UI_head3.SetSoldierInfo(0);
                    }
                    if (!Contains4)
                    {
                        itemView.m_UI_head4.SetSoldierInfo(0);
                    }
                }
            }
        }

                        private void InitTopItem(ItemWarData itemData)
        {
            UI_Item_War_SubView subView = view.m_UI_Item_War;
            subView.InitData();
            if (itemData.detialType == DetialType.rallyDetail)
            {
                subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.gameObject.SetActive(true);
                subView.m_UI_Item_WarTargetMy.m_pl_build.gameObject.SetActive(false);
                if (itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetType == (long)RssType.City)
                {
                    subView.m_UI_Item_WarTarget.m_UI_PlayerHead.gameObject.SetActive(true);
                    subView.m_UI_Item_WarTarget.m_pl_build.gameObject.SetActive(false);
                }
                else
                {
                    subView.m_UI_Item_WarTarget.m_UI_PlayerHead.gameObject.SetActive(false);
                    subView.m_UI_Item_WarTarget.m_pl_build.gameObject.SetActive(true);
                }
                if (!itemData.Isme)
                {
                    if (itemData.Involveme)
                    {
                        subView.m_btn_add_GameButton.gameObject.SetActive(false);
                        subView.m_btn_delete_GameButton.gameObject.SetActive(false);
                        subView.m_UI_Item_WarTargetMy.m_pl_dis.gameObject.SetActive(true);
                        subView.m_lbl_join_LanguageText.text = "";
                    }
                    else
                    {
                        subView.m_btn_add_GameButton.gameObject.SetActive(true);
                        subView.m_btn_delete_GameButton.gameObject.SetActive(false);
                        subView.m_UI_Item_WarTargetMy.m_pl_dis.gameObject.SetActive(true);
                        subView.m_lbl_join_LanguageText.text = LanguageUtils.getText(730026);
                    }
                }
                else
                {
                    subView.m_btn_add_GameButton.gameObject.SetActive(false);
                    subView.m_btn_delete_GameButton.gameObject.SetActive(true);
                    subView.m_UI_Item_WarTargetMy.m_pl_dis.gameObject.SetActive(false);
                    subView.m_lbl_join_LanguageText.text = LanguageUtils.getText(730296);
                }
                float distance = Vector2.Distance(new Vector2(itemData.rallyDetailEntity.rallyPos.x / 100, itemData.rallyDetailEntity.rallyPos.y / 100), new Vector2(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y)) / 10;
                subView.m_UI_Item_WarTargetMy.SetDistance(LanguageUtils.getTextFormat(300220, ((int)distance).ToString("N0")));
                subView.m_btn_add_GameButton.onClick.RemoveAllListeners();
                subView.m_btn_add_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_warDetial);
                    CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                    m_rallyTroopsProxy.SendJoinOrReinforce(itemData);
                });
                subView.m_btn_delete_GameButton.onClick.RemoveAllListeners();
                subView.m_btn_delete_GameButton.onClick.AddListener(() =>
                {
                    Alert.CreateAlert(LanguageUtils.getText(730298))
                          .SetRightButton(null, LanguageUtils.getText(300013))
                          .SetLeftButton(() =>
                          {
                              Rally_DisbandRally.request request = new Rally_DisbandRally.request();
                              AppFacade.GetInstance().SendSproto(request);
                          }, LanguageUtils.getText(300014))
                          .Show();

                });
                subView.AddClickEvent(OnItemBtnClick);
                subView.SetArrowRight(true);
                subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.LoadPlayerIcon(itemData.rallyDetailEntity.rallyHeadId, itemData.rallyDetailEntity.rallyHeadFrameId);
                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.SetLinkText(LanguageUtils.getTextFormat(730286, itemData.rallyDetailEntity.rallyPos.x / 600, itemData.rallyDetailEntity.rallyPos.y / 600));
                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.AddListener(() =>
                {
                    OnTreatyBtnClick(new Vector2(itemData.rallyDetailEntity.rallyPos.x / 100, itemData.rallyDetailEntity.rallyPos.y / 100));

                });

                subView.SetNameSelf(LanguageUtils.getTextFormat(300030, guildInfoEntity.abbreviationName, itemData.rallyDetailEntity.rallyName));
                string armyCount = LanguageUtils.getTextFormat(730290, itemData.rallyDetailEntity.rallyArmyCount.ToString("N0"), itemData.rallyDetailEntity.rallyArmyCountMax.ToString("N0"));
                subView.SetArmyCount(armyCount);
                subView.m_UI_Item_WarTarget.m_UI_PlayerHead.LoadPlayerIcon(itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetHeadId, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetHeadFrameId);
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.SetLinkText(LanguageUtils.getTextFormat(730286, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetPos.x / 600, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetPos.y / 600));
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.AddListener(() =>
                {
                    OnTreatyBtnClick(new Vector2(itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetPos.x / 100, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetPos.y / 100));
                });
                subView.RefreshItemView(itemData);
                subView.RefreshTargetIcon(itemData);
                RefreshReinforceCountView(itemData, subView);
            }
            else if (itemData.detialType == DetialType.rallyedDetail)
            {
                RallyDetailEntity rallyDetail = itemData.rallyDetailEntity;
                RallyedDetailEntity rallyedDetailEntity = itemData.rallyedDetailEntity;
                MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(rallyedDetailEntity.rallyedIndex);
                //------------------------------------
                subView.m_lbl_join_LanguageText.text = LanguageUtils.getText(730026);
                subView.m_btn_delete_GameButton.gameObject.SetActive(false);
                //----------------------------------------
                long type = rallyedDetailEntity.rallyedType;
                if (type == (long)RssType.City || type == (long)RssType.Troop || type == (long)RssType.None)
                {
                    subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.gameObject.SetActive(true);
                    subView.m_UI_Item_WarTargetMy.m_pl_build.gameObject.SetActive(false);
                    subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.LoadPlayerIcon(itemData.rallyedDetailEntity.rallyedHeadId, itemData.rallyedDetailEntity.rallyedHeadFrameId);
                    subView.SetNameSelf(LanguageUtils.getTextFormat(300030, guildInfoEntity.abbreviationName, rallyedDetailEntity.rallyedName));
                }
                else if (type == (long)RssType.CheckPoint || type == (long)RssType.HolyLand || type == (long)RssType.Sanctuary || type == (long)RssType.Altar || type == (long)RssType.Shrine || type == (long)RssType.Checkpoint_1 || type == (long)RssType.Checkpoint_2 || type == (long)RssType.Checkpoint_3)
                {
                    subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.gameObject.SetActive(false);
                    subView.m_UI_Item_WarTargetMy.m_pl_build.gameObject.SetActive(true);
                    int monsterid = 1001;//TODO
                    StrongHoldDataDefine strongHoldDataDefine = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)rallyedDetailEntity.rallyTargetHolyLandId);
                    if (strongHoldDataDefine != null)
                    {
                        StrongHoldTypeDefine strongHoldTypeDefine = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>((int)strongHoldDataDefine.type);
                        if (strongHoldTypeDefine != null)
                        {
                            subView.m_UI_Item_WarTargetMy.SetBuildIcon(strongHoldTypeDefine.iconImg);
                            subView.SetNameSelf(LanguageUtils.getTextFormat(300030, guildInfoEntity.abbreviationName, LanguageUtils.getText(strongHoldTypeDefine.l_nameId)));
                        }
                    }

                }
                else
                {
                    subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.gameObject.SetActive(false);
                    subView.m_UI_Item_WarTargetMy.m_pl_build.gameObject.SetActive(true);
                    subView.m_UI_Item_WarTargetMy.m_img_frame_PolygonImage.gameObject.SetActive(false);
                    int alianceBuildingType = m_allianceProxy.GetBuildServerTypeToConfigType(type);
                    var cconfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(alianceBuildingType);
                    subView.m_UI_Item_WarTargetMy.SetBuildIcon(cconfig.iconImg);
                    subView.SetNameSelf(LanguageUtils.getTextFormat(300030, guildInfoEntity.abbreviationName, LanguageUtils.getText(cconfig.l_nameId)));

                }
                if (!itemData.Isme)
                {
                    subView.m_UI_Item_WarTargetMy.m_pl_dis.gameObject.SetActive(true);

                }
                else
                {
                    subView.m_UI_Item_WarTargetMy.m_pl_dis.gameObject.SetActive(false);
                }

                subView.m_btn_add_GameButton.onClick.RemoveAllListeners();
                subView.m_btn_add_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.CloseUI(UI.s_warDetial);
                    CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                    FightHelper.Instance.Reinfore(1,(int)rallyedDetailEntity.rallyedIndex,rallyedDetailEntity.rallyedIndex);
                });
                subView.AddClickEvent(OnItemBtnClick);
                subView.SetArrowRight(false);
                subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.LoadPlayerIcon(itemData.rallyedDetailEntity.rallyedHeadId, itemData.rallyedDetailEntity.rallyedHeadFrameId);
                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.SetLinkText(LanguageUtils.getTextFormat(730286, itemData.rallyedDetailEntity.rallyedPos.x / 600, itemData.rallyedDetailEntity.rallyedPos.y / 600));
                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.AddListener(() =>
                {
                    OnTreatyBtnClick(new Vector2(itemData.rallyedDetailEntity.rallyedPos.x / 100, itemData.rallyedDetailEntity.rallyedPos.y / 100));
                });
                string armyCount = LanguageUtils.getTextFormat(730290, itemData.rallyDetailEntity.rallyArmyCount.ToString("N0"), itemData.rallyDetailEntity.rallyArmyCountMax.ToString("N0"));
                subView.SetArmyCount(armyCount); 
                float distance = 0;
                distance = Vector2.Distance(new Vector2(rallyedDetailEntity.rallyedPos.x / 100, rallyedDetailEntity.rallyedPos.y / 100), new Vector2(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y)) / 10;
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.SetLinkText(LanguageUtils.getTextFormat(730286, rallyDetail.rallyPos.x / 600, rallyDetail.rallyPos.y / 600));
                subView.m_UI_Item_WarTargetMy.SetDistance(LanguageUtils.getTextFormat(300220, ((int)distance).ToString("N0")));

                subView.m_UI_Item_WarTarget.m_UI_PlayerHead.gameObject.SetActive(true);
                subView.m_UI_Item_WarTarget.m_pl_build.gameObject.SetActive(false);
                subView.m_UI_Item_WarTarget.m_UI_PlayerHead.LoadPlayerIcon(rallyDetail.rallyHeadId, rallyDetail.rallyHeadFrameId);
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.AddListener(() =>
                {
                    OnTreatyBtnClick(new Vector2(rallyDetail.rallyPos.x / 100, rallyDetail.rallyPos.y / 100));
                });
                RefreshReinforceCountView(itemData, subView);
                subView.RefreshItemView(itemData);
                subView.RefreshTargetIcon(itemData);
            }
        }
        private void RefreshReinforceCountView(ItemWarData itemWarData, UI_Item_War_SubView subView)
        {
            long reinforceCount = 0;
            if (itemWarData.detialType == DetialType.rallyedDetail)
            {
                if (!itemWarData.Isme)
                {
                    subView.m_lbl_join_LanguageText.gameObject.SetActive(true);
                    subView.m_btn_add_GameButton.gameObject.SetActive(true);
                }
                else
                {
                    subView.m_lbl_join_LanguageText.gameObject.SetActive(false);
                    subView.m_btn_add_GameButton.gameObject.SetActive(false);
                }
                reinforceCount = m_rallyTroopsProxy.GetReinforceCount(itemWarData.rallyedDetailEntity.rallyedIndex);
                string armyCount = LanguageUtils.getTextFormat(730290, reinforceCount.ToString("N0"), itemWarData.rallyedDetailEntity.rallyedReinforceMax.ToString("N0"));
                subView.SetArmyCount(armyCount);

                if (reinforceCount >= itemWarData.rallyedDetailEntity.rallyedReinforceMax)
                {
                    subView.m_btn_add_GameButton.gameObject.SetActive(false);
                    subView.m_btn_delete_GameButton.gameObject.SetActive(false);
                    subView.m_lbl_join_LanguageText.gameObject.SetActive(false);
                }
            }
            else
            {
                long rallyArmyCount = 0;
                MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(itemWarData.rallyDetailEntity.rallyObjectIndex);
                if (mapObjectInfoEntity != null)
                {
                    rallyArmyCount = mapObjectInfoEntity.armyCount;
                   // Debug.LogErrorFormat("rallyArmyCount{0}", rallyArmyCount);
                }
                else
                {
                    rallyArmyCount = itemWarData.rallyDetailEntity.rallyArmyCount;

                }

                string armyCount = LanguageUtils.getTextFormat(730290, rallyArmyCount.ToString("N0"), itemWarData.rallyDetailEntity.rallyArmyCountMax.ToString("N0"));
                subView.SetArmyCount(armyCount);

                if (itemWarData.rallyDetailEntity.rallyArmyCount >= itemWarData.rallyDetailEntity.rallyArmyCountMax)
                {
                    if (!itemWarData.Isme)
                    {
                        subView.m_btn_add_GameButton.gameObject.SetActive(false);
                        subView.m_lbl_join_LanguageText.gameObject.SetActive(false);
                    }
                }
                else
                {

                }
            }
        }
        public void RefreshMesView()
        {
            if (m_itemWarDetialDatas.Count == 0)
            {
                view.m_lbl_mes_LanguageText.gameObject.SetActive(true);
            }
            else
            {
                view.m_lbl_mes_LanguageText.gameObject.SetActive(false);
            }
        }

        public void OnTreatyBtnClick(Vector2 vector2)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, vector2);
            CoreUtils.uiManager.CloseUI(UI.s_warDetial);
            CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
            CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
        }
        public void OnItemBtnClick()
        {

        }
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_Win_WarMediator
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
using Data;

namespace Game
{
    public class UI_Win_WarMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_WarMediator";

        private RallyTroopsProxy m_rallyTroopsProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private PlayerProxy m_playerProxy;
        private AllianceProxy m_allianceProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;


        private GuildInfoEntity guildInfoEntity;//联盟信息
        private Timer m_timer;

        List<ItemWarData> m_itemWarDatas = new List<ItemWarData>();
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private bool m_assetsReady = false;

        private int m_rallyedReinforceCount = 0;
        #endregion

        //IMediatorPlug needs
        public UI_Win_WarMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_WarView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                
                Rally_RallyBattleInfo.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Rally_RallyBattleInfo.TagName:
                    m_itemWarDatas = m_rallyTroopsProxy.GetItemWarDataList();

                    view.m_sv_list_ListView.FillContent(m_itemWarDatas.Count);
                    if (m_itemWarDatas.Count == 0)
                    {
                        view.m_lbl_safe_LanguageText.gameObject.SetActive(true);
                    }
                    else
                    {
                        view.m_lbl_safe_LanguageText.gameObject.SetActive(false);
                    }
              //      Debug.LogErrorFormat("rallyDelete1111");
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
            Debug.LogFormat("打开联盟战争界面:{0}", Time.realtimeSinceStartup);
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
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.AddCloseEvent(() => {
                CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
            });
            m_timer = Timer.Register(1, onTime, null, true, false, view.vb);
        }

        protected override void BindUIData()
        {
            view.m_UI_Model_Window_Type3.m_lbl_title_LanguageText.text = LanguageUtils.getText(730081);
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            m_rallyedReinforceCount = 0;
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                m_assetsReady = true;
                m_itemWarDatas = m_rallyTroopsProxy.GetItemWarDataList();
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ItemWarEnter;
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_itemWarDatas.Count);
                if (m_itemWarDatas.Count == 0)
                {
                    view.m_lbl_safe_LanguageText.gameObject.SetActive(true);
                }
                else
                {
                    view.m_lbl_safe_LanguageText.gameObject.SetActive(false);
                }
            });
        }
        public override void OnRemove()
        {
            m_timer.Cancel();
            m_timer = null;
        }

        #endregion

        private void onTime()
        {
            for (int i = 0; i < m_itemWarDatas.Count; i++)
            {
                var item = view.m_sv_list_ListView.GetItemByIndex(i);
                if (item != null && item.go != null)
                {
                    ItemWarEnter(item);
                }
            }
        }
        private void ItemWarEnter(ListView.ListItem scrollItem)
        {
            //todo:bgtop需要有移动动画
            int index = scrollItem.index;
            var itemData = m_itemWarDatas[index];
            UI_Item_War_SubView subView;
            if (scrollItem.data == null)
            {
                subView = new UI_Item_War_SubView(scrollItem.go.GetComponent<RectTransform>());
                subView.InitData();
                scrollItem.data = subView;
            }
            else
            {
                subView = scrollItem.data as UI_Item_War_SubView;

            }
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
                subView.m_btn_add_GameButton.onClick.RemoveAllListeners();
                subView.m_btn_delete_GameButton.onClick.RemoveAllListeners();
                float distance = Vector2.Distance(new Vector2(itemData.rallyDetailEntity.rallyPos.x / 100, itemData.rallyDetailEntity.rallyPos.y / 100), new Vector2(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y)) / 10;
                subView.m_UI_Item_WarTargetMy.SetDistance(LanguageUtils.getTextFormat(300220, ((int)distance).ToString("N0")));
                subView.m_btn_add_GameButton.onClick.AddListener(() => {
                    CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                    m_rallyTroopsProxy.SendJoinOrReinforce(itemData);
                });
                subView.m_btn_delete_GameButton.onClick.AddListener(() => {
                    Alert.CreateAlert(LanguageUtils.getText(730298))
                          .SetRightButton(null, LanguageUtils.getText(300013))
                          .SetLeftButton(() =>
                          {
                              Rally_DisbandRally.request request = new Rally_DisbandRally.request();
                              AppFacade.GetInstance().SendSproto(request);
                          }, LanguageUtils.getText(300014))
                          .Show();

                });
                subView.AddClickEvent(() => { OnItemBtnClick(itemData); });
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
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.SetLinkText(LanguageUtils.getTextFormat(730286, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetPos.x / 600, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetPos.y / 600));
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.AddListener(() =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, new Vector2(itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetPos.x / 100, itemData.rallyDetailEntity.rallyTargetDetail.rallyTargetPos.y / 100));
                    CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                });
                subView.RefreshItemView( itemData);
                subView.RefreshTargetIcon(itemData);
                RefreshReinforceCountView(itemData, subView);
            }
            else if (itemData.detialType  == DetialType.rallyedDetail)
            {
                RallyDetailEntity rallyDetail = itemData.rallyDetailEntity;
                RallyedDetailEntity rallyedDetailEntity = itemData.rallyedDetailEntity;
                //------------------------------------
                subView.m_lbl_join_LanguageText.text = LanguageUtils.getText(730026);
                subView.m_btn_delete_GameButton.gameObject.SetActive(false);
                //----------------------------------------
                long type = rallyedDetailEntity.rallyedType;
             //   Debug.LogErrorFormat("type = {0}",type);
                if (type == (long)RssType.City|| type == (long)RssType.Troop || type == (long)RssType.None)
                {
                    subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.gameObject.SetActive(true);

                    subView.m_UI_Item_WarTargetMy.m_pl_build.gameObject.SetActive(false);
                    subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.LoadPlayerIcon(rallyedDetailEntity.rallyedHeadId, rallyedDetailEntity.rallyedHeadFrameId);
                    subView.SetNameSelf(LanguageUtils.getTextFormat(300030, guildInfoEntity.abbreviationName, rallyedDetailEntity.rallyedName));
                }
                else if (type == (long)RssType.CheckPoint || type == (long)RssType.HolyLand|| type == (long)RssType.Sanctuary || type == (long)RssType.Altar || type == (long)RssType.Shrine || type == (long)RssType.Checkpoint_1 || type == (long)RssType.Checkpoint_2 || type == (long)RssType.Checkpoint_3)
                {
                    subView.m_UI_Item_WarTargetMy.m_UI_PlayerHead.gameObject.SetActive(false);
                    subView.m_UI_Item_WarTargetMy.m_pl_build.gameObject.SetActive(true);

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
                    int alianceBuildingType = m_allianceProxy.GetBuildServerTypeToConfigType(type);
                    var cconfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(alianceBuildingType);
                    if (cconfig != null)
                    {
                        subView.m_UI_Item_WarTargetMy.SetBuildIcon(cconfig.iconImg);
                        subView.SetNameSelf(LanguageUtils.getTextFormat(300030, guildInfoEntity.abbreviationName, LanguageUtils.getText(cconfig.l_nameId)));
                    }
                    else
                    {
                    }
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
                    subView.m_btn_add_GameButton.onClick.AddListener(() => {
                            if (itemData.rallyedDetailEntity.rallyedReinforceMax == 0)
                            {
                                Tip.CreateTip(730371, Tip.TipStyle.Middle).Show();
                                return;
                            }
                        CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                        CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                        FightHelper.Instance.Reinfore(1, (int)rallyedDetailEntity.rallyedIndex, rallyedDetailEntity.rallyedIndex, rallyedDetailEntity.rallyedPos.x, rallyedDetailEntity.rallyedPos.y);
                });
                subView.AddClickEvent(() => { OnItemBtnClick(itemData); });
                subView.SetArrowRight(false);

                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.SetLinkText(LanguageUtils.getTextFormat(730286, itemData.rallyedDetailEntity.rallyedPos.x / 600, itemData.rallyedDetailEntity.rallyedPos.y / 600));
                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
                subView.m_UI_Item_WarTargetMy.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.AddListener(() => {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, new Vector2(itemData.rallyedDetailEntity.rallyedPos.x / 100, itemData.rallyedDetailEntity.rallyedPos.y / 100));
                    CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                });
                float distance = 0;
                    distance = Vector2.Distance(new Vector2(rallyedDetailEntity.rallyedPos.x / 100, rallyedDetailEntity.rallyedPos.y / 100), new Vector2(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y)) / 10;
                    subView.m_UI_Item_WarTargetMy.SetDistance(LanguageUtils.getTextFormat(300220, ((int)distance).ToString("N0")));

                subView.m_UI_Item_WarTarget.m_UI_LinkRight.SetLinkText(LanguageUtils.getTextFormat(730286, rallyDetail.rallyPos.x / 600, rallyDetail.rallyPos.y / 600));
                subView.m_UI_Item_WarTarget.m_UI_PlayerHead.gameObject.SetActive(true);
                subView.m_UI_Item_WarTarget.m_pl_build.gameObject.SetActive(false);
                subView.m_UI_Item_WarTarget.m_UI_PlayerHead.LoadPlayerIcon(rallyDetail.rallyHeadId, rallyDetail.rallyHeadFrameId);
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
                subView.m_UI_Item_WarTarget.m_UI_LinkRight.m_btn_treaty_GameButton.onClick.AddListener(() =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, new Vector2(rallyDetail.rallyPos.x / 100, rallyDetail.rallyPos.y / 100));
                    CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                });
                subView.RefreshItemView( itemData);
                RefreshReinforceCountView(itemData, subView);
                subView.RefreshTargetIcon(itemData);
            }

        }
        private void InitView()
        {

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
                    //Debug.LogErrorFormat("rallyArmyCount{0}", rallyArmyCount);
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
        public void OnTreatyBtnClick(Vector2 vector2)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, vector2);
            CoreUtils.uiManager.CloseUI(UI.s_AlianceWar);
            CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
        }
        public void OnItemBtnClick(ItemWarData itemData)
        {
            if (itemData.detialType == DetialType.rallyedDetail)
            {
                if (itemData.rallyedDetailEntity.rallyedReinforceMax == 0)
                {
                    Tip.CreateTip(730371, Tip.TipStyle.Middle).Show();
                    return;
                }
            }
        
            CoreUtils.uiManager.ShowUI(UI.s_warDetial, null, itemData); 
        }
    }
}
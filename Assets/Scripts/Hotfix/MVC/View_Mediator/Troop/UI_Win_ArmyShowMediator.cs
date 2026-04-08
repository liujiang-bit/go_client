// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月9日
// Update Time         :    2020年1月9日
// Class Description   :    UI_Win_ArmyShowMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Hotfix;
using PureMVC.Interfaces;
using SprotoType;
using Data;
using System;
using System.Linq;

namespace Game
{
    public class UI_Win_ArmyShowMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Win_ArmyShowMediator";
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private TroopProxy m_TroopProxy;
        private PlayerProxy m_playerProxy;
        private HeroProxy m_HeroProxy;
        private ScoutProxy m_scoutProxy;
        private SoldierProxy m_SoldierProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;
        private RallyTroopsProxy m_rallyTroopProxy;
        private AllianceProxy m_allianceProxy;

        private TroopMainCreate m_TroopMainData;
        private Dictionary<int, Timer> m_timers = new Dictionary<int, Timer>();
        private bool m_open = false;

        #endregion

        //IMediatorPlug needs
        public UI_Win_ArmyShowMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Win_ArmyShowView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.OnTroopDataChanged,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnTroopDataChanged:
                    OnRefreshTroopView();
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
            IsOpenUpdate = true;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_SoldierProxy = AppFacade.GetInstance().RetrieveProxy(SoldierProxy.ProxyNAME) as SoldierProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_worldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_rallyTroopProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_armys_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, (assetDic) =>
            {
                m_assetDic = assetDic;
                AssetLoadFinish();
            });


            ShowArmys();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.onClick.AddListener(() => { ShowArmys(); });
            view.m_btn_blet_GameButton.onClick.AddListener(() => { ShowList(); });
            view.m_UI_Model_Window_Type2.m_btn_close_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });
        }

        protected override void BindUIData()
        {
            view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(200039);
        }

        public override void OnRemove()
        {
            CancelTimer();
        }
        private void CancelTimer()
        {
            foreach (var time in m_timers.Values)
            {
                if (time != null)
                {
                    time.Cancel();
                }
            }
            m_timers.Clear();
        }

        private void OnRefreshTroopView()
        {
            if (m_TroopMainData == null) return;

            int oldCount = m_TroopMainData.GetDataCount();
            m_TroopMainData.Update();
            int count = m_TroopMainData.GetDataCount();
            if (oldCount != count)
            {
                CancelTimer();
                view.m_sv_armys_ListView.FillContent(count);
            }
            else
            {
                view.m_sv_armys_ListView.ForceRefresh();
            }
        }

        private void AssetLoadFinish()
        {
            m_TroopMainData = new TroopMainCreate();
            m_TroopMainData.Init();
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ListItemByIndex;
            funcTab.GetItemPrefabName = OnGetItemPrefabName;
            view.m_sv_armys_ListView.SetInitData(m_assetDic, funcTab);
            view.m_sv_armys_ListView.FillContent(m_TroopMainData.GetDataCount());
        }

        private string OnGetItemPrefabName(ListView.ListItem listItem)
        {
            int index = listItem.index;

            TroopMainCreateData info = m_TroopMainData.GetData(index);
            if (info.type == TroopMainCreateDataType.Troop)
            {
                return "UI_Item_ArmyShow";
            }
            else if (info.type == TroopMainCreateDataType.Transport)
            {
                return "UI_Item_ArmyShow";
            }
            else
            {
                return "UI_Item_Scout";
            }
        }

        private void ShowArmys()
        {
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(false);
            view.m_sv_desc_ListView.gameObject.SetActive(false);
            view.m_pl_show_PolygonImage.gameObject.SetActive(false);
            view.m_sv_armys_ListView.gameObject.SetActive(true);
            view.m_pl_blet_Animator.gameObject.SetActive(true);
            if (!m_open)
            {
                m_open = true;
            }
            else
            {
                view.m_pl_blet_Animator.Play("Show");
                view.m_sv_armys_Animator.Play("Show");
            }
        }

        private void ShowList()
        {
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_sv_desc_ListView.gameObject.SetActive(true);
            view.m_pl_show_PolygonImage.gameObject.SetActive(false);
            view.m_sv_armys_ListView.gameObject.SetActive(false);
            view.m_pl_blet_Animator.gameObject.SetActive(false);
            view.m_sv_desc_Animator.Play("Show");
        }

        private void ShowDetail(int index)
        {
            view.m_UI_Model_Window_Type2.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_sv_desc_ListView.gameObject.SetActive(false);
            view.m_pl_show_PolygonImage.gameObject.SetActive(true);
            view.m_sv_armys_ListView.gameObject.SetActive(false);
            view.m_pl_blet_Animator.gameObject.SetActive(false);
            view.m_pl_show_Animator.Play("Show");

            TroopMainCreateData info = m_TroopMainData.GetData(index);
            if (info.type == TroopMainCreateDataType.Transport)
            {
                view.m_pl_army.gameObject.SetActive(false);
                view.m_pl_transport_ArabLayoutCompment.gameObject.SetActive(true);
                CreateResItem(index);
            }
            else
            {
                view.m_pl_army.gameObject.SetActive(true);
                view.m_pl_transport_ArabLayoutCompment.gameObject.SetActive(false);
                CreateSoldierItem(index);
            }
        }

        private void CreateResItem(int index)
        {
            view.m_UI_Item_ArmyShowLayOut.m_root_RectTransform.DestroyAllChild();

            //            TroopMainCreateData info = m_TroopMainData.GetData(index);
            var lstTransportInfo = m_TroopProxy.GetAllTransportInfos();
            var transportInfo = lstTransportInfo.Find((data) => data.transportIndex == index + 1);
            var allLoad = 0L;
            if (transportInfo != null)
            {
                foreach (var v in transportInfo.transportResourceInfo)
                {
                    allLoad += v.load;
                    if (v.load <= 0)
                        continue;
                    Debug.Log("v.load" + v.load);
                    Debug.Log("transportInfo.transportResourceInfo.Count" + transportInfo.transportResourceInfo.Count);
                    CoreUtils.assetService.Instantiate("UI_Model_Resources", (go) =>
                    {
                        go.transform.SetParent(view.m_UI_Item_ArmyShowLayOut.m_root_RectTransform);
                        go.transform.localScale = Vector3.one;

                        UI_Model_Resources_SubView SubView =
                            new UI_Model_Resources_SubView(go.GetComponent<RectTransform>());
                        SubView.SetRes((int)v.resourceTypeId, v.load,0);
                    });
                }
            }

            view.m_lbl_count2_LanguageText.text = LanguageUtils.getTextFormat(184004, allLoad.ToString("N0"));

            var txt = RS.TransportNameIndex[index];
            view.m_lbl_name2_LanguageText.text = LanguageUtils.getTextFormat(184002, txt);

            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            ClientUtils.LoadSprite(view.m_img_char_PolygonImage, configDefine.transportIcon);
        }

        private void CreateSoldierItem(int index)
        {
            view.m_UI_Item_ArmyShowLayOut.m_root_RectTransform.DestroyAllChild();
            TroopMainCreateData info = m_TroopMainData.GetData(index);
            if (info != null)
            {
                List<SoldierInfo> soldierInfos = info.ArmyInfo.soldiers.Values.ToList();
                soldierInfos.Sort((x, y) =>
                {
                    if (y.level.CompareTo(x.level) != 0)
                        return y.level.CompareTo(x.level);
                    else
                        return x.type.CompareTo(y.type);
                });
                soldierInfos.ForEach((item) =>
                {
                    CoreUtils.assetService.Instantiate("UI_Item_SoldierHead", (go) =>
                    {
                        go.transform.SetParent(view.m_UI_Item_ArmyShowLayOut.m_root_RectTransform);
                        go.transform.localScale = Vector3.one;
                        UI_Item_SoldierHead_SubView SubView =
                            new UI_Item_SoldierHead_SubView(go.GetComponent<RectTransform>());
                        int id = m_SoldierProxy.GetTemplateId((int)item.type, (int)item.level);
                        ArmsDefine armsDefineCfg = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                        if (armsDefineCfg != null)
                        {
                            SubView.SetSoldierInfo(armsDefineCfg.icon, (int)item.num);
                        }
                    });
                });
                int itemCount = 0;
                itemCount = soldierInfos.Count;
                int minorSoldierCount = GetMinorSoldierCount(info.ArmyInfo);
                if (info.ArmyInfo.minorSoldiers != null && minorSoldierCount != 0)
                {
                    CoreUtils.assetService.Instantiate("UI_Item_SoldierHead", (go) =>
                    {
                        go.transform.SetParent(view.m_UI_Item_ArmyShowLayOut.m_root_RectTransform);
                        go.transform.localScale = Vector3.one;
                        go.SetActive(true);
                        UI_Item_SoldierHead_SubView SubView =
                            new UI_Item_SoldierHead_SubView(go.GetComponent<RectTransform>());
                        CivilizationDefine define =
                            CoreUtils.dataService.QueryRecord<CivilizationDefine>(
                                (int)m_playerProxy.GetCivilization());

                        if (define.hospitalMark == 0)
                        {
                            SubView.SetSoldierInfo(RS.minorSolderIcon_0, minorSoldierCount);
                        }
                        else
                        {
                            SubView.SetSoldierInfo(RS.minorSolderIcon_1, minorSoldierCount);
                        }
                    });
                    itemCount++;
                }
                itemCount = itemCount % 4 == 0 ? itemCount / 4 : (itemCount / 4) + 1;
                float height = (itemCount * view.m_UI_Item_ArmyShowLayOut.m_UI_Item_ArmyShowLayOut_GridLayoutGroup.cellSize.y);
                view.m_sv_list_ListView.listContainer.sizeDelta = new Vector2(view.m_sv_list_ListView.listContainer.sizeDelta.x, height);
                view.m_lbl_armyCount_LanguageText.text = LanguageUtils.getTextFormat(200037, GetSoldierCount(info.ArmyInfo).ToString("N0"));

                view.m_UI_Model_CaptainHeadWithLevel_main_show.SetIcon(info.hero.config.heroIcon);
                view.m_UI_Model_CaptainHeadWithLevel_main_show.SetLevel(info.hero.level);
                view.m_UI_Model_CaptainHeadWithLevel_main_show.SetRare(info.hero.config.rare);

                HeroProxy.Hero subHero = m_HeroProxy.GetHeroByID(info.ArmyInfo.deputyHeroId);
                if (subHero != null)
                {
                    view.m_UI_Model_CaptainHeadWithLevel_sub_show.gameObject.SetActive(true);
                    view.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300001, LanguageUtils.getText(info.hero.config.l_nameID), LanguageUtils.getText(subHero.config.l_nameID));
                    view.m_UI_Model_CaptainHeadWithLevel_sub_show.SetIcon(subHero.config.heroIcon);
                    view.m_UI_Model_CaptainHeadWithLevel_sub_show.SetRare(subHero.config.rare);
                    view.m_UI_Model_CaptainHeadWithLevel_sub_show.SetLevel(subHero.level);
                    view.m_UI_Model_CaptainHeadWithLevel_sub_show.SetRare(subHero.config.rare);
                }
                else
                {
                    view.m_UI_Model_CaptainHeadWithLevel_sub_show.gameObject.SetActive(false);
                    view.m_lbl_name_LanguageText.text = LanguageUtils.getText(info.hero.config.l_nameID);
                }
            }
        }

        private void ListItemByIndex(ListView.ListItem listItem)
        {
            int index = listItem.index;
            TroopMainCreateData info = m_TroopMainData.GetData(index);
            if (info.type == TroopMainCreateDataType.Troop)
            {
                ShowTroopItemView(listItem, info, index);
            }
            else if (info.type == TroopMainCreateDataType.Scout)
            {
                ShowScoutItemView(listItem, info, index);
            }
            else if (info.type == TroopMainCreateDataType.Transport)
            {
                ShowTransportItemView(listItem, info, index);
            }
        }

        private void ShowTroopItemView(ListView.ListItem listItem, TroopMainCreateData info, int index)
        {
            if (info.hero == null || info.hero.config == null) return;
            if (info.ArmyInfo == null) return;

            UI_Item_ArmyShowView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ArmyShowView>(listItem.go);
            itemView.m_pl_army_ArabLayoutCompment.gameObject.SetActive(true);
            itemView.m_pl_transport_ArabLayoutCompment.gameObject.SetActive(false);
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(info.id);
            Troops formation = null;
            if (armyData != null)
            {
                formation =
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
            }
            else
            {
                Debug.Log(" armyData = null");
            }

            itemView.m_btn_info_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_info_GameButton.onClick.AddListener(() => { ShowDetail(index); });

            itemView.m_btn_return_GameButton.gameObject.SetActive(IsCanReturn_Back(info.ArmyInfo.status));
            itemView.m_btn_return_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_return_GameButton.onClick.AddListener(() =>
            {
                TroopMainCreateData info1 = m_TroopMainData.GetData(index);
                if (info1 != null)
                {
                    //播放音效
                    if (info1.hero != null && info1.hero.config != null)
                    {
                        CoreUtils.audioService.PlayOneShot(info1.hero.config.voiceMove);
                    }
                    if ((TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.REINFORCE_MARCH) || TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.GARRISONING)) && info.ArmyInfo.targetArg != null && info.ArmyInfo.targetArg.targetObjectType == 3)
                    {
                        Rally_RepatriationReinforce.request request = new Rally_RepatriationReinforce.request();

                        GuildMemberInfoEntity guildMemberInfoEntity = m_allianceProxy.getMemberByMapID(info.ArmyInfo.targetArg.targetObjectIndex);
                        if (guildMemberInfoEntity != null)
                        {
                            request.repatriationRid = guildMemberInfoEntity.rid;
                            request.repatriationArmyIndex = info1.id;
                            request.fromObjectIndex = guildMemberInfoEntity.cityObjectIndex;
                            request.isSelfBack = true;
                            AppFacade.GetInstance().SendSproto(request);
                        }
                    }
                    else
                    {
                        var sp = new Map_March.request();
                        sp.armyIndex = info1.id;
                        sp.targetType = 5;
                        AppFacade.GetInstance().SendSproto(sp);
                    }
                }

                itemView.m_btn_return_GameButton.onClick.RemoveAllListeners();
            });

            itemView.m_UI_Model_Link.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
            itemView.m_UI_Model_Link.m_btn_treaty_GameButton.onClick.AddListener(() =>
            {
                TouchTroopSelectParam param = new TouchTroopSelectParam();
                param.armIndex = info.id;
                param.isOpenWin = true;
                AppFacade.GetInstance().SendNotification(CmdConstant.TouchTroopSelect, param);
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });
            itemView.m_UI_Model_Link_collect.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
            itemView.m_UI_Model_Link_collect.m_btn_treaty_GameButton.onClick.AddListener(() =>
            {
                TouchTroopSelectParam param = new TouchTroopSelectParam();
                param.armIndex = info.id;
                param.isOpenWin = true;
                AppFacade.GetInstance().SendNotification(CmdConstant.TouchTroopSelect, param);
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });

            itemView.m_UI_Model_CaptainHeadWithLevel_main.SetIcon(info.hero.config.heroIcon);
            itemView.m_UI_Model_CaptainHeadWithLevel_main.SetLevel(info.hero.level);
            itemView.m_UI_Model_CaptainHeadWithLevel_main.SetRare(info.hero.config.rare);
            ClientUtils.LoadSprite(itemView.m_img_state_PolygonImage, info.icon);
            itemView.m_UI_Common_TroopsState.SetData((long)info.armyStatus);

            HeroProxy.Hero subHero = m_HeroProxy.GetHeroByID(info.ArmyInfo.deputyHeroId);
            if (subHero != null)
            {
                itemView.m_pl_head2_ArabLayoutCompment.gameObject.SetActive(true);
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300001,
                    LanguageUtils.getText(info.hero.config.l_nameID), LanguageUtils.getText(subHero.config.l_nameID));
                itemView.m_UI_Model_CaptainHeadWithLevel_sub.SetIcon(subHero.config.heroIcon);
                itemView.m_UI_Model_CaptainHeadWithLevel_sub.SetRare(subHero.config.rare);
                itemView.m_UI_Model_CaptainHeadWithLevel_sub.SetLevel(subHero.level);
                itemView.m_UI_Model_CaptainHeadWithLevel_sub.SetRare(subHero.config.rare);
            }
            else
            {
                itemView.m_pl_head2_ArabLayoutCompment.gameObject.SetActive(false);
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(info.hero.config.l_nameID);
            }

            if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.FAILED_MARCH))
            {
                itemView.m_UI_Model_CaptainHeadWithLevel_main.m_UI_Model_CaptainHead_GrayChildrens.Gray();
                itemView.m_UI_Model_CaptainHeadWithLevel_sub.m_UI_Model_CaptainHead_GrayChildrens.Gray();
            }
            else
            {
                itemView.m_UI_Model_CaptainHeadWithLevel_sub.m_UI_Model_CaptainHead_GrayChildrens.Normal();
                itemView.m_UI_Model_CaptainHeadWithLevel_main.m_UI_Model_CaptainHead_GrayChildrens.Normal();
            }

            if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.COLLECTING))
            {
                itemView.m_img_frame_PolygonImage.gameObject.SetActive(true);
                itemView.m_img_place_PolygonImage.gameObject.SetActive(false);
                ResourceCollectInfo resourceCollectInfo = info.ArmyInfo.collectResource;
                ResourceGatherTypeDefine resourceGatherTypeDefine =
                    CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>(
                        (int)resourceCollectInfo.resourceTypeId);
                if (resourceGatherTypeDefine != null)
                {
                    ClientUtils.LoadSprite(itemView.m_img_collect_PolygonImage,
                        GetIconByResType((EnumResType)resourceGatherTypeDefine.type));
                }
                else
                {
                    long guildBuildType = resourceCollectInfo.guildBuildType;
                    AllianceBuildingTypeDefine define = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)resourceCollectInfo.guildBuildType);
                    if (define != null)
                    {
                        ClientUtils.LoadSprite(itemView.m_img_collect_PolygonImage,
                       GetIconByAllianceBuildingType((int)guildBuildType));
                    }
                }
            }
            else
            {
                itemView.m_img_frame_PolygonImage.gameObject.SetActive(false);
                itemView.m_img_place_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(itemView.m_img_place_PolygonImage, RS.Atrix_icon_1001);
                if (formation != null && formation.gameObject != null)
                {
                    itemView.m_UI_Model_Link.SetLinkText(LanguageUtils.getTextFormat(300032,
                        Mathf.FloorToInt(formation.gameObject.transform.position.x / 6),
                        Mathf.FloorToInt(formation.gameObject.transform.position.z / 6)));
                    itemView.m_UI_Model_Link_collect.SetLinkText(LanguageUtils.getTextFormat(300032,
                        Mathf.FloorToInt(formation.gameObject.transform.position.x / 6),
                        Mathf.FloorToInt(formation.gameObject.transform.position.z / 6)));
                }
            }

            if (m_timers.ContainsKey(index))
            {
                m_timers[index].Cancel();
                m_timers[index] = null;
            }

            RefreshItemView(itemView, info, formation);
            m_timers[index] =
                Timer.Register(1, () => { RefreshItemView(itemView, info, formation); }, null, true, true);

            itemView.m_lbl_armyCount_LanguageText.text =
                LanguageUtils.getTextFormat(200037, GetSoldierCount(info.ArmyInfo).ToString("N0"));
            if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.ATTACK_MARCH) ||
                TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.COLLECT_MARCH))

            {
                RefreshMarchTargetInfo(itemView, info);
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.PALLY_MARCH) ||
               TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RALLY_WAIT) ||
               TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RALLY_JOIN_MARCH))
            {
                RefreshRallyTargetInfo(itemView, info);
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RETREAT_MARCH) ||
            TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.FAILED_MARCH)
        )
            {
                RefreshReturnTargetInfo(itemView, info);
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.REINFORCE_MARCH) )
            {
                RefreshMarchTargetInfo(itemView, info);
            }
            else
            {
                itemView.m_pl_pos_ArabLayoutCompment.gameObject.SetActive(false);
            }
        }

        private void ShowScoutItemView(ListView.ListItem listItem, TroopMainCreateData info, int index)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetScoutDataByScoutId((int)info.id);
            Troops formation = null;
            if (armyData != null)
            {
                formation =
                    WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(armyData.objectId) as Troops;
            }

            var soutInfo = m_scoutProxy.GetSoutInfoById(info.id);
            if (soutInfo == null)
            {
                return;
            }

            UI_Item_ScoutView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ScoutView>(listItem.go);
            itemView.m_UI_btn_Yellow.gameObject.SetActive(false);
            itemView.m_UI_btn_Blue.gameObject.SetActive(true);
            itemView.m_UI_btn_Blue.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
            itemView.m_UI_btn_Blue.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                TouchNotTroopSelect touchSelectScout = new TouchNotTroopSelect();
                touchSelectScout.id = info.id;
                touchSelectScout.isAutoMove = false;
                AppFacade.GetInstance().SendNotification(CmdConstant.TouchScoutSelect, touchSelectScout);
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });

            if (formation != null)
            {
                if (formation.gameObject != null)
                {
                    itemView.m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032,
                        Mathf.FloorToInt(formation.gameObject.transform.position.x / 6),
                        Mathf.FloorToInt(formation.gameObject.transform.position.z / 6)));
                }
            }
            else
            {
                var pos = armyData.GetMovePos();
                itemView.m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032,
                    Mathf.FloorToInt(pos.x / 6),
                    Mathf.FloorToInt(pos.y / 6)));
            }

            itemView.m_pl_Link.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
            itemView.m_pl_Link.m_btn_treaty_GameButton.onClick.AddListener(() =>
            {
                TouchNotTroopSelect touchSelectScout = new TouchNotTroopSelect();
                touchSelectScout.id = info.id;
                touchSelectScout.isAutoMove = false;
                AppFacade.GetInstance().SendNotification(CmdConstant.TouchScoutSelect, touchSelectScout);
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });

            ClientUtils.LoadSprite(itemView.m_img_state_PolygonImage, info.icon);

            //   Debug.Log((ScoutProxy.ScoutState)soutInfo.state + " " + soutInfo.id);
            itemView.m_lbl_name_LanguageText.text = m_scoutProxy.GetNameById(soutInfo.id);
            ClientUtils.LoadSprite(itemView.m_UI_Model_CaptainHead.m_img_char_PolygonImage, info.scoutIcon);
            if (m_timers.ContainsKey(index))
            {
                m_timers[index].Cancel();
                m_timers[index] = null;
            }
            RefreshItemView(itemView, info, formation, soutInfo);
            m_timers[index] = Timer.Register(1, () =>
            {
                if (info != null)
                {
                    RefreshItemView(itemView, info, formation, soutInfo);
                }
                else
                {
                    m_timers[index].Cancel();
                    m_timers[index] = null;
                }
            }, null, true, true);
        }

        private void ShowTransportItemView(ListView.ListItem listItem, TroopMainCreateData info, int index)
        {

            UI_Item_ArmyShowView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ArmyShowView>(listItem.go);
            itemView.m_pl_army_ArabLayoutCompment.gameObject.SetActive(false);
            itemView.m_pl_transport_ArabLayoutCompment.gameObject.SetActive(true);
            ClientUtils.LoadSprite(itemView.m_img_state2_PolygonImage, RS.StateTransport);

            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            ClientUtils.LoadSprite(itemView.m_img_char_PolygonImage, configDefine.transportIcon);

            var lstTransportInfo = m_TroopProxy.GetAllTransportInfos();
            var transportInfo = lstTransportInfo.Find((data) => data.transportIndex == info.id);
            //资源量
            var allLoad = 0L;
            foreach (var v in transportInfo.transportResourceInfo)
            {
                allLoad += v.load;
            }

            itemView.m_lbl_count2_LanguageText.text = LanguageUtils.getTextFormat(184004, allLoad.ToString("N0"));

            var txt = RS.TransportNameIndex[index];
            itemView.m_lbl_name2_LanguageText.text = LanguageUtils.getTextFormat(184002, txt);

            //目标城市
            itemView.m_UI_Model_Link_transport.SetLinkText(
                LanguageUtils.getTextFormat(300056, transportInfo.targetName));
            itemView.m_UI_Model_Link_transport.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
            itemView.m_UI_Model_Link_transport.m_btn_treaty_GameButton.onClick.AddListener(() =>
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
                var worldPos = WorldMapObjectProxy.ServerPosToWorldPos(transportInfo.targetPos);
                WorldCamera.Instance().ViewTerrainPos(worldPos.x, worldPos.z, 1000, null);
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });
            itemView.m_UI_Model_Link_collect.SetLinkText(
              LanguageUtils.getTextFormat(300056, transportInfo.targetName));
            itemView.m_UI_Model_Link_collect.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
            itemView.m_UI_Model_Link_collect.m_btn_treaty_GameButton.onClick.AddListener(() =>
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
                var worldPos = WorldMapObjectProxy.ServerPosToWorldPos(transportInfo.targetPos);
                WorldCamera.Instance().ViewTerrainPos(worldPos.x, worldPos.z, 1000, null);
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });
            Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                .GetTroop((int)transportInfo.objectIndex);
            //运输车位置
            // itemView.m_UI_Model_Link.SetLinkText(LanguageUtils.getTextFormat(300032, Mathf.FloorToInt(formation.gameObject.transform.position.x / 6), Mathf.FloorToInt(formation.gameObject.transform.position.z / 6)));
            itemView.m_UI_Model_Link.m_btn_treaty_GameButton.onClick.RemoveAllListeners();
            itemView.m_UI_Model_Link.m_btn_treaty_GameButton.onClick.AddListener(() =>
            {
                TouchNotTroopSelect param = new TouchNotTroopSelect();
                param.id = info.id;
                param.isShowSelectHud = false;
                AppFacade.GetInstance().SendNotification(CmdConstant.TouchTransportSelect, param);
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });
            itemView.m_img_frame_PolygonImage.gameObject.SetActive(false);
            itemView.m_img_place_PolygonImage.gameObject.SetActive(true);
            //运输车样式的图
            ClientUtils.LoadSprite(itemView.m_img_place_PolygonImage, RS.Atrix_icon_1003);
            //信息
            itemView.m_btn_info2_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_info2_GameButton.onClick.AddListener(() =>
            {
                ShowDetail(index);
                // ShowShow(index);
            });
            //返回按钮
            itemView.m_btn_return_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_return_GameButton.onClick.AddListener(() =>
            {
                if (m_playerProxy.CurrentRoleInfo.rid == transportInfo.targetRid)
                {
                    return;
                }
                Transport_TransportBack.request rep1 = new Transport_TransportBack.request();
                rep1.objectIndex = transportInfo.objectIndex;
                AppFacade.GetInstance().SendSproto(rep1);
            });

            if (m_timers.ContainsKey(index))
            {
                if (m_timers[index] != null)
                {
                    m_timers[index].Cancel();
                    m_timers[index] = null;
                }
            }

            if (formation != null)
            {
                if (formation.gameObject != null)
                {
                    RefreshItemView_Transport(itemView, transportInfo.arrivalTime,
                        (int)formation.gameObject.transform.position.x / 6,
                        (int)formation.gameObject.transform.position.z / 6);
                }
            }

            //剩余时间   
            m_timers[index] = Timer.Register(1, () =>
            {
                if (transportInfo != null && formation != null&& formation.gameObject!=null)
                {
                    RefreshItemView_Transport(itemView, transportInfo.arrivalTime,
                        (int)formation.gameObject.transform.position.x / 6,
                        (int)formation.gameObject.transform.position.z / 6);
                }
                else
                {
                    m_timers[index].Cancel();
                    m_timers[index] = null;
                }
            }, null, true, true);
        }

        private RallyDetailEntity GetRallyDetailInfo(int armyIndex)
        {
            var rallyId = m_rallyTroopProxy.GetRallyDetailEntityByarmIndex(armyIndex);
            if (rallyId == 0) return null;
            return m_rallyTroopProxy.GetRallyDetailEntity(rallyId);
        }
        /// <summary>
        /// 目标位置刷新部队处于行军中状态且不是向空地行军时才显示该项，显示目的地对象的名称，点击后关闭界面选中该对象。
        /// </summary>
        /// <param name="itemView"></param>
        /// <param name="info"></param>
        private void RefreshMarchTargetInfo(UI_Item_ArmyShowView itemView, TroopMainCreateData info)
        {
            itemView.m_pl_pos_ArabLayoutCompment.gameObject.SetActive(true);
            string targetText = "          ";
            long targetX = -1;
            long targetY = -1;

            if (info.ArmyInfo.targetArg != null)
            {
                RssType objectType = (RssType)info.ArmyInfo.targetArg.targetObjectType;
                if (objectType == RssType.None)
                {
                    return;
                }
                if (info.ArmyInfo.targetArg.targetPos != null)
                {
                    targetX = info.ArmyInfo.targetArg.targetPos.x;
                    targetY = info.ArmyInfo.targetArg.targetPos.y;
                }
                else
                {
                    if (info.ArmyInfo.path != null && info.ArmyInfo.path.Count > 0)
                    {
                        targetX = info.ArmyInfo.path[info.ArmyInfo.path.Count - 1].x;
                        targetY = info.ArmyInfo.path[info.ArmyInfo.path.Count - 1].y;
                    }
                }
                switch (objectType)
                {
                    case RssType.GuildCenter:
                    case RssType.GuildFortress1:
                    case RssType.GuildFortress2:
                    case RssType.GuildFlag:
                    case RssType.GuildFoodResCenter://20 联盟谷仓  资源中心
                    case RssType.GuildWoodResCenter: //联盟木料场
                    case RssType.GuildGoldResCenter://联盟石材厂
                    case RssType.GuildGemResCenter: //23联盟铸币场
                        {
                            int configType = m_allianceProxy.GetBuildServerTypeToConfigType((long)objectType);
                            var allianceBuildingCfg = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(configType);
                            itemView.m_pl_pos_ArabLayoutCompment.gameObject.SetActive(allianceBuildingCfg != null);
                            if (allianceBuildingCfg != null)
                            {
                                targetText = LanguageUtils.getText(allianceBuildingCfg.l_nameId);
                            }
                        }
                        break;
                    case RssType.City:
                        {
                            bool hasTargetName = false;
                            if (!string.IsNullOrEmpty(info.ArmyInfo.targetArg.targetName))
                            {
                                hasTargetName = true;
                            }
                            if (hasTargetName)
                            {
                                if (string.IsNullOrEmpty(info.ArmyInfo.targetArg.targetGuildName) || info.ArmyInfo.targetArg.targetGuildName == "")
                                {
                                    targetText = LanguageUtils.getTextFormat(730317, info.ArmyInfo.targetArg.targetName);
                                }
                                else
                                {
                                    targetText = LanguageUtils.getTextFormat(730317, LanguageUtils.getTextFormat(300030, info.ArmyInfo.targetArg.targetGuildName, info.ArmyInfo.targetArg.targetName));
                                }
                            }
                            else
                            {
                                Debug.LogError("info.ArmyInfo.targetArg.targetName ==");
                            }

                        }
                        break;
                    case RssType.Stone:
                    case RssType.Farmland:
                    case RssType.Wood:
                    case RssType.Gold:
                    case RssType.Gem:
                        {
                                ResourceGatherTypeDefine resourceGatherTypeDefine = CoreUtils.dataService.QueryRecord<ResourceGatherTypeDefine>((int)info.ArmyInfo.targetArg.targetResourceId);
                                if (resourceGatherTypeDefine != null)
                                {
                                    targetText = LanguageUtils.getText(resourceGatherTypeDefine.l_nameId);
                                }
                        }
                        break;
                    case RssType.Troop:
                        {
                            bool hasTargetName = false;
                            if (!string.IsNullOrEmpty(info.ArmyInfo.targetArg.targetName))
                            {
                                hasTargetName = true;
                            }
                            if (hasTargetName)
                            {
                                if (string.IsNullOrEmpty(info.ArmyInfo.targetArg.targetGuildName) || info.ArmyInfo.targetArg.targetGuildName == "")
                                {
                                    targetText = LanguageUtils.getTextFormat(200113, info.ArmyInfo.targetArg.targetName);
                                }
                                else
                                {
                                    targetText = LanguageUtils.getTextFormat(200113, LanguageUtils.getTextFormat(300030, info.ArmyInfo.targetArg.targetGuildName, info.ArmyInfo.targetArg.targetName));
                                }
                            }
                        }
                        break;
                    case RssType.Sanctuary:
                    case RssType.Altar:
                    case RssType.Shrine:
                    case RssType.LostTemple:
                    case RssType.HolyLand:
                    case RssType.CheckPoint:
                    case RssType.Checkpoint_1:
                    case RssType.Checkpoint_2:
                    case RssType.Checkpoint_3:
                        {
                            var strongHoldDataCfg = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)info.ArmyInfo.targetArg.targetHolyLandId);
                            if (strongHoldDataCfg != null)
                            {
                                var strongHoldTypeCfg = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataCfg.type);
                                targetText = LanguageUtils.getText(strongHoldTypeCfg.l_nameId);
                            }
                        }
                        break;
                    case RssType.Monster:
                    case RssType.SummonAttackMonster:
                    case RssType.SummonConcentrateMonster:
                    case RssType.Guardian:
                        {
                            var targetMonsterIdcfg = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)info.ArmyInfo.targetArg.targetMonsterId);
                            if (targetMonsterIdcfg != null)
                            {
                                targetText = LanguageUtils.getText(targetMonsterIdcfg.l_nameId);
                            }
                        }
                        break;
                    case RssType.Rune:
                        {
                            var MapItemTypecfg = CoreUtils.dataService.QueryRecord<MapItemTypeDefine>((int)info.ArmyInfo.targetArg.targetMapItemType);
                            if (MapItemTypecfg != null)
                            {
                                targetText = LanguageUtils.getText(MapItemTypecfg.l_nameId);
                            }
                        }
                        break;
                    default:
                        targetText = LanguageUtils.getTextFormat(200505, PosHelper.ServerUnitToClientPos(targetX), PosHelper.ServerUnitToClientPos(targetY));
                        break;
                }
            }
            itemView.m_UI_Model_Link_transport1.SetLinkText(targetText);
            itemView.m_UI_Model_Link_transport1.RemoveAllClickEvent();
            if (targetX != -1)
            {
                itemView.m_UI_Model_Link_transport1.AddClickEvent(() =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosPullUpCameraDxf,new Vector2(PosHelper.ServerUnitToClientUnit(targetX), PosHelper.ServerUnitToClientUnit(targetY)));
                    CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
                });
            }
        }
        private void RefreshReturnTargetInfo(UI_Item_ArmyShowView itemView, TroopMainCreateData info)
        {
            itemView.m_pl_pos_ArabLayoutCompment.gameObject.SetActive(true);
            string targetText = string.Empty;
            targetText = LanguageUtils.getText(300110);
            itemView.m_UI_Model_Link_transport1.SetLinkText(targetText);
            itemView.m_UI_Model_Link_transport1.RemoveAllClickEvent();
            itemView.m_UI_Model_Link_transport1.AddClickEvent(() =>
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosPullUpCameraDxf, new Vector2(PosHelper.ServerUnitToClientUnit(m_playerProxy.CurrentRoleInfo.pos.x), PosHelper.ServerUnitToClientUnit(m_playerProxy.CurrentRoleInfo.pos.y)));
                CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
            });
        }
        /// <summary>
        /// 目标位置刷新部队处于行军中状态且不是向空地行军时才显示该项，显示目的地对象的名称，点击后关闭界面选中该对象。
        /// </summary>
        /// <param name="itemView"></param>
        /// <param name="info"></param>
        private void RefreshRallyTargetInfo(UI_Item_ArmyShowView itemView, TroopMainCreateData info)
        {
            itemView.m_pl_pos_ArabLayoutCompment.gameObject.SetActive(true);
            var rallyDetailInfo = GetRallyDetailInfo(info.id);
            if (rallyDetailInfo == null) return;
            string targetText = string.Empty;
            if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.PALLY_MARCH))
            {
                itemView.m_pl_pos_ArabLayoutCompment.gameObject.SetActive(false);
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RALLY_JOIN_MARCH))
            {
                var joinRallyDetail = m_rallyTroopProxy.GetRallyDetailEntityByArmIndex(info.id);
                //集结部队已经有了地图对象，说明集结部队已经出发了，自己的部队在追着集结部队走
                if (rallyDetailInfo.rallyObjectIndex > 0)
                {
                    if (string.IsNullOrEmpty(rallyDetailInfo.rallyGuildName) || rallyDetailInfo.rallyGuildName == "")
                    {
                        targetText = LanguageUtils.getTextFormat(200097, rallyDetailInfo.rallyName);
                    }
                    else
                    {
                        targetText = LanguageUtils.getTextFormat(200097,
                           LanguageUtils.getTextFormat(300030, rallyDetailInfo.rallyGuildName, rallyDetailInfo.rallyName));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(rallyDetailInfo.rallyGuildName) || rallyDetailInfo.rallyGuildName == "")
                    {
                        targetText = LanguageUtils.getTextFormat(730317, rallyDetailInfo.rallyName);
                    }
                    else
                    {
                        targetText = LanguageUtils.getTextFormat(730317,
                                                  LanguageUtils.getTextFormat(300030, rallyDetailInfo.rallyGuildName, rallyDetailInfo.rallyName));
                    }
                }
                itemView.m_UI_Model_Link_transport1.RemoveAllClickEvent();
                itemView.m_UI_Model_Link_transport1.AddClickEvent(() =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosPullUpCameraDxf, new Vector2(PosHelper.ServerUnitToClientUnit(rallyDetailInfo.rallyPos.x), PosHelper.ServerUnitToClientUnit(rallyDetailInfo.rallyPos.y)));
                    CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
                });
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RALLY_WAIT))
            {
                var rallyTargetDetail = rallyDetailInfo.rallyTargetDetail;
                RssType objectType = (RssType)rallyTargetDetail.rallyTargetType;
                switch (objectType)
                {
                    case RssType.BarbarianCitadel:
                        var monsterCfg =
                            CoreUtils.dataService.QueryRecord<MonsterDefine>((int)rallyTargetDetail
                                .rallyTargetMonsterId);
                        if (monsterCfg != null)
                        {
                            targetText = LanguageUtils.getText(monsterCfg.l_nameId);
                        }

                        break;
                    case RssType.City:
                        if (string.IsNullOrEmpty(rallyTargetDetail.rallyTargetGuildName) || rallyTargetDetail.rallyTargetGuildName == "")
                        {
                            targetText = LanguageUtils.getTextFormat(730317, rallyTargetDetail.rallyTargetName);
                        }
                        else
                        {
                            targetText = LanguageUtils.getTextFormat(730317,
                                                     LanguageUtils.getTextFormat(300030, rallyTargetDetail.rallyTargetGuildName, rallyTargetDetail.rallyTargetName));
                        }
                        break;
                    case RssType.GuildCenter:
                    case RssType.GuildFortress1:
                    case RssType.GuildFortress2:
                    case RssType.GuildFlag:
                        //如果是联盟建筑，显示联盟建筑名字
                        var type = m_allianceProxy.GetBuildServerTypeToConfigType(rallyTargetDetail.rallyTargetType);
                        var allianceBuildingCfg = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);
                        if (allianceBuildingCfg != null)
                        {
                            targetText = LanguageUtils.getText(allianceBuildingCfg.l_nameId);
                        }
                        break;
                    case RssType.Sanctuary:
                    case RssType.Altar:
                    case RssType.Shrine:
                    case RssType.LostTemple:
                    case RssType.CheckPoint:
                    case RssType.Checkpoint_1:
                    case RssType.Checkpoint_2:
                    case RssType.Checkpoint_3:
                        {
                            var strongHoldDataCfg = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)rallyTargetDetail.rallyTargetHolyLandId);
                            if (strongHoldDataCfg != null)
                            {
                                var strongHoldTypeCfg = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataCfg.type);
                                targetText = LanguageUtils.getText(strongHoldTypeCfg.l_nameId);
                            }
                        }
                        break;
                }
                itemView.m_UI_Model_Link_transport1.RemoveAllClickEvent();
                itemView.m_UI_Model_Link_transport1.AddClickEvent(() =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosPullUpCameraDxf, new Vector2(PosHelper.ServerUnitToClientUnit(rallyTargetDetail.rallyTargetPos.x), PosHelper.ServerUnitToClientUnit(rallyTargetDetail.rallyTargetPos.y)));
                    CoreUtils.uiManager.CloseUI(UI.s_WinArmyShow);
                });
            }

            itemView.m_UI_Model_Link_transport1.SetLinkText(targetText);
        }

        private void RefreshItemView(UI_Item_ScoutView itemView, TroopMainCreateData info, Troops formation,
            ScoutProxy.ScoutInfo scoutInfo)
        {
            switch (scoutInfo.state)
            {
                case ScoutProxy.ScoutState.Return:
                case ScoutProxy.ScoutState.Back_City:
                    {
                        itemView.m_lbl_desc_LanguageText.gameObject.SetActive(true);
                        itemView.m_lbl_barText_LanguageText.gameObject.SetActive(false);
                        itemView.m_pb_rogressBar_GameSlider.value = 0;
                        long leftTime = scoutInfo.endTime - ServerTimeModule.Instance.GetServerTime();
                        leftTime = leftTime < 0 ? 0 : leftTime;
                        itemView.m_lbl_desc_LanguageText.text = string.Format("{0} {1}", LanguageUtils.getText(181152),
                            ClientUtils.FormatCountDown((int)leftTime));
                        if (formation != null&& formation.gameObject!=null)
                        {
                            itemView.m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032,
                                Mathf.FloorToInt(formation.gameObject.transform.position.x / 6),
                                Mathf.FloorToInt(formation.gameObject.transform.position.z / 6)));
                        }
                    }
                    break;
                case ScoutProxy.ScoutState.Surveing:
                    {
                        itemView.m_lbl_desc_LanguageText.gameObject.SetActive(true);
                        itemView.m_lbl_barText_LanguageText.gameObject.SetActive(false);
                        itemView.m_pb_rogressBar_GameSlider.value = 0;
                        long leftTime = scoutInfo.endTime - ServerTimeModule.Instance.GetServerTime();
                        leftTime = leftTime < 0 ? 0 : leftTime;
                        itemView.m_lbl_desc_LanguageText.text = string.Format("{0} {1}", LanguageUtils.getText(181153),
                            ClientUtils.FormatCountDown((int)leftTime));

                        ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetScoutDataByScoutId(scoutInfo.id);
                        if (armyData != null)
                        {
                            Vector2 fixedPos = PosHelper.ClientUnitToClientPos(armyData.GetMovePos());
                            itemView.m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032, fixedPos.x, fixedPos.y));
                        }
                    }
                    break;
                case ScoutProxy.ScoutState.Scouting:
                    {
                        itemView.m_lbl_desc_LanguageText.gameObject.SetActive(true);
                        itemView.m_lbl_barText_LanguageText.gameObject.SetActive(false);
                        itemView.m_pb_rogressBar_GameSlider.value = 0;
                        long leftTime = scoutInfo.endTime - ServerTimeModule.Instance.GetServerTime();
                        leftTime = leftTime < 0 ? 0 : leftTime;
                        itemView.m_lbl_desc_LanguageText.text = string.Format("{0} {1}", LanguageUtils.getText(181162),
                            ClientUtils.FormatCountDown((int)leftTime));
                        if (formation != null&& formation.gameObject!=null)
                        {
                            itemView.m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032,
                                Mathf.FloorToInt(formation.gameObject.transform.position.x / 6),
                                Mathf.FloorToInt(formation.gameObject.transform.position.z / 6)));
                        }
                    }
                    break;
                case ScoutProxy.ScoutState.Fog:
                    {
                        itemView.m_lbl_desc_LanguageText.gameObject.SetActive(false);
                        itemView.m_lbl_barText_LanguageText.gameObject.SetActive(true);
                        var x = scoutInfo.x;
                        var y = scoutInfo.y;

                        FogSystemMediator fogMedia =
                            AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                        var fogs = fogMedia.GetGroupFog(x, y);

                        var pro = fogs.Count * 100 / (WarFogMgr.GROUP_SIZE * WarFogMgr.GROUP_SIZE);

                        itemView.m_lbl_barText_LanguageText.text = LanguageUtils.getTextFormat(181145, 100 - pro);

                        itemView.m_pb_rogressBar_GameSlider.value = 1 - (pro / 100.0f);
                        if (formation != null&& formation.gameObject!=null)
                        {
                            itemView.m_pl_Link.SetLinkText(LanguageUtils.getTextFormat(300032,
                                Mathf.FloorToInt(formation.gameObject.transform.position.x / 6),
                                Mathf.FloorToInt(formation.gameObject.transform.position.z / 6)));
                        }
                    }
                    break;
            }
        }
        private void OnClickTargetBtn(Vector2 vector)
        {
            
        }

        private void RefreshItemView_Transport(UI_Item_ArmyShowView itemView, long arrivalTime, int x, int y)
        {
            long leftTime = arrivalTime - ServerTimeModule.Instance.GetServerTime();
            leftTime = leftTime < 0 ? 0 : leftTime;
            itemView.m_lbl_state2_LanguageText.text =
                LanguageUtils.getTextFormat(184003, ClientUtils.FormatCountDown((int)leftTime));
            itemView.m_UI_Model_Link
                .SetLinkText(LanguageUtils.getTextFormat(300032, x, y));
        }

        private void RefreshItemView(UI_Item_ArmyShowView itemView, TroopMainCreateData info, Troops formation)
        {
            itemView.m_pl_bar_ArabLayoutCompment.gameObject.SetActive(false);
            itemView.m_lbl_state1_LanguageText.gameObject.SetActive(true);
            string text = string.Empty;
            if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.ATTACK_MARCH))
            {
                // itemView.m_pb_rogressBar_GameSlider.value = 0;
                long leftTime = info.ArmyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime();
                leftTime = leftTime < 0 ? 0 : leftTime;
                // itemView.m_lbl_barText_LanguageText.text = string.Format("{0} {1}", LanguageUtils.getText(200061), ClientUtils.FormatCountDown((int)leftTime));
                text = string.Format("{0} {1}", LanguageUtils.getText(200061),
                    ClientUtils.FormatCountDown((int)leftTime));
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.BATTLEING) ||
                TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RALLY_BATTLE))
            {
                //  text = LanguageUtils.getTextFormat(200066,m_TroopProxy.GetTargetNameBytargetType(info.ArmyInfo.targetType, info.ArmyInfo.targetArg));
                text = LanguageUtils.getTextFormat(200111);

            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.COLLECTING))
            {
                long leftTime = info.ArmyInfo.collectResource.endTime - ServerTimeModule.Instance.GetServerTime();
                leftTime = leftTime < 0 ? 0 : leftTime;
                itemView.m_pl_bar_ArabLayoutCompment.gameObject.SetActive(true);
                itemView.m_lbl_state1_LanguageText.gameObject.SetActive(false);
                itemView.m_pb_rogressBar_GameSlider.value =
                    (ServerTimeModule.Instance.GetServerTime() - info.ArmyInfo.collectResource.startTime) /
                    (float)(info.ArmyInfo.collectResource.endTime - info.ArmyInfo.collectResource.startTime);
                text = LanguageUtils.getTextFormat(500127, ClientUtils.FormatCountDown((int)leftTime));
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.COLLECTING_NO_DELETE))
            {
                var mapObjectInfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(info.ArmyInfo.objectIndex);
                if (mapObjectInfo != null)
                {
                    if (mapObjectInfo.collectRuneTime > 0)
                    {
                        ConfigDefine configCfg = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                        long leftTime = mapObjectInfo.collectRuneTime + configCfg.collectCircleTime -
                                        ServerTimeModule.Instance.GetServerTime();
                        leftTime = leftTime < 0 ? 0 : leftTime;
                        // itemView.m_pb_rogressBar_GameSlider.value = (ServerTimeModule.Instance.GetServerTime() - mapObjectInfo.collectRuneTime) / (float)configCfg.collectCircleTime;
                        text = LanguageUtils.getTextFormat(500127, ClientUtils.FormatCountDown((int)leftTime));
                    }
                }
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.COLLECT_MARCH))
            {
                // itemView.m_pb_rogressBar_GameSlider.value = 0;
                long leftTime = info.ArmyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime();
                leftTime = leftTime < 0 ? 0 : leftTime;
                text = string.Format("{0} {1}", LanguageUtils.getText(200062),
                    ClientUtils.FormatCountDown((int)leftTime));
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.FAILED_MARCH))
            {
                // itemView.m_pb_rogressBar_GameSlider.value = 0;
                long leftTime = info.ArmyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime();
                leftTime = leftTime < 0 ? 0 : leftTime;
                text = string.Format("{0} {1}", LanguageUtils.getText(200065),
                    ClientUtils.FormatCountDown((int)leftTime));
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.GARRISONING))
            {
                RssType objectType = (RssType)info.ArmyInfo.targetArg.targetObjectType;
                switch (objectType)
                {
                    case RssType.City:
                        text = LanguageUtils.getTextFormat(200101, info.ArmyInfo.targetArg.targetName);
                        break;
                    case RssType.GuildCenter:
                    case RssType.GuildFortress1:
                    case RssType.GuildFortress2:
                    case RssType.GuildFlag:
                        {
                            var type = m_allianceProxy.GetBuildServerTypeToConfigType(info.ArmyInfo.targetArg.targetObjectType);
                            var allianceBuildingCfg = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(type);
                            if (allianceBuildingCfg != null)
                            {
                                text = LanguageUtils.getTextFormat(200102,
                                    LanguageUtils.getText(allianceBuildingCfg.l_nameId));
                            }
                        }
                        break;
                    case RssType.Sanctuary:
                    case RssType.Altar:
                    case RssType.Shrine:
                    case RssType.LostTemple:
                    case RssType.CheckPoint:
                    case RssType.Checkpoint_1:
                    case RssType.Checkpoint_2:
                    case RssType.Checkpoint_3:
                        {
                            var strongHoldDataCfg = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>((int)info.ArmyInfo.targetArg.targetHolyLandId);
                            if (strongHoldDataCfg != null)
                            {
                                var strongHoldTypeCfg = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataCfg.type);
                                text = LanguageUtils.getText(strongHoldTypeCfg.l_nameId);
                            }
                        }
                        break;
                }
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.PALLY_MARCH))
            {
                var rallyDetailInfo = GetRallyDetailInfo(info.id);
                if (rallyDetailInfo != null)
                {
                    long leftTime = info.ArmyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime();
                    leftTime = leftTime < 0 ? 0 : leftTime;
                    text = LanguageUtils.getTextFormat(200103, ClientUtils.FormatCountDown((int)leftTime));
                }
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.REINFORCE_MARCH))
            {
                // itemView.m_pb_rogressBar_GameSlider.value = 0;
                long leftTime = info.ArmyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime();
                leftTime = leftTime < 0 ? 0 : leftTime;
                text = LanguageUtils.getTextFormat(200108, ClientUtils.FormatCountDown((int)leftTime));
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RETREAT_MARCH))
            {
                long leftTime = info.ArmyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime();
                leftTime = leftTime < 0 ? 0 : leftTime;
                text = string.Format("{0} {1}", LanguageUtils.getText(200064),
                    ClientUtils.FormatCountDown((int)leftTime));
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.SPACE_MARCH))
            {
                // itemView.m_pb_rogressBar_GameSlider.value = 0;
                long leftTime = info.ArmyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime();
                leftTime = leftTime < 0 ? 0 : leftTime;
                text = string.Format("{0} {1}", LanguageUtils.getText(200060),
                    ClientUtils.FormatCountDown((int)leftTime));
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.STATIONING))
            {
                text = LanguageUtils.getText(200067);
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RALLY_JOIN_MARCH))
            {
                long leftTime = info.ArmyInfo.arrivalTime - ServerTimeModule.Instance.GetServerTime();
                leftTime = leftTime < 0 ? 0 : leftTime;
                text = LanguageUtils.getTextFormat(200108, ClientUtils.FormatCountDown((int)leftTime));
            }
            else if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.RALLY_WAIT))
            {
                var rallyDetailInfo = GetRallyDetailInfo(info.id);
                if (rallyDetailInfo != null)
                {
                    if (rallyDetailInfo.rallyReadyTime >= ServerTimeModule.Instance.GetServerTime())
                    {
                        long leftTime = rallyDetailInfo.rallyReadyTime - ServerTimeModule.Instance.GetServerTime();
                        leftTime = leftTime < 0 ? 0 : leftTime;
                        text = LanguageUtils.getTextFormat(200098, ClientUtils.FormatCountDown((int)leftTime));
                    }
                    else
                    {
                        long leftTime = rallyDetailInfo.rallyWaitTime - ServerTimeModule.Instance.GetServerTime();
                        leftTime = leftTime < 0 ? 0 : leftTime;
                        text = LanguageUtils.getTextFormat(200099, ClientUtils.FormatCountDown((int)leftTime));
                    }
                }
            }

            itemView.m_lbl_state1_LanguageText.text = text;
            if (TroopHelp.IsHaveState(info.ArmyInfo.status, ArmyStatus.COLLECTING))
            {
                itemView.m_lbl_barText_LanguageText.text = text;
            }

            Vector2Int pos = Vector2Int.zero;


            if (info.ArmyInfo.targetArg != null && info.ArmyInfo.targetArg.pos != null)
            {
                pos = PosHelper.ServerUnitToClientPos(info.ArmyInfo.targetArg.pos);
            }
            else if (formation != null)
            {
                if (formation.gameObject != null)
                {
                    pos = PosHelper.ClientUnitToClientPos(formation.gameObject.transform.position);
                }

            }
            else
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId((int)info.id);
                if (armyData != null)
                {
                    pos = PosHelper.ClientUnitToClientPos(armyData.GetMovePos());
                }
            }


            itemView.m_UI_Model_Link.SetLinkText(LanguageUtils.getTextFormat(300032, pos.x, pos.y));
            itemView.m_UI_Model_Link_collect.SetLinkText(LanguageUtils.getTextFormat(300032, pos.x, pos.y));
        }

        #endregion

        private bool IsCanReturn_Back(long status)
        {
            //已经返回
            if (TroopHelp.IsHaveState(status, ArmyStatus.RETREAT_MARCH) ||
                       TroopHelp.IsHaveState(status, ArmyStatus.FAILED_MARCH))
            {
                return false;
            }

            if (TroopHelp.IsHaveState(status, ArmyStatus.PALLY_MARCH) ||
                TroopHelp.IsHaveState(status, ArmyStatus.RALLY_WAIT) ||
                TroopHelp.IsHaveState(status, ArmyStatus.BATTLEING) ||
                TroopHelp.IsHaveState(status, ArmyStatus.RALLY_BATTLE))
            {
                return false;
            }

            return true;
        }

        private int GetSoldierCount(ArmyInfoEntity armyInfo)
        {
            long count = 0;
            if (armyInfo.soldiers != null)
            {
                foreach (var soldier in armyInfo.soldiers.Values)
                {
                    count += soldier.num;
                }
            }

            if (armyInfo.minorSoldiers != null)
            {
                foreach (var soldier in armyInfo.minorSoldiers.Values)
                {
                    count += soldier.num;
                }
            }

            return (int)count;
        }

        private int GetMinorSoldierCount(ArmyInfoEntity armyInfo)
        {
            long count = 0;
            if (armyInfo.minorSoldiers != null)
            {
                armyInfo.minorSoldiers.Values.ToList().ForEach((minorSoldier) => { count += minorSoldier.num; });
            }

            return (int)count;
        }
        /// <summary>
        /// 资源田缩略图
        /// </summary>
        /// <param name="ResType"></param>
        /// <returns></returns>
        private string GetIconByResType(EnumResType ResType)
        {
            string icon = string.Empty;
            switch (ResType)
            {
                case EnumResType.Food:
                    icon = "matrix_icon[matrix_icon_2000]";
                    break;
                case EnumResType.Gold:
                    icon = "matrix_icon[matrix_icon_2003]";
                    break;
                case EnumResType.Stone:
                    icon = "matrix_icon[matrix_icon_2002]";
                    break;
                case EnumResType.Wood:
                    icon = "matrix_icon[matrix_icon_2001]";
                    break;
                case EnumResType.Diamond:
                    icon = "matrix_icon[matrix_icon_2004]";
                    break;
                default:
                    {
                        Debug.LogError("not find type");
                    }
                    break;
            }

            return icon;
        }
        /// <summary>
        /// 资源田缩略图
        /// </summary>
        /// <param name="ResType"></param>
        /// <returns></returns>
        private string GetIconByAllianceBuildingType(int AllianceBuildingType)
        {
            string icon = string.Empty;
            if (AllianceBuildingType == 8)
            {
                icon = "matrix_icon[matrix_icon_3002]";
            }
            else if (AllianceBuildingType == 9)
            {
                icon = "matrix_icon[matrix_icon_3003]";

            }
            else if (AllianceBuildingType == 10)
            {
                icon = "matrix_icon[matrix_icon_3004]";

            }
            else if (AllianceBuildingType == 11)
            {
                icon = "matrix_icon[matrix_icon_3005]";

            }
            else
            {
                Debug.LogError("未处理的联盟建筑类型");
            }

            return icon;
        }
    }
}
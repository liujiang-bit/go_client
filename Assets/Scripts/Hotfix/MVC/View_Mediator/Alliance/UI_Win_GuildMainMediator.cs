// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Win_GuildMainMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game
{
    public class UI_Win_GuildMainMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "UI_Win_GuildMainMediator";

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildMainMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public UI_Win_GuildMainView view;

        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;
        private TroopProxy m_troopProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;

        private RallyTroopsProxy m_rallyTroopProxy;

        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private List<AllianceMemberLevel> m_guildMemberList = new List<AllianceMemberLevel>(4);


        private UI_Pop_GuildMemTipView powTipView;

        private bool HasReadRallyTroop = true;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Guild_GuildMemberInfo.TagName,
                Guild_GuildInfo.TagName,
                Guild_GuildNotify.TagName,
                CmdConstant.AllianceEixt,
                CmdConstant.UpdatePlayerPower,
                Guild_GuildApplys.TagName,
                CmdConstant.AllianceSettingWelcomeMailChange,
                CmdConstant.AllianceStudyDonateRedCount,
                Guild_GuildRequestHelps.TagName,
                CmdConstant.AllianceGiftRedPoint,
                CmdConstant.AllianceRssRedPoint,
                CmdConstant.RallyTroopChange,
                Guild_ModifyMemberLevel.TagName,
                CmdConstant.AllianceInfoUpdate,
                CmdConstant.OnCloseUI
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OnCloseUI:
                    
                    m_allianceProxy.SendAllianceInfo(1);
                    break;
                case CmdConstant.RallyTroopChange:
                    CheckRallyRedPoint();
                    break;
                case CmdConstant.AllianceRssRedPoint:
                    CheckRssRedPoint();
                    break;
                case Guild_GuildRequestHelps.TagName:
                    CheckHelpRedPoint();
                    break;
                case CmdConstant.AllianceGiftRedPoint:
                    CheckGiftRedPoint();
                    break;
                case CmdConstant.AllianceStudyDonateRedCount:
                    CheckReareachRedPoint();
                    break;
                case CmdConstant.UpdatePlayerPower:
                case Guild_GuildMemberInfo.TagName:
                case Guild_GuildNotify.TagName:
                case Guild_ModifyMemberLevel.TagName:
                case CmdConstant.AllianceInfoUpdate:
                    CheckBtnRoot();
                    CheckRedGuildApplys();
                    RePlayer();
                    ReMemberList();
                    ReAllianceInfo();
                    CheckHasBuilding();
                    if (m_tipView != null && !notification.Name.Equals(CmdConstant.UpdatePlayerPower))
                    {
                        m_tipView.Close();
                        m_tipView = null;
                    }

                    break;
                case CmdConstant.AllianceEixt:

                    if (notification.Body!=null)
                    {
                        long rid = (long)notification.Body;
                        if (rid == m_playerProxy.CurrentRoleInfo.rid)
                        {
                            CoreUtils.uiManager.CloseGroupUI(UI.ALLIANCE_GRPOP);
                        }
                    }
                    
                    break;
                case Guild_GuildInfo.TagName:
                    RePlayer();
                    ReAllianceInfo();
                    break;
                case Guild_GuildApplys.TagName:
                    CheckRedGuildApplys();

                    break;
                case CmdConstant.AllianceSettingWelcomeMailChange:
                    CheckRedGuildSetting();
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
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.AllianceFuncIntro);
        }

        public override void OnRemove()
        {

        }

        public override void WinClose()
        {
            if (powTipView != null && powTipView.gameObject != null && powTipView.gameObject.transform.parent!=null)
            {
                CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                powTipView = null;
            }
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
        }

        private bool isReinforceMode = false;

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            if (view.data != null && view.data is bool)
            {
                isReinforceMode = (bool) view.data;
            }
            
            
            m_rallyTroopProxy = AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730021));

            view.m_UI_Model_Window_Type3.setCloseHandle(onClose);

            view.m_UI_Side.m_pl_side1.m_ck_ck_GameToggle.onValueChanged.AddListener(OnTagMian);
            view.m_UI_Side.m_pl_side2.m_ck_ck_GameToggle.onValueChanged.AddListener(OnTagMember);
            view.m_UI_Side.m_pl_side3.m_ck_ck_GameToggle.onValueChanged.AddListener(OnTagSetting);


            //page first

            view.m_UI_war.m_btn_btn_GameButton.onClick.AddListener(onWar);
            view.m_UI_terrtroy.m_btn_btn_GameButton.onClick.AddListener(onTerrtroy);
            view.m_UI_holy.m_btn_btn_GameButton.onClick.AddListener(onHoly);
            view.m_UI_help.m_btn_btn_GameButton.onClick.AddListener(onHelp);
            
          
            
            view.m_UI_depot.m_btn_btn_GameButton.onClick.AddListener(onDepot);
            view.m_UI_technology.m_btn_btn_GameButton.onClick.AddListener(OnTech);
            view.m_UI_gift.m_btn_btn_GameButton.onClick.AddListener(onGift);
            view.m_UI_Shop.m_btn_btn_GameButton.onClick.AddListener(onShop);

            view.m_btn_mail_GameButton.onClick.AddListener(onMail);

            view.m_btn_info_GameButton.onClick.AddListener(onGuildInfo);

            view.m_btn_trans_GameButton.onClick.AddListener(onTrans);

            view.m_btn_msgBoard_GameButton.onClick.AddListener(onMsgBoard);


            //page member
            view.m_btn_member_info_GameButton.onClick.AddListener(onOpenLevelAcc);
            view.m_btn_PlayerHead_GameButton.onClick.AddListener(onMasterHeader);


            //page  setting

            view.m_UI_setting.m_btn_btn_GameButton.onClick.AddListener(onSetting);
            view.m_UI_list.m_btn_btn_GameButton.onClick.AddListener(onAllianceList);
            view.m_UI_board.m_btn_btn_GameButton.onClick.AddListener(onRank); //TODO

            view.m_UI_check.m_btn_btn_GameButton.onClick.AddListener(onCheckJionList);

            view.m_UI_invide.m_btn_btn_GameButton.onClick.AddListener(onInvidePlayer);


            m_allianceProxy.SendAllianceInfo(1);

            if (isReinforceMode)
            {
                view.m_UI_Side.gameObject.SetActive(false);
                OnTagMember(true);
            }

            CheckBtnRoot();
            RePlayer();
            ReAllianceInfo();
            CheckRedGuildApplys();
            CheckRedGuildSetting();
            CheckReareachRedPoint();
            CheckHelpRedPoint();
            CheckGiftRedPoint();
            CheckRssRedPoint();
            CheckRallyRedPoint();
            CheckHasBuilding();
            
            
            m_rallyTroopProxy.ClearGuildCityHud();
        }

        private void CheckRallyRedPoint()
        {
            view.m_UI_war.m_UI_Common_Redpoint.ShowRedPoint(m_rallyTroopProxy.GetRallyRedPoint());
        }

        private void CheckHelpRedPoint()
        {
            view.m_UI_help.m_UI_Common_Redpoint.ShowRedPoint(m_allianceProxy.RedHelp());
        }
        
        private void CheckRssRedPoint()
        {
            view.m_UI_terrtroy.m_UI_Common_Redpoint.ShowRedPoint(m_allianceProxy.GetRedPointRss());
        }

        private void CheckHasBuilding()
        {
            if (m_allianceProxy.GetAlliance() != null)
            {
                view.m_UI_terrtroy.m_img_build_PolygonImage.gameObject.SetActive(m_allianceProxy.GetAlliance().territoryBuildFlag);
            }
        }

        private void CheckGiftRedPoint()
        {
            
            view.m_UI_gift.m_UI_Common_Redpoint.ShowRedPoint(m_allianceProxy.GetGiftRed());
        }

        private void CheckReareachRedPoint()
        {
            long count = m_allianceProxy.GetStudyDoRedCount();
            view.m_UI_technology.SetRedCount(count);
        }

        private void onMsgBoard()
        {
            var info = m_allianceProxy.GetAlliance();
            CoreUtils.uiManager.ShowUI(UI.s_AllianceMsg, null, info);
        }

        private void onGift()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceGift);
        }

        private void onShop()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceStore);
        }

        private void onTrans()
        {
            ClientUtils.TranslatorSDK(view.m_lbl_desc_LanguageText);
        }

        private void onMasterHeader()
        {
            if (m_playerProxy.CurrentRoleInfo.rid != m_allianceProxy.GetAlliance().leaderRid)
            {
                var data = m_allianceProxy.getMemberInfo(m_allianceProxy.GetAlliance().leaderRid);
                ShowPopMemu(data, view.m_btn_PlayerHead_GameButton.transform as RectTransform);
            }
        }

        private void CheckBtnRoot()
        {
            view.m_UI_quit.m_btn_btn_GameButton.onClick.RemoveAllListeners();
            if (m_allianceProxy.GetSelfRoot(GuildRoot.exit))
            {
                view.m_UI_quit.m_lbl_name_LanguageText.text = LanguageUtils.getText(730102);
                view.m_UI_quit.m_btn_btn_GameButton.onClick.AddListener(onQuit);
            }
            else
            {
                view.m_UI_quit.m_lbl_name_LanguageText.text = LanguageUtils.getText(730190);
                view.m_UI_quit.m_btn_btn_GameButton.onClick.AddListener(onEndGuild);
            }
        }

        private void OnTech()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceResearchMain);
        }

        public void onEndGuild()
        {
            if (m_allianceProxy.GetSelfRoot(GuildRoot.endGuild))
            {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceExitConfirm);
            }
            else
            {
                Tip.CreateTip(730136).Show();
            }
        }

        public void CheckRedGuildSetting()
        {


            if (m_playerProxy==null || m_allianceProxy==null||view.gameObject==null || m_allianceProxy.GetAlliance()==null)
            {
                return;
            }
            
            bool guildMailSettingClicked = PlayerPrefs.GetInt(string.Format($"{m_playerProxy.Rid}{m_allianceProxy.GetAlliance().guildId}_GuildWelcome")) == 1;
            bool showRedDot = !guildMailSettingClicked && m_allianceProxy.GetSelfRoot(GuildRoot.editInfo);
            view.m_UI_Side.m_pl_side3.m_img_redpot_PolygonImage.gameObject.SetActive(showRedDot);
            view.m_UI_setting.SetRedDot(showRedDot);
        }

        public void CheckRedGuildApplys()
        {
            bool hasRoot = m_allianceProxy.GetSelfRoot(GuildRoot.checkInvice);
            view.m_UI_check.SetRedDot(
                hasRoot && m_allianceProxy.getGuildApplys().Count > 0 );
        }

        private void onMail()
        {
            if (!m_allianceProxy.GetSelfRoot(GuildRoot.mail))
            {
                Tip.CreateTip(730325).Show();
                return;
            }

            if (SystemOpen.IsSystemOpen(EnumSystemOpen.guild_mail,true))
            {
                var data = new WriteAMailData()
                {
                    isGuildMail = true,
                    stableName =ClientUtils.ClearRichText(LanguageUtils.getText(570069)),
                };
                CoreUtils.uiManager.ShowUI(UI.s_writeAMail,null,data);
            }

        }

        private void onHoly()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceHoly);
        }

        private void onWar()
        {
            Debug.LogFormat("加载联盟战争界面:{0}", Time.realtimeSinceStartup);
            
            
            // m_rallyTroopProxy.SetReadedRallyRedPoint();
            
            CoreUtils.uiManager.ShowUI(UI.s_AlianceWar);
        }

        private void onHelp()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceHelp);
        }

        private void onDepot()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceDepot);
        }

        private void onTerrtroy()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceTerrtroy);
        }

        private void OnTagMian(bool v)
        {
            view.m_pl_rect0.gameObject.SetActive(true);
            view.m_pl_rect1.gameObject.SetActive(false);
            view.m_pl_rect2.gameObject.SetActive(false);

            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730021));

            if (v)
            {
                m_allianceProxy.SendAllianceInfo(1);
            }
        }

        private void OnTagMember(bool v)
        {
            view.m_pl_rect0.gameObject.SetActive(false);
            view.m_pl_rect1.gameObject.SetActive(true);
            view.m_pl_rect2.gameObject.SetActive(false);

            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730114));

            m_allianceProxy.SendReMember();


            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                funcTab.GetItemSize = GetItemSize;
                funcTab.GetItemPrefabName = GetItemPrefabName;

                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);

                ReMemberList();
            });

            RePlayer();
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            var data = m_guildMemberList[item.index];

            return view.m_sv_list_ListView.ItemPrefabDataList[data.prefab_index];
        }

        private float GetItemSize(ListView.ListItem item)
        {
            var data = m_guildMemberList[item.index];

            if (data.prefab_index == 0)
            {
                return 73f;
            }
            else if (data.prefab_index == 1)
            {
                return 150f;
            }

            return 90f;
        }

        private void OnTagSetting(bool v)
        {
            view.m_pl_rect0.gameObject.SetActive(false);
            view.m_pl_rect1.gameObject.SetActive(false);
            view.m_pl_rect2.gameObject.SetActive(true);

            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730096));
        }

        private void onQuit()
        {
            if (m_allianceProxy.GetSelfRoot(GuildRoot.exit))
            {
                var alert = Alert.CreateAlert(730216, LanguageUtils.getText(730102))
                    .SetRightButton(null, LanguageUtils.getText(730155)).SetLeftButton(
                        () => { m_allianceProxy.SendExitGuild(1); }, LanguageUtils.getText(730154), true)
                    .Show();
            }
            else
            {
                Tip.CreateTip(730136).Show();
            }
        }

        private void onGuildInfo()
        {
            var info = m_allianceProxy.GetAlliance();
            HelpTip.CreateTip(4004, view.m_btn_info_GameButton.transform, true, info.memberLimit, m_allianceProxy.Config.allianceInitialNum,0,0,0)
                .SetStyle(HelpTipData.Style.arrowDown).Show();
        }

        private void onInvidePlayer()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceInvite);
        }

        private void onCheckJionList()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceJionReqList);
        }

        private void onRank()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceRanking);
        }

        private void onAllianceList()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceJionList);
        }

        private void onSetting()
        {
            if (m_allianceProxy.GetSelfRoot(GuildRoot.editInfo))
            {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceCreateWin, null, false);
            }
            else
            {
                Tip.CreateTip(730156).Show();
            }
        }

        private void onOpenLevelAcc()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceMainAccess);
        }


        public void ReAllianceInfo()
        {
            var info = m_allianceProxy.GetAlliance();

            if (info!=null)
            {
                view.m_lbl_guildName_LanguageText.text =
                    LanguageUtils.getTextFormat(300030, info.abbreviationName, info.name);

                view.m_lbl_power_LanguageText.text =
                    LanguageUtils.getTextFormat(730076, ClientUtils.FormatComma(info.power));

                view.m_lbl_master_LanguageText.text = LanguageUtils.getTextFormat(730077, info.leaderName);

                view.m_lbl_terrtroy_LanguageText.text = LanguageUtils.getTextFormat(730078, info.territory);

                var strLevel = LanguageUtils.getTextFormat(180306, info.giftLevel);
                view.m_lbl_gift_LanguageText.text = LanguageUtils.getTextFormat(730079, strLevel);

                view.m_lbl_member_LanguageText.text = LanguageUtils.getTextFormat(730080, info.memberNum, info.memberLimit);

                view.m_lbl_desc_LanguageText.text = info.notice;

                view.m_UI_flag.setData(info);
                
                view.m_UI_msgRedPoint.m_img_redpoint_PolygonImage.gameObject.SetActive(info.messageBoardRedDot);
            }
        }

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            var data = m_guildMemberList[scrollItem.index];
                        
            if (data.prefab_index == 0)
            {
                UI_Item_GuildMemRankView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildMemRankView>(scrollItem.go);


                if (itemView != null)
                {
                    itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(data.data.l_nameID);

                    if (data.data.lv == 4)
                    {
                        itemView.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(730117,
                            data.LevelMembers.Count, data.data.researchersLimit);
                    }
                    else
                    {
                        itemView.m_lbl_count_LanguageText.text = data.LevelMembers.Count.ToString();
                    }

                    ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, data.data.icon);

                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);


                    var index = scrollItem.index;

                    itemView.m_btn_bg_GameButton.onClick.RemoveAllListeners();

                    itemView.m_btn_bg_GameButton.onClick.AddListener(() =>
                        {
                            data.isSelected = !data.isSelected;

                            itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                            itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);

                            if (data.isSelected)
                            {
                                if (data.LevelMembers!=null && data.LevelMembers.Count>0)
                                {
                                    AddMember(data.data.lv, index, data);
                                    view.m_sv_list_ListView.FillContent(m_guildMemberList.Count);
                                }
                               
                            }
                            else
                            {
                                if (data.LevelMembers != null && data.LevelMembers.Count > 0)
                                {
                                    RemoveMember(data.data.lv);
                                    view.m_sv_list_ListView.FillContent(m_guildMemberList.Count);
                                }
                            }
                        }
                    );
                }
            }
            else if (data.prefab_index == 1)
            {
                UI_LC_GuildMemR4View itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_LC_GuildMemR4View>(scrollItem.go);

                if (itemView != null)
                {
                    var subItems =
                        MonoHelper.GetHotFixViewComponentsInChildren<UI_Item_GuildMemR4View>(itemView.gameObject);
                    var len = subItems.Length;
                    for (int i = 0; i < len; i++)
                    {
                        var subItem = subItems[i];
                        var subData = data.subItemsData.Count - 1 >= i ? data.subItemsData[i] : null;

                        subItem.gameObject.SetActive(subData != null);


                        if (subData != null)
                        {
                            subItem.m_UI_PlayerHead.LoadHeadCountry((int) subData.headId);
                            subItem.m_lbl_name_LanguageText.text = subData.name;

                            if (subData.rid == m_playerProxy.CurrentRoleInfo.rid)
                            {
                                subItem.m_lbl_pow_LanguageText.text = ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.combatPower);
                                ClientUtils.TextSetColor(subItem.m_lbl_pow_LanguageText,"#66cc33");
                                ClientUtils.TextSetColor(subItem.m_lbl_name_LanguageText,"#66cc33");
                            }
                            else
                            {
                                subItem.m_lbl_pow_LanguageText.text = ClientUtils.FormatComma(subData.combatPower);
                                ClientUtils.TextSetColor(subItem.m_lbl_pow_LanguageText,"#ffffff");
                                ClientUtils.TextSetColor(subItem.m_lbl_name_LanguageText,"#e2d19b");
                            }


                            if (m_playerProxy.CurrentRoleInfo.rid == m_allianceProxy.GetAlliance().leaderRid)
                            {
                                subItem.m_btn_online_GameButton.gameObject.SetActive(true); //盟主
                            }
                            else
                            {
                                //r4加官职
                                subItem.m_btn_online_GameButton.gameObject.SetActive(
                                    m_allianceProxy.GetSelfRoot(GuildRoot.online) &&
                                    m_allianceProxy.getMemberOfficer(m_playerProxy.CurrentRoleInfo.rid) != null);
                            }


                            subItem.m_btn_online_GameButton.interactable = subData.online;

                            var officer = m_allianceProxy.getMemberOfficer(subData.rid);
                            if (officer != null && officer.officerId > 0)
                            {
                                var cdata =
                                    CoreUtils.dataService
                                        .QueryRecord<AllianceOfficiallyDefine>((int) officer.officerId);

                                if (cdata != null)
                                {
                                    ClientUtils.LoadSprite(subItem.m_img_office_PolygonImage, cdata.icon);
                                }

                                subItem.m_btn_office_GameButton.gameObject.SetActive(true);

                                subItem.m_btn_job_GameButton.gameObject.SetActive(false);
                            }
                            else
                            {
                                subItem.m_btn_office_GameButton.gameObject.SetActive(false);
                                subItem.m_btn_job_GameButton.gameObject.SetActive(true);
                            }

                            subItem.m_btn_job_GameButton.onClick.RemoveAllListeners();

                            subItem.m_btn_rect_GameButton.onClick.RemoveAllListeners();
                            subItem.m_btn_office_GameButton.onClick.RemoveAllListeners();

                            subItem.m_btn_job_GameButton.onClick.AddListener(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceOffice, null, subData);
                            });

                            subItem.m_btn_office_GameButton.onClick.AddListener(() =>
                            {
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceOffice, null, subData);
                            });
                            subItem.m_btn_rect_GameButton.onClick.AddListener(() =>
                            {
                                ShowPopMemu(subData, subItem.m_btn_rect_GameButton.transform as RectTransform);
                            });
                        }
                    }
                }
            }
            else
            {
                UI_LC_GuildMemR0View itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_LC_GuildMemR0View>(scrollItem.go);

                if (itemView != null)
                {
                    var subItems =
                        MonoHelper.GetHotFixViewComponentsInChildren<UI_Item_GuildMemR0View>(itemView.gameObject);
                    var len = subItems.Length;
                    for (int i = 0; i < len; i++)
                    {
                        var subItem = subItems[i];
                        var subData = data.subItemsData.Count - 1 >= i ? data.subItemsData[i] : null;

                        subItem.gameObject.SetActive(subData != null);

                        if (subData != null)
                        {
                            subItem.m_UI_PlayerHead.LoadHeadCountry((int) subData.headId);
                            subItem.m_lbl_name_LanguageText.text = subData.name;
                            
                            if (subData.rid == m_playerProxy.CurrentRoleInfo.rid)
                            {
                                subItem.m_lbl_pow_LanguageText.text = ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.combatPower);
                                ClientUtils.TextSetColor(subItem.m_lbl_pow_LanguageText,"#66cc33");
                                ClientUtils.TextSetColor(subItem.m_lbl_name_LanguageText,"#66cc33");
                            }
                            else
                            {
                                subItem.m_lbl_pow_LanguageText.text = ClientUtils.FormatComma(subData.combatPower);
                                ClientUtils.TextSetColor(subItem.m_lbl_pow_LanguageText,"#ffffff");
                                ClientUtils.TextSetColor(subItem.m_lbl_name_LanguageText,"#e2d19b");
                            }
                            
                            if (m_playerProxy.CurrentRoleInfo.rid == m_allianceProxy.GetAlliance().leaderRid)
                            {
                                subItem.m_btn_online_GameButton.gameObject.SetActive(true); //盟主
                            }
                            else
                            {
                                //r4加官职
                                subItem.m_btn_online_GameButton.gameObject.SetActive(
                                    m_allianceProxy.GetSelfRoot(GuildRoot.online) &&
                                    m_allianceProxy.getMemberOfficer(m_playerProxy.CurrentRoleInfo.rid) != null);
                            }


                            subItem.m_btn_online_GameButton.interactable = subData.online;


                            subItem.m_btn_bg_GameButton.onClick.RemoveAllListeners();

                            subItem.m_btn_bg_GameButton.onClick.AddListener(() =>
                            {
                                if (subData.rid != m_playerProxy.CurrentRoleInfo.rid)
                                {
                                    ShowPopMemu(subData, subItem.m_btn_bg_GameButton.transform as RectTransform);
                                }
                            });
                        }
                    }
                }
            }
        }

        private HelpTip m_tipView;

        private void ShowPopMemu(GuildMemberInfoEntity subData, RectTransform transform)
        {
            
            var myMemberInfo = m_allianceProxy.getMemberInfo(m_playerProxy.CurrentRoleInfo.rid);
            if (subData==null || subData.rid == m_playerProxy.CurrentRoleInfo.rid || view.gameObject ==null ||m_allianceProxy ==null ||m_allianceProxy.GetAlliance()==null || myMemberInfo==null)
            {
                return;
            }

            CoreUtils.assetService.Instantiate("UI_Pop_GuildMemTip", (obj) =>
            {
                powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_GuildMemTipView>(obj);


                
                powTipView.m_UI_info.gameObject.SetActive(!isReinforceMode);
                powTipView.m_UI_mail.gameObject.SetActive(!isReinforceMode);
//                powTipView.m_UI_res_help.gameObject.SetActive( !isReinforceMode);
                powTipView.m_UI_help.gameObject.SetActive(!isReinforceMode);

                powTipView.m_UI_plus.gameObject.SetActive(m_allianceProxy.GetSelfRoot(GuildRoot.LvUpDown) &&
                                                          !isReinforceMode && subData.rid!= m_allianceProxy.GetAlliance().leaderRid);
                powTipView.m_UI_remove.gameObject.SetActive(m_allianceProxy.GetSelfRoot(GuildRoot.removeMember) &&
                                                            !isReinforceMode&& subData.rid!= m_allianceProxy.GetAlliance().leaderRid);


               

                if (myMemberInfo.guildJob == subData.guildJob && m_allianceProxy.GetSelfRoot(GuildRoot.LvUpDown))
                {
                    powTipView.m_UI_plus.gameObject.SetActive(false);
                    powTipView.m_UI_remove.gameObject.SetActive(false);
                }
                
                ClientUtils.UIReLayout(powTipView.m_pl_bg_ContentSizeFitter.gameObject);



                if (LanguageUtils.IsArabic())
                {
                    m_tipView = HelpTip.CreateTip(powTipView.gameObject, powTipView.m_pl_bg_ContentSizeFitter.gameObject.GetComponent<RectTransform>().sizeDelta, transform)
                        .SetStyle(HelpTipData.Style.arrowRight).Show();
                }
                else
                {
                    m_tipView = HelpTip.CreateTip(powTipView.gameObject, powTipView.m_pl_bg_ContentSizeFitter.gameObject.GetComponent<RectTransform>().sizeDelta, transform)
                        .SetStyle(HelpTipData.Style.arrowLeft).Show();
                }

                
                
                
                powTipView.m_UI_info.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_OtherPlayerInfo,null,subData.rid);
                    CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);

                });
                
                
                powTipView.m_UI_mail.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                   
                    WriteAMailData mailData = new WriteAMailData();

                    mailData.stableName = subData.name;
                    mailData.stableRid = subData.rid;
                   
                    CoreUtils.uiManager.ShowUI(UI.s_writeAMail,null,mailData);
                    CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);

                });


                powTipView.m_UI_remove.m_btn_languageButton_GameButton.onClick.AddListener(
                    () =>
                    {
                        Alert.CreateAlert(730164, LanguageUtils.getText(730163))
                            .SetLeftButton(() => { CoreUtils.uiManager.ShowUI(UI.s_AllianceMemRemove, null, subData); },
                                LanguageUtils.getText(730154)).SetRightButton(null, LanguageUtils.getText(730155))
                            .Show();
                        CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                    });

                powTipView.m_UI_plus.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_AllianceMemUpgrade, null, subData);
                    CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                });
                
                
                powTipView.m_UI_help.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                        MinimapProxy minimapProxy = AppFacade.GetInstance().RetrieveProxy(MinimapProxy.ProxyNAME) as MinimapProxy;
                        if (!minimapProxy.MemberPos.ContainsKey(subData.rid))
                        {
                            Tip.CreateTip(184032, Tip.TipStyle.Middle).Show();
                        }
                        else
                        {
                            if (subData.cityObjectIndex != 0)
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.GetCityReinforceInfo, subData, null);
                            }
                        }
                    CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                });
                
                powTipView.m_UI_res_help.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
                    if (!m_troopProxy.GetIsCityCreateTroop())
                    {
                        Tip.CreateTip(184030, Tip.TipStyle.Middle).Show();
                        CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);

                        return;
                    }

                    BuildingInfoEntity buildingInfoEntity =
                        m_cityBuildingProxy.GetBuildingInfoByType((long) EnumCityBuildingType.TradingPost);

                    if (buildingInfoEntity == null)
                    {
                        Tip.CreateTip(184031, Tip.TipStyle.Middle).Show();
                        CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                        return;
                    }
                    MinimapProxy minimapProxy = AppFacade.GetInstance().RetrieveProxy(MinimapProxy.ProxyNAME) as MinimapProxy;
                    if (!minimapProxy.MemberPos.ContainsKey(subData.rid))
                    {
                        Tip.CreateTip(184032, Tip.TipStyle.Middle).Show();
                        CoreUtils.assetService.Destroy(powTipView.gameObject.transform.parent.gameObject);
                        return;
                    }
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
                    Transport_GetTransport.request req = new Transport_GetTransport.request();
                    req.targetRid = subData.rid;
                    AppFacade.GetInstance().SendSproto(req);
                });
                
                


            });
        }


        public void AddMember(int grade, int index, AllianceMemberLevel lvmember)
        {
            if (grade == 4)
            {
                int len = lvmember.LevelMembers.Count;

                for (int i = 0; i < len; i = i + 4)
                {
                    AllianceMemberLevel nMemberLevel = new AllianceMemberLevel();

                    nMemberLevel.prefab_index = 1;
                    nMemberLevel.data = lvmember.data;

                    nMemberLevel.subItemsData = new List<GuildMemberInfoEntity>();


                    for (int j = i; j < i + 4; j++)
                    {
                        if (j < len)
                        {
                            nMemberLevel.subItemsData.Add(lvmember.LevelMembers[j]);
                        }
                    }

                    m_guildMemberList.Insert(index + 1, nMemberLevel);
                    index++;
                }
            }
            else
            {
                int len = lvmember.LevelMembers.Count;

                for (int i = 0; i < len; i = i + 2)
                {
                    AllianceMemberLevel nMemberLevel = new AllianceMemberLevel();

                    nMemberLevel.prefab_index = 2;
                    nMemberLevel.data = lvmember.data;

                    nMemberLevel.subItemsData = new List<GuildMemberInfoEntity>();


                    for (int j = i; j < i + 2; j++)
                    {
                        if (j < len)
                        {
                            nMemberLevel.subItemsData.Add(lvmember.LevelMembers[j]);
                        }
                    }

                    m_guildMemberList.Insert(index + 1, nMemberLevel);
                    index++;
                }
            }
        }

        public void RemoveMember(int lv)
        {
            int startIndex = 0;

            int count = 0;

            int len = m_guildMemberList.Count;

            for (int i = 0; i < len; i++)
            {
                var item = m_guildMemberList[i];

                if (item.data.lv == lv)
                {
                    if (startIndex == 0)
                    {
                        startIndex = i + 1;
                    }
                    else
                    {
                        count = count + 1;
                    }
                }
            }

            m_guildMemberList.RemoveRange(startIndex, count);
        }

        public void ReMemberList()
        {
            if (m_assetDic.Count > 0)
            {
                var initdatas = m_allianceProxy.getAllianceMembers();


                m_guildMemberList.Clear();

                m_guildMemberList.AddRange(initdatas);


                for (int i = 1; i <= 4; i++)
                {
                    for (int j = 0; j < m_guildMemberList.Count; j++)
                    {
                        var level = m_guildMemberList[j];

                        if (level.data.lv == i && level.isSelected)
                        {
                            AddMember(i, j, level);
                        }
                    }
                }

                view.m_sv_list_ListView.FillContent(m_guildMemberList.Count);
            }
        }

        public void RePlayer()
        {
            var info = m_allianceProxy.GetAlliance();

            if (info != null)
            {
                var lvInfo = m_allianceProxy.getMemberInfo(info.leaderRid);

                if (lvInfo != null)
                {
                    view.m_lbl_name_LanguageText.text = lvInfo.name;

                    if (m_playerProxy.CurrentRoleInfo.rid == m_allianceProxy.GetAlliance().leaderRid)
                    {
                        view.m_lbl_power1_LanguageText.text =
                            ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.combatPower);
                        view.m_lbl_kill_LanguageText.text =
                            ClientUtils.FormatComma(m_playerProxy.killCount());
                    }
                    else
                    {
                        view.m_lbl_power1_LanguageText.text = ClientUtils.FormatComma(lvInfo.combatPower);
                        view.m_lbl_kill_LanguageText.text =
                            ClientUtils.FormatComma(m_playerProxy.KillCountOther(lvInfo.killCount));
                    }

                    view.m_UI_PlayerHead.LoadHeadCountry((int) lvInfo.headId);

                    if (lvInfo != null && lvInfo.guildJob > 0)
                    {
                        var cdata = CoreUtils.dataService.QueryRecord<AllianceMemberDefine>((int) lvInfo.guildJob);
                        if (cdata != null)
                        {
                            ClientUtils.LoadSprite(view.m_img_icon_PolygonImage, cdata.icon);
                        }
                    }
                }
            }
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceMain);
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.AllianceMoveCity);
        }

        protected override void BindUIData()
        {
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月24日
// Update Time         :    2020年4月24日
// Class Description   :    UI_Win_OtherPlayerDataMediator
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
using Data;
using System.Linq;
using UnityEngine.UI;
using System;

namespace Game {
    public class UI_Win_OtherPlayerDataMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_OtherPlayerDataMediator";

        private PlayerProxy m_playerProxy;
        private ChatProxy m_chatProxy;

        private MapObjectInfoEntity m_mapObj;

        private RoleInfoEntity m_roleInfo;
        #endregion

        //IMediatorPlug needs
        public UI_Win_OtherPlayerDataMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_OtherPlayerDataView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_GetRoleInfo.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_GetRoleInfo.TagName:
                    {
                        Role_GetRoleInfo.response roleInfo = notification.Body as Role_GetRoleInfo.response;
                        if (roleInfo != null)
                        {
                            m_roleInfo = new RoleInfoEntity();
                            RoleInfoEntity.updateEntity(m_roleInfo, roleInfo.roleInfo);
                            InitRoleInfoView();
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
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_chatProxy = AppFacade.GetInstance().RetrieveProxy(ChatProxy.ProxyNAME) as ChatProxy;

            if (view.data is MapObjectInfoEntity)
            {
                m_mapObj = view.data as MapObjectInfoEntity;
                Role_GetRoleInfo.request req = new Role_GetRoleInfo.request();
                req.queryRid = m_mapObj.cityRid;
                AppFacade.GetInstance().SendSproto(req);
            }
            else if (view.data is RoleInfoEntity)
            {
                m_roleInfo = view.data as RoleInfoEntity;
            }
            else if (view.data is RoleInfo)
            {
                RoleInfo tmp = view.data as RoleInfo;
                m_roleInfo = new RoleInfoEntity();
                RoleInfoEntity.updateEntity(m_roleInfo, tmp);
            }
            else if (view.data is long)
            {
                long rid = (long)view.data;
                Role_GetRoleInfo.request req = new Role_GetRoleInfo.request();
                req.queryRid = rid;
                AppFacade.GetInstance().SendSproto(req);
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Item_PlayerDatahero.gameObject.SetActive(false);

            view.m_UI_Model_Window_TypeMid.setCloseHandle(OnClose);
            view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(300058));

            view.m_btn_changeCountry_GameButton.onClick.AddListener(CopyName);

            view.m_UI_Item_PlayerDataMore.m_btn_btn_GameButton.onClick.AddListener(OnMoreDetail);
            view.m_UI_Item_PlayerDataAlliance.m_btn_btn_GameButton.onClick.AddListener(OnAlliance);
            view.m_UI_Item_PlayerDataChat.m_btn_btn_GameButton.onClick.AddListener(OnChat);
            view.m_UI_Item_PlayerDataMail.m_btn_btn_GameButton.onClick.AddListener(OnMail);

            view.m_btn_picture_GameButton.onClick.AddListener(OnComingSoon);
        }

        private void OnComingSoon()
        {
            Tip.CreateTip(100045, Tip.TipStyle.Middle).Show();
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_OtherPlayerInfo);
        }

        protected override void BindUIData()
        {
            if (m_mapObj != null)
                InitMapObjView();
            else if (m_roleInfo != null)
                InitRoleInfoView();
        }
       
        #endregion

        private void InitMapObjView()
        {
            if(m_mapObj==null)
            {
                return;
            }
            view.m_lbl_roleid_LanguageText.text = LanguageUtils.getTextFormat(300086, m_playerProxy.GetEncryptId(m_mapObj.cityRid));

            var country = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_mapObj.cityCountry);
            if (country != null)
            {
                //文明icon
                ClientUtils.LoadSprite(view.m_img_civiIcon_PolygonImage, country.civilizationMark);
                view.m_lbl_civil_LanguageText.text = LanguageUtils.getText(country.l_civilizationID);
            }

            view.m_lbl_power_LanguageText.text = ClientUtils.FormatComma(m_mapObj.objectPower);
            view.m_lbl_kill_LanguageText.text = ClientUtils.FormatComma(GetKillCount(m_mapObj.killCount));
            view.m_lbl_playerName_LanguageText.text = m_mapObj.cityName;


            //联盟
            if (string.IsNullOrEmpty(m_mapObj.guildFullName))
            {
                view.m_lbl_guild_LanguageText.text = LanguageUtils.getText(570029);
            }
            else
            {
                view.m_lbl_guild_LanguageText.text = LanguageUtils.getTextFormat(300030, m_mapObj.guildAbbName, m_mapObj.guildFullName);
            }

            view.m_UI_Model_PlayerHead.LoadPlayerIcon(m_mapObj.headId,m_mapObj.headFrameID);

            view.m_btn_killHelp_GameButton.onClick.RemoveAllListeners();
            view.m_btn_killHelp_GameButton.onClick.AddListener(ShowKill);
        }


        private void ShowKill()
        {
            CoreUtils.assetService.Instantiate("UI_KillTip", (obj) =>
            {
                UI_KillTipView powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_KillTipView>(obj);

                powTipView.m_lbl_tipkill_LanguageText.text = LanguageUtils.getTextFormat(610002, ClientUtils.FormatComma(GetKillCount(m_mapObj.killCount)));
                //DateTime dt = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(1970, 1, 1)).AddSeconds(m_playerProxy.CurrentRoleInfo.createTime);
                powTipView.m_lbl_tipkilltime_LanguageText.text =string.Empty;

                powTipView.m_lbl_level1_LanguageText.text = m_mapObj.killCount.ContainsKey(1) ? m_mapObj.killCount[1].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level2_LanguageText.text = m_mapObj.killCount.ContainsKey(2) ? m_mapObj.killCount[2].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level3_LanguageText.text = m_mapObj.killCount.ContainsKey(3) ? m_mapObj.killCount[3].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level4_LanguageText.text = m_mapObj.killCount.ContainsKey(4) ? m_mapObj.killCount[4].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level5_LanguageText.text = m_mapObj.killCount.ContainsKey(5) ? m_mapObj.killCount[5].count.ToString("N0") : 0.ToString();

                HelpTip.CreateTip(powTipView.gameObject, (powTipView.gameObject.transform as RectTransform).sizeDelta, view.m_btn_killHelp_GameButton.gameObject.transform as RectTransform).SetStyle(HelpTipData.Style.arrowUp).Show();


            });
        }


        private void OnMoreDetail()
        {
            CoreUtils.uiManager.ShowUI(UI.s_PlayerPower,null,m_roleInfo);
        }

        private void OnAlliance()
        {
            long guildID = 0;
            long cityRid = 0;
            if (m_mapObj!=null)
            {
                guildID = m_mapObj.guildId;
                cityRid = m_mapObj.cityRid;
            }

            if (m_roleInfo!=null)
            {
                guildID = m_roleInfo.guildId;
                cityRid = m_roleInfo.rid;
            }
            //联盟
            if (guildID == 0)
            {
                Alert.CreateAlert(LanguageUtils.getText(730347))
                          .SetRightButton(null, LanguageUtils.getText(192010))
                          .SetLeftButton(() =>
                          {
                              Guild_InviteGuild.request request = new Guild_InviteGuild.request();
                              request.invitedRid = cityRid;
                              AppFacade.GetInstance().SendSproto(request);
                          }, LanguageUtils.getText(192009))
                          .Show();
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceInfo, null, guildID);
            }
        }

        private void OnChat()
        {
            if (m_roleInfo == null)
            {
                return;
            }
            int lvLimit = 0;
            ChatChannelDefine chatChannelDefine = CoreUtils.dataService.QueryRecord<ChatChannelDefine>(100);
            if (chatChannelDefine != null)
            {
                lvLimit = chatChannelDefine.lvLimit;
            }
            if (m_playerProxy.CurrentRoleInfo.level >= lvLimit)
            {
                m_chatProxy.BeginChat(m_roleInfo);
                CoreUtils.uiManager.CloseLayerUI(UILayer.WindowLayer);
                CoreUtils.uiManager.CloseLayerUI(UILayer.WindowPopLayer);
            }
            else
            {
                Tip.CreateTip(LanguageUtils.getTextFormat( 750002,lvLimit), Tip.TipStyle.Middle).Show();
            }
        }
        private void OnMail()
        {
            //Tip.CreateTip("TODO//私聊邮件未完成", Tip.TipStyle.Middle).Show();
            WriteAMailData mailInfo = new WriteAMailData();
            if (m_mapObj != null)
            {
                mailInfo.stableRid = m_mapObj.cityRid;
                mailInfo.stableName = m_mapObj.cityName;
            }
            else if (m_roleInfo != null)
            {
                mailInfo.stableRid = m_roleInfo.rid;
                mailInfo.stableName = m_roleInfo.name;
            }
            CoreUtils.uiManager.ShowUI(UI.s_writeAMail, null, mailInfo);
        }
        

        #region 传入的是RoleInfo数据

        private void InitRoleInfoView()
        {
            if(m_roleInfo==null)
            {
                return;
            }

            view.m_lbl_roleid_LanguageText.text = LanguageUtils.getTextFormat(300086, m_playerProxy.GetEncryptId(m_roleInfo.rid));

            var country = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_roleInfo.country);
            if (country != null)
            {
                //文明icon
                ClientUtils.LoadSprite(view.m_img_civiIcon_PolygonImage, country.civilizationMark);
                view.m_lbl_civil_LanguageText.text = LanguageUtils.getText(country.l_civilizationID);
            }

            view.m_lbl_power_LanguageText.text = ClientUtils.FormatComma(m_roleInfo.combatPower);
            view.m_lbl_kill_LanguageText.text = ClientUtils.FormatComma(GetKillCount(m_roleInfo.killCount));
            view.m_lbl_playerName_LanguageText.text = m_roleInfo.name;

            //联盟
            if (string.IsNullOrEmpty(m_roleInfo.guildName))
            {
                view.m_lbl_guild_LanguageText.text = LanguageUtils.getText(570029);
            }
            else
            {
                view.m_lbl_guild_LanguageText.text = LanguageUtils.getTextFormat(300030, m_roleInfo.guildAbbName, m_roleInfo.guildName);
            }

            view.m_UI_Model_PlayerHead.LoadPlayerIcon(m_roleInfo.headId, m_roleInfo.headFrameID);

            view.m_btn_killHelp_GameButton.onClick.RemoveAllListeners();
            view.m_btn_killHelp_GameButton.onClick.AddListener(ShowRoleInfoKill);
        }

        private void ShowRoleInfoKill()
        {
            CoreUtils.assetService.Instantiate("UI_KillTip", (obj) =>
            {
                UI_KillTipView powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_KillTipView>(obj);

                powTipView.m_lbl_tipkill_LanguageText.text = LanguageUtils.getTextFormat(610002, ClientUtils.FormatComma(GetKillCount(m_roleInfo.killCount)));
                //DateTime dt = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(1970, 1, 1)).AddSeconds(m_roleInfo.createTime);
                powTipView.m_lbl_tipkilltime_LanguageText.text = string.Empty;

                powTipView.m_lbl_level1_LanguageText.text = m_roleInfo.killCount.ContainsKey(1) ? m_roleInfo.killCount[1].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level2_LanguageText.text = m_roleInfo.killCount.ContainsKey(2) ? m_roleInfo.killCount[2].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level3_LanguageText.text = m_roleInfo.killCount.ContainsKey(3) ? m_roleInfo.killCount[3].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level4_LanguageText.text = m_roleInfo.killCount.ContainsKey(4) ? m_roleInfo.killCount[4].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level5_LanguageText.text = m_roleInfo.killCount.ContainsKey(5) ? m_roleInfo.killCount[5].count.ToString("N0") : 0.ToString();

                HelpTip.CreateTip(powTipView.gameObject, (powTipView.gameObject.transform as RectTransform).sizeDelta, view.m_btn_killHelp_GameButton.gameObject.transform as RectTransform).SetStyle(HelpTipData.Style.arrowUp).Show();


            });
        }

        #endregion

        private long GetKillCount(Dictionary<long, KillCount> kills)
        {
            if (kills == null)
            {
                return 0;
            }
            long killCount = 0;
            kills.Values.ToList().ForEach(i =>
            {
                killCount += i.count;
            });
            return killCount;
        }

        private void CopyName()
        {
            string name = "";
            if (m_roleInfo == null && m_mapObj != null)
            {
                name = m_mapObj.cityName;
            }
            else if (m_roleInfo != null)
            {
                name = m_roleInfo.name;
            }
            GUIUtility.systemCopyBuffer = name;
                Tip.CreateTip(100706).Show();
        }
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, January 9, 2020
// Update Time         :    Thursday, January 9, 2020
// Class Description   :    PlayerDataMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class PlayerDataMediator : GameMediator {
        #region Member
        public static string NameMediator = "PlayerDataMediator";

        private PlayerProxy m_playerProxy;

        private CivilizationDefine m_country;

        private PlayerAttributeProxy m_playerAttributeProxy;

        private AllianceProxy m_allianceProxy;
        private long crrPower;
        private long maxPower;

        private UI_Pop_PowerTextTipView powTipView;
        private Timer m_updateTick;
        private BagProxy m_bagProxy = null;
        #endregion

        //IMediatorPlug needs
        public PlayerDataMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public PlayerDataView view;



        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.UpdatePlayerActionPower,
                Role_ModifyName.TagName,
                Role_SetRoleHead.TagName,
                CmdConstant.AccountBindReddotStatus,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdatePlayerActionPower:
                    RefeshUI();
                    break;
                case Role_ModifyName.TagName:
                    view.m_lbl_playerName_LanguageText.text = m_playerProxy.CurrentRoleInfo.name;
                    break;
                case Role_SetRoleHead.TagName:
                    view.m_UI_Model_PlayerHead.LoadPlayerIcon();
                    break;
                case CmdConstant.AccountBindReddotStatus:
                    RefreshPlayerSettingReddot();
                    break;
                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {

        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;

            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            m_country = m_playerProxy.Country();
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            view.m_UI_Item_PlayerDataMore.m_btn_btn_GameButton.onClick.AddListener(ShowPlayerPower);
            view.m_UI_Item_PlayerDataSetting.m_btn_btn_GameButton.onClick.AddListener(ShowSetting);
            view.m_UI_Item_PlayerDataLanguage.m_btn_btn_GameButton.onClick.AddListener(ShowRanking);
            view.m_UI_Item_PlayerDataArmy.m_btn_btn_GameButton.onClick.AddListener(ShowArmy);


            view.m_UI_Model_Window_TypeMid.setCloseHandle(onClose);
            view.m_UI_Model_Window_TypeMid.setWindowTitle(LanguageUtils.getText(300058));

            view.m_btn_name_GameButton.onClick.AddListener(ChangeName);

            view.m_UI_Model_StandardButton_MiniBlue.m_btn_languageButton_GameButton.onClick.AddListener(OnHeadIconChange);

            view.m_btn_apHelp_GameButton.onClick.AddListener(ShowPowerAction);



            view.m_btn_killHelp_GameButton.onClick.AddListener(ShowKill);


            //TODO
            view.m_btn_add_GameButton.onClick.AddListener(OnClickAddActionPoint);
            view.m_btn_picture_GameButton.onClick.AddListener(OnComingSoon);

            view.m_btn_changeCountry_GameButton.onClick.AddListener(OpenChangeCourtry);

            RefeshUI();
            RefreshPlayerSettingReddot();
        }

        private void RefreshPlayerSettingReddot()
        {
            var accountProxy = AppFacade.GetInstance().RetrieveProxy(AccountProxy.ProxyNAME) as AccountProxy;
            view.m_UI_Item_PlayerDataSetting.m_UI_Common_Redpoint.gameObject.SetActive(accountProxy.GetBindReddotStatus());
        }

        private void OnClickAddActionPoint()
        {
            if(crrPower >= maxPower)
            {
                Alert.CreateAlert(128006).SetLeftButton().SetRightButton(() =>
                {
                    PlayerDataHelp.ShowActionUI();
                }).Show();

                return;
            }
            PlayerDataHelp.ShowActionUI();
        }

        private void OnComingSoon()
        {
            Tip.CreateTip(100045, Tip.TipStyle.Middle).Show();
        }

        private void OnHeadIconChange()
        {
            CoreUtils.uiManager.ShowUI(UI.s_playerHeadPic);
        }

        private void ShowKill()
        {
            
            CoreUtils.assetService.Instantiate("UI_KillTip", (obj) =>
            { 
                UI_KillTipView powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_KillTipView>(obj);
                
                powTipView. m_lbl_tipkill_LanguageText.text = LanguageUtils.getTextFormat(610002, ClientUtils.FormatComma(m_playerProxy.killCount()));
                DateTime dt = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(1970, 1, 1)).AddSeconds(m_playerProxy.CurrentRoleInfo.createTime);
                powTipView.m_lbl_tipkilltime_LanguageText.text = LanguageUtils.getTextFormat(300111,dt.Year,dt.Month,dt.Day);

                powTipView.m_lbl_level1_LanguageText.text = m_playerProxy.CurrentRoleInfo.killCount.ContainsKey(1) ? m_playerProxy.CurrentRoleInfo.killCount[1].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level2_LanguageText.text = m_playerProxy.CurrentRoleInfo.killCount.ContainsKey(2) ? m_playerProxy.CurrentRoleInfo.killCount[2].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level3_LanguageText.text = m_playerProxy.CurrentRoleInfo.killCount.ContainsKey(3) ? m_playerProxy.CurrentRoleInfo.killCount[3].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level4_LanguageText.text = m_playerProxy.CurrentRoleInfo.killCount.ContainsKey(4) ? m_playerProxy.CurrentRoleInfo.killCount[4].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level5_LanguageText.text = m_playerProxy.CurrentRoleInfo.killCount.ContainsKey(5) ? m_playerProxy.CurrentRoleInfo.killCount[5].count.ToString("N0") : 0.ToString();
                
                HelpTip.CreateTip(powTipView.gameObject,(powTipView.gameObject.transform as RectTransform).sizeDelta,view.m_btn_killHelp_GameButton.gameObject.transform as RectTransform).SetStyle(HelpTipData.Style.arrowUp).Show();
                
               
            });
        }

        private void ShowPowerAction()
        {

            CoreUtils.assetService.Instantiate("UI_Pop_PowerTextTip", (obj) =>
            { 
                powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_PowerTextTipView>(obj);
                
                HelpTip.CreateTip(powTipView.gameObject,(powTipView.m_pl_content.transform as RectTransform).sizeDelta,view.m_btn_apHelp_GameButton.gameObject.transform as RectTransform).SetStyle(HelpTipData.Style.arrowUp).Show();
                
                UpdateTimeTick();
            });
            
        }

  

        private void UpdateTimeTick()
        {
            if (powTipView!=null && powTipView.m_lbl_tip_LanguageText!=null)
            {
                float vitalityRecoveryTime = CoreUtils.dataService.QueryRecords<ConfigDefine>()[0].vitalityRecoveryTime/1000 *
                                           (1 - m_playerAttributeProxy.GetCityAttribute(attrType.vitalityRecoveryMulti).value);

                long less = maxPower - crrPower;
                long remainTime = 0;
                if (less>0)
                {
                    remainTime = (long)(less * vitalityRecoveryTime -
                                 (ServerTimeModule.Instance.GetServerTime() -m_playerProxy.CurrentRoleInfo.lastActionForceTime  
                                  ));
                    
                    Timer.Register(1f, UpdateTimeTick);
                }
                
                powTipView.m_lbl_tip_LanguageText.text = LanguageUtils.getTextFormat(100708,
                    vitalityRecoveryTime,
                    m_playerAttributeProxy.GetCityAttribute(attrType.vitalityRecoveryMulti).value*100, ClientUtils.FormatTimeUpgrad((int) remainTime, true));
                
            }
            else
            {
               
                powTipView = null;
            }
        }

        private void ChangeHeadIcon()
        {
            
        }

        private void ChangeName()
        {
            CoreUtils.uiManager.ShowUI(UI.s_PlayerChangeName);
        }


        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_PlayerInfo);
        }

        private void ShowSetting()
        {
            CoreUtils.uiManager.ShowUI(UI.s_Setting);
        }

        private void ShowRanking()
        {
            if (SystemOpen.IsSystemOpen(EnumSystemOpen.rank))
            {
                CoreUtils.uiManager.ShowUI(UI.s_ranking);    
            }
        }
        private void ShowArmy()
        {
            CoreUtils.uiManager.ShowUI(UI.s_WinArmyConst);
        }


        private void ShowPlayerPower()
        {
            CoreUtils.uiManager.ShowUI(UI.s_PlayerPower);
        }


        private void RefeshUI()
        {
            view.m_lbl_roleid_LanguageText.text = LanguageUtils.getTextFormat(300086, m_playerProxy.GetEncryptId(m_playerProxy.CurrentRoleInfo.rid));
            //文明icon
            ClientUtils.LoadSprite( view.m_img_civiIcon_PolygonImage,m_country.civilizationMark);

            view.m_lbl_civil_LanguageText.text = LanguageUtils.getText(m_country.l_civilizationID);
            view.m_lbl_power_LanguageText.text = ClientUtils.FormatComma(m_playerProxy.CurrentRoleInfo.combatPower) ;
            view.m_lbl_kill_LanguageText.text = ClientUtils.FormatComma(m_playerProxy.killCount());
            view.m_lbl_playerName_LanguageText.text = m_playerProxy.CurrentRoleInfo.name;

            crrPower = m_playerProxy.CurrentRoleInfo.actionForce;
            maxPower = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).vitalityLimit+ (int)m_playerAttributeProxy.GetCityAttribute(attrType.maxVitality).value;
            view.m_UI_Model_AP_GameSlider.value = (float)crrPower / maxPower;

            view.m_lbl_value_LanguageText.text = string.Format("{0}/{1}", ClientUtils.FormatComma(crrPower), ClientUtils.FormatComma(maxPower));
            //联盟
            if (m_allianceProxy.HasJionAlliance())
            {
                view.m_lbl_guild_LanguageText.text = LanguageUtils.getTextFormat(300030, m_allianceProxy.GetAbbreviationName(), m_allianceProxy.GetAllianceName());
            }
            else
            {
                view.m_lbl_guild_LanguageText.text = LanguageUtils.getText(570029);
            }

            view.m_UI_Model_PlayerHead.LoadPlayerIcon();

        }
        /// <summary>
        /// 打开更换文明界面
        /// </summary>
        private void OpenChangeCourtry()
        {
            CoreUtils.uiManager.ShowUI(UI.s_PlayerChangeCivilization, null, 1); 
        }

        #endregion
    }
}
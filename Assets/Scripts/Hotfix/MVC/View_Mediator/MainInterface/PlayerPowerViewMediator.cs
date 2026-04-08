// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, January 7, 2020
// Update Time         :    Tuesday, January 7, 2020
// Class Description   :    PlayerPowerViewMediator
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

namespace Game {
    public class PlayerPowerViewMediator : GameMediator {
        #region Member
        public static string NameMediator = "PlayerPowerViewMediator";


        private PlayerProxy m_playerProxy;
        private int type = 0;//1自己，2其他人

        private RoleInfoEntity m_roleInfoEntity;
        #endregion

        //IMediatorPlug needs
        public PlayerPowerViewMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public PlayerPowerViewView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.UpdatePlayerPower,
                SprotoType.Role_ModifyName.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.UpdatePlayerPower:
                case SprotoType.Role_ModifyName.TagName:
                    RefreshUI();
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
            m_roleInfoEntity = view.data as RoleInfoEntity;
            if (m_roleInfoEntity == null)
            {
                m_roleInfoEntity = m_playerProxy.CurrentRoleInfo;
            }

            if (m_roleInfoEntity.rid == m_playerProxy.CurrentRoleInfo.rid)
            {
                type = 1;
            }
            else
            {
                type = 2;
            }
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            view.m_UI_Model_Window_Type1.setCloseHandle(onClose);
            
            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(300033));


            view.m_lbl_powerTitle_LanguageText.text = LanguageUtils.getText(300005);
            view.m_lbl_powerTotalTilte_LanguageText.text = LanguageUtils.getText(610008);
            view.m_lbl_resTotalTitle_LanguageText.text = LanguageUtils.getText(610015);
            
            view.m_btn_ackTip_GameButton.onClick.AddListener(ShowTip);
            
            view.m_btn_changename_GameButton.onClick.AddListener(OnChangeName);
            RefreshUI();
        }
        
        private void OnChangeName()
        {
            if (type == 1)
            {
                CoreUtils.uiManager.ShowUI(UI.s_PlayerChangeName);
            }
            else if (type == 2)
            {
                GUIUtility.systemCopyBuffer = m_roleInfoEntity.name;
                Tip.CreateTip(100706).Show();
            }
        }

        private void ShowTip()
        {
            CoreUtils.assetService.Instantiate("UI_KillTip", (obj) =>
            { 
                UI_KillTipView powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_KillTipView>(obj);
                
                
                powTipView. m_lbl_tipkill_LanguageText.text = LanguageUtils.getTextFormat(610002, ClientUtils.FormatComma(m_playerProxy.killCount(m_roleInfoEntity)));
             //   DateTime dt = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(1970, 1, 1)).AddSeconds(m_playerProxy.CurrentRoleInfo.createTime);
             //   powTipView.m_lbl_tipkilltime_LanguageText.text = LanguageUtils.getTextFormat(300111,dt.Year,dt.Month,dt.Day);

                powTipView.m_lbl_level1_LanguageText.text = m_roleInfoEntity.killCount.ContainsKey(1) ? m_roleInfoEntity.killCount[1].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level2_LanguageText.text = m_roleInfoEntity.killCount.ContainsKey(2) ? m_roleInfoEntity.killCount[2].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level3_LanguageText.text = m_roleInfoEntity.killCount.ContainsKey(3) ? m_roleInfoEntity.killCount[3].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level4_LanguageText.text = m_roleInfoEntity.killCount.ContainsKey(4) ? m_roleInfoEntity.killCount[4].count.ToString("N0") : 0.ToString();
                powTipView.m_lbl_level5_LanguageText.text = m_roleInfoEntity.killCount.ContainsKey(5) ? m_roleInfoEntity.killCount[5].count.ToString("N0") : 0.ToString();
                
                HelpTip.CreateTip(powTipView.gameObject,(powTipView.gameObject.transform as RectTransform).sizeDelta,view.m_btn_ackTip_GameButton.gameObject.transform as RectTransform).SetStyle(HelpTipData.Style.arrowUp).Show();
                
               
            });
        }




        private void RefreshUI()
        {
            if (type == 1)
            {
                view.m_pl_bg_VerticalLayoutGroup.gameObject.SetActive(true);
                view.m_pl_bg1_VerticalLayoutGroup .gameObject.SetActive(true);
                view.m_pl_bg2_VerticalLayoutGroup.gameObject.SetActive(true);
                ClientUtils.LoadSprite(view.m_btn_changename_PolygonImage,RS.RenameOrCopy[0]);
                view.m_lbl_name_LanguageText.text = m_roleInfoEntity.name;

                view.m_lbl_power_LanguageText.text = string.Format(LanguageUtils.getText(610014), ClientUtils.FormatComma(m_roleInfoEntity.combatPower));
                view.m_lbl_kill_LanguageText.text = string.Format(LanguageUtils.getText(610001), ClientUtils.FormatComma(m_playerProxy.killCount()));

                view.m_buildLine.SetData(610004, m_playerProxy.BuildPower());
                view.m_technologyLine.SetData(610005, m_playerProxy.TechnologyPower());
                view.m_armyLine.SetData(610006, m_playerProxy.ArmyPower());
                view.m_horeLine.SetData(610007, m_playerProxy.HeroPower());

                view.m_historyHightPower.SetData(610009, m_playerProxy.HistoryHightPower());
                view.m_wincount.SetData(610010, m_playerProxy.WinCount());
                view.m_losscount.SetData(610011, m_playerProxy.LoseCount());
                view.m_deadcount.SetData(610012, m_playerProxy.DeadCount());
                view.m_spycount.SetData(610013, m_playerProxy.SkyCount());

                view.m_resLine.SetData(610016, m_playerProxy.ResourceCollection());
                view.m_resHelpLine.SetData(610017, m_playerProxy.HelpCount());
                view.m_guildHelp.SetData(610018, m_playerProxy.GuildHelpCount());

                CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_roleInfoEntity.country);
                view.m_UI_Model_PlayerHead.LoadPlayerIcon();
            }
            else if (type == 2)
            {
                view.m_pl_bg_VerticalLayoutGroup.gameObject.SetActive(false);
                view.m_pl_bg1_VerticalLayoutGroup.gameObject.SetActive(true);
                view.m_pl_bg2_VerticalLayoutGroup.gameObject.SetActive(true);
                RectTransform rectTransform = view.m_c_list_view_VerticalLayoutGroup.transform as RectTransform;
                RectTransform rectTransform_0 = view.m_pl_bg_VerticalLayoutGroup .transform as RectTransform;
                RectTransform rectTransform_1 = view.m_pl_bg1_VerticalLayoutGroup.transform as RectTransform;
                RectTransform rectTransform_2 = view.m_pl_bg2_VerticalLayoutGroup.transform as RectTransform;
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform_1.sizeDelta.y+ rectTransform_2.sizeDelta.y);
                }

                ClientUtils.LoadSprite(view.m_btn_changename_PolygonImage, RS.RenameOrCopy[1]);
                view.m_lbl_name_LanguageText.text = m_roleInfoEntity.name;

                view.m_lbl_power_LanguageText.text = string.Format(LanguageUtils.getText(610014), ClientUtils.FormatComma(m_roleInfoEntity.combatPower));
                view.m_lbl_kill_LanguageText.text = string.Format(LanguageUtils.getText(610001), ClientUtils.FormatComma(m_playerProxy.killCount(m_roleInfoEntity)));

                view.m_historyHightPower.SetData(610009, m_roleInfoEntity.historyPower);
                view.m_wincount.SetData(610010, m_playerProxy.WinCount(m_roleInfoEntity));
                view.m_losscount.SetData(610011, m_playerProxy.LoseCount(m_roleInfoEntity));
                view.m_deadcount.SetData(610012, m_playerProxy.DeadCount(m_roleInfoEntity));
                view.m_spycount.SetData(610013, m_playerProxy.SkyCount(m_roleInfoEntity));

                view.m_resLine.SetData(610016, m_playerProxy.ResourceCollection(m_roleInfoEntity));
                view.m_resHelpLine.SetData(610017, m_playerProxy.HelpCount(m_roleInfoEntity));
                view.m_guildHelp.SetData(610018, m_playerProxy.GuildHelpCount(m_roleInfoEntity));

                CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_roleInfoEntity.country);
                view.m_UI_Model_PlayerHead.LoadPlayerIcon(m_roleInfoEntity.headId, m_roleInfoEntity.headFrameID);
            }
        }





        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_PlayerPower);
        }

        #endregion
    }
}
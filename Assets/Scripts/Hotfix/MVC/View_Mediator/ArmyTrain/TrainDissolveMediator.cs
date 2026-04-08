// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年1月2日
// Update Time         :    2020年1月2日
// Class Description   :    TrainDissolveMediator 解散士兵
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
    public class TrainDissolveParam
    {
        public int Id;
        public int ArmyNum;
    }

    public class TrainDissolveMediator : GameMediator {
        #region Member
        public static string NameMediator = "TrainDissolveMediator";

        public TrainDissolveParam m_param;

        private PlayerProxy m_playerProxy;

        private ArmsDefine m_armDefine;

        private bool m_isChangeInput;
        private bool m_isChangeSilder;

        private InputSliderControl m_control;

        private int m_currNum;
        #endregion

        //IMediatorPlug needs
        public TrainDissolveMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public TrainDissolveView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {

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
            if (view.data == null)
            {
                Debug.LogError("参数不能为空");
                return;
            }
            m_param = view.data as TrainDissolveParam;
            if (m_param == null)
            {
                return;
            }
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            m_armDefine = CoreUtils.dataService.QueryRecord<ArmsDefine>(m_param.Id);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_TypeMid.setCloseHandle(Close);
            view.m_btn_anim.m_btn_languageButton_GameButton.onClick.AddListener(OnSure);

            m_control = new InputSliderControl();
            m_control.Init(view.m_ipt_count_GameInput, view.m_sd_count_GameSlider, ChangeNum);

            Refresh();
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void Refresh()
        {
            ClientUtils.LoadSprite(view.m_img_army_icon.m_img_army_icon_PolygonImage, SoldierProxy.GetArmyHeadIcon(m_armDefine.ID));

            m_isChangeInput = true;

            m_control.UpdateMinMax(0, m_param.ArmyNum);
            m_control.SetInputVal(0);
        }

        private void ChangeNum(int num, int index)
        {
            m_currNum = num;

            int num1 = m_armDefine.militaryCapability * m_currNum;

            view.m_lbl_combatDown_LanguageText.text = LanguageUtils.getTextFormat(192022, ClientUtils.FormatComma(num1));
        }

        private void OnSure()
        {
            if (m_currNum == 0)
            {
                CoreUtils.uiManager.CloseUI(UI.s_trainDissolve);
                return;
            }
            string str = LanguageUtils.getTextFormat(192032, m_currNum, LanguageUtils.getText(m_armDefine.l_armsID));
            Alert.CreateAlert(str, LanguageUtils.getText(300099)).SetLeftButton().SetRightButton(() => {
                var sp = new Role_DisbandArmy.request();
                sp.type = m_armDefine.armsType;
                sp.num = m_currNum;
                sp.level = m_armDefine.armsLv;
                AppFacade.GetInstance().SendSproto(sp);

                CoreUtils.uiManager.CloseUI(UI.s_trainDissolve);
            }).Show();
        }

        private void Close()
        {
            CoreUtils.uiManager.CloseUI(UI.s_trainDissolve);
        }
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月20日
// Update Time         :    2020年5月20日
// Class Description   :    UI_Win_CivilizationChangeMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using Data;
using Hotfix;
using System.Text;

namespace Game {
    public class UI_Win_CivilizationChangeMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_CivilizationChangeMediator";

        private PlayerProxy _playerProxy;
        private TrainProxy _trainProxy;
        private HospitalProxy _hospitalProxy;
        private CivilizationDefine m_civilizationSelected = null;
        private CreateCharMediator m_createCharMd;

        #endregion

        //IMediatorPlug needs
        public UI_Win_CivilizationChangeMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_CivilizationChangeView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
              Role_ChangeCivilization.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_ChangeCivilization.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;


                        switch ((ErrorCode)error.errorCode)
                        {
                            case ErrorCode.ROLE_ARMY_IN_TRAINING:

                                Tip.CreateTip(LanguageUtils.getTextFormat(130403, 3)).Show();
                                break;
                            case ErrorCode.ROLE_TREATMENT_NOT_FINISH:

                                Tip.CreateTip(LanguageUtils.getTextFormat(130403, 4)).Show();
                                break;
                        }
                        
                        
                    }
                    else
                    {
                        var changeResponse = notification.Body as Role_ChangeCivilization.response;
                        if (changeResponse != null)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.ReloadGame);
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
            _playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            _trainProxy = AppFacade.GetInstance().RetrieveProxy(TrainProxy.ProxyNAME) as TrainProxy;
            _hospitalProxy = AppFacade.GetInstance().RetrieveProxy(HospitalProxy.ProxyNAME) as HospitalProxy;

            m_createCharMd =
                AppFacade.GetInstance().RetrieveMediator(CreateCharMediator.NameMediator) as CreateCharMediator;
            m_civilizationSelected = (CivilizationDefine)view.data;
        }

        protected override void BindUIEvent()
        {
            view.m_btn_sure.m_btn_languageButton_GameButton.onClick.AddListener(ConfrimChangeCivilization);
            view.m_UI_Model_Window_Type2.m_btn_close_GameButton.onClick.AddListener(CloseWindow);
        }

        protected override void BindUIData()
        {
            
        }

        #endregion
        /// <summary>
        /// 确认更改文明
        /// </summary>
        private void ConfrimChangeCivilization()
        {
            var troopCheck = WorldMapLogicMgr.Instance.PlayTroopCheckHandler;
            List<int> cds = new List<int>(4);
            if (troopCheck.isHaveMap())
            {
                cds.Add(1);
            }
            if (troopCheck.isHaveFight())
            {
                cds.Add(2);
            }
            if (_trainProxy.IsTraining())
            {
                cds.Add(3);
            }
            if (_hospitalProxy.GetHospitalStatus() == (int)EnumHospitalStatus.Wound|| _hospitalProxy.GetHospitalStatus() == (int)EnumHospitalStatus.Treatment|| _hospitalProxy.GetHospitalStatus() == (int)EnumHospitalStatus.Finished)
            {
                cds.Add(4);
            }
            if (cds.Count > 0)
            {
                string cdsStr = String.Join(",",cds);
                CoreUtils.uiManager.CloseUI(UI.s_ChangeCivilization);
                AppFacade.GetInstance().SendNotification(CmdConstant.ChangeCivilizationCmd, cdsStr);
                return;
            }
            _playerProxy.ChangeCountry(m_civilizationSelected,m_createCharMd.IsTool());
        }

        private void CloseWindow()
        {
            CoreUtils.uiManager.CloseUI(UI.s_ChangeCivilization);
        }
    }
}
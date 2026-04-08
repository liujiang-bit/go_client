// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月24日
// Update Time         :    2020年2月24日
// Class Description   :    WorldPosSearchMediator
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

namespace Game {
    public class WorldPosSearchMediator : GameMediator {
        #region Member
        public static string NameMediator = "WorldPosSearchMediator";

        private PlayerProxy m_playerProxy;

        private int maxX;
        private int maxY;


        private int inputX = -1;
        private int inputY = -1;

        private int kingdomClientDistance;
        #endregion

        //IMediatorPlug needs
        public WorldPosSearchMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public WorldPosSearchView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
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
        }

        protected override void BindUIEvent()
        {
            view.m_btn_search_GameButton.onClick.AddListener(OnSearch);
            view.m_UI_Model_PosInput_X.m_ipt_val_GameInput.onValueChanged.AddListener(OnInputX);
            view.m_UI_Model_PosInput_Y.m_ipt_val_GameInput.onValueChanged.AddListener(OnInputY);
        }

        protected override void BindUIData()
        {
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            kingdomClientDistance = config.kingdomClientDistance;
            maxX = config.kingdomMapWidth / config.kingdomClientDistance;
            maxY = config.kingdomMapLength / config.kingdomClientDistance;
            view.m_UI_Model_PosInput_server.m_ipt_val_GameInput.text = m_playerProxy.GetGameNode().ToString("N0");
        }

        private void OnInputX(string str)
        {
            int x = 0;
            if(int.TryParse(str,out x))
            {
                if (x > maxX)
                {
                    view.m_UI_Model_PosInput_X.m_ipt_val_GameInput.text = maxX.ToString();
                    return;
                }
                else if ( x < 0)
                {
                    view.m_UI_Model_PosInput_X.m_ipt_val_GameInput.text = 0.ToString();
                    return;
                }
                inputX = x * kingdomClientDistance;
            }
        }

        private void OnInputY(string str)
        {
            int y = 0;
            if (int.TryParse(str, out y))
            {
                if (y > maxY)
                {
                    view.m_UI_Model_PosInput_Y.m_ipt_val_GameInput.text = maxY.ToString();
                    return;
                }
                else if (y < 0)
                {
                    view.m_UI_Model_PosInput_Y.m_ipt_val_GameInput.text = 0.ToString();
                    return;
                }
                inputY = y*kingdomClientDistance;
            }
        }


        private void OnSearch()
        {
            if(view.m_UI_Model_PosInput_X.m_ipt_val_GameInput.text==string.Empty|| view.m_UI_Model_PosInput_Y.m_ipt_val_GameInput.text == string.Empty)
            {
                Tip.CreateTip(100716,Tip.TipStyle.Middle).Show();
                return;
            }

            OnClose();
            if (inputX>=0&&inputY>=0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
                WorldCamera.Instance().ViewTerrainPos(inputX, inputY, 1000, null);
//                Map_Move.request req = new Map_Move.request();
//                req.posInfo = new PosInfo();
//                req.posInfo.x = inputX * 100;
//                req.posInfo.y = inputY * 100;
//                req.isPreview = false;
//                AppFacade.GetInstance().SendSproto(req);
                
                var m_RssProxy = AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
                m_RssProxy.SendMapMove(inputX , inputY);
                
            }
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_worldPosSearch);
        }
       
        #endregion
    }
}
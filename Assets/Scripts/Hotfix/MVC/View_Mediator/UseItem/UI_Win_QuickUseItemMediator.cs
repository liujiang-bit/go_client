// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月5日
// Update Time         :    2020年6月5日
// Class Description   :    UI_Win_QuickUseItemMediator 快速使用 道具
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
using System;

namespace Game {
    public class UI_Win_QuickUseItemMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_QuickUseItemMediator";

        private BagProxy m_bagProxy;
        private ItemDefine m_itemDefine;

        private float m_cameraDxf;

        #endregion

        //IMediatorPlug needs
        public UI_Win_QuickUseItemMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_QuickUseItemView view;

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

        public override void OnRemove()
        {
            base.OnRemove();
            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            int itemId = (int)view.data;
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemId);
            m_itemDefine = itemDefine;
            Int64 count = m_bagProxy.GetItemNum(itemId);
            view.m_UI_Model_Item.Refresh(itemDefine, count);
            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(m_itemDefine.l_nameID);

            WorldCamera.Instance().AddViewChange(OnWorldViewChange);

            m_cameraDxf = WorldCamera.Instance().getCameraDxf("dispatch");
        }

        protected override void BindUIEvent()
        {
            view.m_btn_use.m_btn_languageButton_GameButton.onClick.AddListener(OnUse);
            view.m_btn_close_GameButton.onClick.AddListener(OnClose);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void OnWorldViewChange(float x, float y, float dxf)
        {
            if (dxf <= m_cameraDxf)
            {
                OnClose();
            }
        }

        private void OnUse()
        {
            if (m_itemDefine.itemFunction == 10)
            {
                Int64 itemIndex = m_bagProxy.GetItemIndex(m_itemDefine.ID);
                CoreUtils.uiManager.ShowUI(UI.s_openFogShow, null, itemIndex);
            }
            else
            {

            }
            OnClose();
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_quickUseItem);
        }
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    UI_Win_PlayerNewCharMediator
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

namespace Game {
    public class UI_Win_PlayerNewCharMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_PlayerNewCharMediator";
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private RoleInfoProxy m_RoleInfoProxy;
        private List<ServerListTypeDefine> lsServerListTypeDefines =new List<ServerListTypeDefine>();

        #endregion

        //IMediatorPlug needs
        public UI_Win_PlayerNewCharMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_PlayerNewCharView view;

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
            m_RoleInfoProxy=AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                OnInitData();
                AssetLoadFinish();
            });
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.m_btn_close_GameButton.onClick.AddListener(OnBtnClosePanelClick);
        }

        protected override void BindUIData()
        {

        }

        private void AssetLoadFinish()
        {
            Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
            prefabNameList["UI_Item_PlayerNewChar"] = m_assetDic["UI_Item_PlayerNewChar"];
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = OnListItemEnter;
            funcTab.GetItemPrefabName = OnListGetPrefabName;
            view.m_sv_list_ListView.SetInitData(prefabNameList, funcTab);
            if (lsServerListTypeDefines != null)
            {
                int count = Mathf.CeilToInt(lsServerListTypeDefines.Count * 1.0f / 2);;
                view.m_sv_list_ListView.FillContent(count);
            }
        }

        private void OnInitData()
        {
            lsServerListTypeDefines = m_RoleInfoProxy.GetServerLists();
        }


        private string OnListGetPrefabName(ListView.ListItem listItem)
        {
            return "UI_Item_PlayerNewChar";
        }

        private void OnListItemEnter(ListView.ListItem listItem)
        {
            UI_Item_PlayerNewCharView itemView =
                MonoHelper.GetHotFixViewComponent<UI_Item_PlayerNewCharView>(listItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_PlayerNewCharView>(listItem.go);              
            }

            if (itemView != null)
            {
                List<ServerListTypeDefine> serverListTypeDefines=new List<ServerListTypeDefine>();
                for (int i = listItem.index * 2; i < lsServerListTypeDefines.Count && i < (listItem.index + 1) * 2; i++)
                {
                    serverListTypeDefines.Add(lsServerListTypeDefines[i]);
                }
                
                if (serverListTypeDefines.Count >= 1)
                {                  
                    itemView.m_UI_Item_PlayerNewCharItem1.SetData(serverListTypeDefines[0]);  
                }

                if (serverListTypeDefines.Count >= 2)
                {                   
                    itemView.m_UI_Item_PlayerNewCharItem2.SetData(serverListTypeDefines[1]);
                }
                else
                {
                    itemView.m_UI_Item_PlayerNewCharItem2.SetData(null);
                }
            }
        }


        private void OnBtnClosePanelClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_PlayerNewChar);
        }

        #endregion
    }
}
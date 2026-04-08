// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月17日
// Update Time         :    2020年9月17日
// Class Description   :    UI_Win_PlayerManageMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Win_PlayerManageMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_PlayerManageMediator";
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<RoleInfoEntity> lsRoleInfoEntitys= new List<RoleInfoEntity>();
        public const long copyRid = Int64.MaxValue;

        #endregion

        //IMediatorPlug needs
        public UI_Win_PlayerManageMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_PlayerManageView view;
        private RoleInfoProxy m_RoleInfoProxy;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_GetRoleList.TagName,
                CmdConstant.RoleInfo_Refresh
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case  Role_GetRoleList.TagName:
                    m_RoleInfoProxy.UpdateRoleInfoData(notification);
                    break;
                case CmdConstant.RoleInfo_Refresh:
                    InitPanelData();
                    OnRefreshListView();
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
            m_RoleInfoProxy = AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                InitPanelData();
                m_assetDic = assetDic;
                AssetLoadFinish();
            });
            
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.m_btn_close_GameButton.onClick.AddListener(OnCloseBtnClick);
        }

        protected override void BindUIData()
        {

        }

        private void InitPanelData()
        {
            lsRoleInfoEntitys.Clear();
            foreach (var info in m_RoleInfoProxy.GetRoleInfoEntityS())
            {
                lsRoleInfoEntitys.Add(info);
            }
            if (lsRoleInfoEntitys != null)
            {
                lsRoleInfoEntitys.Sort(Sort);
                RoleInfoEntity roleInfoEntityNull= new RoleInfoEntity();
                roleInfoEntityNull.rid = copyRid;
                lsRoleInfoEntitys.Add(roleInfoEntityNull);
            }

        }

        private int Sort(RoleInfoEntity roleInfoEntityA, RoleInfoEntity roleInfoEntityB)
        {
            if (roleInfoEntityA.historyPower > roleInfoEntityB.historyPower)
            {
                return -1;
            }
            
            if (roleInfoEntityA.historyPower < roleInfoEntityB.historyPower)
            {
                return 1;
            }
            

            int gameA = RoleInfoHelp.GetRoleInfoServerId(roleInfoEntityA.gameNode);
            int gameB = RoleInfoHelp.GetRoleInfoServerId(roleInfoEntityB.gameNode);
            if (gameA > gameB)
            {
                return -1;
            }

            if (gameA < gameB)
            {
                return 1;
            }

            if (roleInfoEntityA.rid > roleInfoEntityB.rid)
            {
                return -1;
            }

            if (roleInfoEntityA.rid < roleInfoEntityB.rid)
            {
                return 1;
            }
            return 0;
        }


        private void AssetLoadFinish()
        {
            Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
            prefabNameList["UI_Item_PlayerManage"] = m_assetDic["UI_Item_PlayerManage"];
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = OnListItemEnter;
            funcTab.GetItemPrefabName = OnListGetPrefabName;
            view.m_sv_list_ListView.SetInitData(prefabNameList, funcTab);
            OnRefreshListView();
        }

        private void OnRefreshListView()
        {
            int count= Mathf.CeilToInt(lsRoleInfoEntitys.Count * 1.0f / 2);
            view.m_sv_list_ListView.FillContent(count);
        }

        private string OnListGetPrefabName(ListView.ListItem listItem)
        {
            return "UI_Item_PlayerManage";
        }

        private void OnListItemEnter(ListView.ListItem listItem)
        {
            UI_Item_PlayerManageView itemView =
                MonoHelper.GetHotFixViewComponent<UI_Item_PlayerManageView>(listItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_PlayerManageView>(listItem.go);              
            }

            if (itemView != null)
            {
                List<RoleInfoEntity> roleInfoEntities= new List<RoleInfoEntity>();
                for (int i = listItem.index * 2; i < lsRoleInfoEntitys.Count && i < (listItem.index + 1) * 2; i++)
                {
                    roleInfoEntities.Add(lsRoleInfoEntitys[i]);
                }
                if (roleInfoEntities.Count >= 1)
                {                  
                    itemView.m_UI_Item_PlayerManageList1.SetData(roleInfoEntities[0]);  
                }

                if (roleInfoEntities.Count >= 2)
                {                   
                    itemView.m_UI_Item_PlayerManageList2.SetData(roleInfoEntities[1]);
                }
                else
                {
                    itemView.m_UI_Item_PlayerManageList2.SetData(null);
                }
            }
        }


        private void OnCloseBtnClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_PlayerManage);
        }

        #endregion
    }
}
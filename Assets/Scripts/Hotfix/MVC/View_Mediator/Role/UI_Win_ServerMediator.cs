// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    UI_Win_ServerMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Win_ServerMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ServerMediator";
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        #endregion

        //IMediatorPlug needs
        public UI_Win_ServerMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ServerView view;
        private RoleInfoProxy m_RoleInfoProxy;
        private List<ServerListTypeDefineTag> lsServerListTypeDefinesGrop= new List<ServerListTypeDefineTag>();

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_GetRoleList.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case  Role_GetRoleList.TagName:
                    m_RoleInfoProxy.UpdateRoleInfoData(notification);
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
            m_RoleInfoProxy=AppFacade.GetInstance().RetrieveProxy(RoleInfoProxy.ProxyNAME) as RoleInfoProxy;
            m_preLoadRes.AddRange(view.m_sv_list1_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                InitPanelData();
                AssetLoadFinish();
            });
        }

        protected override void BindUIEvent()
        {
           view.m_UI_Model_Window_Type3.m_btn_close_GameButton.onClick.AddListener(() =>
           {
               CoreUtils.uiManager.CloseUI(UI.s_WinServer);
           });
        }

        protected override void BindUIData()
        {

        }

        private void InitPanelData()
        {          
            lsServerListTypeDefinesGrop.Clear();        
            foreach (var info in m_RoleInfoProxy.GetDicServerListTypeDefinesGroup)
            {           
                m_RoleInfoProxy.SortGroup(info.Key);
                ServerListTypeDefineTag serverListTypeDefineTag= new ServerListTypeDefineTag();
                serverListTypeDefineTag.PrefabIndex = 0;
                if (info.Value.Count > 0)
                {                   
                    serverListTypeDefineTag.M_ServerListTypeDefineMin  = info.Value[0];
                    serverListTypeDefineTag.M_ServerListTypeDefingMax = info.Value[info.Value.Count-1];
                }

                serverListTypeDefineTag.LsServerListTypeDefines.Clear();
                serverListTypeDefineTag.itemType = info.Key;
                foreach (var infoData in info.Value)
                {
                    serverListTypeDefineTag.LsServerListTypeDefines.Add(infoData);
                }
                lsServerListTypeDefinesGrop.Add(serverListTypeDefineTag);
            }        
        }


        private void AssetLoadFinish()
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ViewItemByIndex;
            funcTab.GetItemSize = GetItemSize;
            funcTab.GetItemPrefabName = GetItemPrefabName;                
            view.m_sv_list1_ListView.SetInitData(m_assetDic, funcTab);
            AddMember(1,0,lsServerListTypeDefinesGrop[0]);
            lsServerListTypeDefinesGrop[0].isSelected = true;
            view.m_sv_list1_ListView.FillContent(lsServerListTypeDefinesGrop.Count);
        }
        
        private string GetItemPrefabName(ListView.ListItem item)
        {
            var data = lsServerListTypeDefinesGrop[item.index];

            return view.m_sv_list1_ListView.ItemPrefabDataList[data.PrefabIndex];
        }
        
        private float GetItemSize(ListView.ListItem item)
        {
            var data = lsServerListTypeDefinesGrop[item.index];

            if (data.PrefabIndex == 0)
            {
                return 76f;
            }
            if (data.PrefabIndex == 1)
            {
                return 100;
            }

            return 90;
        }

        private void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            var data = lsServerListTypeDefinesGrop[scrollItem.index];
            if (data != null)
            {
                if (data.PrefabIndex == 0)
                {
                    UI_Item_ServerTitleView  itemView=  MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ServerTitleView>(scrollItem.go);
                    if (itemView != null)
                    {
                        if (data.M_ServerListTypeDefineMin != null)
                        {
                            if (data.M_ServerListTypeDefineMin.groupNameId > 0)
                            {
                                itemView.m_lbl_kingdomName_LanguageText.text = LanguageUtils.getText(data.M_ServerListTypeDefineMin.groupNameId);
                            }
                            else
                            {
                                itemView.m_lbl_kingdomName_LanguageText.text = string.Empty;
                            }

                            itemView.m_lbl_kingdomNum_LanguageText.text =
                                LanguageUtils.getTextFormat(100528, data.M_ServerListTypeDefineMin.severId, data.M_ServerListTypeDefingMax.severId);
                        }
                        
                        itemView.m_img_arrowdown_PolygonImage.gameObject.SetActive(data.isSelected);
                        itemView.m_img_arrowup_PolygonImage.gameObject.SetActive(!data.isSelected);
                        itemView.m_btn_title_GameButton.onClick.RemoveAllListeners();
                        itemView.m_btn_title_GameButton.onClick.AddListener(() =>
                        {
                            data.isSelected = !data.isSelected;
                            itemView.m_img_arrowdown_PolygonImage.gameObject.SetActive(true);
                            itemView.m_img_arrowup_PolygonImage.gameObject.SetActive(false);
                            if (data.isSelected)
                            {        
                                AddMember(data.itemType,scrollItem.index,data);
                                view.m_sv_list1_ListView.FillContent(lsServerListTypeDefinesGrop.Count);
                            }
                            else
                            {
                                if (data.itemType > 0)
                                {                               
                                    RemoveMember(data.itemType);
                                }

                                view.m_sv_list1_ListView.FillContent(lsServerListTypeDefinesGrop.Count);
                            }
                        });
                    }                  
                }else if (data.PrefabIndex == 1)
                {
                    UI_Item_ServerListView  itemView=  MonoHelper.GetOrAddHotFixViewComponent<UI_Item_ServerListView>(scrollItem.go);
                    if (itemView != null)
                    {
                        if (data.subItemsData.Count >= 1)
                        {                           
                            itemView.m_UI_Item_ServerItem1.SetData(data.subItemsData[0]);
                        }
                      
                        if (data.subItemsData.Count >= 2)
                        {          
                            itemView.m_UI_Item_ServerItem2.SetData(data.subItemsData[1]);  
                        }
                        else
                        {
                            itemView.m_UI_Item_ServerItem2.SetData(null);                            
                        }
                    }
                }
            }
        }

        private void AddMember(int buildType,int index, ServerListTypeDefineTag data)
        {   
            int len = data.LsServerListTypeDefines.Count;
            for (int i = 0; i < len; i = i + 2)
            {            
                ServerListTypeDefineTag newServerListTypeDefineTag= new ServerListTypeDefineTag();
                newServerListTypeDefineTag.PrefabIndex = 1;
                newServerListTypeDefineTag.subItemsData= new List<ServerListTypeDefine>();
                newServerListTypeDefineTag.itemType = buildType;
                for (int j = i; j < i + 2; j++)
                {
                    if (j < len)
                    {
                        newServerListTypeDefineTag.subItemsData.Add(data.LsServerListTypeDefines[j]);                    
                    }
                }
                lsServerListTypeDefinesGrop.Insert(index+1,newServerListTypeDefineTag);
                index++;
            }
        }

        private void RemoveMember(int buildType)
        {
            int startIndex = 0;
            int count = 0;
            int len = lsServerListTypeDefinesGrop.Count;
            for (int i = 0; i < len; i++)
            {
                var item = lsServerListTypeDefinesGrop[i];
                if (item.itemType == buildType)
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
            lsServerListTypeDefinesGrop.RemoveRange(startIndex,count);
        }

        #endregion
    }
}
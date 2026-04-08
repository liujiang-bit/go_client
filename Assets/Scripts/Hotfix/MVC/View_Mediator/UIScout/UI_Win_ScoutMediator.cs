// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Win_ScoutMediator
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

namespace Game {
    public class UI_Win_ScoutMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_ScoutMediator";
        private ScoutProxy m_scoutProxy;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private Timer m_timer;

        #endregion

        //IMediatorPlug needs
        public UI_Win_ScoutMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_ScoutView view;

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
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;

            List<string> prefabNames = new List<string>();
            prefabNames.Add("UI_Item_Scout");
            prefabNames.Add("UI_Item_ScoutDesc");
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.setCloseHandle(onClose);
            view.m_UI_Model_Window_Type1.AddBackEvent(ShowList);
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_scoutWin);
        }

        protected override void BindUIData()
        {

        }

        #endregion
        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            InitUI();
        }

        List<ItemData> m_itemsData = new List<ItemData>();
        class ItemData
        {
            public bool descType;
            public string prefabName;
            public ScoutProxy.ScoutInfo info;
            public float height;
        }
        private void InitUI()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = OnItemEnter;
            functab.GetItemPrefabName = OnGetItemPrefabName;
            functab.GetItemSize = OnGetItemSize;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);

            InitScout();
        }
        private void InitScout()
        {
            m_itemsData.Clear();

            float height1 = m_assetDic["UI_Item_Scout"].GetComponent<RectTransform>().sizeDelta.y;
            float height2 = m_assetDic["UI_Item_ScoutDesc"].GetComponent<RectTransform>().sizeDelta.y;

            var item2 = new ItemData();
            item2.descType = true;
            item2.height = height2;
            item2.prefabName = "UI_Item_ScoutDesc";

            m_itemsData.Add(item2);

            bool isOpenTimer = false;
            int nNum = m_scoutProxy.GetScoutNum();
            for(int i = 0; i < nNum; i++)
            {
                var info = m_scoutProxy.GetScoutInfo(i);
                var item = new ItemData();
                item.descType = false;
                item.height = height2;
                item.info = info;
                item.prefabName = "UI_Item_Scout";
                m_itemsData.Add(item);
                if (info.state != ScoutProxy.ScoutState.None)
                {
                    isOpenTimer = true;
                }
            }
            view.m_sv_list_view_ListView.FillContent(m_itemsData.Count);
            if (isOpenTimer)
            {
                OpenCountDown();
            }
        }

        private string OnGetItemPrefabName(ListView.ListItem listItem)
        {
            return m_itemsData[listItem.index].prefabName;
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            return m_itemsData[listItem.index].height;
        }

        private void OnItemEnter(ListView.ListItem listItem)
        {
            var itemData = m_itemsData[listItem.index];
            if (listItem.isInit == false)
            {
                if (itemData.descType == false)
                {
                    var subView = new UI_Item_Scout_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.SetScoutInfo(itemData.info);
                }
                else
                {
                    var subView = new UI_Item_ScoutDesc_SubView(listItem.go.GetComponent<RectTransform>());
                    subView.m_btn_info_GameButton.onClick.RemoveAllListeners();
                    subView.m_btn_info_GameButton.onClick.AddListener(ShowDesc);
                    view.m_lbl_Desc_LanguageText.text = LanguageUtils.getText(181002);
                    listItem.data = subView;
                }
                listItem.isInit = true;
            }
            else
            {
                if (itemData.descType == false)
                {
                    var subView = (UI_Item_Scout_SubView)listItem.data;
                    subView.SetScoutInfo(itemData.info);
                }
                else
                {
                    var subView = (UI_Item_ScoutDesc_SubView)listItem.data;
                    subView.UpdateData();
                }
            }
        }

        public void ShowDesc()
        {
            view.m_lbl_Desc_LanguageText.gameObject.SetActive(true);
            view.m_sv_list_view_ListView.gameObject.SetActive(false);
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_lbl_Desc_Animator.Play("Show");

        }
        public void ShowList()
        {
            view.m_lbl_Desc_LanguageText.gameObject.SetActive(false);
            view.m_sv_list_view_ListView.gameObject.SetActive(true);
            view.m_UI_Model_Window_Type1.m_btn_back_GameButton.gameObject.SetActive(false);
            view.m_sv_list_view_Animator.Play("Show");
        }
  
        private void OpenCountDown()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateTime, null, true, false, view.m_sv_list_view_ListView);
            }
        }

        private void CancelCountDown()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
            }
        }

        private void UpdateTime()
        {
            bool isCancelTimer = true;
            ListView.ListItem listItem;
            for (int i = 0; i < m_itemsData.Count; i++)
            {
                listItem = view.m_sv_list_view_ListView.GetItemByIndex(i);
                if (listItem != null && listItem.data != null)
                {
                    if (m_itemsData[i].descType)
                    {
                        var subView = listItem.data as UI_Item_ScoutDesc_SubView;
                        subView.UpdateData();
                    }
                    else
                    {
                        var subView = listItem.data as UI_Item_Scout_SubView;
                        subView.RefreshScoutInfo(m_itemsData[i].info);
                        if (m_itemsData[i].info.state != ScoutProxy.ScoutState.None)
                        {
                            isCancelTimer = false;
                        }
                    }
                }
            }
            if (isCancelTimer)
            {
                //取消定时器
                CancelCountDown();
            }
        }
    }
}
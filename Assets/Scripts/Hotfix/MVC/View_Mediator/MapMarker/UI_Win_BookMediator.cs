// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月15日
// Update Time         :    2020年9月15日
// Class Description   :    UI_Win_BookMediator
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
    public class UI_Win_BookMediator : GameMediator {
        #region Member

        public static string NameMediator = "UI_Win_BookMediator";

        private MapMarkerProxy m_mapMarkerProxy;
        private AllianceProxy m_allianceProxy;

        private bool m_loadFinishFlag = false;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();        
        private string[] m_pageEmptyTextArray = new string[5];
        private UI_Item_BookGroup_SubView[] m_pageBtnArray = new UI_Item_BookGroup_SubView[5];        
        private int m_defaultPageIndex = 0;
        private int m_currentPageIndex = 0;
        private List<MapMarkerInfo> m_mapMarkerInfoList;
        
        #endregion

        //IMediatorPlug needs
        public UI_Win_BookMediator(object viewComponent ):base(NameMediator, viewComponent ) {}

        public UI_Win_BookView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Map_ModifyMarker.TagName,
                Map_DeleteMarker.TagName,
                CmdConstant.GuildMapMarkerInfoChanged
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Map_ModifyMarker.TagName:
                    RefreshView();
                    break;
                case Map_DeleteMarker.TagName:
                    RefreshView();
                    break;
                case CmdConstant.GuildMapMarkerInfoChanged:
                    RefreshView();
                    break;
                default:
                    break;
            }
        }       

        #region UI template method

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {
            
        }

        public override void WinClose()
        {
            if (m_allianceProxy.HasJionAlliance())
            {
                m_mapMarkerProxy.UpdateMapMarker();
            }            
        }

        public override void PrewarmComplete()
        {
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_mapMarkerProxy = AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.setCloseHandle(OnClose);
        }

        protected override void BindUIData()
        {
            List<string> prefabs = new List<string>();
            prefabs.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabs, OnLoadFinish);
        }

        #endregion

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_loadFinishFlag = true;
            m_assetDic = asset;

            InitView();
        }

        private void InitView()
        {
            m_pageEmptyTextArray[0] = LanguageUtils.getText(910015);
            m_pageEmptyTextArray[1] = LanguageUtils.getText(910018);
            m_pageEmptyTextArray[2] = LanguageUtils.getText(910019);
            m_pageEmptyTextArray[3] = LanguageUtils.getText(910020);
            m_pageEmptyTextArray[4] = LanguageUtils.getText(910016);

            m_pageBtnArray[0] = view.m_img_all;
            m_pageBtnArray[1] = view.m_img_special;
            m_pageBtnArray[2] = view.m_img_friends;
            m_pageBtnArray[3] = view.m_img_enemy;
            m_pageBtnArray[4] = view.m_img_alliance;

            for (int i = 0; i < m_pageBtnArray.Length; i++)
            {
                int pageIndex = i;
                m_pageBtnArray[pageIndex].m_ck_languageCheckBox_GameToggle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn)
                    {
                        if (pageIndex != m_currentPageIndex)
                        {
                            SwitchPage(pageIndex);
                        }
                    }
                });

                m_pageBtnArray[pageIndex].m_ck_languageCheckBox_GameToggle.isOn = (pageIndex == m_defaultPageIndex);
            }

            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);

            SwitchPage(m_defaultPageIndex);
        }

        private void ItemEnter(ListView.ListItem item)
        {
            MapMarkerInfo mapMarkerInfo = null;
            if (item.index < m_mapMarkerInfoList.Count)
            {
                mapMarkerInfo = m_mapMarkerInfoList[item.index];
            }
            if (mapMarkerInfo == null)
            {
                return;
            }

            UI_Item_BookList_SubView subView;
            if (item.data == null)
            {
                subView = new UI_Item_BookList_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_BookList_SubView;
            }
            subView.Refresh(mapMarkerInfo);
        }

        private void SwitchPage(int pageIndex)
        {
            //离开“全部”和“联盟”分页刷新
            if ((m_currentPageIndex == 0 || m_currentPageIndex == 4) && m_currentPageIndex != pageIndex)
            {
                if (m_allianceProxy.HasJionAlliance())
                {
                    m_mapMarkerProxy.UpdateMapMarker();
                }                    
            }

            m_currentPageIndex = pageIndex;

            RefreshView();
        }

        private void RefreshView()
        {
            if (!m_loadFinishFlag)
            {
                return;
            }

            m_mapMarkerInfoList = m_mapMarkerProxy.GetMapMarkerInfoListByPageIndex(m_currentPageIndex);
            view.m_sv_list_ListView.FillContent(m_mapMarkerInfoList.Count);

            int curNum = 0;
            int maxNum = 0;
            m_mapMarkerProxy.CalMapMarkerProgress(m_currentPageIndex, out curNum, out maxNum);
            view.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(180714, curNum, maxNum);

            if (m_mapMarkerInfoList.Count <= 0)
            {
                view.m_lbl_none_LanguageText.gameObject.SetActive(true);

                string noneTipStr = string.Empty;
                if (m_currentPageIndex < m_pageEmptyTextArray.Length)
                {
                    noneTipStr = m_pageEmptyTextArray[m_currentPageIndex];
                }
                else
                {
                    Debug.LogError("MapMarker PageEmptyTextArray Index Error.");
                }
                view.m_lbl_none_LanguageText.text = noneTipStr;
            }
            else
            {
                view.m_lbl_none_LanguageText.gameObject.SetActive(false);
            }
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_MapMarker);
        }
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 8, 2020
// Update Time         :    Wednesday, April 8, 2020
// Class Description   :    UI_Win_GuildLanguageMediator
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
using UnityEngine.UI;

namespace Game {
    public class UI_Win_GuildLanguageMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildLanguageMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildLanguageMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildLanguageView view;
        
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private AllianceProxy m_allianceProxy;

        private List<AllianceLanguageSetDefine> m_langs;
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
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_langs = m_allianceProxy.AllianceLanguageSetDefines();
        }

        private ToggleGroup m_group;
        private int m_curLanguage;
        
        protected override void BindUIEvent()
        {
            m_preLoadRes.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

         
                ListView.FuncTab funcTab2 = new ListView.FuncTab();
                funcTab2.ItemEnter = FlagBigViewItemByIndex;
                
            
                view.m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab2);
                m_group = view.m_sv_list_view_ListView.gameObject.AddComponent<ToggleGroup>();
                view.m_sv_list_view_ListView.FillContent((m_langs.Count+1)/2);
            });

            m_curLanguage = (int)view.data;
            
            view.m_UI_Model_StandardButton_Blue_sure.m_btn_languageButton_GameButton.onClick.AddListener(onOk);
            
            view.m_UI_Model_Window_TypeMid.setCloseHandle(onClose);
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceLan);
        }

        private void onOk()
        {
            UI_Win_GuildSettingMediator md =
                AppFacade.GetInstance().RetrieveMediator(UI_Win_GuildSettingMediator.NameMediator) as
                    UI_Win_GuildSettingMediator;
            
            md.setLan(m_curLanguage);
            
            CoreUtils.uiManager.CloseUI(UI.s_AllianceLan);
        }

        void FlagBigViewItemByIndex(ListView.ListItem listItem)
        {
            UI_LC_Language_SubView subView;
            if (listItem.isInit == false)
            {
                subView = new UI_LC_Language_SubView(listItem.go.GetComponent<RectTransform>());
                subView.BindGroup(m_group);
                listItem.data = subView;
                listItem.isInit = true;
                subView.AddValueChange(OnValueChange);
            }
            else
            {
                subView = (UI_LC_Language_SubView)listItem.data;
            }
            int left = m_langs[listItem.index * 2].ID;
            int right = -1;
            if (listItem.index * 2 + 1 < m_langs.Count)
            {
                right = m_langs[listItem.index * 2 + 1].ID;
            }
            subView.SetLanguageAllianceId(left, right);
            subView.Selected(m_curLanguage);
        }
        
        private void OnValueChange(int id)
        {
            if (m_curLanguage == id)
                return;
            m_curLanguage = id;
            
            Debug.Log(m_curLanguage+" langid");

        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}
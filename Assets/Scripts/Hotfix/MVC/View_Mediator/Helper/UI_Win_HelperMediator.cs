// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Win_HelperMediator
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
    public class UI_Win_HelperMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_HelperMediator";

        private List<GameSpriteDefine> m_defines;

        private List<TreeViewNode> m_currentTree = new List<TreeViewNode>();

        private List<TreeViewNode> m_allTreeNodes = new List<TreeViewNode>();
        #endregion

        //IMediatorPlug needs
        public UI_Win_HelperMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_HelperView view;

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
            m_defines = CoreUtils.dataService.QueryRecords<GameSpriteDefine>();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_UI_Model_Window_Type3.m_btn_back_GameButton.onClick.AddListener(OnBack);

            view.m_btn_head_GameButton.onClick.AddListener(OnQuestion);
        }

        private void OnQuestion()
        {
            IGGURLBundle.shareInstance().serviceURL((exception, url) =>
            {
                if (exception.isNone())
                {
                    IGGSDKUtils.shareInstance().OpenBrowser(url);
                }
            });
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_helper);
        }

        private void OnBack()
        {
            view.m_UI_Model_Window_Type3.m_btn_back_GameButton.gameObject.SetActive(false);
            view.m_pl_detial.gameObject.SetActive(false);
            view.m_pl_list.gameObject.SetActive(true);
        }

        protected override void BindUIData()
        {
            OnBack();
            ClientUtils.PreLoadRes(view.gameObject,new List<string> {
                "UI_Item_Helper",
                "UI_Item_HelperItem",
            }, InitView);
        }
       
        #endregion

        private void InitView(Dictionary<string,GameObject> assets)
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = OnHelperItemInit;
            functab.GetItemPrefabName = GetItemPrefabName;
            view.m_sv_helperlist_ListView.SetInitData(assets,functab);
            List<int> titles = new List<int>();
            for(int i = 0;i<m_defines.Count;i++)
            {
                if(!titles.Contains(m_defines[i].title))
                {
                    titles.Add(m_defines[i].title);
                    m_allTreeNodes.Add(new TreeViewNode { isParent = true, isFold = true, intData = m_defines[i].title }) ;
                }
                m_allTreeNodes.Add(new TreeViewNode { isParent = false, intData = i });
            }
            OnRefreshView();
        }

        private void OnRefreshView()
        {
            bool isFold = false;
            m_currentTree.Clear();
            for (int i = 0; i < m_allTreeNodes.Count; i++)
            {
                m_allTreeNodes[i].index = i;
                if (m_allTreeNodes[i].isParent)
                {
                    m_currentTree.Add(m_allTreeNodes[i]);
                    isFold = m_allTreeNodes[i].isFold;
                }
                else if (!isFold)
                {
                    m_currentTree.Add(m_allTreeNodes[i]);
                }
            }

            view.m_sv_helperlist_ListView.FillContent(m_currentTree.Count);
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            int index = item.index;
            return m_currentTree[index].isParent ? "UI_Item_Helper" : "UI_Item_HelperItem";
        }

        private void OnHelperItemInit(ListView.ListItem item)
        {
            int index = item.index;
            if(m_currentTree[index].isParent)
            {
                UI_Item_HelperView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_HelperView>(item.go);
                itemView.m_lbl_title_LanguageText.text = LanguageUtils.getText(m_currentTree[index].intData);
                bool isFlod = m_currentTree[index].isFold;
                itemView.m_img_arrowDown_PolygonImage.gameObject.SetActive(!isFlod);
                itemView.m_img_arrowUp_PolygonImage.gameObject.SetActive(isFlod);
                itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
                {
                    m_currentTree[index].isFold = !m_currentTree[index].isFold;
                    OnRefreshView();
                });
            }
            else
            {
                UI_Item_HelperItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_HelperItemView>(item.go);
                itemView.m_lbl_title_LanguageText.text = LanguageUtils.getText(m_defines[m_currentTree[index].intData].subTitle);
                itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_btn_GameButton.onClick.AddListener(()=>
                {
                    ShowDetails(m_currentTree[index].intData);
                });
            }
        }


        private void ShowDetails(int id)
        {
            var define = m_defines[id];

            view.m_pl_detial.gameObject.SetActive(true);
            view.m_pl_list.gameObject.SetActive(false);
            view.m_UI_Model_Window_Type3.m_btn_back_GameButton.gameObject.SetActive(true);

            view.m_lbl_question_LanguageText.text = LanguageUtils.getText(define.subTitle);
            view.m_lbl_detial_LanguageText.text = LanguageUtils.getText(define.text);
        }

    }
}
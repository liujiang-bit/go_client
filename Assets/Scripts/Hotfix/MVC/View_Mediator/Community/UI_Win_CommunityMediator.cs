// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月12日
// Update Time         :    2020年5月12日
// Class Description   :    UI_Win_CommunityMediator
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
    public class UI_Win_CommunityMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_CommunityMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_CommunityMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_CommunityView view;

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
            InitCfgDatas();
            ClientUtils.PreLoadRes(view.gameObject, new List<string> { S_PrefabName }, OnItemPrefabLoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.AddCloseEvent(() =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_community);
            });
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void InitCfgDatas()
        {
            var cfgs = CoreUtils.dataService.QueryRecords<Data.SnsEntranceDefine>();
            foreach(var cfg in cfgs)
            {
                if(cfg.order > 0)
                {
                    m_snsEnteranceList.Add(cfg);
                }
            }
        }

        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            GameObject prefab = null;
            if(!dict.TryGetValue(S_PrefabName, out prefab))
            {
                return;
            }
            foreach(var cfg in m_snsEnteranceList)
            {
                GameObject go = GameObject.Instantiate(prefab);
                InitButton(go, cfg);
            }
        }

        private void InitButton(GameObject go, Data.SnsEntranceDefine cfg)
        {
            go.transform.SetParent(view.m_pl_buttons_GridLayoutGroup.transform);
            go.transform.localScale = Vector3.zero;
            UI_Item_PlayerDataBtn_SubView subView = new UI_Item_PlayerDataBtn_SubView(go.GetComponent<RectTransform>());
            ClientUtils.LoadSprite(subView.m_btn_btn_PolygonImage, cfg.icon);
            subView.m_lbl_Text_LanguageText.text = LanguageUtils.getText(cfg.nameID);
            subView.m_btn_btn_GameButton.onClick.AddListener(()=>
            {
                IGGSDKUtils.shareInstance().OpenBrowser(HotfixUtil.getLanguageLink(cfg.hyperlink));
            });
        }

        private List<Data.SnsEntranceDefine> m_snsEnteranceList = new List<Data.SnsEntranceDefine>();
        private const string S_PrefabName = "UI_Item_PlayerDataBtn";
    }
}
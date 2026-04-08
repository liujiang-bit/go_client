// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Win_GuildPurviewMediator
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
    public class UI_Win_GuildPurviewMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildPurviewMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildPurviewMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildPurviewView view;
        
        private AllianceProxy m_allianceProxy;
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private List<AllianceMemberJurisdictionDefine> m_members;
        
        

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
            m_members = m_allianceProxy.AllianceMemberJurisdictionDefineDefines();

        }

        protected override void BindUIEvent()
        {
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                
            
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_members.Count);
                
              
            });
            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730177));
            view.m_UI_Model_Window_Type3.setCloseHandle(onClose);
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceMainAccess);
        }

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildPurviewView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildPurviewView>(scrollItem.go);

            var data = m_members[scrollItem.index];

            if (itemView!=null)
            {
                itemView.m_img_r1_PolygonImage.gameObject.SetActive(data.R1>0);
                itemView.m_img_r2_PolygonImage.gameObject.SetActive(data.R2>0);
                itemView.m_img_r3_PolygonImage.gameObject.SetActive(data.R3>0);
                itemView.m_img_r4_PolygonImage.gameObject.SetActive(data.R4>0);
                itemView.m_img_r5_PolygonImage.gameObject.SetActive(data.R5>0);

                itemView.m_lbl_titleName_LanguageText.text = LanguageUtils.getText(data.l_typeID);
            }
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 9, 2020
// Update Time         :    Thursday, April 9, 2020
// Class Description   :    UI_Win_GuildAppointMediator
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
    public class UI_Win_GuildAppointMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildAppointMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildAppointMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildAppointView view;
        
        private AllianceProxy m_allianceProxy;
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private List<AllianceOfficiallyDefine> m_offices;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_AppointOfficer.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_AppointOfficer.TagName:
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceOffice);
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


        private int m_selectedLv = 0;
        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_offices = m_allianceProxy.AllianceOfficiallyDefines();
            
            
        }

        private GuildOfficerInfoEntity m_oldOfficer;

        protected override void BindUIEvent()
        {
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                
            
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_offices.Count);
                
              
            });
            
            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(730182));
            
            view.m_UI_Model_Window_Type1.setCloseHandle(onClose);
            
            
            view.m_UI_ok.m_btn_languageButton_GameButton.onClick.AddListener(onOK);
            view.m_UI_ok.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730181);
            
            
            view.m_UI_ok.m_btn_languageButton_GameButton.gameObject.SetActive(m_allianceProxy.GetSelfRoot(GuildRoot.office));
            
            
            GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;
            
            m_oldOfficer = m_allianceProxy.getMemberOfficer(player.rid);

            if (m_oldOfficer!=null)
            {
                m_selectedLv =(int)m_oldOfficer.officerId;
            }

            view.m_UI_ok.m_btn_languageButton_GameButton.interactable = false;

        }

        private void onOK()
        {

            long remTime = m_allianceProxy.getMemberByOfficerIDCDEnd(m_selectedOfficer.ID);
            
            if (remTime<=0)
            {
                GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;
            
                m_allianceProxy.SendAppointOfficial(player.rid,m_selectedLv);
            }
            else
            {
                Alert.CreateAlert(
                    LanguageUtils.getTextFormat(730183, ClientUtils.FormatTime((int)remTime))
                    ).SetLeftButton().SetRightButton().Show();
            }


        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceOffice);
        }

        private UI_Item_GuildAppointView preView;

        private AllianceOfficiallyDefine m_selectedOfficer;

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildAppointView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildAppointView>(scrollItem.go);

            var data = m_offices[scrollItem.index];

            if (itemView!=null)
            {
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(data.l_officiallyID);
                itemView.m_lbl_desc_LanguageText.text = LanguageUtils.getText(data.l_desID);
                itemView.m_lbl_pow1_LanguageText.text =
                    ClientUtils.SafeFormat(LanguageUtils.getText(data.l_addDesID), data.addDesData);
                ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage,data.icon);

                if (m_selectedLv == data.ID)
                {
                    itemView.m_ck_choose_GameToggle.isOn = true;

                    itemView.m_ck_choose_GameToggle.interactable = false;

                    preView = itemView;
                }
                
                itemView.m_ck_choose_GameToggle.gameObject.SetActive(m_allianceProxy.GetSelfRoot(GuildRoot.office));
                
                itemView.m_ck_choose_GameToggle.onValueChanged.RemoveAllListeners();
                itemView.m_ck_choose_GameToggle.onValueChanged.AddListener((bool v) =>
                {
                    
                    
                    if (preView!=null)
                    {
                        preView.m_ck_choose_GameToggle.isOn = false;
                        preView.m_ck_choose_GameToggle.interactable = true;
                    }

                    m_selectedLv = data.ID;
                    

                    preView = itemView;

                    m_selectedOfficer = data;
                    
                    GuildMemberInfoEntity player = view.data as GuildMemberInfoEntity;



                    if (m_oldOfficer ==null && m_selectedLv ==0 || m_oldOfficer!=null && m_oldOfficer.officerId == m_selectedLv )
                    {
                        view.m_UI_ok.m_btn_languageButton_GameButton.interactable = false;
                    }else if(m_oldOfficer!=null && m_oldOfficer.officerId!= m_selectedLv || m_selectedLv>0)
                    {
                        view.m_UI_ok.m_btn_languageButton_GameButton.interactable = true;
                    }
                    
                    
                    if ( m_oldOfficer!=null && m_oldOfficer.officerId == data.ID && itemView.m_ck_choose_GameToggle.isOn )
                    {
                        itemView.m_ck_choose_GameToggle.interactable = false;
                    }


                });
            }
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
    }
}
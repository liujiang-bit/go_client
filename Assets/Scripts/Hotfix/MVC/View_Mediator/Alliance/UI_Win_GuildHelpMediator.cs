// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 15, 2020
// Update Time         :    Wednesday, April 15, 2020
// Class Description   :    UI_Win_GuildHelpMediator
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
    public class UI_Win_GuildHelpMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildHelpMediator";
        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;

        private ResearchProxy m_researchProxy;

        private CityBuildingProxy m_buildProxy;
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();


        private List<GuildRequestHelpInfoEntity> m_helpsList;

        private Timer m_timer;
        private Timer m_timeritems;
        private long rid;
        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildHelpMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildHelpView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_GuildRequestHelps.TagName,
                Guild_HelpGuildMembers.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_GuildRequestHelps.TagName:
                    ReList();
                    break;
                case Guild_HelpGuildMembers.TagName:

                    Guild_HelpGuildMembers.response response = notification.Body as Guild_HelpGuildMembers.response;


                    if (response.result)
                    {
                        m_allianceProxy.HelpsRemoveAll();
                        ReList();
                        //您帮助了所有成员
                        Tip.CreateTip(730002,Tip.TipStyle.AllianceHelp).SetHelpType(Tip.AllianceHelpType.Other).Show();
                        onClose();
                    }
                   
                    
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

            if (m_timer!=null)
            {
                m_timer.Cancel();
                m_timer = null;
                m_timeItemList.Clear();
                
                
                m_timeritems.Cancel();

                m_timeritems = null;
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            
            m_researchProxy = AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;

            m_buildProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;

            //            m_allianceProxy.SendGetRequestHelps();

            rid = m_playerProxy.CurrentRoleInfo.rid;
            m_timer = Timer.Register(1,onTimer,null,true);
            m_timeritems = Timer.Register(3, onTimerItems, null, true,false,view.vb);
            onTimer();
        }

        private bool show = false;
        
        private void onTimer()
        {
         
            view.m_lbl_nextTime_LanguageText.text = LanguageUtils.getTextFormat(730007, ClientUtils.FormatTimeTroop((int)ServerTimeModule.Instance.GetDistanceZeroTime()) );
        }
        private void onTimerItems()
        {
            show = !show;
            if (m_helpsList.Count > 0 && view.m_sv_list_ListView!=null)
            {
                for (int i = 0; i < m_helpsList.Count; i++)
                {
                    var item = view.m_sv_list_ListView.GetItemByIndex(i);
                    if (item != null && item.go != null)
                    {
                        ViewItemByIndex(item);
                    }
                }
            }
        }

     
        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.setWindowTitle(LanguageUtils.getText(730006));
            view.m_UI_Model_Window_Type2.setCloseHandle(onClose);
            
            view.m_btn_info_GameButton.onClick.AddListener(onTipInfo);
            
            view.m_btn_tip_GameButton.onClick.AddListener(onTipCur);
            
            view.m_btn_help.m_btn_languageButton_GameButton.onClick.AddListener(onHelpBtn);
            
            m_helpsList = m_allianceProxy.getHelps();
            
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                
            
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);

                ReList();
              
            });

            
            ReInfo();
        }

        private void onHelpBtn()
        {
            m_allianceProxy.SendHelpGuildMembers();
        }
        
        
        private List<UI_Item_GuildHelpItemView> m_timeItemList = new List<UI_Item_GuildHelpItemView>();


        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildHelpItemView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildHelpItemView>(scrollItem.go);
            var data = m_helpsList[scrollItem.index];
            var member = m_allianceProxy.getMemberInfo(data.rid);
            if (itemView!=null && data!=null && member!=null && view!=null)
            {

                itemView.m_pl_effect_ArabLayoutCompment.gameObject.SetActive(false);
                itemView.m_lbl_name_LanguageText.text = member.name;
                itemView.m_UI_PlayerHead.LoadHeadCountry((int)member.headId);
                // 1 建筑建造升级 2 治疗 3 科技升级 4 战损补偿
                switch (data.type)
                {
                    case 1:

                        var cinfo = m_buildProxy.GetBuildConfig((EnumCityBuildingType)data.args[0]);
                        itemView.m_lbl_desc_LanguageText.text =
                            LanguageUtils.getTextFormat(730010, LanguageUtils.getText(cinfo.l_nameId), data.args[1]);
                        
                        break;
                    case 2:
                        itemView.m_lbl_desc_LanguageText.text =
                            LanguageUtils.getText(730011);
                        break;
                    case 3:

                        var tinfo = m_researchProxy.GetTechnologyByLevel((int)data.args[0], 1);
                        if (tinfo!=null)
                        {
                            itemView.m_lbl_desc_LanguageText.text =
                                LanguageUtils.getTextFormat(730009,LanguageUtils.getText(tinfo.l_nameID),data.args[1]);
                        }
                        
                        break;
                    case 4:
                        itemView.m_pl_effect_ArabLayoutCompment.gameObject.SetActive(true);
                        itemView.m_lbl_desc_LanguageText.text = LanguageUtils.getText(730375);
                        break;
                    
                }

                
                ClientUtils.SetPro((float)data.helpNum / data.helpLimit ,itemView.m_pb_rogressBar_GameSlider);

                if (data.rid == rid)
                {
                    itemView.m_lbl_time_LanguageText.gameObject.SetActive(show);
                    itemView.m_lbl_helpTime_LanguageText.gameObject.SetActive(!show);
                }
                else
                {
                    itemView.m_lbl_time_LanguageText.gameObject.SetActive(false);
                    itemView.m_lbl_helpTime_LanguageText.gameObject.SetActive(true);
                }
                itemView.m_lbl_time_LanguageText.text =
                    LanguageUtils.getTextFormat(730016, ClientUtils.FormatTimeTroop((int)data.reduceTime));
                        
                itemView.m_lbl_helpTime_LanguageText.text = LanguageUtils.getTextFormat(181104, data.helpNum, data.helpLimit);
                

                bool isSelf = data.rid == m_playerProxy.CurrentRoleInfo.rid;
                itemView.m_img_bgNomal_PolygonImage.gameObject.SetActive(!isSelf);
                itemView.m_img_bgmy_PolygonImage.gameObject.SetActive(isSelf);

                if (!m_timeItemList.Contains(itemView))
                {
                    m_timeItemList.Add(itemView);
                }
            }

        }

        
        private void onTipCur()
        {
            HelpTip.CreateTip(4001,view.m_btn_tip_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
        }
        private void onTipInfo()
        {
            HelpTip.CreateTip(4002,view.m_btn_info_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceHelp);
        }

        protected override void BindUIData()
        {

        }

        public void ReInfo()
        {
            ClientUtils.SetPro((float)m_playerProxy.CurrentRoleInfo.guildHelpPoint/m_allianceProxy.Config.individualPointsLimit,view.m_UI_Item_PBinTech.m_pb_rogressBar_GameSlider);
            view.m_UI_Item_PBinTech.m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(730017, m_playerProxy.CurrentRoleInfo.guildHelpPoint, m_allianceProxy.Config.individualPointsLimit);
            
            view.m_btn_help.m_btn_languageButton_GameButton.gameObject.SetActive(m_allianceProxy.canHelpOhter());
        }

        public void ReList()
        {
            if (m_assetDic.Count > 0)
            {
                m_helpsList = m_allianceProxy.getHelps();
                view.m_sv_list_ListView.FillContent(m_helpsList.Count);
                view.m_lbl_nohelp_LanguageText.gameObject.SetActive(m_helpsList.Count==0 );

            }
            ReInfo();
        }

        #endregion
    }
}
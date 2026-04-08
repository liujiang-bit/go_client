// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 8, 2020
// Update Time         :    Wednesday, April 8, 2020
// Class Description   :    UI_Win_GuildJoinMediator
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
    public class UI_Win_GuildJoinMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildJoinMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildJoinMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildJoinView view;

        private PlayerProxy m_playerProxy;


        private List<GuildInfo> queryGuildInfos = new List<GuildInfo>();
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private AllianceProxy m_allianceProxy;

        private int m_selectedIndex = 0; 
//        private 

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_SearchGuild.TagName,
                Guild_ApplyJoinGuild.TagName,
                Guild_CancelGuildApply.TagName,
                Guild_GuildNotify.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case  Guild_SearchGuild.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                    }
                    else
                    {
                        Guild_SearchGuild.response response = notification.Body as Guild_SearchGuild.response;

                        if (response.HasGuildList)
                        {
                            queryGuildInfos = response.guildList;
                            ReList();
                        }
                    }

                    break;
                case Guild_ApplyJoinGuild.TagName:
                {
                    
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
//                        ErrorMessage error = (ErrorMessage)notification.Body;

                        ErrorMessage error = (ErrorMessage)notification.Body;

                        switch ((ErrorCode)error.errorCode)
                        {
                            case ErrorCode.GUILD_NOT_EXIST:
                                Tip.CreateTip(550175).Show();
                                break;
                            case ErrorCode.GUILD_MEMBER_FULL:
                                Tip.CreateTip(730072).SetStyle(Tip.TipStyle.Middle).Show();
                                break;
                        }
                    }
                    else
                    {
                        Guild_ApplyJoinGuild.response response = notification.Body as Guild_ApplyJoinGuild.response;


                        if (response.guildId == m_selectedAlliance.guildId)
                        {
                            if (response.type == 2)
                            {
                                CoreUtils.uiManager.CloseUI(UI.s_AllianceJionList);
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
                            }
                            else
                            {
                                m_selectedAlliance.isApply = true;

                                ReList();
                            }
                        }
                    }

                   
                }
                    break;
                case Guild_GuildNotify.TagName:
                {
                    Guild_GuildNotify.request request = notification.Body as Guild_GuildNotify.request;
                    if (request.notifyOperate==1)
                    {
                        if (request.roleInfos[0].name == m_playerProxy.CurrentRoleInfo.name)
                        {
                            CoreUtils.uiManager.CloseUI(UI.s_AllianceJionList);
                            CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
                        }
                    }
                }
                    
                    
                    break;
                case Guild_CancelGuildApply.TagName:
                {
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;

                        switch ((ErrorCode)error.errorCode)
                        {
                            case ErrorCode.GUILD_NOT_EXIST:
                                Tip.CreateTip(550175).Show();
                                break;
                            case ErrorCode.GUILD_MEMBER_FULL:
                                Tip.CreateTip(730072).SetStyle(Tip.TipStyle.Middle).Show();
                                break;
                        }
                    }
                    else
                    {
                        Guild_CancelGuildApply.response response = notification.Body as Guild_CancelGuildApply.response;

                        if (response.guildId == m_selectedAlliance.guildId)
                        {
                            m_selectedAlliance.isApply = false;

                            ReList();
                        }
                    }
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
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_playerProxy= AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }
        
        private bool hasJion;

        protected override void BindUIEvent()
        {
            
            hasJion = m_allianceProxy.HasJionAlliance();
            
            view.m_UI_Model_Window_Type3.setWindowTitle(hasJion?LanguageUtils.getText(730098): LanguageUtils.getText(730061));
            
            view.m_UI_Model_Window_Type3.setCloseHandle(OnClose);
            
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                
            
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(queryGuildInfos.Count>30?30:queryGuildInfos.Count);
                
              
            });
            
            m_allianceProxy.SearchAlliance(2);

           

            
//            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730061));

            view.m_UI_join.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730026);
            view.m_UI_needJion.m_lbl_line2_LanguageText.text = LanguageUtils.getText(730069);
            //取消//730070
            
            view.m_UI_join.m_btn_languageButton_GameButton.onClick.AddListener(onJoin);
            view.m_UI_needJion.m_btn_languageButton_GameButton.onClick.AddListener(onJoin);
            
            view.m_UI_cancel.m_btn_languageButton_GameButton.onClick.AddListener(onCancel);
            
            view.m_UI_info.m_btn_languageButton_GameButton.onClick.AddListener(onGuildInfo);
            
            view.m_btn_mail_GameButton.onClick.AddListener(OnSendMail);
            
            view.m_btn_search_GameButton.onClick.AddListener(onSearch);

            view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(false);
            
        }

        private void onGuildInfo()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceInfo,null,m_selectedAlliance);
        }

        private void OnSendMail()
        {
            WriteAMailData mailData = new WriteAMailData();

            mailData.stableName = m_selectedAlliance.leaderName;
            mailData.stableRid = m_selectedAlliance.leaderRid;
            CoreUtils.uiManager.ShowUI(UI.s_writeAMail,null,mailData);
        }

        private string preSearchKey;

        private void onSearch()
        {
            if (view.m_ipt_ipt_GameInput.text.Length>=3)
            {
                if (view.m_ipt_ipt_GameInput.text == preSearchKey && queryGuildInfos.Count>0)
                {
                    m_selectedIndex = 0;
                    RmdList();
                    view.m_sv_list_ListView.ForceRefresh();
                }
                else
                {
                    preSearchKey = view.m_ipt_ipt_GameInput.text;
                    m_allianceProxy.SearchAlliance(3,view.m_ipt_ipt_GameInput.text);
                }
               
            }
            else if(view.m_ipt_ipt_GameInput.text.Length>0)
            {
                preSearchKey = view.m_ipt_ipt_GameInput.text;
                queryGuildInfos.Clear();
                ReList();
            }
            else
            {
                if (!string.IsNullOrEmpty(preSearchKey))
                {
                    m_selectedIndex = 0;
                    preSearchKey = "";
                    m_allianceProxy.SearchAlliance(2);
                }
                preSearchKey = "";
            }
        }
        
        private void ReList()
        {
            
            
            if (queryGuildInfos.Count>0)
            {
                view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(true);
                view.m_lbl_tip_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(false);
                view.m_lbl_tip_LanguageText.gameObject.SetActive(true);
            }

            if (m_assetDic.Count>0)
            {
                view.m_sv_list_ListView.FillContent(queryGuildInfos.Count>30?30:queryGuildInfos.Count);
                view.m_sv_list_ListView.ForceRefresh();
            }

           
        }

        private void RmdList()
        {
            int count = queryGuildInfos.Count;
            if (count>0)
            {
                for (int i = 0; i < count; i++)
                {
                    var ia = Random.Range(0, count-1);
                    var ib = Random.Range(0, count-1);
                    if (ia!=ib)
                    {
                        var a = queryGuildInfos[ia];
                        var b = queryGuildInfos[ib];
                        queryGuildInfos[ib] = a;
                        queryGuildInfos[ia] = b;
                    }
                }
                queryGuildInfos.Reverse();
            }

            m_selectedIndex = 0;
        }


        private void onJoin()
        {
            if (m_selectedAlliance!=null )
            {

                if (m_selectedAlliance.memberNum>=m_selectedAlliance.memberLimit)
                {
                    Tip.CreateTip(730072).SetStyle(Tip.TipStyle.Middle).Show();
                }else if (m_selectedAlliance.isSameGame == false)
                {
                    Tip.CreateTip(730073).Show();
                }
                else
                {
                    m_allianceProxy.SendJionAlliance(m_selectedAlliance.guildId);
                }

                
            }
        }
        
        private void onCancel()
        {
            m_allianceProxy.CancelJionAlliance(m_selectedAlliance.guildId);
        }

//        private void onNeedJoin()
//        {
//            if (m_jionAlliance!=null )
//            {
//                //TODO
////                if (m_jionAlliance.memberNum)
//                {
//                    m_allianceProxy.SendJionAlliance(m_jionAlliance.guildId);
//                }
//            }
//        }

        private GuildInfo m_selectedAlliance;

        private UI_Item_GuildJoinView preItem;

        
        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildJoinView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildJoinView>(scrollItem.go);
            var data = queryGuildInfos[scrollItem.index];


            if (itemView!=null)
            {
                int index = scrollItem.index;
                var cdata = CoreUtils.dataService
                    .QueryRecord<AllianceLanguageSetDefine>((int) data.languageId);
                
                if(cdata!=null)
                    itemView.m_lbl_lang_LanguageText.text = LanguageUtils.getText(cdata.l_languageID) ;

                itemView.m_lbl_mem_LanguageText.text = LanguageUtils.getTextFormat(730117, data.memberNum, data.memberLimit);

                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(300030, data.abbreviationName, data.name); 
                itemView.m_lbl_pow_LanguageText.text = ClientUtils.FormatComma(data.power);
                ClientUtils.TextSetColor(itemView.m_lbl_name_LanguageText,Color.white);
                ClientUtils.TextSetColor(itemView.m_lbl_desc_LanguageText,Color.white);
                ClientUtils.TextSetColor(itemView.m_lbl_mem_LanguageText,Color.white);
                ClientUtils.TextSetColor(itemView.m_lbl_pow_LanguageText,Color.white);
                ClientUtils.TextSetColor(itemView.m_lbl_lang_LanguageText,Color.white);

                itemView.m_lbl_desc_LanguageText.text =
                    data.needExamine ? LanguageUtils.getText(730051) : LanguageUtils.getText(730050);
                
                itemView.m_UI_GuildFlag.setData(data);
                
                itemView.m_img_select_PolygonImage.gameObject.SetActive(data.isApply);
                
                itemView.m_img_bgNoSelect_PolygonImage.gameObject.SetActive(true);
                itemView.m_img_bgSelect_PolygonImage.gameObject.SetActive(false);

                if (m_selectedIndex == scrollItem.index)
                {
                    SelectedItem(data, itemView);
                }
                
                itemView.m_img_select_PolygonImage.gameObject.SetActive(data.isApply);
                
                itemView.m_btn_enter_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_enter_GameButton.onClick.AddListener(() =>
                {

                    if (index != m_selectedIndex)
                    {
                        SelectedItem(data, itemView);

                        m_selectedIndex = scrollItem.index;

                        
                    }
                   
                });
                
                itemView.m_UI_GuildFlag.m_btn_flag_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_GuildFlag.m_btn_flag_GameButton.onClick.AddListener(() =>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_AllianceInfo,null,data);
                });
            }

        }

        private void SelectedItem(GuildInfo data,UI_Item_GuildJoinView itemView)
        {
            if (preItem!=null)
            {
                preItem.m_img_bgNoSelect_PolygonImage.gameObject.SetActive(true);
                preItem.m_img_bgSelect_PolygonImage.gameObject.SetActive(false);
                ClientUtils.TextSetColor(preItem.m_lbl_name_LanguageText,Color.white);
                ClientUtils.TextSetColor(preItem.m_lbl_desc_LanguageText,Color.white);
                ClientUtils.TextSetColor(preItem.m_lbl_mem_LanguageText,Color.white);
                ClientUtils.TextSetColor(preItem.m_lbl_pow_LanguageText,Color.white);
                ClientUtils.TextSetColor(preItem.m_lbl_lang_LanguageText,Color.white);
            }
                        
            view.m_lbl_master_LanguageText.text = data.leaderName;
            view.m_lbl_guildDesc_LanguageText.text = data.notice;
            

            preItem = itemView;

            if (m_allianceProxy.HasJionAlliance())
            {
                view.m_UI_needJion.gameObject.SetActive(false);
                view.m_UI_join.gameObject.SetActive(false);
                view.m_UI_cancel.gameObject.SetActive(false);
                view.m_UI_info.gameObject.SetActive(true);
            }
            else
            {
                view.m_UI_needJion.gameObject.SetActive(data.needExamine);
                view.m_UI_join.gameObject.SetActive(!data.needExamine);



                if (hasJion)
                {
                    view.m_UI_cancel.gameObject.SetActive(false);
                
                    view.m_UI_needJion.gameObject.SetActive(false);
                    view.m_UI_join.gameObject.SetActive(false);
                    view.m_UI_info.gameObject.SetActive(true);
                }
                else
                {
                    view.m_UI_info.gameObject.SetActive(false);
                    if (data.isApply)
                    {
                        view.m_UI_cancel.gameObject.SetActive(true);
                
                        view.m_UI_needJion.gameObject.SetActive(false);
                        view.m_UI_join.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.m_UI_cancel.gameObject.SetActive(false);
                    }
                }

               
            }

          

            view.m_UI_PlayerHead.LoadHeadCountry((int)data.leaderHeadId);
            
           
            
            itemView.m_img_bgNoSelect_PolygonImage.gameObject.SetActive(false);
            itemView.m_img_bgSelect_PolygonImage.gameObject.SetActive(true);
            ClientUtils.TextSetColor(itemView.m_lbl_name_LanguageText,Color.black);
            ClientUtils.TextSetColor(itemView.m_lbl_desc_LanguageText,Color.black);
            ClientUtils.TextSetColor(itemView.m_lbl_mem_LanguageText,Color.black);
            ClientUtils.TextSetColor(itemView.m_lbl_pow_LanguageText,Color.black);
            ClientUtils.TextSetColor(itemView.m_lbl_lang_LanguageText,Color.black);
            
            m_selectedAlliance = data;
        }




        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceJionList);
        }

       

        protected override void BindUIData()
        {
            
        }
       
        #endregion
    }
}
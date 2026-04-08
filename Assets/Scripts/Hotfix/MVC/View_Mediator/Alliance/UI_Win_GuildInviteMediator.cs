// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 8, 2020
// Update Time         :    Wednesday, April 8, 2020
// Class Description   :    UI_Win_GuildInviteMediator
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
    public class UI_Win_GuildInviteMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildInviteMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildInviteMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildInviteView view;

        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;

        private RoleInfo m_selectedPlayer;

        private List<RoleInfo> m_serverRole = new List<RoleInfo>();

        private CityBuildingProxy m_cityProxy;

        private int m_selectedIndex = 0;

        private static int MAX_SHOW_COUNT = 20;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_QueryRoleByParam.TagName,
                Guild_InviteGuild.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_QueryRoleByParam.TagName:
                {
                    Role_QueryRoleByParam.response response = notification.Body as Role_QueryRoleByParam.response;

                    m_serverRole = response.roles;

                    ReList();
                }
                    break;
                case  Guild_InviteGuild.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        

                        switch ((ErrorCode)error.errorCode)
                        {
                            case ErrorCode.GUILD_ALREADY_IN_OTHER_GUILD:
                                Tip.CreateTip(730363).Show();
                                break;
                            case ErrorCode.GUILD_INVITE_LIMIT:
                                Tip.CreateTip(730360).Show();
                                break;
                            case ErrorCode.GUILD_ALREADY_DISBAND:
                                Tip.CreateTip(730362).Show();
                                break;
                            case ErrorCode.GUILD_ROLE_ALREADY_INVITED:
                                Tip.CreateTip(730369).Show();
                                m_selectedPlayer.guildInvite = true;
                                view.m_sv_list_ListView.ForceRefresh();
                                break;
                        }
                    }
                    else
                    {
                        Guild_InviteGuild.response response = notification.Body as Guild_InviteGuild.response;

                        if (response.result)
                        {
                            Tip.CreateTip(730109).Show();
                            view.m_sv_list_ListView.ForceRefresh();
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
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
        }

        protected override void BindUIEvent()
        {
            
        }
        
     
        protected override void BindUIData()
        {
            view.m_UI_Model_Window_Type3.setWindowTitle( LanguageUtils.getText(730103));
            
            view.m_UI_Model_Window_Type3.setCloseHandle(OnClose);
            
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = FlagViewItemByIndex;
                
            
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_serverRole.Count>MAX_SHOW_COUNT?MAX_SHOW_COUNT:m_serverRole.Count);
                
              
            });
            
            m_allianceProxy.SendSearchPlayers();

            view.m_btn_search_GameButton.onClick.AddListener(onSearch);
            
            view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(false);

            view.m_UI_Blue.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730101);
            
            view.m_UI_Blue.m_btn_languageButton_GameButton.onClick.AddListener(onInvite);
            
             if (m_playerProxy.CurrentRoleInfo.guildId>0)
             {
                 view.m_txt_Placeholder_LanguageText.text = LanguageUtils.getText(730104);
             }
        }

        private void onInvite()
        {

            if (m_selectedPlayer.guildId>0)
            {
                CoreUtils.uiManager.ShowUI(UI.s_OtherPlayerInfo,null,m_selectedPlayer);
            }
            else
            {
                if (m_allianceProxy.GetSelfRoot(GuildRoot.invicePlayer))
                {
                    m_allianceProxy.SendInvitePlayer(m_selectedPlayer.rid);

                    m_selectedPlayer.guildInvite = true;

                }
                else
                {
                    Tip.CreateTip(730136).Show();
                }

            }

            


        }

        private string preSearchKey = "";

        private void onSearch()
        {
            string key = view.m_ipt_ipt_GameInput.text.Trim();
            if (key.Length>=1 && preSearchKey != key || string.IsNullOrEmpty(key))
            {
                m_allianceProxy.SendSearchPlayers(key);
                preSearchKey = key;
            }
        }

        private UI_Item_GuildInviteView preItem;
        void FlagViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildInviteView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildInviteView>(scrollItem.go);
            var data = m_serverRole[scrollItem.index];


            if (itemView!=null)
            {
                ClientUtils.TextSetColor(itemView.m_lbl_name_LanguageText,Color.white);
                ClientUtils.TextSetColor(itemView.m_lbl_mem_LanguageText,Color.white);
                ClientUtils.TextSetColor(itemView.m_lbl_pow_LanguageText,Color.white);
                
                itemView.m_lbl_name_LanguageText.text = AllianceProxy.FormatGuildName(data.name,data.guildAbbName);;
                itemView.m_lbl_pow_LanguageText.text = ClientUtils.FormatComma(data.combatPower);
                itemView.m_lbl_mem_LanguageText.text = ClientUtils.FormatComma(m_playerProxy.KillCountOther(data.killCount));
                
                itemView.m_img_select_PolygonImage.gameObject.SetActive(data.guildInvite);

                itemView.m_img_bgNoSelect_PolygonImage.gameObject.SetActive(true);
                itemView.m_img_bgSelect_PolygonImage.gameObject.SetActive(false);
                
                itemView.m_UI_PlayerHead.LoadHeadCountry((int)data.headId);
                
                if (m_selectedIndex == scrollItem.index)
                {
                    SelectedItem(data, itemView);
                }
                
                itemView.m_btn_enter_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_enter_GameButton.onClick.AddListener(() =>
                    {
                        m_selectedIndex = scrollItem.index;
                        SelectedItem(data, itemView);
                    });
            }

        }
        
        
        private void SelectedItem(RoleInfo data,UI_Item_GuildInviteView itemView)
        {
            if (preItem!=null)
            {
                preItem.m_img_bgNoSelect_PolygonImage.gameObject.SetActive(true);
                preItem.m_img_bgSelect_PolygonImage.gameObject.SetActive(false);
                ClientUtils.TextSetColor(preItem.m_lbl_name_LanguageText,Color.white);
                ClientUtils.TextSetColor(preItem.m_lbl_mem_LanguageText,Color.white);
                ClientUtils.TextSetColor(preItem.m_lbl_pow_LanguageText,Color.white);
            }
            
            
            itemView.m_img_bgNoSelect_PolygonImage.gameObject.SetActive(false);
            itemView.m_img_bgSelect_PolygonImage.gameObject.SetActive(true);
            ClientUtils.TextSetColor(itemView.m_lbl_name_LanguageText,Color.black);
            ClientUtils.TextSetColor(itemView.m_lbl_mem_LanguageText,Color.black);
            ClientUtils.TextSetColor(itemView.m_lbl_pow_LanguageText,Color.black);

            view.m_lbl_master_LanguageText.text = data.name;
            view.m_UI_PlayerHead.LoadHeadCountry((int)data.headId);

            view.m_lbl_cityLevel_LanguageText.text = data.level.ToString();
            
            view.m_UI_Blue.gameObject.SetActive(data.guildId==0);
                
            view.m_UI_Blue.m_btn_languageButton_GameButton.interactable = !data.guildInvite;
            if (data.guildInvite)
            {
                view.m_UI_Blue.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730110);
            }
            else
            {
                view.m_UI_Blue.m_lbl_Text_LanguageText.text = LanguageUtils.getText(730101);
            }

            

            EnumAgeType ageType = m_cityProxy.GetAgeType(data.level);

            string cityModel = m_cityProxy.GetModelIdByType((long) EnumCityBuildingType.Castel, data.country, ageType);

           

            var civilizationDefine = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)data.country);
            
            ClientUtils.LoadSprite(view.m_img_civi_PolygonImage,civilizationDefine.civilizationMark);

            CoreUtils.assetService.Instantiate("UI_"+cityModel, (obj) =>
            {
                while(view.m_pl_build.childCount>0)
                {
                    var child =view.m_pl_build.GetChild(0).transform; 
                    child.parent = null;
                    CoreUtils.assetService.Destroy(child.gameObject);
                }
                obj.transform.parent = view.m_pl_build;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
            });

            m_selectedPlayer = data;
            
            preItem = itemView;
        }
        
        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceInvite);
        }
        
        private void ReList()
        {

            m_selectedIndex = 0;
            
            if (m_serverRole.Count>0)
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
                view.m_sv_list_ListView.FillContent(m_serverRole.Count>MAX_SHOW_COUNT?MAX_SHOW_COUNT:m_serverRole.Count);
                view.m_sv_list_ListView.ForceRefresh();
            }
           
        }
       
        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 8, 2020
// Update Time         :    Wednesday, April 8, 2020
// Class Description   :    UI_Win_GuildApplicationMediator
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
    public class UI_Win_GuildApplicationMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildApplicationMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildApplicationMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildApplicationView view;
        
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;
        private List<GuildApplyInfoEntity> requset;


        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_ExamineGuildApply.TagName,
                CmdConstant.AllianceApplys
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_ExamineGuildApply.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;

                        if (error.errorCode == 12015)
                        {
                            Tip.CreateTip(730023).Show();
                        }
                    }
                    break;
                case CmdConstant.AllianceApplys:
                    ReApplyList();
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


//            m_allianceProxy.SendJoinApplys();
        }
        

        private void ReApplyList()
        {

            if (m_assetDic.Count>0)
            {
                requset = m_allianceProxy.getGuildApplys();
            
                view.m_sv_list_ListView.FillContent(requset.Count);
            
                view.m_sv_list_ListView.ForceRefresh();
            }
          
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730098));
            
            view.m_UI_Model_Window_Type3.setCloseHandle(onClose);
            
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = FlagViewItemByIndex;
                
            
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                ReApplyList();
                
              
            });
        }

        void FlagViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildApplicationView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildApplicationView>(scrollItem.go);

            var data = requset[scrollItem.index];
            
            itemView.m_btn_yes_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_yes_GameButton.interactable = true;
            
            itemView.m_btn_yes_GameButton.onClick.AddListener(() =>
            {
                
                if (m_allianceProxy.GetSelfRoot(GuildRoot.checkInvice))
                {
                    var alliance = m_allianceProxy.GetAlliance();
                
//                    if (m_allianceProxy.GetMemberCount()>=alliance.memberLimit)
//                    {
//                        Tip.CreateTip(730072).Show();
//                    }
//                    else
//                    {
                        m_allianceProxy.SendApplyPlayer(data,true);
                        itemView.m_btn_yes_GameButton.interactable = false;
                        
                        ReApplyList();
//                    }
                }
                else
                {
                    Tip.CreateTip(730136).Show();
                }
            });
            
            itemView.m_btn_no_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_no_GameButton.interactable = true;
            itemView.m_btn_no_GameButton.onClick.AddListener(() =>
            {
                
                if (m_allianceProxy.GetSelfRoot(GuildRoot.checkInvice))
                {
                    m_allianceProxy.SendApplyPlayer(data,false);
                    itemView.m_btn_no_GameButton.interactable = false;
                    
                    ReApplyList();

                    Tip.CreateTip(730092, data.name).Show();
                }
                else
                {
                    Tip.CreateTip(730136).Show();
                }
            });

            itemView.m_lbl_name_LanguageText.text = data.name;

            itemView.m_lbl_kill_LanguageText.text = ClientUtils.FormatComma(data.killCount);
            
            itemView.m_lbl_power_LanguageText.text = ClientUtils.FormatComma(data.combatPower);

            itemView.m_UI_PlayerHead.LoadHeadCountry((int)data.headId);
            
            
        }

        



        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceJionReqList);
        }

        #endregion
    }
}
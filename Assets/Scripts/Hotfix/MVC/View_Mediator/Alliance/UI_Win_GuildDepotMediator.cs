// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 15, 2020
// Update Time         :    Wednesday, April 15, 2020
// Class Description   :    UI_Win_GuildDepotMediator
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
    public class UI_Win_GuildDepotMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildDepotMediator";


        private AllianceProxy m_allianceProxy;
        private AllianceResarchProxy m_researchProxy;
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        
        private UI_Item_GuildDepotRes_SubView[] m_subViews = new UI_Item_GuildDepotRes_SubView[5];
        private long[] CurrencyIDs = new[] {107L, 108, 109, 110, 111};
        private Dictionary<long,GuildCurrencyInfoEntity> m_depotCurrencyInfo;

        private List<GuildConsumeRecordInfo> m_historyList;

        private Timer m_timeTick;
        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildDepotMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildDepotView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.AllianceDepot
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AllianceDepot:
                    ReHistory();
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
            m_subViews[0] = view.m_UI_Item_GuildDepotRes1;
            m_subViews[1] = view.m_UI_Item_GuildDepotRes2;
            m_subViews[2] = view.m_UI_Item_GuildDepotRes3;
            m_subViews[3] = view.m_UI_Item_GuildDepotRes4;
            m_subViews[4] = view.m_UI_Item_GuildDepotRes5;


            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_researchProxy = AppFacade.GetInstance().RetrieveProxy(AllianceResarchProxy.ProxyNAME) as AllianceResarchProxy;

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.setWindowTitle(LanguageUtils.getText(180014));
            view.m_UI_Model_Window_Type2.setCloseHandle(onClose);
            
            view.m_btn_info_GameButton.onClick.AddListener(onTipInfo);
        }
        
        

        private void onTipInfo()
        {
            HelpTip.CreateTip(4006,view.m_btn_info_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
        }

        private void onClose()
        {

            if (m_timeTick!=null)
            {
                m_timeTick.Cancel();
                m_timeTick = null;
            }
            
            CoreUtils.uiManager.CloseUI(UI.s_AllianceDepot);
        }

        protected override void BindUIData()
        {
            m_depotCurrencyInfo = m_allianceProxy.GetDepotCurrencyInfoEntities();

            
            m_allianceProxy.SendReqDepotList();
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                
                m_historyList = m_allianceProxy.GetDepotHistory();
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_historyList.Count);
                
              
            });
            
            if (m_timeTick == null)
            {
                m_timeTick = Timer.Register(1, UpdateRess, null, true);
            }
            
            UpdateRess();
        }

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildDepotItemView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildDepotItemView>(scrollItem.go);
            var data = m_historyList[scrollItem.index];

            if (itemView!=null && data!=null)
            {
                itemView.m_lbl_name_LanguageText.text = data.roleName;
                itemView.m_UI_PlayerHead.LoadPlayerIcon(data.roleHeadId,data.roleHeadFrameID);
                itemView.m_lbl_time_LanguageText.text = ServerTimeModule.Instance.ConverToServerDateTime(data.consumeTime).ToString("yyyy-MM-dd");
                itemView.m_lbl_num_LanguageText.text = (scrollItem.index+1).ToString();

                switch (data.consumeType)
                {//1 修建建筑 2 研究科技
                    case 1:

                        if (data.consumeArgs!=null)
                        {
                            var cbuildType =
                                CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)data.consumeArgs[0]);

                            if (cbuildType!=null)
                            {
                                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(732086, data.roleName,LanguageUtils.getText(cbuildType.l_nameId));
                            }
                        }
                       
                        break;
                    case 2:
                        var tinfo = m_researchProxy.GetTechnologyByLevel((int)data.consumeArgs[0], 1);

                        if (tinfo!=null)
                        {
                            itemView.m_lbl_name_LanguageText.text = LanguageUtils.getTextFormat(732087, data.roleName,LanguageUtils.getText(tinfo.l_nameID),data.consumeArgs[1]);

                        }
                        break;
                }
                
                
                UI_Model_ResourcesConsume_SubView[] subViews = new UI_Model_ResourcesConsume_SubView[5];

                subViews[0] = itemView.m_UI_leaguePoints;
                subViews[1] = itemView.m_UI_Food;
                subViews[2] = itemView.m_UI_Wood;
                subViews[3] = itemView.m_UI_Stone;
                subViews[4] = itemView.m_UI_Gold;

                for (int i = 0; i < 5; i++)
                {
                    var subView = subViews[i];
                    ConsumeCurrencyInfo cinfo ;
                    data.consumeCurrencies.TryGetValue(CurrencyIDs[i], out cinfo);
                    
                    subView.gameObject.SetActive(cinfo !=null);
                    
                    if (cinfo !=null)
                    {
                        subView.m_lbl_languageText_LanguageText.text = ClientUtils.FormatComma(cinfo.num);
                    }
                }

            }

        }

        public void UpdateRess()
        {
            if (m_depotCurrencyInfo!=null && m_depotCurrencyInfo.Count > 0 && view.gameObject!=null)
            {
                for (int i = 0; i < m_subViews.Length; i++)
                {
                    UI_Item_GuildDepotRes_SubView subView = m_subViews[i];
                
                    var info = m_depotCurrencyInfo[CurrencyIDs[i]];
                    if (i ==0)
                    {
                        subView.m_lbl_timeNum_LanguageText.text = "" ;
                        subView.m_lbl_num_LanguageText.text =ClientUtils.CurrencyFormat(m_allianceProxy.GetCurrenc(info));
                    }
                    else
                    {
                        subView.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(181104, ClientUtils.CurrencyFormat(m_allianceProxy.GetCurrenc(info)),
                            ClientUtils.CurrencyFormat(info.limit));
                        subView.m_lbl_timeNum_LanguageText.text = LanguageUtils.getTextFormat(732083,ClientUtils.FormatComma(info.produce)) ;
                    }
                }
            }
        }




        public void ReHistory()
        {
            if (m_assetDic.Count>0)
            {
                m_historyList = m_allianceProxy.GetDepotHistory();
                Debug.Log("仓库历史记录"+m_historyList);
                view.m_sv_list_ListView.FillContent(m_historyList.Count);
            }
          
        }

        #endregion
    }
}
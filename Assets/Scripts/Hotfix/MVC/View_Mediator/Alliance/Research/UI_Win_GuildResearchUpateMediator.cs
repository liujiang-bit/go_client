// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, May 13, 2020
// Update Time         :    Wednesday, May 13, 2020
// Class Description   :    UI_Win_GuildResearchUpateMediator
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
    public class UI_Win_GuildResearchUpateMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildResearchUpateMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildResearchUpateMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildResearchUpateView view;
        
        private AllianceResarchProxy m_resProxy;
         private CurrencyProxy m_crrProxy;
        
        private List<AllianceStudyDefine> m_crrStudys = null;

        private AllianceStudyDefine m_nextStudy;
        private PlayerProxy m_playerProxy;

        private Timer m_timer;

        private Timer m_resTimer;
        //前置条件
        private List<AllianceStudyDefine> preStudyIDs = null;

        private AllianceProxy m_allianceProxy;


        private long m_serverDonatePoint = -1;
        private long m_critNum = 0;
        private bool m_initedByServer = false;

        private List<GameObject> m_critObjs = new List<GameObject>();

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_GetTechnologyDonate.TagName,
                Guild_DonateTechnology.TagName,
                CmdConstant.AllianceTechUpdate
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_GetTechnologyDonate.TagName:
                {
                    Guild_GetTechnologyDonate.response response =
                        notification.Body as Guild_GetTechnologyDonate.response;

                    if (response.technologyType == GetStudyDefine().studyType)
                    {
                        if (m_serverDonatePoint!=response.exp)
                        {
                            m_serverDonatePoint = response.exp;
                            m_initedByServer = true;
                            BindUIData();
                        }
                       
                    }
                }
                    
                    break;
                case CmdConstant.AllianceTechUpdate:
                    m_initedByServer = true;
                    BindUIData();
                    break;

                case Guild_DonateTechnology.TagName:
                {
                    Guild_DonateTechnology.response response =
                        notification.Body as Guild_DonateTechnology.response;

                    
                    if (response.technologyType == GetStudyDefine().studyType)
                    {
                        m_serverDonatePoint = response.exp;

                        m_critNum = response.critNum;

                        CoreUtils.assetService.Instantiate("UI_Common_Crit", (obj) =>
                        {
                            
                            obj.transform.SetParent(view.m_pl_effect_pos);
                            obj.transform.localPosition = Vector3.zero;
                            obj.transform.localScale = Vector3.one;
                            UI_Common_Crit_SubView critSubView = new UI_Common_Crit_SubView(obj.transform as RectTransform);
                            
                            critSubView.m_lbl_nub_LanguageText.text =
                            LanguageUtils.getTextFormat(733221, m_critNum);
                            
                            critSubView.m_pl_cri_Animator.gameObject.SetActive(m_critNum>1);

                           
                            critSubView.m_pl_effect.gameObject.SetActive(m_critNum>1);
                            
                            
                            ClientUtils.LoadSprite(critSubView.m_img_num_PolygonImage,string.Format("ui_num[img_num_{0}]",m_critNum));

                            critSubView.m_lbl_text_LanguageText.text = LanguageUtils.getText(733220);

                            critSubView.m_lbl_value_LanguageText.text =
                            (int.Parse(view.m_img_curCredit.m_lbl_languageText_LanguageText.text)*m_critNum).ToString();
                            
                            m_critObjs.Add(obj);
                            
                            Timer.Register(1.3f,()=>
                            {


                                var temp = m_critObjs[0];
                                m_critObjs.RemoveAt(0);
                                if (temp!=null)
                                {
                                    CoreUtils.assetService.Destroy(temp);
                                }
                            });

                        });
                        

                        m_initedByServer = true;
                        BindUIData();
                    }
                }
                  
                    break;
                case Guild_GuildTechnologies.TagName:
                    m_initedByServer = true;
                    BindUIData();
                    break;
                default:
                    break;
            }
        }

        public AllianceStudyDefine GetStudyDefine()
        {
            return this.view.data as AllianceStudyDefine;
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
            }

            if (m_resTimer!=null)
            {
                m_resTimer.Cancel();
                m_resTimer = null;
            }

            if (m_critObjs.Count>0)
            {
                m_critObjs.ForEach((obj) =>
                {
                    
                    CoreUtils.assetService.Destroy(obj);
                });
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            view.IsAllowClickMaskClose = false;
            m_resProxy = AppFacade.GetInstance().RetrieveProxy(AllianceResarchProxy.ProxyNAME) as AllianceResarchProxy;
            
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            
            m_crrProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;

            view.gameObject.SetActive(false);
            m_resProxy.SendGetDonate(GetStudyDefine().studyType);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_win.setCloseHandle(CloseRes);
            
            view.m_btn_research.m_btn_languageButton_GameButton.onClick.AddListener(OnResearch);
            
            view.m_btn_donate_gem.m_btn_languageButton_GameButton.onClick.AddListener(onDonateGem);
            view.m_btn_donate_res.m_btn_languageButton_GameButton.onClick.AddListener(onDonateRes);
            
            
            //代币提示
            view.m_img_curCredit.m_btn_btn_GameButton.onClick.AddListener(onHelpTipCredit);
            view.m_img_curGuild.m_btn_btn_GameButton.onClick.AddListener(onHelpTipGuild);
            view.m_btn_icon_GameButton.onClick.AddListener(onHelpTipCredit2);
            
            view.m_btn_more_GameButton.onClick.AddListener(onMore);
        }
        
       
        private void onHelpTipCredit()
        {
            HelpTip.CreateTip(4022, view.m_img_curCredit.m_root_RectTransform).Show();
        }
        
        private void onHelpTipCredit2()
        {
            HelpTip.CreateTip(4022, view.m_btn_icon_GameButton.transform).Show();
        }
        
        private void onHelpTipCreditHelp()
        {
            HelpTip.CreateTip(4022, view.m_btn_help_GameButton.transform).Show();
        }
        
        private void onHelpTipGuild()
        {
            HelpTip.CreateTip(4020, view.m_img_curGuild.m_root_RectTransform).Show();
        }

        
        private void CloseRes()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceResearchUpdate);
        }
        private bool isCostEnough = false;
        
        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();




        private void OnResTime()
        {
            int remainTime = (int) (m_resProxy.GetTechEndTime() + m_nextStudy.costTime - ServerTimeModule.Instance.GetServerTime());
            int totalTime = GetStudyDefine().costTime;

            if (remainTime<=0)
            {
                remainTime = 0;
                m_serverDonatePoint = 0;
                
                m_resTimer.Cancel();
                m_resTimer = null;
            }
            
            view.m_lbl_spTimeLeft_LanguageText.text = LanguageUtils.getTextFormat(733208,ClientUtils.FormatTimeSplit(remainTime)) ;

            view.m_pb_spBar_GameSlider.value =1- (float)remainTime / totalTime;
            
        }


        protected override void BindUIData()
        {


            if (m_initedByServer==false)
            {
                return;
            }
            view.IsAllowClickMaskClose = true;
            view.gameObject.SetActive(true);
            m_initedByServer = false;
            
            var study = GetStudyDefine();

            var spImg =  study.icon;
            var spLanID =  study.l_nameID;
            var spDes = study.l_des;

            view.m_lbl_des_LanguageText.text = LanguageUtils.getText(spDes);
            ClientUtils.LoadSprite(view.m_img_icon_PolygonImage, spImg);

            int maxLv = m_resProxy.GetTechnologyMaxLv(study.studyType);
            int crrLv = m_resProxy.GetCrrTechnologyLv(study.studyType);

            bool isMax = crrLv == maxLv;

            m_crrStudys = m_resProxy.GetTechnologyList(GetStudyDefine().studyType);
            m_nextStudy = m_crrStudys[crrLv < m_crrStudys.Count - 1 ? crrLv : m_crrStudys.Count - 1];

            AllianceStudyDefine crrStudy = null;
            if (crrLv > 0)
            {
                crrStudy = m_crrStudys[crrLv - 1];
            }
            else
            {
                crrStudy = new AllianceStudyDefine();
                crrStudy.desData1 = 0;
                crrStudy.l_buffDesID1 = m_nextStudy.l_buffDesID1;
                crrStudy.l_des = m_crrStudys[0].l_des;
                crrStudy.l_buffDesID2 = m_crrStudys[0].l_buffDesID2;
            }


            
            view.m_lbl_leveLow_LanguageText.text = string.Format(LanguageUtils.getText(180306), crrLv);
            
            view.m_lbl_leveHight_LanguageText.text = (crrLv + 1).ToString();
            
            view.m_pl_upPlane_ArabLayoutCompment.gameObject.SetActive(maxLv > crrLv);

            view.m_pb_bar_GameSlider.value = (float) crrLv / maxLv;

            view.m_lbl_barText_LanguageText.text = string.Format("{0}/{1}", crrLv, maxLv);

            view.m_UI_win.setWindowTitle(LanguageUtils.getText(spLanID));

           
            
            view.m_UI_Model_AttrList.ClearAllData();
            
            //属性加成
            if (crrStudy.l_buffDesID1>0)
            {
                if (isMax)
                {
                    view.m_UI_Model_AttrList.AddGuildAttr(crrStudy.l_buffDesID1, LanguageUtils.getTextFormat(crrStudy.l_fullLevelDes1,crrStudy.desData1));
                }
                else
                {
                    view.m_UI_Model_AttrList.AddGuildAttr(crrStudy.l_buffDesID1, LanguageUtils.getTextFormat(m_nextStudy.l_sketchID1,crrStudy.desData1,m_nextStudy.desData1-crrStudy.desData1));
                }
            }
            
            if (crrStudy.l_buffDesID2>0)
            {
                if (isMax)
                {
                    view.m_UI_Model_AttrList.AddGuildAttr(crrStudy.l_buffDesID2, LanguageUtils.getTextFormat(crrStudy.l_fullLevelDes1,crrStudy.desData2));
                }
                else
                {
                    view.m_UI_Model_AttrList.AddGuildAttr(crrStudy.l_buffDesID2, LanguageUtils.getTextFormat(m_nextStudy.l_sketchID2,crrStudy.desData2,m_nextStudy.desData2-crrStudy.desData2));
                }
            }
            
           

            
            view.m_btn_help_GameButton.onClick.RemoveAllListeners();
            view.m_btn_help_GameButton.onClick.AddListener(onHelpTipCreditHelp);

            
            
            view.m_UI_Model_AttrList.RefreshUI();


            preStudyIDs = m_resProxy.CheckPreAllTechnology(m_nextStudy);

            long resCrrTechnologying = m_resProxy.GetCrrTechnologying();
            
            view.m_img_up_PolygonImage.gameObject.SetActive(crrLv < maxLv);
            view.m_lbl_leveHight_LanguageText.gameObject.SetActive(crrLv < maxLv);
            
            //标记
            view.m_btn_mark_GameButton.onClick.RemoveAllListeners();
            view.m_btn_mark_GameButton.onClick.AddListener(onMark);
            view.m_btn_mark_GameButton.gameObject.SetActive(crrLv>=0||study.columns == 1);


            Debug.Log(resCrrTechnologying+"当前研究");
            
            if (m_nextStudy.studyType == m_resProxy.GetMarkType())
            {
                view.m_img_mark_MakeChildrenGray.Normal();
            }
            else
            {
                view.m_img_mark_MakeChildrenGray.Gray();
            }

            if (crrLv == maxLv)
            {
                //最大等级
                view.m_lbl_unlock_LanguageText.text = LanguageUtils.getText(402020);
                view.m_pl_unlock.gameObject.SetActive(true);
                view.m_pl_condition.gameObject.SetActive(false);
                view.m_pl_donate.gameObject.SetActive(false);
                view.m_pl_conduct_PolygonImage.gameObject.SetActive(false);
                view.m_pl_needPreResearch_PolygonImage.gameObject.SetActive(false);
            }
            else if (preStudyIDs.Count > 0 && crrLv == 0 &&
                     m_resProxy.GetCrrTechnologyLv(preStudyIDs[0].studyType) == 0)
            {
                //解锁前置等级
                view.m_pl_unlock.gameObject.SetActive(true);
                view.m_lbl_unlock_LanguageText.text = LanguageUtils.getText(402019);
                view.m_pl_condition.gameObject.SetActive(false);//资源消耗面板
                view.m_pl_conduct_PolygonImage.gameObject.SetActive(false);
                
                view.m_pl_donate.gameObject.SetActive(false);
                
                view.m_btn_mark_GameButton.gameObject.SetActive(false);
            }
            else if (resCrrTechnologying>0 && m_resProxy.GetCrrTechnologying() == m_nextStudy.studyType)
            {
                //研究中
                view.m_pl_conduct_PolygonImage.gameObject.SetActive(true);
                view.m_pl_unlock.gameObject.SetActive(false);
                view.m_pl_condition.gameObject.SetActive(false);
                view.m_pl_donate.gameObject.SetActive(false);

                ClientUtils.LoadSprite(view.m_img_spIcon_PolygonImage, spImg);

                if (m_resTimer == null)
                {
                    m_resTimer = Timer.Register(1, OnResTime, null, true, true);
                    OnResTime();
                }
            }
            else
            {
                view.m_pl_unlock.gameObject.SetActive(false);


                bool hasDonate = m_serverDonatePoint>= m_nextStudy.progress;

                view.m_pl_conduct_PolygonImage.gameObject.SetActive(false);
                view.m_pl_donate.gameObject.SetActive(hasDonate == false && preStudyIDs.Count==0);
                view.m_pl_condition.gameObject.SetActive(hasDonate);

               

                if (preStudyIDs.Count > 0)
                {
                    //可以前置条件
                    m_preLoadRes.AddRange(view.m_sv_preResearchList_ListView.ItemPrefabDataList);
                    ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
                    {
                        Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                        ListView.FuncTab funcTab = new ListView.FuncTab();
                        funcTab.ItemEnter = PreItemByIndex;

                        view.m_sv_preResearchList_ListView.SetInitData(assetDic, funcTab);
                        view.m_sv_preResearchList_ListView.FillContent(preStudyIDs.Count);
                    });

                    view.m_pl_needPreResearch_PolygonImage.gameObject.SetActive(true);
                }
                else
                {

                    if (hasDonate)
                    {
                        
                        //消耗资源组件
                        isCostEnough = view.m_UI_Model_ResCost.UpdateGuildResCost(m_nextStudy,m_allianceProxy.GetDepotCurrencyInfoEntities());

                        //研究按钮
                        view.m_pl_needPreResearch_PolygonImage.gameObject.SetActive(false);
                        view.m_btn_research.m_lbl_line1_LanguageText.text = LanguageUtils.getText(402015);
                        // PlayerAttributeProxy playerAttributeProxy =
                        //     AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
                        // float speed = playerAttributeProxy.GetCityAttribute(attrType.researchSpeedMulti).origvalue;

                        float needTime = m_nextStudy.costTime;//* (1000 - speed) / 1000;
                        view.m_btn_research.m_lbl_line2_LanguageText.text = ClientUtils.FormatCountDown((int) needTime);
                        
                        ClientUtils.UIReLayout(view.m_btn_research.m_btn_languageButton_GameButton);
                    }
                    else
                    {
                        

                        //进度
                        view.m_pb_donateBar_GameSlider.value = (float)m_serverDonatePoint/m_nextStudy.progress;
                        view.m_lbl_pro_LanguageText.text = LanguageUtils.getTextFormat(181104, ClientUtils.FormatComma(m_serverDonatePoint) , ClientUtils.FormatComma(m_nextStudy.progress));
                        
                        
                        long rate = m_critNum;//暴击倍数
                        //奖励
                        view.m_img_curCredit.m_lbl_languageText_LanguageText.text =
                            (m_allianceProxy.Config.AllianceAcquireStudyDot).ToString();
                        
                        view.m_img_curGuild.m_lbl_languageText_LanguageText.text =
                            (m_allianceProxy.Config.AllianceAcquireSoloFund).ToString();
                        

                        view.m_btn_donate_gem.SetNum(m_playerProxy.CurrentRoleInfo.guildDonateCostDenar.ToString());
                        

                        
                        //资源数量
                        view.m_btn_donate_res.m_lbl_line2_LanguageText.text =
                            ClientUtils.FormatComma(m_nextStudy.currencyNum);

                        var currency = CoreUtils.dataService.QueryRecord<CurrencyDefine>(m_nextStudy.currencyType);
                        ClientUtils.LoadSprite(view.m_btn_donate_res.m_img_icon2_PolygonImage,currency.iconID); 
                        
                        ClientUtils.UIReLayout(view.m_btn_donate_res.m_btn_languageButton_GameButton);
    
                       
                        long count = (ServerTimeModule.Instance.GetServerTime() - m_playerProxy.CurrentRoleInfo.lastGuildDonateTime)/m_allianceProxy.Config.AllianceStudyGiftCD;
                        long remianTime= (ServerTimeModule.Instance.GetServerTime() - m_playerProxy.CurrentRoleInfo.lastGuildDonateTime)%m_allianceProxy.Config.AllianceStudyGiftCD;
                        UpdateCount(count, remianTime);
                        
                        if (remianTime>0 && m_timer==null)
                        {
                            m_timer = Timer.Register(1,onTimer,null,true,true);
                            onTimer();
                        }
                        

                       
                    }

                   
                }
                
            }
        }

        private void UpdateCount(long count,long remianTime)
        {

            
            int totalCount = m_allianceProxy.Config.AllianceStudyGiftTime;
//            Debug.Log(remianTime+"上次"+m_playerProxy.CurrentRoleInfo.lastGuildDonateTime+"  server:"+ServerTimeModule.Instance.GetServerTime()+"  cd:"+m_allianceProxy.Config.AllianceStudyGiftCD);
            
            if (count>totalCount)
            {
                count = totalCount;
            }
            if (count>0)
            {
                view.m_lbl_chance_LanguageText.text = LanguageUtils.getTextFormat(733212,count,totalCount);
            }
            else
            {
                view.m_lbl_chance_LanguageText.text =
                    LanguageUtils.getTextFormat(733213, count, totalCount);
            }


           
            view.m_lbl_time_LanguageText.gameObject.SetActive(count != totalCount);
           
            
            
            view.m_btn_donate_res.m_btn_languageButton_GameButton.interactable = count > 0;
        }


        private void onTimer()
        {
            long remianTime=m_allianceProxy.Config.AllianceStudyGiftCD-((ServerTimeModule.Instance.GetServerTime() - m_playerProxy.CurrentRoleInfo.lastGuildDonateTime)%m_allianceProxy.Config.AllianceStudyGiftCD);
            long count = (ServerTimeModule.Instance.GetServerTime() - m_playerProxy.CurrentRoleInfo.lastGuildDonateTime)/m_allianceProxy.Config.AllianceStudyGiftCD;

            if (remianTime>=0)
            {
                view.m_lbl_time_LanguageText.text = LanguageUtils.getTextFormat(733383,ClientUtils.FormatTimeSplit((int)remianTime));
            }
            else
            {
                view.m_lbl_time_LanguageText.text = "";
            }
            UpdateCount(count,remianTime);
        }

        private void onDonateGem()
        {

            if ( ServerTimeModule.Instance.GetServerTime()-m_playerProxy.CurrentRoleInfo.joinGuildTime<=24*3600)
            {
                Tip.CreateTip(733203).SetStyle(Tip.TipStyle.Middle).Show();
                return;
            }

            if (!m_crrProxy.ShortOfDenar(m_playerProxy.CurrentRoleInfo.guildDonateCostDenar))
            {
                UIHelper.DenarCostRemain((m_playerProxy.CurrentRoleInfo.guildDonateCostDenar), () =>
                {
                    m_resProxy.SendDonateTechnology(2, GetStudyDefine().studyType);
                });

            }
            else
            {
                CoreUtils.uiManager.CloseUI(UI.s_AllianceResearchUpdate);
            }

        }

        private bool CheckRes()
        {
            long food = 0;
            long wood = 0;
            long stone = 0;
            long gold = 0;

            bool isEnough = true;
            
            if(m_nextStudy==null || m_crrProxy==null){
                return false;
            }

            switch ((EnumCurrencyType)m_nextStudy.currencyType)
            {
                case EnumCurrencyType.food:
                    food = m_nextStudy.currencyNum;
                    if (m_crrProxy.FloatFood<food)
                    {
                        isEnough = false;
                    }
                    break;
                case EnumCurrencyType.wood:
                    wood = m_nextStudy.currencyNum;
                    if (m_crrProxy.FloatWood<wood)
                    {
                        isEnough = false;
                    }
                    break;
                case EnumCurrencyType.stone:
                    stone = m_nextStudy.currencyNum;
                    if (m_crrProxy.FloatStone<stone)
                    {
                        isEnough = false;
                    }
                    break;
                case EnumCurrencyType.gold:
                    gold = m_nextStudy.currencyNum;
                    if (m_crrProxy.FloatGold<gold)
                    {
                        isEnough = false;
                    }
                    break;
            }

            if (isEnough==false)
            {
                m_crrProxy.LackOfResources(food, wood, stone,
                    gold);
            }

            return isEnough;
        }

        private float m_preTime = 0; 

        private void onDonateRes()
        {

            if (CheckRes() && Time.realtimeSinceStartup - m_preTime>0.3f)
            {

                m_preTime = Time.realtimeSinceStartup;
                m_resProxy.SendDonateTechnology(1,GetStudyDefine().studyType);
                
                
            }
        }

        private void OnResearch()
        {
            Debug.Log("OnResearch " + isCostEnough);
            
            long resCrrTechnologying = m_resProxy.GetTechEndTime();

            if (resCrrTechnologying > 0)
            {
                Tip.CreateTip(733202).Show();
                return;
            }


            if (!m_allianceProxy.CheckIsR45(m_playerProxy.CurrentRoleInfo.rid))
            {
                Tip.CreateTip(733200).Show();
                return;
            }
            
            if (isCostEnough)
            {
                m_resProxy.SendStudyTechnology(GetStudyDefine().studyType);
                CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);

                CoreUtils.audioService.PlayOneShot(RS.SoundResStart);
            }
            else
            {
                Alert.CreateAlert(732014, LanguageUtils.getText(730184)).SetRightButton().Show();

//                m_crrProxy.LackOfResources(m_nextStudy.needFood, m_nextStudy.needWood, m_nextStudy.needStone,
//                    m_nextStudy.needGold);
            }
        }

        private void onMark()
        {
            if (!m_allianceProxy.CheckIsR45(m_playerProxy.CurrentRoleInfo.rid))
            {
                Tip.CreateTip(730136).Show();
                return;
            }
            if (GetStudyDefine().studyType == m_resProxy.GetMarkType())
            {
                return;
            }
            m_resProxy.SendSetMarkTechnology(GetStudyDefine().studyType);
        }

        void PreItemByIndex(ListView.ListItem scrollItem)
        {
            ResearchPreItemView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<ResearchPreItemView>(scrollItem.go);
            var a = preStudyIDs[scrollItem.index];
            if (a != null)
            {
                itemView.m_lbl_lv_LanguageText.text = string.Format(LanguageUtils.getText(180306), a.studyLv);
                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(a.l_nameID);
                ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, a.icon);
                itemView.m_btn_jump.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_jump.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {

                    UI_Win_GuildResearchMediator md =
                        AppFacade.GetInstance().RetrieveMediator(UI_Win_GuildResearchMediator.NameMediator) as
                            UI_Win_GuildResearchMediator;
                    md.SetJump(a.studyType);
                    CoreUtils.uiManager.CloseUI(UI.s_AllianceResearchUpdate);
                });
            }
        }

        
        
        private void onMore()
        {
            view.m_img_update_Animator.gameObject.SetActive(false);
            view.m_pl_sv_level_Animator.gameObject.SetActive(true);
            
            ClientUtils.PlayUIAnimation(view.m_pl_sv_level_Animator,"Show");
    
            view.m_UI_win.m_btn_back_GameButton.gameObject.SetActive(true);
            view.m_UI_win.m_btn_back_GameButton.onClick.RemoveAllListeners();
            view.m_UI_win.m_btn_back_GameButton.onClick.AddListener(OnBack);



            m_preLoadRes.AddRange(view.m_sv_levelData_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;


                Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                funcTab.GetItemPrefabName = (item) =>
                {
                    int index = item.index;
                  
                    var a = m_crrStudys[index];
                    return view.m_sv_levelData_ListView.ItemPrefabDataList[a.l_attrTitleID.Count-1];
                };


                view.m_sv_levelData_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_levelData_ListView.FillContent(m_crrStudys.Count);
                
                var aa = m_crrStudys[0];
                int colCount = aa.l_attrTitleID.Count;
                CoreUtils.assetService.Instantiate(m_headerTilePrefab[colCount-1], (obj) =>
                {
                    for (int colIndex = 0; colIndex <= colCount; colIndex++)
                    {
                        var transform = obj.transform.Find(labelName[colIndex]);
                 
                        if (transform != null)
                        {
                            var text = transform.GetComponent<LanguageText>();
                            
                            if (colIndex==0)
                            {
                                text.text = LanguageUtils.getText(300004);
                            }
                            else
                            {
                                text.text = LanguageUtils.getText(aa.l_attrTitleID[colIndex-1]);
                            }
                        }
                    }
                    
                    obj.transform.SetParent(view.m_pl_tilte.transform); 
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;


                });

            });
        }

        private string[] m_headerTilePrefab = new[]
        {
            "UI_Item_BuildingLevelData_Title_Two","UI_Item_BuildingLevelData_Title_Three","UI_Item_BuildingLevelData_Title_Four","UI_Item_BuildingLevelData_Title_Five",
                "UI_Item_BuildingLevelData_Title_Six"
        };

        private string[] labelName = new[]
            {"lbl_name1", "lbl_name2", "lbl_name3", "lbl_name4", "lbl_name5", "lbl_name6", "lbl_name"};

            void ViewItemByIndex(ListView.ListItem scrollItem)
            {
                int index = scrollItem.index;
                
                var a = m_crrStudys[index];
                if (a != null)
                {
                    int colCount = a.l_attrTitleID.Count;

                    for (int colIndex = 0; colIndex <= colCount; colIndex++)
                    {
                        var transform = scrollItem.go.transform.Find(labelName[colIndex]);
                        if (transform != null)
                        {
                            var text = transform.GetComponent<LanguageText>();

                            //ID
                            if (colIndex == 0)
                            {
                                text.text = (scrollItem.index+1).ToString();
                            }
                            else
                            {
                                text.text = LanguageUtils.getTextFormat(a.l_attrTemplate[colIndex - 1],
                                    a.attrData[colIndex - 1]);
                            }
                        }
                    }
                }
            }
        
        private void OnBack()
        {
            view.m_img_update_Animator.gameObject.SetActive(true);
            view.m_pl_sv_level_Animator.gameObject.SetActive(false);
            view.m_UI_win.m_btn_back_GameButton.gameObject.SetActive(false);
            
            ClientUtils.PlayUIAnimation(view.m_img_update_Animator,"Show");
        }
       
        #endregion
    }
}
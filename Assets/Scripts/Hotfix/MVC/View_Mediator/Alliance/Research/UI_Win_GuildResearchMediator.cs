// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, May 13, 2020
// Update Time         :    Wednesday, May 13, 2020
// Class Description   :    UI_Win_GuildResearchMediator
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
    public class UI_Win_GuildResearchMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildResearchMediator";

        
        private AllianceResarchProxy m_resProxy;
        private AllianceProxy m_allianceProxy;
        
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();


        private List<List<AllianceStudyDefine>> m_develops;
        private List<List<AllianceStudyDefine>> m_territorys;
        private List<List<AllianceStudyDefine>> m_wars;

        private static int ItemHeight = 110/2;
        private static int WindowHeight = 600;
        

        
        private static int[] itemPos1 = new[] {0,210,105,0,-105,-210,160,0,0,0,0,0};
        private static int[] itemPos2 = new[] { 0,228,158,150,76,0,-76,-150,-158,-228,0,0,0,0,0,0};
        private static int[][] itemPos = new[] {itemPos2,itemPos2,itemPos2} ;



        private bool[] hasInit = new[] {false, false,false};
        private int m_selectType = EAllianceResearchType.develop;

        private int m_jumpStduyID = 0;
        private int m_jumpCol = 0;
        private int m_jumpType = EAllianceResearchType.develop;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildResearchMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildResearchView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.AllianceTechUpdate
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
           
                case CmdConstant.AllianceTechUpdate:
                    CheckList();
                    UpdateInfo();
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
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_resProxy = AppFacade.GetInstance().RetrieveProxy(AllianceResarchProxy.ProxyNAME) as AllianceResarchProxy;

            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            
            m_develops = m_resProxy.GetTechnologyByType(EAllianceResearchType.develop);

            view.m_pl_win.setWindowTitle(LanguageUtils.getText(733204));
        }


        private Timer m_timer;
        protected override void BindUIEvent()
        {
            view.m_pl_win.m_pl_side1.m_ck_ck_GameToggle.onValueChanged.AddListener(ChangeDevelopTags);
            view.m_pl_win.m_pl_side2.m_ck_ck_GameToggle.onValueChanged.AddListener(ChangeTerrtroyTags);
            view.m_pl_win.m_pl_side3.m_ck_ck_GameToggle.onValueChanged.AddListener(ChangeWarTags);

            view.m_pl_win.setCloseHandle(CloseRes);

            CheckList();
            
            m_timer = Timer.Register(1,UpdateInfo,null,true,true);

            UpdateInfo();
        }


        private void UpdateInfo()
        {
            view.m_lbl_donate_LanguageText.text = LanguageUtils.getTextFormat(733207,ClientUtils.FormatComma(m_resProxy.GetDonateNum()));
            
            view.m_lbl_time_LanguageText.text = ClientUtils.FormatTimeSplit((int)ServerTimeModule.Instance.GetDistanceZeroTime());


            long endTime = m_resProxy.GetTechEndTime();
            view.m_pl_speedUp.gameObject.SetActive(endTime>0);


            if (endTime>0)
            {
                var stConfig = m_resProxy.GetTechnologyByLevel((int)m_resProxy.GetCrrTechnologying(),1);
                int remainTime = (int) (m_resProxy.GetTechEndTime() + stConfig.costTime - ServerTimeModule.Instance.GetServerTime());
                int totalTime = stConfig.costTime;

                if (remainTime<0)
                {
                    remainTime = 0;
                }

                view.m_pb_spBar_GameSlider.value =1- (float)remainTime / totalTime;
                view.m_lbl_spTimeLeft_LanguageText.text =
                    LanguageUtils.getTextFormat(733208, ClientUtils.FormatTimeSplit((int)remainTime));

                ClientUtils.LoadSprite(view.m_img_spIcon_PolygonImage,stConfig.icon);
            }
        }

        private void CloseRes()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceResearchMain);
        }

        private void ChangeDevelopTags(bool value)
        {
            view.m_pl_win.m_pl_side1.m_img_iconH_PolygonImage.gameObject.SetActive(value);
            view.m_sv_develop_ListView.gameObject.SetActive(value);
            view.m_sv_terrtroy_ListView.gameObject.SetActive(!value);
            view.m_sv_war_ListView.gameObject.SetActive(!value);

            m_selectType = EAllianceResearchType.develop;
            view.m_pl_win.setWindowTitle(LanguageUtils.getText(733204));

            CheckList();
        }
        
        private void ChangeTerrtroyTags(bool value)
        {
            view.m_pl_win.m_pl_side2.m_img_iconH_PolygonImage.gameObject.SetActive(value);
            view.m_sv_develop_ListView.gameObject.SetActive(!value);
            view.m_sv_terrtroy_ListView.gameObject.SetActive(value);
            view.m_sv_war_ListView.gameObject.SetActive(!value);

            m_selectType = EAllianceResearchType.territory;
            view.m_pl_win.setWindowTitle(LanguageUtils.getText(733205));

            CheckList();
        }

        public void SetJump(int styType)
        {
            m_jumpStduyID = styType;
            
            SaveJumpData();
            CheckJumpStudy();
            
        }


        private void ChangeWarTags(bool value)
        {
            view.m_pl_win.m_pl_side3.m_img_iconH_PolygonImage.gameObject.SetActive(value);
            view.m_sv_develop_ListView.gameObject.SetActive(!value);
            view.m_sv_terrtroy_ListView.gameObject.SetActive(!value);
            view.m_sv_war_ListView.gameObject.SetActive(value);

            m_selectType = EAllianceResearchType.war;
            view.m_pl_win.setWindowTitle(LanguageUtils.getText(733206));

            CheckList();
        }

        private void CheckList()
        {
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                if (m_selectType == EAllianceResearchType.develop && hasInit[EAllianceResearchType.develop-1]==false)
                {
                    hasInit[EAllianceResearchType.develop - 1] = true;
                    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = EconomyByIndex;


                    view.m_sv_develop_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_develop_ListView.FillContent(m_develops.Count);
                    
                    CheckJumpStudy();
                }
                else if (m_selectType == EAllianceResearchType.territory&& hasInit[EAllianceResearchType.territory-1]==false)
                {
                    hasInit[EAllianceResearchType.territory - 1] = true;
                    m_territorys = m_resProxy.GetTechnologyByType(EAllianceResearchType.territory);

                    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = EconomyByIndex;

                    view.m_sv_terrtroy_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_terrtroy_ListView.FillContent(m_territorys.Count);
                    CheckJumpStudy();
                }else if (m_selectType == EAllianceResearchType.war&& hasInit[EAllianceResearchType.war-1]==false)
                {
                    hasInit[EAllianceResearchType.war - 1] = true;
                    m_wars = m_resProxy.GetTechnologyByType(EAllianceResearchType.war);

                    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = EconomyByIndex;

                    view.m_sv_war_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_war_ListView.FillContent(m_wars.Count);
                    CheckJumpStudy();
                }
                

               
            });
            
            
            if (m_selectType == EAllianceResearchType.develop && hasInit[EAllianceResearchType.develop-1])
            {
              
                view.m_sv_develop_ListView.ForceRefresh();
            }
            else if (m_selectType == EAllianceResearchType.territory&& hasInit[EAllianceResearchType.territory-1])
            {
                
                view.m_sv_terrtroy_ListView.ForceRefresh();
            }else if (m_selectType == EAllianceResearchType.war&& hasInit[EAllianceResearchType.war-1])
            {
               
                view.m_sv_war_ListView.ForceRefresh();
            }

            
        }

        private void CheckJumpStudy()
        {
            if (m_jumpStduyID > 0)
            {
                Debug.Log(m_jumpCol+"CheckJumpStudy"+m_jumpStduyID);
                if (m_selectType == EAllianceResearchType.develop)
                {
                    view.m_sv_develop_ListView.ScrollList2IdxCenter(m_jumpCol);
                    view.m_sv_develop_ListView.RefreshItem(m_jumpCol);
                }
                else if (m_selectType == EAllianceResearchType.territory)
                {
                    view.m_sv_terrtroy_ListView.ScrollList2IdxCenter(m_jumpCol);
                    view.m_sv_develop_ListView.RefreshItem(m_jumpCol);
                }
                else if (m_selectType == EAllianceResearchType.war)
                {
                    view.m_sv_war_ListView.ScrollList2IdxCenter(m_jumpCol);
                    view.m_sv_develop_ListView.RefreshItem(m_jumpCol);
                }
            }
        }

        void EconomyByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildResearchItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildResearchItemView>(scrollItem.go);


            List<AllianceStudyDefine> res = null;

            List<List<AllianceStudyDefine>> crrResList = null;

            if (m_selectType == EAllianceResearchType.develop)
            {
                crrResList = m_develops;
            }
            else if(m_selectType == EAllianceResearchType.territory)
            {
                crrResList = m_territorys;
            }
            else
            {
                crrResList = m_wars;
            }

            res =  crrResList[scrollItem.index];
            

            if (res!=null)
            {
                UI_Item_GuildResearchSubItem_SubView[] items = new[]
                    {itemView.m_item1, itemView.m_item2, itemView.m_item3, itemView.m_item4};

                for (int i = 0; i < 4; i++)
                {
                    var vs = res.Count > i;
                    var item = items[i];
                    item.gameObject.SetActive(vs);
                    if (vs)
                    {

                        var st = res[i];
                        item.setData(res[i],m_resProxy.GetCrrTechnologyLv(st.studyType),
                            m_resProxy.GetTechnologyMaxLv(st.studyType),
                            m_resProxy,m_selectType,
                            m_jumpStduyID);
                        item.gameObject.transform.localPosition = new Vector3(item.gameObject.transform.localPosition.x,
                            itemPos[m_selectType-1][st.location],
                            item.gameObject.transform.localPosition.z);

                        if (m_jumpStduyID==st.studyType)
                        {
                            m_jumpStduyID = 0;
                        }
                    }
                }
            }
        }



        protected override void BindUIData()
        {
            m_preLoadRes.AddRange(view.m_sv_develop_ListView.ItemPrefabDataList);
            
            //科技调整列
            if (view.data is GOScrptGuide)
            {
                SaveJumpData();
            }
            else
            {
                if (m_resProxy.GetMarkType() > 0)
                {
                    m_jumpStduyID =(int)m_resProxy.GetMarkType();
                    SaveJumpData();
                }
            }


        }
        
        private void SaveJumpData()
        {

            if (m_jumpStduyID==0)
            {
                GOScrptGuide GOScrptGuide = view.data as GOScrptGuide;

                m_jumpStduyID = GOScrptGuide.param1;
            }
            
            var studyDefines = m_resProxy.GetTechnologyList(m_jumpStduyID);
            int type = studyDefines[0].type;
            if (type == 0)
            {
                type = EAllianceResearchType.develop;
                m_jumpStduyID = 101;
            }
            
            Debug.Log(type+"研究任务跳转"+m_jumpStduyID);
            
            bool needChangeTag = m_selectType != type;
            m_selectType = type;
            if (type==EAllianceResearchType.develop)
            {
                if (studyDefines.Count>0)
                {
                    m_jumpCol = studyDefines[0].columns-1;
                    
                }
                view.m_pl_win.setWindowTitle(LanguageUtils.getText(733204));

                if (needChangeTag)
                {
                    ChangeDevelopTags(true);
                }
            }
            else if(type == EAllianceResearchType.territory)
            {
                if (studyDefines.Count>0)
                {
                    m_jumpCol = studyDefines[0].columns-1;
                }
                
                view.m_pl_win.m_pl_side2.m_ck_ck_GameToggle.isOn = true;
                CheckJumpStudy();
                if (needChangeTag)
                {
                    ChangeTerrtroyTags(true);
                }
            }
            else if(type == EAllianceResearchType.war)
            {
               
                    

                if (studyDefines.Count>0)
                {
                    m_jumpCol = studyDefines[0].columns-1;
                }
                
                view.m_pl_win.m_pl_side3.m_ck_ck_GameToggle.isOn = true;
                CheckJumpStudy();
                if (needChangeTag)
                {
                    ChangeWarTags(true);
                }
            }
            
            
        }
       
        #endregion
    }
}
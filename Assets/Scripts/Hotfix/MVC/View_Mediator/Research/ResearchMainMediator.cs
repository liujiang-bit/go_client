// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, December 31, 2019
// Update Time         :    Tuesday, December 31, 2019
// Class Description   :    ResearchMainMediator
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
    public class ResearchMainMediator : GameMediator {
        #region Member
        public static string NameMediator = "ResearchMainMediator";


        private ResearchProxy m_resProxy;
        private AllianceProxy m_allianceProxy;
        
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();


        private List<List<StudyDefine>> m_economys;
        private List<List<StudyDefine>> m_militarys;

        private static int ItemHeight = 110/2;
        private static int WindowHeight = 600;
        
        private static int three = WindowHeight / 2/ 3;
        private static int four = WindowHeight / 2 / 4;
        private static int two = WindowHeight / 2 / 2;
        
        
        private static int[] itemPos1 = new[] {0,210,105,0,-105,-210,160,0,0,0,0,0};
        private static int[] itemPos2 = new[] { 0,228,158,150,76,0,-76,-150,-158,-228,0,0,0,0,0,0};
//        private static int[] itemPos = new[] {0,three*2,four*3,three*2-ItemHeight,two,four,0,-four,-two,-four*3,-three*2} ;
        private static int[][] itemPos = new[] {itemPos1,itemPos2} ;



        private bool[] hasInit = new[] {false, false};
        private int m_selectType = EResearchType.economy;

        private int m_jumpStduyID = 0;
        private int m_jumpCol = 0;
        private int m_jumpType = EResearchType.economy;
        
        #endregion

        //IMediatorPlug needs
        public ResearchMainMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public ResearchMainView view;

        private QueueInfo m_info;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.technologyChange,
                CmdConstant.technologyQueueChange,
                CmdConstant.ChangeResearchMainToggle
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.technologyChange:
                    Debug.LogFormat("更新研究");
                    m_jumpStduyID = 0;
                    RefeshList();
                    break;
                case CmdConstant.technologyQueueChange:
                   
                    
                    
                    m_jumpStduyID = 0;

                    QueueInfo info = notification.Body as QueueInfo;

                    if (info.finishTime>0)
                    {
                        UpdateSpeed(info);
                        RefeshList();
                    }
                    else
                    {
                        Debug.LogFormat("更新研究进度");
                        CloseRes();
                    }
                    break;
                case CmdConstant.ChangeResearchMainToggle:
                    if((bool)notification.Body)
                    {
                        view.m_UI_SideBtns.m_pl_side1.m_ck_ck_GameToggle.isOn = true;
                    }
                    else
                    {
                        view.m_UI_SideBtns.m_pl_side1.m_ck_ck_GameToggle.isOn = false;
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
            m_resProxy = AppFacade.GetInstance().RetrieveProxy(ResearchProxy.ProxyNAME) as ResearchProxy;

            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            
            m_economys = m_resProxy.GetTechnologyByType(EResearchType.economy);
            
            
            
            
        }

        protected override void BindUIEvent()
        {
            view.m_UI_SideBtns.m_pl_side1.m_ck_ck_GameToggle.onValueChanged.AddListener(ChangeEconomyTags);
            view.m_UI_SideBtns.m_pl_side2.m_ck_ck_GameToggle.onValueChanged.AddListener(ChangeMilitaryTags);


            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(402017));
            
            view.m_UI_Model_Window_Type1.setCloseHandle(CloseRes);


            view.m_btn_cancelRes_GameButton.onClick.AddListener(onCancel);
            
            view.m_UI_speedUp.m_btn_languageButton_GameButton.onClick.AddListener(onAddSpeed);
            
            view.m_UI_speedHelp.m_btn_languageButton_GameButton.onClick.AddListener(onSendHelp);
        }
        //加速道具
        private void onAddSpeed()
        {
            QueueInfo queueInfo = m_resProxy.GetCrrTechnologying();
            SpeedUpData data = new SpeedUpData();
            data.type = EnumSpeedUpType.research;
            data.icon = view.m_img_spIcon_PolygonImage.sprite;
            data.queue = queueInfo;
            //data.cancelCallback = onCancel;
            AppFacade.GetInstance().SendNotification(CmdConstant.SpeedUp,data);
        }

        private void onSendHelp()
        {
            QueueInfo queueInfo = m_resProxy.GetCrrTechnologying();
            m_allianceProxy.SendRequestHelp(3,queueInfo.queueIndex);
        }

        protected override void BindUIData()
        {
            
            
            
           
            m_preLoadRes.AddRange(view.m_sv_economy_ListView.ItemPrefabDataList);
            
            //科技调整列
            if (view.data is GOScrptGuide)
            {
                SaveJumpData();
            }
//            ChangeEconomyTags(true);

            UpdateSpeed(m_resProxy.GetCrrTechnologying());
        }
        
        public void SetJump(int styType)
        {
            m_jumpStduyID = styType;
            
            SaveJumpData();
            CheckJumpStudy();
            
        }

        private void CheckJumpStudy()
        {
            if (m_jumpStduyID > 0)
            {
                if (m_selectType == EResearchType.economy)
                {
                    view.m_sv_economy_ListView.ScrollList2IdxCenter(m_jumpCol);
                    view.m_sv_economy_ListView.RefreshItem(m_jumpCol);
                }
                else if (m_selectType == EResearchType.military)
                {
                    view.m_sv_military_ListView.ScrollList2IdxCenter(m_jumpCol);
                    view.m_sv_military_ListView.RefreshItem(m_jumpCol);
                }
            }
        }

        private void SaveJumpData()
        {
            if (view.data!=null)
            {
                GOScrptGuide GOScrptGuide = view.data as GOScrptGuide;
                m_jumpStduyID = GOScrptGuide.param1;
            }

            if (m_jumpStduyID==0)
            {
                m_jumpStduyID = m_resProxy.GuessNestTechnology();
            }
            
            var studyDefines = m_resProxy.GetTechnologyList(m_jumpStduyID);

            if (studyDefines==null)
            {
                return;
            }
            int type = studyDefines[0].type;
            if (type == 0)
            {
                type = EResearchType.economy;
                m_jumpStduyID = 101;
            }
            
            Debug.Log(type+"研究任务跳转"+m_jumpStduyID);

            bool needChangeTag = m_selectType != type;
                
            m_selectType = type;
            if (type==EResearchType.economy)
            {
                if (studyDefines.Count>0)
                {
                    m_jumpCol = studyDefines[0].columns-1;
                    
                }

                if (needChangeTag)
                {
                    ChangeEconomyTags(true);
                }
            }
            else if(type == EResearchType.military)
            {
                if (needChangeTag)
                {
                    ChangeMilitaryTags(true);
                }

                if (studyDefines.Count>0)
                {
                    m_jumpCol = studyDefines[0].columns-1;
                }
                
                view.m_UI_SideBtns.m_pl_side2.m_ck_ck_GameToggle.isOn = true;
            }
        }

        private void onCancel()
        {
            Alert.CreateAlert(402016, LanguageUtils.getText(300099)).SetRightButton(onCancelOK,LanguageUtils.getText(300014)).SetLeftButton(null,LanguageUtils.getText(300013)).Show();
        }

        private void onCancelOK()
        {
            m_resProxy.StopTechnology();
            Tip.CreateTip(402022).Show();
        }

        private void RefeshList()
        {
            if (m_selectType == EResearchType.economy)
            {
                view.m_sv_economy_ListView.ForceRefresh();
            }
            else
            {
                view.m_sv_military_ListView.ForceRefresh();
            }
        }

        //加速进度条
        private void UpdateSpeed(QueueInfo info)
        {

            if (view.m_pl_speedUp == null)
            {
                return;
            }
            
            m_info = info;
            if (m_info != null && m_info.HasTechnologyType && m_info.technologyType > 0 && m_info.finishTime>0)
            {
                view.m_pl_speedUp.gameObject.SetActive(true);
                
                view.m_UI_speedHelp.gameObject.SetActive(!m_info.requestGuildHelp && m_allianceProxy.HasJionAlliance());
                
                view.m_UI_speedUp.gameObject.SetActive(!view.m_UI_speedHelp.gameObject.activeSelf);

                var dinfo = m_resProxy.GetTechnologyList((int)m_info.technologyType)[0];
                var isSoldierStudy1 = m_resProxy.IsSoldierRes(dinfo.ID);
                var spImg = isSoldierStudy1 != null ? isSoldierStudy1.icon : dinfo.icon;
                ClientUtils.LoadSprite(view.m_img_spIcon_PolygonImage,spImg);
                CityHudCountDownManager.Instance.AddUiQueue(null,view.m_lbl_spTimeLeft_LanguageText,view.m_pb_spBar_GameSlider,m_info,null,TimeEnd,402021);
            }
            else
            {
                m_info = null;
                view.m_pl_speedUp.gameObject.SetActive(false);
            }
        }

        private void TimeEnd(long time)
        {
            CoreUtils.uiManager.CloseUI(UI.s_ResearchUpdate);
            CoreUtils.uiManager.CloseUI(UI.s_ResearchMain);
        }



        private void CloseRes()
        {
            CoreUtils.uiManager.CloseUI(UI.s_ResearchMain,true,true);
        }
        //切换tag
        private void ChangeEconomyTags(bool value)
        {

            view.m_UI_SideBtns.m_pl_side1.m_img_iconH_PolygonImage.gameObject.SetActive(value);
            view.m_sv_economy_ListView.gameObject.SetActive(value);
            view.m_sv_military_ListView.gameObject.SetActive(!value);

            m_selectType = EResearchType.economy;
            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(402017));

            CheckList();
        }
        private void ChangeMilitaryTags(bool value)
        {
            
            view.m_UI_SideBtns.m_pl_side2.m_img_iconH_PolygonImage.gameObject.SetActive(value);
            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(402018));
            
            m_selectType = EResearchType.military;

            view.m_sv_economy_ListView.gameObject.SetActive(!value);
            view.m_sv_military_ListView.gameObject.SetActive(value);
            
            CheckList();
        }

        private void CheckList()
        {
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                if (m_selectType == EResearchType.economy && hasInit[EResearchType.economy-1]==false)
                {
                    hasInit[EResearchType.economy - 1] = true;
                    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = EconomyByIndex;
                    funcTab.GetItemPrefabName = delegate(ListView.ListItem item) { return "UI_ResearchItem"; };


                    view.m_sv_economy_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_economy_ListView.FillContent(m_economys.Count);
                }
                else if (m_selectType == EResearchType.military&& hasInit[EResearchType.military-1]==false)
                {
                    hasInit[EResearchType.military - 1] = true;
                    m_militarys = m_resProxy.GetTechnologyByType(EResearchType.military);

                    Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = EconomyByIndex;

                    view.m_sv_military_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_military_ListView.FillContent(m_militarys.Count);
                }

                CheckJumpStudy();
            });
        }



        void EconomyByIndex(ListView.ListItem scrollItem)
        {
            ResearchItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<ResearchItemView>(scrollItem.go);


            List<StudyDefine> res = null;

            List<List<StudyDefine>> crrResList = null;

            if (m_selectType == EResearchType.economy)
            {
                crrResList = m_economys;
            }
            else
            {
                crrResList = m_militarys;
            }
            
            res =  crrResList[scrollItem.index];
            

            if (res!=null)
            {
                UI_ResearchSubItem_SubView[] items = new[]
                    {itemView.m_item1, itemView.m_item2, itemView.m_item3, itemView.m_item4};

                for (int i = 0; i < 4; i++)
                {
                    var vs = res.Count > i;
                    var item = items[i];
                    item.gameObject.SetActive(vs);
                    if (vs)
                    {
//                        if (m_jumpStduyID>0)
//                        {
//                            Debug.Log("跳转"+res[i].columns);
//                        }
                        var st = res[i];
                        item.setData(res[i],m_resProxy.GetCrrTechnologyLv(st.studyType),
                            m_resProxy.GetTechnologyMaxLv(st.studyType),
                            m_resProxy,m_selectType,
                            m_jumpStduyID);
                        item.gameObject.transform.localPosition = new Vector3(item.gameObject.transform.localPosition.x,
                            itemPos[m_selectType-1][st.location],
                            item.gameObject.transform.localPosition.z);
                    }
                }
            }
        }
        
        
        
       
        #endregion
    }
}
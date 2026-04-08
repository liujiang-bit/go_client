// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月10日
// Update Time         :    2020年1月10日
// Class Description   :    ArmyDetailsMediator
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
using Data;

namespace Game {
    public class TreeChildNode
    {
        public int queueIndex;
        public int parentStatus;
        public long joinTime;
        public long mainHeroId;
        public long mainHeroLevel;
        public long deputyHeroId;
        public long deputyHeroLevel;
        public long beginArmyCount;
        public string name;
        public object data;
        public int leaderStatus;  //0队长 1队员
    }

    public class TreeViewNode
    {
        public object data;
        public bool isParent;
        public bool isFold;
        public int index;
        public int intData;
    }
    public class FightArmyHurtParam
    {
        public BattleReportExDetail ReportExDetail;
        public long ObjectIndex;
    }

    public class ArmyDetailsMediator : GameMediator {
        #region Member
        public static string NameMediator = "ArmyDetailsMediator";

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        //private BattleReport2 m_currentBattleData;

        private List<TreeViewNode> m_allTreeNodes = new List<TreeViewNode>();

        private List<TreeViewNode> m_currentTree = new List<TreeViewNode>();

        private BattleReportExDetail m_battleReport;
        private long m_objectIndex;

        private EmailProxy m_emailProxy;
        private PlayerProxy m_playerProxy;

        private Dictionary<string, float> m_itemHeightDic = new Dictionary<string, float>();

        private bool m_isJoinTimeSort;

        #endregion

        //IMediatorPlug needs
        public ArmyDetailsMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public ArmyDetailsView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>() {

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

        public override void OpenAniEnd() {

        }

        public override void WinFocus() {

        }

        public override void WinClose() {

        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            //m_battleReport = m_emailProxy.CuurentArmyDetail;
            //if (m_battleReport==null)
            //{
            //    Debug.LogError("没有BattleReportExDetail");
            //    m_battleReport = view.data as BattleReportExDetail;
            //}
            FightArmyHurtParam param = view.data as FightArmyHurtParam;
            m_battleReport = param.ReportExDetail;
            m_objectIndex = param.ObjectIndex;
            
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_ArmyDetail);
        }

        protected override void BindUIData()
        {
            List<string> m_list = new List<string>();
            m_list.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_list, OnLoadFinish);
        }

        #endregion
        private void OnLoadFinish(Dictionary<string, GameObject> m_dic)
        {
            m_assetDic = m_dic;

            foreach (var data in m_assetDic)
            {
                m_itemHeightDic[data.Key] = data.Value.GetComponent<RectTransform>().rect.height;
            }

            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = OnBuffItemEnter;
            funcTab.GetItemPrefabName = GetItemPrefabName;
            funcTab.GetItemSize = GetItemPrefabSize;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);

            m_isJoinTimeSort = true;
            if (m_battleReport.battleRallySoldierHurt != null && m_battleReport.battleRallySoldierHurt.Count>0) //集结信息
            {
                foreach (var data in m_battleReport.battleRallySoldierHurt)
                {
                    foreach (var data2 in data.Value.rallyHurt)
                    {
                        if (data2.Value.joinTime <= 0)
                        {
                            m_isJoinTimeSort = false;
                            Debug.Log("不能按时间排序");
                            break;
                        }
                    }
                }

                int queueId = 0;
                int leaderStatus;
                bool isSetLeader = false;
                foreach (var data in m_battleReport.battleRallySoldierHurt)
                {
                    leaderStatus = 1;
                    if (data.Value.isLeader)
                    {
                        leaderStatus = 0;
                    }
                    foreach (var data2 in data.Value.rallyHurt)
                    {
                        queueId = queueId + 1;
                        bool isFold = true;
                        if (leaderStatus == 0)
                        {
                            if (isSetLeader)
                            {
                                leaderStatus = 1;
                            }
                            else
                            {
                                isFold = false;
                                isSetLeader = true;
                            }
                        }

                        long jTime = 0;
                        if (leaderStatus == 0)
                        {
                            jTime = -1;
                        }
                        else
                        {
                            jTime = m_isJoinTimeSort ? data2.Value.joinTime : queueId;
                        }

                        TreeChildNode titleData1 = new TreeChildNode();
                        titleData1.parentStatus = 1;
                        titleData1.joinTime = jTime; // todo
                        titleData1.name = data.Value.rallyRoleName;// todo
                        titleData1.mainHeroId = data2.Value.mainHeroId;
                        titleData1.mainHeroLevel = data2.Value.mainHeroLevel;
                        titleData1.deputyHeroId = data2.Value.deputyHeroId;
                        titleData1.deputyHeroLevel = data2.Value.deputyHeroLevel;
                        titleData1.data = data2.Value.rallySoldierDetail;
                        titleData1.leaderStatus = leaderStatus;
                        titleData1.queueIndex = (leaderStatus ==0)?-1: queueId;

                        m_allTreeNodes.Add(new TreeViewNode { isParent = true, isFold = isFold, data = titleData1 });

                        if (data2.Value.rallySoldierDetail != null)
                        {
                            foreach (var data3 in data2.Value.rallySoldierDetail)
                            {
                                TreeChildNode titleNode1 = new TreeChildNode();
                                titleNode1.parentStatus = 2;
                                titleNode1.joinTime = jTime;
                                titleNode1.data = data3.Value;
                                titleNode1.leaderStatus = leaderStatus;
                                titleNode1.queueIndex = (leaderStatus == 0) ? -1 : queueId;

                                m_allTreeNodes.Add(new TreeViewNode { isParent = false, data = titleNode1 });
                            }
                        }
                    }
                }
            }
            else
            {
                TreeChildNode titleNode = new TreeChildNode();
                titleNode.parentStatus = 1;
                titleNode.joinTime = 0;
                titleNode.mainHeroId = m_battleReport.mainHeroId;
                titleNode.mainHeroLevel = m_battleReport.mainHeroLevel;
                titleNode.deputyHeroId = m_battleReport.deputyHeroId;
                titleNode.deputyHeroLevel = m_battleReport.deputyHeroLevel;
                titleNode.beginArmyCount = m_battleReport.beginArmyCount;
                titleNode.name = m_battleReport.name;
                titleNode.data = m_battleReport.soldierDetail[m_objectIndex].battleSoldierHurt;
                titleNode.leaderStatus = 0;

                m_allTreeNodes.Add(new TreeViewNode { isParent = true, isFold = false, data = titleNode });
                foreach (var data in m_battleReport.soldierDetail[m_objectIndex].battleSoldierHurt)
                {
                    TreeChildNode titleNode1 = new TreeChildNode();
                    titleNode1.parentStatus = 2;
                    titleNode1.joinTime = 0;
                    titleNode1.data = data.Value;
                    titleNode1.leaderStatus = 0;

                    m_allTreeNodes.Add(new TreeViewNode { isParent = false, data = titleNode1 });
                }
            }

            m_allTreeNodes.Sort(SortSoldierHurt);
            m_allTreeNodes.RemoveAll(RemoveSoldierHurt);

            InitView();
        }

        private void InitView()
        {
            bool isFold = false;
            m_currentTree.Clear();
            for (int i = 0; i < m_allTreeNodes.Count; i++)
            {
                m_allTreeNodes[i].index = i;
                if (m_allTreeNodes[i].isParent)
                {
                    m_currentTree.Add(m_allTreeNodes[i]);
                    isFold = m_allTreeNodes[i].isFold;
                }
                else if (!isFold)
                {
                    m_currentTree.Add(m_allTreeNodes[i]);
                }
            }
            view.m_sv_list_view_ListView.FillContent(m_currentTree.Count);
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            int index = item.index;
            return m_currentTree[index].isParent ? "UI_Item_PlayerArmy" : "UI_Item_PlayerArmySoldier";
        }

        private float GetItemPrefabSize(ListView.ListItem item)
        {
            return m_currentTree[item.index].isParent ? m_itemHeightDic["UI_Item_PlayerArmy"] : m_itemHeightDic["UI_Item_PlayerArmySoldier"];
        }

        private int SortSoldierHurt(TreeViewNode soldierHurt1, TreeViewNode soldierHurt2)//TODO：
        {
            var data1 = soldierHurt1.data as TreeChildNode;
            var data2 = soldierHurt2.data as TreeChildNode;

            int re = data1.joinTime.CompareTo(data2.joinTime);
            if (re == 0)
            {
                re = data1.queueIndex.CompareTo(data2.queueIndex);
                if (re == 0)
                {
                    re = data1.leaderStatus.CompareTo(data2.leaderStatus);
                    if (re == 0)
                    {
                        re = data1.parentStatus.CompareTo(data2.parentStatus);
                        if (re == 0)
                        {
                            if (re == 0)
                            {
                                if (data1.parentStatus == 1)
                                {
                                    return -1;
                                }

                                if (data2.parentStatus == 1)
                                {
                                    return 1;
                                }

                                BattleSoldierHurt soldierHurtX = data1.data as BattleSoldierHurt;
                                BattleSoldierHurt soldierHurtY = data2.data as BattleSoldierHurt;
                                ArmsDefine armsDefinesX = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)soldierHurtX.soldierId);
                                ArmsDefine armsDefinesY = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)soldierHurtY.soldierId);

                                if (armsDefinesX == null || armsDefinesY == null)
                                {
                                    return -1;
                                }

                                int typeX = armsDefinesX.armsType;
                                int typeY = armsDefinesY.armsType;
                                int levelX = armsDefinesX.armsLv;
                                int levelY = armsDefinesY.armsLv;
                                // Debug.LogErrorFormat("{0},,,{1},,,{2},,,{3},,,,", typeX, typeY, levelX, levelY);
                                if (typeX == 7)
                                {
                                    return -1;
                                }
                                if (typeY == 7)
                                {
                                    return 1;
                                }
                                re = levelY.CompareTo(levelX);
                                if (re == 0)
                                {
                                    return typeX.CompareTo(typeY);
                                }
                            }
                        }
                    }
                }
            }
            return re;
        }
        private bool RemoveSoldierHurt(TreeViewNode soldierHurt)
        {
            if (soldierHurt.isParent)
            {
                return false;
            }
            var obj = soldierHurt.data as TreeChildNode;
            BattleSoldierHurt soldierHurtX = obj.data as BattleSoldierHurt;
            if ((soldierHurtX.minor + soldierHurtX.hardHurt + soldierHurtX.die + soldierHurtX.remain) == 0)
            {
                return true;
            }
            return false;
        }

        private void OnBuffItemEnter(ListView.ListItem item)
        {
            int index = item.index;
            var treeData = m_currentTree[index];
            var obj = treeData.data as TreeChildNode;
            if (treeData.isParent)
            {
                UI_Item_PlayerArmyView parentView = MonoHelper.AddHotFixViewComponent<UI_Item_PlayerArmyView>(item.go); 
                var dataDic = obj.data as Dictionary<long, BattleSoldierHurt>;
                long total = GetTotalArmyTotalNum(dataDic, out long oHeal, out long oDie, out long oHurt, out long oRemain);
                parentView.m_lbl_col5_LanguageText.text = oRemain.ToString("N0");//剩余
                parentView.m_lbl_col4_LanguageText.text = oHeal.ToString("N0");//轻伤
                parentView.m_lbl_col3_LanguageText.text = oHurt.ToString("N0");//重伤
                parentView.m_lbl_col2_LanguageText.text = oDie.ToString("N0");//死亡
                parentView.m_lbl_col1_LanguageText.text = total.ToString("N0");//总共
                parentView.m_lbl_playername_LanguageText.text = obj.name;
                HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>((int)obj.mainHeroId);
                if(hero!=null)
                {
                    parentView.m_UI_Model_CaptainHeadWithLevel_main.SetIcon(hero.heroIcon);
                    parentView.m_UI_Model_CaptainHeadWithLevel_main.SetRare(hero.rare);
                    parentView.m_UI_Model_CaptainHeadWithLevel_main.SetLevel(obj.mainHeroLevel);
                }
                else
                {
                    parentView.m_UI_Model_CaptainHeadWithLevel_main.gameObject.SetActive(false);
                }

                HeroDefine hero2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)obj.deputyHeroId);
                if (hero2 != null)
                {
                    parentView.m_UI_Model_CaptainHeadWithLevel_sub.SetIcon(hero2.heroIcon);
                    parentView.m_UI_Model_CaptainHeadWithLevel_sub.SetRare(hero2.rare);
                    parentView.m_UI_Model_CaptainHeadWithLevel_sub.SetLevel(obj.deputyHeroLevel);
                }
                else
                {
                    parentView.m_UI_Model_CaptainHeadWithLevel_sub.gameObject.SetActive(false);
                }
                bool isFlod = m_currentTree[index].isFold;
                parentView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!isFlod);
                parentView.m_img_arrow_left_PolygonImage.gameObject.SetActive(isFlod);
                parentView.m_btn_arrow_GameButton.onClick.RemoveAllListeners();
                parentView.m_btn_arrow_GameButton.onClick.AddListener(()=>
                {
                    m_currentTree[index].isFold = !m_currentTree[index].isFold;
                    InitView();
                });

                parentView.m_img_teamleader_PolygonImage.gameObject.SetActive(obj.leaderStatus == 0);
            }
            else
            {
                UI_Item_PlayerArmySoldierView childView = MonoHelper.AddHotFixViewComponent<UI_Item_PlayerArmySoldierView>(item.go);
                BattleSoldierHurt soldierData = obj.data as BattleSoldierHurt;
                childView.m_lbl_col5_LanguageText.text = soldierData.remain.ToString("N0");//剩余
                childView.m_lbl_col4_LanguageText.text = soldierData.minor.ToString("N0");//轻伤
                childView.m_lbl_col3_LanguageText.text = soldierData.hardHurt.ToString("N0");//重伤
                childView.m_lbl_col2_LanguageText.text = soldierData.die.ToString("N0");//死亡
                childView.m_lbl_col1_LanguageText.text = (soldierData.remain+ soldierData.hardHurt+ soldierData.die+ soldierData.minor).ToString("N0");//总共
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)soldierData.soldierId);
                if (define != null)
                {
                    ClientUtils.LoadSprite(childView.m_UI_Model_ArmyTrainHead.m_img_army_icon_PolygonImage, define.icon);
                    if (define.armsType == 7)
                    {
                        childView.m_lbl_col5_LanguageText.text = (soldierData.remain).ToString("N0");//剩余
                        childView.m_lbl_col4_LanguageText.text = "-";//轻伤
                        childView.m_lbl_col3_LanguageText.text = "-";//重伤
                        childView.m_lbl_col2_LanguageText.text = (soldierData.die+soldierData.hardHurt + soldierData.minor).ToString("N0");//死亡
                    }
                    else
                    {
                        childView.m_lbl_col5_LanguageText.text = soldierData.remain.ToString("N0");//剩余
                        childView.m_lbl_col4_LanguageText.text = soldierData.minor.ToString("N0");//轻伤
                        childView.m_lbl_col3_LanguageText.text = soldierData.hardHurt.ToString("N0");//重伤
                        childView.m_lbl_col2_LanguageText.text = soldierData.die.ToString("N0");//死亡
                    }
                }
            }
        }

        private long GetTotalArmyTotalNum(Dictionary<long, BattleSoldierHurt> battleSoldierHurt, out long heal, out long die, out long hurt, out long remain)
        {
            long total = 0;
            remain = 0;
            heal = 0;
            die = 0;
            hurt = 0;
            foreach (var data in battleSoldierHurt)
            {
                BattleSoldierHurt element = data.Value;
                if (element.soldierId / 10000 == 3)
                {
                    die += element.minor;
                    die += element.die;
                    die += element.hardHurt;
                    remain += element.remain;
                }
                else
                {
                    heal += element.minor;
                    die += element.die;
                    hurt += element.hardHurt;
                    remain += element.remain;
                }
                total += element.minor + element.hardHurt + element.die + element.remain;
               // Debug.LogError(element.soldierId + "轻伤 " + element.minor + "重伤" + element.hardHurt + "死亡 " + element.die + "剩余 " + element.remain + "治疗 " + element.heal);
            }
            return total;
        }

    }
}
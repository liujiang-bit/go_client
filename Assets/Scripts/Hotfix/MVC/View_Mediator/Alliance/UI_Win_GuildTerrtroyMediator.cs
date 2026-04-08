// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 16, 2020
// Update Time         :    Thursday, April 16, 2020
// Class Description   :    UI_Win_GuildTerrtroyMediator
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
using ILRuntime.Runtime.Debugger.Protocol;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game {
    public class UI_Win_GuildTerrtroyMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildTerrtroyMediator";



        private AllianceProxy m_allianceProxy;
        private PlayerProxy m_playerProxy;

        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private List<AllianceBuildTypeTag> m_buildTypesList = new List<AllianceBuildTypeTag>(4);

        private GuildResourcePointInfoEntity m_resPoint;

        private UI_Item_GuildTerrtroyBuildingSingle_SubView[] subViews = new UI_Item_GuildTerrtroyBuildingSingle_SubView[5];

        private bool m_isGetData;

        private bool m_isFuncGuide;
        private bool m_isShowGuideArrow;
        private int m_findIndex;
        private int m_findSubIndex;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildTerrtroyMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_GuildTerrtroyView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_GuildMemberInfo.TagName,
                Guild_GuildNotify.TagName,
                CmdConstant.AllianceBuildUpdate
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AllianceBuildUpdate:
                    long reqType = (long)notification.Body;
                    if (reqType == 1)
                    {
                        m_isGetData = true;
                        InitReList();
                    }
                    else
                    {
                        ReList();
                    }
                    break;
                case Guild_GuildMemberInfo.TagName:
                case Guild_GuildNotify.TagName:
                    ReList();
                    break;
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
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        public override void PrewarmComplete() {

        }

        public override void Update()
        {
            if (IsTouchBegin())
            {
                if (m_isShowGuideArrow)
                {
                    m_isShowGuideArrow = false;
                }
            }
        }

        protected override void InitData()
        {
            IsOpenUpdate = true;

            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;

            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            m_resPoint = m_allianceProxy.GetResPoint();

            m_allianceProxy.SendGetGuildBuilds(1);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.setCloseHandle(onClose);

            view.m_UI_Item_AliGuide.RegisterBtnEvent(() => {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceGuideHelp);
            });
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceTerrtroy);
        }

        protected override void BindUIData()
        {
            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) => {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                funcTab.GetItemSize = GetItemSize;
                funcTab.GetItemPrefabName = GetItemPrefabName;

                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);

                //判断是否功能引导
                if (!GuideProxy.IsGuideing)
                {
                    var funcGuideProxy = AppFacade.GetInstance().RetrieveProxy(FuncGuideProxy.ProxyNAME) as FuncGuideProxy;
                    if (!funcGuideProxy.IsCompletedByStage((int)EnumFuncGuide.AllianceBuildCreate))
                    {
                        m_isFuncGuide = true;
                        AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.AllianceBuildCreate);
                    }
                }

                InitReList();
            });

            view.m_btn_info_GameButton.onClick.AddListener(onTipInfo);

            view.m_btn_get.m_btn_languageButton_GameButton.onClick.AddListener(onGetRes);

            resItems[0] = view.m_UI_food;
            resItems[1] = view.m_UI_wood;
            resItems[2] = view.m_UI_stone;
            resItems[3] = view.m_UI_gold;


            UpdateInfo();

            m_timer = Timer.Register(1, OnTime, null, true, true);

        }

        private void OnTime()
        {
            UpdateInfo();
        }

        private void onGetRes()
        {
            if (m_playerProxy.CurrentRoleInfo.level >= m_allianceProxy.Config.allianceResourcePersonRequestLv)
            {
                if (crrResCount > 0)
                {
                    bool hasRes = m_allianceProxy.SendGetResPoint();

                    GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;

                    if (resValues[0] > 0)
                    {
                        mt.FlyUICurrency((int)EnumCurrencyType.food, resValues[0], view.m_UI_food.m_img_icon_PolygonImage.rectTransform.position, view.m_UI_food.m_img_icon_PolygonImage.rectTransform.position + new Vector3(0, 100));
                    }
                    if (resValues[1] > 0)
                    {
                        mt.FlyUICurrency((int)EnumCurrencyType.wood, resValues[1], view.m_UI_wood.m_img_icon_PolygonImage.rectTransform.position, view.m_UI_wood.m_img_icon_PolygonImage.rectTransform.position + new Vector3(0, 100));
                    }
                    if (resValues[2] > 0)
                    {
                        mt.FlyUICurrency((int)EnumCurrencyType.stone, resValues[2], view.m_UI_stone.m_img_icon_PolygonImage.rectTransform.position, view.m_UI_stone.m_img_icon_PolygonImage.rectTransform.position + new Vector3(0, 100));
                    }
                    if (resValues[3] > 0)
                    {
                        mt.FlyUICurrency((int)EnumCurrencyType.gold, resValues[3], view.m_UI_gold.m_img_icon_PolygonImage.rectTransform.position, view.m_UI_gold.m_img_icon_PolygonImage.rectTransform.position + new Vector3(0, 100));
                    }
                }
            }
            else
            {
                Tip.CreateTip(732046, m_allianceProxy.Config.allianceResourcePersonRequestLv).Show();
            }
        }

        private Timer m_timer;

        private long[] CurrencyIDs = new[] { 100L, 101, 102, 103 };
        private int[] ResPointIDs = new[] { 4, 5, 6, 7 };
        private long[] resValues = new[] { 0L, 0, 0, 0 };

        private UI_Model_ResourcesConsume_SubView[] resItems = new UI_Model_ResourcesConsume_SubView[4];


        private long crrResCount;
        private void UpdateInfo()
        {

            var resPointCount = m_allianceProxy.GetResPoint();

            crrResCount = 0;
            int fullCount = 0; ;
            for (int i = 0; i < CurrencyIDs.Length; i++)
            {
                var gaindata = m_allianceProxy.GetTerritoryGainByType(CurrencyIDs[i]);
                var resPointConfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(ResPointIDs[i]);

                var resItem = this.resItems[i];

                long resCount = 0;

                if (gaindata != null)
                {
                    if (gaindata.num >= gaindata.limit && gaindata.limit > 0)
                    {
                        resCount = gaindata.num;

                        crrResCount = crrResCount + resCount;

                        fullCount++;
                    }
                    else
                    {
                        //点的数量*

                        long time = ServerTimeModule.Instance.GetServerTime() - gaindata.territoryTime;
                        float speed = resPointConfig.holdPersonSpeed * (time / 3600f);

                        switch (i)
                        {
                            case 0:
                                resCount = (long)(resPointCount.foodPoint * speed);
                                break;
                            case 1:
                                resCount = (long)(resPointCount.woodPoint * speed);
                                break;
                            case 2:
                                resCount = (long)(resPointCount.stonePoint * speed);
                                break;
                            case 3:
                                resCount = (long)(resPointCount.goldPoint * speed);
                                break;
                        }

                        if (resCount > gaindata.limit && gaindata.limit > 0)
                        {
                            resCount = gaindata.limit;
                            fullCount++;
                        }



                        crrResCount = crrResCount + resCount;
                    }

                    resValues[i] = resCount;


                }
                // 计算
                resItem.SetRes(resCount);
            }

            view.m_btn_get.m_img_redpoint_PolygonImage.gameObject.SetActive(fullCount > 0);

        }


        private void onTipInfo()
        {
            HelpTip.CreateTip(4014, view.m_btn_info_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
        }

        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            var data = m_buildTypesList[scrollItem.index];

            if (data.prefab_index == 0)
            {
                UI_Item_GuildTerrtroyTitleView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildTerrtroyTitleView>(scrollItem.go);


                if (itemView != null)
                {
                    itemView.m_lbl_name_LanguageText.text = data.HeadTitle;

                    ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, data.HeadIcon);


                    if (data.max > 0)
                    {
                        itemView.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(181104, data.num, data.max);

                    }
                    else
                    {
                        itemView.m_lbl_count_LanguageText.text = data.num.ToString();
                    }


                    itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                    itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);


                    var index = scrollItem.index;

                    itemView.m_btn_btn_GameButton.onClick.RemoveAllListeners();

                    itemView.m_btn_btn_GameButton.onClick.AddListener(() =>
                        {
                            data.isSelected = !data.isSelected;

                            itemView.m_img_arrow_down_PolygonImage.gameObject.SetActive(!data.isSelected);
                            itemView.m_img_arrow_up_PolygonImage.gameObject.SetActive(data.isSelected);

                            if (data.isSelected)
                            {
                                AddMember(data.buildType, index, data);
                                view.m_sv_list_ListView.FillContent(m_buildTypesList.Count);

                                //将展开的item 移动到list可视区域
                                ListView.ListItem listItem1 = view.m_sv_list_ListView.GetItemByIndex(scrollItem.index);
                                if (listItem1 != null && listItem1.go != null)
                                {
                                    RectTransform goRect = listItem1.go.GetComponent<RectTransform>();
                                    RectTransform contentRect = view.m_sv_list_ListView.listContainer;
                                    RectTransform listRect = view.m_sv_list_ListView.GetComponent<RectTransform>();
                                    float listSize = listRect.sizeDelta.y;
                                    Vector3 goPos = goRect.position;
                                    float listPos = listRect.InverseTransformPoint(goPos).y;
                                    float tileSize = 100f;
                                    float detailSize = 360f;
                                    if (data.buildType == 3) //资源
                                    {
                                        detailSize = 290f;
                                    }
                                    float bottom = listPos - (tileSize + detailSize);
                                    if (bottom < -listSize)
                                    {
                                        var pos = contentRect.anchoredPosition;
                                        pos.y = pos.y + (-listSize - bottom);
                                        view.m_sv_list_ListView.MovePanelToPos(pos.y);
                                    }

                                    ShowGuide(data.buildType, scrollItem.index + 1);
                                }

                            }
                            else
                            {
                                RemoveMember(data.buildType, index);
                                view.m_sv_list_ListView.FillContent(m_buildTypesList.Count);
                            }
                        }
                    );

                    switch (data.buildType)
                    {
                        case 0:
                            GuildBuildInfoEntity buidInfo1 = m_allianceProxy.GetFortressesByType(1);
                            GuildBuildInfoEntity buidInfo2 = m_allianceProxy.GetFortressesByType(2);
                            GuildBuildInfoEntity buidInfo3 = m_allianceProxy.GetFortressesByType(12);
                            itemView.m_img_build_ArabLayoutCompment.gameObject.SetActive(buidInfo1 != null && buidInfo1.status == (int)GuildBuildState.building
                                                                                         || buidInfo2 != null && buidInfo2.status == (int)GuildBuildState.building ||
                                                                                         buidInfo3 != null && buidInfo3.status == (int)GuildBuildState.building);
                            break;
                        case 1:
                            GuildBuildInfo buidResInfo1 = m_allianceProxy.GetResCenter().resourceCenter;
                            itemView.m_img_build_ArabLayoutCompment.gameObject.SetActive(buidResInfo1 != null && buidResInfo1.status == (int)GuildBuildState.building
                                                                                         );
                            break;
                        case 2:
                            bool isBuild = false;


                            data.TagFlagData.Values.ToList().ForEach((build) =>
                            {
                                if (build.status == (int)GuildBuildState.building)
                                {
                                    isBuild = true;
                                }
                            });

                            itemView.m_img_build_ArabLayoutCompment.gameObject.SetActive(isBuild);
                            break;
                        default:
                            itemView.m_img_build_ArabLayoutCompment.gameObject.SetActive(false);
                            break;
                    }

                }
            } else if (data.prefab_index == 1)
            {
                //资源点
                UI_Item_GuildTerrtroyResView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildTerrtroyResView>(scrollItem.go);

                itemView.m_lbl_text_LanguageText.text = LanguageUtils.getText(732037);

                if (itemView != null)
                {

                    var resPointConfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(ResPointIDs[0]);
                    itemView.m_UI_Item_GuildDepotRes1.m_lbl_timeNum_LanguageText.text =
                        LanguageUtils.getTextFormat(732083, ClientUtils.FormatComma(m_resPoint.foodPoint * resPointConfig.holdAllianceSpeed));
                    var resPointConfig2 = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(ResPointIDs[1]);
                    itemView.m_UI_Item_GuildDepotRes2.m_lbl_timeNum_LanguageText.text =
                        LanguageUtils.getTextFormat(732083, ClientUtils.FormatComma(m_resPoint.woodPoint * resPointConfig2.holdAllianceSpeed));
                    var resPointConfig3 = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(ResPointIDs[2]);
                    itemView.m_UI_Item_GuildDepotRes3.m_lbl_timeNum_LanguageText.text =
                        LanguageUtils.getTextFormat(732083, ClientUtils.FormatComma(m_resPoint.stonePoint * resPointConfig3.holdAllianceSpeed));
                    var resPointConfig4 = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(ResPointIDs[3]);
                    itemView.m_UI_Item_GuildDepotRes4.m_lbl_timeNum_LanguageText.text =
                        LanguageUtils.getTextFormat(732083, ClientUtils.FormatComma(m_resPoint.goldPoint * resPointConfig4.holdAllianceSpeed));

                }
            }
            else
            {
                //建筑旗帜
                UI_Item_GuildTerrtroyBuildingView itemView =
                    MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildTerrtroyBuildingView>(scrollItem.go);

                if (itemView != null)
                {


                    subViews[0] = itemView.m_UI_Item_build1;
                    subViews[1] = itemView.m_UI_Item_build2;
                    subViews[2] = itemView.m_UI_Item_build3;
                    subViews[3] = itemView.m_UI_Item_build4;
                    subViews[4] = itemView.m_UI_Item_build5;


                    itemView.m_lbl_text_LanguageText.text = "";

                    int startIndex = 0;

                    if (data.subItemRow == 0 && data.buildType == 2)
                    {
                        itemView.m_lbl_text_LanguageText.text = LanguageUtils.getText(732004);

                        startIndex = 1;//旗帜第一个特殊处理
                        itemView.m_lbl_text_LanguageText.text = LanguageUtils.getText(RS.AllianceTerrtriyHeadTitle[data.buildType]);
                        subViews[0].gameObject.SetActive(true);
                        subViews[0].setData(data.buildType, data.LockList[0], m_allianceProxy, data.subItemRow, 0);
                    }


                    if (data.subItemRow == 0 && data.buildType == 1)
                    {
                        itemView.m_lbl_text_LanguageText.text = LanguageUtils.getText(732092);
                    }

                    if (data.subItemRow == 0 && data.buildType == 0)
                    {
                        itemView.m_lbl_text_LanguageText.text = LanguageUtils.getText(732002);
                    }

                    for (int col = startIndex; col < subViews.Length; col++)
                    {
                        var subView = subViews[col];
                        if (data.buildType == 2)
                        {
                            GuildBuildInfo subData = null;
                            subData = data.subFlagData.Count - 1 >= col - startIndex ? data.subFlagData[col - startIndex] : null;
                            subView.gameObject.SetActive(subData != null);

                            if (subData != null && data != null)
                            {
                                subView.setData(data.buildType, data.LockList[0], m_allianceProxy, data.subItemRow, col,
                                    subData);
                            }
                        }
                        else
                        {

                            var subData = data.subItemsData.Count - 1 >= col ? data.subItemsData[col] : null;
                            subView.gameObject.SetActive(subData != null);
                            //资源中心
                            if (subData != null && data != null)
                            {
                                subView.setData(data.buildType, subData, m_allianceProxy, data.subItemRow, col,
                                    null);
                            }
                        }

                    }
                    itemView.m_lbl_text_LanguageText.gameObject.SetActive(data.subItemRow == 0);
                }
            }
        }

        public void AddMember(int buildType, int itemRow, AllianceBuildTypeTag tag)
        {
            if (buildType != 2)
            {
                //建筑 资源点
                int len = tag.LockList.Count;

                AllianceBuildTypeTag buildLineData = new AllianceBuildTypeTag();

                if (buildType == 3)
                {
                    buildLineData.prefab_index = 1;//资源点
                }
                else
                {
                    buildLineData.prefab_index = 2;//建筑
                }

                buildLineData.buildType = buildType;

                buildLineData.subItemsData = new List<AllianceBuildingTypeDefine>();

                buildLineData.subItemRow = 0;
                buildLineData.LockList = tag.LockList;

                for (int i = 0; i < tag.LockList.Count; i++)
                {
                    buildLineData.subItemsData.Add(tag.LockList[i]);
                }

                m_buildTypesList.Insert(itemRow + 1, buildLineData);
            }
            else
            {
                //旗帜
                int len = tag.TagFlagData.Count + 1;
                var flagAarry = tag.TagFlagData.Values.ToList();

                int subItemRow = 0;
                for (int i = 0; i < len; i = i + 4)
                {
                    AllianceBuildTypeTag buildLineData = new AllianceBuildTypeTag();

                    buildLineData.prefab_index = 2;
                    buildLineData.buildType = buildType;
                    buildLineData.subItemRow = subItemRow;
                    buildLineData.LockList = tag.LockList;


                    int startIndex = 0;
                    if (subItemRow == 0)
                    {
                        startIndex = 1;//旗帜第一个特殊处理
                        buildLineData.subItemsData = new List<AllianceBuildingTypeDefine>();
                        buildLineData.subItemsData.Add(tag.LockList[0]);
                    }

                    buildLineData.subFlagData = new List<GuildBuildInfo>();



                    for (int j = i; j < i + 4 - startIndex; j++)
                    {

                        if (j < len - startIndex)
                        {
                            int off = startIndex == 1 ? 0 : -1;
                            //Debug.Log(i+" 旗帜 "+(j+off)+"  "+(len-startIndex));
                            buildLineData.subFlagData.Add(flagAarry[j + off]);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (buildLineData.subFlagData.Count > 0 || startIndex == 1)
                    {
                        m_buildTypesList.Insert(itemRow + 1, buildLineData);
                    }

                    subItemRow++;
                    itemRow++;
                }
            }


        }

        public void RemoveMember(int grade, int index)
        {
            int startIndex = 0;

            int count = 0;

            int len = m_buildTypesList.Count;

            for (int i = 0; i < len; i++)
            {
                var item = m_buildTypesList[i];

                if (item.buildType == grade)
                {
                    if (startIndex == 0)
                    {
                        startIndex = i + 1;
                    }
                    else
                    {
                        count = count + 1;
                    }

                }
            }
            m_buildTypesList.RemoveRange(startIndex, count);
        }




        private string GetItemPrefabName(ListView.ListItem item)
        {
            var data = m_buildTypesList[item.index];

            return view.m_sv_list_ListView.ItemPrefabDataList[data.prefab_index];
        }

        private float GetItemSize(ListView.ListItem item)
        {
            var data = m_buildTypesList[item.index];

            if (data.prefab_index == 0)
            {
                return 86f;
            }
            else if (data.prefab_index == 1)
            {
                return 290;
            }

            return data.subItemRow == 0 ? 370f : 320f;
        }

        public void InitReList()
        {
            if (m_assetDic.Count < 1 || !m_isGetData)
            {
                return;
            }
            if (view.data != null && !m_isFuncGuide)
            {
                FindJumpProcess();
            }
            else
            {
                ReList();
            }
        }

        private void FindJumpProcess()
        {
            //是否有创建权限
            if (!m_allianceProxy.GetSelfRoot(GuildRoot.createBuild))
            {
                ReList();
                return;
            }

            bool isFind = false;
            int index1 = 0;
            int index2 = 0;
            //判断一下是否需要跳转
            var initdatas = m_allianceProxy.GetTerritoryUIList();
            for (int i = 0; i < initdatas.Count; i++)
            {
                if (i < 3)
                {
                    if (i == 2) //旗帜
                    {
                        if (m_allianceProxy.IsCanBuild(initdatas[i].LockList[0]))
                        {
                            index1 = i;
                            index2 = 0;
                            isFind = true;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < initdatas[i].LockList.Count; k++)
                        {
                            if (m_allianceProxy.IsCanBuild(initdatas[i].LockList[k]))
                            {
                                index1 = i;
                                index2 = k;
                                isFind = true;
                                break;
                            }
                        }
                    }
                }
                if (isFind)
                {
                    break;
                }
            }
            if (isFind)
            {
                var datas = m_allianceProxy.GetTerritoryUIList();
                m_buildTypesList.Clear();
                m_buildTypesList.AddRange(datas);

                if (index1 < m_buildTypesList.Count)
                {
                    m_buildTypesList[index1].isSelected = true;
                }

                int count = 0;
                for (int i = 0; i < m_buildTypesList.Count; i++)
                {
                    if (m_buildTypesList[i].buildType == index1)
                    {
                        break;
                    }
                    else
                    {
                        count = count + 1;
                        if (m_buildTypesList[i].isSelected)
                        {
                            count = count + 1;
                        }
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < m_buildTypesList.Count; j++)
                    {
                        var level = m_buildTypesList[j];

                        if (level.buildType == i && level.isSelected)
                        {
                            AddMember(i, j, level);
                        }
                    }
                }

                view.m_sv_list_ListView.FillContent(m_buildTypesList.Count);
                view.m_sv_list_ListView.MovePanelToItemIndex(count);

                Timer.Register(0.1f, ()=> {
                    if (view.gameObject == null)
                    {
                        return;
                    }
                    ShowGuide2(count + 1, index2);
                });
            }
            else
            {
                ReList();
            }
        }

        public void ReList()
        {
            if (!m_isGetData)
            {
                return;
            }

            if (m_assetDic.Count>0)
            { 
                var initdatas = m_allianceProxy.GetTerritoryUIList();           
                m_buildTypesList.Clear();
                m_buildTypesList.AddRange(initdatas);                
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < m_buildTypesList.Count; j++)
                    {
                        var level = m_buildTypesList[j];

                        if (level.buildType == i && level.isSelected)
                        {
                            AddMember(i,j,level);
                        }
                    }
                }

                view.m_sv_list_ListView.FillContent(m_buildTypesList.Count);
                JudgeGuideArrowIsCancel();
            }
        }

        private void JudgeGuideArrowIsCancel()
        {
            //判断是否取消引导箭头
            if (m_isShowGuideArrow)
            {
                bool isCancelGuideArrow = true;
                if (m_findIndex >= 0 && m_findIndex < m_buildTypesList.Count)
                {
                    if (m_buildTypesList[m_findIndex].LockList != null && m_findSubIndex < m_buildTypesList[m_findIndex].LockList.Count)
                    {
                        if (m_allianceProxy.IsCanBuild(m_buildTypesList[m_findIndex].LockList[m_findSubIndex], true))
                        {
                            Debug.LogError("不取消");
                            isCancelGuideArrow = false;
                        }
                    }
                }
                if (isCancelGuideArrow)
                {
                    m_isShowGuideArrow = false;
                    AppFacade.GetInstance().SendNotification(CmdConstant.CancelGuideRemind, 2);
                }
            }
        }


        #endregion

        //显示引导标致
        private UI_Item_GuildTerrtroyBuildingSingle_SubView[] m_guideSubViews = new UI_Item_GuildTerrtroyBuildingSingle_SubView[5];
        private void ShowGuide(int buildType, int index)
        {
            if (buildType == 3)//联盟资源点 不可采集
            {
                return;
            }
            if (index >= m_buildTypesList.Count)
            {
                return;
            }
            var data = m_buildTypesList[index];
            ListView.ListItem listItem = view.m_sv_list_ListView.GetItemByIndex(index);
            if (listItem == null || listItem.go == null)
            {
                return;
            }
            UI_Item_GuildTerrtroyBuildingView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildTerrtroyBuildingView>(listItem.go);

            //是否有创建权限
            if (!m_allianceProxy.GetSelfRoot(GuildRoot.createBuild))
            {
                return;
            }

            m_guideSubViews[0] = itemView.m_UI_Item_build1;
            m_guideSubViews[1] = itemView.m_UI_Item_build2;
            m_guideSubViews[2] = itemView.m_UI_Item_build3;
            m_guideSubViews[3] = itemView.m_UI_Item_build4;
            m_guideSubViews[4] = itemView.m_UI_Item_build5;

            int findIndex = -1;
            if (buildType == 0)//联盟要塞
            {
                for (int i = 0; i < m_guideSubViews.Length; i++)
                {
                    if (i < data.subItemsData.Count)
                    {
                        if (m_allianceProxy.IsCanBuild(data.subItemsData[i]))
                        {
                            findIndex = i;
                            break;
                        }
                    }
                }
            }
            else if (buildType == 1)//联盟资源场
            {
                for (int i = 0; i < m_guideSubViews.Length; i++)
                {
                    if (i < data.subItemsData.Count)
                    {
                        if (m_allianceProxy.IsCanBuild(data.subItemsData[i]))
                        {
                            findIndex = i;
                            break;
                        }
                    }
                }
            }
            else if (buildType == 2) //联盟旗帜
            {
                if (m_allianceProxy.IsCanBuild(data.LockList[0]))
                {
                    findIndex = 0;
                }
            }

            if (findIndex < 0)
            {
                return;
            }

            var nodeView = m_guideSubViews[findIndex];
            if (nodeView.m_pl_build.gameObject.activeSelf && nodeView.m_btn_build.gameObject.activeSelf)
            {
                FingerTargetParam param = new FingerTargetParam();
                param.AreaTarget = nodeView.m_btn_build.gameObject;
                param.IsTouchBeginClose = true;
                param.ArrowDirection = (int)EnumArrorDirection.Up;
                param.EffectMountTarget = nodeView.m_btn_build.gameObject;

                param.IsAutoClose = true;
                param.IsUseDefaultAutoCloseTime = false;
                param.AutoCloseTime = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).allianceBuildCheckEffectTime;
                CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
            }
        }

        private void ShowGuide2(int index1, int index2)
        {
            if (index1 >= m_buildTypesList.Count)
            {
                return;
            }
            var data = m_buildTypesList[index1];
            if (data.prefab_index != 2)
            {
                return;
            }
            ListView.ListItem listItem = view.m_sv_list_ListView.GetItemByIndex(index1);
            if (listItem == null || listItem.go == null)
            {
                return;
            }
            UI_Item_GuildTerrtroyBuildingView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildTerrtroyBuildingView>(listItem.go);

            m_guideSubViews[0] = itemView.m_UI_Item_build1;
            m_guideSubViews[1] = itemView.m_UI_Item_build2;
            m_guideSubViews[2] = itemView.m_UI_Item_build3;
            m_guideSubViews[3] = itemView.m_UI_Item_build4;
            m_guideSubViews[4] = itemView.m_UI_Item_build5;

            if (index2 >= m_guideSubViews.Length)
            {
                return;
            }
            var nodeView = m_guideSubViews[index2];

            if (nodeView.m_pl_build.gameObject.activeSelf && nodeView.m_btn_build.gameObject.activeSelf)
            {
                if (index2 < data.LockList.Count && m_allianceProxy.IsCanBuild(data.LockList[index2], true))
                {
                    m_isShowGuideArrow = true;
                    m_findIndex = index1;
                    m_findSubIndex = index2;

                    //将from转换到屏幕坐标
                    Vector2 V2fromInScreen = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(nodeView.m_btn_build.gameObject.transform.position);
                    //将屏幕坐标转换到at的局部坐标中
                    Vector2 V2InAt;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(view.gameObject.GetComponent<RectTransform>(),
                                                                            V2fromInScreen,
                                                                            CoreUtils.uiManager.GetUICamera(),
                                                                            out V2InAt);

                    view.m_pl_guideNode.transform.localPosition = V2InAt;

                    FingerTargetParam param = new FingerTargetParam();
                    param.AreaTarget = view.m_pl_guideNode.gameObject;
                    param.IsTouchBeginClose = true;
                    param.ArrowDirection = (int)EnumArrorDirection.Up;
                    param.SourceType = 2;
                    param.EffectMountTarget = view.m_pl_guideNode.gameObject;

                    param.IsAutoClose = true;
                    param.IsUseDefaultAutoCloseTime = false;
                    param.AutoCloseTime = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).allianceBuildCheckEffectTime;
                    CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
                }
            }
        }

        public static bool IsTouchBegin()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
            return false;
        }
    }
}
// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年6月28日
// Update Time         :    2020年6月28日
// Class Description   :    UI_Item_MailFightReport_SubView 邮件 战斗报告
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System.Collections.Generic;
using SprotoType;
using System;

namespace Game {
    public class FightReportItemData
    {
        public int ItemType;//1标题 2资源 3击杀掉落 4部队信息 5折线图 6敌方攻击时间进度 7战斗详情  
        public float ItemHeight = -1;
    }

    public class FightReportReinforceData
    {
        public int ReinforceType; //援助类型 1加入 2撤离
        public BattleReinforceArmy ReinforceInfo;
    }

    public class ReinforecePercent
    {
        public int StartRound;
        public float StartPercent;
        public int EndRound;
        public float EndPercent;
        public int CurrRound;
    }

    public partial class UI_Item_MailFightReport_SubView : UI_SubView
    {
        private EmailInfoEntity m_currentBattleReport;
        private MailDefine m_currentMailDefine;

        private EmailProxy m_emailProxy;
        private PlayerProxy m_playerProxy;
        private Color FormulaColor = Color.white;
        private bool m_isInit;

        private bool m_isInitList;
        private int m_prefabLoadStatus = 1; //1未加载 2加载中 3已加载
        private Dictionary<string, GameObject> m_assetDic;
        private Dictionary<int, float> m_itemHeightDic = new Dictionary<int, float>();
        private List<FightReportItemData> m_itemList = new List<FightReportItemData>();
        public string m_lastEmailTitleImgBg = string.Empty;

        private Int64 m_selfRid; //己方rid
        private Int64 m_guildId; //己方公会id

        private BattleReportExDetail m_selfBattleDetail;
        private List<BattleReportExDetail> m_otherReportList = new List<BattleReportExDetail>();

        private int m_timeBarStartIndex;
        private int m_fightDetailStartIndex;
        private string m_questionStr = "????";

        private Dictionary<long, List<FightReportReinforceData>> m_reinforceDataDic; //援助信息
        private Dictionary<long, float> m_roundPercentDic = new Dictionary<long, float>();
        private Dictionary<long, float> m_reinforcePercentDic;

        private List<BattleReport> m_battleReportList;

        private BattleReportEx m_battleReportEx;

        public void Refresh(MailDefine mailDefine, EmailInfoEntity mailInfo)
        {
            if (!m_isInit)
            {
                ColorUtility.TryParseHtmlString("#019ABF", out FormulaColor);
                m_emailProxy = AppFacade.GetInstance().RetrieveProxy(EmailProxy.ProxyNAME) as EmailProxy;
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            }

            bool isShowContent = true;

            if (mailInfo.reportStatus == 0)
            {
                isShowContent = false;
            }
            else if (mailInfo.reportStatus == 1 && !m_emailProxy.IsGetBattleReportEx(mailInfo.emailIndex))
            {
                isShowContent = false;
            }
            else if (mailInfo.reportStatus == 2 && !m_emailProxy.IsGetBattleReportEx(mailInfo.emailIndex))
            {
                isShowContent = false;
            }

            if (!isShowContent)
            {
                if (m_isInitList)
                {
                    m_img_loading_PolygonImage.gameObject.SetActive(true);
                    m_sv_fightReport_ListView.RefreshAndRestPos(0);
                    m_sv_fightReport_ListView.FillContent(0);
                }
                else
                {
                    m_img_loading_PolygonImage.gameObject.SetActive(true);
                }
                return;
            }
            m_battleReportEx = m_emailProxy.GetBattleReportEx(mailInfo.emailIndex);

            if (m_battleReportEx == null)
            {
                Debug.LogFormat("战报数据为空 emailIndex:{0}", mailInfo.emailIndex);
                if (m_isInitList)
                {
                    m_img_loading_PolygonImage.gameObject.SetActive(false);
                    m_sv_fightReport_ListView.RefreshAndRestPos(0);
                    m_sv_fightReport_ListView.FillContent(0);
                }
                return;
            }

            m_roundPercentDic.Clear();
            m_reinforcePercentDic = null;
            m_img_loading_PolygonImage.gameObject.SetActive(false);
            Debug.LogFormat("emailId:{0} group:{1}", mailDefine.ID, mailDefine.group);
            m_currentMailDefine = mailDefine;
            m_currentBattleReport = mailInfo;

            if (mailInfo.reportSubTile == null || mailInfo.reportSubTile.Count < 1)
            {
                Debug.LogError("mailInfo.reportSubTile 数据异常");
                return;
            }
            if (m_battleReportEx.objectInfos == null)
            {
                return;
            }
            if (m_battleReportEx.objectInfos.Count < 1)
            {
                return;
            }

            //处理数据
            ProcessData();

            //显示列表
            if (m_prefabLoadStatus == 1)
            {
                m_prefabLoadStatus = 2;
                ClientUtils.PreLoadRes(gameObject, m_sv_fightReport_ListView.ItemPrefabDataList, OnLoadFinish);
            }
            else if (m_prefabLoadStatus == 3)
            {
                RefreshList();
            }
        }

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_assetDic = asset;
            m_prefabLoadStatus = 3;

            m_itemHeightDic[1] = m_assetDic["UI_Item_MailTitle"].GetComponent<RectTransform>().rect.height;
            m_itemHeightDic[2] = m_assetDic["UI_Item_MailWarRes"].GetComponent<RectTransform>().rect.height;
            m_itemHeightDic[3] = m_assetDic["UI_Item_MailWarKill"].GetComponent<RectTransform>().rect.height;
            m_itemHeightDic[4] = m_assetDic["UI_Item_MailWarSelf"].GetComponent<RectTransform>().rect.height;
            m_itemHeightDic[5] = m_assetDic["UI_Item_MailWarCoordinate"].GetComponent<RectTransform>().rect.height;
            m_itemHeightDic[6] = m_assetDic["UI_Item_WarMailBar"].GetComponent<RectTransform>().rect.height;
            m_itemHeightDic[7] = m_assetDic["UI_Item_WarMailWar"].GetComponent<RectTransform>().rect.height;

            RefreshList();
        }

        private void RefreshList()
        {
            if (!m_isInitList)
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ListViewItemByIndex;
                funcTab.GetItemSize = OnGetItemSize;
                funcTab.GetItemPrefabName = GetItemPrefabName;
                m_sv_fightReport_ListView.SetInitData(m_assetDic, funcTab);
                m_isInitList = true;
            }
            m_sv_fightReport_ListView.RefreshAndRestPos(0);
            m_sv_fightReport_ListView.FillContent(m_itemList.Count);
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            int itemType = m_itemList[listItem.index].ItemType;
            return m_itemHeightDic[itemType];
        }

        private string GetItemPrefabName(ListView.ListItem listItem)
        {
            int itemType = m_itemList[listItem.index].ItemType;
            string name = "";
            switch (itemType)
            {
                case 1:
                    name = "UI_Item_MailTitle";
                    break;
                case 2:
                    name = "UI_Item_MailWarRes";
                    break;
                case 3:
                    name = "UI_Item_MailWarKill";
                    break;
                case 4:
                    name = "UI_Item_MailWarSelf";
                    break;
                case 5:
                    name = "UI_Item_MailWarCoordinate";
                    break;
                case 6:
                    name = "UI_Item_WarMailBar";
                    break;
                case 7:
                    name = "UI_Item_WarMailWar";
                    break;
            }
            return name;
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            FightReportItemData itemData = m_itemList[listItem.index];
            if (listItem.data == null)
            {
                if (itemData.ItemType == 1)
                {
                    UI_Item_MailTitle_SubView subView = new UI_Item_MailTitle_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshTitle(subView);
                }
                else if(itemData.ItemType == 2)
                {
                    UI_Item_MailWarRes_SubView subView = new UI_Item_MailWarRes_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshRes(subView);
                }
                else if (itemData.ItemType == 3)
                {
                    UI_Item_MailWarKill_SubView subView = new UI_Item_MailWarKill_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshKillDrop(subView);
                }
                else if (itemData.ItemType == 4)
                {
                    UI_Item_MailWarSelf_SubView subView = new UI_Item_MailWarSelf_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshTroopInfo(subView);
                }
                else if (itemData.ItemType == 5)
                {
                    UI_Item_MailWarCoordinate_SubView subView = new UI_Item_MailWarCoordinate_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshBrokenLine(subView);
                }
                else if (itemData.ItemType == 6)
                {
                    UI_Item_WarMailBar_SubView subView = new UI_Item_WarMailBar_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshTimeBar(listItem, subView);
                }
                else if (itemData.ItemType == 7)
                {
                    UI_Item_WarMailWar_SubView subView = new UI_Item_WarMailWar_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshFightDetail(listItem, subView);

                    float newHeight = listItem.go.GetComponent<RectTransform>().rect.height;
                    if (m_itemList[listItem.index].ItemHeight != newHeight)
                    {
                        m_itemList[listItem.index].ItemHeight = newHeight;
                        m_sv_fightReport_ListView.RefreshItem(listItem.index);
                    }
                }
            }
            else
            {
                if (itemData.ItemType == 1)
                {
                    UI_Item_MailTitle_SubView subView = listItem.data as UI_Item_MailTitle_SubView;
                    listItem.data = subView;
                    RefreshTitle(subView);
                }
                else if (itemData.ItemType == 2)
                {
                    UI_Item_MailWarRes_SubView subView = listItem.data as UI_Item_MailWarRes_SubView;
                    listItem.data = subView;
                    RefreshRes(subView);
                }
                else if (itemData.ItemType == 3)
                {
                    UI_Item_MailWarKill_SubView subView = new UI_Item_MailWarKill_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshKillDrop(subView);
                }
                else if (itemData.ItemType == 4)
                {
                    UI_Item_MailWarSelf_SubView subView = new UI_Item_MailWarSelf_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshTroopInfo(subView);
                }
                else if (itemData.ItemType == 5)
                {
                    UI_Item_MailWarCoordinate_SubView subView = new UI_Item_MailWarCoordinate_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshBrokenLine(subView);
                }
                else if (itemData.ItemType == 6)
                {
                    UI_Item_WarMailBar_SubView subView = new UI_Item_WarMailBar_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshTimeBar(listItem, subView);
                }
                else if (itemData.ItemType == 7)
                {
                    UI_Item_WarMailWar_SubView subView = new UI_Item_WarMailWar_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    RefreshFightDetail(listItem, subView);

                    float newHeight = listItem.go.GetComponent<RectTransform>().rect.height;
                    if (m_itemList[listItem.index].ItemHeight != newHeight)
                    {
                        m_itemList[listItem.index].ItemHeight = newHeight;
                        m_sv_fightReport_ListView.RefreshItem(listItem.index);
                    }
                }
            }
        }

        //刷新标题
        private void RefreshTitle(UI_Item_MailTitle_SubView subView)
        {
            //标题背景
            if (!string.IsNullOrEmpty(m_currentMailDefine.poster))
            {
                if (m_lastEmailTitleImgBg != m_currentMailDefine.poster)
                {
                    m_lastEmailTitleImgBg = m_currentMailDefine.poster;
                    ClientUtils.LoadSprite(subView.m_img_bg_PolygonImage, m_currentMailDefine.poster, false, () =>
                    {
                        if (gameObject != null)
                        {
                            subView.m_img_bg_PolygonImage.gameObject.SetActive(true);
                        }
                    });
                }
            }
            else
            {
                subView.m_img_bg_PolygonImage.gameObject.SetActive(false);
            }

            //标题
            subView.m_lbl_title_LanguageText.text = m_emailProxy.OnTextFormat(LanguageUtils.getText(m_currentMailDefine.l_nameID), m_currentBattleReport.titleContents);
            subView.m_lbl_desc_LanguageText.text = m_emailProxy.FormatBattleReportSubTitle(m_currentMailDefine.ID, LanguageUtils.getText(m_currentMailDefine.l_subheadingID), m_currentBattleReport.reportSubTile);
            subView.m_lbl_time_LanguageText.text = UIHelper.GetServerLongTimeFormat(m_currentBattleReport.sendTime);
        }

        #region 刷新资源

        //刷新资源
        private void RefreshRes(UI_Item_MailWarRes_SubView subView)
        {
            RewardInfo rewardInfo = m_battleReportEx.rewardInfo;
            if (rewardInfo == null)
            {
                return;
            }
            if (rewardInfo.food !=0 || rewardInfo.wood !=0 || rewardInfo.stone !=0 || rewardInfo.gold !=0)
            {  
                subView.m_UI_Model_ResourcesFood.gameObject.SetActive(true);
                subView.m_UI_Model_ResourcesFood.m_lbl_languageText_LanguageText.text = FormatResStr(rewardInfo.food);
                subView.m_UI_Model_ResourcesWood.gameObject.SetActive(true);
                subView.m_UI_Model_ResourcesWood.m_lbl_languageText_LanguageText.text = FormatResStr(rewardInfo.wood);
                subView.m_UI_Model_ResourcesStone.gameObject.SetActive(true);
                subView.m_UI_Model_ResourcesStone.m_lbl_languageText_LanguageText.text = FormatResStr(rewardInfo.stone);
                subView.m_UI_Model_ResourcesGold.gameObject.SetActive(true);
                subView.m_UI_Model_ResourcesGold.m_lbl_languageText_LanguageText.text = FormatResStr(rewardInfo.gold);
            }
        }

        private string FormatResStr(Int64 num)
        {
            if (num > 0)
            {
                return LanguageUtils.getTextFormat(300317, ClientUtils.CurrencyFormat(num));
            }
            else if (num < 0)
            {
                return LanguageUtils.getTextFormat(300318, ClientUtils.CurrencyFormat(Mathf.Abs((int)num)));
            }
            else
            {
                return LanguageUtils.getText(805019);
            }
        }

        #endregion

        #region 刷新击杀掉落

        //刷新击杀掉落
        private void RefreshKillDrop(UI_Item_MailWarKill_SubView subView)
        {
            RewardInfo rewardInfo = m_battleReportEx.rewardInfo;
            //显示击杀获得物品
            if (rewardInfo != null && rewardInfo.items != null && rewardInfo.items.Count > 0)
            {
                OnBattleReward(subView.m_pl_getreward_GridLayoutGroup.transform);
            }
        }

        //掉落奖励
        private void OnBattleReward(Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
            }
            for (int i = 0; i < m_battleReportEx.rewardInfo.items.Count; i++)
            {
                GameObject go = CoreUtils.assetService.Instantiate(m_assetDic["UI_Item_ItemSize65"]);
                go.transform.SetParent(parent);
                go.transform.localScale = Vector3.one;
                UI_Item_ItemSize65View giftView = MonoHelper.AddHotFixViewComponent<UI_Item_ItemSize65View>(go);
                ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>((int)m_battleReportEx.rewardInfo.items[i].itemId);
                giftView.m_UI_Model_Item.m_img_select_PolygonImage.gameObject.SetActive(false);
                giftView.m_UI_Model_Item.m_lbl_count_LanguageText.text = m_battleReportEx.rewardInfo.items[i].itemNum.ToString("N0");
                giftView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
                giftView.m_UI_Model_Item.m_img_quality_PolygonImage.gameObject.SetActive(true);
                giftView.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.RemoveAllListeners();
                giftView.m_UI_Model_Item.m_btn_animButton_GameButton.onClick.AddListener(() =>
                {
                    HelpTip.CreateTip(LanguageUtils.getText(itemDefine.l_nameID), go.transform).SetOffset(go.GetComponent<RectTransform>().rect.height / 2* 0.5f).Show();
                });
                //设置icon
                ClientUtils.LoadSprite(giftView.m_UI_Model_Item.m_img_icon_PolygonImage, itemDefine.itemIcon);
                ClientUtils.LoadSprite(giftView.m_UI_Model_Item.m_img_quality_PolygonImage, RS.ItemQualityBg[itemDefine.quality - 1]);

                if (itemDefine.l_topID >= 1)
                {
                    giftView.m_UI_Model_Item.m_pl_desc_bg_PolygonImage.gameObject.SetActive(true);
                    giftView.m_UI_Model_Item.m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), itemDefine.topData);
                }
            }
        }

        #endregion

        //刷新部队信息
        private void RefreshTroopInfo(UI_Item_MailWarSelf_SubView subView)
        {
            BattleReportExDetail selfBattleDetail = m_selfBattleDetail;
            subView.m_lbl_selfname_LanguageText.text = string.IsNullOrEmpty(selfBattleDetail.guildName)? selfBattleDetail.name : LanguageUtils.getTextFormat(300030, selfBattleDetail.guildName, selfBattleDetail.name);
            //CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)selfBattleDetail.headId);
            //ClientUtils.LoadSprite(subView.m_UI_Model_PlayerHead.m_UI_Model_PlayerHead_PolygonImage, define.playerImg);
            subView.m_UI_Model_PlayerHead.LoadPlayerIcon(selfBattleDetail.headId);
            subView.m_lbl_armyTotal_LanguageText.text = string.Format(LanguageUtils.getText(570024), selfBattleDetail.beginArmyCount.ToString("N0"));
            subView.m_lbl_armyLast_LanguageText.text = string.Format(LanguageUtils.getText(570026), selfBattleDetail.endArmyCount.ToString("N0"));
        }

        #region 折线图 

        //刷新折线图
        private void RefreshBrokenLine(UI_Item_MailWarCoordinate_SubView subView)
        {
            BattleReportExDetail selfBattleDetail = m_selfBattleDetail;

            //考虑到增援情况 获取折线图的最高点
            long maxArmyCount = selfBattleDetail.maxArmyCount;

            //统帅头像
            if (selfBattleDetail.mainHeroId > 0)
            {
                HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>((int)selfBattleDetail.mainHeroId);
                subView.m_lbl_coordinate_LanguageText.text = LanguageUtils.getTextFormat(300015, selfBattleDetail.mainHeroLevel, LanguageUtils.getText(hero.l_nameID));
                subView.m_UI_Model_CaptainHead.m_img_char_PolygonImage.gameObject.SetActive(true);
                subView.m_UI_Model_CaptainHead.SetRare(hero.rare);
                subView.m_UI_Model_CaptainHead.SetIcon(hero.heroIcon);
            }
            else
            {
                subView.m_UI_Model_CaptainHead.SetEmpty();
                subView.m_UI_Model_CaptainHead.SetRare(1);
                subView.m_lbl_coordinate_LanguageText.text = LanguageUtils.getText(570029);
            }

            subView.m_lbl_chartnum3_LanguageText.text = maxArmyCount.ToString("N0");
            subView.m_lbl_chartnum2_LanguageText.text = Mathf.RoundToInt((maxArmyCount * 2.0f / 3.0f)).ToString("N0");
            subView.m_lbl_chartnum1_LanguageText.text = Mathf.RoundToInt((maxArmyCount / 3.0f)).ToString("N0");
            subView.m_lbl_chartnum0_LanguageText.text = 0.ToString();

            subView.m_lbl_starttime_LanguageText.text = UIHelper.GetShortTimeFormat(m_battleReportEx.battleBeginTime);
            subView.m_lbl_endtime_LanguageText.text = UIHelper.GetShortTimeFormat(m_battleReportEx.battleEndTime);

            //折线图
            List<FunctionFormula> formulas = GetFormulas(selfBattleDetail);
            subView.m_img_polylinechart_PolyLineChart.Formulas = formulas;
            subView.m_img_polylinechart_PolyLineChart.SetAllDirty();

            //折线图的起终点位置
            if (formulas != null && formulas.Count > 0)
            {
                //起始点
                subView.m_img_point_start_PolygonImage.gameObject.SetActive(true);
                Vector2 startPos = subView.m_img_point_start_PolygonImage.rectTransform.anchoredPosition;
                startPos.y = subView.m_img_polylinechart_PolyLineChart.rectTransform.sizeDelta.y * (formulas[0].heightPercent) - subView.m_img_polylinechart_PolyLineChart.rectTransform.sizeDelta.y / 2;
                subView.m_img_point_start_PolygonImage.rectTransform.anchoredPosition = startPos;

                //终点
                subView.m_img_point_end_PolygonImage.gameObject.SetActive(true);
                Vector2 pos = subView.m_img_point_end_PolygonImage.rectTransform.anchoredPosition;
                pos.y = subView.m_img_polylinechart_PolyLineChart.rectTransform.sizeDelta.y * (formulas[formulas.Count - 1].heightPercent) - subView.m_img_polylinechart_PolyLineChart.rectTransform.sizeDelta.y / 2;
                subView.m_img_point_end_PolygonImage.rectTransform.anchoredPosition = pos;
                //失败的时候显示溃败标记
                subView.m_img_fail_PolygonImage.gameObject.SetActive(formulas[formulas.Count - 1].heightPercent <= 0.01f);
            }
            else
            {
                subView.m_img_point_start_PolygonImage.gameObject.SetActive(false);
                subView.m_img_point_end_PolygonImage.gameObject.SetActive(false);
                //失败的时候显示溃败标记
                subView.m_img_fail_PolygonImage.gameObject.SetActive(false);
            }

            //刷新援助
            RefreshReinforce(subView);
        }

        private List<FunctionFormula> GetFormulas(BattleReportExDetail detail)
        {
            float formulaWidth = 8f;
            List<FunctionFormula> tmpFunc = new List<FunctionFormula>();
            if (m_battleReportList.Count < 1)
            {
                Debug.LogError("战报折线图信息为空");
                return tmpFunc;
            }
            //ClientUtils.Print(detail);
            //Debug.LogFormat("objectIndex:{0} maxArmyCount:{1}", detail.objectIndex, detail.maxArmyCount);
            long maxArmyCount = detail.maxArmyCount;
            int count = m_battleReportList.Count;
            tmpFunc.Add(new FunctionFormula { heightPercent = detail.beginArmyCount/(float)maxArmyCount, lengthPercent = 0, FormulaWidth = formulaWidth, FormulaColor = FormulaColor });
            for (int index = 0; index < count; index++)
            {
                bool isMyTurn = detail.objectIndex == m_battleReportList[index].attackIndex;
                if (!isMyTurn && detail.objectIndex != m_battleReportList[index].defenseIndex)
                {
                    continue;
                }
                //ClientUtils.Print(m_battleReportList[index]);
                float heightPercent = (isMyTurn ? m_battleReportList[index].attackArmyCount : m_battleReportList[index].defenseArmyCount) / (float)maxArmyCount;
                float percent = (index + 1) / (float)count;
                tmpFunc.Add(new FunctionFormula { heightPercent = heightPercent, lengthPercent = percent, FormulaWidth = formulaWidth, FormulaColor = FormulaColor });

                if (!m_roundPercentDic.ContainsKey(m_battleReportList[index].turn))
                {
                    m_roundPercentDic[m_battleReportList[index].turn] = percent;
                }
            }
            ProcessReinforcePercent();

            return tmpFunc;
        }

        //处理援助显示位置的百分比
        private void ProcessReinforcePercent()
        {
            if (m_reinforcePercentDic != null)
            {
                return;
            }
            m_reinforcePercentDic = new Dictionary<long, float>();
            List<ReinforecePercent> percentList = new List<ReinforecePercent>();
            List<ReinforecePercent> findList = new List<ReinforecePercent>();

            int maxRoundNum = 0;
            if (m_reinforceDataDic.Count > 0)
            {
                Dictionary<int, float> tempDic = new Dictionary<int, float>();
                foreach (var data in m_roundPercentDic)
                {
                    tempDic[(int)data.Key] = data.Value;
                }

                foreach (var data in m_reinforceDataDic)
                {
                    List<FightReportReinforceData> dataList = data.Value;
                    long round = dataList[0].ReinforceInfo.time;
                    if (m_roundPercentDic.ContainsKey(round))
                    {
                        m_reinforcePercentDic[round] = m_roundPercentDic[round];
                    }
                    else
                    {
                        tempDic[(int)round] = -1;
                    }
                }

                List<int> tempList = new List<int>();
                foreach (var data in tempDic)
                {
                    tempList.Add(data.Key);
                }
                tempList.Sort();
                if (tempList.Count > 0)
                {
                    maxRoundNum = tempList[tempList.Count - 1];
                }

                float newPercent = 0f;
                int newRound = 1;
                for(int i=0;i< tempList.Count;i++)
                {
                    int round = tempList[i];
                    if (tempDic[round] > -1)
                    {
                        newPercent = tempDic[round];
                        newRound = round;
                        int count2 = percentList.Count;
                        if (count2 > 0)
                        {
                            for (int j = count2 - 1; j >= 0; j--)
                            {
                                percentList[j].EndRound = newRound;
                                percentList[j].EndPercent = newPercent;
                                findList.Add(percentList[j]);
                                percentList.RemoveAt(j);
                            }
                        }
                    }
                    else
                    {
                        ReinforecePercent roundPercent = new ReinforecePercent();
                        roundPercent.CurrRound = round;
                        roundPercent.StartRound = newRound;
                        roundPercent.StartPercent = newPercent;
                        percentList.Add(roundPercent);
                    }
                }
            }

            if (percentList.Count > 0)
            {
                for (int i = 0; i < percentList.Count; i++)
                {
                    percentList[i].EndRound = (int)maxRoundNum;
                    percentList[i].EndPercent = 1f;
                }
            }
            findList.AddRange(percentList);
            percentList.Clear();

            for (int i = 0; i < findList.Count; i++)
            {
                float percentDiff = findList[i].EndPercent - findList[i].StartPercent;
                int totalRoundDiff = findList[i].EndRound - findList[i].StartRound;
                int startRoundDiff = findList[i].CurrRound - findList[i].StartRound;
                float percent = 0f;
                if (totalRoundDiff > 0f)
                {
                    percent = (float)startRoundDiff / totalRoundDiff;
                }
                else
                {
                    Debug.LogError("计算异常");
                }
                m_reinforcePercentDic[findList[i].CurrRound] = findList[i].StartPercent + percent * percentDiff;
                //m_reinforcePercentDic[findList[i].CurrRound] = findList[i].EndPercent;
            }
            findList.Clear();
        }

        //刷新援助
        private void RefreshReinforce(UI_Item_MailWarCoordinate_SubView subView)
        {
            if (m_reinforceDataDic.Count < 1)
            {
                subView.m_pl_aid.gameObject.SetActive(false);
                return;
            }
            if (m_reinforcePercentDic == null)
            {
                return;
            }

            long totalTime = m_battleReportEx.battleEndTime - m_battleReportEx.battleBeginTime;

            subView.m_pl_aid.gameObject.SetActive(true);
            RectTransform aidRect = subView.m_pl_aid.gameObject.GetComponent<RectTransform>();
            int childCount = subView.m_pl_aid.transform.childCount;
            for (int i = childCount - 1; i > 0; i--)
            {
                CoreUtils.assetService.Destroy(subView.m_pl_aid.transform.GetChild(i).gameObject);
            }

            foreach(var data in m_reinforceDataDic)
            {
                List<FightReportReinforceData> dataList = data.Value;
                long round = dataList[0].ReinforceInfo.time;
                GameObject go = CoreUtils.assetService.Instantiate(subView.m_UI_Item_MailWarReinforce.gameObject);
                go.transform.SetParent(subView.m_pl_aid.transform);
                go.SetActive(true);
                go.transform.localScale = Vector3.one;

                float radio = 0f;
                if (m_reinforcePercentDic.ContainsKey(round))
                {
                    radio = m_reinforcePercentDic[round];
                }
                float posX = aidRect.rect.width * radio;
                go.transform.localPosition = new Vector3(posX, 0, 0);

                UI_Item_MailWarReinforce_SubView btnView = new UI_Item_MailWarReinforce_SubView(go.GetComponent<RectTransform>());
                bool isAdd = dataList[0].ReinforceType == 1;
                btnView.m_img_add_PolygonImage.gameObject.SetActive(isAdd);
                btnView.m_img_remove_PolygonImage.gameObject.SetActive(!isAdd);
                btnView.m_UI_Item_MailWarReinforce_GameButton.onClick.AddListener(() =>
                {
                    FightReportReinforceTipData param = new FightReportReinforceTipData();
                    param.DataList = m_reinforceDataDic[round];
                    param.WorldPos = btnView.m_UI_Item_MailWarReinforce_GameButton.transform.position;
                    CoreUtils.uiManager.ShowUI(UI.s_mailWarTips, null, param);
                });
            }
        }

        #endregion

        //敌方攻击时间进度
        private void RefreshTimeBar(ListView.ListItem listItem, UI_Item_WarMailBar_SubView subView)
        {
            int index = listItem.index-m_timeBarStartIndex;
            BattleReportExDetail otherBattleDetail = m_otherReportList[index];

            long monster_id = GetMonsterId(otherBattleDetail);
            if (monster_id > 0) //怪物
            {
                subView.m_UI_Model_PlayerHead.gameObject.SetActive(false);
                subView.m_img_circle_PolygonImage.gameObject.SetActive(true);
                MonsterDefine monster = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)monster_id);
                if (monster != null)
                {
                    ClientUtils.LoadSprite(subView.m_img_build_PolygonImage, monster.mailIcon);
                    subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(monster.l_nameId);
                }
            }
            else//玩家
            {
                subView.m_UI_Model_PlayerHead.gameObject.SetActive(true);
                subView.m_img_circle_PolygonImage.gameObject.SetActive(false);
                subView.m_UI_Model_PlayerHead.LoadPlayerIcon(otherBattleDetail.headId);
                subView.m_lbl_name_LanguageText.text = string.IsNullOrEmpty(otherBattleDetail.guildName)? otherBattleDetail.name : LanguageUtils.getTextFormat(300030, otherBattleDetail.guildName, otherBattleDetail.name);
            }

            RectTransform rect = subView.m_pl_content.GetComponent<RectTransform>();

            if(m_battleReportEx.battleReport == null)
            {
                Debug.LogError("battleReport is null");
                return;
            }

            BattleReportWithObjectIndex info = null;
            m_battleReportEx.battleReport.TryGetValue(otherBattleDetail.objectIndex, out info);
            if (info != null)
            {
                long totalDiff = m_battleReportEx.battleEndTime - m_battleReportEx.battleBeginTime;
                totalDiff = (totalDiff <= 0) ? 1 : totalDiff;

                long beginTime = info.battleBeginTime;
                long endTime = info.battleEndTime;

                //计算起始进度
                long startTime = beginTime - m_battleReportEx.battleBeginTime;
                startTime = (startTime <= 0) ? 0 : startTime;
                float startWidth = ((float)startTime / totalDiff) * rect.rect.width;

                //计算进度区间
                long diffTime = endTime - beginTime;
                diffTime = (diffTime <= 0) ? 1 : diffTime;
                float width = ((float)diffTime / totalDiff) * rect.rect.width;
                RectTransform barRect = subView.m_btn_bar_GameButton.GetComponent<RectTransform>();
                barRect.sizeDelta = new Vector2(width, barRect.sizeDelta.y);

                subView.m_btn_bar_GameButton.transform.localPosition = new Vector2(-rect.rect.width / 2 + startWidth, subView.m_btn_bar_GameButton.transform.localPosition.y);
            }

            subView.m_btn_bar_GameButton.onClick.RemoveAllListeners();
            subView.m_btn_bar_GameButton.onClick.AddListener(() =>
            {
                m_sv_fightReport_ListView.MovePanelToItemIndex(index+m_fightDetailStartIndex);
            });
        }

        #region 战斗详情

        private void RefreshFightDetail(ListView.ListItem item, UI_Item_WarMailWar_SubView itemView)
        {
            int index = item.index-m_fightDetailStartIndex;
            UI_Item_MailWarPersonInfo_SubView rightView = itemView.m_UI_Item_MailWarPersonInfo_left;

            BattleReportEx report = m_battleReportEx;
            BattleReportExDetail selfBattleDetail = m_selfBattleDetail;
            BattleReportExDetail otherBattleDetail = m_otherReportList[index];

            //标题坐标
            if (otherBattleDetail.pos != null)
            {
                Vector2 pos = PosHelper.ServerPosToLocal(otherBattleDetail.pos);
                itemView.m_lbl_position.m_UI_Model_Link_LanguageText.text = LanguageUtils.getTextFormat(300032, pos.x, pos.y);
                itemView.m_lbl_position.SetPos((int)pos.x, (int)pos.y);
                itemView.m_lbl_position.RegisterPosJumpEvent();
            }
            //时间
            itemView.m_lbl_date_LanguageText.text =  UIHelper.GetAstTimeFormat(report.battleBeginTime);

            //刷新己方显示
            RefreshSelfDetail(selfBattleDetail, itemView, report, otherBattleDetail.objectIndex);

            //刷新敌方显示       
            if (GetMonsterId(otherBattleDetail) > 0 ) //和野蛮人对战
            {
                bool isAttackWin = m_selfRid == m_battleReportEx.objectInfos[(int)m_battleReportEx.winObjectIndex].rid;
                bool isWinner = isAttackWin;
                RefreshMonsterDetail(otherBattleDetail, isWinner, rightView, selfBattleDetail.objectIndex);
            }
            else //和其他玩家对战
            {
                //是否1回合战败
                bool isOneRoundFail = false; 
                BattleReportWithObjectIndex info = null;
                m_battleReportEx.battleReport.TryGetValue(otherBattleDetail.objectIndex, out info);
                if (info != null)
                {
                    if (info.battleDamageHeal != null && info.battleDamageHeal.Count >0)
                    {
                        long diffRound = info.battleDamageHeal[info.battleDamageHeal.Count - 1].turn - info.battleDamageHeal[0].turn;
                        if (diffRound <= 1)
                        {
                            if (m_otherReportList.Count == 1 && m_selfBattleDetail.objectIndex != m_battleReportEx.winObjectIndex)
                            {
                                isOneRoundFail = true;
                            }
                        }
                    }
                }
                RefreshOtherPlayerDetail(otherBattleDetail, rightView, selfBattleDetail.objectIndex, isOneRoundFail);
            }
            if (otherBattleDetail.pos != null) //刷新敌方坐标
            {
                Vector2 pos = PosHelper.ServerPosToLocal(otherBattleDetail.pos);
                rightView.m_lbl_positionL.m_UI_Model_Link_LanguageText.text = LanguageUtils.getTextFormat(300032, pos.x, pos.y);
                rightView.m_lbl_positionL.SetPos((int)pos.x, (int)pos.y);
                rightView.m_lbl_positionL.RegisterPosJumpEvent();
            }

            //部队增益
            itemView.m_UI_btn_buff.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
            itemView.m_UI_btn_buff.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                //todo
                Tip.CreateTip(100045, Tip.TipStyle.Middle).Show();
            });
            //战斗日志
            itemView.m_UI_btn_log.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
            itemView.m_UI_btn_log.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                //todo
                Tip.CreateTip(100045, Tip.TipStyle.Middle).Show();
                //EmailBattleLogParam param = new EmailBattleLogParam();
                //param.EmailInfo = m_currentBattleReport;
                //param.TargetObjectIndex = otherBattleDetail.objectIndex;
                //CoreUtils.uiManager.ShowUI(UI.s_emailBattleLog, null, param);
            });

            //重设item高度
            RectTransform rect1 = itemView.m_UI_Item_MailWarPersonInfo_self.gameObject.GetComponent<RectTransform>();
            RectTransform rect2 = itemView.m_UI_Item_MailWarPersonInfo_left.gameObject.GetComponent<RectTransform>();
            if (rect1.rect.height != rect2.rect.height)
            {
                if (rect1.rect.height > rect2.rect.height)
                {
                    rect2.sizeDelta = rect1.sizeDelta;
                }
                else
                {
                    rect1.sizeDelta = rect2.sizeDelta;
                }
            }
            RectTransform rect3 = itemView.m_pl_rect_GridLayoutGroup.gameObject.GetComponent<RectTransform>();
            rect3.sizeDelta = new Vector2(rect3.rect.width, rect1.rect.height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(itemView.m_pl_rect_GridLayoutGroup.GetComponent<RectTransform>());
        }

        //己方详情
        private void RefreshSelfDetail(BattleReportExDetail myBattleDetail, UI_Item_WarMailWar_SubView itemView, BattleReportEx report, long objectIndex)
        {
            //头像
            itemView.m_UI_Model_PlayerHead.LoadPlayerIcon(myBattleDetail.headId);
            itemView.m_lbl_name_self_LanguageText.text = string.IsNullOrEmpty(myBattleDetail.guildName)? myBattleDetail.name : LanguageUtils.getTextFormat(300030, myBattleDetail.guildName, myBattleDetail.name);

            if (myBattleDetail.pos != null)
            {
                Vector2 pos = PosHelper.ServerPosToLocal(myBattleDetail.pos);
                itemView.m_lbl_position_self.m_UI_Model_Link_LanguageText.text = LanguageUtils.getTextFormat(300032, pos.x, pos.y);
                itemView.m_lbl_position_self.SetPos((int)pos.x, (int)pos.y);
                itemView.m_lbl_position_self.RegisterPosJumpEvent();
            }

            //计算损失战力
            long powerLoss = 0;
            foreach (var data2 in myBattleDetail.soldierDetail[objectIndex].battleSoldierHurt)
            {
                ArmsDefine define2 = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)data2.Value.soldierId);
                if(define2 != null && define2.armsType != 7)
                {
                    powerLoss += define2.militaryCapability * data2.Value.die+ define2.militaryCapability * data2.Value.hardHurt;
                }
            }

            itemView.m_lbl_power_self_LanguageText.text = LanguageUtils.getTextFormat(300109, Mathf.Abs(powerLoss).ToString("N0"));
            if (myBattleDetail.mainHeroId > 0)
            {
                HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>((int)myBattleDetail.mainHeroId);
                itemView.m_lbl_name1_self_LanguageText.text = LanguageUtils.getTextFormat(300015, myBattleDetail.mainHeroLevel, LanguageUtils.getText(hero.l_nameID));
                itemView.m_UI_Model_CaptainHead1_self.SetIcon(hero.heroIcon);
                itemView.m_UI_Model_CaptainHead1_self.SetRare(hero.rare);

                if (report.mainHeroExp > 0)
                {
                    itemView.m_img_exp1_self_PolygonImage.gameObject.SetActive(true);
                    itemView.m_lbl_exp1_self_LanguageText.text = ClientUtils.FormatComma(report.mainHeroExp);
                }
                else
                {
                    itemView.m_img_exp1_self_PolygonImage.gameObject.SetActive(false);
                }
            }
            else
            {
                itemView.m_pl_empty1_self.gameObject.SetActive(true);
                itemView.m_pl_exp1_self.gameObject.SetActive(false);
                itemView.m_pl_key1_self.gameObject.SetActive(false);
            }
            //我方第二个统帅
            if (myBattleDetail.deputyHeroId > 0)
            {
                HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>((int)myBattleDetail.deputyHeroId);
                itemView.m_UI_Model_CaptainHead2_self.SetIcon(hero.heroIcon);
                itemView.m_UI_Model_CaptainHead2_self.SetRare(hero.rare);
                itemView.m_lbl_name2_self_LanguageText.text = LanguageUtils.getTextFormat(300015, myBattleDetail.deputyHeroLevel, LanguageUtils.getText(hero.l_nameID));

                itemView.m_pl_empty2_self.gameObject.SetActive(false); //无
                itemView.m_pl_exp2_self.gameObject.SetActive(true);   //经验
                itemView.m_pl_key2_self.gameObject.SetActive(false);  //是否解锁
                if (report.deputyHeroExp > 0)
                {
                    itemView.m_img_exp2_self_PolygonImage.gameObject.SetActive(true);
                    itemView.m_lbl_exp2_self_LanguageText.text = ClientUtils.FormatComma(report.deputyHeroExp);
                }
                else
                {
                    itemView.m_img_exp2_self_PolygonImage.gameObject.SetActive(false);
                }
            }
            else//没有第二个统帅或未解锁
            {
                HeroProxy heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;

                if (myBattleDetail.mainHeroId > 0)
                {
                    if (heroProxy.GetHeroByID(myBattleDetail.mainHeroId).star >= 3) //没有第二统帅
                    {
                        itemView.m_pl_empty2_self.gameObject.SetActive(true);
                        itemView.m_pl_exp2_self.gameObject.SetActive(false);
                        itemView.m_pl_key2_self.gameObject.SetActive(false);
                    }
                    else //未解锁
                    {
                        itemView.m_pl_empty2_self.gameObject.SetActive(false);
                        itemView.m_pl_exp2_self.gameObject.SetActive(false);
                        itemView.m_pl_key2_self.gameObject.SetActive(true);
                    }
                }
                else
                {
                    itemView.m_pl_empty2_self.gameObject.SetActive(true);
                    itemView.m_pl_exp2_self.gameObject.SetActive(false);
                    itemView.m_pl_key2_self.gameObject.SetActive(false);
                }
                itemView.m_img_exp2_self_PolygonImage.gameObject.SetActive(false);
            }

            long aTotal = GetTotalArmyTotalNum(myBattleDetail.soldierDetail[objectIndex], out long treatment, out long aMinor, out long aDie, out long aHurt, out long aRemain, out long towerHurt);

            itemView.m_lbl_val_total_self_LanguageText.text = myBattleDetail.beginArmyCount.ToString("N0");
            itemView.m_lbl_val_dead_self_LanguageText.text = aDie.ToString("N0");
            itemView.m_lbl_val_reatment_self_LanguageText.text = treatment.ToString("N0");
            itemView.m_lbl_val_heart_self_LanguageText.text = aHurt.ToString("N0");
            itemView.m_lbl_val_littlehurt_self_LanguageText.text = aMinor.ToString("N0");
            itemView.m_lbl_val_last_self_LanguageText.text = aRemain.ToString("N0");
            itemView.m_pl_arrow_self.gameObject.SetActive(towerHurt > 0);
            itemView.m_lbl_val_arrow_self_LanguageText.text = towerHurt.ToString("N0");

            //是否显示失败标致
            itemView.m_pl_defultL_self.gameObject.SetActive(aRemain<=0);

            //部队详情
            itemView.m_btn_more_self_GameButton.gameObject.SetActive(true);
            itemView.m_btn_more_self_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_more_self_GameButton.onClick.AddListener(() =>
            {
                FightArmyHurtParam param = new FightArmyHurtParam();
                param.ReportExDetail = myBattleDetail;
                param.ObjectIndex = objectIndex;
                CoreUtils.uiManager.ShowUI(UI.s_ArmyDetail, null, param);
            });

            //tips
            itemView.m_btn_Question_heart_self_GameButton.gameObject.SetActive(true);
            itemView.m_btn_Question_heart_self_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_Question_heart_self_GameButton.onClick.AddListener(() =>
            {
                HelpTip.CreateTip(3004, itemView.m_btn_Question_heart_self_GameButton.transform).Show();
            });

            itemView.m_btn_Question_ittlehurt_self_GameButton.gameObject.SetActive(true);
            itemView.m_btn_Question_ittlehurt_self_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_Question_ittlehurt_self_GameButton.onClick.AddListener(() =>
            {
                HelpTip.CreateTip(3005, itemView.m_btn_Question_ittlehurt_self_GameButton.transform).Show();
            });

            //重设高度
            float height = 0f;
            height += itemView.m_player_self.gameObject.GetComponent<RectTransform>().rect.height;
            height += itemView.m_captain_self_GridLayoutGroup.gameObject.GetComponent<RectTransform>().rect.height;
            height += itemView.m_pl_detail_self.gameObject.GetComponent<RectTransform>().rect.height;
            if (itemView.m_pl_defultL_self.gameObject.activeSelf)
            {
                height += itemView.m_pl_defultL_self.gameObject.GetComponent<RectTransform>().rect.height;
            }
            RectTransform rect = itemView.m_UI_Item_MailWarPersonInfo_self.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.rect.width, height);
        }

        //敌方怪物详情
        private void RefreshMonsterDetail(BattleReportExDetail otherBattleDetail, bool isWinner, UI_Item_MailWarPersonInfo_SubView rightView, long objectIndex)
        {
            rightView.m_UI_Model_PlayerHeadL.gameObject.SetActive(false);
            rightView.m_img_monsterhead_PolygonImage.gameObject.SetActive(false);
            rightView.m_img_fra_PolygonImage.gameObject.SetActive(true);
            long monster_id = GetMonsterId(otherBattleDetail);
            MonsterDefine monster = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)monster_id);
            if (monster != null)
            {
                SendGuideFuncTrigger(monster, isWinner);
                //头像
                ClientUtils.LoadSprite(rightView.m_img_monster_PolygonImage, monster.mailIcon);

                rightView.m_lbl_nameL_LanguageText.text = LanguageUtils.getText(monster.l_nameId);
                MonsterTroopsDefine troopDefine = CoreUtils.dataService.QueryRecord<MonsterTroopsDefine>(monster.monsterTroopsId);
                if (troopDefine != null)
                {
                    HeroDefine hero1 = CoreUtils.dataService.QueryRecord<HeroDefine>(troopDefine.heroID1);
                    if (hero1 != null)
                    {
                        rightView.m_lbl_name1_LanguageText.text = LanguageUtils.getText(hero1.l_nameID);
                        rightView.m_UI_Model_CaptainHead1.SetIcon(hero1.heroIcon);
                        rightView.m_UI_Model_CaptainHead1.SetRare(hero1.rare);
                    }
                    else
                    {
                        rightView.m_pl_empty1.gameObject.SetActive(true);
                        rightView.m_pl_exp1.gameObject.SetActive(false);
                        rightView.m_pl_key1.gameObject.SetActive(false);
                    }
                    if (troopDefine.heroID2 > 0)
                    {
                        HeroDefine hero2 = CoreUtils.dataService.QueryRecord<HeroDefine>(troopDefine.heroID2);
                        if (hero2 != null)
                        {
                            rightView.m_lbl_name2_LanguageText.text = LanguageUtils.getText(hero2.l_nameID);
                            rightView.m_UI_Model_CaptainHead2.SetIcon(hero2.heroIcon);
                            rightView.m_UI_Model_CaptainHead2.SetRare(hero2.rare);
                        }
                    }
                    else
                    {
                        rightView.m_pl_empty2.gameObject.SetActive(true);
                        rightView.m_pl_exp2.gameObject.SetActive(false);
                        rightView.m_pl_key2.gameObject.SetActive(false);
                    }
                    //获取经验暂时隐藏
                    rightView.m_img_exp1_PolygonImage.gameObject.SetActive(false);
                    rightView.m_img_exp1_PolygonImage.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("野蛮人表读取错误：" + otherBattleDetail.objectType);
            }

            //野蛮人只显示受到伤害
            rightView.m_pl_data_GridLayoutGroup.gameObject.SetActive(false);
            rightView.m_pl_Specail.gameObject.SetActive(true);
            rightView.m_pb_rogressBar_GameSlider.gameObject.SetActive(true);
            rightView.m_lbl_powerL_LanguageText.gameObject.SetActive(false);

            long defenseRemain = 0;
            if (otherBattleDetail.soldierDetail == null || !otherBattleDetail.soldierDetail.ContainsKey(objectIndex))
            {
                Debug.LogErrorFormat("monster objectInfos.soldierDetail属性 没有我方的伤害信息：{0}", objectIndex);
                return;
            }
            long defenseTotal = GetTotalArmyCount(otherBattleDetail.soldierDetail[objectIndex], out defenseRemain);
            double percent = Math.Round((defenseRemain / (double)defenseTotal), 2);
            rightView.m_lbl_specvalL_LanguageText.text = LanguageUtils.getTextFormat(300107, 100 - percent * 100);
            rightView.m_lbl_percent_LanguageText.text = LanguageUtils.getTextFormat(180357, percent * 100);
            rightView.m_pb_rogressBar_GameSlider.value = (float)percent;

            //判断是否显示失败标致
            rightView.m_pl_defultL.gameObject.SetActive(defenseRemain <= 0);

            //重设高度
            float height = 0f;
            height += rightView.m_pl_player.gameObject.GetComponent<RectTransform>().rect.height;
            height += rightView.m_pl_captain_GridLayoutGroup.gameObject.GetComponent<RectTransform>().rect.height;
            height += rightView.m_pl_detailL.gameObject.GetComponent<RectTransform>().rect.height;
            if (rightView.m_pl_defultL.gameObject.activeSelf)
            {
                height += rightView.m_pl_defultL.gameObject.GetComponent<RectTransform>().rect.height;
            }
            RectTransform rect = rightView.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.rect.width, height);
        }

        //敌方玩家详情
        private void RefreshOtherPlayerDetail(BattleReportExDetail otherBattleDetail, UI_Item_MailWarPersonInfo_SubView rightView, long objectIndex, bool isOneRoundFail)
        {
            //头像
            rightView.m_UI_Model_PlayerHeadL.gameObject.SetActive(true);
            rightView.m_img_monsterhead_PolygonImage.gameObject.SetActive(false);
            rightView.m_img_fra_PolygonImage.gameObject.SetActive(false);
            rightView.m_UI_Model_PlayerHeadL.LoadPlayerIcon(otherBattleDetail.headId);

            rightView.m_lbl_nameL_LanguageText.text = string.IsNullOrEmpty(otherBattleDetail.guildName) ? otherBattleDetail.name : LanguageUtils.getTextFormat(300030, otherBattleDetail.guildName, otherBattleDetail.name);

            //计算损失战力
            long powerLoss = 0;
            if (!otherBattleDetail.soldierDetail.ContainsKey(objectIndex))
            {
                Debug.LogErrorFormat("otherBattleDetail not find objectIndex:{0}",objectIndex);
                return;
            }
            foreach (var data2 in otherBattleDetail.soldierDetail[objectIndex].battleSoldierHurt)
            {
                ArmsDefine define2 = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)data2.Value.soldierId);
                if (define2 != null && define2.armsType != 7)
                {
                    powerLoss += define2.militaryCapability * data2.Value.die + define2.militaryCapability * data2.Value.hardHurt;
                }
            }

            //刷新敌方战力
            rightView.m_lbl_powerL_LanguageText.gameObject.SetActive(true);
            rightView.m_lbl_powerL_LanguageText.text = LanguageUtils.getTextFormat(300109, Mathf.Abs(powerLoss).ToString("N0"));

            if (otherBattleDetail.mainHeroId > 0)
            {
                rightView.m_pl_empty1.gameObject.SetActive(false);
                rightView.m_pl_exp1.gameObject.SetActive(true);
                rightView.m_pl_key1.gameObject.SetActive(false);

                HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>((int)otherBattleDetail.mainHeroId);
                rightView.m_UI_Model_CaptainHead1.SetIcon(hero.heroIcon);
                rightView.m_UI_Model_CaptainHead1.SetRare(hero.rare);
                rightView.m_lbl_name1_LanguageText.text = LanguageUtils.getTextFormat(300015, otherBattleDetail.mainHeroLevel, LanguageUtils.getText(hero.l_nameID));
            }
            else
            {
                rightView.m_pl_empty1.gameObject.SetActive(true);
                rightView.m_pl_exp1.gameObject.SetActive(false);
                rightView.m_pl_key1.gameObject.SetActive(false);
            }
            //对方第二个统帅
            if (otherBattleDetail.deputyHeroId > 0)
            {
                rightView.m_pl_empty2.gameObject.SetActive(false);
                rightView.m_pl_exp2.gameObject.SetActive(true);
                rightView.m_pl_key2.gameObject.SetActive(false);

                HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>((int)otherBattleDetail.deputyHeroId);
                rightView.m_lbl_name2_LanguageText.text = LanguageUtils.getTextFormat(300015, otherBattleDetail.deputyHeroLevel, LanguageUtils.getText(hero.l_nameID));
                rightView.m_UI_Model_CaptainHead2.SetIcon(hero.heroIcon);
                rightView.m_UI_Model_CaptainHead2.SetRare(hero.rare);
            }
            else//没有第二个统帅或未解锁
            {
                rightView.m_pl_empty2.gameObject.SetActive(true);
                rightView.m_pl_exp2.gameObject.SetActive(false);
                rightView.m_pl_key2.gameObject.SetActive(false);
            }
            //获取经验暂时隐藏
            rightView.m_img_exp1_PolygonImage.gameObject.SetActive(false);
            rightView.m_img_exp1_PolygonImage.gameObject.SetActive(false);

            rightView.m_pl_data_GridLayoutGroup.gameObject.SetActive(true);
            rightView.m_pl_Specail.gameObject.SetActive(false);
            rightView.m_pb_rogressBar_GameSlider.gameObject.SetActive(false);

            long ototal = GetTotalArmyTotalNum(otherBattleDetail.soldierDetail[objectIndex], out long treatment, out long oMinor, out long oDie, out long oHurt, out long oRemain, out long towerHurt);
            if (isOneRoundFail)
            {
                rightView.m_lbl_val_total_self_LanguageText.text = m_questionStr;
                rightView.m_lbl_val_reatment_self_LanguageText.text = m_questionStr;
                rightView.m_lbl_val_dead_self_LanguageText.text = m_questionStr;
                rightView.m_lbl_val_heart_self_LanguageText.text = m_questionStr;
                rightView.m_lbl_val_littlehurt_self_LanguageText.text = m_questionStr;
                rightView.m_lbl_val_last_self_LanguageText.text = m_questionStr;
                rightView.m_pl_arrow_self.gameObject.SetActive(towerHurt > 0);
                rightView.m_lbl_val_arrow_self_LanguageText.text = m_questionStr;
            }
            else
            {
                rightView.m_lbl_val_total_self_LanguageText.text = otherBattleDetail.beginArmyCount.ToString("N0");
                rightView.m_lbl_val_reatment_self_LanguageText.text = treatment.ToString("N0");
                rightView.m_lbl_val_dead_self_LanguageText.text = oDie.ToString("N0");
                rightView.m_lbl_val_heart_self_LanguageText.text = oHurt.ToString("N0");
                rightView.m_lbl_val_littlehurt_self_LanguageText.text = oMinor.ToString("N0");
                rightView.m_lbl_val_last_self_LanguageText.text = oRemain.ToString("N0");
                rightView.m_pl_arrow_self.gameObject.SetActive(towerHurt > 0);
                rightView.m_lbl_val_arrow_self_LanguageText.text = towerHurt.ToString("N0");
            }

            //判断是否显示失败标致
            rightView.m_pl_defultL.gameObject.SetActive(oRemain<=0);

            //部队详情
            rightView.m_btn_more_self_GameButton.gameObject.SetActive(true);
            rightView.m_btn_more_self_GameButton.onClick.RemoveAllListeners();
            rightView.m_btn_more_self_GameButton.onClick.AddListener(() =>
            {
                FightArmyHurtParam param = new FightArmyHurtParam();
                param.ReportExDetail = otherBattleDetail;
                param.ObjectIndex = objectIndex;
                CoreUtils.uiManager.ShowUI(UI.s_ArmyDetail, null, param);
            });

            //tips
            rightView.m_btn_Question_heart_self_GameButton.gameObject.SetActive(true);
            rightView.m_btn_Question_heart_self_GameButton.onClick.RemoveAllListeners();
            rightView.m_btn_Question_heart_self_GameButton.onClick.AddListener(() =>
            {
                HelpTip.CreateTip(3004, rightView.m_btn_Question_heart_self_GameButton.transform).Show();
            });
            rightView.m_btn_Question_ittlehurt_self_GameButton.gameObject.SetActive(true);
            rightView.m_btn_Question_ittlehurt_self_GameButton.onClick.RemoveAllListeners();
            rightView.m_btn_Question_ittlehurt_self_GameButton.onClick.AddListener(() =>
            {
                HelpTip.CreateTip(3005, rightView.m_btn_Question_ittlehurt_self_GameButton.transform).Show();
            });

            //重设高度
            float height = 0f;
            height += rightView.m_pl_player.gameObject.GetComponent<RectTransform>().rect.height;
            height += rightView.m_pl_captain_GridLayoutGroup.gameObject.GetComponent<RectTransform>().rect.height;
            height += rightView.m_pl_detailL.gameObject.GetComponent<RectTransform>().rect.height;
            if (rightView.m_pl_defultL.gameObject.activeSelf)
            {
                height += rightView.m_pl_defultL.gameObject.GetComponent<RectTransform>().rect.height;
            }
            RectTransform rect = rightView.gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.rect.width, height);
        }

        private long GetMonsterId(BattleReportExDetail exDetail)
        {
            if (exDetail.monsterId > 0)
            {
                return exDetail.monsterId;
            }
            return exDetail.holyLandBuildMonsterId;
        }

        private long GetTotalArmyCount(BattleSoldierHurtWithObject battleSoldierHurt, out long remain)
        {
            long total = 0;
            remain = 0;
            foreach (var data in battleSoldierHurt.battleSoldierHurt)
            {
                BattleSoldierHurt element = data.Value;
                remain += element.remain;
                total += element.minor + element.hardHurt + element.die + element.remain;
            }
            return total;
        }

        private long GetTotalArmyTotalNum(BattleSoldierHurtWithObject battleSoldierHurt, out long treatment, out long minor, out long die, out long hurt, out long remain, out long towerHurt)
        {
            long total = 0;
            remain = 0;
            minor = 0;
            die = 0;
            hurt = 0;
            towerHurt = 0;
            treatment = 0;

            foreach (var data in battleSoldierHurt.battleSoldierHurt)
            {
                BattleSoldierHurt element = data.Value;
                ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>((int)element.soldierId);
                if (define.armsType == 7)
                {
                    towerHurt += element.die+element.minor+element.hardHurt;
                }
                else
                {
                    die += element.die;
                    minor += element.minor;
                    hurt += element.hardHurt;
                    remain += element.remain;
                    total += element.minor + element.hardHurt + element.die + element.remain;
                    treatment += element.heal;
                }
            }

            return total;
        }

        //打开战报发送对应的请求
        private void SendGuideFuncTrigger(MonsterDefine define, bool win)
        {
            if (win)
            {
                return;
            }
            switch (define.level)
            {
                case 1: AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.MonsterFightFail1); break;
                case 2: AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.MonsterFightFail2); break;
                case 3: AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.MonsterFightFail3); break;
                case 4: AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.MonsterFightFail4); break;
                case 5: AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.MonsterFightFail5); break;
                default: break;

            }
        }

        #endregion

        #region 数据处理

        //数据处理
        private void ProcessData()
        {
            //resportSubTitle ={
            //  己方角色rid            0
            //  己方角色名称           1
            //  己方联盟名称           2
            //  防御方角色rid          3
            //  防御方对象id（建筑类） 4
            //  防御方角色名称         5    
            //  防御方联盟名称         6
            //}
            m_selfRid = Int64.Parse(m_currentBattleReport.reportSubTile[0]);
            Dictionary<Int64, BattleReportExDetail> tempDic = m_battleReportEx.objectInfos;
            m_otherReportList.Clear();
            foreach (var data in tempDic)
            {
                if (data.Value.rid == m_selfRid)
                {
                    m_selfBattleDetail = data.Value;
                    break;
                }
            }

            //Debug.Log("打印敌方");
            //foreach (var data in tempDic)
            //{
            //    ClientUtils.Print(data.Value);
            //}

            foreach(var data in m_selfBattleDetail.soldierDetail)
            {
                if (m_battleReportEx.battleReport != null && m_battleReportEx.battleReport.ContainsKey(data.Key))
                {
                    m_otherReportList.Add(tempDic[data.Key]);
                }
            }

            //按开始时间排序
            if (m_otherReportList.Count > 1)
            {
                if (m_battleReportEx.battleReport != null)
                {
                    //Debug.Log("伤害信息：");
                    //foreach (var data in m_battleReportEx.battleReport)
                    //{
                    //    ClientUtils.Print(data.Value);
                    //}

                    m_otherReportList.Sort((BattleReportExDetail x, BattleReportExDetail y) => {
                        return m_battleReportEx.battleReport[x.objectIndex].battleBeginTime.CompareTo(m_battleReportEx.battleReport[y.objectIndex].battleBeginTime);
                    });
                }
            }
            m_guildId = m_selfBattleDetail.guildId;

            //处理援助信息
            ProcessReinforceData();

            //处理折线图数据
            ProceeBrokenLineData();

            //处理需要显示item列表
            ProcessItemListData();
        }

        //处理需要显示item列表
        private void ProcessItemListData()
        {
            m_itemList.Clear();
            m_timeBarStartIndex = -1;
            m_fightDetailStartIndex = -1;
            //标题
            FightReportItemData item1 = new FightReportItemData();
            item1.ItemType = 1;
            m_itemList.Add(item1);

            //资源
            RewardInfo rewardInfo = m_battleReportEx.rewardInfo;
            if (rewardInfo != null && (rewardInfo.food !=0 || rewardInfo.wood !=0 || rewardInfo.stone !=0 || rewardInfo.gold != 0))
            {
                FightReportItemData item2 = new FightReportItemData();
                item2.ItemType = 2;
                m_itemList.Add(item2);
            }

            //显示击杀获得物品
            if (rewardInfo != null && rewardInfo.items != null && rewardInfo.items.Count > 0)
            {
                FightReportItemData item3 = new FightReportItemData();
                item3.ItemType = 3;
                m_itemList.Add(item3);
            }

            //部队信息
            FightReportItemData item4 = new FightReportItemData();
            item4.ItemType = 4;
            m_itemList.Add(item4);

            //折线图
            if (m_battleReportList.Count > 0)
            {
                FightReportItemData item5 = new FightReportItemData();
                item5.ItemType = 5;
                m_itemList.Add(item5);
            }

            if (m_otherReportList.Count > 0)
            {
                m_timeBarStartIndex = m_itemList.Count;
                for (int i = 0; i < m_otherReportList.Count; i++)
                {
                    //敌方攻击时间进度
                    FightReportItemData item6 = new FightReportItemData();
                    item6.ItemType = 6;
                    m_itemList.Add(item6);
                }
            }

            if (m_otherReportList.Count > 0)
            {
                m_fightDetailStartIndex = m_itemList.Count;
                for (int i = 0; i < m_otherReportList.Count; i++)
                {
                    //战斗详情
                    FightReportItemData item7 = new FightReportItemData();
                    item7.ItemType = 7;
                    m_itemList.Add(item7);
                }
            }
        }

        //处理援助信息
        private void ProcessReinforceData()
        {
            //援助信息
            m_reinforceDataDic = new Dictionary<long, List<FightReportReinforceData>>();
            List<FightReportReinforceData> tempList = new List<FightReportReinforceData>();
            if (m_battleReportEx.reinforceJoinArmy != null && m_battleReportEx.reinforceJoinArmy.Count > 0)
            {
                for (int i = 0; i < m_battleReportEx.reinforceJoinArmy.Count; i++)
                {
                    if (m_battleReportEx.reinforceJoinArmy[i].isArmyBack || m_battleReportEx.reinforceJoinArmy[i].isCityJoin || (m_guildId >0 && m_battleReportEx.reinforceJoinArmy[i].guildId == m_guildId))
                    {
                        FightReportReinforceData param = new FightReportReinforceData();
                        param.ReinforceInfo = m_battleReportEx.reinforceJoinArmy[i];
                        param.ReinforceType = 1;
                        tempList.Add(param);
                    }
                }
            }
            if (m_battleReportEx.reinforceLeaveArmy != null && m_battleReportEx.reinforceLeaveArmy.Count > 0)
            {
                for (int i = 0; i < m_battleReportEx.reinforceLeaveArmy.Count; i++)
                {
                    if (m_guildId > 0 && m_battleReportEx.reinforceLeaveArmy[i].guildId == m_guildId)
                    {
                        FightReportReinforceData param = new FightReportReinforceData();
                        param.ReinforceInfo = m_battleReportEx.reinforceLeaveArmy[i];
                        param.ReinforceType = 2;
                        tempList.Add(param);
                    }
                }
            }
            if (tempList.Count > 0)
            {
                tempList.Sort((FightReportReinforceData x, FightReportReinforceData y) => {
                    return x.ReinforceInfo.time.CompareTo(y.ReinforceInfo.time);
                });

                for (int i = 0; i < tempList.Count; i++)
                {
                    if (!m_reinforceDataDic.ContainsKey(tempList[i].ReinforceInfo.time))
                    {
                        m_reinforceDataDic[tempList[i].ReinforceInfo.time] = new List<FightReportReinforceData>();
                    }
                    m_reinforceDataDic[tempList[i].ReinforceInfo.time].Add(tempList[i]);
                }
                //foreach (var data in dic2)
                //{
                //    m_reinforceDataList.Add(data.Value);
                //}
            }
        }

        //处理折线图数据
        private void ProceeBrokenLineData()
        {
            m_battleReportList = new List<BattleReport>();
            if (m_battleReportEx.battleReport != null && m_battleReportEx.battleReport.Count > 0)
            {
                foreach (var data in m_battleReportEx.battleReport)
                {
                    if (data.Value.battleDamageHeal != null && data.Value.battleDamageHeal.Count > 0)
                    {
                        m_battleReportList.AddRange(data.Value.battleDamageHeal);
                    }
                }
                m_battleReportList.Sort((BattleReport x, BattleReport y) => {
                    return x.reportUniqueIndex.CompareTo(y.reportUniqueIndex);
                });
            }
        }

        #endregion 
    }
}
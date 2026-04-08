// =============================================================================== 
// Author              :    xzl
// Create Time         :    2020年5月7日
// Update Time         :    2020年5月7日
// Class Description   :    UI_Win_BattleLogMediator 战斗日志 
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
using System;
using Game;
using Data;

public class EmailBattleLogParam
{
    public EmailInfoEntity EmailInfo;
    public long TargetObjectIndex;
}

public class EmailBattleLogItem
{
    public int ItemType; //1标题 2内容
    public string Desc;
    public float ItemHeight = -1;
}

namespace Game {
    public class UI_Win_BattleLogMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_BattleLogMediator";

        private Dictionary<string, GameObject> m_assetDic;
        private float m_itemDescHeight;
        private float m_itemTitleHeight;

        private EmailInfoEntity m_emailInfo;
        private long m_targetObjectIndex;

        private Vector2 m_pos;
        private long m_times;
        private List<EmailBattleLogItem> m_dataList;

        private BattleReportExDetail m_selfBattleDetail;
        private BattleReportExDetail m_otherBattleDetail;

        private List<BattleReportExDetail> m_selfReportList = new List<BattleReportExDetail>();
        private List<BattleReportExDetail> m_otherReportList = new List<BattleReportExDetail>();

        private BattleReportWithObjectIndex m_roundData;

        private long m_selfRid;//己方角色rid
        private string m_selfRoleName;      //己方角色名称
        private string m_selfAbName;        //己方联盟简称
        private long m_otherRid;            //敌方角色id
        private long m_otherObjectId;       //敌方对象id（建筑类）
        private string m_otherRoleName;     //敌方角色名称     
        private string m_otherAbName;       //敌方联盟名称

        private string m_nullStr = "<color=#FFFFFF00>0000</color>";

        private BattleReportEx m_battleReportEx;

        #endregion

        //IMediatorPlug needs
        public UI_Win_BattleLogMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_BattleLogView view;

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
            EmailBattleLogParam param = view.data as EmailBattleLogParam;
            m_emailInfo = param.EmailInfo;
            m_targetObjectIndex = param.TargetObjectIndex;

            //m_battleReportEx = m_emailInfo.battleReportEx;

            ProcessItemData();

            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, OnLoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.setCloseHandle(OnClose);
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            m_assetDic = asset;

            m_itemDescHeight = m_assetDic["UI_Item_MailBattleLogContent"].gameObject.GetComponent<RectTransform>().rect.height;
            m_itemTitleHeight = m_assetDic["UI_Item_MailBattleLogTitle"].gameObject.GetComponent<RectTransform>().rect.height;

            RefreshList();
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_emailBattleLog);
        }

        private void RefreshList()
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ListViewItemByIndex;
            funcTab.GetItemSize = OnGetItemSize;
            funcTab.GetItemPrefabName = GetItemPrefabName;
            view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);

            view.m_sv_list_ListView.FillContent(m_dataList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            EmailBattleLogItem itemData = m_dataList[listItem.index];
            if (itemData.ItemType == 1)
            {
                if (listItem.data == null)
                {
                    UI_Item_MailBattleLogTitle_SubView subView = new UI_Item_MailBattleLogTitle_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.Refresh(m_times, m_pos);
                }
                else
                {
                    UI_Item_MailBattleLogTitle_SubView subView = listItem.data as UI_Item_MailBattleLogTitle_SubView;
                    subView.Refresh(m_times, m_pos);
                }
            }
            else
            {
                UI_Item_MailBattleLogContent_SubView subView;
                if (listItem.data == null)
                {
                    subView = new UI_Item_MailBattleLogContent_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                }
                else
                {
                    subView = listItem.data as UI_Item_MailBattleLogContent_SubView;
                }
                subView.Refresh(itemData.Desc);
                if (itemData.ItemHeight != subView.m_root_RectTransform.rect.height)
                {
                    itemData.ItemHeight = subView.m_root_RectTransform.rect.height;
                    view.m_sv_list_ListView.RefreshItem(listItem.index);
                }
            }
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            EmailBattleLogItem itemData = m_dataList[listItem.index];
            if (itemData.ItemType == 1)
            {
                return m_itemTitleHeight;
            }
            else
            {
                return itemData.ItemHeight;
            }
        }

        private string GetItemPrefabName(ListView.ListItem listItem)
        {
            int itemType = m_dataList[listItem.index].ItemType;
            if (itemType == 1)
            {
                return "UI_Item_MailBattleLogTitle";
            }
            else {
                return "UI_Item_MailBattleLogContent";
            }
        }

        #region 数据处理

        private void ProcessItemData()
        {
            m_otherBattleDetail = m_battleReportEx.objectInfos[m_targetObjectIndex];
            m_roundData = m_battleReportEx.battleReport[m_targetObjectIndex];

            m_selfRid = Int64.Parse(m_emailInfo.reportSubTile[0]);//己方角色rid
            m_selfRoleName = m_emailInfo.reportSubTile[1];      //己方角色名称
            m_selfAbName = m_emailInfo.reportSubTile[2];        //己方联盟简称
            m_otherRid = Int64.Parse(m_emailInfo.reportSubTile[3]);      //敌方角色id
            m_otherObjectId = Int64.Parse(m_emailInfo.reportSubTile[4]); //敌方对象id（建筑类）
            m_otherRoleName = m_emailInfo.reportSubTile[5];            //敌方角色名称     
            m_otherAbName = m_emailInfo.reportSubTile[6];              //敌方联盟名称

            Dictionary<Int64, BattleReportExDetail> tempDic = m_battleReportEx.objectInfos;
            foreach (var data in tempDic)
            {
                if (data.Value.rid == m_selfRid)
                {
                    m_selfBattleDetail = data.Value;
                    break;
                }
            }

            m_pos = PosHelper.ServerPosToLocal(m_otherBattleDetail.pos);
            m_times = m_roundData.battleBeginTime;

            m_dataList = new List<EmailBattleLogItem>();

            //战斗发生日期、时间及坐标
            EmailBattleLogItem item = new EmailBattleLogItem();
            item.ItemType = 1;
            m_dataList.Add(item);

            //处理战斗简介
            ProcessIntroData();

            //我方阵容
            ProcessSelfTroop();

            //敌方阵容
            ProcessOtherTroop();

            //回合信息
            ProcessRoundData();

            //战斗结束信息
            ProcessFightEnd();
        }

        //战斗简介
        private void ProcessIntroData()
        {
            long emailId = m_emailInfo.emailId;
            string desc = "";
            switch (emailId)
            {
                case 200006:
                case 200007:
                case 200009:
                case 200010:
                case 200039:
                case 200040:
                case 200041:
                case 200042:
                case 200043:
                    //在大地图与怪物发生战斗
                    {
                        HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_selfBattleDetail.mainHeroId);
                        MonsterDefine monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)m_otherBattleDetail.monsterId);
                        HeroDefine monsterHeroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_otherBattleDetail.mainHeroId);
                        desc = string.Format("（缺）{0}【{1}】和{2}【{3}】在野外发生了遭遇战！",
                                                                   GetFormatName(m_selfAbName, m_selfRoleName),
                                                                   LanguageUtils.getText(heroDefine.l_nameID),
                                                                   LanguageUtils.getText(monsterDefine.l_nameId),
                                                                   LanguageUtils.getText(monsterHeroDefine.l_nameID));
                    }
                    break;
                case 200000:
                case 200001:
                case 200011:
                    //todo
                    //在大地图与其他玩家部队发生战斗
                    {
                        HeroDefine heroDefine1 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_selfBattleDetail.mainHeroId);
                        HeroDefine heroDefine2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_otherBattleDetail.mainHeroId);

                        desc = string.Format("(缺){0}【{1}】和{2}【{3}】在野外发生了遭遇战！",
                                                                   GetFormatName(m_selfAbName, m_selfRoleName),
                                                                   LanguageUtils.getText(heroDefine1.l_nameID),
                                                                   GetFormatName(m_otherAbName, m_otherRoleName),
                                                                   LanguageUtils.getText(heroDefine2.l_nameID));
                    }
                    break;
                case 200004:
                case 200005:
                case 200048:
                case 200012:
                case 200013:
                case 200014:
                    //攻击城市
                    {
                        HeroDefine heroDefine1 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_selfBattleDetail.mainHeroId);
                        desc = string.Format("(缺){0}<color=green>【{1}】</color>攻击了{2}的城市！",
                                                                   GetFormatName(m_selfAbName, m_selfRoleName),
                                                                   LanguageUtils.getText(heroDefine1.l_nameID),
                                                                   GetFormatName(m_otherAbName, m_otherRoleName));
                    }
                    break;
                case 200002:
                case 200003:
                case 200044:
                case 200017:
                case 200018:
                case 200045:
                    //城市防守战
                    {
                        HeroDefine heroDefine1 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_otherBattleDetail.mainHeroId);
                        desc = string.Format("(缺){0}<color=red>【{1}】</color>攻击了{2}的城市",
                                                                   GetFormatName(m_otherAbName, m_otherRoleName),
                                                                   LanguageUtils.getText(heroDefine1.l_nameID),
                                                                   GetFormatName(m_selfAbName, m_selfRoleName));
                    }
                    break;
                case 200021:
                case 200022:
                case 200023:
                    //攻击联盟建筑
                    {
                        HeroDefine heroDefine1 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_selfBattleDetail.mainHeroId);
                        AllianceBuildingTypeDefine buildingDefine = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)m_otherObjectId);
                        desc = string.Format("(缺){0}<color=green>【{1}】</color>攻击了{2}的{3}！",
                                                                   GetFormatName(m_selfAbName, m_selfRoleName),
                                                                   LanguageUtils.getText(heroDefine1.l_nameID),
                                                                   GetFormatName(m_otherAbName, m_otherRoleName),
                                                                   LanguageUtils.getText(buildingDefine.l_nameId));
                    }
                    break;
                case 200024:
                case 200025:
                case 200026:
                case 200027:
                case 200028:
                case 200029:
                    //联盟建筑防守战
                    {
                        HeroDefine heroDefine1 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_otherBattleDetail.mainHeroId);
                        AllianceBuildingTypeDefine buildingDefine = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>((int)m_otherObjectId);
                        desc = string.Format("(缺){0}<color=red>【{1}】</color>攻击了{2}的{3}！",
                                                                   GetFormatName(m_otherAbName, m_otherRoleName),
                                                                   LanguageUtils.getText(heroDefine1.l_nameID),
                                                                   GetFormatName(m_selfAbName, m_selfRoleName),
                                                                   LanguageUtils.getText(buildingDefine.l_nameId));
                    }
                    break;


            }

            EmailBattleLogItem item1 = new EmailBattleLogItem();
            item1.ItemType = 2;
            item1.Desc = "(缺)战斗简介";
            m_dataList.Add(item1);

            EmailBattleLogItem item2 = new EmailBattleLogItem();
            item2.ItemType = 2;
            item2.Desc = m_nullStr+desc;
            m_dataList.Add(item2);
        }

        //我方阵容
        private void ProcessSelfTroop()
        {
            m_selfReportList.Add(m_selfBattleDetail);

            EmailBattleLogItem item1 = new EmailBattleLogItem();
            item1.ItemType = 2;
            item1.Desc = "(缺)我方阵容";
            m_dataList.Add(item1);

            bool isMul = false; //是否是多人战斗
            if (isMul)//多人
            {
                string content = GetMyTroopDesc(m_selfBattleDetail,
                                         "(缺)【队长】{0}<color=green>【{1}】</color>等级{2}{3}兵力：<color=orange>{4}</color>",
                                         "{0}<color=green>【{1}】</color>等级{2}");

                EmailBattleLogItem item = new EmailBattleLogItem();
                item.ItemType = 2;
                item.Desc = m_nullStr+content;
                m_dataList.Add(item);

                int count = m_selfReportList.Count;
                //队员信息
                for (int i = 1; i < count; i++)
                {
                    string content2 = GetMyTroopDesc(m_selfReportList[i],
                                             "(缺){0}<color=green>【{1}】</color>等级{2}{3}兵力：{4}",
                                             "{4}<color=red>【{5}】</color>等级{6}");

                    EmailBattleLogItem item2 = new EmailBattleLogItem();
                    item2.ItemType = 2;
                    item2.Desc = m_nullStr + content2;
                    m_dataList.Add(item2);
                }
            }
            else//单人
            {
                string content = GetMyTroopDesc(m_selfBattleDetail,
                         "(缺){0}<color=green>【{1}】</color>等级{2}{3}兵力：{4}",
                         "{4}<color=red>【{5}】</color>等级{6}");

                EmailBattleLogItem item = new EmailBattleLogItem();
                item.ItemType = 2;
                item.Desc = m_nullStr+content;
                m_dataList.Add(item);
            }

            //todo 判断是否是箭塔
            //EmailBattleLogItem item3 = new EmailBattleLogItem();
            //item3.ItemType = 2;
            //item3.Desc = string.Format("{0}<color=green>【哨塔】</color>等级<color=orange>{1}</color>生命值:{2}",
            //                         GetFormatName(m_selfAbName, m_otherAbName),
            //                         0,
            //                         0);
            //m_dataList.Add(item3);
        }

        //敌方阵容
        private void ProcessOtherTroop()
        {
            m_otherReportList.Add(m_otherBattleDetail);

            EmailBattleLogItem item1 = new EmailBattleLogItem();
            item1.ItemType = 2;
            item1.Desc = "(缺)敌方阵容";
            m_dataList.Add(item1);

            if (IsMonster()) //如果是怪物
            {
                EmailBattleLogItem item = new EmailBattleLogItem();
                item.ItemType = 2;
                item.Desc = m_nullStr+GetMonsterTroopDesc();
                m_dataList.Add(item);
                return;
            }

            bool isMul = false; //是否多人战斗
            if (isMul)
            {
                string content = GetOtherTroopLeaderDesc(m_otherBattleDetail,
                                                   "(缺)【队长】{0}<color=red>【{1}】</color> 等级{2}{3}兵力：{4}", 
                                                   "(缺){0} <color=red>【{1}】</color> 等级{2}");
                EmailBattleLogItem item = new EmailBattleLogItem();
                item.ItemType = 2;
                item.Desc = m_nullStr+content;
                m_dataList.Add(item);

                //队员信息
                int count = m_otherReportList.Count;
                for (int i = 1; i < count; i++)
                {
                    string content1 = GetOtherTroopMemberDesc(m_otherReportList[i], 
                                                        "(缺){0}<color=red>【{1}】</color> 等级<color=orange>{2}</color>{3}兵力：{4}",
                                                        "(缺){0} 【{1}】 等级{2}");
                    EmailBattleLogItem item2 = new EmailBattleLogItem();
                    item2.ItemType = 2;
                    item2.Desc = m_nullStr+content1;
                    m_dataList.Add(item2);
                }
            }
            else //单人
            {
                string content = GetOtherTroopMemberDesc(m_otherBattleDetail,
                                                       "(缺){0}【{1}】等级{2}{3}兵力:{4}",
                                                       "(缺){0}【{1}】等级{2}");
                EmailBattleLogItem item = new EmailBattleLogItem();
                item.ItemType = 2;
                item.Desc = m_nullStr+content;
                m_dataList.Add(item);
            }


            //todo 判断是否是箭塔
            //EmailBattleLogItem item3 = new EmailBattleLogItem();
            //item3.ItemType = 2;
            //item3.Desc = string.Format("{0}<color=green>【哨塔】</color>等级<color=orange>{1}</color>生命值:{2}",
            //                         GetFormatName(m_selfAbName, m_otherAbName),
            //                         0,
            //                         0);
            //m_dataList.Add(item3);
        }

        //回合信息
        private void ProcessRoundData()
        {
            BattleReportWithObjectIndex hurtDetail = m_battleReportEx.battleReport[m_targetObjectIndex];

            if (hurtDetail.battleDamageHeal == null)
            {
                return;
            }
            List<BattleReport> hurtList = hurtDetail.battleDamageHeal;
            for (int i = 0; i < hurtList.Count; i++)
            {

            }
            //普通攻击

            //反击伤害
        }

        //战斗结束信息
        private void ProcessFightEnd()
        {
            EmailBattleLogItem item = new EmailBattleLogItem();
            item.ItemType = 2;
            item.Desc = "(缺)战斗结束";
            m_dataList.Add(item);

            GetTotalArmyTotalNum(m_selfBattleDetail.soldierDetail[m_otherBattleDetail.objectIndex], out long treatment, out long oMinor, out long oDie, out long oHurt, out long selfRemain, out long towerHurt);
            GetTotalArmyTotalNum(m_otherBattleDetail.soldierDetail[m_selfBattleDetail.objectIndex], out long treatment1, out long oMinor1, out long oDie1, out long oHur1t, out long otherRemain, out long towerHurt1);

            if (selfRemain > 0 && otherRemain > 0)
            {
                string desc = LanguageUtils.getText(200540);

                EmailBattleLogItem item1 = new EmailBattleLogItem();
                item1.ItemType = 2;
                item1.Desc = m_nullStr + desc;
                m_dataList.Add(item);
            }
            else if (selfRemain > 0) //敌方溃败
            {
                //敌方
                HeroDefine heroDefine2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_otherBattleDetail.mainHeroId);
                string desc2 = LanguageUtils.getTextFormat(200543,
                                                          LanguageUtils.getText(heroDefine2.l_nameID),
                                                          ClientUtils.FormatComma(otherRemain));
                EmailBattleLogItem item2 = new EmailBattleLogItem();
                item2.ItemType = 2;
                item2.Desc = m_nullStr + desc2;
                m_dataList.Add(item2);

                //我方
                HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_selfBattleDetail.mainHeroId);
                string desc = LanguageUtils.getTextFormat(200544,
                                                          LanguageUtils.getText(heroDefine.l_nameID),
                                                          ClientUtils.FormatComma(selfRemain));

                EmailBattleLogItem item1 = new EmailBattleLogItem();
                item1.ItemType = 2;
                item1.Desc = m_nullStr + desc;
                m_dataList.Add(item1);
            }
            else if (otherRemain > 0) //我方溃败
            {
                //我方
                HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_selfBattleDetail.mainHeroId);
                string desc = LanguageUtils.getTextFormat(200541,
                                                          LanguageUtils.getText(heroDefine.l_nameID),
                                                          ClientUtils.FormatComma(selfRemain));

                EmailBattleLogItem item1 = new EmailBattleLogItem();
                item1.ItemType = 2;
                item1.Desc = m_nullStr + desc;
                m_dataList.Add(item1);

                //敌方
                HeroDefine heroDefine2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_otherBattleDetail.mainHeroId);
                string desc2 = LanguageUtils.getTextFormat(200542,
                                                          LanguageUtils.getText(heroDefine2.l_nameID),
                                                          ClientUtils.FormatComma(otherRemain));
                EmailBattleLogItem item2 = new EmailBattleLogItem();
                item2.ItemType = 2;
                item2.Desc = m_nullStr + desc2;
                m_dataList.Add(item2);
            }
            else
            {
                Debug.LogError("双方全部战死 @杜宏彦");
            }
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
                    towerHurt += element.die;
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

        //敌方是否是怪物
        private bool IsMonster()
        {
            long emailId = m_emailInfo.emailId;
            bool isMonster = false;
            switch (emailId)
            {
                case 200006:
                case 200007:
                case 200009:
                case 200010:
                case 200039:
                case 200040:
                case 200041:
                case 200042:
                case 200043:
                    isMonster = true;
                    break;
            }
            return isMonster;
        }

        private string GetFormatName(string allianceAbName, string name)
        {
            if (string.IsNullOrEmpty(allianceAbName))
            {
                return name;
            }
            else
            {
                return LanguageUtils.getTextFormat(300030, allianceAbName, name);
            }
        }

        //获取怪物描述
        private string GetMonsterTroopDesc()
        {
            HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_otherBattleDetail.mainHeroId);
            MonsterDefine monsterDefine = CoreUtils.dataService.QueryRecord<MonsterDefine>((int)m_otherBattleDetail.monsterId);
            //MonsterTroopsDefine monsterTroopsDefine = CoreUtils.dataService.QueryRecord<MonsterTroopsDefine>((int)monsterDefine.monsterTroopsId);
            //是否有副将
            string deputyHeroStr = "";
            if (m_otherBattleDetail.deputyHeroId > 0)
            {
                HeroDefine heroDefine2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_otherBattleDetail.deputyHeroId);
                deputyHeroStr = string.Format("(缺){0}<color=green>【{1}】</color>等级{2}",
                                              LanguageUtils.getText(200515),
                                              LanguageUtils.getText(heroDefine2.l_nameID),
                                              m_otherBattleDetail.deputyHeroLevel);
            }

            string content = string.Format("{0}<color=red>【{1}】</color> 等级{2}{3}兵力:{4}",
                                    LanguageUtils.getText(monsterDefine.l_nameId),
                                    LanguageUtils.getText(heroDefine.l_nameID),
                                    m_otherBattleDetail.mainHeroLevel,
                                    deputyHeroStr,
                                    ClientUtils.FormatComma(m_otherBattleDetail.beginArmyCount));
            return content;
        }

        //获取我方队伍描述
        private string GetMyTroopDesc(BattleReportExDetail info, string lan1, string lan2)
        {
            HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)info.mainHeroId);
            //是否有副将
            string deputyHeroStr = "";
            if (info.deputyHeroId > 0)
            {
                HeroDefine heroDefine2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)info.deputyHeroId);
                deputyHeroStr = string.Format(lan2,
                                              LanguageUtils.getText(200515),
                                              LanguageUtils.getText(heroDefine2.l_nameID),
                                              info.deputyHeroLevel);

            }

            string desc = string.Format(lan1,
                                        GetFormatName(m_selfAbName, m_selfRoleName),
                                        LanguageUtils.getText(heroDefine.l_nameID),
                                        info.mainHeroLevel,
                                        deputyHeroStr,
                                        ClientUtils.FormatComma(info.beginArmyCount));
            return desc;
        }

        //获取敌方队长队伍描述
        private string GetOtherTroopLeaderDesc(BattleReportExDetail info, string lan1, string lan2)
        {
            HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)info.mainHeroId);
            //是否有副将
            string deputyHeroStr = "";
            if (info.deputyHeroId > 0)
            {
                HeroDefine heroDefine2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)info.deputyHeroId);
                deputyHeroStr = string.Format(lan2,
                                              LanguageUtils.getText(200515),
                                              LanguageUtils.getText(heroDefine2.l_nameID),
                                              info.deputyHeroLevel);
            }
            string content = string.Format(lan1,
                                    GetFormatName(m_otherAbName, m_otherRoleName),
                                    LanguageUtils.getText(heroDefine.l_nameID),
                                    info.mainHeroLevel,
                                    deputyHeroStr,
                                    ClientUtils.FormatComma(info.beginArmyCount));
            return content;
        }

        //获取敌方队员描述
        private string GetOtherTroopMemberDesc(BattleReportExDetail info, string lan1, string lan2)
        {
            HeroDefine heroDefine = CoreUtils.dataService.QueryRecord<HeroDefine>((int)info.mainHeroId);
            //是否有副将
            string deputyHeroStr = "";
            if (info.deputyHeroId > 0)
            {
                HeroDefine heroDefine2 = CoreUtils.dataService.QueryRecord<HeroDefine>((int)info.deputyHeroId);
                deputyHeroStr = string.Format(lan2,
                                               LanguageUtils.getText(200515),
                                               LanguageUtils.getText(heroDefine2.l_nameID),
                                               info.deputyHeroLevel);
            }
            string content = string.Format(lan1,
                                            GetFormatName(m_otherAbName, info.name),
                                            LanguageUtils.getText(heroDefine.l_nameID),
                                            info.mainHeroLevel,
                                            deputyHeroStr,
                                            ClientUtils.FormatComma(info.beginArmyCount));
            return content;
        }

        #endregion
    }
}
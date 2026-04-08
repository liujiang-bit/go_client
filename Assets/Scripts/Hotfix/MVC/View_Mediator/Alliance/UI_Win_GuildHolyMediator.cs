// =============================================================================== 
// Author              :    林光志
// Create Time         :    Monday, April 20, 2020
// Update Time         :    Monday, April 20, 2020
// Class Description   :    UI_Win_GuildHolyMediator
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
    
    public enum StrongHoldGroup
    {
        HolyPlace = 1,    // 圣所
        Chancel = 2,    // 圣坛
        Shrine = 3,    // 圣祠
        Fane = 4,        // 神庙
        Level1 = 10,    // 关卡1
        Level2 = 11,    // 关卡2
        Level3 = 12,    // 关卡3
    }
    
    public class UI_Win_GuildHolyMediator : GameMediator {

        public enum StrongHoldItemShowType
        {
            Title = 1,    // 标题
            GuildHoly = 2,    // 建筑
            GuildHolyWithBuff = 3,    // 有buff的建筑
        }
       
        
        // 滑动列表数据
        public class StrongHoldListData
        {
            public StrongHoldItemShowType showType;
            public int title;
            public List<StrongHoldCardData> cardDataLst;

            public StrongHoldListData(StrongHoldItemShowType showType, int title, List<StrongHoldCardData> cardDataLst)
            {
                this.showType = showType;
                this.title = title;
                this.cardDataLst = cardDataLst;
            }
        }
        
//        public class SimulateServerData
//        {
//            public int strongHoldTypeId;
//            public Vector2 pos;
//        }
        
        #region Member
        public static string NameMediator = "UI_Win_GuildHolyMediator";
        
        // 奇观建筑类型多语言
        Dictionary<int, int> m_strongHoldLan = new Dictionary<int, int>()
        {
            {1,500773},    // 圣所
            {2,500772},    // 圣坛
            {3,500771},    // 圣祠
            {4,500770},    // 神庙 
            {10,500774},    // 关卡
            {11,500774},
            {12,500774},
            
        };
        
        Dictionary<int, string> m_showTypeItemPrefabName = new Dictionary<int, string>()
        {
            {1, "UI_Item_GuildHolyLine"},
            {2, "UI_LC_GuildHoly1"},
            {3, "UI_LC_GuildHoly2"},
        };

        Dictionary<int, StrongHoldListData> m_lstDataDict = new Dictionary<int, StrongHoldListData>();
        private List<StrongHoldListData> m_lstViewData = new List<StrongHoldListData>();

        private AllianceProxy allianceProxy;
        
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private List<StrongHoldTypeDefine> m_strongHoldTypeDefines;
        private List<StrongHoldDataDefine> m_strongHoldDataDefines;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildHolyMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildHolyView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.AllianceHolyLandUpdate,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AllianceHolyLandUpdate:
                    UpdateListView();
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

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.setCloseHandle(onClose);
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceHoly);
        }

        protected override void BindUIData()
        {
            allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_strongHoldTypeDefines = CoreUtils.dataService.QueryRecords<StrongHoldTypeDefine>();
            m_strongHoldDataDefines = CoreUtils.dataService.QueryRecords<StrongHoldDataDefine>();
            
            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
            
            
        }
       
        #endregion
        
        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            InitViewList();
        }
        
        private void InitViewList()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            functab.GetItemPrefabName = GetItemPrefabName;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);

            UpdateListView();
        }
        
        /*

        /// <summary>
        /// 模拟添加服务器数据
        /// </summary>
        private void AddSimulateServerData()
        {
            List<SimulateServerData> tmpServerDatas = new List<SimulateServerData>()
            {
                new SimulateServerData(){strongHoldTypeId = 10004, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 10005, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 10006, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 10007, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 20001, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 20002, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 20003, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 20004, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 20005, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 30001, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 30002, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 40001, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 90011, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 90012, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 90021, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 90022, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 90023, pos = new Vector2(0,0)},
                new SimulateServerData(){strongHoldTypeId = 90031, pos = new Vector2(0,0)},
            };

           
        }
        
        */


        private void UpdateListView()
        {
            Dictionary<int, GuildHolyLandInfo> holyLandInfos = allianceProxy.GetGuildHolyLandInfos();

            m_lstViewData.Clear();
            m_lstDataDict.Clear();
            
            foreach (var dict in holyLandInfos)
            {
                GuildHolyLandInfo value = dict.Value;
                int strongHoldTypeId = (int)value.strongHoldId;
                int status = (int) value.status;
                Vector2 pos = new Vector2(value.pos.x, value.pos.y);

                StrongHoldDataDefine dataDefine = GetStrongHoldDataDefine(strongHoldTypeId);
                if (dataDefine == null)
                {
                    CoreUtils.logService.Warn($"圣地=== strongHoldTypeId:[{strongHoldTypeId}]  找不到StrongHoldDataDefine对应配置");
                    continue;
                }
                
                StrongHoldTypeDefine typeDefine = GetStrongHoldTypeDefine(dataDefine.type);
                if (typeDefine == null)
                {
                    CoreUtils.logService.Warn($"圣地=== strongHoldTypeId:[{strongHoldTypeId}]  找不到StrongHoldTypeDefine对应配置");
                    continue;
                }

                int groupLan = m_strongHoldLan[typeDefine.@group];    // 按标题语言划分
                
                StrongHoldListData strongHoldListData;
                if (!m_lstDataDict.TryGetValue(groupLan, out strongHoldListData))
                {
                    // 创建一个标题
                    StrongHoldListData titleData = new StrongHoldListData(StrongHoldItemShowType.Title, groupLan, null);
                    StrongHoldItemShowType showType = GetItemShowType((StrongHoldGroup)typeDefine.@group);
                    
                    strongHoldListData = new StrongHoldListData(showType, groupLan, new List<StrongHoldCardData>());
                    
                    m_lstViewData.Add(titleData);
                    m_lstViewData.Add(strongHoldListData);
                    m_lstDataDict.Add(groupLan, strongHoldListData);
                }
                
                StrongHoldCardData cardData = new StrongHoldCardData();
                cardData.pos = pos;
                cardData.imgShow = typeDefine.imgShow;
                cardData.nameId = typeDefine.l_nameId;
                cardData.state = status == 1 ? StrongHoldCardData.StrongHoldState.Normal : StrongHoldCardData.StrongHoldState.Fighting;
                cardData.descId = typeDefine.l_desc;
                cardData.buffDataLst.Add(typeDefine.buffData1);
                cardData.buffDataLst.Add(typeDefine.buffData2);
                cardData.buffDataLst.Add(typeDefine.buffData3);
                strongHoldListData.cardDataLst.Add(cardData);
            }
            
            view.m_sv_list_view_ListView.FillContent(m_lstViewData.Count);
            view.m_sv_list_view_ListView.ForceRefresh();

            bool isNoData = m_lstViewData.Count == 0 ? true : false;
            view.m_lbl_text_LanguageText.gameObject.SetActive(isNoData);
            view.m_sv_list_view_ListView.gameObject.SetActive(!isNoData);
        }

        private StrongHoldItemShowType GetItemShowType(StrongHoldGroup strongHoldGroup)
        {
            StrongHoldItemShowType type = StrongHoldItemShowType.GuildHoly;
            if (strongHoldGroup == StrongHoldGroup.Shrine || strongHoldGroup == StrongHoldGroup.Chancel || strongHoldGroup == StrongHoldGroup.HolyPlace)
            {
                type = StrongHoldItemShowType.GuildHolyWithBuff;
            }
            
            return type;
        }

        private StrongHoldTypeDefine GetStrongHoldTypeDefine(int typeId)
        {
            for (int i = 0; i < m_strongHoldTypeDefines.Count; i++)
            {
                if (m_strongHoldTypeDefines[i].ID == typeId)
                {
                    return m_strongHoldTypeDefines[i];
                }
            }

            return null;
        }

        private StrongHoldDataDefine GetStrongHoldDataDefine(int typeId)
        {
            for (int i = 0; i < m_strongHoldDataDefines.Count; i++)
            {
                if (m_strongHoldDataDefines[i].ID == typeId)
                {
                    return m_strongHoldDataDefines[i];
                }
            }
            
            return null;
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            if (!listItem.isInit)
            {
                listItem.isInit = true;
            }

            if (listItem.index < 0 || listItem.index >= m_lstViewData.Count)
            {
                return;
            }

            StrongHoldListData listData = m_lstViewData[listItem.index];
            
            switch (listData.showType)
            {
                case StrongHoldItemShowType.Title:
                    UI_Item_GuildHolyLine_SubView guildHolyLineSubView = new UI_Item_GuildHolyLine_SubView(listItem.go.GetComponent<RectTransform>());
                    guildHolyLineSubView.Refresh(listData.title);
                    break;
                case StrongHoldItemShowType.GuildHoly:
                    UI_LC_GuildHoly1_SubView guildHoly1SubView = new UI_LC_GuildHoly1_SubView(listItem.go.GetComponent<RectTransform>());
                    guildHoly1SubView.Refresh(listData.cardDataLst);
                    view.m_sv_list_view_ListView.UpdateItemSize(listItem.index, guildHoly1SubView.preferredHeight);
                    break;
                case StrongHoldItemShowType.GuildHolyWithBuff:
                    UI_LC_GuildHoly2_SubView guildHoly2SubView = new UI_LC_GuildHoly2_SubView(listItem.go.GetComponent<RectTransform>());
                    guildHoly2SubView.Refresh(listData.cardDataLst);
                    view.m_sv_list_view_ListView.UpdateItemSize(listItem.index, guildHoly2SubView.preferredHeight);
                    break;
            }
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            if (item.index < 0 || item.index >= m_lstViewData.Count)
            {
                CoreUtils.logService.Warn($"圣祠 === 数组越界   index:[{item.index}] ");
                return "";
            }
            
            var viewData = m_lstViewData[item.index];
            return m_showTypeItemPrefabName[(int) viewData.showType];
        }
    }
}
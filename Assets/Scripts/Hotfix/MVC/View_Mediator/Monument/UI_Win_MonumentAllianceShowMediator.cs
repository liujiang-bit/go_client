// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月30日
// Update Time         :    2020年4月30日
// Class Description   :    UI_Win_MonumentAllianceShowMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
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
    public class UI_Win_MonumentAllianceShowMediator : GameMediator
    {

        public int lan_allianceDissolve = 183015;    // 联盟已解散
        public int lan_None = 570029;    // 无
        public int lan_abb = 300030;    // 联盟简称
        
        #region Member
        public static string NameMediator = "UI_Win_MonumentAllianceShowMediator";

        private Role_GetMonument.response.MonumentList m_monumentList;
        private List<Role_GetMonument.response.MonumentList.GuildRank> guildRank = new List<Role_GetMonument.response.MonumentList.GuildRank> ();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        public static string[] RankingTop3IconName = new string[]
            {"ui_common[img_com_rank1]", "ui_common[img_com_rank2]", "ui_common[img_com_rank3]"};

        private int m_allianceRankDesc;
        private bool m_isNoData;
        
        
        #endregion

        //IMediatorPlug needs
        public UI_Win_MonumentAllianceShowMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_MonumentAllianceShowView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
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
            m_isNoData = false;
            AllianceProxy allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            object[] objects = view.data as object[];
            if (objects != null && objects.Length >= 2)
            {
                try
                {
                    m_monumentList = objects[0] as Role_GetMonument.response.MonumentList;
                    m_allianceRankDesc = (int)objects[1];
                }
                catch (Exception e)
                {
                    CoreUtils.logService.Warn($"纪念碑 联盟排行榜   获取allianceRankDesc字段错误！    ");   
                }
            }
            
            guildRank.Clear();
            if (m_monumentList != null && m_monumentList.HasGuildRank)
            {
                guildRank.AddRange(m_monumentList.guildRank);
                guildRank.Sort((a, b) => { return (int)(b.count - a.count); });    
            }
            
            
            
//            else
//            {
//                for (int i = 0; i < 10; i++)
//                {
//                    Role_GetMonument.response.MonumentList.GuildRank tmp =
//                        new Role_GetMonument.response.MonumentList.GuildRank();
//
//                    tmp.name = "name_" + i;
//                    tmp.count = i;
//                    tmp.abbreviationName = "abb_" + i;
//                    if (i % 2 == 0)
//                    {
//                        tmp.name = "";
//                    }
//                    
//                    tmp.signs = new List<long>(){1,2,3,4};
//                    guildRank.Add(tmp);
//                }
//            }    // 没有数据测试，自己创造条件。测试代码不影响功能。

            // 策划需求，没有数据时，显示一条默认数据
            if (guildRank.Count <= 0)
            {
                Role_GetMonument.response.MonumentList.GuildRank defaultData = new Role_GetMonument.response.MonumentList.GuildRank();
                defaultData.count = 0;
                defaultData.signs = allianceProxy.GetDefaultGuildFlagSign();
                guildRank.Add(defaultData);
                m_isNoData = true;
            }

            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {

        }
       
        #endregion
        
        
        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            
            InitListView();
        }

        private void InitListView()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = ListViewItemByIndex;
            view.m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
            view.m_sv_list_view_ListView.FillContent(guildRank.Count);
            view.m_sv_list_view_ListView.ForceRefresh();
        }
        
        
        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            UI_Item_AllianceProjectView projectView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_AllianceProjectView>(listItem.go);
            if (!listItem.isInit)
            {
                listItem.isInit = true;
            }

            if (listItem.index < 0 || listItem.index >= guildRank.Count)
                return;

            int rankIndex = listItem.index + 1;
            RefreshAllianceProject(projectView, guildRank[listItem.index], rankIndex);
        }

        /// <summary>
        /// 刷新单条联盟数据
        /// </summary>
        /// <param name="item"></param>
        /// <param name="guildRank"></param>
        /// <param name="rankIndex"></param>
        private void RefreshAllianceProject(UI_Item_AllianceProjectView item, Role_GetMonument.response.MonumentList.GuildRank guildRank, int rankIndex)
        {
            item.m_lbl_rank_LanguageText.text = rankIndex.ToString();
            item.m_lbl_rank_LanguageText.gameObject.SetActive(true);
            item.m_img_rank_PolygonImage.gameObject.SetActive(false);
            if (rankIndex > 0 && rankIndex <= 3)
            {
                item.m_lbl_rank_LanguageText.gameObject.SetActive(false);
                item.m_img_rank_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(item.m_img_rank_PolygonImage, RankingTop3IconName[rankIndex - 1]);
            }
            
            item.m_UI_GuildFlag.setData(guildRank.signs);

            if (m_isNoData)
            {
                item.m_lbl_allianceName_LanguageText.text = LanguageUtils.getText(lan_None);
            }
            else if (string.IsNullOrEmpty(guildRank.guildName))
            {
                item.m_lbl_allianceName_LanguageText.text = LanguageUtils.getText(lan_allianceDissolve);
            }
            else
            {
                item.m_lbl_allianceName_LanguageText.text =
                    LanguageUtils.getTextFormat(lan_abb, guildRank.abbreviationName, guildRank.guildName);
            }

            item.m_lbl_project_LanguageText.text = LanguageUtils.getTextFormat(m_allianceRankDesc, guildRank.count.ToString("N0"));
        }
    }
}
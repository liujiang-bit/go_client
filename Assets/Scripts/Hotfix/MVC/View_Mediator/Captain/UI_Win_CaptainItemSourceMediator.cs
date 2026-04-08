// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月23日
// Update Time         :    2020年4月23日
// Class Description   :    UI_Win_CaptainItemSourceMediator
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
using Data;
using UnityEngine.UI;

namespace Game {

    //统帅升级道具来源类型
    public enum EnumCaptainLevelResourceType
    {
        /// <summary>
        /// 技能升级
        /// </summary>
        SkillLevel = 1,
        /// <summary>
        /// 星级升级
        /// </summary>
        StarLevel = 2,
        /// <summary>
        /// 装备锻造
        /// </summary>
        EquipForge = 3,
        /// <summary>
        /// 召唤来源
        /// </summary>
        Summer = 4,
    }

    public class CaptainItemSourceViewData
    {
        public EnumCaptainLevelResourceType ResourceType { get; set; }
        public int CaptainId { get; set; }
        public int RequireItemId { get; set; }
        public int RequireItemNum { get; set; }
    }

    public class UI_Win_CaptainItemSourceMediator : GameMediator {
        #region Member

        public static string NameMediator = "UI_Win_CaptainItemSourceMediator";

        private CaptainItemSourceViewData m_data = null;
        private int m_requireItemId = 0;
        private int m_requireItemNum = 0;
        private List<int> m_itemGetResourceData = new List<int>();

        #endregion

        //IMediatorPlug needs
        public UI_Win_CaptainItemSourceMediator(object viewComponent ):base(NameMediator, viewComponent ) {}

        public UI_Win_CaptainItemSourceView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.SkillUpSourceRefresh,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.SkillUpSourceRefresh:
                    if (m_data == null) return;
                    switch (m_data.ResourceType)
                    {
                        case EnumCaptainLevelResourceType.SkillLevel:
                        {
                            refreshSkillLevelUI();
                        }
                            break;
                        case EnumCaptainLevelResourceType.StarLevel:
                        case EnumCaptainLevelResourceType.Summer:
                            {
                            refreshStarLevelUI();
                        }
                            break;
                        case EnumCaptainLevelResourceType.EquipForge:
                        {
                            refreshEquipForgeUI();
                        }
                            break;
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
            AppFacade.GetInstance().SendNotification(CmdConstant.SetHeroSkillLineSort,6000);
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_data = view.data as CaptainItemSourceViewData;
            if (m_data == null) return;
            switch (m_data.ResourceType)
            {
                case EnumCaptainLevelResourceType.SkillLevel:
                    {
                        refreshSkillLevelUI();
                    }
                    break;
                case EnumCaptainLevelResourceType.StarLevel:
                case EnumCaptainLevelResourceType.Summer:
                    {
                        refreshStarLevelUI();
                    }
                    break;
                case EnumCaptainLevelResourceType.EquipForge:
                    {
                        refreshEquipForgeUI();
                    }
                    break;
            }
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_requireItemId);
            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if (itemCfg == null || configDefine == null) return;
            foreach (var id in itemCfg.get)
            {
                if (m_data.ResourceType == EnumCaptainLevelResourceType.Summer && id == configDefine.itemGetHide)
                {
                    continue;
                }
                m_itemGetResourceData.Add(id);
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.AddCloseEvent(OnCloseButtonClicked);
        }

        protected override void BindUIData()
        {
            ClientUtils.PreLoadRes(view.gameObject, view.m_sv_list_ListView.ItemPrefabDataList, OnItemPrefabLoadFinish);
        }

        #endregion

        private void OnCloseButtonClicked()
        {
            CoreUtils.uiManager.CloseUI(UI.s_captainItemSource);
        }
        
        private void OnItemPrefabLoadFinish(Dictionary<string, GameObject> dict)
        {
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ItemEnter;
            view.m_sv_list_ListView.SetInitData(dict, funcTab);
            view.m_sv_list_ListView.FillContent(m_itemGetResourceData != null ? m_itemGetResourceData.Count : 0);
        }

        private void ItemEnter(ListView.ListItem item)
        {
            if (item == null || m_itemGetResourceData == null || item.index >= m_itemGetResourceData.Count) return;
            int itemGetId = m_itemGetResourceData[item.index];
            UI_Item_CaptainStatueSourceItem_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_CaptainStatueSourceItem_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_CaptainStatueSourceItem_SubView;
            }
            if (subView == null) return;
            subView.refresh(itemGetId, m_requireItemId);            
        }

        private void refreshSkillLevelUI()
        {
            view.m_pb_rogressBar_GameSlider.gameObject.SetActive(true);
            view.m_lbl_text_LanguageText.gameObject.SetActive(false);
            view.m_pl_itemMes_ArabLayoutCompment.gameObject.SetActive(false);

            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(166074));

            var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            if (heroProxy == null) return;
            var hero = heroProxy.GetHeroByID(m_data.CaptainId);
            if (hero == null) return;
            int requireCount = hero.GetSkillLevelUpCostItemNum();
            m_requireItemId = hero.config.getItem;
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (bagProxy == null) return;
            int itemCount = (int)bagProxy.GetItemNum(m_requireItemId);
            view.m_pb_rogressBar_GameSlider.value = requireCount == 0 ? 1 : itemCount * 1.0f / requireCount;
            if(!hero.IsAllSkillMax())
            {
                view.m_lbl_num_LanguageText.text = $"{ClientUtils.FormatComma(itemCount)}/{ClientUtils.FormatComma(requireCount)}";
            }
            else
            {
                view.m_lbl_num_LanguageText.text = $"{ClientUtils.FormatComma(itemCount)}";
            }

            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(hero.config.l_nameID);

            refreshItemIcon();
        }

        private void refreshStarLevelUI()
        {
            view.m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
            view.m_lbl_text_LanguageText.gameObject.SetActive(true);
            view.m_pl_itemMes_ArabLayoutCompment.gameObject.SetActive(false);

            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(166074));

            m_requireItemId = m_data.RequireItemId;

            refreshItemIcon();
        }

        private void refreshEquipForgeUI_TypeGroup_1()
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_requireItemId);
            if (itemCfg == null) return;

            view.m_pb_rogressBar_GameSlider.gameObject.SetActive(true);
            view.m_lbl_text_LanguageText.gameObject.SetActive(false);
            view.m_pl_itemMes_ArabLayoutCompment.gameObject.SetActive(false);

            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(145063));

            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (bagProxy == null) return;

            int itemCount = (int)bagProxy.GetItemNum(m_requireItemId);
            view.m_pb_rogressBar_GameSlider.value = m_requireItemNum == 0 ? 1 : itemCount * 1.0f / m_requireItemNum;

            PolygonImage fill = view.m_pb_rogressBar_GameSlider.fillRect.GetComponent<PolygonImage>();
            if (itemCount < m_requireItemNum)
            {
                ClientUtils.LoadSprite(fill, "ui_common[pb_com_1000_2]");
            }
            else
            {
                ClientUtils.LoadSprite(fill, "ui_common[pb_com_1000_4]");
            }

            view.m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(300001, itemCount, m_requireItemNum);

            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);

            refreshItemIcon();
        }

        private void refreshEquipForgeUI_TypeGroup_3()
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_requireItemId);
            if (itemCfg == null) return;

            view.m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
            view.m_lbl_text_LanguageText.gameObject.SetActive(false);
            view.m_pl_itemMes_ArabLayoutCompment.gameObject.SetActive(true);

            view.m_UI_Model_Window_Type1.setWindowTitle(LanguageUtils.getText(145063));

            view.m_lbl_item_name_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);
            view.m_lbl_item_desc_LanguageText.text = LanguageUtils.getText(itemCfg.l_desID);

            refreshItemIcon();
        }

        private void refreshEquipForgeUI()
        {
            m_requireItemId = m_data.RequireItemId;
            m_requireItemNum = m_data.RequireItemNum;

            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_requireItemId);
            if (itemCfg == null) return;

            if (itemCfg.typeGroup == 1)
            {
                refreshEquipForgeUI_TypeGroup_1();
            }
            else if (itemCfg.typeGroup == 3)
            {
                refreshEquipForgeUI_TypeGroup_3();
            }
            else
            {
                refreshEquipForgeUI_TypeGroup_3();
                Debug.LogErrorFormat("锻造材料获取来源配置错误：ID:{0} typeGroup{1}", itemCfg.ID, itemCfg.typeGroup);
            }
        }

        private void refreshItemIcon()
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_requireItemId);
            if (itemCfg == null) return;

            ClientUtils.LoadSprite(view.m_UI_Model_Item.m_img_icon_PolygonImage, itemCfg.itemIcon);
            ClientUtils.LoadSprite(view.m_UI_Model_Item.m_img_quality_PolygonImage, view.m_UI_Model_Item.GetQualityImg(itemCfg.quality));
        }
    }
}
// =============================================================================== 
// Author              :    geguoxing
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    UI_Win_GainEffectMediator
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
using System;

namespace Game
{
    public class GainEffectAttr
    {
        public bool Total;
        public string Name;
        public string Value;
        public string iconID;
    }

    public class UI_Win_GainEffectMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Win_GainEffectMediator";

        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private List<GainEffectAttr> m_jingjiGainList = new List<GainEffectAttr>();
        private List<GainEffectAttr> m_junshiGainList = new List<GainEffectAttr>();

        private bool m_assetsReady = false;
        private int m_page = 0;//0经济 1，军事
        #endregion

        //IMediatorPlug needs
        public UI_Win_GainEffectMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public UI_Win_GainEffectView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {

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

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {

        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            if (view.data is int)
            {
                m_page = (int)view.data;
            }
            AttrDataProcess();
            m_preLoadRes.AddRange(view.m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                m_assetsReady = true;
                {
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ItemEnterJingji;
                    funcTab.GetItemPrefabName = OnGetItemPrefabNameJingji;
                    funcTab.GetItemSize = OnGetItemSizeJingji;
                    view.m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_list_view_ListView.FillContent(m_jingjiGainList.Count);
                }
                {
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ItemEnterjunshi;
                    funcTab.GetItemPrefabName = OnGetItemPrefabNamejunshi;
                    funcTab.GetItemSize = OnGetItemSizejunshi;
                    view.m_sv_list_view2_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_list_view2_ListView.FillContent(m_junshiGainList.Count);
                }
                if (m_page == 0)
                {
                    ShowBuildingGroupTypeList(EnumPageType.JINGJI);
                }
                else
                {
                    ShowBuildingGroupTypeList(EnumPageType.JUNSHI);
                }
            });
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_PageButton_Side1.m_btn_btn_GameButton.onClick.AddListener(OnJingjiBtnClick);
            view.m_UI_Model_PageButton_Side2.m_btn_btn_GameButton.onClick.AddListener(OnJunshiBtnClick);
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(OnCloseBtnClick);
        }

        protected override void BindUIData()
        {
    
        }

        #endregion

        #region 点击事件
        private void OnCloseBtnClick()
        {
            CoreUtils.uiManager.CloseUI(UI.s_GainEffect);
        }

        private void OnJingjiBtnClick()
        {
            if (m_assetsReady)
            {
                ShowBuildingGroupTypeList(EnumPageType.JINGJI);
            }
        }

        private float OnGetItemSizeJingji(ListView.ListItem listItem)
        {
            if (!m_jingjiGainList[listItem.index].Total)
            {
                return m_assetDic["UI_Item_GainEffectSingle"].GetComponent<RectTransform>().sizeDelta.y;
            }
            else
            {
                return m_assetDic["UI_Item_GainEffectTotal"].GetComponent<RectTransform>().sizeDelta.y;
            }
        }

        private string OnGetItemPrefabNameJingji(ListView.ListItem listItem)
        {
            if (!m_jingjiGainList[listItem.index].Total)
            {
                return "UI_Item_GainEffectSingle";
            }
            else
            {
                return "UI_Item_GainEffectTotal";
            }

        }

        private float OnGetItemSizejunshi(ListView.ListItem listItem)
        {
            if (!m_junshiGainList[listItem.index].Total)
            {
                return m_assetDic["UI_Item_GainEffectSingle"].GetComponent<RectTransform>().sizeDelta.y;
            }
            else
            {
                return m_assetDic["UI_Item_GainEffectTotal"].GetComponent<RectTransform>().sizeDelta.y;
            }
        }

        private string OnGetItemPrefabNamejunshi(ListView.ListItem listItem)
        {
            if (!m_junshiGainList[listItem.index].Total)
            {
                return "UI_Item_GainEffectSingle";
            }
            else
            {
                return "UI_Item_GainEffectTotal";
            }

        }

        private void OnJunshiBtnClick()
        {
            if (m_assetsReady)
            {
                ShowBuildingGroupTypeList(EnumPageType.JUNSHI);
         
            }
        }

        #endregion
        void ItemEnterJingji(ListView.ListItem scrollItem)
        {
            GainEffectAttr gainEffectAttr = m_jingjiGainList[scrollItem.index];

            if (gainEffectAttr != null)
            {
                if (!gainEffectAttr.Total)
                {
                    UI_Item_GainEffectSingleView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GainEffectSingleView>(scrollItem.go);
                    itemView.m_lbl_name_LanguageText.text = gainEffectAttr.Name;
                    itemView.m_lbl_val_LanguageText.text =  gainEffectAttr.Value;
                    itemView.gameObject.name = gainEffectAttr.Name;
                }
                else
                {
                    UI_Item_GainEffectTotalView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GainEffectTotalView>(scrollItem.go);
                    ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, gainEffectAttr.iconID);
                    itemView.m_lbl_name_LanguageText.text = gainEffectAttr.Name;
                    itemView.m_lbl_val_LanguageText.text = gainEffectAttr.Value;
                    itemView.gameObject.name = gainEffectAttr.Name;
                }
            }

        }

        void ItemEnterjunshi(ListView.ListItem scrollItem)
        {
            GainEffectAttr gainEffectAttr = m_junshiGainList[scrollItem.index];

            if (gainEffectAttr != null)
            {
                if (!gainEffectAttr.Total)
                {
                    UI_Item_GainEffectSingleView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GainEffectSingleView>(scrollItem.go);
                    itemView.m_lbl_name_LanguageText.text = gainEffectAttr.Name;
                    itemView.m_lbl_val_LanguageText.text = gainEffectAttr.Value;
                    itemView.gameObject.name = gainEffectAttr.Name;
                }
                else
                {
                    UI_Item_GainEffectTotalView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GainEffectTotalView>(scrollItem.go);
                    ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, gainEffectAttr.iconID);
                    itemView.m_lbl_name_LanguageText.text = gainEffectAttr.Name;
                    itemView.m_lbl_val_LanguageText.text = gainEffectAttr.Value;
                    itemView.gameObject.name = gainEffectAttr.Name;
                }
            }

        }

        void ShowBuildingGroupTypeList(EnumPageType buildingGroupType)
        {
            switch (buildingGroupType)
            {
                case EnumPageType.JINGJI:
                    view.m_UI_Model_PageButton_Side1.m_img_highLight_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_Side1.m_img_dark_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_Side2.m_img_highLight_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_Side2.m_img_dark_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(181159);
                    view.m_sv_list_view2_ListView.gameObject.SetActive(false);
                    view.m_sv_list_view_ListView.gameObject.SetActive(true);

                    break;
                case EnumPageType.JUNSHI:
                    view.m_UI_Model_PageButton_Side1.m_img_highLight_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_PageButton_Side1.m_img_dark_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_Side2.m_img_highLight_PolygonImage.gameObject.SetActive(true);
                    view.m_UI_Model_PageButton_Side2.m_img_dark_PolygonImage.gameObject.SetActive(false);
                    view.m_UI_Model_Window_Type1.m_lbl_title_LanguageText.text = LanguageUtils.getText(181160);
                    view.m_sv_list_view2_ListView.gameObject.SetActive(true);
                    view.m_sv_list_view_ListView.gameObject.SetActive(false);
                    break;

                default:
                    UnityEngine.Debug.Log("not find type");
                    break;

            }
        }
        public enum EnumPageType
        {
            JINGJI = 1,
            JUNSHI = 2,
        }

        private void AttrDataProcess()
        {
            List<AttrInfoDefine> attrInfoDefineList = CoreUtils.dataService.QueryRecords<AttrInfoDefine>();
            var listAttr = new List<AttrInfoDefine>(attrInfoDefineList.ToArray());
            listAttr.RemoveAll(o => { return o.show == 0; });
            listAttr.Sort((x, y) => { return x.order.CompareTo(y.order); });

            var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
            
            var names = new string[]
            {
                LanguageUtils.getText(100701),
                LanguageUtils.getText(100702),
                LanguageUtils.getText(100703),
                LanguageUtils.getText(100704),
                LanguageUtils.getText(100705),
                LanguageUtils.getText(130700),
                LanguageUtils.getText(730082),
                LanguageUtils.getText(730139),
            };

            int sourceCount = Enum.GetValues(typeof(EnumSourceAttr)).Length;

            for (int i = 0; i < listAttr.Count; i++)
            {
                var config = listAttr[i];
                {
                    var total = playerAttributeProxy.GetCityAttribute(config.ID);
                    GainEffectAttr gainEffectAttr = new GainEffectAttr();
                    gainEffectAttr.Total = true;
                    gainEffectAttr.iconID = config.icon;
                    gainEffectAttr.Name = LanguageUtils.getText(config.nameID);
                    gainEffectAttr.Value = LanguageUtils.getTextFormat(300102, total.GetShowValue());
                    AddattrInfoToList(config.type, gainEffectAttr);
                }

                for(int j = 0; j < sourceCount; j++)
                {
                    var attribute = playerAttributeProxy.GetCityAttributes(config.ID, (EnumSourceAttr)j+1);
                    if (attribute != null && attribute.origvalue > 0)
                    {
                        GainEffectAttr gainEffectAttr = new GainEffectAttr();
                        gainEffectAttr.Total = false;
                        gainEffectAttr.iconID = string.Empty;
                        gainEffectAttr.Name = names[j];
                        gainEffectAttr.Value = LanguageUtils.getTextFormat(300102, attribute.GetShowValue());
                        AddattrInfoToList(config.type, gainEffectAttr);
                    }
                }
            }
        }

        private void AddattrInfoToList(int type, GainEffectAttr gainEffectAttr)
        {
            if (type == (int)EnumPageType.JINGJI)
            {
                m_jingjiGainList.Add(gainEffectAttr);
            }
            else if (type == (int)EnumPageType.JUNSHI)
            {
                m_junshiGainList.Add(gainEffectAttr);
            }
        }

    }
}
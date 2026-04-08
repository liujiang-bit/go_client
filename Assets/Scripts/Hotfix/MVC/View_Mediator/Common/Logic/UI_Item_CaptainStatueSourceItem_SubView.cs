// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月23日
// Update Time         :    2020年4月23日
// Class Description   :    UI_Item_CaptainStatueSourceItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using Data;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_CaptainStatueSourceItem_SubView : UI_SubView
    {
        private int m_nItemId = 0;
        private int m_activityType = 0;//打开的活动id
        protected override void BindEvent()
        {
            m_btn_go.AddClickEvent(OnGotoButtonClicked);
        }

        public void refresh(int itemGetId, int itemId)
        {
            m_nItemId = itemId;
            m_itemGetCfg = CoreUtils.dataService.QueryRecord<Data.ItemGetDefine>(itemGetId);
            if (m_itemGetCfg == null) return;
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, m_itemGetCfg.icon);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(m_itemGetCfg.l_nameID);
            m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_itemGetCfg.l_desID);

            if ((EnumOpenType)m_itemGetCfg.openType == EnumOpenType.Hide)
            {
                m_btn_go.gameObject.SetActive(false);
            }
            else
            {
                m_btn_go.gameObject.SetActive(true);
            }
        }

        private void OnGotoButtonClicked()
        {
            if (m_itemGetCfg == null) return;
            OpenUiDefine define = CoreUtils.dataService.QueryRecord<OpenUiDefine>(m_itemGetCfg.openUi);
            if (define == null)
            {
                return;
            }
            EnumOpenType openType = (EnumOpenType)m_itemGetCfg.openType;
            bool isOpen = false;

            switch (openType)
            {
                case EnumOpenType.Default:
                    {
                        isOpen = true;
                    }
                    break;
                case EnumOpenType.BuildType:
                    {
                        isOpen = SystemOpen.IsBuildType(define.buildType,true);
                    }
                    break;
                case EnumOpenType.SystemOpen:
                    {
                        isOpen = SystemOpen.IsSystemOpen(define.systemOpen,true );
                    }
                    break;
                case EnumOpenType.Activity:
                    {
                        isOpen = SystemOpen.IsActivity(define.activity,out m_activityType, true);
                    }
                    break;
                case EnumOpenType.Package:
                    {
                        isOpen = SystemOpen.IsPackage(define.pakege, true);
                    }
                    break;
                case EnumOpenType.Recharge:
                    {
                        isOpen = SystemOpen.IsRecharge(define.recharge, true);
                    }
                    break;
            }
            if (isOpen)
            {
                ShowGotoUI();
            }
        }

        private void ShowGotoUI()
        {
            if (m_itemGetCfg == null) return;
            EnumOpenType openType = (EnumOpenType)m_itemGetCfg.openType;
            OpenUiDefine openUiDefine = CoreUtils.dataService.QueryRecord<OpenUiDefine>(m_itemGetCfg.openUi);

            switch (openType)
            {
                case EnumOpenType.Default:
                case EnumOpenType.BuildType:
                case EnumOpenType.SystemOpen:
                case EnumOpenType.Package:
                case EnumOpenType.Recharge:
                    {
                        switch (m_itemGetCfg.openUi)
                        {
                            case 3002:
                                {
                                    ItemHeroDefine itemHero = CoreUtils.dataService.QueryRecord<ItemHeroDefine>(m_nItemId);
                                    if (itemHero != null)
                                    {
                                        var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                                        HeroProxy.Hero heroData = heroProxy.GetHeroByID(itemHero.heroID);
                                        if (heroData != null)
                                        {
                                            if (m_itemGetCfg.closeUi == 1)
                                            {
                                                CoreUtils.uiManager.CloseLayerUI(UILayer.WindowLayer);
                                                CoreUtils.uiManager.CloseLayerUI(UILayer.WindowPopLayer);
                                            }
                                            else
                                            {
                                                AppFacade.GetInstance().SendNotification(CmdConstant.OpenUI2, new object[] { m_itemGetCfg.openUi, new ItemExchangeViewData { Hero = heroData } });
                                            }
                                        }
                                    }
                                }
                                break;
                            default:
                                if (m_itemGetCfg.closeUi == 1)
                                {
                                    CoreUtils.uiManager.CloseLayerUI(UILayer.WindowLayer);
                                    CoreUtils.uiManager.CloseLayerUI(UILayer.WindowPopLayer);
                                }
                                AppFacade.GetInstance().SendNotification(CmdConstant.OpenUI, m_itemGetCfg.openUi);
                                break;
                        }
                    }
                    break;
                case EnumOpenType.Activity:
                    {
                        if (m_itemGetCfg.closeUi == 1)
                        {
                            CoreUtils.uiManager.CloseLayerUI(UILayer.WindowLayer);
                            CoreUtils.uiManager.CloseLayerUI(UILayer.WindowPopLayer);
                        }
                        AppFacade.GetInstance().SendNotification(CmdConstant.OpenUI2, new object[] { m_itemGetCfg.openUi, m_activityType  });
                    }
                    break;
            }
        }

        private Data.ItemGetDefine m_itemGetCfg = null;
    }
}
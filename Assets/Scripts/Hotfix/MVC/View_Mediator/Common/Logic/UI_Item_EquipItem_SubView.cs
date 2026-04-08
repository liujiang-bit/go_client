// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Item_EquipItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_EquipItem_SubView : UI_SubView
    {
        private BagProxy m_bagProxy;

        protected override void BindEvent()
        {
            base.BindEvent();
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
        }

        public void SetForgeItemInfo(int index,ForgeEquipItemInfo itemInfo,bool isSelect,UnityAction<ForgeEquipItemInfo,int> callBack)
        {
            m_UI_Model_CaptainHead.gameObject.SetActive(false);
            m_img_talent_PolygonImage.gameObject.SetActive(false);
            m_img_can0_PolygonImage.gameObject.SetActive(itemInfo.IsForgeable);
            m_img_can1_PolygonImage.gameObject.SetActive(!itemInfo.IsForgeable && itemInfo.IsQuickForgeable);

            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemInfo.EquipID);
            bool isShowRedDot = itemInfo.IsForgeable && m_bagProxy.ShowForgeEquipRedDot(itemInfo.EquipID);
            if (isShowRedDot && isSelect)
            {
                isShowRedDot = false;
                m_bagProxy.AddForgeEquipIgnoreRedDot(itemInfo.EquipID);
            }
            m_UI_Model_Item.Refresh(itemCfg,itemInfo.EquipID.ToString(),isSelect,false,isShowRedDot);
            m_UI_Model_Item.RemoveBtnAllListener();
            m_UI_Model_Item.AddBtnListener(() =>
            {
                callBack?.Invoke(itemInfo,index);
                if (isShowRedDot)
                {
                    m_bagProxy.AddForgeEquipIgnoreRedDot(itemInfo.EquipID);
                }
            });
        }

        public void SetEquipItemInfo(int index, EquipItemInfo itemInfo, bool isSelect,
            UnityAction<EquipItemInfo, int> callBack)
        {
            if (itemInfo.Exclusive != 0)
            {
                var talentTypeCfg = CoreUtils.dataService.QueryRecord<HeroTalentTypeDefine>(itemInfo.Exclusive);
                m_img_talent_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(m_img_talent_PolygonImage,talentTypeCfg.equipIcon);
            }
            else
            {
                m_img_talent_PolygonImage.gameObject.SetActive(false);
            }

            if (itemInfo.HeroID != 0)
            {
                m_UI_Model_CaptainHead.gameObject.SetActive(true);
                m_UI_Model_CaptainHead.SetHero(itemInfo.HeroID,0);
            }
            else
            {
                m_UI_Model_CaptainHead.gameObject.SetActive(false);
            }

            m_img_can0_PolygonImage.gameObject.SetActive(false);
            m_img_can1_PolygonImage.gameObject.SetActive(false);

            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemInfo.ItemID);
            m_UI_Model_Item.Refresh(itemCfg,itemInfo.ItemID.ToString(),isSelect,false);
            m_UI_Model_Item.RemoveBtnAllListener();
            m_UI_Model_Item.AddBtnListener(() =>
            {
                callBack?.Invoke(itemInfo,index);
            });
        }
    }
}
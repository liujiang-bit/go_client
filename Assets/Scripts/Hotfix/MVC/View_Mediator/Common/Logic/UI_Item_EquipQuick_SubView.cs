// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月26日
// Update Time         :    2020年5月26日
// Class Description   :    UI_Item_EquipQuick_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_EquipQuick_SubView : UI_SubView
    {
        private List<UI_Model_Item_SubView> m_itemViews = new List<UI_Model_Item_SubView>();
        protected override void BindEvent()
        {
            base.BindEvent();
            m_itemViews.Add(m_UI_Model_Item);
        }

        public void SetMaterialInfo(EquipQuickItemInfo itemInfo)
        {
            DisableMaterialItem();
            int i = 0;
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemInfo.ItemID);
            m_item_target.Refresh(itemCfg,0,false,true);
            m_lbl_Num_LanguageText.text = itemInfo.ItemNum.ToString();
            
            m_UI_Item_ItemEffect.SetQuality(itemCfg.quality);
            
            foreach (var materialInfo in itemInfo.MaterialList)
            {
                if (m_itemViews.Count <= i)
                {
                    var equipMaterialItemObj = GameObject.Instantiate(m_UI_Model_Item.gameObject,m_pl_base_ArabLayoutCompment.transform);
                    var equipMaterialItem = new UI_Model_Item_SubView(equipMaterialItemObj.GetComponent<RectTransform>());
                    equipMaterialItem.m_img_icon_PolygonImage.assetName = String.Empty;
                    m_itemViews.Add(equipMaterialItem);
                }
                m_itemViews[i].gameObject.SetActive(true);
                itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(materialInfo.Key);
                m_itemViews[i].Refresh(itemCfg,(int)materialInfo.Value,false,true);
                i++;
            }
        }

        private void DisableMaterialItem()
        {
            foreach (var view in m_itemViews)
            {
                view.gameObject.SetActive(false);
            }
        }
    }
}
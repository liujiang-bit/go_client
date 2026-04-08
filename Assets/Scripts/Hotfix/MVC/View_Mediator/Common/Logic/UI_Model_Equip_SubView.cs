// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月4日
// Update Time         :    2020年6月4日
// Class Description   :    UI_Model_Equip_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Model_Equip_SubView : UI_SubView
    {

        public void Init(EquipItemInfo equipInfo)
        {
            if (equipInfo == null) return;
            var itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(equipInfo.ItemID);
            ClientUtils.LoadSprite(m_img_equip_PolygonImage,itemDefine.itemIcon);
            Transform animatorObj = gameObject.transform.GetChild(0);
            animatorObj.gameObject.SetActive(true);

            
            if (m_pl_effect.transform.childCount > 0)
            {
                Transform effectObj = m_pl_effect.transform.GetChild(0);
                CoreUtils.assetService.Destroy(effectObj.gameObject);
            }
            
            CoreUtils.assetService.Instantiate($"UI_10039_{itemDefine.quality}", (heroScene) =>
            {
                heroScene.transform.SetParent(m_pl_effect.transform);
                heroScene.transform.localPosition = Vector3.zero;
                heroScene.transform.localScale = new Vector3(2,2,2);
            });
        }
    }
}
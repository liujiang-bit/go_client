// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    UI_Item_EquipMaterialList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using System;

namespace Game {
    public partial class UI_Item_EquipMaterialList_SubView : UI_SubView
    {
        public void SetInfo(int materialID, int num, bool isForge = true, Action callback = null)
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(materialID);
            m_UI_Model_Item.Refresh(itemCfg, false, false, callback);

            if (isForge)
            {
                var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                var itemNum = bagProxy.GetItemNum(materialID);

                if (num > itemNum)
                {
                    m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(182025, itemNum, num);
                }
                else
                {
                    m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(300001, itemNum, num);
                }
            }
            else
            {
                m_lbl_num_LanguageText.text = LanguageUtils.getTextFormat(733378, num);
            }
            m_UI_Item_ItemEffect.SetQuality(itemCfg.quality);
        }
        
        
    }
}
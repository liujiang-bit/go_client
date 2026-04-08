// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月9日
// Update Time         :    2020年5月9日
// Class Description   :    UI_Item_ChargeGemShopList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_ChargeGemShopList_SubView : UI_SubView
    {
        public void Refresh(List<RechargeGemMallDefine> line)
        {
            if (line.Count >= 1)
            {
                m_UI_Item_ChargeTypeDiamondItem0.gameObject.SetActive(true);
                m_UI_Item_ChargeTypeDiamondItem0.Refresh(line[0]);
            }
            else
            {
                m_UI_Item_ChargeTypeDiamondItem0.gameObject.SetActive(false);
            }
            if (line.Count >= 2)
            {
                m_UI_Item_ChargeTypeDiamondItem1.gameObject.SetActive(true);
                m_UI_Item_ChargeTypeDiamondItem1.Refresh(line[1]);
            }
            else
            {
                m_UI_Item_ChargeTypeDiamondItem1.gameObject.SetActive(false);
            }
            if (line.Count >= 3)
            {
                m_UI_Item_ChargeTypeDiamondItem2.gameObject.SetActive(true);
                m_UI_Item_ChargeTypeDiamondItem2.Refresh(line[2]);
            }
            else
            {
                m_UI_Item_ChargeTypeDiamondItem2.gameObject.SetActive(false);
            }
        }
    }
}
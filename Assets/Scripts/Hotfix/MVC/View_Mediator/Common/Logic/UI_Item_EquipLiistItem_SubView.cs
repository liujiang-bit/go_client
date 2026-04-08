// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Item_EquipLiistItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_EquipLiistItem_SubView : UI_SubView
    {
        public void SetForgeItemInfo(int index,List<ForgeEquipItemInfo> itemInfos,int selectItemID,UnityAction<ForgeEquipItemInfo,int> callBack)
        {
            if (itemInfos.Count >= 1)
            {
                m_UI_Item_EquipItem0.gameObject.SetActive(true);
                m_UI_Item_EquipItem0.SetForgeItemInfo(index,itemInfos[0],selectItemID== itemInfos[0].EquipID,callBack);
            }
            else
            {
                m_UI_Item_EquipItem0.gameObject.SetActive(false);
            }

            if (itemInfos.Count >= 2)
            {
                m_UI_Item_EquipItem1.gameObject.SetActive(true);
                m_UI_Item_EquipItem1.SetForgeItemInfo(index, itemInfos[1],selectItemID== itemInfos[1].EquipID,callBack);
            }
            else
            {
                m_UI_Item_EquipItem1.gameObject.SetActive(false);
            }
        }

        public void SetEquipItemInfo(int index, List<EquipItemInfo> itemInfos, long selectItemIndex,
            UnityAction<EquipItemInfo, int> callBack)
        {
            if (itemInfos.Count >= 1)
            {
                m_UI_Item_EquipItem0.gameObject.SetActive(true);
                m_UI_Item_EquipItem0.SetEquipItemInfo(index,itemInfos[0],selectItemIndex== itemInfos[0].ItemIndex,callBack);
            }
            else
            {
                m_UI_Item_EquipItem0.gameObject.SetActive(false);
            }

            if (itemInfos.Count >= 2)
            {
                m_UI_Item_EquipItem1.gameObject.SetActive(true);
                m_UI_Item_EquipItem1.SetEquipItemInfo(index, itemInfos[1],selectItemIndex== itemInfos[1].ItemIndex,callBack);
            }
            else
            {
                m_UI_Item_EquipItem1.gameObject.SetActive(false);
            }
        }
        
    }
}
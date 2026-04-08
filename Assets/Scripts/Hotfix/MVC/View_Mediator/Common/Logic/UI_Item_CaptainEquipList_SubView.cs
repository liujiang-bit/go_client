// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月5日
// Update Time         :    2020年6月5日
// Class Description   :    UI_Item_CaptainEquipList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_CaptainEquipList_SubView : UI_SubView
    {
        public void InitEquipItemInfo(EquipItemInfo info1,EquipItemInfo info2,EquipItemInfo info3,HeroProxy.Hero heroInfo)
        {
            m_UI_Item_CaptainEquipUse1.gameObject.SetActive(false);
            m_UI_Item_CaptainEquipUse2.gameObject.SetActive(false);
            m_UI_Item_CaptainEquipUse3.gameObject.SetActive(false);
            
            if (info1 != null)
            {
                m_UI_Item_CaptainEquipUse1.gameObject.SetActive(true);
                m_UI_Item_CaptainEquipUse1.InitEquipForList(info1,heroInfo);
            }
            
            if (info2 != null)
            {
                m_UI_Item_CaptainEquipUse2.gameObject.SetActive(true);
                m_UI_Item_CaptainEquipUse2.InitEquipForList(info2,heroInfo);
            }
            
            if (info3 != null)
            {
                m_UI_Item_CaptainEquipUse3.gameObject.SetActive(true);
                m_UI_Item_CaptainEquipUse3.InitEquipForList(info3,heroInfo);
            }

        }
    }
}
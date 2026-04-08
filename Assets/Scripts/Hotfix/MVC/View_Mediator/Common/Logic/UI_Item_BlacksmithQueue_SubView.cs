// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月15日
// Update Time         :    2020年5月15日
// Class Description   :    UI_Item_BlacksmithQueue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using Data;
using Skyunion;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_BlacksmithQueue_SubView : UI_SubView
    {
        private int m_index = 0;
        public void AddDelBtnListener(UnityAction<int> callback)
        {
            m_btn_delete_GameButton.onClick.AddListener(()=>
            {
                callback?.Invoke(m_index);
            });
        }
        public void SetInfo(int index,int itemID,int num)
        {
            m_index = index;

            if (itemID == 0)
            {
                m_btn_delete_GameButton.gameObject.SetActive(false);
                m_UI_Model_Item.gameObject.SetActive(false);
                m_img_bg_PolygonImage.gameObject.SetActive(true);
                return;
            }
            var itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
            m_UI_Model_Item.Refresh(itemDefine,num,false,false);
            m_btn_delete_GameButton.gameObject.SetActive(true);
            m_UI_Model_Item.gameObject.SetActive(true);
            m_img_bg_PolygonImage.gameObject.SetActive(false);
        }
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月18日
// Update Time         :    2020年8月18日
// Class Description   :    UI_Item_ItemEffect_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_ItemEffect_SubView : UI_SubView
    {
        private string m_curQualityName;

        public void SetQuality(int quality)
        {
            if (m_curQualityName == $"UI_10039_{quality}")
            {
                return;
            }
            m_curQualityName = $"UI_10039_{quality}";
            CoreUtils.assetService.Instantiate(m_curQualityName, (effectObj) =>
            {
                for (int i = 0; i < m_root_RectTransform.childCount; i++)
                {
                    CoreUtils.assetService.Destroy(m_root_RectTransform.GetChild(i).gameObject);
                }
                effectObj.transform.SetParent(m_root_RectTransform.transform);
                effectObj.transform.localPosition = Vector3.zero;
                effectObj.transform.localScale = new Vector3(2,2,2);
            });
        }
    }
}
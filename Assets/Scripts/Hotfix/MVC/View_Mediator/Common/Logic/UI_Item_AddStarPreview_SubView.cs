// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月26日
// Update Time         :    2020年4月26日
// Class Description   :    UI_Item_AddStarPreview_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;

namespace Game {
    public partial class UI_Item_AddStarPreview_SubView : UI_SubView
    {
        public List<UI_Model_CaptainStar_SubView> CaptainStarList = new List<UI_Model_CaptainStar_SubView>();

        protected override void BindEvent()
        {
            CaptainStarList.Add(m_UI_CaptainStar0);
            CaptainStarList.Add(m_UI_CaptainStar1);
            CaptainStarList.Add(m_UI_CaptainStar2);
            CaptainStarList.Add(m_UI_CaptainStar3);
            CaptainStarList.Add(m_UI_CaptainStar4);
            CaptainStarList.Add(m_UI_CaptainStar5);
        }

        public void Show(int level)
        {
            for(int i = 0; i < level; ++i)
            {
                CaptainStarList[i].m_img_starHighlight_PolygonImage.gameObject.SetActive(true);
            }
        }
    }
}
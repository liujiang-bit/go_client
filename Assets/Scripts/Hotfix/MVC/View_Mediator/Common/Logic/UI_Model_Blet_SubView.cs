// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月26日
// Update Time         :    2020年4月26日
// Class Description   :    UI_Model_Blet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_Blet_SubView : UI_SubView
    {
        public static string[] RankingTopTitle = new string[] { "img_activity_title1", "img_activity_title2", "img_activity_title3", "img_activity_title4"};

        public void RefreshBlet(int rankValue)
        {
            string bletName = RankingTopTitle[3];
            if (rankValue <= 3 && rankValue > 0)
            {
                bletName = RankingTopTitle[rankValue - 1];
            }
            
            ClientUtils.LoadSprite(m_img_Blet0_PolygonImage, bletName);
        }

    }
}
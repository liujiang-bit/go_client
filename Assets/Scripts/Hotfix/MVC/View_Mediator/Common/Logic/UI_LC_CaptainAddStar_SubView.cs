// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月27日
// Update Time         :    2020年4月27日
// Class Description   :    UI_LC_CaptainAddStar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;

namespace Game {
    public partial class UI_LC_CaptainAddStar_SubView : UI_SubView
    {
        public List<UI_Item_CaptainAddStarItem_SubView> CaptainAddStarItemList = new List<UI_Item_CaptainAddStarItem_SubView>();

        protected override void BindEvent()
        {
            CaptainAddStarItemList.Add(m_UI_Item_CaptainAddStarItem_1);
            CaptainAddStarItemList.Add(m_UI_Item_CaptainAddStarItem_2);
            CaptainAddStarItemList.Add(m_UI_Item_CaptainAddStarItem_3);
        }
    }
}
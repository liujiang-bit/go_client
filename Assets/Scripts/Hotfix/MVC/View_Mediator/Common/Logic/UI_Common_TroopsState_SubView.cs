// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月14日
// Update Time         :    2020年8月14日
// Class Description   :    UI_Common_TroopsState_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Hotfix;

namespace Game {
    public partial class UI_Common_TroopsState_SubView : UI_SubView
    {
        public void SetData(long state)
        {
            //this.gameObject.SetActive(false);
            bool isShow = TroopHelp.GetTroopIsFight(state);
            this.gameObject.SetActive(isShow);
            //ClientUtils.LoadSpine(m_UI_Common_TroopsState_SkeletonGraphic,"UE_TroopsState_SD");
        }

    }
}
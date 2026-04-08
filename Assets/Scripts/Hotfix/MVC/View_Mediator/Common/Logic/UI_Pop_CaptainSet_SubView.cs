// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月22日
// Update Time         :    2020年6月22日
// Class Description   :    UI_Pop_CaptainSet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Hotfix;
using System;

namespace Game {
    public partial class UI_Pop_CaptainSet_SubView : UI_SubView
    {

        public ArmyData ArmyData { get; private set; } = null;

        public void Show(ArmyData armyData, Action<ArmyData> clickAction)
        {
            ArmyData = armyData;
            m_UI_Model_CaptainHead.SetHero(armyData.heroId);
            m_btn_ck_GameButton.onClick.AddListener(() =>
            {
                clickAction?.Invoke(ArmyData);
            });
        }
    }
}
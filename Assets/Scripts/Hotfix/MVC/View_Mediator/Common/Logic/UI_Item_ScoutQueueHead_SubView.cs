// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月20日
// Update Time         :    2020年2月20日
// Class Description   :    UI_Item_ScoutQueueHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_ScoutQueueHead_SubView : UI_SubView
    {
        ScoutProxy.ScoutInfo m_scout;
        public void SetScoutInfo(ScoutProxy.ScoutInfo info)
        {
            if(info.state == ScoutProxy.ScoutState.None)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateRest);
            }
            else if (info.state == ScoutProxy.ScoutState.Fog)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateScope);
            }
            else if (info.state == ScoutProxy.ScoutState.Scouting)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateEye);
            }
            else if (info.state == ScoutProxy.ScoutState.Surveing)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateEye);
            }
            else if (info.state == ScoutProxy.ScoutState.Return)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateReturn);
            }
            else if (info.state == ScoutProxy.ScoutState.Back_City)
            {
                ClientUtils.LoadSprite(m_img_state_PolygonImage, RS.ScoutStateReturn);
            }
        }

        public void AddClickEvent(UnityAction action)
        {
            m_btn_noTextButton_GameButton.onClick.AddListener(action);
        }
    }
}
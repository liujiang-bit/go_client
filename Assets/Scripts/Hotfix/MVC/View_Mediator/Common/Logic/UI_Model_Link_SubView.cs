// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月9日
// Update Time         :    2020年1月9日
// Class Description   :    UI_Model_Link_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using System;

namespace Game {
    public partial class UI_Model_Link_SubView : UI_SubView
    {
        private int m_jumpX;
        private int m_jumpY;
        private Action Callback;

        public void RemoveAllClickEvent()
        {
            m_btn_treaty_GameButton.onClick.RemoveAllListeners();
        }

        public void AddClickEvent(UnityAction action)
        {
            m_btn_treaty_GameButton.onClick.AddListener(action);
        }
        public void SetLinkText(string value)
        {
            m_UI_Model_Link_LanguageText.text = value;
        }

        public void SetPos(int x, int y)
        {
            m_jumpX = x;
            m_jumpY = y;
        }

        public void RegisterPosJumpEvent(Action callback = null )
        {
            Callback = callback;
            RemoveAllClickEvent();
            m_btn_treaty_GameButton.onClick.AddListener(OnPosJump);
        }

        public void OnPosJump()
        {
            if (Callback != null)
            {
                Callback();
            }
            CoreUtils.uiManager.CloseUI(UI.s_Email);
            GameHelper.CoordinateJump(m_jumpX, m_jumpY);
            //AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
            //WorldCamera.Instance().SetCameraDxf(WorldCamera.Instance().getCameraDxf("map_tactical"), 1000f, null);
            //WorldCamera.Instance().ViewTerrainPos(m_jumpX * 6 + 3, m_jumpY * 6 + 3, 1000f, null);
        }
    }
}
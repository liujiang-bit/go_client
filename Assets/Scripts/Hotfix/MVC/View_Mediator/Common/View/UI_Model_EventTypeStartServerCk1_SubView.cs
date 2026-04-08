// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_EventTypeStartServerCk1_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_EventTypeStartServerCk1_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_EventTypeStartServerCk1";

        public UI_Model_EventTypeStartServerCk1_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GameToggle m_UI_Model_EventTypeStartServerCk1_GameToggle;



        private void UIFinder()
        {       
			m_UI_Model_EventTypeStartServerCk1_GameToggle = gameObject.GetComponent<GameToggle>();


			BindEvent();
        }

        #endregion
    }
}
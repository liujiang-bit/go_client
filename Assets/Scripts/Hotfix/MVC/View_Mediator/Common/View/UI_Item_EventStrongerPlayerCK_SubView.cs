// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventStrongerPlayerCK_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventStrongerPlayerCK_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventStrongerPlayerCK";

        public UI_Item_EventStrongerPlayerCK_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GameToggle m_UI_Item_EventStrongerPlayerCK_GameToggle;



        private void UIFinder()
        {       
			m_UI_Item_EventStrongerPlayerCK_GameToggle = gameObject.GetComponent<GameToggle>();


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTypeCom1_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTypeCom1_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTypeCom1";

        public UI_Item_EventTypeCom1_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTypeCom1;
		[HideInInspector] public UI_Model_EventType_SubView m_UI_Model_EventType;


        private void UIFinder()
        {       
			m_UI_Item_EventTypeCom1 = gameObject.GetComponent<RectTransform>();
			m_UI_Model_EventType = new UI_Model_EventType_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_EventType"));

			BindEvent();
        }

        #endregion
    }
}
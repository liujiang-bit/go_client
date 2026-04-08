// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Window_Type4_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_Window_Type4_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Window_Type4";

        public UI_Model_Window_Type4_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_Window_Type4;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;



        private void UIFinder()
        {       
			m_UI_Model_Window_Type4 = gameObject.GetComponent<RectTransform>();
			m_btn_close_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_close");


			BindEvent();
        }

        #endregion
    }
}
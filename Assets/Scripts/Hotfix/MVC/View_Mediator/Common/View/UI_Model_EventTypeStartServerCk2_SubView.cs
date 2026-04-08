// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_EventTypeStartServerCk2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_EventTypeStartServerCk2_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_EventTypeStartServerCk2";

        public UI_Model_EventTypeStartServerCk2_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GameToggle m_UI_Model_EventTypeStartServerCk2_GameToggle;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_img_red_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_EventTypeStartServerCk2_GameToggle = gameObject.GetComponent<GameToggle>();

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_img_red_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_red");


			BindEvent();
        }

        #endregion
    }
}
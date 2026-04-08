// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Link_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_Link_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Link";

        public UI_Model_Link_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_UI_Model_Link_LanguageText;
		[HideInInspector] public ContentSizeFitter m_UI_Model_Link_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_treaty_PolygonImage;
		[HideInInspector] public GameButton m_btn_treaty_GameButton;



        private void UIFinder()
        {       
			m_UI_Model_Link_LanguageText = gameObject.GetComponent<LanguageText>();
			m_UI_Model_Link_ContentSizeFitter = gameObject.GetComponent<ContentSizeFitter>();

			m_btn_treaty_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_treaty");
			m_btn_treaty_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_treaty");


			BindEvent();
        }

        #endregion
    }
}
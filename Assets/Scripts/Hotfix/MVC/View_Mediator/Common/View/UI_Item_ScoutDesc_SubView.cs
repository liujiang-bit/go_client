// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ScoutDesc_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ScoutDesc_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ScoutDesc";

        public UI_Item_ScoutDesc_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ScoutDesc;
		[HideInInspector] public LanguageText m_lbl_progress_LanguageText;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;

		[HideInInspector] public LanguageText m_lbl_test_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ScoutDesc = gameObject.GetComponent<RectTransform>();
			m_lbl_progress_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_progress");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_info");

			m_lbl_test_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_test");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainPartline_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_CaptainPartline_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainPartline";

        public UI_Item_CaptainPartline_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptainPartline;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_CaptainPartline = gameObject.GetComponent<RectTransform>();
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/lbl_text");


			BindEvent();
        }

        #endregion
    }
}
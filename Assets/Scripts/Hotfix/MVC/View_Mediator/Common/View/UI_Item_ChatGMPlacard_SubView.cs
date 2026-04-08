// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatGMPlacard_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChatGMPlacard_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatGMPlacard";

        public UI_Item_ChatGMPlacard_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChatGMPlacard;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ContentSizeFitter m_img_bg_ContentSizeFitter;
		[HideInInspector] public VerticalLayoutGroup m_img_bg_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public LanguageText m_lbl_content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_content_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_content_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ChatGMPlacard = gameObject.GetComponent<RectTransform>();
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");
			m_img_bg_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"img_bg");
			m_img_bg_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"img_bg");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/lbl_title");

			m_lbl_content_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/lbl_content");
			m_lbl_content_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"img_bg/lbl_content");
			m_lbl_content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/lbl_content");


			BindEvent();
        }

        #endregion
    }
}
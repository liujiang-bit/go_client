// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_Buildinglevelup_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_Buildinglevelup_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_Buildinglevelup";

        public UI_Item_Buildinglevelup_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_Buildinglevelup_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_before_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_before_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_before_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_after_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_after_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_after_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_Buildinglevelup_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_before_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_before");
			m_lbl_before_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_before");
			m_lbl_before_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_before");

			m_img_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"lbl_before/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_before/img_arrow");

			m_lbl_after_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_before/lbl_after");
			m_lbl_after_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_before/lbl_after");
			m_lbl_after_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_before/lbl_after");


			BindEvent();
        }

        #endregion
    }
}
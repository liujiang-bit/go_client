// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventBtnSource_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventBtnSource_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventBtnSource";

        public UI_Item_EventBtnSource_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Item_EventBtnSource_PolygonImage;
		[HideInInspector] public GameButton m_UI_Item_EventBtnSource_GameButton;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_EventBtnSource_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_source_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_source_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_sourcefra_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_sourcefra_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_source_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_source_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EventBtnSource_PolygonImage = gameObject.GetComponent<PolygonImage>();
			m_UI_Item_EventBtnSource_GameButton = gameObject.GetComponent<GameButton>();
			m_UI_Item_EventBtnSource_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_img_source_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_source");
			m_img_source_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_source");

			m_img_sourcefra_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_sourcefra");
			m_img_sourcefra_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_sourcefra");

			m_lbl_source_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_source");
			m_lbl_source_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_source");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldObjInfoTextLayer_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldObjInfoTextLayer_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjInfoTextLayer";

        public UI_Item_WorldObjInfoTextLayer_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WorldObjInfoTextLayer;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_more_PolygonImage;
		[HideInInspector] public GameButton m_btn_more_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_more_ArabLayoutCompment;
		[HideInInspector] public LayoutElement m_btn_more_LayoutElement;

		[HideInInspector] public LanguageText m_lbl_content_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_content_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_WorldObjInfoTextLayer = gameObject.GetComponent<RectTransform>();
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_title");

			m_btn_more_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"content/btn_more");
			m_btn_more_GameButton = FindUI<GameButton>(gameObject.transform ,"content/btn_more");
			m_btn_more_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"content/btn_more");
			m_btn_more_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"content/btn_more");

			m_lbl_content_LanguageText = FindUI<LanguageText>(gameObject.transform ,"content/lbl_content");
			m_lbl_content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"content/lbl_content");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_Book_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Pop_Book_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_Book";

        public UI_Pop_Book_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Pop_Book_ViewBinder;
		[HideInInspector] public MapElementUI m_UI_Pop_Book_MapElementUI;

		[HideInInspector] public ViewBinder m_UI_Pop_TextTip_ViewBinder;

		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;
		[HideInInspector] public ContentSizeFitter m_pl_pos_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ContentSizeFitter m_img_bg_ContentSizeFitter;
		[HideInInspector] public HorizontalLayoutGroup m_img_bg_HorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_btn_translate_PolygonImage;
		[HideInInspector] public GameButton m_btn_translate_GameButton;
		[HideInInspector] public BtnAnimation m_btn_translate_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_translate_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Pop_Book_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Pop_Book_MapElementUI = gameObject.GetComponent<MapElementUI>();

			m_UI_Pop_TextTip_ViewBinder = FindUI<ViewBinder>(gameObject.transform ,"UI_Pop_TextTip");

			m_pl_pos_Animator = FindUI<Animator>(gameObject.transform ,"UI_Pop_TextTip/pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"UI_Pop_TextTip/pl_pos");
			m_pl_pos_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"UI_Pop_TextTip/pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg");
			m_img_bg_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg");
			m_img_bg_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg");

			m_btn_translate_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg/btn_translate");
			m_btn_translate_GameButton = FindUI<GameButton>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg/btn_translate");
			m_btn_translate_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg/btn_translate");
			m_btn_translate_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg/btn_translate");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg/lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_bg/lbl_text");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"UI_Pop_TextTip/pl_pos/img_arrowSideButtom");


			BindEvent();
        }

        #endregion
    }
}
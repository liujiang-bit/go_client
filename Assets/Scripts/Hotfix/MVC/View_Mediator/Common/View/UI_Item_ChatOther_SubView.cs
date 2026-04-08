// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatOther_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChatOther_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatOther";

        public UI_Item_ChatOther_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ChatOther_ViewBinder;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_head_PolygonImage;
		[HideInInspector] public GameButton m_btn_head_GameButton;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_chatbg_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_tanslate_PolygonImage;
		[HideInInspector] public GameButton m_btn_tanslate_GameButton;

		[HideInInspector] public UI_Common_Spin_SubView m_UI_Common_Spin;
		[HideInInspector] public PolygonImage m_btn_chatFunction_PolygonImage;
		[HideInInspector] public GameButton m_btn_chatFunction_GameButton;
		[HideInInspector] public LongPressBtn m_btn_chatFunction_LongClickButton;

		[HideInInspector] public PolygonImage m_img_chatarrow_PolygonImage;

		[HideInInspector] public VerticalLayoutGroup m_pl_view_VerticalLayoutGroup;

		[HideInInspector] public ArabLayoutCompment m_lbl_content_ArabLayoutCompment;
		[HideInInspector] public LanguageText m_lbl_content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_content_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_lbl_tanslatetext_ArabLayoutCompment;
		[HideInInspector] public LanguageText m_lbl_tanslatetext_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_tanslatetext_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_alreadyTrans_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_alreadyTrans_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ChatOther_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_btn_head_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_head");
			m_btn_head_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_head");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_img_chatbg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_chatbg");

			m_btn_tanslate_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_chatbg/btn_tanslate");
			m_btn_tanslate_GameButton = FindUI<GameButton>(gameObject.transform ,"img_chatbg/btn_tanslate");

			m_UI_Common_Spin = new UI_Common_Spin_SubView(FindUI<RectTransform>(gameObject.transform ,"img_chatbg/UI_Common_Spin"));
			m_btn_chatFunction_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_chatbg/btn_chatFunction");
			m_btn_chatFunction_GameButton = FindUI<GameButton>(gameObject.transform ,"img_chatbg/btn_chatFunction");
			m_btn_chatFunction_LongClickButton = FindUI<LongPressBtn>(gameObject.transform ,"img_chatbg/btn_chatFunction");

			m_img_chatarrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_chatarrow");

			m_pl_view_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_view");

			m_lbl_content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/lbl_content");
			m_lbl_content_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view/lbl_content");
			m_lbl_content_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_view/lbl_content");

			m_img_line_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view/img_line");

			m_lbl_tanslatetext_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/lbl_tanslatetext");
			m_lbl_tanslatetext_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view/lbl_tanslatetext");
			m_lbl_tanslatetext_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_view/lbl_tanslatetext");

			m_img_alreadyTrans_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view/img_alreadyTrans");

			m_lbl_alreadyTrans_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view/img_alreadyTrans/lbl_alreadyTrans");


			BindEvent();
        }

        #endregion
    }
}
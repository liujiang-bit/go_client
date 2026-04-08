// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeCitySupplyPop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeCitySupplyPop_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeCitySupplyPop";

        public UI_Item_ChargeCitySupplyPop_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ChargeCitySupplyPop_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_closeButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_closeButton_GameButton;

		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_curnum_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_curnum_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_cur_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_getdec_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_getdec_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_getdec_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_name_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ChargeCitySupplyPop_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_closeButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_closeButton");
			m_btn_closeButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_closeButton");

			m_pl_pos_Animator = FindUI<Animator>(gameObject.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_lbl_curnum_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_pos/lbl_curnum");
			m_lbl_curnum_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_pos/lbl_curnum");

			m_img_cur_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/lbl_curnum/img_cur");

			m_lbl_getdec_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_pos/lbl_getdec");
			m_lbl_getdec_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_pos/lbl_getdec");
			m_lbl_getdec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos/lbl_getdec");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_pos/UI_Model_Item"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_pos/lbl_name");
			m_lbl_name_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_pos/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos/lbl_name");


			BindEvent();
        }

        #endregion
    }
}
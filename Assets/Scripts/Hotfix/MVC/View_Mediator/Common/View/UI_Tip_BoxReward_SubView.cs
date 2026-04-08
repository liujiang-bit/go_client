// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Tip_BoxReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Tip_BoxReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Tip_BoxReward";

        public UI_Tip_BoxReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Tip_BoxReward_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_closeButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_closeButton_GameButton;

		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public RectMask2D m_img_bg_RectMask2D;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public VerticalLayoutGroup m_pl_info_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_dec_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_dec_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_bgSize;
		[HideInInspector] public GridLayoutGroup m_pl_boxTips_GridLayoutGroup;

		[HideInInspector] public UI_Item_BoxTipsItem_SubView m_UI_Item_BoxTipsItem;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;


        private void UIFinder()
        {       
			m_UI_Tip_BoxReward_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_closeButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_closeButton");
			m_btn_closeButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_closeButton");

			m_pl_pos_Animator = FindUI<Animator>(gameObject.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg");
			m_img_bg_RectMask2D = FindUI<RectMask2D>(gameObject.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_pl_info_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_pos/img_bg/pl_info");

			m_lbl_dec_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_pos/img_bg/pl_info/lbl_dec");
			m_lbl_dec_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_pos/img_bg/pl_info/lbl_dec");
			m_lbl_dec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos/img_bg/pl_info/lbl_dec");

			m_pl_bgSize = FindUI<RectTransform>(gameObject.transform ,"pl_pos/img_bg/pl_info/lbl_dec/pl_bgSize");
			m_pl_boxTips_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_pos/img_bg/pl_info/pl_boxTips");

			m_UI_Item_BoxTipsItem = new UI_Item_BoxTipsItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_pos/img_bg/pl_info/pl_boxTips/UI_Item_BoxTipsItem"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_pos/img_bg/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(gameObject.transform ,"pl_pos/img_bg/sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(gameObject.transform ,"pl_pos/img_bg/sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(gameObject.transform ,"pl_pos/img_bg/sv_list_view/v_list_view/c_list_view");

			BindEvent();
        }

        #endregion
    }
}
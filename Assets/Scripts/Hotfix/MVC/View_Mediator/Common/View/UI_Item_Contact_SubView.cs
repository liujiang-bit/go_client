// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_Contact_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_Contact_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_Contact";

        public UI_Item_Contact_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_Contact_ViewBinder;

		[HideInInspector] public Image m_sv_contact_Image;
		[HideInInspector] public ScrollRect m_sv_contact_ScrollRect;
		[HideInInspector] public ListView m_sv_contact_ListView;

		[HideInInspector] public Image m_v_contact_Image;
		[HideInInspector] public Mask m_v_contact_Mask;

		[HideInInspector] public RectTransform m_c_contact;
		[HideInInspector] public RectTransform m_pl_filter;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_content_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_content_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_img_channel_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_channel_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_shrinkreddot_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_shrinkreddot_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_shrinkreddot_LanguageText;

		[HideInInspector] public Empty4Raycast m_btn_click_Empty4Raycast;
		[HideInInspector] public GameButton m_btn_click_GameButton;

		[HideInInspector] public RectTransform m_pl_left;
		[HideInInspector] public GameButton m_btn_top_GameButton;
		[HideInInspector] public PolygonImage m_btn_top_PolygonImage;

		[HideInInspector] public GameButton m_btn_untop_GameButton;
		[HideInInspector] public PolygonImage m_btn_untop_PolygonImage;

		[HideInInspector] public GameButton m_btn_delete_GameButton;
		[HideInInspector] public PolygonImage m_btn_delete_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_Contact_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_sv_contact_Image = FindUI<Image>(gameObject.transform ,"sv_contact");
			m_sv_contact_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_contact");
			m_sv_contact_ListView = FindUI<ListView>(gameObject.transform ,"sv_contact");

			m_v_contact_Image = FindUI<Image>(gameObject.transform ,"sv_contact/v_contact");
			m_v_contact_Mask = FindUI<Mask>(gameObject.transform ,"sv_contact/v_contact");

			m_c_contact = FindUI<RectTransform>(gameObject.transform ,"sv_contact/v_contact/c_contact");
			m_pl_filter = FindUI<RectTransform>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter");
			m_pl_mes = FindUI<RectTransform>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/img_bg");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/img_select");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/lbl_time");

			m_lbl_content_LanguageText = FindUI<LanguageText>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/lbl_content");
			m_lbl_content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/lbl_content");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/lbl_name");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/UI_Model_PlayerHead"));
			m_img_channel_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/img_channel");
			m_img_channel_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/img_channel");

			m_img_shrinkreddot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/img_shrinkreddot");
			m_img_shrinkreddot_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/img_shrinkreddot");

			m_lbl_shrinkreddot_LanguageText = FindUI<LanguageText>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/img_shrinkreddot/lbl_shrinkreddot");

			m_btn_click_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/btn_click");
			m_btn_click_GameButton = FindUI<GameButton>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_mes/btn_click");

			m_pl_left = FindUI<RectTransform>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/pl_left");
			m_btn_top_GameButton = FindUI<GameButton>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/btn_top");
			m_btn_top_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/btn_top");

			m_btn_untop_GameButton = FindUI<GameButton>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/btn_untop");
			m_btn_untop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/btn_untop");

			m_btn_delete_GameButton = FindUI<GameButton>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/btn_delete");
			m_btn_delete_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_contact/v_contact/c_contact/pl_filter/btn_delete");


			BindEvent();
        }

        #endregion
    }
}
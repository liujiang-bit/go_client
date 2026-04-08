// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Reinforce_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_Reinforce_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Reinforce";

        public UI_Model_Reinforce_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_Reinforce;
		[HideInInspector] public RectTransform m_pl_view1;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_cancel_PolygonImage;
		[HideInInspector] public GameButton m_btn_cancel_GameButton;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_statepro_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_statepro_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_fill_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_pro_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_pro_ArabLayoutCompment;

		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_info_Empty4Raycast;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_view2;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {       
			m_UI_Model_Reinforce = gameObject.GetComponent<RectTransform>();
			m_pl_view1 = FindUI<RectTransform>(gameObject.transform ,"pl_view1");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view1/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view1/img_bg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view1/img_bg/img_icon");

			m_btn_cancel_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view1/img_bg/btn_cancel");
			m_btn_cancel_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_view1/img_bg/btn_cancel");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view1/UI_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view1/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view1/lbl_name");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view1/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view1/lbl_time");

			m_lbl_statepro_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view1/lbl_statepro");
			m_lbl_statepro_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view1/lbl_statepro");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pl_view1/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view1/pb_rogressBar");

			m_img_fill_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view1/pb_rogressBar/Fill Area/img_fill");

			m_lbl_pro_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view1/pb_rogressBar/lbl_pro");
			m_lbl_pro_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view1/pb_rogressBar/lbl_pro");

			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_view1/pb_rogressBar/btn_info");
			m_btn_info_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_view1/pb_rogressBar/btn_info");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view1/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view1/lbl_desc");

			m_pl_view2 = FindUI<RectTransform>(gameObject.transform ,"pl_view2");
			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_view2/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_view2/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"pl_view2/sv_list");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WarnInfo_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WarnInfo_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WarnInfo";

        public UI_Item_WarnInfo_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WarnInfo;
		[HideInInspector] public GameButton m_pl_rect_GameButton;
		[HideInInspector] public Empty4Raycast m_pl_rect_Empty4Raycast;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_ignore_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_ignore_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Warning_SubView m_UI_Model_Warning;
		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_ignore_PolygonImage;
		[HideInInspector] public GameButton m_btn_ignore_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_ignore_ArabLayoutCompment;

		[HideInInspector] public VerticalLayoutGroup m_pl_mes_VerticalLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_mes_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CapHeadSub;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CapHeadMain;
		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count_ArabLayoutCompment;

		[HideInInspector] public VerticalLayoutGroup m_pl_mes2_VerticalLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_mes2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name2_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_view2;
		[HideInInspector] public PolygonImage m_img_woker_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_woker_MakeChildrenGray;
		[HideInInspector] public ArabLayoutCompment m_img_woker_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count2_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_WarnInfo = gameObject.GetComponent<RectTransform>();
			m_pl_rect_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_rect");
			m_pl_rect_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_rect");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/img_icon");

			m_img_ignore_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/img_icon/img_ignore");
			m_img_ignore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/img_icon/img_ignore");

			m_UI_Model_Warning = new UI_Model_Warning_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rect/img_icon/UI_Model_Warning"));
			m_UI_Model_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rect/posAndTime/UI_Model_Link"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/posAndTime/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/posAndTime/lbl_time");

			m_img_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/posAndTime/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/posAndTime/img_arrow");

			m_btn_ignore_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/posAndTime/btn_ignore");
			m_btn_ignore_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_rect/posAndTime/btn_ignore");
			m_btn_ignore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/posAndTime/btn_ignore");

			m_pl_mes_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_rect/pl_mes");
			m_pl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/pl_mes");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/pl_mes/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/pl_mes/lbl_name");

			m_pl_view = FindUI<RectTransform>(gameObject.transform ,"pl_rect/pl_mes/pl_view");
			m_UI_Model_CapHeadSub = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rect/pl_mes/pl_view/UI_Model_CapHeadSub"));
			m_UI_Model_CapHeadMain = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rect/pl_mes/pl_view/UI_Model_CapHeadMain"));
			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/pl_mes/pl_view/lbl_count");
			m_lbl_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/pl_mes/pl_view/lbl_count");

			m_pl_mes2_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_rect/pl_mes2");
			m_pl_mes2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/pl_mes2");

			m_lbl_name2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/pl_mes2/lbl_name2");
			m_lbl_name2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/pl_mes2/lbl_name2");

			m_pl_view2 = FindUI<RectTransform>(gameObject.transform ,"pl_rect/pl_mes2/pl_view2");
			m_img_woker_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/pl_mes2/pl_view2/img_woker");
			m_img_woker_MakeChildrenGray = FindUI<GrayChildrens>(gameObject.transform ,"pl_rect/pl_mes2/pl_view2/img_woker");
			m_img_woker_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/pl_mes2/pl_view2/img_woker");

			m_img_char_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/pl_mes2/pl_view2/img_woker/img_char");

			m_lbl_count2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/pl_mes2/pl_view2/lbl_count2");
			m_lbl_count2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_rect/pl_mes2/pl_view2/lbl_count2");


			BindEvent();
        }

        #endregion
    }
}
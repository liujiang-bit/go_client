// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailWarCoordinate_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailWarCoordinate_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailWarCoordinate";

        public UI_Item_MailWarCoordinate_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailWarCoordinate;
		[HideInInspector] public LanguageText m_lbl_coordinate_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_coordinate_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_coordinatebg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;

		[HideInInspector] public PolyLineChart m_img_polylinechart_PolyLineChart;

		[HideInInspector] public LanguageText m_lbl_chartnum3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_chartnum2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_chartnum1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_chartnum0_LanguageText;

		[HideInInspector] public LanguageText m_lbl_starttime_LanguageText;

		[HideInInspector] public LanguageText m_lbl_endtime_LanguageText;

		[HideInInspector] public PolygonImage m_img_fail_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point_start_PolygonImage;

		[HideInInspector] public PolygonImage m_img_point_end_PolygonImage;

		[HideInInspector] public RectTransform m_pl_aid;
		[HideInInspector] public UI_Item_MailWarReinforce_SubView m_UI_Item_MailWarReinforce;


        private void UIFinder()
        {       
			m_UI_Item_MailWarCoordinate = gameObject.GetComponent<RectTransform>();
			m_lbl_coordinate_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_coordinate");
			m_lbl_coordinate_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_coordinate");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));
			m_img_coordinatebg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_coordinatebg");

			m_img_line_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_coordinatebg/img_line");

			m_img_polylinechart_PolyLineChart = FindUI<PolyLineChart>(gameObject.transform ,"img_polylinechart");

			m_lbl_chartnum3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_polylinechart/lbl_chartnum3");

			m_lbl_chartnum2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_polylinechart/lbl_chartnum2");

			m_lbl_chartnum1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_polylinechart/lbl_chartnum1");

			m_lbl_chartnum0_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_polylinechart/lbl_chartnum0");

			m_lbl_starttime_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_polylinechart/lbl_starttime");

			m_lbl_endtime_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_polylinechart/lbl_endtime");

			m_img_fail_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_polylinechart/img_fail");

			m_img_point_start_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_polylinechart/img_point_start");

			m_img_point_end_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_polylinechart/img_point_end");

			m_pl_aid = FindUI<RectTransform>(gameObject.transform ,"pl_aid");
			m_UI_Item_MailWarReinforce = new UI_Item_MailWarReinforce_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_aid/UI_Item_MailWarReinforce"));

			BindEvent();
        }

        #endregion
    }
}
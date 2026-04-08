// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月17日
// Update Time         :    2020年3月17日
// Class Description   :    UI_Item_MailTypeWarView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_MailTypeWarView : GameView
    {
		public const string VIEW_NAME = "UI_Item_MailTypeWar";

        public UI_Item_MailTypeWarView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public RectTransform m_pl_res;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesFood;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesWood;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesStone;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesGold;
		[HideInInspector] public RectTransform m_pl_kill;
		[HideInInspector] public GridLayoutGroup m_pl_getreward_GridLayoutGroup;

		[HideInInspector] public RectTransform m_pl_self;
		[HideInInspector] public LanguageText m_lbl_armyTotal_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyTotal_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyLast_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyLast_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_selfname_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_selfname_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public RectTransform m_pl_coordinate;
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

		[HideInInspector] public ScrollRect m_sv_enemyBar_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_enemyBar_PolygonImage;
		[HideInInspector] public ListView m_sv_enemyBar_ListView;

		[HideInInspector] public ScrollRect m_sv_war_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_war_PolygonImage;
		[HideInInspector] public ListView m_sv_war_ListView;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_MailTitle"));
			m_pl_res = FindUI<RectTransform>(vb.transform ,"pl_res");
			m_UI_Model_ResourcesFood = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/rect/UI_Model_ResourcesFood"));
			m_UI_Model_ResourcesWood = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/rect/UI_Model_ResourcesWood"));
			m_UI_Model_ResourcesStone = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/rect/UI_Model_ResourcesStone"));
			m_UI_Model_ResourcesGold = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"pl_res/rect/UI_Model_ResourcesGold"));
			m_pl_kill = FindUI<RectTransform>(vb.transform ,"pl_kill");
			m_pl_getreward_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_kill/pl_getreward");

			m_pl_self = FindUI<RectTransform>(vb.transform ,"pl_self");
			m_lbl_armyTotal_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_self/lbl_armyTotal");
			m_lbl_armyTotal_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_self/lbl_armyTotal");

			m_lbl_armyLast_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_self/lbl_armyLast");
			m_lbl_armyLast_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_self/lbl_armyLast");

			m_lbl_selfname_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_self/lbl_selfname");
			m_lbl_selfname_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_self/lbl_selfname");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_self/UI_Model_PlayerHead"));
			m_pl_coordinate = FindUI<RectTransform>(vb.transform ,"pl_coordinate");
			m_lbl_coordinate_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_coordinate/lbl_coordinate");
			m_lbl_coordinate_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_coordinate/lbl_coordinate");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_coordinate/UI_Model_CaptainHead"));
			m_img_coordinatebg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_coordinate/img_coordinatebg");

			m_img_line_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_coordinate/img_coordinatebg/img_line");

			m_img_polylinechart_PolyLineChart = FindUI<PolyLineChart>(vb.transform ,"pl_coordinate/img_polylinechart");

			m_lbl_chartnum3_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_coordinate/img_polylinechart/lbl_chartnum3");

			m_lbl_chartnum2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_coordinate/img_polylinechart/lbl_chartnum2");

			m_lbl_chartnum1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_coordinate/img_polylinechart/lbl_chartnum1");

			m_lbl_chartnum0_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_coordinate/img_polylinechart/lbl_chartnum0");

			m_lbl_starttime_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_coordinate/img_polylinechart/lbl_starttime");

			m_lbl_endtime_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_coordinate/img_polylinechart/lbl_endtime");

			m_img_fail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_coordinate/img_polylinechart/img_fail");

			m_img_point_start_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_coordinate/img_polylinechart/img_point_start");

			m_img_point_end_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_coordinate/img_polylinechart/img_point_end");

			m_sv_enemyBar_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_enemyBar");
			m_sv_enemyBar_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_enemyBar");
			m_sv_enemyBar_ListView = FindUI<ListView>(vb.transform ,"sv_enemyBar");

			m_sv_war_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_war");
			m_sv_war_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_war");
			m_sv_war_ListView = FindUI<ListView>(vb.transform ,"sv_war");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
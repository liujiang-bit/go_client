// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月8日
// Update Time         :    2020年4月8日
// Class Description   :    UIPopTreatmentOnBuildingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UIPopTreatmentOnBuildingView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_TreatmentOnBuilding";

        public UIPopTreatmentOnBuildingView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_wound;
		[HideInInspector] public PolygonImage m_img_civilization_PolygonImage;

		[HideInInspector] public PolygonImage m_img_shizi_PolygonImage;

		[HideInInspector] public GameButton m_btn_wound_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_wound_Empty4Raycast;

		[HideInInspector] public RectTransform m_pl_get;
		[HideInInspector] public UI_Model_TreatmentHeadAlign_SubView m_UI_Model_TreatmentHeadAlign;
		[HideInInspector] public GameButton m_btn_get_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_get_Empty4Raycast;

		[HideInInspector] public RectTransform m_pl_treatment;
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public Animator m_lbl_time_Animator;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public Animator m_lbl_desc_Animator;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_wound = FindUI<RectTransform>(vb.transform ,"pl_wound");
			m_img_civilization_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_wound/plArrow/plSize/img_civilization");

			m_img_shizi_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_wound/plArrow/plSize/img_shizi");

			m_btn_wound_GameButton = FindUI<GameButton>(vb.transform ,"pl_wound/btn_wound");
			m_btn_wound_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"pl_wound/btn_wound");

			m_pl_get = FindUI<RectTransform>(vb.transform ,"pl_get");
			m_UI_Model_TreatmentHeadAlign = new UI_Model_TreatmentHeadAlign_SubView(FindUI<RectTransform>(vb.transform ,"pl_get/plArrow/plSize/UI_Model_TreatmentHeadAlign"));
			m_btn_get_GameButton = FindUI<GameButton>(vb.transform ,"pl_get/btn_get");
			m_btn_get_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"pl_get/btn_get");

			m_pl_treatment = FindUI<RectTransform>(vb.transform ,"pl_treatment");
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_treatment/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_treatment/pb_rogressBar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_treatment/pb_rogressBar/lbl_time");
			m_lbl_time_Animator = FindUI<Animator>(vb.transform ,"pl_treatment/pb_rogressBar/lbl_time");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_treatment/pb_rogressBar/lbl_desc");
			m_lbl_desc_Animator = FindUI<Animator>(vb.transform ,"pl_treatment/pb_rogressBar/lbl_desc");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
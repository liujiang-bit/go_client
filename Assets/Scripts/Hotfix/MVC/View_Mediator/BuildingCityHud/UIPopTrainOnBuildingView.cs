// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月15日
// Update Time         :    2020年1月15日
// Class Description   :    UIPopTrainOnBuildingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UIPopTrainOnBuildingView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_TrainOnBuilding";

        public UIPopTrainOnBuildingView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_none;
		[HideInInspector] public RectTransform m_pl_get;
		[HideInInspector] public PolygonImage m_img_soldier_icon_PolygonImage;

		[HideInInspector] public GameButton m_btn_get_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_get_Empty4Raycast;

		[HideInInspector] public RectTransform m_pl_train;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_none = FindUI<RectTransform>(vb.transform ,"pl_none");
			m_pl_get = FindUI<RectTransform>(vb.transform ,"pl_get");
			m_img_soldier_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_get/plArrow/plSize/img_soldier_icon");

			m_btn_get_GameButton = FindUI<GameButton>(vb.transform ,"pl_get/btn_get");
			m_btn_get_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"pl_get/btn_get");

			m_pl_train = FindUI<RectTransform>(vb.transform ,"pl_train");
			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_train/img_icon");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_train/pb_rogressBar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_train/pb_rogressBar/lbl_time");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_train/pb_rogressBar/lbl_desc");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
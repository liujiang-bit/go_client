// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月14日
// Update Time         :    2020年1月14日
// Class Description   :    UI_Tip_BuildingBarView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_BuildingBarView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_BuildingBar";

        public UI_Tip_BuildingBarView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pb_rogressBar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pb_rogressBar/lbl_time");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pb_rogressBar/lbl_name");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
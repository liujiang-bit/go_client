// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, March 24, 2020
// Update Time         :    Tuesday, March 24, 2020
// Class Description   :    ResearchPreItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ResearchPreItemView : GameView
    {
		public const string VIEW_NAME = "UI_ResearchPreItem";

        public ResearchPreItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_jump;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_lv_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");

			m_btn_jump = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"btn_jump"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");

			m_lbl_lv_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_lv");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
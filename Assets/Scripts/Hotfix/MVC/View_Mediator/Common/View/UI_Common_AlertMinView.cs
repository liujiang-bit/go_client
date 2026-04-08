// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月13日
// Update Time         :    2020年2月13日
// Class Description   :    UI_Common_AlertMinView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Common_AlertMinView : GameView
    {
		public const string VIEW_NAME = "UI_Common_AlertMin";

        public UI_Common_AlertMinView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_UI_Model_StandardButton_Blue_sure;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_text");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"img_bg/lbl_title");

			m_UI_Model_StandardButton_Blue_sure = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/UI_Model_StandardButton_Blue_sure"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
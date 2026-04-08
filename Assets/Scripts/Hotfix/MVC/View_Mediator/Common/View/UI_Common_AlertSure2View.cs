// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Common_AlertSure2View
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Common_AlertSure2View : GameView
    {
		public const string VIEW_NAME = "UI_Common_AlertSure2";

        public UI_Common_AlertSure2View () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public GameToggle m_ck_tips_GameToggle;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_UI_Model_StandardButton_Blue;
		[HideInInspector] public UI_Model_DoubleLineButton_Red2_SubView m_UI_Model_StandardButton_Red;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_text");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"img_bg/lbl_title");

			m_ck_tips_GameToggle = FindUI<GameToggle>(vb.transform ,"img_bg/ck_tips");

			m_UI_Model_StandardButton_Blue = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_StandardButton_Blue"));
			m_UI_Model_StandardButton_Red = new UI_Model_DoubleLineButton_Red2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_StandardButton_Red"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
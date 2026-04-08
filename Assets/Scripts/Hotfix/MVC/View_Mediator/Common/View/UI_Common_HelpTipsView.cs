// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月6日
// Update Time         :    2020年5月6日
// Class Description   :    UI_Common_HelpTipsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Common_HelpTipsView : GameView
    {
		public const string VIEW_NAME = "UI_Common_HelpTips";

        public UI_Common_HelpTipsView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public CanvasGroup m_img_bg_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_message_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"img_bg");

			m_lbl_message_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_message");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
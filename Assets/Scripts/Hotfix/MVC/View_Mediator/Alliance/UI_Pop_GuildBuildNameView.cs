// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 22, 2020
// Update Time         :    Wednesday, April 22, 2020
// Class Description   :    UI_Pop_GuildBuildNameView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_GuildBuildNameView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_GuildBuildName";

        public UI_Pop_GuildBuildNameView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_Model_GuildFlag;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bg");

			m_UI_Model_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_GuildFlag"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_name");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
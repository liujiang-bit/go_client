// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月2日
// Update Time         :    2020年1月2日
// Class Description   :    ResourcesConsumeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ResourcesConsumeView : GameView
    {
		public const string VIEW_NAME = "UI_Model_ResourcesConsume";

        public ResourcesConsumeView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_languageText");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
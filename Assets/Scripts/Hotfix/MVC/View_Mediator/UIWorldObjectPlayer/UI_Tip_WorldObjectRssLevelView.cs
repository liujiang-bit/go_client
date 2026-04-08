// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月8日
// Update Time         :    2020年7月8日
// Class Description   :    UI_Tip_WorldObjectRssLevelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_WorldObjectRssLevelView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_WorldObjectRssLevel";

        public UI_Tip_WorldObjectRssLevelView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/lbl_level");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
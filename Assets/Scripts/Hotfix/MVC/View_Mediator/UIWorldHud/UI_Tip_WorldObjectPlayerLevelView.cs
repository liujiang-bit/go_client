// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月6日
// Update Time         :    2020年3月6日
// Class Description   :    UI_Tip_WorldObjectPlayerLevelView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_WorldObjectPlayerLevelView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_WorldObjectPlayerLevel";

        public UI_Tip_WorldObjectPlayerLevelView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;

		[HideInInspector] public PolygonImage m_img_cion_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"bg0/lbl_playerName");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_level");

			m_img_cion_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_cion");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
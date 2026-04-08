// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月15日
// Update Time         :    2020年6月15日
// Class Description   :    UI_Item_ChatTimeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ChatTimeView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ChatTime";

        public UI_Item_ChatTimeView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_time;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_time = FindUI<RectTransform>(vb.transform ,"pl_time");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_time/img_bg");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_time/img_bg/lbl_time");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    TroopMainItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class TroopMainItemView : GameView
    {
		public const string VIEW_NAME = "TroopMainItem";

        public TroopMainItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon1_PolygonImage;

		[HideInInspector] public LanguageText m_txt_times_LanguageText;

		[HideInInspector] public PolygonImage m_btn_click_PolygonImage;
		[HideInInspector] public GameButton m_btn_click_GameButton;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_select_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_select");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");

			m_img_icon1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon1");

			m_txt_times_LanguageText = FindUI<LanguageText>(vb.transform ,"txt_times");

			m_btn_click_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_click");
			m_btn_click_GameButton = FindUI<GameButton>(vb.transform ,"btn_click");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
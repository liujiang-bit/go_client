// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 14, 2020
// Update Time         :    Tuesday, April 14, 2020
// Class Description   :    UI_Item_GuildMemRankView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildMemRankView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildMemRank";

        public UI_Item_GuildMemRankView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_bg_PolygonImage;
		[HideInInspector] public GameButton m_btn_bg_GameButton;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_img_mem_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public PolygonImage m_img_arrow_down_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrow_up_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_bg");
			m_btn_bg_GameButton = FindUI<GameButton>(vb.transform ,"btn_bg");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_bg/lbl_name");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_bg/img_icon");

			m_img_mem_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_bg/img_mem");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_bg/lbl_count");

			m_img_arrow_down_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_bg/img_arrow_down");

			m_img_arrow_up_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_bg/img_arrow_up");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
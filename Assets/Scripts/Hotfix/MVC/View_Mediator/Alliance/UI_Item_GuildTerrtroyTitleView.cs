// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Sunday, June 28, 2020
// Update Time         :    Sunday, June 28, 2020
// Class Description   :    UI_Item_GuildTerrtroyTitleView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildTerrtroyTitleView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildTerrtroyTitle";

        public UI_Item_GuildTerrtroyTitleView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_down_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_down_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_up_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_up_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_build_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_build_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"btn_btn");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/lbl_name");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_count");
			m_lbl_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/lbl_count");

			m_img_arrow_down_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_arrow_down");
			m_img_arrow_down_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_arrow_down");

			m_img_arrow_up_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_arrow_up");
			m_img_arrow_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_arrow_up");

			m_img_build_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_build");
			m_img_build_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_build");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
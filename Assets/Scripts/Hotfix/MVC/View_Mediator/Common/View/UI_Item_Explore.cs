// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月19日
// Update Time         :    2020年5月19日
// Class Description   :    UI_Item_ExploreView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ExploreView : GameView
    {
		public const string VIEW_NAME = "UI_Item_Explore";

        public UI_Item_ExploreView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_building_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_building_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_explore_PolygonImage;
		[HideInInspector] public GameButton m_btn_explore_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_explore_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_explore_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_explore_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_lbl_linkImageText;
		[HideInInspector] public PolygonImage m_img_reddot_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_reddot_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_building_PolygonImage = FindUI<PolygonImage>(vb.transform ,"count/img_building");
			m_img_building_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"count/img_building");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"count/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"count/lbl_desc");

			m_btn_explore_PolygonImage = FindUI<PolygonImage>(vb.transform ,"count/btn_explore");
			m_btn_explore_GameButton = FindUI<GameButton>(vb.transform ,"count/btn_explore");
			m_btn_explore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"count/btn_explore");

			m_lbl_explore_LanguageText = FindUI<LanguageText>(vb.transform ,"count/lbl_explore");
			m_lbl_explore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"count/lbl_explore");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_time");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_name");

			m_lbl_linkImageText = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"lbl_linkImageText"));
			m_img_reddot_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_reddot");
			m_img_reddot_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_reddot");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
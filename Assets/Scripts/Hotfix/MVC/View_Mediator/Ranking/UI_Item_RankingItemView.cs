// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月26日
// Update Time         :    2020年4月26日
// Class Description   :    UI_Item_RankingItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_RankingItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_RankingItem";

        public UI_Item_RankingItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_rank_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_rank_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_rank_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_rank_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_guildName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_guildName_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowUp_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowUp_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowDown_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowDown_ArabLayoutCompment;

		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"btn_btn");

			m_img_rank_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_rank");
			m_img_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_rank");

			m_lbl_rank_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_rank");
			m_lbl_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/lbl_rank");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/lbl_name");

			m_lbl_guildName_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_guildName");
			m_lbl_guildName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/lbl_guildName");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_btn/UI_PlayerHead"));
			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_btn/lbl_power");
			m_lbl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/lbl_power");

			m_img_arrowUp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_arrowUp");
			m_img_arrowUp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_arrowUp");

			m_img_arrowDown_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_btn/img_arrowDown");
			m_img_arrowDown_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_btn/img_arrowDown");

			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"btn_btn/UI_GuildFlag"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
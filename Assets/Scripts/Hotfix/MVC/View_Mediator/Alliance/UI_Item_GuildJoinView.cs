// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, July 13, 2020
// Update Time         :    Monday, July 13, 2020
// Class Description   :    UI_Item_GuildJoinView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildJoinView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildJoin";

        public UI_Item_GuildJoinView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bgNoSelect_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bgSelect_PolygonImage;

		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_pow_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_pow_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_mem_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mem_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_lang_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lang_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_select_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_enter_PolygonImage;
		[HideInInspector] public GameButton m_btn_enter_GameButton;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bgNoSelect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bgNoSelect");

			m_img_bgSelect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bgSelect");

			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlag"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_name");

			m_lbl_pow_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_pow");
			m_lbl_pow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_pow");

			m_lbl_mem_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_mem");
			m_lbl_mem_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_mem");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_desc");

			m_lbl_lang_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_lang");
			m_lbl_lang_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_lang");

			m_img_select_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_select");
			m_img_select_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_select");

			m_btn_enter_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_enter");
			m_btn_enter_GameButton = FindUI<GameButton>(vb.transform ,"btn_enter");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
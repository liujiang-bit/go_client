// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月10日
// Update Time         :    2020年6月10日
// Class Description   :    UITroopsSaveItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UITroopsSaveItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_TroopsSaveItem";

        public UITroopsSaveItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_save_PolygonImage;
		[HideInInspector] public GameButton m_btn_save_GameButton;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_light_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHeadMain;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHeadSub;
		[HideInInspector] public LanguageText m_lbl_empty_LanguageText;

		[HideInInspector] public PolygonImage m_btn_delete_PolygonImage;
		[HideInInspector] public GameButton m_btn_delete_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_delete_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_def;
		[HideInInspector] public PolygonImage m_pl_def_img_bg_PolygonImage;
		[HideInInspector] public GameButton m_pl_def_img_bg_GameButton;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public LanguageText m_lbl_id_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_id_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_save_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_save");
			m_btn_save_GameButton = FindUI<GameButton>(vb.transform ,"btn_save");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_save/img_bg");

			m_img_light_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_save/img_light");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_save/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_save/lbl_name");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_save/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_save/lbl_num");

			m_UI_Model_CaptainHeadMain = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_save/UI_Model_CaptainHeadMain"));
			m_UI_Model_CaptainHeadSub = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_save/UI_Model_CaptainHeadSub"));
			m_lbl_empty_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_save/lbl_empty");

			m_btn_delete_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_delete");
			m_btn_delete_GameButton = FindUI<GameButton>(vb.transform ,"btn_delete");
			m_btn_delete_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_delete");

			m_pl_def = FindUI<RectTransform>(vb.transform ,"pl_def");
			m_pl_def_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_def/pl_def_img_bg");
			m_pl_def_img_bg_GameButton = FindUI<GameButton>(vb.transform ,"pl_def/pl_def_img_bg");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_def/lbl_languageText");

			m_lbl_id_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_id");
			m_lbl_id_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_id");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
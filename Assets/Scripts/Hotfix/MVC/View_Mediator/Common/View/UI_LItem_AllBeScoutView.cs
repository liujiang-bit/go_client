// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月8日
// Update Time         :    2020年5月8日
// Class Description   :    UI_LItem_AllBeScoutView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_LItem_AllBeScoutView : GameView
    {
		public const string VIEW_NAME = "UI_LItem_AllBeScout";

        public UI_LItem_AllBeScoutView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_languageText_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_Link_SubView m_UI_Link;
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
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"count/res/lbl_languageText");
			m_lbl_languageText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"count/res/lbl_languageText");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"count/res/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"count/res/lbl_desc");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"count/res/UI_PlayerHead"));
			m_UI_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"count/res/UI_Link"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"title/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"title/lbl_time");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"title/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"title/lbl_name");

			m_lbl_linkImageText = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"title/lbl_linkImageText"));
			m_img_reddot_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_reddot");
			m_img_reddot_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_reddot");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
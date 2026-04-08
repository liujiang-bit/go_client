// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月16日
// Update Time         :    2020年3月16日
// Class Description   :    UI_Item_CollectReportView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_CollectReportView : GameView
    {
		public const string VIEW_NAME = "UI_Item_CollectReport";

        public UI_Item_CollectReportView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_languageText_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_add_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_add_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

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

			m_lbl_add_LanguageText = FindUI<LanguageText>(vb.transform ,"count/res/lbl_add");
			m_lbl_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"count/res/lbl_add");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"count/res/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"count/res/img_icon");

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
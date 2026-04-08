// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月11日
// Update Time         :    2020年6月11日
// Class Description   :    UI_Item_MailType13View
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_MailType13View : GameView
    {
		public const string VIEW_NAME = "UI_Item_MailType13";

        public UI_Item_MailType13View () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public PolygonImage m_btn_careful_PolygonImage;
		[HideInInspector] public GameButton m_btn_careful_GameButton;
		[HideInInspector] public LayoutElement m_btn_careful_LayoutElement;

		[HideInInspector] public ArabLayoutCompment m_pl_pop_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideL_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_popmes;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_report;
		[HideInInspector] public GameSlider m_sd_shield_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_shield_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_content_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_content_PolygonImage;
		[HideInInspector] public ListView m_sv_content_ListView;

		[HideInInspector] public PolygonImage m_v_content_PolygonImage;
		[HideInInspector] public Mask m_v_content_Mask;

		[HideInInspector] public RectTransform m_c_content;
		[HideInInspector] public LanguageText m_lbl_Content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_Content_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_Content_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_MailTitle"));
			m_btn_careful_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_careful");
			m_btn_careful_GameButton = FindUI<GameButton>(vb.transform ,"btn_careful");
			m_btn_careful_LayoutElement = FindUI<LayoutElement>(vb.transform ,"btn_careful");

			m_pl_pop_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_careful/pl_pop");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_careful/pl_pop/img_bg");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_careful/pl_pop/img_bg/img_arrowSideL");
			m_img_arrowSideL_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_careful/pl_pop/img_bg/img_arrowSideL");

			m_pl_popmes = FindUI<RectTransform>(vb.transform ,"btn_careful/pl_pop/pl_popmes");
			m_btn_report = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"btn_careful/pl_pop/pl_popmes/btn_report"));
			m_sd_shield_GameSlider = FindUI<GameSlider>(vb.transform ,"btn_careful/pl_pop/pl_popmes/sd_shield");
			m_sd_shield_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_careful/pl_pop/pl_popmes/sd_shield");

			m_sv_content_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_content");
			m_sv_content_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_content");
			m_sv_content_ListView = FindUI<ListView>(vb.transform ,"sv_content");

			m_v_content_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_content/v_content");
			m_v_content_Mask = FindUI<Mask>(vb.transform ,"sv_content/v_content");

			m_c_content = FindUI<RectTransform>(vb.transform ,"sv_content/v_content/c_content");
			m_lbl_Content_LanguageText = FindUI<LanguageText>(vb.transform ,"sv_content/v_content/c_content/lbl_Content");
			m_lbl_Content_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"sv_content/v_content/c_content/lbl_Content");
			m_lbl_Content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"sv_content/v_content/c_content/lbl_Content");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
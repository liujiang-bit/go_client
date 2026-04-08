// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月14日
// Update Time         :    2020年3月14日
// Class Description   :    UI_Item_MailBtnView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_MailBtnView : GameView
    {
		public const string VIEW_NAME = "UI_Item_MailBtn";

        public UI_Item_MailBtnView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bgActive_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bgActive_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bag_PolygonImage;
		[HideInInspector] public Animation m_img_bag_Animation;
		[HideInInspector] public ArabLayoutCompment m_img_bag_ArabLayoutCompment;

		[HideInInspector] public GameButton m_btn_click_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_click_Empty4Raycast;

		[HideInInspector] public PolygonImage m_img_redpot_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bg");

			m_img_bgActive_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bgActive");
			m_img_bgActive_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bgActive");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_icon");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"texts/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"texts/lbl_title");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"texts/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"texts/lbl_desc");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"texts/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"texts/lbl_time");

			m_img_bag_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bag");
			m_img_bag_Animation = FindUI<Animation>(vb.transform ,"img_bag");
			m_img_bag_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_bag");

			m_btn_click_GameButton = FindUI<GameButton>(vb.transform ,"btn_click");
			m_btn_click_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"btn_click");

			m_img_redpot_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_redpot");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
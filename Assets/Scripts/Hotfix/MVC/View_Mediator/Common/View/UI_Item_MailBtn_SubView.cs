// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailBtn_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailBtn_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailBtn";

        public UI_Item_MailBtn_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailBtn_ViewBinder;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bgActive_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bgActive_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
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
		[HideInInspector] public ArabLayoutCompment m_img_redpot_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MailBtn_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg");

			m_img_bgActive_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bgActive");
			m_img_bgActive_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bgActive");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"texts/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"texts/lbl_title");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"texts/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"texts/lbl_desc");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"texts/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"texts/lbl_time");

			m_img_bag_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bag");
			m_img_bag_Animation = FindUI<Animation>(gameObject.transform ,"img_bag");
			m_img_bag_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bag");

			m_btn_click_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_click");
			m_btn_click_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"btn_click");

			m_img_redpot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_redpot");
			m_img_redpot_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_redpot");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_MailGift_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_MailGift_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_MailGift";

        public UI_Model_MailGift_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_MailGift_ViewBinder;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;
		[HideInInspector] public UI_Item_Bag_SubView m_UI_Item_Bag;
		[HideInInspector] public ViewBinder m_pl_cur_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_cur_PolygonImage;
		[HideInInspector] public GameButton m_btn_cur_GameButton;
		[HideInInspector] public BtnAnimation m_btn_cur_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_curicon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public Shadow m_lbl_count_Shadow;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_MailGift_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_ClickAnimeMsg_btn"));
			m_UI_Item_Bag = new UI_Item_Bag_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_Bag"));
			m_pl_cur_ViewBinder = FindUI<ViewBinder>(gameObject.transform ,"pl_cur");

			m_btn_cur_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_cur/btn_cur");
			m_btn_cur_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_cur/btn_cur");
			m_btn_cur_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"pl_cur/btn_cur");

			m_img_curicon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_cur/btn_cur/img_curicon");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_cur/btn_cur/lbl_count");
			m_lbl_count_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_cur/btn_cur/lbl_count");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");


			BindEvent();
        }

        #endregion
    }
}
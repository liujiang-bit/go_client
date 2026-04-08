// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月3日
// Update Time         :    2020年7月3日
// Class Description   :    UI_Model_MailGiftView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Model_MailGiftView : GameView
    {
		public const string VIEW_NAME = "UI_Model_MailGift";

        public UI_Model_MailGiftView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_Bag_SubView m_UI_Item_Bag;
		[HideInInspector] public ViewBinder m_pl_cur_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_cur_PolygonImage;
		[HideInInspector] public GameButton m_btn_cur_GameButton;
		[HideInInspector] public BtnAnimation m_btn_cur_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_curicon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public Shadow m_lbl_count_Shadow;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_Bag = new UI_Item_Bag_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_Bag"));
			m_pl_cur_ViewBinder = FindUI<ViewBinder>(vb.transform ,"pl_cur");

			m_btn_cur_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_cur/btn_cur");
			m_btn_cur_GameButton = FindUI<GameButton>(vb.transform ,"pl_cur/btn_cur");
			m_btn_cur_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_cur/btn_cur");

			m_img_curicon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_cur/btn_cur/img_curicon");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_cur/btn_cur/lbl_count");
			m_lbl_count_Shadow = FindUI<Shadow>(vb.transform ,"pl_cur/btn_cur/lbl_count");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WarMailBar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WarMailBar_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WarMailBar";

        public UI_Item_WarMailBar_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_WarMailBar_ViewBinder;

		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public PolygonImage m_btn_bar_PolygonImage;
		[HideInInspector] public GameButton m_btn_bar_GameButton;

		[HideInInspector] public PolygonImage m_img_fight_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_img_circle_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_circle_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_build_PolygonImage;
		[HideInInspector] public ViewBinder m_img_build_ViewBinder;



        private void UIFinder()
        {       
			m_UI_Item_WarMailBar_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_content = FindUI<RectTransform>(gameObject.transform ,"pl_content");
			m_btn_bar_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_content/btn_bar");
			m_btn_bar_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_content/btn_bar");

			m_img_fight_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_content/btn_bar/img_fight");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_content/btn_bar/img_fight/UI_Model_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_content/btn_bar/img_fight/lbl_name");

			m_img_circle_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_content/btn_bar/img_fight/img_circle");
			m_img_circle_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_content/btn_bar/img_fight/img_circle");

			m_img_build_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_content/btn_bar/img_fight/img_circle/img_build");
			m_img_build_ViewBinder = FindUI<ViewBinder>(gameObject.transform ,"pl_content/btn_bar/img_fight/img_circle/img_build");


			BindEvent();
        }

        #endregion
    }
}
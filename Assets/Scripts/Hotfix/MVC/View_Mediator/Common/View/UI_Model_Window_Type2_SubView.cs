// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Window_Type2_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Model_Window_Type2_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Window_Type2";

        public UI_Model_Window_Type2_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_Window_Type2;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;
		[HideInInspector] public BtnAnimation m_btn_close_ButtonAnimation;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public BtnAnimation m_btn_back_ButtonAnimation;

		[HideInInspector] public GridLayoutGroup m_pl_page_GridLayoutGroup;

		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side1;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side2;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side3;
		[HideInInspector] public UI_Model_SideToggle_SubView m_pl_side4;


        private void UIFinder()
        {       
			m_UI_Model_Window_Type2 = gameObject.GetComponent<RectTransform>();
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(gameObject.transform ,"title/lbl_title");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"title/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(gameObject.transform ,"title/btn_close");
			m_btn_close_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"title/btn_close");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"title/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"title/btn_back");
			m_btn_back_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"title/btn_back");

			m_pl_page_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_page");

			m_pl_side1 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_page/pl_side1"));
			m_pl_side2 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_page/pl_side2"));
			m_pl_side3 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_page/pl_side3"));
			m_pl_side4 = new UI_Model_SideToggle_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_page/pl_side4"));

			BindEvent();
        }

        #endregion
    }
}
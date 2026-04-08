// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月19日
// Update Time         :    2020年3月19日
// Class Description   :    UI_Item_PlayerArmyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_PlayerArmyView : GameView
    {
		public const string VIEW_NAME = "UI_Item_PlayerArmy";

        public UI_Item_PlayerArmyView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_playername_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playername_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHeadWithLevel_sub;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHeadWithLevel_main;
		[HideInInspector] public GridLayoutGroup m_UI_Model_ArmyDetailsColunm_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Model_ArmyDetailsColunm_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_col1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col4_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col5_LanguageText;

		[HideInInspector] public PolygonImage m_img_teamleader_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_teamleader_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_arrow_PolygonImage;
		[HideInInspector] public GameButton m_btn_arrow_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_arrow_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_down_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrow_left_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_playername_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_playername");
			m_lbl_playername_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_playername");

			m_UI_Model_CaptainHeadWithLevel_sub = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_CaptainHeadWithLevel_sub"));
			m_UI_Model_CaptainHeadWithLevel_main = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_CaptainHeadWithLevel_main"));
			m_UI_Model_ArmyDetailsColunm_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"UI_Model_ArmyDetailsColunm");
			m_UI_Model_ArmyDetailsColunm_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Model_ArmyDetailsColunm");

			m_lbl_col1_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col1");

			m_lbl_col2_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col2");

			m_lbl_col3_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col3");

			m_lbl_col4_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col4");

			m_lbl_col5_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col5");

			m_img_teamleader_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_teamleader");
			m_img_teamleader_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_teamleader");

			m_btn_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_arrow");
			m_btn_arrow_GameButton = FindUI<GameButton>(vb.transform ,"btn_arrow");
			m_btn_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_arrow");

			m_img_arrow_down_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_arrow/img_arrow_down");

			m_img_arrow_left_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_arrow/img_arrow_left");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
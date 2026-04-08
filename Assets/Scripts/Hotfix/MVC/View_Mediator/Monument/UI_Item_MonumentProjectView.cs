// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月11日
// Update Time         :    2020年5月11日
// Class Description   :    UI_Item_MonumentProjectView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_MonumentProjectView : GameView
    {
		public const string VIEW_NAME = "UI_Item_MonumentProject";

        public UI_Item_MonumentProjectView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_tips_LanguageText;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_awardText_LanguageText;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public UI_Model_MonumentItem_SubView m_UI_MonumentItem;
		[HideInInspector] public Empty4Raycast m_btn_receiveReward_Empty4Raycast;
		[HideInInspector] public GameButton m_btn_receiveReward_GameButton;

		[HideInInspector] public PolygonImage m_img_reddot_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_reddot_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_status_LanguageText;

		[HideInInspector] public PolygonImage m_btn_check01_PolygonImage;
		[HideInInspector] public GameButton m_btn_check01_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_check01_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public LanguageText m_lbl_milestone01_LanguageText;

		[HideInInspector] public LanguageText m_lbl_milestone02_LanguageText;

		[HideInInspector] public PolygonImage m_btn_check02_PolygonImage;
		[HideInInspector] public GameButton m_btn_check02_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_check02_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_milestoneBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_milestoneBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_process_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_tips_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_tips");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_title");

			m_img_img_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/img_img");

			m_lbl_awardText_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_awardText");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"bg/Item/UI_Model_Item"));
			m_UI_MonumentItem = new UI_Model_MonumentItem_SubView(FindUI<RectTransform>(vb.transform ,"bg/Item/UI_MonumentItem"));
			m_btn_receiveReward_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"bg/btn_receiveReward");
			m_btn_receiveReward_GameButton = FindUI<GameButton>(vb.transform ,"bg/btn_receiveReward");

			m_img_reddot_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/img_reddot");
			m_img_reddot_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"bg/img_reddot");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"bg/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"bg/btn_info");

			m_lbl_status_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_status");

			m_btn_check01_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/btn_check01");
			m_btn_check01_GameButton = FindUI<GameButton>(vb.transform ,"bg/btn_check01");
			m_btn_check01_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"bg/btn_check01");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_desc");

			m_lbl_milestone01_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/pl/lbl_milestone01");

			m_lbl_milestone02_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/pl/lbl_milestone02");

			m_btn_check02_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/pl/btn_check02");
			m_btn_check02_GameButton = FindUI<GameButton>(vb.transform ,"bg/pl/btn_check02");
			m_btn_check02_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"bg/pl/btn_check02");

			m_pb_milestoneBar_GameSlider = FindUI<GameSlider>(vb.transform ,"bg/pl/pb_milestoneBar");
			m_pb_milestoneBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"bg/pl/pb_milestoneBar");

			m_lbl_process_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/pl/pb_milestoneBar/lbl_process");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
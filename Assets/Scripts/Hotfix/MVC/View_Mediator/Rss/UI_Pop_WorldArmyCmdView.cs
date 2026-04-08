// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月3日
// Update Time         :    2020年8月3日
// Class Description   :    UI_Pop_WorldArmyCmdView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_WorldArmyCmdView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_WorldArmyCmd";

        public UI_Pop_WorldArmyCmdView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_head;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public Shadow m_lbl_name_Shadow;

		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag;
		[HideInInspector] public LanguageText m_lbl_ScoutsNameName_LanguageText;

		[HideInInspector] public PolygonImage m_img_line_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_namebg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;

		[HideInInspector] public Animator m_pl_captainhead_Animator;

		[HideInInspector] public GameSlider m_pb_Hp_GameSlider;

		[HideInInspector] public GameSlider m_pl_sd_GameSlider_GameSlider;

		[HideInInspector] public PolygonImage m_img_Fill_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_FightPlayerName_LanguageText;
		[HideInInspector] public Shadow m_lbl_FightPlayerName_Shadow;
		[HideInInspector] public ContentSizeFitter m_lbl_FightPlayerName_ContentSizeFitter;

		[HideInInspector] public RectTransform m_pl_ap;
		[HideInInspector] public PolygonImage m_img_apbg_PolygonImage;

		[HideInInspector] public UI_Item_WorldArmyCmdAp_SubView m_UI_Item_WorldArmyCmdAp;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_btn_captainHead_PolygonImage;
		[HideInInspector] public GameButton m_btn_captainHead_GameButton;

		[HideInInspector] public Animator m_pl_beTarget_Animator;

		[HideInInspector] public CanvasGroup m_pl_subCaptain_CanvasGroup;
		[HideInInspector] public Animator m_pl_subCaptain_Animator;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_SubCaptain;
		[HideInInspector] public RectTransform m_pl_time;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_time_ContentSizeFitter;

		[HideInInspector] public UI_Item_CMDBtns_SubView m_UI_Item_CMDBtns;
		[HideInInspector] public RectTransform m_pl_state;
		[HideInInspector] public PolygonImage m_img_state_fail_PolygonImage;

		[HideInInspector] public PolygonImage m_img_state_stop_PolygonImage;

		[HideInInspector] public PolygonImage m_icon_state_stop_PolygonImage;

		[HideInInspector] public LanguageText m_pl_stateName_LanguageText;
		[HideInInspector] public ContentSizeFitter m_pl_stateName_ContentSizeFitter;
		[HideInInspector] public Shadow m_pl_stateName_Shadow;

		[HideInInspector] public UI_Model_GuildFlag_SubView m_UI_GuildFlag1;
		[HideInInspector] public PolygonImage m_img_state_atk_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_head = FindUI<RectTransform>(vb.transform ,"pl_head");
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_head/lbl_name");
			m_lbl_name_Shadow = FindUI<Shadow>(vb.transform ,"pl_head/lbl_name");

			m_UI_GuildFlag = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"pl_head/lbl_name/UI_GuildFlag"));
			m_lbl_ScoutsNameName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_head/lbl_ScoutsNameName");

			m_img_line_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_head/img_line");

			m_pl_namebg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_head/pl_namebg");

			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_head/pl_namebg/lbl_count");

			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_head/pl_namebg/lbl_playerName");

			m_pl_captainhead_Animator = FindUI<Animator>(vb.transform ,"pl_head/pl_captainhead");

			m_pb_Hp_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_head/pl_captainhead/pb_Hp");

			m_pl_sd_GameSlider_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_head/pl_captainhead/pb_Hp/pl_sd_GameSlider");

			m_img_Fill_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_head/pl_captainhead/pb_Hp/pl_sd_GameSlider/Fill Area/img_Fill");

			m_lbl_FightPlayerName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_head/pl_captainhead/pb_Hp/lbl_FightPlayerName");
			m_lbl_FightPlayerName_Shadow = FindUI<Shadow>(vb.transform ,"pl_head/pl_captainhead/pb_Hp/lbl_FightPlayerName");
			m_lbl_FightPlayerName_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_head/pl_captainhead/pb_Hp/lbl_FightPlayerName");

			m_pl_ap = FindUI<RectTransform>(vb.transform ,"pl_head/pl_captainhead/pl_ap");
			m_img_apbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_head/pl_captainhead/pl_ap/img_apbg");

			m_UI_Item_WorldArmyCmdAp = new UI_Item_WorldArmyCmdAp_SubView(FindUI<RectTransform>(vb.transform ,"pl_head/pl_captainhead/pl_ap/UI_Item_WorldArmyCmdAp"));
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_head/pl_captainhead/UI_Model_CaptainHead"));
			m_btn_captainHead_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_head/pl_captainhead/btn_captainHead");
			m_btn_captainHead_GameButton = FindUI<GameButton>(vb.transform ,"pl_head/pl_captainhead/btn_captainHead");

			m_pl_beTarget_Animator = FindUI<Animator>(vb.transform ,"pl_head/pl_captainhead/pl_beTarget");

			m_pl_subCaptain_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_head/pl_subCaptain");
			m_pl_subCaptain_Animator = FindUI<Animator>(vb.transform ,"pl_head/pl_subCaptain");

			m_UI_SubCaptain = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_head/pl_subCaptain/UI_SubCaptain"));
			m_pl_time = FindUI<RectTransform>(vb.transform ,"pl_time");
			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_time/lbl_time");
			m_lbl_time_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_time/lbl_time");

			m_UI_Item_CMDBtns = new UI_Item_CMDBtns_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_CMDBtns"));
			m_pl_state = FindUI<RectTransform>(vb.transform ,"pl_state");
			m_img_state_fail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_state/img_state_fail");

			m_img_state_stop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_state/img_state_stop");

			m_icon_state_stop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_state/img_state_stop/icon_state_stop");

			m_pl_stateName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_state/pl_stateName");
			m_pl_stateName_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_state/pl_stateName");
			m_pl_stateName_Shadow = FindUI<Shadow>(vb.transform ,"pl_state/pl_stateName");

			m_UI_GuildFlag1 = new UI_Model_GuildFlag_SubView(FindUI<RectTransform>(vb.transform ,"pl_state/pl_stateName/UI_GuildFlag1"));
			m_img_state_atk_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_state/img_state_atk");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
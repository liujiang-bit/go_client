// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月3日
// Update Time         :    2020年8月3日
// Class Description   :    UI_Tip_WorldObjectPvPView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_WorldObjectPvPView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_WorldObjectPvP";

        public UI_Tip_WorldObjectPvPView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_buildingHp;
		[HideInInspector] public GameSlider m_sd_buildingHpSilder_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_sd_buildingHpSilder_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_head;
		[HideInInspector] public Animator m_pl_captainhead_Animator;

		[HideInInspector] public GameSlider m_pb_Hp_GameSlider;

		[HideInInspector] public GameSlider m_pl_sd_GameSlider_GameSlider;

		[HideInInspector] public PolygonImage m_img_Fill_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_FightPlayerName_LanguageText;

		[HideInInspector] public RectTransform m_pl_ap;
		[HideInInspector] public PolygonImage m_img_apbg_PolygonImage;

		[HideInInspector] public UI_Item_WorldArmyCmdAp_SubView m_UI_Item_WorldArmyCmdAp;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public Animator m_pl_beTarget_Animator;

		[HideInInspector] public CanvasGroup m_pl_subCaptain_CanvasGroup;
		[HideInInspector] public Animator m_pl_subCaptain_Animator;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_SubCaptain;
		[HideInInspector] public PolygonImage m_pl_bg0_PolygonImage;
		[HideInInspector] public ContentSizeFitter m_pl_bg0_ContentSizeFitter;
		[HideInInspector] public HorizontalLayoutGroup m_pl_bg0_HorizontalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_playerName_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_pl_bg1_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_level_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_buildingHp = FindUI<RectTransform>(vb.transform ,"pl_buildingHp");
			m_sd_buildingHpSilder_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_buildingHp/sd_buildingHpSilder");
			m_sd_buildingHpSilder_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_buildingHp/sd_buildingHpSilder");

			m_pl_head = FindUI<RectTransform>(vb.transform ,"pl_head");
			m_pl_captainhead_Animator = FindUI<Animator>(vb.transform ,"pl_head/pl_captainhead");

			m_pb_Hp_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_head/pl_captainhead/pb_Hp");

			m_pl_sd_GameSlider_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_head/pl_captainhead/pb_Hp/pl_sd_GameSlider");

			m_img_Fill_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_head/pl_captainhead/pb_Hp/pl_sd_GameSlider/Fill Area/img_Fill");

			m_lbl_FightPlayerName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_head/pl_captainhead/pb_Hp/lbl_FightPlayerName");

			m_pl_ap = FindUI<RectTransform>(vb.transform ,"pl_head/pl_captainhead/pl_ap");
			m_img_apbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_head/pl_captainhead/pl_ap/img_apbg");

			m_UI_Item_WorldArmyCmdAp = new UI_Item_WorldArmyCmdAp_SubView(FindUI<RectTransform>(vb.transform ,"pl_head/pl_captainhead/pl_ap/UI_Item_WorldArmyCmdAp"));
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_head/pl_captainhead/UI_Model_CaptainHead"));
			m_pl_beTarget_Animator = FindUI<Animator>(vb.transform ,"pl_head/pl_captainhead/pl_beTarget");

			m_pl_subCaptain_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_head/pl_subCaptain");
			m_pl_subCaptain_Animator = FindUI<Animator>(vb.transform ,"pl_head/pl_subCaptain");

			m_UI_SubCaptain = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_head/pl_subCaptain/UI_SubCaptain"));
			m_pl_bg0_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_bg0");
			m_pl_bg0_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_bg0");
			m_pl_bg0_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"pl_bg0");

			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_bg0/lbl_playerName");
			m_lbl_playerName_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_bg0/lbl_playerName");

			m_pl_bg1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_bg1");

			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_bg1/lbl_level");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
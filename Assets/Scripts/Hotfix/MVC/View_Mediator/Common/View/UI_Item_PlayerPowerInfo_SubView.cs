// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_PlayerPowerInfo_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_PlayerPowerInfo_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_PlayerPowerInfo";

        public UI_Item_PlayerPowerInfo_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_PlayerPowerInfo_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_PlayerPowerInfo_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_PlayerPowerInfo_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public Animator m_Pl_Power_Animator;
		[HideInInspector] public ArabLayoutCompment m_Pl_Power_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_sword_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_sword_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_powerVal_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_powerVal_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_powerVal_Shadow;

		[HideInInspector] public PolygonImage m_btn_powerShow_PolygonImage;
		[HideInInspector] public GameButton m_btn_powerShow_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_powerShow_ArabLayoutCompment;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;
		[HideInInspector] public Animator m_Pl_Vip_Animator;
		[HideInInspector] public ArabLayoutCompment m_Pl_Vip_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_vip_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_vip_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_vip_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_vip_ArabLayoutCompment;
		[HideInInspector] public FontGradient m_lbl_vip_FontGradient;
		[HideInInspector] public Shadow m_lbl_vip_Shadow;

		[HideInInspector] public PolygonImage m_btn_vip_PolygonImage;
		[HideInInspector] public GameButton m_btn_vip_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_vip_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_redpoint_ArabLayoutCompment;

		[HideInInspector] public Animator m_Pl_Head_Animator;
		[HideInInspector] public ArabLayoutCompment m_Pl_Head_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_playerHeadIcon_PolygonImage;
		[HideInInspector] public GameButton m_btn_playerHeadIcon_GameButton;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_img_head;
		[HideInInspector] public GameSlider m_pb_ap_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_ap_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bottle_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bottle_ArabLayoutCompment;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;
		[HideInInspector] public ArabLayoutCompment m_Pl_Arrow_ArabLayoutCompment;
		[HideInInspector] public Empty4Raycast m_Pl_Arrow_Empty4Raycast;

		[HideInInspector] public PolygonImage m_btn_arrow_PolygonImage;
		[HideInInspector] public GameButton m_btn_arrow_GameButton;

		[HideInInspector] public ArabLayoutCompment m_Pl_Buffs_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_Pl_Buffs_GridLayoutGroup;
		[HideInInspector] public RectMask2D m_Pl_Buffs_RectMask2D;

		[HideInInspector] public UI_Item_MainIFBuff_SubView m_UI_Item_MainIFBuff;
		[HideInInspector] public UI_Tag_MainIFAnime_Left_SubView m_UI_Tag_MainIFAnime_Left;


        private void UIFinder()
        {       
			m_UI_Item_PlayerPowerInfo_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_PlayerPowerInfo_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
			m_UI_Item_PlayerPowerInfo_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg");

			m_Pl_Power_Animator = FindUI<Animator>(gameObject.transform ,"Pl_Power");
			m_Pl_Power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Power");

			m_img_sword_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"Pl_Power/img_sword");
			m_img_sword_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Power/img_sword");

			m_lbl_powerVal_LanguageText = FindUI<LanguageText>(gameObject.transform ,"Pl_Power/lbl_powerVal");
			m_lbl_powerVal_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Power/lbl_powerVal");
			m_lbl_powerVal_Shadow = FindUI<Shadow>(gameObject.transform ,"Pl_Power/lbl_powerVal");

			m_btn_powerShow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"Pl_Power/btn_powerShow");
			m_btn_powerShow_GameButton = FindUI<GameButton>(gameObject.transform ,"Pl_Power/btn_powerShow");
			m_btn_powerShow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Power/btn_powerShow");

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(gameObject.transform ,"Pl_Power/btn_powerShow/UI_Tag_ClickAnimeMsg_btn"));
			m_Pl_Vip_Animator = FindUI<Animator>(gameObject.transform ,"Pl_Vip");
			m_Pl_Vip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Vip");

			m_img_vip_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"Pl_Vip/img_vip");
			m_img_vip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Vip/img_vip");

			m_lbl_vip_LanguageText = FindUI<LanguageText>(gameObject.transform ,"Pl_Vip/lbl_vip");
			m_lbl_vip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Vip/lbl_vip");
			m_lbl_vip_FontGradient = FindUI<FontGradient>(gameObject.transform ,"Pl_Vip/lbl_vip");
			m_lbl_vip_Shadow = FindUI<Shadow>(gameObject.transform ,"Pl_Vip/lbl_vip");

			m_btn_vip_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"Pl_Vip/btn_vip");
			m_btn_vip_GameButton = FindUI<GameButton>(gameObject.transform ,"Pl_Vip/btn_vip");
			m_btn_vip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Vip/btn_vip");

			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"Pl_Vip/img_redpoint");
			m_img_redpoint_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Vip/img_redpoint");

			m_Pl_Head_Animator = FindUI<Animator>(gameObject.transform ,"Pl_Head");
			m_Pl_Head_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Head");

			m_btn_playerHeadIcon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"Pl_Head/btn_playerHeadIcon");
			m_btn_playerHeadIcon_GameButton = FindUI<GameButton>(gameObject.transform ,"Pl_Head/btn_playerHeadIcon");

			m_img_head = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"Pl_Head/img_head"));
			m_pb_ap_GameSlider = FindUI<GameSlider>(gameObject.transform ,"Pl_Head/pb_ap");
			m_pb_ap_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Head/pb_ap");

			m_img_bottle_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"Pl_Head/img_bottle");
			m_img_bottle_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Head/img_bottle");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"Pl_Head/UI_Common_Redpoint"));
			m_Pl_Arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Arrow");
			m_Pl_Arrow_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"Pl_Arrow");

			m_btn_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"Pl_Arrow/btn_arrow");
			m_btn_arrow_GameButton = FindUI<GameButton>(gameObject.transform ,"Pl_Arrow/btn_arrow");

			m_Pl_Buffs_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"Pl_Buffs");
			m_Pl_Buffs_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"Pl_Buffs");
			m_Pl_Buffs_RectMask2D = FindUI<RectMask2D>(gameObject.transform ,"Pl_Buffs");

			m_UI_Item_MainIFBuff = new UI_Item_MainIFBuff_SubView(FindUI<RectTransform>(gameObject.transform ,"Pl_Buffs/UI_Item_MainIFBuff"));
			m_UI_Tag_MainIFAnime_Left = new UI_Tag_MainIFAnime_Left_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_MainIFAnime_Left"));

			BindEvent();
        }

        #endregion
    }
}
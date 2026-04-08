// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventNarmer_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventNarmer_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventNarmer";

        public UI_Item_EventNarmer_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventNarmer;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public PolygonImage m_img_activitybg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_coming_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_coming_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;
		[HideInInspector] public Outline m_lbl_time_Outline;

		[HideInInspector] public GridLayoutGroup m_pl_rewards_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_rewards_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item4;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item5;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_btn_go;
		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_pl_info_CanvasGroup;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_info_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_info_PolygonImage;
		[HideInInspector] public ListView m_sv_info_ListView;

		[HideInInspector] public PolygonImage m_v_info_PolygonImage;
		[HideInInspector] public Mask m_v_info_Mask;

		[HideInInspector] public ContentSizeFitter m_c_info_ContentSizeFitter;
		[HideInInspector] public VerticalLayoutGroup m_c_info_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_info_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_info_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_info_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EventNarmer = gameObject.GetComponent<RectTransform>();
			m_pl_mes = FindUI<RectTransform>(gameObject.transform ,"pl_mes");
			m_img_activitybg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_activitybg");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_title");

			m_lbl_coming_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_coming");
			m_lbl_coming_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_coming");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_desc");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_time");
			m_lbl_time_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/lbl_time");

			m_pl_rewards_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_rewards");
			m_pl_rewards_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_rewards");

			m_UI_Model_Item1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item4"));
			m_UI_Model_Item5 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item5"));
			m_btn_go = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/btn_go"));
			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_info");

			m_pl_info_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_info");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_info/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_info/btn_back");

			m_sv_info_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_info/sv_info");
			m_sv_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/sv_info");
			m_sv_info_ListView = FindUI<ListView>(gameObject.transform ,"pl_info/sv_info");

			m_v_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/sv_info/v_info");
			m_v_info_Mask = FindUI<Mask>(gameObject.transform ,"pl_info/sv_info/v_info");

			m_c_info_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_info/sv_info/v_info/c_info");
			m_c_info_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_info/sv_info/v_info/c_info");

			m_lbl_info_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");
			m_lbl_info_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");
			m_lbl_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventStrongerPlayer1_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventStrongerPlayer1_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventStrongerPlayer1";

        public UI_Item_EventStrongerPlayer1_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventStrongerPlayer1;
		[HideInInspector] public PolygonImage m_img_big_bg_PolygonImage;

		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_coming_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_coming_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_rewards_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_rewards_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item4;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Model_StandardButton_Blue;


        private void UIFinder()
        {       
			m_UI_Item_EventStrongerPlayer1 = gameObject.GetComponent<RectTransform>();
			m_img_big_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_big_bg");

			m_pl_mes = FindUI<RectTransform>(gameObject.transform ,"pl_mes");
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_title");

			m_lbl_coming_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_coming");
			m_lbl_coming_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_coming");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_desc");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_time");

			m_pl_rewards_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_rewards");
			m_pl_rewards_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_rewards");

			m_UI_Model_Item1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_rewards/UI_Model_Item4"));
			m_UI_Model_StandardButton_Blue = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/UI_Model_StandardButton_Blue"));

			BindEvent();
        }

        #endregion
    }
}
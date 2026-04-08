// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_NewRoleActivityItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_NewRoleActivityItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_NewRoleActivityItem";

        public UI_Item_NewRoleActivityItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_NewRoleActivityItem;
		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_rewards_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_rewards_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item4;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item5;
		[HideInInspector] public LanguageText m_lbl_process_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_process_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_go;
		[HideInInspector] public UI_Model_StandardButton_MiniGreen_SubView m_btn_get;
		[HideInInspector] public LanguageText m_lbl_already_LanguageText;
		[HideInInspector] public Shadow m_lbl_already_Shadow;
		[HideInInspector] public ArabLayoutCompment m_lbl_already_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_NewRoleActivityItem = gameObject.GetComponent<RectTransform>();
			m_pl_view = FindUI<RectTransform>(gameObject.transform ,"pl_view");
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/lbl_title");

			m_pl_rewards_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_view/pl_rewards");
			m_pl_rewards_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/pl_rewards");

			m_UI_Model_Item1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_rewards/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_rewards/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_rewards/UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_rewards/UI_Model_Item4"));
			m_UI_Model_Item5 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/pl_rewards/UI_Model_Item5"));
			m_lbl_process_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view/lbl_process");
			m_lbl_process_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/lbl_process");

			m_btn_go = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/btn_go"));
			m_btn_get = new UI_Model_StandardButton_MiniGreen_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/btn_get"));
			m_lbl_already_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view/lbl_already");
			m_lbl_already_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_view/lbl_already");
			m_lbl_already_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_view/lbl_already");


			BindEvent();
        }

        #endregion
    }
}
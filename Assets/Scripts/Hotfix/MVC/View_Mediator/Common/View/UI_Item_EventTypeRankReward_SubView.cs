// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTypeRankReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTypeRankReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTypeRankReward";

        public UI_Item_EventTypeRankReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_EventTypeRankReward_ViewBinder;

		[HideInInspector] public PolygonImage m_img_title_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_reward_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reward_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item4;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_Item5;


        private void UIFinder()
        {       
			m_UI_Item_EventTypeRankReward_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_title_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_title");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_pl_reward_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_reward");
			m_pl_reward_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_reward");

			m_UI_Model_Item1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Model_Item4"));
			m_UI_Model_Item5 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Model_Item5"));

			BindEvent();
        }

        #endregion
    }
}
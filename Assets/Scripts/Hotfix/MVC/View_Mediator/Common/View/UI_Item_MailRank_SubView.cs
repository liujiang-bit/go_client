// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailRank_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailRank_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailRank";

        public UI_Item_MailRank_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailRank;
		[HideInInspector] public LanguageText m_lbl_rank_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_rank_title_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_reward_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reward_ArabLayoutCompment;

		[HideInInspector] public UI_Item_MailRankReward_SubView m_UI_Item_MailRankReward1;
		[HideInInspector] public UI_Item_MailRankReward_SubView m_UI_Item_MailRankReward2;
		[HideInInspector] public UI_Item_MailRankReward_SubView m_UI_Item_MailRankReward3;
		[HideInInspector] public UI_Item_MailRankReward_SubView m_UI_Item_MailRankReward4;
		[HideInInspector] public UI_Item_MailRankReward_SubView m_UI_Item_MailRankReward5;
		[HideInInspector] public ContentSizeFitter m_pl_player_ContentSizeFitter;
		[HideInInspector] public GridLayoutGroup m_pl_player_GridLayoutGroup;

		[HideInInspector] public UI_Item_MailRankChild_SubView m_UI_Item_MailRankChild;


        private void UIFinder()
        {       
			m_UI_Item_MailRank = gameObject.GetComponent<RectTransform>();
			m_lbl_rank_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_rank_title");
			m_lbl_rank_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_rank_title");

			m_pl_reward_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_reward");
			m_pl_reward_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_reward");

			m_UI_Item_MailRankReward1 = new UI_Item_MailRankReward_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Item_MailRankReward1"));
			m_UI_Item_MailRankReward2 = new UI_Item_MailRankReward_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Item_MailRankReward2"));
			m_UI_Item_MailRankReward3 = new UI_Item_MailRankReward_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Item_MailRankReward3"));
			m_UI_Item_MailRankReward4 = new UI_Item_MailRankReward_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Item_MailRankReward4"));
			m_UI_Item_MailRankReward5 = new UI_Item_MailRankReward_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_reward/UI_Item_MailRankReward5"));
			m_pl_player_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_player");
			m_pl_player_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_player");

			m_UI_Item_MailRankChild = new UI_Item_MailRankChild_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_player/UI_Item_MailRankChild"));

			BindEvent();
        }

        #endregion
    }
}
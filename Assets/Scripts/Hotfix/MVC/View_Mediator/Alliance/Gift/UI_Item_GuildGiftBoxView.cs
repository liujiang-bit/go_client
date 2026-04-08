// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, June 8, 2020
// Update Time         :    Monday, June 8, 2020
// Class Description   :    UI_Item_GuildGiftBoxView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildGiftBoxView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildGiftBox";

        public UI_Item_GuildGiftBoxView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_pl_reward_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_key_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_keyCount_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_source_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_sourceCount_LanguageText;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet;
		[HideInInspector] public LanguageText m_lbl_boxName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_boxName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_passTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_passTime_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_state_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_get;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_reward_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_reward");

			m_pl_key_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_reward/pl_key");

			m_lbl_keyCount_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_reward/pl_key/lbl_keyCount");

			m_pl_source_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_reward/pl_source");

			m_lbl_sourceCount_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_reward/pl_source/lbl_sourceCount");

			m_UI_Model_RewardGet = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_RewardGet"));
			m_lbl_boxName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_boxName");
			m_lbl_boxName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_boxName");

			m_lbl_passTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_passTime");
			m_lbl_passTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_passTime");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_desc");

			m_lbl_state_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_state");
			m_lbl_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_state");

			m_UI_get = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_get"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
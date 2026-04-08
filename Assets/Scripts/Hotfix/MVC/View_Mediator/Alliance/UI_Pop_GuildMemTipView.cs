// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, June 4, 2020
// Update Time         :    Thursday, June 4, 2020
// Class Description   :    UI_Pop_GuildMemTipView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_GuildMemTipView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_GuildMemTip";

        public UI_Pop_GuildMemTipView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_pl_bg_GridLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_pl_bg_ContentSizeFitter;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_mail;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_info;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_help;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_res_help;
		[HideInInspector] public UI_Model_StandardButton_MiniRed_SubView m_UI_remove;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_plus;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_bg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_bg");
			m_pl_bg_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_bg");

			m_UI_mail = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_bg/UI_mail"));
			m_UI_info = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_bg/UI_info"));
			m_UI_help = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_bg/UI_help"));
			m_UI_res_help = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_bg/UI_res_help"));
			m_UI_remove = new UI_Model_StandardButton_MiniRed_SubView(FindUI<RectTransform>(vb.transform ,"pl_bg/UI_remove"));
			m_UI_plus = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_bg/UI_plus"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
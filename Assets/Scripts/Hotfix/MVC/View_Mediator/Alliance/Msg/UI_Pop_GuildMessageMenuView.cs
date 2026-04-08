// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, July 7, 2020
// Update Time         :    Tuesday, July 7, 2020
// Class Description   :    UI_Pop_GuildMessageMenuView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_GuildMessageMenuView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_GuildMessageMenu";

        public UI_Pop_GuildMessageMenuView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public RectTransform m_pl_offset;
		[HideInInspector] public GridLayoutGroup m_pl_bg_GridLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_pl_bg_ContentSizeFitter;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_copy;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_rep;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_imgRep;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_del;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_report;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_pl_offset = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset");
			m_pl_bg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_pos/pl_offset/pl_bg");
			m_pl_bg_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_pos/pl_offset/pl_bg");

			m_UI_copy = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/pl_bg/UI_copy"));
			m_UI_rep = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/pl_bg/UI_rep"));
			m_UI_imgRep = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/pl_bg/UI_imgRep"));
			m_UI_del = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/pl_bg/UI_del"));
			m_UI_report = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/pl_offset/pl_bg/UI_report"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
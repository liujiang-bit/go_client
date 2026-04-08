// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月17日
// Update Time         :    2020年2月17日
// Class Description   :    UI_Item_ChapterQuestView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ChapterQuestView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ChapterQuest";

        public UI_Item_ChapterQuestView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_finish_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_Blue;
		[HideInInspector] public UI_Model_StandardButton_MiniGreen_SubView m_UI_Model_Green;
		[HideInInspector] public LanguageText m_lbl_itemName_LanguageText;

		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesConsume;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_finish_LanguageText = FindUI<LanguageText>(vb.transform ,"btn/lbl_finish");

			m_UI_Model_Blue = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"btn/UI_Model_Blue"));
			m_UI_Model_Green = new UI_Model_StandardButton_MiniGreen_SubView(FindUI<RectTransform>(vb.transform ,"btn/UI_Model_Green"));
			m_lbl_itemName_LanguageText = FindUI<LanguageText>(vb.transform ,"content/lbl_itemName");

			m_UI_Model_ResourcesConsume = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(vb.transform ,"content/reward/rewards/UI_Model_ResourcesConsume"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
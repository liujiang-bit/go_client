// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChapterQuest_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChapterQuest_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChapterQuest";

        public UI_Item_ChapterQuest_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ChapterQuest_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_finish_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_Blue;
		[HideInInspector] public UI_Model_StandardButton_MiniGreen_SubView m_UI_Model_Green;
		[HideInInspector] public LanguageText m_lbl_itemName_LanguageText;

		[HideInInspector] public UI_Model_ResourcesConsumeInCQ_SubView m_UI_Model_ResourcesConsume;


        private void UIFinder()
        {       
			m_UI_Item_ChapterQuest_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_finish_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn/lbl_finish");

			m_UI_Model_Blue = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"btn/UI_Model_Blue"));
			m_UI_Model_Green = new UI_Model_StandardButton_MiniGreen_SubView(FindUI<RectTransform>(gameObject.transform ,"btn/UI_Model_Green"));
			m_lbl_itemName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"content/lbl_itemName");

			m_UI_Model_ResourcesConsume = new UI_Model_ResourcesConsumeInCQ_SubView(FindUI<RectTransform>(gameObject.transform ,"content/reward/rewards/UI_Model_ResourcesConsume"));

			BindEvent();
        }

        #endregion
    }
}
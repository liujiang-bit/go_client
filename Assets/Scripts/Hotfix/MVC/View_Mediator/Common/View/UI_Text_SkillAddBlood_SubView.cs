// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Text_SkillAddBlood_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Text_SkillAddBlood_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Text_SkillAddBlood";

        public UI_Text_SkillAddBlood_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Text_SkillAddBlood;
		[HideInInspector] public LanguageText m_Txt_word_LanguageText;
		[HideInInspector] public Outline m_Txt_word_Outline;
		[HideInInspector] public Animator m_Txt_word_Animator;



        private void UIFinder()
        {       
			m_UI_Text_SkillAddBlood = gameObject.GetComponent<RectTransform>();
			m_Txt_word_LanguageText = FindUI<LanguageText>(gameObject.transform ,"Txt_word");
			m_Txt_word_Outline = FindUI<Outline>(gameObject.transform ,"Txt_word");
			m_Txt_word_Animator = FindUI<Animator>(gameObject.transform ,"Txt_word");


			BindEvent();
        }

        #endregion
    }
}
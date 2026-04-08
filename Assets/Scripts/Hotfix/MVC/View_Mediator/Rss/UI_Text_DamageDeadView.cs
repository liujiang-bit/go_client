// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月29日
// Update Time         :    2020年7月29日
// Class Description   :    UI_Text_DamageDeadView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Text_DamageDeadView : GameView
    {
		public const string VIEW_NAME = "UI_Text_DamageDead";

        public UI_Text_DamageDeadView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public Outline m_lbl_text_Outline;
		[HideInInspector] public Shadow m_lbl_text_Shadow;

		[HideInInspector] public LanguageText m_lbl_ordinaryAtk_LanguageText;
		[HideInInspector] public Animator m_lbl_ordinaryAtk_Animator;
		[HideInInspector] public Outline m_lbl_ordinaryAtk_Outline;

		[HideInInspector] public RectTransform m_pl_skillAtkOffset;
		[HideInInspector] public UI_Text_SkillAddBlood_SubView m_UI_Text_SkillAddBlood;
		[HideInInspector] public UI_Text_SkillAtk_SubView m_UI_Text_SkillAtk;
		[HideInInspector] public RectTransform m_pl_buffOffset;
		[HideInInspector] public LanguageText m_lbl_deBuff_LanguageText;
		[HideInInspector] public Outline m_lbl_deBuff_Outline;
		[HideInInspector] public Shadow m_lbl_deBuff_Shadow;
		[HideInInspector] public ContentSizeFitter m_lbl_deBuff_ContentSizeFitter;
		[HideInInspector] public Animator m_lbl_deBuff_Animator;
		[HideInInspector] public CanvasGroup m_lbl_deBuff_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_buff_LanguageText;
		[HideInInspector] public Outline m_lbl_buff_Outline;
		[HideInInspector] public Shadow m_lbl_buff_Shadow;
		[HideInInspector] public ContentSizeFitter m_lbl_buff_ContentSizeFitter;
		[HideInInspector] public Animator m_lbl_buff_Animator;
		[HideInInspector] public CanvasGroup m_lbl_buff_CanvasGroup;

		[HideInInspector] public RectTransform m_pl_atkbyatkOffset;
		[HideInInspector] public LanguageText m_lbl_atkbyatk_LanguageText;
		[HideInInspector] public Outline m_lbl_atkbyatk_Outline;
		[HideInInspector] public Animator m_lbl_atkbyatk_Animator;

		[HideInInspector] public RectTransform m_pl_routOffset;
		[HideInInspector] public LanguageText m_lbl_rout_LanguageText;
		[HideInInspector] public Outline m_lbl_rout_Outline;
		[HideInInspector] public Shadow m_lbl_rout_Shadow;
		[HideInInspector] public Animator m_lbl_rout_Animator;

		[HideInInspector] public LanguageText m_lbl_fail_LanguageText;
		[HideInInspector] public Outline m_lbl_fail_Outline;
		[HideInInspector] public Shadow m_lbl_fail_Shadow;
		[HideInInspector] public Animator m_lbl_fail_Animator;

		[HideInInspector] public UI_Item_CaptainLevelUpOnHead_SubView m_UI_CaptainLevelUpOnHead;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_text");
			m_lbl_text_Outline = FindUI<Outline>(vb.transform ,"lbl_text");
			m_lbl_text_Shadow = FindUI<Shadow>(vb.transform ,"lbl_text");

			m_lbl_ordinaryAtk_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_ordinaryAtk");
			m_lbl_ordinaryAtk_Animator = FindUI<Animator>(vb.transform ,"lbl_ordinaryAtk");
			m_lbl_ordinaryAtk_Outline = FindUI<Outline>(vb.transform ,"lbl_ordinaryAtk");

			m_pl_skillAtkOffset = FindUI<RectTransform>(vb.transform ,"pl_skillAtkOffset");
			m_UI_Text_SkillAddBlood = new UI_Text_SkillAddBlood_SubView(FindUI<RectTransform>(vb.transform ,"pl_skillAtkOffset/UI_Text_SkillAddBlood"));
			m_UI_Text_SkillAtk = new UI_Text_SkillAtk_SubView(FindUI<RectTransform>(vb.transform ,"pl_skillAtkOffset/UI_Text_SkillAtk"));
			m_pl_buffOffset = FindUI<RectTransform>(vb.transform ,"pl_buffOffset");
			m_lbl_deBuff_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_Outline = FindUI<Outline>(vb.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_Shadow = FindUI<Shadow>(vb.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_Animator = FindUI<Animator>(vb.transform ,"pl_buffOffset/lbl_deBuff");
			m_lbl_deBuff_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_buffOffset/lbl_deBuff");

			m_lbl_buff_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_Outline = FindUI<Outline>(vb.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_Shadow = FindUI<Shadow>(vb.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_Animator = FindUI<Animator>(vb.transform ,"pl_buffOffset/lbl_buff");
			m_lbl_buff_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_buffOffset/lbl_buff");

			m_pl_atkbyatkOffset = FindUI<RectTransform>(vb.transform ,"pl_atkbyatkOffset");
			m_lbl_atkbyatk_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_atkbyatkOffset/lbl_atkbyatk");
			m_lbl_atkbyatk_Outline = FindUI<Outline>(vb.transform ,"pl_atkbyatkOffset/lbl_atkbyatk");
			m_lbl_atkbyatk_Animator = FindUI<Animator>(vb.transform ,"pl_atkbyatkOffset/lbl_atkbyatk");

			m_pl_routOffset = FindUI<RectTransform>(vb.transform ,"pl_routOffset");
			m_lbl_rout_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_routOffset/lbl_rout");
			m_lbl_rout_Outline = FindUI<Outline>(vb.transform ,"pl_routOffset/lbl_rout");
			m_lbl_rout_Shadow = FindUI<Shadow>(vb.transform ,"pl_routOffset/lbl_rout");
			m_lbl_rout_Animator = FindUI<Animator>(vb.transform ,"pl_routOffset/lbl_rout");

			m_lbl_fail_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_routOffset/lbl_fail");
			m_lbl_fail_Outline = FindUI<Outline>(vb.transform ,"pl_routOffset/lbl_fail");
			m_lbl_fail_Shadow = FindUI<Shadow>(vb.transform ,"pl_routOffset/lbl_fail");
			m_lbl_fail_Animator = FindUI<Animator>(vb.transform ,"pl_routOffset/lbl_fail");

			m_UI_CaptainLevelUpOnHead = new UI_Item_CaptainLevelUpOnHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_CaptainLevelUpOnHead"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
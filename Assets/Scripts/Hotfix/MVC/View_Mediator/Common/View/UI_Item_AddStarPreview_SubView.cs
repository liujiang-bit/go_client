// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_AddStarPreview_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_AddStarPreview_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_AddStarPreview";

        public UI_Item_AddStarPreview_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_AddStarPreview;
		[HideInInspector] public LanguageText m_lbl_effect_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_effect_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar0;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar1;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar2;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar3;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar4;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_CaptainStar5;
		[HideInInspector] public LanguageText m_lbl_plus_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_plus_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_plus_Shadow;

		[HideInInspector] public ArabLayoutCompment m_pl_skill_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill;


        private void UIFinder()
        {       
			m_UI_Item_AddStarPreview = gameObject.GetComponent<RectTransform>();
			m_lbl_effect_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_effect");
			m_lbl_effect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_effect");

			m_UI_CaptainStar0 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"stars/UI_CaptainStar0"));
			m_UI_CaptainStar1 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"stars/UI_CaptainStar1"));
			m_UI_CaptainStar2 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"stars/UI_CaptainStar2"));
			m_UI_CaptainStar3 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"stars/UI_CaptainStar3"));
			m_UI_CaptainStar4 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"stars/UI_CaptainStar4"));
			m_UI_CaptainStar5 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(gameObject.transform ,"stars/UI_CaptainStar5"));
			m_lbl_plus_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_plus");
			m_lbl_plus_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_plus");
			m_lbl_plus_Shadow = FindUI<Shadow>(gameObject.transform ,"lbl_plus");

			m_pl_skill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_skill");

			m_UI_Item_CaptainSkill = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill/UI_Item_CaptainSkill"));

			BindEvent();
        }

        #endregion
    }
}
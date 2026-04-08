// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_UseAddItemInWin_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Pop_UseAddItemInWin_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_UseAddItemInWin";

        public UI_Pop_UseAddItemInWin_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Pop_UseAddItemInWin;
		[HideInInspector] public ArabLayoutCompment m_pl_offset_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_offset_Animator;
		[HideInInspector] public UIDefaultValue m_pl_offset_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_StandardButton_Blue_use;
		[HideInInspector] public UI_Tag_PopAnime_SkillTip_SubView m_UI_Tag_PopAnime_SkillTip;


        private void UIFinder()
        {       
			m_UI_Pop_UseAddItemInWin = gameObject.GetComponent<RectTransform>();
			m_pl_offset_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_offset");
			m_pl_offset_Animator = FindUI<Animator>(gameObject.transform ,"pl_offset");
			m_pl_offset_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"pl_offset");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_offset/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_offset/img_bg");

			m_UI_Model_StandardButton_Blue_use = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_offset/img_bg/UI_Model_StandardButton_Blue_use"));
			m_UI_Tag_PopAnime_SkillTip = new UI_Tag_PopAnime_SkillTip_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_PopAnime_SkillTip"));

			BindEvent();
        }

        #endregion
    }
}
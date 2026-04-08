// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TalentSkillPop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_TalentSkillPop_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TalentSkillPop";

        public UI_Item_TalentSkillPop_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_TalentSkillPop_Animator;
		[HideInInspector] public UIDefaultValue m_UI_Item_TalentSkillPop_UIDefaultValue;

		[HideInInspector] public PolygonImage m_btn_closeButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_closeButton_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_closeButton_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideL_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_skillname_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_skillname_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_skilllv_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_skilllv_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_leveup;


        private void UIFinder()
        {       
			m_UI_Item_TalentSkillPop_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_TalentSkillPop_UIDefaultValue = gameObject.GetComponent<UIDefaultValue>();

			m_btn_closeButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_closeButton");
			m_btn_closeButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_closeButton");
			m_btn_closeButton_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_closeButton");

			m_pl_pos = FindUI<RectTransform>(gameObject.transform ,"pl_pos");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideL");
			m_img_arrowSideL_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_lbl_skillname_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_pos/lbl_skillname");
			m_lbl_skillname_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos/lbl_skillname");

			m_lbl_skilllv_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_pos/lbl_skilllv");
			m_lbl_skilllv_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos/lbl_skilllv");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_pos/lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_pos/lbl_mes");

			m_btn_leveup = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_pos/btn_leveup"));

			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Win_TalentChangeName_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Win_TalentChangeName_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Win_TalentChangeName";

        public UI_Win_TalentChangeName_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Win_TalentChangeName_ViewBinder;
		[HideInInspector] public Animator m_UI_Win_TalentChangeName_Animator;
		[HideInInspector] public CanvasGroup m_UI_Win_TalentChangeName_CanvasGroup;

		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public PolygonImage m_ipt_name_PolygonImage;
		[HideInInspector] public GameInput m_ipt_name_GameInput;

		[HideInInspector] public LanguageText m_lbl_des_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_des_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_btn_sure;


        private void UIFinder()
        {       
			m_UI_Win_TalentChangeName_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Win_TalentChangeName_Animator = gameObject.GetComponent<Animator>();
			m_UI_Win_TalentChangeName_CanvasGroup = gameObject.GetComponent<CanvasGroup>();

			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Window_TypeMid"));
			m_ipt_name_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ipt_name");
			m_ipt_name_GameInput = FindUI<GameInput>(gameObject.transform ,"ipt_name");

			m_lbl_des_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_des");
			m_lbl_des_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_des");

			m_btn_sure = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_sure"));

			BindEvent();
        }

        #endregion
    }
}
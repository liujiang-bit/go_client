// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_BuildUpGo_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Pop_BuildUpGo_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_BuildUpGo";

        public UI_Pop_BuildUpGo_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Pop_BuildUpGo_ArabLayoutCompment;
		[HideInInspector] public Animator m_UI_Pop_BuildUpGo_Animator;
		[HideInInspector] public UIDefaultValue m_UI_Pop_BuildUpGo_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_mist_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_mist_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_state_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideR_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_times_GridLayoutGroup;
		[HideInInspector] public ToggleGroup m_pl_times_ToggleGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_times_ArabLayoutCompment;

		[HideInInspector] public UI_Item_BuildUpGoTime_SubView m_UI_Item_BuildUpGoTime0;
		[HideInInspector] public UI_Item_BuildUpGoTime_SubView m_UI_Item_BuildUpGoTime1;
		[HideInInspector] public UI_Item_BuildUpGoTime_SubView m_UI_Item_BuildUpGoTime2;
		[HideInInspector] public UI_Item_BuildUpGoTime_SubView m_UI_Item_BuildUpGoTime3;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_BuildUp;


        private void UIFinder()
        {       
			m_UI_Pop_BuildUpGo_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();
			m_UI_Pop_BuildUpGo_Animator = gameObject.GetComponent<Animator>();
			m_UI_Pop_BuildUpGo_UIDefaultValue = gameObject.GetComponent<UIDefaultValue>();

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/lbl_title");

			m_img_mist_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/img_mist");
			m_img_mist_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/img_mist");

			m_lbl_state_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/img_mist/lbl_state");
			m_lbl_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/img_mist/lbl_state");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/img_arrowSideR");
			m_img_arrowSideR_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/img_arrowSideR");

			m_pl_times_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"img_bg/pl_times");
			m_pl_times_ToggleGroup = FindUI<ToggleGroup>(gameObject.transform ,"img_bg/pl_times");
			m_pl_times_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_bg/pl_times");

			m_UI_Item_BuildUpGoTime0 = new UI_Item_BuildUpGoTime_SubView(FindUI<RectTransform>(gameObject.transform ,"img_bg/pl_times/UI_Item_BuildUpGoTime0"));
			m_UI_Item_BuildUpGoTime1 = new UI_Item_BuildUpGoTime_SubView(FindUI<RectTransform>(gameObject.transform ,"img_bg/pl_times/UI_Item_BuildUpGoTime1"));
			m_UI_Item_BuildUpGoTime2 = new UI_Item_BuildUpGoTime_SubView(FindUI<RectTransform>(gameObject.transform ,"img_bg/pl_times/UI_Item_BuildUpGoTime2"));
			m_UI_Item_BuildUpGoTime3 = new UI_Item_BuildUpGoTime_SubView(FindUI<RectTransform>(gameObject.transform ,"img_bg/pl_times/UI_Item_BuildUpGoTime3"));
			m_UI_BuildUp = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(gameObject.transform ,"img_bg/UI_BuildUp"));

			BindEvent();
        }

        #endregion
    }
}
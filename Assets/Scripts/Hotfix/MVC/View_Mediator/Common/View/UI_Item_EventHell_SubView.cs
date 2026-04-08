// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventHell_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventHell_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventHell";

        public UI_Item_EventHell_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_EventHell_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_box_PolygonImage;
		[HideInInspector] public GameButton m_btn_box_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_box_ArabLayoutCompment;

		[HideInInspector] public UI_Model_AnimationBox_SubView m_UI_Model_AnimationBox;
		[HideInInspector] public LanguageText m_lbl_name0_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name0_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_source0_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_source0_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_source1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_source1_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EventHell_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_btn_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/btn_box");
			m_btn_box_GameButton = FindUI<GameButton>(gameObject.transform ,"rect/btn_box");
			m_btn_box_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/btn_box");

			m_UI_Model_AnimationBox = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/btn_box/UI_Model_AnimationBox"));
			m_lbl_name0_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_name0");
			m_lbl_name0_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_name0");

			m_lbl_name1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_name1");
			m_lbl_name1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_name1");

			m_lbl_source0_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_source0");
			m_lbl_source0_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_source0");

			m_lbl_source1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_source1");
			m_lbl_source1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_source1");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ExpeditionFightTask_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ExpeditionFightTask_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ExpeditionFightTask";

        public UI_Item_ExpeditionFightTask_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_ExpeditionFightTask_GridLayoutGroup;

		[HideInInspector] public RectTransform m_pl_target1;
		[HideInInspector] public LanguageText m_lbl_target1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_target1_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_target2;
		[HideInInspector] public LanguageText m_lbl_target2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_target2_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_target3;
		[HideInInspector] public LanguageText m_lbl_target3_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_target3_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ExpeditionFightTask_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();

			m_pl_target1 = FindUI<RectTransform>(gameObject.transform ,"pl_target1");
			m_lbl_target1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_target1/lbl_target1");
			m_lbl_target1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_target1/lbl_target1");

			m_pl_target2 = FindUI<RectTransform>(gameObject.transform ,"pl_target2");
			m_lbl_target2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_target2/lbl_target2");
			m_lbl_target2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_target2/lbl_target2");

			m_pl_target3 = FindUI<RectTransform>(gameObject.transform ,"pl_target3");
			m_lbl_target3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_target3/lbl_target3");
			m_lbl_target3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_target3/lbl_target3");


			BindEvent();
        }

        #endregion
    }
}
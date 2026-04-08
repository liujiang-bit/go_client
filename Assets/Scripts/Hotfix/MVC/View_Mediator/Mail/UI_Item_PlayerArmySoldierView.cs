// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月16日
// Update Time         :    2020年3月16日
// Class Description   :    UI_Item_PlayerArmySoldierView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_PlayerArmySoldierView : GameView
    {
		public const string VIEW_NAME = "UI_Item_PlayerArmySoldier";

        public UI_Item_PlayerArmySoldierView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Model_ArmyDetailsColunm_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Model_ArmyDetailsColunm_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_col1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col4_LanguageText;

		[HideInInspector] public LanguageText m_lbl_col5_LanguageText;

		[HideInInspector] public UI_Model_ArmyTrainHead_SubView m_UI_Model_ArmyTrainHead;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Model_ArmyDetailsColunm_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"UI_Model_ArmyDetailsColunm");
			m_UI_Model_ArmyDetailsColunm_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"UI_Model_ArmyDetailsColunm");

			m_lbl_col1_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col1");

			m_lbl_col2_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col2");

			m_lbl_col3_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col3");

			m_lbl_col4_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col4");

			m_lbl_col5_LanguageText = FindUI<LanguageText>(vb.transform ,"UI_Model_ArmyDetailsColunm/lbl_col5");

			m_UI_Model_ArmyTrainHead = new UI_Model_ArmyTrainHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_ArmyTrainHead"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
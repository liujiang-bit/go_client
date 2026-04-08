// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_ResCost_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_ResCost_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_ResCost";

        public UI_Model_ResCost_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Model_ResCost_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Model_ResCost_ArabLayoutCompment;

		[HideInInspector] public UI_Model_ArmyTrainRes_SubView m_UI_Model_ArmyTrainRes_1;
		[HideInInspector] public UI_Model_ArmyTrainRes_SubView m_UI_Model_ArmyTrainRes_2;
		[HideInInspector] public UI_Model_ArmyTrainRes_SubView m_UI_Model_ArmyTrainRes_3;
		[HideInInspector] public UI_Model_ArmyTrainRes_SubView m_UI_Model_ArmyTrainRes_4;
		[HideInInspector] public UI_Model_ArmyTrainRes_SubView m_UI_Model_ArmyTrainRes_5;


        private void UIFinder()
        {       
			m_UI_Model_ResCost_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Model_ResCost_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Model_ArmyTrainRes_1 = new UI_Model_ArmyTrainRes_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_ArmyTrainRes_1"));
			m_UI_Model_ArmyTrainRes_2 = new UI_Model_ArmyTrainRes_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_ArmyTrainRes_2"));
			m_UI_Model_ArmyTrainRes_3 = new UI_Model_ArmyTrainRes_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_ArmyTrainRes_3"));
			m_UI_Model_ArmyTrainRes_4 = new UI_Model_ArmyTrainRes_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_ArmyTrainRes_4"));
			m_UI_Model_ArmyTrainRes_5 = new UI_Model_ArmyTrainRes_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_ArmyTrainRes_5"));

			BindEvent();
        }

        #endregion
    }
}
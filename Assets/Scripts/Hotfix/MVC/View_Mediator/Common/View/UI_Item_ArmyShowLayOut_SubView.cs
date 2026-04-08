// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyShowLayOut_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ArmyShowLayOut_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyShowLayOut";

        public UI_Item_ArmyShowLayOut_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_ArmyShowLayOut_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_ArmyShowLayOut_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ArmyShowLayOut_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Item_ArmyShowLayOut_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();


			BindEvent();
        }

        #endregion
    }
}
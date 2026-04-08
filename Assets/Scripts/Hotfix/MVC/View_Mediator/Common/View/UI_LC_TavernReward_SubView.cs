// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_LC_TavernReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_LC_TavernReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_LC_TavernReward";

        public UI_LC_TavernReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_LC_TavernReward_GridLayoutGroup;

		[HideInInspector] public UI_Item_PreviewReward_SubView m_UI_Item_PreviewReward1;
		[HideInInspector] public UI_Item_PreviewReward_SubView m_UI_Item_PreviewReward2;
		[HideInInspector] public UI_Item_PreviewReward_SubView m_UI_Item_PreviewReward3;


        private void UIFinder()
        {       
			m_UI_LC_TavernReward_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();

			m_UI_Item_PreviewReward1 = new UI_Item_PreviewReward_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_PreviewReward1"));
			m_UI_Item_PreviewReward2 = new UI_Item_PreviewReward_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_PreviewReward2"));
			m_UI_Item_PreviewReward3 = new UI_Item_PreviewReward_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_PreviewReward3"));

			BindEvent();
        }

        #endregion
    }
}
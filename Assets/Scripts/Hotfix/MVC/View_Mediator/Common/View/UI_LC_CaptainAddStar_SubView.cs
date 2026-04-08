// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_LC_CaptainAddStar_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_LC_CaptainAddStar_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_LC_CaptainAddStar";

        public UI_LC_CaptainAddStar_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_LC_CaptainAddStar_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_LC_CaptainAddStar_ArabLayoutCompment;
		[HideInInspector] public ViewBinder m_UI_LC_CaptainAddStar_ViewBinder;

		[HideInInspector] public UI_Item_CaptainAddStarItem_SubView m_UI_Item_CaptainAddStarItem_1;
		[HideInInspector] public UI_Item_CaptainAddStarItem_SubView m_UI_Item_CaptainAddStarItem_2;
		[HideInInspector] public UI_Item_CaptainAddStarItem_SubView m_UI_Item_CaptainAddStarItem_3;


        private void UIFinder()
        {       
			m_UI_LC_CaptainAddStar_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_LC_CaptainAddStar_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();
			m_UI_LC_CaptainAddStar_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_CaptainAddStarItem_1 = new UI_Item_CaptainAddStarItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainAddStarItem_1"));
			m_UI_Item_CaptainAddStarItem_2 = new UI_Item_CaptainAddStarItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainAddStarItem_2"));
			m_UI_Item_CaptainAddStarItem_3 = new UI_Item_CaptainAddStarItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainAddStarItem_3"));

			BindEvent();
        }

        #endregion
    }
}
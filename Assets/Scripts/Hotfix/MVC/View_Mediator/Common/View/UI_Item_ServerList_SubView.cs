// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ServerList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_ServerList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ServerList";

        public UI_Item_ServerList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_ServerList_GridLayoutGroup;
		[HideInInspector] public ViewBinder m_UI_Item_ServerList_ViewBinder;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_ServerList_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ServerItem_SubView m_UI_Item_ServerItem1;
		[HideInInspector] public UI_Item_ServerItem_SubView m_UI_Item_ServerItem2;


        private void UIFinder()
        {       
			m_UI_Item_ServerList_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Item_ServerList_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Item_ServerList_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Item_ServerItem1 = new UI_Item_ServerItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ServerItem1"));
			m_UI_Item_ServerItem2 = new UI_Item_ServerItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ServerItem2"));

			BindEvent();
        }

        #endregion
    }
}
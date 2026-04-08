// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_VipStoreList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_VipStoreList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_VipStoreList";

        public UI_Item_VipStoreList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_VipStoreList_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_VipStoreList_ArabLayoutCompment;

		[HideInInspector] public UI_Item_VipStoreItem_SubView m_UI_Item_VipStoreItem0;
		[HideInInspector] public UI_Item_VipStoreItem_SubView m_UI_Item_VipStoreItem1;


        private void UIFinder()
        {       
			m_UI_Item_VipStoreList_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Item_VipStoreList_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Item_VipStoreItem0 = new UI_Item_VipStoreItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_VipStoreItem0"));
			m_UI_Item_VipStoreItem1 = new UI_Item_VipStoreItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_VipStoreItem1"));

			BindEvent();
        }

        #endregion
    }
}
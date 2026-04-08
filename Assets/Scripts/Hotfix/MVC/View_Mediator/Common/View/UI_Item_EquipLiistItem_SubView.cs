// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipLiistItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EquipLiistItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipLiistItem";

        public UI_Item_EquipLiistItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_EquipLiistItem_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_UI_Item_EquipLiistItem_GridLayoutGroup;

		[HideInInspector] public UI_Item_EquipItem_SubView m_UI_Item_EquipItem0;
		[HideInInspector] public UI_Item_EquipItem_SubView m_UI_Item_EquipItem1;


        private void UIFinder()
        {       
			m_UI_Item_EquipLiistItem_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();
			m_UI_Item_EquipLiistItem_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();

			m_UI_Item_EquipItem0 = new UI_Item_EquipItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_EquipItem0"));
			m_UI_Item_EquipItem1 = new UI_Item_EquipItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_EquipItem1"));

			BindEvent();
        }

        #endregion
    }
}
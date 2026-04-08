// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeGemShopList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeGemShopList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeGemShopList";

        public UI_Item_ChargeGemShopList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_Item_ChargeGemShopList_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_ChargeGemShopList_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ChargeGemShopItem_SubView m_UI_Item_ChargeTypeDiamondItem0;
		[HideInInspector] public UI_Item_ChargeGemShopItem_SubView m_UI_Item_ChargeTypeDiamondItem1;
		[HideInInspector] public UI_Item_ChargeGemShopItem_SubView m_UI_Item_ChargeTypeDiamondItem2;


        private void UIFinder()
        {       
			m_UI_Item_ChargeGemShopList_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_Item_ChargeGemShopList_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Item_ChargeTypeDiamondItem0 = new UI_Item_ChargeGemShopItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ChargeTypeDiamondItem0"));
			m_UI_Item_ChargeTypeDiamondItem1 = new UI_Item_ChargeGemShopItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ChargeTypeDiamondItem1"));
			m_UI_Item_ChargeTypeDiamondItem2 = new UI_Item_ChargeGemShopItem_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_ChargeTypeDiamondItem2"));

			BindEvent();
        }

        #endregion
    }
}
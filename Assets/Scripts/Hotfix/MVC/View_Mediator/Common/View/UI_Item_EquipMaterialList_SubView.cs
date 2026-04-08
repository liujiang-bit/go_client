// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipMaterialList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EquipMaterialList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipMaterialList";

        public UI_Item_EquipMaterialList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EquipMaterialList;
		[HideInInspector] public RectTransform m_pl_view;
		[HideInInspector] public UI_Item_ItemEffect_SubView m_UI_Item_ItemEffect;
		[HideInInspector] public LanguageText m_lbl_num_LanguageText;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;


        private void UIFinder()
        {       
			m_UI_Item_EquipMaterialList = gameObject.GetComponent<RectTransform>();
			m_pl_view = FindUI<RectTransform>(gameObject.transform ,"pl_view");
			m_UI_Item_ItemEffect = new UI_Item_ItemEffect_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/UI_Item_ItemEffect"));
			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_view/lbl_num");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_view/UI_Model_Item"));

			BindEvent();
        }

        #endregion
    }
}
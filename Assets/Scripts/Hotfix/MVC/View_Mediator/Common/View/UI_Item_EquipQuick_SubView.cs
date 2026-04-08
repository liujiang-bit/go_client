// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipQuick_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EquipQuick_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipQuick";

        public UI_Item_EquipQuick_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_EquipQuick_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_target_ArabLayoutCompment;

		[HideInInspector] public UI_Item_ItemEffect_SubView m_UI_Item_ItemEffect;
		[HideInInspector] public UI_Model_Item_SubView m_item_target;
		[HideInInspector] public LanguageText m_lbl_Num_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_base_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_base_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;


        private void UIFinder()
        {       
			m_UI_Item_EquipQuick_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_pl_target_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_target");

			m_UI_Item_ItemEffect = new UI_Item_ItemEffect_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_target/UI_Item_ItemEffect"));
			m_item_target = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_target/item_target"));
			m_lbl_Num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_target/lbl_Num");

			m_pl_base_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_base");
			m_pl_base_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_base");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_base/UI_Model_Item"));

			BindEvent();
        }

        #endregion
    }
}
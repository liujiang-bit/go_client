// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventType1Item_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventType1Item_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventType1Item";

        public UI_Item_EventType1Item_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventType1Item;
		[HideInInspector] public ArabLayoutCompment m_pl_layout_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_layout_GridLayoutGroup;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_ItemAfter2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_ItemAfter1;
		[HideInInspector] public ArabLayoutCompment m_pl_arrow_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_ItemBefore2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_ItemBefore1;
		[HideInInspector] public LanguageText m_lbl_times_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_times_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_exchange;


        private void UIFinder()
        {       
			m_UI_Item_EventType1Item = gameObject.GetComponent<RectTransform>();
			m_pl_layout_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_layout");
			m_pl_layout_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_layout");

			m_UI_ItemAfter2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_layout/UI_ItemAfter2"));
			m_UI_ItemAfter1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_layout/UI_ItemAfter1"));
			m_pl_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_layout/pl_arrow");

			m_UI_ItemBefore2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_layout/UI_ItemBefore2"));
			m_UI_ItemBefore1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_layout/UI_ItemBefore1"));
			m_lbl_times_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_times");
			m_lbl_times_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_times");

			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_title");
			m_lbl_title_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_title");

			m_btn_exchange = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_exchange"));

			BindEvent();
        }

        #endregion
    }
}
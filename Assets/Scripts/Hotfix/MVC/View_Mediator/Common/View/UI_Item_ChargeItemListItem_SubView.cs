// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeItemListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeItemListItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeItemListItem";

        public UI_Item_ChargeItemListItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeItemListItem;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet;


        private void UIFinder()
        {       
			m_UI_Item_ChargeItemListItem = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_num");

			m_UI_Model_RewardGet = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_RewardGet"));

			BindEvent();
        }

        #endregion
    }
}
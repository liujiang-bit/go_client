// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_VipStoreItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_VipStoreItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_VipStoreItem";

        public UI_Item_VipStoreItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_VipStoreItem;
		[HideInInspector] public UI_Model_Item_SubView m_pl_item;
		[HideInInspector] public PolygonImage m_img_cutOff_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_cutOff_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_cutOff_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_cutOff_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_cutOff_Shadow;

		[HideInInspector] public LanguageText m_lbl_itemName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemName_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_buy_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemCount_ArabLayoutCompment;

		[HideInInspector] public UI_Model_HorizontalButton_MiniYellow_SubView m_btn_buy;
		[HideInInspector] public LanguageText m_lbl_vipLimit_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_vipLimit_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_VipStoreItem = gameObject.GetComponent<RectTransform>();
			m_pl_item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"bg/pl_item"));
			m_img_cutOff_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bg/img_cutOff");
			m_img_cutOff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/img_cutOff");

			m_lbl_cutOff_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_cutOff");
			m_lbl_cutOff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_cutOff");
			m_lbl_cutOff_Shadow = FindUI<Shadow>(gameObject.transform ,"bg/lbl_cutOff");

			m_lbl_itemName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_itemName");
			m_lbl_itemName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_itemName");

			m_pl_buy_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/pl_buy");

			m_lbl_itemCount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/pl_buy/lbl_itemCount");
			m_lbl_itemCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/pl_buy/lbl_itemCount");

			m_btn_buy = new UI_Model_HorizontalButton_MiniYellow_SubView(FindUI<RectTransform>(gameObject.transform ,"bg/pl_buy/btn_buy"));
			m_lbl_vipLimit_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_vipLimit");
			m_lbl_vipLimit_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_vipLimit");


			BindEvent();
        }

        #endregion
    }
}
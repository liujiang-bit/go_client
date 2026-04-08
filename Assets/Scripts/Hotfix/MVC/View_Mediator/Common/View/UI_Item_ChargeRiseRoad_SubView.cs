// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeRiseRoad_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeRiseRoad_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeRiseRoad";

        public UI_Item_ChargeRiseRoad_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeRiseRoad;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_char_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_spin_char_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_spin_char_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_mes1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_mes2_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_item_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_item_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item1;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item4;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item5;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_btn_get;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_btn_charge;


        private void UIFinder()
        {       
			m_UI_Item_ChargeRiseRoad = gameObject.GetComponent<RectTransform>();
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"content/img_bg");

			m_pl_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"content/pl_char");

			m_spin_char_SkeletonGraphic = FindUI<SkeletonGraphic>(gameObject.transform ,"content/pl_char/spin_char");
			m_spin_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"content/pl_char/spin_char");

			m_lbl_mes1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"content/lbl_mes1");

			m_lbl_mes2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"content/lbl_mes2");

			m_pl_item_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"content/pl_item");
			m_pl_item_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"content/pl_item");

			m_UI_Model_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"content/pl_item/UI_Model_Item1"));
			m_UI_Model_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"content/pl_item/UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"content/pl_item/UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"content/pl_item/UI_Model_Item4"));
			m_UI_Model_Item5 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"content/pl_item/UI_Model_Item5"));
			m_btn_get = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(gameObject.transform ,"content/btn_get"));
			m_btn_charge = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(gameObject.transform ,"content/btn_charge"));

			BindEvent();
        }

        #endregion
    }
}
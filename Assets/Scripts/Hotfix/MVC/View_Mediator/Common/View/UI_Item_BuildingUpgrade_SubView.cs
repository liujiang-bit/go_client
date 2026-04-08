// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BuildingUpgrade_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_BuildingUpgrade_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BuildingUpgrade";

        public UI_Item_BuildingUpgrade_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BuildingUpgrade;
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_languageText_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_resBg_PolygonImage;
		[HideInInspector] public GridLayoutGroup m_img_resBg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_img_resBg_ArabLayoutCompment;

		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesConsume;
		[HideInInspector] public HorizontalLayoutGroup m_pl_btns_HorizontalLayoutGroup;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue_long_SubView m_UI_Model_DoubleLineButton_Blue;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_long_SubView m_UI_Model_DoubleLineButton_Yellow;
		[HideInInspector] public HorizontalLayoutGroup m_pl_limit_HorizontalLayoutGroup;
		[HideInInspector] public PolygonImage m_pl_limit_PolygonImage;

		[HideInInspector] public UI_Item_BuildingUpLimit_SubView m_UI_Item_BuildingUpLimit;


        private void UIFinder()
        {       
			m_UI_Item_BuildingUpgrade = gameObject.GetComponent<RectTransform>();
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_languageText");
			m_lbl_languageText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_languageText");

			m_img_resBg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_resBg");
			m_img_resBg_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"img_resBg");
			m_img_resBg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_resBg");

			m_UI_Model_ResourcesConsume = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(gameObject.transform ,"img_resBg/UI_Model_ResourcesConsume"));
			m_pl_btns_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_btns");

			m_UI_Model_DoubleLineButton_Blue = new UI_Model_DoubleLineButton_Blue_long_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_btns/UI_Model_DoubleLineButton_Blue"));
			m_UI_Model_DoubleLineButton_Yellow = new UI_Model_DoubleLineButton_Yellow_long_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_btns/UI_Model_DoubleLineButton_Yellow"));
			m_pl_limit_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_limit");
			m_pl_limit_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_limit");

			m_UI_Item_BuildingUpLimit = new UI_Item_BuildingUpLimit_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_limit/UI_Item_BuildingUpLimit"));

			BindEvent();
        }

        #endregion
    }
}
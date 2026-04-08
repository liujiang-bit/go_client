// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TrainSoldierDetail_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_TrainSoldierDetail_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TrainSoldierDetail";

        public UI_Item_TrainSoldierDetail_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_TrainSoldierDetail_ViewBinder;
		[HideInInspector] public VerticalLayoutGroup m_UI_Item_TrainSoldierDetail_VerticalLayoutGroup;

		[HideInInspector] public LayoutElement m_pl_desc_LayoutElement;

		[HideInInspector] public PolygonImage m_img_army_type_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_desc_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_content_LanguageText;

		[HideInInspector] public UI_Model_TrainSoldierAttr_SubView m_UI_Model_TrainSoldierAttr_1;
		[HideInInspector] public UI_Model_TrainSoldierAttr_SubView m_UI_Model_TrainSoldierAttr_2;
		[HideInInspector] public UI_Model_TrainSoldierAttr_SubView m_UI_Model_TrainSoldierAttr_3;
		[HideInInspector] public UI_Model_TrainSoldierAttr_SubView m_UI_Model_TrainSoldierAttr_4;
		[HideInInspector] public UI_Model_TrainSoldierAttr_SubView m_UI_Model_TrainSoldierAttr_5;
		[HideInInspector] public UI_Model_TrainSoldierAttr_SubView m_UI_Model_TrainSoldierAttr_6;
		[HideInInspector] public UI_Model_TrainSoldierAttr_SubView m_UI_Model_TrainSoldierAttr_7;
		[HideInInspector] public LayoutElement m_pl_btn_pos_LayoutElement;
		[HideInInspector] public GridLayoutGroup m_pl_btn_pos_GridLayoutGroup;

		[HideInInspector] public UI_Model_TrainSoldierSkill_SubView m_UI_Model_TrainSoldierSkill_3;
		[HideInInspector] public UI_Model_TrainSoldierSkill_SubView m_UI_Model_TrainSoldierSkill_2;
		[HideInInspector] public UI_Model_TrainSoldierSkill_SubView m_UI_Model_TrainSoldierSkill_1;


        private void UIFinder()
        {       
			m_UI_Item_TrainSoldierDetail_ViewBinder = gameObject.GetComponent<ViewBinder>();
			m_UI_Item_TrainSoldierDetail_VerticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();

			m_pl_desc_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_desc");

			m_img_army_type_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_desc/img_army_type_icon");

			m_lbl_desc_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_desc/lbl_desc_name");

			m_lbl_desc_content_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_desc/lbl_desc_content");

			m_UI_Model_TrainSoldierAttr_1 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_TrainSoldierAttr_1"));
			m_UI_Model_TrainSoldierAttr_2 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_TrainSoldierAttr_2"));
			m_UI_Model_TrainSoldierAttr_3 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_TrainSoldierAttr_3"));
			m_UI_Model_TrainSoldierAttr_4 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_TrainSoldierAttr_4"));
			m_UI_Model_TrainSoldierAttr_5 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_TrainSoldierAttr_5"));
			m_UI_Model_TrainSoldierAttr_6 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_TrainSoldierAttr_6"));
			m_UI_Model_TrainSoldierAttr_7 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_TrainSoldierAttr_7"));
			m_pl_btn_pos_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_btn_pos");
			m_pl_btn_pos_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_btn_pos");

			m_UI_Model_TrainSoldierSkill_3 = new UI_Model_TrainSoldierSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_btn_pos/UI_Model_TrainSoldierSkill_3"));
			m_UI_Model_TrainSoldierSkill_2 = new UI_Model_TrainSoldierSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_btn_pos/UI_Model_TrainSoldierSkill_2"));
			m_UI_Model_TrainSoldierSkill_1 = new UI_Model_TrainSoldierSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_btn_pos/UI_Model_TrainSoldierSkill_1"));

			BindEvent();
        }

        #endregion
    }
}
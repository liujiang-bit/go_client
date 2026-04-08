// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月18日
// Update Time         :    2020年3月18日
// Class Description   :    ItemTrainSoldierDetailView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemTrainSoldierDetailView : GameView
    {
		public const string VIEW_NAME = "UI_Item_TrainSoldierDetail";

        public ItemTrainSoldierDetailView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LayoutElement m_pl_desc_LayoutElement;

		[HideInInspector] public PolygonImage m_img_army_type_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_army_type_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_name_ArabLayoutCompment;

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
		[HideInInspector] public ArabLayoutCompment m_pl_btn_pos_ArabLayoutCompment;

		[HideInInspector] public UI_Model_TrainSoldierSkill_SubView m_UI_Model_TrainSoldierSkill_3;
		[HideInInspector] public UI_Model_TrainSoldierSkill_SubView m_UI_Model_TrainSoldierSkill_2;
		[HideInInspector] public UI_Model_TrainSoldierSkill_SubView m_UI_Model_TrainSoldierSkill_1;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_desc_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_desc");

			m_img_army_type_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_desc/img_army_type_icon");
			m_img_army_type_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_desc/img_army_type_icon");

			m_lbl_desc_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_desc/lbl_desc_name");
			m_lbl_desc_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_desc/lbl_desc_name");

			m_lbl_desc_content_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_desc/lbl_desc_content");

			m_UI_Model_TrainSoldierAttr_1 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_TrainSoldierAttr_1"));
			m_UI_Model_TrainSoldierAttr_2 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_TrainSoldierAttr_2"));
			m_UI_Model_TrainSoldierAttr_3 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_TrainSoldierAttr_3"));
			m_UI_Model_TrainSoldierAttr_4 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_TrainSoldierAttr_4"));
			m_UI_Model_TrainSoldierAttr_5 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_TrainSoldierAttr_5"));
			m_UI_Model_TrainSoldierAttr_6 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_TrainSoldierAttr_6"));
			m_UI_Model_TrainSoldierAttr_7 = new UI_Model_TrainSoldierAttr_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_TrainSoldierAttr_7"));
			m_pl_btn_pos_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_btn_pos");
			m_pl_btn_pos_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_btn_pos");
			m_pl_btn_pos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_btn_pos");

			m_UI_Model_TrainSoldierSkill_3 = new UI_Model_TrainSoldierSkill_SubView(FindUI<RectTransform>(vb.transform ,"pl_btn_pos/UI_Model_TrainSoldierSkill_3"));
			m_UI_Model_TrainSoldierSkill_2 = new UI_Model_TrainSoldierSkill_SubView(FindUI<RectTransform>(vb.transform ,"pl_btn_pos/UI_Model_TrainSoldierSkill_2"));
			m_UI_Model_TrainSoldierSkill_1 = new UI_Model_TrainSoldierSkill_SubView(FindUI<RectTransform>(vb.transform ,"pl_btn_pos/UI_Model_TrainSoldierSkill_1"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月27日
// Update Time         :    2020年7月27日
// Class Description   :    UI_Item_StandardUseItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_StandardUseItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_StandardUseItem";

        public UI_Item_StandardUseItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_itemName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemDesc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemDesc_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemCount_ArabLayoutCompment;

		[HideInInspector] public UI_Item_Bag_SubView m_UI_Item_Bag;
		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_UI_Model_Yellow;
		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_UI_Model_Blue_big;
		[HideInInspector] public PolygonImage m_img_Shelter_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_Shelter_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_quick_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideL_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_StandardButton_MiniBlue;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_itemName_LanguageText = FindUI<LanguageText>(vb.transform ,"content/lbl_itemName");
			m_lbl_itemName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"content/lbl_itemName");

			m_lbl_itemDesc_LanguageText = FindUI<LanguageText>(vb.transform ,"content/lbl_itemDesc");
			m_lbl_itemDesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"content/lbl_itemDesc");

			m_lbl_itemCount_LanguageText = FindUI<LanguageText>(vb.transform ,"content/lbl_itemCount");
			m_lbl_itemCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"content/lbl_itemCount");

			m_UI_Item_Bag = new UI_Item_Bag_SubView(FindUI<RectTransform>(vb.transform ,"content/icon/UI_Item_Bag"));
			m_UI_Model_Yellow = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"btn/UI_Model_Yellow"));
			m_UI_Model_Blue_big = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"btn/UI_Model_Blue_big"));
			m_img_Shelter_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_Shelter");
			m_img_Shelter_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_Shelter");

			m_pl_quick_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_quick");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_quick/quickbg/img_arrowSideL");
			m_img_arrowSideL_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_quick/quickbg/img_arrowSideL");

			m_UI_Model_StandardButton_MiniBlue = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_quick/UI_Model_StandardButton_MiniBlue"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
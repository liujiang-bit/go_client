// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月9日
// Update Time         :    2020年4月9日
// Class Description   :    UI_Item_QuestView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_QuestView : GameView
    {
		public const string VIEW_NAME = "UI_Item_Quest";

        public UI_Item_QuestView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_blue;
		[HideInInspector] public UI_Model_StandardButton_MiniGreen_SubView m_UI_Model_green;
		[HideInInspector] public LanguageText m_lbl_itemName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_itemDesc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_itemDesc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_iconbg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_iconbg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_rewards_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_rewards_GridLayoutGroup;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Model_blue = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"btn/UI_Model_blue"));
			m_UI_Model_green = new UI_Model_StandardButton_MiniGreen_SubView(FindUI<RectTransform>(vb.transform ,"btn/UI_Model_green"));
			m_lbl_itemName_LanguageText = FindUI<LanguageText>(vb.transform ,"content/lbl_itemName");
			m_lbl_itemName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"content/lbl_itemName");

			m_lbl_itemDesc_LanguageText = FindUI<LanguageText>(vb.transform ,"content/lbl_itemDesc");
			m_lbl_itemDesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"content/lbl_itemDesc");

			m_img_iconbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"content/img_iconbg");
			m_img_iconbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"content/img_iconbg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"content/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"content/img_icon");

			m_pl_rewards_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"content/reward/pl_rewards");
			m_pl_rewards_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"content/reward/pl_rewards");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
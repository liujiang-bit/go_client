// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    ItemQuickUseResourcesView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemQuickUseResourcesView : GameView
    {
		public const string VIEW_NAME = "UI_Item_QuickUseResources";

        public ItemQuickUseResourcesView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public UI_Item_Bag_SubView m_UI_Item_Bag;
		[HideInInspector] public LanguageText m_lbl_itemname_LanguageText;

		[HideInInspector] public LanguageText m_lbl_itemdes_LanguageText;

		[HideInInspector] public LanguageText m_lbl_itemcount_LanguageText;

		[HideInInspector] public PolygonImage m_btn_use_PolygonImage;
		[HideInInspector] public GameButton m_btn_use_GameButton;
		[HideInInspector] public BtnAnimation m_btn_use_ButtonAnimation;

		[HideInInspector] public LanguageText m_lbl_use_LanguageText;

		[HideInInspector] public PolygonImage m_btn_buy_PolygonImage;
		[HideInInspector] public GameButton m_btn_buy_GameButton;
		[HideInInspector] public BtnAnimation m_btn_buy_ButtonAnimation;

		[HideInInspector] public LanguageText m_lbl_buy_LanguageText;

		[HideInInspector] public LanguageText m_lbl_buycost_LanguageText;

		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public PolygonImage m_img_tip_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_enough_PolygonImage;
		[HideInInspector] public GameButton m_btn_enough_GameButton;
		[HideInInspector] public BtnAnimation m_btn_enough_ButtonAnimation;

		[HideInInspector] public LanguageText m_lbl_enough_LanguageText;

		[HideInInspector] public PolygonImage m_btn_max_PolygonImage;
		[HideInInspector] public GameButton m_btn_max_GameButton;
		[HideInInspector] public BtnAnimation m_btn_max_ButtonAnimation;

		[HideInInspector] public LanguageText m_lbl_max_LanguageText;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_UI_Item_Bag = new UI_Item_Bag_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/UI_Item_Bag"));
			m_lbl_itemname_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_itemname");

			m_lbl_itemdes_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_itemdes");

			m_lbl_itemcount_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_itemcount");

			m_btn_use_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_use");
			m_btn_use_GameButton = FindUI<GameButton>(vb.transform ,"btn_use");
			m_btn_use_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_use");

			m_lbl_use_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_use/lbl_use");

			m_btn_buy_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_buy");
			m_btn_buy_GameButton = FindUI<GameButton>(vb.transform ,"btn_buy");
			m_btn_buy_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_buy");

			m_lbl_buy_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_buy/lbl_buy");

			m_lbl_buycost_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_buy/lbl_buycost");

			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_buy/img_polygonImage");

			m_img_tip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_tip");

			m_btn_enough_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_tip/btn_enough");
			m_btn_enough_GameButton = FindUI<GameButton>(vb.transform ,"img_tip/btn_enough");
			m_btn_enough_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"img_tip/btn_enough");

			m_lbl_enough_LanguageText = FindUI<LanguageText>(vb.transform ,"img_tip/btn_enough/lbl_enough");

			m_btn_max_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_tip/btn_max");
			m_btn_max_GameButton = FindUI<GameButton>(vb.transform ,"img_tip/btn_max");
			m_btn_max_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"img_tip/btn_max");

			m_lbl_max_LanguageText = FindUI<LanguageText>(vb.transform ,"img_tip/btn_max/lbl_max");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
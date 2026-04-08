// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月18日
// Update Time         :    2020年3月18日
// Class Description   :    ItemBuildView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemBuildView : GameView
    {
		public const string VIEW_NAME = "UI_Item_BuildCityBuilding";

        public ItemBuildView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public Animator m_pl_rect_Animator;

		[HideInInspector] public PolygonImage m_img_bgLight_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public RectTransform m_pl_icon;
		[HideInInspector] public PolygonImage m_pl_lock_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_lock_LanguageText;

		[HideInInspector] public RectTransform m_pl_claim;
		[HideInInspector] public GridLayoutGroup m_pl_cost_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_cost_ArabLayoutCompment;

		[HideInInspector] public UI_Model_ResourcesConsumeInBCB_SubView m_UI_Model_ResourcesConsume;
		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_rect_Animator = FindUI<Animator>(vb.transform ,"pl_rect");

			m_img_bgLight_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/bg/img_bgLight");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/textIcon/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/textIcon/lbl_desc");

			m_pl_icon = FindUI<RectTransform>(vb.transform ,"pl_rect/textIcon/pl_icon");
			m_pl_lock_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/pl_lock");

			m_lbl_lock_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_lock/lbl_lock");

			m_pl_claim = FindUI<RectTransform>(vb.transform ,"pl_rect/pl_claim");
			m_pl_cost_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_rect/pl_claim/pl_cost");
			m_pl_cost_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_rect/pl_claim/pl_cost");

			m_UI_Model_ResourcesConsume = new UI_Model_ResourcesConsumeInBCB_SubView(FindUI<RectTransform>(vb.transform ,"pl_rect/pl_claim/pl_cost/UI_Model_ResourcesConsume"));
			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_claim/time/lbl_count");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_rect/pl_claim/time/lbl_time");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_rect/btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(vb.transform ,"pl_rect/btn_btn");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BuildCityBuilding_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_BuildCityBuilding_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BuildCityBuilding";

        public UI_Item_BuildCityBuilding_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_BuildCityBuilding_ViewBinder;

		[HideInInspector] public Animator m_pl_rect_Animator;

		[HideInInspector] public PolygonImage m_img_bgLight_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_lock_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_lock_LanguageText;

		[HideInInspector] public RectTransform m_pl_claim;
		[HideInInspector] public GridLayoutGroup m_pl_cost_GridLayoutGroup;

		[HideInInspector] public UI_Model_ResourcesConsumeInBCB_SubView m_UI_Model_ResourcesConsume;
		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_BuildCityBuilding_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_rect_Animator = FindUI<Animator>(gameObject.transform ,"pl_rect");

			m_img_bgLight_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/bg/img_bgLight");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/textIcon/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/textIcon/lbl_desc");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/textIcon/img_icon");

			m_pl_lock_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/pl_lock");

			m_lbl_lock_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/pl_lock/lbl_lock");

			m_pl_claim = FindUI<RectTransform>(gameObject.transform ,"pl_rect/pl_claim");
			m_pl_cost_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_rect/pl_claim/pl_cost");

			m_UI_Model_ResourcesConsume = new UI_Model_ResourcesConsumeInBCB_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_rect/pl_claim/pl_cost/UI_Model_ResourcesConsume"));
			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/pl_claim/time/lbl_count");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_rect/pl_claim/time/lbl_time");

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_rect/btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_rect/btn_btn");


			BindEvent();
        }

        #endregion
    }
}
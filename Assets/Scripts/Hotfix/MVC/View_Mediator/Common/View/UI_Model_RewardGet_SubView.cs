// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_RewardGet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_RewardGet_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_RewardGet";

        public UI_Model_RewardGet_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Model_RewardGet_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_animButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_animButton_GameButton;
		[HideInInspector] public BtnAnimation m_btn_animButton_BtnAnimation;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public ViewBinder m_pl_cur_ViewBinder;

		[HideInInspector] public PolygonImage m_img_curicon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_curcount_LanguageText;
		[HideInInspector] public Shadow m_lbl_curcount_Shadow;

		[HideInInspector] public ViewBinder m_pl_package_ViewBinder;

		[HideInInspector] public PolygonImage m_img_packageicon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_packagecount_LanguageText;
		[HideInInspector] public Shadow m_lbl_packagecount_Shadow;

		[HideInInspector] public ViewBinder m_pl_soldier_ViewBinder;

		[HideInInspector] public PolygonImage m_img_soldiericon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_soldiercount_LanguageText;
		[HideInInspector] public Shadow m_lbl_soldiercount_Shadow;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_RewardGet_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_animButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton");
			m_btn_animButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_animButton");
			m_btn_animButton_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_animButton");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_animButton/UI_Model_Item"));
			m_pl_cur_ViewBinder = FindUI<ViewBinder>(gameObject.transform ,"btn_animButton/pl_cur");

			m_img_curicon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/pl_cur/img_curicon");

			m_lbl_curcount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_animButton/pl_cur/lbl_curcount");
			m_lbl_curcount_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_animButton/pl_cur/lbl_curcount");

			m_pl_package_ViewBinder = FindUI<ViewBinder>(gameObject.transform ,"btn_animButton/pl_package");

			m_img_packageicon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/pl_package/img_packageicon");

			m_lbl_packagecount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_animButton/pl_package/lbl_packagecount");
			m_lbl_packagecount_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_animButton/pl_package/lbl_packagecount");

			m_pl_soldier_ViewBinder = FindUI<ViewBinder>(gameObject.transform ,"btn_animButton/pl_soldier");

			m_img_soldiericon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_animButton/pl_soldier/img_soldiericon");

			m_lbl_soldiercount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_animButton/pl_soldier/lbl_soldiercount");
			m_lbl_soldiercount_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_animButton/pl_soldier/lbl_soldiercount");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_animButton/lbl_name");


			BindEvent();
        }

        #endregion
    }
}
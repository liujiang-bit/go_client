// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_LC_RewardGet_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_LC_RewardGet_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_LC_RewardGet";

        public UI_LC_RewardGet_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_UI_LC_RewardGet_GridLayoutGroup;
		[HideInInspector] public ViewBinder m_UI_LC_RewardGet_ViewBinder;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item1;
		[HideInInspector] public RectTransform m_pl_cur;
		[HideInInspector] public PolygonImage m_img_iconcur_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_countcur_LanguageText;
		[HideInInspector] public Shadow m_lbl_countcur_Shadow;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public Shadow m_lbl_name_Shadow;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item2;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item3;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item4;


        private void UIFinder()
        {       
			m_UI_LC_RewardGet_GridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
			m_UI_LC_RewardGet_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Model_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item1"));
			m_pl_cur = FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item1/pl_cur");
			m_img_iconcur_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"UI_Model_Item1/pl_cur/img_iconcur");

			m_lbl_countcur_LanguageText = FindUI<LanguageText>(gameObject.transform ,"UI_Model_Item1/pl_cur/lbl_countcur");
			m_lbl_countcur_Shadow = FindUI<Shadow>(gameObject.transform ,"UI_Model_Item1/pl_cur/lbl_countcur");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"UI_Model_Item1/lbl_name");
			m_lbl_name_Shadow = FindUI<Shadow>(gameObject.transform ,"UI_Model_Item1/lbl_name");

			m_UI_Model_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item2"));
			m_UI_Model_Item3 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item3"));
			m_UI_Model_Item4 = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item4"));

			BindEvent();
        }

        #endregion
    }
}
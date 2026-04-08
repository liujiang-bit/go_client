// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WarnDetail_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WarnDetail_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WarnDetail";

        public UI_Item_WarnDetail_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WarnDetail;
		[HideInInspector] public ArabLayoutCompment m_pl_captain_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_CapHeadSub;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_CapHeadMain;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_number_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_number_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_soldier_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_soldier_ArabLayoutCompment;

		[HideInInspector] public UI_Item_SoldierHead_SubView m_UI_Item_SoldierHead;
		[HideInInspector] public RectTransform m_pl_res;
		[HideInInspector] public PolygonImage m_img_woker_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_woker_MakeChildrenGray;
		[HideInInspector] public ArabLayoutCompment m_img_woker_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_woker_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_woker_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_res_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_res_num_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_reslist_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reslist_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_Resources1;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_Resources2;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_Resources3;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_Resources4;


        private void UIFinder()
        {       
			m_UI_Item_WarnDetail = gameObject.GetComponent<RectTransform>();
			m_pl_captain_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_captain");

			m_UI_CapHeadSub = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_captain/head/UI_CapHeadSub"));
			m_UI_CapHeadMain = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_captain/head/UI_CapHeadMain"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_captain/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_captain/lbl_name");

			m_lbl_number_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_captain/lbl_number");
			m_lbl_number_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_captain/lbl_number");

			m_pl_soldier_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_soldier");
			m_pl_soldier_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_soldier");

			m_UI_Item_SoldierHead = new UI_Item_SoldierHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_soldier/UI_Item_SoldierHead"));
			m_pl_res = FindUI<RectTransform>(gameObject.transform ,"pl_res");
			m_img_woker_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_res/reswoker/img_woker");
			m_img_woker_MakeChildrenGray = FindUI<GrayChildrens>(gameObject.transform ,"pl_res/reswoker/img_woker");
			m_img_woker_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_res/reswoker/img_woker");

			m_img_char_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_res/reswoker/img_woker/img_char");

			m_lbl_woker_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_res/reswoker/lbl_woker_name");
			m_lbl_woker_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_res/reswoker/lbl_woker_name");

			m_lbl_res_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_res/reswoker/lbl_res_num");
			m_lbl_res_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_res/reswoker/lbl_res_num");

			m_pl_reslist_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_res/pl_reslist");
			m_pl_reslist_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_res/pl_reslist");

			m_UI_Model_Resources1 = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_res/pl_reslist/UI_Model_Resources1"));
			m_UI_Model_Resources2 = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_res/pl_reslist/UI_Model_Resources2"));
			m_UI_Model_Resources3 = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_res/pl_reslist/UI_Model_Resources3"));
			m_UI_Model_Resources4 = new UI_Model_Resources_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_res/pl_reslist/UI_Model_Resources4"));

			BindEvent();
        }

        #endregion
    }
}
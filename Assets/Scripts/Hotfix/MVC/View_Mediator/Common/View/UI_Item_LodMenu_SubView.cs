// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_LodMenu_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_LodMenu_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_LodMenu";

        public UI_Item_LodMenu_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_LodMenu_Animator;
		[HideInInspector] public CanvasGroup m_UI_Item_LodMenu_CanvasGroup;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_LodMenu_ArabLayoutCompment;
		[HideInInspector] public UIDefaultValue m_UI_Item_LodMenu_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_toggles_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_toggles_ArabLayoutCompment;

		[HideInInspector] public VerticalLayoutGroup m_pl_rect_VerticalLayoutGroup;

		[HideInInspector] public UI_Model_Ck_MainIFLod_SubView m_UI_Modle_Ck_guild;
		[HideInInspector] public UI_Model_Ck_MainIFLod_SubView m_UI_Modle_Ck_explore;
		[HideInInspector] public UI_Model_Ck_MainIFLod_SubView m_UI_Modle_Ck_res;
		[HideInInspector] public UI_Model_Ck_MainIFLod_SubView m_UI_Modle_Ck_mark;
		[HideInInspector] public PolygonImage m_img_typeExplore_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_typeExplore_ArabLayoutCompment;

		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_NoSurvey;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_Survey;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_NoVisit;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_Visit;
		[HideInInspector] public PolygonImage m_img_typeRes_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_typeRes_ArabLayoutCompment;

		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_1;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_2;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_3;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_4;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_5;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_6;
		[HideInInspector] public PolygonImage m_img_typeGuild_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_typeGuild_ArabLayoutCompment;

		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_Master;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_Friend;
		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_Build;
		[HideInInspector] public PolygonImage m_img_typeMark_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_typeMark_ArabLayoutCompment;

		[HideInInspector] public UI_Model_LodGizmos_SubView m_UI_Model_Spec;
		[HideInInspector] public UI_Tag_MainIFAnime_Left_SubView m_UI_Tag_MainIFAnime_Left;


        private void UIFinder()
        {       
			m_UI_Item_LodMenu_Animator = gameObject.GetComponent<Animator>();
			m_UI_Item_LodMenu_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
			m_UI_Item_LodMenu_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();
			m_UI_Item_LodMenu_UIDefaultValue = gameObject.GetComponent<UIDefaultValue>();

			m_img_toggles_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_toggles");
			m_img_toggles_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_toggles");

			m_pl_rect_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"img_toggles/pl_rect");

			m_UI_Modle_Ck_guild = new UI_Model_Ck_MainIFLod_SubView(FindUI<RectTransform>(gameObject.transform ,"img_toggles/pl_rect/UI_Modle_Ck_guild"));
			m_UI_Modle_Ck_explore = new UI_Model_Ck_MainIFLod_SubView(FindUI<RectTransform>(gameObject.transform ,"img_toggles/pl_rect/UI_Modle_Ck_explore"));
			m_UI_Modle_Ck_res = new UI_Model_Ck_MainIFLod_SubView(FindUI<RectTransform>(gameObject.transform ,"img_toggles/pl_rect/UI_Modle_Ck_res"));
			m_UI_Modle_Ck_mark = new UI_Model_Ck_MainIFLod_SubView(FindUI<RectTransform>(gameObject.transform ,"img_toggles/pl_rect/UI_Modle_Ck_mark"));
			m_img_typeExplore_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_typeExplore");
			m_img_typeExplore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_typeExplore");

			m_UI_Model_NoSurvey = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeExplore/pl_rect/UI_Model_NoSurvey"));
			m_UI_Model_Survey = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeExplore/pl_rect/UI_Model_Survey"));
			m_UI_Model_NoVisit = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeExplore/pl_rect/UI_Model_NoVisit"));
			m_UI_Model_Visit = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeExplore/pl_rect/UI_Model_Visit"));
			m_img_typeRes_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_typeRes");
			m_img_typeRes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_typeRes");

			m_UI_Model_1 = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeRes/pl_rect/UI_Model_1"));
			m_UI_Model_2 = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeRes/pl_rect/UI_Model_2"));
			m_UI_Model_3 = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeRes/pl_rect/UI_Model_3"));
			m_UI_Model_4 = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeRes/pl_rect/UI_Model_4"));
			m_UI_Model_5 = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeRes/pl_rect/UI_Model_5"));
			m_UI_Model_6 = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeRes/pl_rect/UI_Model_6"));
			m_img_typeGuild_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_typeGuild");
			m_img_typeGuild_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_typeGuild");

			m_UI_Model_Master = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeGuild/pl_rect/UI_Model_Master"));
			m_UI_Model_Friend = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeGuild/pl_rect/UI_Model_Friend"));
			m_UI_Model_Build = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeGuild/pl_rect/UI_Model_Build"));
			m_img_typeMark_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_typeMark");
			m_img_typeMark_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_typeMark");

			m_UI_Model_Spec = new UI_Model_LodGizmos_SubView(FindUI<RectTransform>(gameObject.transform ,"img_typeMark/pl_rect/UI_Model_Spec"));
			m_UI_Tag_MainIFAnime_Left = new UI_Tag_MainIFAnime_Left_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Tag_MainIFAnime_Left"));

			BindEvent();
        }

        #endregion
    }
}
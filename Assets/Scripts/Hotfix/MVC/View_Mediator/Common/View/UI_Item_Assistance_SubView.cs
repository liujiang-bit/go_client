// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_Assistance_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_Assistance_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_Assistance";

        public UI_Item_Assistance_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_Assistance_ViewBinder;

		[HideInInspector] public GridLayoutGroup m_pl_res_GridLayoutGroup;

		[HideInInspector] public UI_Item_AssistanceRes_SubView m_UI_Item_AssistanceRes1;
		[HideInInspector] public UI_Item_AssistanceRes_SubView m_UI_Item_AssistanceRes2;
		[HideInInspector] public UI_Item_AssistanceRes_SubView m_UI_Item_AssistanceRes3;
		[HideInInspector] public UI_Item_AssistanceRes_SubView m_UI_Item_AssistanceRes4;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_reddot_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_reddot_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_Assistance_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_res_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_res");

			m_UI_Item_AssistanceRes1 = new UI_Item_AssistanceRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_res/UI_Item_AssistanceRes1"));
			m_UI_Item_AssistanceRes2 = new UI_Item_AssistanceRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_res/UI_Item_AssistanceRes2"));
			m_UI_Item_AssistanceRes3 = new UI_Item_AssistanceRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_res/UI_Item_AssistanceRes3"));
			m_UI_Item_AssistanceRes4 = new UI_Item_AssistanceRes_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_res/UI_Item_AssistanceRes4"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/lbl_time");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/lbl_name");

			m_img_reddot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_reddot");
			m_img_reddot_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_reddot");


			BindEvent();
        }

        #endregion
    }
}
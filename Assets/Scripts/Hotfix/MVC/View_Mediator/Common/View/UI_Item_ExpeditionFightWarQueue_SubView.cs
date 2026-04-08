// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ExpeditionFightWarQueue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ExpeditionFightWarQueue_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ExpeditionFightWarQueue";

        public UI_Item_ExpeditionFightWarQueue_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ExpeditionFightWarQueue_ViewBinder;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public GameButton m_img_bg_GameButton;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_btn_ck_PolygonImage;
		[HideInInspector] public GameButton m_btn_ck_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_ck_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ExpeditionFightWarQueue_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");
			m_img_bg_GameButton = FindUI<GameButton>(gameObject.transform ,"img_bg");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_num");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));
			m_btn_ck_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_ck");
			m_btn_ck_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_ck");
			m_btn_ck_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_ck");


			BindEvent();
        }

        #endregion
    }
}
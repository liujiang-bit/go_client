// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TalentPage_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_TalentPage_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TalentPage";

        public UI_Item_TalentPage_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_TalentPage;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public PolygonImage m_btn_page_PolygonImage;
		[HideInInspector] public GameButton m_btn_page_GameButton;

		[HideInInspector] public LanguageText m_lbl_page_LanguageText;

		[HideInInspector] public PolygonImage m_img_unuse_PolygonImage;

		[HideInInspector] public PolygonImage m_img_use_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_use_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num3_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num3_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_choose_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_name_PolygonImage;
		[HideInInspector] public GameButton m_btn_name_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_TalentPage = gameObject.GetComponent<RectTransform>();
			m_pl_mes = FindUI<RectTransform>(gameObject.transform ,"pl_mes");
			m_btn_page_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_page");
			m_btn_page_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_page");

			m_lbl_page_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/btn_page/lbl_page");

			m_img_unuse_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_page/img_unuse");

			m_img_use_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_page/img_use");
			m_img_use_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_page/img_use");

			m_lbl_num1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/btn_page/lbl_num1");
			m_lbl_num1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_page/lbl_num1");

			m_lbl_num2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/btn_page/lbl_num2");
			m_lbl_num2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_page/lbl_num2");

			m_lbl_num3_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/btn_page/lbl_num3");
			m_lbl_num3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_page/lbl_num3");

			m_img_choose_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_page/img_choose");

			m_btn_name_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_name");
			m_btn_name_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_name");


			BindEvent();
        }

        #endregion
    }
}
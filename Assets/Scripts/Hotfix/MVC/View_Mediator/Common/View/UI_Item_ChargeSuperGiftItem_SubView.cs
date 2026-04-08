// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeSuperGiftItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeSuperGiftItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeSuperGiftItem";

        public UI_Item_ChargeSuperGiftItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeSuperGiftItem;
		[HideInInspector] public PolygonImage m_pl_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_banner_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_banner_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_blet_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_blet_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_cutoff_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_cutoff_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_cutoff_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_cutoff_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_get_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_get_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_get_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_cur_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public Outline m_lbl_name_Outline;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public LanguageText m_lbl_timeLimit_LanguageText;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_btn_buy;
		[HideInInspector] public LanguageText m_lbl_limit_LanguageText;

		[HideInInspector] public PolygonImage m_img_bestsell_PolygonImage;

		[HideInInspector] public PolygonImage m_img_sellout_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_ChargeSuperGiftItem = gameObject.GetComponent<RectTransform>();
			m_pl_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_bg");

			m_img_banner_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_banner");
			m_img_banner_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/img_banner");

			m_img_blet_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_blet");
			m_img_blet_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/img_blet");

			m_img_cutoff_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_cutoff");
			m_img_cutoff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/img_cutoff");

			m_lbl_cutoff_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_cutoff");
			m_lbl_cutoff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_cutoff");

			m_lbl_dec_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_dec");
			m_lbl_dec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_dec");

			m_lbl_get_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_get");
			m_lbl_get_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"rect/lbl_get");
			m_lbl_get_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"rect/lbl_get");

			m_img_cur_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/lbl_get/img_cur");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_name");
			m_lbl_name_Outline = FindUI<Outline>(gameObject.transform ,"rect/lbl_name");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"rect/sv_list");

			m_lbl_timeLimit_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_timeLimit");

			m_btn_buy = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/btn_buy"));
			m_lbl_limit_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_limit");

			m_img_bestsell_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_bestsell");

			m_img_sellout_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_sellout");


			BindEvent();
        }

        #endregion
    }
}
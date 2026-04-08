// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BookList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_BookList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BookList";

        public UI_Item_BookList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BookList;
		[HideInInspector] public Empty4Raycast m_pl_mes_Empty4Raycast;

		[HideInInspector] public PolygonImage m_img_booktype_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_booktype_ArabLayoutCompment;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;
		[HideInInspector] public PolygonImage m_btn_change_PolygonImage;
		[HideInInspector] public GameButton m_btn_change_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_change_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_name_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_lbl_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_lbl_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_lbl_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_dec_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_coordinate_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_coordinate_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_coordinate_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_share_PolygonImage;
		[HideInInspector] public GameButton m_btn_share_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_share_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_delete_PolygonImage;
		[HideInInspector] public GameButton m_btn_delete_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_delete_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_go;


        private void UIFinder()
        {       
			m_UI_Item_BookList = gameObject.GetComponent<RectTransform>();
			m_pl_mes_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_mes");

			m_img_booktype_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_booktype");
			m_img_booktype_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/img_booktype");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/UI_Common_Redpoint"));
			m_btn_change_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_change");
			m_btn_change_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_change");
			m_btn_change_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_change");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_name");
			m_lbl_name_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_mes/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_name");

			m_lbl_lbl_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_name/lbl_lbl");
			m_lbl_lbl_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_mes/lbl_name/lbl_lbl");
			m_lbl_lbl_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_name/lbl_lbl");

			m_lbl_dec_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_name/lbl_dec");
			m_lbl_dec_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_mes/lbl_name/lbl_dec");
			m_lbl_dec_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_name/lbl_dec");

			m_lbl_coordinate_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_coordinate");
			m_lbl_coordinate_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_mes/lbl_coordinate");
			m_lbl_coordinate_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/lbl_coordinate");

			m_btn_share_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_share");
			m_btn_share_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_share");
			m_btn_share_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_share");

			m_btn_delete_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_delete");
			m_btn_delete_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_delete");
			m_btn_delete_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_delete");

			m_btn_go = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/btn_go"));

			BindEvent();
        }

        #endregion
    }
}
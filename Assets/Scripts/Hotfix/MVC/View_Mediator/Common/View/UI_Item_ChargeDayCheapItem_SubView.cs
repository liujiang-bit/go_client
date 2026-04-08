// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeDayCheapItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeDayCheapItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeDayCheapItem";

        public UI_Item_ChargeDayCheapItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeDayCheapItem;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public RectTransform m_pl_box;
		[HideInInspector] public GameButton m_btn_boxinfo_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_boxinfo_Empty4Raycast;

		[HideInInspector] public PolygonImage m_img_box_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_get_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_get_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_cur_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_dec_LanguageText;

		[HideInInspector] public RectTransform m_pl_tips;
		[HideInInspector] public PolygonImage m_btn_tipsinfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_tipsinfo_GameButton;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_btn_buy;


        private void UIFinder()
        {       
			m_UI_Item_ChargeDayCheapItem = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_name");

			m_pl_box = FindUI<RectTransform>(gameObject.transform ,"rect/pl_box");
			m_btn_boxinfo_GameButton = FindUI<GameButton>(gameObject.transform ,"rect/pl_box/btn_boxinfo");
			m_btn_boxinfo_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"rect/pl_box/btn_boxinfo");

			m_img_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_box/img_box");

			m_lbl_get_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_box/lbl_get");
			m_lbl_get_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"rect/pl_box/lbl_get");

			m_img_cur_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_box/lbl_get/img_cur");

			m_lbl_dec_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/pl_box/lbl_dec");

			m_pl_tips = FindUI<RectTransform>(gameObject.transform ,"rect/pl_tips");
			m_btn_tipsinfo_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_tips/btn_tipsinfo");
			m_btn_tipsinfo_GameButton = FindUI<GameButton>(gameObject.transform ,"rect/pl_tips/btn_tipsinfo");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"rect/pl_tips/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/pl_tips/sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"rect/pl_tips/sv_list");

			m_btn_buy = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/btn_buy"));

			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_IconOnEquip_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Pop_IconOnEquip_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_IconOnEquip";

        public UI_Pop_IconOnEquip_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Pop_IconOnEquip_ViewBinder;

		[HideInInspector] public RectTransform m_pl_offset;
		[HideInInspector] public PolygonImage m_pl_arrow_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_size_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public GameButton m_btn_click_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_click_Empty4Raycast;

		[HideInInspector] public RectTransform m_pl_offset1;
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public GameButton m_btn_click1_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_click1_Empty4Raycast;

		[HideInInspector] public RectTransform m_pl_get;
		[HideInInspector] public PolygonImage m_pl_completeItems_PolygonImage;

		[HideInInspector] public PolygonImage m_pl_Size_PolygonImage;

		[HideInInspector] public HorizontalLayoutGroup m_pl_itemList_HorizontalLayoutGroup;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public GameButton m_btn_get_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_get_Empty4Raycast;

		[HideInInspector] public RectTransform m_pl_item;
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public Animator m_lbl_time_Animator;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public Animator m_lbl_desc_Animator;

		[HideInInspector] public UI_Model_Item_SubView m_img_get;


        private void UIFinder()
        {       
			m_UI_Pop_IconOnEquip_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_pl_offset = FindUI<RectTransform>(gameObject.transform ,"pl_offset");
			m_pl_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_offset/pl_arrow");

			m_pl_size_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_offset/pl_arrow/pl_size");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_offset/pl_arrow/pl_size/img_icon");

			m_btn_click_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_offset/btn_click");
			m_btn_click_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_offset/btn_click");

			m_pl_offset1 = FindUI<RectTransform>(gameObject.transform ,"pl_offset1");
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_offset1/pl_size/lbl_languageText");

			m_btn_click1_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_offset1/btn_click1");
			m_btn_click1_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_offset1/btn_click1");

			m_pl_get = FindUI<RectTransform>(gameObject.transform ,"pl_get");
			m_pl_completeItems_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_get/pl_completeItems");

			m_pl_Size_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_get/pl_completeItems/pl_Size");

			m_pl_itemList_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"pl_get/pl_completeItems/pl_Size/pl_itemList");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_get/pl_completeItems/pl_Size/pl_itemList/UI_Model_Item"));
			m_btn_get_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_get/btn_get");
			m_btn_get_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_get/btn_get");

			m_pl_item = FindUI<RectTransform>(gameObject.transform ,"pl_get/pl_item");
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pl_get/pl_item/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_get/pl_item/pb_rogressBar");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_get/pl_item/pb_rogressBar/lbl_time");
			m_lbl_time_Animator = FindUI<Animator>(gameObject.transform ,"pl_get/pl_item/pb_rogressBar/lbl_time");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_get/pl_item/pb_rogressBar/lbl_desc");
			m_lbl_desc_Animator = FindUI<Animator>(gameObject.transform ,"pl_get/pl_item/pb_rogressBar/lbl_desc");

			m_img_get = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_get/pl_item/img_get"));

			BindEvent();
        }

        #endregion
    }
}
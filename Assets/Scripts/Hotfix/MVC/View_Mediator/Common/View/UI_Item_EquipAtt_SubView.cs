// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipAtt_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EquipAtt_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipAtt";

        public UI_Item_EquipAtt_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public VerticalLayoutGroup m_UI_Item_EquipAtt_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_name_Shadow;

		[HideInInspector] public LanguageText m_lbl_lv_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lv_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_lv_Shadow;

		[HideInInspector] public Image m_pl_att_Image;

		[HideInInspector] public GridLayoutGroup m_pl_equipatt_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_equipatt_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_pl_equipatt_ContentSizeFitter;

		[HideInInspector] public UI_Model_EquipAtt_SubView m_UI_Model_EquipAtt;
		[HideInInspector] public Image m_pl_talent_Image;

		[HideInInspector] public LanguageText m_lbl_title2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_title2_ArabLayoutCompment;
		[HideInInspector] public Shadow m_lbl_title2_Shadow;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_talent_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_talent_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_talent_ContentSizeFitter;

		[HideInInspector] public Image m_pl_compose_Image;
		[HideInInspector] public ArabLayoutCompment m_pl_compose_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_composeAt_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_composeAt_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_UI_Item_EquipComposeAtt_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_EquipComposeAtt_ArabLayoutCompment;
		[HideInInspector] public Shadow m_UI_Item_EquipComposeAtt_Shadow;



        private void UIFinder()
        {       
			m_UI_Item_EquipAtt_VerticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");
			m_lbl_name_Shadow = FindUI<Shadow>(gameObject.transform ,"lbl_name");

			m_lbl_lv_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_lv");
			m_lbl_lv_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_lv");
			m_lbl_lv_Shadow = FindUI<Shadow>(gameObject.transform ,"lbl_lv");

			m_pl_att_Image = FindUI<Image>(gameObject.transform ,"pl_att");

			m_pl_equipatt_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_att/pl_equipatt");
			m_pl_equipatt_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_att/pl_equipatt");
			m_pl_equipatt_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_att/pl_equipatt");

			m_UI_Model_EquipAtt = new UI_Model_EquipAtt_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_att/pl_equipatt/UI_Model_EquipAtt"));
			m_pl_talent_Image = FindUI<Image>(gameObject.transform ,"pl_talent");

			m_lbl_title2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_talent/lbl_title2");
			m_lbl_title2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_talent/lbl_title2");
			m_lbl_title2_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_talent/lbl_title2");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_talent/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_talent/img_icon");

			m_lbl_talent_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_talent/lbl_talent");
			m_lbl_talent_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_talent/lbl_talent");
			m_lbl_talent_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_talent/lbl_talent");

			m_pl_compose_Image = FindUI<Image>(gameObject.transform ,"pl_compose");
			m_pl_compose_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_compose");

			m_pl_composeAt_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_compose/pl_composeAt");
			m_pl_composeAt_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_compose/pl_composeAt");

			m_UI_Item_EquipComposeAtt_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_compose/pl_composeAt/UI_Item_EquipComposeAtt");
			m_UI_Item_EquipComposeAtt_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_compose/pl_composeAt/UI_Item_EquipComposeAtt");
			m_UI_Item_EquipComposeAtt_Shadow = FindUI<Shadow>(gameObject.transform ,"pl_compose/pl_composeAt/UI_Item_EquipComposeAtt");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTurntableItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EventTurntableItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTurntableItem";

        public UI_Item_EventTurntableItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTurntableItem;
		[HideInInspector] public PolygonImage m_img_rare_PolygonImage;

		[HideInInspector] public PolygonImage m_img_itemmask_PolygonImage;
		[HideInInspector] public Mask m_img_itemmask_Mask;

		[HideInInspector] public PolygonImage m_img_item_PolygonImage;
		[HideInInspector] public RectMask2D m_img_item_RectMask2D;

		[HideInInspector] public PolygonImage m_pl_desc_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public PolygonImage m_img_get_PolygonImage;
		[HideInInspector] public Mask m_img_get_Mask;

		[HideInInspector] public LanguageText m_lbl_get_LanguageText;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;

		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public PolygonImage m_btn_node_PolygonImage;
		[HideInInspector] public GameButton m_btn_node_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_EventTurntableItem = gameObject.GetComponent<RectTransform>();
			m_img_rare_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_rare");

			m_img_itemmask_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_rare/img_itemmask");
			m_img_itemmask_Mask = FindUI<Mask>(gameObject.transform ,"img_rare/img_itemmask");

			m_img_item_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_rare/img_itemmask/img_item");
			m_img_item_RectMask2D = FindUI<RectMask2D>(gameObject.transform ,"img_rare/img_itemmask/img_item");

			m_pl_desc_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_rare/pl_desc_bg");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_rare/pl_desc_bg/lbl_desc");

			m_img_get_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_rare/img_get");
			m_img_get_Mask = FindUI<Mask>(gameObject.transform ,"img_rare/img_get");

			m_lbl_get_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_rare/img_get/lbl_get");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_rare/lbl_num");

			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");
			m_btn_node_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_node");
			m_btn_node_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_node");


			BindEvent();
        }

        #endregion
    }
}
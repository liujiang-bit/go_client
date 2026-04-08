// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainEquipUse_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainEquipUse_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainEquipUse";

        public UI_Item_CaptainEquipUse_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptainEquipUse;
		[HideInInspector] public GameButton m_btn_btn_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_btn_Empty4Raycast;

		[HideInInspector] public PolygonImage m_img_color_PolygonImage;

		[HideInInspector] public PolygonImage m_img_set_PolygonImage;

		[HideInInspector] public PolygonImage m_img_equip_PolygonImage;

		[HideInInspector] public PolygonImage m_img_selected_PolygonImage;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_talent_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_talent_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_key_PolygonImage;

		[HideInInspector] public PolygonImage m_img_add_PolygonImage;

		[HideInInspector] public UI_Common_Redpoint_SubView m_UI_Common_Redpoint;


        private void UIFinder()
        {       
			m_UI_Item_CaptainEquipUse = gameObject.GetComponent<RectTransform>();
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");
			m_btn_btn_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"btn_btn");

			m_img_color_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_color");

			m_img_set_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_set");

			m_img_equip_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_equip");

			m_img_selected_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_selected");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_btn/UI_Model_CaptainHead"));
			m_img_talent_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_talent");
			m_img_talent_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/img_talent");

			m_img_key_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_key");

			m_img_add_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_add");

			m_UI_Common_Redpoint = new UI_Common_Redpoint_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Common_Redpoint"));

			BindEvent();
        }

        #endregion
    }
}
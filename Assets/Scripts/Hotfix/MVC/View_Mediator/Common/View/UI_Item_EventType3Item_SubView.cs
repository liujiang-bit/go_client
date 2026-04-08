// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventType3Item_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventType3Item_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventType3Item";

        public UI_Item_EventType3Item_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventType3Item;
		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_frame_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_go;


        private void UIFinder()
        {       
			m_UI_Item_EventType3Item = gameObject.GetComponent<RectTransform>();
			m_img_frame_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_frame");
			m_img_frame_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_frame");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_frame/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_desc");

			m_btn_go = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_go"));

			BindEvent();
        }

        #endregion
    }
}
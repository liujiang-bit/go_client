// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainStatueSourceItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_CaptainStatueSourceItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainStatueSourceItem";

        public UI_Item_CaptainStatueSourceItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptainStatueSourceItem;
		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_go;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_CaptainStatueSourceItem = gameObject.GetComponent<RectTransform>();
			m_btn_go = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/btn_go"));
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"rect/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rect/lbl_desc");


			BindEvent();
        }

        #endregion
    }
}
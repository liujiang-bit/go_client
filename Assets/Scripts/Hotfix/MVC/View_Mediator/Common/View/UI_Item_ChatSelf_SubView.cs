// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatSelf_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChatSelf_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatSelf";

        public UI_Item_ChatSelf_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ChatSelf_ViewBinder;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_img_chatbg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_content_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_chatarrow_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_ChatSelf_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_img_chatbg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_chatbg");

			m_lbl_content_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_content");
			m_lbl_content_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_content");

			m_img_chatarrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_chatarrow");


			BindEvent();
        }

        #endregion
    }
}
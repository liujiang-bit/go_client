// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailType15_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailType15_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailType15";

        public UI_Item_MailType15_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public VerticalLayoutGroup m_UI_Item_MailType15_VerticalLayoutGroup;
		[HideInInspector] public ViewBinder m_UI_Item_MailType15_ViewBinder;

		[HideInInspector] public UI_Item_MailTitle_SubView m_UI_Item_MailTitle;
		[HideInInspector] public LanguageText m_lbl_Content_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_Content_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_Content_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_build_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_buff_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_icon_buff2_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_icon_buff2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_buffatt2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buffatt2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_icon_buff1_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_icon_buff1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_buffatt1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buffatt1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mes_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_frame_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_icon_build_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_icon_build_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link;


        private void UIFinder()
        {       
			m_UI_Item_MailType15_VerticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();
			m_UI_Item_MailType15_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Item_MailTitle = new UI_Item_MailTitle_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_MailTitle"));
			m_lbl_Content_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_Content");
			m_lbl_Content_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_Content");
			m_lbl_Content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_Content");

			m_pl_build_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build");

			m_pl_buff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build/pl_buff");

			m_icon_buff2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_build/pl_buff/icon_buff2");
			m_icon_buff2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build/pl_buff/icon_buff2");

			m_lbl_buffatt2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_build/pl_buff/icon_buff2/lbl_buffatt2");
			m_lbl_buffatt2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build/pl_buff/icon_buff2/lbl_buffatt2");

			m_icon_buff1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_build/pl_buff/icon_buff1");
			m_icon_buff1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build/pl_buff/icon_buff1");

			m_lbl_buffatt1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_build/pl_buff/icon_buff1/lbl_buffatt1");
			m_lbl_buffatt1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build/pl_buff/icon_buff1/lbl_buffatt1");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_build/lbl_mes");
			m_lbl_mes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build/lbl_mes");

			m_img_frame_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_build/img_frame");
			m_img_frame_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build/img_frame");

			m_icon_build_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_build/icon_build");
			m_icon_build_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_build/icon_build");

			m_UI_Model_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_build/UI_Model_Link"));

			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailBeScout_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailBeScout_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailBeScout";

        public UI_Item_MailBeScout_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_MailBeScout_ViewBinder;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_Link_SubView m_lbl_playername;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_lbl_linkImageText;
		[HideInInspector] public PolygonImage m_img_reddot_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_reddot_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MailBeScout_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_lbl_desc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"count/res/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"count/res/lbl_desc");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"count/res/UI_PlayerHead"));
			m_lbl_playername = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"count/res/lbl_playername"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/lbl_time");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"title/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"title/lbl_name");

			m_lbl_linkImageText = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"title/lbl_linkImageText"));
			m_img_reddot_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_reddot");
			m_img_reddot_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_reddot");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailBattleLogTitle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailBattleLogTitle_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailBattleLogTitle";

        public UI_Item_MailBattleLogTitle_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailBattleLogTitle;
		[HideInInspector] public PolygonImage m_img_line1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_line2_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_date_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_date_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_date_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link;


        private void UIFinder()
        {       
			m_UI_Item_MailBattleLogTitle = gameObject.GetComponent<RectTransform>();
			m_img_line1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_line1");

			m_img_line2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_line2");

			m_lbl_date_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_date");
			m_lbl_date_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_date");
			m_lbl_date_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_date");

			m_UI_Model_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Link"));

			BindEvent();
        }

        #endregion
    }
}
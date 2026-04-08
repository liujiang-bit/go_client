// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailReward";

        public UI_Item_MailReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailReward;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public Shadow m_lbl_name_Shadow;



        private void UIFinder()
        {       
			m_UI_Item_MailReward = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_bg");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_name");
			m_lbl_name_Shadow = FindUI<Shadow>(gameObject.transform ,"btn_btn/lbl_name");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_SettingNoticeItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_SettingNoticeItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_SettingNoticeItem";

        public UI_Item_SettingNoticeItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_SettingNoticeItem;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_des_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_des_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_switch_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_switch_ArabLayoutCompment;
		[HideInInspector] public SwitchToggle m_ck_switch_SwitchToggle;

		[HideInInspector] public PolygonImage m_img_switch_PolygonImage;

		[HideInInspector] public PolygonImage m_img_switchClose_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_SettingNoticeItem = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_name");

			m_lbl_des_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_des");
			m_lbl_des_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/lbl_des");

			m_ck_switch_GameToggle = FindUI<GameToggle>(gameObject.transform ,"bg/ck_switch");
			m_ck_switch_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"bg/ck_switch");
			m_ck_switch_SwitchToggle = FindUI<SwitchToggle>(gameObject.transform ,"bg/ck_switch");

			m_img_switch_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bg/ck_switch/img_switch");

			m_img_switchClose_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"bg/ck_switch/img_switchClose");


			BindEvent();
        }

        #endregion
    }
}
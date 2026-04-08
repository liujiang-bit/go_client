// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GeneralSetting_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_GeneralSetting_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GeneralSetting";

        public UI_Item_GeneralSetting_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_GeneralSetting;
		[HideInInspector] public LanguageText m_lbl_dec1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_dec2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_dec2_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_switch_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_switch_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_close_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_close_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_open_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_open_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_GeneralSetting = gameObject.GetComponent<RectTransform>();
			m_lbl_dec1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec1");
			m_lbl_dec1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec1");

			m_lbl_dec2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_dec2");
			m_lbl_dec2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_dec2");

			m_ck_switch_GameToggle = FindUI<GameToggle>(gameObject.transform ,"ck_switch");
			m_ck_switch_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ck_switch");

			m_img_close_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ck_switch/img_close");
			m_img_close_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ck_switch/img_close");

			m_img_open_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ck_switch/img_open");
			m_img_open_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ck_switch/img_open");


			BindEvent();
        }

        #endregion
    }
}
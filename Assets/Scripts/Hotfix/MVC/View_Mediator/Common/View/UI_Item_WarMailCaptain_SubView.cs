// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WarMailCaptain_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WarMailCaptain_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WarMailCaptain";

        public UI_Item_WarMailCaptain_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WarMailCaptain;
		[HideInInspector] public RectTransform m_pl_key;
		[HideInInspector] public LanguageText m_lbl_key_LanguageText;

		[HideInInspector] public PolygonImage m_img_key_PolygonImage;

		[HideInInspector] public PolygonImage m_icon_key_PolygonImage;

		[HideInInspector] public RectTransform m_pl_exp;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public PolygonImage m_img_exp_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_exp_LanguageText;

		[HideInInspector] public RectTransform m_pl_empty;
		[HideInInspector] public LanguageText m_lbl_empty_LanguageText;

		[HideInInspector] public PolygonImage m_img_empty_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_WarMailCaptain = gameObject.GetComponent<RectTransform>();
			m_pl_key = FindUI<RectTransform>(gameObject.transform ,"pl_key");
			m_lbl_key_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_key/lbl_key");

			m_img_key_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_key/img_key");

			m_icon_key_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_key/icon_key");

			m_pl_exp = FindUI<RectTransform>(gameObject.transform ,"pl_exp");
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_exp/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_exp/lbl_name");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_exp/UI_Model_CaptainHead"));
			m_img_exp_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_exp/img_exp");

			m_lbl_exp_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_exp/img_exp/lbl_exp");

			m_pl_empty = FindUI<RectTransform>(gameObject.transform ,"pl_empty");
			m_lbl_empty_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_empty/lbl_empty");

			m_img_empty_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_empty/img_empty");


			BindEvent();
        }

        #endregion
    }
}
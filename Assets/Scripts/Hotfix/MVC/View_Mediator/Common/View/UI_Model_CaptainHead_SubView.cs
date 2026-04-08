// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_CaptainHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_CaptainHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_CaptainHead";

        public UI_Model_CaptainHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Model_CaptainHead_PolygonImage;
		[HideInInspector] public GrayChildrens m_UI_Model_CaptainHead_GrayChildrens;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;

		[HideInInspector] public PolygonImage m_img_lvbg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_lv_LanguageText;

		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public UI_10063_1_SubView m_UI_10063_1;


        private void UIFinder()
        {       
			m_UI_Model_CaptainHead_PolygonImage = gameObject.GetComponent<PolygonImage>();
			m_UI_Model_CaptainHead_GrayChildrens = gameObject.GetComponent<GrayChildrens>();

			m_img_char_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_char");

			m_img_lvbg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_lvbg");

			m_lbl_lv_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_lvbg/lbl_lv");

			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");
			m_UI_10063_1 = new UI_10063_1_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_effect/UI_10063_1"));

			BindEvent();
        }

        #endregion
    }
}
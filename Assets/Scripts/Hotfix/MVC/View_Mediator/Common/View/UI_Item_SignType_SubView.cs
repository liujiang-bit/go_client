// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_SignType_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_SignType_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_SignType";

        public UI_Item_SignType_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_SignType;
		[HideInInspector] public PolygonImage m_btn_type_PolygonImage;
		[HideInInspector] public GameButton m_btn_type_GameButton;

		[HideInInspector] public PolygonImage m_img_type_PolygonImage;

		[HideInInspector] public PolygonImage m_img_choose_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_use_LanguageText;

		[HideInInspector] public RectTransform m_pl_effect;


        private void UIFinder()
        {       
			m_UI_Item_SignType = gameObject.GetComponent<RectTransform>();
			m_btn_type_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_type");
			m_btn_type_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_type");

			m_img_type_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_type/img_type");

			m_img_choose_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_type/img_choose");

			m_lbl_use_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_type/lbl_use");

			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");

			BindEvent();
        }

        #endregion
    }
}
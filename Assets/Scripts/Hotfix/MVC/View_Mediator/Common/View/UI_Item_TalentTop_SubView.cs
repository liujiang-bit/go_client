// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TalentTop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_TalentTop_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TalentTop";

        public UI_Item_TalentTop_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_TalentTop;
		[HideInInspector] public GameButton m_pl_mes_GameButton;
		[HideInInspector] public Empty4Raycast m_pl_mes_Empty4Raycast;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public RectTransform m_pl_dark;
		[HideInInspector] public RectTransform m_pl_light;
		[HideInInspector] public PolygonImage m_img_light5_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_light5_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_light4_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_light4_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_light3_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_light3_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_light2_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_light2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_light1_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_light1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_TalentTop = gameObject.GetComponent<RectTransform>();
			m_pl_mes_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes");
			m_pl_mes_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_mes");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_icon");

			m_pl_dark = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_dark");
			m_pl_light = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_light");
			m_img_light5_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_light/img_light5");
			m_img_light5_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_light/img_light5");

			m_img_light4_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_light/img_light4");
			m_img_light4_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_light/img_light4");

			m_img_light3_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_light/img_light3");
			m_img_light3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_light/img_light3");

			m_img_light2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_light/img_light2");
			m_img_light2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_light/img_light2");

			m_img_light1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_light/img_light1");
			m_img_light1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_light/img_light1");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_name");


			BindEvent();
        }

        #endregion
    }
}
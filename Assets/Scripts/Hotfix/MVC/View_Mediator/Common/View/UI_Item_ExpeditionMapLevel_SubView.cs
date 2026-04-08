// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ExpeditionMapLevel_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ExpeditionMapLevel_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ExpeditionMapLevel";

        public UI_Item_ExpeditionMapLevel_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ExpeditionMapLevel;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;

		[HideInInspector] public RectTransform m_pl_flag1;
		[HideInInspector] public PolygonImage m_img_flag1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_flag2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_flag3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_flag4_PolygonImage;

		[HideInInspector] public RectTransform m_pl_flag2;
		[HideInInspector] public PolygonImage m_img_atk_PolygonImage;

		[HideInInspector] public PolygonImage m_img_def_PolygonImage;

		[HideInInspector] public RectTransform m_pl_level;
		[HideInInspector] public LanguageText m_lbl_level_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_dark_GridLayoutGroup;

		[HideInInspector] public GridLayoutGroup m_pl_star_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_star_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_star1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_star2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_star3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_box_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_ExpeditionMapLevel = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");

			m_pl_flag1 = FindUI<RectTransform>(gameObject.transform ,"btn_btn/pl_flag1");
			m_img_flag1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_flag1/img_flag1");

			m_img_flag2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_flag1/img_flag2");

			m_img_flag3_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_flag1/img_flag3");

			m_img_flag4_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_flag1/img_flag4");

			m_pl_flag2 = FindUI<RectTransform>(gameObject.transform ,"btn_btn/pl_flag2");
			m_img_atk_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_flag2/img_atk");

			m_img_def_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_flag2/img_def");

			m_pl_level = FindUI<RectTransform>(gameObject.transform ,"btn_btn/pl_level");
			m_lbl_level_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/pl_level/lbl_level");

			m_pl_dark_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"btn_btn/pl_level/pl_dark");

			m_pl_star_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"btn_btn/pl_level/pl_star");
			m_pl_star_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_btn/pl_level/pl_star");

			m_img_star1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_level/pl_star/img_star1");

			m_img_star2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_level/pl_star/img_star2");

			m_img_star3_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/pl_level/pl_star/img_star3");

			m_img_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn/img_box");


			BindEvent();
        }

        #endregion
    }
}
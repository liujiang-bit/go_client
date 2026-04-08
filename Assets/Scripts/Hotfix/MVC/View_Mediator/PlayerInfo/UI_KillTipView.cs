// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月16日
// Update Time         :    2020年7月16日
// Class Description   :    UI_KillTipView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_KillTipView : GameView
    {
		public const string VIEW_NAME = "UI_KillTip";

        public UI_KillTipView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_layout_GridLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_level1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_level2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_level3_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level3_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_level4_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level4_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_level5_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level5_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_tipkilltime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_tipkilltime_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_tipkill_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_tipkill_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_pos_Animator;
		[HideInInspector] public UIDefaultValue m_pl_pos_UIDefaultValue;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_arrowSideL");

			m_pl_layout_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_layout");

			m_lbl_level1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_layout/armlevelbg/lbl_level1");
			m_lbl_level1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_layout/armlevelbg/lbl_level1");

			m_lbl_level2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_layout/armlevelbg2/lbl_level2");
			m_lbl_level2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_layout/armlevelbg2/lbl_level2");

			m_lbl_level3_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_layout/armlevelbg3/lbl_level3");
			m_lbl_level3_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_layout/armlevelbg3/lbl_level3");

			m_lbl_level4_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_layout/armlevelbg4/lbl_level4");
			m_lbl_level4_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_layout/armlevelbg4/lbl_level4");

			m_lbl_level5_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_layout/armlevelbg5/lbl_level5");
			m_lbl_level5_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_layout/armlevelbg5/lbl_level5");

			m_lbl_tipkilltime_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_tipkilltime");
			m_lbl_tipkilltime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_tipkilltime");

			m_lbl_tipkill_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_tipkill");
			m_lbl_tipkill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_tipkill");

			m_pl_pos_Animator = FindUI<Animator>(vb.transform ,"pl_pos");
			m_pl_pos_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
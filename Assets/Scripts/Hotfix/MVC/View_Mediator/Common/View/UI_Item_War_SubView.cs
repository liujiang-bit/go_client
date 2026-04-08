// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_War_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_War_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_War";

        public UI_Item_War_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_War_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_bg_PolygonImage;
		[HideInInspector] public GameButton m_btn_bg_GameButton;

		[HideInInspector] public GridLayoutGroup m_pl_arrow_GridLayoutGroup;

		[HideInInspector] public PolygonImage m_img_war_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_nameSelf_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_nameSelf_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_nameTarget_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_nameTarget_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;

		[HideInInspector] public GameSlider m_pb_ready_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_ready_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_Fill_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_armystate_LanguageText;

		[HideInInspector] public LanguageText m_lbl_join_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_join_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_delete_PolygonImage;
		[HideInInspector] public GameButton m_btn_delete_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_delete_ArabLayoutCompment;

		[HideInInspector] public UI_Item_WarTarget_SubView m_UI_Item_WarTargetMy;
		[HideInInspector] public UI_Item_WarTarget_SubView m_UI_Item_WarTarget;


        private void UIFinder()
        {       
			m_UI_Item_War_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_bg");
			m_btn_bg_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_bg");

			m_pl_arrow_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"mid/pl_arrow");

			m_img_war_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"mid/img_war");

			m_lbl_nameSelf_LanguageText = FindUI<LanguageText>(gameObject.transform ,"mid/lbl_nameSelf");
			m_lbl_nameSelf_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"mid/lbl_nameSelf");

			m_lbl_nameTarget_LanguageText = FindUI<LanguageText>(gameObject.transform ,"mid/lbl_nameTarget");
			m_lbl_nameTarget_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"mid/lbl_nameTarget");

			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"mid/bg/lbl_armyCount");

			m_pb_ready_GameSlider = FindUI<GameSlider>(gameObject.transform ,"mid/bg/pb_ready");
			m_pb_ready_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"mid/bg/pb_ready");

			m_img_Fill_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"mid/bg/pb_ready/Fill Area/img_Fill");

			m_lbl_armystate_LanguageText = FindUI<LanguageText>(gameObject.transform ,"mid/bg/lbl_armystate");

			m_lbl_join_LanguageText = FindUI<LanguageText>(gameObject.transform ,"mid/lbl_join");
			m_lbl_join_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"mid/lbl_join");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"mid/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(gameObject.transform ,"mid/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"mid/btn_add");

			m_btn_delete_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"mid/btn_delete");
			m_btn_delete_GameButton = FindUI<GameButton>(gameObject.transform ,"mid/btn_delete");
			m_btn_delete_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"mid/btn_delete");

			m_UI_Item_WarTargetMy = new UI_Item_WarTarget_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_WarTargetMy"));
			m_UI_Item_WarTarget = new UI_Item_WarTarget_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_WarTarget"));

			BindEvent();
        }

        #endregion
    }
}
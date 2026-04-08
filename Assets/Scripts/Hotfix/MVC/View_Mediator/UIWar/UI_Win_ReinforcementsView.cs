// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月24日
// Update Time         :    2020年6月24日
// Class Description   :    UI_Win_ReinforcementsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ReinforcementsView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Reinforcements";

        public UI_Win_ReinforcementsView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public RectTransform m_pl_armyList;
		[HideInInspector] public LanguageText m_lbl_Empty_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_barText_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_icon_PolygonImage;
		[HideInInspector] public GameButton m_btn_icon_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_icon_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_histroy;
		[HideInInspector] public Animator m_pl_tip_Animator;
		[HideInInspector] public UIDefaultValue m_pl_tip_UIDefaultValue;
		[HideInInspector] public ArabLayoutCompment m_pl_tip_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideTop_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_text_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_histroy;
		[HideInInspector] public RectTransform m_pl_histroyTitle;
		[HideInInspector] public ScrollRect m_sv_list_histroy_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_histroy_PolygonImage;
		[HideInInspector] public ListView m_sv_list_histroy_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_armyList = FindUI<RectTransform>(vb.transform ,"pl_armyList");
			m_lbl_Empty_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_armyList/lbl_Empty");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_armyList/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_armyList/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_armyList/sv_list");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_armyList/top/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_armyList/top/pb_rogressBar");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_armyList/top/pb_rogressBar/lbl_barText");
			m_lbl_barText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_armyList/top/pb_rogressBar/lbl_barText");

			m_btn_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_armyList/top/btn_icon");
			m_btn_icon_GameButton = FindUI<GameButton>(vb.transform ,"pl_armyList/top/btn_icon");
			m_btn_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_armyList/top/btn_icon");

			m_UI_histroy = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_armyList/top/UI_histroy"));
			m_pl_tip_Animator = FindUI<Animator>(vb.transform ,"pl_armyList/top/pl_tip");
			m_pl_tip_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_armyList/top/pl_tip");
			m_pl_tip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_armyList/top/pl_tip");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_armyList/top/pl_tip/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_armyList/top/pl_tip/img_bg");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_armyList/top/pl_tip/img_bg/img_arrowSideTop");
			m_img_arrowSideTop_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_armyList/top/pl_tip/img_bg/img_arrowSideTop");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_armyList/top/pl_tip/img_bg/lbl_text");
			m_lbl_text_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_armyList/top/pl_tip/img_bg/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_armyList/top/pl_tip/img_bg/lbl_text");

			m_pl_histroy = FindUI<RectTransform>(vb.transform ,"pl_histroy");
			m_pl_histroyTitle = FindUI<RectTransform>(vb.transform ,"pl_histroy/pl_histroyTitle");
			m_sv_list_histroy_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_histroy/sv_list_histroy");
			m_sv_list_histroy_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_histroy/sv_list_histroy");
			m_sv_list_histroy_ListView = FindUI<ListView>(vb.transform ,"pl_histroy/sv_list_histroy");


            UI_Win_ReinforcementsMediator mt = new UI_Win_ReinforcementsMediator(vb.gameObject);
            mt.view = this;
            AppFacade.GetInstance().RegisterMediator(mt);
			if(mt.IsOpenUpdate)
			{
                vb.fixedUpdateCallback = mt.FixedUpdate;
                vb.lateUpdateCallback = mt.LateUpdate;
				vb.updateCallback = mt.Update;
			}
            vb.openAniEndCallback = mt.OpenAniEnd;
            vb.onWinFocusCallback = mt.WinFocus;
            vb.onWinCloseCallback = mt.WinClose;
            vb.onPrewarmCallback = mt.PrewarmComplete;
        }

        #endregion

        public override void Start () {
            UIFinder();
    	}
        public override void OnDestroy()
        {
            AppFacade.GetInstance().RemoveView(vb);
        }

    }
}

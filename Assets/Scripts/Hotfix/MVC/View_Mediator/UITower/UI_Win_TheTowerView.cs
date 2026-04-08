// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月5日
// Update Time         :    2020年6月5日
// Class Description   :    UI_Win_TheTowerView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_TheTowerView : GameView
    {
        public const string VIEW_NAME = "UI_Win_TheTower";

        public UI_Win_TheTowerView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public UI_Effect_BuildShow_SubView m_UI_Effect_BuildShow;
		[HideInInspector] public PolygonImage m_img_buildImg_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_Right_ArabLayoutCompment;
		[HideInInspector] public Animator m_pl_Right_Animator;

		[HideInInspector] public CanvasGroup m_pl_info_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_hp_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_hp_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_state_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_state_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_bar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_bar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_Fill_PolygonImage;

		[HideInInspector] public PolygonImage m_img_tips_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_tips_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideTop_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_tiptext_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_tiptext_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_tiptext_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_BuildLevelInfo_PolygonImage;
		[HideInInspector] public GameButton m_btn_BuildLevelInfo_GameButton;
		[HideInInspector] public BtnAnimation m_btn_BuildLevelInfo_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_BuildLevelInfo_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_BuildLevelInfo_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_explain_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_explain_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_explain_PolygonImage;
		[HideInInspector] public ListView m_sv_explain_ListView;

		[HideInInspector] public LanguageText m_lbl_explain_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_explain_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_explain_ContentSizeFitter;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_pl_content = FindUI<RectTransform>(vb.transform ,"Rect/pl_content");
			m_UI_Effect_BuildShow = new UI_Effect_BuildShow_SubView(FindUI<RectTransform>(vb.transform ,"Rect/pl_content/Left/UI_Effect_BuildShow"));
			m_img_buildImg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/Left/img_buildImg");

			m_pl_Right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right");
			m_pl_Right_Animator = FindUI<Animator>(vb.transform ,"Rect/pl_content/pl_Right");

			m_pl_info_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"Rect/pl_content/pl_Right/pl_info");

			m_lbl_hp_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/hp/lbl_hp");
			m_lbl_hp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/hp/lbl_hp");

			m_lbl_state_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/state/lbl_state");
			m_lbl_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/state/lbl_state");

			m_pl_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/pl_state");

			m_pb_bar_GameSlider = FindUI<GameSlider>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/pb_bar");
			m_pb_bar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/pb_bar");

			m_img_Fill_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/pb_bar/Fill Area/img_Fill");

			m_img_tips_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/img_tips");
			m_img_tips_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/img_tips");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/img_tips/img_arrowSideTop");
			m_img_arrowSideTop_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/img_tips/img_arrowSideTop");

			m_lbl_tiptext_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/img_tips/lbl_tiptext");
			m_lbl_tiptext_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/img_tips/lbl_tiptext");
			m_lbl_tiptext_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/img_tips/lbl_tiptext");

			m_btn_BuildLevelInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/btn_BuildLevelInfo");
			m_btn_BuildLevelInfo_GameButton = FindUI<GameButton>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/btn_BuildLevelInfo");
			m_btn_BuildLevelInfo_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/btn_BuildLevelInfo");
			m_btn_BuildLevelInfo_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/btn_BuildLevelInfo");

			m_img_BuildLevelInfo_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_info/btn_BuildLevelInfo/img_BuildLevelInfo");

			m_pl_explain_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_explain");

			m_sv_explain_ScrollRect = FindUI<ScrollRect>(vb.transform ,"Rect/pl_content/pl_Right/pl_explain/sv_explain");
			m_sv_explain_PolygonImage = FindUI<PolygonImage>(vb.transform ,"Rect/pl_content/pl_Right/pl_explain/sv_explain");
			m_sv_explain_ListView = FindUI<ListView>(vb.transform ,"Rect/pl_content/pl_Right/pl_explain/sv_explain");

			m_lbl_explain_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/pl_content/pl_Right/pl_explain/sv_explain/v/c/lbl_explain");
			m_lbl_explain_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/pl_content/pl_Right/pl_explain/sv_explain/v/c/lbl_explain");
			m_lbl_explain_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"Rect/pl_content/pl_Right/pl_explain/sv_explain/v/c/lbl_explain");


            UI_Win_TheTowerMediator mt = new UI_Win_TheTowerMediator(vb.gameObject);
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

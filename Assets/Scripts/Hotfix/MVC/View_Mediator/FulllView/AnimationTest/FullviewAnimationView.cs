// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月16日
// Update Time         :    2019年12月16日
// Class Description   :    FullviewAnimationView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class FullviewAnimationView : GameView
    {
        public const string VIEW_NAME = "UI_Fullview_Animation";

        public FullviewAnimationView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public Animator m_img_bg_Animator;

		[HideInInspector] public Mask m_pl_menumask_Mask;
		[HideInInspector] public Animator m_pl_menumask_Animator;

		[HideInInspector] public PolygonImage m_img_menubg_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_func1_PolygonImage;
		[HideInInspector] public GameButton m_btn_func1_GameButton;

		[HideInInspector] public PolygonImage m_btn_func2_PolygonImage;
		[HideInInspector] public GameButton m_btn_func2_GameButton;

		[HideInInspector] public PolygonImage m_btn_func3_PolygonImage;
		[HideInInspector] public GameButton m_btn_func3_GameButton;

		[HideInInspector] public PolygonImage m_btn_func4_PolygonImage;
		[HideInInspector] public GameButton m_btn_func4_GameButton;

		[HideInInspector] public PolygonImage m_btn_menu_PolygonImage;
		[HideInInspector] public GameButton m_btn_menu_GameButton;
		[HideInInspector] public BtnAnimation m_btn_menu_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_chatbg_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_world_PolygonImage;
		[HideInInspector] public GameButton m_btn_world_GameButton;

		[HideInInspector] public LanguageText m_lbl_chat1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_chat2_LanguageText;

		[HideInInspector] public LanguageText m_lbl_chat3_LanguageText;

		[HideInInspector] public LanguageText m_lbl_chat4_LanguageText;

		[HideInInspector] public Animator m_pl_task_Animator;

		[HideInInspector] public PolygonImage m_img_taskbg_PolygonImage;

		[HideInInspector] public CanvasGroup m_pl_taskcontent_CanvasGroup;

		[HideInInspector] public PolygonImage m_img_task1_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_task1_LanguageText;

		[HideInInspector] public PolygonImage m_img_task2_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_task2_LanguageText;

		[HideInInspector] public PolygonImage m_img_task3_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_task3_LanguageText;

		[HideInInspector] public PolygonImage m_btn_taskzoom_PolygonImage;
		[HideInInspector] public GameButton m_btn_taskzoom_GameButton;

		[HideInInspector] public PolygonImage m_btn_task_PolygonImage;
		[HideInInspector] public GameButton m_btn_task_GameButton;

		[HideInInspector] public RectTransform m_pl_queue;
		[HideInInspector] public PolygonImage m_img_queuebg_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_queuezoom_PolygonImage;
		[HideInInspector] public GameButton m_btn_queuezoom_GameButton;

		[HideInInspector] public PolygonImage m_img_head1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_head2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_head3_PolygonImage;



        private void UIFinder()
        {
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");
			m_img_bg_Animator = FindUI<Animator>(vb.transform ,"img_bg");

			m_pl_menumask_Mask = FindUI<Mask>(vb.transform ,"img_bg/pl_menumask");
			m_pl_menumask_Animator = FindUI<Animator>(vb.transform ,"img_bg/pl_menumask");

			m_img_menubg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_menumask/img_menubg");

			m_btn_func1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_menumask/img_menubg/btn_func1");
			m_btn_func1_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/pl_menumask/img_menubg/btn_func1");

			m_btn_func2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_menumask/img_menubg/btn_func2");
			m_btn_func2_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/pl_menumask/img_menubg/btn_func2");

			m_btn_func3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_menumask/img_menubg/btn_func3");
			m_btn_func3_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/pl_menumask/img_menubg/btn_func3");

			m_btn_func4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_menumask/img_menubg/btn_func4");
			m_btn_func4_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/pl_menumask/img_menubg/btn_func4");

			m_btn_menu_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/btn_menu");
			m_btn_menu_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/btn_menu");
			m_btn_menu_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"img_bg/btn_menu");

			m_img_chatbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_chatbg");

			m_btn_world_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_chatbg/btn_world");
			m_btn_world_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/img_chatbg/btn_world");

			m_lbl_chat1_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/img_chatbg/lbl_chat1");

			m_lbl_chat2_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/img_chatbg/lbl_chat2");

			m_lbl_chat3_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/img_chatbg/lbl_chat3");

			m_lbl_chat4_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/img_chatbg/lbl_chat4");

			m_pl_task_Animator = FindUI<Animator>(vb.transform ,"img_bg/pl_task");

			m_img_taskbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_task/img_taskbg");

			m_pl_taskcontent_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"img_bg/pl_task/img_taskbg/pl_taskcontent");

			m_img_task1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_task/img_taskbg/pl_taskcontent/img_task1");

			m_lbl_task1_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/pl_task/img_taskbg/pl_taskcontent/img_task1/lbl_task1");

			m_img_task2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_task/img_taskbg/pl_taskcontent/img_task2");

			m_lbl_task2_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/pl_task/img_taskbg/pl_taskcontent/img_task2/lbl_task2");

			m_img_task3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_task/img_taskbg/pl_taskcontent/img_task3");

			m_lbl_task3_LanguageText = FindUI<LanguageText>(vb.transform ,"img_bg/pl_task/img_taskbg/pl_taskcontent/img_task3/lbl_task3");

			m_btn_taskzoom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_task/btn_taskzoom");
			m_btn_taskzoom_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/pl_task/btn_taskzoom");

			m_btn_task_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_task/btn_task");
			m_btn_task_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/pl_task/btn_task");

			m_pl_queue = FindUI<RectTransform>(vb.transform ,"img_bg/pl_queue");
			m_img_queuebg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_queue/img_queuebg");

			m_btn_queuezoom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_queue/btn_queuezoom");
			m_btn_queuezoom_GameButton = FindUI<GameButton>(vb.transform ,"img_bg/pl_queue/btn_queuezoom");

			m_img_head1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_queue/img_task1/img_head1");

			m_img_head2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_queue/img_task2/img_head2");

			m_img_head3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/pl_queue/img_task3/img_head3");


            FullviewAnimationMediator mt = new FullviewAnimationMediator(vb.gameObject);
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

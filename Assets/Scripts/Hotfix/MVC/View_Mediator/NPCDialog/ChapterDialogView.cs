// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月20日
// Update Time         :    2020年3月20日
// Class Description   :    ChapterDialogView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class ChapterDialogView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ChapterDialog";

        public ChapterDialogView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public Animator m_pl_left_Animator;
		[HideInInspector] public CanvasGroup m_pl_left_CanvasGroup;

		[HideInInspector] public SkeletonGraphic m_spine_hero_left_SkeletonGraphic;

		[HideInInspector] public UI_Pop_TalkTip_SubView m_UI_Pop_TalkTip_left;
		[HideInInspector] public Animator m_pl_right_Animator;
		[HideInInspector] public CanvasGroup m_pl_right_CanvasGroup;

		[HideInInspector] public SkeletonGraphic m_spine_hero_right_SkeletonGraphic;

		[HideInInspector] public UI_Pop_TalkTip_SubView m_UI_Pop_TalkTip_right;
		[HideInInspector] public GameButton m_btn_noTextButton_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_noTextButton_Empty4Raycast;



        private void UIFinder()
        {
			m_pl_left_Animator = FindUI<Animator>(vb.transform ,"pl_left");
			m_pl_left_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_left");

			m_spine_hero_left_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"pl_left/spine_hero_left");

			m_UI_Pop_TalkTip_left = new UI_Pop_TalkTip_SubView(FindUI<RectTransform>(vb.transform ,"pl_left/UI_Pop_TalkTip_left"));
			m_pl_right_Animator = FindUI<Animator>(vb.transform ,"pl_right");
			m_pl_right_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_right");

			m_spine_hero_right_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"pl_right/spine_hero_right");

			m_UI_Pop_TalkTip_right = new UI_Pop_TalkTip_SubView(FindUI<RectTransform>(vb.transform ,"pl_right/UI_Pop_TalkTip_right"));
			m_btn_noTextButton_GameButton = FindUI<GameButton>(vb.transform ,"btn_noTextButton");
			m_btn_noTextButton_Empty4Raycast = FindUI<Empty4Raycast>(vb.transform ,"btn_noTextButton");


            ChapterDialogMediator mt = new ChapterDialogMediator(vb.gameObject);
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

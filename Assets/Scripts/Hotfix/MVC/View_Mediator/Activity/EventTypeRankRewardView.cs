// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, 26 October 2020
// Update Time         :    Monday, 26 October 2020
// Class Description   :    EventTypeRankRewardView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class EventTypeRankRewardView : GameView
    {
        public const string VIEW_NAME = "UI_Win_EventTypeRankReward";

        public EventTypeRankRewardView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type4_SubView m_UI_Model_Window_Type4;
		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public ToggleGroup m_pl_rankGroup_ToggleGroup;

		[HideInInspector] public GameToggle m_ck_total_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_total_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_total_LanguageText;

		[HideInInspector] public GameToggle m_ck_single_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_single_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_single_LanguageText;

		[HideInInspector] public RectTransform m_list_node;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;



        private void UIFinder()
        {
			m_UI_Model_Window_Type4 = new UI_Model_Window_Type4_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type4"));
			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_desc");

			m_pl_rankGroup_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"rect/pl_rankGroup");

			m_ck_total_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_rankGroup/ck_total");
			m_ck_total_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_rankGroup/ck_total");

			m_lbl_total_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_rankGroup/ck_total/lbl_total");

			m_ck_single_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/pl_rankGroup/ck_single");
			m_ck_single_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_rankGroup/ck_single");

			m_lbl_single_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_rankGroup/ck_single/lbl_single");

			m_list_node = FindUI<RectTransform>(vb.transform ,"rect/list_node");
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/list_node/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/list_node/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/list_node/sv_list");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"btn_close");


            EventTypeRankRewardMediator mt = new EventTypeRankRewardMediator(vb.gameObject);
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
            vb.onMenuBackCallback = mt.onMenuBackCallback;
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

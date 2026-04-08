// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月3日
// Update Time         :    2020年9月3日
// Class Description   :    UI_IF_ExpeditionFightWinView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_ExpeditionFightWinView : GameView
    {
        public const string VIEW_NAME = "UI_IF_ExpeditionFightWin";

        public UI_IF_ExpeditionFightWinView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_firsticon_PolygonImage;
		[HideInInspector] public LayoutElement m_img_firsticon_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_img_firsticon_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_starbg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_starbg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_starNormal1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starNormal2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starNormal3_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_star_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_star_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_starHighlight1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starHighlight2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_starHighlight3_PolygonImage;

		[HideInInspector] public UI_Item_ExpeditionFightTask_SubView m_UI_Item_ExpeditionFightTask;
		[HideInInspector] public GridLayoutGroup m_pl_reward_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reward_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_reward2_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reward2_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet4;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet5;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet6;
		[HideInInspector] public LanguageText m_lbl_rewardget2_LanguageText;
		[HideInInspector] public LayoutElement m_lbl_rewardget2_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_lbl_rewardget2_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_reward1_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_reward1_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_firstBg_PolygonImage;
		[HideInInspector] public LayoutElement m_img_firstBg_LayoutElement;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet3;
		[HideInInspector] public LanguageText m_lbl_rewardget1_LanguageText;
		[HideInInspector] public LayoutElement m_lbl_rewardget1_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_lbl_rewardget1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_haveget_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_spin_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_pl_spinGraphic_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_pl_spinGraphic_ArabLayoutCompment;

		[HideInInspector] public UI_Pop_TalkTip_SubView m_UI_Pop_TalkTip;
		[HideInInspector] public LanguageText m_lbl_spinName_LanguageText;

		[HideInInspector] public PolygonImage m_btn_ck_PolygonImage;
		[HideInInspector] public GameButton m_btn_ck_GameButton;



        private void UIFinder()
        {
			m_img_firsticon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_firsticon");
			m_img_firsticon_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/img_firsticon");
			m_img_firsticon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_firsticon");

			m_pl_starbg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_starbg");
			m_pl_starbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_starbg");

			m_img_starNormal1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_starbg/img_starNormal1");

			m_img_starNormal2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_starbg/img_starNormal2");

			m_img_starNormal3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_starbg/img_starNormal3");

			m_pl_star_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_star");
			m_pl_star_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_star");

			m_img_starHighlight1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_star/img_starHighlight1");

			m_img_starHighlight2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_star/img_starHighlight2");

			m_img_starHighlight3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_star/img_starHighlight3");

			m_UI_Item_ExpeditionFightTask = new UI_Item_ExpeditionFightTask_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Item_ExpeditionFightTask"));
			m_pl_reward_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_reward");
			m_pl_reward_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_reward");

			m_pl_reward2_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_reward/pl_reward2");
			m_pl_reward2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_reward/pl_reward2");

			m_UI_Model_RewardGet4 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_reward/pl_reward2/UI_Model_RewardGet4"));
			m_UI_Model_RewardGet5 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_reward/pl_reward2/UI_Model_RewardGet5"));
			m_UI_Model_RewardGet6 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_reward/pl_reward2/UI_Model_RewardGet6"));
			m_lbl_rewardget2_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_reward/pl_reward2/lbl_rewardget2");
			m_lbl_rewardget2_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_reward/pl_reward2/lbl_rewardget2");
			m_lbl_rewardget2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_reward/pl_reward2/lbl_rewardget2");

			m_pl_reward1_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_reward/pl_reward1");
			m_pl_reward1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_reward/pl_reward1");

			m_img_firstBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_reward/pl_reward1/img_firstBg");
			m_img_firstBg_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_reward/pl_reward1/img_firstBg");

			m_UI_Model_RewardGet1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_reward/pl_reward1/UI_Model_RewardGet1"));
			m_UI_Model_RewardGet2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_reward/pl_reward1/UI_Model_RewardGet2"));
			m_UI_Model_RewardGet3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_reward/pl_reward1/UI_Model_RewardGet3"));
			m_lbl_rewardget1_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_reward/pl_reward1/lbl_rewardget1");
			m_lbl_rewardget1_LayoutElement = FindUI<LayoutElement>(vb.transform ,"rect/pl_reward/pl_reward1/lbl_rewardget1");
			m_lbl_rewardget1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_reward/pl_reward1/lbl_rewardget1");

			m_lbl_haveget_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_haveget");

			m_pl_spin_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_spin");

			m_pl_spinGraphic_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"rect/pl_spin/pl_spinGraphic");
			m_pl_spinGraphic_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_spin/pl_spinGraphic");

			m_UI_Pop_TalkTip = new UI_Pop_TalkTip_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_spin/UI_Pop_TalkTip"));
			m_lbl_spinName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_spin/lbl_spinName");

			m_btn_ck_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_ck");
			m_btn_ck_GameButton = FindUI<GameButton>(vb.transform ,"btn_ck");


            UI_IF_ExpeditionFightWinMediator mt = new UI_IF_ExpeditionFightWinMediator(vb.gameObject);
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

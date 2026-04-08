// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_IF_CaptainStarUpSuccessView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_CaptainStarUpSuccessView : GameView
    {
        public const string VIEW_NAME = "UI_IF_CaptainStarUpSuccess";

        public UI_IF_CaptainStarUpSuccessView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_CaptainHead;
		[HideInInspector] public ArabLayoutCompment m_pl_stars_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_stars_GridLayoutGroup;

		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar1;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar2;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar3;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar4;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar5;
		[HideInInspector] public UI_Model_CaptainStar_SubView m_UI_Model_CaptainStar6;
		[HideInInspector] public LanguageText m_lbl_maxLevel_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_maxLevel_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_levelBefore_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_levelBefore_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_levelAfter_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_levelAfter_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_point;
		[HideInInspector] public LanguageText m_lbl_point_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_point_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_giftPoint_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_giftPoint_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_newskill;
		[HideInInspector] public GridLayoutGroup m_pl_skill_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_skill_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill;


        private void UIFinder()
        {
			m_UI_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_CaptainHead"));
			m_pl_stars_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_stars");
			m_pl_stars_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_stars");

			m_UI_Model_CaptainStar1 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_stars/UI_Model_CaptainStar1"));
			m_UI_Model_CaptainStar2 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_stars/UI_Model_CaptainStar2"));
			m_UI_Model_CaptainStar3 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_stars/UI_Model_CaptainStar3"));
			m_UI_Model_CaptainStar4 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_stars/UI_Model_CaptainStar4"));
			m_UI_Model_CaptainStar5 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_stars/UI_Model_CaptainStar5"));
			m_UI_Model_CaptainStar6 = new UI_Model_CaptainStar_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_stars/UI_Model_CaptainStar6"));
			m_lbl_maxLevel_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_maxLevel");
			m_lbl_maxLevel_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_maxLevel");

			m_lbl_levelBefore_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_levelBefore");
			m_lbl_levelBefore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_levelBefore");

			m_lbl_levelAfter_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_levelAfter");
			m_lbl_levelAfter_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_levelAfter");

			m_pl_point = FindUI<RectTransform>(vb.transform ,"rect/pl_point");
			m_lbl_point_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_point/lbl_point");
			m_lbl_point_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_point/lbl_point");

			m_lbl_giftPoint_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_point/lbl_giftPoint");
			m_lbl_giftPoint_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_point/lbl_giftPoint");

			m_pl_newskill = FindUI<RectTransform>(vb.transform ,"rect/pl_newskill");
			m_pl_skill_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_newskill/pl_skill");
			m_pl_skill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_newskill/pl_skill");

			m_UI_Item_CaptainSkill = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_newskill/pl_skill/UI_Item_CaptainSkill"));

            UI_IF_CaptainStarUpSuccessMediator mt = new UI_IF_CaptainStarUpSuccessMediator(vb.gameObject);
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

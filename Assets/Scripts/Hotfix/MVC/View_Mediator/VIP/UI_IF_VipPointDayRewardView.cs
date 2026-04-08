// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月26日
// Update Time         :    2020年8月26日
// Class Description   :    UI_IF_VipPointDayRewardView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_VipPointDayRewardView : GameView
    {
        public const string VIEW_NAME = "UI_IF_VipPointDayReward";

        public UI_IF_VipPointDayRewardView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_todayVal_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_todayVal_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_todayVal_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_loginDay_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_loginDay_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_loginDay_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_tomorrowVal_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_tomorrowVal_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_tomorrowVal_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_pl_close_PolygonImage;



        private void UIFinder()
        {
			m_lbl_todayVal_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_todayVal");
			m_lbl_todayVal_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/lbl_todayVal");
			m_lbl_todayVal_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_todayVal");

			m_lbl_loginDay_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_loginDay");
			m_lbl_loginDay_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/lbl_loginDay");
			m_lbl_loginDay_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_loginDay");

			m_lbl_tomorrowVal_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_tomorrowVal");
			m_lbl_tomorrowVal_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/lbl_tomorrowVal");
			m_lbl_tomorrowVal_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_tomorrowVal");

			m_pl_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_close");


            UI_IF_VipPointDayRewardMediator mt = new UI_IF_VipPointDayRewardMediator(vb.gameObject);
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

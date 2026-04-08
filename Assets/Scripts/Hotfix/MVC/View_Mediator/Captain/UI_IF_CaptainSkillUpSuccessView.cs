// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月28日
// Update Time         :    2020年4月28日
// Class Description   :    UI_IF_CaptainSkillUpSuccessView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_IF_CaptainSkillUpSuccessView : GameView
    {
        public const string VIEW_NAME = "UI_IF_CaptainSkillUpSuccess";

        public UI_IF_CaptainSkillUpSuccessView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_level_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill;
		[HideInInspector] public LanguageText m_lbl_levelBefore_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_levelBefore_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_levelAfter_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_levelAfter_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;



        private void UIFinder()
        {
			m_lbl_level_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_level");
			m_lbl_level_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_level");

			m_UI_Item_CaptainSkill = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Item_CaptainSkill"));
			m_lbl_levelBefore_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_levelBefore");
			m_lbl_levelBefore_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_levelBefore");

			m_lbl_levelAfter_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_levelAfter");
			m_lbl_levelAfter_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_levelAfter");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_text");


            UI_IF_CaptainSkillUpSuccessMediator mt = new UI_IF_CaptainSkillUpSuccessMediator(vb.gameObject);
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

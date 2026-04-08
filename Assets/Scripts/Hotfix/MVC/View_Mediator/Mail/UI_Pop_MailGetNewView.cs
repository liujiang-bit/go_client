// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月20日
// Update Time         :    2020年8月20日
// Class Description   :    UI_Pop_MailGetNewView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_MailGetNewView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_MailGetNew";

        public UI_Pop_MailGetNewView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_click_PolygonImage;
		[HideInInspector] public GameButton m_btn_click_GameButton;



        private void UIFinder()
        {
			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"offset/bg/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"offset/bg/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"offset/bg/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"offset/bg/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"offset/bg/lbl_desc");
			m_lbl_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"offset/bg/lbl_desc");

			m_img_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"offset/bg/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"offset/bg/img_arrow");

			m_btn_click_PolygonImage = FindUI<PolygonImage>(vb.transform ,"offset/bg/btn_click");
			m_btn_click_GameButton = FindUI<GameButton>(vb.transform ,"offset/bg/btn_click");


            UI_Pop_MailGetNewMediator mt = new UI_Pop_MailGetNewMediator(vb.gameObject);
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

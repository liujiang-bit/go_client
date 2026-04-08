// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月22日
// Update Time         :    2020年7月22日
// Class Description   :    UI_Win_MainTainView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_MainTainView : GameView
    {
        public const string VIEW_NAME = "UI_Win_MainTain";

        public UI_Win_MainTainView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_languageText_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_languageText_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_Tip_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_facebook;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_reflash;
		[HideInInspector] public PolygonImage m_btn_service_PolygonImage;
		[HideInInspector] public GameButton m_btn_service_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_service_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/sv_list/v/c/lbl_languageText");
			m_lbl_languageText_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"rect/sv_list/v/c/lbl_languageText");
			m_lbl_languageText_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_list/v/c/lbl_languageText");

			m_lbl_Tip_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/tip/lbl_Tip");

			m_UI_facebook = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/btns/UI_facebook"));
			m_UI_reflash = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/btns/UI_reflash"));
			m_btn_service_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_service");
			m_btn_service_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_service");
			m_btn_service_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/btn_service");


            UI_Win_MainTainMediator mt = new UI_Win_MainTainMediator(vb.gameObject);
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

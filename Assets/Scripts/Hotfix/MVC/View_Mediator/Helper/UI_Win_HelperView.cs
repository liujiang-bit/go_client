// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月21日
// Update Time         :    2020年7月21日
// Class Description   :    UI_Win_HelperView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_HelperView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Helper";

        public UI_Win_HelperView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public RectTransform m_pl_list;
		[HideInInspector] public PolygonImage m_btn_head_PolygonImage;
		[HideInInspector] public GameButton m_btn_head_GameButton;
		[HideInInspector] public BtnAnimation m_btn_head_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_head_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_service_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_service_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_char_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_helperlist_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_helperlist_PolygonImage;
		[HideInInspector] public ListView m_sv_helperlist_ListView;

		[HideInInspector] public RectTransform m_pl_detial;
		[HideInInspector] public LanguageText m_lbl_question_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_question_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public LanguageText m_lbl_detial_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_detial_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_detial_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_list = FindUI<RectTransform>(vb.transform ,"pl_list");
			m_btn_head_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_list/btn_head");
			m_btn_head_GameButton = FindUI<GameButton>(vb.transform ,"pl_list/btn_head");
			m_btn_head_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_list/btn_head");
			m_btn_head_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list/btn_head");

			m_lbl_service_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_list/lbl_service");
			m_lbl_service_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list/lbl_service");

			m_img_char_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_list/img_char");
			m_img_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_list/img_char");

			m_sv_helperlist_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_list/sv_helperlist");
			m_sv_helperlist_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_list/sv_helperlist");
			m_sv_helperlist_ListView = FindUI<ListView>(vb.transform ,"pl_list/sv_helperlist");

			m_pl_detial = FindUI<RectTransform>(vb.transform ,"pl_detial");
			m_lbl_question_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_detial/lbl_question");
			m_lbl_question_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_detial/lbl_question");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_detial/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_detial/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_detial/sv_list");

			m_lbl_detial_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_detial/sv_list/v/c/lbl_detial");
			m_lbl_detial_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_detial/sv_list/v/c/lbl_detial");
			m_lbl_detial_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_detial/sv_list/v/c/lbl_detial");


            UI_Win_HelperMediator mt = new UI_Win_HelperMediator(vb.gameObject);
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

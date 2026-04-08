// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月5日
// Update Time         :    2020年6月5日
// Class Description   :    UI_Win_MailContactListView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_MailContactView : GameView
    {
        public const string VIEW_NAME = "UI_Win_MailContact";

        public UI_Win_MailContactView() 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public LanguageText m_lbl_none_LanguageText;

		[HideInInspector] public RectTransform m_pl_list1;
		[HideInInspector] public ScrollRect m_sv_list1_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list1_PolygonImage;
		[HideInInspector] public ListView m_sv_list1_ListView;

		[HideInInspector] public RectMask2D m_v_list1_RectMask2D;

		[HideInInspector] public RectTransform m_c_list1;
		[HideInInspector] public RectTransform m_pl_list2;
		[HideInInspector] public ScrollRect m_sv_list2_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list2_PolygonImage;
		[HideInInspector] public ListView m_sv_list2_ListView;

		[HideInInspector] public RectMask2D m_v_list2_RectMask2D;

		[HideInInspector] public RectTransform m_c_list2;
		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_btn_sure;
		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_lbl_none_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_none");

			m_pl_list1 = FindUI<RectTransform>(vb.transform ,"rect/pl_list1");
			m_sv_list1_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_list1/sv_list1");
			m_sv_list1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_list1/sv_list1");
			m_sv_list1_ListView = FindUI<ListView>(vb.transform ,"rect/pl_list1/sv_list1");

			m_v_list1_RectMask2D = FindUI<RectMask2D>(vb.transform ,"rect/pl_list1/sv_list1/v_list1");

			m_c_list1 = FindUI<RectTransform>(vb.transform ,"rect/pl_list1/sv_list1/v_list1/c_list1");
			m_pl_list2 = FindUI<RectTransform>(vb.transform ,"rect/pl_list2");
			m_sv_list2_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_list2/sv_list2");
			m_sv_list2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_list2/sv_list2");
			m_sv_list2_ListView = FindUI<ListView>(vb.transform ,"rect/pl_list2/sv_list2");

			m_v_list2_RectMask2D = FindUI<RectMask2D>(vb.transform ,"rect/pl_list2/sv_list2/v_list2");

			m_c_list2 = FindUI<RectTransform>(vb.transform ,"rect/pl_list2/sv_list2/v_list2/c_list2");
			m_btn_sure = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_sure"));
			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_close");


            UI_Win_MailContactMediator mt = new UI_Win_MailContactMediator(vb.gameObject);
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

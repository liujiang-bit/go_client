// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月27日
// Update Time         :    2020年4月27日
// Class Description   :    UI_Win_PlayerHeadPicView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_PlayerHeadPicView : GameView
    {
        public const string VIEW_NAME = "UI_Win_PlayerHeadPic";

        public UI_Win_PlayerHeadPicView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public UI_Model_SideBtn_SubView m_UI_Common_SideBtn;
		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_head_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_head_PolygonImage;
		[HideInInspector] public ListView m_sv_head_ListView;

		[HideInInspector] public ScrollRect m_sv_headframe_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_headframe_PolygonImage;
		[HideInInspector] public ListView m_sv_headframe_ListView;

		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_img_rank_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_rank_LanguageText;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_UI_Model_StandardButton_Blue;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_UI_Common_SideBtn = new UI_Model_SideBtn_SubView(FindUI<RectTransform>(vb.transform ,"UI_Common_SideBtn"));
			m_lbl_count_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/left/lbl_count");
			m_lbl_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/left/lbl_count");

			m_sv_head_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/left/sv_head");
			m_sv_head_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/sv_head");
			m_sv_head_ListView = FindUI<ListView>(vb.transform ,"rect/left/sv_head");

			m_sv_headframe_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/left/sv_headframe");
			m_sv_headframe_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/left/sv_headframe");
			m_sv_headframe_ListView = FindUI<ListView>(vb.transform ,"rect/left/sv_headframe");

			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_right");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/UI_Model_PlayerHead"));
			m_img_rank_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_right/img_rank");

			m_lbl_rank_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/lbl_rank");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_right/lbl_desc");

			m_UI_Model_StandardButton_Blue = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_right/UI_Model_StandardButton_Blue"));

            UI_Win_PlayerHeadPicMediator mt = new UI_Win_PlayerHeadPicMediator(vb.gameObject);
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

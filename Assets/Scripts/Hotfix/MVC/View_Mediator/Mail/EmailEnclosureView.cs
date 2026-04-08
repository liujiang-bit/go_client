// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月3日
// Update Time         :    2020年3月3日
// Class Description   :    EmailEnclosureView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class EmailEnclosureView : GameView
    {
        public const string VIEW_NAME = "UI_Win_EmailEnclosure";

        public EmailEnclosureView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeSmall_SubView m_UI_Model_Window_TypeSmall;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_pl_btn;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeSmall = new UI_Model_Window_TypeSmall_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeSmall"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"img_bg/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"img_bg/sv_list_view");

			m_pl_btn = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"img_bg/pl_btn"));

            EmailEnclosureMediator mt = new EmailEnclosureMediator(vb.gameObject);
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

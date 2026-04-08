// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 30, 2020
// Update Time         :    Thursday, April 30, 2020
// Class Description   :    UI_Win_GuildPurviewView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildPurviewView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildPurview";

        public UI_Win_GuildPurviewView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public LanguageText m_lbl_titleName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_titleName_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_lbl_titleName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect0/frame/lbl_titleName");
			m_lbl_titleName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect0/frame/lbl_titleName");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect0/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect0/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect0/sv_list");


            UI_Win_GuildPurviewMediator mt = new UI_Win_GuildPurviewMediator(vb.gameObject);
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

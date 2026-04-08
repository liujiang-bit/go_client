// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月27日
// Update Time         :    2020年8月27日
// Class Description   :    UI_Win_GuildBuildView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildBuildView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildBuild";

        public UI_Win_GuildBuildView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public UI_Model_Reinforce_SubView m_UI_Model_Reinforce;
		[HideInInspector] public ScrollRect m_sv_all_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_all_PolygonImage;
		[HideInInspector] public ListView m_sv_all_ListView;

		[HideInInspector] public PolygonImage m_v_all_PolygonImage;
		[HideInInspector] public Mask m_v_all_Mask;

		[HideInInspector] public RectTransform m_c_all;
		[HideInInspector] public LanguageText m_lbl_notroops_LanguageText;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_UI_Model_Reinforce = new UI_Model_Reinforce_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_Reinforce"));
			m_sv_all_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_all");
			m_sv_all_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_all");
			m_sv_all_ListView = FindUI<ListView>(vb.transform ,"rect/sv_all");

			m_v_all_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_all/v_all");
			m_v_all_Mask = FindUI<Mask>(vb.transform ,"rect/sv_all/v_all");

			m_c_all = FindUI<RectTransform>(vb.transform ,"rect/sv_all/v_all/c_all");
			m_lbl_notroops_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_notroops");


            UI_Win_GuildBuildMediator mt = new UI_Win_GuildBuildMediator(vb.gameObject);
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

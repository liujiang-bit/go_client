// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    UI_Win_PlayerNewCharView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_PlayerNewCharView : GameView
    {
        public const string VIEW_NAME = "UI_Win_PlayerNewChar";

        public UI_Win_PlayerNewCharView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type3;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public RectTransform m_pl_top;
		[HideInInspector] public LanguageText m_lbl_kingdomName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomName_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_kingdomName_ContentSizeFitter;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type3 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type3"));
			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_pl_top = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_top");
			m_lbl_kingdomName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_top/lbl_kingdomName");
			m_lbl_kingdomName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_top/lbl_kingdomName");
			m_lbl_kingdomName_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_mes/pl_top/lbl_kingdomName");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_mes/sv_list");


            UI_Win_PlayerNewCharMediator mt = new UI_Win_PlayerNewCharMediator(vb.gameObject);
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

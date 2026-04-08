// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月7日
// Update Time         :    2020年8月7日
// Class Description   :    UI_Win_WorkerView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_WorkerView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Worker";

        public UI_Win_WorkerView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public LanguageText m_lbl_TextArea_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_TextArea_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_TextArea_ContentSizeFitter;

		[HideInInspector] public UI_Item_Worker_SubView m_UI_Item_Worker1;
		[HideInInspector] public UI_Item_Worker_SubView m_UI_Item_Worker2;


        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_lbl_TextArea_LanguageText = FindUI<LanguageText>(vb.transform ,"Rect/lbl_TextArea");
			m_lbl_TextArea_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"Rect/lbl_TextArea");
			m_lbl_TextArea_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"Rect/lbl_TextArea");

			m_UI_Item_Worker1 = new UI_Item_Worker_SubView(FindUI<RectTransform>(vb.transform ,"Rect/workers/UI_Item_Worker1"));
			m_UI_Item_Worker2 = new UI_Item_Worker_SubView(FindUI<RectTransform>(vb.transform ,"Rect/workers/UI_Item_Worker2"));

            UI_Win_WorkerMediator mt = new UI_Win_WorkerMediator(vb.gameObject);
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

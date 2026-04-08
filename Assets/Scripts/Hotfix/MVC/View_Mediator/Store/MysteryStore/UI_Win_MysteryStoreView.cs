// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月2日
// Update Time         :    2020年6月2日
// Class Description   :    UI_Win_MysteryStoreView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_MysteryStoreView : GameView
    {
        public const string VIEW_NAME = "UI_Win_MysteryStore";

        public UI_Win_MysteryStoreView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public LanguageText m_lbl_lastTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lastTime_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_times_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_times_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_btn_Free;
		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_refresh;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_pl_char_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_pl_char_ArabLayoutCompment;



        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_lbl_lastTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/Top/lbl_lastTime");
			m_lbl_lastTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/Top/lbl_lastTime");

			m_lbl_times_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/Top/lbl_times");
			m_lbl_times_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/Top/lbl_times");

			m_btn_Free = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"rect/Top/btn_Free"));
			m_btn_refresh = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"rect/Top/btn_refresh"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");
			m_sv_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/sv_list");

			m_pl_char_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"rect/pl_char");
			m_pl_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_char");


            UI_Win_MysteryStoreMediator mt = new UI_Win_MysteryStoreMediator(vb.gameObject);
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

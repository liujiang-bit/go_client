// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月15日
// Update Time         :    2020年4月15日
// Class Description   :    TavernBoxDescView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class TavernBoxDescView : GameView
    {
        public const string VIEW_NAME = "UI_Win_TavernBoxDesc";

        public TavernBoxDescView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public PolygonImage m_img_title_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_lbl2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lbl2_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_lbl1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_lbl1_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_img_title_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_title");

			m_lbl_lbl2_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_title/lbl_lbl2");
			m_lbl_lbl2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_title/lbl_lbl2");

			m_lbl_lbl1_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_title/lbl_lbl1");
			m_lbl_lbl1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_title/lbl_lbl1");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");


            TavernBoxDescMediator mt = new TavernBoxDescMediator(vb.gameObject);
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

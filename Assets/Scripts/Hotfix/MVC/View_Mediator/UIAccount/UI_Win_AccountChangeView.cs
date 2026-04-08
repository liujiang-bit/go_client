// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月12日
// Update Time         :    2020年8月12日
// Class Description   :    UI_Win_AccountChangeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_AccountChangeView : GameView
    {
        public const string VIEW_NAME = "UI_Win_AccountChange";

        public UI_Win_AccountChangeView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public PolygonImage m_img_accountIcon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_accountID_LanguageText;

		[HideInInspector] public PolygonImage m_img_logIcon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_logName_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_view_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_view_ArabLayoutCompment;

		[HideInInspector] public UI_Model_ImgBtn_Blue_SubView m_UI_Igg;
		[HideInInspector] public UI_Model_ImgBtn_Blue_SubView m_UI_Machine;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_img_accountIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_accountIcon");

			m_lbl_accountID_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_accountIcon/lbl_accountID");

			m_img_logIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_logIcon");

			m_lbl_logName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/img_logIcon/lbl_logName");

			m_pl_view_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_view");
			m_pl_view_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_view");

			m_UI_Igg = new UI_Model_ImgBtn_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_view/UI_Igg"));
			m_UI_Machine = new UI_Model_ImgBtn_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_view/UI_Machine"));

            UI_Win_AccountChangeMediator mt = new UI_Win_AccountChangeMediator(vb.gameObject);
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

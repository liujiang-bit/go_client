// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    UI_Win_PlayerChangeSureView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_PlayerChangeSureView : GameView
    {
        public const string VIEW_NAME = "UI_Win_PlayerChangeSure";

        public UI_Win_PlayerChangeSureView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public RectTransform m_pl_view1;
		[HideInInspector] public LanguageText m_lbl_kingdomNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomNum_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kingdomName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomName_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_view2;
		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerName_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public UI_Model_DoubleLineButton_Red2_SubView m_btn_cancel;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_sure;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_pl_view1 = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_view1");
			m_lbl_kingdomNum_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_view1/lbl_kingdomNum");
			m_lbl_kingdomNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_view1/lbl_kingdomNum");

			m_lbl_kingdomName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_view1/lbl_kingdomName");
			m_lbl_kingdomName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_view1/lbl_kingdomName");

			m_pl_view2 = FindUI<RectTransform>(vb.transform ,"pl_mes/pl_view2");
			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/pl_view2/lbl_playerName");
			m_lbl_playerName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/pl_view2/lbl_playerName");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/pl_view2/UI_Model_PlayerHead"));
			m_btn_cancel = new UI_Model_DoubleLineButton_Red2_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/btn_cancel"));
			m_btn_sure = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/btn_sure"));

            UI_Win_PlayerChangeSureMediator mt = new UI_Win_PlayerChangeSureMediator(vb.gameObject);
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

// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月15日
// Update Time         :    2020年1月15日
// Class Description   :    ResShortView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class ResShortView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ResShort";

        public ResShortView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public LanguageText m_lbl_Tip_LanguageText;

		[HideInInspector] public UI_Item_ShowResShort_SubView m_UI_Item_Food;
		[HideInInspector] public UI_Item_ShowResShort_SubView m_UI_Item_Stone;
		[HideInInspector] public UI_Item_ShowResShort_SubView m_UI_Item_Wood;
		[HideInInspector] public UI_Item_ShowResShort_SubView m_UI_Item_Gold;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_Model_btn;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_lbl_Tip_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_Tip");

			m_UI_Item_Food = new UI_Item_ShowResShort_SubView(FindUI<RectTransform>(vb.transform ,"rect/bg/UI_Item_Food"));
			m_UI_Item_Stone = new UI_Item_ShowResShort_SubView(FindUI<RectTransform>(vb.transform ,"rect/bg/UI_Item_Stone"));
			m_UI_Item_Wood = new UI_Item_ShowResShort_SubView(FindUI<RectTransform>(vb.transform ,"rect/bg/UI_Item_Wood"));
			m_UI_Item_Gold = new UI_Item_ShowResShort_SubView(FindUI<RectTransform>(vb.transform ,"rect/bg/UI_Item_Gold"));
			m_UI_Model_btn = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_btn"));

            ResShortMediator mt = new ResShortMediator(vb.gameObject);
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

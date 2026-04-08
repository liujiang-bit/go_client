// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月19日
// Update Time         :    2020年5月19日
// Class Description   :    UI_Win_ItemCollectionView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ItemCollectionView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ItemCollection";

        public UI_Win_ItemCollectionView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public LanguageText m_lbl_tips_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_items_GridLayoutGroup;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardGet;
		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_UI_BtnSure;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_lbl_tips_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_tips");

			m_pl_items_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_items");

			m_UI_Model_RewardGet = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_items/UI_Model_RewardGet"));
			m_UI_BtnSure = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_BtnSure"));

            UI_Win_ItemCollectionMediator mt = new UI_Win_ItemCollectionMediator(vb.gameObject);
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

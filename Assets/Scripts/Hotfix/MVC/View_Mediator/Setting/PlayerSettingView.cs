// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年8月20日
// Update Time         :    2020年8月20日
// Class Description   :    PlayerSettingView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class PlayerSettingView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Setting";

        public PlayerSettingView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public LanguageText m_lbl_version_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_version_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_date_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_date_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn1;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn2;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn3;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn4;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn5;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn6;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn7;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn8;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn9;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn10;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataBtn12;


        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_lbl_version_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_version");
			m_lbl_version_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_version");

			m_lbl_date_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_date");
			m_lbl_date_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_date");

			m_UI_Model_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_Link"));
			m_UI_Item_PlayerDataBtn = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn"));
			m_UI_Item_PlayerDataBtn1 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn1"));
			m_UI_Item_PlayerDataBtn2 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn2"));
			m_UI_Item_PlayerDataBtn3 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn3"));
			m_UI_Item_PlayerDataBtn4 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn4"));
			m_UI_Item_PlayerDataBtn5 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn5"));
			m_UI_Item_PlayerDataBtn6 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn6"));
			m_UI_Item_PlayerDataBtn7 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn7"));
			m_UI_Item_PlayerDataBtn8 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn8"));
			m_UI_Item_PlayerDataBtn9 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn9"));
			m_UI_Item_PlayerDataBtn10 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn10"));
			m_UI_Item_PlayerDataBtn12 = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/GameObject/UI_Item_PlayerDataBtn12"));

            PlayerSettingMediator mt = new PlayerSettingMediator(vb.gameObject);
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

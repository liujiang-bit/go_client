// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, 28 September 2020
// Update Time         :    Monday, 28 September 2020
// Class Description   :    UI_Win_AccountMgrView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_AccountMgrView : GameView
    {
        public const string VIEW_NAME = "UI_Win_AccountMgr";

        public UI_Win_AccountMgrView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public UI_Model_StandardButton_Yellow_big_SubView m_UI_Change;
		[HideInInspector] public PolygonImage m_img_accountIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_accountIcon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_accountID_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_accountID_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_logIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_logIcon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_logName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_logName_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_SubView m_btn_register;
		[HideInInspector] public PolygonImage m_img_safeLevelBg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_safeLevelBg_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_safeLevel_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_safeLevel_ArabLayoutCompment;

		[HideInInspector] public UI_Item_accountMgrBinding_SubView m_UI_BindingMachine;
		[HideInInspector] public UI_Item_accountMgrBinding_SubView m_UI_BindingIGG;


        private void UIFinder()
        {
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_UI_Change = new UI_Model_StandardButton_Yellow_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Change"));
			m_img_accountIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/id/img_accountIcon");
			m_img_accountIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/id/img_accountIcon");

			m_lbl_accountID_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/id/img_accountIcon/lbl_accountID");
			m_lbl_accountID_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/id/img_accountIcon/lbl_accountID");

			m_img_logIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/id/img_logIcon");
			m_img_logIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/id/img_logIcon");

			m_lbl_logName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/id/img_logIcon/lbl_logName");
			m_lbl_logName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/id/img_logIcon/lbl_logName");

			m_btn_register = new UI_Model_StandardButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"rect/id/btn_register"));
			m_img_safeLevelBg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/binding/safe/img_safeLevelBg");
			m_img_safeLevelBg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/binding/safe/img_safeLevelBg");

			m_lbl_safeLevel_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/binding/safe/lbl_safeLevel");
			m_lbl_safeLevel_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/binding/safe/lbl_safeLevel");

			m_UI_BindingMachine = new UI_Item_accountMgrBinding_SubView(FindUI<RectTransform>(vb.transform ,"rect/binding/UI_BindingMachine"));
			m_UI_BindingIGG = new UI_Item_accountMgrBinding_SubView(FindUI<RectTransform>(vb.transform ,"rect/binding/UI_BindingIGG"));

            UI_Win_AccountMgrMediator mt = new UI_Win_AccountMgrMediator(vb.gameObject);
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

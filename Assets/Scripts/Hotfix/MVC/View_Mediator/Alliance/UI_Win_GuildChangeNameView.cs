// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Sunday, April 26, 2020
// Update Time         :    Sunday, April 26, 2020
// Class Description   :    UI_Win_GuildChangeNameView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GuildChangeNameView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GuildChangeName";

        public UI_Win_GuildChangeNameView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public LanguageText m_lbl_textCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_textCount_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_error_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_error_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_des_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_des_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_name_PolygonImage;
		[HideInInspector] public GameInput m_ipt_name_GameInput;

		[HideInInspector] public PolygonImage m_img_greenMark_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_greenMark_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_redMark_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_redMark_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_btn_change;
		[HideInInspector] public UI_Model_StandardButton_Blue_sure_SubView m_UI_BlueSure;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_lbl_textCount_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_textCount");
			m_lbl_textCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_textCount");

			m_lbl_error_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_error");
			m_lbl_error_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_error");

			m_lbl_des_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_des");
			m_lbl_des_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_des");

			m_ipt_name_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/ipt_name");
			m_ipt_name_GameInput = FindUI<GameInput>(vb.transform ,"rect/ipt_name");

			m_img_greenMark_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/ipt_name/img_greenMark");
			m_img_greenMark_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/ipt_name/img_greenMark");

			m_img_redMark_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/ipt_name/img_redMark");
			m_img_redMark_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/ipt_name/img_redMark");

			m_btn_change = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_change"));
			m_UI_BlueSure = new UI_Model_StandardButton_Blue_sure_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_BlueSure"));

            UI_Win_GuildChangeNameMediator mt = new UI_Win_GuildChangeNameMediator(vb.gameObject);
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

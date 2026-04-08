// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月13日
// Update Time         :    2020年4月13日
// Class Description   :    ChangeNameView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class ChangeNameView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ChangeName";

        public ChangeNameView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public PolygonImage m_ipt_name_PolygonImage;
		[HideInInspector] public GameInput m_ipt_name_GameInput;

		[HideInInspector] public LanguageText m_lbl_des_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_des_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue_SubView m_btn_item;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_btn_cur;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_ipt_name_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ipt_name");
			m_ipt_name_GameInput = FindUI<GameInput>(vb.transform ,"ipt_name");

			m_lbl_des_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_des");
			m_lbl_des_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_des");

			m_btn_item = new UI_Model_DoubleLineButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"btn_item"));
			m_btn_cur = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"btn_cur"));

            ChangeNameMediator mt = new ChangeNameMediator(vb.gameObject);
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

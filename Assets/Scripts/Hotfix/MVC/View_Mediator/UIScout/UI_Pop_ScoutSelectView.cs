// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月13日
// Update Time         :    2020年5月13日
// Class Description   :    UI_Pop_ScoutSelectView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_ScoutSelectView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_ScoutSelect";

        public UI_Pop_ScoutSelectView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public UI_Item_ScoutQueueHead_SubView m_UI_Item_ScoutQueueHead1;
		[HideInInspector] public UI_Item_ScoutQueueHead_SubView m_UI_Item_ScoutQueueHead2;
		[HideInInspector] public UI_Item_ScoutQueueHead_SubView m_UI_Item_ScoutQueueHead3;
		[HideInInspector] public Animator m_pl_Tip_Animator;
		[HideInInspector] public UIDefaultValue m_pl_Tip_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_mist_PolygonImage;
		[HideInInspector] public GridLayoutGroup m_img_mist_GridLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_state_LanguageText;

		[HideInInspector] public LanguageText m_lbl_state_val_LanguageText;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_UI_Model_StandardButton_Yellow;
		[HideInInspector] public UI_Tag_PopAnime_QueueShow_SubView m_UI_Tag_PopAnime_QueueShow;


        private void UIFinder()
        {
			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_languageText");

			m_UI_Item_ScoutQueueHead1 = new UI_Item_ScoutQueueHead_SubView(FindUI<RectTransform>(vb.transform ,"bg/queueHead/UI_Item_ScoutQueueHead1"));
			m_UI_Item_ScoutQueueHead2 = new UI_Item_ScoutQueueHead_SubView(FindUI<RectTransform>(vb.transform ,"bg/queueHead/UI_Item_ScoutQueueHead2"));
			m_UI_Item_ScoutQueueHead3 = new UI_Item_ScoutQueueHead_SubView(FindUI<RectTransform>(vb.transform ,"bg/queueHead/UI_Item_ScoutQueueHead3"));
			m_pl_Tip_Animator = FindUI<Animator>(vb.transform ,"pl_Tip");
			m_pl_Tip_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_Tip");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Tip/img_bg");

			m_img_mist_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Tip/img_bg/img_mist");
			m_img_mist_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_Tip/img_bg/img_mist");

			m_lbl_state_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Tip/img_bg/img_mist/lbl_state");

			m_lbl_state_val_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Tip/img_bg/img_mist/lbl_state_val");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_Tip/img_bg/lbl_time");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_Tip/img_bg/img_arrowSideR");

			m_UI_Model_StandardButton_Yellow = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(vb.transform ,"pl_Tip/img_bg/UI_Model_StandardButton_Yellow"));
			m_UI_Tag_PopAnime_QueueShow = new UI_Tag_PopAnime_QueueShow_SubView(FindUI<RectTransform>(vb.transform ,"pl_Tip/UI_Tag_PopAnime_QueueShow"));

            UI_Pop_ScoutSelectMediator mt = new UI_Pop_ScoutSelectMediator(vb.gameObject);
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

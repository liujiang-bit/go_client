// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, 20 October 2020
// Update Time         :    Tuesday, 20 October 2020
// Class Description   :    UI_Pop_EventTurntableRewardsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Pop_EventTurntableRewardsView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_EventTurntableRewards";

        public UI_Pop_EventTurntableRewardsView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_pl_pos_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_bg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrowSideTop_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;

		[HideInInspector] public ArabLayoutCompment m_pl_box_ArabLayoutCompment;

		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_img_box1;
		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_img_box2;
		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_img_box3;
		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_img_box4;
		[HideInInspector] public UI_Item_EventTribeKingBox_SubView m_img_box5;
		[HideInInspector] public UI_Tip_BoxReward_SubView m_UI_Tip_BoxReward;


        private void UIFinder()
        {
			m_pl_pos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg");
			m_img_bg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideTop");
			m_img_arrowSideTop_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/img_arrowSideL");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/img_bg/rect/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"pl_pos/img_bg/rect/btn_close");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_pos/img_bg/rect/pb_rogressBar");

			m_pl_box_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_pos/img_bg/rect/pl_box");

			m_img_box1 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_box/img_box1"));
			m_img_box2 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_box/img_box2"));
			m_img_box3 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_box/img_box3"));
			m_img_box4 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_box/img_box4"));
			m_img_box5 = new UI_Item_EventTribeKingBox_SubView(FindUI<RectTransform>(vb.transform ,"pl_pos/img_bg/rect/pl_box/img_box5"));
			m_UI_Tip_BoxReward = new UI_Tip_BoxReward_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tip_BoxReward"));

            UI_Pop_EventTurntableRewardsMediator mt = new UI_Pop_EventTurntableRewardsMediator(vb.gameObject);
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

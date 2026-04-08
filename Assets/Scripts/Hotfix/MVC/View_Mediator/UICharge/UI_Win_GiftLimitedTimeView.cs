// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月22日
// Update Time         :    2020年6月22日
// Class Description   :    UI_Win_GiftLimitedTimeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_GiftLimitedTimeView : GameView
    {
        public const string VIEW_NAME = "UI_Win_GiftLimitedTime";

        public UI_Win_GiftLimitedTimeView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_picture_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Yellow_big_SubView m_btn_buy;
		[HideInInspector] public HorizontalLayoutGroup m_pl_mes_HorizontalLayoutGroup;

		[HideInInspector] public HorizontalLayoutGroup m_pl_item_HorizontalLayoutGroup;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public RectTransform m_pl_firstcharge;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Model_Resources;


        private void UIFinder()
        {
			m_img_picture_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/img_picture");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"bg/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"bg/btn_close");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_title");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"bg/lbl_desc");

			m_btn_buy = new UI_Model_StandardButton_Yellow_big_SubView(FindUI<RectTransform>(vb.transform ,"bg/btn_buy"));
			m_pl_mes_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"bg/pl_mes");

			m_pl_item_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"bg/pl_mes/pl_item");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"bg/pl_mes/pl_item/UI_Model_Item"));
			m_pl_firstcharge = FindUI<RectTransform>(vb.transform ,"bg/pl_mes/pl_firstcharge");
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"bg/pl_mes/pl_firstcharge/UI_Model_CaptainHead"));
			m_UI_Model_Resources = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Resources"));

            UI_Win_GiftLimitedTimeMediator mt = new UI_Win_GiftLimitedTimeMediator(vb.gameObject);
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

// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月22日
// Update Time         :    2020年7月22日
// Class Description   :    BagGiftOpenView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class BagGiftOpenView : GameView
    {
        public const string VIEW_NAME = "UI_Win_BagGiftOpen";

        public BagGiftOpenView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public RectTransform m_pl_chose;
		[HideInInspector] public GridLayoutGroup m_pl_itemsInChose_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_itemsInChose_ArabLayoutCompment;

		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardSelect1;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardSelect2;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardSelect3;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardSelect4;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardSelect5;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_Model_RewardSelect6;
		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_ChoseSure;
		[HideInInspector] public LanguageText m_lbl_ChoseCount_LanguageText;

		[HideInInspector] public RectTransform m_pl_exchange;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_itemBefore;
		[HideInInspector] public UI_Model_RewardGet_SubView m_UI_itemAfter;
		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_exchangeSure;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_pl_chose = FindUI<RectTransform>(vb.transform ,"rect/pl_chose");
			m_pl_itemsInChose_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_chose/pl_itemsInChose");
			m_pl_itemsInChose_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_chose/pl_itemsInChose");

			m_UI_Model_RewardSelect1 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_chose/pl_itemsInChose/UI_Model_RewardSelect1"));
			m_UI_Model_RewardSelect2 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_chose/pl_itemsInChose/UI_Model_RewardSelect2"));
			m_UI_Model_RewardSelect3 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_chose/pl_itemsInChose/UI_Model_RewardSelect3"));
			m_UI_Model_RewardSelect4 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_chose/pl_itemsInChose/UI_Model_RewardSelect4"));
			m_UI_Model_RewardSelect5 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_chose/pl_itemsInChose/UI_Model_RewardSelect5"));
			m_UI_Model_RewardSelect6 = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_chose/pl_itemsInChose/UI_Model_RewardSelect6"));
			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_chose/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_chose/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/pl_chose/sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_chose/sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"rect/pl_chose/sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"rect/pl_chose/sv_list_view/v_list_view/c_list_view");
			m_UI_ChoseSure = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_chose/UI_ChoseSure"));
			m_lbl_ChoseCount_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_chose/lbl_ChoseCount");

			m_pl_exchange = FindUI<RectTransform>(vb.transform ,"rect/pl_exchange");
			m_UI_itemBefore = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_exchange/UI_itemBefore"));
			m_UI_itemAfter = new UI_Model_RewardGet_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_exchange/UI_itemAfter"));
			m_img_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_exchange/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_exchange/img_arrow");

			m_UI_exchangeSure = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_exchange/UI_exchangeSure"));

            BagGiftOpenMediator mt = new BagGiftOpenMediator(vb.gameObject);
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

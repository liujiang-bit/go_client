// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年10月19日
// Update Time         :    2020年10月19日
// Class Description   :    TavernSummonView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class TavernSummonView : GameView
    {
        public const string VIEW_NAME = "UI_IF_TavernSummon";

        public TavernSummonView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Interface_SubView m_UI_Model_Interface;
		[HideInInspector] public GridLayoutGroup m_pl_ress_GridLayoutGroup;
		[HideInInspector] public UIDefaultValue m_pl_ress_UIDefaultValue;

		[HideInInspector] public UI_Model_Resources_SubView m_UI_Dia;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Gold;
		[HideInInspector] public UI_Model_Resources_SubView m_UI_Sil;
		[HideInInspector] public RectTransform m_pl_box;
		[HideInInspector] public RectTransform m_pl_woodbox;
		[HideInInspector] public Animator m_pl_woodbox_ani_Animator;

		[HideInInspector] public PolygonImage m_btn_woodbox_PolygonImage;
		[HideInInspector] public GameButton m_btn_woodbox_GameButton;

		[HideInInspector] public UI_Model_HallBox_SubView m_UI_woodbox;
		[HideInInspector] public RectTransform m_pl_woodbox_standby_effect;
		[HideInInspector] public UI_10082_1_SubView m_UI_10082_1;
		[HideInInspector] public RectTransform m_pl_woodbox_content;
		[HideInInspector] public LanguageText m_lbl_woodbox_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_wooddesc_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_woodbox_btns_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_woodbox_btns_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_OpenAll_woodbox;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_Open_woodbox;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_Free_woodbox;
		[HideInInspector] public RectTransform m_pl_goldbox;
		[HideInInspector] public Animator m_pl_goldbox_ani_Animator;

		[HideInInspector] public PolygonImage m_btn_goldbox_PolygonImage;
		[HideInInspector] public GameButton m_btn_goldbox_GameButton;

		[HideInInspector] public UI_Model_HallBox_SubView m_UI_goldbox;
		[HideInInspector] public RectTransform m_pl_goldbox_standby_effect;
		[HideInInspector] public UI_10082_2_SubView m_UI_10082_2;
		[HideInInspector] public RectTransform m_pl_goldbox_content;
		[HideInInspector] public LanguageText m_lbl_goldbox_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_golddesc_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_goldbox_btns_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_goldbox_btns_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_OpenAll_goldbox;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_Open_goldbox;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_Free_goldbox;
		[HideInInspector] public PolygonImage m_btn_showall_PolygonImage;
		[HideInInspector] public GameButton m_btn_showall_GameButton;

		[HideInInspector] public LanguageText m_lbl_showall_LanguageText;

		[HideInInspector] public PolygonImage m_btn_rewardMask_PolygonImage;
		[HideInInspector] public GameButton m_btn_rewardMask_GameButton;

		[HideInInspector] public Animator m_pl_showReward_Animator;
		[HideInInspector] public CanvasGroup m_pl_showReward_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_pl_resultShow_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_resul_btns_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_resul_btns_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_Free;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_Open;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_OpenAll;
		[HideInInspector] public UI_Item_TavernSingleReward_SubView m_UI_Item_TavernSingleReward1;
		[HideInInspector] public UI_Item_TavernSingleReward_SubView m_UI_Item_TavernSingleReward2;
		[HideInInspector] public UI_Item_TavernSingleReward_SubView m_UI_Item_TavernSingleReward3;
		[HideInInspector] public UI_Item_TavernSingleReward_SubView m_UI_Item_TavernSingleReward4;
		[HideInInspector] public PolygonImage m_btn_singleRewardMask_PolygonImage;
		[HideInInspector] public GameButton m_btn_singleRewardMask_GameButton;



        private void UIFinder()
        {
			m_UI_Model_Interface = new UI_Model_Interface_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Interface"));
			m_pl_ress_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_ress");
			m_pl_ress_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_ress");

			m_UI_Dia = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"pl_ress/UI_Dia"));
			m_UI_Gold = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"pl_ress/UI_Gold"));
			m_UI_Sil = new UI_Model_Resources_SubView(FindUI<RectTransform>(vb.transform ,"pl_ress/UI_Sil"));
			m_pl_box = FindUI<RectTransform>(vb.transform ,"rect/pl_box");
			m_pl_woodbox = FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_woodbox");
			m_pl_woodbox_ani_Animator = FindUI<Animator>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_ani");

			m_btn_woodbox_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_ani/btn_woodbox");
			m_btn_woodbox_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_ani/btn_woodbox");

			m_UI_woodbox = new UI_Model_HallBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_ani/btn_woodbox/UI_woodbox"));
			m_pl_woodbox_standby_effect = FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_standby_effect");
			m_UI_10082_1 = new UI_10082_1_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_standby_effect/UI_10082_1"));
			m_pl_woodbox_content = FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_content");
			m_lbl_woodbox_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_content/lbl_woodbox_name");

			m_lbl_wooddesc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_content/lbl_wooddesc");

			m_pl_woodbox_btns_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_content/pl_woodbox_btns");
			m_pl_woodbox_btns_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_content/pl_woodbox_btns");

			m_UI_OpenAll_woodbox = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_content/pl_woodbox_btns/UI_OpenAll_woodbox"));
			m_UI_Open_woodbox = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_content/pl_woodbox_btns/UI_Open_woodbox"));
			m_UI_Free_woodbox = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_woodbox/pl_woodbox_content/pl_woodbox_btns/UI_Free_woodbox"));
			m_pl_goldbox = FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_goldbox");
			m_pl_goldbox_ani_Animator = FindUI<Animator>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_ani");

			m_btn_goldbox_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_ani/btn_goldbox");
			m_btn_goldbox_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_ani/btn_goldbox");

			m_UI_goldbox = new UI_Model_HallBox_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_ani/btn_goldbox/UI_goldbox"));
			m_pl_goldbox_standby_effect = FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_standby_effect");
			m_UI_10082_2 = new UI_10082_2_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_standby_effect/UI_10082_2"));
			m_pl_goldbox_content = FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_content");
			m_lbl_goldbox_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_content/lbl_goldbox_name");

			m_lbl_golddesc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_content/lbl_golddesc");

			m_pl_goldbox_btns_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_content/pl_goldbox_btns");
			m_pl_goldbox_btns_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_content/pl_goldbox_btns");

			m_UI_OpenAll_goldbox = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_content/pl_goldbox_btns/UI_OpenAll_goldbox"));
			m_UI_Open_goldbox = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_content/pl_goldbox_btns/UI_Open_goldbox"));
			m_UI_Free_goldbox = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/pl_box/pl_goldbox/pl_goldbox_content/pl_goldbox_btns/UI_Free_goldbox"));
			m_btn_showall_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_showall");
			m_btn_showall_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_showall");

			m_lbl_showall_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/btn_showall/lbl_showall");

			m_btn_rewardMask_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/btn_rewardMask");
			m_btn_rewardMask_GameButton = FindUI<GameButton>(vb.transform ,"rect/btn_rewardMask");

			m_pl_showReward_Animator = FindUI<Animator>(vb.transform ,"rect/pl_showReward");
			m_pl_showReward_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"rect/pl_showReward");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_showReward/lbl_title");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/pl_showReward/sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_showReward/sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"rect/pl_showReward/sv_list_view");

			m_pl_resultShow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_resultShow");

			m_pl_resul_btns_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_resultShow/pl_resul_btns");
			m_pl_resul_btns_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_resultShow/pl_resul_btns");

			m_UI_Free = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"pl_resultShow/pl_resul_btns/UI_Free"));
			m_UI_Open = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_resultShow/pl_resul_btns/UI_Open"));
			m_UI_OpenAll = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_resultShow/pl_resul_btns/UI_OpenAll"));
			m_UI_Item_TavernSingleReward1 = new UI_Item_TavernSingleReward_SubView(FindUI<RectTransform>(vb.transform ,"pl_resultShow/show/p1/UI_Item_TavernSingleReward1"));
			m_UI_Item_TavernSingleReward2 = new UI_Item_TavernSingleReward_SubView(FindUI<RectTransform>(vb.transform ,"pl_resultShow/show/p2/UI_Item_TavernSingleReward2"));
			m_UI_Item_TavernSingleReward3 = new UI_Item_TavernSingleReward_SubView(FindUI<RectTransform>(vb.transform ,"pl_resultShow/show/p3/UI_Item_TavernSingleReward3"));
			m_UI_Item_TavernSingleReward4 = new UI_Item_TavernSingleReward_SubView(FindUI<RectTransform>(vb.transform ,"pl_resultShow/show/p4/UI_Item_TavernSingleReward4"));
			m_btn_singleRewardMask_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_resultShow/btn_singleRewardMask");
			m_btn_singleRewardMask_GameButton = FindUI<GameButton>(vb.transform ,"pl_resultShow/btn_singleRewardMask");


            TavernSummonMediator mt = new TavernSummonMediator(vb.gameObject);
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

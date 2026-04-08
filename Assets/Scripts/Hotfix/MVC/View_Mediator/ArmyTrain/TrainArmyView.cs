// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月21日
// Update Time         :    2020年9月21日
// Class Description   :    TrainArmyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class TrainArmyView : GameView
    {
        public const string VIEW_NAME = "UI_Win_TrainArmy";

        public TrainArmyView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type2;
		[HideInInspector] public Image m_pl_mes_Image;

		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_pl_spine_SkeletonGraphic;

		[HideInInspector] public RectTransform m_pl_detailUp;
		[HideInInspector] public PolygonImage m_btn_detail_PolygonImage;
		[HideInInspector] public GameButton m_btn_detail_GameButton;
		[HideInInspector] public BtnAnimation m_btn_detail_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_detail_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_up_PolygonImage;
		[HideInInspector] public GameButton m_btn_up_GameButton;
		[HideInInspector] public BtnAnimation m_btn_up_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_up_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public Animator m_pl_train_Animator;
		[HideInInspector] public CanvasGroup m_pl_train_CanvasGroup;

		[HideInInspector] public UI_Model_ArmyTrainHead_SubView m_UI_Model_ArmyTrainHead_5;
		[HideInInspector] public UI_Model_ArmyTrainHead_SubView m_UI_Model_ArmyTrainHead_4;
		[HideInInspector] public UI_Model_ArmyTrainHead_SubView m_UI_Model_ArmyTrainHead_3;
		[HideInInspector] public UI_Model_ArmyTrainHead_SubView m_UI_Model_ArmyTrainHead_2;
		[HideInInspector] public UI_Model_ArmyTrainHead_SubView m_UI_Model_ArmyTrainHead_1;
		[HideInInspector] public RectTransform m_pl_desc;
		[HideInInspector] public PolygonImage m_img_army_type_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_army_type_icon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_desc_content_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_desc_content_ArabLayoutCompment;

		[HideInInspector] public RectTransform m_pl_res_cost;
		[HideInInspector] public UI_Model_TrainResCost_SubView m_UI_Model_TrainResCost;
		[HideInInspector] public UI_Model_DoubleLineButton_Blue_SubView m_UI_btn_train;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_btn_complete;
		[HideInInspector] public LanguageText m_lbl_lock_LanguageText;

		[HideInInspector] public RectTransform m_pl_time_cost;
		[HideInInspector] public GameSlider m_pb_time_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_countdown_LanguageText;

		[HideInInspector] public UI_Model_ArmyTrainHead_SubView m_UI_Model_ArmyTrainHead;
		[HideInInspector] public LanguageText m_lbl_time_cost_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_cost_desc_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue_SubView m_UI_btn_speedUp;
		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_btn_complete_speedUp;
		[HideInInspector] public RectTransform m_pl_num;
		[HideInInspector] public LanguageText m_lbl_num_up_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_up_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_last_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_last_num_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_detail_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_upgrade_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_pl_Body;



        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Model_Window_Type2 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type2"));
			m_pl_mes_Image = FindUI<Image>(vb.transform ,"pl_mes");

			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_left");

			m_pl_spine_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_spine");

			m_pl_detailUp = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp");
			m_btn_detail_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_detail");
			m_btn_detail_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_detail");
			m_btn_detail_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_detail");
			m_btn_detail_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_detail");

			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_detail/img_polygonImage");

			m_btn_up_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_up");
			m_btn_up_GameButton = FindUI<GameButton>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_up");
			m_btn_up_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_up");
			m_btn_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_left/pl_detailUp/btn_up");

			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_right");

			m_pl_train_Animator = FindUI<Animator>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train");
			m_pl_train_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train");

			m_UI_Model_ArmyTrainHead_5 = new UI_Model_ArmyTrainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/plArmyList/UI_Model_ArmyTrainHead_5"));
			m_UI_Model_ArmyTrainHead_4 = new UI_Model_ArmyTrainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/plArmyList/UI_Model_ArmyTrainHead_4"));
			m_UI_Model_ArmyTrainHead_3 = new UI_Model_ArmyTrainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/plArmyList/UI_Model_ArmyTrainHead_3"));
			m_UI_Model_ArmyTrainHead_2 = new UI_Model_ArmyTrainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/plArmyList/UI_Model_ArmyTrainHead_2"));
			m_UI_Model_ArmyTrainHead_1 = new UI_Model_ArmyTrainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/plArmyList/UI_Model_ArmyTrainHead_1"));
			m_pl_desc = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_desc");
			m_img_army_type_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_desc/img_army_type_icon");
			m_img_army_type_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_desc/img_army_type_icon");

			m_lbl_desc_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_desc/lbl_desc_name");
			m_lbl_desc_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_desc/lbl_desc_name");

			m_lbl_desc_content_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_desc/lbl_desc_content");
			m_lbl_desc_content_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_desc/lbl_desc_content");

			m_pl_res_cost = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_res_cost");
			m_UI_Model_TrainResCost = new UI_Model_TrainResCost_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_res_cost/UI_Model_TrainResCost"));
			m_UI_btn_train = new UI_Model_DoubleLineButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_res_cost/UI_btn_train"));
			m_UI_btn_complete = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_res_cost/UI_btn_complete"));
			m_lbl_lock_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_res_cost/lbl_lock");

			m_pl_time_cost = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost");
			m_pb_time_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost/pb_time");
			m_pb_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost/pb_time");

			m_lbl_countdown_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost/pb_time/lbl_countdown");

			m_UI_Model_ArmyTrainHead = new UI_Model_ArmyTrainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost/UI_Model_ArmyTrainHead"));
			m_lbl_time_cost_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost/lbl_time_cost_desc");
			m_lbl_time_cost_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost/lbl_time_cost_desc");

			m_UI_btn_speedUp = new UI_Model_DoubleLineButton_Blue_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost/UI_btn_speedUp"));
			m_UI_btn_complete_speedUp = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_time_cost/UI_btn_complete_speedUp"));
			m_pl_num = FindUI<RectTransform>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_num");
			m_lbl_num_up_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_num/lbl_num_up");
			m_lbl_num_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_num/lbl_num_up");

			m_lbl_last_num_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_num/lbl_last_num");
			m_lbl_last_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_right/pl_train/pl_num/lbl_last_num");

			m_pl_detail_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_detail");

			m_pl_upgrade_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_mes/imgContentBg/pl_upgrade");

            m_pl_Body = FindUI<PolygonImage>(vb.transform, "pl_mes/imgContentBg/pl_left/body");

            TrainArmyMediator mt = new TrainArmyMediator(vb.gameObject);
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

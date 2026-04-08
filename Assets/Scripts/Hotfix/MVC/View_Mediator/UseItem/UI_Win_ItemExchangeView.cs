// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月9日
// Update Time         :    2020年6月9日
// Class Description   :    UI_Win_ItemExchangeView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ItemExchangeView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ItemExchange";

        public UI_Win_ItemExchangeView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public RectTransform m_UI_Item_herodes;
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public UI_Model_Item_SubView m_UI_herolvup_item;
		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;

		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_changeNum_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_numbg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_changeNum_LanguageText;

		[HideInInspector] public PolygonImage m_btn_substract_PolygonImage;
		[HideInInspector] public GameButton m_btn_substract_GameButton;
		[HideInInspector] public BtnAnimation m_btn_substract_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_substract_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_substract_normal_PolygonImage;

		[HideInInspector] public PolygonImage m_img_substract_gray_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public BtnAnimation m_btn_add_ButtonAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_add_normal_PolygonImage;
		[HideInInspector] public GameButton m_img_add_normal_GameButton;
		[HideInInspector] public BtnAnimation m_img_add_normal_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_add_gray_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_quick_PolygonImage;
		[HideInInspector] public GameButton m_btn_quick_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_quick_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_quick_num_LanguageText;
		[HideInInspector] public Outline m_lbl_quick_num_Outline;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_btn_sure;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item1;
		[HideInInspector] public LanguageText m_lbl_item_name1_LanguageText;
		[HideInInspector] public Shadow m_lbl_item_name1_Shadow;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item2;
		[HideInInspector] public LanguageText m_lbl_item_name2_LanguageText;
		[HideInInspector] public Shadow m_lbl_item_name2_Shadow;



        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Item_herodes = FindUI<RectTransform>(vb.transform ,"rect/UI_Item_herodes");
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/UI_Item_herodes/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_herodes/pb_rogressBar");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_herodes/pb_rogressBar/lbl_name");

			m_UI_herolvup_item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Item_herodes/UI_herolvup_item"));
			m_lbl_mes_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_mes");

			m_img_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_arrow");

			m_pl_changeNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_changeNum");

			m_img_numbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_changeNum/img_numbg");

			m_lbl_changeNum_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_changeNum/lbl_changeNum");

			m_btn_substract_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_changeNum/btn_substract");
			m_btn_substract_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_changeNum/btn_substract");
			m_btn_substract_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/pl_changeNum/btn_substract");
			m_btn_substract_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_changeNum/btn_substract");

			m_img_substract_normal_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_changeNum/btn_substract/img_substract_normal");

			m_img_substract_gray_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_changeNum/btn_substract/img_substract_gray");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_changeNum/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_changeNum/btn_add");
			m_btn_add_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/pl_changeNum/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_changeNum/btn_add");

			m_img_add_normal_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_changeNum/btn_add/img_add_normal");
			m_img_add_normal_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_changeNum/btn_add/img_add_normal");
			m_img_add_normal_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/pl_changeNum/btn_add/img_add_normal");

			m_img_add_gray_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_changeNum/btn_add/img_add_gray");

			m_btn_quick_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/pl_changeNum/btn_quick");
			m_btn_quick_GameButton = FindUI<GameButton>(vb.transform ,"rect/pl_changeNum/btn_quick");
			m_btn_quick_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pl_changeNum/btn_quick");

			m_lbl_quick_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_changeNum/btn_quick/lbl_quick_num");
			m_lbl_quick_num_Outline = FindUI<Outline>(vb.transform ,"rect/pl_changeNum/btn_quick/lbl_quick_num");

			m_btn_sure = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_sure"));
			m_UI_Model_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_Item1"));
			m_lbl_item_name1_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Model_Item1/btn_animButton/lbl_item_name1");
			m_lbl_item_name1_Shadow = FindUI<Shadow>(vb.transform ,"rect/UI_Model_Item1/btn_animButton/lbl_item_name1");

			m_UI_Model_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_Item2"));
			m_lbl_item_name2_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Model_Item2/btn_animButton/lbl_item_name2");
			m_lbl_item_name2_Shadow = FindUI<Shadow>(vb.transform ,"rect/UI_Model_Item2/btn_animButton/lbl_item_name2");


            UI_Win_ItemExchangeMediator mt = new UI_Win_ItemExchangeMediator(vb.gameObject);
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

// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月16日
// Update Time         :    2020年6月16日
// Class Description   :    UI_Win_ExChangeActionPointView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_ExChangeActionPointView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ExChangeActionPoint";

        public UI_Win_ExChangeActionPointView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_TypeMid_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_img_barIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_barIcon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_btn_sure;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item2;
		[HideInInspector] public LanguageText m_lbl_item_name2_LanguageText;
		[HideInInspector] public Shadow m_lbl_item_name2_Shadow;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item1;
		[HideInInspector] public LanguageText m_lbl_item_name1_LanguageText;
		[HideInInspector] public Shadow m_lbl_item_name1_Shadow;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_TypeMid_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/plBar/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/plBar/pb_rogressBar");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/plBar/pb_rogressBar/lbl_name");

			m_img_barIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/plBar/img_barIcon");
			m_img_barIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/plBar/img_barIcon");

			m_img_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_arrow");
			m_img_arrow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_arrow");

			m_btn_sure = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn_sure"));
			m_UI_Model_Item2 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_Item2"));
			m_lbl_item_name2_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Model_Item2/btn_animButton/lbl_item_name2");
			m_lbl_item_name2_Shadow = FindUI<Shadow>(vb.transform ,"rect/UI_Model_Item2/btn_animButton/lbl_item_name2");

			m_UI_Model_Item1 = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_Item1"));
			m_lbl_item_name1_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Model_Item1/btn_animButton/lbl_item_name1");
			m_lbl_item_name1_Shadow = FindUI<Shadow>(vb.transform ,"rect/UI_Model_Item1/btn_animButton/lbl_item_name1");


            UI_Win_ExChangeActionPointMediator mt = new UI_Win_ExChangeActionPointMediator(vb.gameObject);
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

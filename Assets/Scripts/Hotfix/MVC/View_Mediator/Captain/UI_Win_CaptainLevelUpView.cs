// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月7日
// Update Time         :    2020年5月7日
// Class Description   :    UI_Win_CaptainLevelUpView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_CaptainLevelUpView : GameView
    {
        public const string VIEW_NAME = "UI_Win_CaptainLevelUp";

        public UI_Win_CaptainLevelUpView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_PBinTech_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_add_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_add_ArabLayoutCompment;
		[HideInInspector] public Animator m_lbl_add_Animator;

		[HideInInspector] public LanguageText m_lbl_NoneUse_LanguageText;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public UI_Item_CaptainLevelUpOnHead_SubView m_UI_Item_CaptainLevelUpOnHead;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Item_PBinTech_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_num");

			m_lbl_add_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/lbl_add");
			m_lbl_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/lbl_add");
			m_lbl_add_Animator = FindUI<Animator>(vb.transform ,"rect/UI_Item_PBinTech/lbl_add");

			m_lbl_NoneUse_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_NoneUse");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_CaptainHead"));
			m_UI_Item_CaptainLevelUpOnHead = new UI_Item_CaptainLevelUpOnHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Item_CaptainLevelUpOnHead"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");


            UI_Win_CaptainLevelUpMediator mt = new UI_Win_CaptainLevelUpMediator(vb.gameObject);
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

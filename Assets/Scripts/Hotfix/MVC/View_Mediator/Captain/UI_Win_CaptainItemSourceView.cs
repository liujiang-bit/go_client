// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月11日
// Update Time         :    2020年9月11日
// Class Description   :    UI_Win_CaptainItemSourceView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_CaptainItemSourceView : GameView
    {
        public const string VIEW_NAME = "UI_Win_CaptainItemSource";

        public UI_Win_CaptainItemSourceView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_PBinTech_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_text_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_itemMes_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_item_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_item_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_item_desc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_item_desc_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_Type1"));
			m_UI_Item_PBinTech_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_name");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pb_rogressBar/lbl_num");

			m_lbl_text_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/lbl_text");
			m_lbl_text_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/lbl_text");

			m_pl_itemMes_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pl_itemMes");

			m_lbl_item_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/pl_itemMes/lbl_item_name");
			m_lbl_item_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pl_itemMes/lbl_item_name");

			m_lbl_item_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/UI_Item_PBinTech/pl_itemMes/lbl_item_desc");
			m_lbl_item_desc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/UI_Item_PBinTech/pl_itemMes/lbl_item_desc");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(vb.transform ,"rect/UI_Model_Item"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"rect/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"rect/sv_list");


            UI_Win_CaptainItemSourceMediator mt = new UI_Win_CaptainItemSourceMediator(vb.gameObject);
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

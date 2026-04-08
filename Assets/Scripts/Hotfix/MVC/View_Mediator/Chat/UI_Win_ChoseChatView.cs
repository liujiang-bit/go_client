// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月21日
// Update Time         :    2020年9月21日
// Class Description   :    UI_Win_ChoseChatView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_ChoseChatView : GameView
    {
        public const string VIEW_NAME = "UI_Win_ChoseChat";

        public UI_Win_ChoseChatView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public PolygonImage m_btn_close_PolygonImage;
		[HideInInspector] public GameButton m_btn_close_GameButton;
		[HideInInspector] public BtnAnimation m_btn_close_BtnAnimation;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public BtnAnimation m_btn_back_BtnAnimation;

		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_v_list_PolygonImage;
		[HideInInspector] public Mask m_v_list_Mask;

		[HideInInspector] public RectTransform m_c_list;
		[HideInInspector] public RectTransform m_pl_share;
		[HideInInspector] public UI_Item_ChoseContact_SubView m_UI_Item_ChoseContact;
		[HideInInspector] public LanguageText m_lbl_share_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_share_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_text_PolygonImage;
		[HideInInspector] public GameInput m_ipt_text_GameInput;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_sure;
		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_btn_cancel;


        private void UIFinder()
        {
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"title/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"title/lbl_title");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"title/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"title/btn_close");
			m_btn_close_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"title/btn_close");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(vb.transform ,"title/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(vb.transform ,"title/btn_back");
			m_btn_back_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"title/btn_back");

			m_pl_mes = FindUI<RectTransform>(vb.transform ,"pl_mes");
			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_mes/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_mes/sv_list");

			m_v_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_mes/sv_list/v_list");
			m_v_list_Mask = FindUI<Mask>(vb.transform ,"pl_mes/sv_list/v_list");

			m_c_list = FindUI<RectTransform>(vb.transform ,"pl_mes/sv_list/v_list/c_list");
			m_pl_share = FindUI<RectTransform>(vb.transform ,"pl_share");
			m_UI_Item_ChoseContact = new UI_Item_ChoseContact_SubView(FindUI<RectTransform>(vb.transform ,"pl_share/UI_Item_ChoseContact"));
			m_lbl_share_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_share/lbl_share");
			m_lbl_share_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_share/lbl_share");

			m_ipt_text_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_share/ipt_text");
			m_ipt_text_GameInput = FindUI<GameInput>(vb.transform ,"pl_share/ipt_text");

			m_btn_sure = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_share/btn_sure"));
			m_btn_cancel = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"pl_share/btn_cancel"));

            UI_Win_ChoseChatMediator mt = new UI_Win_ChoseChatMediator(vb.gameObject);
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

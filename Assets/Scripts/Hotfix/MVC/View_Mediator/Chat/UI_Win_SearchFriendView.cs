// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_Win_SearchFriendView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_SearchFriendView : GameView
    {
        public const string VIEW_NAME = "UI_Win_SearchFriend";

        public UI_Win_SearchFriendView () 
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
		[HideInInspector] public BtnAnimation m_btn_close_ButtonAnimation;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public BtnAnimation m_btn_back_ButtonAnimation;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public ScrollRect m_sv_list_view_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_view_PolygonImage;
		[HideInInspector] public ListView m_sv_list_view_ListView;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public PolygonImage m_ipt_playerid_PolygonImage;
		[HideInInspector] public GameInput m_ipt_playerid_GameInput;

		[HideInInspector] public PolygonImage m_btn_search_PolygonImage;
		[HideInInspector] public GameButton m_btn_search_GameButton;
		[HideInInspector] public BtnAnimation m_btn_search_ButtonAnimation;



        private void UIFinder()
        {
			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"title/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"title/lbl_title");

			m_btn_close_PolygonImage = FindUI<PolygonImage>(vb.transform ,"title/btn_close");
			m_btn_close_GameButton = FindUI<GameButton>(vb.transform ,"title/btn_close");
			m_btn_close_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"title/btn_close");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(vb.transform ,"title/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(vb.transform ,"title/btn_back");
			m_btn_back_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"title/btn_back");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_languageText");

			m_sv_list_view_ScrollRect = FindUI<ScrollRect>(vb.transform ,"sv_list_view");
			m_sv_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list_view");
			m_sv_list_view_ListView = FindUI<ListView>(vb.transform ,"sv_list_view");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"sv_list_view/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"sv_list_view/v_list_view");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"sv_list_view/v_list_view/c_list_view");
			m_ipt_playerid_PolygonImage = FindUI<PolygonImage>(vb.transform ,"ipt_playerid");
			m_ipt_playerid_GameInput = FindUI<GameInput>(vb.transform ,"ipt_playerid");

			m_btn_search_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_search");
			m_btn_search_GameButton = FindUI<GameButton>(vb.transform ,"btn_search");
			m_btn_search_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_search");


            UI_Win_SearchFriendMediator mt = new UI_Win_SearchFriendMediator(vb.gameObject);
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

// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月24日
// Update Time         :    2020年4月24日
// Class Description   :    UI_Win_OtherPlayerDataView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Win_OtherPlayerDataView : GameView
    {
        public const string VIEW_NAME = "UI_Win_OtherPlayerData";

        public UI_Win_OtherPlayerDataView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type2_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_picture_PolygonImage;
		[HideInInspector] public GameButton m_btn_picture_GameButton;

		[HideInInspector] public LanguageText m_lbl_roleid_LanguageText;

		[HideInInspector] public PolygonImage m_btn_name_PolygonImage;
		[HideInInspector] public GameButton m_btn_name_GameButton;

		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;

		[HideInInspector] public LanguageText m_lbl_rolelbl_LanguageText;

		[HideInInspector] public PolygonImage m_img_civiIcon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_civil_LanguageText;

		[HideInInspector] public PolygonImage m_img_change_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_changeCountry_PolygonImage;
		[HideInInspector] public GameButton m_btn_changeCountry_GameButton;

		[HideInInspector] public LanguageText m_lbl_guild_LanguageText;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;

		[HideInInspector] public PolygonImage m_btn_killHelp_PolygonImage;
		[HideInInspector] public GameButton m_btn_killHelp_GameButton;

		[HideInInspector] public LanguageText m_lbl_kill_LanguageText;

		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataMore;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataAlliance;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDatahero;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataMail;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataChat;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_Type2_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/headarea/UI_Model_PlayerHead"));
			m_btn_picture_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/headarea/btn_picture");
			m_btn_picture_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/headarea/btn_picture");

			m_lbl_roleid_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/data/plname/name/lbl_roleid");

			m_btn_name_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/data/plname/name/btn_name");
			m_btn_name_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/data/plname/name/btn_name");

			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/data/plname/name/btn_name/lbl_playerName");

			m_lbl_rolelbl_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/data/plname/name/lbl_rolelbl");

			m_img_civiIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/data/plname/civil/img_civiIcon");

			m_lbl_civil_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/data/plname/civil/img_civiIcon/lbl_civil");

			m_img_change_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/data/plname/civil/img_change");

			m_btn_changeCountry_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/data/plname/civil/btn_changeCountry");
			m_btn_changeCountry_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/data/plname/civil/btn_changeCountry");

			m_lbl_guild_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/data/info/guild/lbl_guild");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/data/info/power/lbl_power");

			m_btn_killHelp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/data/info/btn_killHelp");
			m_btn_killHelp_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/data/info/btn_killHelp");

			m_lbl_kill_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/data/info/kill/lbl_kill");

			m_UI_Item_PlayerDataMore = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn/UI_Item_PlayerDataMore"));
			m_UI_Item_PlayerDataAlliance = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn/UI_Item_PlayerDataAlliance"));
			m_UI_Item_PlayerDatahero = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn/UI_Item_PlayerDatahero"));
			m_UI_Item_PlayerDataMail = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn/UI_Item_PlayerDataMail"));
			m_UI_Item_PlayerDataChat = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/btn/UI_Item_PlayerDataChat"));

            UI_Win_OtherPlayerDataMediator mt = new UI_Win_OtherPlayerDataMediator(vb.gameObject);
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

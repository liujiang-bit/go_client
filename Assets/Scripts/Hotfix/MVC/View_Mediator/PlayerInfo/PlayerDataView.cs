// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月28日
// Update Time         :    2020年9月28日
// Class Description   :    PlayerDataView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class PlayerDataView : GameView
    {
        public const string VIEW_NAME = "UI_Win_PlayerData";

        public PlayerDataView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Model_Window_Type1_SubView m_UI_Model_Window_TypeMid;
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_picture_PolygonImage;
		[HideInInspector] public GameButton m_btn_picture_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_picture_ArabLayoutCompment;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_StandardButton_MiniBlue;
		[HideInInspector] public LanguageText m_lbl_roleid_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_roleid_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_name_PolygonImage;
		[HideInInspector] public GameButton m_btn_name_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_playerName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_rolelbl_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_rolelbl_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_civiIcon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_civiIcon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_civil_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_civil_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_change_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_change_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_changeCountry_PolygonImage;
		[HideInInspector] public GameButton m_btn_changeCountry_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_changeCountry_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_guild_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_guild_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_killHelp_PolygonImage;
		[HideInInspector] public GameButton m_btn_killHelp_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_killHelp_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kill_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kill_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_UI_Model_AP_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_UI_Model_AP_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_value_LanguageText;

		[HideInInspector] public PolygonImage m_btn_add_PolygonImage;
		[HideInInspector] public GameButton m_btn_add_GameButton;
		[HideInInspector] public BtnAnimation m_btn_add_BtnAnimation;
		[HideInInspector] public ArabLayoutCompment m_btn_add_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_apHelp_PolygonImage;
		[HideInInspector] public GameButton m_btn_apHelp_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_apHelp_ArabLayoutCompment;

		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataAchieve;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataArmy;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDatahero;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataMore;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataLanguage;
		[HideInInspector] public UI_Item_PlayerDataBtn_SubView m_UI_Item_PlayerDataSetting;


        private void UIFinder()
        {
			m_UI_Model_Window_TypeMid = new UI_Model_Window_Type1_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_Window_TypeMid"));
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"UI_Tag_T1_WinAnime"));
			m_pl_effect = FindUI<RectTransform>(vb.transform ,"rect/info/bg/headarea/pl_effect");
			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/bg/headarea/UI_Model_PlayerHead"));
			m_btn_picture_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/bg/headarea/btn_picture");
			m_btn_picture_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/bg/headarea/btn_picture");
			m_btn_picture_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/headarea/btn_picture");

			m_UI_Model_StandardButton_MiniBlue = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/bg/headarea/UI_Model_StandardButton_MiniBlue"));
			m_lbl_roleid_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/bg/data/plname/name/lbl_roleid");
			m_lbl_roleid_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/plname/name/lbl_roleid");

			m_btn_name_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/bg/data/plname/name/btn_name");
			m_btn_name_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/bg/data/plname/name/btn_name");
			m_btn_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/plname/name/btn_name");

			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/bg/data/plname/name/btn_name/lbl_playerName");
			m_lbl_playerName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/plname/name/btn_name/lbl_playerName");

			m_lbl_rolelbl_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/bg/data/plname/name/lbl_rolelbl");
			m_lbl_rolelbl_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/plname/name/lbl_rolelbl");

			m_img_civiIcon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/bg/data/plname/civil/img_civiIcon");
			m_img_civiIcon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/plname/civil/img_civiIcon");

			m_lbl_civil_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/bg/data/plname/civil/img_civiIcon/lbl_civil");
			m_lbl_civil_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/plname/civil/img_civiIcon/lbl_civil");

			m_img_change_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/bg/data/plname/civil/img_change");
			m_img_change_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/plname/civil/img_change");

			m_btn_changeCountry_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/bg/data/plname/civil/btn_changeCountry");
			m_btn_changeCountry_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/bg/data/plname/civil/btn_changeCountry");
			m_btn_changeCountry_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/plname/civil/btn_changeCountry");

			m_lbl_guild_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/bg/data/info/guild/lbl_guild");
			m_lbl_guild_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/info/guild/lbl_guild");

			m_lbl_power_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/bg/data/info/power/lbl_power");
			m_lbl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/info/power/lbl_power");

			m_btn_killHelp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/bg/data/info/btn_killHelp");
			m_btn_killHelp_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/bg/data/info/btn_killHelp");
			m_btn_killHelp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/info/btn_killHelp");

			m_lbl_kill_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/bg/data/info/kill/lbl_kill");
			m_lbl_kill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/info/kill/lbl_kill");

			m_UI_Model_AP_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/info/bg/data/bar/UI_Model_AP");
			m_UI_Model_AP_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/bar/UI_Model_AP");

			m_lbl_value_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/info/bg/data/bar/UI_Model_AP/lbl_value");

			m_btn_add_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/bg/data/bar/btn_add");
			m_btn_add_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/bg/data/bar/btn_add");
			m_btn_add_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"rect/info/bg/data/bar/btn_add");
			m_btn_add_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/bar/btn_add");

			m_btn_apHelp_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/info/bg/data/bar/btn_apHelp");
			m_btn_apHelp_GameButton = FindUI<GameButton>(vb.transform ,"rect/info/bg/data/bar/btn_apHelp");
			m_btn_apHelp_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/info/bg/data/bar/btn_apHelp");

			m_UI_Item_PlayerDataAchieve = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/btn/UI_Item_PlayerDataAchieve"));
			m_UI_Item_PlayerDataArmy = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/btn/UI_Item_PlayerDataArmy"));
			m_UI_Item_PlayerDatahero = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/btn/UI_Item_PlayerDatahero"));
			m_UI_Item_PlayerDataMore = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/btn/UI_Item_PlayerDataMore"));
			m_UI_Item_PlayerDataLanguage = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/btn/UI_Item_PlayerDataLanguage"));
			m_UI_Item_PlayerDataSetting = new UI_Item_PlayerDataBtn_SubView(FindUI<RectTransform>(vb.transform ,"rect/info/btn/UI_Item_PlayerDataSetting"));

            PlayerDataMediator mt = new PlayerDataMediator(vb.gameObject);
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

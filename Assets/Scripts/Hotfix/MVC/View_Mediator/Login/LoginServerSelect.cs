// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, February 19, 2020
// Update Time         :    Wednesday, February 19, 2020
// Class Description   :    LoginView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class LoginServerSelect : GameView
    {
        public const string VIEW_NAME = "LoginServerSelect";

        public LoginServerSelect() 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_login_root;
		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_error_LanguageText;

		[HideInInspector] public LanguageText m_lbl_username_LanguageText;

		[HideInInspector] public LanguageText m_lbl_password_LanguageText;

		[HideInInspector] public PolygonImage m_ipt_username_PolygonImage;
		[HideInInspector] public GameInput m_ipt_username_GameInput;

		[HideInInspector] public PolygonImage m_ipt_password_PolygonImage;
		[HideInInspector] public GameInput m_ipt_password_GameInput;

		[HideInInspector] public Image m_btn_login_Image;
		[HideInInspector] public Button m_btn_login_Button;

		[HideInInspector] public PolygonImage m_btn_rmdname_PolygonImage;
		[HideInInspector] public GameButton m_btn_rmdname_GameButton;

		[HideInInspector] public Image m_dd_serverip_Image;
		[HideInInspector] public Dropdown m_dd_serverip_Dropdown;

		[HideInInspector] public PolygonImage m_ipt_serverGameNode_PolygonImage;
		[HideInInspector] public GameInput m_ipt_serverGameNode_GameInput;



        private void UIFinder()
        {
			m_login_root = FindUI<RectTransform>(vb.transform ,"login_root");
			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"login_root/img_polygonImage");

			m_lbl_error_LanguageText = FindUI<LanguageText>(vb.transform ,"login_root/img_polygonImage/lbl_error");

			m_lbl_username_LanguageText = FindUI<LanguageText>(vb.transform ,"login_root/img_polygonImage/lbl_username");

			m_lbl_password_LanguageText = FindUI<LanguageText>(vb.transform ,"login_root/img_polygonImage/lbl_password");

			m_ipt_username_PolygonImage = FindUI<PolygonImage>(vb.transform ,"login_root/img_polygonImage/ipt_username");
			m_ipt_username_GameInput = FindUI<GameInput>(vb.transform ,"login_root/img_polygonImage/ipt_username");

			m_ipt_password_PolygonImage = FindUI<PolygonImage>(vb.transform ,"login_root/img_polygonImage/ipt_password");
			m_ipt_password_GameInput = FindUI<GameInput>(vb.transform ,"login_root/img_polygonImage/ipt_password");

			m_btn_login_Image = FindUI<Image>(vb.transform ,"login_root/img_polygonImage/btn_login");
			m_btn_login_Button = FindUI<Button>(vb.transform ,"login_root/img_polygonImage/btn_login");

			m_btn_rmdname_PolygonImage = FindUI<PolygonImage>(vb.transform ,"login_root/img_polygonImage/btn_rmdname");
			m_btn_rmdname_GameButton = FindUI<GameButton>(vb.transform ,"login_root/img_polygonImage/btn_rmdname");

			m_dd_serverip_Image = FindUI<Image>(vb.transform ,"login_root/img_polygonImage/dd_serverip");
			m_dd_serverip_Dropdown = FindUI<Dropdown>(vb.transform ,"login_root/img_polygonImage/dd_serverip");

			m_ipt_serverGameNode_PolygonImage = FindUI<PolygonImage>(vb.transform ,"login_root/img_polygonImage/ipt_serverGameNode");
			m_ipt_serverGameNode_GameInput = FindUI<GameInput>(vb.transform ,"login_root/img_polygonImage/ipt_serverGameNode");


			LoginServerSelectMediator mt = new LoginServerSelectMediator(vb.gameObject);
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

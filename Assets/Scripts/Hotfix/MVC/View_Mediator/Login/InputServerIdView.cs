// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月7日
// Update Time         :    2020年9月7日
// Class Description   :    InputServerIdView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class InputServerIdView : GameView
    {
        public const string VIEW_NAME = "InputServerId";

        public InputServerIdView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_login_root;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_polygonImage_PolygonImage;

		[HideInInspector] public PolygonImage m_ipt_serverGameNode_PolygonImage;
		[HideInInspector] public GameInput m_ipt_serverGameNode_GameInput;

		[HideInInspector] public Image m_btn_login_Image;
		[HideInInspector] public Button m_btn_login_Button;



        private void UIFinder()
        {
			m_login_root = FindUI<RectTransform>(vb.transform ,"login_root");
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"login_root/img_bg");

			m_img_polygonImage_PolygonImage = FindUI<PolygonImage>(vb.transform ,"login_root/img_polygonImage");

			m_ipt_serverGameNode_PolygonImage = FindUI<PolygonImage>(vb.transform ,"login_root/img_polygonImage/ipt_serverGameNode");
			m_ipt_serverGameNode_GameInput = FindUI<GameInput>(vb.transform ,"login_root/img_polygonImage/ipt_serverGameNode");

			m_btn_login_Image = FindUI<Image>(vb.transform ,"login_root/btn_login");
			m_btn_login_Button = FindUI<Button>(vb.transform ,"login_root/btn_login");


            InputServerIdMediator mt = new InputServerIdMediator(vb.gameObject);
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

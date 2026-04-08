// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月18日
// Update Time         :    2020年9月18日
// Class Description   :    FingerView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class FingerView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Finger";

        public FingerView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public PolygonImage m_img_target_PolygonImage;

		[HideInInspector] public UE_GuideGuild_SubView m_UE_GuideGuild;
		[HideInInspector] public RectTransform m_pl_arrow;
		[HideInInspector] public RectTransform m_pl_arrow_rotation;
		[HideInInspector] public PolygonImage m_img_anim_arrow_PolygonImage;
		[HideInInspector] public Animation m_img_anim_arrow_Animation;



        private void UIFinder()
        {
			m_pl_content = FindUI<RectTransform>(vb.transform ,"pl_content");
			m_img_target_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/img_target");

			m_UE_GuideGuild = new UE_GuideGuild_SubView(FindUI<RectTransform>(vb.transform ,"pl_content/img_target/UE_GuideGuild"));
			m_pl_arrow = FindUI<RectTransform>(vb.transform ,"pl_content/pl_arrow");
			m_pl_arrow_rotation = FindUI<RectTransform>(vb.transform ,"pl_content/pl_arrow/pl_arrow_rotation");
			m_img_anim_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_arrow/pl_arrow_rotation/rotation/arrowOffset/img_anim_arrow");
			m_img_anim_arrow_Animation = FindUI<Animation>(vb.transform ,"pl_content/pl_arrow/pl_arrow_rotation/rotation/arrowOffset/img_anim_arrow");


            FingerMediator mt = new FingerMediator(vb.gameObject);
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

// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, 29 October 2020
// Update Time         :    Thursday, 29 October 2020
// Class Description   :    UI_IF_EquipSuccessView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_IF_EquipSuccessView : GameView
    {
        public const string VIEW_NAME = "UI_IF_EquipSuccess";

        public UI_IF_EquipSuccessView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_success;
		[HideInInspector] public Animator m_pl_view_Animator;

		[HideInInspector] public LanguageText m_lbl_title_LanguageText;
		[HideInInspector] public Shadow m_lbl_title_Shadow;

		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public Animation m_pl_equipicon_Animation;

		[HideInInspector] public PolygonImage m_img_euqip_PolygonImage;
		[HideInInspector] public Animation m_img_euqip_Animation;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_talentName_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_talentName_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_img_talent_PolygonImage;



        private void UIFinder()
        {
			m_pl_success = FindUI<RectTransform>(vb.transform ,"pl_success");
			m_pl_view_Animator = FindUI<Animator>(vb.transform ,"pl_success/pl_view");

			m_lbl_title_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_success/pl_view/lbl_title");
			m_lbl_title_Shadow = FindUI<Shadow>(vb.transform ,"pl_success/pl_view/lbl_title");

			m_pl_effect = FindUI<RectTransform>(vb.transform ,"pl_success/pl_effect");
			m_pl_equipicon_Animation = FindUI<Animation>(vb.transform ,"pl_success/pl_equipicon");

			m_img_euqip_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_success/pl_equipicon/img_euqip");
			m_img_euqip_Animation = FindUI<Animation>(vb.transform ,"pl_success/pl_equipicon/img_euqip");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_success/lbl_name");

			m_lbl_talentName_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_success/lbl_talentName");
			m_lbl_talentName_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_success/lbl_talentName");

			m_img_talent_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_success/lbl_talentName/img_talent");


            UI_IF_EquipSuccessMediator mt = new UI_IF_EquipSuccessMediator(vb.gameObject);
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

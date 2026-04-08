// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年3月2日
// Update Time         :    2020年3月2日
// Class Description   :    GuideView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class GuideView : GameView
    {
        public const string VIEW_NAME = "UI_Win_Guide";

        public GuideView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_fullMask_PolygonImage;

		[HideInInspector] public RectTransform m_pl_content;
		[HideInInspector] public GuideHighlightMask m_img_mask_GuideHighlightMask;

		[HideInInspector] public PolygonImage m_img_target_PolygonImage;

		[HideInInspector] public RectTransform m_pl_tip;
		[HideInInspector] public PolygonImage m_img_tipbg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public RectTransform m_pl_tipArrow;
		[HideInInspector] public PolygonImage m_img_tipArrow_PolygonImage;

		[HideInInspector] public RectTransform m_pl_arrow;
		[HideInInspector] public RectTransform m_pl_arrow_rotation;
		[HideInInspector] public PolygonImage m_img_anim_arrow_PolygonImage;
		[HideInInspector] public Animation m_img_anim_arrow_Animation;

		[HideInInspector] public PolygonImage m_btn_jump_PolygonImage;
		[HideInInspector] public GameButton m_btn_jump_GameButton;

		[HideInInspector] public PolygonImage m_btn_neverJump_PolygonImage;
		[HideInInspector] public GameButton m_btn_neverJump_GameButton;



        private void UIFinder()
        {
			m_img_fullMask_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_fullMask");

			m_pl_content = FindUI<RectTransform>(vb.transform ,"pl_content");
			m_img_mask_GuideHighlightMask = FindUI<GuideHighlightMask>(vb.transform ,"pl_content/img_mask");

			m_img_target_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/img_target");

			m_pl_tip = FindUI<RectTransform>(vb.transform ,"pl_content/pl_tip");
			m_img_tipbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_tip/img_tipbg");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_content/pl_tip/img_tipbg/lbl_desc");

			m_pl_tipArrow = FindUI<RectTransform>(vb.transform ,"pl_content/pl_tip/img_tipbg/pl_tipArrow");
			m_img_tipArrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_tip/img_tipbg/pl_tipArrow/img_tipArrow");

			m_pl_arrow = FindUI<RectTransform>(vb.transform ,"pl_content/pl_arrow");
			m_pl_arrow_rotation = FindUI<RectTransform>(vb.transform ,"pl_content/pl_arrow/pl_arrow_rotation");
			m_img_anim_arrow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_content/pl_arrow/pl_arrow_rotation/rotation/arrowOffset/img_anim_arrow");
			m_img_anim_arrow_Animation = FindUI<Animation>(vb.transform ,"pl_content/pl_arrow/pl_arrow_rotation/rotation/arrowOffset/img_anim_arrow");

			m_btn_jump_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_jump");
			m_btn_jump_GameButton = FindUI<GameButton>(vb.transform ,"btn_jump");

			m_btn_neverJump_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_neverJump");
			m_btn_neverJump_GameButton = FindUI<GameButton>(vb.transform ,"btn_neverJump");


            GuideMediator mt = new GuideMediator(vb.gameObject);
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

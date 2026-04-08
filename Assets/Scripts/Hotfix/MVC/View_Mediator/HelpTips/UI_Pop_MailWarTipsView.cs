// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月30日
// Update Time         :    2020年6月30日
// Class Description   :    UI_Pop_MailWarTipsView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public class UI_Pop_MailWarTipsView : GameView
    {
        public const string VIEW_NAME = "UI_Pop_MailWarTips";

        public UI_Pop_MailWarTipsView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public RectTransform m_pl_pos;
		[HideInInspector] public Animator m_pl_content_Animator;
		[HideInInspector] public UIDefaultValue m_pl_content_UIDefaultValue;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;

		[HideInInspector] public PolygonImage m_v_list_PolygonImage;
		[HideInInspector] public Mask m_v_list_Mask;

		[HideInInspector] public RectTransform m_c_list;


        private void UIFinder()
        {
			m_pl_pos = FindUI<RectTransform>(vb.transform ,"pl_pos");
			m_pl_content_Animator = FindUI<Animator>(vb.transform ,"pl_pos/pl_content");
			m_pl_content_UIDefaultValue = FindUI<UIDefaultValue>(vb.transform ,"pl_pos/pl_content");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideR");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideTop");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/img_bg/img_arrowSideL");

			m_sv_list_ScrollRect = FindUI<ScrollRect>(vb.transform ,"pl_pos/pl_content/sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/sv_list");
			m_sv_list_ListView = FindUI<ListView>(vb.transform ,"pl_pos/pl_content/sv_list");

			m_v_list_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_pos/pl_content/sv_list/v_list");
			m_v_list_Mask = FindUI<Mask>(vb.transform ,"pl_pos/pl_content/sv_list/v_list");

			m_c_list = FindUI<RectTransform>(vb.transform ,"pl_pos/pl_content/sv_list/v_list/c_list");

            UI_Pop_MailWarTipsMediator mt = new UI_Pop_MailWarTipsMediator(vb.gameObject);
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

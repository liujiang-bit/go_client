// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Pop_EventHellBoxPop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Pop_EventHellBoxPop_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Pop_EventHellBoxPop";

        public UI_Pop_EventHellBoxPop_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Pop_EventHellBoxPop;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideL_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideButtom_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideTop_PolygonImage;

		[HideInInspector] public PolygonImage m_img_arrowSideR_PolygonImage;

		[HideInInspector] public ScrollRect m_sv_rewards_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_rewards_PolygonImage;
		[HideInInspector] public ListView m_sv_rewards_ListView;



        private void UIFinder()
        {       
			m_UI_Pop_EventHellBoxPop = gameObject.GetComponent<RectTransform>();
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_img_arrowSideL_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/img_arrowSideL");

			m_img_arrowSideButtom_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/img_arrowSideButtom");

			m_img_arrowSideTop_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/img_arrowSideTop");

			m_img_arrowSideR_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/img_arrowSideR");

			m_sv_rewards_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"list/sv_rewards");
			m_sv_rewards_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"list/sv_rewards");
			m_sv_rewards_ListView = FindUI<ListView>(gameObject.transform ,"list/sv_rewards");


			BindEvent();
        }

        #endregion
    }
}
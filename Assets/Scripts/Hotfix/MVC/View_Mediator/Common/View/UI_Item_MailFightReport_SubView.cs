// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailFightReport_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailFightReport_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailFightReport";

        public UI_Item_MailFightReport_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailFightReport;
		[HideInInspector] public ScrollRect m_sv_fightReport_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_fightReport_PolygonImage;
		[HideInInspector] public ListView m_sv_fightReport_ListView;

		[HideInInspector] public PolygonImage m_img_loading_PolygonImage;
		[HideInInspector] public Animation m_img_loading_Animation;



        private void UIFinder()
        {       
			m_UI_Item_MailFightReport = gameObject.GetComponent<RectTransform>();
			m_sv_fightReport_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_fightReport");
			m_sv_fightReport_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_fightReport");
			m_sv_fightReport_ListView = FindUI<ListView>(gameObject.transform ,"sv_fightReport");

			m_img_loading_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_loading");
			m_img_loading_Animation = FindUI<Animation>(gameObject.transform ,"img_loading");


			BindEvent();
        }

        #endregion
    }
}
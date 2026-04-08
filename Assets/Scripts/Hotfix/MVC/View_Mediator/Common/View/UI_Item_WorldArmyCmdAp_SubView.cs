// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldArmyCmdAp_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldArmyCmdAp_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldArmyCmdAp";

        public UI_Item_WorldArmyCmdAp_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WorldArmyCmdAp;
		[HideInInspector] public GridLayoutGroup m_pl_bg_GridLayoutGroup;

		[HideInInspector] public GridLayoutGroup m_pl_ap_GridLayoutGroup;

		[HideInInspector] public PolygonImage m_img_ap1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_ap2_PolygonImage;

		[HideInInspector] public PolygonImage m_img_ap3_PolygonImage;

		[HideInInspector] public PolygonImage m_img_ap4_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_WorldArmyCmdAp = gameObject.GetComponent<RectTransform>();
			m_pl_bg_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_bg");

			m_pl_ap_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_ap");

			m_img_ap1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_ap/img_ap1");

			m_img_ap2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_ap/img_ap2");

			m_img_ap3_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_ap/img_ap3");

			m_img_ap4_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_ap/img_ap4");


			BindEvent();
        }

        #endregion
    }
}
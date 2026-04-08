// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_Equip_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_Equip_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_Equip";

        public UI_Model_Equip_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_Equip;
		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public PolygonImage m_img_equip_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_Equip = gameObject.GetComponent<RectTransform>();
			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_effect");
			m_img_equip_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_equip");


			BindEvent();
        }

        #endregion
    }
}
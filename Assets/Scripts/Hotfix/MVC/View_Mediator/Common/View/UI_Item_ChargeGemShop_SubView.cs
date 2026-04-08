// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeGemShop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeGemShop_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeGemShop";

        public UI_Item_ChargeGemShop_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeGemShop;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;



        private void UIFinder()
        {       
			m_UI_Item_ChargeGemShop = gameObject.GetComponent<RectTransform>();
			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"sv_list");


			BindEvent();
        }

        #endregion
    }
}
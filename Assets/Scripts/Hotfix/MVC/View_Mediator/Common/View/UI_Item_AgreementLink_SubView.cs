// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_AgreementLink_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_AgreementLink_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_AgreementLink";

        public UI_Item_AgreementLink_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_AgreementLink;
		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link;


        private void UIFinder()
        {       
			m_UI_Item_AgreementLink = gameObject.GetComponent<RectTransform>();
			m_UI_Model_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Link"));

			BindEvent();
        }

        #endregion
    }
}
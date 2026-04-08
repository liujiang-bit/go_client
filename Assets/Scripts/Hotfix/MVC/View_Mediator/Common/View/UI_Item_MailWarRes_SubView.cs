// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailWarRes_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailWarRes_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailWarRes";

        public UI_Item_MailWarRes_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailWarRes;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesFood;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesWood;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesStone;
		[HideInInspector] public UI_Model_ResourcesConsume_SubView m_UI_Model_ResourcesGold;


        private void UIFinder()
        {       
			m_UI_Item_MailWarRes = gameObject.GetComponent<RectTransform>();
			m_UI_Model_ResourcesFood = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/UI_Model_ResourcesFood"));
			m_UI_Model_ResourcesWood = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/UI_Model_ResourcesWood"));
			m_UI_Model_ResourcesStone = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/UI_Model_ResourcesStone"));
			m_UI_Model_ResourcesGold = new UI_Model_ResourcesConsume_SubView(FindUI<RectTransform>(gameObject.transform ,"rect/UI_Model_ResourcesGold"));

			BindEvent();
        }

        #endregion
    }
}
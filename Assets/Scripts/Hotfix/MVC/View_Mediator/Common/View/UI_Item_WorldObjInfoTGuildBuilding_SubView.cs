// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldObjInfoTGuildBuilding_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldObjInfoTGuildBuilding_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjInfoTGuildBuilding";

        public UI_Item_WorldObjInfoTGuildBuilding_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WorldObjInfoTGuildBuilding;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line1;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line2;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line3;
		[HideInInspector] public UI_Item_IconAndTime_SubView m_UI_Item_IconAndTime;


        private void UIFinder()
        {       
			m_UI_Item_WorldObjInfoTGuildBuilding = gameObject.GetComponent<RectTransform>();
			m_UI_Item_line1 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line1"));
			m_UI_Item_line2 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line2"));
			m_UI_Item_line3 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line3"));
			m_UI_Item_IconAndTime = new UI_Item_IconAndTime_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_IconAndTime"));

			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainEquipList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_CaptainEquipList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainEquipList";

        public UI_Item_CaptainEquipList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_CaptainEquipList;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Item_CaptainEquipUse1;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Item_CaptainEquipUse2;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Item_CaptainEquipUse3;


        private void UIFinder()
        {       
			m_UI_Item_CaptainEquipList = gameObject.GetComponent<RectTransform>();
			m_UI_Item_CaptainEquipUse1 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainEquipUse1"));
			m_UI_Item_CaptainEquipUse2 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainEquipUse2"));
			m_UI_Item_CaptainEquipUse3 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Item_CaptainEquipUse3"));

			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月6日
// Update Time         :    2020年1月6日
// Class Description   :    ItemBuildingDescSoilerView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class ItemBuildingDescSoilerView : GameView
    {
		public const string VIEW_NAME = "UI_Item_BuildingDescSoiler";

        public ItemBuildingDescSoilerView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_ArmyInBuildingDesc_SubView m_UI_Item_ArmyInBuildingDesc1;
		[HideInInspector] public UI_Item_ArmyInBuildingDesc_SubView m_UI_Item_ArmyInBuildingDesc2;
		[HideInInspector] public UI_Item_ArmyInBuildingDesc_SubView m_UI_Item_ArmyInBuildingDesc3;
		[HideInInspector] public UI_Item_ArmyInBuildingDesc_SubView m_UI_Item_ArmyInBuildingDesc4;
		[HideInInspector] public UI_Item_ArmyInBuildingDesc_SubView m_UI_Item_ArmyInBuildingDesc5;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_Item_ArmyInBuildingDesc1 = new UI_Item_ArmyInBuildingDesc_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ArmyInBuildingDesc1"));
			m_UI_Item_ArmyInBuildingDesc2 = new UI_Item_ArmyInBuildingDesc_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ArmyInBuildingDesc2"));
			m_UI_Item_ArmyInBuildingDesc3 = new UI_Item_ArmyInBuildingDesc_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ArmyInBuildingDesc3"));
			m_UI_Item_ArmyInBuildingDesc4 = new UI_Item_ArmyInBuildingDesc_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ArmyInBuildingDesc4"));
			m_UI_Item_ArmyInBuildingDesc5 = new UI_Item_ArmyInBuildingDesc_SubView(FindUI<RectTransform>(vb.transform ,"UI_Item_ArmyInBuildingDesc5"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
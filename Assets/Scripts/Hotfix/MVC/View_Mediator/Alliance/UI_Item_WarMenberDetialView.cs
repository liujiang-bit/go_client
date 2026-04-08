// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 30, 2020
// Update Time         :    Thursday, April 30, 2020
// Class Description   :    UI_Item_WarMenberDetialView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_WarMenberDetialView : GameView
    {
		public const string VIEW_NAME = "UI_Item_WarMenberDetial";

        public UI_Item_WarMenberDetialView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public GridLayoutGroup m_pl_addHere_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_addHere_ArabLayoutCompment;

		[HideInInspector] public UI_Item_SoldierHead_SubView m_UI_head1;
		[HideInInspector] public UI_Item_SoldierHead_SubView m_UI_head2;
		[HideInInspector] public UI_Item_SoldierHead_SubView m_UI_head3;
		[HideInInspector] public UI_Item_SoldierHead_SubView m_UI_head4;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_addHere_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"pl_addHere");
			m_pl_addHere_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_addHere");

			m_UI_head1 = new UI_Item_SoldierHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_addHere/UI_head1"));
			m_UI_head2 = new UI_Item_SoldierHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_addHere/UI_head2"));
			m_UI_head3 = new UI_Item_SoldierHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_addHere/UI_head3"));
			m_UI_head4 = new UI_Item_SoldierHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_addHere/UI_head4"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
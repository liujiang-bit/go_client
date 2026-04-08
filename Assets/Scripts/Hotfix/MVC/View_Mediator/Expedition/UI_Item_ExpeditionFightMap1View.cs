// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月10日
// Update Time         :    2020年6月10日
// Class Description   :    UI_Item_ExpeditionFightMap1View
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_ExpeditionFightMap1View : GameView
    {
		public const string VIEW_NAME = "UI_Item_ExpeditionFightMap1";

        public UI_Item_ExpeditionFightMap1View () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_map_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_map_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_map_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_map");
			m_img_map_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_map");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
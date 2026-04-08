// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月6日
// Update Time         :    2020年7月6日
// Class Description   :    UI_Tip_GuildBuildStateView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Tip_GuildBuildStateView : GameView
    {
		public const string VIEW_NAME = "UI_Tip_GuildBuildState";

        public UI_Tip_GuildBuildStateView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;

		[HideInInspector] public PolygonImage m_lbl_icon_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_img_frame_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/img_frame");

			m_lbl_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg/lbl_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

        public void SetState(long state)
        {
	        
        }
    }
}
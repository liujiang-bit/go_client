// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, July 21, 2020
// Update Time         :    Tuesday, July 21, 2020
// Class Description   :    UI_Pop_GuildResTextView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_GuildResTextView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_GuildResText";

        public UI_Pop_GuildResTextView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public Animation m_pl_offset_Animation;
		[HideInInspector] public CanvasGroup m_pl_offset_CanvasGroup;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public Outline m_lbl_num_Outline;

		[HideInInspector] public PolygonImage m_img_cur_PolygonImage;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_pl_offset_Animation = FindUI<Animation>(vb.transform ,"pl_offset");
			m_pl_offset_CanvasGroup = FindUI<CanvasGroup>(vb.transform ,"pl_offset");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_offset/lbl_num");
			m_lbl_num_Outline = FindUI<Outline>(vb.transform ,"pl_offset/lbl_num");

			m_img_cur_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_offset/img_cur");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
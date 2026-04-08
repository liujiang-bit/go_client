// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月23日
// Update Time         :    2020年4月23日
// Class Description   :    UI_Item_BuffListItemView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_BuffListItemView : GameView
    {
		public const string VIEW_NAME = "UI_Item_BuffListItem";

        public UI_Item_BuffListItemView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_buffName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buffName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_buffTime_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buffTime_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_buffName_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_buffName");
			m_lbl_buffName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_buffName");

			m_lbl_buffTime_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_buffTime");
			m_lbl_buffTime_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/lbl_buffTime");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"rect/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/pb_rogressBar");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"rect/img_icon");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
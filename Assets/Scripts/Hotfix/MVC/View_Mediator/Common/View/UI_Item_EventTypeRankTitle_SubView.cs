// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTypeRankTitle_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTypeRankTitle_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTypeRankTitle";

        public UI_Item_EventTypeRankTitle_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTypeRankTitle;
		[HideInInspector] public PolygonImage m_img_stageTitle_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_stageName_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_EventTypeRankTitle = gameObject.GetComponent<RectTransform>();
			m_img_stageTitle_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_stageTitle");

			m_lbl_stageName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_stageName");


			BindEvent();
        }

        #endregion
    }
}
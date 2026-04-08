// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_TrainUpHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Model_TrainUpHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_TrainUpHead";

        public UI_Model_TrainUpHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_TrainUpHead;
		[HideInInspector] public PolygonImage m_img_army_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder()
        {       
			m_UI_Model_TrainUpHead = gameObject.GetComponent<RectTransform>();
			m_img_army_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_army_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");


			BindEvent();
        }

        #endregion
    }
}
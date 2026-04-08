// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_VipBuffListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_VipBuffListItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_VipBuffListItem";

        public UI_Item_VipBuffListItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_VipBuffListItem;
		[HideInInspector] public LanguageText m_lbl_buff_LanguageText;

		[HideInInspector] public PolygonImage m_img_new_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_VipBuffListItem = gameObject.GetComponent<RectTransform>();
			m_lbl_buff_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_buff");

			m_img_new_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_new");


			BindEvent();
        }

        #endregion
    }
}
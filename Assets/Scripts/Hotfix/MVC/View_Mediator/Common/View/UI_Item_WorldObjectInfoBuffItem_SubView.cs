// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldObjectInfoBuffItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldObjectInfoBuffItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjectInfoBuffItem";

        public UI_Item_WorldObjectInfoBuffItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Item_WorldObjectInfoBuffItem_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_WorldObjectInfoBuffItem_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_buffDesc_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_buffDesc_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_buff_PolygonImage;
		[HideInInspector] public UIDefaultValue m_img_buff_UIDefaultValue;
		[HideInInspector] public ArabLayoutCompment m_img_buff_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_WorldObjectInfoBuffItem_PolygonImage = gameObject.GetComponent<PolygonImage>();
			m_UI_Item_WorldObjectInfoBuffItem_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_lbl_buffDesc_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_buffDesc");
			m_lbl_buffDesc_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_buffDesc");

			m_img_buff_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_buff");
			m_img_buff_UIDefaultValue = FindUI<UIDefaultValue>(gameObject.transform ,"img_buff");
			m_img_buff_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_buff");


			BindEvent();
        }

        #endregion
    }
}
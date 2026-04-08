// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyBuffData_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ArmyBuffData_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyBuffData";

        public UI_Item_ArmyBuffData_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ArmyBuffData;
		[HideInInspector] public PolygonImage m_img_img_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_val_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ArmyBuffData = gameObject.GetComponent<RectTransform>();
			m_img_img_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_img");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");

			m_lbl_val_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_val");


			BindEvent();
        }

        #endregion
    }
}
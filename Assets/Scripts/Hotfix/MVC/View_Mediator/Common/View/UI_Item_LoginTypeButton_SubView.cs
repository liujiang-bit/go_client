// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_LoginTypeButton_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_LoginTypeButton_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_LoginTypeButton";

        public UI_Item_LoginTypeButton_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_LoginTypeButton;
		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_LoginTypeButton = gameObject.GetComponent<RectTransform>();
			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");


			BindEvent();
        }

        #endregion
    }
}
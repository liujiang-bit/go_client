// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyInBuildingDesc_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_ArmyInBuildingDesc_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyInBuildingDesc";

        public UI_Item_ArmyInBuildingDesc_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ArmyInBuildingDesc;
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_icon_MakeChildrenGray;

		[HideInInspector] public PolygonImage m_btn_des_PolygonImage;
		[HideInInspector] public GameButton m_btn_des_GameButton;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ArmyInBuildingDesc = gameObject.GetComponent<RectTransform>();
			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_MakeChildrenGray = FindUI<GrayChildrens>(gameObject.transform ,"img_icon");

			m_btn_des_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_des");
			m_btn_des_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_des");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_count");


			BindEvent();
        }

        #endregion
    }
}
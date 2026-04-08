// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_GuildFlagImg_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_GuildFlagImg_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_GuildFlagImg";

        public UI_Item_GuildFlagImg_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_GuildFlagImg;
		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_icon_PolygonImage;
		[HideInInspector] public GameButton m_btn_icon_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_GuildFlagImg = gameObject.GetComponent<RectTransform>();
			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_select");

			m_btn_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_icon");
			m_btn_icon_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_icon");


			BindEvent();
        }

        #endregion
    }
}
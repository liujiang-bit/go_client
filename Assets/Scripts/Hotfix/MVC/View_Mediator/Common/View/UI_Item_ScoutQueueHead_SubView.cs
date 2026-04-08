// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ScoutQueueHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ScoutQueueHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ScoutQueueHead";

        public UI_Item_ScoutQueueHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ScoutQueueHead;
		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;

		[HideInInspector] public PolygonImage m_img_state_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_noTextButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_noTextButton_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_ScoutQueueHead = gameObject.GetComponent<RectTransform>();
			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_select");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_img_char_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/img_char");

			m_img_state_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/img_state");

			m_btn_noTextButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg/btn_noTextButton");
			m_btn_noTextButton_GameButton = FindUI<GameButton>(gameObject.transform ,"img_bg/btn_noTextButton");


			BindEvent();
        }

        #endregion
    }
}
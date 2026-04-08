// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailWarReinforce_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailWarReinforce_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailWarReinforce";

        public UI_Item_MailWarReinforce_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Item_MailWarReinforce_PolygonImage;
		[HideInInspector] public GameButton m_UI_Item_MailWarReinforce_GameButton;

		[HideInInspector] public PolygonImage m_img_add_PolygonImage;

		[HideInInspector] public PolygonImage m_img_remove_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_MailWarReinforce_PolygonImage = gameObject.GetComponent<PolygonImage>();
			m_UI_Item_MailWarReinforce_GameButton = gameObject.GetComponent<GameButton>();

			m_img_add_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_add");

			m_img_remove_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_remove");


			BindEvent();
        }

        #endregion
    }
}
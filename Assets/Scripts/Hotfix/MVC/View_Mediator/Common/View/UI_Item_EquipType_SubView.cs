// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipType_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EquipType_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipType";

        public UI_Item_EquipType_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public GameToggle m_UI_Item_EquipType_GameToggle;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_EquipType_GameToggle = gameObject.GetComponent<GameToggle>();

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");


			BindEvent();
        }

        #endregion
    }
}
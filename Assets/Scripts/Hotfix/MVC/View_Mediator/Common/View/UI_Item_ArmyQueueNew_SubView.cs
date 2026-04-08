// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyQueueNew_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_ArmyQueueNew_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyQueueNew";

        public UI_Item_ArmyQueueNew_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ArmyQueueNew;
		[HideInInspector] public PolygonImage m_img_normal_PolygonImage;
		[HideInInspector] public GameButton m_img_normal_GameButton;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_def_PolygonImage;

		[HideInInspector] public PolygonImage m_img_atk_PolygonImage;

		[HideInInspector] public PolygonImage m_img_key_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_ArmyQueueNew = gameObject.GetComponent<RectTransform>();
			m_img_normal_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_normal");
			m_img_normal_GameButton = FindUI<GameButton>(gameObject.transform ,"img_normal");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_select");

			m_img_def_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_def");

			m_img_atk_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_atk");

			m_img_key_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_key");


			BindEvent();
        }

        #endregion
    }
}
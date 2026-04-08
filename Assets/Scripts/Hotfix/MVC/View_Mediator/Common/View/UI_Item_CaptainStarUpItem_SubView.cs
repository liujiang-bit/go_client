// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CaptainStarUpItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_CaptainStarUpItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CaptainStarUpItem";

        public UI_Item_CaptainStarUpItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Item_CaptainStarUpItem_PolygonImage;
		[HideInInspector] public Animator m_UI_Item_CaptainStarUpItem_Animator;

		[HideInInspector] public PolygonImage m_btn_reduce_PolygonImage;
		[HideInInspector] public GameButton m_btn_reduce_GameButton;

		[HideInInspector] public PolygonImage m_img_reduce_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_CaptainStarUpItem_PolygonImage = gameObject.GetComponent<PolygonImage>();
			m_UI_Item_CaptainStarUpItem_Animator = gameObject.GetComponent<Animator>();

			m_btn_reduce_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_reduce");
			m_btn_reduce_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_reduce");

			m_img_reduce_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_reduce/img_reduce");


			BindEvent();
        }

        #endregion
    }
}
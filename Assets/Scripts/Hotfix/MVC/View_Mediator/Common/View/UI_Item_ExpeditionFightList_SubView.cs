// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ExpeditionFightList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ExpeditionFightList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ExpeditionFightList";

        public UI_Item_ExpeditionFightList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ExpeditionFightList;
		[HideInInspector] public PolygonImage m_img_normal_PolygonImage;
		[HideInInspector] public GameButton m_img_normal_GameButton;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_def_PolygonImage;

		[HideInInspector] public PolygonImage m_img_atk_PolygonImage;

		[HideInInspector] public PolygonImage m_img_key_PolygonImage;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public GameButton m_btn_reduce_GameButton;
		[HideInInspector] public Empty4Raycast m_btn_reduce_Empty4Raycast;



        private void UIFinder()
        {       
			m_UI_Item_ExpeditionFightList = gameObject.GetComponent<RectTransform>();
			m_img_normal_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_normal");
			m_img_normal_GameButton = FindUI<GameButton>(gameObject.transform ,"img_normal");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_select");

			m_img_def_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_def");

			m_img_atk_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_atk");

			m_img_key_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_key");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));
			m_btn_reduce_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_reduce");
			m_btn_reduce_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"btn_reduce");


			BindEvent();
        }

        #endregion
    }
}
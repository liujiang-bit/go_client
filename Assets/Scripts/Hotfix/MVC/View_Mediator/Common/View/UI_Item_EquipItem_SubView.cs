// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EquipItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipItem";

        public UI_Item_EquipItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EquipItem;
		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public PolygonImage m_img_can0_PolygonImage;

		[HideInInspector] public PolygonImage m_img_can1_PolygonImage;

		[HideInInspector] public PolygonImage m_img_talent_PolygonImage;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;


        private void UIFinder()
        {       
			m_UI_Item_EquipItem = gameObject.GetComponent<RectTransform>();
			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item"));
			m_img_can0_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_can0");

			m_img_can1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_can1");

			m_img_talent_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_talent");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));

			BindEvent();
        }

        #endregion
    }
}
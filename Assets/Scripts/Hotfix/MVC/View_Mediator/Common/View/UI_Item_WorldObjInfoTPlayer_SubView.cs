// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WorldObjInfoTPlayer_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WorldObjInfoTPlayer_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WorldObjInfoTPlayer";

        public UI_Item_WorldObjInfoTPlayer_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment;

		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line1;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line2;
		[HideInInspector] public UI_Item_WorldObjInfoTextLayer_SubView m_UI_Item_line3;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_head_PolygonImage;
		[HideInInspector] public GameButton m_btn_head_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_head_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Item_line1 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line1"));
			m_UI_Item_line2 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line2"));
			m_UI_Item_line3 = new UI_Item_WorldObjInfoTextLayer_SubView(FindUI<RectTransform>(gameObject.transform ,"textLayer/UI_Item_line3"));
			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_btn_head_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_head");
			m_btn_head_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_head");
			m_btn_head_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_head");


			BindEvent();
        }

        #endregion
    }
}
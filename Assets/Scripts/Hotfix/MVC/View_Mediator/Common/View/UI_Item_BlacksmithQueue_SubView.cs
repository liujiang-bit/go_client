// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BlacksmithQueue_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_BlacksmithQueue_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BlacksmithQueue";

        public UI_Item_BlacksmithQueue_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_BlacksmithQueue;
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_index_LanguageText;

		[HideInInspector] public UI_Model_Item_SubView m_UI_Model_Item;
		[HideInInspector] public PolygonImage m_btn_delete_PolygonImage;
		[HideInInspector] public GameButton m_btn_delete_GameButton;
		[HideInInspector] public BtnAnimation m_btn_delete_BtnAnimation;



        private void UIFinder()
        {       
			m_UI_Item_BlacksmithQueue = gameObject.GetComponent<RectTransform>();
			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_bg");

			m_lbl_index_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_bg/lbl_index");

			m_UI_Model_Item = new UI_Model_Item_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_Item"));
			m_btn_delete_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_delete");
			m_btn_delete_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_delete");
			m_btn_delete_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_delete");


			BindEvent();
        }

        #endregion
    }
}
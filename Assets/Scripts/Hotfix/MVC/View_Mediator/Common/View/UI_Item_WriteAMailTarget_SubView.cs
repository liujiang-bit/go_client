// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WriteAMailTarget_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WriteAMailTarget_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WriteAMailTarget";

        public UI_Item_WriteAMailTarget_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public HorizontalLayoutGroup m_UI_Item_WriteAMailTarget_HorizontalLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_UI_Item_WriteAMailTarget_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_clear_PolygonImage;
		[HideInInspector] public GameButton m_btn_clear_GameButton;
		[HideInInspector] public LayoutElement m_btn_clear_LayoutElement;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_name_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LayoutElement m_pl_layout_LayoutElement;



        private void UIFinder()
        {       
			m_UI_Item_WriteAMailTarget_HorizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();
			m_UI_Item_WriteAMailTarget_ContentSizeFitter = gameObject.GetComponent<ContentSizeFitter>();

			m_btn_clear_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_clear");
			m_btn_clear_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_clear");
			m_btn_clear_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"btn_clear");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_pl_layout_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_layout");


			BindEvent();
        }

        #endregion
    }
}
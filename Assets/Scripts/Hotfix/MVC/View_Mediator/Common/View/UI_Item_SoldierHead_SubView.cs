// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_SoldierHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_SoldierHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_SoldierHead";

        public UI_Item_SoldierHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_SoldierHead_ViewBinder;

		[HideInInspector] public PolygonImage m_img_head_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_head_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_count_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_head_PolygonImage;
		[HideInInspector] public GameButton m_btn_head_GameButton;



        private void UIFinder()
        {       
			m_UI_Item_SoldierHead_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_img_head_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_head");
			m_img_head_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_head");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_head/lbl_count");
			m_lbl_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_head/lbl_count");
			m_lbl_count_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"img_head/lbl_count");

			m_btn_head_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_head/btn_head");
			m_btn_head_GameButton = FindUI<GameButton>(gameObject.transform ,"img_head/btn_head");


			BindEvent();
        }

        #endregion
    }
}
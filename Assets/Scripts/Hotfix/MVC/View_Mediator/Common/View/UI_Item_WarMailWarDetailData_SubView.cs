// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WarMailWarDetailData_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WarMailWarDetailData_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WarMailWarDetailData";

        public UI_Item_WarMailWarDetailData_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WarMailWarDetailData;
		[HideInInspector] public LanguageText m_lbl_title_LanguageText;

		[HideInInspector] public PolygonImage m_btn_more_PolygonImage;
		[HideInInspector] public GameButton m_btn_more_GameButton;

		[HideInInspector] public LanguageText m_lbl_val_LanguageText;

		[HideInInspector] public PolygonImage m_btn_Question_PolygonImage;
		[HideInInspector] public GameButton m_btn_Question_GameButton;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Item_WarMailWarDetailData = gameObject.GetComponent<RectTransform>();
			m_lbl_title_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_title");

			m_btn_more_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_more");
			m_btn_more_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_more");

			m_lbl_val_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_val");

			m_btn_Question_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_Question");
			m_btn_Question_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_Question");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_Question/img_icon");


			BindEvent();
        }

        #endregion
    }
}
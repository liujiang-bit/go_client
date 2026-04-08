// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatOtherEmoji_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChatOtherEmoji_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatOtherEmoji";

        public UI_Item_ChatOtherEmoji_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ChatOtherEmoji_ViewBinder;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_btn_head_PolygonImage;
		[HideInInspector] public GameButton m_btn_head_GameButton;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_spine_emoji_SkeletonGraphic;



        private void UIFinder()
        {       
			m_UI_Item_ChatOtherEmoji_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_PlayerHead"));
			m_btn_head_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_head");
			m_btn_head_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_head");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_spine_emoji_SkeletonGraphic = FindUI<SkeletonGraphic>(gameObject.transform ,"spine_emoji");


			BindEvent();
        }

        #endregion
    }
}
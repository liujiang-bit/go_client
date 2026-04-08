// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChatEmoji_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChatEmoji_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChatEmoji";

        public UI_Item_ChatEmoji_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChatEmoji;
		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_emoji_PolygonImage;
		[HideInInspector] public LongPressBtn m_btn_emoji_LongClickButton;
		[HideInInspector] public UIClickListener m_btn_emoji_UIClickListener;

		[HideInInspector] public SkeletonGraphic m_spine_emoji_SkeletonGraphic;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ChatEmoji = gameObject.GetComponent<RectTransform>();
			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_select");

			m_btn_emoji_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_emoji");
			m_btn_emoji_LongClickButton = FindUI<LongPressBtn>(gameObject.transform ,"btn_emoji");
			m_btn_emoji_UIClickListener = FindUI<UIClickListener>(gameObject.transform ,"btn_emoji");

			m_spine_emoji_SkeletonGraphic = FindUI<SkeletonGraphic>(gameObject.transform ,"btn_emoji/spine_emoji");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");


			BindEvent();
        }

        #endregion
    }
}
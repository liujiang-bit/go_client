// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_AssitResItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_AssitResItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_AssitResItem";

        public UI_Item_AssitResItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_AssitResItem;
		[HideInInspector] public GameSlider m_pb_bar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_bar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_ipt_count_PolygonImage;
		[HideInInspector] public GameInput m_ipt_count_GameInput;
		[HideInInspector] public ArabLayoutCompment m_ipt_count_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_text_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_AssitResItem = gameObject.GetComponent<RectTransform>();
			m_pb_bar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"pb_bar");
			m_pb_bar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pb_bar");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");

			m_ipt_count_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ipt_count");
			m_ipt_count_GameInput = FindUI<GameInput>(gameObject.transform ,"ipt_count");
			m_ipt_count_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ipt_count");

			m_lbl_text_LanguageText = FindUI<LanguageText>(gameObject.transform ,"ipt_count/lbl_text");


			BindEvent();
        }

        #endregion
    }
}
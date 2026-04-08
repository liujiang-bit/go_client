// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChoseChat_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_ChoseChat_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChoseChat";

        public UI_Item_ChoseChat_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChoseChat;
		[HideInInspector] public PolygonImage m_btn_choose_PolygonImage;
		[HideInInspector] public GameButton m_btn_choose_GameButton;
		[HideInInspector] public BtnAnimation m_btn_choose_BtnAnimation;

		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_img_channel_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_channel_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ChoseChat = gameObject.GetComponent<RectTransform>();
			m_btn_choose_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_choose");
			m_btn_choose_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_choose");
			m_btn_choose_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_choose");

			m_img_bg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_choose/img_bg");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_choose/UI_Model_PlayerHead"));
			m_img_channel_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_choose/img_channel");
			m_img_channel_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_choose/img_channel");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_choose/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_choose/lbl_name");


			BindEvent();
        }

        #endregion
    }
}
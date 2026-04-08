// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_QuestReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_QuestReward_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_QuestReward";

        public UI_Item_QuestReward_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_QuestReward_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_btn_PolygonImage;
		[HideInInspector] public GameButton m_btn_btn_GameButton;
		[HideInInspector] public BtnAnimation m_btn_btn_ButtonAnimation;

		[HideInInspector] public LanguageText m_lbl_languageText_LanguageText;

		[HideInInspector] public RectTransform m_img_icon;
		[HideInInspector] public UI_Item_Bag_SubView m_UI_Item_Bag;


        private void UIFinder()
        {       
			m_UI_Item_QuestReward_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_btn_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_btn");
			m_btn_btn_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_btn");
			m_btn_btn_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_btn");

			m_lbl_languageText_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_btn/lbl_languageText");

			m_img_icon = FindUI<RectTransform>(gameObject.transform ,"btn_btn/img_icon");
			m_UI_Item_Bag = new UI_Item_Bag_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_btn/img_icon/UI_Item_Bag"));

			BindEvent();
        }

        #endregion
    }
}
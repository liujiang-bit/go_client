// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_ArmyTrainHead_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_ArmyTrainHead_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_ArmyTrainHead";

        public UI_Model_ArmyTrainHead_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_ArmyTrainHead;
		[HideInInspector] public PolygonImage m_btn_event_PolygonImage;
		[HideInInspector] public GameButton m_btn_event_GameButton;

		[HideInInspector] public PolygonImage m_img_select_PolygonImage;

		[HideInInspector] public PolygonImage m_img_army_icon_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_army_icon_GrayChildrens;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;

		[HideInInspector] public PolygonImage m_btn_dismiss_PolygonImage;
		[HideInInspector] public GameButton m_btn_dismiss_GameButton;
		[HideInInspector] public BtnAnimation m_btn_dismiss_BtnAnimation;

		[HideInInspector] public PolygonImage m_img_up_PolygonImage;



        private void UIFinder()
        {       
			m_UI_Model_ArmyTrainHead = gameObject.GetComponent<RectTransform>();
			m_btn_event_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_event");
			m_btn_event_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_event");

			m_img_select_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_select");

			m_img_army_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_army_icon");
			m_img_army_icon_GrayChildrens = FindUI<GrayChildrens>(gameObject.transform ,"img_army_icon");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_num");

			m_btn_dismiss_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_dismiss");
			m_btn_dismiss_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_dismiss");
			m_btn_dismiss_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_dismiss");

			m_img_up_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_up");


			BindEvent();
        }

        #endregion
    }
}
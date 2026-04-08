// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_Worker_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_Worker_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_Worker";

        public UI_Item_Worker_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_Worker;
		[HideInInspector] public PolygonImage m_img_empty_PolygonImage;

		[HideInInspector] public PolygonImage m_img_place_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_place_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyCount_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_btnTip_LanguageText;

		[HideInInspector] public RectTransform m_UI_Model_StandardButton_Yellow;
		[HideInInspector] public PolygonImage m_btn_languageButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_languageButton_GameButton;
		[HideInInspector] public VerticalLayoutGroup m_btn_languageButton_VerticalLayoutGroup;
		[HideInInspector] public BtnAnimation m_btn_languageButton_ButtonAnimation;

		[HideInInspector] public PolygonImage m_img_img_PolygonImage;
		[HideInInspector] public LayoutElement m_img_img_LayoutElement;

		[HideInInspector] public PolygonImage m_img_forbid_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_line1_LanguageText;
		[HideInInspector] public Shadow m_lbl_line1_Shadow;

		[HideInInspector] public HorizontalLayoutGroup m_pl_line2_HorizontalLayoutGroup;

		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;

		[HideInInspector] public PolygonImage m_img_icon2_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_line2_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_line2_ContentSizeFitter;

		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_UI_Model_DoubleLineButton_Blue;
		[HideInInspector] public UI_Model_StandardButton_Blue2_SubView m_UI_Model_StandardButton_Blue;


        private void UIFinder()
        {       
			m_UI_Item_Worker = gameObject.GetComponent<RectTransform>();
			m_img_empty_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_empty");

			m_img_place_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_place");
			m_img_place_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_place");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(gameObject.transform ,"army/bar/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"army/bar/pb_rogressBar");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"army/bar/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"army/bar/lbl_name");

			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"army/bar/lbl_armyCount");
			m_lbl_armyCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"army/bar/lbl_armyCount");

			m_lbl_btnTip_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btns/lbl_btnTip");

			m_UI_Model_StandardButton_Yellow = FindUI<RectTransform>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow");
			m_btn_languageButton_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton");
			m_btn_languageButton_GameButton = FindUI<GameButton>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton");
			m_btn_languageButton_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton");
			m_btn_languageButton_ButtonAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton");

			m_img_img_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/img_img");
			m_img_img_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/img_img");

			m_img_forbid_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/img_img/img_forbid");

			m_lbl_line1_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/lbl_line1");
			m_lbl_line1_Shadow = FindUI<Shadow>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/lbl_line1");

			m_pl_line2_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/pl_line2");

			m_img_frame_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/pl_line2/img_frame");

			m_img_icon2_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/pl_line2/img_frame/img_icon2");

			m_lbl_line2_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/pl_line2/lbl_line2");
			m_lbl_line2_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"btns/UI_Model_StandardButton_Yellow/btn_languageButton/pl_line2/lbl_line2");

			m_UI_Model_DoubleLineButton_Blue = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/UI_Model_DoubleLineButton_Blue"));
			m_UI_Model_StandardButton_Blue = new UI_Model_StandardButton_Blue2_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/UI_Model_StandardButton_Blue"));

			BindEvent();
        }

        #endregion
    }
}
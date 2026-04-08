// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyConst_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ArmyConst_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyConst";

        public UI_Item_ArmyConst_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_ArmyConst_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_Join_PolygonImage;
		[HideInInspector] public GameButton m_btn_Join_GameButton;

		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyCount_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_captainName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_captainName_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Captain2;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Captain1;
		[HideInInspector] public PolygonImage m_img_arrow_up_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_up_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_down_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_down_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ArmyConst_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_Join_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_Join");
			m_btn_Join_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_Join");

			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_Join/lbl_armyCount");
			m_lbl_armyCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_Join/lbl_armyCount");

			m_lbl_captainName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_Join/lbl_captainName");
			m_lbl_captainName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_Join/lbl_captainName");

			m_UI_Captain2 = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_Join/UI_Captain2"));
			m_UI_Captain1 = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_Join/UI_Captain1"));
			m_img_arrow_up_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_Join/img_arrow_up");
			m_img_arrow_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_Join/img_arrow_up");

			m_img_arrow_down_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_Join/img_arrow_down");
			m_img_arrow_down_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_Join/img_arrow_down");


			BindEvent();
        }

        #endregion
    }
}
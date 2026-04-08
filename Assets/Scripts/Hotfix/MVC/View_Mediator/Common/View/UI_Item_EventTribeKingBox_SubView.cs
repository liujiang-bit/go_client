// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTribeKingBox_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EventTribeKingBox_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTribeKingBox";

        public UI_Item_EventTribeKingBox_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_EventTribeKingBox_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_box_PolygonImage;
		[HideInInspector] public GameButton m_btn_box_GameButton;
		[HideInInspector] public BtnAnimation m_btn_box_BtnAnimation;

		[HideInInspector] public UI_Model_AnimationBox_SubView m_UI_Model_AnimationBox;
		[HideInInspector] public LanguageText m_lbl_target_LanguageText;
		[HideInInspector] public Outline m_lbl_target_Outline;



        private void UIFinder()
        {       
			m_UI_Item_EventTribeKingBox_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_btn_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_box");
			m_btn_box_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_box");
			m_btn_box_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_box");

			m_UI_Model_AnimationBox = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_box/UI_Model_AnimationBox"));
			m_lbl_target_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_box/lbl_target");
			m_lbl_target_Outline = FindUI<Outline>(gameObject.transform ,"btn_box/lbl_target");


			BindEvent();
        }

        #endregion
    }
}
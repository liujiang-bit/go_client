// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_QuestBox_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_QuestBox_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_QuestBox";

        public UI_Item_QuestBox_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_QuestBox_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_box_PolygonImage;
		[HideInInspector] public GameButton m_btn_box_GameButton;

		[HideInInspector] public LanguageText m_lbl_count_LanguageText;

		[HideInInspector] public RectTransform m_pl_bgeffect;
		[HideInInspector] public UI_Model_AnimationBox_SubView m_pl_box;


        private void UIFinder()
        {       
			m_UI_Item_QuestBox_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_btn_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_box");
			m_btn_box_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_box");

			m_lbl_count_LanguageText = FindUI<LanguageText>(gameObject.transform ,"bg/lbl_count");

			m_pl_bgeffect = FindUI<RectTransform>(gameObject.transform ,"pl_bgeffect");
			m_pl_box = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_box"));

			BindEvent();
        }

        #endregion
    }
}
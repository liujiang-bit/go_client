// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_TavernRewardProb_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_TavernRewardProb_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_TavernRewardProb";

        public UI_Item_TavernRewardProb_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_TavernRewardProb;
		[HideInInspector] public PolygonImage m_img_goldbox_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_mes_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_MiniBlue_SubView m_UI_Model_StandardButton_MiniBlue;


        private void UIFinder()
        {       
			m_UI_Item_TavernRewardProb = gameObject.GetComponent<RectTransform>();
			m_img_goldbox_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_goldbox");

			m_lbl_mes_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_mes");

			m_UI_Model_StandardButton_MiniBlue = new UI_Model_StandardButton_MiniBlue_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_StandardButton_MiniBlue"));

			BindEvent();
        }

        #endregion
    }
}
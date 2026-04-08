// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTribeKingScore_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_EventTribeKingScore_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTribeKingScore";

        public UI_Item_EventTribeKingScore_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTribeKingScore;
		[HideInInspector] public LanguageText m_lbl_score_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_score_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_score_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_score_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_score_num_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_EventTribeKingScore = gameObject.GetComponent<RectTransform>();
			m_lbl_score_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_score");
			m_lbl_score_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_score");
			m_lbl_score_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_score");

			m_lbl_score_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_score/lbl_score_num");
			m_lbl_score_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_score/lbl_score_num");


			BindEvent();
        }

        #endregion
    }
}
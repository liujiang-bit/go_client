// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_MailRankChild_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_MailRankChild_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_MailRankChild";

        public UI_Item_MailRankChild_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_MailRankChild;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_score_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_score_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_rank_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_rank_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_rank_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_MailRankChild = gameObject.GetComponent<RectTransform>();
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_lbl_score_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_score");
			m_lbl_score_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_score");

			m_img_rank_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_rank");

			m_lbl_rank_LanguageText = FindUI<LanguageText>(gameObject.transform ,"img_rank/lbl_rank");
			m_lbl_rank_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_rank/lbl_rank");


			BindEvent();
        }

        #endregion
    }
}
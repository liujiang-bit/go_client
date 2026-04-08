// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Model_EquipAtt_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Model_EquipAtt_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Model_EquipAtt";

        public UI_Model_EquipAtt_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Model_EquipAtt;
		[HideInInspector] public LanguageText m_lbl_equipAtt_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_equipAtt_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_equipAtt_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_plus_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_plus_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_plus_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_icon_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Model_EquipAtt = gameObject.GetComponent<RectTransform>();
			m_lbl_equipAtt_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_equipAtt");
			m_lbl_equipAtt_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_equipAtt");
			m_lbl_equipAtt_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_equipAtt");

			m_lbl_plus_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_equipAtt/lbl_plus");
			m_lbl_plus_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"lbl_equipAtt/lbl_plus");
			m_lbl_plus_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_equipAtt/lbl_plus");

			m_img_icon_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_icon");
			m_img_icon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_icon");


			BindEvent();
        }

        #endregion
    }
}
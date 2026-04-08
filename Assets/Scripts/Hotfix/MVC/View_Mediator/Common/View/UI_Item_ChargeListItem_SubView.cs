// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeListItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeListItem";

        public UI_Item_ChargeListItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeListItem;
		[HideInInspector] public GameToggle m_ck_type_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_type_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_redpoint_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_redpoint_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_ChargeListItem = gameObject.GetComponent<RectTransform>();
			m_ck_type_GameToggle = FindUI<GameToggle>(gameObject.transform ,"ck_type");
			m_ck_type_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ck_type");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"ck_type/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ck_type/lbl_name");

			m_img_redpoint_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"ck_type/img_redpoint");
			m_img_redpoint_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"ck_type/img_redpoint");

			m_lbl_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"ck_type/img_redpoint/lbl_num");


			BindEvent();
        }

        #endregion
    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ChargeGrowing_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ChargeGrowing_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ChargeGrowing";

        public UI_Item_ChargeGrowing_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ChargeGrowing;
		[HideInInspector] public LanguageText m_lbl_tip_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_tip_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow2_SubView m_btn_buy;
		[HideInInspector] public ScrollRect m_sv_list_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_list_PolygonImage;
		[HideInInspector] public ListView m_sv_list_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_list_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_ChargeGrowing = gameObject.GetComponent<RectTransform>();
			m_lbl_tip_LanguageText = FindUI<LanguageText>(gameObject.transform ,"top/lbl_tip");
			m_lbl_tip_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"top/lbl_tip");

			m_btn_buy = new UI_Model_DoubleLineButton_Yellow2_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_buy"));
			m_sv_list_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"sv_list");
			m_sv_list_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"sv_list");
			m_sv_list_ListView = FindUI<ListView>(gameObject.transform ,"sv_list");
			m_sv_list_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"sv_list");


			BindEvent();
        }

        #endregion
    }
}
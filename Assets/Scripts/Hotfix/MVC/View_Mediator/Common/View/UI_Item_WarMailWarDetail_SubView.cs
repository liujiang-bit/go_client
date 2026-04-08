// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_WarMailWarDetail_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_WarMailWarDetail_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_WarMailWarDetail";

        public UI_Item_WarMailWarDetail_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_WarMailWarDetail;
		[HideInInspector] public VerticalLayoutGroup m_pl_data_VerticalLayoutGroup;

		[HideInInspector] public UI_Item_WarMailWarDetailData_SubView m_UI_Item_WarMailWarDetailDataTotal;
		[HideInInspector] public UI_Item_WarMailWarDetailData_SubView m_UI_Item_WarMailWarDetailDataHeal;
		[HideInInspector] public UI_Item_WarMailWarDetailData_SubView m_UI_Item_WarMailWarDetailDataDead;
		[HideInInspector] public UI_Item_WarMailWarDetailData_SubView m_UI_Item_WarMailWarDetailDataHeart;
		[HideInInspector] public UI_Item_WarMailWarDetailData_SubView m_UI_Item_WarMailWarDetailDataLast;
		[HideInInspector] public UI_Item_WarMailWarDetailData_SubView m_UI_Item_WarMailWarDetailDataArrow;
		[HideInInspector] public RectTransform m_pl_Specail;
		[HideInInspector] public LanguageText m_lbl_specTitleL_LanguageText;

		[HideInInspector] public LanguageText m_lbl_specvalL_LanguageText;



        private void UIFinder()
        {       
			m_UI_Item_WarMailWarDetail = gameObject.GetComponent<RectTransform>();
			m_pl_data_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_data");

			m_UI_Item_WarMailWarDetailDataTotal = new UI_Item_WarMailWarDetailData_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_data/UI_Item_WarMailWarDetailDataTotal"));
			m_UI_Item_WarMailWarDetailDataHeal = new UI_Item_WarMailWarDetailData_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_data/UI_Item_WarMailWarDetailDataHeal"));
			m_UI_Item_WarMailWarDetailDataDead = new UI_Item_WarMailWarDetailData_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_data/UI_Item_WarMailWarDetailDataDead"));
			m_UI_Item_WarMailWarDetailDataHeart = new UI_Item_WarMailWarDetailData_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_data/UI_Item_WarMailWarDetailDataHeart"));
			m_UI_Item_WarMailWarDetailDataLast = new UI_Item_WarMailWarDetailData_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_data/UI_Item_WarMailWarDetailDataLast"));
			m_UI_Item_WarMailWarDetailDataArrow = new UI_Item_WarMailWarDetailData_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_data/UI_Item_WarMailWarDetailDataArrow"));
			m_pl_Specail = FindUI<RectTransform>(gameObject.transform ,"pl_Specail");
			m_lbl_specTitleL_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Specail/lbl_specTitleL");

			m_lbl_specvalL_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_Specail/lbl_specvalL");


			BindEvent();
        }

        #endregion
    }
}
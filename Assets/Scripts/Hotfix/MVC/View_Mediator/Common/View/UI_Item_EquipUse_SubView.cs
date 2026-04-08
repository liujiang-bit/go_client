// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EquipUse_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EquipUse_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EquipUse";

        public UI_Item_EquipUse_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_EquipUse_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Equipslo1;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Equipslo2;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Equipslo3;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Equipslo4;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Equipslo5;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Equipslo6;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Equipslo7;
		[HideInInspector] public UI_Item_CaptainEquipUse_SubView m_UI_Equipslo8;


        private void UIFinder()
        {       
			m_UI_Item_EquipUse_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_UI_Equipslo1 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Equipslo1"));
			m_UI_Equipslo2 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Equipslo2"));
			m_UI_Equipslo3 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Equipslo3"));
			m_UI_Equipslo4 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Equipslo4"));
			m_UI_Equipslo5 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Equipslo5"));
			m_UI_Equipslo6 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Equipslo6"));
			m_UI_Equipslo7 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Equipslo7"));
			m_UI_Equipslo8 = new UI_Item_CaptainEquipUse_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Equipslo8"));

			BindEvent();
        }

        #endregion
    }
}
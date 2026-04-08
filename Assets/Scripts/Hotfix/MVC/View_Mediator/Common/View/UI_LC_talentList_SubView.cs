// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_LC_talentList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_LC_talentList_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_LC_talentList";

        public UI_LC_talentList_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_LC_talentList;
		[HideInInspector] public GridLayoutGroup m_pl_mes_GridLayoutGroup;

		[HideInInspector] public UI_Item_TalentSkill_SubView m_UI_Item_TalentSkill1;
		[HideInInspector] public UI_Item_TalentSkill_SubView m_UI_Item_TalentSkill2;
		[HideInInspector] public UI_Item_TalentSkill_SubView m_UI_Item_TalentSkill3;


        private void UIFinder()
        {       
			m_UI_LC_talentList = gameObject.GetComponent<RectTransform>();
			m_pl_mes_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes");

			m_UI_Item_TalentSkill1 = new UI_Item_TalentSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/UI_Item_TalentSkill1"));
			m_UI_Item_TalentSkill2 = new UI_Item_TalentSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/UI_Item_TalentSkill2"));
			m_UI_Item_TalentSkill3 = new UI_Item_TalentSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/UI_Item_TalentSkill3"));

			BindEvent();
        }

        #endregion
    }
}
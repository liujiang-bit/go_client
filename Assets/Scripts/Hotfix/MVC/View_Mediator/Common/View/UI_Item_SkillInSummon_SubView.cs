// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_SkillInSummon_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_SkillInSummon_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_SkillInSummon";

        public UI_Item_SkillInSummon_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ArabLayoutCompment m_UI_Item_SkillInSummon_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_skillsbg_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_skillsbg_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_skillicons_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_skillicons_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill_1;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill_2;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill_3;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill_4;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill_5;


        private void UIFinder()
        {       
			m_UI_Item_SkillInSummon_ArabLayoutCompment = gameObject.GetComponent<ArabLayoutCompment>();

			m_img_skillsbg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"img_skillsbg");
			m_img_skillsbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_skillsbg");

			m_pl_skillicons_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"img_skillsbg/pl_skillicons");
			m_pl_skillicons_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"img_skillsbg/pl_skillicons");

			m_UI_Item_CaptainSkill_1 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"img_skillsbg/pl_skillicons/UI_Item_CaptainSkill_1"));
			m_UI_Item_CaptainSkill_2 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"img_skillsbg/pl_skillicons/UI_Item_CaptainSkill_2"));
			m_UI_Item_CaptainSkill_3 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"img_skillsbg/pl_skillicons/UI_Item_CaptainSkill_3"));
			m_UI_Item_CaptainSkill_4 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"img_skillsbg/pl_skillicons/UI_Item_CaptainSkill_4"));
			m_UI_Item_CaptainSkill_5 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"img_skillsbg/pl_skillicons/UI_Item_CaptainSkill_5"));

			BindEvent();
        }

        #endregion
    }
}
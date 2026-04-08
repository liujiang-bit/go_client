// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_BattleTroopsTipsCaptain_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_BattleTroopsTipsCaptain_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_BattleTroopsTipsCaptain";

        public UI_Item_BattleTroopsTipsCaptain_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_UI_Item_BattleTroopsTipsCaptain_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_stars_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_stars_ArabLayoutCompment;

		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar1;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar2;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar3;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar4;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar5;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar6;
		[HideInInspector] public LanguageText m_lbl_level_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_level_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_skill_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_skill_GridLayoutGroup;
		[HideInInspector] public LayoutElement m_pl_skill_LayoutElement;

		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill1;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill2;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill3;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill4;
		[HideInInspector] public UI_Item_CaptainSkill_SubView m_UI_Item_CaptainSkill5;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;


        private void UIFinder()
        {       
			m_UI_Item_BattleTroopsTipsCaptain_PolygonImage = gameObject.GetComponent<PolygonImage>();

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_name");

			m_pl_stars_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_stars");
			m_pl_stars_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_stars");

			m_UI_Model_HeadStar1 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar1"));
			m_UI_Model_HeadStar2 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar2"));
			m_UI_Model_HeadStar3 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar3"));
			m_UI_Model_HeadStar4 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar4"));
			m_UI_Model_HeadStar5 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar5"));
			m_UI_Model_HeadStar6 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_stars/UI_Model_HeadStar6"));
			m_lbl_level_LanguageText = FindUI<LanguageText>(gameObject.transform ,"lbl_level");
			m_lbl_level_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"lbl_level");

			m_pl_skill_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_skill");
			m_pl_skill_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_skill");
			m_pl_skill_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_skill");

			m_UI_Item_CaptainSkill1 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill/UI_Item_CaptainSkill1"));
			m_UI_Item_CaptainSkill2 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill/UI_Item_CaptainSkill2"));
			m_UI_Item_CaptainSkill3 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill/UI_Item_CaptainSkill3"));
			m_UI_Item_CaptainSkill4 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill/UI_Item_CaptainSkill4"));
			m_UI_Item_CaptainSkill5 = new UI_Item_CaptainSkill_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_skill/UI_Item_CaptainSkill5"));
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));

			BindEvent();
        }

        #endregion
    }
}
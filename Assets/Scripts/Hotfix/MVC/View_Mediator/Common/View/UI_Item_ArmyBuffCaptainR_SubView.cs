// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyBuffCaptainR_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ArmyBuffCaptainR_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyBuffCaptainR";

        public UI_Item_ArmyBuffCaptainR_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ArmyBuffCaptainR;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar1;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar2;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar3;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar4;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar5;
		[HideInInspector] public UI_Model_HeadStar_SubView m_UI_Model_HeadStar6;
		[HideInInspector] public UI_Item_CaptainSkill_SizeMid_SubView m_UI_Item_CaptainSkill1;
		[HideInInspector] public UI_Item_CaptainSkill_SizeMid_SubView m_UI_Item_CaptainSkill2;
		[HideInInspector] public UI_Item_CaptainSkill_SizeMid_SubView m_UI_Item_CaptainSkill3;
		[HideInInspector] public UI_Item_CaptainSkill_SizeMid_SubView m_UI_Item_CaptainSkill4;
		[HideInInspector] public UI_Item_CaptainSkill_SizeMid_SubView m_UI_Item_CaptainSkill5;


        private void UIFinder()
        {       
			m_UI_Item_ArmyBuffCaptainR = gameObject.GetComponent<RectTransform>();
			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(gameObject.transform ,"UI_Model_CaptainHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"NamenStar/lbl_name");

			m_UI_Model_HeadStar1 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"NamenStar/stars/UI_Model_HeadStar1"));
			m_UI_Model_HeadStar2 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"NamenStar/stars/UI_Model_HeadStar2"));
			m_UI_Model_HeadStar3 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"NamenStar/stars/UI_Model_HeadStar3"));
			m_UI_Model_HeadStar4 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"NamenStar/stars/UI_Model_HeadStar4"));
			m_UI_Model_HeadStar5 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"NamenStar/stars/UI_Model_HeadStar5"));
			m_UI_Model_HeadStar6 = new UI_Model_HeadStar_SubView(FindUI<RectTransform>(gameObject.transform ,"NamenStar/stars/UI_Model_HeadStar6"));
			m_UI_Item_CaptainSkill1 = new UI_Item_CaptainSkill_SizeMid_SubView(FindUI<RectTransform>(gameObject.transform ,"skills/UI_Item_CaptainSkill1"));
			m_UI_Item_CaptainSkill2 = new UI_Item_CaptainSkill_SizeMid_SubView(FindUI<RectTransform>(gameObject.transform ,"skills/UI_Item_CaptainSkill2"));
			m_UI_Item_CaptainSkill3 = new UI_Item_CaptainSkill_SizeMid_SubView(FindUI<RectTransform>(gameObject.transform ,"skills/UI_Item_CaptainSkill3"));
			m_UI_Item_CaptainSkill4 = new UI_Item_CaptainSkill_SizeMid_SubView(FindUI<RectTransform>(gameObject.transform ,"skills/UI_Item_CaptainSkill4"));
			m_UI_Item_CaptainSkill5 = new UI_Item_CaptainSkill_SizeMid_SubView(FindUI<RectTransform>(gameObject.transform ,"skills/UI_Item_CaptainSkill5"));

			BindEvent();
        }

        #endregion
    }
}
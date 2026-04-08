// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_CMDBtns_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_CMDBtns_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_CMDBtns";

        public UI_Item_CMDBtns_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public Animator m_UI_Item_CMDBtns_Animator;

		[HideInInspector] public RectTransform m_pl_tp11;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn0;
		[HideInInspector] public RectTransform m_pl_tp21;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn;
		[HideInInspector] public RectTransform m_pl_tp22;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn1;
		[HideInInspector] public RectTransform m_pl_tp31;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn3;
		[HideInInspector] public RectTransform m_pl_tp32;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn4;
		[HideInInspector] public RectTransform m_pl_tp33;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn5;
		[HideInInspector] public RectTransform m_pl_tp41;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn6;
		[HideInInspector] public RectTransform m_pl_tp42;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn7;
		[HideInInspector] public RectTransform m_pl_tp43;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn8;
		[HideInInspector] public RectTransform m_pl_tp44;
		[HideInInspector] public UI_Model_CommandBtn_SubView m_UI_Model_CommandBtn9;


        private void UIFinder()
        {       
			m_UI_Item_CMDBtns_Animator = gameObject.GetComponent<Animator>();

			m_pl_tp11 = FindUI<RectTransform>(gameObject.transform ,"btns/onePoint/pl_tp11");
			m_UI_Model_CommandBtn0 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/onePoint/pl_tp11/UI_Model_CommandBtn0"));
			m_pl_tp21 = FindUI<RectTransform>(gameObject.transform ,"btns/twoPoint/pl_tp21");
			m_UI_Model_CommandBtn = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/twoPoint/pl_tp21/UI_Model_CommandBtn"));
			m_pl_tp22 = FindUI<RectTransform>(gameObject.transform ,"btns/twoPoint/pl_tp22");
			m_UI_Model_CommandBtn1 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/twoPoint/pl_tp22/UI_Model_CommandBtn1"));
			m_pl_tp31 = FindUI<RectTransform>(gameObject.transform ,"btns/threePoint/pl_tp31");
			m_UI_Model_CommandBtn3 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/threePoint/pl_tp31/UI_Model_CommandBtn3"));
			m_pl_tp32 = FindUI<RectTransform>(gameObject.transform ,"btns/threePoint/pl_tp32");
			m_UI_Model_CommandBtn4 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/threePoint/pl_tp32/UI_Model_CommandBtn4"));
			m_pl_tp33 = FindUI<RectTransform>(gameObject.transform ,"btns/threePoint/pl_tp33");
			m_UI_Model_CommandBtn5 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/threePoint/pl_tp33/UI_Model_CommandBtn5"));
			m_pl_tp41 = FindUI<RectTransform>(gameObject.transform ,"btns/fourPoint/pl_tp41");
			m_UI_Model_CommandBtn6 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/fourPoint/pl_tp41/UI_Model_CommandBtn6"));
			m_pl_tp42 = FindUI<RectTransform>(gameObject.transform ,"btns/fourPoint/pl_tp42");
			m_UI_Model_CommandBtn7 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/fourPoint/pl_tp42/UI_Model_CommandBtn7"));
			m_pl_tp43 = FindUI<RectTransform>(gameObject.transform ,"btns/fourPoint/pl_tp43");
			m_UI_Model_CommandBtn8 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/fourPoint/pl_tp43/UI_Model_CommandBtn8"));
			m_pl_tp44 = FindUI<RectTransform>(gameObject.transform ,"btns/fourPoint/pl_tp44");
			m_UI_Model_CommandBtn9 = new UI_Model_CommandBtn_SubView(FindUI<RectTransform>(gameObject.transform ,"btns/fourPoint/pl_tp44/UI_Model_CommandBtn9"));

			BindEvent();
        }

        #endregion
    }
}
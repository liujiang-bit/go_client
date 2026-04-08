// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_ArmyBuff_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_ArmyBuff_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_ArmyBuff";

        public UI_Item_ArmyBuff_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_ArmyBuff;
		[HideInInspector] public RectTransform m_pl_plplayerL;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHeadL;
		[HideInInspector] public LanguageText m_lbl_nameL_LanguageText;

		[HideInInspector] public RectTransform m_pl_captainL;
		[HideInInspector] public UI_Item_ArmyBuffCaptainL_SubView m_UI_Item_ArmyBuffCaptainL1;
		[HideInInspector] public UI_Item_ArmyBuffCaptainL_SubView m_UI_Item_ArmyBuffCaptainL2;
		[HideInInspector] public RectTransform m_pl_dataL;
		[HideInInspector] public UI_Item_ArmyBuffData_SubView m_UI_Item_ArmyBuffData;
		[HideInInspector] public RectTransform m_pl_playerR;
		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHeadR;
		[HideInInspector] public LanguageText m_lbl_nameR_LanguageText;

		[HideInInspector] public RectTransform m_pl_captainR;
		[HideInInspector] public UI_Item_ArmyBuffCaptainR_SubView m_UI_Item_ArmyBuffCaptainR1;
		[HideInInspector] public UI_Item_ArmyBuffCaptainR_SubView m_UI_Item_ArmyBuffCaptainR2;
		[HideInInspector] public RectTransform m_pl_dataR;


        private void UIFinder()
        {       
			m_UI_Item_ArmyBuff = gameObject.GetComponent<RectTransform>();
			m_pl_plplayerL = FindUI<RectTransform>(gameObject.transform ,"rectL/pl_plplayerL");
			m_UI_Model_PlayerHeadL = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"rectL/pl_plplayerL/UI_Model_PlayerHeadL"));
			m_lbl_nameL_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rectL/pl_plplayerL/lbl_nameL");

			m_pl_captainL = FindUI<RectTransform>(gameObject.transform ,"rectL/pl_captainL");
			m_UI_Item_ArmyBuffCaptainL1 = new UI_Item_ArmyBuffCaptainL_SubView(FindUI<RectTransform>(gameObject.transform ,"rectL/pl_captainL/UI_Item_ArmyBuffCaptainL1"));
			m_UI_Item_ArmyBuffCaptainL2 = new UI_Item_ArmyBuffCaptainL_SubView(FindUI<RectTransform>(gameObject.transform ,"rectL/pl_captainL/UI_Item_ArmyBuffCaptainL2"));
			m_pl_dataL = FindUI<RectTransform>(gameObject.transform ,"rectL/pl_dataL");
			m_UI_Item_ArmyBuffData = new UI_Item_ArmyBuffData_SubView(FindUI<RectTransform>(gameObject.transform ,"rectL/pl_dataL/UI_Item_ArmyBuffData"));
			m_pl_playerR = FindUI<RectTransform>(gameObject.transform ,"rectR/pl_playerR");
			m_UI_Model_PlayerHeadR = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"rectR/pl_playerR/UI_Model_PlayerHeadR"));
			m_lbl_nameR_LanguageText = FindUI<LanguageText>(gameObject.transform ,"rectR/pl_playerR/lbl_nameR");

			m_pl_captainR = FindUI<RectTransform>(gameObject.transform ,"rectR/pl_captainR");
			m_UI_Item_ArmyBuffCaptainR1 = new UI_Item_ArmyBuffCaptainR_SubView(FindUI<RectTransform>(gameObject.transform ,"rectR/pl_captainR/UI_Item_ArmyBuffCaptainR1"));
			m_UI_Item_ArmyBuffCaptainR2 = new UI_Item_ArmyBuffCaptainR_SubView(FindUI<RectTransform>(gameObject.transform ,"rectR/pl_captainR/UI_Item_ArmyBuffCaptainR2"));
			m_pl_dataR = FindUI<RectTransform>(gameObject.transform ,"rectR/pl_dataR");

			BindEvent();
        }

        #endregion
    }
}
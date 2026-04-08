// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月25日
// Update Time         :    2020年2月25日
// Class Description   :    UI_Pop_WorldArmyHeadView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Pop_WorldArmyHeadView : GameView
    {
		public const string VIEW_NAME = "UI_Pop_WorldArmyHead";

        public UI_Pop_WorldArmyHeadView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public LanguageText m_lbl_playerName_LanguageText;

		[HideInInspector] public GameSlider m_pb_Hp_GameSlider;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHead;
		[HideInInspector] public UI_Item_WorldArmyCmdAp_SubView m_UI_Item_WorldArmyCmdAp;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_lbl_playerName_LanguageText = FindUI<LanguageText>(vb.transform ,"head/lbl_playerName");

			m_pb_Hp_GameSlider = FindUI<GameSlider>(vb.transform ,"head/pb_Hp");

			m_UI_Model_CaptainHead = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"head/UI_Model_CaptainHead"));
			m_UI_Item_WorldArmyCmdAp = new UI_Item_WorldArmyCmdAp_SubView(FindUI<RectTransform>(vb.transform ,"head/UI_Item_WorldArmyCmdAp"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
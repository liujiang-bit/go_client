// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月15日
// Update Time         :    2020年4月15日
// Class Description   :    UI_Item_SearchFriendView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_SearchFriendView : GameView
    {
		public const string VIEW_NAME = "UI_Item_SearchFriend";

        public UI_Item_SearchFriendView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_bg_PolygonImage;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public PolygonImage m_btn_addfriend_PolygonImage;
		[HideInInspector] public GameButton m_btn_addfriend_GameButton;
		[HideInInspector] public BtnAnimation m_btn_addfriend_ButtonAnimation;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_bg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_bg");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"UI_Model_PlayerHead"));
			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_name");

			m_btn_addfriend_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_addfriend");
			m_btn_addfriend_GameButton = FindUI<GameButton>(vb.transform ,"btn_addfriend");
			m_btn_addfriend_ButtonAnimation = FindUI<BtnAnimation>(vb.transform ,"btn_addfriend");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
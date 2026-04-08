// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 8, 2020
// Update Time         :    Wednesday, April 8, 2020
// Class Description   :    UI_LC_GuildFlagImgView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_LC_GuildFlagImgView : GameView
    {
		public const string VIEW_NAME = "UI_LC_GuildFlagImg";

        public UI_LC_GuildFlagImgView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public UI_Item_GuildFlagImg_SubView m_UI_GuildFlagImg0;
		[HideInInspector] public UI_Item_GuildFlagImg_SubView m_UI_GuildFlagImg1;
		[HideInInspector] public UI_Item_GuildFlagImg_SubView m_UI_GuildFlagImg2;
		[HideInInspector] public UI_Item_GuildFlagImg_SubView m_UI_GuildFlagImg3;
		[HideInInspector] public UI_Item_GuildFlagImg_SubView m_UI_GuildFlagImg4;
		[HideInInspector] public UI_Item_GuildFlagImg_SubView m_UI_GuildFlagImg5;
		[HideInInspector] public UI_Item_GuildFlagImg_SubView m_UI_GuildFlagImg6;
		[HideInInspector] public UI_Item_GuildFlagImg_SubView m_UI_GuildFlagImg7;


        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_UI_GuildFlagImg0 = new UI_Item_GuildFlagImg_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlagImg0"));
			m_UI_GuildFlagImg1 = new UI_Item_GuildFlagImg_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlagImg1"));
			m_UI_GuildFlagImg2 = new UI_Item_GuildFlagImg_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlagImg2"));
			m_UI_GuildFlagImg3 = new UI_Item_GuildFlagImg_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlagImg3"));
			m_UI_GuildFlagImg4 = new UI_Item_GuildFlagImg_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlagImg4"));
			m_UI_GuildFlagImg5 = new UI_Item_GuildFlagImg_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlagImg5"));
			m_UI_GuildFlagImg6 = new UI_Item_GuildFlagImg_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlagImg6"));
			m_UI_GuildFlagImg7 = new UI_Item_GuildFlagImg_SubView(FindUI<RectTransform>(vb.transform ,"UI_GuildFlagImg7"));


        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
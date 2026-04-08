// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月5日
// Update Time         :    2019年12月5日
// Class Description   :    UIHudBuildingMenuView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UIHudBuildingMenuView : GameView
    {
		public const string VIEW_NAME = "UI_Hud_BuildingMenu";

        public UIHudBuildingMenuView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public Image m_img_name_Image;

		[HideInInspector] public Text m_lbl_name_Text;

		[HideInInspector] public Image m_img_bg_Image;

		[HideInInspector] public HorizontalLayoutGroup m_pl_btncontains_HorizontalLayoutGroup;
		[HideInInspector] public ContentSizeFitter m_pl_btncontains_ContentSizeFitter;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;
            m_img_name_Image = FindUI<Image>(vb.transform ,"img_name");

			m_lbl_name_Text = FindUI<Text>(vb.transform ,"img_name/lbl_name");

			m_img_bg_Image = FindUI<Image>(vb.transform ,"img_bg");

			m_pl_btncontains_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(vb.transform ,"pl_btncontains");
			m_pl_btncontains_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_btncontains");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
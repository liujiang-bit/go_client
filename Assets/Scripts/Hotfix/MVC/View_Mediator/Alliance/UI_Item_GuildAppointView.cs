// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, May 6, 2020
// Update Time         :    Wednesday, May 6, 2020
// Class Description   :    UI_Item_GuildAppointView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_GuildAppointView : GameView
    {
		public const string VIEW_NAME = "UI_Item_GuildAppoint";

        public UI_Item_GuildAppointView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_icon_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;

		[HideInInspector] public LanguageText m_lbl_desc_LanguageText;

		[HideInInspector] public GridLayoutGroup m_pl_att_GridLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_pow1_LanguageText;

		[HideInInspector] public LanguageText m_lbl_pow2_LanguageText;

		[HideInInspector] public GameToggle m_ck_choose_GameToggle;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_icon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"rect/img_icon");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_name");

			m_lbl_desc_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/lbl_desc");

			m_pl_att_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"rect/pl_att");

			m_lbl_pow1_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_att/lbl_pow1");

			m_lbl_pow2_LanguageText = FindUI<LanguageText>(vb.transform ,"rect/pl_att/lbl_pow2");

			m_ck_choose_GameToggle = FindUI<GameToggle>(vb.transform ,"rect/ck_choose");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_PlayerManageItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_PlayerManageItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_PlayerManageItem";

        public UI_Item_PlayerManageItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public ViewBinder m_UI_Item_PlayerManageItem_ViewBinder;

		[HideInInspector] public PolygonImage m_btn_player_PolygonImage;
		[HideInInspector] public GameButton m_btn_player_GameButton;
		[HideInInspector] public BtnAnimation m_btn_player_BtnAnimation;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kingdom_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdom_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_kingdomName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_kingdomName_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_power_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_power_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_Model_PlayerHead;
		[HideInInspector] public PolygonImage m_img_ck_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_ck_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_creat_PolygonImage;
		[HideInInspector] public GameButton m_btn_creat_GameButton;
		[HideInInspector] public BtnAnimation m_btn_creat_BtnAnimation;

		[HideInInspector] public LanguageText m_lbl_CreatName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_CreatName_ArabLayoutCompment;



        private void UIFinder()
        {       
			m_UI_Item_PlayerManageItem_ViewBinder = gameObject.GetComponent<ViewBinder>();

			m_btn_player_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_player");
			m_btn_player_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_player");
			m_btn_player_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_player");

			m_lbl_name_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_player/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_player/lbl_name");

			m_lbl_kingdom_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_player/lbl_kingdom");
			m_lbl_kingdom_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_player/lbl_kingdom");

			m_lbl_kingdomName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_player/lbl_kingdomName");
			m_lbl_kingdomName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_player/lbl_kingdomName");

			m_lbl_power_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_player/lbl_power");
			m_lbl_power_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_player/lbl_power");

			m_UI_Model_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(gameObject.transform ,"btn_player/UI_Model_PlayerHead"));
			m_img_ck_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_player/img_ck");
			m_img_ck_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_player/img_ck");

			m_btn_creat_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"btn_creat");
			m_btn_creat_GameButton = FindUI<GameButton>(gameObject.transform ,"btn_creat");
			m_btn_creat_BtnAnimation = FindUI<BtnAnimation>(gameObject.transform ,"btn_creat");

			m_lbl_CreatName_LanguageText = FindUI<LanguageText>(gameObject.transform ,"btn_creat/lbl_CreatName");
			m_lbl_CreatName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"btn_creat/lbl_CreatName");


			BindEvent();
        }

        #endregion
    }
}
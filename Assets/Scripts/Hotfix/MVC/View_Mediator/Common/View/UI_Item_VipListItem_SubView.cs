// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_VipListItem_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;

namespace Game {
    public partial class UI_Item_VipListItem_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_VipListItem";

        public UI_Item_VipListItem_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_VipListItem;
		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_titleBox_LanguageText;

		[HideInInspector] public RectTransform m_pl_free;
		[HideInInspector] public RectTransform m_pl_goldBlet;
		[HideInInspector] public LanguageText m_lbl_titlefree_LanguageText;

		[HideInInspector] public PolygonImage m_btn_freebox_PolygonImage;
		[HideInInspector] public GameButton m_btn_freebox_GameButton;

		[HideInInspector] public UI_Model_AnimationBox_SubView m_pl_freebox;
		[HideInInspector] public UI_Model_StandardButton_MiniGreen_SubView m_btn_get;
		[HideInInspector] public LanguageText m_lbl_time_LanguageText;

		[HideInInspector] public RectTransform m_pl_speical;
		[HideInInspector] public RectTransform m_pl_greenBlet;
		[HideInInspector] public LanguageText m_lbl_titlespeical_LanguageText;

		[HideInInspector] public PolygonImage m_btn_speicalbox_PolygonImage;
		[HideInInspector] public GameButton m_btn_speicalbox_GameButton;

		[HideInInspector] public UI_Model_AnimationBox_SubView m_pl_speicalbox;
		[HideInInspector] public UI_Model_StandardButton_MiniYellow_SubView m_btn_buy;
		[HideInInspector] public LanguageText m_lbl_soldOut_LanguageText;

		[HideInInspector] public ArabLayoutCompment m_pl_right_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_titleBuff_LanguageText;

		[HideInInspector] public ScrollRect m_sv_listBuff_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_listBuff_PolygonImage;
		[HideInInspector] public ListView m_sv_listBuff_ListView;



        private void UIFinder()
        {       
			m_UI_Item_VipListItem = gameObject.GetComponent<RectTransform>();
			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_left");

			m_lbl_titleBox_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_left/lbl_titleBox");

			m_pl_free = FindUI<RectTransform>(gameObject.transform ,"pl_left/pl_free");
			m_pl_goldBlet = FindUI<RectTransform>(gameObject.transform ,"pl_left/pl_free/pl_goldBlet");
			m_lbl_titlefree_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_left/pl_free/lbl_titlefree");

			m_btn_freebox_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_left/pl_free/btn_freebox");
			m_btn_freebox_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_left/pl_free/btn_freebox");

			m_pl_freebox = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/pl_free/btn_freebox/pl_freebox"));
			m_btn_get = new UI_Model_StandardButton_MiniGreen_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/pl_free/btn_get"));
			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_left/pl_free/lbl_time");

			m_pl_speical = FindUI<RectTransform>(gameObject.transform ,"pl_left/pl_speical");
			m_pl_greenBlet = FindUI<RectTransform>(gameObject.transform ,"pl_left/pl_speical/bg/pl_greenBlet");
			m_lbl_titlespeical_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_left/pl_speical/bg/lbl_titlespeical");

			m_btn_speicalbox_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_left/pl_speical/bg/btn_speicalbox");
			m_btn_speicalbox_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_left/pl_speical/bg/btn_speicalbox");

			m_pl_speicalbox = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/pl_speical/bg/btn_speicalbox/pl_speicalbox"));
			m_btn_buy = new UI_Model_StandardButton_MiniYellow_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_left/pl_speical/bg/btn_buy"));
			m_lbl_soldOut_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_left/pl_speical/lbl_soldOut");

			m_pl_right_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_right");

			m_lbl_titleBuff_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_right/lbl_titleBuff");

			m_sv_listBuff_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_right/sv_listBuff");
			m_sv_listBuff_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_right/sv_listBuff");
			m_sv_listBuff_ListView = FindUI<ListView>(gameObject.transform ,"pl_right/sv_listBuff");


			BindEvent();
        }

        #endregion
    }
}
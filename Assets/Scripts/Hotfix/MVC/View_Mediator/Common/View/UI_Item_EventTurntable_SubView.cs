// =============================================================================== 
// Author              :    Gen By Tools
// Class Description   :    UI_Item_EventTurntable_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public partial class UI_Item_EventTurntable_SubView : UI_SubView
    {
		public const string VIEW_NAME = "UI_Item_EventTurntable";

        public UI_Item_EventTurntable_SubView (RectTransform transform) 
        {
            m_root_RectTransform = transform;
            this.gameObject = m_root_RectTransform.gameObject;     
            UIFinder();
        }

        #region gen ui code 
		[HideInInspector] public RectTransform m_UI_Item_EventTurntable;
		[HideInInspector] public RectTransform m_pl_mes;
		[HideInInspector] public PolygonImage m_img_activitybg_PolygonImage;

		[HideInInspector] public PolygonImage m_img_turntable_PolygonImage;
		[HideInInspector] public Animation m_img_turntable_Animation;

		[HideInInspector] public Empty4Raycast m_pl_rewards_Empty4Raycast;

		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable1;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable2;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable3;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable4;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable5;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable6;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable7;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable8;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable9;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable10;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable11;
		[HideInInspector] public UI_Item_EventTurntableItem_SubView m_UI_Item_EventTurntable12;
		[HideInInspector] public PolygonImage m_img_turntablefra_PolygonImage;

		[HideInInspector] public PolygonImage m_img_turntable_arrow_PolygonImage;

		[HideInInspector] public PolygonImage m_img_turntable_arrow_1_PolygonImage;
		[HideInInspector] public Animation m_img_turntable_arrow_1_Animation;

		[HideInInspector] public RectTransform m_pl_effect;
		[HideInInspector] public RectTransform m_pl_box;
		[HideInInspector] public RectTransform m_pl_effect_box;
		[HideInInspector] public UI_Model_AnimationBox_SubView m_img_box;
		[HideInInspector] public LanguageText m_lbl_box_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_box_num_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_ex_box_PolygonImage;
		[HideInInspector] public GameButton m_btn_ex_box_GameButton;

		[HideInInspector] public LanguageText m_lbl_chance_LanguageText;
		[HideInInspector] public LayoutElement m_lbl_chance_LayoutElement;
		[HideInInspector] public Outline m_lbl_chance_Outline;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public LayoutElement m_lbl_time_LayoutElement;
		[HideInInspector] public Outline m_lbl_time_Outline;

		[HideInInspector] public GridLayoutGroup m_pl_btn_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_btn_ArabLayoutCompment;

		[HideInInspector] public UI_Model_DoubleLineButton_Blue2_SubView m_btn_free;
		[HideInInspector] public RectTransform m_pl_one;
		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_one;
		[HideInInspector] public PolygonImage m_img_discount_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_discount_LanguageText;

		[HideInInspector] public UI_Model_StandardButton_Yellow2_SubView m_btn_more;
		[HideInInspector] public SkeletonGraphic m_pl_char_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_pl_char_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public CanvasGroup m_pl_info_CanvasGroup;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_info_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_info_PolygonImage;
		[HideInInspector] public ListView m_sv_info_ListView;

		[HideInInspector] public PolygonImage m_v_info_PolygonImage;
		[HideInInspector] public Mask m_v_info_Mask;

		[HideInInspector] public ContentSizeFitter m_c_info_ContentSizeFitter;
		[HideInInspector] public VerticalLayoutGroup m_c_info_VerticalLayoutGroup;

		[HideInInspector] public LanguageText m_lbl_info_LanguageText;
		[HideInInspector] public ContentSizeFitter m_lbl_info_ContentSizeFitter;
		[HideInInspector] public ArabLayoutCompment m_lbl_info_ArabLayoutCompment;

		[HideInInspector] public VerticalLayoutGroup m_pl_probMes_VerticalLayoutGroup;

		[HideInInspector] public RectTransform m_pl_title;
		[HideInInspector] public UI_Item_EventTurntablePro_SubView m_UI_Item_EventTurntablePro;


        private void UIFinder()
        {       
			m_UI_Item_EventTurntable = gameObject.GetComponent<RectTransform>();
			m_pl_mes = FindUI<RectTransform>(gameObject.transform ,"pl_mes");
			m_img_activitybg_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_activitybg");

			m_img_turntable_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_turntable");
			m_img_turntable_Animation = FindUI<Animation>(gameObject.transform ,"pl_mes/img_turntable");

			m_pl_rewards_Empty4Raycast = FindUI<Empty4Raycast>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards");

			m_UI_Item_EventTurntable1 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable1"));
			m_UI_Item_EventTurntable2 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable2"));
			m_UI_Item_EventTurntable3 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable3"));
			m_UI_Item_EventTurntable4 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable4"));
			m_UI_Item_EventTurntable5 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable5"));
			m_UI_Item_EventTurntable6 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable6"));
			m_UI_Item_EventTurntable7 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable7"));
			m_UI_Item_EventTurntable8 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable8"));
			m_UI_Item_EventTurntable9 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable9"));
			m_UI_Item_EventTurntable10 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable10"));
			m_UI_Item_EventTurntable11 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable11"));
			m_UI_Item_EventTurntable12 = new UI_Item_EventTurntableItem_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/img_turntable/pl_rewards/UI_Item_EventTurntable12"));
			m_img_turntablefra_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_turntablefra");

			m_img_turntable_arrow_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_turntable_arrow");

			m_img_turntable_arrow_1_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/img_turntable_arrow/img_turntable_arrow_1");
			m_img_turntable_arrow_1_Animation = FindUI<Animation>(gameObject.transform ,"pl_mes/img_turntable_arrow/img_turntable_arrow_1");

			m_pl_effect = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_effect");
			m_pl_box = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_box");
			m_pl_effect_box = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_box/pl_effect_box");
			m_img_box = new UI_Model_AnimationBox_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_box/img_box"));
			m_lbl_box_num_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_box/lbl_box_num");
			m_lbl_box_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_box/lbl_box_num");

			m_btn_ex_box_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_box/btn_ex_box");
			m_btn_ex_box_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/pl_box/btn_ex_box");

			m_lbl_chance_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_chance");
			m_lbl_chance_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/lbl_chance");
			m_lbl_chance_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/lbl_chance");

			m_lbl_time_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/lbl_time");
			m_lbl_time_LayoutElement = FindUI<LayoutElement>(gameObject.transform ,"pl_mes/lbl_time");
			m_lbl_time_Outline = FindUI<Outline>(gameObject.transform ,"pl_mes/lbl_time");

			m_pl_btn_GridLayoutGroup = FindUI<GridLayoutGroup>(gameObject.transform ,"pl_mes/pl_btn");
			m_pl_btn_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_btn");

			m_btn_free = new UI_Model_DoubleLineButton_Blue2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_btn/btn_free"));
			m_pl_one = FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_btn/pl_one");
			m_btn_one = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_btn/pl_one/btn_one"));
			m_img_discount_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/pl_btn/pl_one/img_discount");

			m_lbl_discount_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_mes/pl_btn/pl_one/img_discount/lbl_discount");

			m_btn_more = new UI_Model_StandardButton_Yellow2_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_mes/pl_btn/btn_more"));
			m_pl_char_SkeletonGraphic = FindUI<SkeletonGraphic>(gameObject.transform ,"pl_mes/pl_char");
			m_pl_char_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/pl_char");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_mes/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_mes/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_mes/btn_info");

			m_pl_info_CanvasGroup = FindUI<CanvasGroup>(gameObject.transform ,"pl_info");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(gameObject.transform ,"pl_info/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_info/btn_back");

			m_sv_info_ScrollRect = FindUI<ScrollRect>(gameObject.transform ,"pl_info/sv_info");
			m_sv_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/sv_info");
			m_sv_info_ListView = FindUI<ListView>(gameObject.transform ,"pl_info/sv_info");

			m_v_info_PolygonImage = FindUI<PolygonImage>(gameObject.transform ,"pl_info/sv_info/v_info");
			m_v_info_Mask = FindUI<Mask>(gameObject.transform ,"pl_info/sv_info/v_info");

			m_c_info_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_info/sv_info/v_info/c_info");
			m_c_info_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_info/sv_info/v_info/c_info");

			m_lbl_info_LanguageText = FindUI<LanguageText>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");
			m_lbl_info_ContentSizeFitter = FindUI<ContentSizeFitter>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");
			m_lbl_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/lbl_info");

			m_pl_probMes_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/pl_probMes");

			m_pl_title = FindUI<RectTransform>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/pl_probMes/pl_title");
			m_UI_Item_EventTurntablePro = new UI_Item_EventTurntablePro_SubView(FindUI<RectTransform>(gameObject.transform ,"pl_info/sv_info/v_info/c_info/pl_probMes/UI_Item_EventTurntablePro"));

			BindEvent();
        }

        #endregion
    }
}
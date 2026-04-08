// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月25日
// Update Time         :    2020年9月25日
// Class Description   :    UI_Item_ArmyShowView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Item_ArmyShowView : GameView
    {
		public const string VIEW_NAME = "UI_Item_ArmyShow";

        public UI_Item_ArmyShowView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_img_place_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_place_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link;
		[HideInInspector] public PolygonImage m_img_frame_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_frame_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_collect_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_collect_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link_collect;
		[HideInInspector] public ArabLayoutCompment m_pl_army_ArabLayoutCompment;
		[HideInInspector] public NodeHorizontalLayoutGroup m_pl_army_NodeHorizontalLayoutGroup;

		[HideInInspector] public ArabLayoutCompment m_pl_head_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHeadWithLevel_main;
		[HideInInspector] public PolygonImage m_img_state_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_state_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_img_state_ContentSizeFitter;

		[HideInInspector] public UI_Common_TroopsState_SubView m_UI_Common_TroopsState;
		[HideInInspector] public ArabLayoutCompment m_pl_head2_ArabLayoutCompment;

		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Model_CaptainHeadWithLevel_sub;
		[HideInInspector] public ArabLayoutCompment m_pl_mid_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_bar_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_rogressBar_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_rogressBar_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_Background_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_Background_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_barText_LanguageText;
		[HideInInspector] public Shadow m_lbl_barText_Shadow;

		[HideInInspector] public LanguageText m_lbl_state1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LayoutElement m_pl_left_LayoutElement;
		[HideInInspector] public ArabLayoutCompment m_pl_left_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyCount_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_armyCount_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_info_PolygonImage;
		[HideInInspector] public GameButton m_btn_info_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_pos_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link_transport1;
		[HideInInspector] public ArabLayoutCompment m_pl_transport_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_woker_PolygonImage;
		[HideInInspector] public GrayChildrens m_img_woker_GrayChildrens;
		[HideInInspector] public ArabLayoutCompment m_img_woker_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_char_PolygonImage;

		[HideInInspector] public PolygonImage m_img_state2_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_state2_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_img_state2_ContentSizeFitter;

		[HideInInspector] public LanguageText m_lbl_count2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_count2_ArabLayoutCompment;
		[HideInInspector] public ContentSizeFitter m_lbl_count2_ContentSizeFitter;

		[HideInInspector] public PolygonImage m_btn_info2_PolygonImage;
		[HideInInspector] public GameButton m_btn_info2_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_info2_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_pos0_ArabLayoutCompment;

		[HideInInspector] public UI_Model_Link_SubView m_UI_Model_Link_transport;
		[HideInInspector] public LanguageText m_lbl_state2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_return_PolygonImage;
		[HideInInspector] public GameButton m_btn_return_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_return_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_img_place_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_place");
			m_img_place_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_place");

			m_UI_Model_Link = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"img_place/UI_Model_Link"));
			m_img_frame_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_frame");
			m_img_frame_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_frame");

			m_img_collect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_frame/img_collect");
			m_img_collect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_frame/img_collect");

			m_UI_Model_Link_collect = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"img_frame/img_collect/UI_Model_Link_collect"));
			m_pl_army_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army");
			m_pl_army_NodeHorizontalLayoutGroup = FindUI<NodeHorizontalLayoutGroup>(vb.transform ,"pl_army");

			m_pl_head_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_head");

			m_UI_Model_CaptainHeadWithLevel_main = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_army/pl_head/UI_Model_CaptainHeadWithLevel_main"));
			m_img_state_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_army/pl_head/img_state");
			m_img_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_head/img_state");
			m_img_state_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_army/pl_head/img_state");

			m_UI_Common_TroopsState = new UI_Common_TroopsState_SubView(FindUI<RectTransform>(vb.transform ,"pl_army/pl_head/UI_Common_TroopsState"));
			m_pl_head2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_head2");

			m_UI_Model_CaptainHeadWithLevel_sub = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"pl_army/pl_head2/UI_Model_CaptainHeadWithLevel_sub"));
			m_pl_mid_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_mid");

			m_pl_bar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_mid/pl_bar");

			m_pb_rogressBar_GameSlider = FindUI<GameSlider>(vb.transform ,"pl_army/pl_mid/pl_bar/pb_rogressBar");
			m_pb_rogressBar_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_mid/pl_bar/pb_rogressBar");

			m_img_Background_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_army/pl_mid/pl_bar/pb_rogressBar/img_Background");
			m_img_Background_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_mid/pl_bar/pb_rogressBar/img_Background");

			m_lbl_barText_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_army/pl_mid/pl_bar/pb_rogressBar/lbl_barText");
			m_lbl_barText_Shadow = FindUI<Shadow>(vb.transform ,"pl_army/pl_mid/pl_bar/pb_rogressBar/lbl_barText");

			m_lbl_state1_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_army/pl_mid/lbl_state1");
			m_lbl_state1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_mid/lbl_state1");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_army/pl_mid/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_mid/lbl_name");

			m_pl_left_LayoutElement = FindUI<LayoutElement>(vb.transform ,"pl_army/pl_left");
			m_pl_left_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_left");

			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_army/pl_left/lbl_armyCount");
			m_lbl_armyCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_left/lbl_armyCount");
			m_lbl_armyCount_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_army/pl_left/lbl_armyCount");

			m_btn_info_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_army/pl_left/lbl_armyCount/btn_info");
			m_btn_info_GameButton = FindUI<GameButton>(vb.transform ,"pl_army/pl_left/lbl_armyCount/btn_info");
			m_btn_info_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_left/lbl_armyCount/btn_info");

			m_pl_pos_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_army/pl_left/pl_pos");

			m_UI_Model_Link_transport1 = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"pl_army/pl_left/pl_pos/UI_Model_Link_transport1"));
			m_pl_transport_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_transport");

			m_lbl_name2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_transport/lbl_name2");
			m_lbl_name2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_transport/lbl_name2");

			m_img_woker_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_transport/img_woker");
			m_img_woker_GrayChildrens = FindUI<GrayChildrens>(vb.transform ,"pl_transport/img_woker");
			m_img_woker_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_transport/img_woker");

			m_img_char_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_transport/img_woker/img_char");

			m_img_state2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_transport/img_state2");
			m_img_state2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_transport/img_state2");
			m_img_state2_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_transport/img_state2");

			m_lbl_count2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_transport/lbl_count2");
			m_lbl_count2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_transport/lbl_count2");
			m_lbl_count2_ContentSizeFitter = FindUI<ContentSizeFitter>(vb.transform ,"pl_transport/lbl_count2");

			m_btn_info2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pl_transport/lbl_count2/btn_info2");
			m_btn_info2_GameButton = FindUI<GameButton>(vb.transform ,"pl_transport/lbl_count2/btn_info2");
			m_btn_info2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_transport/lbl_count2/btn_info2");

			m_pl_pos0_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_transport/pl_pos0");

			m_UI_Model_Link_transport = new UI_Model_Link_SubView(FindUI<RectTransform>(vb.transform ,"pl_transport/pl_pos0/UI_Model_Link_transport"));
			m_lbl_state2_LanguageText = FindUI<LanguageText>(vb.transform ,"pl_transport/lbl_state2");
			m_lbl_state2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pl_transport/lbl_state2");

			m_btn_return_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_return");
			m_btn_return_GameButton = FindUI<GameButton>(vb.transform ,"btn_return");
			m_btn_return_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_return");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
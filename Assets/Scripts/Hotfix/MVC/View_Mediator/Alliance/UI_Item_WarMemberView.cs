// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月4日
// Update Time         :    2020年6月4日
// Class Description   :    UI_Item_WarMemberView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public class UI_Item_WarMemberView : GameView
    {
		public const string VIEW_NAME = "UI_Item_WarMember";

        public UI_Item_WarMemberView () 
        {
        }

        #region gen ui code 
		[HideInInspector] public PolygonImage m_btn_Join_PolygonImage;
		[HideInInspector] public GameButton m_btn_Join_GameButton;

		[HideInInspector] public GameSlider m_pb_state_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_state_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_num_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_num_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_stateicon_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_stateicon_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyCount_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyCount_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_name_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_name_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_captainName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_captainName_ArabLayoutCompment;

		[HideInInspector] public UI_Model_PlayerHead_SubView m_UI_PlayerHead;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Captain2;
		[HideInInspector] public UI_Model_CaptainHead_SubView m_UI_Captain1;
		[HideInInspector] public PolygonImage m_btn_leader_PolygonImage;
		[HideInInspector] public GameButton m_btn_leader_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_leader_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_back_ArabLayoutCompment;

		[HideInInspector] public ArabLayoutCompment m_pl_time_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_time_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_time_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_cur_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_cur_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_curNum_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_curNum_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_down_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_down_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_arrow_up_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_arrow_up_ArabLayoutCompment;

		[HideInInspector] public GameSlider m_pb_collect_GameSlider;
		[HideInInspector] public ArabLayoutCompment m_pb_collect_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_fill_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_colPro_LanguageText;

		[HideInInspector] public LanguageText m_lbl_state_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_state_ArabLayoutCompment;



        private void UIFinder(GameObject obj)
        {
            this.vb = ViewBinder.Create(obj);
            this.gameObject = obj;            
			m_btn_Join_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_Join");
			m_btn_Join_GameButton = FindUI<GameButton>(vb.transform ,"btn_Join");

			m_pb_state_GameSlider = FindUI<GameSlider>(vb.transform ,"btn_Join/pb_state");
			m_pb_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/pb_state");

			m_lbl_num_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_Join/pb_state/lbl_num");
			m_lbl_num_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/pb_state/lbl_num");

			m_img_stateicon_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_Join/pb_state/img_stateicon");
			m_img_stateicon_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/pb_state/img_stateicon");

			m_lbl_armyCount_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_Join/lbl_armyCount");
			m_lbl_armyCount_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/lbl_armyCount");

			m_lbl_name_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_Join/lbl_name");
			m_lbl_name_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/lbl_name");

			m_lbl_captainName_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_Join/lbl_captainName");
			m_lbl_captainName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/lbl_captainName");

			m_UI_PlayerHead = new UI_Model_PlayerHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_Join/UI_PlayerHead"));
			m_UI_Captain2 = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_Join/UI_Captain2"));
			m_UI_Captain1 = new UI_Model_CaptainHead_SubView(FindUI<RectTransform>(vb.transform ,"btn_Join/UI_Captain1"));
			m_btn_leader_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_Join/btn_leader");
			m_btn_leader_GameButton = FindUI<GameButton>(vb.transform ,"btn_Join/btn_leader");
			m_btn_leader_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/btn_leader");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_Join/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(vb.transform ,"btn_Join/btn_back");
			m_btn_back_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/btn_back");

			m_pl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/pl_time");

			m_lbl_time_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_Join/pl_time/lbl_time");
			m_lbl_time_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/pl_time/lbl_time");

			m_img_cur_PolygonImage = FindUI<PolygonImage>(vb.transform ,"btn_Join/pl_time/img_cur");
			m_img_cur_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/pl_time/img_cur");

			m_lbl_curNum_LanguageText = FindUI<LanguageText>(vb.transform ,"btn_Join/pl_time/lbl_curNum");
			m_lbl_curNum_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"btn_Join/pl_time/lbl_curNum");

			m_img_arrow_down_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_arrow_down");
			m_img_arrow_down_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_arrow_down");

			m_img_arrow_up_PolygonImage = FindUI<PolygonImage>(vb.transform ,"img_arrow_up");
			m_img_arrow_up_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"img_arrow_up");

			m_pb_collect_GameSlider = FindUI<GameSlider>(vb.transform ,"pb_collect");
			m_pb_collect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"pb_collect");

			m_img_fill_PolygonImage = FindUI<PolygonImage>(vb.transform ,"pb_collect/Fill Area/img_fill");

			m_lbl_colPro_LanguageText = FindUI<LanguageText>(vb.transform ,"pb_collect/lbl_colPro");

			m_lbl_state_LanguageText = FindUI<LanguageText>(vb.transform ,"lbl_state");
			m_lbl_state_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"lbl_state");



        }

        #endregion

        public override void BindSingleUI(GameObject obj) {
           UIFinder(obj);
    	}

    }
}
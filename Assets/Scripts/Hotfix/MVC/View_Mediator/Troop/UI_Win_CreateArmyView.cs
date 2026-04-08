// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月27日
// Update Time         :    2020年9月27日
// Class Description   :    UI_Win_CreateArmyView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Spine.Unity;
using UnityEngine.Rendering;

namespace Game {
    public class UI_Win_CreateArmyView : GameView
    {
        public const string VIEW_NAME = "UI_Win_CreateArmy";

        public UI_Win_CreateArmyView () 
        {
        }
        
        public override void LoadUI(System.Action action){
			ViewBinder.Create(VIEW_NAME,this,action);
		}

        #region gen ui code 
		[HideInInspector] public UI_Tag_T1_WinAnime_SubView m_UI_Tag_T1_WinAnime;
		[HideInInspector] public UI_Model_Window_Type3_SubView m_UI_Model_Window_Type1;
		[HideInInspector] public PolygonImage m_pl_Center_PolygonImage;

		[HideInInspector] public ArabLayoutCompment m_pl_armyDataRect_ArabLayoutCompment;
		[HideInInspector] public GridLayoutGroup m_pl_armyDataRect_GridLayoutGroup;
		[HideInInspector] public PolygonImage m_pl_armyDataRect_PolygonImage;

		[HideInInspector] public UI_Item_ArmyData_SubView m_UI_Item_ArmyData1;
		[HideInInspector] public UI_Item_ArmyData_SubView m_UI_Item_ArmyData2;
		[HideInInspector] public UI_Model_MiniButton_White_SubView m_UI_Model_MiniButton_White;
		[HideInInspector] public PolygonImage m_pl_SingleCaptain_PolygonImage;

		[HideInInspector] public PolygonImage m_img_NoSingleCaptain_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_SingleName_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_SingleName_ArabLayoutCompment;

		[HideInInspector] public SkeletonGraphic m_spin_Schar_SkeletonGraphic;

		[HideInInspector] public PolygonImage m_img_SchoseEffect_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_img_SchoseEffect_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_plus_SingleChar_PolygonImage;
		[HideInInspector] public GameButton m_btn_plus_SingleChar_GameButton;

		[HideInInspector] public PolygonImage m_btn_SingleChange_PolygonImage;
		[HideInInspector] public GameButton m_btn_SingleChange_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_SingleChange_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_SingleSkillsbg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_SingleSkillsbg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_SingleSkillsbg_1_PolygonImage;

		[HideInInspector] public PolygonImage m_SingleSkillsbg_2_PolygonImage;

		[HideInInspector] public PolygonImage m_SingleSkillsbg_3_PolygonImage;

		[HideInInspector] public PolygonImage m_SingleSkillsbg_4_PolygonImage;

		[HideInInspector] public PolygonImage m_SingleSkillsbg_5_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_noTextButton_PolygonImage;
		[HideInInspector] public GameButton m_btn_noTextButton_GameButton;

		[HideInInspector] public UI_Tag_ClickAnimeMsg_btn_SubView m_UI_Tag_ClickAnimeMsg_btn;
		[HideInInspector] public PolygonImage m_img_unlockbg_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_SingleSkills_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_SingleSkills_ArabLayoutCompment;

		[HideInInspector] public UI_Item_CaptainSkill_M1_SubView m_UI_Item_CaptainSkill_M1;
		[HideInInspector] public PolygonImage m_pl_DoubleCaptain_PolygonImage;

		[HideInInspector] public LanguageText m_lbl_DoubleName1_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_DoubleName1_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_DoubleName2_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_DoubleName2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_DingleChange1_PolygonImage;
		[HideInInspector] public GameButton m_btn_DingleChange1_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_DingleChange1_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_DingleChange2_PolygonImage;
		[HideInInspector] public GameButton m_btn_DingleChange2_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_DingleChange2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_NoSingleCaptain2_PolygonImage;

		[HideInInspector] public SkeletonGraphic m_spin_Dchar2_SkeletonGraphic;
		[HideInInspector] public ArabLayoutCompment m_spin_Dchar2_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_img_DchoseEffect2_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_plus2_PolygonImage;
		[HideInInspector] public GameButton m_btn_plus2_GameButton;

		[HideInInspector] public PolygonImage m_btn_delete2_PolygonImage;
		[HideInInspector] public GameButton m_btn_delete2_GameButton;

		[HideInInspector] public PolygonImage m_img_NoSingleCaptain1_PolygonImage;

		[HideInInspector] public SkeletonGraphic m_spin_Dchar1_SkeletonGraphic;

		[HideInInspector] public PolygonImage m_img_DchoseEffect1_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_plus1_PolygonImage;
		[HideInInspector] public GameButton m_btn_plus1_GameButton;

		[HideInInspector] public GridLayoutGroup m_pl_DoubleSkillsMainbg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_DoubleSkillsMainbg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_DoubleSkillsMainbg_1_PolygonImage;

		[HideInInspector] public PolygonImage m_DoubleSkillsMainbg_2_PolygonImage;

		[HideInInspector] public PolygonImage m_DoubleSkillsMainbg_3_PolygonImage;

		[HideInInspector] public PolygonImage m_DoubleSkillsMainbg_4_PolygonImage;

		[HideInInspector] public PolygonImage m_DoubleSkillsMainbg_5_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_DoubleSkillsSubbg_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_DoubleSkillsSubbg_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_DoubleSkillsSubbg_1_PolygonImage;

		[HideInInspector] public PolygonImage m_DoubleSkillsSubbg_2_PolygonImage;

		[HideInInspector] public PolygonImage m_DoubleSkillsSubbg_3_PolygonImage;

		[HideInInspector] public PolygonImage m_DoubleSkillsSubbg_4_PolygonImage;

		[HideInInspector] public PolygonImage m_DoubleSkillsSubbg_5_PolygonImage;

		[HideInInspector] public GridLayoutGroup m_pl_DoubleSkillsMain_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_DoubleSkillsMain_ArabLayoutCompment;

		[HideInInspector] public GridLayoutGroup m_pl_DoubleSkillsSub_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_DoubleSkillsSub_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_powerTotal_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_powerTotal_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_powerCap_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_powerCap_ArabLayoutCompment;

		[HideInInspector] public ScrollRect m_sv_armyList_ScrollRect;
		[HideInInspector] public PolygonImage m_sv_armyList_PolygonImage;
		[HideInInspector] public ListView m_sv_armyList_ListView;
		[HideInInspector] public ArabLayoutCompment m_sv_armyList_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_no_soldier_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_no_soldier_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_Question_PolygonImage;
		[HideInInspector] public GameButton m_btn_Question_GameButton;
		[HideInInspector] public ArabLayoutCompment m_btn_Question_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyCapa_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyCapa_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_icon_mobility_PolygonImage;
		[HideInInspector] public ArabLayoutCompment m_icon_mobility_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_mobility_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_mobility_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_armyWeight_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_armyWeight_ArabLayoutCompment;

		[HideInInspector] public LanguageText m_lbl_compare_LanguageText;
		[HideInInspector] public ArabLayoutCompment m_lbl_compare_ArabLayoutCompment;
		[HideInInspector] public LayoutElement m_lbl_compare_LayoutElement;

		[HideInInspector] public UI_Model_DoubleLineButton_Yellow_SubView m_UI_Model_DoubleLineButton_Yellow;
		[HideInInspector] public UI_Model_StandardButton_Blue_big_SubView m_UI_Model_StandardButton_Blue;
		[HideInInspector] public GridLayoutGroup m_pl_Save_GridLayoutGroup;
		[HideInInspector] public ArabLayoutCompment m_pl_Save_ArabLayoutCompment;
		[HideInInspector] public ToggleGroup m_pl_Save_ToggleGroup;

		[HideInInspector] public GameToggle m_ck_sls_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_sls_ArabLayoutCompment;

		[HideInInspector] public GameToggle m_ck_sll_GameToggle;
		[HideInInspector] public ArabLayoutCompment m_ck_sll_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_refresh_PolygonImage;
		[HideInInspector] public GameButton m_btn_refresh_GameButton;
		[HideInInspector] public BtnAnimation m_btn_refresh_BtnAnimation;

		[HideInInspector] public ScrollRect m_list_SaveIndex_View_Blue_ScrollRect;
		[HideInInspector] public PolygonImage m_list_SaveIndex_View_Blue_PolygonImage;
		[HideInInspector] public ListView m_list_SaveIndex_View_Blue_ListView;
		[HideInInspector] public ArabLayoutCompment m_list_SaveIndex_View_Blue_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_v_list_view_PolygonImage;
		[HideInInspector] public Mask m_v_list_view_Mask;

		[HideInInspector] public ToggleGroup m_c_list_view_Blue_ToggleGroup;

		[HideInInspector] public ScrollRect m_list_SaveIndex_View_Red_ScrollRect;
		[HideInInspector] public PolygonImage m_list_SaveIndex_View_Red_PolygonImage;
		[HideInInspector] public ListView m_list_SaveIndex_View_Red_ListView;
		[HideInInspector] public ArabLayoutCompment m_list_SaveIndex_View_Red_ArabLayoutCompment;

		[HideInInspector] public ToggleGroup m_c_list_view_Red_ToggleGroup;

		[HideInInspector] public ScrollRect m_list_SaveIndex_View_Yellow_ScrollRect;
		[HideInInspector] public PolygonImage m_list_SaveIndex_View_Yellow_PolygonImage;
		[HideInInspector] public ListView m_list_SaveIndex_View_Yellow_ListView;
		[HideInInspector] public ArabLayoutCompment m_list_SaveIndex_View_Yellow_ArabLayoutCompment;

		[HideInInspector] public ToggleGroup m_c_list_view_Yellow_ToggleGroup;

		[HideInInspector] public UI_Item_CaptainList_SubView m_UI_Item_CaptainList;
		[HideInInspector] public ArabLayoutCompment m_UI_Item_ArmySaveView_ArabLayoutCompment;

		[HideInInspector] public PolygonImage m_btn_back_PolygonImage;
		[HideInInspector] public GameButton m_btn_back_GameButton;

		[HideInInspector] public PolygonImage m_img_back_PolygonImage;

		[HideInInspector] public PolygonImage m_btn_set_PolygonImage;
		[HideInInspector] public GameButton m_btn_set_GameButton;
		[HideInInspector] public BtnAnimation m_btn_set_BtnAnimation;

		[HideInInspector] public ScrollRect m_list_View_save_ScrollRect;
		[HideInInspector] public PolygonImage m_list_View_save_PolygonImage;
		[HideInInspector] public ListView m_list_View_save_ListView;

		[HideInInspector] public RectTransform m_c_list_view;
		[HideInInspector] public LanguageText m_lbl_saveData_LanguageText;

		[HideInInspector] public LanguageText m_lbl_des_LanguageText;



        private void UIFinder()
        {
			m_UI_Tag_T1_WinAnime = new UI_Tag_T1_WinAnime_SubView(FindUI<RectTransform>(vb.transform ,"win/UI_Tag_T1_WinAnime"));
			m_UI_Model_Window_Type1 = new UI_Model_Window_Type3_SubView(FindUI<RectTransform>(vb.transform ,"win/UI_Model_Window_Type1"));
			m_pl_Center_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center");

			m_pl_armyDataRect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_armyDataRect");
			m_pl_armyDataRect_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"win/pl_Center/CaptainRect/pl_armyDataRect");
			m_pl_armyDataRect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_armyDataRect");

			m_UI_Item_ArmyData1 = new UI_Item_ArmyData_SubView(FindUI<RectTransform>(vb.transform ,"win/pl_Center/CaptainRect/pl_armyDataRect/UI_Item_ArmyData1"));
			m_UI_Item_ArmyData2 = new UI_Item_ArmyData_SubView(FindUI<RectTransform>(vb.transform ,"win/pl_Center/CaptainRect/pl_armyDataRect/UI_Item_ArmyData2"));
			m_UI_Model_MiniButton_White = new UI_Model_MiniButton_White_SubView(FindUI<RectTransform>(vb.transform ,"win/pl_Center/CaptainRect/UI_Model_MiniButton_White"));
			m_pl_SingleCaptain_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain");

			m_img_NoSingleCaptain_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/img_NoSingleCaptain");

			m_lbl_SingleName_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/SingleChar/lbl_SingleName");
			m_lbl_SingleName_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/SingleChar/lbl_SingleName");

			m_spin_Schar_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/SingleChar/spin_Schar");

			m_img_SchoseEffect_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/SingleChar/img_SchoseEffect");
			m_img_SchoseEffect_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/SingleChar/img_SchoseEffect");

			m_btn_plus_SingleChar_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/SingleChar/btn_plus_SingleChar");
			m_btn_plus_SingleChar_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/SingleChar/btn_plus_SingleChar");

			m_btn_SingleChange_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/btn_SingleChange");
			m_btn_SingleChange_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/btn_SingleChange");
			m_btn_SingleChange_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/btn_SingleChange");

			m_pl_SingleSkillsbg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg");
			m_pl_SingleSkillsbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg");

			m_SingleSkillsbg_1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/SingleSkillsbg_1");

			m_SingleSkillsbg_2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/SingleSkillsbg_2");

			m_SingleSkillsbg_3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/SingleSkillsbg_3");

			m_SingleSkillsbg_4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/SingleSkillsbg_4");

			m_SingleSkillsbg_5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/SingleSkillsbg_5");

			m_btn_noTextButton_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/unlockbg/btn_noTextButton");
			m_btn_noTextButton_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/unlockbg/btn_noTextButton");

			m_UI_Tag_ClickAnimeMsg_btn = new UI_Tag_ClickAnimeMsg_btn_SubView(FindUI<RectTransform>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/unlockbg/btn_noTextButton/UI_Tag_ClickAnimeMsg_btn"));
			m_img_unlockbg_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkillsbg/unlockbg/img_unlockbg");

			m_pl_SingleSkills_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkills");
			m_pl_SingleSkills_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkills");

			m_UI_Item_CaptainSkill_M1 = new UI_Item_CaptainSkill_M1_SubView(FindUI<RectTransform>(vb.transform ,"win/pl_Center/CaptainRect/pl_SingleCaptain/pl_SingleSkills/UI_Item_CaptainSkill_M1"));
			m_pl_DoubleCaptain_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain");

			m_lbl_DoubleName1_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/lbl_DoubleName1");
			m_lbl_DoubleName1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/lbl_DoubleName1");

			m_lbl_DoubleName2_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/lbl_DoubleName2");
			m_lbl_DoubleName2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/lbl_DoubleName2");

			m_btn_DingleChange1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/btn_DingleChange1");
			m_btn_DingleChange1_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/btn_DingleChange1");
			m_btn_DingleChange1_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/btn_DingleChange1");

			m_btn_DingleChange2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/btn_DingleChange2");
			m_btn_DingleChange2_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/btn_DingleChange2");
			m_btn_DingleChange2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/btn_DingleChange2");

			m_img_NoSingleCaptain2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar2/img_NoSingleCaptain2");

			m_spin_Dchar2_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar2/spin_Dchar2");
			m_spin_Dchar2_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar2/spin_Dchar2");

			m_img_DchoseEffect2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar2/img_DchoseEffect2");

			m_btn_plus2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar2/btn_plus2");
			m_btn_plus2_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar2/btn_plus2");

			m_btn_delete2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar2/btn_delete2");
			m_btn_delete2_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar2/btn_delete2");

			m_img_NoSingleCaptain1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar1/img_NoSingleCaptain1");

			m_spin_Dchar1_SkeletonGraphic = FindUI<SkeletonGraphic>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar1/spin_Dchar1");

			m_img_DchoseEffect1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar1/img_DchoseEffect1");

			m_btn_plus1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar1/btn_plus1");
			m_btn_plus1_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleChar1/btn_plus1");

			m_pl_DoubleSkillsMainbg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMainbg");
			m_pl_DoubleSkillsMainbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMainbg");

			m_DoubleSkillsMainbg_1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMainbg/DoubleSkillsMainbg_1");

			m_DoubleSkillsMainbg_2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMainbg/DoubleSkillsMainbg_2");

			m_DoubleSkillsMainbg_3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMainbg/DoubleSkillsMainbg_3");

			m_DoubleSkillsMainbg_4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMainbg/DoubleSkillsMainbg_4");

			m_DoubleSkillsMainbg_5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMainbg/DoubleSkillsMainbg_5");

			m_pl_DoubleSkillsSubbg_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSubbg");
			m_pl_DoubleSkillsSubbg_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSubbg");

			m_DoubleSkillsSubbg_1_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSubbg/DoubleSkillsSubbg_1");

			m_DoubleSkillsSubbg_2_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSubbg/DoubleSkillsSubbg_2");

			m_DoubleSkillsSubbg_3_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSubbg/DoubleSkillsSubbg_3");

			m_DoubleSkillsSubbg_4_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSubbg/DoubleSkillsSubbg_4");

			m_DoubleSkillsSubbg_5_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSubbg/DoubleSkillsSubbg_5");

			m_pl_DoubleSkillsMain_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMain");
			m_pl_DoubleSkillsMain_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsMain");

			m_pl_DoubleSkillsSub_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSub");
			m_pl_DoubleSkillsSub_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/CaptainRect/pl_DoubleCaptain/DoubleSkills/pl_DoubleSkillsSub");

			m_lbl_powerTotal_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/ArmyRect/power/lbl_powerTotal");
			m_lbl_powerTotal_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/power/lbl_powerTotal");

			m_lbl_powerCap_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/ArmyRect/power/lbl_powerCap");
			m_lbl_powerCap_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/power/lbl_powerCap");

			m_sv_armyList_ScrollRect = FindUI<ScrollRect>(vb.transform ,"win/pl_Center/ArmyRect/sv_armyList");
			m_sv_armyList_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/ArmyRect/sv_armyList");
			m_sv_armyList_ListView = FindUI<ListView>(vb.transform ,"win/pl_Center/ArmyRect/sv_armyList");
			m_sv_armyList_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/sv_armyList");

			m_lbl_no_soldier_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/ArmyRect/lbl_no_soldier");
			m_lbl_no_soldier_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/lbl_no_soldier");

			m_btn_Question_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/ArmyRect/weight/btn_Question");
			m_btn_Question_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/ArmyRect/weight/btn_Question");
			m_btn_Question_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/weight/btn_Question");

			m_lbl_armyCapa_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/ArmyRect/weight/lbl_armyCapa");
			m_lbl_armyCapa_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/weight/lbl_armyCapa");

			m_icon_mobility_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/ArmyRect/weight/icon_mobility");
			m_icon_mobility_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/weight/icon_mobility");

			m_lbl_mobility_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/ArmyRect/weight/icon_mobility/lbl_mobility");
			m_lbl_mobility_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/weight/icon_mobility/lbl_mobility");

			m_lbl_armyWeight_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/ArmyRect/weight/lbl_armyWeight");
			m_lbl_armyWeight_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/weight/lbl_armyWeight");

			m_lbl_compare_LanguageText = FindUI<LanguageText>(vb.transform ,"win/pl_Center/ArmyRect/btns/lbl_compare");
			m_lbl_compare_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/ArmyRect/btns/lbl_compare");
			m_lbl_compare_LayoutElement = FindUI<LayoutElement>(vb.transform ,"win/pl_Center/ArmyRect/btns/lbl_compare");

			m_UI_Model_DoubleLineButton_Yellow = new UI_Model_DoubleLineButton_Yellow_SubView(FindUI<RectTransform>(vb.transform ,"win/pl_Center/ArmyRect/btns/UI_Model_DoubleLineButton_Yellow"));
			m_UI_Model_StandardButton_Blue = new UI_Model_StandardButton_Blue_big_SubView(FindUI<RectTransform>(vb.transform ,"win/pl_Center/ArmyRect/btns/UI_Model_StandardButton_Blue"));
			m_pl_Save_GridLayoutGroup = FindUI<GridLayoutGroup>(vb.transform ,"win/pl_Center/pl_Save");
			m_pl_Save_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/pl_Save");
			m_pl_Save_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"win/pl_Center/pl_Save");

			m_ck_sls_GameToggle = FindUI<GameToggle>(vb.transform ,"win/pl_Center/pl_Save/ck_sls");
			m_ck_sls_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/pl_Save/ck_sls");

			m_ck_sll_GameToggle = FindUI<GameToggle>(vb.transform ,"win/pl_Center/pl_Save/ck_sll");
			m_ck_sll_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/pl_Save/ck_sll");

			m_btn_refresh_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/pl_Save/btn_refresh");
			m_btn_refresh_GameButton = FindUI<GameButton>(vb.transform ,"win/pl_Center/pl_Save/btn_refresh");
			m_btn_refresh_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"win/pl_Center/pl_Save/btn_refresh");

			m_list_SaveIndex_View_Blue_ScrollRect = FindUI<ScrollRect>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Blue");
			m_list_SaveIndex_View_Blue_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Blue");
			m_list_SaveIndex_View_Blue_ListView = FindUI<ListView>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Blue");
			m_list_SaveIndex_View_Blue_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Blue");

			m_v_list_view_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Blue/v_list_view");
			m_v_list_view_Mask = FindUI<Mask>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Blue/v_list_view");

			m_c_list_view_Blue_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Blue/v_list_view/c_list_view_Blue");

			m_list_SaveIndex_View_Red_ScrollRect = FindUI<ScrollRect>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Red");
			m_list_SaveIndex_View_Red_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Red");
			m_list_SaveIndex_View_Red_ListView = FindUI<ListView>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Red");
			m_list_SaveIndex_View_Red_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Red");

			m_c_list_view_Red_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Red/v_list_view/c_list_view_Red");

			m_list_SaveIndex_View_Yellow_ScrollRect = FindUI<ScrollRect>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Yellow");
			m_list_SaveIndex_View_Yellow_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Yellow");
			m_list_SaveIndex_View_Yellow_ListView = FindUI<ListView>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Yellow");
			m_list_SaveIndex_View_Yellow_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Yellow");

			m_c_list_view_Yellow_ToggleGroup = FindUI<ToggleGroup>(vb.transform ,"win/pl_Center/list_SaveIndex_View_Yellow/v_list_view/c_list_view_Yellow");

			m_UI_Item_CaptainList = new UI_Item_CaptainList_SubView(FindUI<RectTransform>(vb.transform ,"win/UI_Item_CaptainList"));
			m_UI_Item_ArmySaveView_ArabLayoutCompment = FindUI<ArabLayoutCompment>(vb.transform ,"win/UI_Item_ArmySaveView");

			m_btn_back_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/UI_Item_ArmySaveView/btn_back");
			m_btn_back_GameButton = FindUI<GameButton>(vb.transform ,"win/UI_Item_ArmySaveView/btn_back");

			m_img_back_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/UI_Item_ArmySaveView/btn_back/img_back");

			m_btn_set_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/UI_Item_ArmySaveView/btn_set");
			m_btn_set_GameButton = FindUI<GameButton>(vb.transform ,"win/UI_Item_ArmySaveView/btn_set");
			m_btn_set_BtnAnimation = FindUI<BtnAnimation>(vb.transform ,"win/UI_Item_ArmySaveView/btn_set");

			m_list_View_save_ScrollRect = FindUI<ScrollRect>(vb.transform ,"win/UI_Item_ArmySaveView/list_View_save");
			m_list_View_save_PolygonImage = FindUI<PolygonImage>(vb.transform ,"win/UI_Item_ArmySaveView/list_View_save");
			m_list_View_save_ListView = FindUI<ListView>(vb.transform ,"win/UI_Item_ArmySaveView/list_View_save");

			m_c_list_view = FindUI<RectTransform>(vb.transform ,"win/UI_Item_ArmySaveView/list_View_save/v_list_view/c_list_view");
			m_lbl_saveData_LanguageText = FindUI<LanguageText>(vb.transform ,"win/UI_Item_ArmySaveView/lbl_saveData");

			m_lbl_des_LanguageText = FindUI<LanguageText>(vb.transform ,"win/UI_Item_ArmySaveView/lbl_des");


            UI_Win_CreateArmyMediator mt = new UI_Win_CreateArmyMediator(vb.gameObject);
            mt.view = this;
            AppFacade.GetInstance().RegisterMediator(mt);
			if(mt.IsOpenUpdate)
			{
                vb.fixedUpdateCallback = mt.FixedUpdate;
                vb.lateUpdateCallback = mt.LateUpdate;
				vb.updateCallback = mt.Update;
			}
            vb.openAniEndCallback = mt.OpenAniEnd;
            vb.onWinFocusCallback = mt.WinFocus;
            vb.onWinCloseCallback = mt.WinClose;
            vb.onPrewarmCallback = mt.PrewarmComplete;
            vb.onMenuBackCallback = mt.onMenuBackCallback;
        }

        #endregion

        public override void Start () {
            UIFinder();
    	}
        public override void OnDestroy()
        {
            AppFacade.GetInstance().RemoveView(vb);
        }

    }
}

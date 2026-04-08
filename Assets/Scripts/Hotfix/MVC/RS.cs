using System.Runtime.InteropServices;
using Client;
using Hotfix;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 存放所有写死的代码里面的资源文件名称
    /// </summary>
    public class RS
    {
        public const string LoginBG = "";

        public const string PlayerDefaultHeadIcon = "player_head[img_player_head_empty]";

        //音量
        public const string BGMVolume = "BGMVolume";
        public const string SfxVolume = "SfxVolume";

        //通用Tip资源
        public const string Tip_Up = "UI_Common_UpTips";
        public const string Tip_Mid = "UI_Common_MidTips";
        public const string Tip_City = "UI_Common_CityTips";
        public const string Tip_AllianceHelp = "UI_Common_HelpTips";
        public const string Tip_MysteryStore = "UI_Tip_MysteryStoreTip";
        
        public const string Tip_AskForHelp = "ui_common[icon_com_help3]";

        public const string Tip_HelpOther = "ui_common[icon_com_help2]";
        
        
        //通用Alert资源
        public const string Alert = "UI_Common_Alert";
        public const string AlertMin = "UI_Common_AlertMin";
        public const string AlertRemind = "UI_Common_AlertSure";
        public const string AlertRemindNoCurrency = "UI_Common_AlertSure2";

        //背包item品质
        public static string[] ItemQualityBg = new[] { "item0[img_item_bg1]",
                                                       "item0[img_item_bg2]",
                                                       "item0[img_item_bg3]",
                                                       "item0[img_item_bg4]",
                                                       "item0[img_item_bg5]" };
        //统帅品质
        public static string[] HeroQualityBg = new[] { "hero_head[img_com_heroframe1]",
                                                       "hero_head[img_com_heroframe2]",
                                                       "hero_head[img_com_heroframe3]",
                                                       "hero_head[img_com_heroframe4]",
                                                       "hero_head[img_com_heroframe5]" };
        
        //统帅觉醒品质
        public static string[] HeroUpQualityBg = new[] { "hero_head[img_com_heroframe_up2]",
                                                         "hero_head[img_com_heroframe_up3]",};

        public static string HeroUpEffect_5 = "UI_10063_2";
        public static string HeroUpEffect_4 = "UI_10063_1";

        //统帅升星
        public const string HeroPutItemEffectName = "UI_10048"; //添加升星材料特效
        public const string HeroStarLevelUpEffectName = "UI_10049"; //升星操作特效
        public const string HeroStarLevelUpStarEffectName = "UI_10054"; //升星成功界面星星特效

        //部队选中特效
        public const string TroopSelectEffectName = "operation_2004";
        public const string EnemySelectEffectName = "operation_2003";
        public const string greenGoName = "operation_2008";
        public const string blueGoName = "operation_2008_3";
        public const string redGoName = "operation_2008_4";
        public const string TroopmSelectAnimatorGo = "operation_2005";

        //研究item背景颜色
        public static string[] HeroQualityColor = new[] { "#ebe7d3", "#53db5b", "#1fc4ec", "#ca5cee", "#f3ca4e"};

        //军队类型Icon
        public static string[] ArmyTypeIcon = new[] {"ui_troops[icon_tro_1001_1]", //步兵
                                                     "ui_troops[icon_tro_1001_3]", //骑兵
                                                     "ui_troops[icon_tro_1001_2]",//弓兵
                                                     "ui_troops[icon_tro_1001_4]"};//车兵

        
        //研究item背景颜色
        public static string[] ResearchBG = new[] {"ui_research[img_research_btnBgDark]","ui_research[img_research_btnBgLight]"};
        
        //研究item背景颜色
        public static string[] ResearchItemFontColor = new[] {"#eae3d3","#4d4945"};
                                                           //军队类型Icon
        public static string[] FireName = new[] {"operation_2006_4", //1级火焰
                                                     "operation_2006_3", //2级火焰
                                                     "operation_2006_2",//3级火焰
                                                     "operation_2006_1",
                                                          "operation_2006"};//4级火焰

        // 经济 统计1对多  空 ，1-1 ，1-2 ,1-3,1-4
        public static string[] ResearchLine1 = new[] {"",
            "ui_research[line_research_101]",
            "ui_research[line_research_110]",
            "ui_research[line_research_109]",
            "ui_research[line_research_104]",
            "",""};

       
        
        // 经济 统计1对多  空 ，1-1 ，1-2 ,1-3,1-4
        public static string[] ResearchLine2 = new[] {"",
            "ui_research[line_research_201]",
            "ui_research[line_research_202]",
            "ui_research[line_research_203]",
            "ui_research[line_research_204]",
            "",""};

        public static string[][] ResearchLineJJ = new[] {ResearchLine1, ResearchLine2};
        
        
        // 军事  统计1对多  空 ，1-1 ，1-2 ,1-3,1-4
        public static string[] ResearchLine3 = new[] {"",
            "ui_research[line_research_101]",
            "ui_research[line_research_102]",
            "ui_research[line_research_103]",
            "ui_research[line_research_104]",
            "",""};

       
        
        // 军事 统计1对多  空 ，1-1 ，1-2 ,1-3,1-4
        public static string[] ResearchLine4 = new[] {"",
            "ui_research[line_research_201]",
            "ui_research[line_research_202]",
            "ui_research[line_research_214]",
            "ui_research[line_research_204]",
            "",""};
        //军事 
        public static string[][] ResearchLineJX = new[] {ResearchLine3, ResearchLine4};

        public static string[][][] ResearchLine = new[] {ResearchLineJJ, ResearchLineJX};
        
        public static string[][][] GuildResearchLine = new[] {ResearchLineJX, ResearchLineJX,ResearchLineJX};
        
        //联盟科技1-2中线
        public static string[] GuildResearch1_2 =
        { "ui_research[line_research_115]",//下到上
            "ui_research[line_research_213]"};//一分2 

        public static string[] ResearchLineSp1TO2 = new[]
        {
            "ui_research[line_research_105]",
            "ui_research[line_research_203]"
        };
        
        //下到上
        public static string[] ResearchSubAndCrossLine1 =
            {"", "ui_research[line_research_108]",//下到上
                "ui_research[line_research_115]"};//一分2 
        
        public static string[] ResearchSubAndCrossLine2 =
        {"", "ui_research[line_research_215]", 
            "ui_research[line_research_213]"};//一分2 
        public static string[] RenameOrCopy =
    {"ui_setting[icon_set_rename]",
            "ui_setting[btn_set_copy]"};//改名，复制名

        //跨列
        public static string[][] ResearchSubAndCrossLine = new[] {ResearchSubAndCrossLine1, ResearchSubAndCrossLine2};
        
        //上到下大
        
        
        //上到下小
        
        
        //3分1  位置2-5|8-5 = 3
        private static string[] ResearchLineUpToDown3 =
            new[] {"ui_research[line_research_112]", "ui_research[line_research_207]"};
        
        //4分2  位置1-3|4-3 = 2|1  特殊独立掉
        private static string[] ResearchLineUpToDown42 =
            new[] {"ui_research[line_research_111]", "ui_research[line_research_210]"};
        
        
        //4分1  位置1-5|9-5 = 4  w 118 h 231
        private static string[] ResearchLineUpToDown41l =
            new[] {"ui_research[line_research_113]", "ui_research[line_research_209]"};
        
        //4分1  位置4-5|6-5 = 1  w 119 h81
        private static string[] ResearchLineUpToDown41 =
            new[] {"ui_research[line_research_114]", "ui_research[line_research_210]"};
        
        //2-1   3-5 //TODO 叶滢  资源缺失
         public static string[] ResearchLineUpToDown21 =
                    { "ui_research[line_research_116]","ui_research[line_research_216]"}; 
        
        //差值来算
        public static string[][] ResearchLineUpToDown = new[] {ResearchLineUpToDown41, ResearchLineUpToDown41,ResearchLineUpToDown41,ResearchLineUpToDown3,ResearchLineUpToDown41l};

        public static string[] HospitalMark = new[] { "ui_mainmenu[icon_mmu_hospital_1]", "ui_mainmenu[icon_mmu_hospital_2]" };
        public static string[] HospitalMarkFrame = new[] { "ui_common[icon_com_hospital_1]", "ui_common[icon_com_hospital_2]" };

        public static string Type2Buff = "operation_2001";//城市保护罩
        public static string CityBuff = "UI_10037";//城市buff获得
        public static string ArmyTrainFreeEffect = "build_3007";
        public static string HospitalTreatmentEffect = "build_3002";
        public static string[] cave_lod3_icon = new[] { "map_icon[map_icon_cave]", "map_icon[map_icon_cave1]" };//山洞未探索，已探索图标
        public static string[] village_lod3_icon = new[] { "map_icon[map_icon_village]", "map_icon[map_icon_village1]" };//村庄未探索，已探索图标
        public static string CountDownIcon = "ui_common[icon_com_1001]";
        public static string AgeTips = "UE_AgeChange";
        public static string TaskFinshCollectReward = "UE_TaskFinsh_collectReward";//按钮
        public static string TitleEffect = "UI_10062";//奖励title特效

        public static string TaskBoxBgEffect = "UI_10035";//箱子背后光效
        public static string TaskBoxBgOpenEffect = "UI_10036";//箱子打开光效
        public static string ActionForceFly = "UI_Item_EneryUse";//行动力飘飞

        public static string UI_Common_Crit1 = "UI_Common_Crit1";//倍率提升
        public static string[] ui_num = new[] { "ui_num[img_num_2]", "ui_num[img_num_5]", "ui_num[img_num_10]" };//倍率提升

        public static string RssLevel_Bg = "ui_map[pl_map_levelBg]";//UI_Tip_WorldObjectRssLevelView 默认背景
        
        public static string Atrix_icon_1001 = "matrix_icon[matrix_icon_1001]";//部队缩略图
        public static string Atrix_icon_1003 = "matrix_icon[matrix_icon_1003]";//运输车缩略图
        public static string Atrix_icon_1000 = "matrix_icon[matrix_icon_1000]";//队列在空闲时显示图标
        public static string Atrix_icon_1005 = "matrix_icon[matrix_icon_1005]";//队列在空闲时显示图标
        public static string Atrix_icon_1004 = "matrix_icon[matrix_icon_1004]";//队列在建造、升级中时，显示图标
        public static string MMU_building = "ui_mainmenu[img_mmu_building]";//进度条建造图标
        public static string MMU_levelup = "ui_mainmenu[img_mmu_levelp]";//队列在建造、升级中时，显示图标
        public static string StateCollect1 = "ui_map[img_map_state_collect1]";//我采集中
        public static string StateCollect2 = "ui_map[img_map_state_collect2]";//盟友采集中
        public static string StateCollect3 = "ui_map[img_map_state_collect3]";//其他人采集中
        public static string StateTransport = "ui_map[img_map_state_move]";//运输车
        public static string SoundLogin = "Bgm_Loading";
        public static string Cityfly = "operation_2018";
        public static string CityDown = "operation_2019";
        public static string HolyLandCircle = "operation_2016";
        public static string Sound_Ui_TaskFinish_1 = "Sound_Ui_TaskFinish_1";//卷轴画面时播放音效
        public static string GameSlider_green  = "ui_common[pb_com_1000_4]";
        public static string GameSlider_yellow = "ui_common[pb_com_1000_2]";
        public static string GameSlider_red    = "ui_common[pb_com_1000_6]";
        public static string GameSlider_gray   = "ui_common[pb_com_1000_7]";
        public static string SoundCreateChar = "Bgm_CreatingRole";

        public static string SoundCityDay = "Bgm_PeaceDay";
        
        public static string SoundCityNight = "Bgm_PeaceNight";

        public static string SoundWind = "sfx_env_wind";

        public static string SoundCityBattle = "Bgm_Battle";

        public static string SoundResStart = "Sound_Ui_StartStudy";

        public static string SoundUiStartHealing = "Sound_Ui_SrartHealing";
        public static string SoundUiEndHealing = "Sound_Ui_EndHealing";
        public static string SoundUiTrainingWarriors = "Sound_Ui_TrainingWarriors";
        public static string SoundUiTrainingKnights = "Sound_Ui_TrainingKnights";
        public static string SoundUiTrainingArcher = "Sound_Ui_TrainingArcher";
        public static string SoundUiTrainingSieges = "Sound_Ui_TrainingSieges";
        public static string SoundUiTrainingEnd = "Sound_Ui_TrainingEnd";

        public static string SoundUiSummonEpic = "Sound_Ui_SummonEpic";
        public static string SoundUiSummonRare = "Sound_Ui_SummonRare";
        public static string SoundUiSummonNormal = "Sound_Ui_SummonNormal";

        public static string SoundUiCommonClickButton3 = "Sound_Ui_CommonClickButton3";
        public static string SoundUiDestroyMist = "Sound_Ui_DestroyMist";
        
        //选中城内建筑
        public static string SoundBuildSelected = "Sound_Ui_BuildingSelect";
        public static string SoundTreeSelected = "Sound_Ui_TreeSelect";
        public static string SoundRoadSelected = "Sound_Ui_RoadSelect";
        //移动城内建筑
        public static string SoundBuildMove = "Sound_Ui_BuildingMove";
        public static string SoundBuildSet = "Sound_Ui_BuildingLand";
        public static string SoundBuildingStartLevelup = "Sound_Ui_BuildingStartLevelup";
        //通用按钮点击
        public static string SoundUiCommonClickButton = "Sound_Ui_CommonClickButton";
        //开始升级
        public static string Sound_Ui_BuildingStartLevelup = "Sound_Ui_BuildingStartLevelup";

        public static string SoundVictory = "Sound_Ui_Victory";//战斗胜利表现

        public static string SoundUseExpBook= "Sound_Ui_UseExpBook";
        public static string SoundUiTalentAddPoint = "Sound_Ui_TalentAddPoint";
        public static string SoundUiCommonSlider = "Sound_Ui_CommonScroll";
        public static string Sound_Ui_SelectSelf = "Sound_Ui_SelectSelf";
        public static string SoundUiSkillLvUp = "Sound_Ui_SkillLvUp";
        public static string SoundUiCaptainLvUp = "Sound_Ui_CaptainLvUp";
        public static string SoundUiCommonSidePage = "Sound_Ui_CommonSidePage";
        public static string SoundUiCommonClickButtonCancel = "Sound_Ui_CommonClickButtonCancel";
        public static string SoundUiCommonClickButtonSure = "Sound_Ui_CommonClickButtonSure";

        // 斥候状态
        public static string ScoutStateRest = "ui_map[img_map_state_rest]";            //待命中
        public static string ScoutStateEye = "ui_map[img_map_state_scout]";            //侦查他人
        public static string ScoutStateScope = "ui_map[img_map_state_explore]";        //探索迷雾
        public static string ScoutStateReturn = "ui_map[img_map_state_back]";          //返回
        public static string minorSolderIcon_0 = "troops_head[troops_head0_1]";          //轻伤士兵头像0
        public static string minorSolderIcon_1 = "troops_head[troops_head0_2]";          //轻伤士兵头像1
        
        // 部队行军线和部队名字城堡名字等地图相关颜色
        public static Color white = Color.white;
        public static Color yellow = Color.yellow;
        public static Color red = new Color(225/255.0f, 85/255.0f, 79/255.0f);
        public static Color green = new Color(0.266666f, 0.831372f, 0.509803f);
        public static Color blue = new Color(0.286274f, 0.607843f, 0.854901f);
        public static Color purple = new Color(0.784313f, 0.349019f, 0.835294f);
        //

        public static Color blue_troop = new Color(0.137255f, 0.529411f, 0.831372f);
        public static Color red_troop = new Color(225 / 225.0f, 0 / 70.0f, 0 / 70.0f);

        //自己
        public static Color LodCityMine = new Color(0, 1, 0.46274509803922f, 1);
        //盟友
        public static Color LodCityAlly = new Color(0, 0.7843137254902f, 1);

        public static Color buildRed = new Color(225 / 255.0f, 85 / 255.0f, 79 / 255.0f);
        public static Color buildGreen = new Color(62 / 255.0f, 125 / 255.0f, 195 / 255.0f);
        public static Color buildBlue = new Color(0.286274f, 0.607843f, 0.854901f);

        public static Color LodGuildBuildMine = new Color(0, 0.7843137254902f, 1);
        
        public static Color LodGuildBuildAlly = Color.red;
        public static Color OriginDenarTextColor = new Color(1, 1, 1,1); //代币充足的字体颜色

        //领取资源的音效
        public static string[] HarvestRssSound = new[] { "Sound_Ui_GetFood", "Sound_Ui_GetWood", "Sound_Ui_GetStone", "Sound_Ui_GetGlod", "Sound_Ui_GetSthButton","Sound_Ui_GetGlod" };

        //斥候的音效
        public static string CreateScoutSound = "Sound_Ui_CreateScout";
        //不能探索迷雾的音效
        public static string UnExploreSound = "sfx_cant";
        //升级特效的声音
        public static string sfx_buildingUp = "sfx_buildingUp";
        public static string Sound_Ui_WinGetReward = "Sound_Ui_WinGetReward";
        public static string sfx_collect_reward = "sfx_collect_reward";
        public static string HudInvestigationIcon = "ui_hud[btn_hud_1041]";
        public static string HudAttackIcon = "ui_hud[btn_hud_1001]";
        public static string HudHeroWhite = "hero_head[img_com_heroframe_white]";
        public static string HudHeroGreen = "hero_head[img_com_heroframe_green]";
        public static string HudHeroBlue = "hero_head[img_com_heroframe_blue]";
        public static string HudHeroRed = "hero_head[img_com_heroframe_red]";
        public static string FightHudUILineGreen = "img_map_line[img_map_line]";
        public static string FightHudUILineBlue = "img_map_line[img_map_line]";
        public static string FightHudUILineRed = "img_map_line[img_map_line]";
        public static string FightHudUILineWhite = "img_map_line[img_map_line]";
        public static string FightHudUIHeadBorderRed = "hero_head[img_com_heroframe_red]";
        public static string HudRally = "ui_hud[btn_hud_1000]";

        public static string PlayerFightArmyFrame = "hero_head[img_com_heroframe_green]";
        public static string MonsterFightArmyFrame = "hero_head[img_com_heroframe_red]";
        public static string AllianceFightArmyFrame = "hero_head[img_com_heroframe_blue]";
        public static string OtherMapFightFrame = "hero_head[img_com_heroframe_white]";


        public static string[] TroopsSaveIcon = new[]
            {"ui_troops[pl_tro_1006_1]", "ui_troops[pl_tro_1006_2]", "ui_troops[pl_tro_1006_3]"};
        public static string[] TroopsSaveDefIcon = new[]
            {"ui_troops[pl_tro_1007_1]", "ui_troops[pl_tro_1007_2]", "ui_troops[pl_tro_1007_3]"};

        public static string[] TroopsSaveBtnIcon = new[]
        {
            "ui_troops[btn_tro_1003_1]",
            "ui_troops[btn_tro_1003_2]",
            "ui_troops[btn_tro_1003_3]"
        };
        public static string[] BuildScaffold = new[]//脚手架
    {
            "Build_shelf_1",
            "Build_shelf_2",
            "Build_shelf_3",
            "Build_shelf_4",
            "Build_shelf_5",
            "Build_shelf_6",
            "Build_shelf_7",
            "Build_shelf_8",
            "Build_shelf_9",
            "Build_shelf_10",
            "Build_shelf_11",
            "Build_shelf_12",
        };

        public static string[] TroopsSaveBtnDefIcon = new[]
        {
            "ui_troops[btn_tro_1004_1]",
            "ui_troops[btn_tro_1004_2]",
            "ui_troops[btn_tro_1004_3]"
        };
        public static string[] TransportNameIndex = new string[]{"A","B","C","D","E"};

        public static string TavernSilverBoxIcon = "ui_res_build[img_tavern_box1]";
        public static string TavernGoldBoxIcon = "ui_res_build[img_tavern_box2]";
        public static string[] TavernSummonBoxKey = new[] { "item5[item_5_1023]", "item5[item_5_1024]" };
        public static string[] TavernBoxShowEffect = new[] { "UI_10060_1", "UI_10060_2", "UI_10060_3", "UI_10060_4" };
        public static string[] TavernBoxBottomEffect = new[] { "UI_10061_1", "UI_10061_2", "UI_10061_3", "UI_10061_4" };

        public static string ResourcesConsumeDefaultColor = "#D9D4BA";
        public static string ResourcesConsumeInBCBDefaultColor = "#232320";
        public static int[] AllianceTerrtriyHeadTitle = new[] {732002, 732092, 732004, 732037};
        
        public static int[] AllianceTerrtriyTagTitle = new[] {732068, 732069, 732003, 732070};
        
//# 0 失效
//# 1 建造中
//# 2 正常
//# 3 燃烧中
//# 4 维修中
//# 5 战斗中
//# 6 采集中
        public static int[] AllianceBuildStatie = new[] {732064, 732093, 732060, 732061, 732062, 732063};

        public static string GetGuildBuildStateColor(GuildBuildInfoEntity buildInfo)
        {
            var state = buildInfo.status;

            if (buildInfo.isBattle)
            {
                state =(long) GuildBuildState.battle;
            }

            return RS.AllianceBuildStateColor[state];
        }
        
        public static string GetGuildBuildStateColor(MapObjectInfoEntity buildInfo)
        {
            var state = buildInfo.guildBuildStatus;

            if (TroopHelp.GetTroopState(buildInfo.status) == Troops.ENMU_SQUARE_STAT.FIGHT)
            {
                state =(long) GuildBuildState.battle;
            }

            return RS.AllianceBuildStateColor[state];
        }
        
        public static string GetGuildBuildStateColor(GuildBuildInfo buildInfo)
        {
            var state = buildInfo.status;

            if (buildInfo.isBattle)
            {
                state =(long) GuildBuildState.battle;
            }

            return RS.AllianceBuildStateColor[state];
        }

        public static string GetGuildBuildState(GuildBuildInfoEntity buildInfo)
        {
            if (buildInfo.isBattle)
            {
                return LanguageUtils.getText(732063);
            }
            return LanguageUtils.getText(AllianceBuildStatie[buildInfo.status]);
        }
        
        public static string GetGuildBuildState(GuildBuildInfo buildInfo)
        {
            if (buildInfo.isBattle)
            {
                return LanguageUtils.getText(732063);
            }
            return LanguageUtils.getText(AllianceBuildStatie[buildInfo.status]);
        }

      
        
        public static string GetGuildBuildState(MapObjectInfoEntity buildInfo)
        {
            if ( TroopHelp.GetTroopState(buildInfo.status) == Troops.ENMU_SQUARE_STAT.FIGHT)
            {
                return LanguageUtils.getText(732063);
            }
            return LanguageUtils.getText(AllianceBuildStatie[buildInfo.guildBuildStatus]);
        }
        
                                                                // 失效    建造        正常        燃烧        维修        战斗     采集
        private static string[] AllianceBuildStateColor= new[] {"#C0C0C0", "#007FFF", "#007FFF", "#FF4F38", "#32CD32", "#FF4F38","#007FFF"};
        
        public static string[] AllianceBuildStateImg= new[] {"ui_common[pb_com_1000_4]", "ui_common[pb_com_1000_4]", "ui_common[pb_com_1000_4]", "ui_common[pb_com_1000_6]", "ui_common[pb_com_1000_2]", "ui_common[pb_com_1000_4]"};
        
        
        
        public static string[] AllianceBuildTypeTag = new[]
        {
            "ui_alliance[img_ali_ter1]", "ui_alliance[img_ali_ter2]", "ui_alliance[img_ali_ter3]",
            "ui_alliance[img_ali_ter4]"
        };

        public static string[] ActivityDateProgressBar = new[] {
            "ui_activity[pb_activity_1000]",
            "ui_activity[pb_activity_1001]",
            "ui_activity[pb_activity_1002]",
            "ui_activity[pb_activity_1003]",
        };
        public static string[] ActivityRankIconBg = new[] {
            "ui_activity[img_activity_rank1]",
            "ui_activity[img_activity_rank2]",
            "ui_activity[img_activity_rank3]",
        };
        public static string[] ActivityRankRewardTitleBg = new[] {
            "ui_activity[img_activity_title1]",
            "ui_activity[img_activity_title2]",
            "ui_activity[img_activity_title3]",
            "ui_activity[img_activity_title4]",
        };

        public const string PlayerPrefs_Key_HasShownChargePop = "hasShownChargePop";

        public const string RoleCommonHeadFrame = "player_head[img_player_frame]";
        public const string RoleCommonHead = "player_head[img_player_head_empty]";
        public const string BattleHudHeadEffect = "UI_10026";

        public static string[] TuneFrameImage = new[]
        {
            "rune_icon[rune_icon_frame1]",
            "rune_icon[rune_icon_frame2]",
            "rune_icon[rune_icon_frame3]",
            "rune_icon[rune_icon_frame4]",
            "rune_icon[rune_icon_frame5]",
        };

        public static string[] GuildRssTypeIcons = new[]
        {
            "ui_common[icon_com_alicur1]", "ui_common[icon_com_alicur2]", "ui_common[icon_com_alicur3]",
            "ui_common[icon_com_alicur4]"
        };
        
        #region 折扣图标

        public const string YellowPriceBg = "ui_build[img_build_1000]";
        public const string PurplePriceBg = "ui_build[img_build_1001]";

        public const string VipStoreYellowPriceBg = "ui_build[img_build_1005]";
        public const string VipStorePurplePriceBg = "ui_build[img_build_1006]";
        #endregion
        
        public static string[] RankingTop3IconName = new[] {"ui_activity[img_activity_rank1]", "ui_activity[img_activity_rank2]", "ui_activity[img_activity_rank3]"};

        public static string[] DonateRankingIcon = new[] { "ui_common[img_com_rank1]", "ui_common[img_com_rank2]", "ui_common[img_com_rank3]" };
        //道具品质特效
        public static string[] ItemEffectName = new[]
            {"UI_10039_1", "UI_10039_2", "UI_10039_3", "UI_10039_4", "UI_10039_5",};

        public static string GuildBuildState_building = "ui_map[img_map_state_build]";
        public static string GuildBuildState_battle = "ui_map[img_map_state_war]";
        public static string GuildBuildState_fire = "ui_map[img_map_state_fire]";
        
        
        public static long[] GuildCurrencyIDs = new[] {100L, 101, 102, 103};
        public static int[] GuildResPointIDs = new[] {4, 5, 6, 7};
        
        public static string CurCurrencyDisplay = "$";

        public static string VipStoreBubbleIcon = "vip_icon[vip_icon_101]";

        public static string HeroState_defendCity = "ui_map[img_map_state_def]";

        public static string EventCalendarIcon = "activity_icon[activity_icon_1000]";

        public static string[] TurntableActivityItemIcon = new[] { "ui_mainmenu[icon_mmu_apevent_fra1]", "ui_mainmenu[icon_mmu_apevent_fra2]", "ui_mainmenu[icon_mmu_apevent_fra3]",
                                                                   "ui_mainmenu[icon_mmu_apevent_fra4]", "ui_mainmenu[icon_mmu_apevent_fra5]"};
    }
}
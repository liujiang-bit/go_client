// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    GameToolMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;
using Hotfix;
using System;
using Data;
using UnityEngine.Profiling;
using System.Text;
using IGGSDKConstant;
using Sproto;
using U3D.Threading.Tasks;

namespace Game
{
    public class GameToolMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "GameToolMediator";

        private static Troops m_Formation = null;
        private static Troops m_FormationTwo = null;
        private static Guardian formationGuardian = null;
        private static GameObject m_FormationMoveAtkTargetGo = null;
        private static float m_MoveAtkAngleCount = 225;
        private Timer m_Timer = null;
        private static List<TroopsHero> m_heros = null;
        private static List<TroopsCell> m_units = null;
        private GameObject mWegamer;

        #endregion

        //IMediatorPlug needs
        public GameToolMediator(object viewComponent) : base(NameMediator, viewComponent)
        {
        }


        public GameToolView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                    break;
            }
        }


        #region UI template method

        public override void OpenAniEnd()
        {
        }

        public override void WinFocus()
        {
        }

        public override void WinClose()
        {
            CoreUtils.assetService.UnloadUnusedAssets();
        }

        public override void PrewarmComplete()
        {
        }

        public override void Update()
        {
          
        }

        public static void UpdateView()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (m_Formation != null)
                {
                    Troops.TriggerSkillS(m_Formation, "1001", Vector3.zero);
                }
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (m_FormationTwo != null)
                {
                    Troops.TriggerSkillS(m_FormationTwo, "1001", Vector3.zero);
                }
            }
        }

        protected override void InitData()
        {
            IsOpenUpdate = true;
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (playerProxy.CurrentRoleInfo != null)
                view.m_lbl_rid_LanguageText.text = string.Format("Rid:{0}", playerProxy.CurrentRoleInfo.rid);

            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
            ClientUtils.LoadSprite(view.m_img_test_PolygonImage, "rune_icon[rune_icon_101]");
        }
        protected override void BindUIEvent()
        {
            view.m_ls_template_BtnAnimation.gameObject.SetActive(true);
        }

        protected override void BindUIData()
        {
            AddMenu("测试。。。", () =>
            {
                //long? session = null;
                //var bHasSession = session != null;
                //AppFacade.GetInstance().SendNotification(CmdConstant.MoveToPosFixedCameraDxf, new Vector2(0, 0));

                Timer.Register(1.0f, () =>
                {
                    var troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
                    var armys = troopProxy.GetArmys();
                    foreach (var army in armys)
                    {
                        CoreUtils.audioService.PlayOneShot(RS.SoundUiCaptainLvUp);
                        AppFacade.GetInstance().SendNotification(CmdConstant.FightUpdateHeroLevel, army.mainHeroId);
                        //WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)army.objectIndex, BattleUIType.UpdateHeroLevel, (int)army.mainHeroId);
                    }
                });
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("测试重启", () =>
            {
                Alert.CreateAlert(LanguageUtils.getTextFormat(100025, HotfixUtil.FormatFileSize(1))).SetRightButton(() =>
                {
                    // 直接重启
                    CoreUtils.RestarGame();
                }).Show();
            });
            AddMenu("测试00点的迷雾淡出", () =>
            {
                List<Vector2Int> tiles = new List<Vector2Int>();
                for(int i = 0; i < 20; i++)
                {
                    for (int j= 0; j < 20; j++)
                    {
                        tiles.Add(new Vector2Int(i, j));
                    }

                }
                WarFogMgr.CrateFadeFog(tiles.ToArray());

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("测试账号被封", () =>
            {
                SendNotification(CmdConstant.AccountBan);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("发送引导完成事件", () =>
            {
                //CoreUtils.adService.SendEvent("Purchases999", "");
                //CoreUtils.adService.SendEvent("Tutorials Completed", "");
            });
            AddMenu("SDK账号失效", () =>
            {
                IGGSession.invalidateCurrentSession();
            });
#if UNITY_ANDROID && !UNITY_EDITOR
            AddMenu("测试获取权限", () =>
            {
                Debug.LogWarning("需要获取读卡权限");

                string[] permission = new string[] { "android.permission.READ_EXTERNAL_STORAGE" };
                Permissions.requestPermissions(permission, (int requestCode, string[] permissions, int[] grantResults) =>
                {
                    Task.RunInMainThread(()=>
                    {
                        if (grantResults[0] == 0)
                        {
                            Debug.Log("获取权限成功");
                        }
                        else
                        {
                            Permissions.GotoSetting("\"External Storage Read\" is required. Please grant it in \"application information > permission\"");
                        }
                    });
                });
            });
#endif
            AddMenu("当前IGGID", () =>
            {
                view.m_lbl_rid_LanguageText.text = IGGSession.currentSession.getAccesskey();
                Debug.Log(IGGSession.currentSession.ToString());
                string copyBuffer = $"{IGGSession.currentSession.ToString()}\nPwd:{IGGSDK.appConfig.getClientIp()}|{IGGSession.currentSession.getAccesskey()}";
                GUIUtility.systemCopyBuffer = copyBuffer;
                IGGSDKUtils.shareInstance().ShowToast(IGGSession.currentSession.ToString());
            });
            AddMenu("打印所有属性", () =>
            {
                var playerAttributesProxy =
                    AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;

                var count = Enum.GetValues(typeof(attrType)).Length - 1;
                for (int i = 0; i < count; i++)
                {
                    var type = (attrType) (i + 1);
                    var attribute = playerAttributesProxy.GetCityAttribute(type);
                    if (attribute != null)
                    {
                        Debug.Log($"{type}:{attribute.GetShowValue()}");
                    }
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });

            AddMenu(HotfixUtil.IsShowLoginView() ? "关闭登陆界面" : "开启登陆界面", () =>
            {
                HotfixUtil.SetShowLoginView(!HotfixUtil.IsShowLoginView());
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });

            AddMenu(HotfixUtil.IsDebugable()?"关闭调试":"开启调试", () =>
            {
                if(HotfixUtil.IsDebugable())
                {
                    HotfixUtil.SetDebugable(false);
                    GameObject debugConsole = GameObject.Find("IngameDebugConsole");
                    if (debugConsole != null)
                        CoreUtils.assetService.Destroy(debugConsole);
                    GameObject graphy = GameObject.Find("Graphy");
                    if (graphy != null)
                        CoreUtils.assetService.Destroy(graphy);
                }
                else
                {
                    HotfixUtil.SetDebugable(true);
                    if (GameObject.Find("IngameDebugConsole") == null)
                    {
                        CoreUtils.assetService.Instantiate("IngameDebugConsole", (gameObject) => { });
                        CoreUtils.assetService.Instantiate("Graphy", (gameObject) => { });
                        HotfixUtil.SetDebugable(true);
                    }
                }
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu(HotfixUtil.IsLogShow() ? "关闭日记" : "开启日记", () =>
            {
                if (HotfixUtil.IsLogShow())
                {
                    HotfixUtil.SetLogShow(false);
                    UnityEngine.Debug.unityLogger.filterLogType = LogType.Error;
                    PlayerPrefs.SetInt("LogType", (int)LogType.Error);
                    PlayerPrefs.Save();
                }
                else
                {
                    HotfixUtil.SetLogShow(true);
                    UnityEngine.Debug.unityLogger.filterLogType = LogType.Log;
                    PlayerPrefs.SetInt("LogType", (int)LogType.Log);
                    PlayerPrefs.Save();
                }
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu(HotfixUtil.IsShowLoginView()?"关闭登陆界面":"开启登陆界面", () =>
            {
                HotfixUtil.SetShowLoginView(!HotfixUtil.IsShowLoginView());
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu(HotfixUtil.IsDebugServerConfig() ? "使用正式服" : "使用测试服", () =>
            {
                HotfixUtil.SetDebugServerConfig(!HotfixUtil.IsDebugServerConfig());
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("切换语言", () =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
                CoreUtils.uiManager.ShowUI(UI.s_Pop_LanguageSet, null, LanguageSetMediator.OpenType.Start);
            });
            AddMenu("清楚缓存(语言和账号)", () =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
                LanguageUtils.ClearCache();
                PlayerPrefs.DeleteKey("lastLoginName");
                PlayerPrefs.DeleteKey("InitQualitySetting");
                PlayerPrefs.DeleteKey("PlatfommScore");
                var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                bagProxy.SetFirstGetItem(true);
                PlayerPrefs.Save();
            });
#if UNITY_EDITOR
            AddMenu("昼夜变化轮替", () =>
            {
                Color[][] lightSetting = new[]
                {
                    new[]
                    {
                        new Color(0.6470588f, 0.6470588f, 0.6470588f),
                        new Color(0.54509803921569f, 0.30980392156863f, 0.14117647058824f)
                    },
                    new[] {new Color(0.53676474f, 0.53676474f, 0.53676474f), new Color(0.7137f, 0.7137f, 0.7137f)},
                    new[]
                    {
                        new Color(0.6470588f, 0.6470588f, 0.6470588f),
                        new Color(0.54509803921569f, 0.30980392156863f, 0.14117647058824f)
                    },
                    new[]
                    {
                        new Color(0.202f, 0.317f, 0.654f),
                        new Color(0.20392156862745f, 0.41960784313725f, 0.79607843137255f)
                    }
                };
                float[] m_lightSettingValue = new[] { 0.85f, 1f, 0.85f, 0.85f };

                int light_index = PlayerPrefs.GetInt("lightindex", 0);
                int next_light_index = light_index + 1;
                if (next_light_index == lightSetting.Length)
                {
                    next_light_index = 0;
                }

                PlayerPrefs.SetInt("lightindex", next_light_index);

                var org_ambient_color = lightSetting[light_index][0];
                var org_direction_color = lightSetting[light_index][1];
                var org_direction_intensity = m_lightSettingValue[light_index];
                var new_ambient_color = lightSetting[next_light_index][0];
                var new_direction_color = lightSetting[next_light_index][1];
                var new_direction_intensity = m_lightSettingValue[next_light_index];

                LightManager.Instance().SetIsNight(light_index == 2);

                LightManager.Instance().UpdateLighting(
                    org_ambient_color,
                    org_direction_color,
                    org_direction_intensity,
                    new_ambient_color,
                    new_direction_color,
                    new_direction_intensity,
                    5,
                    5
                );
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
#endif
            AddMenu("创建行军线", () =>
            {
                var cityBuildingProxy =
                    AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                var pos = cityBuildingProxy.RolePos;

                WorldCamera.Instance().ViewTerrainPos(pos.x, pos.y, 500, () => { });

                List<Vector2> paths = new List<Vector2>();
                paths.Add(new Vector2(pos.x, pos.y - 20));
                paths.Add(new Vector2(pos.x, pos.y - 3.5f));
                paths.Add(new Vector2(pos.x - 3.5f, pos.y));
                paths.Add(new Vector2(pos.x, pos.y + 3.5f));
                paths.Add(new Vector2(pos.x, pos.y + 20));
                MarchLineMgr.Instance().CreateTroopLine((line) =>
                {
                    MarchLineMgr.Instance().SetTroopLinePath(line, paths.ToArray());
                }, "troop_line_send_troop");
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("采集设备信息", () =>
            {
                SendClientDeviceInfoMedia.ReportData();
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("完成章节", () =>
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.ChapterIdChange, 1L);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("时代变迁", () =>
            {
                CityGlobalMediator cityGlobalMediator =
                    AppFacade.GetInstance().RetrieveMediator(CityGlobalMediator.NameMediator) as CityGlobalMediator;
                cityGlobalMediator.AgeChangeEffect();
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);

                SendNotification(CmdConstant.OpenAppRating);
            });
            AddMenu("创建普通部队", () =>
            {
                CreateFormation(Troops.ENMU_MATRIX_TYPE.COMMON);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("创建野蛮人部队", () =>
            {
                CreateFormation(Troops.ENMU_MATRIX_TYPE.BARBARIAN);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("部队移动", () =>
            {
                if (m_Formation != null)
                {
                    var pos = Troops.GetShowPositionS(m_Formation);
                    var cur_pos = new Vector2(pos.x, pos.z);
                    var target_pos =
                        cur_pos + new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3));
                    //var target_pos = cur_pos + (new Vector2(m_Formation.transform.forward.x, m_Formation.transform.forward.z).normalized * 5.0f);
                    Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.MOVE, cur_pos, target_pos, 1.2f);
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("部队误差移动", () =>
            {
                if (m_Formation != null)
                {
                    var pos = Troops.GetShowPositionS(m_Formation);
                    var cur_pos = new Vector2(pos.x+0, pos.z+3);
                    var target_pos =
                        cur_pos + new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3));
                    //var target_pos = cur_pos + (new Vector2(m_Formation.transform.forward.x, m_Formation.transform.forward.z).normalized * 5.0f);
                    Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.MOVE, cur_pos, target_pos, 1.2f);
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("围击", () =>
            {
                if (m_Formation != null)
                {
                    float targetSideDis = 3.6f;
                    var cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                    Vector2 rolePos = new Vector2(cityBuildingProxy.RolePos.x + 8, cityBuildingProxy.RolePos.y);
                    Vector2 target_pos = rolePos + new Vector2(targetSideDis, targetSideDis);

                    if (m_FormationMoveAtkTargetGo == null)
                    {
                        var landRoot = GameObject.Find("SceneObject/land_root").transform;
                        CoreUtils.assetService.Instantiate("troop_line_mine_end", (targetGo) =>
                        {
                            m_FormationMoveAtkTargetGo = targetGo;
                            m_FormationMoveAtkTargetGo.transform.SetParent(landRoot);
                            m_FormationMoveAtkTargetGo.transform.localPosition = new Vector3(target_pos.x, 0, target_pos.y);
                        });
                    }

                    double targetDis = Math.Sqrt(targetSideDis * targetSideDis + targetSideDis * targetSideDis);
                    float angle = (m_MoveAtkAngleCount += 22.5f) % 360;
                    float cur_pos_x = (float)(target_pos.x + targetDis * Math.Cos(angle * 3.14 / 180));
                    float cur_pos_y = (float)(target_pos.y + targetDis * Math.Sin(angle * 3.14 / 180));
                    Vector2 cur_pos = new Vector2(cur_pos_x, cur_pos_y);

                    Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.FIGHT, cur_pos, target_pos);
                    Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.FIGHT, cur_pos, target_pos);
                    Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.FIGHT, cur_pos, target_pos);
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("部队攻击", () =>
            {
                if (m_Formation != null)
                {
                    var pos = m_Formation.transform.position;
                    var cur_pos = new Vector2(pos.x, pos.z);
                    var target_pos =
                        cur_pos + (new Vector2(m_Formation.transform.forward.x, m_Formation.transform.forward.z)
                                       .normalized * 0.001f);
                    Debug.Log($"{cur_pos.ToString()}, {target_pos.ToString()}");
                    // 攻击态
                    Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.FIGHT, cur_pos, target_pos, 1.2f);
                    // 攻击阵型
                    Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.FIGHT, cur_pos, target_pos, 1.2f);
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("部队环绕", () =>
            {
                if (m_Formation != null)
                {
                    var pos = m_Formation.transform.position;
                    var cur_pos = new Vector2(pos.x, pos.z);
                    var target_pos =
                        cur_pos + (new Vector2(m_Formation.transform.forward.x, m_Formation.transform.forward.z)
                                       .normalized * 2.0f);
                    Debug.Log($"{cur_pos.ToString()}, {target_pos.ToString()}");
                    // 攻击态
                    Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.FIGHT, cur_pos + Vector2.left*2.0f, target_pos, 0.0f);
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("部队淡入", () =>
            {
                if (m_Formation != null)
                {
                    Troops.FadeIn_S(m_Formation);
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("部队淡出", () =>
            {
                if (m_Formation != null)
                {
                    Troops.FadeOut_S(m_Formation);
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("部队减员", () =>
            {
                if (m_Formation != null)
                {
                    bool bIsDead = false;
                    foreach (var unit in m_units)
                    {
                        unit.unitCount -= ((unit.unitMaxCount / 5) - 1);
                        if (unit.unitCount < 0)
                        {
                            unit.unitCount = 0;
                            bIsDead = true;
                        }
                    }

                    if (bIsDead)
                    {
                        var pos = m_Formation.transform.position;
                        var cur_pos = new Vector2(pos.x, pos.z);
                        Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.DEAD, pos, pos, 0.0f);
                    }
                    else
                    {
                        string des = SquareHelper.Instance.GetSquareString(m_units, m_heros,
                            (int) Troops.ENMU_MATRIX_TYPE.COMMON);
                        Troops.SetFormationInfoS(m_Formation, des);
                    }
                }

                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });

            AddMenu("创建部队2", () =>
            {
                CreateFormationTwo(Troops.ENMU_MATRIX_TYPE.COMMON);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });

            AddMenu("部队交战", () =>
            {
                CreateFormation(Troops.ENMU_MATRIX_TYPE.COMMON);
                CreateFormationTwo(Troops.ENMU_MATRIX_TYPE.COMMON, () => { CreateAttackFormation(); });
            });

            AddMenu("部队主动播放特效", () =>
            {
                if (m_Formation != null)
                {
                    Troops.TriggerSkillS(m_Formation, "1001", Vector3.zero);
                }
            });

            AddMenu("创建圣地守护者",()=>
            {
                CreateGuardian();
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            
            AddMenu("圣地守护者移动", () =>
            {
                if (formationGuardian != null)
                {
                    Vector2 curPos= new Vector2(formationGuardian.transform.position.x,formationGuardian.transform.position.z);
                    var target_pos =
                        curPos + new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3));
                    Guardian.SetStateS(formationGuardian,Troops.ENMU_SQUARE_STAT.MOVE,curPos,target_pos,0.5f);
                }
            });
            
            AddMenu("圣地守护者战斗", () =>
            {
                if (formationGuardian != null)
                {
                    Vector2 curPos= new Vector2(formationGuardian.transform.position.x,formationGuardian.transform.position.z);
                    var target_pos =
                        curPos + new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3));
 
                    Guardian.SetStateS(formationGuardian,Troops.ENMU_SQUARE_STAT.FIGHT,curPos,target_pos,0.5f);
                    Guardian.SetStateS(formationGuardian,Troops.ENMU_SQUARE_STAT.FIGHT,curPos,target_pos,0.5f);
                    Guardian.TriggerSkillS(formationGuardian,string.Empty);
                }
            });
            
            
            AddMenu("测试所有表格", () =>
            {
                TestTable();
            });
            AddMenu("UI组件demo", () =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
                CoreUtils.uiManager.ShowUI(UI.s_testUICompoment);
            });
            AddMenu("重启", () =>
            {
                CoreUtils.RestarGame();
            });
            AddMenu("跳过新手引导", () =>
            {
                GuideProxy proxy = AppFacade.GetInstance().RetrieveProxy(GuideProxy.ProxyNAME) as GuideProxy;
                Int64 noviceGuideStep = 0;
                for (int i = 1; i < 13; i++)
                {
                    noviceGuideStep = noviceGuideStep | proxy.ConvertStage(i);
                }
                var sp = new Role_NoviceGuideStep.request();
                sp.noviceGuideStep = noviceGuideStep;
                sp.noviceGuideDetailStep = 0;
                AppFacade.GetInstance().SendSproto(sp);

                AppFacade.GetInstance().SendNotification(CmdConstant.ForceCloseGuide);
            });
            AddMenu("模拟添加奇观建筑(坐标自己设置)", () =>
            {
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
                
                CoreUtils.logService.Warn($" ============   生成测试奇观");
                Map_ObjectInfo.request req = new Map_ObjectInfo.request();
                var mapObjectInfo = new MapObjectInfo();
                req.mapObjectInfo = mapObjectInfo;
                mapObjectInfo.objectId = 999;
                mapObjectInfo.objectType = 25;
                mapObjectInfo.strongHoldId = 10007;
                mapObjectInfo.holyLandStatus = 1;
                mapObjectInfo.objectPos = new PosInfo();
                mapObjectInfo.objectPos.x = 582000;
                mapObjectInfo.objectPos.y = 119400;
                    
                WorldMapObjectProxy proxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                proxy.UpdateMapObject(req);

            });
            AddMenu("加载远征地图", () =>
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.EnterExpeditionMap, 1);
            });
            AddMenu("行军线调试", () =>
            {
                var troopMedia = GlobalBehaviourManger.Instance.GetGlobalMediator(TroopLineMediator.NameMediator) as TroopLineMediator;
                if (troopMedia == null)
                {
                    troopMedia = GlobalBehaviourManger.Instance.AddGlobalMeditor<TroopLineMediator>(false);
                }
                troopMedia.ShowUI(true);
                CoreUtils.uiManager.CloseAll();
            });
            AddMenu("部队套上包围盒", () =>
            {
                var formations = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroops();
                foreach(var formation in formations)
                {
                    if(!formation.transform.Find("Cube"))
                    {
                        GameObject obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        obj1.transform.SetParent(formation.transform);
                        obj1.transform.localScale = new Vector3(3, 1, 3);
                        obj1.transform.localPosition = Vector3.zero;
                        obj1.transform.localRotation = Quaternion.identity;
                        obj1.layer = LayerMask.NameToLayer("Default");
                        var boxCollider = obj1.GetComponent<BoxCollider>();
                    }
                }
            });
            AddMenu("部队移除包围盒", () =>
            {
                var formations = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroops();
                foreach (var formation in formations)
                {
                    var cube = formation.transform.Find("Cube");
                    if (cube != null)
                    {
                        GameObject.DestroyImmediate(cube.gameObject);
                    }
                }
            });

            AddMenu("逃跑", () =>
            {
                var md = AppFacade.GetInstance()
                    .RetrieveMediator(CityGlobalMediator.NameMediator) as CityGlobalMediator;
                
                md.CreateRunCitizen();
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });

            AddMenu("屏蔽UI", () =>
            {
                //支持配置任意UI层级的显隐,999特指Debug相关UI
                //避免配置BrowserLayer显隐，防止GameTool相关界面无法显示
                Dictionary<string, int> OptionInfoDic = new Dictionary<string, int>();
                OptionInfoDic.Add("显示Debug", 999);
                OptionInfoDic.Add("显示主界面", (int)UILayer.FullViewLayer);
                OptionInfoDic.Add("显示Hud", (int)UILayer.HUDLayer);
                OptionInfoDic.Add("显示窗口", (int)UILayer.WindowLayer);
                OptionInfoDic.Add("显示弹窗", (int)UILayer.WindowPopLayer);

                CoreUtils.uiManager.ShowUI(UI.s_gameToolViewSetting, null, OptionInfoDic);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("重置埋点", () =>
            {
                Role_UpdateEventTrancking.request request = new Role_UpdateEventTrancking.request();
                request.eventTrancking = 0;
                AppFacade.GetInstance().SendSproto(request);
                PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                playerProxy.CurrentRoleInfo.eventTrancking = 0;
            });
            
            AddMenu("打印玩家城市", () =>
            {
                WorldMapObjectProxy mapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                {
                    MapObjectInfoEntity mapObjectInfoEntity = mapObjectProxy.GetWorldMapObjectByobjectId(mapObjectProxy.MyCityId);
                    if (mapObjectInfoEntity != null)
                    {
                        Debug.LogErrorFormat("objectId{0}Rid{1}pX{2}pY{3}", mapObjectInfoEntity.objectId, mapObjectInfoEntity.cityRid, mapObjectInfoEntity.objectPos.x, mapObjectInfoEntity.objectPos.y);
                    }
                }
                {
                    MapObjectInfoEntity mapObjectInfoEntity = mapObjectProxy.GetWorldMapObjectByRid(playerProxy.CurrentRoleInfo.rid);
                    if (mapObjectInfoEntity != null)
                    {
                        Debug.LogErrorFormat("objectId{0}Rid{1}pX{2}pY{3}", mapObjectInfoEntity.objectId, mapObjectInfoEntity.cityRid, mapObjectInfoEntity.objectPos.x, mapObjectInfoEntity.objectPos.y);
                    }
                }
            });
            //AddMenu("测试热更新", () =>
            //{
            //    Alert.CreateAlert(900573).SetLeftButton().Show();
            //});
            AddMenu("压包解包", () =>
            {
                SprotoPack packObj = new SprotoPack();

                BattleReportEx exInfo = new BattleReportEx();
                exInfo.battleBeginTime = 101;
                exInfo.battleEndTime = 201;
                exInfo.deputyHeroExp = 1000;

                Debug.Log("压包前");
                ClientUtils.Print(exInfo);

                Sproto.SprotoStream stream = new Sproto.SprotoStream();
                exInfo.encode(stream);
                byte[] byteArr = packObj.pack(stream.Buffer, 0);

                byte[] unData = packObj.unpack(byteArr);
                BattleReportEx exInfo1 = new BattleReportEx(unData);
                Debug.Log("解包后");
                ClientUtils.Print(exInfo1);

            });

            AddMenu("创建集结部队", () =>
            {
                CreateFormationTwo(Troops.ENMU_MATRIX_TYPE.RALLY);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            
            AddMenu("集结部队士兵信息变化", () =>
            {
                SetFormationPlayInfo();
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            

            AddSDKMenu();
        }


        private void AddSDKMenu()
        {

            AddMenu("账户中心", () =>
            {
                IGGAccountManagementGuideline.shareInstance().loadUserFromServerOrCache((IGGException exception, IGGUserProfile userProfile) =>
                {
                    if (exception.isNone())
                    {
                        //IGGSDKAccountInfoUI.ShowUI();
                    }
                });
            });

            AddMenu("游戏商店", () =>
            {
                //IGGSDKShopUI.ShowUI();
            });
            AddMenu("账号失效", () =>
            {
                IGGSession.invalidateCurrentSession();
            });
            AddMenu("协议列表", () =>
            {
                IGGSDK.shareInstance().getAgreementSigning();
                var mIGGAgreementSigning = IGGSDK.shareInstance().getAgreementSigning();
                mIGGAgreementSigning.requestAssignedAgreements((IGGException exception, IGGAssignedAgreements assignedAgreements) =>
                {
                    if (exception.isNone())
                    {
                        var list = assignedAgreements.getAgreements();
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < list.Count; i++)
                        {
                            var ag = list[i];
                            sb.Append(ag.ToString());
                            Debug.Log(ag.ToString());
                        }
                        IGGSDKUtils.shareInstance().ShowToast(sb.ToString());
                    }
                    else
                    {
                        Debug.Log(exception.ToString());
                    }
                });
            });
            AddMenu("终止协议(CN)", () =>
            {
                var mIGGAgreementSigning = IGGSDK.shareInstance().getAgreementSigning();
                mIGGAgreementSigning.termination().requestAssignedAgreements((IGGException exception, IGGAgreementSignedFile signingFile, IGGAgreementTerminationAlert alert) =>
                {
                    if (exception.isNone())
                    {
                        var list = signingFile.getAgreements();
                        for (int i = 0; i < list.Count; i++)
                        {
                            var ag = list[i];
                            Debug.Log(ag.ToString());
                        }

                        IGGSDKUtils.shareInstance().ShowMsgBox(alert.getLocalizedCaption(),
                            alert.getLocalizedTitle(),
                            alert.getLocalizedActionDismiss(),
                            (bool bSure) =>
                            {
                                if (bSure)
                                {
                                    mIGGAgreementSigning = IGGSDK.shareInstance().getAgreementSigning();
                                    mIGGAgreementSigning.termination().terminate((IGGException ex) =>
                                    {
                                        if (ex.isNone())
                                        {
                                            IGGSDKUtils.shareInstance().ShowToast("agreement terrminate success");
                                        }
                                        else
                                        {
                                            IGGSDKUtils.shareInstance().ShowToast("agreement terrminate error:" + ex.ToString());
                                        }
                                    }
                                    );
                                }
                            });

                    }
                    else
                    {
                        Debug.Log(exception.ToString());
                    }
                });
            });
            mWegamer = AddMenu("Wegamer", () =>
            {
                WegamersSDK.shareInstance().startBrowser();
            });
            AddMenu("游戏论坛", () =>
            {
                IGGURLBundle.shareInstance().forumURL((exception, url) =>
                {
                    if (exception.isNone())
                    {
                        IGGSDKUtils.shareInstance().OpenBrowser(url);
                    }
                });
            });
            AddMenu("普通客服", () =>
            {
                IGGURLBundle.shareInstance().livechatURL((exception, url) =>
                {
                    if (exception.isNone())
                    {
                        IGGSDKUtils.shareInstance().OpenBrowser(url);
                    }
                });
            });
            AddMenu("支付客服", () =>
            {
                IGGURLBundle.shareInstance().paymentLivechatURL((exception, url) =>
                {
                    if (exception.isNone())
                    {
                        IGGSDKUtils.shareInstance().OpenBrowser(url);
                    }
                });
            });
            AddMenu("提交问题", () =>
            {
                IGGURLBundle.shareInstance().serviceURL((exception, url) =>
                {
                    if (exception.isNone())
                    {
                        IGGSDKUtils.shareInstance().OpenBrowser(url);
                    }
                });
            });
            AddMenu("测试推送", () =>
            {
                var pushUrl = string.Format("http://push.igg.com/api/send_msg.php?g_id={0}&m_push_type=2&m_iggid_file={1}&m_time_to_send=0&m_display=0&m_by_timezone=0&m_msg=test5&m_data={{%22test%22:%201,%20%22id%22:%201236}}", IGGSDK.shareInstance().getGameId(), IGGSession.currentSession.getIGGId());
                IGGSDKUtils.shareInstance().OpenBrowser(pushUrl);
            });
            AddMenu("语言翻译", () =>
            {
                Debug.Log("onTranslate");

                IGGTranslator translator = new IGGTranslator(IGGLanguage.auto, IGGLanguage.Zh_CN);

                translator.translateText(new IGGTranslationSource("we are the king"), (IGGTranslationSet set) =>
                {
                    IGGTranslation trans = set.getByIndex(0);
                    IGGSDKUtils.shareInstance().ShowToast(trans.ToString());
                }, (IGGException var1, List<IGGTranslationSource> sources)=>
                {
                    Debug.LogError(var1.ToString());
                });

                //var sources = new List<IGGTranslationSource>();
                //sources.Add(new IGGTranslationSource("我是谁"));
                //sources.Add(new IGGTranslationSource("中国人"));
                //translator.translateTexts(sources, (IGGTranslationSet set) =>
                //{
                //    if (set.getType() == IGGTranslationType.NORMAL)
                //    {
                //        for (int i = 0; i < sources.Count; i++)
                //        {
                //            IGGTranslation trans = set.getByIndex(i);
                //            Debug.Log(trans.ToString());
                //        }
                //    }
                //}, null);

                //var nameSources = new List<IGGTranslationNamedSource>();
                //nameSources.Add(new IGGTranslationNamedSource("hellow", "我是谁"));
                //nameSources.Add(new IGGTranslationNamedSource("name", "中国人"));
                //translator.translateNamedTexts(nameSources, (IGGTranslationSet set) =>
                //{
                //    if (set.getType() == IGGTranslationType.NAME)
                //    {
                //        for (int i = 0; i < nameSources.Count; i++)
                //        {
                //            IGGTranslation trans = set.getByName(nameSources[i].getName());
                //            Debug.Log(trans.ToString());
                //        }
                //    }
                //}, null);
                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            });
            AddMenu("举报玩家", () =>
            {
                IGGInGameReporting.shareInstance().ReportComplain("119737175", "test", ".-_~我test report", (bool bSussessed, IGGInGameReporting.WebRequestReturn requestReturn) =>
                {
                    if (bSussessed)
                    {
                        if (requestReturn.error.code == 0)
                        {
                            IGGSDKUtils.shareInstance().ShowToast("举报成功");
                        }
                        else
                        {
                            IGGSDKUtils.shareInstance().ShowToast("举报失败:" + requestReturn.error.message);
                        }
                    }
                    else
                    {
                        IGGSDKUtils.shareInstance().ShowToast("举报失败:连接错误");
                    }
                });
            });
            AddMenu("评分(运营设置)", () =>
            {
                var appRating = IGGSDK.shareInstance().getAppRating();
                appRating.requestReview(
                (IGGAppRatingStatus status) =>
                {
                    IGGSDKUtils.shareInstance().ShowToast("评星关闭状态（通知运营打开）");
                },
                (IGGException exception) =>
                {
                    Debug.Log($"OnError:{exception.ToString()}");
                },
                (IGGMinimizedAppRating rating) =>
                {
                    Debug.Log($"OnMinimizedAppRating");
                    rating.goRating((IGGException exception) =>
                    {
                        Debug.Log(exception.ToString());
                    });
                },
                (IGGStarndardAppRating rating) =>
                {
                    IGGSDKUtils.shareInstance().ShowMsgBox("喜歡我們遊戲嗎？我們為所有玩家提供最好的體驗！", "标准评星", "喜歡", "不喜歡", (bool bSure) =>
                    {
                        if (bSure)
                        {
                            IGGSDKUtils.shareInstance().ShowMsgBox("因為你，我們不斷成長！請為我們評分！", "标准评星", "前往評分", (bool bSure2) =>
                            {
                                if (bSure2)
                                {
                                    rating.like((ex) =>
                                    {
                                    });
                                }
                            });
                        }
                        else
                        {
                            IGGSDKUtils.shareInstance().ShowMsgBox("非常抱歉，沒有提供最好的體驗給您，有什麼意見可以提供給我們嗎？", "标准评星", "网页建议", "内置建议", (bool bSure2) =>
                            {
                                if (bSure2)
                                {
                                    rating.getFeedbackWebPageURL((url) =>
                                    {
                                        IGGSDKUtils.shareInstance().OpenBrowser(url);
                                    });
                                }
                                else
                                {
                                    rating.feedback(new IGGAppRatingFeedback(5, "test"), (IGGException ex) =>
                                    {
                                        if (ex.isNone())
                                        {
                                            IGGSDKUtils.shareInstance().ShowToast("内置建议完成");
                                        }
                                    });
                                }
                            });
                        }
                    });
                    Debug.Log($"OnStarndardAppRating");
                });
            });

            AddMenu("检测weGamer红点", () =>
            {
                // 社区界面打开就需要调用
                WegamersSDK.shareInstance().CheckState(IGGSession.currentSession.getIGGId(), IGGSession.currentSession.getIGGId(), "1001000330", "c5fTfIp8ig2qO6Nu", WeGamersSDKParams.SkinType.SKIN_DARK, (bool bShowBtn) =>
                {
                    // 显示隐藏Wegamer入口
                    mWegamer.gameObject.SetActive(bShowBtn);
                },
                (bool bHasRed) =>
                {
                    var text = mWegamer.GetComponentInChildren<Text>();
                    // 显示红点
                    if (bHasRed)
                    {
                        text.text = "Wegamer(消息)";
                        text.color = Color.red;
                    }
                    else
                    {
                        text.text = "Wegamer";
                        text.color = Color.black;
                    }
                });
            });
            AddMenu("帐号红点时间清理", () =>
            {
                Debug.LogError("帐号红点时间清理");
                PlayerPrefs.SetInt("AccountBindCheckTime", 0);
            });
            AddMenu("创建运输部队", () =>
            {
                var troop = new Map_ObjectInfo.request();
                troop.mapObjectInfo = new MapObjectInfo();
                troop.mapObjectInfo.objectId = 10000;
                troop.mapObjectInfo.objectType = 27;

                PosInfo objectPos = new PosInfo();
                objectPos.x = 173797;
                objectPos.y = 118750;
                troop.mapObjectInfo.objectPos = objectPos; //起点

                List<PosInfo> posList = new List<PosInfo>();
                PosInfo objectPos1 = new PosInfo(); //起点
                objectPos1.x = 173797;
                objectPos1.y = 118750;
                posList.Add(objectPos1);
                PosInfo objectPos2 = new PosInfo(); //终点
                objectPos2.x = 173533;
                objectPos2.y = 115350;
                posList.Add(objectPos2);
                troop.mapObjectInfo.objectPath = posList;

                troop.mapObjectInfo.armyRid = 10043075;
                troop.mapObjectInfo.armyName = "城主19035112";
                troop.mapObjectInfo.status = (long)ArmyStatus.SPACE_MARCH;
                troop.mapObjectInfo.arrivalTime = ServerTimeModule.Instance.GetServerTime() + 10;
                troop.mapObjectInfo.startTime = ServerTimeModule.Instance.GetServerTime();
                troop.mapObjectInfo.guildAbbName = "ghra";
                troop.mapObjectInfo.guildId = 10000081;
                troop.mapObjectInfo.transportIndex = 1000000;

                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop);

                Timer.Register(10f, () =>
                {
                    Map_ObjectDelete.request deleteReq = new Map_ObjectDelete.request();
                    deleteReq.objectId = 10000;

                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq);
                });
            });
        }

        private void CreateGuardian()
        {
            CoreUtils.assetService.Instantiate("GuardianFormation_1", (go)=>
            {
                formationGuardian = go.GetComponent<Guardian>();
                m_heros = new List<TroopsHero>();
                {
                    var hero = new TroopsHero();
                    hero.heroId = 2011;
                    hero.label = "1";
                    m_heros.Add(hero);
                }
                m_units = new List<TroopsCell>();
                {
                    var unit = new TroopsCell();
                    unit.unitCount = 50000;
                    unit.unitMaxCount = 50000;
                    unit.unitType = 1;
                    unit.unitId = 10101;
                    m_units.Add(unit);
                }
                {
                    var unit = new TroopsCell();
                    unit.unitCount = 50000;
                    unit.unitMaxCount = 50000;
                    unit.unitType = 2;
                    unit.unitId = 10201;
                    m_units.Add(unit);
                }
                {
                    var unit = new TroopsCell();
                    unit.unitCount = 50000;
                    unit.unitMaxCount = 50000;
                    unit.unitType = 3;
                    unit.unitId = 10301;
                    m_units.Add(unit);
                }
                {
                    var unit = new TroopsCell();
                    unit.unitCount = 50000;
                    unit.unitMaxCount = 50000;
                    unit.unitType = 4;
                    unit.unitId = 10401;
                    m_units.Add(unit);
                }

                var cityBuildingProxy =
                    AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                Vector2 pos = new Vector2(cityBuildingProxy.RolePos.x + 8, cityBuildingProxy.RolePos.y);
                var dir = new Vector2(1, 1).normalized;
                SquareHelper.Instance.InitUnitPrefabDict();
                string des = SquareHelper.Instance.GetSquareString(m_units, m_heros, Troops.ENMU_MATRIX_TYPE.SHAMAN_GUARDIAN);
                Guardian.InitFormationS(formationGuardian, des,Color.gray);
                Guardian.InitPositionS(formationGuardian,pos,pos+dir);
                Guardian.SetCampS(formationGuardian,Troops.ENUM_FORMATION_CAMP.Enemy);
            });
        }

#endregion

        private void CreateAttackFormation()
        {
            if (m_Formation != null && m_FormationTwo != null)
            {
                var pos = m_Formation.transform.position;
                var pos2 = m_FormationTwo.transform.position;
                var cur_pos = new Vector2(pos.x, pos.z);
                var target_pos = new Vector2(pos2.x, pos2.z);
                Vector2 v2 = (target_pos - cur_pos) / 3.6f;
                target_pos = target_pos - v2;
                // 移动过去
                Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.MOVE, cur_pos, target_pos);
                m_Timer = Timer.Register(1f, () =>
                {
                    cur_pos = new Vector2(m_Formation.transform.position.x, m_Formation.transform.position.z);
                    //Debug.LogError($"{cur_pos.ToString()}, {target_pos.ToString()}");
                    float distance = Vector2.Distance(cur_pos, target_pos);
                    if (distance < 0.2f)
                    {
                        Vector2 targetV2 = new Vector2(pos2.x, pos2.z);
                        //攻击态
                        Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.FIGHT, cur_pos, targetV2);
                        // 攻击阵型
                        Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.FIGHT, cur_pos, targetV2);

                        Vector2 twoPos = new Vector2(m_FormationTwo.transform.position.x,
                            m_FormationTwo.transform.position.z);
                        //攻击态
                        Troops.SetStateS(m_FormationTwo, Troops.ENMU_SQUARE_STAT.FIGHT, twoPos, cur_pos);
                        // 攻击阵型
                        Troops.SetStateS(m_FormationTwo, Troops.ENMU_SQUARE_STAT.FIGHT, twoPos, cur_pos);

                        if (m_Timer != null)
                        {
                            m_Timer.Cancel();
                        }
                    }
                }, null, true);


                CoreUtils.uiManager.CloseUI(UI.s_gameTool);
            }
        }


        private void CreateFormation(Troops.ENMU_MATRIX_TYPE type, Action action = null)
        {
            if (m_Formation != null)
            {
                //if (m_Formation.GetFormationType() == type)
                //{
                //    action?.Invoke();
                //    return;
                //}

                CoreUtils.assetService.Destroy(m_Formation.gameObject);
                m_Formation = null;
            }

            var cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            Vector2 pos = new Vector2(cityBuildingProxy.RolePos.x + 8, cityBuildingProxy.RolePos.y);

            m_heros = new List<TroopsHero>();
            {
                var hero = new TroopsHero();
                hero.heroId = 1001;
                hero.label = "3";
                m_heros.Add(hero);
            }
            m_units = new List<TroopsCell>();
            {
                var unit = new TroopsCell();
                unit.unitCount = 50000;
                unit.unitMaxCount = 50000;
                unit.unitType = 1;
                unit.unitId = 10101;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 50000;
                unit.unitMaxCount = 50000;
                unit.unitType = 2;
                unit.unitId = 10201;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 50000;
                unit.unitMaxCount = 50000;
                unit.unitType = 3;
                unit.unitId = 10301;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 50000;
                unit.unitMaxCount = 50000;
                unit.unitType = 4;
                unit.unitId = 10401;
                m_units.Add(unit);
            }


            var squareHelper = SquareHelper.Instance;
            squareHelper.InitUnitPrefabDict();
            string des = squareHelper.GetSquareString(m_units, m_heros, type);
            string strFormationPrefab = "Formation";
            string objectName;
            if (type == Troops.ENMU_MATRIX_TYPE.BARBARIAN)
            {
                strFormationPrefab = "BarbarianFormation_test";
                objectName = "BarbarianFormation";
            }
            else
            {
                strFormationPrefab = "Formation";
                objectName = strFormationPrefab;
            }

            CoreUtils.assetService.InstantiateOnePerFrame(strFormationPrefab, (go) =>
            {
                var land_root = GameObject.Find("SceneObject/land_root").transform;
                go.transform.SetParent(land_root);
                go.gameObject.name = string.Format("{0}_{1}", objectName, 1);
                go.transform.localPosition = new Vector3(pos.x, 0, pos.y);
                m_Formation = go.GetComponent<Troops>();
                Troops.InitFormationS(m_Formation, des, Color.gray);
                var dir = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1)).normalized;
                Troops.SetStateS(m_Formation, Troops.ENMU_SQUARE_STAT.IDLE, pos, pos + dir, 0.0f);
                Troops.SetCampS(m_Formation, Troops.ENUM_FORMATION_CAMP.Mine);
                action?.Invoke();
                CoreUtils.assetService.Instantiate("troop_line_mine_start", (go2) =>
                {
                    go2.transform.SetParent(m_Formation.transform);
                    go2.transform.localPosition = Vector3.zero;
                });
            });
        }


        /// <summary>
        /// 变化集结部队士兵信息
        /// </summary>
        private void SetFormationPlayInfo()
        {
            if (m_FormationTwo == null)
            {
                return;
            }
            
            m_heros = new List<TroopsHero>();
            {
                var hero = new TroopsHero();
                hero.heroId = 1001;
                hero.label = "3";
                m_heros.Add(hero);
            }
            m_units = new List<TroopsCell>();
            {
                var unit = new TroopsCell();
                unit.unitCount = 10000;
                unit.unitMaxCount = 50000;
                unit.unitType = 1;
                unit.unitId = 10101;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 10000;
                unit.unitMaxCount = 50000;
                unit.unitType = 2;
                unit.unitId = 10201;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 10000;
                unit.unitMaxCount = 50000;
                unit.unitType = 3;
                unit.unitId = 10301;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 10000;
                unit.unitMaxCount = 50000;
                unit.unitType = 4;
                unit.unitId = 10401;
                m_units.Add(unit);
            }
            
            var squareHelper = SquareHelper.Instance;
            squareHelper.InitUnitPrefabDict();
            string des = squareHelper.GetSquareString(m_units, m_heros, Troops.ENMU_MATRIX_TYPE.RALLY);
            Troops.SetFormationInfoS(m_FormationTwo,des);
        }



        private void CreateFormationTwo(Troops.ENMU_MATRIX_TYPE type, Action action = null)
        {
            if (m_FormationTwo != null)
            {
                if (m_FormationTwo.GetFormationType() == type)
                {
                    action?.Invoke();
                    return;
                }

                CoreUtils.assetService.Destroy(m_FormationTwo.gameObject);
                m_FormationTwo = null;
            }

            var cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            Vector2 pos = new Vector2(cityBuildingProxy.RolePos.x + 18, cityBuildingProxy.RolePos.y + 18);

            m_heros = new List<TroopsHero>();
            {
                var hero = new TroopsHero();
                hero.heroId = 1001;
                hero.label = "3";
                m_heros.Add(hero);
            }
            m_units = new List<TroopsCell>();
            {
                var unit = new TroopsCell();
                unit.unitCount = 50000;
                unit.unitMaxCount = 50000;
                unit.unitType = 1;
                unit.unitId = 10101;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 50000;
                unit.unitMaxCount = 50000;
                unit.unitType = 2;
                unit.unitId = 10201;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 50000;
                unit.unitMaxCount = 50000;
                unit.unitType = 3;
                unit.unitId = 10301;
                m_units.Add(unit);
            }
            {
                var unit = new TroopsCell();
                unit.unitCount = 50000;
                unit.unitMaxCount = 50000;
                unit.unitType = 4;
                unit.unitId = 10401;
                m_units.Add(unit);
            }


            var squareHelper = SquareHelper.Instance;
            squareHelper.InitUnitPrefabDict();
            string des = squareHelper.GetSquareString(m_units, m_heros, type);
            string strFormationPrefab = "Formation";
            string objectName;
            if (type == Troops.ENMU_MATRIX_TYPE.BARBARIAN)
            {
                strFormationPrefab = "BarbarianFormation_test";
                objectName = "BarbarianFormation";
            }
            else
            {
                strFormationPrefab = "Formation";
                objectName = strFormationPrefab;
            }

            CoreUtils.assetService.InstantiateOnePerFrame(strFormationPrefab, (go) =>
            {
                var land_root = GameObject.Find("SceneObject/land_root").transform;
                go.transform.SetParent(land_root);
                go.gameObject.name = string.Format("{0}_{1}", objectName, 1);
                go.transform.localPosition = new Vector3(pos.x, 0, pos.y);
                m_FormationTwo = go.GetComponent<Troops>();
                Troops.InitFormationS(m_FormationTwo, des, Color.gray);
                var dir = new Vector2(1, 1).normalized;
                Vector2 targetV2 = Vector2.zero;
                if (m_Formation == null)
                {
                    targetV2 = pos + dir;
                }
                else
                {
                    targetV2 = new Vector2(m_Formation.gameObject.transform.position.x,
                        m_Formation.gameObject.transform.position.z);
                }

                Troops.InitPositionS(m_FormationTwo, pos, targetV2);
                Troops.SetCampS(m_FormationTwo, Troops.ENUM_FORMATION_CAMP.Enemy);
                action?.Invoke();
                CoreUtils.assetService.Instantiate("troop_line_mine_start", (go2) =>
                {
                    go2.transform.SetParent(m_FormationTwo.transform);
                    go2.transform.localPosition = Vector3.zero;
                });
            });
        }


        private GameObject AddMenu(string name, UnityEngine.Events.UnityAction action)
        {
            var menu = GameObject.Instantiate(view.m_ls_template_BtnAnimation.gameObject,
                view.m_ls_template_BtnAnimation.transform.parent);
            menu.SetActive(true);
            menu.GetComponent<GameButton>().onClick.AddListener(action);
            menu.GetComponentInChildren<Text>().text = name;
            return menu;
        }

        private void TestTable()
        {
            Profiler.BeginSample("LoadTable");
            float startTime1 = Time.realtimeSinceStartup;
            CoreUtils.dataService.QueryTable<Data.ActivityCalendarDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityConversionTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityDaysTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityDropTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityEndHandingDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityInfernalDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityKillIntegralDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityKillTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityRankingTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityRewardPreviewDefine>();
            CoreUtils.dataService.QueryTable<Data.ActivityTargetTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceAttrInfoDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceBuildingDataDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceBuildingTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceDonateRankingDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceGiftLevelDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceGiftRewardDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceGiftTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceLanguageSetDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceMemberDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceMemberJurisdictionDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceOfficiallyDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceShopItemDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceSignDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceStudyDefine>();
            CoreUtils.dataService.QueryTable<Data.AllianceTreasureDefine>();
            CoreUtils.dataService.QueryTable<Data.AnnouncementDefine>();
            CoreUtils.dataService.QueryTable<Data.ArmsDefine>();
            CoreUtils.dataService.QueryTable<Data.ArmsSkillDefine>();
            CoreUtils.dataService.QueryTable<Data.AttrInfoDefine>();
            CoreUtils.dataService.QueryTable<Data.AudioConfig3dDefine>();
            CoreUtils.dataService.QueryTable<Data.AudioGroupInfoDefine>();
            CoreUtils.dataService.QueryTable<Data.AudioInfoDefine>();
            CoreUtils.dataService.QueryTable<Data.BacksourcingRestrictDefine>();
            CoreUtils.dataService.QueryTable<Data.BattleBuffDefine>();
            CoreUtils.dataService.QueryTable<Data.BattleLossDefine>();
            CoreUtils.dataService.QueryTable<Data.BlockDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingAllianceCenterDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingArcheryrangeDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingBarracksDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingCampusDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingCastleDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingCityWallDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingCountLimitDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingFreightDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingGuardTowerDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingHospitalDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingLevelDataDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingMailDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingMenuDataDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingModelDataDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingResourcesProduceDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingScoutcampDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingSiegeWorkshopDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingStableDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingStorageDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingTavernDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingTownCenterDefine>();
            CoreUtils.dataService.QueryTable<Data.BuildingTypeConfigDefine>();
            CoreUtils.dataService.QueryTable<Data.CameraParamLodDefine>();
            CoreUtils.dataService.QueryTable<Data.ChatChannelDefine>();
            CoreUtils.dataService.QueryTable<Data.ChatEmojiDefine>();
            CoreUtils.dataService.QueryTable<Data.CityAgeSizeDefine>();
            CoreUtils.dataService.QueryTable<Data.CityBuffDefine>();
            CoreUtils.dataService.QueryTable<Data.CityBuffGroupDefine>();
            CoreUtils.dataService.QueryTable<Data.CityBuffSeriesDefine>();
            CoreUtils.dataService.QueryTable<Data.CivilizationDefine>();
            CoreUtils.dataService.QueryTable<Data.ConfigDefine>();
            CoreUtils.dataService.QueryTable<Data.CurrencyDefine>();
            CoreUtils.dataService.QueryTable<Data.EffectInfoDefine>();
            CoreUtils.dataService.QueryTable<Data.EquipAttDefine>();
            CoreUtils.dataService.QueryTable<Data.EquipComposeDefine>();
            CoreUtils.dataService.QueryTable<Data.EquipDefine>();
            CoreUtils.dataService.QueryTable<Data.EquipMaterialDefine>();
            CoreUtils.dataService.QueryTable<Data.EquipMaterialGroupDefine>();
            CoreUtils.dataService.QueryTable<Data.EvolutionMileStoneDefine>();
            CoreUtils.dataService.QueryTable<Data.EvolutionRankRewardDefine>();
            CoreUtils.dataService.QueryTable<Data.ExpeditionHeadDefine>();
            CoreUtils.dataService.QueryTable<Data.ExpeditionShopDefine>();
            CoreUtils.dataService.QueryTable<Data.GameSpriteDefine>();
            CoreUtils.dataService.QueryTable<Data.GuardTowerAttrDefine>();
            CoreUtils.dataService.QueryTable<Data.GuideDefine>();
            CoreUtils.dataService.QueryTable<Data.GuideDialogDefine>();
            CoreUtils.dataService.QueryTable<Data.GuideExDefine>();
            CoreUtils.dataService.QueryTable<Data.HelpTipsDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroLevelDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroSkillDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroSkillEffectDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroSkillLevelDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroStarDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroStarExpDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroTalentDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroTalentGainTreeDefine>();
            CoreUtils.dataService.QueryTable<Data.HeroTalentTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.HyperlinkDefine>();
            CoreUtils.dataService.QueryTable<Data.InitBuildingDefine>();
            CoreUtils.dataService.QueryTable<Data.instantPriceDefine>();
            CoreUtils.dataService.QueryTable<Data.ItemDefine>();
            CoreUtils.dataService.QueryTable<Data.ItemGetDefine>();
            CoreUtils.dataService.QueryTable<Data.ItemHeroDefine>();
            CoreUtils.dataService.QueryTable<Data.ItemPackageDefine>();
            CoreUtils.dataService.QueryTable<Data.ItemPackageShowDefine>();
            CoreUtils.dataService.QueryTable<Data.ItemPlayerHeadDefine>();
            CoreUtils.dataService.QueryTable<Data.ItemRewardChoiceDefine>();
            CoreUtils.dataService.QueryTable<Data.JumpTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.LanguageDefine>();
            CoreUtils.dataService.QueryTable<Data.LanguageImgDefine>();
            CoreUtils.dataService.QueryTable<Data.LanguageServerDefine>();
            CoreUtils.dataService.QueryTable<Data.LanguageSetDefine>();
            CoreUtils.dataService.QueryTable<Data.LeaderboardDefine>();
            CoreUtils.dataService.QueryTable<Data.LoadTipsDefine>();
            CoreUtils.dataService.QueryTable<Data.MailDefine>();
            CoreUtils.dataService.QueryTable<Data.MailLevelLimitDefine>();
            CoreUtils.dataService.QueryTable<Data.MapBuildingDataDefine>();
            CoreUtils.dataService.QueryTable<Data.MapFixPointDefine>();
            CoreUtils.dataService.QueryTable<Data.MapItemTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.MapZoneSFDefine>();
            CoreUtils.dataService.QueryTable<Data.MonsterDefine>();
            CoreUtils.dataService.QueryTable<Data.MonsterDamageRewardDefine>();
            CoreUtils.dataService.QueryTable<Data.MonsterLootRuleDefine>();
            CoreUtils.dataService.QueryTable<Data.MonsterRefreshLevelDefine>();
            CoreUtils.dataService.QueryTable<Data.MonsterTroopsAttrDefine>();
            CoreUtils.dataService.QueryTable<Data.MonsterTroopsDefine>();
            CoreUtils.dataService.QueryTable<Data.MonsterZoneLevelDefine>();
            CoreUtils.dataService.QueryTable<Data.MysteryStoreDefine>();
            CoreUtils.dataService.QueryTable<Data.MysteryStoreProDefine>();
            CoreUtils.dataService.QueryTable<Data.OpenLodUiDefine>();
            CoreUtils.dataService.QueryTable<Data.OpenUiDefine>();
            CoreUtils.dataService.QueryTable<Data.PlayerBehaviorDataDefine>();
            CoreUtils.dataService.QueryTable<Data.PlayerHeadDefine>();
            CoreUtils.dataService.QueryTable<Data.PriceDefine>();
            CoreUtils.dataService.QueryTable<Data.PushMessageDataDefine>();
            CoreUtils.dataService.QueryTable<Data.PushMessageGroupDefine>();
            CoreUtils.dataService.QueryTable<Data.QualitySetDefine>();
            CoreUtils.dataService.QueryTable<Data.RallyTimesDefine>();
            CoreUtils.dataService.QueryTable<Data.RechargeDailySpecialDefine>();
            CoreUtils.dataService.QueryTable<Data.RechargeFirstDefine>();
            CoreUtils.dataService.QueryTable<Data.RechargeFundDefine>();
            CoreUtils.dataService.QueryTable<Data.RechargeGemMallDefine>();
            CoreUtils.dataService.QueryTable<Data.RechargeLimitTimeBagDefine>();
            CoreUtils.dataService.QueryTable<Data.RechargeListDefine>();
            CoreUtils.dataService.QueryTable<Data.RechargeSaleDefine>();
            CoreUtils.dataService.QueryTable<Data.RechargeSupplyDefine>();
            CoreUtils.dataService.QueryTable<Data.ResourceGatherRuleDefine>();
            CoreUtils.dataService.QueryTable<Data.ResourceGatherTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.ResourcesPlunderLossDefine>();
            CoreUtils.dataService.QueryTable<Data.ResourceZoneLevelDefine>();
            CoreUtils.dataService.QueryTable<Data.SkillBattleDefine>();
            CoreUtils.dataService.QueryTable<Data.SkillStatusDefine>();
            CoreUtils.dataService.QueryTable<Data.SnsEntranceDefine>();
            CoreUtils.dataService.QueryTable<Data.SquareEmojiOffsetDefine>();
            CoreUtils.dataService.QueryTable<Data.SquareMaxNumberDefine>();
            CoreUtils.dataService.QueryTable<Data.SquareNumberBySumDefine>();
            CoreUtils.dataService.QueryTable<Data.SquareOffsetDefine>();
            CoreUtils.dataService.QueryTable<Data.SquareRowWidthDefine>();
            CoreUtils.dataService.QueryTable<Data.SquareSpacingDefine>();
            CoreUtils.dataService.QueryTable<Data.StrongHoldDataDefine>();
            CoreUtils.dataService.QueryTable<Data.StrongHoldTypeDefine>();
            CoreUtils.dataService.QueryTable<Data.StudyDefine>();
            CoreUtils.dataService.QueryTable<Data.StudyDataDefine>();
            CoreUtils.dataService.QueryTable<Data.SystemOpenDefine>();
            CoreUtils.dataService.QueryTable<Data.TaskActivityRewardDefine>();
            CoreUtils.dataService.QueryTable<Data.TaskChapterDefine>();
            CoreUtils.dataService.QueryTable<Data.TaskChapterDataDefine>();
            CoreUtils.dataService.QueryTable<Data.TaskDailyDefine>();
            CoreUtils.dataService.QueryTable<Data.TaskDialogDefine>();
            CoreUtils.dataService.QueryTable<Data.TaskMainDefine>();
            CoreUtils.dataService.QueryTable<Data.TaskSideDefine>();
            CoreUtils.dataService.QueryTable<Data.TavernProbabilityDefine>();
            CoreUtils.dataService.QueryTable<Data.UnitMaxBeAttackedDefine>();
            CoreUtils.dataService.QueryTable<Data.UnitViewDefine>();
            CoreUtils.dataService.QueryTable<Data.VillageRewardDataDefine>();
            CoreUtils.dataService.QueryTable<Data.VipAttDefine>();
            CoreUtils.dataService.QueryTable<Data.VipDefine>();
            CoreUtils.dataService.QueryTable<Data.VipDayPointDefine>();
            CoreUtils.dataService.QueryTable<Data.VipStoreDefine>();
            CoreUtils.logService.Info($"LoadAllTable:{Time.realtimeSinceStartup - startTime1}", Color.green);
            Profiler.EndSample();
            float startTime2 = Time.realtimeSinceStartup;
            Profiler.BeginSample("ConvertTable");
            CoreUtils.dataService.QueryRecords<Data.ActivityCalendarDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityConversionTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityDaysTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityDropTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityEndHandingDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityInfernalDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityKillIntegralDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityKillTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityRankingTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityRewardPreviewDefine>();
            CoreUtils.dataService.QueryRecords<Data.ActivityTargetTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceAttrInfoDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceBuildingDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceBuildingTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceDonateRankingDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceGiftLevelDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceGiftRewardDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceGiftTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceLanguageSetDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceMemberDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceMemberJurisdictionDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceOfficiallyDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceShopItemDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceSignDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceStudyDefine>();
            CoreUtils.dataService.QueryRecords<Data.AllianceTreasureDefine>();
            CoreUtils.dataService.QueryRecords<Data.AnnouncementDefine>();
            CoreUtils.dataService.QueryRecords<Data.ArmsDefine>();
            CoreUtils.dataService.QueryRecords<Data.ArmsSkillDefine>();
            CoreUtils.dataService.QueryRecords<Data.AttrInfoDefine>();
            CoreUtils.dataService.QueryRecords<Data.AudioConfig3dDefine>();
            CoreUtils.dataService.QueryRecords<Data.AudioGroupInfoDefine>();
            CoreUtils.dataService.QueryRecords<Data.AudioInfoDefine>();
            CoreUtils.dataService.QueryRecords<Data.BacksourcingRestrictDefine>();
            CoreUtils.dataService.QueryRecords<Data.BattleBuffDefine>();
            CoreUtils.dataService.QueryRecords<Data.BattleLossDefine>();
            CoreUtils.dataService.QueryRecords<Data.BlockDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingAllianceCenterDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingArcheryrangeDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingBarracksDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingCampusDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingCastleDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingCityWallDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingCountLimitDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingFreightDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingGuardTowerDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingHospitalDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingLevelDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingMailDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingMenuDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingModelDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingResourcesProduceDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingScoutcampDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingSiegeWorkshopDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingStableDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingStorageDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingTavernDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingTownCenterDefine>();
            CoreUtils.dataService.QueryRecords<Data.BuildingTypeConfigDefine>();
            CoreUtils.dataService.QueryRecords<Data.CameraParamLodDefine>();
            CoreUtils.dataService.QueryRecords<Data.ChatChannelDefine>();
            CoreUtils.dataService.QueryRecords<Data.ChatEmojiDefine>();
            CoreUtils.dataService.QueryRecords<Data.CityAgeSizeDefine>();
            CoreUtils.dataService.QueryRecords<Data.CityBuffDefine>();
            CoreUtils.dataService.QueryRecords<Data.CityBuffGroupDefine>();
            CoreUtils.dataService.QueryRecords<Data.CityBuffSeriesDefine>();
            CoreUtils.dataService.QueryRecords<Data.CivilizationDefine>();
            CoreUtils.dataService.QueryRecords<Data.ConfigDefine>();
            CoreUtils.dataService.QueryRecords<Data.CurrencyDefine>();
            CoreUtils.dataService.QueryRecords<Data.EffectInfoDefine>();
            CoreUtils.dataService.QueryRecords<Data.EquipAttDefine>();
            CoreUtils.dataService.QueryRecords<Data.EquipComposeDefine>();
            CoreUtils.dataService.QueryRecords<Data.EquipDefine>();
            CoreUtils.dataService.QueryRecords<Data.EquipMaterialDefine>();
            CoreUtils.dataService.QueryRecords<Data.EquipMaterialGroupDefine>();
            CoreUtils.dataService.QueryRecords<Data.EvolutionMileStoneDefine>();
            CoreUtils.dataService.QueryRecords<Data.EvolutionRankRewardDefine>();
            CoreUtils.dataService.QueryRecords<Data.ExpeditionHeadDefine>();
            CoreUtils.dataService.QueryRecords<Data.ExpeditionShopDefine>();
            CoreUtils.dataService.QueryRecords<Data.GameSpriteDefine>();
            CoreUtils.dataService.QueryRecords<Data.GuardTowerAttrDefine>();
            CoreUtils.dataService.QueryRecords<Data.GuideDefine>();
            CoreUtils.dataService.QueryRecords<Data.GuideDialogDefine>();
            CoreUtils.dataService.QueryRecords<Data.GuideExDefine>();
            CoreUtils.dataService.QueryRecords<Data.HelpTipsDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroLevelDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroSkillDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroSkillEffectDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroSkillLevelDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroStarDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroStarExpDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroTalentDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroTalentGainTreeDefine>();
            CoreUtils.dataService.QueryRecords<Data.HeroTalentTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.HyperlinkDefine>();
            CoreUtils.dataService.QueryRecords<Data.InitBuildingDefine>();
            CoreUtils.dataService.QueryRecords<Data.instantPriceDefine>();
            CoreUtils.dataService.QueryRecords<Data.ItemDefine>();
            CoreUtils.dataService.QueryRecords<Data.ItemGetDefine>();
            CoreUtils.dataService.QueryRecords<Data.ItemHeroDefine>();
            CoreUtils.dataService.QueryRecords<Data.ItemPackageDefine>();
            CoreUtils.dataService.QueryRecords<Data.ItemPackageShowDefine>();
            CoreUtils.dataService.QueryRecords<Data.ItemPlayerHeadDefine>();
            CoreUtils.dataService.QueryRecords<Data.ItemRewardChoiceDefine>();
            CoreUtils.dataService.QueryRecords<Data.JumpTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.LanguageDefine>();
            CoreUtils.dataService.QueryRecords<Data.LanguageImgDefine>();
            CoreUtils.dataService.QueryRecords<Data.LanguageServerDefine>();
            CoreUtils.dataService.QueryRecords<Data.LanguageSetDefine>();
            CoreUtils.dataService.QueryRecords<Data.LeaderboardDefine>();
            CoreUtils.dataService.QueryRecords<Data.LoadTipsDefine>();
            CoreUtils.dataService.QueryRecords<Data.MailDefine>();
            CoreUtils.dataService.QueryRecords<Data.MailLevelLimitDefine>();
            CoreUtils.dataService.QueryRecords<Data.MapBuildingDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.MapFixPointDefine>();
            CoreUtils.dataService.QueryRecords<Data.MapItemTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.MapZoneSFDefine>();
            CoreUtils.dataService.QueryRecords<Data.MonsterDefine>();
            CoreUtils.dataService.QueryRecords<Data.MonsterDamageRewardDefine>();
            CoreUtils.dataService.QueryRecords<Data.MonsterLootRuleDefine>();
            CoreUtils.dataService.QueryRecords<Data.MonsterRefreshLevelDefine>();
            CoreUtils.dataService.QueryRecords<Data.MonsterTroopsAttrDefine>();
            CoreUtils.dataService.QueryRecords<Data.MonsterTroopsDefine>();
            CoreUtils.dataService.QueryRecords<Data.MonsterZoneLevelDefine>();
            CoreUtils.dataService.QueryRecords<Data.MysteryStoreDefine>();
            CoreUtils.dataService.QueryRecords<Data.MysteryStoreProDefine>();
            CoreUtils.dataService.QueryRecords<Data.OpenLodUiDefine>();
            CoreUtils.dataService.QueryRecords<Data.OpenUiDefine>();
            CoreUtils.dataService.QueryRecords<Data.PlayerBehaviorDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.PlayerHeadDefine>();
            CoreUtils.dataService.QueryRecords<Data.PriceDefine>();
            CoreUtils.dataService.QueryRecords<Data.PushMessageDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.PushMessageGroupDefine>();
            CoreUtils.dataService.QueryRecords<Data.QualitySetDefine>();
            CoreUtils.dataService.QueryRecords<Data.RallyTimesDefine>();
            CoreUtils.dataService.QueryRecords<Data.RechargeDailySpecialDefine>();
            CoreUtils.dataService.QueryRecords<Data.RechargeFirstDefine>();
            CoreUtils.dataService.QueryRecords<Data.RechargeFundDefine>();
            CoreUtils.dataService.QueryRecords<Data.RechargeGemMallDefine>();
            CoreUtils.dataService.QueryRecords<Data.RechargeLimitTimeBagDefine>();
            CoreUtils.dataService.QueryRecords<Data.RechargeListDefine>();
            CoreUtils.dataService.QueryRecords<Data.RechargeSaleDefine>();
            CoreUtils.dataService.QueryRecords<Data.RechargeSupplyDefine>();
            CoreUtils.dataService.QueryRecords<Data.ResourceGatherRuleDefine>();
            CoreUtils.dataService.QueryRecords<Data.ResourceGatherTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.ResourcesPlunderLossDefine>();
            CoreUtils.dataService.QueryRecords<Data.ResourceZoneLevelDefine>();
            CoreUtils.dataService.QueryRecords<Data.SkillBattleDefine>();
            CoreUtils.dataService.QueryRecords<Data.SkillStatusDefine>();
            CoreUtils.dataService.QueryRecords<Data.SnsEntranceDefine>();
            CoreUtils.dataService.QueryRecords<Data.SquareEmojiOffsetDefine>();
            CoreUtils.dataService.QueryRecords<Data.SquareMaxNumberDefine>();
            CoreUtils.dataService.QueryRecords<Data.SquareNumberBySumDefine>();
            CoreUtils.dataService.QueryRecords<Data.SquareOffsetDefine>();
            CoreUtils.dataService.QueryRecords<Data.SquareRowWidthDefine>();
            CoreUtils.dataService.QueryRecords<Data.SquareSpacingDefine>();
            CoreUtils.dataService.QueryRecords<Data.StrongHoldDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.StrongHoldTypeDefine>();
            CoreUtils.dataService.QueryRecords<Data.StudyDefine>();
            CoreUtils.dataService.QueryRecords<Data.StudyDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.SystemOpenDefine>();
            CoreUtils.dataService.QueryRecords<Data.TaskActivityRewardDefine>();
            CoreUtils.dataService.QueryRecords<Data.TaskChapterDefine>();
            CoreUtils.dataService.QueryRecords<Data.TaskChapterDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.TaskDailyDefine>();
            CoreUtils.dataService.QueryRecords<Data.TaskDialogDefine>();
            CoreUtils.dataService.QueryRecords<Data.TaskMainDefine>();
            CoreUtils.dataService.QueryRecords<Data.TaskSideDefine>();
            CoreUtils.dataService.QueryRecords<Data.TavernProbabilityDefine>();
            CoreUtils.dataService.QueryRecords<Data.UnitMaxBeAttackedDefine>();
            CoreUtils.dataService.QueryRecords<Data.UnitViewDefine>();
            CoreUtils.dataService.QueryRecords<Data.VillageRewardDataDefine>();
            CoreUtils.dataService.QueryRecords<Data.VipAttDefine>();
            CoreUtils.dataService.QueryRecords<Data.VipDefine>();
            CoreUtils.dataService.QueryRecords<Data.VipDayPointDefine>();
            CoreUtils.dataService.QueryRecords<Data.VipStoreDefine>();
            CoreUtils.logService.Info($"ConvertAllTable:{Time.realtimeSinceStartup - startTime2}", Color.green);
            Profiler.EndSample();
        }
    }
}
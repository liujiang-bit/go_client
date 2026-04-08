using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using Client;
using UnityEngine;
using Hotfix;
using System;
using System.IO;
using Data;

namespace Game
{
    public class GlobalFilmMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "GMGlobalMediator";

        public GuideFilm1 film1;
        public GuideFilm2 film2;
        public GuideFilm3 film3;

        #endregion
        public GlobalFilmMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public GlobalFilmMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.GuideFindMonster,
                CmdConstant.GuideFirstMarch,
                CmdConstant.GuideSecondMarch,
                CmdConstant.GuideSecondSearchMonster,
                CmdConstant.GuideArmyReturn,
                CmdConstant.EnterCity,
                CmdConstant.NetWorkReconnecting,
            }.ToArray();
        }

        public override void HandleNotification(INotification notificationName)
        {
            switch (notificationName.Name)
            {
                case CmdConstant.GuideFindMonster:
                    {
                        film3 = new GuideFilm3();
                        film3.CreateFirstBarbarian();
                        film3.MoveCameraToFirstBarbarian((pos)=>
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.GuideFoundMonster, GuideFilm3.FirstBarbarianID);
                        });
                    }
                    break;

                case CmdConstant.GuideFirstMarch:
                    {
                        film3.PlayFirstFight(()=>
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.MonsterFightEnd);
                        });
                        film3.PlaySecondChase();
                    }
                    break;
                case CmdConstant.GuideSecondSearchMonster:
                    {
                        film3.MoveCameraToSecondBarbarian((pos)=>
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.GuideFoundMonster, GuideFilm3.SecondBarbarianID);
                        });
                    }
                    break;
                case CmdConstant.GuideSecondMarch:
                    {
                        film3.PlayJoinFinalFight(()=>
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.MonsterFightEnd);
                        });
                    }
                    break;
                case CmdConstant.GuideArmyReturn:
                    {
                        film3.ReturnToCity();
                    }
                    break;
                case CmdConstant.EnterCity:
                    {
                        if(film2!=null&&film2.m_quitSignal)
                        {
                            film2.OnQuit();
                            film2 = null;
                        }
                    }
                    break;
                case CmdConstant.NetWorkReconnecting:
                    {
                        if (film3 != null)
                        {
                            Debug.Log("网络重连，移除野蛮人");
                            //film3.ClearData();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void InitData()
        {
            IsOpenUpdate = true;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {

        }

        public override void Update()
        {
            if(film2!=null)
            {
                film2.OnUpdate();
            }
        }

        public override void LateUpdate()
        {

        }

        public override void FixedUpdate()
        {

        }

        public void InitFilm1(Action quitCallback)
        {
            GuideFilm1Data data = new GuideFilm1Data();
            //try
            //{
            //    string fileTxt = GetPath() + "Film1.txt";
            //    Debug.LogFormat("film:{0}", fileTxt);
            //    if (File.Exists(fileTxt))
            //        data = LitJson.JsonMapper.ToObject<GuideFilm1Data>(File.ReadAllText(fileTxt));
            //}
            //catch
            //{
            //    Debug.LogError("film1的配表读取失败");
            //}
            film1 = new GuideFilm1(data);
            film1.GuideFilm();
            film1.quitCallback = quitCallback;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            if (film1 != null)
            {
                film1.OnQuit();
            }
            if(film3!=null)
            {
                film3.ClearData();
            }
            if (film2 != null)
            {
                film2.OnQuit();
            }
        }


        public void InitFilm2()
        {
            GuideFilm2Data data = new GuideFilm2Data();
            film2 = new GuideFilm2(data);
            film2.GuideFilm();
        }

        public void QuitFilm2(Action citizenLeftCallback)
        {
            if (film2 != null)
            {
                film2.m_leftSignal = true;
                film2.m_quitSignal = true;
                if (!film2.isCizizenLeft)
                {
                    film2.citizenLeftCallback = () =>
                    {
                        citizenLeftCallback?.Invoke();
                    };
                    return;
                }
            }
        }

        public void SetCitizenFilmCallback(Action citizenLeftCallback)
        {
            if (film2 != null)
            {
                if (film2.isCizizenLeft)
                {
                    citizenLeftCallback?.Invoke();
                    return;
                }
                film2.citizenLeftCallback = citizenLeftCallback;
            }
        }

        //初始化野蛮人的战斗
        public void InitFilm3()
        {
            film3 = new GuideFilm3();
        }

        private string GetPath()
        {
            string folder = Application.streamingAssetsPath;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        #region 村民返回城市 怪物攻击村庄

        public class GuideFilm1Data
        {
            public int cizizenNum; //村民数量
            public People[] m_citizen = new People[1];//村民
            public string m_citizenAssetName = "NPC_04";        //村民资源
            public float m_citizenFadeTime = 3.5f;      //村民消失总时间间隔
            public float[] m_speed = new float[1] { 0.55f };       //移动速度

            public float[] m_citizenStartPos = new float[2] { 2.2f, -2.4f };//村民初始位置(相对)
            public float[] m_cameraPos = new float[2] { 1.5f, -1.5f };//摄像机位置（相对）
            public float[] m_citizenEndPos = new float[2] { 0.8f, -0.7f };//村民结束位置（相对）
        }

        public class GuideFilm1
        {
            public int cizizenNum; //村民数量
            public string m_citizenAssetName = "NPC_04";        //村民资源
            public float m_citizenFadeTime = 2.5f;      //村民消失总时间间隔
            public float[] m_speed = new float[1] { 0.5f };       //移动速度
            public Vector2 m_citizenStartPos = new Vector2(4, -4f);//村民初始位置(相对)
            public Vector2 m_cameraPos = new Vector2(1, -1f);//摄像机位置（相对）
            public Vector2 m_citizenEndPos = new Vector2(3f, -3f);//村民结束位置（相对）


            public Action quitCallback;
            public People[] m_citizen = new People[1];//村民

            public bool m_isDispose;

            public GuideFilm1(GuideFilm1Data data)
            {
                cizizenNum = data.cizizenNum;
                m_citizenAssetName = data.m_citizenAssetName;
                m_citizenFadeTime = data.m_citizenFadeTime;
                m_speed = data.m_speed;
                m_citizenStartPos = FloatToV2(data.m_citizenStartPos);
                m_cameraPos = FloatToV2(data.m_cameraPos);
                m_citizenEndPos = FloatToV2(data.m_citizenEndPos);
            }
            public void GuideFilm()
            {
                CityGlobalMediator cityGlobal = AppFacade.GetInstance().RetrieveMediator(CityGlobalMediator.NameMediator) as CityGlobalMediator;
                Vector2 cityPos = new Vector2(cityGlobal.CityBuildingContainer.position.x, cityGlobal.CityBuildingContainer.position.z);
                m_citizenStartPos += cityPos;
                m_citizenEndPos += cityPos;
                PlayerProxy m_playerPorxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                WorldCamera.Instance().ViewTerrainPos(m_playerPorxy.CurrentRoleInfo.pos.x / 100 + m_cameraPos.x, m_playerPorxy.CurrentRoleInfo.pos.y / 100 + m_cameraPos.y, 1000, () =>
                {
                    CreateCitizen();
                });

            }

            private void CreateCitizen()
            {
                for (int i = 0; i < m_citizen.Length; i++)
                {
                    int k = i;
                    CoreUtils.assetService.Instantiate("citizen", (GameObject gameObject) =>
                    {
                        m_citizen[k] = gameObject.GetComponent<People>();
                        gameObject.SetActive(false);
                        CoreUtils.assetService.Instantiate(m_citizenAssetName, (GameObject uint_obj) =>
                        {
                            People.InitCitizenS(m_citizen[k], uint_obj, Color.white);
                            People.SetUnitFootprintsActiveS(m_citizen[k], true);
                            People.ResetUnitPosS(m_citizen[k]);
                            People.SetStateS(m_citizen[k], People.ENMU_CITIZEN_STAT.RUN, m_citizenStartPos, m_citizenEndPos, m_speed[k]);
                            gameObject.SetActive(true);
                        });
                    });
                    Timer.Register(m_citizenFadeTime, () =>
                    {
                        if (m_isDispose)
                        {
                            return;
                        }
                        for (int j = 0; j < m_citizen.Length; j++)
                        {
                            m_citizen[j].FadeOut();
                        }
                        PlayerProxy m_playerPorxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                        WorldCamera.Instance().ViewTerrainPos(m_playerPorxy.CurrentRoleInfo.pos.x / 100, m_playerPorxy.CurrentRoleInfo.pos.y / 100, 1000,
                            () =>
                            {
                                quitCallback?.Invoke();
                            });
                    });
                }
            }

            public void OnQuit()
            {
                m_isDispose = true;
            }
        }

        public class GuideFilm2Data
        {
            public int citizenNum = 5;
            public string m_barbarianName = "BarbarianFormation_00";//野蛮人资源
            public string m_smokeName = "operation_2006";//冒烟资源
            public string villiageAssetName = "build_filmvillige";//村庄资源
            public float[] m_village3DPos = new float[3] { 0.3f, 0, 10.7f };//村庄相对主城的相对位置，有且只有这个是3D的
            public float[] m_barbarianStartPosData = new float[2] { -9f, 0.3f };//野蛮人出生点
            public float[] m_barbarianAttackPosData = new float[2] { -5.5f, 0.3f };//野蛮人攻击点
            public float[] m_barbarianLeavePosData = new float[2] { -20f, -20f };//野蛮人消失点
            public float[] m_cameraPos = new float[2] { -4f, 5f };//摄像机位置
            public string[] m_citizenAssetName = new string[] { "NPC_04", "NPC_03", "NPC_04", "NPC_03", "NPC_04" };  //村民资源
            public float[][] m_citizenTargetPos = new float[5][] { new float[2] { 6.87f, 9.95f }, new float[2] { -7.35f, -6.16f }, new float[2] { 5f, -4.09f }, new float[2] { -1.81f, -7.54f }, new float[2] { 8.93f, -2.21f } };//村民消失位置
            public float m_fightTime = 2f;      //野蛮人攻击开始时间
            public float m_smokeTime = 4f;      //烟雾飘起的开始时间
            public float[] m_citizenTime = new float[] { 3.5f, 4.8f, 4f, 5f, 4.5f };  //村民开始逃跑的时间
            public float m_citizenFadeTime = 2.5f;      //每个村民逃跑的时间
            public float[] m_speed = new float[5] { 1.5f, 2.5f, 1.8f, 2.3f, 3f };           //村民移动速度
            public float m_barbarianSpeed = 1.5f;           //野蛮人移动速度
            public float m_villageRadius = 3f;//村庄半径
        }

        public class GuideFilm2
        {
            public int citizenNum;
            public string m_barbarianName;//野蛮人资源
            public string m_smokeName;//冒烟资源
            public Vector3 m_village3DPos;//村庄相对主城的相对位置，有且只有这个是3D的
            public Vector2 m_barbarianStartPos;//野蛮人出生点
            public Vector2 m_barbarianLeavePos;//野蛮人消失点
            public Vector2 m_cameraPos;//摄像机位置
            public string[] m_citizenAssetName;           //村民资源
            public Vector2[] m_citizenTargetPos;//村民消失位置
            public Vector2[] m_citizenStartPos = new Vector2[5];//村民消失位置
            public Vector2 m_barbarianAttackPos;//野蛮人攻击点

            public float m_fightTime;      //野蛮人攻击开始时间
            public float m_smokeTime;      //烟雾飘起的开始时间
            public float[] m_citizenTime ;      //村民开始逃跑的时间
            public float m_citizenFadeTime ;      //村民逃跑的总时间间隔
            public float[] m_speed = new float[5] { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };           //村民移动速度
            public float m_barbarianSpeed = 0.6f;           //野蛮人移动速度
            public string villiageAssetName = "build_filmvillige";//村庄资源
            public float m_villageRadius = 3f;//村庄半径

            public bool m_isDispose = false;

            public GuideFilm2(GuideFilm2Data data)
            {
                citizenNum = data.citizenNum;
                m_barbarianName = data.m_barbarianName;
                m_smokeName = data.m_smokeName;
                m_village3DPos = FloatToV3(data.m_village3DPos);
                m_barbarianStartPos = FloatToV2(data.m_barbarianStartPosData);
                m_barbarianLeavePos = FloatToV2(data.m_barbarianLeavePosData);
                m_cameraPos = FloatToV2(data.m_cameraPos);
                m_citizenAssetName = data.m_citizenAssetName;
                m_citizenTargetPos = FloatToV2Array(data.m_citizenTargetPos);
                m_fightTime = data.m_fightTime;
                m_smokeTime = data.m_smokeTime;
                m_citizenTime = data.m_citizenTime;
                m_citizenFadeTime = data.m_citizenFadeTime;
                m_speed = data.m_speed;
                m_barbarianSpeed = data.m_barbarianSpeed;
                villiageAssetName = data.villiageAssetName;
                m_barbarianAttackPos = FloatToV2(data.m_barbarianAttackPosData);
                m_villageRadius = data.m_villageRadius;
            }

            public bool m_fightSignal = false;         //野蛮人开始攻击信号
            public bool m_leftSignal = false;         //野蛮人攻击结束信号
            public bool m_smokeSignal = false;       //村庄着火信号
            public bool m_quitSignal = false;       //film完成信号
            public Transform m_parent;
            public GameObject m_village;
            public GameObject m_smoke;
            public Troops m_Formation;
            public People[] m_citizen;//村民
            public bool[] m_citizenLeft;
            public Action citizenLeftCallback;
            public bool isCizizenLeft = false;
            public Action quitCallback;
            public Vector2 m_villagePos = new Vector2(-3f, 3f);//村庄2D位置


            public AudioHandler FightAudio;
            public void GuideFilm( )
            {
                PlayerProxy m_playerPorxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                InitPos(InitFormation);
                Timer.Register(0.5f,()=> {
                    if (m_isDispose)
                    {
                        return;
                    }
                    WorldCamera.Instance().ViewTerrainPos(m_playerPorxy.CurrentRoleInfo.pos.x / 100 + m_cameraPos.x, m_playerPorxy.CurrentRoleInfo.pos.y / 100 + m_cameraPos.y, 500, () =>
                    {

                    });
                });

            }
            private void InitFormation()
            {
                CoreUtils.assetService.Instantiate(m_barbarianName, (go) =>
                {
                    go.transform.SetParent(m_parent);
                    go.transform.localPosition = Vector3.zero;
                    m_Formation = go.GetComponent<Troops>();
                    if(m_Formation==null)
                    {
                        Debug.LogError("野蛮人资源未加载成功："+ m_barbarianName);
                    }
                    else
                    {
                        m_Formation.transform.gameObject.name = "barbarian";
                        SquareHelper squareHelper = SquareHelper.Instance;
                        string des = squareHelper.GetBarbarianDes(Troops.ENMU_MATRIX_TYPE.BARBARIAN);
                        Troops.InitPositionS(m_Formation, m_barbarianStartPos, m_barbarianAttackPos);
                        Troops.InitFormationS(m_Formation, des, Color.gray);
                        m_Formation.SetState(Troops.ENMU_SQUARE_STAT.MOVE, m_barbarianStartPos, m_barbarianAttackPos, m_barbarianSpeed);
                    }
                    Timer.Register(m_fightTime, () => { m_fightSignal = true; });
                    Timer.Register(m_smokeTime, () => { m_smokeSignal = true; });
                    for (int i = 0; i < citizenNum; i++)
                    {
                        int k = i;
                        Timer.Register(m_citizenTime[k], () => { CreateCitizen(k); });
                    }
                });
            }

            private void InitPos(Action callback)
            {
                CityGlobalMediator cityGlobal = AppFacade.GetInstance().RetrieveMediator(CityGlobalMediator.NameMediator) as CityGlobalMediator;
                m_parent = cityGlobal.CityBuildingContainer;
                CoreUtils.assetService.Instantiate(villiageAssetName, (village) =>
                {
                    m_village = village;
                    village.transform.SetParent(m_parent);
                    village.transform.localPosition = m_village3DPos;
                    m_villagePos = new Vector2(village.transform.position.x, village.transform.position.z);
                    m_barbarianStartPos = m_barbarianStartPos + m_villagePos;
                    m_barbarianAttackPos = m_barbarianAttackPos+ m_villagePos;
                    m_barbarianLeavePos = m_villagePos + m_barbarianLeavePos;
                    m_citizenStartPos[0] = m_citizenStartPos[0]+ m_villagePos + m_citizenTargetPos[0].normalized * m_villageRadius;
                    m_citizenStartPos[1] = m_citizenStartPos[1] + m_villagePos + m_citizenTargetPos[1].normalized * m_villageRadius;
                    m_citizenStartPos[2] = m_citizenStartPos[2] + m_villagePos + m_citizenTargetPos[2].normalized * m_villageRadius;
                    m_citizenStartPos[3] = m_citizenStartPos[3] + m_villagePos + m_citizenTargetPos[3].normalized * m_villageRadius;
                    m_citizenStartPos[4] = m_citizenStartPos[4] + m_villagePos + m_citizenTargetPos[4].normalized * m_villageRadius;
                    m_citizenTargetPos[0] = m_citizenTargetPos[0] + m_villagePos;
                    m_citizenTargetPos[1] = m_citizenTargetPos[1] + m_villagePos;
                    m_citizenTargetPos[2] = m_citizenTargetPos[2] + m_villagePos;
                    m_citizenTargetPos[3] = m_citizenTargetPos[3] + m_villagePos;
                    m_citizenTargetPos[4] = m_citizenTargetPos[4] + m_villagePos;

                    callback();
                });

            }

            public void OnUpdate()
            {
                if (m_fightSignal&&m_Formation&& m_Formation.gameObject)
                {
                    m_fightSignal = false;
                    m_Formation.SetState(Troops.ENMU_SQUARE_STAT.FIGHT, m_barbarianAttackPos, m_barbarianAttackPos, m_barbarianSpeed);
                    CoreUtils.audioService.PlayLoop3D("Sound_Battle_hit", m_Formation.gameObject,(ah)=>
                    {
                        FightAudio = ah;
                    });
                }

                if (m_leftSignal && m_Formation)
                {
                    m_leftSignal = false;
                    m_Formation.SetState(Troops.ENMU_SQUARE_STAT.MOVE, m_barbarianAttackPos, m_barbarianLeavePos, m_barbarianSpeed);
                    if(FightAudio!=null)
                    {
                        CoreUtils.audioService.StopByHandler(FightAudio);
                    }
                }

                if (m_smokeSignal&& m_village)
                {
                    m_smokeSignal = false;
                    CoreUtils.assetService.Instantiate(m_smokeName, (go) =>
                    {
                        m_smoke = go;
                        go.transform.SetParent(m_village.transform);
                        go.transform.localPosition = Vector3.zero;
                        go.transform.localScale = Vector3.one*3;
                    });
                }
            }

            private void CreateCitizen(int k)
            {
                if(m_citizen==null)
                {
                    m_citizen = new People[citizenNum];
                    m_citizenLeft = new bool[citizenNum];
                }
                CoreUtils.assetService.Instantiate("citizen", (GameObject gameObject) =>
                {
                    if (m_isDispose)
                    {
                        return;
                    }
                    m_citizen[k] = gameObject.GetComponent<People>();
                    gameObject.SetActive(false);
                    CoreUtils.assetService.Instantiate(m_citizenAssetName[k], (GameObject uint_obj) =>
                    {
                        if (m_isDispose)
                        {
                            return;
                        }
                        if (uint_obj==null||uint_obj.name.Contains("ErrorPrefab"))
                        {
                            Debug.LogError("村民加载失败");
                        }
                        else
                        {
                            People.InitCitizenS(m_citizen[k], uint_obj, Color.white);
                            People.SetUnitFootprintsActiveS(m_citizen[k], true);
                            People.ResetUnitPosS(m_citizen[k]);
                            People.SetStateS(m_citizen[k], People.ENMU_CITIZEN_STAT.RUN,m_citizenStartPos[k], m_citizenTargetPos[k], m_speed[k]);
                            gameObject.SetActive(true);
                        }
                        Timer.Register(m_citizenFadeTime, () =>
                        {
                            if (m_isDispose)
                            {
                                return;
                            }
                            m_citizen[k].FadeOut();
                            m_citizenLeft[k] = true;
                            for(int j = 0;j<m_citizenLeft.Length;j++)
                            {
                                if(!m_citizenLeft[j])
                                {
                                    return;
                                }
                            }
                            isCizizenLeft = true;
                            citizenLeftCallback?.Invoke();
                        });
                    });
                });
            }

            public void OnQuit()
            {
                m_isDispose = true;
                m_quitSignal = false;
                if (FightAudio != null)
                {
                    CoreUtils.audioService.StopByHandler(FightAudio);
                }
                if (m_Formation)
                {
                    m_Formation.SetState(Troops.ENMU_SQUARE_STAT.DEAD,Vector2.zero,Vector2.zero);
                    CoreUtils.assetService.Destroy(m_Formation.gameObject);            
                }
                if(m_smoke)
                {
                    GameObject.DestroyImmediate(m_smoke);
                }
                if(m_village)
                {
                    GameObject.DestroyImmediate(m_village);
                }
                quitCallback?.Invoke();
                quitCallback = null;
            }
        }
        #endregion

        public class GuideFilm3
        {
            public static long FirstTroopID = int.MaxValue - 1;
            public static long SecondTroopID = int.MaxValue -2;
            public static long FirstBarbarianID = int.MaxValue - 3;
            public static long SecondBarbarianID = int.MaxValue - 4;

            private PlayerProxy m_playerProxy;

            public Map_ObjectInfo.request m_firstTroop;
            public Map_ObjectInfo.request FirstTroop;
            public Map_ObjectInfo.request m_firstBarbarian;

            public Map_ObjectInfo.request m_secondTroop;
            public Map_ObjectInfo.request m_secondBarbarian;


            public long FirstTroopArmyID = 10101;
            public long FirstTroopArmyCount = 1000;

            private Action m_firstFightEndCallback;
            private Action m_finalFightEndCallback;

            //第一场战斗直至获胜的信号
            public bool m_playFirstFightSignal;
            //第二场战斗开始信号
            public bool m_playSecondFightSingal;
            //加入双人战斗信号
            public bool m_togetherFightJoin;
            //开始双人战斗信号
            public bool m_togetherFightBegin;
            //所有战斗结束信号
            public bool m_stopAllFightSingal;
            //回城信号
            public bool m_returnSingal;
            //回城时间
            public long m_returnSpeed = 7;            
            //最后回城的位置
            private PosInfo m_returnPos = new PosInfo { x = 0, y = 300 };
            //第二次摄像机位置
            private PosInfo m_secondCameraPos = new PosInfo { x = 100, y = 790 };
            //城市位置
            private PosInfo m_cityPos;


            //第一只部队第一场战斗移动到达的时间
            private long m_firstTroopMoveSpeed = 8;
            //第一只部队最后一场战斗移动到达的时间
            private long m_firstTroopFinalMoveSpeed = 8;
            //第一只部队起始位置
            private PosInfo m_firstTroopStartPos = new PosInfo { x = -200, y = 200 };
            //第一只部队战斗位置
            private PosInfo m_firstTroopFightPos = new PosInfo { x = -590, y = 590 };
            //第一只部队最后战斗的位置
            private PosInfo m_firstTroopFinalFightPos = new PosInfo { x = -30, y = 1170 };



            //第一只部队主将ID
            private long m_firstTroopMainHeroID = 101;
            //第一只部队主将技能
            private long m_fightMainHeroSkillId = 0;
            //第一只部队士兵ID
            private long m_firstSoldierID = 10101;
            //第一只部队士兵类型
            private long m_firstSoldierType = 1;
            //第一只部队士兵等级
            private long m_firstSoldierLevel = 1;
            //第一只部队士兵数量
            private long m_firstSoldierNum = 500;
            //第一只部队战斗力
            private long m_firstArmyPower = 1000;
            //第一场战斗打完的停留时间
            private float m_firstFightTime = 1f;

            //第一只野蛮人部队的ID -相对主城
            private long m_firstBarbarianID = 1001;
            //第一只野蛮人相对位置
            private PosInfo m_firstBarbarianPos = new PosInfo { x = -800, y = 800 };
            //第一只野蛮人统帅ID
            private long m_firstBarbarianMainHeroID = 9001;
            //第一只野蛮人士兵数量
            private long m_firstBarbarianSoldierNum = 400;
            //第一只野蛮人战斗力
            private long m_firstBarbarianArmyPower = 1000;

            //第二只部队开始追逐的延迟
            private long m_secondTroopChaseDelay = 2;
            //第二只部队起始的相对位置
            private PosInfo m_secondTroopFirstPos = new PosInfo { x = -1600,y = 3100 };
            //第二只部队结束的相对位置
            private PosInfo m_secondTroopSecondPos = new PosInfo { x = 640, y = 1240 };
            //第二只部队移动到达的时间
            private long m_secondTroopMoveSpeed = 20;//移动时间 含野蛮人
            //第二只部队掉头行军时间 
            private long m_secondTroopUTurnSpeed = 2;
            //第二只部队掉头的相对位置
            private PosInfo m_secondTroopThirdPos = new PosInfo { x = 540, y = 1340 };
            //第二只部队主将ID
            private long m_secondTroopMainHeroID = 101;
            //第二只部队主将技能
            private long m_secondMainHeroSkillId = 0;
            //第二只部队士兵ID
            private long m_secondSoldierID = 10301;
            //第二只部队士兵类型
            private long m_secondSoldierType = 3;
            //第二只部队士兵等级
            private long m_secondSoldierLevel = 1;
            //第二只部队士兵数量
            private long m_secondSoldierNum = 400;
            //第二只部队战斗力
            private long m_secondArmyPower = 1000;

            //第二只野蛮人部队的ID
            private long m_secondBarbarianID = 1001;
            //第二只野蛮人起始相对位置
            private PosInfo m_secondBarbarianFirstPos = new PosInfo { x = -1828, y = 3448 };
            //第二只野蛮人终点相对位置
            private PosInfo m_secondBarbarianSecondPos = new PosInfo { x = 248, y = 1680 };
            //第二只野蛮人统帅ID
            private long m_secondBarbarianMainHeroID = 9001;
            //第二只野蛮人士兵数量
            private long m_secondBarbarianSoldierNum = 600;
            //第二只野蛮人战斗力
            private long m_secondBarbarianArmyPower = 1000;

            private bool m_isDispose;

            public GuideFilm3()
            {
                InitFilm();
            }

            public void InitFilm()
            {
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME)as PlayerProxy;
                CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)m_playerProxy.CurrentRoleInfo.country);
                m_firstTroopMainHeroID = define.initialHero;
                m_fightMainHeroSkillId = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_firstTroopMainHeroID).skill[0];
                m_secondTroopMainHeroID = CoreUtils.dataService.QueryRecord<ConfigDefine>(0).guideHero;
                m_secondMainHeroSkillId = CoreUtils.dataService.QueryRecord<HeroDefine>((int)m_secondTroopMainHeroID).skill[0];
                InitFakeData();
            }

            //创建野蛮人
            public void CreateFirstBarbarian()
            {
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, m_firstBarbarian);
            }

            //移动摄像机到第一个野蛮人
            public void MoveCameraToFirstBarbarian(Action<PosInfo> callBack)
            {
                WorldCamera.Instance().ViewTerrainPos(m_firstBarbarianPos.x/100, m_firstBarbarianPos.y/100, 1000,
    () => { callBack?.Invoke(m_firstBarbarianPos); });
            }

            //移动摄像机到第二个野蛮人
            public void MoveCameraToSecondBarbarian(Action<PosInfo> callBack)
            {
                WorldCamera.Instance().ViewTerrainPos(m_secondCameraPos.x/ 100, m_secondCameraPos.y / 100, 1000,
    () => { callBack?.Invoke(m_secondCameraPos); });

            }

            //派出第一场战斗
            public void PlayFirstFight(Action callback)
            {
                m_firstFightEndCallback = callback;
                long time = ServerTimeModule.Instance.GetServerTime();
                m_firstTroop.mapObjectInfo.isGuide = true;
                m_firstTroop.mapObjectInfo.startTime = time;
                m_firstTroop.mapObjectInfo.arrivalTime = time + m_firstTroopMoveSpeed;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, m_firstTroop);
                Timer.Register(m_firstTroopMoveSpeed, StartFirstFight);
            }

            //野蛮人追逐第二只部队
            public void PlaySecondChase()
            {
                Timer.Register(m_secondTroopChaseDelay, () =>
                {
                    long time = ServerTimeModule.Instance.GetServerTime();
                    m_secondBarbarian.mapObjectInfo.startTime = time;
                    m_secondBarbarian.mapObjectInfo.arrivalTime = time + m_secondTroopMoveSpeed;

                    m_secondTroop.mapObjectInfo.startTime = time;
                    m_secondTroop.mapObjectInfo.arrivalTime = time + m_secondTroopMoveSpeed - m_secondTroopUTurnSpeed;
                    //创建部队
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, m_secondBarbarian);
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, m_secondTroop);
                    WorldCamera.Instance().ViewTerrainPos(WorldCamera.Instance().GetViewCenter().x, WorldCamera.Instance().GetViewCenter().y, 50, null);
                    //掉头
                    Timer.Register(m_secondTroopMoveSpeed - m_secondTroopUTurnSpeed, UTurn);
                    //开始战斗
                    Timer.Register(m_secondTroopMoveSpeed, StarSecondFight);
                });

            }

            //第二只部队 往回走一段
            private void UTurn()
            {
                long startTime = ServerTimeModule.Instance.GetServerTime();
                Map_ObjectInfo.request troop2 = new Map_ObjectInfo.request();
                troop2.mapObjectInfo = new MapObjectInfo();
                troop2.mapObjectInfo.objectId = SecondTroopID;
                troop2.mapObjectInfo.objectPath = new List<PosInfo>();
                troop2.mapObjectInfo.objectPos = m_secondTroopSecondPos;
                troop2.mapObjectInfo.objectPath.Add(m_secondTroopSecondPos);
                troop2.mapObjectInfo.objectPath.Add(m_secondTroopThirdPos);
                troop2.mapObjectInfo.status = (long)ArmyStatus.SPACE_MARCH;
                troop2.mapObjectInfo.startTime = startTime;
                troop2.mapObjectInfo.arrivalTime = startTime + m_secondTroopUTurnSpeed;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop2);
            }

            //返回城市
            public void ReturnToCity()
            {
                //Debug.LogError("ReturnToCity:"+ FirstTroopID);
                long currentTime = ServerTimeModule.Instance.GetServerTime();
                m_firstTroop = new Map_ObjectInfo.request();
                m_firstTroop.mapObjectInfo = new MapObjectInfo();
                m_firstTroop.mapObjectInfo.objectId = FirstTroopID;
                m_firstTroop.mapObjectInfo.objectPath = new List<PosInfo>();
                m_firstTroop.mapObjectInfo.objectPos = m_firstTroopFinalFightPos;
                m_firstTroop.mapObjectInfo.objectPath.Add(m_firstTroopFinalFightPos);
                m_firstTroop.mapObjectInfo.objectPath.Add(m_returnPos);
                m_firstTroop.mapObjectInfo.startTime = currentTime;
                m_firstTroop.mapObjectInfo.arrivalTime = currentTime + m_returnSpeed;
                m_firstTroop.mapObjectInfo.status = (int)ArmyStatus.SPACE_MARCH;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, m_firstTroop);

                m_secondTroop = new Map_ObjectInfo.request();
                m_secondTroop.mapObjectInfo = new MapObjectInfo();
                m_secondTroop.mapObjectInfo.objectId = SecondTroopID;
                m_secondTroop.mapObjectInfo.objectPath = new List<PosInfo>();
                m_secondTroop.mapObjectInfo.objectPos = m_secondTroopThirdPos;
                m_secondTroop.mapObjectInfo.objectPath.Add(m_secondTroopThirdPos);
                m_secondTroop.mapObjectInfo.objectPath.Add(m_returnPos);
                m_secondTroop.mapObjectInfo.startTime = currentTime;
                m_secondTroop.mapObjectInfo.arrivalTime = currentTime + m_returnSpeed;
                m_secondTroop.mapObjectInfo.armyRid = m_playerProxy.CurrentRoleInfo.rid;
                m_secondTroop.mapObjectInfo.status = (int)ArmyStatus.SPACE_MARCH;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, m_secondTroop);

                TroopProxy troopPorxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
                Troops formation =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop((int)FirstTroopID) as Troops;
                if(formation)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.AutoFollowTroopReturnCity,formation.gameObject);
                }

                Timer.Register(m_returnSpeed,()=>
                {
                    WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveLine((int)FirstTroopID);

                    //m_firstTroop = new Map_ObjectInfo.request();
                    //m_firstTroop.mapObjectInfo = new MapObjectInfo();
                    //m_firstTroop.mapObjectInfo.objectId = FirstTroopID;

                    m_firstTroop.mapObjectInfo.armyRid = 0;

                    //AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, m_firstTroop);

                    m_secondTroop.mapObjectInfo.armyRid = 0;

                    Map_ObjectDelete.request deleteReq = new Map_ObjectDelete.request();
                    deleteReq.objectId = FirstTroopID;
                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq);

                    Map_ObjectDelete.request deleteReq2 = new Map_ObjectDelete.request();
                    deleteReq2.objectId = SecondTroopID;
                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq2);

                    m_firstTroop.mapObjectInfo.objectId = 0;
                    m_secondTroop.mapObjectInfo.objectId = 0;

                    AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopRemove, new ArmyData((int)FirstTroopID));
                    AppFacade.GetInstance().SendNotification(CmdConstant.ArmyDataLodPopRemove, new ArmyData((int)SecondTroopID));
                });
            }

            //开始第一场战斗
            private void StartFirstFight()
            {
                WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveLine((int)FirstTroopID);
                Map_ObjectInfo.request troop1 = new Map_ObjectInfo.request();
                troop1.mapObjectInfo = new MapObjectInfo();
                troop1.mapObjectInfo.objectId = FirstTroopID;
                troop1.mapObjectInfo.targetObjectIndex = FirstBarbarianID;
                troop1.mapObjectInfo.status = (long)ArmyStatus.BATTLEING;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop1);


                Map_ObjectInfo.request barbarian1 = new Map_ObjectInfo.request();
                barbarian1.mapObjectInfo = new MapObjectInfo();
                barbarian1.mapObjectInfo.objectId = FirstBarbarianID;
                barbarian1.mapObjectInfo.targetObjectIndex = FirstTroopID;
                barbarian1.mapObjectInfo.status = (long)ArmyStatus.BATTLEING;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian1);
                Timer.Register(1f, ContinuteFirstFight);
            }

            //第一场战斗持续攻击
            private void ContinuteFirstFight()
            {
                if (m_isDispose)
                {
                    return;
                }
                long attackCount = UnityEngine.Random.Range(25, 40);
                long defenseCount = UnityEngine.Random.Range(5, 20);

                //播放技能特效 技能伤害
                if (m_firstTroop.mapObjectInfo.sp >= m_firstTroop.mapObjectInfo.maxSp)
                {
                    //头像放大
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)FirstTroopID, BattleUIType.BattleUI_HeadChangeScale, null);

                    //播放技能特效
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)FirstTroopID, BattleUIType.BattleUI_Skills, (int)m_fightMainHeroSkillId, (int)FirstBarbarianID);
                    
                    //敌方播放技能伤害
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)FirstBarbarianID, BattleUIType.BattleUI_HP, Mathf.CeilToInt(attackCount*1.5f), (int)m_fightMainHeroSkillId);

                    //受击表现
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)FirstBarbarianID, BattleUIType.BattleUI_ShowBeAttack, (int)m_fightMainHeroSkillId);

                    //怒气清空
                    Map_ObjectInfo.request addTroop1 = new Map_ObjectInfo.request();
                    addTroop1.mapObjectInfo = new MapObjectInfo();
                    addTroop1.mapObjectInfo.objectId = FirstTroopID;
                    addTroop1.mapObjectInfo.sp = 0;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, addTroop1);

                    m_firstTroop.mapObjectInfo.sp = 0;
                }

                Map_ObjectInfo.request troop1 = new Map_ObjectInfo.request();
                troop1.mapObjectInfo = new MapObjectInfo();
                troop1.mapObjectInfo.objectId = FirstTroopID;

                Map_ObjectInfo.request barbarian1 = new Map_ObjectInfo.request();
                barbarian1.mapObjectInfo = new MapObjectInfo();
                barbarian1.mapObjectInfo.objectId = FirstBarbarianID;

                m_firstBarbarian.mapObjectInfo.armyCount -= attackCount; 
                m_firstTroop.mapObjectInfo.armyCount -= defenseCount;

                barbarian1.mapObjectInfo.armyCount = m_firstBarbarian.mapObjectInfo.armyCount;
                troop1.mapObjectInfo.armyCount = m_firstTroop.mapObjectInfo.armyCount;

                Battle_BattleDamageInfo.request req = new Battle_BattleDamageInfo.request();
                req.battleDamageInfo=  new Dictionary<long, BattleDamageInfo>(); 
                BattleDamageInfo battleDamageInfo= new BattleDamageInfo();
                battleDamageInfo.damage = attackCount;
                battleDamageInfo.objectIndex = FirstBarbarianID;
                req.battleDamageInfo.Add(battleDamageInfo.objectIndex,battleDamageInfo);              
                AppFacade.GetInstance().SendNotification(Battle_BattleDamageInfo.TagName,req);

                Battle_BattleDamageInfo.request req2 = new Battle_BattleDamageInfo.request();
                req2.battleDamageInfo=  new Dictionary<long, BattleDamageInfo>();
                BattleDamageInfo battleDamageInfo1= new BattleDamageInfo();
                battleDamageInfo1.damage = defenseCount;
                battleDamageInfo1.objectIndex = FirstTroopID;
                req2.battleDamageInfo.Add(battleDamageInfo1.objectIndex,battleDamageInfo1);
                AppFacade.GetInstance().SendNotification(Battle_BattleDamageInfo.TagName, req2);

                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian1);
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop1);

                if (m_firstBarbarian.mapObjectInfo.armyCount>0)
                {                    
                    Timer.Register(1f, ContinuteFirstFight);

                    Timer.Register(0.2f, ()=> {
                        if (m_isDispose)
                        {
                            return;
                        }

                        //增加怒气
                        m_firstTroop.mapObjectInfo.sp = m_firstTroop.mapObjectInfo.sp + 33;
                        if (m_firstTroop.mapObjectInfo.sp >= 100)
                        {
                            m_firstTroop.mapObjectInfo.sp = 100;
                        }
                        Map_ObjectInfo.request addTroop1 = new Map_ObjectInfo.request();
                        addTroop1.mapObjectInfo = new MapObjectInfo();
                        addTroop1.mapObjectInfo.objectId = FirstTroopID;
                        addTroop1.mapObjectInfo.sp = m_firstTroop.mapObjectInfo.sp;
                        AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, addTroop1);
                    });
                }
                else
                {
                    Timer.Register(m_firstFightTime, m_firstFightEndCallback);
                    m_playFirstFightSignal = true;
                    //切换到待机
                    Map_ObjectInfo.request barbarian2 = new Map_ObjectInfo.request();
                    barbarian2.mapObjectInfo = new MapObjectInfo();
                    barbarian2.mapObjectInfo.objectId = FirstBarbarianID;
                    barbarian2.mapObjectInfo.status = (long)ArmyStatus.ARMY_SATNBY;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian2);
                    //溃败状态
                    barbarian1 = new Map_ObjectInfo.request();
                    barbarian1.mapObjectInfo = new MapObjectInfo();
                    barbarian1.mapObjectInfo.objectId = FirstBarbarianID;
                    barbarian1.mapObjectInfo.status = (long)ArmyStatus.MONSTER_FAILED;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian1);
                    //删除第一只怪物
                    Map_ObjectDelete.request deleteReq = new Map_ObjectDelete.request();
                    deleteReq.objectId = m_firstBarbarian.mapObjectInfo.objectId;
                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq);
                    m_firstBarbarian.mapObjectInfo.objectId = 0;
                    //玩家切换到驻扎状态
                    Map_ObjectInfo.request troop1_1 = new Map_ObjectInfo.request();
                    troop1_1.mapObjectInfo = new MapObjectInfo();
                    troop1_1.mapObjectInfo.objectPos = m_firstTroopFightPos;
                    troop1_1.mapObjectInfo.objectId = m_firstTroop.mapObjectInfo.objectId;
                    troop1_1.mapObjectInfo.status = (int)ArmyStatus.STATIONING;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop1_1);
                }
            }

            //开始第二场战斗
            private void StarSecondFight()
            {
                Map_ObjectInfo.request troop2 = new Map_ObjectInfo.request();
                troop2.mapObjectInfo = new MapObjectInfo();
                troop2.mapObjectInfo.objectId = SecondTroopID;
                troop2.mapObjectInfo.objectPos = m_secondTroopThirdPos;
                troop2.mapObjectInfo.targetObjectIndex = SecondBarbarianID;
                troop2.mapObjectInfo.objectPath = new List<PosInfo>();
                troop2.mapObjectInfo.objectPath.Add(m_secondTroopThirdPos);
                troop2.mapObjectInfo.status = (long)ArmyStatus.BATTLEING;
                PosInfo troop2Pos = new PosInfo { x = m_secondTroopThirdPos.x-10,y = m_secondTroopThirdPos.y+10 };
                troop2.mapObjectInfo.objectPath.Add(troop2Pos);
                troop2.mapObjectInfo.arrivalTime = ServerTimeModule.Instance.GetServerTime() + 1;

                Map_ObjectInfo.request barbarian2 = new Map_ObjectInfo.request();
                barbarian2.mapObjectInfo = new MapObjectInfo();
                barbarian2.mapObjectInfo.objectId = SecondBarbarianID;
                barbarian2.mapObjectInfo.targetObjectIndex = SecondTroopID;
                barbarian2.mapObjectInfo.objectPos = m_secondBarbarianSecondPos;
                barbarian2.mapObjectInfo.objectPath = new List<PosInfo>();;
                barbarian2.mapObjectInfo.objectPath.Add(m_secondBarbarianSecondPos);
                PosInfo barbarian2Pos = new PosInfo { x = m_secondBarbarianSecondPos.x + 5, y = m_secondBarbarianSecondPos.y - 5 };
                barbarian2.mapObjectInfo.objectPath.Add(barbarian2Pos);
                barbarian2.mapObjectInfo.arrivalTime = ServerTimeModule.Instance.GetServerTime()+1;

                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop2);
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian2);


                Timer.Register(0f,()=>
                {
                    Map_ObjectInfo.request barbarian2_2 = new Map_ObjectInfo.request();
                    barbarian2_2.mapObjectInfo = new MapObjectInfo();
                    barbarian2_2.mapObjectInfo.objectId = SecondBarbarianID;
                    barbarian2_2.mapObjectInfo.status = (long)ArmyStatus.BATTLEING;
                    barbarian2_2.mapObjectInfo.targetObjectIndex = SecondTroopID;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian2_2);
                    ContinuteSecondFight();
                });
            }

            //第二场战斗持续攻击
            private void ContinuteSecondFight()
            {
                long attackCount = UnityEngine.Random.Range(15, 40);
                long defenseCount = UnityEngine.Random.Range(5, 20);
                Battle_BattleDamageInfo.request req1 = new Battle_BattleDamageInfo.request();
                req1.battleDamageInfo= new Dictionary<long, BattleDamageInfo>();
                BattleDamageInfo battleDamageInfo =new BattleDamageInfo();
                battleDamageInfo.damage = attackCount;
                battleDamageInfo.objectIndex = SecondBarbarianID;
                req1.battleDamageInfo.Add(battleDamageInfo.objectIndex,battleDamageInfo);
                AppFacade.GetInstance().SendNotification(Battle_BattleDamageInfo.TagName, req1);
                Battle_BattleDamageInfo.request req2 = new Battle_BattleDamageInfo.request();
                req2.battleDamageInfo=new Dictionary<long, BattleDamageInfo>();
                
                BattleDamageInfo battleDamageInfo1= new BattleDamageInfo();
                battleDamageInfo1.damage = defenseCount;
                battleDamageInfo1.objectIndex = SecondTroopID;
                req2.battleDamageInfo.Add(battleDamageInfo1.objectIndex,battleDamageInfo1);
                AppFacade.GetInstance().SendNotification(Battle_BattleDamageInfo.TagName, req2);
                //掉血到百分之八十就不掉血了
                if (m_secondBarbarian.mapObjectInfo.armyCount >= 0.8 * m_firstBarbarianSoldierNum)
                {
                    Map_ObjectInfo.request troop2 = new Map_ObjectInfo.request();
                    troop2.mapObjectInfo = new MapObjectInfo();
                    Map_ObjectInfo.request barbarian2 = new Map_ObjectInfo.request();
                    barbarian2.mapObjectInfo = new MapObjectInfo();
                    troop2.mapObjectInfo.objectId = SecondTroopID;
                    barbarian2.mapObjectInfo.objectId = SecondBarbarianID;
                    m_secondBarbarian.mapObjectInfo.armyCount -= attackCount;
                    m_secondTroop.mapObjectInfo.armyCount -= defenseCount;
                    barbarian2.mapObjectInfo.armyCount = m_secondBarbarian.mapObjectInfo.armyCount;
                    troop2.mapObjectInfo.armyCount = m_secondTroop.mapObjectInfo.armyCount;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop2);
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian2);
                }


                if (!m_togetherFightBegin)
                {
                    Timer.Register(1f, ContinuteSecondFight);
                }
            }

            //开始向第二场战斗行军
            public void PlayJoinFinalFight(Action callback)
            {
                //Debug.LogError("FirstTroopID:"+ FirstTroopID);
                long currentTime = ServerTimeModule.Instance.GetServerTime();
                m_finalFightEndCallback = callback;
                Map_ObjectInfo.request troop1 = new Map_ObjectInfo.request();
                troop1.mapObjectInfo = new MapObjectInfo();
                troop1.mapObjectInfo.objectId = FirstTroopID;
                troop1.mapObjectInfo.objectPos = m_firstTroopFightPos;
                troop1.mapObjectInfo.objectPath = new List<PosInfo>();
                troop1.mapObjectInfo.objectPath.Add(m_firstTroopFightPos);
                troop1.mapObjectInfo.objectPath.Add(m_firstTroopFinalFightPos);
                troop1.mapObjectInfo.startTime = currentTime;
                troop1.mapObjectInfo.arrivalTime = currentTime + m_firstTroopFinalMoveSpeed;
                troop1.mapObjectInfo.sp = 0;
                troop1.mapObjectInfo.maxSp = 100;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop1);

                troop1 = new Map_ObjectInfo.request();
                troop1.mapObjectInfo = new MapObjectInfo();
                troop1.mapObjectInfo.objectId = FirstTroopID;
                //troop1.mapObjectInfo.objectType = 0;
                troop1.mapObjectInfo.status = (long)ArmyStatus.SPACE_MARCH;
                troop1.mapObjectInfo.startTime = currentTime;
                troop1.mapObjectInfo.arrivalTime = currentTime + m_firstTroopFinalMoveSpeed;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop1);
                Timer.Register(m_firstTroopFinalMoveSpeed, FightTogether);
            }

            //一起战斗
            private void FightTogether()
            {
                WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveLine((int)m_firstTroop.mapObjectInfo.objectId);
                Map_ObjectInfo.request troop1 = new Map_ObjectInfo.request();
                troop1.mapObjectInfo = new MapObjectInfo();
                troop1.mapObjectInfo.objectId = m_firstTroop.mapObjectInfo.objectId;
                troop1.mapObjectInfo.status = (long)ArmyStatus.BATTLEING;
                troop1.mapObjectInfo.objectPos = m_firstTroopFinalFightPos;
                troop1.mapObjectInfo.attackerPos = m_firstTroopFinalFightPos;
                troop1.mapObjectInfo.targetObjectIndex = m_secondBarbarian.mapObjectInfo.objectId;
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop1);
                Timer.Register(1f, ContinuteFightTogether);
            }

            //最后一场战斗持续攻击
            private void ContinuteFightTogether()
            {
                m_togetherFightBegin = true;
                long attack1 = UnityEngine.Random.Range(10, 25);
                long attack2= UnityEngine.Random.Range(10, 25);
                long defenseCount = UnityEngine.Random.Range(5, 20);

                //播放技能特效 技能伤害
                if (m_firstTroop.mapObjectInfo.sp >= m_firstTroop.mapObjectInfo.maxSp)
                {
                    //头像放大
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)FirstTroopID, BattleUIType.BattleUI_HeadChangeScale, null);

                    //播放技能特效
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)FirstTroopID, BattleUIType.BattleUI_Skills, (int)m_fightMainHeroSkillId, (int)SecondBarbarianID);

                    //敌方播放技能伤害
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)SecondBarbarianID, BattleUIType.BattleUI_HP, Mathf.CeilToInt(attack1 * 1.5f), (int)m_fightMainHeroSkillId);

                    //受击表现
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)SecondBarbarianID, BattleUIType.BattleUI_ShowBeAttack, (int)m_fightMainHeroSkillId);

                    //怒气清空
                    Map_ObjectInfo.request addTroop1 = new Map_ObjectInfo.request();
                    addTroop1.mapObjectInfo = new MapObjectInfo();
                    addTroop1.mapObjectInfo.objectId = FirstTroopID;
                    addTroop1.mapObjectInfo.sp = 0;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, addTroop1);

                    m_firstTroop.mapObjectInfo.sp = 0;
                }

                if (m_secondTroop.mapObjectInfo.sp >= m_secondTroop.mapObjectInfo.maxSp)
                {
                    //头像放大
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)SecondTroopID, BattleUIType.BattleUI_HeadChangeScale, null);

                    //播放技能特效
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)SecondTroopID, BattleUIType.BattleUI_Skills, (int)m_secondMainHeroSkillId, (int)SecondBarbarianID);

                    //敌方播放技能伤害
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)SecondBarbarianID, BattleUIType.BattleUI_HP, Mathf.CeilToInt(attack2 * 1.5f), (int)m_secondMainHeroSkillId);

                    //受击表现
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData((int)SecondBarbarianID, BattleUIType.BattleUI_ShowBeAttack, (int)m_secondMainHeroSkillId);

                    //怒气清空
                    Map_ObjectInfo.request addTroop1 = new Map_ObjectInfo.request();
                    addTroop1.mapObjectInfo = new MapObjectInfo();
                    addTroop1.mapObjectInfo.objectId = SecondTroopID;
                    addTroop1.mapObjectInfo.sp = 0;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, addTroop1);

                    m_secondTroop.mapObjectInfo.sp = 0;
                }


                m_secondBarbarian.mapObjectInfo.armyCount -= attack1+attack2;
                m_firstTroop.mapObjectInfo.armyCount -= defenseCount;
                m_secondTroop.mapObjectInfo.armyCount -= defenseCount;

                Map_ObjectInfo.request barbarian2 = new Map_ObjectInfo.request();
                barbarian2.mapObjectInfo = new MapObjectInfo();
                barbarian2.mapObjectInfo.objectId = SecondBarbarianID;
                barbarian2.mapObjectInfo.armyCount = m_secondBarbarian.mapObjectInfo.armyCount;

                Map_ObjectInfo.request troop1 = new Map_ObjectInfo.request();
                troop1.mapObjectInfo = new MapObjectInfo();
                troop1.mapObjectInfo.objectId = FirstTroopID;
                troop1.mapObjectInfo.armyCount = m_firstTroop.mapObjectInfo.armyCount;

                Map_ObjectInfo.request troop2 = new Map_ObjectInfo.request();
                troop2.mapObjectInfo = new MapObjectInfo();
                troop2.mapObjectInfo.objectId = SecondTroopID;
                troop2.mapObjectInfo.armyCount = m_secondTroop.mapObjectInfo.armyCount;

                Battle_BattleDamageInfo.request req = new Battle_BattleDamageInfo.request();
                req.battleDamageInfo= new Dictionary<long, BattleDamageInfo>();
                BattleDamageInfo battleDamageInfo= new BattleDamageInfo();               
                battleDamageInfo.objectIndex = FirstTroopID;
                battleDamageInfo.damage = defenseCount;
                req.battleDamageInfo.Add(battleDamageInfo.objectIndex,battleDamageInfo);
                AppFacade.GetInstance().SendNotification(Battle_BattleDamageInfo.TagName, req);

                Battle_BattleDamageInfo.request req2 = new Battle_BattleDamageInfo.request();
                req2.battleDamageInfo= new Dictionary<long, BattleDamageInfo>();
                BattleDamageInfo battleDamageInfo1= new BattleDamageInfo();                  
                battleDamageInfo1.objectIndex = SecondTroopID;
                battleDamageInfo1.damage = defenseCount;
                req2.battleDamageInfo.Add(battleDamageInfo1.objectIndex,battleDamageInfo1);
                AppFacade.GetInstance().SendNotification(Battle_BattleDamageInfo.TagName, req2);

                Battle_BattleDamageInfo.request req3 = new Battle_BattleDamageInfo.request();
                req3.battleDamageInfo= new Dictionary<long, BattleDamageInfo>();
                BattleDamageInfo battleDamageInfo2= new BattleDamageInfo();       
                battleDamageInfo2.objectIndex = SecondBarbarianID;
                battleDamageInfo2.damage = attack1 + attack2;
                req3.battleDamageInfo.Add(battleDamageInfo2.objectIndex,battleDamageInfo2);
                AppFacade.GetInstance().SendNotification(Battle_BattleDamageInfo.TagName, req3);

                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian2);
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop1);
                AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop2);


                if(m_secondBarbarian.mapObjectInfo.armyCount>0)
                {
                    Timer.Register(1f, ContinuteFightTogether);

                    Timer.Register(0.2f, () => {
                        if (m_isDispose)
                        {
                            return;
                        }

                        //增加怒气
                        m_firstTroop.mapObjectInfo.sp = m_firstTroop.mapObjectInfo.sp + 33;
                        if (m_firstTroop.mapObjectInfo.sp >= 100)
                        {
                            m_firstTroop.mapObjectInfo.sp = 100;
                        }
                        Map_ObjectInfo.request addTroop1 = new Map_ObjectInfo.request();
                        addTroop1.mapObjectInfo = new MapObjectInfo();
                        addTroop1.mapObjectInfo.objectId = FirstTroopID;
                        addTroop1.mapObjectInfo.sp = m_firstTroop.mapObjectInfo.sp;
                        AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, addTroop1);

                        //增加部队2的怒气
                        m_secondTroop.mapObjectInfo.sp = m_secondTroop.mapObjectInfo.sp + 20;
                        if (m_secondTroop.mapObjectInfo.sp >= 100)
                        {
                            m_secondTroop.mapObjectInfo.sp = 100;
                        }
                        Map_ObjectInfo.request addTroop2 = new Map_ObjectInfo.request();
                        addTroop2.mapObjectInfo = new MapObjectInfo();
                        addTroop2.mapObjectInfo.objectId = SecondTroopID;
                        addTroop2.mapObjectInfo.sp = m_firstTroop.mapObjectInfo.sp;
                        AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, addTroop2);
                    });
                }
                else
                {
                    m_finalFightEndCallback?.Invoke();
                    m_stopAllFightSingal = true;

                    //待机
                    Map_ObjectInfo.request barbarian3 = new Map_ObjectInfo.request();
                    barbarian3.mapObjectInfo = new MapObjectInfo();
                    barbarian3.mapObjectInfo.objectId = SecondBarbarianID;
                    barbarian3.mapObjectInfo.status = (long)ArmyStatus.ARMY_SATNBY;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian3);

                    //溃败    
                    barbarian2 = new Map_ObjectInfo.request();
                    barbarian2.mapObjectInfo = new MapObjectInfo();
                    barbarian2.mapObjectInfo.objectId = SecondBarbarianID;
                    barbarian2.mapObjectInfo.status = (long)ArmyStatus.MONSTER_FAILED;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, barbarian2);

                    Map_ObjectDelete.request deleteReq = new Map_ObjectDelete.request();
                    deleteReq.objectId = m_secondBarbarian.mapObjectInfo.objectId;                 
                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq);
                    m_secondBarbarian.mapObjectInfo.objectId = 0;

                    Map_ObjectInfo.request troop1_1 = new Map_ObjectInfo.request();
                    troop1_1.mapObjectInfo = new MapObjectInfo();
                    troop1_1.mapObjectInfo.objectId = FirstTroopID;
                    troop1_1.mapObjectInfo.status = (int)ArmyStatus.STATIONING;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop1_1);

                    Map_ObjectInfo.request troop2_1 = new Map_ObjectInfo.request();
                    troop2_1.mapObjectInfo = new MapObjectInfo();
                    troop2_1.mapObjectInfo.objectId = SecondTroopID;
                    troop2_1.mapObjectInfo.status = (int)ArmyStatus.STATIONING;
                    AppFacade.GetInstance().SendNotification(Map_ObjectInfo.TagName, troop2_1);

                    AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveTroopHud,(int)SecondTroopID);
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveTroopHud,(int)FirstTroopID);
                }
            }

            private void InitFakeData()
            {
                m_cityPos = new PosInfo { x = m_playerProxy.CurrentRoleInfo.pos.x, y = m_playerProxy.CurrentRoleInfo.pos.y };
                m_firstTroopStartPos = new PosInfo { x = m_firstTroopStartPos.x + m_cityPos.x, y = m_firstTroopStartPos.y + m_cityPos.y };
                m_firstBarbarianPos = new PosInfo { x = m_firstBarbarianPos.x + m_cityPos.x, y = m_firstBarbarianPos.y + m_cityPos.y };
                m_firstTroopFightPos = new PosInfo { x = m_firstTroopFightPos.x + m_cityPos.x, y = m_firstTroopFightPos.y + m_cityPos.y };
                m_firstTroopFinalFightPos = new PosInfo { x = m_firstTroopFinalFightPos.x + m_cityPos.x, y = m_firstTroopFinalFightPos.y + m_cityPos.y };
                m_returnPos = new PosInfo { x = m_returnPos.x + m_cityPos.x, y = m_returnPos.y + m_cityPos.y };
                m_secondBarbarianFirstPos = new PosInfo { x = m_secondBarbarianFirstPos.x + m_cityPos.x, y = m_secondBarbarianFirstPos.y + m_cityPos.y };
                m_secondTroopFirstPos = new PosInfo { x = m_secondTroopFirstPos.x + m_cityPos.x, y = m_secondTroopFirstPos.y + m_cityPos.y };
                m_secondBarbarianSecondPos = new PosInfo { x = m_secondBarbarianSecondPos.x + m_cityPos.x, y = m_secondBarbarianSecondPos.y + m_cityPos.y };
                m_secondTroopSecondPos = new PosInfo { x = m_secondTroopSecondPos.x + m_cityPos.x, y = m_secondTroopSecondPos.y + m_cityPos.y };
                m_secondTroopThirdPos = new PosInfo { x = m_secondTroopThirdPos.x + m_cityPos.x, y = m_secondTroopThirdPos.y + m_cityPos.y };
                m_secondCameraPos = new PosInfo { x = m_secondCameraPos.x + m_cityPos.x, y = m_secondCameraPos.y + m_cityPos.y };
                InitFirstFightData();
                InitSecondFight();
            }

            private void InitFirstFightData()
            {
                m_firstBarbarian = new Map_ObjectInfo.request();
                m_firstBarbarian.mapObjectInfo = new MapObjectInfo();
                m_firstBarbarian.mapObjectInfo.objectType = 2;
                m_firstBarbarian.mapObjectInfo.objectId = FirstBarbarianID;
                m_firstBarbarian.mapObjectInfo.objectPos = m_firstBarbarianPos;
                m_firstBarbarian.mapObjectInfo.objectPath = new List<PosInfo>();
                m_firstBarbarian.mapObjectInfo.objectPath.Add(m_firstBarbarianPos);
                PosInfo tmpPos = new PosInfo {x = m_firstBarbarianPos.x+50,y = m_firstBarbarianPos.y-50 };
                m_firstBarbarian.mapObjectInfo.objectPath.Add(tmpPos);
                m_firstBarbarian.mapObjectInfo.troopsCapacity = 1000000;
                m_firstBarbarian.mapObjectInfo.status = (long)ArmyStatus.STATIONING;
                m_firstBarbarian.mapObjectInfo.mainHeroId = m_firstBarbarianMainHeroID;
                m_firstBarbarian.mapObjectInfo.armyCount = m_firstBarbarianSoldierNum;
                m_firstBarbarian.mapObjectInfo.armyCountMax = m_firstBarbarianSoldierNum;
                m_firstBarbarian.mapObjectInfo.objectPower = m_firstBarbarianArmyPower;
                m_firstBarbarian.mapObjectInfo.monsterId = m_firstBarbarianID;
                m_firstBarbarian.mapObjectInfo.refreshTime = ServerTimeModule.Instance.GetServerTime();
                m_firstBarbarian.mapObjectInfo.sp = 0;
                m_firstBarbarian.mapObjectInfo.maxSp = 100;

                m_firstTroop = new Map_ObjectInfo.request();
                m_firstTroop.mapObjectInfo = new MapObjectInfo();
                m_firstTroop.mapObjectInfo.objectType = 1;
                m_firstTroop.mapObjectInfo.objectId = FirstTroopID;
                m_firstTroop.mapObjectInfo.objectPos = m_firstTroopStartPos;
                m_firstTroop.mapObjectInfo.objectPath = new List<PosInfo>();
                m_firstTroop.mapObjectInfo.objectPath.Add(m_firstTroopStartPos);
                m_firstTroop.mapObjectInfo.objectPath.Add(m_firstTroopFightPos);
                m_firstTroop.mapObjectInfo.troopsCapacity = 1000000;
                m_firstTroop.mapObjectInfo.armyRid = m_playerProxy.CurrentRoleInfo.rid;
                m_firstTroop.mapObjectInfo.armyLevel = 1;
                m_firstTroop.mapObjectInfo.sp = 0;
                m_firstTroop.mapObjectInfo.maxSp = 100;
                List<SkillInfo> skillList = new List<SkillInfo>();
                SkillInfo skillInfo = new SkillInfo();
                skillInfo.skillId = m_fightMainHeroSkillId;
                skillInfo.skillLevel = 1;
                skillList.Add(skillInfo);
                m_firstTroop.mapObjectInfo.mainHeroSkills = skillList;

                m_firstTroop.mapObjectInfo.status = (long)ArmyStatus.ATTACK_MARCH;//向空地行军
                m_firstTroop.mapObjectInfo.armyIndex = -1;
                m_firstTroop.mapObjectInfo.mainHeroId = m_firstTroopMainHeroID;
                m_firstTroop.mapObjectInfo.soldiers = new Dictionary<long, SoldierInfo>();
                m_firstTroop.mapObjectInfo.soldiers.Add(m_firstSoldierID, new SoldierInfo { id = m_firstSoldierID, type = m_firstSoldierType, num = m_firstSoldierNum, level = m_firstSoldierLevel });
                m_firstTroop.mapObjectInfo.targetObjectIndex = m_firstBarbarian.mapObjectInfo.objectId;
                m_firstTroop.mapObjectInfo.armyCount = m_firstSoldierNum;
                m_firstTroop.mapObjectInfo.armyCountMax = m_firstSoldierNum;
                m_firstTroop.mapObjectInfo.objectPower = m_firstArmyPower;
                FirstTroop = m_firstTroop;
            }
         
            private void InitSecondFight()
            {
                m_secondBarbarian = new Map_ObjectInfo.request();
                m_secondBarbarian.mapObjectInfo = new MapObjectInfo();
                m_secondBarbarian.mapObjectInfo.objectType = 2;
                m_secondBarbarian.mapObjectInfo.isGuide = true;
                m_secondBarbarian.mapObjectInfo.objectId = SecondBarbarianID;
                m_secondBarbarian.mapObjectInfo.objectPos = m_secondBarbarianFirstPos;
                m_secondBarbarian.mapObjectInfo.objectPath = new List<PosInfo>();
                m_secondBarbarian.mapObjectInfo.objectPath.Add(m_secondBarbarian.mapObjectInfo.objectPos);

                m_secondBarbarian.mapObjectInfo.objectPath.Add(m_secondBarbarianSecondPos);
                m_secondBarbarian.mapObjectInfo.troopsCapacity = 1000000;
                m_secondBarbarian.mapObjectInfo.status = (long)ArmyStatus.SPACE_MARCH;
                m_secondBarbarian.mapObjectInfo.mainHeroId = m_secondBarbarianMainHeroID;
                m_secondBarbarian.mapObjectInfo.armyCount = m_secondBarbarianSoldierNum;
                m_secondBarbarian.mapObjectInfo.armyCountMax = m_secondBarbarianSoldierNum;
                m_secondBarbarian.mapObjectInfo.objectPower = m_secondBarbarianArmyPower;
                m_secondBarbarian.mapObjectInfo.monsterId = m_secondBarbarianID;
                m_secondBarbarian.mapObjectInfo.refreshTime = ServerTimeModule.Instance.GetServerTime();
                m_secondBarbarian.mapObjectInfo.sp = 0;
                m_secondBarbarian.mapObjectInfo.maxSp = 100;

                m_secondTroop = new Map_ObjectInfo.request();
                m_secondTroop.mapObjectInfo = new MapObjectInfo();
                m_secondTroop.mapObjectInfo.objectType = 1;
                m_secondTroop.mapObjectInfo.objectId = SecondTroopID;
                m_secondTroop.mapObjectInfo.objectPos = m_secondTroopFirstPos;
                m_secondTroop.mapObjectInfo.objectPath = new List<PosInfo>();
                m_secondTroop.mapObjectInfo.objectPath.Add(m_secondTroopFirstPos);
                m_secondTroop.mapObjectInfo.objectPath.Add(m_secondTroopSecondPos);
                m_secondTroop.mapObjectInfo.troopsCapacity = 1000000;
                m_secondTroop.mapObjectInfo.armyIndex = -2;
                m_secondTroop.mapObjectInfo.armyLevel = 1;
                m_secondTroop.mapObjectInfo.status = (long)ArmyStatus.SPACE_MARCH;//向空地行军
                m_secondTroop.mapObjectInfo.mainHeroId = m_secondTroopMainHeroID;
                m_secondTroop.mapObjectInfo.soldiers = new Dictionary<long, SoldierInfo>();
                m_secondTroop.mapObjectInfo.soldiers.Add(m_secondSoldierID, new SoldierInfo { id = m_secondSoldierID, type = m_secondSoldierType, num = m_secondSoldierNum, level = m_secondSoldierLevel });
                m_secondTroop.mapObjectInfo.armyCount = m_secondSoldierNum;
                m_secondTroop.mapObjectInfo.armyCountMax = m_secondSoldierNum;
                m_secondTroop.mapObjectInfo.objectPower = m_secondArmyPower;
                m_secondTroop.mapObjectInfo.targetObjectIndex = m_secondBarbarian.mapObjectInfo.objectId;
                m_secondTroop.mapObjectInfo.sp = 0;
                m_secondTroop.mapObjectInfo.maxSp = 100;
                List<SkillInfo> skillList = new List<SkillInfo>();
                SkillInfo skillInfo = new SkillInfo();
                skillInfo.skillId = m_secondTroopMainHeroID; ;
                skillInfo.skillLevel = 1;
                skillList.Add(skillInfo);
                m_secondTroop.mapObjectInfo.mainHeroSkills = skillList;

            }

            public void ClearData()
            {
                m_isDispose = true;

                if(m_firstTroop!=null&&m_firstTroop.mapObjectInfo.objectId>0)
                {
                    Map_ObjectDelete.request deleteReq = new Map_ObjectDelete.request();
                    deleteReq.objectId = m_firstTroop.mapObjectInfo.objectId;
                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq);
                }
                if (m_secondTroop != null && m_secondTroop.mapObjectInfo.objectId > 0)
                {
                    Map_ObjectDelete.request deleteReq = new Map_ObjectDelete.request();
                    deleteReq.objectId = m_secondTroop.mapObjectInfo.objectId;
                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq);
                }
                if (m_firstBarbarian != null && m_firstBarbarian.mapObjectInfo.objectId > 0)
                {
                    Map_ObjectDelete.request deleteReq = new Map_ObjectDelete.request();
                    deleteReq.objectId = m_firstBarbarian.mapObjectInfo.objectId;
                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq);
                }
                if (m_secondBarbarian != null && m_secondBarbarian.mapObjectInfo.objectId > 0)
                {
                    Map_ObjectDelete.request deleteReq = new Map_ObjectDelete.request();
                    deleteReq.objectId = m_secondBarbarian.mapObjectInfo.objectId;
                    AppFacade.GetInstance().SendNotification(Map_ObjectDelete.TagName, deleteReq);
                }
            }
        }

        #region Tools
        public static Vector2 FloatToV2(float[] tmp)
        {
            Vector2 v2 = new Vector2();
            try
            {

                v2.x = tmp[0];
                v2.y = tmp[1];
            }
            catch
            {
                Debug.LogError("数组数据错误，无法进行npc对话");
            }
            return v2;
        }

        public static Vector2[] FloatToV2Array(float[][] tmp)
        {
            Vector2[] v2Array = new Vector2[tmp.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                v2Array[i] = FloatToV2(tmp[i]);
            }
            return v2Array;
        }

        public static Vector3 FloatToV3(float[] tmp)
        {
            Vector3 v3 = new Vector3();
            try
            {
                v3.x = tmp[0];
                v3.y = tmp[1];
                v3.z = tmp[2];
            }
            catch
            {
                Debug.LogError("数组数据错误，无法进行npc对话");
            }
            return v3;
        }

        #endregion

    }

}



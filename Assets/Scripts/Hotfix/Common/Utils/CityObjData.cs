using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Skyunion;
using UnityEngine.Events;
using DG.Tweening;
using System;
using UnityEngine.UI;
using System.Text;
using SprotoType;
using Client.FSM;
using Data;
using System.Linq;

namespace Game
{
    public enum CityState
    {
        RES_LOADING = 1,
        RES_LOADED = 2,
        DISCARD = 3,
    }

    public enum BuildingState
    {
        NONE = 0,
        PRELOAD = 1,
        LOADING = 2,
        LOADED = 3,
    }
    public enum FireState
    {
        FIRED = 1,

        NONE = 0,

    }

    /// <summary>
    /// 城市
    /// </summary>
    public class CityObjData//TODO:能整合到这个的要尽量整合
    {
        //一个小格子的单位
        public static float CITY_GRID_SIZE = 0.1F;

        //半个格单位
        public static float CITY_GRID_SIZE_HALF = 0.05F;


        //格子常态
        public static int CITY_GRID_STATE_NORMAL = 5;

        //相互的
        public static int CITY_GRID_SIZE_RECIPROCAL = 10;

        //保留的
        public static int CITY_GRID_RESERVED_OFFSET = 7;

        public static int CITY_GRID_BODEDER = 8;


        //建筑类型
        public static int CITY_BUILDING_TYPE_OFFSET = 100;
        public static Vector2[] SIZE_UPGRADEBOARD = new[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(-0.08f, -0.08f), new Vector2(0, 0), new Vector2(-0.18f, -0.18f), new Vector2(-0.225f, -0.225f), new Vector2(-0.275f, -0.275f), new Vector2(0, 0), new Vector2(-0.375f, -0.375f) };
        public static Vector3[] SIZE_TITLE = new[] { new Vector3(0,0, 0), new Vector3(0,0, 0), new Vector3(0, 0,0), new Vector3(0, 0,0), new Vector3(0,0.15f, 0), new Vector3(0,0.2f,0), new Vector3(0,0.4f, 0), new Vector3(0,0, 0), new Vector3(0,0.45f, 0) };
        public static Vector3[] SIZE_BUTTON = new[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0.1f, 0), new Vector3(0, 0, 0), new Vector3(0, 0.25f, 0), new Vector3(0, 0.25f, 0), new Vector3(0, 0.25f, 0), new Vector3(0, 0, 0), new Vector3(0, 0.15f, 0) };

        public GameObject go;
        public Dictionary<long, BuldingObjData> buildingList = new Dictionary<long, BuldingObjData>();
        public string modelObjId;
        public GameObject townCenter;
        public Dictionary<EnumCityBuildingType, Dictionary<long, BuldingObjData>> buildingListByType = new Dictionary<EnumCityBuildingType, Dictionary<long, BuldingObjData>>();
        public bool buildingListValid;
        public CityState state;
        public BuildingState buildingState;
        public Vector2 pos;//服务器下发的位置/100
        public int lv;
        public long rid;
        public TownSearch cityMapFinder;
        public Color cityColor;
        public Color troopColor;
        public FireState fireState;
        public FireState fireobjState;
        public bool lostHp;
        public bool assetLoading;
        public bool fireLoading;
        public bool CreateBuilfinginfo;//在城市没有生成玩的时候，收到了城市建筑，等城市创建完成，生成建筑
        public PatrolSoldier cityWallPatrolSoldierDummy;
        public TownCenterFloor m_buildGroundHelper;
        public Dictionary<long, BuildingInfoEntity> BuildingInfoEntityDic = new Dictionary<long, BuildingInfoEntity>();
        public MapObjectInfoEntity mapObjectExtEntity;
        public EnumAgeType AgeType;
        public CityBuildingProxy m_cityBuildingProxy;
        public string ProvinceName;//省份名
        public Transform transform_effect;//特效位置
        public Transform transform_buffEffect;//buff特效位置
        public bool incity;
        //城墙上巡逻士兵个数
        private static int PARTOL_SOLDIER_COUNT = 7;
        private List<PeopleFSM> m_soldiers = new List<PeopleFSM>(PARTOL_SOLDIER_COUNT);
        public List<WorkerFMS> m_workers = new List<WorkerFMS>();
        //TODO//其他人城市的逻辑和自己城市的最好是统一在一起
        private Dictionary<string, TrainSoldiersFMS> m_trains = new Dictionary<string, TrainSoldiersFMS>();
        //X轴上下，Y轴左右  
        private Vector2[][] m_trainSoldierPos = new[]
        {
            new[] //兵营 右 中 左
            {
                new Vector2(1.5f, -0.5f), new Vector2(0f, -1f), new Vector2(0.5f, 0.5f) //, new Vector2(1, 3)
            },
            new[] //马厩
            {
                new Vector2(1f, 2f), new Vector2(2.5f, 1.8f)
            },
            new[] //靶场
            {
                new Vector2(1.2f, 2f), new Vector2(3f, 2f) //, new Vector2(3.2f, 1.6f)
            },
            new[] //攻城武器厂
            {
                new Vector2(2.5f, 2.5f)
            }
        };

        private  Vector2[][] m_trainSoldierDirPos = new[]
        {
            new[]
            {
                new Vector2(-4f, 0f), new Vector2(3f, 3f), new Vector2(4f, 0f) //, new Vector2(0f, 1f)
            },
            new[]
            {
                new Vector2(-4f, 4f), new Vector2(-4f, 4f)
            },
            new[]
            {
                new Vector2(-4f, 4f), new Vector2(-4f, 4f) //, new Vector2(0f, 1f)
            },
            new[]
            {
                new Vector2(-4, 4)
            }
        };
        public void SetGameobject(GameObject gameObject)
        {
            this.go = gameObject;
            this.ProvinceName = MapManager.Instance().GetMapProvinceName(this.pos);
        }
        public void InitData()
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_GridState = m_cityBuildingProxy.MakeCityMapEmpty((int)AgeType);
        }

        public void CreateTrainSoldiers()
        {
            var builds = GetTrainBuilds();

            foreach (var build in builds)
            {
                var posArr = m_trainSoldierPos[build.type - 9];
                var dirArr = m_trainSoldierDirPos[build.type - 9];
                for (int i = 0; i < posArr.Length; i++)
                {
                    string sname = string.Format("TrainSoldiers_{0}_{1}", build.type, i);

                    if (!m_trains.ContainsKey(sname))
                    {
                        Vector2 off = posArr[i];
                        Vector2 dir = dirArr[i];
                        BuildingInfoEntity tempBuild = build;
                        long buildType = build.type;
                        long buildIndex = build.buildingIndex;
                        int index = i;
                        CoreUtils.assetService.Instantiate("citizen",
                            (GameObject gameObject) =>
                            {
                                if (go == null)
                                {
                                    CoreUtils.assetService.Destroy(gameObject);
                                    return;
                                }
                                if (!incity)
                                {
                                    gameObject.SetActive(false);
                                }
                                gameObject.transform.SetParent(go.transform, false);
                                gameObject.name = sname;

                                var citizen = gameObject.GetComponent<People>();

                                var arm = GetMaxUnlockSoldier((EnumCityBuildingType)build.type);

                                if (string.IsNullOrEmpty(arm.armsModel))
                                {
                                    Debug.Log("兵种模型没有配置@刘晨" + arm.armsShow);
                                }

                                CoreUtils.assetService.Instantiate(arm.armsModel,
                                    (GameObject uint_obj) =>
                                    {
                                        People.InitCitizenS(citizen, uint_obj, troopColor);
                                        People.SetUnitFootprintsActiveS(citizen, false);
                                        People.ResetUnitPosS(citizen);
                                        var fms = gameObject.AddComponent<TrainSoldiersFMS>();
                                        AnimationBase component = uint_obj.GetComponent<AnimationBase>();
                                        if (component != null)
                                        {
                                            component.ReleaseParticle();
                                        }
                                        fms.Owner = citizen;
                                        fms.name = sname;
                                      fms.Finder = cityMapFinder;

                                        fms.buildType = buildType;
                                        fms.buildIndex = buildIndex;
                                        fms.index = index;

                                       fms.WaitFor(new Vector2(tempBuild.pos.x, tempBuild.pos.y), off, dir);
                                     if(!m_trains.ContainsKey(sname))
                                        m_trains.Add(sname, fms);

                                    });
                            });
                    }

                }
            }
        }

        /// <summary>
        /// 士兵逃跑
        /// </summary>
        public void CreateRunCitizen()
        {
            Dictionary<long, BuldingObjData> buldingObjDatas = null;
            if (buildingListByType.TryGetValue(EnumCityBuildingType.TownCenter, out buldingObjDatas))
            {
                var etInfo = buldingObjDatas.Values.ToList()[0];

                List<Vector2> stPaths = GetBuildingFinderPosition(etInfo.buildingInfoEntity.buildingIndex);
                if (timeFIREFIGHTING != null)
                {
                    timeFIREFIGHTING.Cancel();
                    timeFIREFIGHTING = null;
                }
                foreach (var work in m_workers)
                {
                    work.RunToCitizen(stPaths[5]);
                }
                timeRunCitizen = Timer.Register(1, () =>
                {
                    bool home = true;//所有的工人都躲进市政厅了
                    foreach (var m_work in m_workers)
                    {
                        if (!m_work.IsEndStep())
                        {
                            home = false;
                        }
                    }
                    if (home)
                    {
                        if (timeRunCitizen != null)
                        {
                            timeRunCitizen.Cancel();
                            timeRunCitizen = null;
                            CreateFIREFIGHTINGCitizen();
                        }
                    }
                }, null, true, true);
            }


        }
        bool alldone = false;
        Timer timeRunCitizen;
        Timer timeFIREFIGHTING;
        /// <summary>
        /// 士兵救火
        /// </summary>
        public void CreateFIREFIGHTINGCitizen()
        {
            List<BuldingObjData> allList = buildingList.Values.ToList();
            if (mapObjectExtEntity == null || this == null)
            {
                return;
            }
            Dictionary<long, BuldingObjData> buldingObjDatas = null;
            if (buildingListByType.TryGetValue(EnumCityBuildingType.TownCenter, out buldingObjDatas))
            {
                var resBuildInfo = buldingObjDatas.Values.ToList()[0];

                List<Vector2> stPaths = GetBuildingFinderPosition(resBuildInfo.buildingInfoEntity.buildingIndex);

                timeFIREFIGHTING = Timer.Register(1, () =>
                {

                    foreach (var m_work in m_workers)
                    {
                        if (!m_work.IsWorking())
                        {
                            for (int i = 0; i < allList.Count; i++)
                            {
                                BuldingObjData buldingObj = allList[i];
                                if (buldingObj.type == EnumCityBuildingType.CityWall || buldingObj.type == EnumCityBuildingType.GuardTower)
                                {
                                    continue;
                                }
                                if (buldingObj.fireState == FireState.FIRED && buldingObj.fireObjState == FireState.FIRED)
                                {
                                    if (buldingObj.workerNum == 5)
                                    {
                                        continue;
                                    }
                                    buldingObj.workerNum++;
                                    long buildID = buldingObj.buildingInfoEntity.buildingIndex;
                                    List<Vector2> paths = GetBuildingFinderPosition(buildID);
                                    m_work.gameObject.SetActive(true);

                                  //  var workBuildInfo = m_cityBuildingProxy.GetBuildingInfoByindex(buildID);

                                    List<People.ENMU_CARRY_RESOURCE_TYPE> resList = new List<People.ENMU_CARRY_RESOURCE_TYPE>();

                                    resList.Add(People.ENMU_CARRY_RESOURCE_TYPE.BUCKET);

                                  //  Debug.Log(m_work.index + " 建筑 派遣 分配建筑工人去灭火 " + buildID);

                                    m_work.GoGetWaterToFireBuild(stPaths, paths, buildID, resBuildInfo.buildingInfoEntity.buildingIndex, resList, FireCountCallBack);
                                    break;
                                }
                            }
                        }
                    }
                    bool alldone = true;
                    allList.ForEach((temp) => { if (temp.fireObjState == FireState.FIRED && temp.type != EnumCityBuildingType.CityWall) alldone = false; });
                    if (alldone)
                    {
                        if (timeFIREFIGHTING != null)
                        {
                            timeFIREFIGHTING.Cancel();
                            timeFIREFIGHTING = null;
                        }
                        foreach (var m_work in m_workers)
                        {
                            m_work.WalkAround();
                        }
                    }
                }, null, true, true);
            }
        }
        private void FireCountCallBack(long buildID, long workIndex)
        {
            BuldingObjData buldingObjData = null;
            buildingList.TryGetValue(buildID, out buldingObjData);
            if (buldingObjData == null)
            {
                return;
            }
            m_workers[(int)workIndex].Clear();
            buldingObjData.fireFightingNum--;
            if (buldingObjData.fireFightingNum <= 0)
            {
                buldingObjData.fireFightingNum = 0;
                buldingObjData.SetfireState(FireState.NONE);
            }
            buldingObjData.UpdateFireStateShow();

         //   Debug.LogError(workIndex + "灭火次数回调了 " + buildID + " " + buldingObjData.fireFightingNum);
        }
        public ArmsDefine GetMaxUnlockSoldier(EnumCityBuildingType type)
        {
            int armyType = 0;
            int id = 0;
            if (type == EnumCityBuildingType.Barracks)
            {
                armyType = (int)EnumSoldierType.Infantry;
                id = 10101;
            }
            else if (type == EnumCityBuildingType.Stable)
            {
                armyType = (int)EnumSoldierType.Cavalry;
                id = 10201;
            }
            else if (type == EnumCityBuildingType.ArcheryRange)
            {
                armyType = (int)EnumSoldierType.Bowmen;
                id = 10301;
            }
            else if (type == EnumCityBuildingType.SiegeWorkshop)
            {
                armyType = (int)EnumSoldierType.SiegeEngines;
                id = 10401;
            }
            ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
            return define;
        }
        public List<BuildingInfoEntity> GetTrainBuilds() 
        {
            List<BuildingInfoEntity> builds = new List<BuildingInfoEntity>();
            foreach (var value in buildingList.Values)
            {
                if (value.type == EnumCityBuildingType.Barracks|| value.type == EnumCityBuildingType.Stable|| value.type == EnumCityBuildingType.ArcheryRange|| value.type == EnumCityBuildingType.SiegeWorkshop)
                {
                    builds.Add(value.buildingInfoEntity);
                }
            }

            return builds;
        }
        private int[] m_GridState;
        public void StopBuilder(bool needCreate = false)
        {
            incity = false;
      //      Debug.LogError(rid);
            if (m_workers.Count > 0)
            {
                foreach (var worker in m_workers)
                {
                    if (needCreate)
                    {
                        CoreUtils.assetService.Destroy(worker.gameObject);
                    }
                    else
                    {
                        worker.gameObject.SetActive(false);
                    }
                }

                foreach (var soldier in m_soldiers)
                {
                    soldier.gameObject.SetActive(false);
                }

                foreach (var train in m_trains)
                {
                    train.Value.gameObject.SetActive(false);
                }
           
            }
            if (needCreate)
            {
                m_workers.Clear();
            }
            else
            {
                
            }
            foreach (var building in buildingList.Values)
            {
                if (building.type > EnumCityBuildingType.Road)
                {
                    if (building.gameObject != null)
                    {
                        building.gameObject.SetActive(false);
                    }
                }
            }
        }
        public void StartBuilder(bool needCreate = true)
        {
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_GridState = m_cityBuildingProxy.MakeCityMapEmpty((int)AgeType);
            if (cityMapFinder == null )
            {
                return;
            }
            if (m_workers.Count > 0)
            {
                if (!incity)
                {
                    return;
                }
                foreach (var worker in m_workers)
                {
                    worker.gameObject.SetActive(true);
                }

                foreach (var soldier in m_soldiers)
                {
                    soldier.gameObject.SetActive(true);
                }
                foreach (var train in m_trains)
                {
                    train.Value.gameObject.SetActive(true);
                }
            }
            if (m_workers.Count == 0 )
            {
                   CreateWallSoldiers();

                CreateTrainSoldiers();

                CreateCityWorker();

            }

            foreach (var building in buildingList.Values)
            {
                if (building.type > EnumCityBuildingType.Road)
                {
                    if (building.gameObject != null)
                    {
                        building.gameObject.SetActive(true);
                    }
                }

            }
        }
        public void OnRemove()
        {
            CoreUtils.assetService.Destroy(go);
            if (timeFIREFIGHTING != null)
            {
                timeFIREFIGHTING.Cancel();
                timeFIREFIGHTING = null;
            }
            if (timeRunCitizen != null)
            {
                timeRunCitizen.Cancel();
                timeRunCitizen = null;
            }
        }
        private void CreateCityWorker()
        {

            CityAgeSizeDefine cityAgeSizeDefine = m_cityBuildingProxy.GetCityAgeSizeDefineByAge(AgeType);

            int builderCount = cityAgeSizeDefine.BuilderFemaleNum + cityAgeSizeDefine.BuilderMaleNum;


            cityMapFinder.SetFinderMap(m_GridState, cityAgeSizeDefine.size, go.transform);

            if (builderCount == m_workers.Count)
            {
                return;
            }
            var config = CoreUtils.dataService.QueryRecords<ConfigDefine>()[0];
            for (int i = 0; i < builderCount; i++)
            {
                var index = i;

                CoreUtils.assetService.Instantiate("citizen",
                    (GameObject gameObject) =>
                    {
                        if (!incity)
                        {
                            gameObject.SetActive(false);
                        }
                        //                        Vector2 slocal = m_cityMapFinder.GetRandPos();//GetRandPosLogic();
                        //                        Vector2 elocal = m_cityMapFinder.GetRandPos();//GetRandPosLogic();
                        gameObject.transform.SetParent(go.transform, false);
                        gameObject.name = "builder_" + index;

                        //                        gameObject.transform.localPosition = ConvertCityObjTileToLocal(slocal.x, slocal.y);

                        var citizen = gameObject.GetComponent<People>();
                        string name = config.builderMaleModel;

                        if (index >= cityAgeSizeDefine.BuilderMaleNum)
                        {
                            name = config.builderFemaleModel;
                        }

                        CoreUtils.assetService.Instantiate(name,
                            (GameObject uint_obj) =>
                            {
                                People.InitCitizenS(citizen, uint_obj, Color.white);
                                People.SetUnitFootprintsActiveS(citizen, true);
                                People.ResetUnitPosS(citizen);

                                var fms = gameObject.AddComponent<WorkerFMS>();

                                fms.Finder = cityMapFinder;
                                fms.Owner = citizen;
                                fms.name = gameObject.name;


                                fms.index = index;


                                fms.SetBorn(cityMapFinder.GetRandPos());
                                fms.WalkAround();

                                m_workers.Add(fms);

                                //                                fms.WorldPaths = m_cityMapFinder.GetFindWorldPaths( slocal, elocal);

                                //if (m_workers.Count == builderCount)
                                //{

                                //    Timer.Register(0.1f, () => { WorkAssign(); });

                                //}

                            });
                    });
            }
        }
        private void CreateWallSoldiers()
        {
            for (int i = 1; i <= PARTOL_SOLDIER_COUNT; i++)
            {
                var index = i;
                Vector2[] sp = null;
                var p = Vector3.zero;
                if (cityWallPatrolSoldierDummy != null)
                {
                    sp = cityWallPatrolSoldierDummy.GetSoldierPath(index);
                    p = cityWallPatrolSoldierDummy.GetSoldierPos(index);
                }
                CoreUtils.assetService.Instantiate("citizen",
                    (GameObject gameObject) =>
                    {
                        if (go == null)
                        {
                            CoreUtils.assetService.Destroy(gameObject);
                            return;
                        }
                        gameObject.transform.SetParent(go.transform, false);
                        gameObject.transform.localPosition = p;
                        gameObject.name = "patrol_" + index;

                        var citizen = gameObject.GetComponent<People>();
                        ArmsDefine define = CoreUtils.dataService.QueryRecord<ArmsDefine>(10302);
                        string namepefab = define.armsModel;
                        CoreUtils.assetService.Instantiate(namepefab,
                            (GameObject uint_obj) =>
                            {
                                if (gameObject == null || citizen==null ||uint_obj==null )
                                {
                                    Debug.LogError("找不到城墙上的兵" + namepefab);
                                    return;
                                }

                                People.InitCitizenS(citizen, uint_obj, troopColor);
                                People.SetUnitFootprintsActiveS(citizen, false);
                                People.ResetUnitPosS(citizen);
                                if (sp!=null &&sp[0] != null)
                                {
                                         var startPos = MakeWorldPosFormLocal(sp[0].x, sp[0].y);
                                          var endPos = MakeWorldPosFormLocal(sp[1].x, sp[1].y);

                                    citizen.WorldPaths.Add(startPos);
                                    citizen.WorldPaths.Add(endPos);
                                    var fsm = gameObject.AddComponent<PeopleFSM>();
                                    fsm.Owner = citizen;

                                    m_soldiers.Add(fsm);
                                }

                            });
                    });
            }
        }
        public Vector2 MakeWorldPosFormLocal(float x, float y)
        {
            var p = new Vector3(x, 0, y);
            var l = go.transform.TransformPoint(p);
            return new Vector2(l.x, l.z);
        }
        /// <summary>
        /// 建造周围点
        /// </summary>
        /// <param name="buildIndex"></param>
        /// <returns></returns>
        public List<Vector2> GetBuildingFinderPosition(long buildIndex)
        {
            List<Vector2> poss = new List<Vector2>();
            var build = buildingList[buildIndex];
            var buildEntiy = build.buildingInfoEntity;
            var buildConfig = m_cityBuildingProxy.GetBuildConfig((EnumCityBuildingType)buildEntiy.type);

            Vector2 p = new Vector2(buildEntiy.pos.x, buildEntiy.pos.y);

            int w = buildConfig.width;
            int l = buildConfig.length;


            int posX = (int)buildEntiy.pos.x - 1;
            int posY = (int)buildEntiy.pos.y - 1;

            int posXExt = posX + w + 1;
            int posYExt = posY + l + 1;


            for (int x = posX; x < posXExt; x++)
            {
                if (!CheckPosIsUesed(x, posY))
                {
                    poss.Add(new Vector2(x, posY));
                }
            }

            for (int y = posY; y < posYExt; y++)
            {
                if (!CheckPosIsUesed(posXExt, y))
                {
                    poss.Add(new Vector2(posXExt, y));
                }
            }

            for (int x = posXExt; x > posX; x--)
            {
                if (!CheckPosIsUesed(x, posYExt))
                {
                    poss.Add(new Vector2(x, posYExt));
                }
            }

            for (int y = posYExt; y > posY; y--)
            {
                if (!CheckPosIsUesed(posX, y))
                {
                    poss.Add(new Vector2(posX, y));
                }
            }

            //            Debug.Log(posX+" 建筑周围点 "+posY+"  "+posXExt+"   "+posYExt+poss[0]);

            //            foreach (var pp in poss)
            //            {
            //                Debug.Log(pp);
            //            }

            return poss;
        }
        private bool CheckPosIsUesed(int x, int y)
        {
            int index = MakeCityGridKey(x, y);
            if (index > m_GridState.Length - 1 || index < 0 || m_GridState[index] != CITY_GRID_STATE_NORMAL)
            {
                return true;
            }

            return false;
        }
        private int MakeCityGridKey(int x, int y)
        {
            int size = m_cityBuildingProxy.GetCitySize(lv);
            float a = Mathf.Ceil((size - 1f) * 0.5f);
            if (x > a || x < -a || y > a || y < -a)
            {
                return -1;
            }

            float line = y + a;
            float col = x + a + 1f;
            float key = line * size + col;
            return Mathf.FloorToInt(key + 0.5f);
        }
        //private void clearBuilder()
        //{
        //    if (m_workers.Count > 0)
        //    {
        //        foreach (var worker in m_workers)
        //        {

        //            CoreUtils.assetService.Destroy(worker.gameObject);
        //        }
        //        m_workers.Clear();

        //        foreach (var soldier in m_soldiers)
        //        {
        //            CoreUtils.assetService.Destroy(soldier.gameObject);
        //        }

        //        m_soldiers.Clear();

        //        m_initedBuilders = false;

        //        //                foreach (var train in m_trains)
        //        //                {
        //        //                    CoreUtils.assetService.Destroy(train.Value.gameObject);
        //        //                }
        //        //                
        //        //                m_trains.Clear();
        //    }
        //}

        public void SetTownCenter(GameObject gameObject)
        {
            this.townCenter = gameObject;
            TownBuilding cityBuildingHelper = gameObject.GetComponentInChildren<TownBuilding>();
            if(cityBuildingHelper!=null)
                cityBuildingHelper.SetColor(cityColor);
        }

        public void AddBuidlingByType(BuldingObjData ObjData)
        {
            if (!buildingListByType.ContainsKey(ObjData.type))
            {
                buildingListByType[ObjData.type] = new Dictionary<long, BuldingObjData>();
            }
            buildingListByType[ObjData.type][ObjData.index] = ObjData;
        }
        public static GameObject GeBuildTipTargetGameObject(long index)
        {
            CityBuildingProxy m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            BuldingObjData buldingObjData = m_cityBuildingProxy.GetBuldingObjDataByIndex(index);
            if (buldingObjData != null)
            {
                if (buldingObjData.type != EnumCityBuildingType.GuardTower && buldingObjData.type < EnumCityBuildingType.Road)
                {
                    if (buldingObjData.transform_tip != null)
                    {
                        return buldingObjData.transform_tip.gameObject;
                    }
                }
            }
            else
            {
              return   m_cityBuildingProxy.GetBuildObjByID(index);
            }
            return null;
        }
        public static GameObject GetMenuTargetGameObject(long index, bool lastFollow = true)
        {
            CityBuildingProxy m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            BuldingObjData buldingObjData = m_cityBuildingProxy.GetBuldingObjDataByIndex(index);
            if (buldingObjData != null)
            {
                if (buldingObjData.type != EnumCityBuildingType.GuardTower )
                {
                    if (buldingObjData.transform_button != null)
                    {
                        return buldingObjData.transform_button.gameObject;
                    }

                }
                else if (buldingObjData.type == EnumCityBuildingType.GuardTower)
                {
                    string str = m_cityBuildingProxy.FollowGameObject;
                    if (!string.IsNullOrEmpty(str)&& lastFollow)
                    {
                        string[] strArr = str.Split('|');
                        if (buldingObjData.gameObject.name == strArr[0])
                        {
                            if (buldingObjData.cityBuildingHelper != null)
                            {
                                if (strArr.Length == 1)
                                {
                                        return buldingObjData.transform_button.gameObject;
                                }
                                else if (strArr.Length == 2)
                                {
                                    foreach (var c in buldingObjData.transform_buttonList)
                                    {
                                        if (c.name == strArr[1])
                                        {
                                            return c.gameObject;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            return buldingObjData.transform_button.gameObject;
                        }
                    }
                    else
                    {
                        if (buldingObjData.transform_buttonList.Count > 0)
                        {
                            return buldingObjData.transform_button.gameObject;
                        }
                    }
                }
            }
            else
            {
                return m_cityBuildingProxy.GetBuildObjByID(index);
            }
            return null;
        }
        public static GameObject GetMenuTitleTargetGameObject(long index, bool lastFollow = true)
        {
            CityBuildingProxy m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            BuldingObjData buldingObjData = m_cityBuildingProxy.GetBuldingObjDataByIndex(index);
            if (buldingObjData != null)
            {
                if (buldingObjData.type != EnumCityBuildingType.GuardTower&& buldingObjData.type != EnumCityBuildingType.CityWall)
                {
                    if (buldingObjData.transform_title != null)
                    {
                        return buldingObjData.transform_title.gameObject;
                    }
                }
            }
            else
            {
                return m_cityBuildingProxy.GetBuildObjByID(index);
            }
            return null;
        }

        public void AddBuilding(BuldingObjData ObjData)
        {
            buildingList[ObjData.index] = ObjData;
            AddBuidlingByType(ObjData);
            ObjData.SetCityObj(this);
        }

        public BuldingObjData GetBuldingObjData(long buildingindex)
        {
            BuldingObjData buldingObjData = null;
             buildingList.TryGetValue(buildingindex, out buldingObjData);
            return buldingObjData;
        }
    }

    /// <summary>
    /// 城市建筑占坑
    /// </summary>
    public class BuldingObjData
    {
        public long rid;//玩家Rid
        public long index;
        public string name;
        public string modelObjId;//模型id
        public string curmodelId;//模型id
        public int level;
        public int width;
        public int length;
        public bool isSelected;
        public GameObject gameObject;
        public BoxCollider[] boxColliders;
        public bool isGameObjectLoading;//建造正在加载
        public bool isFireLoading;//火焰正在加载
        public bool enableClick;
        public CityObjData cityObj;
        public HUDUI hudui;
        public bool newPosValid;
        public string selectedCollider;
        public  List<GameObject> fireObjs;//火焰
        public FireState fireObjState;
        public long autoFireFightingTime;//火焰时间
        public long fireFightingNum = 5;//火焰粒子数量
        public long fireObjFightingNum = 5;//火焰粒子数量
        public long workerNum = 0;//派往的工人数量
        public int roadDirCode;//道路
        public List<Transform> t_effects = new List<Transform>();//火焰挂载点
        public Vector3 LocalPos;
        public PosInfo PosInfo;
        public bool assetReady;
        public BuildingInfoEntity buildingInfoEntity;
        public BuildingInfo buildingInfo;
        public BuildingTypeConfigDefine buildingTypeConfigDefine;
        public BuildingLevelDataDefine buildingLevelDataDefine;
        public EnumCityBuildingType type;
        public TownBuilding cityBuildingHelper;
        public MaskSprite spriteDualHelper;
        public bool isUp;//抬起
        public FireState fireState;
        public Transform transform_shelf;//脚手架点
        public Transform transform_upgradeBoard;//升级标志
        public Transform transform_title;//用于放置建筑标题
        public Transform transform_tip;//用于放置建筑提示
        public List<Transform> transform_titleList = new List<Transform>();//用于放置建筑标题
        public Transform transform_button;//用于 放置 操作按钮
        public List<Transform> transform_buttonList = new List<Transform>();//用于 放置 操作按钮
        //ornMaterial
        //selectedMaterial



        public void SetCityObj(CityObjData cityObj)
        {
            this.cityObj = cityObj;
        }

        /// <summary>
        /// 显示火焰
        /// </summary>
        /// <param name="buldingObj"></param>
        public void UpdateFireStateShow()
        {
           // Debug.LogError( "fireObjFightingNum " + fireObjFightingNum + "fireFightingNum" + fireFightingNum + fireState);
            switch (fireState)
            {
                case FireState.FIRED:
                    {
                        if (fireObjState == FireState.NONE || (fireObjState == FireState.FIRED && fireFightingNum != fireObjFightingNum))
                        {
                            if (fireObjState == FireState.FIRED)
                            {
                                t_effects.ForEach((trans) =>
                                {
                                    if (trans.childCount > 0)
                                    {
                                        Transform fire = trans.Find("Fire");
                                        if (fire != null)
                                        {
                                            CoreUtils.assetService.Destroy(fire.gameObject);
                                        }
                                    }
                                    else
                                    {
                                    }
                                });
                            }
                            if (!isFireLoading)
                            {
                                for (int i = 0; i < t_effects.Count; i++)
                                {
                                    int j = i;
                                    Transform trans = t_effects[i];
                                    isFireLoading = true;
                                    CoreUtils.assetService.Instantiate(RS.FireName[fireFightingNum - 1], (temp) =>
                                    {
                                        if (fireState == FireState.NONE)
                                        {
                                            fireObjState = FireState.NONE;
                                            CoreUtils.assetService.Destroy(temp);
                                            return;
                                        }
                                        temp.transform.SetParent(trans);
                                        temp.name = "Fire";
                                        temp.transform.localPosition = Vector3.zero;
                                        fireObjFightingNum = fireFightingNum;
                                        if (j == t_effects.Count - 1)
                                        {
                                            fireObjState = FireState.FIRED;
                                            isFireLoading = false;
                                        }
                                    });
                                }
                            }
                        }
                        else if (fireObjState == FireState.FIRED&&! isFireLoading)
                        {
                            if (fireFightingNum == fireObjFightingNum)
                            {
                                t_effects.ForEach((trans) =>
                                {
                                    if (trans.childCount > 0)
                                    {
                                        Transform fire = trans.Find("Fire");
                                        if (fire != null)
                                            fire.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        Debug.LogError("没有生成成功对象？" + trans.name);
                                    }
                                });

                            }
                        }
                    }
                    break;
                case FireState.NONE:
                    {
                        DestroyFireObj();
                    }
                    break;
                default:
                    break;
            }
        }
        public void SetBuildingInfoEntity(BuildingInfoEntity buildingInfo)
        {
            this.buildingInfoEntity = buildingInfo;
            this.type = (EnumCityBuildingType)buildingInfo.type;
            this.level = (int)buildingInfo.level;
        }
        public void SetBuildingTypeConfig(BuildingTypeConfigDefine buildingTypeConfig)
        {
            this.buildingTypeConfigDefine = buildingTypeConfig;
            this.name = LanguageUtils.getText(buildingTypeConfig.l_nameId);
            this.width = (int)buildingTypeConfig.width;
            this.length = (int)buildingTypeConfig.length;
        }
        public void SetBuildingLevelData(BuildingLevelDataDefine buildingLevelData)
        {
            this.buildingLevelDataDefine = buildingLevelData;
        }
        public void SetGameObject(GameObject gameObject)
        {
            this.gameObject = gameObject;
            if (gameObject == null)
                return;
            this.modelObjId = curmodelId;
            this.cityBuildingHelper = gameObject.GetComponentInChildren<TownBuilding>();
            if (this.cityBuildingHelper != null)
            {
                cityBuildingHelper.SetColor((this.cityObj).cityColor);
                boxColliders = cityBuildingHelper.colliders;
                if (boxColliders.Length == 0)
                {
                    boxColliders = gameObject.transform.GetComponentsInChildren<BoxCollider>();
                }
            }
            transform_shelf = gameObject.transform.Find("shelf");
            transform_titleList.Clear();
            transform_buttonList.Clear();
            if (this.type != EnumCityBuildingType.CityWall && this.type != EnumCityBuildingType.GuardTower && this.type < EnumCityBuildingType.Road)
            {
                transform_title = ClientUtils.FindDeepChild( gameObject,"title");
                if (transform_title == null)
                {
                    Debug.LogErrorFormat("not find  {0}title", modelObjId);
                }
                else
                {
                    transform_title.localPosition = CityObjData.SIZE_TITLE[this.width - 1]; 
                }
                transform_button = ClientUtils.FindDeepChild(gameObject, "button");
                if (transform_button == null)
                {
                    Debug.LogErrorFormat("not find  {0}button", modelObjId);
                }
                else
                {
                    transform_button.localPosition = CityObjData.SIZE_BUTTON[this.width - 1]; 
                }
                if (boxColliders.Length > 0)
                {
                    transform_tip = boxColliders[0].transform;
                }
            }
            else if (this.type == EnumCityBuildingType.GuardTower)
            {
                transform_tip = null;
                transform_title = null;
                transform_button = null;
                for (int i = 0; i < boxColliders.Length; i++)
                {
                    BoxCollider boxCollider = boxColliders[i];
                    if (string.Equals(boxCollider.name, "Collider1"))
                    {
                        transform_button = boxCollider.transform;
                    }
                    transform_buttonList.Add(boxCollider.transform);
                }
            }
            else if (this.type == EnumCityBuildingType.CityWall)
            {
                if (boxColliders.Length > 0)
                {
                    transform_tip = boxColliders[0].transform;
                    transform_title = boxColliders[0].transform;
                    transform_button = boxColliders[0].transform;
                }
            }
            else if (this.type >= EnumCityBuildingType.Road)
            {
                if (boxColliders.Length > 0)
                {
                    transform_tip = boxColliders[0].transform;
                    transform_title = boxColliders[0].transform;
                    transform_button = boxColliders[0].transform;
                }
            }
            transform_upgradeBoard = gameObject.transform.Find("UpgradeBoard");
            if (transform_upgradeBoard != null)
            {
                transform_upgradeBoard.localPosition = new Vector3(CityObjData.SIZE_UPGRADEBOARD[this.width - 1].x, 0, CityObjData.SIZE_UPGRADEBOARD[this.width - 1].y);
                if (transform_upgradeBoard.gameObject.activeSelf)
                {
                    transform_upgradeBoard.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 销毁火焰
        /// </summary>
        /// <param name="buldingObj"></param>
        public void DestroyFireObj()
        {
            fireState = FireState.NONE;
            fireObjState = FireState.NONE;
            if (t_effects != null)
            {
                t_effects.ForEach((trans) =>
                {
                    if (trans != null)
                    {
                        Transform fire = trans.Find("Fire");
                        if (fire != null)
                        {
                            CoreUtils.assetService.Destroy(fire.gameObject);
                        }
                        else
                        {
                            //Debug.LogError("没有特效？");
                        }
                    }
                    else
                    {
                       // Debug.LogError("没有特效？");
                    }
                });
            }
        }
        private bool IsDay()
        {
            return true;
        }
        public void SetfireState(FireState fireState)
        {
            switch (fireState)
            {
                case FireState.FIRED:
                    this.fireFightingNum = 5;
                    this.fireObjFightingNum = 5;
                    this.workerNum = 0;
                    break;
            }
        
            this.fireState = fireState;
        }

    }

}


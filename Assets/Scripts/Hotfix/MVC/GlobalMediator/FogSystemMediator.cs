// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    CityGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoTemp;
using Client;
using System;
using Data;
using SprotoType;
using Hotfix;

namespace Game
{
    public class FogSystemMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "FogSystemMediator";

        private CityBuildingProxy m_cityBuildingProxy;
        private DataProxy m_dataProxy;
        private ScoutProxy m_ScoutProxy;
        private TroopProxy m_troopProxy;
        

        private Transform m_cityBuildingContainer;
        private Camera m_curCamera;
        private bool LockTouch = false; //是否不可点击状态
        private bool m_hasTouchUI = false; //是否点击到了UI
        private bool m_clickDown = false; //摁下状态
        private bool m_is2Fingers = false; //是否单指点击


        private GridCollideMgr m_tileCollederManager;

        //一个小格子的单位
        public static float CITY_GRID_SIZE = 0.1F;

        //
        public static int CITY_GRID_SIZE_RECIPROCAL = 10;

        //建筑类型
        public static int CITY_BUILDING_TYPE_OFFSET = 100;

        private GridLines m_cityMesh;

        private bool m_isDispose;

        private ITroopMgr troopMgr;
        
        private List<ArmyInfoEntity> myArmy = new List<ArmyInfoEntity>();
        private List<Vector2Int> wantTempOpenFogLst = new List<Vector2Int>();
        private float updateTroopFogTimer;

        Dictionary<long, TroopFogData> troopFogDataDict = new Dictionary<long, TroopFogData>();
        
        #endregion

        //IMediatorPlug needs
        public FogSystemMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        public FogSystemMediator(object viewComponent) : base(NameMediator, null)
        {
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Map_DenseFogOpen.TagName,
                Map_DenseFogClose.TagName,
                CmdConstant.CityBuildingDone,
                CmdConstant.CityBuildingLevelUP,
                Map_PreDenseFogOpen.TagName,
                CmdConstant.SetFogBorderVisible,
                CmdConstant.UnlockAllFog,
                Role_RoleLogin.TagName,
                CmdConstant.ChangeRolePos
            }.ToArray();
        }
        [IFix.Patch]
        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_RoleLogin.TagName:
                    {
                        
                        var scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
                        m_groupSize = scoutProxy.GetSoutView();
                        WarFogMgr.GROUP_SIZE = m_groupSize;

                        var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                        if (playerProxy.CurrentRoleInfo.denseFogOpenFlag)
                        {
                            //雾已全开
                            UnlocakAllFog();
                        }
                        else
                        {
                            //雾未全开
                            var unlockdata = new Int64[400 * 400 / 64];
                            foreach (var kv in playerProxy.CurrentRoleInfo.denseFog.Values)
                            {
                                if (kv.index > unlockdata.Length)
                                {
                                    continue;
                                }
                                unlockdata[kv.index - 1] = kv.rule;
                                //OpenFog(kv.Key, kv.Value.rule);
                            }
                            WarFogMgr.InitFogSystem(400, unlockdata, null);
                        }
                        //RebuildConnection();
                        UpdateExplorer();
                        Debug.Log("迷雾数据完成");
                        AppFacade.GetInstance().SendNotification(CmdConstant.FogSystemLoadEnd);
                    }
                    break;
                case Map_DenseFogOpen.TagName:
                    {
                        Map_DenseFogOpen.request fogDenses = notification.Body as Map_DenseFogOpen.request;
                        if (fogDenses != null)
                        {
                            if (fogDenses.noArrivalOpen == true) // 若有地形无法达到的，会直接把迷雾块驱散 需要提醒
                            {
                                Tip.CreateTip(181148).SetStyle(Tip.TipStyle.Middle).Show();
                            }
                            bool isOpenFog = false;
                            List<Vector2Int> fogOpen = new List<Vector2Int>();
                            foreach (var fog in fogDenses.denseFogIndex)
                            {
                                var tile = PosHelper.FogIndexToTile((int)fog);
                                int x = tile.x;
                                int y = tile.y;
                                if (!WarFogMgr.HasFogAt(x, y, false))
                                {
                                    continue;
                                }

                                WarFogMgr.OpenFog(x, y);

                                var worldPos = PosHelper.TileToWorldPos(x, y);

                                if (Common.IsInViewPort2D(WorldCamera.Instance().GetCamera(),worldPos.x,worldPos.z,0))
                                {
                                    fogOpen.Add(new Vector2Int(x, y));

                                    //添加开雾特效
                                    CoreUtils.assetService.Instantiate("operation_2002", (fotObj) =>
                                    {
                                        if (!isOpenFog)
                                        {
                                            isOpenFog = true;
                                            CoreUtils.audioService.PlayOneShot3D(RS.SoundUiDestroyMist, fotObj);
                                        }
                                        if (AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) ==
                                            this)
                                        {
                                            fotObj.transform.position = worldPos;
                                        }
                                    });
                                }    
                            }
                            if(fogOpen.Count > 0)
                            {
                                WarFogMgr.CrateFadeFog(fogOpen.ToArray());
                            }

                            if (GuideManager.Instance.IsExploreFogGuide)
                            {
                                AppFacade.GetInstance().SendNotification(CmdConstant.FirstExploreFog);
                            }
                        }
                    }
                    break;
                case CmdConstant.ChangeRolePos:
                    {
                        m_bBuildConnect = false;
                    }
                    break;

                case Map_DenseFogClose.TagName:
                    
                    Map_DenseFogClose.request denseFogClose = notification.Body as Map_DenseFogClose.request;
                    if (denseFogClose != null)
                    {
                        foreach (var fog in denseFogClose.denseFogIndex)
                        {
                            Vector2Int tile = PosHelper.FogIndexToTile((int)fog);
                            int x = tile.x;
                            int y = tile.y;

                            bool hasFog = WarFogMgr.HasFogAt(x, y);
                            if (hasFog)
                            {
                                continue;
                            }
                            WarFogMgr.CloseFog(x, y);
                        }
                    }
                    AppFacade.GetInstance().SendNotification(CmdConstant.AllianceEixt);
                    break;
                case CmdConstant.CityBuildingLevelUP:
                    {
                        Int64 buildingIndex = (Int64)notification.Body;
                        BuildingInfoEntity buildingInfo = m_cityBuildingProxy.GetBuildingInfoByindex(buildingIndex);
                        if (buildingInfo != null && buildingInfo.type == (long)EnumCityBuildingType.ScoutCamp)
                        {
                            var scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
                            m_groupSize = scoutProxy.GetSoutView();
                            WarFogMgr.GROUP_SIZE = m_groupSize;
                        }
                    }
                    break;
                case Map_PreDenseFogOpen.TagName: //使用道具开启的迷雾地图
                    ItemPropOpenFog(notification.Body);
                    break;
                case CmdConstant.SetFogBorderVisible:
                    SetFogBorderVisible((bool)notification.Body);
                    break;
                case CmdConstant.UnlockAllFog:
                    UnlocakAllFog();
                    break;
                default:
                    break;
            }
        }

        #region UI template method

        //道具开雾
        private void ItemPropOpenFog(object body)
        {
            Dictionary<int, List<Vector2>> tAniDicList = new Dictionary<int, List<Vector2>>();
            Map_PreDenseFogOpen.request fogDenses = body as Map_PreDenseFogOpen.request;

            if (fogDenses == null)
            {
                Debug.LogError("Map_PreDenseFogOpen is null");
                return;              
            }
            var tile = Pos2Tile(fogDenses.pos.x/100, fogDenses.pos.y/100);
            //Debug.LogErrorFormat("tileX:{0} tileY:{1}", tile.x, tile.y);
            float centPosX = tile.x + 4.5f;
            float centPosY = tile.y + 4.5f;

            //分层解雾
            foreach (var fog in fogDenses.denseFogIndex)
            {
                var fogTile = PosHelper.FogIndexToTile((int)fog);
                int x = fogTile.x;
                int y = fogTile.y;
                if (!WarFogMgr.HasFogAt(x, y))
                {
                    continue;
                }
                int fogX = (int)x;
                int fogY = (int)y;
                int diffX = (int)(Mathf.Abs(centPosX - fogX) / 0.5f);
                int diffY = (int)(Mathf.Abs(centPosY - fogY) / 0.5f);
                //Debug.LogErrorFormat("x:{0} y:{1}", (Mathf.Abs(centPosX - fogX) / 0.5f), (Mathf.Abs(centPosY - fogY) / 0.5f));
                int key = (diffX > diffY) ? diffX : diffY;
                key = (key <= 1) ? 1 : key;
                if (!tAniDicList.ContainsKey(key))
                {
                    tAniDicList.Add(key, new List<Vector2>());
                }
                tAniDicList[key].Add(new Vector2(fogX, fogY));
            }

            //排序一下
            List<int> sortKeyList = new List<int>();
            foreach (var data in tAniDicList)
            {
                sortKeyList.Add(data.Key);
            }
            sortKeyList.Sort();
            //ClientUtils.Print(sortKeyList);
            float times = 0f;
            for (int j = 0; j < sortKeyList.Count; j++)
            {
                int index = j;
                List<Vector2> vecList = tAniDicList[sortKeyList[j]];
                Timer.Register(times, () =>
                {
                    if (m_isDispose)
                    {
                        return;
                    }
                    for (int i = 0; i < vecList.Count; i++)
                    {
                        //Debug.LogError("for:" + vecList[i]+ " index:"+ index);
                        int posX = (int)vecList[i].x;
                        int posY = (int)vecList[i].y;
                        CoreUtils.audioService.PlayOneShot(RS.SoundUiDestroyMist);
                        WarFogMgr.OpenFog(posX, posY);
                        //添加开雾特效
                        CoreUtils.assetService.Instantiate("operation_2002", (fotObj) =>
                        {
                            if (m_isDispose)
                            {
                                return;
                            }
                            fotObj.transform.position = PosHelper.TileToWorldPos(posX, posY);
                        });
                    }
                });
                times = times + 0.3f;
            }
            Timer.Register(times+0.5f, ()=> {
                if (m_isDispose)
                {
                    return;
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.FogUnlock);
            });
        }

        protected override void InitData()
        {
            m_ScoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            WorldCamera.Instance().AddMapClickListner(OnMapClick);
            WorldCamera.Instance().AddViewChange(OnMapViewChange);
            troopMgr = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr();
            //FogSystem.CreateFadeGroup(0, 1);
            InitFogSystem();
        }


        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
        }

        public override void Update()
        {
            UpdateFogByMovingTroop();
        }

        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }

        #endregion

        public override void OnRemove()
        {
            m_isDispose = true;
            WorldCamera.Instance().RemoveMapClickListner(OnMapClick);
            WorldCamera.Instance().RemoveViewChange(OnMapViewChange);
            if (m_fogSystem != null)
            {
                GameObject.Destroy(m_fogSystem);
                m_fogSystem = null;
            }
        }


        private GameObject m_fogSystem;
        private Transform m_fogBorder = null;
        private Transform m_highLevelFog = null;
        private Transform m_mountainLevelFog = null;
        private Transform [] m_fogTile = new Transform[9];

        private void SetFogBorderVisible(bool visible)
        {
            m_fogBorder.gameObject.SetActive(visible);
        }

        private void InitFogSystem()
        {
            CoreUtils.assetService.LoadAssetAsync<GameObject>("fogSystem", (asset) =>
            {
                if (AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) == this)
                {
                    m_fogSystem = CoreUtils.assetService.Instantiate(asset.asset() as GameObject);
                    asset.Attack(m_fogSystem);
                    m_fogBorder = m_fogSystem.transform.Find("fog_border");
                    m_highLevelFog = m_fogSystem.transform.Find("highLevelFog");
                    m_mountainLevelFog = m_fogSystem.transform.Find("mountainLevelFog");
                    for(int i = 0; i < 9; i++)
                    {
                        m_fogTile[i] = m_fogSystem.transform.Find($"fog_fullKingdom/tile{i+1}");
                    }
                    m_fogTile[0].gameObject.SetActive(false);
                    // 迷雾
                    byte[] unlock = new byte[4 * 4 / 8];
                    for (int i = 0; i < unlock.Length; i++)
                    {
                        unlock[i] = 255;
                    }

                    WarFogMgr.InitFogSystem(4, unlock, null);
                    WarFogMgr.ChangeLevel(2);
                }
            }, null);
        }

        private int m_groupSize = 5; // 斥候探索半径


        //private static int FADE_TYPE_NULL = 0;
        public static int FADE_TYPE_CLICK = 1;
        private static int FADE_TYPE_EXPLORE = 2;
        private static int TIlE_BASE_SIZE = 18;
        private float m_kingdomOffsetX = 0;
        private float m_kingdomOffsetY = 0;
        private static short NotConnection = 0;
        private static short ConnectionOpen = 1;
        private static short ConnectionClose = 2;
        private static int ID_FORMATER = 100000;

        private void UnlocakAllFog()
        {
            m_highLevelFog.gameObject.SetActive(false);
            m_mountainLevelFog.gameObject.SetActive(false);
            // 迷雾
            byte[] unlock = new byte[400 * 400 / 8];
            for (int i = 0; i < unlock.Length; i++)
            {
                unlock[i] = 255;
            }
            WarFogMgr.InitFogSystem(400, unlock, null);
            //for (int i = 1; i < 2501; i++)
            //{
            //    OpenFog(i, -1);
            //}
        }

        private void OpenFog(long id, long flag)
        {
            long y = ((id - 1) * 64) / 400;
            long x = ((id - 1) * 64) % 400;
            long shift = 1;
            for (int i = 0; i < 64; i++, x++)
            {
                if (x == 400)
                {
                    x = 0;
                    y++;
                }

                if ((flag & (shift << i)) != 0)
                {
                    WarFogMgr.OpenFog((int) x, (int) y);
                }
            }
        }

        public Vector2Int Pos2Tile(float worldX, float worldY)
        {
            float px = worldX = worldX - m_kingdomOffsetX;
            float py = worldY = worldY - m_kingdomOffsetY;
            return new Vector2Int(Mathf.FloorToInt(px / TIlE_BASE_SIZE), Mathf.FloorToInt((py) / TIlE_BASE_SIZE));
        }

        public Vector3 GetGroupFogCenterPos(float worldX, float worldY, int groupSize)
        {
            return new Vector3(worldX + (float)groupSize / 2 * TIlE_BASE_SIZE, 0, worldY + (float)groupSize / 2 * TIlE_BASE_SIZE);
        }

        public Vector2 Tile2Pos(int tx, int ty)
        {
            return new Vector2(tx * TIlE_BASE_SIZE + m_kingdomOffsetX, ty * TIlE_BASE_SIZE + m_kingdomOffsetY);
        }

        public int Tile2Id(int tx, int ty)
        {
            return ty * ID_FORMATER + tx;
        }

        public int GetFadeGroupId(float worldX, float worldY, int groupSize)
        {
            var tilePos = Pos2Tile(worldX, worldY);
            tilePos.x = Mathf.FloorToInt(tilePos.x / groupSize) * groupSize;
            tilePos.y = Mathf.FloorToInt(tilePos.y / groupSize) * groupSize;
            return Tile2Id(tilePos.x, tilePos.y);
        }

        public int GetGroupIdByPos(float worldX, float worldY)
        {
            var tilePos = Pos2Tile(worldX, worldY);
            return WarFogMgr.GetGroupIdByTile(tilePos.x, tilePos.y);
        }

        private Vector2 m_viewCenter;
        private bool m_bBuildConnect = false;

        private void RebuildConnection()
        {
            var cityBuildingProxy =
                AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            var pos = cityBuildingProxy.RolePos;
            var homeTilePos = Pos2Tile(pos.x, pos.y);
            WarFogMgr.BuildConnections(homeTilePos.x, homeTilePos.y, true);
            m_bBuildConnect = true;
        }
        private void OnMapClick(float x, float y)
        {
            if (WarFogMgr.IsAllFogClear())
                return;

            if (m_bBuildConnect == false)
            {
                RebuildConnection();
            }

            WarFogMgr.RemoveFadeGroupByType(FADE_TYPE_CLICK);

            var tilePos = Pos2Tile(x, y);
            var canExplorer = WarFogMgr.CanExploreTile(tilePos.x, tilePos.y);

            int fadeGroupId = GetFadeGroupId(x, y, m_groupSize);
            if (canExplorer == ConnectionClose)
            {
                var cityProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                var level = (int) cityProxy.GetMaxLevelofType((long) EnumCityBuildingType.ScoutCamp);
                if (level == 0)
                {
                    CoreUtils.audioService.PlayOneShot(RS.UnExploreSound);//不能探索的音效
                    Tip.CreateTip(610022, Tip.TipStyle.Middle).Show();
                    return;
                }

                // 这边需要判断是否有空闲的斥候
                WarFogMgr.CreateFadeGroup(fadeGroupId, FADE_TYPE_CLICK, m_groupSize);
                CoreUtils.uiManager.ShowUI(UI.s_scoutSearchMenuu, null, new Vector2(x, y));
            }
            else if (canExplorer == NotConnection)
            {
                var cityProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
                var level = (int) cityProxy.GetMaxLevelofType((long) EnumCityBuildingType.ScoutCamp);
                if (level == 0)
                {
                    CoreUtils.audioService.PlayOneShot(RS.UnExploreSound);//不能探索的音效
                    Tip.CreateTip(610022, Tip.TipStyle.Middle).Show();
                    return;
                }
                CoreUtils.audioService.PlayOneShot(RS.UnExploreSound);//不能探索的音效
                WarFogMgr.CreateFadeGroup(fadeGroupId, FADE_TYPE_CLICK, m_groupSize);
                Tip.CreateTip(181149, Tip.TipStyle.Middle).Show();
            }
        }

        //是否点击到未解锁的迷雾
        public bool IsClickUnlockFog(int x, int y)
        {
            if (WarFogMgr.IsAllFogClear())
            {
                return false;
            }

            var touchTerrainPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
            var tilePos = Pos2Tile(touchTerrainPos.x, touchTerrainPos.z);
            var canExplorer = WarFogMgr.CanExploreTile(tilePos.x, tilePos.y);
            if (canExplorer == ConnectionClose)
            {
                return true;
            }
            else if (canExplorer == NotConnection)
            {
                return true;
            }
            return false;
        }

        private void OnMapViewChange(float y, float dxf, float x)
        {
            //FogSystem.RemoveFadeGroupByType(FADE_TYPE_CLICK);
        }

        public Vector2 FindClosestAtWorldPos(float x, float y)
        {
            var tilePos = Pos2Tile(x, y);
            tilePos.x = Mathf.Clamp(tilePos.x, 5, 400 - 5);
            tilePos.y = Mathf.Clamp(tilePos.y, 5, 400 - 5);

            var pos = WarFogMgr.FindFogClosestAt(tilePos.x, tilePos.y);

            var worldPos = Tile2Pos((int) pos.x, (int) pos.y);

            return worldPos;
            //int fgId = GetFadeGroupId(worldPos.x, worldPos.y, 5);
            //FogSystem.RemoveFadeGroupByType(1);
            //FogSystem.CreateFadeGroup(fgId, 1, 5);
        }

        public Vector2 FindClosestAtWorldPos2(float x, float y, Dictionary<int, bool> ignoreGroupDic)
        {
            var tilePos = Pos2Tile(x, y);
            tilePos.x = Mathf.Clamp(tilePos.x, 5, 400 - 5);
            tilePos.y = Mathf.Clamp(tilePos.y, 5, 400 - 5);

            var pos = WarFogMgr.FindFogClosestAt(tilePos.x, tilePos.y, ignoreGroupDic);
            var worldPos = Tile2Pos((int)pos.x, (int)pos.y);

            return worldPos;
        }

        public bool HasFogAtWorldPos(float x, float y)
        {
            var tilePos = Pos2Tile(x, y);
            tilePos.x = Mathf.Clamp(tilePos.x, 5, 400 - 5);
            tilePos.y = Mathf.Clamp(tilePos.y, 5, 400 - 5);

            return WarFogMgr.HasFogAt(tilePos.x, tilePos.y);
        }

        public List<Vector2Int> GetGroupFog(float x, float y)
        {
            var tilePos = Pos2Tile(x, y);
            tilePos.x = Mathf.Clamp(tilePos.x, 5, 400 - 5);
            tilePos.y = Mathf.Clamp(tilePos.y, 5, 400 - 5);

            List<Vector2Int> fogs = new List<Vector2Int>();
            int num = Mathf.FloorToInt((float) (tilePos.x / WarFogMgr.GROUP_SIZE)) * WarFogMgr.GROUP_SIZE;
            int num2 = Mathf.FloorToInt((float) (tilePos.y / WarFogMgr.GROUP_SIZE)) * WarFogMgr.GROUP_SIZE;
            for (int i = num; i < num + WarFogMgr.GROUP_SIZE; i++)
            {
                for (int j = num2; j < num2 + WarFogMgr.GROUP_SIZE; j++)
                {
                    if (WarFogMgr.HasFogAt(i, j, false))
                    {
                        Vector2Int pos = new Vector2Int();
                        pos.x = i - num;
                        pos.y = j - num2;
                        fogs.Add(pos);
                    }
                }
            }

            return fogs;
        }

        public void UpdateExplorer()
        {
            if (m_fogSystem == null)
                return;

            WarFogMgr.RemoveFadeGroupByType(FADE_TYPE_EXPLORE);

            var scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            int nNum = scoutProxy.GetScoutNum();
            for (int i = 0; i < nNum; i++)
            {
                var info = scoutProxy.GetScoutInfo(i);
                if (info.state == ScoutProxy.ScoutState.Fog)
                {
                    int fgId = GetFadeGroupId(info.x, info.y, m_groupSize);
                    WarFogMgr.CreateFadeGroup(fgId, FADE_TYPE_EXPLORE, m_groupSize);
                }
            }
        }

        // 根据当前移动的军队，更新雾
        void UpdateFogByMovingTroop()
        {
            updateTroopFogTimer += Time.deltaTime;
            if (updateTroopFogTimer < 0.3f)    // 0.3秒刷新一次
                return;

            updateTroopFogTimer = 0;

            wantTempOpenFogLst.Clear();
            myArmy.Clear();
            myArmy.AddRange(m_troopProxy.GetArmys());

            for (int i = 0; i < myArmy.Count; i++)
            {
                ArmyInfoEntity armyInfoEntity = myArmy[i];  
                int troopId = (int) armyInfoEntity.objectIndex;
                Troops formation = troopMgr.GetTroop(troopId);
                ArmyData army = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId((int)armyInfoEntity.armyIndex);
                var tilePos = Vector2Int.zero;
                if (formation != null)
                {
                    tilePos = Pos2Tile(formation.transform.position.x, formation.transform.position.z);
                }
                else if (army != null)
                {
                    Vector2 pos = army.GetMovePos();
                    tilePos = Pos2Tile(pos.x, pos.y);
                }
                else
                {
                    continue;
                }

                if (!troopFogDataDict.ContainsKey(troopId))
                {
                    troopFogDataDict.Add(troopId, new TroopFogData());
                }
                
                TroopFogData troopFogData = troopFogDataDict[troopId];
                troopFogData.UpdateTile(tilePos);

                for (int tilePosIndex = 0; tilePosIndex < troopFogData.lastTileList.Length; tilePosIndex++)
                {
                    Vector2Int nowTile = troopFogData.nowTileList[tilePosIndex];
                    Vector2Int lastTile = troopFogData.lastTileList[tilePosIndex];
                    
                    bool isTheSame = TroopFogData.IsTheSameValue(nowTile, lastTile);
                    bool IsValidTile = TroopFogData.IsValidTile(lastTile);
                    bool HasFogAt = WarFogMgr.HasFogAt(lastTile.x, lastTile.y);
                    if (!isTheSame && IsValidTile && !HasFogAt)   // 之前位置没有雾，尝试关闭临时雾
                    {
                        WarFogMgr.RemoveTempOpenFog(lastTile.x, lastTile.y);
                    }
                }
                
                wantTempOpenFogLst.AddRange(troopFogData.nowTileList);
            }

            for (int i = 0; i < wantTempOpenFogLst.Count; i++)
            {
                Vector2Int tilePos = wantTempOpenFogLst[i];
                if (WarFogMgr.HasFogAt(tilePos.x,tilePos.y))
                {
                    WarFogMgr.AddTempOpenFog(tilePos.x, tilePos.y);
                }
            }
            
        }

    }
}
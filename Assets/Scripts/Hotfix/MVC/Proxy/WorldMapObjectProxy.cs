using System;
using System.Collections.Generic;
using System.Linq;
using Client;
using Client.Utils;
using Data;
using Hotfix;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class WorldMapObjectProxy : GameProxy {

        #region Member
        public const string ProxyNAME = "WorldMapObjectProxy";
        
        private Dictionary<long,MapObjectInfoEntity> m_worldMapObjectDic = new Dictionary<long,MapObjectInfoEntity>();
        private Dictionary<long,MapObjectInfoEntity> m_worldMapObjectCityDic = new Dictionary<long,MapObjectInfoEntity>();//城市
        private Dictionary<long, MapObjectInfoEntity> m_resourcePointDic = new Dictionary<long, MapObjectInfoEntity>();//村庄山洞
        private List<MapObjectInfoEntity> m_worldMapObjectList = new List<MapObjectInfoEntity>();

        private PlayerProxy m_playerProxy;
        private CityBuildingProxy m_cityBuildingProxy;
        private RssProxy m_RssProxy;
        private long m_myCityID = 0;
        
        //领地大小格式
        public static Vector2 TerritoryPerUnit = new Vector2(18,18);

        public static int TerritoryCol = 7200 / (int)TerritoryPerUnit.x;
        
        
        #endregion

        // Use this for initialization
        public WorldMapObjectProxy(string proxyName)
            : base(proxyName)
        {

        }
        
        public override void OnRegister()
        {
            Debug.Log(" DataProxy register");   
        }

        public void Init()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_RssProxy= AppFacade.GetInstance().RetrieveProxy(RssProxy.ProxyNAME) as RssProxy;
        }

        public override void OnRemove()
        {
            Debug.Log(" DataProxy remove");
            ClearAllMapObject();
        }

        public long MyCityId => m_myCityID;


        public void UpdateMapObject(Map_ObjectInfo.request mapobject)
        {
            MapObjectInfoEntity old;

            if (m_playerProxy.CurrentRoleInfo !=null && mapobject.mapObjectInfo.cityRid == m_playerProxy.CurrentRoleInfo.rid)
            {
                 m_myCityID = mapobject.mapObjectInfo.objectId;
            }
            
            if (!m_worldMapObjectDic.TryGetValue(mapobject.mapObjectInfo.objectId, out old) )
            {
                old = new MapObjectInfoEntity();
                m_worldMapObjectDic.Add(mapobject.mapObjectInfo.objectId, old);
                m_worldMapObjectList.Add(old);

                //当自己部队不在视野范围内worldMapObject会删除，armyData不会删除，并且引用了worldMapObject
                //当worldMapObject重新创建后需要刷新armyData中引用的worldMapObject
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData((int)mapobject.mapObjectInfo.objectId);
                if (armyData != null)
                {
                    armyData.FillMapObjectInfo((int)mapobject.mapObjectInfo.objectId);
                }
            }
          
            MapObjectInfoEntity.updateEntity(old, mapobject.mapObjectInfo);
            if (mapobject.mapObjectInfo.objectType == (long)RssType.Village || mapobject.mapObjectInfo.objectType == (long)RssType.Cave)
            {
                if (!m_resourcePointDic.ContainsKey(mapobject.mapObjectInfo.resourcePointId))
                {
                    m_resourcePointDic.Add(mapobject.mapObjectInfo.resourcePointId, old);
                }
                m_RssProxy.SetViallAgeState(old);
            }
            else if (mapobject.mapObjectInfo.objectType == (long)RssType.City)
            {
                if (!m_worldMapObjectCityDic.ContainsKey(mapobject.mapObjectInfo.cityRid))
                {
                    m_worldMapObjectCityDic.Add(mapobject.mapObjectInfo.cityRid, old);
                }
            }
            else if (mapobject.mapObjectInfo.objectType == (long)RssType.Transport)
            {
                if (mapobject.mapObjectInfo.isBattleLose)
                {
                    if (old.guildId <= 0)
                    {
                        mapobject.mapObjectInfo.armyRid = 0;
                        mapobject.mapObjectInfo.armyName = LanguageUtils.getText(730374);
                        old.armyRid = 0;
                        old.armyName = LanguageUtils.getText(730374);
                    }
                }
            }
            if (old.objectType == 0)
            {
                Debug.LogWarning(old.objectType+"错误信息 objectId:"+old.objectId);
                m_worldMapObjectDic.Remove(old.objectId);
                m_worldMapObjectList.Remove(old);
                return;
            }
            if (old.objectType == 3)
            {
                if (old.objectPos == null)
                {
                    Debug.LogError(old.objectType + "没有接收到位置 objectId:" + old.objectId);
                    return;
                }
            }

            if (old.gridX == -1)
            {
                old.gridX = GetGridX(old.objectPos.x / 100);
                old.gridY = GetGridX(old.objectPos.y / 100);
                old.rssType = (RssType)old.objectType;
            }
            
            AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectChange_ObjectInfoReq, mapobject.mapObjectInfo);
            HotfixUtil.InvokOncePerfOneFrame($"MapObjectChange_{old.objectId}", ()=>
            {
                m_RssProxy.UpdateRss(old);
                AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectChange, old);
            });
            if(mapobject.mapObjectInfo.objectType == 0)
            {
                HotfixUtil.InvokOncePerfOneFrame($"MapLogicObjectChange_{mapobject.GetHashCode()}", () =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapLogicObjectChange, mapobject);
                });
            }
            if (mapobject.mapObjectInfo.HasBeginBurnTime)
            {
             //   Debug.LogErrorFormat("{0},,{1}", mapobject.mapObjectInfo.beginBurnTime, mapobject.mapObjectInfo.cityRid);
                AppFacade.GetInstance().SendNotification(CmdConstant.CityBeginBurnTimeChange,old);
            }
            if (mapobject.mapObjectInfo.HasCityBuff)
            {
                //   Debug.LogErrorFormat("{0},,{1}", mapobject.mapObjectInfo.beginBurnTime, mapobject.mapObjectInfo.cityRid);
                AppFacade.GetInstance().SendNotification(CmdConstant.CityCitybuffChange, old);
            }
            if (mapobject.mapObjectInfo.HasCityPosTime)
            {
                //   Debug.LogErrorFormat("{0},,{1}", mapobject.mapObjectInfo.beginBurnTime, mapobject.mapObjectInfo.cityRid);
                AppFacade.GetInstance().SendNotification(CmdConstant.CityPosTimeChange, old);
            }

            if (mapobject.mapObjectInfo.HasObjectType)
            {
                if (mapobject.mapObjectInfo.objectType > 0)
                {
                    WorldMapLogicMgr.Instance.UpdateMapDataHandler.InitMapData(mapobject);
                }
            }

            WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().UpdateAOITroopLines((int)mapobject.mapObjectInfo.objectId);
        }       

        public static int GetGridX( float worldPosX)
        {
            return PosHelper.ClientUnitToClientGrid(worldPosX);
//            return Mathf.FloorToInt(worldPosX / 30f);
        }
        
        public static int GetGridY( float worldPosY)
        {
            return PosHelper.ClientUnitToClientGrid(worldPosY);
//            return Mathf.FloorToInt(worldPosY / 30f);
        }
       
        public static Vector2Int GetGrid(Vector3 worldPos)
        {
            return PosHelper.ClientUnitToClientGrid(worldPos);
//            return new Vector2Int(GetGridX(worldPos.x), GetGridY(worldPos.z));
        }

        public static Vector3 ServerPosToWorldPos(PosInfo posInfo)
        {
            return PosHelper.ServerUnitToClientUnit(posInfo);
//            return new Vector3(posInfo.x / 100f, 0, posInfo.y / 100f);
        }

        public static Vector2Int ServerPosToGamePos(PosInfo posInfo)
        {
            return PosHelper.ServerUnitToClientPos(posInfo);
//            return new Vector2Int((int)posInfo.x / 600, (int)posInfo.y / 600);
        }

        public static Vector2Int WorldPosToGamePos(Vector3 worldPos)
        {
            return PosHelper.ClientUnitToClientPos(worldPos);
//            return new Vector2Int((int)worldPos.x / 6, (int)worldPos.z / 6);
        }

        public void AddWorldObj(MapObjectInfoEntity objData)
        {
            MapObjectInfoEntity old;
            if (!m_worldMapObjectDic.TryGetValue(objData.objectId, out old))
            {
                m_worldMapObjectDic.Add(objData.objectId,objData);
                m_worldMapObjectList.Add(objData);
            }
        }




        public void ClearAllMapObject()
        {
            Debug.Log("清理全部地图对象");
            long[] ids = new long[m_worldMapObjectDic.Count];
            int index = 0;
            foreach (var obj in m_worldMapObjectDic)
            {
                ids[index] = obj.Value.objectId;
                index++;
            }

            for (int i = 0; i < ids.Length; i++)
            {
                    DelMapObject(ids[i]);
            }
            
            m_worldMapObjectList.Clear();
            m_worldMapObjectDic.Clear();
            m_worldMapObjectCityDic.Clear();
            CleanTerritory();
        }

        public void DelMapObject(long objectId)
        {
            if (objectId == MyCityId || objectId == -MyCityId)
            {
                return;
            }

            MapObjectInfoEntity obj;
            if (m_worldMapObjectDic.TryGetValue(objectId,out obj))
            {


                m_worldMapObjectList.Remove(obj);
                    
                
                obj.delTime = ServerTimeModule.Instance.GetServerTime();
                if (obj.objectType == (long)RssType.Village || obj.objectType == (long)RssType.Cave)
                {
                    m_resourcePointDic.Remove(obj.resourcePointId);
                }
                else if (obj.objectType == (long)RssType.City)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.RemoveOtherCity, obj.cityRid);
                    m_worldMapObjectCityDic.Remove(obj.cityRid);
                }
                m_worldMapObjectDic.Remove(objectId);

                //临时移除一下后面要改下策略
                if (obj.gameobject!=null)
                {
                    //移除地图特效
                    var m = AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as
                        WorldMgrMediator;
                    m.RemoveByObjectId(obj.objectId);
                    //移除联盟建筑上的燃烧特效
                    if (TroopHelp.IsAttackGuildType(obj.rssType))
                    {
                        WorldMapLogicMgr.Instance.MapBuildingFightHandler.StopBurning((int)obj.objectId);
                    }

                    //> 这边有隐患，没有区分是不是战斗
                    if (obj.rssType == RssType.Monster ||
                        obj.rssType == RssType.Guardian ||
                        obj.rssType == RssType.SummonAttackMonster ||
                        obj.rssType == RssType.SummonConcentrateMonster)
                    {
                        Troops formation = obj.gameobject.GetComponent<Troops>();
                        if (formation != null)
                        {
                            Troops.FadeOut_S(formation);
                        }

                        AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveMonsterFightHud, (int)obj.objectId);
                        Timer.Register(2, () =>
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.MapStopShottTextHud, (int)obj.objectId);
                            WorldMapLogicMgr.Instance.BattleUIHandler.Remove((int)obj.objectId);
                            WorldMapLogicMgr.Instance.BattleBuffHandler.ClearBuff((int)obj.objectId);
                            CoreUtils.assetService.Destroy(obj.gameobject);
                        });
                    }
                    else if (obj.rssType != RssType.City)
                    {
                        CoreUtils.assetService.Destroy(obj.gameobject);
                    }
                    
                    WorldMapLogicMgr.Instance.UpdateDataHandler.UpdateTroopData().DeleteAOITroopLines((int)obj.objectId);                   
                }                                
                AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectRemove,obj);
            }
        }

        public List<MapObjectInfoEntity> GetWorldMapObjects()
        {
            return m_worldMapObjectList;
        }
        public List<MapObjectInfoEntity> GetWorldMapObjectCitys()
        {
            return m_worldMapObjectCityDic.Values.ToList();
        }


        public MapObjectInfoEntity GetWorldMapObjectByRid(long cityRid)
        {
            MapObjectInfoEntity request = null;
            if (m_worldMapObjectCityDic.ContainsKey(cityRid))
            {
                request = m_worldMapObjectCityDic[cityRid];
            }
            return request;
        }
        public MapObjectInfoEntity GetWorldMapObjectByPos(long x,long y)
        {
            MapObjectInfoEntity request = null;
            foreach (MapObjectInfoEntity temp in m_worldMapObjectDic.Values)
            {
                    if (temp.objectPos.x/100 == x&& temp.objectPos.y/100 == y)
                    {
                        request = temp;
                        break;
                    }
            }
            return request;
        }
        public MapObjectInfoEntity GetWorldMapObjectByresourcePoint(long resourcePointId)
        {
            MapObjectInfoEntity mapObject = null;
            m_resourcePointDic.TryGetValue(resourcePointId, out mapObject);
            return mapObject;
        }

        public MapObjectInfoEntity GetWorldMapObjectByobjectId(long objectId)
        {
            MapObjectInfoEntity request = null;
            if (m_worldMapObjectDic.ContainsKey(objectId))
            {
                request = m_worldMapObjectDic[objectId];
            }
            return request;
        }

        #region 领地相关

        
        //激活占领
        private List<ManorItem> m_territoryActiveList = new List<ManorItem>();
        
        //失效领地
        private List<ManorItem> m_territoryDisableList = new List<ManorItem>();
        
        //lod3以上需要服务器范围
        private List<ManorLod3Data> m_territoryLod3DisablePoints = new List<ManorLod3Data>();
        private List<ManorLod3Data> m_territoryLod3ActivePoints = new List<ManorLod3Data>();

        //建造中
        private List<ManorItem> m_territoryPreBuildList = new List<ManorItem>();
        //伪装的，建造前拖动时候实时显示范围使用
        private List<ManorItem> m_territoryFakeList = new List<ManorItem>();
        
        private HashSet<int> m_territoryhash = new HashSet<int>();


        public void CleanTerritory()
        {
            m_territoryActiveList.Clear();
            m_territoryDisableList.Clear();
            m_territoryPreBuildList.Clear();
            
            m_territoryFakeList.Clear();
            
            m_territoryLod3ActivePoints.Clear();
            m_territoryLod3DisablePoints.Clear();

            m_guildTerritoryDic.Clear();
            m_guildTerritoryIndexDic.Clear();
            m_guildColor.Clear();
        }
            
        //激活
        public List<ManorItem> GetTerritoryActices()
        {
            List<ManorItem> nestterritoryActiveList = new List<ManorItem>();

            var gridPos = WorldCamera.Instance().GetViewCenter();

            m_territoryActiveList.ForEach((item) =>
            {
                if (Mathf.Abs(gridPos.x - item.startPosX) < 120 && Mathf.Abs(gridPos.y - item.startPosY) < 100)
                {
                    nestterritoryActiveList.Add(item);
                }
            });

            return nestterritoryActiveList;
        }
        //失效
        public List<ManorItem> GetTerritoryDisables()
        {
            List<ManorItem> nestterritoryActiveList = new List<ManorItem>();
            var gridPos = WorldCamera.Instance().GetViewCenter();

            m_territoryDisableList.ForEach((item) =>
            {
                if (Mathf.Abs(gridPos.x - item.startPosX) < 120 && Mathf.Abs(gridPos.y - item.startPosY) < 100)
                {
                    nestterritoryActiveList.Add(item);
                }
            });

            return nestterritoryActiveList;
        }
        //预占领
        public List<ManorItem> GetTerritoryInactives()
        {
            List<ManorItem> nestterritoryActiveList = new List<ManorItem>();
            var gridPos = WorldCamera.Instance().GetViewCenter();
            m_territoryPreBuildList.ForEach((item) =>
            {
                if (Mathf.Abs(gridPos.x - item.startPosX) < 120 && Mathf.Abs(gridPos.y - item.startPosY) < 100)
                {
                    nestterritoryActiveList.Add(item);
                }
            });

            return nestterritoryActiveList;
           
        }
        //客户端虚拟范围
        public List<ManorItem> GetTerritoryFakes()
        {
            return m_territoryFakeList;
        }
        
        public List<ManorLod3Data> getTerritoryLod3ActicesPoints()
        {
            return m_territoryLod3ActivePoints;
        }
        
        public List<ManorLod3Data> getTerritoryLod3DisablePoints()
        {
            return m_territoryLod3DisablePoints;
        }

        public bool HasChangeFake()
        {
            var t = m_isFakeChange;

            m_isFakeChange = false;
            return t;
        }

        private bool m_isFakeChange = false;
        public bool AddFakeTerritory(List<ManorItem> list)
        {

            m_isFakeChange = false;
            if (list.Count == m_territoryhash.Count)
            {
                list.ForEach((item =>
                {
                    if (m_isFakeChange==false && !m_territoryhash.Contains(item.startPosX*1000+item.startPosY))
                    {
                        m_isFakeChange = true;
                    }
                }));
            }
            else
            {
                m_isFakeChange = true;
            }

            if (m_isFakeChange)
            {
                m_territoryFakeList.Clear();
                m_territoryFakeList.AddRange(list);
                m_territoryhash.Clear();
                list.ForEach(item => { m_territoryhash.Add(item.startPosX * 1000 + item.startPosY); });
            }
            return m_isFakeChange;
        }

        public void ClearFakeTerritory()
        {
            m_isFakeChange = true;
            m_territoryFakeList.Clear();
        }

        private Dictionary<long,ManorItem> m_guildTerritoryDic = new Dictionary<long,ManorItem>();
        private Dictionary<long,ManorItem> m_guildTerritoryIndexDic = new Dictionary<long, ManorItem>();
        private Dictionary<long,Color> m_guildColor = new Dictionary<long, Color>();


        private long[] m_territoryGuildArea =
            new long[TerritoryCol*TerritoryCol];
        

        private Dictionary<long,Color> m_flagColorCache = new Dictionary<long, Color>(16);


        private Color GetTerritoryColor(long colorID)
        {
            Color color = Color.white;

            if (!m_flagColorCache.TryGetValue(colorID,out color))
            {
                var colorConfig = CoreUtils.dataService.QueryRecord<AllianceSignDefine>((int)colorID);

                if (color!=null)
                {
                    ColorUtility.TryParseHtmlString(colorConfig.colour,out color);
                }
            }

            return color;
        }

        
        private static int unit = 7200 / (int)TerritoryPerUnit.x;
        private ManorItem createTerritoryItem(long index,int gid,Color color)
        {
            
            long x = index % unit -1 ;
            long y = index / unit ;
            int startx = (int) (x * TerritoryPerUnit.x);
            int starty = (int) (y * TerritoryPerUnit.y);
                        
            var tite = new ManorItem(gid,color, startx,starty,startx+(int)TerritoryPerUnit.x,starty+(int)TerritoryPerUnit.x);

            return tite;
        }

        //public List<Vector2Int> GuildArea(long guildID)
        //{
        //    List<Vector2Int> area = new List<Vector2Int>();
        //    for (int i = 0; i < m_territoryGuildArea.Length; ++i)
        //    {
        //        if(m_territoryGuildArea[i] == guildID)
        //        {
        //            int x = i % unit - 1;
        //            int y = i / unit;
        //            area.Add(new Vector2Int(x, y));
        //        }
        //    }
        //    return area;
        //}

        public bool IsTerritoryPosInGuildArea(int row, int col, long guildID)
        {
            int index = row * unit + col;

            if (index < 0 || index > m_territoryGuildArea.Length - 1)
            {
                return false;
            }

            long tGuildID = m_territoryGuildArea[index];

            return tGuildID == guildID;
        }

        public bool IsPosInGuildArea(float x,float y,long guildID)
        {
            int col = (int)(x / TerritoryPerUnit.x);
            int row = (int)(y / TerritoryPerUnit.y);
            return IsTerritoryPosInGuildArea(row, col, guildID);            
        }
        /// <summary>
        /// 时是否是联盟区域，是返回联盟id，否返回0
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// 
        /// <returns></returns>
        public long IsPosInGuildArea(float x, float y,ref bool isLoseOrOPreBuild,ref long loseOrPreGuildID)
        {
            int col = (int)(x / TerritoryPerUnit.x);
            int row = (int)(y / TerritoryPerUnit.y);

            

            int index = row * unit + col;

            if (index < 0 || index > m_territoryGuildArea.Length - 1)
            {
                return 0;
            }
            
            

            long tGuildID = m_territoryGuildArea[index];
            
            Debug.Log(x+" "+y+" 查询领地"+col+"  "+row+"  "+tGuildID );

            ManorItem item;
            
            index = row * unit + (col+1);

            if (m_guildTerritoryIndexDic.TryGetValue(index,out item))
            {
                if (!m_territoryActiveList.Contains(item))
                {
                    Debug.Log(x+" "+y+" 查询领地 失效"+col+"  "+row+"  "+item.allianceId );
                    isLoseOrOPreBuild = true;
                    loseOrPreGuildID = item.allianceId;
                }
            }

            return tGuildID;
        }

        private void AddGuildArea(long x, long y, long guildID)
        {
            long index = y * unit + x;

            if (index<0 || index> m_territoryGuildArea.Length-1)
            {
                return ;
            }
            
           // Debug.Log("设置领地"+x+"  "+y+"  "+guildID );

            m_territoryGuildArea[index] = guildID;
        }

        
        private Rect m_kingdomRect = new Rect(1,1,7200-1,7200-1);

        public bool IsPosInNoBuilding(MeshCollider collider,Vector3 cityModelPosition,float rd)
        {


            if (!m_kingdomRect.Contains(new Vector2(cityModelPosition.x,cityModelPosition.z)))
            {
                return true;
            }
            
            Vector2 screenPos = WorldCamera.Instance().GetCamera().WorldToScreenPoint(cityModelPosition);

            Ray ray = WorldCamera.Instance().GetCamera().ScreenPointToRay(screenPos);
            RaycastHit hit;

            bool isHit = collider.Raycast(ray, out hit, 1000.0f);
            
            
            if (isHit)
            {
                return true;
            }
            
            
            Vector3 closestPoint = MathExtension.NearestVertexTo(collider, cityModelPosition);
            //Vector3 closestPoint = collider.ClosestPoint(cityModelPosition);
            
            float dis = Vector3.Distance(cityModelPosition, closestPoint);
           // Debug.Log(isHit+"  dis: "+dis+"  IsPosInNoBuilding  c:"+cityModelPosition+" h: "+closestPoint);


            if (!isHit&&  dis < rd )
            {
                return true;
            }
            return false;
        }

        private void RemoveGuildArea(long x, long y, long guildID)
        {
            long index = y * unit + x;

            if (index<0 || index> m_territoryGuildArea.Length-1)
            {
                return ;
            }


            if (m_territoryGuildArea[index] ==  guildID)
            {
                m_territoryGuildArea[index] =0;
            }
        }

        private Vector2[] ServerPoint2Local(List<PosInfo> posInfos)
        {
            Vector2[] list = new Vector2[posInfos.Count+2];

            int i = 1;
            posInfos.ForEach((pos)=>{
                list[i]=(new Vector2(pos.x,pos.y));
                i++;
            });
            var midPoint =(list[1] + list[list.Length - 2]) / 2;
            list[0] = midPoint;
            //补齐线
            list[list.Length - 1] = midPoint;

            // float d = 0;
            // for (int j = 0; j < list.Length -1; j++)
            // {
            //     d+= -(0.5f*(list[j+1].y+list[j].y)*(list[j+1].x-list[j].x));
            // }
            //
            // if (d<0)
            // {
            //     //逆时针  转回来
            //     list.Reverse();
            // }
            
            return list;
        }

        public void UpdateTerritoryLines(Map_GuildTerritoryLines.request request)
        {
            if (request.HasGuildTerritoryLines)
            {
                foreach (var kv in request.guildTerritoryLines)
                {
                    var tl = kv.Value;

                    long gid = tl.guildId;
                    var color = GetTerritoryColor(tl.colorId);


                    foreach (var lines in tl.validLines)
                    {
                        if (lines.linePos!=null)
                        {
                            ManorLod3Data lod3 = new ManorLod3Data();

                            lod3.points = ServerPoint2Local(lines.linePos);
                            lod3.color = color;
                            lod3.dir = (byte)lines.direction;
                            m_territoryLod3ActivePoints.Add(lod3);
                        }
                       
                    }
                    
                    foreach (var lines in tl.invalidLines)
                    {
                        if (lines.linePos != null)
                        {
                            ManorLod3Data lod3 = new ManorLod3Data();

                            lod3.points = ServerPoint2Local(lines.linePos);
                            lod3.color = Color.white;
                            lod3.dir = (byte)lines.direction;
                            m_territoryLod3DisablePoints.Add(lod3);
                        }
                    }
                }
                
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceTerritoryStrategicUpdate);
            }
        }

        public void UpdateTerritoris(Map_GuildTerritories.request request)
        {
            //ClientUtils.Print(request,"领土修改");
            if (request.HasGuildTerritories)
            {
                foreach (var guildTerritory in request.guildTerritories)
                {
                    var tValue = guildTerritory.Value;
                    int gid = (int)tValue.guildId;

                    int len = 0;
                    
                    Color guildColor = GetTerritoryColor(tValue.colorId);

                    if (!m_guildColor.ContainsKey(gid))
                    {
                        m_guildColor.Add(gid,guildColor);
                    }
                    else
                    {
                        m_guildColor[gid] = guildColor;
                        
                        m_territoryPreBuildList.ForEach((tlan) =>
                        {
                            if (tlan.allianceId == gid)
                            {
                                tlan.color = guildColor;
                            }
                        });
                    }


                    if (guildTerritory.Value.HasValidTerritoryIds)
                    {
                        len= tValue.validTerritoryIds.Count;
                    
                        Vector2[] points = new Vector2[len];

                        var color = GetTerritoryColor(tValue.colorId);

                        for (int i = 0; i < len; i++)
                        {
                            long index = tValue.validTerritoryIds[i];

                            ManorItem tite;

                            if (m_guildTerritoryIndexDic.TryGetValue(index,out tite))
                            {
                                tite.color = color;
                                
                                //Debug.Log(index+"修改颜色"+lod3.color);
                            }
                            else
                            {
                                tite = createTerritoryItem(index, gid, color);
                                points[i]= new Vector2(tite.startPosX,tite.startPosY);
                                m_guildTerritoryIndexDic.Add(index,tite);
                                m_territoryActiveList.Add(tite);


                                AddGuildArea(tite.startPosX/(int)TerritoryPerUnit.x, tite.startPosY/(int)TerritoryPerUnit.y, gid);
                            }
                        }


                    }
                    else
                    {
                        m_territoryActiveList.ForEach((item) =>
                        {
                            if (item.allianceId == gid)
                            {
                                item.color = guildColor;
                            }
                            
                        });
                    }


                    if (tValue.HasInvalidTerritoryIds)
                    {
                        len = tValue.invalidTerritoryIds.Count;
                    
                        Vector2[] pointsInvalid = new Vector2[len];

                        var color = Color.white;

                        for (int i = 0; i < len; i++)
                        {
                            long index = tValue.invalidTerritoryIds[i];
                        
                            ManorItem tite;
                            if (!m_guildTerritoryIndexDic.TryGetValue(index,out tite))
                            {
                                tite = createTerritoryItem(index, gid, color);
                                pointsInvalid[i]= new Vector2(tite.startPosX,tite.startPosY);
                                m_guildTerritoryIndexDic.Add(index,tite);
                                m_territoryDisableList.Add(tite);
                            }
                        }
                    }
                    
                    if (tValue.HasPreOccupyTerritoryIds)
                    {
                        len = tValue.preOccupyTerritoryIds.Count;
                    
                        Vector2[] pointsPreBuild = new Vector2[len];

                        var color = GetTerritoryColor(tValue.colorId);

                        for (int i = 0; i < len; i++)
                        {
                            long index = tValue.preOccupyTerritoryIds[i];
                        
                            ManorItem tite;
                            if (!m_guildTerritoryIndexDic.TryGetValue(index,out tite))
                            {
                                tite = createTerritoryItem(index, gid, color);
                                pointsPreBuild[i]= new Vector2(tite.startPosX,tite.startPosY);
                                m_guildTerritoryIndexDic.Add(index,tite);
                                m_territoryPreBuildList.Add(tite);
                            }
                        }
                    }
                    
                }
            }
            
            if (request.HasDelGuildTerritories)
            {
                request.delGuildTerritories.ForEach((delinfo) =>
                {
                    delinfo.validTerritoryIds?.ForEach((delIndex)=>
                    {
                        ManorItem item;
                        if (m_guildTerritoryIndexDic.TryGetValue(delIndex, out item))
                        {
                            m_territoryActiveList.Remove(item);
                            m_guildTerritoryIndexDic.Remove(delIndex);

                            RemoveGuildArea(item.startPosX/(int)TerritoryPerUnit.x, item.endPosY/(int)TerritoryPerUnit.y, item.allianceId);
                        }
                    });

                    
                    delinfo.invalidTerritoryIds?.ForEach((delIndex) =>
                    {
                        ManorItem item;
                        if (m_guildTerritoryIndexDic.TryGetValue(delIndex, out item))
                        {
                            m_territoryDisableList.Remove(item);
                            m_guildTerritoryIndexDic.Remove(delIndex);
                        }
                    });

                   
                    delinfo.preOccupyTerritoryIds?.ForEach((delIndex)=>
                    {
                        ManorItem item;
                        if (m_guildTerritoryIndexDic.TryGetValue(delIndex, out item))
                        {
                            m_territoryPreBuildList.Remove(item);
                            m_guildTerritoryIndexDic.Remove(delIndex);
                        }
                    });
                    
                });
            }

            if (request.HasAddGuildTerritories)
            {
                request.addGuildTerritories.ForEach((addinfo) =>
                {
                    int gid = (int)addinfo.guildId;
                    Color color;

                    if (m_guildColor.ContainsKey(gid))
                    {
                        color = m_guildColor[gid];
                    }
                    else
                    {
                        color = GetTerritoryColor(addinfo.colorId);
                        m_guildColor.Add(gid,color);
                    }


                    addinfo.validTerritoryIds?.ForEach((addIndex) =>
                    {
                        ManorItem tite;
                        if (!m_guildTerritoryIndexDic.TryGetValue(addIndex,out tite))
                        {
                            tite = createTerritoryItem(addIndex, gid, color);
                            AddGuildArea(tite.startPosX / (int)TerritoryPerUnit.x, tite.startPosY / (int)TerritoryPerUnit.y, gid);
                            m_guildTerritoryIndexDic.Add(addIndex,tite);
                            m_territoryActiveList.Add(tite);
                        }
                        else
                        {
                            tite.color = color;
                        }
                    });
                    
                    
                    addinfo.invalidTerritoryIds?.ForEach((addIndex) =>
                    {
                        ManorItem tite;
                        if (!m_guildTerritoryIndexDic.TryGetValue(addIndex,out tite))
                        {
                            tite = createTerritoryItem(addIndex, gid, Color.white);
                            m_guildTerritoryIndexDic.Add(addIndex,tite);
                            m_territoryDisableList.Add(tite);
                        }
                        else
                        {
                            tite.color = Color.white;
                        }
                    });
                    
                    addinfo.preOccupyTerritoryIds?.ForEach((addIndex) =>
                    {
                        ManorItem tite;
                        if (!m_guildTerritoryIndexDic.TryGetValue(addIndex,out tite))
                        {
                            tite = createTerritoryItem(addIndex, gid, color);
                            m_guildTerritoryIndexDic.Add(addIndex,tite);
                            m_territoryPreBuildList.Add(tite);
                        }
                        else
                        {
                            Debug.Log(gid+"  x:"+tite.startPosX+" 错误的服务器数据，已经存在领地了 有效领地 y: "+tite.startPosY +"  索引:"+addIndex);
                        }
                    });
                    
                });
            }

            //ManorMgr.ClearAllLine_S(true,false);
            HotfixUtil.InvokOncePerfOneFrame("AllianceTerritoryUpdate", () =>
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceTerritoryUpdate);
            });            
        }



        #endregion

       

    }
}
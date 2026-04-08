// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月29日
// Update Time         :    2020年4月29日
// Class Description   :    UI_Item_LodMenu_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using System.Collections.Generic;
using PureMVC.Interfaces;
using UnityEngine.EventSystems;
using Data;
using System.Linq;

namespace Game {
    public partial class UI_Item_LodMenu_SubView : UI_SubView
    {
        private Transform m_root;
        private const string m_root_path = "SceneObject/lod3_root";

        private Dictionary<LodMenuToggle, GameToggle> m_lodMenuToggles = new Dictionary<LodMenuToggle, GameToggle>();
        private Dictionary<LodMenuToggle, GameObject> m_lodMenuObjs = new Dictionary<LodMenuToggle, GameObject>();
        private Dictionary<LodMenuToggle, bool> m_lodMenuStates = new Dictionary<LodMenuToggle, bool>();
        private PlayerProxy m_playerProxy;
        private AllianceProxy m_allianceProxy;
        private MinimapProxy m_miniMapProxy;
        private MapMarkerProxy m_mapMarkerProxy;

        private FogSystemMediator m_fogSystemMediator;
        private FogSystemMediator FogSystemMediator
        {
            get
            {
                if (m_fogSystemMediator == null)
                {
                    m_fogSystemMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
                }
                return m_fogSystemMediator;
            }
        }

        private List<MapFixPointDefine> m_villiageCaves;
        private List<MapFixPointDefine> VilliageCaves
        {
            get
            {
                if(m_villiageCaves==null)
                {
                    m_villiageCaves = CoreUtils.dataService.QueryRecords<MapFixPointDefine>().FindAll(i=>i.group==1);
                }
                return m_villiageCaves;
            }
        }        
        
        private List<MapFixPointDefine> m_guildRss;
        private List<MapFixPointDefine> GuildRss
        {
            get
            {
                if(m_guildRss==null)
                {
                    m_guildRss = CoreUtils.dataService.QueryRecords<MapFixPointDefine>().FindAll(i=>i.group==2);
                }
                return m_guildRss;
            }
        }

        private Dictionary<long, MemberPosInfo> m_alliancePosInfos = new Dictionary<long, MemberPosInfo>();
        private Dictionary<long, GameObject> m_alliancePosObj = new Dictionary<long, GameObject>();
        private Dictionary<int, GameObject> m_villiageCaveObj = new Dictionary<int, GameObject>();        
        private Dictionary<int, GameObject> m_guildRssObj = new Dictionary<int, GameObject>();

        private List<int> m_isLoadingVilliageCaveObj = new List<int>();
        private List<int> m_isLoadingRssObj = new List<int>();
        private List<long> m_isLoadingAllianceObj = new List<long>();

        protected override void BindEvent()
        {
            base.BindEvent();

            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_miniMapProxy = AppFacade.GetInstance().RetrieveProxy(MinimapProxy.ProxyNAME) as MinimapProxy;
            m_mapMarkerProxy = AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy;

            SubViewManager.Instance.AddListener(new string[] {
                    CmdConstant.AllianceEixt,
                    CmdConstant.RefreshGuildMemberPosView,
            }, this.gameObject, OnNotification);

            InitLodMenu();
        }

        private void OnNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.AllianceEixt:
                    {
                        DeleteAlliance();
                    }
                    break;
                case CmdConstant.RefreshGuildMemberPosView:
                    {
                        ClearAlliancePos();
                        OnAllianceBuilding(m_lodMenuStates[LodMenuToggle.Alliance]);
                    }
                    break;
                default:
                    break;
            }
        }


        private List<MapFixPointDefine> [] m_guildRssRegins;
        private List<MapFixPointDefine> [] m_villiageCavesRegins;
        private static int PointCellSize = 300;
        private void InitMapPoints()
        {
            var points = CoreUtils.dataService.QueryRecords<MapFixPointDefine>();
            int nSize = 7200 / PointCellSize;
            m_guildRssRegins = new List<MapFixPointDefine>[nSize* nSize];
            m_villiageCavesRegins = new List<MapFixPointDefine>[nSize * nSize];

            for (int i = 0; i < points.Count; i++)
            {
                var item = points[i];
                int nX = item.posX / PointCellSize;
                int nY = item.posY / PointCellSize;
                int nIndex = nX + nY * nSize;
                if(item.group == 2)
                {
                    var listPoints = m_guildRssRegins[nIndex];
                    if (m_guildRssRegins[nIndex] == null)
                    {
                        m_guildRssRegins[nIndex] = listPoints = new List<MapFixPointDefine>();
                    }
                    listPoints.Add(item);
                }
                else if (item.group == 1)
                {
                    var listPoints = m_villiageCavesRegins[nIndex];
                    if (m_villiageCavesRegins[nIndex] == null)
                    {
                        m_villiageCavesRegins[nIndex] = listPoints = new List<MapFixPointDefine>();
                    }
                    listPoints.Add(item);
                }
            }
        }
        
        private void InitLodMenu()
        {
            InitMapPoints();
            m_lodMenuToggles[LodMenuToggle.Alliance] = m_UI_Modle_Ck_guild.m_UI_Model_Ck_MainIFLod_GameToggle;
            m_lodMenuToggles[LodMenuToggle.Explore] = m_UI_Modle_Ck_explore.m_UI_Model_Ck_MainIFLod_GameToggle;
            m_lodMenuToggles[LodMenuToggle.Resoureces] = m_UI_Modle_Ck_res.m_UI_Model_Ck_MainIFLod_GameToggle;
            m_lodMenuToggles[LodMenuToggle.Markers] = m_UI_Modle_Ck_mark.m_UI_Model_Ck_MainIFLod_GameToggle;
            m_lodMenuObjs[LodMenuToggle.Alliance] = m_img_typeGuild_PolygonImage.gameObject;
            m_lodMenuObjs[LodMenuToggle.Explore] = m_img_typeExplore_PolygonImage.gameObject;
            m_lodMenuObjs[LodMenuToggle.Resoureces] = m_img_typeRes_PolygonImage.gameObject;
            m_lodMenuObjs[LodMenuToggle.Markers] = m_img_typeMark_PolygonImage.gameObject;
            m_lodMenuStates[LodMenuToggle.Alliance] = false;
            m_lodMenuStates[LodMenuToggle.Explore] = false;
            m_lodMenuStates[LodMenuToggle.Resoureces] = false;
            m_lodMenuStates[LodMenuToggle.Markers] = false;
            //SetLodMenuToggle(LodMenuToggle.None);
            foreach (var element in m_lodMenuToggles)
            {
                element.Value.isOn = false;
                m_lodMenuObjs[element.Key].gameObject.SetActive(false);
                element.Value.onValueChanged.AddListener((isOn) =>
                {
                    m_lodMenuStates[element.Key] = isOn;
                    OnLodMenuFuntion(element.Key, isOn);
                    m_lodMenuObjs[element.Key].SetActive(isOn);
                    if (isOn)
                    {
                        foreach (var item in m_lodMenuToggles)
                        {
                            if (item.Key != element.Key && item.Value.isOn)
                            {
                                item.Value.isOn = false;
                            }
                        }
                    }
                });
            }
            m_lodMenuToggles[LodMenuToggle.Alliance].isOn = true;
            OnLodMenuFuntion(LodMenuToggle.Alliance, true);
        }
        
        private void OnLodMenuFuntion(LodMenuToggle toggle, bool show)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.OnLodMenuChange, toggle,show.ToString());
            m_currentPos = new Vector2Int(-1, -1);
            switch (toggle)
            {
                case LodMenuToggle.None:
                    break;
                case LodMenuToggle.Alliance:
                    OnAllianceBuilding(show);                    
                    break;
                case LodMenuToggle.Explore:
                    OnCaveAndVillage(show);
                    break;
                case LodMenuToggle.Resoureces:
                    OnRssMiniMap(show);
                    break;
                case LodMenuToggle.Markers:
                    OnPersonMapMarker(show);
                    break;
                default:
                    break;
            }
        }

        private bool lastAllianceToggle = true;
        private bool CurrentAllianceShow;
        public void OnAllianceBuilding(bool show)
        {
            show &= m_lodMenuStates[LodMenuToggle.Alliance];
            CurrentAllianceShow = show;
            if (show)
            {
                lastAllianceToggle = true;
//                if (m_alliancePosInfos.Count <= 0)
//                {
                    m_alliancePosInfos = m_miniMapProxy.MemberPos;
//                }
                
                m_alliancePosInfos.Values.ToList().ForEach((item)=>
                {
                    if (item.HasPos)
                    {
                        int x = (int)item.pos.x / 100;
                        int y = (int)item.pos.y / 100;
                        if (m_alliancePosObj.TryGetValue(item.rid, out var value) && value != null)
                        {
                            bool visable = IsVisable(x, y);
                            value.SetActive(visable);
                            if (visable)
                            {
                                value.transform.position = new Vector3(x, 0, y);
                            }
                        }
                        else if (!m_isLoadingAllianceObj.Contains(item.rid) && IsVisable(x, y))
                        {
                            m_isLoadingAllianceObj.Add(item.rid);
                            CoreUtils.assetService.Instantiate("city_lod3", (go) =>
                            {
                                go.name = string.Format("Lod3_GuildCity_{0}", item.rid);
                                go.transform.SetParent(GetRoot());
                                go.transform.localScale = Vector3.one;
                                go.transform.position = new Vector3(x, 0, y);
                                m_isLoadingAllianceObj.Remove(item.rid);
                                m_alliancePosObj[item.rid] = go;
                                go.SetActive(IsVisable(x, y) && CurrentAllianceShow);
                                var lod = go.GetComponent<LevelDetailScale>();
                                if (lod)
                                {
                                    lod.IsExplore = true;
                                    lod.UpdateLod();
                                }

                                ChangeSpriteColor colorHelper = go.GetComponent<ChangeSpriteColor>();
                                if (colorHelper != null)
                                {
                                    Color color = RS.blue;

                                    if (m_allianceProxy.HasJionAlliance() && m_allianceProxy.GetAlliance().leaderRid == item.rid)
                                    {
                                        color = RS.purple;
                                    }
                                    ChangeSpriteColor.SetColor(colorHelper, color);
                                }

                            });
                        }

                    }

                });
            }
            else if(lastAllianceToggle)
            {
                lastAllianceToggle = false;
                ClearAlliancePos();
            }
        }

        private void ClearAlliancePos()
        {
            m_alliancePosObj.Values.ToList().ForEach((value) =>
            {
                CoreUtils.assetService.Destroy(value);
            });
            m_alliancePosObj.Clear();
        }
//        private void UpdateAllianceMember(Guild_GuildMemberPos.request req)
//        {
//            if (req == null)
//            {
//                return;
//            }
//            if (req.HasMemberPos)
//            {
//                long leaderRid = m_allianceProxy.GetAlliance().leaderRid;
//                req.memberPos.Values.ToList().ForEach((item) =>
//                {
//                    if (item.rid == m_playerProxy.Rid)
//                    {
//                        return;
//                    }
//                    if (m_alliancePosInfos.ContainsKey(item.rid))
//                    {
//                        m_alliancePosInfos[item.rid] = item;
//                    }
//                    else
//                    {
//                        m_alliancePosInfos.Add(item.rid,item);
//                    }
//                });
//            }
//
//            if (req.HasDeleteRid)
//            {
//                m_alliancePosInfos.Remove(req.deleteRid);
//            }
//            OnAllianceBuilding(m_lodMenuStates[LodMenuToggle.Alliance]);
//        }

        private void DeleteAlliance()
        {
            m_alliancePosObj.Values.ToList().ForEach((value) =>
            {
                if (value)
                {
                    CoreUtils.assetService.Destroy(value);
                }
            });
            m_alliancePosObj.Clear();
            m_alliancePosInfos.Clear();
        }

        private bool lastCaveToggle = true;
        private bool CurrentCaveShow = false;
        public void OnCaveAndVillage(bool show)
        {
           show &= m_lodMenuStates[LodMenuToggle.Explore];
            CurrentCaveShow = show;
            if (show)
            {
                lastCaveToggle = true;
                
                if (WorldCamera.Instance().IsAutoMoving())
                    return;

                var center = WorldCamera.Instance().GetViewCenter();

                int nX = (int)(center.x) / PointCellSize;
                int nY = (int)(center.y) / PointCellSize;

                var centerPos = new Vector2Int(nX, nY);
                if (m_currentPos.Equals(centerPos))
                    return;

                m_currentPos = centerPos;
                int nSize = 7200 / PointCellSize;

                m_setShowRss.Clear();
                // 九宫范围内的资源点检索
                for (int i = nX - 1; i < nSize && i <= nX + 1; i++)
                {
                    if (i < 0)
                        continue;

                    for (int j = nY - 1; j < nSize && j <= nY + 1; j++)
                    {
                        if (j < 0)
                            continue;

                        var listPoints = m_villiageCavesRegins[i + j * nSize];

                        for (int k = 0; k < listPoints.Count; k++)
                        {
                            var item = listPoints[k];
                            m_setShowRss.Add(item.ID, item);
                        }
                    }
                }

                foreach (var element in new Dictionary<int, GameObject>(m_villiageCaveObj))
                {
                    if (!m_setShowRss.ContainsKey(element.Key))
                    {
                        CoreUtils.assetService.Destroy(element.Value);
                        m_villiageCaveObj.Remove(element.Key);
                    }
                }

                foreach (var itemElem in m_setShowRss)
                {
                    var item = itemElem.Value;
                    if (!m_villiageCaveObj.ContainsKey(item.ID) && !m_isLoadingVilliageCaveObj.Contains(item.ID))
                    {
                        m_isLoadingVilliageCaveObj.Add(item.ID);
                        CoreUtils.assetService.Instantiate(GetResName(item.type), (go) =>
                        {
                            if (!m_isLoadingVilliageCaveObj.Contains(item.ID))
                            {
                                CoreUtils.assetService.Destroy(go);
                                return;
                            }
                            m_isLoadingVilliageCaveObj.Remove(item.ID);
                            if (!m_setShowRss.ContainsKey(item.ID))
                            {
                                CoreUtils.assetService.Destroy(go);
                                return;
                            }
                            go.name = string.Format("Lod3_VilliageCaves_{0}", item.ID);
                            go.transform.SetParent(GetRoot());
                            go.transform.localScale = Vector3.one;
                            go.transform.position = new Vector3(item.posX, 0, item.posY);
                            m_isLoadingVilliageCaveObj.Remove(item.ID);
                            m_villiageCaveObj[item.ID] = go;
                            //go.SetActive(IsVisable(item.posX, item.posY) && CurrentCaveShow);
                            var image = go.GetComponent<SpriteRenderer>();
                            if (image)
                            {
                                string icon = GetResIcon(item.type, item.ID);
                                ClientUtils.LoadSprite(image, icon);
                            }
                            var lod = go.GetComponent<LevelDetailScale>();
                            if (lod)
                            {
                                lod.IsExplore = true;
                                lod.UpdateLod();
                            }
                        });

                    }
                }
            }
            else if(lastCaveToggle)
            {
                lastCaveToggle = false;
                m_villiageCaveObj.Values.ToList().ForEach((value)=>
                {
                    CoreUtils.assetService.Destroy(value);
                });
                m_villiageCaveObj.Clear();
                m_isLoadingVilliageCaveObj.Clear();
                m_currentPos = new Vector2Int(-1, -1);
            }
        }

        private bool personMapMarkerPointFlag = false;
        private bool lastMapMarkerToggle = true;
        private Dictionary<long, GameObject> m_personMapMarkerObj = new Dictionary<long, GameObject>();
        private Dictionary<long, MapMarkerInfo> m_setShowMapMarker = new Dictionary<long, MapMarkerInfo>();
        private List<long> m_isLoadingMapMarkerObj = new List<long>();
        private List<MapMarkerInfo>[] m_personMapMarkerRegins;
        private void InitPersonMapMarkerPoints()
        {
            if (personMapMarkerPointFlag)
            {
                return;
            }

            Dictionary<long, MapMarkerInfo> personMapMarkerInfoDic = m_mapMarkerProxy.GetPersonMapMarkerInfoDic();
            int nSize = 7200 / PointCellSize;
            m_personMapMarkerRegins = new List<MapMarkerInfo>[nSize * nSize];

            foreach (var personMapMarkerInfo in personMapMarkerInfoDic.Values)
            {
                int nX = (int)PosHelper.ServerUnitToClientUnit(personMapMarkerInfo.pos.x) / PointCellSize;
                int nY = (int)PosHelper.ServerUnitToClientUnit(personMapMarkerInfo.pos.y) / PointCellSize;
                int nIndex = nX + nY * nSize;

                var mapMarkerList = m_personMapMarkerRegins[nIndex];
                if (m_personMapMarkerRegins[nIndex] == null)
                {
                    m_personMapMarkerRegins[nIndex] = mapMarkerList = new List<MapMarkerInfo>();
                }
                mapMarkerList.Add(personMapMarkerInfo);
            }

            personMapMarkerPointFlag = true;
        }
        public void OnPersonMapMarker(bool show)
        {
            show &= m_lodMenuStates[LodMenuToggle.Markers];
            if (show)
            {
                lastMapMarkerToggle = true;

                InitPersonMapMarkerPoints();

                if (WorldCamera.Instance().IsAutoMoving())
                    return;

                var center = WorldCamera.Instance().GetViewCenter();

                int nX = (int)(center.x) / PointCellSize;
                int nY = (int)(center.y) / PointCellSize;

                var centerPos = new Vector2Int(nX, nY);
                if (m_currentPos.Equals(centerPos))
                    return;

                m_currentPos = centerPos;
                int nSize = 7200 / PointCellSize;

                m_setShowMapMarker.Clear();
                for (int i = nX - 1; i < nSize && i <= nX + 1; i++)
                {
                    if (i < 0)
                        continue;

                    for (int j = nY - 1; j < nSize && j <= nY + 1; j++)
                    {
                        if (j < 0)
                            continue;

                        var mapMarkerList = m_personMapMarkerRegins[i + j * nSize];
                        if (mapMarkerList != null)
                        {
                            for (int k = 0; k < mapMarkerList.Count; k++)
                            {
                                var mapMarker = mapMarkerList[k];
                                m_setShowMapMarker.Add(mapMarker.markerIndex, mapMarker);
                            }
                        }                        
                    }
                }

                foreach (var element in new Dictionary<long, GameObject>(m_personMapMarkerObj))
                {
                    if (!m_setShowMapMarker.ContainsKey(element.Key))
                    {
                        CoreUtils.assetService.Destroy(element.Value);
                        m_personMapMarkerObj.Remove(element.Key);
                    }
                }

                foreach (var itemElem in m_setShowMapMarker)
                {
                    var itemKey = itemElem.Key;
                    var itemValue = itemElem.Value;

                    if (!m_personMapMarkerObj.ContainsKey(itemKey) && !m_isLoadingMapMarkerObj.Contains(itemKey))
                    {
                        m_isLoadingMapMarkerObj.Add(itemKey);

                        CoreUtils.assetService.Instantiate("book_lod3", (go) =>
                        {
                            if (!m_isLoadingMapMarkerObj.Contains(itemKey))
                            {
                                CoreUtils.assetService.Destroy(go);
                                return;
                            }
                            m_isLoadingMapMarkerObj.Remove(itemKey);
                            if (!m_setShowMapMarker.ContainsKey(itemKey))
                            {
                                CoreUtils.assetService.Destroy(go);
                                return;
                            }
                            go.name = string.Format("Lod3_MapMarker_{0}", itemKey);
                            go.transform.SetParent(GetRoot());
                            go.transform.localScale = Vector3.one;
                            go.transform.position = new Vector3(PosHelper.ServerUnitToClientUnit(itemValue.pos.x), 0, PosHelper.ServerUnitToClientUnit(itemValue.pos.y));
                            m_personMapMarkerObj[itemKey] = go;
                            var image = go.GetComponent<SpriteRenderer>();
                            if (image)
                            {
                                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)itemValue.markerId);
                                if (mapMarkerTypeDefine != null)
                                {
                                    ClientUtils.LoadSprite(image, mapMarkerTypeDefine.iconImg);
                                }                                
                            }
                            var lod = go.GetComponent<LevelDetailScale>();
                            if (lod)
                            {
                                lod.IsExplore = true;
                                lod.UpdateLod();
                            }
                        });
                    }
                }
            }
            else if (lastMapMarkerToggle)
            {
                lastMapMarkerToggle = false;
                personMapMarkerPointFlag = false;

                m_personMapMarkerObj.Values.ToList().ForEach((value) =>
                {
                    CoreUtils.assetService.Destroy(value);
                });

                m_personMapMarkerObj.Clear();
                m_isLoadingMapMarkerObj.Clear();
                m_personMapMarkerRegins = null;
                m_currentPos = new Vector2Int(-1, -1);
            }
        }

        private string GetResName(int type)
        {

            switch (type)
            {
                case 4:
                    return "alliance_food_lod3";
                case 5:
                    return "alliance_wood_lod3";
                case 6:
                    return "alliance_stone_lod3";
                case 7:
                    return "alliance_coin_lod3";
                case 8:
                    return "alliance_super_mine_food_lod3";
                case 9:
                    return "alliance_super_mine_wood_lod3";
                case 10:
                    return "alliance_super_mine_stone_lod3";
                case 11:
                    return "alliance_super_mine_coin_lod3";
            }
            
            if(type<70000)
            {
                return "cave_lod3";
            }
            else
            {
                return "village_lod3";
            }
        }

        private string GetResIcon(int type,int id)
        {
            int index = Mathf.CeilToInt(id / 64f);
            bool open = false;
            if(m_playerProxy.CurrentRoleInfo!=null&& m_playerProxy.CurrentRoleInfo.villageCaves!=null&& m_playerProxy.CurrentRoleInfo.villageCaves.ContainsKey(index))
            {
                open = (ulong)(m_playerProxy.CurrentRoleInfo.villageCaves[index].rule & (1L << (id % 64))) != 0;
            }
            if (type<70000)
            {
                return open ? RS.cave_lod3_icon[1] : RS.cave_lod3_icon[0];
            }
            else
            {
                return open ? RS.village_lod3_icon[1] : RS.village_lod3_icon[0];
            }
        }

        private GameObject m_rssMinimap;
        private bool rssMinimapIsLoading;


        private Vector2Int m_currentPos = new Vector2Int(-1, -1);

        private bool lastRssToggle = true;
        private bool CurrentRssShow = false;
        private Dictionary<int, MapFixPointDefine> m_setShowRss = new Dictionary<int, MapFixPointDefine>();
        public void OnRssMiniMap(bool show)
        {
            show &= m_lodMenuStates[LodMenuToggle.Resoureces];
            CurrentRssShow = show;
            if (show)
            {
                lastRssToggle = true;
                if (m_rssMinimap != null)
                {
                    if (!m_rssMinimap.activeSelf)
                        m_rssMinimap.SetActive(true);
                }
                else if(!rssMinimapIsLoading)
                {
                    rssMinimapIsLoading = true;
                    CoreUtils.assetService.Instantiate("resource_minimap", (go) =>
                    {
                        rssMinimapIsLoading = false;
                        m_rssMinimap = go;
                        m_rssMinimap.transform.SetParent(GameObject.Find("SceneObject/lod3_root").transform);
                        m_rssMinimap.transform.localPosition = new Vector3(3600, 0, 3600);
                    });
                }

                if (WorldCamera.Instance().IsAutoMoving())
                    return;

                var center = WorldCamera.Instance().GetViewCenter();

                int nX = (int)(center.x) / PointCellSize;
                int nY = (int)(center.y) / PointCellSize;

                var centerPos = new Vector2Int(nX, nY);
                if (m_currentPos.Equals(centerPos))
                    return;

                m_currentPos = centerPos;
                int nSize = 7200 / PointCellSize;

                m_setShowRss.Clear();
                // 九宫范围内的资源点检索
                for (int i = nX - 1; i < nSize && i <= nX + 1; i++)
                {
                    if (i < 0)
                        continue;

                    for (int j = nY - 1; j < nSize && j <= nY + 1; j++)
                    {
                        if (j < 0)
                            continue;

                        var listPoints = m_guildRssRegins[i + j * nSize];

                        for (int k = 0; k < listPoints.Count; k++)
                        {
                            var item = listPoints[k];
                            m_setShowRss.Add(item.ID, item);
                        }
                    }
                }

                foreach (var element in new Dictionary<int, GameObject>(m_guildRssObj))
                {
                    if(!m_setShowRss.ContainsKey(element.Key))
                    {
                        CoreUtils.assetService.Destroy(element.Value);
                        m_guildRssObj.Remove(element.Key);
                    }
                }

                foreach(var itemElem in m_setShowRss)
                {
                    var item = itemElem.Value;
                    if(!m_guildRssObj.ContainsKey(item.ID) && !m_isLoadingRssObj.Contains(item.ID))
                    {
                        m_isLoadingRssObj.Add(item.ID);
                        CoreUtils.assetService.Instantiate(GetResName(item.type), (go) =>
                        {
                            if (!m_isLoadingRssObj.Contains(item.ID))
                            {
                                CoreUtils.assetService.Destroy(go);
                                return;
                            }
                            m_isLoadingRssObj.Remove(item.ID);
                            if (!m_setShowRss.ContainsKey(item.ID))
                            {
                                CoreUtils.assetService.Destroy(go);
                                return;
                            }
                            go.name = string.Format("Lod3_GuildRss_{0}", item.ID);
                            go.transform.SetParent(GetRoot());
                            go.transform.localScale = Vector3.one;
                            go.transform.position = new Vector3(item.posX, 0, item.posY);
                            m_guildRssObj[item.ID] = go;
                            //go.SetActive(IsVisable(item.posX, item.posY));
                            var lod = go.GetComponent<LevelDetailScale>();
                            if (lod)
                            {
                                lod.IsExplore = true;
                                lod.UpdateLod();
                            }
                        });

                    }
                }
            }
            else
            {
                if (m_rssMinimap != null)
                    m_rssMinimap.SetActive(false);


                if (lastRssToggle)
                {
                    lastRssToggle = false;
                    m_guildRssObj.Values.ToList().ForEach((value)=>
                    {
                        CoreUtils.assetService.Destroy(value);
                    });
                    m_guildRssObj.Clear();
                    m_isLoadingRssObj.Clear();
                }
                m_currentPos = new Vector2Int(-1, -1);
            }
        }

        private bool IsVisable(int x,int y)
        {
            if (FogSystemMediator != null && Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), x, y))
            {
                if (!FogSystemMediator.HasFogAtWorldPos(x, y))
                {
                    return true;
                }
            }
            return false;
        }

        private Transform GetRoot()
        {
            if (this.m_root == null)
            {
                this.m_root = GameObject.Find(m_root_path).transform;
            }

            return this.m_root;
        }

    }
}
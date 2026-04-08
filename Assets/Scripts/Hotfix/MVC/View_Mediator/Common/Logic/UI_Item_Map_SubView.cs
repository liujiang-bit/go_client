// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月29日
// Update Time         :    2020年4月29日
// Class Description   :    UI_Item_Map_SubView
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
using System.Linq;

namespace Game {

    public enum EnumMinimapType
    {
        MyCity, //自身
        Alliance, //盟友
        Lord, //盟主
        Mark, //联盟标记
    }

    public class MinimapObj
    {
        public EnumMinimapType type;
        public GameObject go;
        public PosInfo worldPos;
        public bool bIsGoLoaded;
        public bool bDispose;
    }



    public partial class UI_Item_Map_SubView : UI_SubView
    {
        private float worldMaxX;
        private float worldMaxY;
        private Vector2[] m_minimapUVs;
        private MinimapObj m_minimapMyCityObj;
        private Dictionary<long, MinimapObj> m_allianceObj = new Dictionary<long, MinimapObj>();

        private CityBuildingProxy m_cityBuildingProxy;
        private AllianceProxy m_allianceProxy;
        private MinimapProxy m_miniMapProxy;
        private PlayerProxy m_playerProxy;
        
        private Dictionary<long, MemberPosInfo> m_alliancePosInfos = new Dictionary<long, MemberPosInfo>();

        private Color m_cityColor;

        private Color m_allianceMasterColor;

        private Color m_allianceColor;

        protected override void BindEvent()
        {
            base.BindEvent();
            ColorUtility.TryParseHtmlString("#64d37c", out  m_cityColor);
            ColorUtility.TryParseHtmlString("#C630DE", out  m_allianceMasterColor);
            ColorUtility.TryParseHtmlString("#52AEEE", out  m_allianceColor);
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_miniMapProxy = AppFacade.GetInstance().RetrieveProxy(MinimapProxy.ProxyNAME) as MinimapProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;


            SubViewManager.Instance.AddListener(new string[] {
                CmdConstant.ChangeRolePos,
                CmdConstant.AllianceEixt,
                Guild_GuildMemberPos.TagName,
                CmdConstant.OnCityLoadFinished,
                CmdConstant.ChangeRolePosGuide,
                CmdConstant.RefreshGuildMemberPosView,
                CmdConstant.DeleteMiniMapGuildMemberPos,
        }, this.gameObject, OnNotification);
            InitMiniMap();
        }

        private void OnNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ChangeRolePos:
                case CmdConstant.OnCityLoadFinished:
                case CmdConstant.ChangeRolePosGuide:
                    RefreshMinimapMyCityPos();
                    break;
                case CmdConstant.AllianceEixt:
                    {
                        DeleteAlliance();
                    }
                    break;
//                case Guild_GuildMemberPos.TagName:
//                    {
//                        RefreshAlliancePos(notification.Body as Guild_GuildMemberPos.request);
//                    }
//                    break;
                case CmdConstant.RefreshGuildMemberPosView:
                    {
                        RefreshAlliancePosInfo();
                    }
                    break;
                case CmdConstant.DeleteMiniMapGuildMemberPos:
                    long rid = (long)notification.Body;
                    DeleteAlliancePosInfoByRid(rid);
                    break;
                default:
                    break;
            }
        }

        private void InitMiniMap()
        {
            m_et_minimap_UIEventTrigger.onDrag = OnMinimapDrag;
            m_et_minimap_UIEventTrigger.onPointerUp = OnMinimapEndDrag;
            m_et_minimap_UIEventTrigger.onPointerDown = OnMinimapBeginDrag;
            worldMaxX = (float)WorldCamera.Instance().worldMaxX;
            worldMaxY = (float)WorldCamera.Instance().worldMaxY;
            m_minimapUVs = new Vector2[4] { new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0) };
            RefreshMinimapMyCityPos();
        }

        public void RefreshMinimapViewArea()
        {
            Vector3[] cornors = Common.GetCameraCornors(WorldCamera.Instance().GetCamera(),
    WorldCamera.Instance().GetTerrainPlane);
            Vector2[] poses = new Vector2[4];
            for (int i = 3; i >= 0; i--)
            {
                float x = cornors[i].x / worldMaxX;
                float y = cornors[i].z / worldMaxY;
                if (x < 0)
                    x = 0;
                if (x > 1)
                    x = 1;
                if (y < 0)
                    y = 0;
                if (y > 1)
                    y = 1;
                poses[i] = new Vector2(x, y);
            }

            m_map_minimapViewPort_MiniMapImage.lineWidth = 2.5f;
            m_map_minimapViewPort_MiniMapImage.SetUvs(m_minimapUVs);
            m_map_minimapViewPort_MiniMapImage.SetPos(poses);
            
            
            RefreshAlliancePosInfo();
        }

        private void RefreshMinimapMyCityPos()
        {
            if (m_minimapMyCityObj == null)
            {
                m_minimapMyCityObj = new MinimapObj();
                m_minimapMyCityObj.type = EnumMinimapType.MyCity;
                CoreUtils.assetService.Instantiate("UI_Item_MapMarkSelf", (go) =>
                {
                    m_minimapMyCityObj.bIsGoLoaded = true;
                    m_minimapMyCityObj.go = go;
                    PolygonImage image = m_minimapMyCityObj.go.GetComponent<PolygonImage>();
                    if (image != null)
                        image.color = m_cityColor;

                    m_minimapMyCityObj.go.transform.SetParent(m_pl_self_ArabLayoutCompment.GetComponent<RectTransform>());
                    m_minimapMyCityObj.go.transform.localScale = Vector3.one;
                    if (m_playerProxy.CurrentRoleInfo!=null)
                    {
                        m_minimapMyCityObj.worldPos = m_playerProxy.CurrentRoleInfo.pos;
                        SetMinimapPosBySelfPos(m_minimapMyCityObj);
                    }
                });
            }
            else if(m_playerProxy.CurrentRoleInfo!=null)
            {
                m_minimapMyCityObj.worldPos = m_playerProxy.CurrentRoleInfo.pos;
                SetMinimapPosBySelfPos(m_minimapMyCityObj);
            }
        }

        private void DeleteAlliancePosInfoByRid(long rid)
        {
            if (m_allianceObj.ContainsKey(rid))
            {
                if (m_allianceObj[rid].go != null)
                {
                    GameObject.DestroyImmediate(m_allianceObj[rid].go);
                }
                m_allianceObj.Remove(rid);
            }
        }

        private void RefreshAlliancePosInfo()
        {
//            if (!m_allianceProxy.HasJionAlliance())
//            {
//                return;
//            }
            m_alliancePosInfos = m_miniMapProxy.MemberPos;
            if (m_alliancePosInfos.Values.Count <= 0)
            {
                return;
            }
            long leaderRid = m_allianceProxy.GetAlliance().leaderRid;
            m_alliancePosInfos.Values.ToList().ForEach((item) =>
            {
                if (item.rid == m_playerProxy.Rid)
                {
                    return;
                }
                if (m_allianceObj.TryGetValue(item.rid, out MinimapObj value))
                {
                    if (!value.bIsGoLoaded)
                    {
                        return;
                    }
                    value.worldPos = item.pos;
                    PolygonImage image = m_allianceObj[item.rid].go.GetComponent<PolygonImage>();
                    if (image != null)
                    {
                        if (item.rid != leaderRid)
                        {
                            image.color = m_allianceColor;
                        }
                        else
                        {
                            image.color = m_allianceMasterColor;
                        }
                    }
                    SetMinimapPosByWorldPos(m_allianceObj[item.rid]);
                }
                else
                {
                    m_allianceObj.Add(item.rid,new MinimapObj());
                    m_allianceObj[item.rid].worldPos = item.pos;
                    CoreUtils.assetService.Instantiate("UI_Item_MapMark", (go) =>
                    {
                        if (m_allianceObj.ContainsKey(item.rid))
                        {
                            m_allianceObj[item.rid].bIsGoLoaded = true;
                            m_allianceObj[item.rid].go = go;
                            if (item.rid != leaderRid)
                            {
                                m_allianceObj[item.rid].go.transform
                                    .SetParent(m_pl_guild_ArabLayoutCompment.GetComponent<RectTransform>());
                            }
                            else
                            {
                                m_allianceObj[item.rid].go.transform
                                    .SetParent(m_pl_master_ArabLayoutCompment.GetComponent<RectTransform>());
                            }
                            m_allianceObj[item.rid].go.transform.localScale = Vector3.one;
                            PolygonImage image = m_allianceObj[item.rid].go.GetComponent<PolygonImage>();
                            if (image != null)
                            {
                                if (item.rid != leaderRid)
                                {
                                    image.color = m_allianceColor;
                                }
                                else
                                {
                                    image.color = m_allianceMasterColor;
                                }
                            }
                            SetMinimapPosByWorldPos(m_allianceObj[item.rid]);
                        }
                        else
                        {
                            GameObject.Destroy(go);
                        }
                    });
                }
            });
        }

//        private void RefreshAlliancePos(Guild_GuildMemberPos.request req)
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
//                    if (m_allianceObj.TryGetValue(item.rid, out MinimapObj value))
//                    {
//                        if (!value.bIsGoLoaded)
//                        {
//                            return;
//                        }
//                        value.worldPos = item.pos;
//                        PolygonImage image = m_allianceObj[item.rid].go.GetComponent<PolygonImage>();
//                        if (image != null)
//                        {
//                            if (item.rid != leaderRid)
//                            {
//                                image.color = m_allianceColor;
//                            }
//                            else
//                            {
//                                image.color = m_allianceMasterColor;
//                            }
//                        }
//                        SetMinimapPosByWorldPos(m_allianceObj[item.rid]);
//                    }
//                    else
//                    {
//                        m_allianceObj.Add(item.rid,new MinimapObj());
//                        m_allianceObj[item.rid].worldPos = item.pos;
//                        CoreUtils.assetService.Instantiate("UI_Item_MapMark", (go) =>
//                        {
//                            if (m_allianceObj.ContainsKey(item.rid))
//                            {
//                                m_allianceObj[item.rid].bIsGoLoaded = true;
//                                m_allianceObj[item.rid].go = go;
//                                m_allianceObj[item.rid].go.transform.SetParent(m_pl_guild_ArabLayoutCompment.GetComponent<RectTransform>());
//                                m_allianceObj[item.rid].go.transform.localScale = Vector3.one;
//                                PolygonImage image = m_allianceObj[item.rid].go.GetComponent<PolygonImage>();
//                                if (image != null)
//                                {
//                                    if (item.rid != leaderRid)
//                                    {
//                                        image.color = m_allianceColor;
//                                    }
//                                    else
//                                    {
//                                        image.color = m_allianceMasterColor;
//                                    }
//                                }
//                                SetMinimapPosByWorldPos(m_allianceObj[item.rid]);
//                            }
//                            else
//                            {
//                                GameObject.Destroy(go);
//                            }
//                        });
//                    }
//                });
//            }
//
//            if(req.HasDeleteRid)
//            {
//                if(m_allianceObj.TryGetValue(req.deleteRid,out var value))
//                {
//                    GameObject.Destroy(value.go);
//                    m_allianceObj.Remove(req.deleteRid);
//                }
//            }
//        }

        private void DeleteAlliance()
        {
            m_allianceObj.Values.ToList().ForEach((value)=>
            {
                if(value.go)
                {
                    GameObject.DestroyImmediate(value.go);
                }
            });
            m_allianceObj.Clear();
            m_alliancePosInfos.Clear();
        }

        private void SetMinimapPosBySelfPos(MinimapObj obj)
        {
            if (obj.bDispose || !obj.bIsGoLoaded||obj.worldPos==null)
            {
                return;
            }
            long x = obj.worldPos.x / 100;
            long y = obj.worldPos.y / 100;
            Vector2 sizeDelta = m_map_minimapViewPort_MiniMapImage.rectTransform.sizeDelta;
            float xPos = (x / worldMaxX - 0.5f) * sizeDelta.x;
            float yPos = (y / worldMaxY - 0.5f) * sizeDelta.y;
            obj.go.transform.localPosition = new Vector3(xPos, yPos, 0);
        }

        private void SetMinimapPosByWorldPos(MinimapObj obj)
        {
            if (obj.bDispose || !obj.bIsGoLoaded || obj.worldPos == null)
            {
                return;
            }

            Vector2 sizeDelta = m_map_minimapViewPort_MiniMapImage.rectTransform.sizeDelta;
            float x = ((obj.worldPos.x /100f)/ worldMaxX - 0.5f) * sizeDelta.x;
            float y = ((obj.worldPos.y / 100f) / worldMaxY - 0.5f) * sizeDelta.y;
            obj.go.transform.localPosition = new Vector3(x, y, 0);
        }

        private void OnMinimapBeginDrag(PointerEventData pointerEventData)
        {
            WorldCamera.Instance().AllowTouchWhenMovingOrZooming = true;
            OnMinimapDrag(pointerEventData);
        }


        private void OnMinimapDrag(PointerEventData pointerEventData)
        {
            Vector2 pos = GetMinimapTouchPos(pointerEventData.position);
            Vector2 sizeDelta = m_map_minimapViewPort_MiniMapImage.rectTransform.sizeDelta;
            pos = (pos + sizeDelta / 2) / sizeDelta;
            WorldCamera.Instance().ViewTerrainPos(pos.x * worldMaxX, pos.y * worldMaxY, 0f, null);
        }

        private void OnMinimapEndDrag(PointerEventData pointerEventData)
        {
            WorldCamera.Instance().AllowTouchWhenMovingOrZooming = false;
            OnMinimapDrag(pointerEventData);
        }

        private Vector2 GetMinimapTouchPos(Vector2 screenPoint)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                m_map_minimapViewPort_MiniMapImage.rectTransform, screenPoint, CoreUtils.uiManager.GetUICamera(),
                out pos);
            return pos;
        }
    }
}
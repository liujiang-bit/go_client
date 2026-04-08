using System.Collections.Generic;
using Client;
using DG.Tweening;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public class MapTouchHandler : IMapTouchHandler
    {
        private WorldMapObjectProxy m_WorldMapObjectProxy;
        private PlayerProxy m_PlayerProxy;
        private AllianceProxy m_allianceProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private MapObjectInfoEntity infoEntity;
        private readonly Dictionary<int, HUDUI> dicHudui = new Dictionary<int, HUDUI>();
        private int lastObjectId = 0;
        private Vector2 pos= Vector2.zero;
        private Vector2 targetSpacePos= Vector2.zero;
        private HUDUI huduiSpace;
        private UI_Tip_ComBuildStateView view;

        public void Init()
        {
            m_WorldMapObjectProxy =
                AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_RallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
        }

        public void Clear()
        {
            Close();
            lastObjectId = 0;
        }

        public void Play(int objectId, Vector2 pos)
        {        
            GameObject go = null;
            infoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(objectId);
            if (infoEntity == null)
            {
                return;
            }

            CloseHUDUISpace();
            if (lastObjectId != objectId)
            {
                Close(false);
            }

            if (dicHudui.ContainsKey(objectId))
            {
                return;
            }

            go = infoEntity.gameobject;
            if (TroopHelp.IsTroopType(infoEntity.rssType))
            {
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(objectId);
                if (armyData != null)
                {
                    go = armyData.go;
                }
            }

            this.pos = pos;
            HUDUI hudui = HUDUI
                .Register(UI_Tip_ComBuildStateView.VIEW_NAME, typeof(UI_Tip_ComBuildStateView),
                    HUDLayer.world, go).SetInitCallback(OnHudCallBack)
                .SetUpdateCallback(OnHudUpdateCallBack, 1f).SetTargetGameObject(go)
                .SetPositionAutoAnchor(true)
                .SetCameraLodDist(0, 3000f);
            ClientUtils.hudManager.ShowHud(hudui);
            dicHudui[objectId] = hudui;
            lastObjectId = objectId;
        }

        public void Play(Vector2 pos,Vector2 targetPos)
        {
            if (dicHudui.Count > 0)
            {
                return;
            }

            Vector3 v3= new Vector3(targetPos.x,0,targetPos.y);
            this.pos = pos;
            this.targetSpacePos = targetPos;
            if (huduiSpace != null)
            {
                huduiSpace.UpdateTargetPos(v3);
                UpdateHudCallBackSpace();
                //ClientUtils.hudManager.CloseSingleHud(ref huduiSpace);
                return;
            }

            huduiSpace= HUDUI.Register(UI_Tip_ComBuildStateView.VIEW_NAME,v3,HUDLayer.world).SetInitCallback(OnHudCallBackBySpace)
                .SetPositionAutoAnchor(true)
                .SetCameraLodDist(0, 3000f);
            ClientUtils.hudManager.ShowHud(huduiSpace);
        }
        

        public void Stop()
        {
            Close();
        }

        public void StopSpace()
        {
            CloseHUDUISpace();
        }

        private void UpdateHudCallBackSpace()
        {
            if (view != null)
            {
                GetDistance(view, this.targetSpacePos);
            }
        }

        private void OnHudCallBackBySpace(HUDUI info)
        {
            view = MonoHelper.GetOrAddHotFixViewComponent<UI_Tip_ComBuildStateView>(info.uiObj); 
            if (view != null)
            {
                view.gameObject.GetComponent<MapElementUI>().enabled = false;
                view.gameObject.name =
                    string.Format("{0}_{1}_{2}", UI_Pop_WorldArmyCmdView.VIEW_NAME, "MapTouchHandlerSpace",
                        -1000);
                string icon = "ui_map[img_map_state_stop1]";
                ClientUtils.LoadSprite(view.m_lbl_icon_PolygonImage, icon);
                GetDistance(view, this.targetSpacePos);
            }
        }


        private void OnHudCallBack(HUDUI info)
        {
            UI_Tip_ComBuildStateView view = info.gameView as UI_Tip_ComBuildStateView;
            if (view != null && infoEntity != null)
            {
                view.gameObject.GetComponent<MapElementUI>().enabled = false;
                view.gameObject.name =
                    string.Format("{0}_{1}_{2}", UI_Pop_WorldArmyCmdView.VIEW_NAME, "MapTouchHandler",
                        infoEntity.objectId);
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr()
                    .GetArmyData((int) infoEntity.objectId);
                bool isGuild = m_PlayerProxy.CurrentRoleInfo.guildId != 0 &&
                               m_PlayerProxy.CurrentRoleInfo.guildId == infoEntity.guildId;
                string icon = string.Empty;
                if (TroopHelp.IsCollectType(infoEntity.rssType)) //采集
                {
                    if (infoEntity.collectRid != 0)
                    {
                        GuildMemberInfoEntity guildMemberInfoEntity =
                            m_allianceProxy.getMemberInfo(infoEntity.collectRid);
                        if (guildMemberInfoEntity != null|| m_PlayerProxy.Rid==infoEntity.collectRid) //联盟成员或者自己的部队在采集
                        {
                            icon = "ui_map[img_map_state_collect1]";
                        }
                        else //其他联盟在采集可攻击
                        {
                            icon = "ui_map[img_map_state_war]";
                        }
                    }
                }
                else if (infoEntity.rssType == RssType.City) //城市
                {
                    if (m_WorldMapObjectProxy.MyCityId == infoEntity.objectId) //自己的城市
                    {
                        icon = "ui_map[img_map_state_back]";
                    }
                    else if (isGuild) //盟友的城市
                    {
                        icon = "ui_map[img_map_state_massing]";
                    }
                    else //可攻击的城市
                    {
                        icon = "ui_map[img_map_state_war]";
                    }
                }
                else if (armyData != null && armyData.isRally) //集结部队
                {
                    if (m_RallyTroopsProxy.HasRallyed(armyData.targetId))
                    {
                        icon = "ui_map[img_map_state_war]";
                    }
                    else if (m_RallyTroopsProxy.IsCaptainByarmIndex(armyData.troopId))
                    {
                        icon = "ui_map[img_map_state_stop1]";
                    }
                    else if (m_RallyTroopsProxy.isRallyTroopHaveGuid(armyData.armyRid))
                    {
                        icon = "ui_map[img_map_state_stop1]";
                    }
                    else
                    {
                        icon = "ui_map[img_map_state_war]";
                    }
                }
                else if (isGuild) //可增援
                {
                    if (TroopHelp.IsTroopType(infoEntity.rssType)) //盟友的的部队伺候运输车
                    {
                        icon = "ui_map[img_map_state_stop2]";
                    }
                    else if (TroopHelp.IsTouchGuildRss(infoEntity.rssType)) //自己的联盟资源中心是向空地行军类型
                    {
                        icon = "ui_map[img_map_state_stop1]";
                    }
                    else
                    {                  
                        icon = "ui_map[img_map_state_massing]";   
                    }
                }else if (armyData != null&&armyData.isPlayerHave) //自己的部队
                {
                    icon = "ui_map[img_map_state_stop1]";
                }else if (TroopHelp.IsTouchMoveAllianceBuilding(infoEntity.rssType))  //联盟资源向空地行军类型
                {
                    icon = "ui_map[img_map_state_stop1]";
                }
                else //可攻击
                {
                    icon = "ui_map[img_map_state_war]";
                }
                
                ClientUtils.LoadSprite(view.m_lbl_icon_PolygonImage, icon);
                
                Vector2 targetPos= Vector2.zero;
                if (armyData != null)
                {
                    targetPos= new Vector2(armyData.go.transform.position.x,armyData.go.transform.position.z);
                }
                else
                {
                    targetPos= new Vector2(infoEntity.gameobject.transform.position.x,infoEntity.gameobject.transform.position.z);
                }

                GetDistance(view, targetPos);
            }
        }

        private void GetDistance( UI_Tip_ComBuildStateView view, Vector2 targetPos)
        {
            if (view == null)
            {
                return ;
            }

            float distance = Vector2.Distance(this.pos, targetPos)/10;
            if (view.m_pl_distance!= null&& view.m_pl_distance.gameObject!=null)
            {              
                view.m_pl_distance.gameObject.SetActive(distance > 0);
            }

            if (distance > 0.1f)
            {
                if (distance <= 1)
                {
                    distance = 1;
                }

                if (view.m_lbl_distance_LanguageText!=null&&view.m_lbl_distance_LanguageText.gameObject != null)
                {           
                    view.m_img_bg_PolygonImage.transform.DOScale(Vector3.one, 0f);
                    view.m_lbl_distance_LanguageText.text= LanguageUtils.getTextFormat(300220, (int)distance);  
                }
            }
            else
            {
                view.m_img_bg_PolygonImage.transform.DOScale(Vector3.zero, 0f);
            }
        }

        private void OnHudUpdateCallBack(HUDUI hudui)
        {
        }

        private void CloseHUDUISpace()
        {
            if (huduiSpace != null)
            {
                ClientUtils.hudManager.CloseSingleHud(ref huduiSpace);
                huduiSpace = null;
            }
        }

        private void Close(bool isClearAll = true)
        {
            if (isClearAll)
            {
                if (dicHudui.Count > 0)
                {
                    foreach (var hudui in dicHudui.Values)
                    {
                        var hud = hudui;
                        CloseHUDUISpace();
                        ClientUtils.hudManager.CloseSingleHud(ref hud);
                    }

                    dicHudui.Clear();
                }
            }
            else
            {
                HUDUI hudui;
                if (dicHudui.TryGetValue(lastObjectId, out hudui))
                {
                    CloseHUDUISpace();
                    ClientUtils.hudManager.CloseSingleHud(ref hudui);
                    dicHudui.Remove(lastObjectId);
                }
            }

        }
    }
}
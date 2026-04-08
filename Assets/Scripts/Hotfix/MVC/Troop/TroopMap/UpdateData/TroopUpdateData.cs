using System;
using System.Collections.Generic;
using System.Text;
using Client;
using Data;
using Game;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Hotfix
{
    public sealed class TroopUpdateData : ITroopUpdateData
    {
        private TroopProxy m_TroopProxy;
        private PlayerProxy m_PlayerProxy;
        private WorldMapObjectProxy m_worldMapObject;
        private SquareHelper m_SquareHelper;
        private readonly Dictionary<long,long> dicTroopAOILines;
        private readonly Dictionary<long, List<Vector2>> path;
        private readonly List<long> lsTagertAOILines;
        private readonly Dictionary<long, List<long>> dicTroopAOIData;

        public TroopUpdateData()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_PlayerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_worldMapObject = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_SquareHelper = SquareHelper.Instance;
            dicTroopAOILines= new Dictionary<long, long>();
            lsTagertAOILines= new List<long>();
            path=  new Dictionary<long, List<Vector2>>();
            dicTroopAOIData= new Dictionary<long, List<long>>();
        }

        public void UpdateAttackTargetId(int troopId, int targetId)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(troopId);
            if (armyData != null)
            {
                if (TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.BATTLEING))
                {
                    WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(troopId,(int)armyData.armyStatus);
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.MapUpdateTroopHud, armyData.objectId);

                WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateLineColor(troopId);
                if (Application.isEditor)
                {
                    Color color;
                    ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                    CoreUtils.logService.Debug($"{troopId}\tBattleData: targetIndex:{targetId}", color);
                }
            }
        }

        public void UpdateMovePath(INotification data)
        {
            Map_ObjectInfo.request mapItemInfo = data.Body as Map_ObjectInfo.request;
            if (mapItemInfo != null)
            {
                int id = (int) mapItemInfo.mapObjectInfo.objectId;
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                if (armyData != null)
                {
                    if (armyData.go != null)
                    {                    
                        armyData.isCreate = false;
                    }

                    if (mapItemInfo.mapObjectInfo.HasObjectPath)
                    {
                        armyData.ClearMovePath();
                        foreach (var pos in mapItemInfo.mapObjectInfo.objectPath)
                        {
                            Vector2 v2 = PosHelper.ServerUnitToClientUnit_Vec2(new Vector2(pos.x, pos.y));
                            armyData.SetMovePath(v2);
                        }

                        armyData.autoMoveIndex = armyData.GetMoveIndex();
                        AutoMoveMgr.Instance.Insert(id);

                        if (Application.isEditor)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine();
                            foreach (var pos in mapItemInfo.mapObjectInfo.objectPath)
                            {
                                Vector2 v2 = PosHelper.ServerUnitToClientUnit_Vec2(new Vector2(pos.x, pos.y));
                                sb.Append("\t");
                                sb.Append(v2.ToString());
                            }
                            Color color;
                            ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                            CoreUtils.logService.Debug($"{id}\tBattleData: movePath:{sb.ToString()}", color);
                        }
                        // Debug.LogError("更新部队移动路径" +mapItemInfo.mapObjectInfo.objectId);
                    }
                }
            }
        }

        public void UpdateHp(int id, int hp)
        {
        }

 
        public void UpdateBuff(int id, object buff)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                List<BattleBuffDetail> buffId = buff as List<BattleBuffDetail>;
                if (Application.isEditor)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine();
                    foreach (var buf in buffId)
                    {
                        sb.Append("\t");
                        sb.Append($"{buf.buffId}");
                        sb.Append($"{buf.isNew}"); 
                    }
                    Color color;
                    ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                    CoreUtils.logService.Debug($"{id}\tBattleData: UpdateBuff:{sb.ToString()}", color);
                }
                armyData.buffID.Clear();
                buffId.ForEach((bid) =>
                {
                    SkillStatusDefine statusDefine = CoreUtils.dataService.QueryRecord<SkillStatusDefine>((int)bid.buffId);
                    if (statusDefine != null)
                    {
                        if (!string.IsNullOrEmpty(statusDefine.showEffect))
                        {
                            armyData.buffID.Add(bid.buffId);
                        }
                    }

                    if (bid.isNew)
                    {                      
                        WorldMapLogicMgr.Instance.BattleUIHandler.PushBattleBuff(id, (int) bid.buffId);  
                    }

                });

                if (armyData.buffID.Count > 0)
                {
                    WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(id, BattleUIType.BattleUI_BuffSkill, (int)armyData.buffID[0]);
                }
                else
                {
                    WorldMapLogicMgr.Instance.BattleBuffHandler.ClearBuff(id);
                }
            }
        }

        public void ClearMovePath(int id)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                armyData.ClearMovePath();
            }
        }

        public void UpdateCollectRuneTime(int id, long time)
        {
            var mapObjectInfo = m_worldMapObject.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfo == null) return;
            if(mapObjectInfo.IsCollectRune)
            {
                if(time == 0)
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.MapRemoveRuneGatherHud, id);
                }
            }
            else if(time > 0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.MapCreateRuneGatherHud, id);
            }
            mapObjectInfo.IsCollectRune = time > 0;
        }

        public void UpdateTroopSoldiers(int id, object parm)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            Dictionary<Int64, SoldierInfo> soldiers = parm as Dictionary<Int64, SoldierInfo>;
            if (armyData != null)
            {
                string des =
                    m_SquareHelper.GetMapCreateTroopDes(armyData.heroId, armyData.viceId, soldiers, Troops.ENMU_MATRIX_TYPE.RALLY);
                armyData.des = des;
                WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().SetFormationInfo(id, des);
            }
        }

        public void UpdateGuildAddName(int id)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
               AppFacade.GetInstance().SendNotification(CmdConstant.MapUpdateGuildAddName,id);
            }
        }

        public void UpdateAttackCount(int id, int num)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                WorldMapLogicMgr.Instance.BattleUIHandler.SetBattleUIData(id, BattleUIType.BattleUI_Attack, num);
            }
        }

        public void SetAOITroopLines(int id)
        {
            MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObject.GetWorldMapObjectByobjectId(id);
            if (mapObjectInfoEntity != null)
            {            
                if (mapObjectInfoEntity.armyMarchInfos != null)
                {
                    dicTroopAOILines.Clear();  
                    path.Clear();
                    lsTagertAOILines.Clear();
                    lsTagertAOILines.Add(id);
                    if (!dicTroopAOIData.ContainsKey(id))
                    {
                        dicTroopAOIData[id] = new List<long>();
                    }

                    foreach (var info in mapObjectInfoEntity.armyMarchInfos.Values)
                    {                   
                        bool isGuild = false;
                        bool isPlayHave = false;
                        bool isBeFight = false;
                        bool isHave = false;
                        if (info.HasGuildId)
                        {
                            if (mapObjectInfoEntity.guildId != info.guildId) //和当前对象联盟不一致
                            {
                                if (mapObjectInfoEntity.guildId == m_PlayerProxy.CurrentRoleInfo.guildId)
                                {
                                    isBeFight = true;
                                }
                                else
                                {
                                    isBeFight = false;
                                }
                                
                                if(m_PlayerProxy.CurrentRoleInfo.guildId == info.guildId&& m_PlayerProxy.CurrentRoleInfo.guildId!=0) //和我的联盟一致
                                {
                                    isGuild = true;
                                }
                                
                            }
                            else if(m_PlayerProxy.CurrentRoleInfo.guildId == info.guildId&& m_PlayerProxy.CurrentRoleInfo.guildId!=0) //和我的联盟一致
                            {
                                isGuild = true;
                            }
                            
//                            Debug.LogError("更新联盟id"+isGuild+"***"+mapObjectInfoEntity.cityName+"***"+m_PlayerProxy.CurrentRoleInfo.guildId+"***"+
//                                           info.guildId);
                        }

                        if (info.HasObjectIndex)
                        {
                            if (!dicTroopAOILines.ContainsKey(info.objectIndex))
                            {
                                dicTroopAOILines.Add(info.objectIndex,info.guildId);
                            }
                            MapObjectInfoEntity mapInfo= m_worldMapObject.GetWorldMapObjectByobjectId(info.objectIndex);
                            if (mapInfo != null)
                            {
                                isHave = true;
                            }
                            
                            dicTroopAOIData[id].Add(info.objectIndex);
                        }

                        if (info.HasRid)
                        {
                            isPlayHave = m_PlayerProxy.CurrentRoleInfo.rid== info.rid;
                        }

                        if (isPlayHave)
                        {
                            continue;
                        }

                        if (isHave)
                        {
                            continue;
                        }

                        Color color = RS.white;
                        if (isGuild)
                        {
                            color = RS.blue;
                        }else if (isPlayHave)
                        {
                            color = RS.green;
                        }else if (isBeFight)
                        {
                            color = RS.red;
                        }

                        if (info.HasPath && info.HasObjectIndex)
                        {
                            if (info.path.Count >= 2)
                            {
                                if (!path.ContainsKey(info.objectIndex))
                                {
                                    MapObjectInfoEntity infoEntity =
                                        m_worldMapObject.GetWorldMapObjectByobjectId(info.objectIndex);
                                    if (infoEntity == null) //画线的那个对象不在当前AOI范围才要画AOI线
                                    {
                                        if (!path.ContainsKey(info.objectIndex))
                                        {
                                            path[info.objectIndex] = new List<Vector2>();
                                        }

                                        Vector2 v1 = PosHelper.ServerUnitToClientUnit_Vec2(new Vector2(info.path[info.path.Count-1].x,
                                            info.path[info.path.Count-1].y));
                                        Vector2 v2 = PosHelper.ServerUnitToClientUnit_Vec2(new Vector2(info.path[info.path.Count-2].x,
                                            info.path[info.path.Count-2].y));
                                        
                                        //Debug.LogError("路径"+v1+"****"+v2 +"***"+ info.objectIndex);                                        
                                        Vector2 dir = (v2 - v1).normalized*4f;
                                        path[info.objectIndex].Add(v1+dir);
                                        path[info.objectIndex].Add(v1);                                    
                                        WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().SetAoiTroopLines((int) info.objectIndex, path[info.objectIndex], color);
                                    }
                                }
                                else
                                {
                                    path.Remove(info.objectIndex);
                                }
                            }
                        }

                        if (info.HasIsDelete)
                        {
                            WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveAoiTroopLines((int)info.objectIndex);
                        }
                        WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateAoiTroopLinesColor((int)info.objectIndex,color);
                    }
                }
            }
        }

        public void UpdateAOITroopLines(int id)
        {
            if (dicTroopAOILines.ContainsKey(id))
            {
                WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveAoiTroopLines(id);
                dicTroopAOILines.Remove(id);
            }
        }

        public void UpdateAOITroopLinesColor()
        {
            Color color = RS.white;                   
            foreach (var info in dicTroopAOILines)
            {            
                foreach (var id in lsTagertAOILines)
                {
                    MapObjectInfoEntity mapObjectInfoEntity = m_worldMapObject.GetWorldMapObjectByobjectId(id);
                    if (mapObjectInfoEntity != null)
                    {
                        if (mapObjectInfoEntity.guildId != info.Value) //和当前对象联盟不一致
                        {
                            if (mapObjectInfoEntity.guildId == m_PlayerProxy.CurrentRoleInfo.guildId)
                            {
                                color = RS.red;
                                break;
                            }
                        }
                    }
                }
                               
                if(m_PlayerProxy.CurrentRoleInfo.guildId == info.Value&&m_PlayerProxy.CurrentRoleInfo.guildId!=0) //和我的联盟一致
                {
                    color = RS.blue;                  
                }

                WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().UpdateAoiTroopLinesColor((int)info.Key,color);
            }
        }

        public void DeleteAOITroopLines(int id)
        {
            List<long> lsAoiId;
            if (dicTroopAOIData.TryGetValue(id,out lsAoiId))
            {
                foreach (var aoiId in lsAoiId)
                {
                    WorldMapLogicMgr.Instance.TroopLinesHandler.GetITroopLine().RemoveAoiTroopLines((int)aoiId);
                }

                dicTroopAOIData[id].Clear();
                dicTroopAOIData.Remove(id);
            }
        }

        public void UpdateHeroLevel(int id, int heroId)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData != null)
            {
                if (armyData.heroId == heroId)
                {
                    Troops formation = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetTroop(id);
                    if (formation != null)
                    {                 
                        Troops.SetHeroLevelUpEffectiveS(formation,"");   
                        
                    }
                }
            }
        }

        public void ClearData()
        {
            dicTroopAOILines.Clear();
            path.Clear();
            lsTagertAOILines.Clear();
            dicTroopAOIData.Clear();
        }
    }
}
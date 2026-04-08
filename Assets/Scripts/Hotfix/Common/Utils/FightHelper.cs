using Client;
using Data;
using Hotfix;
using Skyunion;
using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class FightHelper : Hotfix.TSingleton<FightHelper>
    {
        private WorldMapObjectProxy m_WorldMapObjectProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private TroopProxy m_TroopProxy;
        private ScoutProxy m_ScoutProxy;
        private AllianceProxy allianceProxy;

        public void InitProxy()
        {
            m_WorldMapObjectProxy= AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_RallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_TroopProxy= AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_ScoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;  
        }

        /// <summary>
        /// 打开创建部队界面
        /// </summary>
        /// <param name="objectId"></param>
        public void OpenCreateArmyPanel(int objectId)
        {
            if (AvailableHelp.GetAvailableHero().Count <= 0)
            {
                Tip.CreateTip(200007).Show();
               return;
            }
            
            if (!m_TroopProxy.GetIsCityCreateTroop())
            {
                Tip.CreateTip(200107).Show();
                return;
            }
            
            OpenPanelData openPanelData = new OpenPanelData(objectId, OpenPanelType.Common);
            CoreUtils.uiManager.ShowUI(UI.s_createAnmy, null, openPanelData);
        }
        
        /// <summary>
        /// 打开创建部队界面1
        /// </summary>
        /// <param name="openPanelData"></param>
        
        public void OpenCreateArmyPanel(OpenPanelData openPanelData)
        {
            if (AvailableHelp.GetAvailableHero(openPanelData.ExpeditionTroopIndex).Count <= 0)
            {
                Tip.CreateTip(200007).Show();
                return;
            }
            
            if (!m_TroopProxy.GetIsCityCreateTroop())
            {
                Tip.CreateTip(200107).Show();
                return;
            }
            CoreUtils.uiManager.ShowUI(UI.s_createAnmy, null, openPanelData);
        }

        /// <summary>
        /// 侦察
        /// </summary>
        /// <param name="服务器坐标单位"></param>
        /// <param name="目标ID"></param>
        private  void Scout(Vector2 serverUnit, int objectId)
        {
            Vector2 fixUnit = PosHelper.ServerUnitToClientUnit_Vec2(serverUnit);
            UI_Pop_ScoutSelectMediator.Param param = new UI_Pop_ScoutSelectMediator.Param();
            param.pos = fixUnit;
            param.targetIndex = objectId;
            CoreUtils.uiManager.ShowUI(UI.s_scoutSelectMenu, null, param);
        }

        /// <summary>
        /// 侦察
        /// </summary>
        /// <param name="服务器坐标单位 x"></param>
        /// <param name="服务器坐标单位 y"></param>
        /// <param name="目标ID"></param>
        public  void Scout(float serverUnitX, float serverUnitY, int objectId)
        {
            m_ScoutProxy.CheckScoutCondition(objectId,
                () => { Scout(new Vector2(serverUnitX, serverUnitY), objectId); });
        }

        /// <summary>
        /// 集结
        /// </summary>
        /// <param name="目标ID"></param>
        public  void Concentrate(int objectId)
        {
            int tipId = -1;
            if (!IsHolylandCanAttack(objectId, ref tipId))
            {
                if (tipId != -1)
                {
                    Tip.CreateTip(tipId).Show();
                }
                return;
            }
            
            
            MapObjectInfoEntity infoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(objectId);
            if (infoEntity != null)
            {
                if (!m_RallyTroopsProxy.isRally(infoEntity.cityRid, objectId, () =>
                {
                    if (m_TroopProxy.CheckTroopAttack(objectId,
                        () => { OnCreateRally(objectId); }
                    ))
                    {
                        OnCreateRally(objectId);
                    }
                }))
                {
                    return;
                }
            }

            if (!m_TroopProxy.CheckTroopAttack(objectId, () => { OnCreateRally(objectId); }))
            {
                return;
            }

            OnCreateRally(objectId);
        }

        private void OnCreateRally(int objectId)
        {
            OpenPanelData openPanelData = new OpenPanelData(objectId, OpenPanelType.CreateRally);   
            CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
        }

        /// <summary>
        /// 攻击野蛮人
        /// </summary>
        /// <param name="objectId"></param>
        public void AttackMonster(int objectId)
        {
            OpenPanelData openPanelData = new OpenPanelData(objectId, OpenPanelType.Common);
            CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
        }

        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="目标ID"></param>
        public  void Attack(int objectId)
        {
            if (m_TroopProxy.CheckAttackOtherCity(objectId))
            {                       
                return;
            }

            int tipId = -1;
            if (!IsHolylandCanAttack(objectId, ref tipId))
            {
                if (tipId != -1)
                {
                    Tip.CreateTip(tipId).Show();
                }
                return;
            }
            
            if (!m_RallyTroopsProxy.IsWasFever(objectId, OpenPanelType.Common, () =>
            {
                CheckTroopAttack(objectId);
            }))               
            {
                return; 
            }

           
            
            CheckTroopAttack(objectId);
        }


        private void CheckTroopAttack(int objectId)
        {
            bool isShow = true;
            if (m_TroopProxy.CheckTroopAttack(objectId, () =>
            {
                ShowCommonMainTroopsPanel(objectId);
                isShow = false;

            }))
                if (isShow)
                {                   
                    ShowCommonMainTroopsPanel(objectId);
                }
        }


        private void ShowCommonMainTroopsPanel(int objectId)
        {
                                
            OpenPanelData openPanelData = new OpenPanelData(objectId, OpenPanelType.Common);
            CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
        }


        /// <summary>
        /// 采集
        /// </summary>
        /// <param name="objectId"></param>
        public void Gather(int objectId)
        {
            OpenPanelData openPanelData= new OpenPanelData(objectId,OpenPanelType.Common);
            CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
        }

        /// <summary>
        /// 增援
        /// </summary>
        /// <param name="地图数据类"></param>
        public  void Reinforce(MapObjectInfoEntity mapObjectInfoEntity)
        {
            int tipId = -1;
            if (!IsHolylandCanAttack((int)mapObjectInfoEntity.objectId, ref tipId, OpenPanelType.Reinfore))
            {
                if (tipId != -1)
                {
                    Tip.CreateTip(tipId).Show();
                }
                return;
            }
            
            CoreUtils.uiManager.ShowUI(UI.s_AllianceBuild, null, mapObjectInfoEntity);
        }

        /// <summary>
        /// 加入集结
        /// </summary>
        /// <param name="data"></param>

        public void JoinRally(int objectId, long jonRid, float serverUnitX, float serverUnitY, long rallyTroopNum=0,long timespan = 0, long rallyTargetMonsterId = 0)
        {
            int tipId = -1;
            if (!IsHolylandCanAttack(objectId, ref tipId))
            {
                if (tipId != -1)
                {
                    Tip.CreateTip(tipId).Show();
                }
                return;
            }
            
            OpenPanelData openPanelData = new OpenPanelData(objectId, OpenPanelType.JoinRally);
            openPanelData.jonRid = jonRid;
            openPanelData.joinRallyTimes = timespan;
            Vector2 pos= PosHelper.ServerUnitToClientUnit_Vec2(new Vector2(serverUnitX,serverUnitY));
            openPanelData.pos= pos;
            openPanelData.rallyTroopNum = rallyTroopNum;
            openPanelData.targetMonsterId = rallyTargetMonsterId;
            CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
        }

        /// <summary>
        /// 增援打开创建部队
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="serverUnitX"></param>
        /// <param name="serverUnitY"></param>
        public void Reinfore(int objectId,int rallyRid=0, long rallyObjectIndex=0, float serverUnitX=0f, float serverUnitY=0f, bool isAllianceBulid=false,long rallyTroopNum=0,bool viewflag = true)
        {
            int tipId = -1;
            if (!IsHolylandCanAttack(objectId, ref tipId,OpenPanelType.Reinfore))
            {
                if (tipId != -1)
                {
                    Tip.CreateTip(tipId).Show();
                }
                return;
            }
            
            OpenPanelData openPanelData = new OpenPanelData(objectId, OpenPanelType.Reinfore);
            openPanelData.reinforceObjectIndex = rallyObjectIndex;
            if (serverUnitX != 0 && serverUnitY != 0)
            {
                Vector2 pos= PosHelper.ServerUnitToClientUnit_Vec2(new Vector2(serverUnitX,serverUnitY));
                openPanelData.pos = pos;
            }

            if (rallyRid != 0)
            {
                openPanelData.SetData(rallyRid,objectId);
            }

            openPanelData.isAllianceBulid = isAllianceBulid;
            openPanelData.rallyTroopNum = rallyTroopNum;
            openPanelData.viewFlag = viewflag;
            CoreUtils.uiManager.ShowUI(UI.s_createMainTroops, null, openPanelData);
        }
        
        
        /// <summary>
        /// 直接打开创建部队界面
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="rallyRid"></param>
        /// <param name="rallyObjectIndex"></param>
        /// <param name="serverUnitX"></param>
        /// <param name="serverUnitY"></param>
        /// <param name="isAllianceBulid"></param>
        /// <param name="rallyTroopNum"></param>

        public void ReinforeTroop(int objectId, int rallyRid = 0, long rallyObjectIndex = 0, float serverUnitX = 0f,
            float serverUnitY = 0f, bool isAllianceBulid = false, long rallyTroopNum = 0)
        {
            if (AvailableHelp.GetAvailableHero().Count <= 0)
            {
                Tip.CreateTip(200007).Show();
                return;
            }
            
            if (!m_TroopProxy.GetIsCityCreateTroop())
            {
                Tip.CreateTip(200107).Show();
                return;
            }
                     
            OpenPanelData openPanelData = new OpenPanelData(objectId, OpenPanelType.Reinfore);
            openPanelData.reinforceObjectIndex = rallyObjectIndex;
            if (serverUnitX != 0 && serverUnitY != 0)
            {
                Vector2 pos= PosHelper.ServerUnitToClientUnit_Vec2(new Vector2(serverUnitX,serverUnitY));
                openPanelData.pos = pos;
            }

            if (rallyRid != 0)
            {
                openPanelData.SetData(rallyRid,objectId);
            }

            openPanelData.isAllianceBulid = isAllianceBulid;
            openPanelData.rallyTroopNum = rallyTroopNum;
            CoreUtils.uiManager.ShowUI(UI.s_createAnmy, null, openPanelData);
        }


        /// <summary>
        /// 判断是否可以攻击圣地
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public bool IsHolylandCanAttack(int objectId, ref int tipValue,OpenPanelType type = OpenPanelType.None)
        {
            bool value = true;
            MapObjectInfoEntity objectInfo = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(objectId);

            if (objectInfo != null)
            {
                RssType rssType = (RssType)objectInfo.objectType;
                if (rssType == RssType.HolyLand || rssType == RssType.CheckPoint)
                {              
                    // 圣地保护期
                    if (type != OpenPanelType.Reinfore&& IsStrongHolyCanProtecting(objectInfo))
                    {
                        value = false;
                        tipValue = 730264;
                    }
                    else if (!allianceProxy.HasJionAlliance()) // 是否加入联盟
                    {
                        value = false;
                        tipValue = 500800;
                    }
                    else if(type != OpenPanelType.Reinfore&&!IsStrongHoldConnectGuildArea(objectInfo.objectPos.x, objectInfo.objectPos.y, (int)objectInfo.strongHoldId, allianceProxy.GetAllianceId()))
                    {
                        value = false;
                        tipValue = 732028;
                    }

                }else if (TroopHelp.IsAttackGuildType(rssType))
                {
                    if (!allianceProxy.HasJionAlliance()) // 是否加入联盟
                    {
                        value = false;
                        tipValue = 500800;
                    }
                    //TODO 先注释掉。交给服务器错误码提示
//                    else if (!m_TroopProxy.IsProvince(PosHelper.ServerUnitToClientUnit_Vec3(new Vector2(objectInfo.pos.x,objectInfo.pos.y))))
//                    {
//                        value = false;
//                        tipValue = 730263;
//                    }
                }
            }
            return value;
        }

        public bool IsStrongHoldConnectGuildArea(long posX, long posY, int strongHoldDataId, long guildId)
        {
            var strongHoldDataCfg = CoreUtils.dataService.QueryRecord<StrongHoldDataDefine>(strongHoldDataId);
            if (strongHoldDataCfg == null) return false;
            var strongHoldTypeCfg = CoreUtils.dataService.QueryRecord<StrongHoldTypeDefine>(strongHoldDataCfg.type);
            if (strongHoldTypeCfg == null) return false;
            Vector2Int worldPos = new Vector2Int(PosHelper.ServerUnitToClientUnit((int)posX), PosHelper.ServerUnitToClientUnit((int)posY));
            Vector2Int territoryPos = new Vector2Int((int)(worldPos.x / WorldMapObjectProxy.TerritoryPerUnit.x), 
                                                            (int)(worldPos.y / WorldMapObjectProxy.TerritoryPerUnit.y));
            if (worldPos.y % WorldMapObjectProxy.TerritoryPerUnit.y == 0)
            {
                territoryPos.y -= 1;
            }
            if (worldPos.x % WorldMapObjectProxy.TerritoryPerUnit.x == 0)
            {
                territoryPos.x -= 1;
            }
            Vector2Int strongHoldTerritoryHalfSize = new Vector2Int((int)(strongHoldTypeCfg.territorySize / WorldMapObjectProxy.TerritoryPerUnit.x / 2), 
                                                              (int)(strongHoldTypeCfg.territorySize / WorldMapObjectProxy.TerritoryPerUnit.y / 2));
            Vector2Int min = territoryPos - strongHoldTerritoryHalfSize;
            Vector2Int max = territoryPos + strongHoldTerritoryHalfSize;
            //上下          
            for (int x = min.x; x <= max.x; ++x)
            {
                if(m_WorldMapObjectProxy.IsTerritoryPosInGuildArea(max.y + 1, x, guildId))
                {
                    return true;
                }
                if (m_WorldMapObjectProxy.IsTerritoryPosInGuildArea(min.y - 1, x, guildId))
                {
                    return true;
                }
            }
            //左右         
            for (int y = min.y; y <= max.y; ++y)
            {
                if (m_WorldMapObjectProxy.IsTerritoryPosInGuildArea(y, min.x - 1, guildId))
                {
                    return true;
                }
                if (m_WorldMapObjectProxy.IsTerritoryPosInGuildArea(y, max.x + 1, guildId))
                {
                    return true;
                }
            }
            return false;
        }

        // 奇观建筑是否处于保护期
        public bool IsStrongHolyCanProtecting(MapObjectInfoEntity mapObjectInfoEntity)
        {
            UI_Item_IconAndTime_SubView.BuildingState holyLandStatus = (UI_Item_IconAndTime_SubView.BuildingState)mapObjectInfoEntity.holyLandStatus;
            if (holyLandStatus == UI_Item_IconAndTime_SubView.BuildingState.NotUnlock ||
                holyLandStatus == UI_Item_IconAndTime_SubView.BuildingState.InitProtecting ||
                holyLandStatus == UI_Item_IconAndTime_SubView.BuildingState.Protecting)
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 获取战斗难度提示
        /// </summary>
        /// <param name="soliderPow"></param>
        /// <param name="targetPow"></param>
        /// <returns></returns>
        public int GetFightingDifficultTip(int soliderPow, int targetPow)
        {
            int value = 200056;
            ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            if (targetPow * configDefine.battleRecommend1 <= soliderPow)
            {
                value = 200053;
            }
            else if (targetPow * configDefine.battleRecommend2 <= soliderPow)
            {
                value = 200054;
            }
            else if (targetPow * configDefine.battleRecommend3 <= soliderPow)
            {
                value = 200055;
            }
            else if (targetPow * configDefine.battleRecommend4 <= soliderPow)
            {
                value = 200056;
            }
            else
            {
                value = 200057;
            }

            return value;
        }

    }
}



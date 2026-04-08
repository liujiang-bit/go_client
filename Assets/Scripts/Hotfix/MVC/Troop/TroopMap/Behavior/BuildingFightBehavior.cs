using Game;
using Skyunion;
using System;
using System.Text;
using UnityEngine;

namespace Hotfix
{
    public class BuildingFightBehavior: Behavior
    {
        private MapObjectInfoEntity m_MapObjectInfoEntity;
        private int buildingId;
        public override void Init(int id)
        {
            base.Init(id);   
            // Debug.LogError("当前建筑状态"+(ArmyStatus)State+"***"+id);
            m_MapObjectInfoEntity = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            buildingId = id;
            if (Application.isEditor)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                var values = Enum.GetValues(typeof(ArmyStatus));
                foreach (var state in values)
                {
                    var enumState = (ArmyStatus)state;
                    if (enumState == ArmyStatus.None)
                        continue;

                    if (TroopHelp.IsHaveState(State, enumState))
                    {
                        sb.Append("\t");
                        sb.Append($"{enumState.ToString()}");
                    }
                }
                Color color;
                ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color);
                CoreUtils.logService.Debug($"{id}\tBattleBuildingData: ChangeState:{TroopHelp.GetTroopState(State)} {sb.ToString()}", color);
            }
        }

        public override void OnIDLE()
        {
            base.OnIDLE();
            if (m_MapObjectInfoEntity != null)
            {                      
                WorldMapLogicMgr.Instance.MapBuildingFightHandler.UpdateWorldHud(buildingId,true);
                WorldMapLogicMgr.Instance.MapBuildingFightHandler.StopBuildingHud(buildingId);
                WorldMapLogicMgr.Instance.MapBuildingFightHandler.StopSkill(buildingId);
            }
        }


        public override void OnFIGHT()
        {
            base.OnFIGHT();

            if (m_MapObjectInfoEntity != null)
            {         
                WorldMapLogicMgr.Instance.MapBuildingFightHandler.UpdateWorldHud(buildingId,false);
                WorldMapLogicMgr.Instance.MapBuildingFightHandler.PlayBuildingHud(buildingId);   
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Game;

namespace Hotfix
{
    public class AutoMoveMgr : TSingleton<AutoMoveMgr>
    {
        private readonly Dictionary<int, IAutoMove> dicAutoMove = new Dictionary<int, IAutoMove>();
        private TroopProxy m_TroopProxy;
        private WorldMapObjectProxy m_worldMapObjectProxy;

        protected override void Init()
        {
            base.Init();
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
        }

        public void Insert(int id)
        {
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            MapObjectInfoEntity mapObjectInfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);

            if (armyData != null)
            {
                DoInsert(id, armyData, MoveCallBack);
            }
            else if (mapObjectInfo != null)
            {
                DoInsert(id, mapObjectInfo, MapObjectMoveCallBack);
            }
        }
        
        public void Update()
        {
            foreach (var info in dicAutoMove)
            {
                info.Value.Update(info.Key);
            }          
        }

        private void MoveCallBack(int id,int index)
        {
            ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            DoCallback(id, index, armyData);
        }

        private void MapObjectMoveCallBack(int id, int index)
        {
            MapObjectInfoEntity mapObjectInfo = m_worldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            MapObjectDoCallback(id, index, mapObjectInfo);
        }
        
        private void DoInsert(int id , ArmyData armyData , Action<int,int> callback)
        {
            if (armyData.movePath.Count > 2)
            {
                IAutoMove autoMove = new AutoMove();
                AutoMoveData moveData = new AutoMoveData();
                moveData.id = id;
                moveData.path.Clear();
                foreach (var pos in armyData.movePath)
                {
                    moveData.path.Add(pos);
                }

                moveData.go = armyData.go;
                moveData.moveCallBack = callback;
                moveData.startIndex = armyData.GetMoveIndex();
                autoMove.Init(moveData);
                dicAutoMove[armyData.objectId]= autoMove;                 
            }
            else
            {
                Remove(armyData.objectId);
            }
        }

        private void DoInsert(int id, MapObjectInfoEntity mapObjectInfo, Action<int, int> callback)
        {
            if (mapObjectInfo.path.Count > 2)
            {
                IAutoMove autoMove = new AutoMove();
                AutoMoveData moveData = new AutoMoveData();
                moveData.id = id;
                moveData.path.Clear();
                foreach (var pos in mapObjectInfo.path)
                {
                    moveData.path.Add(pos);
                }

                moveData.go = mapObjectInfo.gameobject;
                moveData.moveCallBack = callback;
                moveData.startIndex = mapObjectInfo.GetMoveIndex();
                autoMove.Init(moveData);
                dicAutoMove[(int)mapObjectInfo.objectId] = autoMove;
            }
            else
            {
                Remove((int)mapObjectInfo.objectId);
            }
        }

        public void Remove(int objectId)
        {
            if (dicAutoMove.ContainsKey(objectId))
            {
                dicAutoMove[objectId].Remove(objectId);
                dicAutoMove.Remove(objectId);
            }
        }

        private void DoCallback(int id, int index , ArmyData armyData)
        {
            if (armyData != null)
            {
                armyData.autoMoveIndex = index;
                if (index < armyData.movePath.Count-1)
                {          
                    WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(id, (int)ArmyStatus.SPACE_MARCH); 
                }
            }
        }

        private void MapObjectDoCallback(int id, int index, MapObjectInfoEntity mapObjectInfo)
        {
            if (mapObjectInfo != null)
            {
                mapObjectInfo.autoMoveIndex = index;
                if (index < mapObjectInfo.path.Count - 1)
                {
                    WorldMapLogicMgr.Instance.BehaviorHandler.ChageState(id, (int)ArmyStatus.SPACE_MARCH);
                }
            }
        }
    }
}
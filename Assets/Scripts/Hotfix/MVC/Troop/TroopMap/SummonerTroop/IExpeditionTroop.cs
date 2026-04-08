using System.Collections.Generic;
using UnityEngine;
using Client;
using Hotfix;

namespace Game
{
    public interface IExpeditionTroop : IGameModeTroop
    {
        void CreatePreviewMonsterFormation(ExpeditionMosnterTroopData monsterTroopData);
        Troops GetPreviewMonsterFormation(int monsterIndex);
        void CreatePreviewPlayerFormation(ExpeditionPlayerTroopData playerTroopData);
        void DestroyPreviewPlayerFormation(int troopIndex);
        ArmyData CreateArmyData(SprotoType.MapObjectInfo mapObjectInfo);
        void CreateEnemyArmyData(ExpeditionMosnterTroopData monseterTroopData);
        void CreatePlayerArmyData(ExpeditionPlayerTroopData playerTroopData);
        void RemoveArmyData(int objectId);
        ArmyData GetArmyData(int objectId);
        ArmyData GetArmyData(bool isPlayer, int troopIndex);
        List<ArmyData> GetEnemyDatas();
        List<ArmyData> GetPlayerTroopDatas();
        void PlayArmyDeadPerofrmance(int objectId, Troops formation);
        void CalScreenViceArmList(int objectId, ref List<int> viceArmyObjectIdList);
        void CalWorldViceArmList(int objectId, ref List<int> viceArmyObjectIdList);
    }
}
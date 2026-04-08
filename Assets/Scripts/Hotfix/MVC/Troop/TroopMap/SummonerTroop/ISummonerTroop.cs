using PureMVC.Interfaces;
using Game;
using System.Collections.Generic;

namespace Hotfix
{
    public interface ISummonerTroop  : IGameModeTroop
    {
        void UpdateSummonerTroop(INotification data);
        void UpdateSummonerScout(INotification data);
        void UpdateSummonerTransport(INotification data);
        ArmyData AddSummonerArmyByMapObjectInfo(SprotoType.MapObjectInfo data);
        ArmyData GetArmyData(RssType objectType, int armyId);
        ArmyData GetArmyDataByObjectId(RssType objectType, int objectId);
        ArmyData GetArmyDataByObjectId(int objectId);
        List<ArmyData> GetSummonerArmyDatas();
        int GetStationingArmyId(int objectId);
        void CalScreenViceArmList(int troopId, ref List<int> viceArmyIndexList);
        void CalWorldViceArmList(int troopId, ref List<int> viceArmyIndexList);
    }
}
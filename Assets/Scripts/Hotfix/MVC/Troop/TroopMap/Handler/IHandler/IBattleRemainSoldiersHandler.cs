using SprotoType;
using System.Collections.Generic;

namespace Hotfix
{
    public interface IBattleRemainSoldiersHandler:IBaseHandler
    {
        void AddBattleRemainSoldiersInfo(int objectId, Dictionary<long, BattleRemainSoldiers> infos);
        Dictionary<long, BattleRemainSoldiers> GetBattleRemainSoldiers(long objectId);
    }
}
using System.Collections.Generic;
using SprotoType;

namespace Hotfix
{
    public class BattleRemainSoldiersHandler:IBattleRemainSoldiersHandler
    {
        private readonly Dictionary<long, Dictionary<long, BattleRemainSoldiers>> dicBattleRemainSoldiers = new Dictionary<long, Dictionary<long, BattleRemainSoldiers>>();
        public void Init()
        {
           
        }

        public void Clear()
        {
            dicBattleRemainSoldiers.Clear();
        }

        public void AddBattleRemainSoldiersInfo(int objectId, Dictionary<long, BattleRemainSoldiers> infos)
        {
            if (!dicBattleRemainSoldiers.ContainsKey(objectId))
            {
                dicBattleRemainSoldiers[objectId] = new Dictionary<long, BattleRemainSoldiers>();
            }
            foreach (var info in infos)
            {
                dicBattleRemainSoldiers[objectId][info.Key] = info.Value;
            }
        }

        public Dictionary<long, BattleRemainSoldiers> GetBattleRemainSoldiers(long objectId)
        {
            Dictionary<long, BattleRemainSoldiers> battleRemainSoldiers = null;
            if (dicBattleRemainSoldiers.TryGetValue(objectId, out battleRemainSoldiers))
            {
                return battleRemainSoldiers;
            }

            return null;
        }
    }
}
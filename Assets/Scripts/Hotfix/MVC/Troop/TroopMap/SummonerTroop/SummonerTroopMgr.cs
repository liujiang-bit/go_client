using Game;
using System.Collections.Generic;

namespace Hotfix
{
    public class SummonerTroopMgr: TSingleton<SummonerTroopMgr>
    {
        private ISummonerTroop m_SummonerTroop;
        public IExpeditionTroop ExpeditionTroop { get; private set; }

        private List<IGameModeTroop> m_gameModeTroopList = new List<IGameModeTroop>();

        protected override void Init()
        {
            base.Init();
            m_SummonerTroop = AddGameModeTroop<SummonerTroop>();
            ExpeditionTroop = AddGameModeTroop<ExpeditionTroop>();
        }

        private T AddGameModeTroop<T>() where T : IGameModeTroop, new()
        {
            T t = new T();
            m_gameModeTroopList.Add(t);
            return t;
        }

        public void Initialize()
        {
            foreach(var troop in m_gameModeTroopList)
            {
                troop.Init();
            }
        }

        public void Clear()
        {
            foreach (var troop in m_gameModeTroopList)
            {
                troop.Clear();
            }
        }

        public ISummonerTroop GetISummonerTroop()
        {
            return m_SummonerTroop;
        }


    }
}
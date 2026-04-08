using System.Collections.Generic;
using Game;

namespace Hotfix
{
    public class AvailableHelp
    {
        public static List<HeroProxy.Hero> GetAvailableHero(int expeditionTroopIndex = 0)
        {
            List<HeroProxy.Hero> heros = null;
            switch (GameModeManager.Instance.CurGameMode)
            {
                case GameModeType.World:
                    var mTroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
                    heros = mTroopProxy.GetAvailableHeros();
                    break;
                case GameModeType.Expedition:
                    var expeditionProxy =
                        AppFacade.GetInstance().RetrieveProxy(ExpeditionProxy.ProxyNAME) as ExpeditionProxy;
                    if (expeditionProxy != null)
                    {
                        heros = expeditionProxy.GetAvailableHeros(expeditionTroopIndex);
                    }

                    break;
            }

            return heros;
        }
    }
}
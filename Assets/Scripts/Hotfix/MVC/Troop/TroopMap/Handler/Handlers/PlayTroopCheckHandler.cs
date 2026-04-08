using System.Collections.Generic;
using Game;

namespace Hotfix
{
    public class PlayTroopCheckHandler: IPlayTroopCheckHandler
    {
        private TroopProxy m_TroopProxy;
        private ScoutProxy m_ScoutProxy;
        public void Init()
        {
            m_TroopProxy=AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy; 
            m_ScoutProxy= AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy; 
        }

        public void Clear()
        {
         
        }

        public bool isHaveFight()
        {
            var troops = m_TroopProxy.GetArmys();
            var status = ArmyStatus.BATTLEING |
                         ArmyStatus.FOLLOWUP |
                         ArmyStatus.MOVE;
            foreach (var info in troops)
            {
                if (TroopHelp.IsHaveState(info.status, status))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isHaveRun()
        {
            var troops = m_TroopProxy.GetArmys();
            var status = ArmyStatus.SPACE_MARCH |
                         ArmyStatus.ATTACK_MARCH |
                         ArmyStatus.COLLECT_MARCH |
                         ArmyStatus.REINFORCE_MARCH |
                         ArmyStatus.RETREAT_MARCH |
                         ArmyStatus.FAILED_MARCH |
                         ArmyStatus.DISCOVER |
                         ArmyStatus.SCOUNT |
                         ArmyStatus.RETURN;
            foreach (var info in troops)
            {
                if (TroopHelp.IsHaveAnyState(info.status, (long)status))
                {
                    return true;
                }
            }
            
            
            return false;
        }

        public bool isHaveCollect()
        {
            var troops = m_TroopProxy.GetArmys();
            foreach (var info in troops)
            {          
                if (TroopHelp.IsHaveState(info.status, ArmyStatus.COLLECTING))
                {
                    return true;
                }
            }

            return false;
        }

        public bool isHaveStationing()
        {
            var troops = m_TroopProxy.GetArmys();
            foreach (var info in troops)
            {
                if (TroopHelp.IsHaveState(info.status, ArmyStatus.STATIONING))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isHavePlayMarch()
        {
            var troops = m_TroopProxy.GetArmys();
            foreach (var info in troops)
            {
                if (TroopHelp.IsHaveState(info.status, ArmyStatus.PALLY_MARCH))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isHaveScoutMap()
        {
            var socount = m_ScoutProxy.GetAllActiveScounts();
            return socount.Count>0;
        }

        public bool isHaveFightCity()
        {
            return false;
        }

        public bool isHaveMap()
        {
            var troops = m_TroopProxy.GetArmys();
            return troops.Count>0;
        }

        public bool isHaveRally()
        {
            var troops = m_TroopProxy.GetArmys();
            var status = ArmyStatus.RALLY_JOIN_MARCH | ArmyStatus.RALLY_WAIT | ArmyStatus.RALLY_BATTLE;
            foreach (var info in troops)
            {
                if (TroopHelp.IsHaveAnyState(info.status, (long)status))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
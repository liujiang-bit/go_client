using Client;
using Game;
using UnityEngine;

namespace Hotfix
{
    public class GuardianHandler : IGuardianHandler
    {
        private WorldMapObjectProxy m_WorldMapObjectProxy;
        public void Init()
        {
            if (m_WorldMapObjectProxy == null)
            {
                m_WorldMapObjectProxy= AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            }
        }

        public void Clear()
        {
           
        }

        public void SetStates(Guardian self, Troops.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos,
            float move_speed = 2)
        {
            Guardian.SetStateS(self,state,current_pos,target_pos,move_speed);
        }

        public void FadeOut_S(Guardian self)
        {
            Guardian.FadeOut_S(self);
        }

        public void TriggerSkillS(Guardian self, string param)
        {
            Guardian.TriggerSkillS(self,param);
        }

        public Guardian GetFormationGuardian(int id)
        {
            MapObjectInfoEntity mapObjectExtEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
            if (mapObjectExtEntity == null)
            {
                return null;
            }
            
            if (mapObjectExtEntity.rssType == RssType.Guardian && mapObjectExtEntity.gameobject != null)
            {
                return mapObjectExtEntity.gameobject.GetComponent<Guardian>();
            }
            return null;
        }
    }
}
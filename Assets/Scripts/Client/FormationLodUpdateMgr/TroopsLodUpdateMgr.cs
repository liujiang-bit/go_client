using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// 布局lod
    /// </summary>
    public class TroopsLodUpdateMgr : MonoBehaviour
    {
        public static TroopsLodUpdateMgr m_instance;

        private Queue<Troops> m_to_be_update_formation = new Queue<Troops>();

        private Queue<Guardian> m_to_be_update_formationGuardian = new Queue<Guardian>();

        private void Awake()
        {
            m_instance = this;
        }

        public void AddUpdateFormationRequest(Troops formation)
        {
            if (!m_to_be_update_formation.Contains(formation))
            {
                m_to_be_update_formation.Enqueue(formation);
            }
        }

        public void AddUpdateFormationRequest(Guardian formation)
        {
            if (!m_to_be_update_formationGuardian.Contains(formation))
            {
                m_to_be_update_formationGuardian.Enqueue(formation);
            }
        }

        private void Update()
        {
            try
            {
                if (m_to_be_update_formation.Count != 0)
                {
                    Troops formation = m_to_be_update_formation.Dequeue();
                    if (formation != null)
                    {
                        formation.UpdateLodNow();
                    }
                }
                if (m_to_be_update_formationGuardian.Count != 0)
                {
                    Guardian formationGuardian = m_to_be_update_formationGuardian.Dequeue();
                    if (formationGuardian != null)
                    {
                        formationGuardian.UpdateLodNow();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class FormationLodUpdateMgr : MonoBehaviour
    {
        public static FormationLodUpdateMgr m_instance;

        private Queue<Formation> m_to_be_update_formation = new Queue<Formation>();

        private Queue<FormationGuardian> m_to_be_update_formationGuardian = new Queue<FormationGuardian>();

        private void Awake()
        {
            FormationLodUpdateMgr.m_instance = this;
        }

        public void AddUpdateFormationRequest(Formation formation)
        {
            if (!this.m_to_be_update_formation.Contains(formation))
            {
                this.m_to_be_update_formation.Enqueue(formation);
            }
        }

        public void AddUpdateFormationRequest(FormationGuardian formation)
        {
            if (!this.m_to_be_update_formationGuardian.Contains(formation))
            {
                this.m_to_be_update_formationGuardian.Enqueue(formation);
            }
        }

        private void Update()
        {
            try
            {
                if (this.m_to_be_update_formation.Count != 0)
                {
                    Formation formation = this.m_to_be_update_formation.Dequeue();
                    if (formation != null)
                    {
                        formation.UpdateLodNow();
                    }
                }
                if (this.m_to_be_update_formationGuardian.Count != 0)
                {
                    FormationGuardian formationGuardian = this.m_to_be_update_formationGuardian.Dequeue();
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
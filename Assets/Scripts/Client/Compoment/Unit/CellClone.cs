using System;
using UnityEngine;

namespace Client
{
    public class CellClone : MonoBehaviour
    {
        public CellBase m_unit;

        private Vector3 m_org_pos = Vector3.zero;

        private bool m_inited_org_pos;

        public Vector3 OrgPos
        {
            get
            {
                return this.m_org_pos;
            }
        }

        public bool InitedOrgPos
        {
            get
            {
                return this.m_inited_org_pos;
            }
        }

        public void UpdateInitPos()
        {
            if (!this.m_inited_org_pos)
            {
                this.m_inited_org_pos = true;
                this.m_org_pos = base.transform.localPosition;
            }
        }

        public void ResumeInitPos()
        {
            if (this.m_inited_org_pos)
            {
                base.transform.localPosition = this.m_org_pos;
            }
        }

        private void OnDespawn()
        {
            base.transform.position = Vector3.zero;
            base.transform.eulerAngles = Vector3.zero;
            this.m_org_pos = Vector3.zero;
            this.m_inited_org_pos = false;
        }
    }
}
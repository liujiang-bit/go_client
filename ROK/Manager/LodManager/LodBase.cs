using System;
using UnityEngine;

namespace ROK
{
    public class LodBase : MonoBehaviour
    {
        public float[] m_lod_array;

        private bool m_has_registered;

        private EventDispather.EventHandler m_update_lod;

        protected LodBase()
        {
            this.m_update_lod = new EventDispather.EventHandler(this.UpdateLod);
        }

        protected bool IsLodChanged()
        {
            float previousLodDistance = Common.GetPreviousLodDistance();
            float lodDistance = Common.GetLodDistance();
            for (int i = 0; i < this.m_lod_array.Length; i++)
            {
                if ((this.m_lod_array[i] < lodDistance && previousLodDistance <= this.m_lod_array[i]) || (lodDistance <= this.m_lod_array[i] && previousLodDistance > this.m_lod_array[i]))
                {
                    return true;
                }
            }
            return false;
        }

        protected int GetCurrentLodLevel()
        {
            if (this.m_lod_array == null)
            {
                return 0;
            }
            float lodDistance = Common.GetLodDistance();
            for (int i = 0; i < this.m_lod_array.Length; i++)
            {
                if (lodDistance <= this.m_lod_array[i])
                {
                    return i;
                }
            }
            return this.m_lod_array.Length - 1;
        }

        protected int GetPreviousLodLevel()
        {
            float previousLodDistance = Common.GetPreviousLodDistance();
            for (int i = 0; i < this.m_lod_array.Length; i++)
            {
                if (previousLodDistance <= this.m_lod_array[i])
                {
                    return i;
                }
            }
            return this.m_lod_array.Length - 1;
        }

        protected void Start()
        {
            if (!this.m_has_registered)
            {
                LodManager.GetInstance().m_event_dispatcher.AddHandler(this.m_update_lod);
                this.m_has_registered = true;
            }
        }

        protected void OnDestroy()
        {
            LodManager.GetInstance().m_event_dispatcher.RemoveHandler(this.m_update_lod);
            this.m_has_registered = false;
        }

        protected void OnSpawn()
        {
            if (!this.m_has_registered)
            {
                LodManager.GetInstance().m_event_dispatcher.AddHandler(this.m_update_lod);
                this.m_has_registered = true;
            }
        }

        protected void OnDespawn()
        {
            LodManager.GetInstance().m_event_dispatcher.RemoveHandler(this.m_update_lod);
            this.m_has_registered = false;
        }

        public virtual void UpdateLod()
        {
        }

        private void OnEnable()
        {
            this.UpdateLod();
        }
    }
}
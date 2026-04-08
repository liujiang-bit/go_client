using Skyunion;
using System;
using UnityEngine;

namespace Client
{
    public class LevelDetailBase : MonoBehaviour
    {
        public float[] m_lod_array;

        private bool m_has_registered;

        protected LevelDetailBase()
        {
        }

        protected bool IsLodChanged()
        {
            float previousLodDistance = ClientUtils.lodManager.GetPreviousLodDistance();
            float lodDistance = ClientUtils.lodManager.GetLodDistance();
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
            float lodDistance = ClientUtils.lodManager.GetLodDistance();
           // Debug.LogError("距离"+lodDistance);
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
            float previousLodDistance = ClientUtils.lodManager.GetPreviousLodDistance();
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
                ClientUtils.lodManager.AddHandler(this.UpdateLod);
                this.m_has_registered = true;
            }
        }

        protected void OnDestroy()
        {
            if (Application.isPlaying)
            {
                ClientUtils.lodManager.RemoveHandler(this.UpdateLod);
            }
            this.m_has_registered = false;
        }

        protected void OnSpawn()
        {
            if (!this.m_has_registered)
            {
                ClientUtils.lodManager.AddHandler(this.UpdateLod);
                this.m_has_registered = true;
            }
        }

        protected void OnDespawn()
        {
            // 编辑器关闭之后就不需要处理了
            if (CoreUtils.assetService == null)
                return;

            ClientUtils.lodManager.RemoveHandler(this.UpdateLod);
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
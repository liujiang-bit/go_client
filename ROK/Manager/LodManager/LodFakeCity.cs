using System;
using UnityEngine;

namespace ROK
{
    public class LodFakeCity : LodCastle
    {
        public GameObject[] m_hide_object_array;

        public override void UpdateLod()
        {
            if (base.IsLodChanged())
            {
                this.UpdateObjectVisible();
            }
            base.UpdateLod();
        }

        private new void OnSpawn()
        {
            this.UpdateObjectVisible();
            base.OnSpawn();
        }

        private void UpdateObjectVisible()
        {
            int currentLodLevel = base.GetCurrentLodLevel();
            if (currentLodLevel == 0)
            {
                this.SetGameObjectVisible(true);
            }
            else if (currentLodLevel == 1)
            {
                this.SetGameObjectVisible(false);
            }
        }

        private void SetGameObjectVisible(bool visible)
        {
            for (int i = 0; i < this.m_hide_object_array.Length; i++)
            {
                this.m_hide_object_array[i].SetActive(visible);
            }
        }
    }
}
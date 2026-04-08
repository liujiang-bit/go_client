using System;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class Rss : LevelDetailBase
    {
        private int lodLevel;
        private bool isTrigger = true;
        private int lastnum = 0;
        private GameObject lodGo;

        private void Awake()
        {
            this.m_lod_array = new[] {200f, 250f, 2000f};
        }

        public override void UpdateLod()
        {
            base.UpdateLod();
            lodLevel = GetCurrentLodLevel();
            if (lodLevel == 0)
            {
                SetLodType(true);
            }
            else if (lodLevel == 1)
            {
                SetLodType(false);
            }
        }

        public void SetLodGo(GameObject lodGO)
        {
            this.lodGo = lodGO;
            this.lodGo.gameObject.SetActive(false);
        }

        public void SetLodType(bool isLodShow)
        {
            if (this.lodGo == null)
            {
                return;
            }

            if (isLodShow)
            {
                this.gameObject.SetActive(true);
                this.lodGo.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(false); 
                this.lodGo.gameObject.SetActive(true);
            }
        }
    }
}
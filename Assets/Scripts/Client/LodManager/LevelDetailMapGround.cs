using Skyunion;
using System;
using UnityEngine;

namespace Client
{
    public class LevelDetailMapGround : LevelDetailBase
    {
        public override void UpdateLod()
        {
            if (base.IsLodChanged())
            {
                this.UpdateMat();
            }
            base.UpdateLod();
        }

        public new void OnSpawn()
        {
            this.UpdateMat();
            base.OnSpawn();
        }

        private void UpdateMat()
        {
            int currentLodLevel = base.GetCurrentLodLevel();
            if (currentLodLevel == 0)
            {
                string asset_name = base.transform.name.Replace("(Clone)", string.Empty) + "_mat";
                CoreUtils.assetService.LoadAssetAsync<Material>(asset_name, (IAsset asset) =>
                {
                    if (this == null)
                        return;
                    base.GetComponent<MeshRenderer>().material = asset.asset() as Material;
                }, gameObject);
            }
            else if (currentLodLevel == 1)
            {
                string asset_name2 = base.transform.name.Replace("(Clone)", string.Empty) + "_lod1";
                CoreUtils.assetService.LoadAssetAsync<Material>(asset_name2, (IAsset asset) =>
                {
                    if (this == null)
                        return;
                    base.GetComponent<MeshRenderer>().material = asset.asset() as Material;
                }, gameObject);
            }
        }
    }
}
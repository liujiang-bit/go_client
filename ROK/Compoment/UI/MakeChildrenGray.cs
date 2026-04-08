using Skyunion;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ROK
{
    public class MakeChildrenGray : MonoBehaviour
    {
        private Material gray_material;

        private Image[] allChildren;

        private bool IsNormal = false;

        private void Awake()
        {
            LoadGrayMaterial(null);
            this.allChildren = base.GetComponentsInChildren<Image>(true);
        }

        private void LoadGrayMaterial(Action action)
        {
            CoreUtils.assetService.LoadAssetAsync<Material>("UI_Gray", (IAsset asset) =>
            {
                this.gray_material = asset.asset() as Material;
                action?.Invoke();
            });
        }

        public void Gray()
        {
            IsNormal = false;
            if (this.allChildren == null)
            {
                return;
            }
            if (gray_material == null)
            {
                LoadGrayMaterial(() =>
                {
                    if (IsNormal == false)
                    {
                        Gray();
                    }
                });
                return;
            }
            Image[] array = this.allChildren;
            for (int i = 0; i < array.Length; i++)
            {
                Image image = array[i];
                image.material = this.gray_material;
            }
        }

        public void Normal()
        {
            IsNormal = true;
            if (this.allChildren == null)
            {
                return;
            }
            Image[] array = this.allChildren;
            for (int i = 0; i < array.Length; i++)
            {
                Image image = array[i];
                image.material = image.defaultMaterial;
            }
        }
    }
}
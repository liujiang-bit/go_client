using System;
using UnityEngine;
using UnityEngine.UI;

namespace ROK
{
    public class SpriteGraphic : MaskableGraphic
    {
        public SpriteAsset m_spriteAsset;

        public override Texture mainTexture
        {
            get
            {
                if (this.m_spriteAsset == null || this.m_spriteAsset.texSource == null)
                {
                    return Graphic.s_WhiteTexture;
                }
                return this.m_spriteAsset.texSource;
            }
        }

        protected override void OnEnable()
        {
        }

        protected override void OnRectTransformDimensionsChange()
        {
        }

        public new void UpdateMaterial()
        {
            base.UpdateMaterial();
        }
    }
}
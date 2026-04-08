using System;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

namespace ROK
{
    public class SpriteDualHelper : MonoBehaviour
    {
        public Sprite m_mask_sprite;

        private SpriteRenderer sprite_renderer;

        private CanvasGroup m_canvas_group;

        private float m_org_sprite_alpha = 1f;

        private bool m_has_inited;

        public static Dictionary<string, Material> s_mat_catch = new Dictionary<string, Material>();

        private SpriteRenderer m_sprite_renderer
        {
            get
            {
                if (this.sprite_renderer == null)
                {
                    this.sprite_renderer = base.GetComponent<SpriteRenderer>();
                }
                return this.sprite_renderer;
            }
        }

        private void Awake()
        {
        }

        private void Start()
        {
            this.m_org_sprite_alpha = this.m_sprite_renderer.color.a;
            this.UpdateDualTex();
            this.m_has_inited = true;
        }

        public static void UpdateDualTex_S(SpriteDualHelper helper)
        {
            helper.UpdateDualTex();
        }

        private void UpdateDualTex()
        {
            if (this.m_mask_sprite != null && this.m_sprite_renderer.sharedMaterial.GetTexture("_Mask") != this.m_mask_sprite.texture)
            {
                string name = this.m_sprite_renderer.material.name;
                string key = this.m_mask_sprite.texture.name + "_" + name.Replace("(Instance)", string.Empty).Trim();
                Material material;
                if (SpriteDualHelper.s_mat_catch.ContainsKey(key))
                {
                    material = SpriteDualHelper.s_mat_catch[key];
                }
                else
                {
                    material = this.m_sprite_renderer.material;
                    material.SetTexture("_Mask", this.m_mask_sprite.texture);
                    SpriteDualHelper.s_mat_catch.Add(key, material);
                }
                this.m_sprite_renderer.material = material;
            }
        }

        public static void SetColor_S(SpriteDualHelper helper, string property_name, Color color)
        {
            helper.SetColor(property_name, color);
        }

        private void SetColor(string property_name, Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            this.m_sprite_renderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor(property_name, color);
            this.m_sprite_renderer.SetPropertyBlock(materialPropertyBlock);
        }

        public static void SetFloat_S(SpriteDualHelper helper, string property_name, float f)
        {
            helper.SetFloat(property_name, f);
        }

        private void SetFloat(string property_name, float f)
        {
            this.m_sprite_renderer.material.SetFloat(property_name, f);
        }

        public static void GetUpdatedMaterial_S(SpriteDualHelper sprite_dual_helper, string mat_path, Action<Material> action)
        {
            sprite_dual_helper.GetUpdatedMaterial(mat_path, action);
        }

        private void GetUpdatedMaterial(string mat_path, Action<Material> action)
        {
            string key = this.m_mask_sprite.texture.name + "_" + mat_path;
            Material material;
            if (SpriteDualHelper.s_mat_catch.ContainsKey(key))
            {
                material = SpriteDualHelper.s_mat_catch[key];
                if (this.m_sprite_renderer != null)
                {
                    this.m_sprite_renderer.material = material;
                }
                this.UpdateDualTex();
                action?.Invoke(material);
            }
            else
            {
                CoreUtils.assetService.LoadAssetAsync<Material>(mat_path, (IAsset asset) =>
                {
                    material = asset.asset() as Material;
                    if (this.m_sprite_renderer != null)
                    {
                        this.m_sprite_renderer.material = material;
                    }
                    this.UpdateDualTex();
                    action?.Invoke(material);
                });
            }
        }

        private void OnCanvasGroupChanged()
        {
            if (this.m_has_inited)
            {
                if (this.m_canvas_group == null)
                {
                    this.m_canvas_group = base.GetComponentInParent<CanvasGroup>();
                }
                Color color = this.m_sprite_renderer.color;
                color.a = this.m_canvas_group.alpha * this.m_org_sprite_alpha;
                this.m_sprite_renderer.color = color;
            }
        }

        public static void SetCanChangeLight_S(SpriteDualHelper sprite_dual_helper, bool canChangeLight)
        {
            NightMask component = sprite_dual_helper.GetComponent<NightMask>();
            if (component != null)
            {
                component.SetCanChangeLight(canChangeLight);
            }
        }
    }
}
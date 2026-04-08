using System;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

namespace Client
{
    [ExecuteInEditMode]
    public class MaskSprite : MonoBehaviour
    {
        public Sprite m_mask_sprite;
        private Sprite m_nowSprite;

        private SpriteRenderer sprite_renderer;

        private CanvasGroup m_canvas_group;

        private float m_org_sprite_alpha = 1f;

        private bool m_has_inited;

        public static Dictionary<string, Material> s_mat_catch = new Dictionary<string, Material>();

        private SpriteRenderer m_sprite_renderer
        {
            get
            {
                if (sprite_renderer == null)
                {
                    sprite_renderer = GetComponent<SpriteRenderer>();
                }
                return sprite_renderer;
            }
        }

        private void Awake()
        {
        }

        private void Start()
        {
            Color color = m_sprite_renderer.color;
            m_org_sprite_alpha = color.a;
            UpdateMaskTex();
            m_has_inited = true;
        }

        private void UpdateMaskTex()
        {

            if (m_mask_sprite!=null)
            {
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                m_sprite_renderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetTexture("_Mask", m_mask_sprite.texture);
                m_sprite_renderer.SetPropertyBlock(propertyBlock);
                m_nowSprite = m_mask_sprite;
            }
            else
            {
                Debug.Log(gameObject.name+" MaskSprite 丢失");
            }


            //#if UNITY_EDITOR
            //            if (m_mask_sprite != null)
            //            {
            //                m_sprite_renderer.material.SetTexture("_Mask", m_mask_sprite.texture);
            //            }
            //#else
            //if (m_mask_sprite != null && m_sprite_renderer.sharedMaterial.GetTexture("_Mask") != m_mask_sprite.texture)
            //{
            //    string name = m_sprite_renderer.material.name;
            //    string key = m_mask_sprite.texture.name + "_" + name.Replace("(Instance)", string.Empty).Trim();
            //    Material material;
            //    if (s_mat_catch.ContainsKey(key))
            //    {
            //        material = s_mat_catch[key];
            //    }
            //    else
            //    {
            //        material = m_sprite_renderer.material;
            //        material.SetTexture("_Mask", m_mask_sprite.texture);
            //        s_mat_catch.Add(key, material);
            //    }
            //    m_sprite_renderer.material = material;
            //}
            //#endif
        }
#if UNITY_EDITOR
        private void Update()
        { 
            if (m_nowSprite != m_mask_sprite)
            {
                UpdateMaskTex();
            }
        }
#endif

        public void SetColor(string property_name, Color color)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            m_sprite_renderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor(property_name, color);
            m_sprite_renderer.SetPropertyBlock(materialPropertyBlock);
        }

        public void SetFloat(string property_name, float f)
        {
            m_sprite_renderer.material.SetFloat(property_name, f);
        }

        public void UpdatedMaterial(string mat_path,Action callback = null)
        {
            //string key = m_mask_sprite.texture.name + "_" + mat_path;
            //if (s_mat_catch.ContainsKey(key))
            //{
            //    Material material = s_mat_catch[key];
            //    if (m_sprite_renderer != null)
            //    {
            //        m_sprite_renderer.material = material;
            //    }
            //    UpdateMaskTex();
            //}
            //else
            //{
            CoreUtils.assetService.LoadAssetAsync<Material>(mat_path, (asset) =>
            {
                if (asset.asset() && this!=null)
                {
                    m_sprite_renderer.material = asset.asset() as Material;
                    UpdateMaskTex();
                    
                    callback?.Invoke();
                }
            }, gameObject);
            //}
        }

        private void OnCanvasGroupChanged()
        {
            if (m_has_inited)
            {
                if (m_canvas_group == null)
                {
                    m_canvas_group = GetComponentInParent<CanvasGroup>();
                }
                Color color = m_sprite_renderer.color;
                color.a = m_canvas_group.alpha * m_org_sprite_alpha;
                m_sprite_renderer.color = color;
            }
        }

        public void SetCanChangeLight(bool canChangeLight)
        {
            NightingMask component = GetComponent<NightingMask>();
            if (component != null)
            {
                component.SetCanChangeLight(canChangeLight);
            }
        }
    }
}
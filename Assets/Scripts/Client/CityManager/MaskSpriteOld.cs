
using System.Collections.Generic;
using UnityEngine;

public class MaskSpriteOld : MonoBehaviour
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
		UpdateTex();
		m_has_inited = true;
	}

	private void UpdateTex()
	{
		if (m_mask_sprite != null && m_sprite_renderer.sharedMaterial.GetTexture("_Mask") != m_mask_sprite.texture)
		{
			string name = m_sprite_renderer.material.name;
			string key = m_mask_sprite.texture.name + "_" + name.Replace("(Instance)", string.Empty).Trim();
			Material material;
			if (s_mat_catch.ContainsKey(key))
			{
				material = s_mat_catch[key];
			}
			else
			{
				material = m_sprite_renderer.material;
				material.SetTexture("_Mask", m_mask_sprite.texture);
				s_mat_catch.Add(key, material);
			}
			m_sprite_renderer.material = material;
		}
	}

	private void SetColor(string property_name, Color color)
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		m_sprite_renderer.GetPropertyBlock(materialPropertyBlock);
		materialPropertyBlock.SetColor(property_name, color);
		m_sprite_renderer.SetPropertyBlock(materialPropertyBlock);
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
}

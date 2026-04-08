using UnityEngine;

public class ChangeSpriteColor : MonoBehaviour
{
	public Transform m_change_color_sprite;

	private Renderer m_renderer;

	private void Awake()
	{
		if ((bool)m_change_color_sprite)
		{
			m_renderer = m_change_color_sprite.GetComponent<Renderer>();
		}
	}

	public static void SetColor(ChangeSpriteColor self, Color color)
	{
        if (self == null)
        {
            return;
        }
		if ((bool)self.m_change_color_sprite)
		{
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            self.m_renderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_Color", color);
            self.m_renderer.SetPropertyBlock(materialPropertyBlock);
        }
	}
}

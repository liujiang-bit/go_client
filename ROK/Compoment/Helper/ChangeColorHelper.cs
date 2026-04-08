using System;
using UnityEngine;

namespace ROK
{
    public class ChangeColorHelper : MonoBehaviour
    {
        public Transform m_change_color_sprite;

        private Renderer m_renderer;

        private void Awake()
        {
            if (this.m_change_color_sprite)
            {
                this.m_renderer = this.m_change_color_sprite.GetComponent<Renderer>();
            }
        }

        public static void SetColor(ChangeColorHelper self, Color color)
        {
            if (self.m_change_color_sprite)
            {
                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                materialPropertyBlock.SetColor("_Color", color);
                self.m_renderer.SetPropertyBlock(materialPropertyBlock);
            }
        }
    }
}
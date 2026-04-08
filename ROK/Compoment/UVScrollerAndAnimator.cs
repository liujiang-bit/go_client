using System;
using UnityEngine;

namespace ROK
{
    public class UVScrollerAndAnimator : MonoBehaviour
    {
        public float speedX;

        public float speedY = 0.5f;

        public float tileX = 24f;

        public float tileY = 1f;

        private float m_timeWentX;

        private float m_timeWentY;

        private Renderer m_renderer;

        public string UVName = "_MainTex";

        private void Start()
        {
            this.m_renderer = base.GetComponent<Renderer>();
        }

        private void Update()
        {
            try
            {
                this.m_timeWentY += Time.deltaTime * this.speedY;
                this.m_timeWentX += Time.deltaTime * this.speedX;
                if (this.m_renderer)
                {
                    this.m_renderer.material.SetTextureOffset(this.UVName, new Vector2(this.m_timeWentX, this.m_timeWentY));
                }
                Vector2 value = new Vector2(1f / this.tileX, 1f / this.tileY);
                if (this.m_renderer)
                {
                    this.m_renderer.material.SetTextureScale(this.UVName, value);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
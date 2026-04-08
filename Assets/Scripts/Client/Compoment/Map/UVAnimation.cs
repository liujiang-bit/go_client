using System;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// UI特效动画  附在物体上的挠动效果
    /// map_building_egypt/AltarOfOsiris  /map_landform/river_E
    /// </summary>
    public class UVAnimation : MonoBehaviour
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
            m_renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            try
            {
                m_timeWentY += Time.deltaTime * speedY;
                m_timeWentX += Time.deltaTime * speedX;
                if ((bool)m_renderer)
                {
                    m_renderer.material.SetTextureOffset(UVName, new Vector2(m_timeWentX, m_timeWentY));
                }
                Vector2 value = new Vector2(1f / tileX, 1f / tileY);
                if ((bool)m_renderer)
                {
                    m_renderer.material.SetTextureScale(UVName, value);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
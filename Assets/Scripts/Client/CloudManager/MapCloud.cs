using System;
using UnityEngine;
namespace Client
{
    /// <summary>
    /// map_cloud/map_cloud10.prefab   地图上的云处理
    /// </summary>
    public class MapCloud : MonoBehaviour
    {
        public enum CloudState
        {
            None,
            FadeIn,
            Idle,
            FadeOut,
            Destroy
        }

        private CloudState m_state = CloudState.FadeIn;

        private float m_stateMaxTime;

        private float m_stateTime;

        public float m_speed;

        public Vector2 m_size = Vector2.one;

        public float m_idleTime;

        public float m_scale = 1f;

        public float m_scaleRate = 1f;

        public float m_scaleSpeed = 1f;

        public bool m_needAdjustScalePos;

        private void Start()
        {
        }

        public void UpdateCloud()
        {
            if (m_state == CloudState.FadeIn)
            {
                m_stateTime += Time.deltaTime;
                float a = 1f;
                if (m_stateTime >= m_stateMaxTime)
                {
                    m_state = CloudState.Idle;
                    m_stateMaxTime = m_idleTime;
                    m_stateTime = 0f;
                }
                else
                {
                    a = m_stateTime / m_stateMaxTime;
                }
                SpriteRenderer component = GetComponent<SpriteRenderer>();
                Color color = component.color;
                color.a = a;
                component.color = color;
                UpdateMove();
                UpdateScale();
            }
            else if (m_state == CloudState.Idle)
            {
                m_stateTime += Time.deltaTime;
                if (m_stateTime >= m_stateMaxTime)
                {
                    SetFadeOut();
                    return;
                }
                UpdateMove();
                UpdateScale();
                Camera main = Camera.main;
                Vector3 vector = main.WorldToViewportPoint(base.transform.position);
                float num = 0f;
                float num2 = 0f;
                float num3 = main.nearClipPlane * Mathf.Cos((float)Math.PI / 180f * (main.fieldOfView * 0.5f));
                Vector3 position = main.transform.position;
                float y = position.y;
                Vector3 position2 = base.transform.position;
                float num4 = num3 / (y - position2.y);
                num += m_size.x * 0.5f * num4;
                num2 += m_size.y * 0.5f * num4;
                if (vector.x < 0f - num || vector.x > 1f + num || vector.y < 0f - num2 || vector.y > 1f + num2 * 0.2f)
                {
                    m_state = CloudState.Destroy;
                }
            }
            else if (m_state == CloudState.FadeOut)
            {
                m_stateTime += Time.deltaTime;
                float a2 = 0f;
                if (m_stateTime >= m_stateMaxTime)
                {
                    m_state = CloudState.Destroy;
                }
                else
                {
                    a2 = 1f - m_stateTime / m_stateMaxTime;
                }
                SpriteRenderer component2 = GetComponent<SpriteRenderer>();
                Color color2 = component2.color;
                color2.a = a2;
                component2.color = color2;
                UpdateMove();
                UpdateScale();
            }
        }

        public void UpdateMove()
        {
            if (m_speed > 0f)
            {
                Vector3 position = base.transform.position;
                position += Vector3.right * (m_speed * Time.deltaTime);
                base.transform.position = position;
            }
        }

        public void UpdateScale()
        {
            if (m_scale < 1f)
            {
                float scale = m_scale;
                m_scale += m_scaleSpeed * Time.deltaTime;
                if (m_scale > 1f)
                {
                    m_scale = 1f;
                }
                float num = m_scale * m_scaleRate;
                base.transform.localScale = new Vector3(num, num, num);
                if (m_needAdjustScalePos)
                {
                    float num2 = (m_scale - scale) * m_scaleRate;
                    float num3 = num2 * 0.2f * m_size.x;
                    Vector3 position = base.transform.position;
                    position.x += num3;
                    base.transform.position = position;
                }
            }
        }

        public void SetStart(float fadeTime = 2f)
        {
            m_state = CloudState.FadeIn;
            m_stateMaxTime = fadeTime;
            m_stateTime = 0f;
        }

        public void SetFadeOut(float fadeTime = 1.5f)
        {
            if (m_state < CloudState.FadeOut)
            {
                m_state = CloudState.FadeOut;
                m_stateMaxTime = fadeTime;
                m_stateTime = 0f;
            }
        }

        public bool NeedDestroy()
        {
            return m_state == CloudState.Destroy;
        }
    }
}
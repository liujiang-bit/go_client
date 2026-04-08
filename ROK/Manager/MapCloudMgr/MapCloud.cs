using System;
using UnityEngine;

namespace ROK
{
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

        private MapCloud.CloudState m_state = MapCloud.CloudState.FadeIn;

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
            if (this.m_state == MapCloud.CloudState.FadeIn)
            {
                this.m_stateTime += Time.deltaTime;
                float a = 1f;
                if (this.m_stateTime >= this.m_stateMaxTime)
                {
                    this.m_state = MapCloud.CloudState.Idle;
                    this.m_stateMaxTime = this.m_idleTime;
                    this.m_stateTime = 0f;
                }
                else
                {
                    a = this.m_stateTime / this.m_stateMaxTime;
                }
                SpriteRenderer component = base.GetComponent<SpriteRenderer>();
                Color color = component.color;
                color.a = a;
                component.color = color;
                this.UpdateMove();
                this.UpdateScale();
            }
            else if (this.m_state == MapCloud.CloudState.Idle)
            {
                this.m_stateTime += Time.deltaTime;
                if (this.m_stateTime >= this.m_stateMaxTime)
                {
                    this.SetFadeOut(1.5f);
                }
                else
                {
                    this.UpdateMove();
                    this.UpdateScale();
                    Camera main = Camera.main;
                    Vector3 vector = main.WorldToViewportPoint(base.transform.position);
                    float num = 0f;
                    float num2 = 0f;
                    float num3 = main.nearClipPlane * Mathf.Cos(0.0174532924f * (main.fieldOfView * 0.5f)) / (main.transform.position.y - base.transform.position.y);
                    num += this.m_size.x * 0.5f * num3;
                    num2 += this.m_size.y * 0.5f * num3;
                    if (vector.x < -num || vector.x > 1f + num || vector.y < -num2 || vector.y > 1f + num2 * 0.2f)
                    {
                        this.m_state = MapCloud.CloudState.Destroy;
                    }
                }
            }
            else if (this.m_state == MapCloud.CloudState.FadeOut)
            {
                this.m_stateTime += Time.deltaTime;
                float a2 = 0f;
                if (this.m_stateTime >= this.m_stateMaxTime)
                {
                    this.m_state = MapCloud.CloudState.Destroy;
                }
                else
                {
                    a2 = 1f - this.m_stateTime / this.m_stateMaxTime;
                }
                SpriteRenderer component2 = base.GetComponent<SpriteRenderer>();
                Color color2 = component2.color;
                color2.a = a2;
                component2.color = color2;
                this.UpdateMove();
                this.UpdateScale();
            }
        }

        public void UpdateMove()
        {
            if (this.m_speed > 0f)
            {
                Vector3 vector = base.transform.position;
                vector += Vector3.right * (this.m_speed * Time.deltaTime);
                base.transform.position = vector;
            }
        }

        public void UpdateScale()
        {
            if (this.m_scale < 1f)
            {
                float scale = this.m_scale;
                this.m_scale += this.m_scaleSpeed * Time.deltaTime;
                if (this.m_scale > 1f)
                {
                    this.m_scale = 1f;
                }
                float num = this.m_scale * this.m_scaleRate;
                base.transform.localScale = new Vector3(num, num, num);
                if (this.m_needAdjustScalePos)
                {
                    float num2 = (this.m_scale - scale) * this.m_scaleRate;
                    float num3 = num2 * 0.2f * this.m_size.x;
                    Vector3 position = base.transform.position;
                    position.x += num3;
                    base.transform.position = position;
                }
            }
        }

        public void SetStart(float fadeTime = 2f)
        {
            this.m_state = MapCloud.CloudState.FadeIn;
            this.m_stateMaxTime = fadeTime;
            this.m_stateTime = 0f;
        }

        public void SetFadeOut(float fadeTime = 1.5f)
        {
            if (this.m_state < MapCloud.CloudState.FadeOut)
            {
                this.m_state = MapCloud.CloudState.FadeOut;
                this.m_stateMaxTime = fadeTime;
                this.m_stateTime = 0f;
            }
        }

        public bool NeedDestroy()
        {
            return this.m_state == MapCloud.CloudState.Destroy;
        }
    }
}
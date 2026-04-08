using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ROK
{
    public class UnitShadowLod1 : MonoBehaviour
    {
        public enum ENUM_FADE_STATE
        {
            NONE,
            FADE_IN,
            FADE_OUT,
            NORMAL
        }

        public UnitShadowLod1.ENUM_FADE_STATE m_fade_state;

        private SpriteRenderer m_sprite_renderer;

        private void Awake()
        {
            this.m_sprite_renderer = base.GetComponent<SpriteRenderer>();
        }

        public void FadeIn()
        {
            UnitShadowLod1.ENUM_FADE_STATE fade_state = this.m_fade_state;
            this.m_fade_state = UnitShadowLod1.ENUM_FADE_STATE.FADE_IN;
            if (fade_state == UnitShadowLod1.ENUM_FADE_STATE.NONE && this.m_sprite_renderer != null)
            {
                this.m_sprite_renderer.color = new Color(0f, 0f, 0f, 0f);
                if (base.isActiveAndEnabled)
                {
                    base.StartCoroutine(this.UpdateFade());
                }
            }
        }

        public bool Fadeout()
        {
            if (base.gameObject.activeInHierarchy)
            {
                UnitShadowLod1.ENUM_FADE_STATE fade_state = this.m_fade_state;
                this.m_fade_state = UnitShadowLod1.ENUM_FADE_STATE.FADE_OUT;
                if (fade_state == UnitShadowLod1.ENUM_FADE_STATE.NONE)
                {
                    this.m_sprite_renderer.color = new Color(0f, 0f, 0f, 1f);
                    if (base.isActiveAndEnabled)
                    {
                        base.StartCoroutine(this.UpdateFade());
                    }
                }
                return true;
            }
            return false;
        }

        private IEnumerator UpdateFade()
        {
            while (m_fade_state != 0)
            {
                try
                {
                    if (m_fade_state == ENUM_FADE_STATE.FADE_IN)
                    {
                        Color color = m_sprite_renderer.color;
                        color.a += Time.deltaTime * 3f;
                        if (color.a >= 1f)
                        {
                            color.a = 1f;
                            m_fade_state = ENUM_FADE_STATE.NONE;
                            m_sprite_renderer.color = color;
                        }
                        m_sprite_renderer.color = color;
                    }
                    else if (m_fade_state == ENUM_FADE_STATE.FADE_OUT)
                    {
                        Color color2 = m_sprite_renderer.color;
                        float num = Time.deltaTime * 3f;
                        color2.a -= num;
                        if (color2.a <= 0f)
                        {
                            color2.a = 0f;
                            m_fade_state = ENUM_FADE_STATE.NONE;
                            m_sprite_renderer.color = color2;
                        }
                        m_sprite_renderer.color = color2;
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
                yield return null;
            }
        }
    }
}
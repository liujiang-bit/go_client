using System;
using UnityEngine;

namespace Client
{
    public class SpiteAnimation : MonoBehaviour
    {
        private int m_cur_frame;

        public float m_update_rate = 0.2f;

        public Sprite[] m_cur_sprite;

        private SpriteRenderer m_sprite_render;

        public bool m_autoPlay = true;

        public bool m_isNeat;

        private void Awake()
        {
            m_sprite_render = GetComponent<SpriteRenderer>();
            if (m_autoPlay && !m_isNeat)
            {
                m_cur_frame = UnityEngine.Random.Range(0, m_cur_sprite.Length);
            }
        }

        private void OnEnable()
        {
            if (m_autoPlay)
            {
                InvokeRepeating("UpdateAnimation", UnityEngine.Random.Range(0f, 0.016f), m_update_rate);
            }
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        private void UpdateAnimation()
        {
            try
            {
                if (m_cur_frame == m_cur_sprite.Length - 1)
                {
                    m_cur_frame = 0;
                }
                else if (m_cur_frame >= m_cur_sprite.Length)
                {
                    m_cur_frame = 0;
                }
                else
                {
                    m_cur_frame++;
                }
                m_sprite_render.sprite = m_cur_sprite[m_cur_frame];
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        virtual public void Play()
        {
            if (m_autoPlay)
            {
                return;
            }
            m_autoPlay = true;
            InvokeRepeating("UpdateAnimation", UnityEngine.Random.Range(0f, 0.016f), m_update_rate);
        }

        virtual public void Stop()
        {
            if (!m_autoPlay)
            {
                return;
            }
            m_autoPlay = false;
            CancelInvoke("UpdateAnimation");
            m_sprite_render.sprite = m_cur_sprite[0];
        }
    }
}
using System;
using UnityEngine;

namespace ROK
{
    public class TextureSheetAnimationSimple : MonoBehaviour
    {
        private int m_cur_frame;

        public float m_update_rate = 0.2f;

        public Sprite[] m_cur_sprite;

        private SpriteRenderer m_sprite_render;

        public bool m_autoPlay = true;

        public bool m_isNeat;

        private void Awake()
        {
            this.m_sprite_render = base.GetComponent<SpriteRenderer>();
            if (this.m_autoPlay && !this.m_isNeat)
            {
                this.m_cur_frame = UnityEngine.Random.Range(0, this.m_cur_sprite.Length);
            }
        }

        private void OnEnable()
        {
            if (this.m_autoPlay)
            {
                base.InvokeRepeating("UpdateAnimation", UnityEngine.Random.Range(0f, 0.016f), this.m_update_rate);
            }
        }

        private void OnDisable()
        {
            base.CancelInvoke();
        }

        private void UpdateAnimation()
        {
            try
            {
                if (this.m_cur_frame == this.m_cur_sprite.Length - 1)
                {
                    this.m_cur_frame = 0;
                }
                else if (this.m_cur_frame >= this.m_cur_sprite.Length)
                {
                    this.m_cur_frame = 0;
                }
                else
                {
                    this.m_cur_frame++;
                }
                this.m_sprite_render.sprite = this.m_cur_sprite[this.m_cur_frame];
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void PlayS(TextureSheetAnimationSimple self)
        {
            self.Play();
        }

        private void Play()
        {
            if (this.m_autoPlay)
            {
                return;
            }
            this.m_autoPlay = true;
            base.InvokeRepeating("UpdateAnimation", UnityEngine.Random.Range(0f, 0.016f), this.m_update_rate);
        }

        public static void StopS(TextureSheetAnimationSimple self)
        {
            self.Stop();
        }

        private void Stop()
        {
            if (!this.m_autoPlay)
            {
                return;
            }
            this.m_autoPlay = false;
            base.CancelInvoke("UpdateAnimation");
            this.m_sprite_render.sprite = this.m_cur_sprite[0];
        }
    }
}
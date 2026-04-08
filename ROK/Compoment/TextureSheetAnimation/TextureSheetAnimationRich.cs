using System;
using UnityEngine;

namespace ROK
{
    public class TextureSheetAnimationRich : MonoBehaviour
    {
        private int m_cur_frame;

        public float m_update_rate = 0.1f;

        private Sprite[] m_cur_sprite;

        private SpriteRenderer m_sprite_render;

        public bool m_autoPlay = true;

        public SpriteGroup[] m_states;

        public TextureSheetAnimationStateController[] m_state_controllers;

        private int m_cur_repeat_times;

        private int m_cur_state_idx;

        private void Awake()
        {
            this.m_sprite_render = base.GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (this.m_autoPlay)
            {
                this.ResetAniState();
            }
        }

        private void OnDisable()
        {
            base.CancelInvoke();
        }

        private void OnSpawn()
        {
            this.ResetAniState();
        }

        private void OnDespawn()
        {
            base.CancelInvoke();
        }

        private void ResetAniState()
        {
            this.m_cur_frame = 0;
            this.m_cur_state_idx = 0;
            this.m_cur_repeat_times = 0;
            if (this.m_state_controllers == null || this.m_state_controllers.Length == 0)
            {
                base.InvokeRepeating("UpdateAnimation", 0f, this.m_update_rate);
            }
            else
            {
                this.m_cur_sprite = this.m_states[0].m_sprite_array;
                base.InvokeRepeating("UpdateAnimationRich", 0f, this.m_update_rate);
            }
        }

        private void UpdateAnimation()
        {
            try
            {
                if (this.m_cur_frame <= this.m_cur_sprite.Length - 1)
                {
                    this.m_sprite_render.sprite = this.m_cur_sprite[this.m_cur_frame];
                }
                this.m_cur_frame++;
                if (this.m_cur_frame > this.m_cur_sprite.Length - 1)
                {
                    this.m_cur_frame = 0;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void UpdateAnimationRich()
        {
            try
            {
                if (this.m_cur_frame <= this.m_cur_sprite.Length - 1)
                {
                    this.m_sprite_render.sprite = this.m_cur_sprite[this.m_cur_frame];
                }
                this.m_cur_frame++;
                if (this.m_cur_frame > this.m_cur_sprite.Length - 1)
                {
                    if (this.m_cur_state_idx > this.m_state_controllers.Length - 1)
                    {
                        this.m_cur_state_idx = 0;
                    }
                    TextureSheetAnimationStateController textureSheetAnimationStateController = this.m_state_controllers[this.m_cur_state_idx];
                    if (this.m_cur_repeat_times > textureSheetAnimationStateController.m_repeat_times - 1)
                    {
                        this.m_cur_state_idx++;
                        if (this.m_cur_state_idx > this.m_state_controllers.Length - 1)
                        {
                            this.m_cur_state_idx = 0;
                        }
                        this.m_cur_repeat_times = 0;
                        this.m_cur_frame = 0;
                        textureSheetAnimationStateController = this.m_state_controllers[this.m_cur_state_idx];
                        this.m_cur_sprite = this.m_states[textureSheetAnimationStateController.m_state_id].m_sprite_array;
                    }
                    else
                    {
                        this.m_cur_repeat_times++;
                        this.m_cur_frame = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void PlayS(TextureSheetAnimationRich self)
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
            base.InvokeRepeating("UpdateAnimation", 0f, this.m_update_rate);
        }

        public static void StopS(TextureSheetAnimationRich self)
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
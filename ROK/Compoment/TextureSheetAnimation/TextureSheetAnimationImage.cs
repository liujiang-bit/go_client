using System;
using UnityEngine;
using UnityEngine.UI;

namespace ROK
{
    [ExecuteInEditMode]
    public class TextureSheetAnimationImage : MonoBehaviour
    {
        private int m_cur_frame;

        public float m_update_rate = 0.2f;

        public Sprite[] m_cur_sprite;

        private Image m_img;

        private void Awake()
        {
            this.m_img = base.GetComponent<Image>();
            this.m_cur_frame = UnityEngine.Random.Range(0, this.m_cur_sprite.Length);
        }

        private void OnEnable()
        {
            base.InvokeRepeating("UpdateAnimation", UnityEngine.Random.Range(0f, 0.016f), this.m_update_rate);
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
                this.m_img.sprite = this.m_cur_sprite[this.m_cur_frame];
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
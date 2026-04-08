using System;
using UnityEngine;

namespace ROK
{
    public class TroopLine : MonoBehaviour
    {
        private enum STATE
        {
            NORMAL,
            FADE_OUT,
            FADE_IN,
            NUMBER
        }

        private Material m_mat;

        private LineRenderer m_line_renderer;

        private Color m_color = Color.red;

        public static float s_fade_time = 0.5f;

        private float fade_timer;

        private TroopLine.STATE m_state;

        private float m_remove_point_thresh_hold = 0.1f;

        private void Awake()
        {
            this.m_line_renderer = base.transform.GetComponent<LineRenderer>();
        }

        public void SetColor(Color color)
        {
            this.m_color = color;
            this.m_line_renderer.startColor = color;
            this.m_line_renderer.endColor = color;
        }

        public void Fade(bool fade_in)
        {
            this.fade_timer = 0f;
            if (fade_in)
            {
                this.m_state = TroopLine.STATE.FADE_IN;
            }
            else
            {
                this.m_state = TroopLine.STATE.FADE_OUT;
            }
        }

        private void Update()
        {
            try
            {
                if (this.m_state == TroopLine.STATE.FADE_IN)
                {
                    this.m_color.a = 1f - this.fade_timer / TroopLine.s_fade_time;
                    if (this.m_color.a <= 0f)
                    {
                        this.m_color.a = 0f;
                        this.m_state = TroopLine.STATE.NORMAL;
                    }
                    this.SetColor(this.m_color);
                    this.fade_timer += Time.deltaTime;
                }
                else if (this.m_state == TroopLine.STATE.FADE_OUT)
                {
                    this.m_color.a = this.fade_timer / TroopLine.s_fade_time;
                    if (this.m_color.a >= 1f)
                    {
                        this.m_color.a = 1f;
                        this.m_state = TroopLine.STATE.NORMAL;
                    }
                    this.SetColor(this.m_color);
                    this.fade_timer += Time.deltaTime;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
using System;

namespace ROK
{
    public struct Thunder
    {
        public float m_intensity;

        public float m_fadein_time;

        public float m_fadeout_time;

        public float m_timer;

        public Thunder(float intensity, float fadein, float fadeout)
        {
            this.m_intensity = intensity;
            this.m_fadein_time = fadein;
            this.m_fadeout_time = fadeout;
            this.m_timer = 0f;
        }
    }
}
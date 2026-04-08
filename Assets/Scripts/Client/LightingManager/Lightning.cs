namespace Client
{
    public struct Lighting
    {
        public float m_intensity;

        public float m_fadein_time;

        public float m_fadeout_time;

        public float m_timer;

        public Lighting(float intensity, float fadein, float fadeout)
        {
            m_intensity = intensity;
            m_fadein_time = fadein;
            m_fadeout_time = fadeout;
            m_timer = 0f;
        }
    }
}
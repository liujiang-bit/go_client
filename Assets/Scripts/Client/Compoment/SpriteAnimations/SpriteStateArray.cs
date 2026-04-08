using System;

namespace Client
{
    [Serializable]
    public class SpriteStateArray
    {
        public SpriteArray[] m_direction;

        public float m_update_rate = 0.1f;
    }
}

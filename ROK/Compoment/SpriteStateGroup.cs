using System;

namespace ROK
{
    [Serializable]
    public class SpriteStateGroup
    {
        public SpriteGroup[] m_direction;

        public float m_update_rate = 0.1f;
    }
}

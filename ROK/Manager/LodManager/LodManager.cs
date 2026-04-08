using System;

namespace ROK
{
    public class LodManager
    {
        private static readonly LodManager m_instance = new LodManager();

        public EventDispather m_event_dispatcher = new EventDispather();

        public static LodManager GetInstance()
        {
            return LodManager.m_instance;
        }

        public void UpdateLod()
        {
            this.m_event_dispatcher.Dispath();
        }

    }
}
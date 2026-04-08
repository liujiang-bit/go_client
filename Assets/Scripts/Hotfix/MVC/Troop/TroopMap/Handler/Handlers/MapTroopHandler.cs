namespace Hotfix
{
    public class MapTroopHandler : IMapTroopHandler
    {
        private ITroopMgr m_TroopMgr;
        public void Init()
        {
            if (m_TroopMgr == null)
            {
                m_TroopMgr= new TroopMgr();
            }
        }

        public void Clear()
        {
            if(m_TroopMgr != null)
            {
                m_TroopMgr.Clear();
            }
        }

        public ITroopMgr GetITroopMgr()
        {
            return m_TroopMgr;
        }
    }
}
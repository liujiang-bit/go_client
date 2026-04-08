using System;

namespace ROK
{
    public class GraphicSettingMgr
    {
        public enum GraphicLevel
        {
            NONE,
            LOW,
            MEDIUM,
            HIGH
        }

        private static GraphicLevel m_graphic_level = GraphicSettingMgr.GraphicLevel.LOW;

        public static void SetGraphicLevel(int level)
        {
            m_graphic_level = (GraphicSettingMgr.GraphicLevel)level;
        }

        public static GraphicSettingMgr.GraphicLevel GetGraphicLevel()
        {
            return m_graphic_level;
        }
    }
}
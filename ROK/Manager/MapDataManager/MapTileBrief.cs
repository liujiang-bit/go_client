using System;
using UnityEngine;

namespace ROK
{
    public class MapTileBrief
    {
        public static int m_province_name_count = 4;

        public int m_x;

        public int m_y;

        public string m_tile_id = string.Empty;

        public float m_tile_rot;

        public bool m_enable_bridge = true;

        public bool m_river_flow_direction;

        public bool m_show_province_name;

        public string[] m_province_name_array = new string[MapTileBrief.m_province_name_count];

        public MapTileBrief(int x, int y, string id, float rot, bool enable_brigde, bool river_flow_direction, bool show_province_name, string[] province_name_array)
        {
            this.m_x = x;
            this.m_y = y;
            this.m_tile_id = id;
            this.m_tile_rot = rot;
            this.m_enable_bridge = enable_brigde;
            this.m_river_flow_direction = river_flow_direction;
            this.m_show_province_name = show_province_name;
            this.m_province_name_array = province_name_array;
        }

        public Vector3 GetWorldPos()
        {
            float tile_width = MapDataManager.GetInstance().m_tile_width;
            return new Vector3(((float)this.m_x + 0.5f) * tile_width, 0f, ((float)this.m_y + 0.5f) * tile_width);
        }
    }
}
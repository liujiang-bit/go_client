using System;
using UnityEngine;

namespace ROK
{
    public class MapObjectData
    {
        public string m_prefab_id = string.Empty;

        public Vector3 m_pos = Vector3.zero;

        public float m_rot_y;

        public Vector3 m_scale = Vector3.one;

        public bool m_river_flow_direction = true;

        public int m_lod = -1;

        public MapObjectData(string prefab_id, Vector3 pos, float rot_y, Vector3 scale, bool river_flow_direction, int lod)
        {
            this.m_prefab_id = prefab_id;
            this.m_pos = pos;
            this.m_rot_y = rot_y;
            this.m_scale = scale;
            this.m_river_flow_direction = river_flow_direction;
            this.m_lod = lod;
        }

        public MapObjectData(MapObjectData map_object_data)
        {
            this.m_prefab_id = map_object_data.m_prefab_id;
            this.m_pos = map_object_data.m_pos;
            this.m_rot_y = map_object_data.m_rot_y;
            this.m_scale = map_object_data.m_scale;
            this.m_river_flow_direction = map_object_data.m_river_flow_direction;
            this.m_lod = map_object_data.m_lod;
        }
    }
}
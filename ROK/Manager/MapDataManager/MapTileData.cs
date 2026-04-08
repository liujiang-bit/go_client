using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class MapTileData : ICloneable
    {
        public List<MapObjectData> m_map_obj_data_list = new List<MapObjectData>();

        public List<MapObjectData> GetMapObjDataInRange(Rect range_rect)
        {
            List<MapObjectData> list = new List<MapObjectData>();
            for (int i = 0; i < this.m_map_obj_data_list.Count; i++)
            {
                Vector2 point = new Vector2(this.m_map_obj_data_list[i].m_pos.x, this.m_map_obj_data_list[i].m_pos.z);
                if (Common.RectLeftBottomContains(range_rect, point))
                {
                    list.Add(this.m_map_obj_data_list[i]);
                }
            }
            return list;
        }

        public void GenerateMapObjectData(int tile_x, int tile_y, float rot_y, bool enable_bridge, bool river_flow_direction)
        {
            for (int i = 0; i < this.m_map_obj_data_list.Count; i++)
            {
                if (enable_bridge || !this.m_map_obj_data_list[i].m_prefab_id.Contains("bridge"))
                {
                    float num = MapDataManager.GetInstance().m_tile_width / 2f;
                    Vector3 b = new Vector3(num, 0f, num);
                    this.m_map_obj_data_list[i].m_pos -= b;
                    float f = rot_y / 180f * 3.14159274f;
                    float x = this.m_map_obj_data_list[i].m_pos.x;
                    float z = this.m_map_obj_data_list[i].m_pos.z;
                    this.m_map_obj_data_list[i].m_pos.x = x * Mathf.Cos(f) + z * Mathf.Sin(f);
                    this.m_map_obj_data_list[i].m_pos.z = -x * Mathf.Sin(f) + z * Mathf.Cos(f);
                    this.m_map_obj_data_list[i].m_pos += b;
                    this.m_map_obj_data_list[i].m_pos += new Vector3((float)tile_x * MapDataManager.GetInstance().m_tile_width, 0f, (float)tile_y * MapDataManager.GetInstance().m_tile_width);
                    this.m_map_obj_data_list[i].m_rot_y += rot_y;
                    this.m_map_obj_data_list[i].m_river_flow_direction = !(river_flow_direction ^ this.m_map_obj_data_list[i].m_river_flow_direction);
                }
            }
        }

        public object DeepClone()
        {
            MapTileData mapTileData = new MapTileData();
            for (int i = 0; i < this.m_map_obj_data_list.Count; i++)
            {
                mapTileData.m_map_obj_data_list.Add(new MapObjectData(this.m_map_obj_data_list[i]));
            }
            return mapTileData;
        }

        public object DeepCloneHalf()
        {
            MapTileData mapTileData = new MapTileData();
            for (int i = 0; i < this.m_map_obj_data_list.Count; i++)
            {
                if (UnityEngine.Random.Range(0, 2) != 1)
                {
                    mapTileData.m_map_obj_data_list.Add(new MapObjectData(this.m_map_obj_data_list[i]));
                }
            }
            return mapTileData;
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class MapCityManager
    {
        public static Dictionary<string, MapCityItem> m_city_dict = new Dictionary<string, MapCityItem>();

        public static void AddCity(string city_id, float city_pos_x, float city_pos_y, float remove_grove_distance)
        {
            Vector2 pos = new Vector2(city_pos_x, city_pos_y);
            if (!MapCityManager.m_city_dict.ContainsKey(city_id))
            {
                MapCityManager.m_city_dict.Add(city_id, new MapCityItem(pos, remove_grove_distance));
                MapPieceManager.GetInstance().UpdateGrove(city_pos_x, city_pos_y, remove_grove_distance);
            }
        }

        public static void RemoveCity(string city_id)
        {
            MapCityManager.m_city_dict.Remove(city_id);
        }
    }
}
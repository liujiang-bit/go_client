using System;
using UnityEngine;

namespace Client
{
    public class MapTownItem
    {
        public Vector2 m_pos;

        public float m_remove_grove_distance;

        public MapTownItem(Vector2 pos, float remove_grove_distance)
        {
            this.m_pos = pos;
            this.m_remove_grove_distance = remove_grove_distance;
        }
    }
}
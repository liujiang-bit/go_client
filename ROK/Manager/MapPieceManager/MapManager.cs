using System;
using UnityEngine;

namespace ROK
{
    public class MapManager 
    {
        private static readonly MapManager m_instance = new MapManager();

        private Vector2 m_current_view_center = Vector2.zero;

        public static MapManager GetInstance()
        {
            return MapManager.m_instance;
        }

        public void UpdateViewCenter(Vector2 pos)
        {
            this.m_current_view_center = pos;
            MapDataManager.GetInstance().UpdateTile(pos);
            MapPieceManager.GetInstance().UpdatePiece(pos);
        }
    }
}
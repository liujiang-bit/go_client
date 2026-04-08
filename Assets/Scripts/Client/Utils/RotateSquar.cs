using System;
using UnityEngine;

namespace Client
{
    public class RotateSquar
    {
        public Vector2[] m_world_corners = new Vector2[4];

        public Vector2[] m_local_corners = new Vector2[4];

        private const float sin45 = 0.70721f;

        private const float sqrt2 = 1.414f;

        public RotateSquar(Vector2 pos, Vector2 size)
        {
            this.UpdateRect(pos, size);
        }
        /// <summary>
        /// 地图坐标与选中坐标之间的换算
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        public void UpdateRect(Vector2 pos, Vector2 size)
        {
            size /= 2f;
            this.m_world_corners[0].x = pos.x + size.y * 0.70721f - size.x * 0.70721f;
            this.m_world_corners[0].y = pos.y + size.y * 0.70721f + size.x * 0.70721f;
            this.m_world_corners[1].x = pos.x + size.y * 0.70721f + size.x * 0.70721f;
            this.m_world_corners[1].y = pos.y + size.y * 0.70721f - size.x * 0.70721f;
            this.m_world_corners[2].x = pos.x - size.y * 0.70721f + size.x * 0.70721f;
            this.m_world_corners[2].y = pos.y - size.y * 0.70721f - size.x * 0.70721f;
            this.m_world_corners[3].x = pos.x - size.y * 0.70721f - size.x * 0.70721f;
            this.m_world_corners[3].y = pos.y - size.y * 0.70721f + size.x * 0.70721f;
            this.m_local_corners[0].x = this.m_world_corners[0].x * 0.70721f + this.m_world_corners[0].y * 0.70721f;
            this.m_local_corners[0].y = -this.m_world_corners[0].x * 0.70721f + this.m_world_corners[0].y * 0.70721f;
            this.m_local_corners[1].x = this.m_world_corners[1].x * 0.70721f + this.m_world_corners[1].y * 0.70721f;
            this.m_local_corners[1].y = -this.m_world_corners[1].x * 0.70721f + this.m_world_corners[1].y * 0.70721f;
            this.m_local_corners[2].x = this.m_world_corners[2].x * 0.70721f + this.m_world_corners[2].y * 0.70721f;
            this.m_local_corners[2].y = -this.m_world_corners[2].x * 0.70721f + this.m_world_corners[2].y * 0.70721f;
            this.m_local_corners[3].x = this.m_world_corners[3].x * 0.70721f + this.m_world_corners[3].y * 0.70721f;
            this.m_local_corners[3].y = -this.m_world_corners[3].x * 0.70721f + this.m_world_corners[3].y * 0.70721f;
        }

        public bool Overlaps(RotateSquar rotate_rect)
        {
            return this.IsOverlaps(this, rotate_rect) || this.IsOverlaps(rotate_rect, this);
        }

        public bool Contains(RotateSquar rotate_rect)
        {
            for (int i = 0; i < rotate_rect.m_local_corners.Length; i++)
            {
                if (rotate_rect.m_local_corners[i].x < this.m_local_corners[2].x || rotate_rect.m_local_corners[i].x > this.m_local_corners[1].x || rotate_rect.m_local_corners[i].y < this.m_local_corners[2].y || rotate_rect.m_local_corners[i].y > this.m_local_corners[0].y)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsOverlaps(RotateSquar rect0, RotateSquar rect1)
        {
            for (int i = 0; i < rect1.m_local_corners.Length; i++)
            {
                if (rect0.m_local_corners[2].x <= rect1.m_local_corners[i].x && rect0.m_local_corners[1].x >= rect1.m_local_corners[i].x && rect0.m_local_corners[2].y <= rect1.m_local_corners[i].y && rect0.m_local_corners[0].y >= rect1.m_local_corners[i].y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
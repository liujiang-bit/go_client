using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class TileCollideManager : MonoBehaviour
    {
        public Vector2 m_center;

        public Vector2 m_size = new Vector2(0.5f, 0.5f);

        private const float c_default_last_scale = -1f;

        private float m_lastScale = -1f;

        private bool m_is_need_update_onece;

        private int m_always_show_priority = 100;

        private static float s_default_scale = 1f;

        private RotateRect m_rect = new RotateRect(Vector2.zero, Vector2.one);

        private List<TileCollideItem> tileCollision = new List<TileCollideItem>();

        private float m_target_alpha = 1f;

        public float targetAlpha
        {
            get
            {
                return this.m_target_alpha;
            }
            set
            {
                this.m_target_alpha = value;
            }
        }

        public void SetTargetAlpha(float target_alpha)
        {
            for (int i = 0; i < this.tileCollision.Count; i++)
            {
                this.targetAlpha = target_alpha;
                this.tileCollision[i].SetAlpha(this.targetAlpha);
            }
        }

        private bool IsNeedUpdateLod(float scale)
        {
            if (this.m_is_need_update_onece)
            {
                this.m_is_need_update_onece = false;
                return true;
            }
            return this.m_lastScale != scale;
        }

        public void ShiftPos(float scale)
        {
            if (this.tileCollision.Count > 0 && this.tileCollision[0].m_shift_pos)
            {
                this.tileCollision[0].ShiftPos(scale);
            }
        }

        public void SetSize(Vector2 size)
        {
            this.m_size = size;
        }

        public void SetScale(float scale, bool isforce = false)
        {
            if (this.IsNeedUpdateLod(scale) || isforce)
            {
                this.ShiftPos(scale);
                List<TileCollideItem> list = new List<TileCollideItem>();
                if (scale > this.m_lastScale)
                {
                    foreach (TileCollideItem current in this.tileCollision)
                    {
                        if (current.m_is_active)
                        {
                            list.Add(current);
                        }
                    }
                }
                else
                {
                    list = new List<TileCollideItem>(this.tileCollision);
                }
                foreach (TileCollideItem current2 in this.tileCollision)
                {
                    current2.SetScale(scale);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].m_priority < this.m_always_show_priority && !this.GetGlobalRect().Contains(list[i].GetGlobalRect()))
                    {
                        list[i].Deactivate();
                        list.RemoveAt(i);
                        i--;
                    }
                }
                for (int j = 0; j <= list.Count - 1; j++)
                {
                    for (int k = j + 1; k < list.Count; k++)
                    {
                        if (list[j].GetGlobalRect().Overlaps(list[k].GetGlobalRect()))
                        {
                            list[k].Deactivate();
                            list.RemoveAt(k);
                            k--;
                        }
                    }
                }
                for (int l = 0; l < list.Count; l++)
                {
                    list[l].Activate();
                }
                this.m_lastScale = scale;
            }
        }

        private RotateRect GetGlobalRect()
        {
            Vector3 position = base.transform.position;
            this.m_rect.UpdateRect(new Vector2(base.transform.position.x, base.transform.position.z), new Vector2(this.m_size.x, this.m_size.y));
            return this.m_rect;
        }

        private void DebugDrawRect()
        {
            RotateRect globalRect = this.GetGlobalRect();
            Vector3 vector = new Vector3(globalRect.m_world_corners[0].x, 0f, globalRect.m_world_corners[0].y);
            Vector3 vector2 = new Vector3(globalRect.m_world_corners[1].x, 0f, globalRect.m_world_corners[1].y);
            Vector3 vector3 = new Vector3(globalRect.m_world_corners[2].x, 0f, globalRect.m_world_corners[2].y);
            Vector3 vector4 = new Vector3(globalRect.m_world_corners[3].x, 0f, globalRect.m_world_corners[3].y);
            Gizmos.DrawLine(vector, vector2);
            Gizmos.DrawLine(vector2, vector3);
            Gizmos.DrawLine(vector3, vector4);
            Gizmos.DrawLine(vector4, vector);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            this.DebugDrawRect();
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            this.DebugDrawRect();
        }

        public void ResetScale()
        {
            this.m_lastScale = -1f;
        }

        public void Add(TileCollideItem tc)
        {
            if (!this.tileCollision.Contains(tc))
            {
                tc.SetTileCollideManager(this);
                this.tileCollision.Add(tc);
                this.tileCollision.Sort((TileCollideItem x, TileCollideItem y) => y.m_priority - x.m_priority);
                this.m_is_need_update_onece = true;
                this.m_lastScale = -1f;
            }
        }

        public void Remove(TileCollideItem tc)
        {
            this.tileCollision.Remove(tc);
        }

        public void RemoveAll()
        {
            this.tileCollision.Clear();
        }
    }
}
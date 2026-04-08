using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ROK
{
    public class TileCollideItem : MonoBehaviour
    {
        public Vector2 size = new Vector2(0.1f, 0.1f);

        public int m_priority;

        public bool m_shift_pos;

        private Vector3 m_init_local_pos;

        private TileCollideManager m_tile_collide_manager;

        public float m_shift_pos_start_scale = 1f;

        public float m_shift_pos_end_scale = 1f;

        public bool m_auto_registre;

        private float m_fade_time = 0.3f;

        private float m_fade_timer;

        private SpriteRenderer m_sprite_renderer;

        public bool m_is_active = true;

        private RotateRect m_rect = new RotateRect(Vector2.zero, Vector2.one);

        public void SetTileCollideManager(TileCollideManager mgr)
        {
            this.m_tile_collide_manager = mgr;
        }

        private void ResetInitLocalPos()
        {
            this.m_init_local_pos = base.transform.localPosition;
        }

        public static void ResetInitLocalPosS(TileCollideItem self)
        {
            self.ResetInitLocalPos();
        }

        private void Awake()
        {
            this.ResetInitLocalPos();
            this.m_sprite_renderer = base.transform.Find("sprite").GetComponent<SpriteRenderer>();
            if (base.transform.parent)
            {
                TileCollideManager component = base.transform.parent.GetComponent<TileCollideManager>();
                if (this.m_auto_registre && component != null)
                {
                    component.Add(this);
                }
            }
        }

        private void Start()
        {
            if (base.transform.parent)
            {
                LodBase component = base.transform.parent.GetComponent<LodBase>();
                if (component)
                {
                    component.UpdateLod();
                }
            }
        }

        public void SetAlpha(float alpha)
        {
            this.m_sprite_renderer.color = new Vector4(this.m_sprite_renderer.color.r, this.m_sprite_renderer.color.g, this.m_sprite_renderer.color.b, alpha);
        }

        public void Activate()
        {
            if (!this.m_is_active)
            {
                this.m_is_active = true;
                base.gameObject.SetActive(true);
            }
        }

        public void Deactivate()
        {
            if (this.m_is_active)
            {
                this.m_is_active = false;
                base.gameObject.SetActive(false);
            }
        }

        public void OnDespawn()
        {
            if (this.m_tile_collide_manager)
            {
                this.m_tile_collide_manager.Remove(this);
            }
        }

        private void OnDestroy()
        {
            this.OnDespawn();
        }

        public void ShiftPos(float scale)
        {
            if (scale <= this.m_shift_pos_start_scale)
            {
                base.transform.localPosition = this.m_init_local_pos;
            }
            else if (scale > this.m_shift_pos_start_scale && scale < this.m_shift_pos_end_scale)
            {
                base.transform.localPosition = Vector3.Lerp(this.m_init_local_pos, Vector3.zero, (scale - this.m_shift_pos_start_scale) / (this.m_shift_pos_end_scale - this.m_shift_pos_start_scale));
            }
            else
            {
                base.transform.localPosition = Vector3.zero;
            }
        }

        public RotateRect GetGlobalRect()
        {
            Vector2 vector = this.CalSizeScaled();
            this.m_rect.UpdateRect(new Vector2(base.transform.position.x, base.transform.position.z), vector);
            return this.m_rect;
        }

        public Vector2 CalSizeScaled()
        {
            return new Vector2(this.size.x * base.transform.localScale.x, this.size.y * base.transform.localScale.z);
        }

        private void DrawRect2D()
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
            this.DrawRect2D();
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            this.DrawRect2D();
        }

        public void SetScale(float scale)
        {
            base.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
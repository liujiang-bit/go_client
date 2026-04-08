using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Client
{
    public class GridCollideItem : MonoBehaviour
    {
        [Header("设置物件底座大小,如果不设置无法lod缩放的时候动态隐藏")]
        public Vector2 size = new Vector2(0.1f, 0.1f);

        [Header("中心不隐藏的建筑必须大于100，不然缩放的时候会消失")]
        public int m_priority;
        
        [Header("主城不在中心，缩放的时候矫正到中心点")]
        public bool m_shift_pos;

        private Vector3 m_init_local_pos;

        private GridCollideMgr m_tile_collide_manager;

        public float m_shift_pos_start_scale = 1f;

        public float m_shift_pos_end_scale = 1f;

        /// <summary>
        /// 启动时候自动注册到到
        /// </summary>
        [Header("野外资源田需要勾起来   城市内建筑程序动态设置")]
        public bool m_auto_registre;

        private float m_fade_time = 0.3f;

        private float m_fade_timer;

        private SpriteRenderer m_sprite_renderer;
        
        public bool m_is_active = true;

        private RotateSquar m_rect = new RotateSquar(Vector2.zero, Vector2.one);

        public void SetTileCollideManager(GridCollideMgr mgr)
        {
            this.m_tile_collide_manager = mgr;
        }

        private void ResetInitLocalPos()
        {
            this.m_init_local_pos = base.transform.localPosition;
        }

        public static void ResetInitLocalPosS(GridCollideItem self)
        {
            self.ResetInitLocalPos();
        }

        private void Awake()
        {
            this.ResetInitLocalPos();
            var obj = base.transform.Find("sprite");
            if (obj!=null)
            {
                this.m_sprite_renderer = obj.GetComponent<SpriteRenderer>();
            }
            else
            {
                Debug.Log("not find sprite "+base.transform.gameObject.name);
            }
           
            
        }

        private void Start()
        {
            if (base.transform.parent)
            {
                GridCollideMgr component = base.transform.parent.GetComponent<GridCollideMgr>();
                if (this.m_auto_registre && component != null)
                {
                    component.Add(this);
                }
            }
            if (base.transform.parent)
            {
                LevelDetailBase component = base.transform.parent.GetComponent<LevelDetailBase>();
                if (component)
                {
                    component.UpdateLod();
                }
            }
        }

        public void SetAlpha(float alpha)
        {
            if (m_sprite_renderer!=null)
            {
                this.m_sprite_renderer.color = new Vector4(this.m_sprite_renderer.color.r, this.m_sprite_renderer.color.g, this.m_sprite_renderer.color.b, alpha);
            }
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

        public RotateSquar GetGlobalRect()
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
            RotateSquar globalRect = this.GetGlobalRect();
            Vector3 vector = new Vector3(globalRect.m_world_corners[0].x, 0f, globalRect.m_world_corners[0].y);
            Vector3 vector2 = new Vector3(globalRect.m_world_corners[1].x, 0f, globalRect.m_world_corners[1].y);
            Vector3 vector3 = new Vector3(globalRect.m_world_corners[2].x, 0f, globalRect.m_world_corners[2].y);
            Vector3 vector4 = new Vector3(globalRect.m_world_corners[3].x, 0f, globalRect.m_world_corners[3].y);
            vector -= transform.position;
            vector2 -= transform.position;
            vector3 -= transform.position;
            vector4 -= transform.position;
            vector = vector * 0.8f;
            vector2 = vector2 * 0.8f;
            vector3 = vector3 * 0.8f;
            vector4 = vector4 * 0.8f;
            vector = Quaternion.AngleAxis(45, Vector3.left) * vector;
            vector2 = Quaternion.AngleAxis(45, Vector3.left) * vector2;
            vector3 = Quaternion.AngleAxis(45, Vector3.left) * vector3;
            vector4 = Quaternion.AngleAxis(45, Vector3.left) * vector4;

            vector += transform.position;
            vector2 += transform.position;
            vector3 += transform.position;
            vector4 += transform.position;
            Gizmos.DrawLine(vector, vector2);
            Gizmos.DrawLine(vector2, vector3);
            Gizmos.DrawLine(vector3, vector4);
            Gizmos.DrawLine(vector4, vector);
            
            Gizmos.DrawLine(vector, vector3);
            Gizmos.DrawLine(vector2, vector4);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            this.DrawRect2D();
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            this.DrawRect2D();
        }

        public void SetScale(float scale)
        {
            base.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
using System;
using UnityEngine;

namespace ROK
{
    public class OcclusionCullMgr
    {
        private static GameObject m_cull_obj;

        private static float m_cull_obj_height_on_ui = 0.933f;

        private static float m_default_camera_fov = 30f;

        private static float m_previous_width;

        private static float m_previous_height;

        public static void InitCullObj(GameObject obj)
        {
            OcclusionCullMgr.m_cull_obj = obj;
            obj.SetActive(false);
        }

        public static void EnableCull(float w, float h)
        {
            if (Camera.main != null)
            {
                OcclusionCullMgr.m_cull_obj.SetActive(true);
                float num = Camera.main.fieldOfView / OcclusionCullMgr.m_default_camera_fov / (float)Screen.height / OcclusionCullMgr.m_cull_obj_height_on_ui;
                if (w > OcclusionCullMgr.m_previous_width)
                {
                    OcclusionCullMgr.m_previous_width = w;
                }
                if (h > OcclusionCullMgr.m_previous_height)
                {
                    OcclusionCullMgr.m_previous_height = h;
                }
                OcclusionCullMgr.m_previous_height *= num;
                OcclusionCullMgr.m_previous_width *= num;
                OcclusionCullMgr.m_cull_obj.transform.localScale = new Vector3(OcclusionCullMgr.m_previous_width, OcclusionCullMgr.m_previous_height, 1f);
            }
        }

        public static void DisableCull()
        {
            OcclusionCullMgr.m_previous_width = 0f;
            OcclusionCullMgr.m_previous_height = 0f;
            OcclusionCullMgr.m_cull_obj.SetActive(false);
        }
    }
}
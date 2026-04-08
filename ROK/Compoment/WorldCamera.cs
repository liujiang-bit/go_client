using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ROK
{
    [RequireComponent(typeof(Camera))]
    public class WorldCamera : MonoBehaviour
    {
        private static Camera _camera;

        public AnimationCurve m_fog_curve;

        private float m_fog_start_distance;

        private float m_fog_end_distance;

        public static WorldCamera m_instance;

        public float _dist;

        public Vector3 _look_dir = default(Vector3);

        private WaitForSeconds m_wait_for_one_second = new WaitForSeconds(1f);

        public static Camera camera
        {
            get
            {
                return WorldCamera._camera;
            }
        }

        private void Awake()
        {
            WorldCamera.m_instance = this;
            WorldCamera._camera = base.GetComponent<Camera>();
            this.m_fog_start_distance = RenderSettings.fogStartDistance;
            this.m_fog_end_distance = RenderSettings.fogEndDistance;
            //LightingManager.GetInstance().lightingHandler = Camera.main.GetComponent<LightingHandler>();
        }

        private void OnDestroy()
        {
            WorldCamera._camera = null;
        }

        public static void EnableBlur(bool enable)
        {
        }

        private void Update()
        {
            try
            {
                if (MapDataManager.GetInstance().loadDataDone && !WorldCameraImpl.GetInstance().isMovingToPos)
                {
                    MapManager.GetInstance().UpdateViewCenter(this.GetViewCenter());
                }
                this._look_dir = base.gameObject.transform.forward;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }

        public Vector2 GetViewCenter()
        {
            return new Vector2(base.transform.position.x, base.transform.position.z + base.transform.position.y * 1.1547f);
        }

        public void UpdateFog()
        {
            float num = this.m_fog_curve.Evaluate(Common.GetLodDistance());
            RenderSettings.fogStartDistance = this.m_fog_start_distance * num;
            RenderSettings.fogEndDistance = this.m_fog_end_distance * num;
        }

        public void IncreaseFPS()
        {
            if (GraphicSettingMgr.GetGraphicLevel() != GraphicSettingMgr.GraphicLevel.LOW)
            {
                base.StopCoroutine("DoDecreaseFPS");
                Application.targetFrameRate = 40;
            }
        }

        public void DecreaseFPS()
        {
            if (GraphicSettingMgr.GetGraphicLevel() != GraphicSettingMgr.GraphicLevel.LOW)
            {
                base.StartCoroutine("DoDecreaseFPS");
            }
        }

        private IEnumerator DoDecreaseFPS()
        {
            yield return new WaitForSeconds(1f);
            Application.targetFrameRate = 30;
        }

        public void UpdateShowCameraViewRect()
        {
            if (Application.isPlaying)
            {
                WorldCameraImpl.GetInstance().ShowCameraViewRect();
            }
        }
    }
}

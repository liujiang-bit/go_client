using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class LevelDetailCamera : LevelDetailBase
    {
        public AnimationCurve m_map_piece_width_curve;

        public AnimationCurve m_map_piece_plane_width_curve;

        public float m_current_lod_distance;

        public GameObject m_dark_corner_obj;

        //public CloudPlane m_cloud_plane;

        public GameObject m_blur_plane;

        public static LevelDetailCamera instance;

        public AnimationCurve m_camera_nearClipPlane_curve;

        public AnimationCurve m_camera_farClipPlane_curve;

        private Camera m_camera;

        private WorldCamera m_worldCamera;

        private float m_curShadowdistance;

        private Action<int, int> m_lodChange;

        public Camera m_mainCamera
        {
            get
            {
                if (this.m_camera == null)
                {
                    this.m_camera = base.GetComponent<Camera>();
                }
                return this.m_camera;
            }
        }
        public WorldCamera worldCamera
        {
            get
            {
                if (this.m_worldCamera == null)
                {
                    this.m_worldCamera = base.GetComponent<WorldCamera>();
                }
                return this.m_worldCamera;
            }
        }

        public void AddLodChange(Action<int, int> lodChange)
        {
            m_lodChange += lodChange;
        }

        public void RemoveLodChange(Action<int,int> lodChange)
        {
            m_lodChange -= lodChange;
        }

        private void Awake()
        {
            LevelDetailCamera.instance = this;
            //OcclusionCullMgr.InitCullObj(base.transform.Find("cull_plane").gameObject);
        }

        public static void ReadLodArrayFromGameDataS(LevelDetailCamera self)
        {
            self.ReadLodArrayFromGameData();
        }

        private void ReadLodArrayFromGameData()
        {
            //> todo
            //LuaTable gameDataTable = UnityGameDatas.GetInstance().GetGameDataTable("CameraParams_CameraLod");
            //int num = gameDataTable.length();
            //this.m_lod_array = new float[num];
            //for (int i = 1; i < num + 1; i++)
            //{
            //	LuaTable luaTable = (LuaTable)gameDataTable[i];
            //	this.m_lod_array[i - 1] = (float)((double)luaTable["dxf"]);
            //}
        }

        public override void UpdateLod()
        {
            float lodDistance = ClientUtils.lodManager.GetLodDistance();
            if (base.IsLodChanged())
            {
                int currentLodLevel = this.GetCurrentLodLevel();
                int previousLodLevel = base.GetPreviousLodLevel();
                if (previousLodLevel == 1 && currentLodLevel == 2)
                {
                    this.UpdateMapPieceWidth();
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    //dictionary["dxf"] = this.m_lod_array[2] + 1f;
                    //dictionary["cur_lod"] = currentLodLevel;
                    //dictionary["pre_lod"] = previousLodLevel;
                    //> todo
                    //Common.DispatchLuaEvent("EVENT_CAMERA_AUTO_ZOOM_TO_LOD", dictionary);
                    CoreUtils.logService.Debug($"LodChange:dfx:{ClientUtils.lodManager.GetLodDistance()} cur_lod:{currentLodLevel}", Color.green);
                }
                else if (previousLodLevel == 3 && currentLodLevel == 2)
                {
                    this.UpdateMapPieceWidth();
                    Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                    //dictionary2["dxf"] = this.m_lod_array[1] - 1f;
                    //dictionary2["cur_lod"] = currentLodLevel;
                    //dictionary2["pre_lod"] = previousLodLevel;
                    // //> todo
                    //Common.DispatchLuaEvent("EVENT_CAMERA_AUTO_ZOOM_TO_LOD", dictionary2);
                    CoreUtils.logService.Debug($"LodChange:dfx:{ClientUtils.lodManager.GetLodDistance()} cur_lod:{currentLodLevel}", Color.green);
                }
                else if (previousLodLevel == 1 && currentLodLevel == 0)
                {
                    this.UpdateMapPieceWidth();
                    if (this.m_blur_plane)
                    {
                        this.m_blur_plane.SetActive(false);
                    }
                    CoreUtils.logService.Debug($"LodChange:dfx:{ClientUtils.lodManager.GetLodDistance()} cur_lod:{currentLodLevel}", Color.green);
                }
                else
                {
                    this.UpdateMapPieceWidth();
                    if (this.m_blur_plane)
                    {
                        this.m_blur_plane.SetActive(true);
                    }
                    CoreUtils.logService.Debug($"LodChange:dfx:{ClientUtils.lodManager.GetLodDistance()} cur_lod:{currentLodLevel}", Color.green);
                }

                
                m_lodChange?.Invoke(previousLodLevel,currentLodLevel);
                
            }
            float previousLodDistance = ClientUtils.lodManager.GetPreviousLodDistance();
            if (previousLodDistance != lodDistance)
            {
                this.UpdateCameraClipPlane();
            }
            base.UpdateLod();
        }

        public void UpdateCameraClipPlane()
        {
            float lodDistance = ClientUtils.lodManager.GetLodDistance();
            int num = Mathf.RoundToInt(this.m_camera_nearClipPlane_curve.Evaluate(lodDistance));
            int num2 = Mathf.RoundToInt(this.m_camera_farClipPlane_curve.Evaluate(lodDistance));
            if (this.m_mainCamera != null)
            {
                //this.m_mainCamera.nearClipPlane = (float)num;
                //this.m_mainCamera.farClipPlane = (float)num2;
            }
        }

        public new int GetCurrentLodLevel()
        {
            return base.GetCurrentLodLevel();
        }

        public void UpdateMapPieceWidth()
        {
            int num = Mathf.RoundToInt(this.m_map_piece_width_curve.Evaluate(ClientUtils.lodManager.GetLodDistance()));
            int num2 = Mathf.RoundToInt(this.m_map_piece_plane_width_curve.Evaluate(ClientUtils.lodManager.GetLodDistance()));
            ClientUtils.mapManager.UpdatePieceWidth((float)num, (float)(num2));
        }

        public float GetMapPieceWidth()
        {
            return this.m_map_piece_width_curve.Evaluate(ClientUtils.lodManager.GetLodDistance());
        }

        public int GetLodLevelByDxf(float dxf)
        {
            for (int i = 0; i < this.m_lod_array.Length; i++)
            {
                if (dxf <= this.m_lod_array[i])
                {
                    return i;
                }
            }
            return this.m_lod_array.Length - 1;
        }

        private void UpdateShadowDistance()
        {
            float lodDistance = ClientUtils.lodManager.GetLodDistance();
            float num = 0.04f * lodDistance + 10f;
            num = Mathf.Clamp(num, 50f, 130f);
            if (this.m_curShadowdistance != num)
            {
                QualitySettings.shadowDistance = num;
                this.m_curShadowdistance = num;
            }
        }
    }
}
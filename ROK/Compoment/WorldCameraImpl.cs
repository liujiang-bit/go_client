using Skyunion;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ROK
{
    public class WorldCameraImpl
    {
        private class cameraInfoItem
        {
            public string Id;

            public float dist;

            public float fov;

            public float dxf;

            public Vector3 forward;
        }

        private static readonly WorldCameraImpl m_instance = new WorldCameraImpl();

        public double boundOffX = 1.6;

        public double boundOffY = 1.0;

        public double worldMinX;

        public double worldMaxX;

        public double worldMinY;

        public double worldMaxY;

        public double worldCenterX;

        public double worldCenterY;

        public float customMaxDxf = -1f;

        public float customMinDxf = -1f;

        public bool enableReboundXY;

        public float reboundBaseOffX = 0.00444444455f;

        public float reboundBaseOffY = 0.002962963f;

        public float reboundTime = 150f;

        public float slowDragRate = 0.5f;

        public float slowDragPowValue = 2f;

        private float m_moveReboundSpeedX;

        private float m_moveReboundSpeedY;

        public bool stopUITouchWhenAutoMoving = true;

        public static float INVALID_FLOAT_VALUE = -999f;

        private static Vector2 INVALID_VECTOR2 = new Vector2(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);

        private static Vector3 INVALID_VECTOR3 = new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);

        private WorldCamera cswcamera;

        private bool isTouching;

        private bool isDragging;

        private int lastTouchX;

        private int lastTouchY;

        private int touchStartX;

        private int touchStartY;

        private Vector3 touchStartTerrainPos;

        private Vector2 touchStartViewCenter;

        private int lastTouchTime;

        private Vector2 viewCenter = new Vector2(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);

        private Vector3 dragDir = Vector3.one;

        private Vector3 reboundDir = Vector3.one;

        private bool reboundAcc;

        private Vector3 inertialVec = Vector3.zero;

        private float dragSpeed;

        private ArrayList dragHistory = new ArrayList();

        private bool canDragOut;

        private double reboundSpeed;

        private bool isZooming;

        private double zoomCenterX;

        private double zoomCenterY;

        private Vector3 reboundInertial = Vector3.zero;

        private Vector3 dragInertial = Vector3.zero;

        private Vector3 releaseSpeed = Vector3.zero;

        private float ZOOM_REBOUND_SPEED;

        private int releaseTimeStamp;

        private bool canClick = true;

        private bool canDrag = true;

        private bool canZoom = true;

        private bool _isMovingToPos = true;

        private int viewTerrainStartTime;

        private int viewTerrainEndTime;

        private Vector2 viewTerrainStartPos = new Vector2(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);

        private Vector2 viewTerrainEndPos = new Vector2(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);

        private bool _isZoomingToDxf;

        private int zoomStartTime;

        private int zoomEndTime;

        private double zoomStartDxf;

        private double zoomEndDxf;

        private Vector2 autoZoomScreenPos = WorldCameraImpl.INVALID_VECTOR2;

        private string followObjId = string.Empty;

        private int moveFlag;

        private int moveStopTimeStamp;

        private int lastUpdateTimeStamp;

        private float cameraView_dist;

        private static ArrayList cameraInfoList = new ArrayList();

        private GameObject eventSystemObj;

        private int m_dt;

        private WorldCameraImpl.cameraInfoItem cameraInfo_limit_min;

        private WorldCameraImpl.cameraInfoItem cameraInfo_min;

        private WorldCameraImpl.cameraInfoItem cameraInfo_city;

        private WorldCameraImpl.cameraInfoItem cameraInfo_init;

        private WorldCameraImpl.cameraInfoItem cameraInfo_max;

        private WorldCameraImpl.cameraInfoItem cameraInfo_limit_max;

        private Camera camera;

        private Vector3 cameraPos = Vector3.zero;

        private float m_additionHeightForMinDxf;

        private float m_addDxfforAdditionHeight_delta;

        private float interpolateTime = WorldCameraImpl.INVALID_FLOAT_VALUE;

        private float WorldMgr_worldMinX;

        private float WorldMgr_worldMaxX;

        private float WorldMgr_worldMinY;

        private float WorldMgr_worldMaxY;

        private Plane WorldMgr_terrainPlane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));

        private float[] ViewTerrainPos_curve;

        private float[] SetCameraDxf_curve;

        private Action ViewTerrainPos_callback;

        private const float m_auto_zoom_speed = 500f;

        public bool isMovingToPos
        {
            get
            {
                return this._isMovingToPos;
            }
            set
            {
                this._isMovingToPos = value;
                if (this.stopUITouchWhenAutoMoving)
                {
                    this.CheckStopTouchWhenAutoMoving();
                }
            }
        }

        private bool isZoomingToDxf
        {
            get
            {
                return this._isZoomingToDxf;
            }
            set
            {
                this._isZoomingToDxf = value;
                if (this.stopUITouchWhenAutoMoving)
                {
                    this.CheckStopTouchWhenAutoMoving();
                }
            }
        }

        public static WorldCameraImpl GetInstance()
        {
            return WorldCameraImpl.m_instance;
        }

        public float getCameraDxf(string Id)
        {
            for (int i = 0; i < WorldCameraImpl.cameraInfoList.Count; i++)
            {
                WorldCameraImpl.cameraInfoItem cameraInfoItem = (WorldCameraImpl.cameraInfoItem)WorldCameraImpl.cameraInfoList[i];
                if (Id.CompareTo(cameraInfoItem.Id) == 0)
                {
                    return cameraInfoItem.dxf;
                }
            }
            Debug.LogError("error id on getCameraDxf!!!!!!");
            return 0f;
        }

        private WorldCameraImpl.cameraInfoItem getCameraInfo(string Id)
        {
            for (int i = 0; i < WorldCameraImpl.cameraInfoList.Count; i++)
            {
                WorldCameraImpl.cameraInfoItem cameraInfoItem = (WorldCameraImpl.cameraInfoItem)WorldCameraImpl.cameraInfoList[i];
                if (Id.CompareTo(cameraInfoItem.Id) == 0)
                {
                    return cameraInfoItem;
                }
            }
            Debug.LogError("error id on getCameraDxf!!!!!!");
            return null;
        }

        public void setAdditionHeightForMinDxf(float value)
        {
            this.m_addDxfforAdditionHeight_delta = value - this.m_additionHeightForMinDxf;
            if (this.m_addDxfforAdditionHeight_delta < 0f)
            {
                this.m_addDxfforAdditionHeight_delta = 0f;
            }
            this.m_additionHeightForMinDxf = value;
        }

        private float getcamraInfo_min_dxf()
        {
            return this.cameraInfo_min.dxf + this.m_additionHeightForMinDxf;
        }

        private float getcamraInfo_limit_min_dxf()
        {
            return this.cameraInfo_limit_min.dxf + this.m_additionHeightForMinDxf * (this.cameraInfo_limit_min.dxf / this.cameraInfo_min.dxf);
        }

        public float getCurrentCameraDxf()
        {
            if (this.camera == null)
            {
                return Camera.main.fieldOfView * Camera.main.transform.position.y;
            }
            return this.cameraView_dist * this.camera.fieldOfView;
        }

        public float getCurrentCameraFov()
        {
            if (this.camera == null)
            {
                return 0f;
            }
            return this.camera.fieldOfView;
        }

        public float getCurrentCameraDist()
        {
            if (this.camera == null)
            {
                return 0f;
            }
            return this.cameraView_dist;
        }

        public bool GetDistFovByDxf(float dxf, out float dist, out float fov)
        {
            dist = 0f;
            fov = 0f;
            WorldCameraImpl.cameraInfoItem cameraInfoItem = this.cameraInfo_limit_min;
            WorldCameraImpl.cameraInfoItem cameraInfoItem2 = this.cameraInfo_limit_max;
            float dxf2 = cameraInfoItem2.dxf;
            if (this.customMaxDxf > 0f && dxf2 > this.customMaxDxf)
            {
                dxf2 = this.customMaxDxf;
            }
            float dxf3 = cameraInfoItem.dxf;
            float num = this.m_additionHeightForMinDxf;
            if (this.customMinDxf > 0f && dxf3 < this.customMinDxf)
            {
                dxf3 = this.customMinDxf;
                num = 0f;
            }
            dxf = Mathf.Max(dxf3 + num, Mathf.Min(dxf, dxf2));
            if (dxf <= cameraInfoItem.dxf)
            {
                dist = cameraInfoItem.dist;
                fov = cameraInfoItem.fov;
                return true;
            }
            if (dxf >= cameraInfoItem2.dxf)
            {
                dist = cameraInfoItem2.dist;
                fov = cameraInfoItem2.fov;
                return true;
            }
            for (int i = 0; i < WorldCameraImpl.cameraInfoList.Count - 1; i++)
            {
                WorldCameraImpl.cameraInfoItem cameraInfoItem3 = (WorldCameraImpl.cameraInfoItem)WorldCameraImpl.cameraInfoList[i];
                WorldCameraImpl.cameraInfoItem cameraInfoItem4 = (WorldCameraImpl.cameraInfoItem)WorldCameraImpl.cameraInfoList[i + 1];
                if (dxf == cameraInfoItem4.dxf)
                {
                    dist = cameraInfoItem4.dist;
                    fov = cameraInfoItem4.fov;
                    return true;
                }
                if (dxf > cameraInfoItem3.dxf && dxf < cameraInfoItem4.dxf)
                {
                    double num2 = 0.0;
                    double num3 = 0.0;
                    if (this.GetDistFovWithTwoCameraInfo((double)cameraInfoItem3.dist, (double)cameraInfoItem3.fov, (double)cameraInfoItem4.dist, (double)cameraInfoItem4.fov, (double)dxf, out num2, out num3))
                    {
                        dist = (float)num2;
                        fov = (float)num3;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool GetDistFovWithTwoCameraInfo(double dist_min, double fov_min, double dist_max, double fov_max, double dxf, out double dist, out double fov)
        {
            dist = 0.0;
            fov = 0.0;
            if (dist_min * fov_min > dist_max * fov_max)
            {
                return false;
            }
            if (dxf < dist_min * fov_min || dxf > dist_max * fov_max)
            {
                return false;
            }
            if (dxf == dist_min * fov_min)
            {
                dist = dist_min;
                fov = fov_min;
                return true;
            }
            if (dxf == dist_max * fov_max)
            {
                dist = dist_max;
                fov = fov_max;
                return true;
            }
            if (dist_max == dist_min)
            {
                dist = dist_min;
                fov = dxf / dist_min;
                return true;
            }
            if (fov_max == fov_min)
            {
                fov = fov_min;
                dist = dxf / fov_min;
                return true;
            }
            double num = 0.0;
            double num2 = 1.0;
            double num3 = -1.0;
            double num4 = (dist_min + num * (dist_max - dist_min)) * (fov_min + num * (fov_max - fov_min));
            double num5 = (dist_min + num2 * (dist_max - dist_min)) * (fov_min + num2 * (fov_max - fov_min));
            for (int i = 0; i < 24; i++)
            {
                if (dxf == num4)
                {
                    num3 = num;
                    break;
                }
                if (dxf == num5)
                {
                    num3 = num2;
                    break;
                }
                double num6 = (num2 + num) / 2.0;
                double num7 = (dist_min + num6 * (dist_max - dist_min)) * (fov_min + num6 * (fov_max - fov_min));
                if (dxf == num7)
                {
                    num3 = num6;
                    break;
                }
                if (dxf > num7)
                {
                    if (num == num6)
                    {
                        num3 = num6;
                        break;
                    }
                    num = num6;
                    num4 = num7;
                }
                else if (dxf < num7)
                {
                    if (num2 == num6)
                    {
                        num3 = num6;
                        break;
                    }
                    num2 = num6;
                    num5 = num7;
                }
            }
            if (num3 == -1.0)
            {
                num3 = num;
            }
            dist = dist_min + num3 * (dist_max - dist_min);
            fov = fov_min + num3 * (fov_max - fov_min);
            return true;
        }

        public bool IsAutoMoving()
        {
            return this.isMovingToPos || this.isZoomingToDxf;
        }

        public static void SetInitValueFromLua()
        {
            //WorldCameraImpl.cameraInfoList.Clear();
            //WorldCameraImpl.WorldMgrLtb = WorldMgrluatab;
            //int num = cameraInfo.length();
            //float num2 = 0f;
            //for (int i = 1; i <= num; i++)
            //{
            //	LuaTable luaTable = (LuaTable)cameraInfo[i];
            //	WorldCameraImpl.cameraInfoItem cameraInfoItem = new WorldCameraImpl.cameraInfoItem();
            //	cameraInfoItem.Id = (string)luaTable["Id"];
            //	cameraInfoItem.dist = (float)((double)luaTable["dist"]);
            //	cameraInfoItem.fov = (float)((double)luaTable["fov"]);
            //	cameraInfoItem.dxf = cameraInfoItem.dist * cameraInfoItem.fov;
            //	if (num2 >= cameraInfoItem.dist * cameraInfoItem.fov)
            //	{
            //		Common.LogError("CameraParams_CamaraParams data (dist * fov) error ");
            //	}
            //	LuaTable luaTable2 = (LuaTable)luaTable["forward"];
            //	cameraInfoItem.forward.x = (float)((double)luaTable2[1]);
            //	cameraInfoItem.forward.y = (float)((double)luaTable2[2]);
            //	cameraInfoItem.forward.z = (float)((double)luaTable2[3]);
            //	WorldCameraImpl.cameraInfoList.Add(cameraInfoItem);
            //}
        }

        //public static LuaTable GetLuaTable(string tablefunctions)
        //{
        //	LuaState luaState = GameRoot.GetLuaSvrStatic().luaState;
        //	string[] array = tablefunctions.Split(new char[]
        //	{
        //		'.'
        //	});
        //	LuaTable luaTable = null;
        //	for (int i = 0; i < array.Length; i++)
        //	{
        //		if (i == 0)
        //		{
        //			luaTable = luaState.getTable(array[i]);
        //		}
        //		else if (i < array.Length - 1)
        //		{
        //			luaTable = (LuaTable)luaTable[array[i]];
        //		}
        //		else if (i == array.Length - 1)
        //		{
        //			return (LuaTable)luaTable[array[i]];
        //		}
        //	}
        //	return null;
        //}

        //[DoNotToLua]
        //public static LuaFunction GetLuaFunction(string tablefunctions)
        //{
        //	LuaState luaState = GameRoot.GetLuaSvrStatic().luaState;
        //	string[] array = tablefunctions.Split(new char[]
        //	{
        //		'.'
        //	});
        //	LuaTable luaTable = null;
        //	for (int i = 0; i < array.Length; i++)
        //	{
        //		if (i == 0)
        //		{
        //			luaTable = luaState.getTable(array[i]);
        //		}
        //		else if (i < array.Length - 1)
        //		{
        //			luaTable = (LuaTable)luaTable[array[i]];
        //		}
        //		else if (i == array.Length - 1)
        //		{
        //			return (LuaFunction)luaTable[array[i]];
        //		}
        //	}
        //	return null;
        //}

        public Camera GetCamera()
        {
            return this.camera;
        }

        public void Reset()
        {
            this.Init();
            this.CalcViewCenter(null);
        }

        public void Init()
        {
            if (this.eventSystemObj == null)
            {
                this.eventSystemObj = GameObject.Find("EventSystem");
            }
            this.isTouching = false;
            this.isDragging = false;
            this.lastTouchX = 0;
            this.lastTouchY = 0;
            this.touchStartX = 0;
            this.touchStartY = 0;
            this.touchStartTerrainPos = new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            this.touchStartViewCenter = new Vector2(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            this.lastTouchTime = 0;
            this.viewCenter = new Vector2(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            this.dragDir = Vector3.one;
            this.reboundDir = Vector3.one;
            this.reboundAcc = false;
            this.dragSpeed = 0f;
            this.dragHistory = new ArrayList();
            this.canDragOut = false;
            this.reboundSpeed = 0.0;
            this.zoomCenterX = 0.0;
            this.zoomCenterY = 0.0;
            this.reboundInertial = Vector3.zero;
            this.dragInertial = Vector3.zero;
            this.releaseSpeed = Vector3.zero;
            this.releaseTimeStamp = 0;
            this.canClick = true;
            this.canDrag = true;
            this.canZoom = true;
            this._isMovingToPos = false;
            this.viewTerrainStartTime = 0;
            this.viewTerrainEndTime = 0;
            this.viewTerrainStartPos = new Vector2(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            this.viewTerrainEndPos = new Vector2(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            this._isZoomingToDxf = false;
            this.zoomStartTime = 0;
            this.zoomEndTime = 0;
            this.zoomStartDxf = (double)WorldCameraImpl.INVALID_FLOAT_VALUE;
            this.zoomEndDxf = (double)WorldCameraImpl.INVALID_FLOAT_VALUE;
            this.autoZoomScreenPos = WorldCameraImpl.INVALID_VECTOR2;
            this.followObjId = string.Empty;
            this.moveFlag = 0;
            this.moveStopTimeStamp = 0;
            this.lastUpdateTimeStamp = 0;
            this.camera = WorldCamera.camera;
            this.cswcamera = this.camera.GetComponent<WorldCamera>();
            this.cameraPos = this.camera.transform.position;
            this.InitViewCenter(this.camera);
            this.cameraInfo_limit_min = this.getCameraInfo("limit_min");
            this.cameraInfo_min = this.getCameraInfo("min");
            this.cameraInfo_city = this.getCameraInfo("city");
            this.cameraInfo_init = this.getCameraInfo("init");
            this.cameraInfo_max = this.getCameraInfo("max");
            this.cameraInfo_limit_max = this.getCameraInfo("limit_max");
            if (this.stopUITouchWhenAutoMoving)
            {
                this.CheckStopTouchWhenAutoMoving();
            }
        }

        private WorldCamera getWorldCamera()
        {
            if (this.cswcamera == null)
            {
                this.cswcamera = this.camera.GetComponent<WorldCamera>();
            }
            return this.cswcamera;
        }

        public void Update(int dt)
        {
            try
            {
                this.m_dt = dt;
                int num = (int)((double)(Time.realtimeSinceStartup * 1000f));
                while (this.dragHistory.Count > 0)
                {
                    if (((Vector2)this.dragHistory[0]).x >= (float)(num - 100))
                    {
                        break;
                    }
                    this.dragHistory.RemoveAt(0);
                }
                this.UpdateRebound(this.camera, dt);
                Vector3 moveDistVec = this.UpdateInertial(this.camera, dt);
                this.UpdateZooming(this.camera, moveDistVec, dt);
                this.UpdateMoveToPos();
                if (this.moveFlag > 0)
                {
                    this.moveFlag--;
                    if (this.moveFlag == 0)
                    {
                        //WorldCameraImpl.GetLuaFunction("WorldCamera.SendEventCameraStopMove").call();
                        this.moveStopTimeStamp = (int)Common.RealtimeSinceStartup();
                    }
                }
                else if (!this.isTouching && !this.isDragging && !this.isMovingToPos && (double)(this.lastUpdateTimeStamp - this.moveStopTimeStamp) < 0.5 && (double)(Common.RealtimeSinceStartup() - (float)this.moveStopTimeStamp) >= 0.5)
                {
                    float x = this.viewCenter.x;
                    float y = this.viewCenter.y;
                    //double num2 = (double)WorldCameraImpl.GetLuaFunction("WorldMgr.OnViewChanged").call(new object[]
                    //{
                    //	WorldCameraImpl.WorldMgrLtb,
                    //	x,
                    //	y,
                    //	this.getCurrentCameraDxf(),
                    //	x,
                    //	y
                    //});
                    //this.setAdditionHeightForMinDxf((float)num2);
                    //this.UpdateChangeMinCameraDxf(this.m_dt);
                }
                this.lastUpdateTimeStamp = (int)Common.RealtimeSinceStartup();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void ViewTerrainPos(float terrainX, float terrainY, float interpolateTime)
        {
            this.ViewTerrainPosCurve(terrainX, terrainY, interpolateTime, null);
        }

        public void ViewTerrainPosCurve(float terrainX, float terrainY, float interpolateTime, float[] curve)
        {
            if (Common.IsNaN(terrainX) || Common.IsNaN(terrainY))
            {
                terrainX = this.viewCenter.x;
                terrainY = this.viewCenter.y;
            }
            this.ViewTerrainPos_curve = curve;
            this.isTouching = false;
            this.isDragging = false;
            this.dragSpeed = 0f;
            this.reboundSpeed = 0.0;
            this.reboundAcc = false;
            this.inertialVec = Vector3.zero;
            this.dragInertial = Vector3.zero;
            this.releaseSpeed = Vector3.zero;
            this.releaseTimeStamp = 0;
            this.reboundInertial = Vector3.zero;
            this.dragHistory.Clear();
            this.isZooming = false;
            terrainX = Mathf.Max((float)this.worldMinX, Mathf.Min((float)this.worldMaxX, terrainX));
            terrainY = Mathf.Max((float)this.worldMinY, Mathf.Min((float)this.worldMaxY, terrainY));
            int num = 0;
            int num2 = 0;
            Common.GetScreenSize(out num, out num2);
            Vector3 vector = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
            float x = vector.x;
            float z = vector.z;
            Vector3 vector2 = new Vector3(terrainX - x, 0f, terrainY - z);
            Vector3 pos = new Vector3(terrainX, 0f, terrainY);
            if (interpolateTime != WorldCameraImpl.INVALID_FLOAT_VALUE && !this.isMovingToPos)
            {
                float magnitude = vector2.magnitude;
                float num3 = magnitude / interpolateTime;
                if ((double)num3 < 0.005)
                {
                    num3 = 0.005f;
                    interpolateTime = magnitude / num3;
                }
                this.isMovingToPos = true;
                this.viewTerrainStartTime = (int)((double)(Time.realtimeSinceStartup * 1000f));
                this.viewTerrainEndTime = this.viewTerrainStartTime + (int)interpolateTime;
                this.viewTerrainStartPos = new Vector2(x, z);
                this.viewTerrainEndPos = new Vector3(terrainX, terrainY);
            }
            else
            {
                this.isMovingToPos = false;
                this.SetCameraPosByViewPos(this.camera, pos, false);
                //if (this.ViewTerrainPos_callback != null)
                //{
                //	this.ViewTerrainPos_callback.call();
                //}
            }
        }

        private float getValueFromCurve(float[] curve, float input)
        {
            if (curve == null)
            {
                return input;
            }
            int num = curve.Length;
            if (num < 2)
            {
                return input;
            }
            if (input <= 0f)
            {
                return curve[0];
            }
            if (input >= 1f)
            {
                return curve[num - 1];
            }
            float num2 = input * (float)(num - 1);
            int num3 = Mathf.FloorToInt(num2);
            int num4 = num3 + 1;
            if (num4 >= num)
            {
                return input;
            }
            float num5 = num2 - (float)num3;
            float num6 = curve[num4] - curve[num3];
            float num7 = num5 * num6;
            return curve[num3] + num7;
        }

        public void SetCameraDxf(Camera camera, float dxf, float interpolateTime)
        {
            this.autoZoomScreenPos = WorldCameraImpl.INVALID_VECTOR2;
            this.SetCameraDxfCurve(camera, dxf, interpolateTime, null);
        }

        public void SetCameraDxfAtScreenPos(Camera camera, float dxf, float interpolateTime, Vector2 screenPos)
        {
            if (this.isZoomingToDxf)
            {
                return;
            }
            this.autoZoomScreenPos = screenPos;
            this.SetCameraDxfCurve(camera, dxf, interpolateTime, null);
        }

        public void SetCameraDxfCurveAtScreenPos(Camera camera, float dxf, float interpolateTime, Vector2 screenPos, float[] curve)
        {
            if (this.isZoomingToDxf)
            {
                return;
            }
            this.autoZoomScreenPos = screenPos;
            this.SetCameraDxfCurve(camera, dxf, interpolateTime, curve);
        }

        public void SetCameraDxfCurve(Camera camera, float dxf, float interpolateTime, float[] curve)
        {
            if (this.isZoomingToDxf)
            {
                return;
            }
            this.SetCameraDxf_curve = curve;
            this.isTouching = false;
            this.isDragging = false;
            this.dragSpeed = 0f;
            this.reboundSpeed = 0.0;
            this.reboundAcc = false;
            this.inertialVec = Vector3.zero;
            this.dragInertial = Vector3.zero;
            this.releaseSpeed = Vector3.zero;
            this.releaseTimeStamp = 0;
            this.reboundInertial = Vector3.zero;
            this.dragHistory.Clear();
            this.isZooming = false;
            float currentCameraDxf = this.getCurrentCameraDxf();
            if (interpolateTime != WorldCameraImpl.INVALID_FLOAT_VALUE && !this.isZoomingToDxf)
            {
                this.isZoomingToDxf = true;
                this.zoomStartTime = (int)((double)(Time.realtimeSinceStartup * 1000f));
                this.zoomEndTime = this.zoomStartTime + (int)interpolateTime;
                this.zoomStartDxf = (double)this.getCurrentCameraDxf();
                this.zoomEndDxf = (double)dxf;
            }
            else
            {
                this.isZoomingToDxf = false;
                float fieldOfView = 0f;
                float num = 0f;
                if (!this.GetDistFovByDxf(dxf, out num, out fieldOfView))
                {
                    return;
                }
                this.cameraView_dist = num;
                this.getWorldCamera()._dist = this.cameraView_dist;
                camera.fieldOfView = fieldOfView;
                if (this.autoZoomScreenPos == WorldCameraImpl.INVALID_VECTOR2)
                {
                    this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                }
                else
                {
                    Vector3 touchTerrainPos = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                    this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                    Vector3 touchTerrainPos2 = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                    if (touchTerrainPos2 != WorldCameraImpl.INVALID_VECTOR3)
                    {
                        Vector3 vector = touchTerrainPos2 - touchTerrainPos;
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x - vector.x, 0f, this.viewCenter.y - vector.z), false);
                    }
                    this.autoZoomScreenPos = WorldCameraImpl.INVALID_VECTOR2;
                }
                LodManager.GetInstance().UpdateLod();
                camera.GetComponent<WorldCamera>().UpdateFog();
                //if (this.SetCameraDxf_callback != null)
                //{
                //	this.SetCameraDxf_callback.call();
                //}
            }
        }

        public void UpdateRebound(Camera camera, int dt)
        {
        }

        public Vector3 UpdateInertial(Camera camera, int dt)
        {
            if (this.isTouching || this.isMovingToPos)
            {
                return new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            }
            float magnitude = this.reboundInertial.magnitude;
            Vector3 vector = new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            if (magnitude > 0f)
            {
                vector = this.reboundInertial * (float)dt;
            }
            double num = (double)(Time.realtimeSinceStartup * 1000f);
            Vector3 vector2 = new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            Vector3 a = new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            if (this.releaseTimeStamp != 0)
            {
                float num2 = (float)((double)((float)num - (float)this.releaseTimeStamp) * 0.001);
                if (num2 >= 0f && num2 < 2f)
                {
                    float magnitude2 = this.releaseSpeed.magnitude;
                    if ((double)magnitude2 > 0.0001)
                    {
                        a = this.releaseSpeed / magnitude2;
                        double num3 = (double)Mathf.Exp((float)((double)(-(double)num2) / 0.18));
                        double num4 = (double)magnitude2 * num3;
                        if (num4 > 0.0001)
                        {
                            vector2 = a * (float)(num4 * (double)dt);
                            if (this.enableReboundXY)
                            {
                                float x = this.viewCenter.x;
                                float y = this.viewCenter.y;
                                float currentCameraDxf = this.getCurrentCameraDxf();
                                float offset = this.reboundBaseOffX * currentCameraDxf;
                                float offset2 = this.reboundBaseOffY * currentCameraDxf;
                                float num5 = this.CalcSlowDrag(vector2.x, x, (float)this.worldMinX, (float)this.worldMaxX, offset);
                                float num6 = this.CalcSlowDrag(vector2.z, y, (float)this.worldMinY, (float)this.worldMaxY, offset2);
                                if (num5 != vector2.x || num6 != vector2.z)
                                {
                                    vector2.x = num5;
                                    vector2.z = num6;
                                    if (vector2.magnitude < 0.001f)
                                    {
                                        vector2 = Vector3.zero;
                                        this.releaseTimeStamp = 0;
                                    }
                                }
                            }
                            if (vector.x == WorldCameraImpl.INVALID_FLOAT_VALUE && vector.y == WorldCameraImpl.INVALID_FLOAT_VALUE && vector.z == WorldCameraImpl.INVALID_FLOAT_VALUE)
                            {
                                vector = vector2;
                            }
                            else
                            {
                                vector += vector2;
                            }
                        }
                        else
                        {
                            this.releaseTimeStamp = 0;
                        }
                    }
                    else
                    {
                        this.releaseTimeStamp = 0;
                    }
                }
                else
                {
                    this.releaseTimeStamp = 0;
                }
            }
            if (this.enableReboundXY && !this.isTouching && !this.isMovingToPos)
            {
                float num7 = this.viewCenter.x;
                float num8 = this.viewCenter.y;
                float moveReboundSpeedX = 0f;
                float moveReboundSpeedY = 0f;
                bool flag = false;
                bool flag2 = false;
                num7 = this.CalcRebound(num7, this.m_moveReboundSpeedX, (float)this.worldMinX, (float)this.worldMaxX, this.reboundTime, (float)dt, out moveReboundSpeedX, out flag);
                num8 = this.CalcRebound(num8, this.m_moveReboundSpeedY, (float)this.worldMinY, (float)this.worldMaxY, this.reboundTime, (float)dt, out moveReboundSpeedY, out flag2);
                this.m_moveReboundSpeedX = moveReboundSpeedX;
                this.m_moveReboundSpeedY = moveReboundSpeedY;
                if (flag || flag2)
                {
                    vector.x = num7 - this.viewCenter.x;
                    vector.z = num8 - this.viewCenter.y;
                }
            }
            if (vector.x != WorldCameraImpl.INVALID_FLOAT_VALUE || vector.y != WorldCameraImpl.INVALID_FLOAT_VALUE || vector.z != WorldCameraImpl.INVALID_FLOAT_VALUE)
            {
                this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x + vector.x, 0f, this.viewCenter.y + vector.z), true);
            }
            if (vector2.x != WorldCameraImpl.INVALID_FLOAT_VALUE || vector2.y != WorldCameraImpl.INVALID_FLOAT_VALUE || vector2.z != WorldCameraImpl.INVALID_FLOAT_VALUE)
            {
                float x2 = this.viewCenter.x;
                float y2 = this.viewCenter.y;
                if (this.enableReboundXY)
                {
                    float currentCameraDxf2 = this.getCurrentCameraDxf();
                    float num9 = this.reboundBaseOffX * currentCameraDxf2;
                    float num10 = this.reboundBaseOffY * currentCameraDxf2;
                    if (((double)x2 < this.worldMinX && a.x < 0f) || ((double)x2 > this.worldMaxX && a.x > 0f))
                    {
                        this.releaseSpeed.x = (float)((double)this.releaseSpeed.x * 0.001 * (double)dt);
                        this.releaseSpeed.z = (float)((double)this.releaseSpeed.z * 0.001 * (double)dt);
                    }
                    else if (((double)y2 < this.worldMinY && a.z < 0f) || ((double)y2 > this.worldMaxY && a.z > 0f))
                    {
                        this.releaseSpeed.z = (float)((double)this.releaseSpeed.z * 0.001 * (double)dt);
                        this.releaseSpeed.x = (float)((double)this.releaseSpeed.x * 0.001 * (double)dt);
                    }
                }
                else
                {
                    float num11 = 10f;
                    float num12 = 6f;
                    if (((double)x2 < this.worldMinX - (double)num11 && a.x < 0f) || ((double)x2 > this.worldMaxX + (double)num11 && a.x > 0f))
                    {
                        this.releaseSpeed.x = (float)((double)this.releaseSpeed.x * 0.001 * (double)dt);
                    }
                    if (((double)y2 < this.worldMinY - (double)num12 && a.z < 0f) || ((double)y2 > this.worldMaxY + (double)num12 && a.z > 0f))
                    {
                        this.releaseSpeed.z = (float)((double)this.releaseSpeed.z * 0.001 * (double)dt);
                    }
                }
            }
            return vector;
        }

        public float CalcInertial_spd(Vector3 inertialVec, int dt, float speed, float deAcc)
        {
            if ((double)speed < 0.0001)
            {
                return 0f;
            }
            float num = deAcc * (float)dt;
            speed *= 1f - num;
            if ((double)speed < 0.001)
            {
                return 0f;
            }
            return speed;
        }

        public Vector3 CalcInertial_dir(Vector3 inertialVec, int dt, float speed, float deAcc)
        {
            if ((double)speed < 0.0001)
            {
                return Vector3.zero;
            }
            Vector3 result = inertialVec / speed;
            float num = deAcc * (float)dt;
            speed *= 1f - num;
            if ((double)speed < 0.001)
            {
                return Vector3.zero;
            }
            return result;
        }

        public Vector2 GetViewCenter()
        {
            return this.viewCenter;
        }

        public bool CanClick()
        {
            return this.canClick;
        }

        public void SetCanClick(bool isEnable)
        {
            this.canClick = isEnable;
        }

        public bool CanDrag()
        {
            return this.canDrag;
        }

        public void SetCanDrag(bool isEnable)
        {
            this.canDrag = isEnable;
        }

        public bool CanZoom()
        {
            return this.canZoom;
        }

        public void SetCanZoom(bool isEnable)
        {
            this.canZoom = isEnable;
        }

        public bool IsMovingToPos()
        {
            return this.isMovingToPos;
        }

        public void SetFollowObj(string id)
        {
            this.followObjId = id;
        }

        public string GetFollowObj()
        {
            return this.followObjId;
        }

        private bool WorldMgrLtb_IsEntered()
        {
            //> todo
            //return (bool)((LuaFunction)WorldCameraImpl.WorldMgrLtb["IsEntered"]).call();
            return false;
        }

        private bool CommonMsgBox_HasMsg()
        {
            //> todo
            //return Common.CastLuaBool(WorldCameraImpl.GetLuaFunction("CommonMsgBox.HasMsg").call());
            return false;
        }

        public void OnTouchBegan(float x, float y)
        {
            if (!this.WorldMgrLtb_IsEntered() || CoreUtils.inputManager.IsTouchedUI())
            {
                return;
            }
            if (this.isMovingToPos)
            {
                return;
            }
            if (this.isZoomingToDxf)
            {
                return;
            }
            if (this.camera == null)
            {
                return;
            }
            if (this.isZooming)
            {
                this.isZooming = false;
            }
            if (!this.canClick && !this.canDrag)
            {
                return;
            }
            Vector3 touchTerrainPos = this.GetTouchTerrainPos(this.camera, x, y);
            if (touchTerrainPos.x == WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos.y == WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos.z == WorldCameraImpl.INVALID_FLOAT_VALUE)
            {
                return;
            }
            this.touchStartTerrainPos = touchTerrainPos;
            this.touchStartViewCenter = this.viewCenter;
            this.isTouching = true;
            this.dragSpeed = 0f;
            this.reboundSpeed = 0.0;
            this.reboundAcc = false;
            this.canDragOut = this.CheckCanDragout(this.viewCenter.x, this.viewCenter.y);
            this.inertialVec = Vector3.zero;
            this.dragInertial = Vector3.zero;
            this.releaseSpeed = Vector3.zero;
            this.releaseTimeStamp = 0;
            this.reboundInertial = Vector3.zero;
            this.dragHistory.Clear();
            this.touchStartX = (int)x;
            this.touchStartY = (int)y;
            this.lastTouchX = (int)x;
            this.lastTouchY = (int)y;
            this.lastTouchTime = (int)((double)(Time.realtimeSinceStartup * 1000f));
            //> todo
            //WorldCameraImpl.GetLuaFunction("WorldMgr.OnPressed").call(WorldCameraImpl.WorldMgrLtb);
        }

        public void OnTouchMoved(float x, float y)
        {
            if (!this.WorldMgrLtb_IsEntered() || !this.isTouching)
            {
                return;
            }
            if (!this.canDrag)
            {
                return;
            }
            if (this.isZooming)
            {
                return;
            }
            if (this.CommonMsgBox_HasMsg())
            {
                return;
            }
            this.isDragging = true;
            if (this.camera != null)
            {
                Vector3 touchTerrainPos = this.GetTouchTerrainPos(this.camera, x, y);
                if (touchTerrainPos.x != WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos.y != WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos.z != WorldCameraImpl.INVALID_FLOAT_VALUE)
                {
                    Vector3 a = -1f * (touchTerrainPos - this.touchStartTerrainPos);
                    a.y = 0f;
                    float magnitude = a.magnitude;
                    Vector3 vector = a / magnitude;
                    bool flag = true;
                    float x2 = this.viewCenter.x;
                    float y2 = this.viewCenter.y;
                    this.canDragOut = true;
                    if (!this.enableReboundXY)
                    {
                        this.canDragOut = this.CheckCanDragout(x2, y2);
                    }
                    if (!this.canDragOut)
                    {
                        bool flag2 = false;
                        float num = 5f;
                        float num2 = 3f;
                        if (((double)x2 < this.worldMinX - (double)num && a.x < 0f) || ((double)x2 > this.worldMaxX + (double)num && a.x > 0f))
                        {
                            a.x = 0f;
                            flag2 = true;
                        }
                        if (((double)y2 < this.worldMinY - (double)num2 && a.z < 0f) || ((double)y2 > this.worldMaxY + (double)num2 && a.z > 0f))
                        {
                            a.z = 0f;
                            flag2 = true;
                        }
                        if (flag2)
                        {
                            float magnitude2 = a.magnitude;
                            if ((double)magnitude2 > 0.1)
                            {
                                magnitude = a.magnitude;
                                vector = a / magnitude;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                    if (flag)
                    {
                        if (this.enableReboundXY)
                        {
                            float currentCameraDxf = this.getCurrentCameraDxf();
                            float offset = this.reboundBaseOffX * currentCameraDxf;
                            float offset2 = this.reboundBaseOffY * currentCameraDxf;
                            a.x = this.CalcSlowDrag(a.x, x2, (float)this.worldMinX, (float)this.worldMaxX, offset);
                            a.z = this.CalcSlowDrag(a.z, y2, (float)this.worldMinY, (float)this.worldMaxY, offset2);
                        }
                        this.SetCameraPosByViewPos(this.camera, new Vector3(this.viewCenter.x + a.x, 0f, this.viewCenter.y + a.z), true);
                        Vector2 vector2 = new Vector2((float)((double)(Time.realtimeSinceStartup * 1000f)), a.magnitude);
                        this.dragHistory.Add(vector2);
                        this.dragDir = vector;
                    }
                }
            }
            this.lastTouchX = (int)x;
            this.lastTouchY = (int)y;
            this.lastTouchTime = (int)((double)(Time.realtimeSinceStartup * 1000f));
        }

        public void OnTouchEnded(float x, float y)
        {
            EventSystem current = EventSystem.current;
            if (current != null)
            {
                GameObject currentSelectedGameObject = current.currentSelectedGameObject;
                if (currentSelectedGameObject != null)
                {
                    Button component = currentSelectedGameObject.GetComponent<Button>();
                    if (component != null && component.name == "BLOCK_UI_ONLY_WITH_ARROW")
                    {
                        Vector3 touchTerrainPos = new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
                        if (!this.isDragging && Mathf.Abs(x - (float)this.touchStartX) < 10f && Mathf.Abs(y - (float)this.touchStartY) < 10f && !CoreUtils.inputManager.IsTouchedUI())
                        {
                            touchTerrainPos = this.GetTouchTerrainPos(this.camera, x, y);
                        }
                        //> todo
                        //WorldCameraImpl.GetLuaFunction("WorldMgr.OnClicked").call(new object[]
                        //{
                        //	WorldCameraImpl.WorldMgrLtb,
                        //	touchTerrainPos.x,
                        //	touchTerrainPos.z,
                        //	x,
                        //	y
                        //});
                        return;
                    }
                }
            }
            if (!this.WorldMgrLtb_IsEntered() || !this.isTouching)
            {
                return;
            }
            if (this.isZooming)
            {
                return;
            }
            Vector3 touchTerrainPos2 = new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
            if (!this.isDragging && Mathf.Abs(x - (float)this.touchStartX) < 10f && Mathf.Abs(y - (float)this.touchStartY) < 10f && !CoreUtils.inputManager.IsTouchedUI())
            {
                touchTerrainPos2 = this.GetTouchTerrainPos(this.camera, x, y);
            }
            this.isTouching = false;
            this.isDragging = false;
            float num = (float)((double)(Time.realtimeSinceStartup * 1000f));
            while (this.dragHistory.Count > 0)
            {
                if (((Vector2)this.dragHistory[0]).x >= num - 100f)
                {
                    break;
                }
                this.dragHistory.RemoveAt(0);
            }
            if (this.dragHistory.Count > 0)
            {
                float num2 = 0f;
                float num3 = num - ((Vector2)this.dragHistory[0]).x;
                for (int i = 0; i < this.dragHistory.Count; i++)
                {
                    num2 += ((Vector2)this.dragHistory[i]).y;
                }
                float num4 = num2 / num3;
                if (num4 > 0f)
                {
                    this.dragInertial = this.dragDir * num4;
                    this.releaseSpeed = this.dragDir * num4;
                    this.releaseTimeStamp = (int)((double)(Time.realtimeSinceStartup * 1000f));
                }
            }
            this.dragHistory.Clear();
            if (this.CommonMsgBox_HasMsg())
            {
                return;
            }
            if (touchTerrainPos2.x != WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos2.y != WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos2.z != WorldCameraImpl.INVALID_FLOAT_VALUE && this.canClick)
            {
                bool flag = false;
                EventSystem current2 = EventSystem.current;
                if (current2 != null)
                {
                    GameObject currentSelectedGameObject2 = current2.currentSelectedGameObject;
                    if (currentSelectedGameObject2 != null)
                    {
                        Button component2 = currentSelectedGameObject2.GetComponent<Button>();
                        if (component2 != null && component2.interactable && component2.name != "ALLOW_MAP_CLICK")
                        {
                            flag = true;
                        }
                    }
                }
                if (!flag)
                {
                    //> todo
                    //WorldCameraImpl.GetLuaFunction("WorldMgr.OnClicked").call(new object[]
                    //{
                    //	WorldCameraImpl.WorldMgrLtb,
                    //	touchTerrainPos2.x,
                    //	touchTerrainPos2.z,
                    //	x,
                    //	y
                    //});
                }
            }
            if (this.getcamraInfo_min_dxf() > this.getCurrentCameraDxf())
            {
                this.zoomCenterX = (double)x;
                this.zoomCenterY = (double)y;
            }
        }

        public void OnTouchZoomedBegin(float centerX, float centerY)
        {
            this.zoomStartDxf = (double)this.getCurrentCameraDxf();
        }

        public void OnTouchZoomed(float centerX, float centerY, float scrollRate)
        {
            if (!this.WorldMgrLtb_IsEntered())
            {
                return;
            }
            if (this.isMovingToPos)
            {
                return;
            }
            if (this.isZoomingToDxf)
            {
                return;
            }
            if (!this.canZoom)
            {
                return;
            }
            if (!this.canClick)
            {
                return;
            }
            if (!this.camera)
            {
                return;
            }
            if (this.CommonMsgBox_HasMsg())
            {
                return;
            }
            Vector3 touchTerrainPos = this.GetTouchTerrainPos(this.camera, centerX, centerY);
            if (touchTerrainPos.x == WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos.y == WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos.z == WorldCameraImpl.INVALID_FLOAT_VALUE)
            {
                return;
            }
            if (!this.isZooming)
            {
                this.isZooming = true;
                this.isTouching = false;
                this.isDragging = false;
                this.reboundSpeed = 0.0;
                this.reboundAcc = false;
                this.inertialVec = Vector3.zero;
                this.dragInertial = Vector3.zero;
                this.releaseSpeed = Vector3.zero;
                this.releaseTimeStamp = 0;
                this.reboundInertial = Vector3.zero;
            }
            float currentCameraDxf = this.getCurrentCameraDxf();
            float num = this.getcamraInfo_min_dxf();
            float num2 = this.getcamraInfo_limit_min_dxf();
            if (this.customMinDxf > 0f && this.customMinDxf > num)
            {
                num = this.customMinDxf * (num / num2);
                if (this.customMaxDxf > 0f && num > this.customMaxDxf)
                {
                    num = this.customMaxDxf;
                }
                num2 = this.customMinDxf;
            }
            if (currentCameraDxf < num && scrollRate < 1f)
            {
                float num3 = 1f;
                if (this.zoomStartDxf > (double)currentCameraDxf)
                {
                    num3 = currentCameraDxf / (float)this.zoomStartDxf;
                }
                float num4 = Mathf.Max(0f, currentCameraDxf - num2) / (num - num2);
                num4 = Mathf.Pow(num4, 2f);
                scrollRate += (1f - num4) * (num3 - scrollRate);
            }
            float dxf = (float)this.zoomStartDxf * scrollRate;
            float fieldOfView = 0f;
            float num5 = 0f;
            if (!this.GetDistFovByDxf(dxf, out num5, out fieldOfView))
            {
                return;
            }
            this.cameraView_dist = num5;
            this.getWorldCamera()._dist = this.cameraView_dist;
            this.camera.fieldOfView = fieldOfView;
            float num6 = this.viewCenter.x;
            float num7 = this.viewCenter.y;
            num6 = Mathf.Clamp(num6, (float)this.worldMinX, (float)this.worldMaxX);
            num7 = Mathf.Clamp(num7, (float)this.worldMinY, (float)this.worldMaxY);
            this.SetCameraPosByViewPos(this.camera, new Vector3(num6, 0f, num7), false);
            Vector3 touchTerrainPos2 = this.GetTouchTerrainPos(this.camera, centerX, centerY);
            if (touchTerrainPos2.x == WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos2.y == WorldCameraImpl.INVALID_FLOAT_VALUE && touchTerrainPos2.z == WorldCameraImpl.INVALID_FLOAT_VALUE)
            {
                return;
            }
            Vector3 vector = touchTerrainPos2 - touchTerrainPos;
            float num8 = this.viewCenter.x - vector.x;
            float num9 = this.viewCenter.y - vector.z;
            num8 = Mathf.Clamp(num8, (float)this.worldMinX, (float)this.worldMaxX);
            num9 = Mathf.Clamp(num9, (float)this.worldMinY, (float)this.worldMaxY);
            this.SetCameraPosByViewPos(this.camera, new Vector3(num8, 0f, num9), false);
            LodManager.GetInstance().UpdateLod();
            this.camera.GetComponent<WorldCamera>().UpdateFog();
            this.zoomCenterX = (double)centerX;
            this.zoomCenterY = (double)centerY;
        }

        public void ViewTerrainPos(float terrainX, float terrainY, float interpolateTime, Action callback)
        {
            this.ViewTerrainPosCurve(terrainX, terrainY, interpolateTime, null, callback);
        }

        public void ViewTerrainPosCurve(float terrainX, float terrainY, float interpolateTime, float[] curve, Action callback)
        {
            if (Common.IsNaN(terrainX) || Common.IsNaN(terrainY))
            {
                terrainX = this.viewCenter.x;
                terrainY = this.viewCenter.y;
            }
            this.ViewTerrainPos_callback = callback;
            this.ViewTerrainPos_curve = curve;
            this.isTouching = false;
            this.isDragging = false;
            this.dragSpeed = 0f;
            this.reboundSpeed = 0.0;
            this.reboundAcc = false;
            this.inertialVec = Vector3.zero;
            this.dragInertial = Vector3.zero;
            this.releaseSpeed = Vector3.zero;
            this.releaseTimeStamp = 0;
            this.reboundInertial = Vector3.zero;
            this.dragHistory.Clear();
            this.isZooming = false;
            terrainX = Mathf.Max((float)this.worldMinX, Mathf.Min((float)this.worldMaxX, terrainX));
            terrainY = Mathf.Max((float)this.worldMinY, Mathf.Min((float)this.worldMaxY, terrainY));
            int num = 0;
            int num2 = 0;
            Common.GetScreenSize(out num, out num2);
            Vector3 vector = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
            float x = vector.x;
            float z = vector.z;
            Vector3 vector2 = new Vector3(terrainX - x, 0f, terrainY - z);
            Vector3 pos = new Vector3(terrainX, 0f, terrainY);
            if (interpolateTime != WorldCameraImpl.INVALID_FLOAT_VALUE && !this.isMovingToPos)
            {
                float magnitude = vector2.magnitude;
                float num3 = magnitude / interpolateTime;
                if ((double)num3 < 0.005)
                {
                    num3 = 0.005f;
                    interpolateTime = magnitude / num3;
                }
                this.isMovingToPos = true;
                this.viewTerrainStartTime = (int)((double)(Time.realtimeSinceStartup * 1000f));
                this.viewTerrainEndTime = this.viewTerrainStartTime + (int)interpolateTime;
                this.viewTerrainStartPos = new Vector2(x, z);
                this.viewTerrainEndPos = new Vector3(terrainX, terrainY);
            }
            else
            {
                this.isMovingToPos = false;
                this.SetCameraPosByViewPos(this.camera, pos, false);
                if (this.ViewTerrainPos_callback != null)
                {
                    this.ViewTerrainPos_callback?.Invoke();
                }
            }
        }

        public void UpdateZooming(Camera camera, Vector3 moveDistVec, int dt)
        {
            if (this.isZooming)
            {
                this.isZooming = false;
            }
            if (this.isZoomingToDxf)
            {
                float num = (float)((double)(Time.realtimeSinceStartup * 1000f));
                if (num >= (float)this.zoomEndTime)
                {
                    this.isZoomingToDxf = false;
                    float fieldOfView = 0f;
                    float num2 = 0f;
                    if (!this.GetDistFovByDxf((float)this.zoomEndDxf, out num2, out fieldOfView))
                    {
                        return;
                    }
                    this.cameraView_dist = num2;
                    this.getWorldCamera()._dist = this.cameraView_dist;
                    camera.fieldOfView = fieldOfView;
                    if (this.autoZoomScreenPos == WorldCameraImpl.INVALID_VECTOR2)
                    {
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                    }
                    else
                    {
                        Vector3 touchTerrainPos = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                        Vector3 touchTerrainPos2 = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                        if (touchTerrainPos2 != WorldCameraImpl.INVALID_VECTOR3)
                        {
                            Vector3 vector = touchTerrainPos2 - touchTerrainPos;
                            this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x - vector.x, 0f, this.viewCenter.y - vector.z), false);
                        }
                        this.autoZoomScreenPos = WorldCameraImpl.INVALID_VECTOR2;
                    }
                    LodManager.GetInstance().UpdateLod();
                    camera.GetComponent<WorldCamera>().UpdateFog();
                    //if (this.SetCameraDxf_callback != null)
                    //{
                    //	this.SetCameraDxf_callback.call();
                    //}
                }
                else
                {
                    float num3 = (float)(this.zoomEndDxf - this.zoomStartDxf);
                    float num4 = (num - (float)this.zoomStartTime) / (float)(this.zoomEndTime - this.zoomStartTime);
                    if (this.SetCameraDxf_curve == null)
                    {
                        float num5 = 0.7f;
                        float num6 = 1f - num5;
                        if (num4 < num5)
                        {
                            num4 /= num5;
                            num4 *= num4;
                            num4 *= num5;
                        }
                        else
                        {
                            num4 = (num4 - num5) / num6;
                            num4 = Mathf.Sqrt(num4);
                            num4 = num4 * num6 + num5;
                        }
                    }
                    else
                    {
                        num4 = this.getValueFromCurve(this.SetCameraDxf_curve, num4);
                    }
                    float dxf = (float)this.zoomStartDxf + num3 * num4;
                    float fieldOfView2 = 0f;
                    float num7 = 0f;
                    if (!this.GetDistFovByDxf(dxf, out num7, out fieldOfView2))
                    {
                        return;
                    }
                    this.cameraView_dist = num7;
                    this.getWorldCamera()._dist = this.cameraView_dist;
                    camera.fieldOfView = fieldOfView2;
                    if (this.autoZoomScreenPos == WorldCameraImpl.INVALID_VECTOR2)
                    {
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                    }
                    else
                    {
                        Vector3 touchTerrainPos3 = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                        Vector3 touchTerrainPos4 = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                        if (touchTerrainPos4 != WorldCameraImpl.INVALID_VECTOR3)
                        {
                            Vector3 vector2 = touchTerrainPos4 - touchTerrainPos3;
                            this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x - vector2.x, 0f, this.viewCenter.y - vector2.z), false);
                        }
                    }
                    LodManager.GetInstance().UpdateLod();
                    camera.GetComponent<WorldCamera>().UpdateFog();
                }
                return;
            }
            if (!this.isZooming && CoreUtils.inputManager.GetTouchCount() == 0 && moveDistVec.x == WorldCameraImpl.INVALID_FLOAT_VALUE && moveDistVec.y == WorldCameraImpl.INVALID_FLOAT_VALUE && moveDistVec.z == WorldCameraImpl.INVALID_FLOAT_VALUE)
            {
                float num8 = this.getcamraInfo_min_dxf();
                float num9 = this.getcamraInfo_limit_min_dxf();
                if (this.customMinDxf > 0f && this.customMinDxf > num8)
                {
                    num8 = this.customMinDxf * (num8 / num9);
                    if (this.customMaxDxf > 0f && num8 > this.customMaxDxf)
                    {
                        num8 = this.customMaxDxf;
                    }
                    num9 = this.customMinDxf;
                }
                float num10 = num8 - this.getCurrentCameraDxf();
                if (num10 > 0f)
                {
                    Debug.Log("zoonReboundDist:" + num10);
                    if (this.ZOOM_REBOUND_SPEED == 0f)
                    {
                        this.ZOOM_REBOUND_SPEED = (num8 - num9) / 500f;
                    }
                    float num11 = this.ZOOM_REBOUND_SPEED * (float)dt;
                    if (num11 > num10)
                    {
                        num11 = num10;
                    }
                    float dxf2 = this.getCurrentCameraDxf() + num11;
                    Debug.Log("getCurrentCameraDxf:" + this.getCurrentCameraDxf());
                    Debug.Log("zoomDist:" + num11);
                    float x = (float)this.zoomCenterX;
                    float y = (float)this.zoomCenterY;
                    Vector3 touchTerrainPos5 = this.GetTouchTerrainPos(camera, x, y);
                    if (touchTerrainPos5.x != WorldCameraImpl.INVALID_FLOAT_VALUE || touchTerrainPos5.y != WorldCameraImpl.INVALID_FLOAT_VALUE || touchTerrainPos5.z != WorldCameraImpl.INVALID_FLOAT_VALUE)
                    {
                        float num12 = 0f;
                        float num13 = 0f;
                        if (!this.GetDistFovByDxf(dxf2, out num13, out num12))
                        {
                            return;
                        }
                        this.cameraView_dist = num13;
                        this.getWorldCamera()._dist = this.cameraView_dist;
                        camera.fieldOfView = num12;
                        Debug.Log(string.Concat(new object[]
                        {
                        "cameraView_dist : ",
                        this.cameraView_dist,
                        " fov:",
                        num12
                        }));
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                        LodManager.GetInstance().UpdateLod();
                        camera.GetComponent<WorldCamera>().UpdateFog();
                    }
                }
                else
                {
                    this.ZOOM_REBOUND_SPEED = 0f;
                }
            }
        }

        public void UpdateChangeMinCameraDxf(int dt)
        {
            float num = this.getcamraInfo_min_dxf() - this.getCurrentCameraDxf();
            if (num > 0f && this.m_addDxfforAdditionHeight_delta > 0f)
            {
                float num2 = this.getCurrentCameraDxf();
                if (num2 < this.getcamraInfo_limit_min_dxf())
                {
                    num2 = this.getcamraInfo_limit_min_dxf();
                }
                if (num2 < this.getcamraInfo_min_dxf())
                {
                    num2 = this.getcamraInfo_min_dxf();
                }
                float x = (float)this.zoomCenterX;
                float y = (float)this.zoomCenterY;
                Vector3 touchTerrainPos = this.GetTouchTerrainPos(this.camera, x, y);
                if (touchTerrainPos.x != WorldCameraImpl.INVALID_FLOAT_VALUE || touchTerrainPos.y != WorldCameraImpl.INVALID_FLOAT_VALUE || touchTerrainPos.z != WorldCameraImpl.INVALID_FLOAT_VALUE)
                {
                    float fieldOfView = 0f;
                    float num3 = 0f;
                    if (!this.GetDistFovByDxf(num2, out num3, out fieldOfView))
                    {
                        return;
                    }
                    this.cameraView_dist = num3;
                    this.getWorldCamera()._dist = this.cameraView_dist;
                    this.camera.fieldOfView = fieldOfView;
                    Vector3 a = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
                    this.cameraPos = a - this.camera.transform.forward * this.cameraView_dist;
                    Common.SetComponentPos(this.camera, this.cameraPos);
                    LodManager.GetInstance().UpdateLod();
                    this.camera.GetComponent<WorldCamera>().UpdateFog();
                }
            }
            this.m_addDxfforAdditionHeight_delta = 0f;
        }

        public void UpdateMoveToPos()
        {
            if (!this.isMovingToPos)
            {
                return;
            }
            float num = (float)((double)(Time.realtimeSinceStartup * 1000f));
            if (num >= (float)this.viewTerrainEndTime)
            {
                this.isMovingToPos = false;
                float x = this.viewTerrainEndPos.x;
                float y = this.viewTerrainEndPos.y;
                this.SetCameraPosByViewPos(this.camera, new Vector3(x, 0f, y), false);
                if (this.ViewTerrainPos_callback != null)
                {
                    this.ViewTerrainPos_callback?.Invoke();
                }
            }
            else
            {
                Vector2 a = this.viewTerrainEndPos - this.viewTerrainStartPos;
                float magnitude = a.magnitude;
                Vector2 a2 = a / magnitude;
                float num2 = (num - (float)this.viewTerrainStartTime) / (float)(this.viewTerrainEndTime - this.viewTerrainStartTime);
                if (this.ViewTerrainPos_curve == null)
                {
                    float num3 = 0.7f;
                    float num4 = 1f - num3;
                    if (num2 < num3)
                    {
                        num2 /= num3;
                        num2 *= num2;
                        num2 *= num3;
                    }
                    else
                    {
                        num2 = (num2 - num3) / num4;
                        num2 = Mathf.Sqrt(num2);
                        num2 = num2 * num4 + num3;
                    }
                }
                else
                {
                    num2 = this.getValueFromCurve(this.ViewTerrainPos_curve, num2);
                }
                float d = magnitude * num2;
                Vector2 vector = this.viewTerrainStartPos + a2 * d;
                float x2 = vector.x;
                float y2 = vector.y;
                this.SetCameraPosByViewPos(this.camera, new Vector3(x2, 0f, y2), false);
            }
        }

        private float CalcSlowDrag(float value, float viewPos, float minValue, float maxValue, float offset)
        {
            if (viewPos > maxValue && value > 0f)
            {
                float num = Mathf.Clamp(viewPos - maxValue, 0f, offset);
                float num2 = num / offset;
                num2 = 1f - num2;
                float num3 = Mathf.Pow(num2, this.slowDragPowValue);
                value = value * this.slowDragRate * num3;
                return value;
            }
            if (viewPos < minValue && value < 0f)
            {
                float num4 = Mathf.Clamp(minValue - viewPos, 0f, offset);
                float num5 = num4 / offset;
                num5 = 1f - num5;
                float num6 = Mathf.Pow(num5, this.slowDragPowValue);
                value = value * this.slowDragRate * num6;
                return value;
            }
            return value;
        }

        private float CalcRebound(float value, float curSpeed, float minValue, float maxValue, float time, float dt, out float outSpeed, out bool changed)
        {
            if (value > maxValue)
            {
                float num = (value - maxValue) / time;
                if (num > curSpeed)
                {
                    curSpeed = num;
                }
                else
                {
                    num = curSpeed;
                }
                value = Mathf.Max(maxValue, value - num * dt);
                outSpeed = curSpeed;
                changed = true;
            }
            else if (value < minValue)
            {
                float num2 = (minValue - value) / time;
                if (num2 > curSpeed)
                {
                    curSpeed = num2;
                }
                else
                {
                    num2 = curSpeed;
                }
                value = Mathf.Min(minValue, value + num2 * dt);
                outSpeed = curSpeed;
                changed = true;
            }
            else
            {
                outSpeed = 0f;
                changed = false;
            }
            return value;
        }

        public Vector3 GetScreenSpecailPosTerrainPos(Camera camera, int sign)
        {
            Vector3 position = new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f);
            if (sign == 0)
            {
                position = new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f);
            }
            else if (sign == 1)
            {
                position = new Vector3(0f, 0f, 0f);
            }
            else if (sign == 2)
            {
                position = new Vector3((float)Screen.width, 0f, 0f);
            }
            else if (sign == 3)
            {
                position = new Vector3((float)Screen.width, (float)Screen.height, 0f);
            }
            else if (sign == 4)
            {
                position = new Vector3(0f, (float)Screen.height, 0f);
            }
            Ray ray = camera.ScreenPointToRay(position);
            float enter = 0f;
            bool flag = this.WorldMgr_terrainPlane.Raycast(ray, out enter);
            if (flag)
            {
                return Common.CalcRayEnterPos(ray, enter);
            }
            return new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
        }

        public Vector3 GetTouchTerrainPos(Camera camera, float x, float y)
        {
            Vector3 position = camera.transform.position;
            camera.transform.position = Vector3.zero;
            Ray ray = camera.ScreenPointToRay(new Vector3(x, y, 0f));
            camera.transform.position = position;
            Ray ray2 = new Ray(position, ray.direction);
            float enter = 0f;
            bool flag = this.WorldMgr_terrainPlane.Raycast(ray2, out enter);
            if (flag)
            {
                return Common.CalcRayEnterPos(ray2, enter);
            }
            return new Vector3(WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE, WorldCameraImpl.INVALID_FLOAT_VALUE);
        }

        //> todo
        public Vector2 gridSize { get; set; }
        public void SetCameraPosByViewPos(Camera camera, Vector3 pos, bool isPredict)
        {
            float num = gridSize.x;
            float num2 = gridSize.y;

            if (Common.IsNaN(pos.x) || Common.IsNaN(pos.y) || Common.IsNaN(pos.z))
            {
                pos = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
            }
            Vector3 vector = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
            this.cameraPos = pos - camera.transform.forward * this.cameraView_dist;
            Common.SetComponentPos(camera, this.cameraPos);
            this.viewCenter.x = pos.x;
            this.viewCenter.y = pos.z;
            float x;
            float y;
            if (this.isMovingToPos)
            {
                x = this.viewTerrainEndPos.x;
                y = this.viewTerrainEndPos.y;
            }
            else if (isPredict)
            {
                Vector2 a = new Vector2(pos.x, pos.z);
                Vector2 b = new Vector2(vector.x, vector.y);
                Vector2 a2 = a - b;
                float magnitude = a2.magnitude;
                if ((double)magnitude > 0.001)
                {
                    Vector2 vector2 = a2 / magnitude;
                    Vector2 b2 = new Vector2((float)((double)(vector2.x * num) * 0.2), (float)((double)(vector2.y * num2) * 0.2));
                    Vector2 vector3 = this.viewCenter + b2;
                    x = vector3.x;
                    y = vector3.y;
                }
                else
                {
                    x = this.viewCenter.x;
                    y = this.viewCenter.y;
                }
            }
            else
            {
                x = this.viewCenter.x;
                y = this.viewCenter.y;
            }
            float x2 = this.viewCenter.x;
            float y2 = this.viewCenter.y;
            //double num3 = (double)WorldCameraImpl.GetLuaFunction("WorldMgr.OnViewChanged").call(new object[]
            //{
            //	WorldCameraImpl.WorldMgrLtb,
            //	x2,
            //	y2,
            //	this.cameraView_dist * camera.fieldOfView,
            //	x,
            //	y
            //});
            //this.setAdditionHeightForMinDxf((float)num3);
            this.UpdateChangeMinCameraDxf(this.m_dt);
            //WorldCameraImpl.GetLuaFunction("WorldMgr.CheckCameraParam").call(WorldCameraImpl.WorldMgrLtb);
            //if (this.moveFlag == 0)
            //{
            //	WorldCameraImpl.GetLuaFunction("WorldCamera.SendEventCameraStartMove").call();
            //}
            this.moveFlag = 3;
        }

        public void CalcViewCenter(Camera camera1)
        {
        }

        public void InitViewCenter(Camera camera1)
        {
            if (camera1 == null)
            {
                camera1 = this.camera;
            }
            Ray ray = camera1.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            float enter = 0f;
            bool flag = this.WorldMgr_terrainPlane.Raycast(ray, out enter);
            if (flag)
            {
                Vector3 b = Common.CalcRayEnterPos(ray, enter);
                this.cameraView_dist = (camera1.transform.position - b).magnitude;
                this.viewCenter = new Vector2(b.x, b.z);
            }
        }

        public bool IsInBound(float x, float y)
        {
            return (double)x >= this.worldMinX && (double)x <= this.worldMaxX && (double)y >= this.worldMinY && (double)y <= this.worldMaxY;
        }

        public bool CheckCanDragout(float x, float y)
        {
            float num = 0f;
            float num2 = 0f;
            return (double)x >= this.worldMinX + (double)num && (double)x <= this.worldMaxX - (double)num && (double)y >= this.worldMinY + (double)num2 && (double)y <= this.worldMaxY - (double)num2;
        }

        public void ViewCityToWorld()
        {
            Camera camera = (!(this.camera == null)) ? this.camera : WorldCamera.camera;
            float dxf = this.cameraInfo_city.dxf;
            this.SetCameraDxf(camera, dxf, 300f);
        }

        public void AutoZoomToDxf(float to_dxf)
        {
            Camera camera = (!(this.camera == null)) ? this.camera : WorldCamera.camera;
            this.SetCameraDxf(camera, to_dxf, 500f);
        }

        private void CheckStopTouchWhenAutoMoving()
        {
            if (this.eventSystemObj == null)
            {
                return;
            }
            EventSystem component = this.eventSystemObj.GetComponent<EventSystem>();
            if (this._isMovingToPos || this._isZoomingToDxf)
            {
                if (component != null && component.enabled)
                {
                    component.enabled = false;
                }
            }
            else if (component != null && !component.enabled)
            {
                component.enabled = true;
            }
        }

        public void ShowCameraViewRect()
        {
            if (this.camera == null)
            {
                return;
            }
            Ray ray = this.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            float enter = 0f;
            bool flag = this.WorldMgr_terrainPlane.Raycast(ray, out enter);
            if (flag)
            {
                Vector3 center = Common.CalcRayEnterPos(ray, enter);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(center, 0.1f);
            }
            Ray ray2 = this.camera.ViewportPointToRay(new Vector3(0f, 0f, 0f));
            Ray ray3 = this.camera.ViewportPointToRay(new Vector3(1f, 0f, 0f));
            Ray ray4 = this.camera.ViewportPointToRay(new Vector3(1f, 1f, 0f));
            Ray ray5 = this.camera.ViewportPointToRay(new Vector3(0f, 1f, 0f));
            flag = this.WorldMgr_terrainPlane.Raycast(ray2, out enter);
            Vector3 vector = Common.CalcRayEnterPos(ray2, enter);
            flag = this.WorldMgr_terrainPlane.Raycast(ray3, out enter);
            Vector3 vector2 = Common.CalcRayEnterPos(ray3, enter);
            flag = this.WorldMgr_terrainPlane.Raycast(ray4, out enter);
            Vector3 vector3 = Common.CalcRayEnterPos(ray4, enter);
            flag = this.WorldMgr_terrainPlane.Raycast(ray5, out enter);
            Vector3 vector4 = Common.CalcRayEnterPos(ray5, enter);
            Gizmos.DrawLine(vector, vector2);
            Gizmos.DrawLine(vector2, vector3);
            Gizmos.DrawLine(vector3, vector4);
            Gizmos.DrawLine(vector4, vector);
        }
    }
}
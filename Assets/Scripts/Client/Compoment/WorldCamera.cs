using Skyunion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    [RequireComponent(typeof(Camera))]
    public class WorldCamera : MonoSingleton<WorldCamera>
    {
        [SerializeField]
        public class cameraInfoItem
        {
            public string Id;

            public float dist;

            public float fov;

            public float dxf;

            public Vector3 forward;
        }

        private Camera m_Camera;

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

        private static Vector2 INVALID_VECTOR2 = new Vector2(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);

        private static Vector3 INVALID_VECTOR3 = new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);

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

        private Vector2 viewCenter = new Vector2(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);

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

        private Vector2 viewTerrainStartPos = new Vector2(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);

        private Vector2 viewTerrainEndPos = new Vector2(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);

        private bool _isZoomingToDxf;

        private int zoomStartTime;

        private int zoomEndTime;

        private double zoomStartDxf;

        private double zoomEndDxf;

        private Vector2 autoZoomScreenPos = INVALID_VECTOR2;

        private string followObjId = string.Empty;

        private int moveFlag;

        private int moveStopTimeStamp;

        private int lastUpdateTimeStamp;

        private float cameraView_dist;

        [SerializeField]
        public List<cameraInfoItem> cameraInfoList = new List<cameraInfoItem>();

        private GameObject eventSystemObj;

        private int m_dt;

        private cameraInfoItem cameraInfo_limit_min;

        private cameraInfoItem cameraInfo_min;

        private cameraInfoItem cameraInfo_limit_max;

        private Vector3 cameraPos = Vector3.zero;

        private float m_additionHeightForMinDxf;

        private float m_addDxfforAdditionHeight_delta;

        private float interpolateTime = INVALID_FLOAT_VALUE;

        private float WorldMgr_worldMinX;

        private float WorldMgr_worldMaxX;

        private float WorldMgr_worldMinY;

        private float WorldMgr_worldMaxY;

        private Plane WorldMgr_terrainPlane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));

        private float[] ViewTerrainPos_curve;

        private float[] SetCameraDxf_curve;

        private Action ViewTerrainPos_callback;

        private Action SetCameraDxf_callback;

        private Action<float, float, float> ViewChange_callback;

        private Action<float, float> MapClick_callback;

        private const float m_auto_zoom_speed = 500f;

        private Vector2 touchMovePos;
        public Vector2 gridSize { get; set; }

        public bool AllowTouchWhenMovingOrZooming { get; set; }

        private int m_zoomLevel;

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

        public Plane GetTerrainPlane
        {
            get { return WorldMgr_terrainPlane; }
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


        public override void Init()
        {
            m_Camera = GetComponent<Camera>();
            m_Camera.depthTextureMode = DepthTextureMode.Depth;

            if (CoreUtils.inputManager == null)
            {
                Invoke("Init", 0.0f);
                return;
            }

            CoreUtils.inputManager.SetTouchZoomEvent(OnTouchZoomedBegin, OnTouchZoomed);
            CoreUtils.inputManager.AddTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);

            if (eventSystemObj == null)
            {
                eventSystemObj = GameObject.Find("EventSystem");
            }

            cameraInfoList.Add(new cameraInfoItem()
            {
                Id = "limit_min",
                dist = 14.47f,
                fov = 5.3f,
                dxf = 76.691f,
                forward = new Vector3(0, -0.7f, 0.7f),
            });
            cameraInfoList.Add(new cameraInfoItem()
            {
                Id = "min",
                dist = 15.47f,
                fov = 5.33f,
                dxf = 82.4551f,
                forward = new Vector3(0, -0.7f, 0.7f),
            });
            cameraInfoList.Add(new cameraInfoItem()
            {
                Id = "limit_max",
                dist = 2425,
                fov = 30,
                dxf = 72750,
                forward = new Vector3(0, -0.7f, 0.7f),
            });

            InitParam();
        }
        public void ResetCamera(List<cameraInfoItem> cameras)
        {
            cameraInfoList.Clear();
            cameraInfoList = cameras;
            InitParam();
        }

        private void InitParam()
        {

            isTouching = false;
            isDragging = false;
            lastTouchX = 0;
            lastTouchY = 0;
            touchStartX = 0;
            touchStartY = 0;

            touchStartTerrainPos = new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            touchStartViewCenter = new Vector2(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            lastTouchTime = 0;
            viewCenter = new Vector2(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            dragDir = Vector3.one;
            reboundDir = Vector3.one;
            reboundAcc = false;
            dragSpeed = 0f;
            dragHistory = new ArrayList();
            canDragOut = false;
            reboundSpeed = 0.0;
            zoomCenterX = 0.0;
            zoomCenterY = 0.0;
            reboundInertial = Vector3.zero;
            dragInertial = Vector3.zero;
            releaseSpeed = Vector3.zero;
            releaseTimeStamp = 0;
            canClick = true;
            canDrag = true;
            canZoom = true;
            _isMovingToPos = false;
            viewTerrainStartTime = 0;
            viewTerrainEndTime = 0;
            viewTerrainStartPos = new Vector2(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            viewTerrainEndPos = new Vector2(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            _isZoomingToDxf = false;
            zoomStartTime = 0;
            zoomEndTime = 0;
            zoomStartDxf = (double)INVALID_FLOAT_VALUE;
            zoomEndDxf = (double)INVALID_FLOAT_VALUE;
            autoZoomScreenPos = INVALID_VECTOR2;
            followObjId = string.Empty;
            moveFlag = 0;
            moveStopTimeStamp = 0;
            lastUpdateTimeStamp = 0;
            cameraPos = m_Camera.transform.position;
            InitViewCenter(m_Camera);
            cameraInfo_limit_min = this.getCameraInfo("limit_min");
            cameraInfo_min = this.getCameraInfo("min");
            cameraInfo_limit_max = this.getCameraInfo("limit_max");
            if (stopUITouchWhenAutoMoving)
            {
                CheckStopTouchWhenAutoMoving();
            }
        }

        public void AddMapClickListner(Action<float, float> callback)
        {
            MapClick_callback += callback;
        }
        public void RemoveMapClickListner(Action<float, float> callback)
        {
            MapClick_callback -= callback;
        }

        public bool IsSlipping()
        {
            return releaseTimeStamp > 0 || isDragging || this.moveFlag>0;
        }

        public void OnTouchBegan(int x, int y)
        {
            if (isMovingToPos)
            {
                return;
            }
            if (isZoomingToDxf)
            {
                return;
            }
            if (m_Camera == null)
            {
                return;
            }
            if (isZooming)
            {
                isZooming = false;
            }
            if (!canClick && !this.canDrag)
            {
                return;
            }
            Vector3 touchTerrainPos = this.GetTouchTerrainPos(m_Camera, x, y);
            if (touchTerrainPos.x == INVALID_FLOAT_VALUE && touchTerrainPos.y == INVALID_FLOAT_VALUE && touchTerrainPos.z == INVALID_FLOAT_VALUE)
            {
                return;
            }
            touchStartTerrainPos = touchTerrainPos;
            touchStartViewCenter = this.viewCenter;
            isTouching = true;
            dragSpeed = 0f;
            reboundSpeed = 0.0;
            reboundAcc = false;
            canDragOut = this.CheckCanDragout(this.viewCenter.x, this.viewCenter.y);
            inertialVec = Vector3.zero;
            dragInertial = Vector3.zero;
            releaseSpeed = Vector3.zero;
            releaseTimeStamp = 0;
            reboundInertial = Vector3.zero;
            dragHistory.Clear();
            touchStartX = (int)x;
            touchStartY = (int)y;
            lastTouchX = (int)x;
            lastTouchY = (int)y;
            lastTouchTime = (int)((double)(Time.realtimeSinceStartup * 1000f));
        }
        public void OnTouchMoved(int x, int y)
        {

            if (!canDrag)
            {
                return;
            }
            if (isZooming)
            {
                return;
            }
            if (!isTouching)
            {
                return;
            }
            isDragging = true;
            if (m_Camera != null)
            {
                Vector3 touchTerrainPos = GetTouchTerrainPos(m_Camera, x, y);
                if (touchTerrainPos.x != INVALID_FLOAT_VALUE && touchTerrainPos.y != INVALID_FLOAT_VALUE && touchTerrainPos.z != INVALID_FLOAT_VALUE)
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
                        this.SetCameraPosByViewPos(this.m_Camera, new Vector3(this.viewCenter.x + a.x, 0f, this.viewCenter.y + a.z), true);
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
        public void OnTouchEnded(int x, int y)
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
                        Vector3 touchTerrainPos = new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
                        if (!this.isDragging && Mathf.Abs(x - (float)this.touchStartX) < 10f && Mathf.Abs(y - (float)this.touchStartY) < 10f && !CoreUtils.inputManager.IsTouchedUI())
                        {
                            touchTerrainPos = this.GetTouchTerrainPos(m_Camera, x, y);
                            MapClick_callback?.Invoke(touchTerrainPos.x, touchTerrainPos.z);
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
            if (!this.isTouching)
            {
                return;
            }
            if (this.isZooming)
            {
                return;
            }
            Vector3 touchTerrainPos2 = new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            if (!this.isDragging && Mathf.Abs(x - (float)this.touchStartX) < 10f && Mathf.Abs(y - (float)this.touchStartY) < 10f && !CoreUtils.inputManager.IsTouchedUI())
            {
                touchTerrainPos2 = this.GetTouchTerrainPos(this.m_Camera, x, y);
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
            if (touchTerrainPos2.x != INVALID_FLOAT_VALUE && touchTerrainPos2.y != INVALID_FLOAT_VALUE && touchTerrainPos2.z != INVALID_FLOAT_VALUE && this.canClick)
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
                    var touchTerrainPos = this.GetTouchTerrainPos(m_Camera, x, y);
                    MapClick_callback?.Invoke(touchTerrainPos.x, touchTerrainPos.z);
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

            touchMovePos = new Vector2(touchTerrainPos2.x, touchTerrainPos2.z);
        }

        public Vector2 GetTouchMovePos()
        {
            return touchMovePos;
        }

        public void OnTouchZoomedBegin(int centerX, int centerY)
        {
            this.zoomStartDxf = (double)this.getCurrentCameraDxf();
        }
        public void OnTouchZoomed(int centerX, int centerY, float scrollRate)
        {
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
            if (!this.m_Camera)
            {
                return;
            }
            if (CoreUtils.uiManager.IsHasPopView())
            {
                return;
            }
            Vector3 touchTerrainPos = this.GetTouchTerrainPos(m_Camera, centerX, centerY);
            if (touchTerrainPos.x == INVALID_FLOAT_VALUE && touchTerrainPos.y == INVALID_FLOAT_VALUE && touchTerrainPos.z == INVALID_FLOAT_VALUE)
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
            this.m_Camera.fieldOfView = fieldOfView;
            float num6 = this.viewCenter.x;
            float num7 = this.viewCenter.y;
            num6 = Mathf.Clamp(num6, (float)this.worldMinX, (float)this.worldMaxX);
            num7 = Mathf.Clamp(num7, (float)this.worldMinY, (float)this.worldMaxY);
            this.SetCameraPosByViewPos(this.m_Camera, new Vector3(num6, 0f, num7), false);
            Vector3 touchTerrainPos2 = this.GetTouchTerrainPos(this.m_Camera, centerX, centerY);
            if (touchTerrainPos2.x == INVALID_FLOAT_VALUE && touchTerrainPos2.y == INVALID_FLOAT_VALUE && touchTerrainPos2.z == INVALID_FLOAT_VALUE)
            {
                return;
            }
            Vector3 vector = touchTerrainPos2 - touchTerrainPos;
            float num8 = this.viewCenter.x - vector.x;
            float num9 = this.viewCenter.y - vector.z;
            num8 = Mathf.Clamp(num8, (float)this.worldMinX, (float)this.worldMaxX);
            num9 = Mathf.Clamp(num9, (float)this.worldMinY, (float)this.worldMaxY);
            this.SetCameraPosByViewPos(this.m_Camera, new Vector3(num8, 0f, num9), false);
            ClientUtils.lodManager.UpdateLodDistance(m_Camera, cameraView_dist);
            this.zoomCenterX = (double)centerX;
            this.zoomCenterY = (double)centerY;
        }

        public void ViewTerrainPos(float terrainX, float terrainY, float interpolateTime, Action callback)
        {
            this.ViewTerrainPosCurve(terrainX, terrainY, interpolateTime, null, callback);
        }

        public void ViewTerrainPosCurve(float terrainX, float terrainY, float interpolateTime, float[] curve, Action callback)
        {
            if (float.IsNaN(terrainX) || float.IsNaN(terrainY))
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
            int num = Screen.width;
            int num2 = Screen.height;
            Vector3 vector = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
            float x = vector.x;
            float z = vector.z;
            Vector3 vector2 = new Vector3(terrainX - x, 0f, terrainY - z);
            Vector3 pos = new Vector3(terrainX, 0f, terrainY);
            if (interpolateTime != INVALID_FLOAT_VALUE && !this.isMovingToPos)
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
                this.SetCameraPosByViewPos(this.m_Camera, pos, false);
                if (this.ViewTerrainPos_callback != null)
                {
                    this.ViewTerrainPos_callback?.Invoke();
                }
            }
        }



        public void SetCameraDxf(float dxf, float interpolateTime, Action callback)
        {
            this.autoZoomScreenPos = INVALID_VECTOR2;
            this.SetCameraDxfCurve(dxf, interpolateTime, null, callback);
        }

        public void SetCameraDxfAtScreenPos(float dxf, float interpolateTime, Vector2 screenPos, Action callback)
        {
            if (this.isZoomingToDxf)
            {
                return;
            }
            this.autoZoomScreenPos = screenPos;
            this.SetCameraDxfCurve(dxf, interpolateTime, null, callback);
        }

        public void SetCameraDxfCurveAtScreenPos(float dxf, float interpolateTime, Vector2 screenPos, float[] curve, Action callback)
        {
            if (this.isZoomingToDxf)
            {
                return;
            }
            this.autoZoomScreenPos = screenPos;
            this.SetCameraDxfCurve(dxf, interpolateTime, curve, callback);
        }

        public void SetCameraDxfCurve(float dxf, float interpolateTime, float[] curve, Action callback)
        {
            if (this.isZoomingToDxf)
            {
                return;
            }
            this.SetCameraDxf_curve = curve;
            this.SetCameraDxf_callback = callback;
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
            if (interpolateTime != INVALID_FLOAT_VALUE && !this.isZoomingToDxf)
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
                m_Camera.fieldOfView = fieldOfView;
                if (this.autoZoomScreenPos == INVALID_VECTOR2)
                {
                    this.SetCameraPosByViewPos(m_Camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                }
                else
                {
                    Vector3 touchTerrainPos = this.GetTouchTerrainPos(m_Camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                    this.SetCameraPosByViewPos(m_Camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                    Vector3 touchTerrainPos2 = this.GetTouchTerrainPos(m_Camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                    if (touchTerrainPos2 != INVALID_VECTOR3)
                    {
                        Vector3 vector = touchTerrainPos2 - touchTerrainPos;
                        this.SetCameraPosByViewPos(m_Camera, new Vector3(this.viewCenter.x - vector.x, 0f, this.viewCenter.y - vector.z), false);
                    }
                    this.autoZoomScreenPos = INVALID_VECTOR2;
                }
                ClientUtils.lodManager.UpdateLodDistance(m_Camera, cameraView_dist);
                if (this.SetCameraDxf_callback != null)
                {
                    this.SetCameraDxf_callback?.Invoke();
                }
            }
        }

        public void Update()
        {
            try
            {
                if (cameraInfo_min == null)
                    return;

                this.m_dt = (int)(Time.deltaTime * 1000);
                int num = (int)((double)(Time.realtimeSinceStartup * 1000f));
                while (this.dragHistory.Count > 0)
                {
                    if (((Vector2)this.dragHistory[0]).x >= (float)(num - 100))
                    {
                        break;
                    }
                    this.dragHistory.RemoveAt(0);
                }
                Vector3 moveDistVec = this.UpdateInertial(this.m_Camera, m_dt);
                this.UpdateZooming(this.m_Camera, moveDistVec, m_dt);
                this.UpdateMoveToPos();
                if (this.moveFlag > 0)
                {
                    this.moveFlag--;
                    if (this.moveFlag == 0)
                    {
                        //WorldCameraImpl.GetLuaFunction("WorldCamera.SendEventCameraStopMove").call();
                        this.moveStopTimeStamp = (int)Time.realtimeSinceStartup;
                    }
                    float x = this.viewCenter.x;
                    float y = this.viewCenter.y;
                    ViewChange_callback?.Invoke(x, y, getCurrentCameraDxf());
                }
                else if (!this.isTouching && !this.isDragging && !this.isMovingToPos && (double)(this.lastUpdateTimeStamp - this.moveStopTimeStamp) < 0.5 && (double)(Time.realtimeSinceStartup - (float)this.moveStopTimeStamp) >= 0.5)
                {
                    float x = this.viewCenter.x;
                    float y = this.viewCenter.y;
                    ViewChange_callback?.Invoke(x, y, getCurrentCameraDxf());
                    //double num2 = (double)WorldCameraImpl.GetLuaFunction("WorldMgr.OnViewChanged").call(new object[]
                    //{
                    //	WorldCameraImpl.WorldMgrLtb,
                    //	x,
                    //	y,
                    //	this.getCurrentCameraDxf(),
                    //	x,
                    //	y
                    //});
                    this.UpdateChangeMinCameraDxf(this.m_dt);
                }
                
                

                
                
                this.lastUpdateTimeStamp = (int)Time.realtimeSinceStartup;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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
                this.SetCameraPosByViewPos(this.m_Camera, new Vector3(x, 0f, y), false);
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
                this.SetCameraPosByViewPos(this.m_Camera, new Vector3(x2, 0f, y2), false);
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
                    camera.fieldOfView = fieldOfView;
                    if (this.autoZoomScreenPos == INVALID_VECTOR2)
                    {
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                    }
                    else
                    {
                        Vector3 touchTerrainPos = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                        Vector3 touchTerrainPos2 = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                        if (touchTerrainPos2 != INVALID_VECTOR3)
                        {
                            Vector3 vector = touchTerrainPos2 - touchTerrainPos;
                            this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x - vector.x, 0f, this.viewCenter.y - vector.z), false);
                        }
                        this.autoZoomScreenPos = INVALID_VECTOR2;
                    }
                    ClientUtils.lodManager.UpdateLodDistance(m_Camera, cameraView_dist);
                    if (this.SetCameraDxf_callback != null)
                    {
                        this.SetCameraDxf_callback?.Invoke();
                    }
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
                    camera.fieldOfView = fieldOfView2;
                    if (this.autoZoomScreenPos == INVALID_VECTOR2)
                    {
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                    }
                    else
                    {
                        Vector3 touchTerrainPos3 = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                        Vector3 touchTerrainPos4 = this.GetTouchTerrainPos(camera, this.autoZoomScreenPos.x, this.autoZoomScreenPos.y);
                        if (touchTerrainPos4 != INVALID_VECTOR3)
                        {
                            Vector3 vector2 = touchTerrainPos4 - touchTerrainPos3;
                            this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x - vector2.x, 0f, this.viewCenter.y - vector2.z), false);
                        }
                    }
                    ClientUtils.lodManager.UpdateLodDistance(m_Camera, cameraView_dist);
                }
                return;
            }
            if (!this.isZooming && CoreUtils.inputManager.GetTouchCount() == 0 && moveDistVec.x == INVALID_FLOAT_VALUE && moveDistVec.y == INVALID_FLOAT_VALUE && moveDistVec.z == INVALID_FLOAT_VALUE)
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
                    //Debug.Log("zoonReboundDist:" + num10);
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
                    //Debug.Log("getCurrentCameraDxf:" + this.getCurrentCameraDxf());
                    //Debug.Log("zoomDist:" + num11);
                    float x = (float)this.zoomCenterX;
                    float y = (float)this.zoomCenterY;
                    Vector3 touchTerrainPos5 = this.GetTouchTerrainPos(camera, x, y);
                    if (touchTerrainPos5.x != INVALID_FLOAT_VALUE || touchTerrainPos5.y != INVALID_FLOAT_VALUE || touchTerrainPos5.z != INVALID_FLOAT_VALUE)
                    {
                        float num12 = 0f;
                        float num13 = 0f;
                        if (!this.GetDistFovByDxf(dxf2, out num13, out num12))
                        {
                            return;
                        }
                        this.cameraView_dist = num13;
                        camera.fieldOfView = num12;
                        this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x, 0f, this.viewCenter.y), false);
                        ClientUtils.lodManager.UpdateLodDistance(m_Camera, cameraView_dist);
                    }
                }
                else
                {
                    this.ZOOM_REBOUND_SPEED = 0f;
                }
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
        public Vector3 UpdateInertial(Camera camera, int dt)
        {
            if (this.isTouching || this.isMovingToPos)
            {
                return new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            }
            float magnitude = this.reboundInertial.magnitude;
            Vector3 vector = new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            if (magnitude > 0f)
            {
                vector = this.reboundInertial * (float)dt;
            }
            double num = (double)(Time.realtimeSinceStartup * 1000f);
            Vector3 vector2 = new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
            Vector3 a = new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
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
                        if (num4 > 0.001)
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
                            if (vector.x == INVALID_FLOAT_VALUE && vector.y == INVALID_FLOAT_VALUE && vector.z == INVALID_FLOAT_VALUE)
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
            if (vector.x != INVALID_FLOAT_VALUE || vector.y != INVALID_FLOAT_VALUE || vector.z != INVALID_FLOAT_VALUE)
            {
                this.SetCameraPosByViewPos(camera, new Vector3(this.viewCenter.x + vector.x, 0f, this.viewCenter.y + vector.z), true);
            }
            if (vector2.x != INVALID_FLOAT_VALUE || vector2.y != INVALID_FLOAT_VALUE || vector2.z != INVALID_FLOAT_VALUE)
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


        public void SetCameraPosByViewPos(Camera camera, Vector3 pos, bool isPredict)
        {
            float num = gridSize.x;
            float num2 = gridSize.y;

            if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
            {
                pos = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
            }
            Vector3 vector = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
            this.cameraPos = pos - camera.transform.forward * this.cameraView_dist;
            this.m_Camera.transform.position = cameraPos;
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
            ViewChange_callback?.Invoke(x, y, getCurrentCameraDxf());

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
                Vector3 touchTerrainPos = this.GetTouchTerrainPos(this.m_Camera, x, y);
                if (touchTerrainPos.x != INVALID_FLOAT_VALUE || touchTerrainPos.y != INVALID_FLOAT_VALUE || touchTerrainPos.z != INVALID_FLOAT_VALUE)
                {
                    float fieldOfView = 0f;
                    float num3 = 0f;
                    if (!this.GetDistFovByDxf(num2, out num3, out fieldOfView))
                    {
                        return;
                    }
                    this.cameraView_dist = num3;
                    this.m_Camera.fieldOfView = fieldOfView;
                    Vector3 a = new Vector3(this.viewCenter.x, 0f, this.viewCenter.y);
                    this.cameraPos = a - this.m_Camera.transform.forward * this.cameraView_dist;
                    this.m_Camera.transform.position = cameraPos;
                    ClientUtils.lodManager.UpdateLodDistance(m_Camera, cameraView_dist);
                }
            }
            this.m_addDxfforAdditionHeight_delta = 0f;
        }

        public bool GetDistFovByDxf(float dxf, out float dist, out float fov)
        {
            dist = 0f;
            fov = 0f;
            cameraInfoItem cameraInfoItem = this.cameraInfo_limit_min;
            cameraInfoItem cameraInfoItem2 = this.cameraInfo_limit_max;
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
            for (int i = 0; i < cameraInfoList.Count - 1; i++)
            {
                cameraInfoItem cameraInfoItem3 = (cameraInfoItem)cameraInfoList[i];
                cameraInfoItem cameraInfoItem4 = (cameraInfoItem)cameraInfoList[i + 1];
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

        private void CheckStopTouchWhenAutoMoving()
        {
            if (this.eventSystemObj == null)
            {
                return;
            }
            EventSystem component = this.eventSystemObj.GetComponent<EventSystem>();
            if (!this.AllowTouchWhenMovingOrZooming&&(this._isMovingToPos || this._isZoomingToDxf))
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
        public float getCameraDxf(string Id)
        {
            for (int i = 0; i < cameraInfoList.Count; i++)
            {
                cameraInfoItem cameraInfoItem = cameraInfoList[i];
                if (Id.CompareTo(cameraInfoItem.Id) == 0)
                {
                    return cameraInfoItem.dxf;
                }
            }
            Debug.LogError("error id on getCameraDxf!!!!!!");
            return 0f;
        }

        private cameraInfoItem getCameraInfo(string Id)
        {
            for (int i = 0; i < cameraInfoList.Count; i++)
            {
                cameraInfoItem cameraInfoItem = (cameraInfoItem)cameraInfoList[i];
                if (Id.CompareTo(cameraInfoItem.Id) == 0)
                {
                    return cameraInfoItem;
                }
            }
            Debug.LogError("error id on getCameraDxf!!!!!!");
            return null;
        }

        private Vector3 CalcRayEnterPos(Ray ray, float enter)
        {
            return ray.origin + ray.direction * enter;
        }

        public void InitViewCenter(Camera camera1)
        {
            if (camera1 == null)
            {
                camera1 = m_Camera;
            }
            Ray ray = camera1.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            float enter = 0f;
            bool flag = this.WorldMgr_terrainPlane.Raycast(ray, out enter);
            if (flag)
            {
                Vector3 b = CalcRayEnterPos(ray, enter);
                cameraView_dist = (camera1.transform.position - b).magnitude;
                viewCenter = new Vector2(b.x, b.z);
            }
        }

        public Camera GetCamera()
        {
            return m_Camera;
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
                return CalcRayEnterPos(ray2, enter);
            }
            return new Vector3(INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE, INVALID_FLOAT_VALUE);
        }
        public bool CheckCanDragout(float x, float y)
        {
            float num = 0f;
            float num2 = 0f;
            return (double)x >= this.worldMinX + (double)num && (double)x <= this.worldMaxX - (double)num && (double)y >= this.worldMinY + (double)num2 && (double)y <= this.worldMaxY - (double)num2;
        }
        public float getCurrentCameraDxf()
        {
            if (m_Camera == null)
            {
                return Camera.main.fieldOfView * Camera.main.transform.position.y;
            }
            return cameraView_dist * m_Camera.fieldOfView;
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
            //            Debug.Log("设置世界摄像头是否可拖动"+isEnable);
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
        public bool IsAutoMoving()
        {
            return this.isMovingToPos || this.isZoomingToDxf;
        }
        public float getCurrentCameraFov()
        {
            if (this.m_Camera == null)
            {
                return 0f;
            }
            return this.m_Camera.fieldOfView;
        }

        public float getCurrentCameraDist()
        {
            if (this.m_Camera == null)
            {
                return 0f;
            }
            return this.cameraView_dist;
        }

        public void AddViewChange(Action<float, float, float> callback)
        {
            ViewChange_callback += callback;
        }
        public void RemoveViewChange(Action<float, float, float> callback)
        {
            ViewChange_callback -= callback;
        }

        public void SetZoomLevel(int level)
        {
            m_zoomLevel = level;
        }

        public int GetZoomLevel()
        {
            return m_zoomLevel;
        }

        public void ClearViewChange()
        {
            ViewChange_callback = null;
        }
    }
}

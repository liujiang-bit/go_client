// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月25日
// Update Time         :    2019年12月25日
// Class Description   :    CityGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoTemp;
using Client;
using System;
using System.Text;
using Client.FSM;
using Data;
using SprotoType;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Linq;
using Hotfix;

namespace Game
{
    public struct CameraParam
    {
        public float min_dxf;
        public float max_dxf;
        public float max_distance;
        public float min_distance;
        public CameraParam(float min_dxf, float max_dxf, float max_distance, float min_distance)
        {
            this.min_dxf = min_dxf;
            this.max_dxf = max_dxf;
            this.max_distance = max_distance;
            this.min_distance = min_distance;
        }
    }

    public class MapUIiconMyCityMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "MapUIiconMyCityMediator";
        GlobalViewLevelMediator m_mapViewLevelMgr;
        private MainInterfaceMediator mainInterfaceMediator;
        private WorldMgrMediator m_WorldMgrMediator;
        WorldMapObjectProxy m_worldMapObjectProxy;
        CityBuildingProxy m_cityBuildingProxy;
        TroopProxy m_troopProxy;
        ScoutProxy m_scoutProxy;

        private const int CAMERA_MOVE_TIME = 200;
        private const int speed = 100;
        private const float offset = 0.05f;
        private List<string> m_preLoadRes = new List<string>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private CityObjData m_cityObjData;
        private GameObject m_obj;
        private UI_Tip_Screen_SubView m_subView;
        private Camera m_camera;
        private int cursorTopPosition = 50;
        private int cursorBottomPosition = -50;

        Vector3 startUIPos = Vector3.zero;
        float screenW, screenH;
        Vector3 cursorPos;
        Vector2 posForDir = new Vector2(0.5f, 0.5f);
        private float left_offset;
        private float right_offset;
        private float y_offset;
        private Vector2 centerPos = new Vector2(0.5f, 0.5f);
        public MapUIRect HudRect;

        public MapUIRect QuestRect { get; private set; }
        public MapUIRect QuestRectFull { get; private set; }
        public MapUIRect NameRect { get; private set; }
        public MapUIRect[] ResRect { get; private set; }
        public MapUIRect NameWithCoordRect { get; private set; }
        public MapUIRect IapRect { get; private set; }
        public MapUIRect IapRectWithLostLand { get; private set; }
        public MapUIRect[] TroopBarRect { get; private set; }
        public MapUIRect RightBottomRect { get; private set; }
        public MapUIRect RightBottomWithAlertRect { get; private set; }
        public MapUIRect MoreBtnRect { get; private set; }
        public MapUIRect ChatRect { get; private set; }
        public MapUIRect SearchBtnRect { get; private set; }
        public MapUIRect CreateatmWinRect { get; private set; }
        public MapUIRect StrategicSelNoneRect { get; private set; }
        public MapUIRect StrategicSelRect { get; private set; }
        public MapUIRect StrategicRightBottomRect { get; private set; }
        public MapUIRect StrategicRightTopRect { get; private set; }
        public MapUIRect HUDHideCoordRect { get; private set; }
        public MapUIRect GVGScoreNormalRect { get; private set; }
        public MapUIRect GVGScoreDetailRect { get; private set; }
        public MapUIRect GVGGemRect { get; private set; }
        public MapUIRect GVGLeftBottomRect { get; private set; }
        public MapUIRect MailRect { get; private set; }
        public MapUIRect MapRect { get; private set; }
        public MapUIRect ActivityRect { get; private set; }
        public MapUIRect LodMenu { get; private set; }
        public MapUIRect QuestRect_AR { get; private set; }
        public MapUIRect QuestRectFull_AR { get; private set; }
        public MapUIRect NameRect_AR { get; private set; }
        public MapUIRect NameWithCoordRect_AR { get; private set; }
        public MapUIRect LodMenu_AR { get; private set; }
        public MapUIRect[] ResRect_AR { get; private set; }
        public MapUIRect[] TroopBarRect_AR { get; private set; }
        public MapUIRect MapRect_AR { get; private set; }
        public MapUIRect ActivityRectt_AR { get; private set; }
        public MapUIRect RightBottomRect_AR { get; private set; }
        public MapUIRect MoreBtnRect_AR { get; private set; }
        public MapUIRect MailRect_AR { get; private set; }
        public MapUIRect ChatRect_AR { get; private set; }
        public MapUIRect LeftBottomRect_AR { get; private set; }
        public MapUIRect CreateatmWinRect_AR { get; private set; }

        #region 界面打开情况
        public bool m_isFeatureBtnOn = false;
        public int TroopBarSize = 0;
        public bool m_isTaskAniOn = false;
        #endregion
        public CameraParam cameraParam;
        public bool m_assetReady = false;
        #endregion

        //IMediatorPlug needs
        public MapUIiconMyCityMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }

        public MapUIiconMyCityMediator(object viewComponent) : base(NameMediator, null)
        {
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.QuestBtnChange,
                CmdConstant.FeatureBtnChange,
                CmdConstant.MapViewChange,
                CmdConstant.OnShowUI,
                CmdConstant.OnCloseUI,
                CmdConstant.GameModeChanged,
                CmdConstant.WorldEditStateChanged,
                CmdConstant.NetWorkReconnecting,
                CmdConstant.ChangeRoleObjPos,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.QuestBtnChange:

                    if (notification.Body is bool)
                    {
                        m_isTaskAniOn = (bool)notification.Body;
                        SetCurViewPos();
                    }
                    break;
                case CmdConstant.FeatureBtnChange:
                    if (notification.Body is bool)
                    {
                        m_isFeatureBtnOn = (bool)notification.Body;
                        SetCurViewPos();
                    }
                    break;
                case CmdConstant.ChangeRoleObjPos:
                    SetCurViewPos();
                    break;
                case CmdConstant.MapViewChange:
                    {
                        MapViewLevel crrLevel = m_mapViewLevelMgr.GetViewLevel();
                        MapViewLevel preLevel = m_mapViewLevelMgr.GetPreMapViewLevel();
                        if (crrLevel != preLevel)
                        {
                            OnWorldViewChange();
                        }
                    }
                    break;
                case CmdConstant.NetWorkReconnecting:
                    NetWorkReconnecting();
                    break;
                case CmdConstant.OnShowUI:
                    {
                        OnShowUI(notification.Body as UIInfo);
                    }
                    break;
                case CmdConstant.OnCloseUI:
                    {
                        OnCloseUI(notification.Body as UIInfo);
                    }
                    break;
                case CmdConstant.WorldEditStateChanged:
                case CmdConstant.GameModeChanged:
                    {
                        OnGameModeChange();
                    }
                    break;
                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_mapViewLevelMgr = AppFacade.GetInstance().RetrieveMediator(GlobalViewLevelMediator.NameMediator) as GlobalViewLevelMediator;
            mainInterfaceMediator = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
            m_worldMapObjectProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_cityBuildingProxy = AppFacade.GetInstance().RetrieveProxy(CityBuildingProxy.ProxyNAME) as CityBuildingProxy;
            m_troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_scoutProxy = AppFacade.GetInstance().RetrieveProxy(ScoutProxy.ProxyNAME) as ScoutProxy;
            m_cityObjData = m_cityBuildingProxy.MyCityObjData;
            m_camera = WorldCamera.Instance().GetCamera();
            screenW = CoreUtils.uiManager.GetUILayer((int)UILayer.FullViewLayer).rect.width;
            screenH = CoreUtils.uiManager.GetUILayer((int)UILayer.FullViewLayer).rect.height;
            InitUIPolygonPoint();
            float cameraDxfInit = WorldCamera.Instance().getCameraDxf("init");
            cameraParam = new CameraParam(WorldCamera.Instance().customMinDxf, cameraDxfInit, 22f, 5f);

        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
            CoreUtils.assetService.LoadAssetAsync<GameObject>("UI_Tip_Screen", (asset) =>
            {
                GameObject obj = asset.asset() as GameObject;
                if (obj != null)
                {
                    m_assetDic[obj.name] = obj;
                }
                m_assetReady = true;
            });
            //地图坐标变化绑定
            WorldCamera.Instance().AddViewChange(OnWorldViewChange);
        }

        public override void Update()
        {
        }

        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void OnRemove()
        {
            WorldCamera.Instance().RemoveViewChange(OnWorldViewChange);
            if (m_obj != null)
            {
                CoreUtils.assetService.Destroy(m_obj);
                m_obj = null;
            }
        }

        /// <summary>
        /// 重连清数据
        /// </summary>
        public void NetWorkReconnecting()
        {
            m_cityObjData = null;
         }


        #endregion

        private void OnGameModeChange()
        {
            OnWorldViewChange();
        }

        private void OnWorldViewChange(float x, float y, float dxf)
        {
            ViewPosx = x;
            ViewPosy = y;
            SetCurViewPos(ViewPosx, ViewPosy);
        }
        private void OnWorldViewChange()
        {
            ViewPosx = WorldCamera.Instance().GetViewCenter().x;
            ViewPosy = WorldCamera.Instance().GetViewCenter().y;
            SetCurViewPos(ViewPosx, ViewPosy);
        }
        private bool IsInStartegic()
        {
            return MapViewLevel.TacticsToStrategy_1 <= m_mapViewLevelMgr.GetViewLevel();
        }
        private void Show()
        {
        }

        private void RefreshCursorObjDelay()
        {

        }

        private void RefreshCursorObjImmediate()
        {

        }
        private void OnClickIcon()
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.CancelCameraFollow);
            WorldCamera.Instance().ViewTerrainPos(m_cityBuildingProxy.RolePos.x, m_cityBuildingProxy.RolePos.y, 500, () => { });
            CoreUtils.assetService.Destroy(m_obj);
        }

        private void InitUIPolygonPoint()//绘制矩形边界
        {
            left_offset = 0.03f;
            right_offset = 0.03f;

            y_offset = left_offset * (screenW / screenH);
            HudRect = new MapUIRect(0 + left_offset, 0 + y_offset, 1 - right_offset, 1 - y_offset);

            QuestRect = new MapUIRect(0 + left_offset, 1 - y_offset - 220 / screenH, 0 + left_offset + 100 / screenW, 1 - y_offset - 137 / screenH);//任务缩进状态
            QuestRectFull = new MapUIRect(0 + left_offset, 1 - y_offset - 500 / screenH, 0 + left_offset + 215 / screenW, 1 - y_offset - 137 / screenH);//任务展开状态
            NameRect = new MapUIRect(0 + left_offset, 1 - y_offset - 124 / screenH, 0 + left_offset + 300 / screenW, 1 - y_offset); //UI_Item_PlayerPowerInfo
            NameWithCoordRect = new MapUIRect(0 + left_offset, 1 - y_offset - 80 / screenH, 0 + left_offset + 540 / screenW, 1 - y_offset); //UI_Item_Position+UI_Item_PlayerPowerInfo
            LodMenu = new MapUIRect(0 + left_offset, 1 - y_offset - 450 / screenH, 0 + left_offset + 167 / screenW, 1 - y_offset - 137 / screenH); //UI_Item_LodMenu
            ResRect = new[] {
            new MapUIRect (1 - (right_offset + 390 / screenW), 1 - y_offset - 52 / screenH, 1 - right_offset, 1 - y_offset),
            new MapUIRect (1 - (right_offset + 510 / screenW), 1 - y_offset - 52 / screenH, 1 - right_offset, 1 - y_offset),
            new MapUIRect (1 - (right_offset + 730 / screenW), 1 - y_offset - 52 / screenH, 1 - right_offset, 1 - y_offset),
        };
            TroopBarRect = new[] {
            new MapUIRect(),
            new MapUIRect (  1 - right_offset - 116 / screenW, 1 - (y_offset + 467 / screenH), 1 - right_offset, 1 - y_offset - 177/ screenH ),
            new MapUIRect (  1 - right_offset - 116 / screenW, 1-  (y_offset + 467 / screenH), 1 - right_offset, 1 - y_offset - 177 / screenH),
            new MapUIRect (  1 - right_offset - 116 / screenW, 1- (y_offset + 467 / screenH), 1 - right_offset, 1 - y_offset - 177/ screenH),
        };
            MapRect = new MapUIRect(1 - (right_offset + 220 / screenW), 1 - y_offset - 150 / screenH, 1 - right_offset, 1 - y_offset);//小地图
            ActivityRect = new MapUIRect(1 - (right_offset + 200 / screenW), 1 - y_offset - 178 / screenH, 1 - right_offset, 1 - y_offset);//活动充值按钮
            RightBottomRect = new MapUIRect(1 - right_offset - 90 / screenW, 0 + y_offset, 1 - right_offset, 0 + y_offset + 124 / screenH);//不展开的右下角
            MoreBtnRect = new MapUIRect(1 - (right_offset + 576 / screenW), 0 + y_offset, 1 - right_offset, 0 + y_offset + 124 / screenH);//展开的右下角
            MailRect = new MapUIRect(1 - right_offset - 90 / screenW, 0 + y_offset + 124 / screenH, 1 - right_offset, 0 + y_offset + 200 / screenH);//邮件
            ChatRect = new MapUIRect(0 + left_offset, 0 + y_offset, 0 + (left_offset + 550 / screenW), 0 + y_offset + 80 / screenH);//聊天
            SearchBtnRect = new MapUIRect(0 + left_offset, 0 + y_offset, 0 + (left_offset + 100 / screenW), 0 + y_offset + 240 / screenH);//搜索按钮
            CreateatmWinRect = new MapUIRect(1 - right_offset - 142 / screenW, 0.5f +384 / screenH, 1 - right_offset, 0.5f  - 384 / screenH);//选择队列
            //------------------------------------------------------------------------------------------------------------------------------------------------
            QuestRect_AR = new MapUIRect(1 - left_offset, 1 - y_offset - 220 / screenH, 1 - (left_offset + 100 / screenW), 1 - y_offset - 137 / screenH);//任务缩进状态
            QuestRectFull_AR = new MapUIRect(1 - left_offset, 1 - y_offset - 500 / screenH, 1 - (left_offset + 215 / screenW), 1 - y_offset - 137 / screenH);//任务展开状态
            NameRect_AR = new MapUIRect(1 - left_offset, 1 - y_offset - 124 / screenH, 1 - (left_offset + 300 / screenW), 1 - y_offset); //UI_Item_PlayerPowerInfo
            NameWithCoordRect_AR = new MapUIRect(1 - left_offset, 1 - y_offset - 80/ screenH, 1 - (left_offset + 540 / screenW), 1 - y_offset); //UI_Item_Position+UI_Item_PlayerPowerInfo
            LodMenu_AR = new MapUIRect(1 - left_offset, 1 - y_offset - 450 / screenH, 1 - (left_offset + 167 / screenW), 1 - y_offset - 137 / screenH); //UI_Item_LodMenu
            ResRect_AR = new[] {
            new MapUIRect (0+( right_offset + 390 / screenW), 1 - y_offset - 52 / screenH, 0+ right_offset, 1 - y_offset),
            new MapUIRect (0+( right_offset + 510 / screenW), 1 - y_offset - 52 / screenH, 0+  right_offset, 1 - y_offset),
            new MapUIRect (0+( right_offset + 730 / screenW), 1 - y_offset - 52 / screenH, 0+  right_offset, 1 - y_offset),
        };
            TroopBarRect_AR = new[] {
            new MapUIRect(),
            new MapUIRect (  0+ ( right_offset +116 / screenW), 1-( y_offset + 467 / screenH), 0+ right_offset, 1 - y_offset - 177 / screenH ),
            new MapUIRect (  0+( right_offset + 116 / screenW), 1- ( y_offset + 467 / screenH),0+ right_offset, 1 - y_offset - 177 / screenH),
            new MapUIRect (  0+(  right_offset + 116 / screenW), 1- ( y_offset + 467/ screenH), 0+right_offset, 1 - y_offset - 177/ screenH),
        };
            MapRect_AR = new MapUIRect(0 + (right_offset + 220 / screenW), 1 - y_offset - 150 / screenH, 0 + right_offset, 1 - y_offset);//小地图
            ActivityRectt_AR = new MapUIRect(0 + (right_offset + 200 / screenW), 1 - y_offset - 178 / screenH, 0+ right_offset, 1 - y_offset);//活动充值按钮
            RightBottomRect_AR = new MapUIRect(0 + (right_offset + 90 / screenW), 0 + y_offset, 0 + right_offset, 0 + y_offset + 124 / screenH);//不展开的右下角
            MoreBtnRect_AR = new MapUIRect(0 + right_offset + 576 / screenW, 0 + y_offset, 0 + right_offset, 0 + y_offset + 124 / screenH);//展开的右下角
            MailRect_AR = new MapUIRect(0 + (right_offset + 90 / screenW), 0 + y_offset + 124 / screenH, 0 + right_offset, 0 + y_offset + 200 / screenH);//邮件
            ChatRect_AR = new MapUIRect(1 - left_offset, 0 + y_offset, 1 - (left_offset + 550 / screenW), 0 + y_offset + 80 / screenH);//聊天
            LeftBottomRect_AR = new MapUIRect(1 - left_offset, 0 + y_offset, 1 - (left_offset + 100 / screenW), 0 + y_offset + 240 / screenH);//搜索按钮
            CreateatmWinRect_AR = new MapUIRect(0 + (right_offset + 142 / screenW), 0.5f + 384 / screenH, 0 + right_offset, 0.5f - 384 / screenH);//选择队列
            if (LanguageUtils.IsArabic())
            {
                QuestRect = QuestRect_AR;
                QuestRectFull = QuestRectFull_AR;
                NameRect = NameRect_AR;
                NameWithCoordRect = NameWithCoordRect_AR;
                LodMenu = LodMenu_AR;
                ResRect = ResRect_AR;
                TroopBarRect = TroopBarRect_AR;
                MapRect = MapRect_AR;
                RightBottomRect = RightBottomRect_AR;
                MoreBtnRect = MoreBtnRect_AR;
                MailRect = MailRect_AR;
                ChatRect = ChatRect_AR;
                SearchBtnRect = LeftBottomRect_AR;
                ActivityRect = ActivityRectt_AR;
                CreateatmWinRect = CreateatmWinRect_AR;
            }
        }

        /// <summary>
        /// 距离不够不显示主城头像
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool NeedShowHomeIcon(float distance, float x, float y)
        {
            if (GameModeManager.Instance.CurGameMode != GameModeType.World) return false;
            if (!Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), m_cityObjData.pos.x, m_cityObjData.pos.y, String.Empty))
            {
                if (distance >= 1)
                {
                    return true;
                }
            }
            return false;
        }

        private float ViewPosx = 0;
        private float ViewPosy = 0;
        private void SetCurViewPos()
        {
            SetCurViewPos(ViewPosx, ViewPosy);
        }
        private void ClearDistanceIcon()
        {
            if (m_obj != null)
            {
                CoreUtils.assetService.Destroy(m_obj);
                m_obj = null;
            }
        }
        /// <summary>
        /// 更新图标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetCurViewPos(float x, float y)
        {
            if (m_cityObjData == null)
            {
                m_cityObjData = m_cityBuildingProxy.MyCityObjData;
                ClearDistanceIcon();
                return;
            }
            if (m_cityObjData.go == null)
            {
                ClearDistanceIcon();
                return;
            }
            if (!m_assetReady)
            {
                ClearDistanceIcon();
                return;
            }
            if (m_WorldMgrMediator == null)
            {
                m_WorldMgrMediator =
                    AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            }

            if (!m_WorldMgrMediator.IsWorldMapStateNormal())
            {
                ClearDistanceIcon();
                return;
            }
            if (GameModeManager.Instance.CurGameMode != GameModeType.World)
            {
                ClearDistanceIcon();
                return;
            }
            float distance = (Vector2.Distance(m_cityObjData.pos, new Vector2(x, y))) / 10;
            if (NeedShowHomeIcon(distance, x, y))
            {
                if (m_obj == null)
                {
                  //  Debug.LogErrorFormat("x={0},y={1}", x, y);
                    m_obj = CoreUtils.assetService.Instantiate(m_assetDic["UI_Tip_Screen"]);
                    m_obj.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.FullViewLayer));
                    m_obj.transform.localScale = new Vector3(1, 1, 1);
                    m_subView = new UI_Tip_Screen_SubView(m_obj.GetComponent<RectTransform>());
                    m_subView.AddClickEvent(() =>
                    {
                        OnClickIcon();
                    });
                    m_obj.gameObject.SetActive(false);
                }
                if (mainInterfaceMediator == null)
                {
                    mainInterfaceMediator = AppFacade.GetInstance().RetrieveMediator(MainInterfaceMediator.NameMediator) as MainInterfaceMediator;
                }
                if (mainInterfaceMediator == null)
                {
                    return;
                }

                TroopBarSize = Mathf.Min(3, m_troopProxy.GetArmyCount() + m_scoutProxy.GetActiveScountCount());
                startUIPos = WorldCamera.Instance().GetCamera().WorldToViewportPoint(m_cityObjData.go.transform.position);
                if (startUIPos.x > 0 && startUIPos.x < 1 && startUIPos.y > 0 && startUIPos.y < 1 && m_obj.activeSelf)
                {
                    m_obj.SetActive(false);
                    return;
                }
                if (startUIPos.z <= 0)
                {
                    startUIPos.y = -startUIPos.y;
                    startUIPos.x = -startUIPos.x;
                }
                if (!CoreUtils.uiManager.ExistUI(UI.s_openFogShow) && !m_obj.activeSelf)
                {
                    m_obj.SetActive(true);
                }

                Vector2 iv2 = GetCursorPos(startUIPos.x, startUIPos.y);

                Vector2 cursorPos = new Vector2(iv2.x, iv2.y);
                Vector2 screenPos = new Vector2(cursorPos.x * screenW, cursorPos.y * screenH);
                m_obj.GetComponent<RectTransform>().anchoredPosition = screenPos;
                //  Debug.LogErrorFormat("startUIPos:{0},cursorPos:{1}screenPos:{2}", startUIPos, cursorPos, screenPos);
                m_subView.m_img_bg_PolygonImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, screenPos.y <= 50 ? cursorTopPosition : cursorBottomPosition);
                Vector2 dir = (iv2 - posForDir).normalized;
                double theta = Math.Atan2(dir.y, dir.x);
                double theta1 = Math.Atan2(dir.y, 0);
                if (theta <= 0)
                {
                    theta = theta + 2 * Math.PI;
                }

                m_subView.m_img_arrow_PolygonImage.transform.localEulerAngles = new Vector3(0, 0, (float)((180 / Math.PI) * theta + 90));
                m_subView.m_lbl_languageText_LanguageText.text = LanguageUtils.getTextFormat(300220, (int)distance);
            }
            else
            {
                ClearDistanceIcon();
            }
        }
        /// <summary>
        /// 图标最终位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Vector2 GetCursorPos(float x, float y)
        {
            MapViewLevel viewLevel = m_mapViewLevelMgr.GetViewLevel();
            Vector2 point = new Vector2(x, y);
            if (CoreUtils.uiManager.ExistUI(UI.s_createMainTroops))
            {
                point = GetCursorPosWin(point);
            }
            else
            {
                if (viewLevel <= MapViewLevel.Tactical)
                {
                    point = GetCursorPosCity(point);
                }
                else if (viewLevel <= MapViewLevel.TacticsToStrategy_2)
                {
                    point = GetCursorPosToStrategy(point);
                }
                else
                {
                    point = GetCursorPosContinental(point);
                }

            }
         
            return point;
        }
        private Vector2 GetCursorPosCity(Vector2 point)
        {
            Vector2 p1 = Vector2.zero;
            Vector2 p2 = Vector2.zero;
            Vector2 result = new Vector2();
            List<Vector2> temp = null;
            result = point;
            result = point;
            bool left = true;
            if (LanguageUtils.IsArabic())
            {
                if (point.x < centerPos.x)
                {
                    left = false;
                }
            }
            else
            {
                if (point.x < centerPos.x)
                {
                    left = true;
                }
                else
                {
                    left = false;
                }
            }
            if (left)
            {
                if (mainInterfaceMediator.m_moduleState.ContainsKey(EnumMainModule.Position))
                {
                    if (mainInterfaceMediator.m_moduleState[EnumMainModule.Position])
                    {
                        temp = MathUtil.RectIntersectSegment(NameWithCoordRect, point, centerPos);

                        if (temp.Count == 2)
                        {
                            p1 = temp[0];
                            p2 = temp[1];
                            result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                    else
                    {
                        temp = MathUtil.RectIntersectSegment(NameRect, point, centerPos);
                        if (temp.Count == 2)
                        {
                            p1 = temp[0];
                            p2 = temp[1];
                            result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                }
                if (!m_isTaskAniOn)
                {
                    temp = MathUtil.RectIntersectSegment(QuestRectFull, point, centerPos);

                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                }
                else
                {
                    temp = MathUtil.RectIntersectSegment(QuestRect, point, centerPos);

                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                }


                temp = MathUtil.RectIntersectSegment(SearchBtnRect, point, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
                temp = MathUtil.RectIntersectSegment(ChatRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }

                temp = MathUtil.RectIntersectSegment(HudRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
            }
            else
            {
                if (TroopBarSize > 0)
                {
                    temp = MathUtil.RectIntersectSegment((TroopBarRect)[TroopBarSize], point, centerPos);
                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                }
                if (point.y < centerPos.y)
                {
                    if (m_isFeatureBtnOn)
                    {
                        temp = MathUtil.RectIntersectSegment(RightBottomRect, point, centerPos);
                        if (temp.Count == 2)
                        {
                            p1 = temp[0];
                            p2 = temp[1];
                            result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                    else
                    {
                        temp = MathUtil.RectIntersectSegment(MoreBtnRect, point, centerPos);
                        if (temp.Count == 2)
                        {
                            p1 = temp[0];
                            p2 = temp[1];
                            result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                }
                else
                {
                    temp = MathUtil.RectIntersectSegment(ResRect[2], point, centerPos);
                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                    temp = MathUtil.RectIntersectSegment(ActivityRect, result, centerPos);
                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                }
                temp = MathUtil.RectIntersectSegment(MailRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
                temp = MathUtil.RectIntersectSegment(HudRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
            }
            return result;
        }
        private Vector2 GetCursorPosToStrategy(Vector2 point)
        {
            Vector2 p1 = Vector2.zero;
            Vector2 p2 = Vector2.zero;
            Vector2 result = new Vector2();
            List<Vector2> temp = null;
            result = point;
            bool left = true;
            if (LanguageUtils.IsArabic())
            {
                if (point.x < centerPos.x)
                {
                    left = false;
                }
            }
            else
            {
                if (point.x < centerPos.x)
                {
                    left = true;
                }
                else
                {
                    left = false;
                }
            }
            if (left)
            {
                if (mainInterfaceMediator.m_moduleState.ContainsKey(EnumMainModule.Position))
                {
                    if (mainInterfaceMediator.m_moduleState[EnumMainModule.Position])
                    {
                        temp = MathUtil.RectIntersectSegment(NameWithCoordRect, point, centerPos);

                        if (temp.Count == 2)
                        {
                            p1 = temp[0];
                            p2 = temp[1];
                            result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                    else
                    {
                        temp = MathUtil.RectIntersectSegment(NameRect, point, centerPos);
                        if (temp.Count == 2)
                        {
                            p1 = temp[0];
                            p2 = temp[1];
                            result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                }
                temp = MathUtil.RectIntersectSegment(SearchBtnRect, point, centerPos);

                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
                temp = MathUtil.RectIntersectSegment(ChatRect, result, centerPos);

                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }

                temp = MathUtil.RectIntersectSegment(HudRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
            }
            else
            {
                if (TroopBarSize > 0)
                {
                    temp = MathUtil.RectIntersectSegment((TroopBarRect)[TroopBarSize], point, centerPos);
                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                }
                temp = MathUtil.RectIntersectSegment(MapRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
                temp = MathUtil.RectIntersectSegment(HudRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
            }
            return result;
        }
        private Vector2 GetCursorPosContinental(Vector2 point)
        {
            Vector2 p1 = Vector2.zero;
            Vector2 p2 = Vector2.zero;
            Vector2 result = new Vector2();
            List<Vector2> temp = null;
            result = point;
            result = point;
            bool left = true;
            if (LanguageUtils.IsArabic())
            {
                if (point.x < centerPos.x)
                {
                    left = false;
                }
            }
            else
            {
                if (point.x < centerPos.x)
                {
                    left = true;
                }
                else
                {
                    left = false;
                }
            }
            if (left)
            {
                if (mainInterfaceMediator.m_moduleState.ContainsKey(EnumMainModule.Position))
                {
                    if (mainInterfaceMediator.m_moduleState[EnumMainModule.Position])
                    {
                        temp = MathUtil.RectIntersectSegment(NameWithCoordRect, point, centerPos);

                        if (temp.Count == 2)
                        {
                            p1 = temp[0];
                            p2 = temp[1];
                            result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                    else
                    {
                        temp = MathUtil.RectIntersectSegment(NameRect, point, centerPos);
                        if (temp.Count == 2)
                        {
                            p1 = temp[0];
                            p2 = temp[1];
                            result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                        }
                    }
                }
                temp = MathUtil.RectIntersectSegment(LodMenu, point, centerPos);

                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }

                temp = MathUtil.RectIntersectSegment(SearchBtnRect, point, centerPos);

                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
                temp = MathUtil.RectIntersectSegment(ChatRect, result, centerPos);

                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
                temp = MathUtil.RectIntersectSegment(HudRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
            }
            else
            {
                if (TroopBarSize > 0)
                {
                    temp = MathUtil.RectIntersectSegment((TroopBarRect)[TroopBarSize], point, centerPos);
                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                }
                if (point.y < centerPos.y)
                {
                    temp = MathUtil.RectIntersectSegment(RightBottomRect, point, centerPos);
                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                }
                else
                {
                }
                temp = MathUtil.RectIntersectSegment(MapRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
                temp = MathUtil.RectIntersectSegment(HudRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
            }
            return result;
        }
        private Vector2 GetCursorPosWin(Vector2 point)
        {
            Vector2 p1 = Vector2.zero;
            Vector2 p2 = Vector2.zero;
            Vector2 result = new Vector2();
            List<Vector2> temp = null;
            result = point;
            result = point;
            bool left = true;
            if (LanguageUtils.IsArabic())
            {
                if (point.x < centerPos.x)
                {
                    left = false;
                }
            }
            else
            {
                if (point.x < centerPos.x)
                {
                    left = true;
                }
                else
                {
                    left = false;
                }
            }
            if (left)
            {
                temp = MathUtil.RectIntersectSegment(HudRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
            }
            else
            {
                
                    temp = MathUtil.RectIntersectSegment(CreateatmWinRect, point, centerPos);
                    if (temp.Count == 2)
                    {
                        p1 = temp[0];
                        p2 = temp[1];
                        result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                    }
                temp = MathUtil.RectIntersectSegment(HudRect, result, centerPos);
                if (temp.Count == 2)
                {
                    p1 = temp[0];
                    p2 = temp[1];
                    result = MathUtil.FindClosestPoint(p1.x, p1.y, p2.x, p2.y);
                }
            }
            return result;
        }


        private void OnShowUI(UIInfo ui)
        {
            if (ui == UI.s_openFogShow)
            {
                if (m_obj != null)
                {
                    m_obj.SetActive(false);
                }
            }
        }

        private void OnCloseUI(UIInfo ui)
        {
            if (ui == UI.s_openFogShow)
            {
                if (m_obj != null)
                {
                    m_obj.SetActive(true);
                }
            }
        }
    }
}
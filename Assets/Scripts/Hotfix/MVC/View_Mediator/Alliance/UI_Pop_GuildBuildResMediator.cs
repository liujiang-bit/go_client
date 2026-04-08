// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 21, 2020
// Update Time         :    Tuesday, April 21, 2020
// Class Description   :    UI_Pop_GuildBuildResMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using Hotfix.Utils;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {


    
    public class UI_Pop_GuildBuildResMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_GuildBuildResMediator";

        
        private AllianceBuildingTypeDefine config;
        private AllianceProxy m_allianceProxy;

        private WorldMapObjectProxy m_worldProxy;


        private GameObject buildModelObj;
        private Vector3 buildModelPosition = Vector3.one;

        private GameObject buildRadiusObj;

        private GameObject buildBgObj;

        private GameObject forbiddenMeshModelObj;

        private Vector3 buildModelAlignPosition;
        private Vector3 preCheckBuildPos;
        
        private Transform m_root;
        private const string m_root_path = "SceneObject/rss_root";

        private bool isDrag = false;
        private bool isDragModel = false;
        private bool canBuild = false;

        private Vector3 touchStartTerrainPos;
        private Vector3 touchStartModelPos;
        private Vector2 dragTouchPos;
        
        private FogSystemMediator m_fogMediator;

        private Vector2 touchPos;
        
        private AllianceBuildingDataDefine m_buildData;

        
        #endregion

        //IMediatorPlug needs
        public UI_Pop_GuildBuildResMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_GuildBuildResView view;

        private WorldMgrMediator m_worldMgrMediator;

        private UI_Pop_GuildBuildMediator m_createMd;

        private bool m_isFirstInit = true;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.OnTouche3D,
                CmdConstant.OnTouche3DBegin,
                CmdConstant.OnTouche3DEnd,
                CmdConstant.OnTouche3DReleaseOutside,
                Guild_CheckGuildBuildCreate.TagName,
                CmdConstant.AllianceBlock,
                CmdConstant.AllianceDepot,
                CmdConstant.NetWorkReconnecting
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {

                case CmdConstant.NetWorkReconnecting:
                {
                    DelBuildModel();
                    getBuildMediator().ExitCreateMode();
                }
                    break;
                case CmdConstant.OnTouche3D:
                {
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3D(touche3DData.x, touche3DData.y, touche3DData.parentName, touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.OnTouche3DBegin:
                {
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3DBegin(touche3DData.x, touche3DData.y, touche3DData.parentName,
                            touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.OnTouche3DEnd:
                {
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3DEnd(touche3DData.x, touche3DData.y, touche3DData.parentName,
                            touche3DData.colliderName);
                    }
                }
                    break;
                case CmdConstant.OnTouche3DReleaseOutside:
                {
                    if (notification.Body is Touche3DData)
                    {
                        Touche3DData touche3DData = (Touche3DData) notification.Body;
                        OnTouche3DReleaseOutside(touche3DData.x, touche3DData.y, touche3DData.parentName,
                            touche3DData.colliderName);
                    }
                }
                    break;
                case Guild_CheckGuildBuildCreate.TagName:
                    
                    
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;

                        
                        if ((ErrorCode)error.errorCode!= ErrorCode.GUILD_CURRENCY_NOT_ENOUGH && (ErrorCode)error.errorCode!= ErrorCode.GUILD_POINT_NOT_ENOUGH)
                        {
                            //Debug.Log("server "+isDragModel);
                            if (isDragModel==false && isDrag==false)
                            {
                                ErrorCodeHelper.ShowErrorCodeTip(error);
                            }
                            SetCanBuild(false);
                        }
                        
                       
                    }
                    else
                    {
                        
                        Guild_CheckGuildBuildCreate.response response =
                            notification.Body as Guild_CheckGuildBuildCreate.response;

                        if (response.result==false)
                        {
                            
                            SetCanBuild(false);
                        }
                    }

                    
                    
                    break;
                case CmdConstant.AllianceBlock:

                    if (m_isFirstInit)
                    {
                        CheckCanBuild(true);
                        m_isFirstInit = false;
                    }
                    
                    break;
                case  CmdConstant.AllianceDepot:

                    UpdateRess(0);
                    break;
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){

            if (m_timeTick!=null)
            {
                m_timeTick.Cancel();
                m_timeTick = null;
            }
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        
        
        public static long[] CurrencyIDs = new[] {107L, 108, 109, 110, 111};
        private Dictionary<long,GuildCurrencyInfoEntity> m_depotCurrencyInfo;
        
        private UI_Model_GuildRes_SubView[] m_subViews = new UI_Model_GuildRes_SubView[5];
        protected override void InitData()
        {
           
            
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;

            m_worldMgrMediator =
                AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            
            
            m_fogMediator = AppFacade.GetInstance().RetrieveMediator(FogSystemMediator.NameMediator) as FogSystemMediator;
            
            config = view.data as AllianceBuildingTypeDefine;
            
            m_depotCurrencyInfo = m_allianceProxy.GetDepotCurrencyInfoEntities();

            
            int count = 1;

            if (config.type == 3) //旗帜个数
            {
                count = (int) m_allianceProxy.GetFlagInfoEntity().flagNum + 1;
            }
            
            m_buildData = CoreUtils.dataService.QueryRecord<AllianceBuildingDataDefine>(config.type * 10000+count);
//            view.m_pl_res1.setNum(m_buildData.fund);
//            view.m_pl_res2.setNum(m_buildData.food);
//            view.m_pl_res3.setNum(m_buildData.wood);
//            view.m_pl_res4.setNum(m_buildData.stone);
//            view.m_pl_res5.setNum(m_buildData.coin);

            m_subViews[0] = view.m_pl_res1;
            m_subViews[1] = view.m_pl_res2;
            m_subViews[2] = view.m_pl_res3;
            m_subViews[3] = view.m_pl_res4;
            m_subViews[4] = view.m_pl_res5;

        }

        private UI_Pop_GuildBuildMediator getBuildMediator()
        {
            if (m_createMd==null)
            {
                m_createMd =
                    AppFacade.GetInstance().RetrieveMediator(UI_Pop_GuildBuildMediator.NameMediator) as UI_Pop_GuildBuildMediator;
            }

            return m_createMd;
        }

        protected override void BindUIEvent()
        {
            m_worldMgrMediator.SetWorldMapState(WorldEditState.GuildBuildCreate);
        }

        protected override void BindUIData()
        {
            CoreUtils.inputManager.AddTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);

            var cconfig = CoreUtils.dataService
                .QueryRecord<AllianceSignDefine>((int)m_allianceProxy.GetAlliance().signs[1]);
            
            m_color = ClientUtils.StringToHtmlColor(cconfig.colour);
            
            CreateForbidenMesh();
            CreateBuildModels();

            UpdateRess(0);
        }
        
        
        private Timer m_timeTick;
        private float preTime = -2;

        private void UpdateEnd()
        {
        }

        public void UpdateRess(float time)
        {
            
            m_depotCurrencyInfo = m_allianceProxy.GetDepotCurrencyInfoEntities();
            if (time-preTime>1 && m_depotCurrencyInfo.Count > 0)
            {
                preTime = time;
                if (m_timeTick == null)
                {
                    m_timeTick = Timer.Register(1000,UpdateEnd,UpdateRess,true);
                }
                
                for (int i = 0; i < m_subViews.Length; i++)
                {
                    var subView = m_subViews[i];
                
                    var info = m_depotCurrencyInfo[CurrencyIDs[i]];

                    
                    subView.m_lbl_resnum_LanguageText.text =ClientUtils.CurrencyFormat(m_allianceProxy.GetCurrenc(info));
                   
                }
            }
        }
        

        private void OnTouche3DBegin(int x, int y, string parentName, string colliderName)
        {
        }

        private void OnTouche3D(int x, int y, string parentName, string colliderName)
        {
        }

        private void OnTouche3DEnd(int x, int y, string parentName, string colliderName)
        {
        }

        private void OnTouche3DReleaseOutside(int x, int y, string parentName, string colliderName)
        {
            //Debug.Log("OnTouche3DReleaseOutside"+parentName+"  "+colliderName);
        }


        private void OnTouchBegan(int x, int y)
        {
            if (buildModelObj == null)
            {
                return;
            }
            
            isDrag = true;

            isDragModel = !isDrag;
            
            touchPos = new Vector2(x,y);

            if (buildModelObj!=null)
            {
                var terrainPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);
                float radius = 2 * 3;
                var modelPos = buildModelObj.transform.position;
                var distSq = Vector3.Distance(modelPos, terrainPos);

                if (distSq < radius)
                {
                    isDragModel = true;
                    touchStartTerrainPos = terrainPos;
                    touchStartModelPos = modelPos;
                    dragTouchPos = new Vector2(x,y);
                    
                    WorldCamera.Instance().SetCanDrag(false);
                    WorldCamera.Instance().SetCanClick(false);
                    
                    getBuildMediator().setVisbleUI(false);
                }
            }

            if (!isDragModel && getBuildMediator()!=null)
            {
                getBuildMediator().UpdatePopPos();
            }
        }

        private void OnTouchMoved(int x, int y)
        {
            if (! isDrag && buildModelObj == null)
            {
                return;
            }

            if (isDragModel)
            {
                dragTouchPos = new Vector2(x,y);
                var newTerrainPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);

                if (newTerrainPos!= touchStartTerrainPos)
                {
                    var distVec = newTerrainPos - touchStartTerrainPos;
                    var newPos = touchStartModelPos + distVec;
                    if (buildModelObj!=null)
                    {
                        var oldPos = buildModelAlignPosition;
                        
                        SetCityModelPos(newPos);
                        
                        m_worldMgrMediator.OnWorldViewChange(newPos.x,newPos.z,WorldCamera.Instance().getCurrentCameraDxf());

                        CheckCanBuild();
                      
                        if (oldPos != buildModelAlignPosition)
                        {
                            //TODO 播放音效
                        }
                    }
                }
            }
            else
            {
                if (buildModelObj != null)
                {
                    var modelPos = buildModelObj.transform.position;

                    Vector2 screenPos = WorldCamera.Instance().GetCamera().WorldToViewportPoint(modelPos);
                    float edgeRateX = 0.05f;
                    float edgeRateY = 0.04f;

                    var targetScreenPos = new Vector2(screenPos.x, screenPos.y);
                    if (screenPos.x < edgeRateX)
                    {
                        targetScreenPos.x = edgeRateX;
                    }
                    else {
                        if(1 - edgeRateX < screenPos.x)
                        {
                            targetScreenPos.x = 1 - edgeRateX;
                        }
                    }

                    if (screenPos.y < edgeRateY)
                    {
                        targetScreenPos.y = edgeRateY;
                    }

                    else
                    {
                        if (1 - edgeRateY < screenPos.y)
                        {
                            targetScreenPos.y = 1 - edgeRateY;
                        }
                    }
                    
                    if (targetScreenPos != screenPos)
                    {
                        float screenWidth = Screen.safeArea.width;
                        float screenHeight = Screen.safeArea.height;
                        float nx = screenWidth * targetScreenPos.x;
                        float ny = screenHeight * targetScreenPos.y;
                        var newTerrainPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), nx, ny);
                        SetCityModelPos(newTerrainPos); 
                        if (getBuildMediator()!=null)
                        {
                            getBuildMediator().UpdatePopPos();
                        }
                        
                        CheckCanBuild();
                    }

                    if (getBuildMediator()!=null)
                    {
                        getBuildMediator().UpdatePopPos();
                    }
                }
            }
            
        }

        private void OnTouchEnded(int x, int y)
        {

            if (buildModelObj == null)
            {
                return;
            }
            
            var endPos = new Vector2(x,y);
            
            
            WorldCamera.Instance().SetCanDrag(true);
            WorldCamera.Instance().SetCanClick(true);
            
            if (Vector2.Distance(touchPos,endPos)<10)
            {
                if (getBuildMediator()!=null)
                {
                    getBuildMediator().ExitCreateMode();
                }
                return;
            }
            
            var newPos = WorldCamera.Instance().GetTouchTerrainPos(WorldCamera.Instance().GetCamera(), x, y);

            m_worldMgrMediator.OnWorldViewChange(newPos.x,newPos.z,WorldCamera.Instance().getCurrentCameraDxf());
          
            
            isDrag = false;
            isDragModel = false;
            
            
            SetCityModelPos(buildModelAlignPosition);
            
            
            if (getBuildMediator()!=null)
            {
                getBuildMediator().setVisbleUI(true);
            }
            CheckCanBuild(true);


        }

        private float m_lastSendServerCheck = -1;

        public void CheckCanBuild(bool isMouseUp=false)
        {
            bool HasMapCollode = m_worldMgrMediator.HasCollideMapObject();

//                Debug.Log(HasMapCollode+"CheckCanBuild"+isMouseUp+"  "+buildModelAlignPosition+"  "+preCheckBuildPos);

            SetCanBuild(!HasMapCollode);
            
            var pos = buildModelAlignPosition;

            if (pos != preCheckBuildPos|| isMouseUp)
            {
                
                preCheckBuildPos = buildModelAlignPosition;
                
                if (m_fogMediator.HasFogAtWorldPos(pos.x, pos.z))
                {
                    SetCanBuild(false,isMouseUp);

                    if (isMouseUp)
                    {
                        Tip.CreateTip(732016).Show();
                    }
                    return;
                }

               
                
                if (HasMapCollode)
                {
                    SetCanBuild(false,isMouseUp);
                    if (isMouseUp)
                    {
                        Tip.CreateTip(732011).Show();
                    }
                    return;
                }
                
                //todo other server check  local mesh check
                
                if (canBuild && (Time.realtimeSinceStartup -  m_lastSendServerCheck>2|| isMouseUp))
                {
                    var posModel = buildModelObj.transform.position;
                    m_allianceProxy.SendCheckBuildCanBuilded(m_buildData.type,posModel.x,posModel.z);

                    m_lastSendServerCheck = Time.realtimeSinceStartup;
                }
            }
            

        }

        private void SetCanBuild(bool canBuild,bool isMouseUp = false)
        {
          
//            Debug.Log(this.canBuild+" setcanBuild new : "+canBuild+"  "+buildModelPosition);
            if (canBuild== this.canBuild)
            {
                return;
            }
            
            this.canBuild = canBuild;

            if (buildBgObj)
            {
                var sr = buildBgObj.GetComponent<SpriteRenderer>();

                sr.color = canBuild ? Color.green : Color.red;
            }
            else
            {
                Debug.Log("no find SpriteRenderer");
            }

            if (buildRadiusObj)
            {
                var sr = buildRadiusObj.GetComponent<SpriteRenderer>();

                sr.color = canBuild ? Color.green : Color.red;
            }else
            {
                Debug.Log("no find SpriteRenderer2");
            }

            if (getBuildMediator()!=null)
            {
                getBuildMediator().SetCanBuild(canBuild);
            }
            
            
        }

        public bool CanBuild => canBuild;

        private Color m_color;

        private void CreateForbidenMesh()
        {
            CoreUtils.assetService.Instantiate("map_4_NoBuilding_NavMesh", (go) => { forbiddenMeshModelObj = go; });
        }

        private void CreateBuildModels()
        {
            
            m_worldMgrMediator.SetWorldMapState(WorldEditState.GuildBuildCreate);
            
            CoreUtils.assetService.Instantiate(config.modelId, (go) =>
            {
                if (go!=null)
                {
                    var t = GetRoot();
                    go.transform.SetParent(t);
                    go.transform.localScale = Vector3.one;

                    buildModelObj = go;
                    
                    AllianceProxy.setFlag(go,m_allianceProxy.GetAlliance().signs);
                    
                    
                    CoreUtils.assetService.Instantiate("teleportcity_bg", (rgo) =>
                    {
                        buildRadiusObj = rgo;
                        
                        rgo.transform.localScale = new Vector3(config.radiusCollide,config.radiusCollide,config.radiusCollide);




                        if (config.territorySize>0)
                        {
                            CoreUtils.assetService.Instantiate("alliancebuildarea_bg", (bgo) =>
                            {
                            
                                buildBgObj = bgo;

                                float scale = config.territorySize * 0.25f -config.territorySize/WorldMapObjectProxy.TerritoryPerUnit.x/2 -1;
    
                                bgo.transform.localScale = new Vector3(scale,scale,scale);
                                CenterBuildModelPos();
                                CheckCanBuild(true);
                                CoreUtils.uiManager.ShowUI(UI.s_AllianceCreateBuildPopup,null,config);
                            });
                        }
                        else
                        {
                            CenterBuildModelPos();
                            CheckCanBuild(true);
                            CoreUtils.uiManager.ShowUI(UI.s_AllianceCreateBuildPopup,null,config);
                        }
                    });
                }
                
            });
        }

        public void DelBuildModel()
        {
            if (buildRadiusObj!=null)
            {
                
                m_worldProxy.ClearFakeTerritory();
                m_worldMgrMediator.OnWorldTerritoryChanged();

                if (buildBgObj!=null)
                {
                    CoreUtils.assetService.Destroy(buildBgObj);
                }
                CoreUtils.assetService.Destroy(buildRadiusObj);
                CoreUtils.assetService.Destroy(buildModelObj);
                
                CoreUtils.assetService.Destroy(forbiddenMeshModelObj);
                buildBgObj = null;
                buildRadiusObj = null;
                buildModelObj = null;
                forbiddenMeshModelObj = null;
            }
            
            CoreUtils.inputManager.RemoveTouch2DEvent(OnTouchBegan, OnTouchMoved, OnTouchEnded);
        }

        private void CenterBuildModelPos()
        {
            if (buildModelObj!=null)
            {
                var viewCenter = WorldCamera.Instance().GetViewCenter();
                var pos = new Vector3(viewCenter.x,0,viewCenter.y);
                
                SetCityModelPos(pos);
            }
        }

        public void SetCityModelPos(Vector3 pos)
        {
            buildModelPosition = pos;
            var alignPos = GetAlignPos(pos);

            float unitX = WorldMapObjectProxy.TerritoryPerUnit.x;
            float unitY = WorldMapObjectProxy.TerritoryPerUnit.y;

            int gridPosX = Mathf.FloorToInt(alignPos.x / unitX);
            int gridPosY = Mathf.FloorToInt(alignPos.z / unitY);

            if (buildModelAlignPosition.x != alignPos.x || buildModelAlignPosition.z != alignPos.z)
            {
                buildModelAlignPosition = alignPos;


                if (config.territorySize>0)
                {
                    int allianceId = (int)m_allianceProxy.GetAlliance().guildId;
                    int width = Mathf.FloorToInt(config.territorySize/unitX/2);
                
                    List<ManorItem> fakes = new List<ManorItem>();
                    
                    for (int gx = -width; gx <= width; gx++)
                    {
                        int col = gridPosX + gx;
                        for (int gy = -width; gy <= width; gy++)
                        {
                            int row = gridPosY + gy;
                            
                            if (col>=0 && col * unitX < 7200 && row>=0 && row * unitY< 7200)
                            {
//                                Debug.Log(col*unitX+"--假领地---"+row*unitY  +"  "+col+"  "+row+"  索引:"+(row*400+col));
                                var item = new ManorItem(allianceId,m_color, (int)(col*unitX),(int)(row*unitY)
                                    ,(int)(col*unitX+unitX),(int)(row*unitY+unitY));
                            
                                fakes.Add(item);
                            }
                        }
                    }

                    if (fakes.Count>0)
                    {
                        
                        if (m_worldProxy.AddFakeTerritory(fakes))
                        {
//                            Debug.Log(fakes[0].startPosX+" "+ fakes[0].startPosY +"领地假数据刷新"+fakes.Count);
                            //领地数据变化
                            m_worldMgrMediator.OnWorldTerritoryChanged();
                        }
                    }
                }
            }

            float centerPosX = gridPosX * (WorldMapObjectProxy.TerritoryPerUnit).x +
                               (WorldMapObjectProxy.TerritoryPerUnit).x * 0.5f;
            
            float centerPosY = gridPosY * (WorldMapObjectProxy.TerritoryPerUnit).x +
                               (WorldMapObjectProxy.TerritoryPerUnit).y * 0.5f;

            buildModelObj.transform.position = pos;
            
            if (buildBgObj)
            {
                buildBgObj.transform.position = new Vector3(centerPosX,0,centerPosY);
            }

            if (config.radiusCollide>0)
            {
                m_worldMgrMediator.SetGuildCreateBuildPos(buildModelAlignPosition,config.radiusCollide);
            }

            if (buildRadiusObj)
            {
                buildRadiusObj.transform.position = buildModelAlignPosition;
            }

            if ( getBuildMediator()!=null)
            {
                getBuildMediator().UpdatePopPos();
            }
           
        }

     

        public Vector3 GetBuildModelPos()
        {
            return buildModelPosition;
        }

        private static float ALIGN_DIST = 0.01f;
        public static Vector3 GetAlignPos(Vector3 pos)
        {
            float x = Mathf.Floor(pos.x / ALIGN_DIST + 0.5f) * ALIGN_DIST;
            float z = Mathf.Floor(pos.z / ALIGN_DIST + 0.5f) * ALIGN_DIST;
            return new Vector3(x, pos.y, z);
        }
        
        private Transform GetRoot()
        {
            if (this.m_root == null)
            {
                this.m_root = GameObject.Find(m_root_path).transform;
            }

            return this.m_root;
        }

        #endregion
    }
}
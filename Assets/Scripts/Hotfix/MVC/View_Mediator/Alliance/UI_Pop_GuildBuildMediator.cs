// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, April 20, 2020
// Update Time         :    Monday, April 20, 2020
// Class Description   :    UI_Pop_GuildBuildMediator
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
    public class UI_Pop_GuildBuildMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_GuildBuildMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Pop_GuildBuildMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_GuildBuildView view;

        private UI_Pop_GuildBuildResMediator _resMediator;

        private Vector3 lastWorldPos;

        private Vector2 lastViewCenter;
        
        private AllianceProxy m_allianceProxy;
        
        private AllianceBuildingTypeDefine m_buildType;
        private AllianceBuildingDataDefine m_buildData;
        
        private WorldMgrMediator m_worldMgrMediator;
        private Dictionary<long,GuildCurrencyInfoEntity> m_depotCurrencyInfo;

        private PlayerProxy m_playerProxy;


        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_CreateGuildBuild.TagName,
                CmdConstant.AllianceDepot
            }.ToArray();
        }
        
        
        

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Guild_CreateGuildBuild.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        ErrorCodeHelper.ShowErrorCodeTip(error);
                        ExitCreateMode();
                    }
                    else
                    {
                        Guild_CreateGuildBuild.response response = notification.Body as Guild_CreateGuildBuild.response;

                        if (response.HasObjectIndex)
                        {
                            FightHelper.Instance.Reinfore((int)response.objectIndex,(int)response.objectIndex,response.objectIndex, response.pos.x, response.pos.y);
                        }
                        ExitCreateMode();
                    }

                    

                    break;
                
                case CmdConstant.AllianceDepot:
                    UpdateRss();
                    
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
            if (m_timer!=null)
            {
                m_timer.Cancel();
                m_timer = null;
            }

            if (_resMediator!=null)
            {
                Timer.Register(0.0f, () =>
                {
                    ExitCreateMode();
                });
               
            }
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            _resMediator =
                AppFacade.GetInstance().RetrieveMediator(UI_Pop_GuildBuildResMediator.NameMediator) as
                    UI_Pop_GuildBuildResMediator;
            m_worldMgrMediator =
                AppFacade.GetInstance().RetrieveMediator(WorldMgrMediator.NameMediator) as WorldMgrMediator;
            
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            
            m_buildType = view.data as AllianceBuildingTypeDefine;
        }

        protected override void BindUIEvent()
        {
            view.m_btn_build.m_btn_languageButton_GameButton.onClick.AddListener(onCreateBuild);
            view.m_btn_mes_GameButton.onClick.AddListener(onTipView);
            view.m_btn_descBack_GameButton.onClick.AddListener(onBackMain);
            
            UpdatePopPos();

        }

        private void onCreateBuild()
        {

            if (m_playerProxy.CurrentRoleInfo.guildId ==0)
            {
                Tip.CreateTip(730137).Show();
                ExitCreateMode(); 
                return;
            }
            
            
            if (m_allianceProxy.GetSelfRoot(GuildRoot.createBuild)==false && m_buildType.imgShowIndex != 2 )//不是旗帜
            {
                Tip.CreateTip(730137).Show();
                return;
            }else if (m_allianceProxy.GetSelfRoot(GuildRoot.createFlag)==false && m_buildType.imgShowIndex == 2)
            {
                Tip.CreateTip(730137).Show();
                return;
            }
            
            view.m_btn_build.m_btn_languageButton_GameButton.interactable = false;

            float posx = GetBuildModelPos().x;
            float posy = GetBuildModelPos().z;
            
            m_allianceProxy.SendCreateBuild(m_buildType.type,posx,posy);

            
        }


        public void ExitCreateMode()
        {
            if (_resMediator!=null)
            {
                CoreUtils.uiManager.CloseGroupUI(UI.ALLIANCE_GRPOP);
                AppFacade.GetInstance().SendNotification(CmdConstant.ShowMainCityUI,EnumMainModule.All);
                m_worldMgrMediator.SetWorldMapState(WorldEditState.Normal);
                _resMediator.DelBuildModel();
                _resMediator = null;
            }
        }


        private bool m_isCurrencyRich;

        private Timer m_timer;
        protected override void BindUIData()
        {
            view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_buildType.l_desc);
            view.m_lbl_desc2_LanguageText.text = LanguageUtils.getText(m_buildType.l_desc2);
           
            m_timer = Timer.Register(10,UpdateRss,null,true);

            UpdateRss();

            SetCanBuild(_resMediator.CanBuild);
            
            UpdatePopPos();
        }

        private void UpdateRss()
        {

            if (m_timer==null)
            {
                return;
            }
            
            int count = 1;

            if (m_buildType.type == 3) //旗帜个数
            {
                count = (int) m_allianceProxy.GetFlagInfoEntity().flagNum + 1;
            }
            m_buildData = CoreUtils.dataService.QueryRecord<AllianceBuildingDataDefine>(m_buildType.type * 10000+count);
            
            m_depotCurrencyInfo = m_allianceProxy.GetDepotCurrencyInfoEntities();

            var CurrencyIDs = UI_Pop_GuildBuildResMediator.CurrencyIDs;
            
            AllianceResarchProxy resarchProxy =  AppFacade.GetInstance().RetrieveProxy(AllianceResarchProxy.ProxyNAME) as AllianceResarchProxy;
            long costFund = resarchProxy.GetAttrMulti(allianceAttrType.allianceBuildingCostMulti,m_buildData.fund);
            long costFood = resarchProxy.GetAttrMulti(allianceAttrType.allianceBuildingCostMulti,m_buildData.food);
            long costWood = resarchProxy.GetAttrMulti(allianceAttrType.allianceBuildingCostMulti,m_buildData.wood);
            long costStone = resarchProxy.GetAttrMulti(allianceAttrType.allianceBuildingCostMulti,m_buildData.stone);
            long costCoin = resarchProxy.GetAttrMulti(allianceAttrType.allianceBuildingCostMulti,m_buildData.coin);

            bool fund =costFund == 0 ||
                       costFund > 0 && m_allianceProxy.GetCurrenc(m_depotCurrencyInfo[CurrencyIDs[0]]) >= costFund;
            bool food = costFood == 0 ||
                        costFood > 0 && m_allianceProxy.GetCurrenc(m_depotCurrencyInfo[CurrencyIDs[1]]) >= costFood;
            bool wood = costWood == 0 ||
                        costWood > 0 && m_allianceProxy.GetCurrenc(m_depotCurrencyInfo[CurrencyIDs[2]]) >= costWood;
            bool stone = costStone == 0 ||
                         costStone > 0 && m_allianceProxy.GetCurrenc(m_depotCurrencyInfo[CurrencyIDs[3]]) >= costStone;
            bool coin = costCoin == 0 ||
                        costCoin > 0 && m_allianceProxy.GetCurrenc(m_depotCurrencyInfo[CurrencyIDs[4]]) >= costCoin;
            
            
            view.m_pl_res1.setNum((int)costFund, fund);
            view.m_pl_res2.setNum((int)costFood, food);
            view.m_pl_res3.setNum((int)costWood, wood);
            view.m_pl_res4.setNum((int)costStone, stone);
            view.m_pl_res5.setNum((int)costCoin, coin);

            m_isCurrencyRich = fund && food && wood && stone && coin;
            
            SetCanBuild(m_canBuild);
        }




        private void onTipView()
        {
            view.m_pl_mes.gameObject.SetActive(false);
            view.m_pl_description.gameObject.SetActive(true);
        }
        
        private void onBackMain()
        {
            view.m_pl_mes.gameObject.SetActive(true);
            view.m_pl_description.gameObject.SetActive(false);
        }

        private bool m_canBuild = true;

        public void SetCanBuild(bool canBuild)
        {
            if (m_canBuild!=canBuild)
            {
                m_canBuild = canBuild;
//                Debug.Log(canBuild+" SetCanBuild ");
            }
            view.m_btn_build.m_btn_languageButton_GameButton.interactable = m_canBuild && m_isCurrencyRich;
        }

        public void UpdatePopPos()
        {
            var world_pos = GetBuildModelPos();
            var viewCenter = WorldCamera.Instance().GetViewCenter();
            if (world_pos ==lastWorldPos && viewCenter == lastViewCenter )
            {
                return;
            }

            lastWorldPos = world_pos;
            lastViewCenter = viewCenter;
            
            float radius = 30f;
            UIHelper.CalcPopupPos2(view.gameObject.transform as RectTransform,
                view.m_pl_pos.GetComponent<RectTransform>(),
                view.m_img_bg_PolygonImage.GetComponent<RectTransform>(),
                world_pos,
                view.m_img_arrowSideL_PolygonImage.gameObject, view.m_img_arrowSideR_PolygonImage.gameObject,
                view.m_img_arrowSideTop_PolygonImage.gameObject, view.m_img_arrowSideButtom_PolygonImage.gameObject,
                radius);
        }

        public Vector3 GetBuildModelPos()
        {
            return _resMediator.GetBuildModelPos();
        }

        public void setVisbleUI(bool isV)
        {
            this.view.gameObject.SetActive(isV);    
        }
        
        

        #endregion
    }
}
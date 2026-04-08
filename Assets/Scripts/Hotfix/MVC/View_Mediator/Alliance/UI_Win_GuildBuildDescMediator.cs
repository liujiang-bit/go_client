// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Monday, April 20, 2020
// Update Time         :    Monday, April 20, 2020
// Class Description   :    UI_Win_GuildBuildDescMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Win_GuildBuildDescMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildBuildDescMediator";
        private WorldMapObjectProxy m_worldProxy;

        private AllianceProxy m_allianceProxy;

        private PlayerProxy m_playerProxy;

        private MapObjectInfoEntity m_mapData;

        private int m_buildType;

        private AllianceBuildingTypeDefine m_buildTypeConfig;

        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildBuildDescMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildBuildDescView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.MapObjectRemove,
                CmdConstant.MapObjectChange,
                Guild_RepairGuildBuild.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.MapObjectRemove:
                    
                    MapObjectInfoEntity mapItemInfo = notification.Body as MapObjectInfoEntity;
                    if (mapItemInfo == null)
                    {
                        return;
                    }

                    if (mapItemInfo.objectId == m_mapData.objectId)
                    {
                        onClose();
                    }
                    break;
                case CmdConstant.MapObjectChange:
                    
                    UpdateState();
                    break;
                case Guild_RepairGuildBuild.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        Tip.CreateTip(733366).Show();
                    }
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
            StopTime();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_worldProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            m_mapData =  view.data as MapObjectInfoEntity;

            m_buildType = m_allianceProxy.GetBuildServerTypeToConfigType(m_mapData.objectType);

            m_buildTypeConfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(m_buildType);
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type2.setCloseHandle(onClose);
            
            view.m_UI_Model_Window_Type2.setWindowTitle(LanguageUtils.getText(m_buildTypeConfig.l_nameId));
            
            view.m_btn_info_GameButton.onClick.AddListener(onInfo);
            
            view.m_btn_infoBar_GameButton.onClick.AddListener(onInfoBar);
            
            view.m_btn_remove_GameButton.onClick.AddListener(onRemoveBuild);
            
            view.m_btn_remove_GameButton.gameObject.SetActive(m_allianceProxy.GetSelfRoot(GuildRoot.removeBuild));
            
            view.m_btn_fireFightingAlliance.m_btn_languageButton_GameButton.onClick.AddListener(onFileAlliance);

            view.m_btn_fireFightingAlliance.m_btn_languageButton_GameButton.interactable =
                m_allianceProxy.CheckIsR45(m_playerProxy.CurrentRoleInfo.rid);
            
            view.m_btn_fireFightingDaner.m_btn_languageButton_GameButton.onClick.AddListener(onFileDaner);
            
            UpdateState();
        }

        private void onFileAlliance()
        {
            
            m_allianceProxy.SendRepairBuild(m_mapData.objectId,2);
        }
        
        private void onFileDaner()
        {
            m_allianceProxy.SendRepairBuild(m_mapData.objectId,1);
        }

        private void onRemoveBuild()
        {
         
            Alert.CreateAlert(732022, LanguageUtils.getText(300099))
                .SetLeftButton(() =>
                {
                    m_allianceProxy.SendRemoveBuild(m_mapData.objectId);
                    
                },LanguageUtils.getText(730154)).SetRightButton(null,LanguageUtils.getText(730155)).Show();
            
        }

        private void onInfo()
        {
            if (m_mapData.rssType<= RssType.GuildFortress2)
            {
                HelpTip.CreateTip(4009, view.m_btn_info_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
            }
            else
            {
                HelpTip.CreateTip(4008, view.m_btn_info_GameButton.transform).SetStyle(HelpTipData.Style.arrowUp).Show();
            }

            
        }
        
        private void onInfoBar()
        {
           
            HelpTip.CreateTip(4010, view.m_btn_info_GameButton.transform).Show();
            
        }

        protected override void BindUIData()
        {

            view.m_btn_fireFightingAlliance.m_lbl_line2_LanguageText.text =
                ClientUtils.FormatComma(m_buildTypeConfig.fixFund);
            
            
            view.m_btn_fireFightingDaner.m_lbl_line2_LanguageText.text =
                ClientUtils.FormatComma(m_buildTypeConfig.fixGem);
            
            ClientUtils.UIReLayout(view.m_btn_fireFightingDaner.m_btn_languageButton_GameButton);
            ClientUtils.UIReLayout(view.m_btn_fireFightingAlliance.m_btn_languageButton_GameButton);

            view.m_lbl_mes_LanguageText.text = LanguageUtils.getText(m_buildTypeConfig.l_desc);
            
            CoreUtils.assetService.Instantiate(m_buildTypeConfig.imgShow, (obj) =>
                {
                    if (obj!=null)
                    {
                        obj.transform.parent = view.m_pl_build_PolygonImage.transform;
                        obj.transform.localScale = Vector3.one;
                        obj.transform.localPosition = Vector3.zero;
                        
                        //旗帜样式

                        if (m_mapData.rssType == RssType.GuildFlag)
                        {
                            AllianceProxy.SetUIFlag(m_mapData.guildFlagSigns,obj);
                        }
                    }
                    else
                    {
                       Debug.Log("无法找到"+m_buildTypeConfig.imgShow); 
                    }
                });
            
            UpdateState();
        }

        private Timer m_timer;

        private void onTime()
        {

            if (view.gameObject==null)
            {
                StopTime();
                return;
            }
            
            long endTime = m_mapData.buildBurnTime + m_buildTypeConfig.burnLast;
            view.m_lbl_fireTime_LanguageText.text = LanguageUtils.getTextFormat(732079,
                ClientUtils.FormatTimeTroop((int) (endTime - ServerTimeModule.Instance.GetServerTime()))
                );

            if ((GuildBuildState) m_mapData.guildBuildStatus == GuildBuildState.fire ||(GuildBuildState) m_mapData.guildBuildStatus == GuildBuildState.fix)
            {
                UpdateDurable();
            }
            
            long outFireTime = m_mapData.lastOutFireTime + m_buildTypeConfig.outFireCD -
                              ServerTimeModule.Instance.GetServerTime();

            bool ShowCdTime = m_mapData.lastOutFireTime > 0 && outFireTime > 0;

            if (m_mapData.lastOutFireTime>0 && outFireTime > 0)
            {
                //灭火cd时间
                view.m_lbl_cdTime_LanguageText.text = LanguageUtils.getTextFormat(732081,ClientUtils.FormatTimeSplit((int)outFireTime));
            }
            
            view.m_btn_fireFightingAlliance.m_btn_languageButton_GameButton.interactable =
                !ShowCdTime && m_allianceProxy.GetSelfRoot(GuildRoot.outfire);
            
            view.m_btn_fireFightingDaner.m_btn_languageButton_GameButton.interactable =
                !ShowCdTime;
            
           view.m_lbl_fireFighting_LanguageText.gameObject.SetActive(false);

            view.m_lbl_cdTime_LanguageText.gameObject.SetActive(ShowCdTime );
            view.m_lbl_limit_LanguageText.gameObject.SetActive(!ShowCdTime );
            
        }

        private void UpdateDurable()
        {
              
             switch ((GuildBuildState) m_mapData.guildBuildStatus)
             {
                 case GuildBuildState.fire:
                     float durable = m_mapData.durable -
                                     (ServerTimeModule.Instance.GetServerTime() - m_mapData.buildBurnTime) *
                                     m_mapData.buildBurnSpeed / 100f;

                     //耐久度上下限
                     view.m_lbl_hp_LanguageText.text =
                         LanguageUtils.getTextFormat(181104,  ClientUtils.FormatComma((long)durable), ClientUtils.FormatComma(m_mapData.durableLimit));
                     view.m_pb_rogressBar_GameSlider.value = durable / m_mapData.durableLimit;
                     break;
                 case GuildBuildState.fix:
                     int buildID =
                         m_allianceProxy.GetBuildServerTypeToConfigType((long) m_mapData.rssType);

                     var buildInfo =
                         CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(buildID);

                     float durablefix = m_mapData.durable +
                                        (ServerTimeModule.Instance.GetServerTime() -
                                         m_mapData.buildDurableRecoverTime) *
                                        buildInfo.durableUp / 3600f;
                     
                     if (durablefix>m_mapData.durableLimit)
                     {
                         durablefix = m_mapData.durableLimit;
                     }
                     //耐久度
                     view.m_lbl_hp_LanguageText.text =
                         LanguageUtils.getTextFormat(181104,  ClientUtils.FormatComma((long)durablefix), ClientUtils.FormatComma(m_mapData.durableLimit));
                     view.m_pb_rogressBar_GameSlider.value = durablefix / m_mapData.durableLimit;
                     
                     break;

                 default:
                     //耐久度上下限
                     view.m_lbl_hp_LanguageText.text =
                         LanguageUtils.getTextFormat(181104,  ClientUtils.FormatComma(m_mapData.durable), ClientUtils.FormatComma(m_mapData.durableLimit));
                     view.m_pb_rogressBar_GameSlider.value = (float)m_mapData.durable / m_mapData.durableLimit;
                     break;
             }
                            
            
        }



        public void UpdateState()
        {

            UpdateDurable();

            //状态
            view.m_lbl_state_LanguageText.text = LanguageUtils.getTextFormat(732074,RS.GetGuildBuildState(m_mapData) ) ;

            //Color color = Color.green;
            //进度条颜色
            //ColorUtility.TryParseHtmlString(RS.AllianceBuildStateColor[m_mapData.guildBuildStatus], out color);
            
            ClientUtils.LoadSprite(view.m_img_fill_PolygonImage,RS.AllianceBuildStateImg[m_mapData.guildBuildStatus]);
            
            //view.m_img_fill_PolygonImage.color = color ;
            
            //燃烧面板
            view.m_pl_fire.gameObject.SetActive(m_mapData.guildBuildStatus == (int)GuildBuildState.fire);//燃烧中

            switch ((GuildBuildState)m_mapData.guildBuildStatus)
            {
                case GuildBuildState.fire :
                    //燃烧速度
                    view.m_lbl_fireSpeed_LanguageText.text = LanguageUtils.getTextFormat(732082,ClientUtils.FormatComma(m_buildTypeConfig.durableDown));

                    ClientUtils.TextSetColor(view.m_lbl_fireSpeed_LanguageText,RS.GetGuildBuildStateColor(m_mapData));
                    
                    onTime();
                    if (m_timer ==null)
                    {
                        m_timer = Timer.Register(1,onTime,null,true);
                    }
                    view.m_btn_fireFightingAlliance.gameObject.SetActive(true);
                    view.m_btn_fireFightingDaner.gameObject.SetActive(true);

                    
                    long outFireTime = m_mapData.lastOutFireTime + m_buildTypeConfig.outFireCD -
                                      ServerTimeModule.Instance.GetServerTime();
                    
                    bool ShowCdTime = m_mapData.lastOutFireTime > 0 && outFireTime > 0;
                    
                    view.m_btn_fireFightingAlliance.m_btn_languageButton_GameButton.interactable =
                        !ShowCdTime && m_allianceProxy.GetSelfRoot(GuildRoot.outfire);

                    view.m_btn_fireFightingDaner.m_btn_languageButton_GameButton.interactable =
                        !ShowCdTime;
                    view.m_lbl_fireSpeed_LanguageText.gameObject.SetActive(true);
                    break;
                
                case GuildBuildState.fix :
                    onTime();
                    if (m_timer ==null)
                    {
                        m_timer = Timer.Register(1,onTime,null,true);
                    }
                    
                    view.m_btn_fireFightingAlliance.gameObject.SetActive(false);
                    view.m_btn_fireFightingDaner.gameObject.SetActive(false);
                    //修复速度
                    view.m_lbl_fireSpeed_LanguageText.text = LanguageUtils.getTextFormat(732083,ClientUtils.FormatComma(m_buildTypeConfig.durableUp));
                    view.m_lbl_fireSpeed_LanguageText.gameObject.SetActive(true);
                    ClientUtils.TextSetColor(view.m_lbl_fireSpeed_LanguageText,RS.GetGuildBuildStateColor(m_mapData));
                    break;
                default:
                    view.m_lbl_fireSpeed_LanguageText.gameObject.SetActive(false);
                    StopTime();
                    break;
            }

            //ClientUtils.TextSetColor(view.m_lbl_fireSpeed_LanguageText, RS.AllianceBuildStateColor[m_mapData.guildBuildStatus]);
        }


        private void StopTime()
        {
            if (m_timer!=null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }

        private void onClose()
        {
            StopTime();
            CoreUtils.uiManager.CloseUI(UI.s_AllianceBuildInfoDes);
        }

        #endregion
    }
}
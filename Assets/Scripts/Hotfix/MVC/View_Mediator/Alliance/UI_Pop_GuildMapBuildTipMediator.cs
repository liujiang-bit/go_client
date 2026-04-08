// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Wednesday, April 22, 2020
// Update Time         :    Wednesday, April 22, 2020
// Class Description   :    UI_Pop_GuildMapBuildTipMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using Hotfix;
using PureMVC.Interfaces;
using SprotoType;

namespace Game {
    public class UI_Pop_GuildMapBuildTipMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Pop_GuildMapBuildTipMediator";


        private WorldMapObjectProxy m_worldProxy;

        private AllianceProxy m_allianceProxy;

        private PlayerProxy m_playerProxy;
        private TroopProxy m_TroopProxy;

        private MapObjectInfoEntity m_mapData;

        private int m_buildType;

        private AllianceBuildingTypeDefine m_buildTypeConfig;

        private Timer m_timer;
        #endregion

        //IMediatorPlug needs
        public UI_Pop_GuildMapBuildTipMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Pop_GuildMapBuildTipView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.MapObjectChange,
                CmdConstant.MapObjectRemove
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.MapObjectChange:
                {
                    MapObjectInfoEntity mapItemInfo = notification.Body as MapObjectInfoEntity;
                    if (mapItemInfo == null)
                    {
                        return;
                    }

                    if (mapItemInfo.objectId == m_mapData.objectId)
                    {
                        UpdateInfo();
                    }
                }
                    break;
                case CmdConstant.MapObjectRemove:
                {
                    MapObjectInfoEntity mapItemInfo = notification.Body as MapObjectInfoEntity;
                    if (mapItemInfo == null)
                    {
                        return;
                    }

                    if (mapItemInfo.objectId == m_mapData.objectId)
                    {
                        onClose();
                    }
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

        public override void WinClose()
        {
            CheckStopTime();

        }

        private void CheckStopTime()
        {
            if (m_timer!=null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
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
            m_TroopProxy= AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;

            m_mapData = m_worldProxy.GetWorldMapObjectByobjectId((long) view.data);

            if (m_mapData==null)
            {
                onClose();
                return;
            }

            m_buildType = m_allianceProxy.GetBuildServerTypeToConfigType(m_mapData.objectType);

            m_buildTypeConfig = CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(m_buildType);

            //功能介绍引导
            if (m_mapData.rssType == RssType.GuildFlag)
            {
                //联盟旗帜
                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.AllianceFlag);
            }else if(m_mapData.rssType == RssType.GuildFood || m_mapData.rssType == RssType.GuildWood ||
                     m_mapData.rssType == RssType.GuildStone || m_mapData.rssType == RssType.GuildGold)
            {
                //不可采集联盟资源点
                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.AllianceRes);
            }
        }

        protected override void BindUIEvent()
        {
            
        }

        protected override void BindUIData()
        {
            view.m_lbl_position_LanguageText.text = PosHelper.FormatServerPos(m_mapData.objectPos);

            view.m_lbl_name_LanguageText.text = LanguageUtils.getText(m_buildTypeConfig.l_nameId);
            
            view.m_btn_create.m_btn_languageButton_GameButton.onClick.AddListener(onCreate);
            
            view.m_btn_addhelp.m_btn_languageButton_GameButton.onClick.AddListener(onHelpMember);
            
            view.m_btn_spy.m_btn_languageButton_GameButton.onClick.AddListener(onSky);
            
            view.m_btn_seeInfo.m_btn_languageButton_GameButton.onClick.AddListener(onDesInfo);
            
            view.m_btn_atk.m_btn_languageButton_GameButton.onClick.AddListener(onAtk);
            
            view.m_btn_mass.m_btn_languageButton_GameButton.onClick.AddListener(onMass);

            view.m_UI_Item_line3.m_btn_more_GameButton.onClick.AddListener(onGuildInfo);
            
            view.m_btn_descinfo_GameButton.onClick.AddListener(DesTip);
            
            view.m_btn_descBack_GameButton.onClick.AddListener(onBack);
            
            view.m_btn_more4_GameButton.onClick.AddListener(onGuildInfo);
            
            ClientUtils.LoadSprite(view.m_UI_Item_IconAndTime.m_img_icon_PolygonImage,m_buildTypeConfig.iconImg);
            
            UpdateInfo();
            UpdatePopPos();

            view.m_UI_Common_PopFun.InitSubView(m_mapData);
        }


        private bool IsGuildResPoint()
        {
            return m_mapData.rssType >= RssType.GuildFood && m_mapData.rssType <= RssType.GuildGold;
            
        }

        private void onBack()
        {
            if (IsGuildResPoint())
            {
                view.m_UI_Item_WorldObjInfoTPlayer1_ArabLayoutCompment.gameObject.SetActive(true);
            }
            else
            {
                view.m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment.gameObject.SetActive(true);
            }

            view.m_pl_description_Animator.gameObject.SetActive(false);
            view.m_btn_descinfo_GameButton.gameObject.SetActive(true);
//            view.m_btn_guildInfo_GameButton.gameObject.SetActive(true);
        }


        private void DesTip()
        {
            
            if (IsGuildResPoint())
            {
                view.m_UI_Item_WorldObjInfoTPlayer1_ArabLayoutCompment.gameObject.SetActive(false);
            }
            else
            {
                view.m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment.gameObject.SetActive(false);
            }
            
            view.m_btn_descinfo_GameButton.gameObject.SetActive(false);
            view.m_pl_description_Animator.gameObject.SetActive(true);
            view.m_lbl_desc_LanguageText.text = LanguageUtils.getText(m_buildTypeConfig.l_desc);
        }

        public void UpdatePopPos()
        {
            var world_pos = PosHelper.ServerPosToClient(m_mapData.objectPos);
            float radius = 50f;
            if (m_mapData.rssType == RssType.GuildFlag)
            {
                world_pos.Set(world_pos.x, 0, world_pos.z + 1);
                radius = 30f;
            }
            UIHelper.CalcPopupPos2(
                view.gameObject.GetComponent<RectTransform>(),
                view.m_pl_pos.gameObject.GetComponent<RectTransform>(),
                view.m_img_bg_PolygonImage.GetComponent<RectTransform>(), 
                world_pos,
                view.m_img_arrowSideL_PolygonImage.gameObject, view.m_img_arrowSideR_PolygonImage.gameObject,
                view.m_img_arrowSideTop_PolygonImage.gameObject, view.m_img_arrowSideButtom_PolygonImage.gameObject, radius);

        }

        private void onGuildInfo()
        {
            if (m_mapData.guildId == m_playerProxy.CurrentRoleInfo.guildId)
            {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_AllianceInfo,null,m_mapData.guildId);
            }
            
            onClose();
        }

        private void onClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceBuildInfoTip,true,false);
        }

      
        private void onTime()
        {
            if (m_mapData.guildBuildStatus == (long)GuildBuildState.building)
            {
                
                //剩余时间

                if (m_mapData.buildFinishTime==0)
                {
                    //无人在建造
                    view.m_UI_Item_line2.setLine2(LanguageUtils.getText(732077),ClientUtils.FormatTimeSplit((int)m_mapData.needBuildTime));
                    view.m_UI_Item_line1.setLine1(LanguageUtils.getText(732065),LanguageUtils.getTextFormat(180357,(int) ((float) m_mapData.buildProgress / m_buildTypeConfig.S * 100)));
                }
                else
                {
                    view.m_UI_Item_line2.setLine2(LanguageUtils.getText(732077),ClientUtils.FormatTimeSplit((int) (m_mapData.buildFinishTime - ServerTimeModule.Instance.GetServerTime())));
                    float rate = ((float)m_buildTypeConfig.S - m_mapData.buildProgress) /
                                 (m_mapData.buildFinishTime - m_mapData.buildProgressTime);

                    int pro = Mathf.FloorToInt((m_mapData.buildProgress+rate * (ServerTimeModule.Instance.GetServerTime()-m_mapData.buildProgressTime))/m_buildTypeConfig.S*100f);

                    if (pro<0)
                    {
                        pro = 0;
                    }
                    //建造进度
                    view.m_UI_Item_line1.setLine1(LanguageUtils.getText(732065),LanguageUtils.getTextFormat(180357,pro));
                }

                
            }
            else if (m_mapData.guildBuildStatus == (long)GuildBuildState.fire || m_mapData.guildBuildStatus == (long)GuildBuildState.fix)
            {
                UpdateInfo();
            }
            else
            {
                CheckStopTime();
            }
        }

        private void UpdateBuildIngState(bool isSelf)
        {
            view.m_pl_create.gameObject.SetActive(isSelf && m_mapData.guildBuildStatus== (long)GuildBuildState.building);
            view.m_pl_self_GridLayoutGroup.gameObject.SetActive(false);
            view.m_pl_am_GridLayoutGroup.gameObject.SetActive(!isSelf);


            if (m_mapData.rssType>=RssType.GuildFoodResCenter)
            {
                view.m_pl_am_GridLayoutGroup.gameObject.SetActive(false);
            }
                            
            view.m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
            //建造按钮
            view.m_btn_create.m_lbl_Text_LanguageText.text = LanguageUtils.getText(180520);

            if (m_timer==null && (m_mapData.guildBuildStatus == (long)GuildBuildState.building))
            {
                m_timer = Timer.Register(1,onTime,null,true);
                onTime();
            }
            else
            {
                if (m_mapData.needBuildTime > 0)
                {
                    //建造进度
                    view.m_UI_Item_line1.setLine1(LanguageUtils.getText(732065),LanguageUtils.getTextFormat(180357,(int)((float)m_mapData.buildProgress/m_buildTypeConfig.S*100)));
                    //剩余时间
                    view.m_UI_Item_line2.setLine2(LanguageUtils.getText(732077),ClientUtils.FormatTimeSplit((int)m_mapData.needBuildTime));
                }
            }
        }

        private void UpdateBuildFire()
        {
            
            if (m_timer==null && (m_mapData.guildBuildStatus == (long)GuildBuildState.fire || m_mapData.guildBuildStatus == (long)GuildBuildState.fix))
            {
                m_timer = Timer.Register(1,onTime,null,true);
            }
        }

        private bool m_fristTime = false;


        private void UpdateInfo()
        {
            bool isSelf = m_playerProxy.CurrentRoleInfo.guildId == m_mapData.guildId;
            switch (m_mapData.rssType)
            {
                    
                case RssType.GuildCenter:
                case RssType.GuildFortress1:
                case RssType.GuildFortress2:
                case RssType.GuildFlag:
                    switch ((GuildBuildState) m_mapData.guildBuildStatus)
                    {
                        case GuildBuildState.building:
                        {
                           UpdateBuildIngState(isSelf);
                        }
                            break;
                        default:
                            
                            view.m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
                            view.m_pl_create.gameObject.SetActive(false);
                            view.m_pl_am_GridLayoutGroup.gameObject.SetActive(!isSelf);
                            view.m_pl_self_GridLayoutGroup.gameObject.SetActive(isSelf);

                             switch ((GuildBuildState) m_mapData.guildBuildStatus)
                             {
                                 case GuildBuildState.fire:
                                     UpdateBuildFire();
                                     float durable = m_mapData.durable-
                                         (ServerTimeModule.Instance.GetServerTime() - m_mapData.buildBurnTime) *
                                         m_mapData.buildBurnSpeed / 100f;

                                     //耐久度
                                     view.m_UI_Item_line1.setLine1(LanguageUtils.getText(732071),LanguageUtils.getTextFormat(181104,ClientUtils.FormatComma((long)durable),ClientUtils.FormatComma(m_mapData.durableLimit)));
                                     break;
                                 case GuildBuildState.fix:
                                     UpdateBuildFire();
                                     int buildID = 
                                         m_allianceProxy.GetBuildServerTypeToConfigType((long) m_mapData.rssType);

                                     var buildInfo =
                                         CoreUtils.dataService.QueryRecord<AllianceBuildingTypeDefine>(buildID);

                                     float durablefix = m_mapData.durable+
                                         (ServerTimeModule.Instance.GetServerTime() -
                                          m_mapData.buildDurableRecoverTime) *
                                         buildInfo.durableUp / 3600f;
                                     
                                     if (durablefix>m_mapData.durableLimit)
                                     {
                                         durablefix = m_mapData.durableLimit;
                                     }
                                     //耐久度
                                     view.m_UI_Item_line1.setLine1(LanguageUtils.getText(732071),LanguageUtils.getTextFormat(181104,ClientUtils.FormatComma((long)durablefix),ClientUtils.FormatComma(m_mapData.durableLimit)));
                                     break;

                                 default:
                                     CheckStopTime();
                                     //耐久度
                                     view.m_UI_Item_line1.setLine1(LanguageUtils.getText(732071),LanguageUtils.getTextFormat(181104,ClientUtils.FormatComma(m_mapData.durable),ClientUtils.FormatComma(m_mapData.durableLimit)));
                                     break;
                             }
                            
                     
                            //状态
                            view.m_UI_Item_line2.setLine2(LanguageUtils.getText(732073), RS.GetGuildBuildState(m_mapData),
                                RS.GetGuildBuildStateColor(m_mapData));
                            break;
                    }
                    //所属联盟
                    view.m_UI_Item_line3.setLine3(LanguageUtils.getText(500006),LanguageUtils.getTextFormat(300030, m_mapData.guildAbbName, m_mapData.guildFullName));
                    
                    view.m_UI_Item_IconAndTime.m_btn_timebg_GameButton.gameObject.SetActive(false);
                    break;
                case RssType.GuildFood:
                case RssType.GuildWood:
                case RssType.GuildStone:
                case RssType.GuildGold:
                    //联盟资源点
                    //查看联盟信息
                    
                    view.m_UI_Item_WorldObjInfoTPlayer1_ArabLayoutCompment.gameObject.SetActive(true);
                    view.m_UI_Item_WorldObjInfoTPlayer_ArabLayoutCompment.gameObject.SetActive(false);
                    
                    ClientUtils.LoadSprite(view.m_UI_Item_IconAndTime1.m_img_icon_PolygonImage,m_buildTypeConfig.iconImg);

                    
                    view.m_btn_more4_GameButton.gameObject.SetActive(m_mapData.guildId>0);
                    //占领联盟 无
                    view.m_lbl_content4_LanguageText.text =  string.IsNullOrEmpty(m_mapData.guildAbbName)?
                        LanguageUtils.getText(570029):LanguageUtils.getTextFormat(730138,m_mapData.guildAbbName);
                    //领土收益
                    view.m_lbl_content5_LanguageText.text = LanguageUtils.getTextFormat(732083,ClientUtils.FormatComma(m_buildTypeConfig.holdAllianceSpeed));
                    
                    //资源图标
                    ClientUtils.LoadSprite(view.m_img_cur_PolygonImage,RS.GuildRssTypeIcons[(int)m_mapData.rssType-(int)RssType.GuildFood]);
                    
                    view.m_pl_create.gameObject.SetActive(false);
                    view.m_pl_self_GridLayoutGroup.gameObject.SetActive(false);
                    view.m_pl_am_GridLayoutGroup.gameObject.SetActive(false);
                    view.m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
                    break;
                case RssType.GuildFoodResCenter:
                case RssType.GuildWoodResCenter:
                case RssType.GuildGoldResCenter:
                case RssType.GuildGemResCenter:
                    
                    
                    //联盟资源中心  联盟麦场
                    switch ((GuildBuildState) m_mapData.guildBuildStatus)
                    {
                        case GuildBuildState.building:
                        {
                            UpdateBuildIngState(isSelf);
                            
                            view.m_UI_Item_IconAndTime.m_btn_timebg_GameButton.gameObject.SetActive(false);

                        }
                            break;
                        //采集状态
                        default:

                            CheckStopTime();
                            
                            if (m_timer ==null)
                            {
                                m_timer = Timer.Register(1, UpdateResCenter, null, true);
                            }
                            view.m_pb_rogressBar_GameSlider.gameObject.SetActive(isSelf);
                            view.m_btn_create.m_lbl_Text_LanguageText.text = LanguageUtils.getText(170014);
                            view.m_pl_create.gameObject.SetActive(isSelf);
                            view.m_pl_am_GridLayoutGroup.gameObject.SetActive(false);
                            view.m_pl_self_GridLayoutGroup.gameObject.SetActive(false);

                            m_fristTime = true;
                            UpdateResCenter();

                            break;
                    }

                    //所属联盟
                    view.m_UI_Item_line3.setLine3(LanguageUtils.getText(500006),
                        LanguageUtils.getTextFormat(300030, m_mapData.guildAbbName, m_mapData.guildFullName));
                    break;
            }
            
            
        }



        private int m_timeCount = 0;
        public void UpdateResCenter()
        {

            long passTime = ServerTimeModule.Instance.GetServerTime() - m_mapData.collectTime;
            long collectRes = passTime * m_mapData.collectSpeed / 10000;
            
            view.m_pb_rogressBar_GameSlider.gameObject.SetActive(m_mapData.collectRoleNum>0);
            
            long collect = m_mapData.resourceAmount - collectRes;
            if (collect<0)
            {
                collect = 0;
            }
            //储量
            view.m_UI_Item_line1.setLine1(LanguageUtils.getText(500004), ClientUtils.FormatComma(collect));
            //采集人数
            view.m_UI_Item_line2.setLine2(LanguageUtils.getText(500015),m_mapData.collectRoleNum.ToString());
            //所属联盟
            view.m_UI_Item_line3.setLine3(LanguageUtils.getText(500006),LanguageUtils.getTextFormat(300030, m_mapData.guildAbbName, m_mapData.guildFullName));
            //剩余天数
            view.m_UI_Item_IconAndTime.m_btn_timebg_GameButton.gameObject.SetActive(true);
            view.m_UI_Item_IconAndTime.m_lbl_time_LanguageText.text = ClientUtils.FormatTimeSplit((int)(m_mapData.resourceCenterDeleteTime - ServerTimeModule.Instance.GetServerTime()));

      
            int armyIndex = m_TroopProxy.GetArmyIndex((int)m_mapData.objectId);;
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyDataByArmyId(armyIndex);
            if (armyData != null && (armyIndex>0&& TroopHelp.IsHaveState(armyData.armyStatus, ArmyStatus.COLLECTING)))
            {
                view.m_pb_rogressBar_GameSlider.gameObject.SetActive(true);

                if (ServerTimeModule.Instance.GetServerTime()%5==0)
                {
                    m_timeCount++;
                }
              
                if (m_timeCount%2==0||m_fristTime)
                {
                    m_fristTime = false;
                    //采集量  
                    Int64 collectNum = ArmyInfoHelp.Instance.GetArmyCollectNum(armyIndex);
                    float m_canCollectNum = ArmyInfoHelp.Instance.GetArmyWeight(armyIndex);  
                
                    view.m_lbl_colPro_LanguageText.text = LanguageUtils.getTextFormat(500008, ClientUtils.FormatComma(collectNum),
                        ClientUtils.FormatComma((long)m_canCollectNum));;     
                    view.m_pb_rogressBar_GameSlider.maxValue= m_canCollectNum;
                    view.m_pb_rogressBar_GameSlider.value=collectNum;
                    view.m_lbl_colPro_LanguageText.gameObject.SetActive(true);
                    view.m_lbl_remainDayTime_LanguageText.gameObject.SetActive(false);
                }
                else
                {
                    //采集时间
                    long  residueTime = ArmyInfoHelp.Instance.GetArmyCollectResidueTime(armyIndex);
                    view.m_lbl_remainDayTime_LanguageText.text = LanguageUtils.getTextFormat(500009, ClientUtils.FormatCountDown((int) residueTime));
                
                    view.m_lbl_colPro_LanguageText.gameObject.SetActive(false);
                    view.m_lbl_remainDayTime_LanguageText.gameObject.SetActive(true);
                }
            }
            else
            {
                view.m_pb_rogressBar_GameSlider.gameObject.SetActive(false);
            }


        }

        private void onDesInfo()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceBuildInfoDes,null,m_mapData);
            
            onClose();
        }
        
        private void onCreate()
        {
            
            CoreUtils.uiManager.ShowUI(UI.s_AllianceBuild,null,m_mapData);
            onClose();
        }

        private void onHelpMember()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceBuild,null,m_mapData);
            onClose();
        }

        private void onSky()
        {
            onClose();
            FightHelper.Instance.Scout(m_mapData.objectPos.x,m_mapData.objectPos.y,(int)m_mapData.objectId);
        }

        private void onAtk()
        {
            onClose();
            FightHelper.Instance.Attack((int)m_mapData.objectId);
        }
        
        private void onMass()
        {
            onClose();
            FightHelper.Instance.Concentrate((int)m_mapData.objectId);
        }

        #endregion
    }
}
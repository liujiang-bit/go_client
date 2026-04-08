// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, December 27, 2019
// Update Time         :    Friday, December 27, 2019
// Class Description   :    ServerPushMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using Data;

namespace Game
{
    public class ServerPushMediator : GameMediator
    {
        #region Member

        public static string NameMediator = "ServerPushMediator";


        private PlayerProxy m_playerProxy;
        private PlayerAttributeProxy m_playerAttributeProxy;
        private NetProxy m_netProxy;

        private WorldMapObjectProxy m_worldMapProxy;

        private AllianceProxy m_allianceProxy;

        private AllianceResarchProxy m_allianceResProxy;
        
        private TroopProxy m_troopProxy;

        #endregion

        //IMediatorPlug needs
        public ServerPushMediator() : base(NameMediator, null)
        {
        }

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                Role_RoleInfo.TagName,
                System_KickConnect.TagName,
                Guild_GuildInfo.TagName,
                Guild_GuildApplys.TagName,
                Guild_GuildMemberInfo.TagName,
                Guild_GuildNotify.TagName,
                Guild_GuildDepotInfo.TagName,
                Guild_GuildRequestHelps.TagName,
                Guild_GuildBuilds.TagName,
                Map_GuildTerritories.TagName,
                Map_GuildTerritoryLines.TagName,
                Map_GuildBuildArmys.TagName,
                Guild_GuildHolyLands.TagName,
                Guild_GuildTechnologies.TagName,
                Guild_GuildGifts.TagName,
                Map_HolyLandArmys.TagName,
                CmdConstant.NetWorkReconnecting,
                Guild_MapMarkers.TagName
            }.ToArray();
        }

        public AllianceProxy MAllianceProxy
        {
            get
            {
                if (m_allianceProxy ==null)
                {
                    m_allianceProxy =
                        AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
                }
                return m_allianceProxy;
            }
        }
        
        public PlayerAttributeProxy MPlayerAttributeProxy
        {
            get
            {
                if (m_playerAttributeProxy ==null)
                {
                    m_playerAttributeProxy =
                        AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;
                }
                return m_playerAttributeProxy;
            }
        }

        public override void HandleNotification(INotification notification)
        {
            if (notification.Type == RpcTypeExtend.REQUEST)
            {
                switch (notification.Name)
                {
                    case Role_RoleInfo.TagName:
                        {

                            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                            Role_RoleInfo.request request = notification.Body as Role_RoleInfo.request;
                            if (request.roleInfo != null)
                            {
                                m_playerProxy.UpdateRoleInfo(request.roleInfo);
                            }


                        }
                        break;

                    case Guild_GuildInfo.TagName:
                        {


                            Guild_GuildInfo.request request = notification.Body as Guild_GuildInfo.request;
                            if (request.guildInfo != null)
                            {
                                MAllianceProxy.UpdateGuildInfo(request);
                            }
                        }
                        break;
                    case Guild_GuildApplys.TagName:
                        {


                            Guild_GuildApplys.request request = notification.Body as Guild_GuildApplys.request;

                            MAllianceProxy.UpdateGuildApplys(request);
                        }
                        break;
                    case Guild_GuildMemberInfo.TagName:
                        {


                            Guild_GuildMemberInfo.request request = notification.Body as Guild_GuildMemberInfo.request;

                            MAllianceProxy.UpdateGuildMembers(request);
                        }
                        break;
                    case Guild_GuildNotify.TagName:
                        {
                            Guild_GuildNotify.request request = notification.Body as Guild_GuildNotify.request;
                            if (request.notifyOperate > 0)
                            {
                                // # 1(入帮通知, roleInfos name 为加入联盟角色名称)
                                // # 2(移除成员通知, roleInfos name 为被移除角色名称)
                                // # 3(任命官员通知, roleInfos name 为被任命的角色名称, numArg为被任命的官职ID)
                                // # 4(联盟帮助通知, roleInfos name 为帮助的角色名称, numArg为求助类型: 1 建筑建造升级 2 治疗 3 科技升级)
                                // # 5(联盟解散通知)
                                // # 21(添加联盟标记通知, roleInfos name 为添加标记的角色名称, numArg为标记ID)
                                // # 22(删除联盟标记通知, roleInfos name 为删除标记的角色名称, numArg为标记ID)
                                switch (request.notifyOperate)
                                {
                                    case 1:
                                        foreach (var role in request.roleInfos)
                                        {
                                            Tip.CreateTip(730093, role.name).Show();

                                            if (role.rid == m_playerProxy.CurrentRoleInfo.rid)
                                            {
                                                CoreUtils.uiManager.CloseUI(UI.s_AllianceInviteTip);
                                                CoreUtils.uiManager.ShowUI(UI.s_AllianceMain);
                                                AppFacade.GetInstance().SendNotification(CmdConstant.AllianceJoinUpdate);
                                            }
                                        }
                                        break;
                                    case 2:
                                        foreach (var role in request.roleInfos)
                                        {
                                            Tip.CreateTip(730165, role.name).Show();
                                        }
                                        break;
                                    case 3:
                                        foreach (var role in request.roleInfos)
                                        {

                                            var asset = new Tip.OtherAssetData("UI_Common_Aggregation", (obj) =>
                                            {

                                                UI_Common_AggregationView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_AggregationView>(obj);
                                                var oinfo = MAllianceProxy.getMemberOfficerConfig(request.numArg[0]);

                                                tipView.m_lbl_message_LanguageText.text =
                                                    LanguageUtils.getTextFormat(730346, role.name,
                                                        LanguageUtils.getText(oinfo.l_officiallyID));

                                                tipView.m_UI_Model_GuildFlag.setData(MAllianceProxy.GetAlliance().signs);
                                            });

                                            Tip.CreateTip(asset).Show();
                                        }
                                        break;
                                    case 4:
                                        foreach (var type in request.numArg)
                                        {
                                            switch (type)
                                            {
                                                // 1 建筑建造升级 2 治疗 3 科技升级
                                                case 1:
                                                    Tip.CreateTip(730003, request.roleInfos[0].name).SetStyle(Tip.TipStyle.AllianceHelp).Show();
                                                    break;
                                                case 2:
                                                    Tip.CreateTip(730005, request.roleInfos[0].name).SetStyle(Tip.TipStyle.AllianceHelp).Show();
                                                    break;
                                                case 3:
                                                    Tip.CreateTip(730004, request.roleInfos[0].name).SetStyle(Tip.TipStyle.AllianceHelp).Show();
                                                    break;
                                            }
                                        }
                                        break;
                                    case 21:
                                        if (request.roleInfos.Count == request.numArg.Count)
                                        {
                                            for (int i = 0; i < request.roleInfos.Count; i++)
                                            {
                                                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)request.numArg[i]);
                                                if (mapMarkerTypeDefine != null)
                                                {
                                                    Tip.CreateTip(910005, request.roleInfos[i].name, LanguageUtils.getText(mapMarkerTypeDefine.l_nameId)).Show();
                                                }
                                            }
                                        }
                                        break;
                                    case 22:
                                        if (request.roleInfos.Count == request.numArg.Count)
                                        {
                                            for (int i = 0; i < request.roleInfos.Count; i++)
                                            {
                                                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)request.numArg[i]);
                                                if (mapMarkerTypeDefine != null)
                                                {
                                                    Tip.CreateTip(910008, request.roleInfos[i].name, LanguageUtils.getText(mapMarkerTypeDefine.l_nameId)).Show();
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;

                    case Guild_GuildDepotInfo.TagName:
                        {

                            Guild_GuildDepotInfo.request request = notification.Body as Guild_GuildDepotInfo.request;

                            MAllianceProxy.UpdateGuildDepotInfo(request);
                        }
                        break;

                    case Guild_GuildRequestHelps.TagName:
                        {

                            Guild_GuildRequestHelps.request request = notification.Body as Guild_GuildRequestHelps.request;

                            MAllianceProxy.UpdateHelps(request);
                        }
                        break;

                    case Guild_GuildBuilds.TagName:
                        {

                            Guild_GuildBuilds.request request = notification.Body as Guild_GuildBuilds.request;

                            MAllianceProxy.UpdateBuilds(request);
                            AppFacade.GetInstance().SendNotification(CmdConstant.AllianceJoinUpdate);
                        }
                        break;

                    case Map_GuildTerritories.TagName:
                        {
                            m_worldMapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                            Map_GuildTerritories.request request = notification.Body as Map_GuildTerritories.request;
                            m_worldMapProxy.UpdateTerritoris(request);
                        }
                        break;
                    case Map_GuildTerritoryLines.TagName:
                        {
                            m_worldMapProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                            Map_GuildTerritoryLines.request request = notification.Body as Map_GuildTerritoryLines.request;
                            m_worldMapProxy.UpdateTerritoryLines(request);
                        }
                        break;

                    case Map_GuildBuildArmys.TagName:
                        {
                            Map_GuildBuildArmys.request request = notification.Body as Map_GuildBuildArmys.request;
                            MAllianceProxy.UpdateArmsList(request);
                        }
                        break;
                    case Guild_GuildHolyLands.TagName:
                        Guild_GuildHolyLands.request req = notification.Body as Guild_GuildHolyLands.request;

                        MPlayerAttributeProxy.RemoveHolyland(MAllianceProxy.GetGuildHolyLandInfos());    // 移除之前的属性
                        MAllianceProxy.UpdateGuildHolyLandInfo(req);
                        MPlayerAttributeProxy.UpdateHolyland(MAllianceProxy.GetGuildHolyLandInfos());

                        break;
                    case Guild_GuildTechnologies.TagName:

                        if (m_allianceResProxy == null)
                        {
                            m_allianceResProxy = AppFacade.GetInstance().RetrieveProxy(AllianceResarchProxy.ProxyNAME) as AllianceResarchProxy;
                        }
                        m_allianceResProxy.UpdateTech(notification.Body as Guild_GuildTechnologies.request);

                        break;

                    case Guild_GuildGifts.TagName:

                        MAllianceProxy.UpdateGifts(notification.Body as Guild_GuildGifts.request);
                        break;
                    case Map_HolyLandArmys.TagName:
                        MAllianceProxy.UpdateHolyLandArmys(notification.Body as Map_HolyLandArmys.request);
                        break;
                    case Guild_MapMarkers.TagName:
                        AppFacade.GetInstance().SendNotification(CmdConstant.GuildMapMarkerInfoChanged, notification.Body);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                switch (notification.Name)
                {
                    case CmdConstant.NetWorkReconnecting:
                        m_playerProxy.m_isFirstGetRoleInfo = true;
                        break;
                }                                  
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            m_netProxy = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
        }

        protected override void BindUIEvent()
        {
        }

        protected override void BindUIData()
        {
            
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

        #endregion
    }
}
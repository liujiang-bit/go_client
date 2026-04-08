using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns;
using System;
using LitJson;
using PureMVC.Patterns.Facade;
using Skyunion;
using PureMVC.Patterns.Mediator;
using PureMVC.Interfaces;
using Sproto;
using SprotoType;
using PureMVC.Patterns.Observer;

namespace Game
{
    public class AppFacade : Facade
    {
        public const string STARTUP = "AppFacade.StartUp";

        private Dictionary<ViewBinder, IMediator> m_viewMap = new Dictionary<ViewBinder, IMediator>();
        private Dictionary<string, IMediator> m_MediatorMap = new Dictionary<string, IMediator>();
        private Dictionary<string, Mediator> m_globalMap = new Dictionary<string, Mediator>();

        public static AppFacade GetInstance()
        {
            return GetInstance(() => new AppFacade()) as AppFacade;
        }

        public static void Destroy()
        {
            instance = null;
            PureMVC.Core.View.Destroy();
            PureMVC.Core.Model.Destroy();
            PureMVC.Core.Controller.Destroy();
        }

        protected override void InitializeController()
        {
            base.InitializeController();
            #region "吴江海专用"
            RegisterCommand(STARTUP, ()=>new StartUpCommand());
            RegisterCommand(CmdConstant.ReloadGame, () => new ReloadGameCMD());
            RegisterCommand(CmdConstant.ExitGame, () => new ExitGameCMD());


            RegisterCommand(CmdConstant.AgreementCheck, () => new AgreementCMD());
            RegisterCommand(CmdConstant.LoadAppConfig, () => new LoadAppConfigCMD());
            RegisterCommand(CmdConstant.AutoLogin, () => new LoginCommand());
            RegisterCommand(CmdConstant.LoginAccount, () => new LoginCommand());
            RegisterCommand(CmdConstant.SwitchAccount, () => new LoginCommand());
            RegisterCommand(CmdConstant.BindindAccount, () => new LoginCommand());
            RegisterCommand(CmdConstant.LoadAccountProfile, () => new LoginCommand());
            RegisterCommand(CmdConstant.AccountBan, () => new LoginCommand());
            RegisterCommand(CmdConstant.SwitchAccountFinished, () => new LoginCommand());
            
            RegisterCommand(CmdConstant.OpenAppRating, () => new AppRatingCMD());

            RegisterCommand(CmdConstant.MaintainCheck, () => new MaintainCMD());
            RegisterCommand(CmdConstant.MaintainCheckSingleServer, () => new MaintainCMD());
            RegisterCommand(CmdConstant.PackageUpdateCheck, () => new MaintainCMD());
            RegisterCommand(CmdConstant.HotfixUpteCheck, () => new MaintainCMD());
            RegisterCommand(CmdConstant.HotfixDownCompleted, () => new MaintainCMD());

            RegisterCommand(CmdConstant.MoveCameraCmd, () => new CameraCmd());
            
            #endregion


            CoreUtils.uiManager.AddShowUIListener(OnShowUI);
            CoreUtils.uiManager.AddCloseUIListener(OnCloseUI);

            // 在这里注册所有的 Command
            //RegisterCommand(CmdConstant.NetWorkError, new NetCommand());

            RegisterCommand(CmdConstant.SwitchMainCityCmd, () => new SwitchMainCityCmd());
            RegisterCommand(CmdConstant.ResetSceneCmd, () => new ResetSceneCmd());
            RegisterCommand(Item_ItemInfo.TagName, () => new PlayerCmd());
            RegisterCommand(Build_BuildingInfo.TagName, () => new PlayerCmd());
            RegisterCommand(Item_ItemUse.TagName, () => new PlayerCmd());
            RegisterCommand(CmdConstant.SystemDayTimeChange, () => new PlayerCmd());
            RegisterCommand(Map_ObjectInfo.TagName, () => new MapObjCmd());
            RegisterCommand(Map_ObjectDelete.TagName, () => new MapObjCmd());
            RegisterCommand(Build_GetBuildResources.TagName,()=>new CurrencyCMD());
            //RegisterCommand(CmdConstant.GuideForceShowResCollect, ()=>new CurrencyCMD());
            RegisterCommand(Role_Heart.TagName, () => new ServerTimeCommand());
            RegisterCommand(Hero_HeroInfo.TagName, () => new HeroCmd());
            RegisterCommand(CmdConstant.GetNewHero, () => new HeroCmd());
            
            //邮件
            RegisterCommand(Email_GetEmails.TagName, () => new EmailCMD());
            RegisterCommand(Email_EmailList.TagName, () => new EmailCMD());
            RegisterCommand(Email_TakeEnclosure.TagName, () => new EmailCMD());
            RegisterCommand(Email_CollectEmail.TagName, () => new EmailCMD());
            RegisterCommand(Email_DeleteEmail.TagName, () => new EmailCMD());
            RegisterCommand(CmdConstant.SystemHourChange, () => new EmailCMD());

            //加速
            RegisterCommand(CmdConstant.SpeedUp,()=>new SpeedUpCMD());
            //奖励展示
            
            RegisterCommand(CmdConstant.RewardGet, () => new RewardGetCMD());
            //章节
            RegisterCommand(CmdConstant.ChapterTimelineShow, ()=>new TaskCMD());
            RegisterCommand(CmdConstant.ChapterIdChange,()=>new TaskCMD());
            RegisterCommand(CmdConstant.OnChapterDiaglogEnd, ()=>new TaskCMD());
             RegisterCommand(CmdConstant.TaskRewardEnd, ()=>new TaskCMD());
            //工人小屋
            RegisterCommand (CmdConstant.buildQueueChange, () => new WorkerCMD());
   
            //开场剧情对话
            RegisterCommand(CmdConstant.ShowNPCDiaglog,()=>new NPCDialogCMD());
            //章节对白
            RegisterCommand(CmdConstant.ShowChapterDiaglog,()=>new NPCDialogCMD());

            //科技研究完成
            RegisterCommand(Technology_ResearchTechnology.TagName,()=>new ResearchCMD());

            //特效层遮罩
            RegisterCommand(CmdConstant.ShowMask,()=>new StoryCMD());
            RegisterCommand(CmdConstant.HideMask, () => new StoryCMD());
            RegisterCommand(CmdConstant.OnMysteryStoreRefresh, () => new StoryCMD());
            RegisterCommand(Role_MysteryStore.TagName, () => new StoryCMD());

            //斥候
            RegisterCommand(Map_Scouts.TagName, () => new ScoutCMD());
            RegisterCommand(Map_ScoutsBack.TagName, () => new ScoutCMD());

            //玩家属性相关
            RegisterCommand(CmdConstant.UpdatePlayerCountry, () => new PlayerAttributeCmd());
            RegisterCommand(CmdConstant.technologyChange, () => new PlayerAttributeCmd());
            RegisterCommand(CmdConstant.CityBuildinginfoFirst, () => new PlayerAttributeCmd());
            RegisterCommand(CmdConstant.CityBuildinginfoChange, () => new PlayerAttributeCmd());
            RegisterCommand(CmdConstant.UpdateHero, () => new PlayerAttributeCmd());
            RegisterCommand(CmdConstant.VipLevelUP, () => new PlayerAttributeCmd());
            RegisterCommand(Guild_GuildTechnologies.TagName,() => new PlayerAttributeCmd()); 
            RegisterCommand(CmdConstant.AllianceEixtEx, () => new PlayerAttributeCmd()); 
            RegisterCommand(CmdConstant.GuildOfficerInfoChange, () => new PlayerAttributeCmd()); 


            //聊天
            RegisterCommand(Chat_PushMsg.TagName, () => new ChatCMD());
            RegisterCommand(CmdConstant.ChatClientNetEvent,()=>new ChatCMD());
            RegisterCommand(CmdConstant.OnCityLoadFinished,()=>new ChatCMD());
            RegisterCommand(Guild_CreateGuild.TagName, ()=>new ChatCMD());
            RegisterCommand(Guild_ApplyJoinGuild.TagName, ()=>new ChatCMD());
            RegisterCommand(CmdConstant.AllianceEixt, ()=>new ChatCMD());
            RegisterCommand(Chat_Msg2GSQueryPrivateChatLst.TagName, () => new ChatCMD());
            RegisterCommand(Chat_Msg2GSQueryPrivateChatByRid.TagName, () => new ChatCMD());
            RegisterCommand(Chat_SendMsg.TagName, () => new ChatCMD());
            
            //城市buff
            RegisterCommand(CmdConstant.CityBuffChange,()=>new CityBuffCMD());

            //OpenUI
            RegisterCommand(CmdConstant.OpenUI,()=>new OpenUICMD());
            RegisterCommand(CmdConstant.OpenUI2, () => new OpenUICMD());

            RegisterCommand(Activity_Reward.TagName, ()=>new ActivityCMD());        
            RegisterCommand(Activity_RewardBox.TagName, ()=>new ActivityCMD());        
            RegisterCommand(Activity_ScheduleInfo.TagName, ()=>new ActivityCMD());
            RegisterCommand(Activity_Exchange.TagName, ()=>new ActivityCMD());
            RegisterCommand(CmdConstant.ItemInfoChange, ()=>new ActivityCMD());
            RegisterCommand(CmdConstant.SystemDayChange, ()=>new ActivityCMD());
            RegisterCommand(Activity_Rank.TagName, ()=>new ActivityCMD());
            RegisterCommand(CmdConstant.ActivityActivePointUpdate, ()=>new ActivityCMD());
            RegisterCommand(Activity_TurnTable.TagName, ()=>new ActivityCMD()); 
            RegisterCommand(Activity_ClickActivity.TagName, ()=>new ActivityCMD()); 

            //小地图
            RegisterCommand(Guild_GuildMemberPos.TagName, () => new MinimapCMD());

            //角色模块通知
            RegisterCommand(Role_RoleNotify.TagName, () => new RoleNotify());
            //资源运输
            RegisterCommand(Transport_GetTransport.TagName, () => new AssitResCMD());
            RegisterCommand(Transport_CreateTransport.TagName, () => new AssitResCMD());
            RegisterCommand(Transport_TransportSuccess.TagName, () => new AssitResCMD());
            //战争预警
            RegisterCommand(Role_EarlyWarningInfo.TagName, () => new WarWarningCMD());
            //跑马灯
            RegisterCommand(Chat_MarqueeNotify.TagName,()=>new ScrollMessageCMD());
            
            //充值
            RegisterCommand(Recharge_RechargeInfo.TagName, () => new RechargeCMD());
            RegisterCommand(Map_MoveCity.TagName, () => new MoveCityCMD());
            RegisterCommand(CmdConstant.EventTracking, () => new EventTrackingCMD());

            //地图书签
            RegisterCommand(Map_AddMarker.TagName, () => new MapMarkerCMD());
            RegisterCommand(Map_ModifyMarker.TagName, () => new MapMarkerCMD());
            RegisterCommand(Map_DeleteMarker.TagName, () => new MapMarkerCMD());
            RegisterCommand(CmdConstant.PersonMapMarkerInfoChanged, () => new MapMarkerCMD());
            RegisterCommand(CmdConstant.GuildMapMarkerInfoChanged, () => new MapMarkerCMD());
        }

        public void StartUp()
        {
            SendNotification(STARTUP);
        }

        public override void RegisterMediator(IMediator mediator)
        {
            ViewBinder vb = null;
            if (mediator.ViewComponent != null)
            {
                vb = ((GameObject) mediator.ViewComponent).GetComponent<ViewBinder>();
            }

            if (vb != null)
            {
                m_viewMap[vb] = mediator;
            }

            if (!m_MediatorMap.ContainsKey(mediator.MediatorName))
            {
                m_MediatorMap[mediator.MediatorName] = mediator;
            }

            base.RegisterMediator(mediator);
        }

        public override IMediator RetrieveMediator(string mediatorName)
        {
            if (m_MediatorMap.ContainsKey(mediatorName))
            {
                return m_MediatorMap[mediatorName];
            }

            return base.RetrieveMediator(mediatorName);
        }

        public override IMediator RemoveMediator(string mediatorName)
        {
            if (m_MediatorMap.ContainsKey(mediatorName))
            {
                m_MediatorMap.Remove(mediatorName);
            }

            return base.RemoveMediator(mediatorName);
        }

        public void RemoveView(ViewBinder view)
        {
            if (m_viewMap.ContainsKey(view))
            {
                IMediator md = m_viewMap[view];
                AppFacade.GetInstance().RemoveMediator(md.MediatorName);
            }
        }

        public void RegisterGlobalMediator(Mediator mediator)
        {
            m_globalMap.Add(mediator.MediatorName, mediator);
            base.RegisterMediator(mediator);
        }

        public void RemoveGlobalMediator(string mediatorName)
        {
            if (m_globalMap.ContainsKey(mediatorName))
            {
                Mediator md = m_globalMap[mediatorName];
                AppFacade.GetInstance().RemoveMediator(md.MediatorName);
                m_globalMap.Remove(mediatorName);
            }
        }

        public override void SendNotification(string notificationName, object body = null, string type = null)
        {
            base.SendNotification(notificationName, body, type);
            SubViewManager.Instance.NotifyObervers(new Notification(notificationName, body, type));
        }

        public void SendSproto(SprotoTypeBase obj)
        {
            var net = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
            net.SendSproto(obj);
        }

        private void OnShowUI(UIInfo ui)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.OnShowUI, ui);
        }

        private void OnCloseUI(UIInfo ui)
        {
            AppFacade.GetInstance().SendNotification(CmdConstant.OnCloseUI, ui);
        }
    }
}
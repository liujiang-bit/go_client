using Client;
using Data;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using System;
using UnityEngine;

namespace Game
{
    public class HeroCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            //m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            switch (notification.Name)
            {
                case Hero_HeroInfo.TagName:
                    var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                    heroProxy.UpdateHeroInfo(notification);
                    break;
                case CmdConstant.GetNewHero:
                    {
                        CoreUtils.uiManager.CloseUI(UI.s_gameTool);
                        long heroId = (long)notification.Body;

                        //判断一下是否需要引导剧情表现
                        var m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
                        Int64 civilization = m_playerProxy.GetCivilization();
                        CivilizationDefine define = CoreUtils.dataService.QueryRecord<CivilizationDefine>((int)civilization);
                        //Debug.LogErrorFormat("heroId:{0} initId:{1}", heroId, define.initialHero);
                        if (heroId == define.initialHero)
                        {
                            AppFacade.GetInstance().SendNotification(CmdConstant.FirstGetHeroGuide);
                            return;
                        }

                        CoreUtils.uiManager.ShowUI(UI.s_captainSummon, null, heroId);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
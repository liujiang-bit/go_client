using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;


namespace Game
{
    public class PlayerAttributeCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            var playerAttributeProxy = AppFacade.GetInstance().RetrieveProxy(PlayerAttributeProxy.ProxyNAME) as PlayerAttributeProxy;

            switch (notification.Name)
            {
                case CmdConstant.GuildOfficerInfoChange:
                    playerAttributeProxy.UpdateGuildOfficerInfo(notification.Body as GuildOfficerInfoEntity);
                    break;
                case CmdConstant.technologyChange:
                    playerAttributeProxy.UpdateTechInfo(notification.Body as Dictionary<Int64, TechnologyInfo>);
                    break;
                case CmdConstant.CityBuildinginfoFirst:
                    {
                        playerAttributeProxy.UpdateCityBuildInfo(notification.Body as List<BuildingInfoEntity>);
                        break;
                    }
                case Guild_GuildTechnologies.TagName:
                    {
                        Guild_GuildTechnologies.request request =  notification.Body as Guild_GuildTechnologies.request;
                        if (request!=null)
                        {
                            if (request.technologies != null)
                            {
                                playerAttributeProxy.UpdateAllianceTechInfo(request.technologies);
                            }
  
                        }
                
                        break;
                    }
                case CmdConstant.AllianceEixtEx:
                    playerAttributeProxy.UpdateAllianceTechInfo();
                    playerAttributeProxy.UpdateGuildOfficerInfo();
                    break;
                case CmdConstant.CityBuildinginfoChange:
                    {
                        playerAttributeProxy.UpdateCityBuildInfo(notification.Body as List<BuildingInfoEntity>);
                        break;
                    }
                case CmdConstant.UpdatePlayerCountry:
                    {
                        playerAttributeProxy.UpdateCivilization((long)notification.Body);
                        break;
                    }
                case CmdConstant.UpdateHero:
                    {
                        playerAttributeProxy.UpdateHeroInfo(notification.Body as HeroInfoEntity);
                        break;
                    }
                case CmdConstant.VipLevelUP:
                    {
                        playerAttributeProxy.UpdateVip((long)notification.Body);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}


using System.Collections;
using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;
using Data;
using UnityEngine.UI;

namespace Game
{
    public class CityBuffCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
      
            switch (notification.Name)
            {
                case CmdConstant.CityBuffChange:
                    {
                  //     Debug.LogError("CityBuffChange");
                        CityBuffProxy m_cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
                        m_cityBuffProxy.UpdateCityBuff();

                    }
                    break;
                default: break;
            }
        }
    }
}


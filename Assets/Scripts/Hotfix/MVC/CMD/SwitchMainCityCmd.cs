using Client;
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class SwitchMainCityCmd : GameCmd
    {
        public override void Execute(INotification notification)
        {
            
            Shader.SetGlobalFloat("_CityTransparency", 1);

            ClientUtils.lightingManager.SetCameraFillBaseColor(new Color(0.72f, 0.6f, 0.36f, 1));
//            ClientUtils.lightingManager.UpdateLighting(new Color(0.6470588f, 0.6470588f, 0.6470588f), new Color(0.54509803921569f, 0.30980392156863f, 0.14117647058824f), 0.85f, new Color(0.6470588f, 0.6470588f, 0.6470588f), new Color(0.54509803921569f, 0.30980392156863f, 0.14117647058824f), 0.85f, 0.0f, 0.0f);
            ClientUtils.mapManager.SetMapWidth(7200 / 180);
            //策划要求暂时关闭
            ClientUtils.weatherMgr.StartUpdateRain(false);
            CoreUtils.assetService.Instantiate("cloudManager", null);

            AppFacade.GetInstance().SendNotification(CmdConstant.DayNightInit);
            AppFacade.GetInstance().SendNotification(CmdConstant.GuideInitData);
            AppFacade.GetInstance().SendNotification(CmdConstant.DayNightTimeTick);

            ClientUtils.mapManager.ReadMapBriefDataFromFile2("map_4_data", 0, 0, () =>
            {
                ClientUtils.mapManager.ReadMapDataFromFile2("tile_data", () =>
                {
                    PlayerProxy.LoginInitIsFinish = true;
                    Debug.Log("迷雾地表加载完毕");
                    AppFacade.GetInstance().SendNotification(CmdConstant.LoginInitFinish);
                });
            });
        }
    }
}
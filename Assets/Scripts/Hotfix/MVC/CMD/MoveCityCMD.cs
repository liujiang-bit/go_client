// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月28日
// Update Time         :    2020年6月28日
// Class Description   :    MoveCityCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using SprotoType;
using Skyunion;

namespace Game {
    public class MoveCityCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case Map_MoveCity.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            switch ((ErrorCode)@error.errorCode)
                            {
                                case ErrorCode.MAP_MOVE_CITY_CANT_ARRIVE:
                                    Tip.CreateTip(770009).Show();
                                    break;
                                case ErrorCode.MAP_MOVE_CITY_BUILD_INVALID:
                                    Tip.CreateTip(732011).Show();
                                    break;
                                case ErrorCode.MAP_MOVE_CITY_NO_GUILD_TERRITORY:
                                    Tip.CreateTip(770007).Show();
                                    break;
                                case ErrorCode.MAP_MOVE_CITY_RALLY_ARMY:
                                    Tip.CreateTip(770002).Show();
                                    break;
                                case ErrorCode.MAP_MOVE_CITY_DENSEFOG:
                                    Tip.CreateTip(770005).SetStyle(Tip.TipStyle.Middle).Show();
                                    break;
                                case ErrorCode.MAP_MOVE_CITY_HOLYLAND_TERRITORY:
                                    Tip.CreateTip(770008).SetStyle(Tip.TipStyle.Middle).Show();  
                                    break;
                            }
                        }
                        else
                        {

                        }
                        CoreUtils.uiManager.CloseUI(UI.s_moveCity);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
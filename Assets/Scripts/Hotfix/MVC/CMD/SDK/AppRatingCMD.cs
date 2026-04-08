using IGGSDKConstant;
using Newtonsoft.Json;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AppRatingCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.OpenAppRating:
                    {
                        IGGSDK.shareInstance().getAppRating().requestReview(OnDisabled, OnError, onMinimizedModeEnabled, onStarndardModeEnabled);
                    }
                    break;
            }
        }

        private void OnDisabled(IGGAppRatingStatus status)
        {
            Debug.Log("AppRatingStatus:" + status.getMode().ToString());
        }
        private void OnError(IGGException exception)
        {
            Debug.LogError(exception.ToString());
        }

        private void onMinimizedModeEnabled(IGGMinimizedAppRating rating)
        {
            rating.goRating((IGGException ex) =>
            {
            });
        }

        private void onStarndardModeEnabled(IGGStarndardAppRating rating)
        {
            CoreUtils.uiManager.ShowUI(UI.s_EvaluateStar, null, rating);
        }
    }
}


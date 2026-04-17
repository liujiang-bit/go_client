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
                        SDKBridge.shareInstance().getAppRating().requestReview(OnDisabled, OnError, onMinimizedModeEnabled, onStarndardModeEnabled);
                    }
                    break;
            }
        }

        private void OnDisabled(SDKAppRatingStatus status)
        {
            Debug.Log("AppRatingStatus:" + status.getMode().ToString());
        }
        private void OnError(SDKException exception)
        {
            Debug.LogError(exception.ToString());
        }

        private void onMinimizedModeEnabled(SDKMinimizedAppRating rating)
        {
            rating.goRating((SDKException ex) =>
            {
            });
        }

        private void onStarndardModeEnabled(SDKStandardAppRating rating)
        {
            CoreUtils.uiManager.ShowUI(UI.s_EvaluateStar, null, rating);
        }
    }
}

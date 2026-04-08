using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using System;
using UnityEngine;

namespace Game
{
    public class CameraCmd : GameCmd
    {
        public class Param
        {
            public float moveTime = 0.0f;
            public Vector2 terrainPos = Vector2.zero;
            public Action moveCallback = null;
            public float zoomTime = 0.0f;
            public float dfx = 0.0f;
            public Action zoomCallback = null;
            public string cameraId = string.Empty;
            public bool autoLock = true;
        };  
        public override void Execute(INotification notification)
        {
            Param param = notification.Body as Param;
            if (param == null)
                return;

            if (param.autoLock)
            {
                WorldCamera.Instance().SetCanDrag(false);
                WorldCamera.Instance().SetCanZoom(false);
                WorldCamera.Instance().SetCanClick(false);
                Timer.Register(param.moveTime + param.zoomTime+1.0f/Application.targetFrameRate, () =>
                {
                    WorldCamera.Instance().SetCanDrag(true);
                    WorldCamera.Instance().SetCanZoom(true);
                    WorldCamera.Instance().SetCanClick(true);
                });
            }

            if (param.terrainPos.Equals(Vector2.zero))
            {
                if(!string.IsNullOrEmpty(param.cameraId))
                {
                    param.dfx = WorldCamera.Instance().getCameraDxf(param.cameraId);
                }

                WorldCamera.Instance().SetCameraDxf(param.dfx, param.zoomTime, () =>
                {
                    param.zoomCallback?.Invoke();
                });
            }
            else
            {
                WorldCamera.Instance().ViewTerrainPos(param.terrainPos.x, param.terrainPos.y, param.moveTime, () =>
                {
                    param.moveCallback?.Invoke();

                    if (!string.IsNullOrEmpty(param.cameraId))
                    {
                        param.dfx = WorldCamera.Instance().getCameraDxf(param.cameraId);
                    }
                    WorldCamera.Instance().SetCameraDxf(param.dfx, param.zoomTime, () =>
                    {
                        param.zoomCallback?.Invoke();
                    });
                });
            }
        }
    }
}
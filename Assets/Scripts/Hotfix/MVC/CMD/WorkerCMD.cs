using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class WorkerCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.buildQueueChange:
                    WorkerProxy workerProxy = AppFacade.GetInstance().RetrieveProxy(WorkerProxy.ProxyNAME) as WorkerProxy;
                    workerProxy.UpdateBuildQueue();
                    break;
                default: break;
            }
        }
    }
}
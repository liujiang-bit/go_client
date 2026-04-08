using Client;
using PureMVC.Interfaces;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class ServerTimeCommand : GameCmd
    {
        public override void Execute(INotification notification)
        {
            ServerTimeModule.Instance.Execute(notification.Body);
        }
    }
}
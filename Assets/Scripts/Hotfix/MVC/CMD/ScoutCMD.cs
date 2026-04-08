using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    public class ScoutCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch(notification.Name)
            {
                case Map_Scouts.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage) notification.Body;
                            switch ((ErrorCode) @error.errorCode)
                            {
                                case ErrorCode.SCOUTS_TARGET_NOT_EXIST:
                                    Tip.CreateTip(200001).Show();
                                    break;
                                case ErrorCode.SCOUTS_SCOUTSING_BUSY:
                                    Tip.CreateTip(181277).SetStyle(Tip.TipStyle.Middle).Show();
                                    break;
                                default:
                                    break;
                            }

                        }
                        else
                        {
                            CoreUtils.audioService.PlayOneShot(RS.CreateScoutSound);
                        }

                        
                    }
                    break;
                case Map_ScoutsBack.TagName:
                    {
                        CoreUtils.audioService.PlayOneShot(RS.CreateScoutSound);
                    }
                    break;
                default:break;
            }
        }
    }
}


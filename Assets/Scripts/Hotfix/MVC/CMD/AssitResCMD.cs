// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月24日
// Update Time         :    2019年12月24日
// Class Description   :    TroopsCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using PureMVC.Interfaces;
using UnityEngine;
using PureMVC.Patterns;
using SprotoType;
using Skyunion;
using Data;

namespace Game {
    public class AssitResCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case Transport_GetTransport.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            switch ((ErrorCode)@error.errorCode)
                            {
                                case ErrorCode.TRANSPORT_TARGET_NOT_BUILDING:
                                    Tip.CreateTip(184033).Show();
                                    break;
                            }
                        }
                        else
                        {
                            Transport_GetTransport.response response = notification.Body as Transport_GetTransport.response;
                            if (response != null)
                            {
                                if (response.status)
                                {
                                    CoreUtils.uiManager.ShowUI(UI.s_assitRes, null, response);
                                }
                                else
                                {
                                    Tip.CreateTip(184032, Tip.TipStyle.Middle).Show();
                                }

                            }
                        }

                    }
                    break;
                case Transport_TransportSuccess.TagName:
                    Transport_TransportSuccess.request request = notification.Body as Transport_TransportSuccess.request;
                    if (request != null)
                    {
                        if (request.type == 1)
                        {
                            Tip.CreateTip(184034).Show();
                        }else if (request.type == 2)
                        {
                            Tip.CreateTip(184035).Show();    
                        }
                    }
                    break;
                case Transport_CreateTransport.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            switch ((ErrorCode)@error.errorCode)
                            {
                                case ErrorCode.TRANSPORT_TARGET_NOT_BUILDING:
                                    Tip.CreateTip(184033).Show();
                                    break;
                            }
                        }
                        else
                        {
                            Transport_CreateTransport.response response = notification.Body as Transport_CreateTransport.response;
                            if (response != null)
                            {
                              

                            }
                        }

                    }
                    break;

                default: break;
            }

        }
    }
}
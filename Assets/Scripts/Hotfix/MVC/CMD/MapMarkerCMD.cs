// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年9月15日
// Update Time         :    2020年9月15日
// Class Description   :    MapMarkerCMD
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections;
using System.Collections.Generic;
using Client;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using UnityEngine;
using Data;

namespace Game {
    public class MapMarkerCMD : GameCmd
    {
        public override void Execute(INotification notification)
        {
            MapMarkerProxy m_mapMarkerProxy = AppFacade.GetInstance().RetrieveProxy(MapMarkerProxy.ProxyNAME) as MapMarkerProxy;

            switch (notification.Name)
            {
                case Map_AddMarker.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            switch ((ErrorCode)@error.errorCode)
                            {
                                case ErrorCode.GUILD_NO_JURISDICTION:
                                    Tip.CreateTip(730136).Show();
                                    break;
                                case ErrorCode.MAP_PERSON_MARKER_LIMIT:
                                    Tip.CreateTip(910001).Show();
                                    break;
                            }
                        }
                        else
                        {
                            if (notification.Body is Map_AddMarker.response)
                            {
                                Map_AddMarker.response response = notification.Body as Map_AddMarker.response;
                                if (response.HasMarkerId)
                                {
                                    MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)response.markerId);
                                    if (mapMarkerTypeDefine != null)
                                    {
                                        if (mapMarkerTypeDefine.type == 1)
                                        {
                                            Tip.CreateTip(910002).Show();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case Map_ModifyMarker.TagName:
                    {
                        if (notification.Body is Map_ModifyMarker.response)
                        {
                            Map_ModifyMarker.response response = notification.Body as Map_ModifyMarker.response;
                            if (response.HasMarkerId)
                            {
                                MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)response.markerId);
                                if (mapMarkerTypeDefine != null)
                                {
                                    if (mapMarkerTypeDefine.type == 1)
                                    {
                                        Tip.CreateTip(910003).Show();
                                    }
                                }
                            }
                        }
                    }
                    break;
                case Map_ModifyGuildMarker.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        switch ((ErrorCode)@error.errorCode)
                        {
                            case ErrorCode.GUILD_NO_JURISDICTION:
                                Tip.CreateTip(730136).Show();
                                break;
                        }
                    }
                    break;
                case Map_DeleteMarker.TagName:
                    {
                        if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                        {
                            ErrorMessage error = (ErrorMessage)notification.Body;
                            switch ((ErrorCode)@error.errorCode)
                            {
                                case ErrorCode.GUILD_NO_JURISDICTION:
                                    Tip.CreateTip(730136).Show();
                                    break;
                            }
                        }
                        else
                        {
                            if (notification.Body is Map_DeleteMarker.response)
                            {
                                Map_DeleteMarker.response response = notification.Body as Map_DeleteMarker.response;
                                if (response.HasMarkerId)
                                {
                                    MapMarkerTypeDefine mapMarkerTypeDefine = CoreUtils.dataService.QueryRecord<MapMarkerTypeDefine>((int)response.markerId);
                                    if (mapMarkerTypeDefine != null)
                                    {
                                        if (mapMarkerTypeDefine.type == 1)
                                        {
                                            Tip.CreateTip(910004).Show();
                                        }
                                    }
                                }
                            }
                        }                        
                    }
                    break;
                case CmdConstant.PersonMapMarkerInfoChanged:
                    {
                        m_mapMarkerProxy.UpdatePersonMapMarkerInfo(notification.Body as Dictionary<long, MapMarkerInfo>);
                    }
                    break;
                case CmdConstant.GuildMapMarkerInfoChanged:
                    {
                        if (notification.Body is Guild_MapMarkers.request)
                        {
                            Guild_MapMarkers.request request = notification.Body as Guild_MapMarkers.request;
                            if (request.HasMarkers && request.markers != null)
                            {
                                m_mapMarkerProxy.UpdateGuildMapMarkerInfo(request.markers);
                            }
                        }
                    }
                    break;
                default: break;
            }
        }
    }
}
//
// GameApp.cs
// Create:
//      2020-2-12
// Description:
//      广告服务接口
// Author:
//      吴江海 <421465201@qq.com>
//
// Copyright (c) 2019 Johance

using UnityEngine;

namespace Skyunion
{
    public enum Channel
    {
        None = 0,
        Facebook = 0x01,
        Appsflyer = 0x02,
        Firebase = 0x04,
        All = 0xffffff,
    }
    public interface IADService : IModule
    {
        void SendEvent(string key, string value, Channel channel = Channel.All);
        void SetupGameID(string strGameId);
        void OnFetchIGGID(string strIggId);
        void SetCharacterID(string characterId);
    }
}

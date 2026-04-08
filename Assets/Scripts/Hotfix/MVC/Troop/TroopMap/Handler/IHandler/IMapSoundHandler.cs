using System;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public interface IMapSoundHandler: IBaseHandler
    {
        void AddMapSound(RssType type);
        void RemoveSound(RssType type);
        void PlayTouchMyCity();
    }
}
using UnityEngine;

namespace Hotfix
{
    public interface IMapTouchHandler:IBaseHandler
    {
        void Play(int objectId, Vector2 pos);
        void Play(Vector2 pos ,Vector2 targetPos);
        void Stop();
        void StopSpace();
    }
}
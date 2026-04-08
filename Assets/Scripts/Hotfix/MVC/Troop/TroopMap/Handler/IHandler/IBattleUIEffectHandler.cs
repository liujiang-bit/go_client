using UnityEngine;

namespace Hotfix
{
    public interface IBattleUIEffectHandler :IBaseHandler
    {
        void Play(int id, string name, Transform transform);
        void Stop(int id);
        void Remove(int id);
    }
}
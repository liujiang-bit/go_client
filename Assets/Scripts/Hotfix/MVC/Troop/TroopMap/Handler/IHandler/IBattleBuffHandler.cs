using UnityEngine;

namespace Hotfix
{
    public interface IBattleBuffHandler :IBaseHandler
    {
        void CreateBuffGo(int id, int buffid, Transform parent);
        void ClearBuff(int id);
    }
}
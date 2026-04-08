namespace Hotfix
{
    public interface IMapSelectEffectHandler : IBaseHandler
    {
        void Play(int id, int targetId);
        void Remove();
    }
}
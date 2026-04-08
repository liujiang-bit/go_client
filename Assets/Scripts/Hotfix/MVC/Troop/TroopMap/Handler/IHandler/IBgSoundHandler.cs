namespace Hotfix
{
    public interface IBgSoundHandler : IBaseHandler
    {
        void WantChangeBGMOnBehaviorStateChange(int id);
    }
}
namespace Hotfix
{
    public interface IBattleBroadcastsHandler :IBaseHandler
    {
        void Show(int id,object parm);
        void Hide(int id);
    }
}
using PureMVC.Interfaces;
using SprotoType;

namespace Hotfix
{
    public interface IUpdateMapDataHandler: IBaseHandler
    {
        void UpdateMapData(INotification notification);
        void UpdateLastTroopId(long troopId);
        void InitMapData(Map_ObjectInfo.request mapItemInfo);
    }
}
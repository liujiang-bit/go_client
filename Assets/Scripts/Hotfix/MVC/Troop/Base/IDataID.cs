using System.Collections.Generic;

namespace Hotfix
{
    public interface IDataID
    {
        List<int> GetListId();
        long GetSoldiersCount();
        void SetListSort(int sortType);
        List<int> GetSortListId();
    }
}
using System.Collections.Generic;
using Game;

namespace Hotfix
{
    public interface ITroopSave
    {
        void InsertSave(int id, int type,int heroId, int viceId, Dictionary<int, int> solds,bool isSerialization=true);
        void DeleteSave(int id);
        void UpdateSave(int id, int heroId, int viceId, Dictionary<int, int> solds);
        void RestSave();
        void UpdateAllSave(object parm);
        bool GetIsDelete();
        void SetSaveType(TroopSaveType type);
        TroopSaveType GetSaveType();
        void DeletSaveType(TroopSaveNumType type, object parm);
        object GetLsSave(TroopSaveNumType type, int id);
        int GetLsSaveCount(TroopSaveNumType type);
        bool IsContainsData(TroopSaveNumType type);
        void SetSelect(TroopSaveNumType type,int id);
        object GetNextTroopSaveNumType(TroopSaveNumType troopSaveNumType);
        object GetSaveDataByType(TroopSaveNumType type);
    }
}
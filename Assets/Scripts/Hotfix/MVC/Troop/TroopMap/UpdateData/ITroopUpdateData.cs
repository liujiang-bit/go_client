namespace Hotfix
{
    public interface ITroopUpdateData : IBaseUpdateData, IClearData
    {
        void UpdateCollectRuneTime(int id, long time);
        void UpdateTroopSoldiers(int id, object parm);
        void UpdateGuildAddName(int id);
        void UpdateAttackCount(int id, int num);
        void SetAOITroopLines(int id);
        void UpdateAOITroopLines(int id);
        void UpdateAOITroopLinesColor();
        void DeleteAOITroopLines(int id);
        void UpdateHeroLevel(int id, int heroId);
    }
}
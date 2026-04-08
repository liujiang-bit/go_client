namespace Hotfix
{
    public interface IScoutAndTroop
    {
        int GetScountAndTroopCount();
        object GetScoutAndTroopData(int index);
        void SetGray(bool gray);
        object GetTroopData();
    }
}
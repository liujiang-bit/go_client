namespace Hotfix
{
    public interface IBuildingFightUpdateData: IBaseUpdateData
    {
        void UpdateHpMax(int id, int hpMax);
        void UpdateHead(int id);

    }
}
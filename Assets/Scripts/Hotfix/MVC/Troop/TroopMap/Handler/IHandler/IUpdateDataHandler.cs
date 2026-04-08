namespace Hotfix
{
    public interface IUpdateDataHandler:IBaseHandler
    {
        ITroopUpdateData UpdateTroopData();
        IMonsterUpdateData UpdateMonsterData();
        IBuildingFightUpdateData UpdateBuildingFightData();
    }
}
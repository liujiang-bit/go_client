namespace Hotfix
{
    public interface IMapBuildingFightHandler:IBaseHandler
    {
        void PlaySkills(int id, int targetId, int skillId);
        void StopSkill(int id);
        void PlayBuildingHud(int id);
        void StopBuildingHud(int id);
        void UpdateWorldHud(long id, bool isFight);
        void PlayBurning(int id);
        void StopBurning(int id);
    }
}
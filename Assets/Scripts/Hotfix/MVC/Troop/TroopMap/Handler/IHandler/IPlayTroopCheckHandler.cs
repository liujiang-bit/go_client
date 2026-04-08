namespace Hotfix
{
    public interface IPlayTroopCheckHandler: IBaseHandler
    {
        bool isHaveFight();
        bool isHaveRun();
        bool isHaveCollect();
        bool isHaveStationing();
        bool isHavePlayMarch();
        bool isHaveScoutMap();
        bool isHaveFightCity();
        bool isHaveMap();
        bool isHaveRally();
    }
}
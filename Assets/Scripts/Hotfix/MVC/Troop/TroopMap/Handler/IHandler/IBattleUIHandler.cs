namespace Hotfix
{
    public interface IBattleUIHandler:IBaseHandler
    {
        void SetBattleUIData(int id, BattleUIType type, object parm1, object parm2 = null);
        void PushBattleBuff(int id, int buffId);
        void Remove(int id);
    }
}
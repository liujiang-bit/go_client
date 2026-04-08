using Client;
using UnityEngine;

namespace Hotfix
{
    public interface IGuardianHandler: IBaseHandler
    {
        void SetStates(Guardian self, Troops.ENMU_SQUARE_STAT state, Vector2 current_pos,
            Vector2 target_pos, float move_speed = 2f);

        void FadeOut_S(Guardian self);

        void TriggerSkillS(Guardian self, string param);
        Guardian GetFormationGuardian(int id);
    }
}
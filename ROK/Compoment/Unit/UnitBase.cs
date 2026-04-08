using System;

namespace ROK
{
    public class UnitBase : LodBase
    {
        public enum MOVE_STATE
        {
            STATIC,
            CHASE,
            UNBOUND,
            NUMBER = 4
        }

        public enum CHASE_MODE
        {
            ROTATE_SPRITE,
            ORIGIN_SPRITE,
            ORIGIN_MODEL
        }

        private int m_unit_type = -1;

        private string m_clone_to_prefab = string.Empty;

        public int unitType
        {
            get
            {
                return this.m_unit_type;
            }
            set
            {
                this.m_unit_type = value;
            }
        }

        public string cloneToPrefab
        {
            get
            {
                return this.m_clone_to_prefab;
            }
            set
            {
                this.m_clone_to_prefab = value;
            }
        }

        public virtual void SetDummy(UnitDummy dummy)
        {
        }

        public virtual void InitDummy(UnitDummy dummy)
        {
        }

        public virtual void SetMoveSpeedForce(float moveSpeed)
        {
        }

        public virtual void SetMoveSpeed(float moveSpeed)
        {
        }

        public virtual void ChangeMoveState(UnitBase.MOVE_STATE state, bool isMoveAtk = false, float delay_chase_time = 0f, float delay_chase_speed = 1f)
        {
        }

        public virtual void SetSpriteLoigicalState(TextureSheetAnimation.ENMU_SPRITE_STATE state)
        {
        }

        public virtual void PlayDeadParticle()
        {
        }

        public virtual void SetLevelupEffect(string effectName)
        {
        }

        public virtual void FadeIn()
        {
        }

        public virtual void FadeOut()
        {
        }

        public virtual void SetChaseMode(UnitBase.CHASE_MODE chase_mode)
        {
        }

        public virtual void PlaySpriteAniOnce(TextureSheetAnimation.ENMU_SPRITE_STATE state)
        {
        }
    }
}
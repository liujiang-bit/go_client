using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// 守卫者布阵  挂载troop/BarbarianSlayerBoss
    /// </summary>
    public class Guardian : LevelDetailBase
    {
        [Serializable]
        public class AtkEffectParam
        {
            public float disMin;

            public float disMax;

            public float width;

            public float roundTrigger = 1f;

            public int roundInterval = 500;

            public int roundMin = 3;

            public int roundMax = 5;

            public int singleIntervalMin = 50;

            public int singleIntervalMax = 100;
        }

        public class AtkSingleEffect
        {
            public float dis;

            public float width;

            public float delay;
        }

        public float m_square_pos_offset;

        private Vector2 m_start_pos = Vector2.zero;

        private Vector2 m_target_pos = Vector2.zero;

        private float m_move_timer;

        private float m_move_time;

        private float m_move_speed = 1f;

        public float m_set_around_speed_factor = 0.9f;

        private Troops.ENUM_FORMATION_CAMP m_camp;

        private string m_movableStr = "moved";

        public Troops.ENMU_SQUARE_STAT m_formation_state;

        private Troops.ENMU_MATRIX_TYPE m_formation_type;

        public string m_beHitEffect = string.Empty;

        public string m_beHitEffectEnemy = string.Empty;

        public string m_curBeHitEffect = string.Empty;

        public AtkEffectParam m_atk_effect_param;

        private bool m_atk_effect_launch;

        private float m_atk_effect_dur_time;

        private float m_atk_effect_next_round_interval;

        private bool m_atk_effect_this_round_trigger = true;

        private List<AtkSingleEffect> m_atk_effect_array = new List<AtkSingleEffect>();

        private GameObject m_move_smoke;

        public string m_move_smoke_particle_str;

        public int max_row_number = 5;

        private float m_formation_radius;

        private GuardianFormationMap m_gfm;

        private Dictionary<int, Dictionary<int, int[]>> m_squareInfo = new Dictionary<int, Dictionary<int, int[]>>();

        private List<int> m_heros = new List<int>();

        private int m_loadFrame;

        private bool m_lastMoveAtk;

        private Troops.ENMU_SQUARE_STAT m_curState;

        private GuardianFormationMap GetGFM()
        {
            if (m_gfm == null)
            {
                m_gfm = GetComponent<GuardianFormationMap>();
            }
            return m_gfm;
        }

        public static void SetRadiusS(Guardian self, float r)
        {
            if (self != null)
            {
                self.SetRadius(r);
            }
        }

        public void SetRadius(float r)
        {
            m_formation_radius = r;
            if ((bool)m_move_smoke)
            {
                Vector3 localScale = m_move_smoke.transform.localScale;
                localScale.x = r;
                localScale.y = r;
                localScale.z = r;
                m_move_smoke.transform.localScale = localScale;
            }
        }

        private void Reset()
        { 
            m_loadFrame = Time.frameCount;
        }

        private void ParseFormationData(string square_info)
        {
            string[] array = square_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
            array[1].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                int num = int.Parse(array2[0]);
                string[] array3 = array2[1].Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<int, int[]> dictionary;
                if (m_squareInfo.ContainsKey(num))
                {
                    dictionary = m_squareInfo[num];
                }
                else
                {
                    dictionary = new Dictionary<int, int[]>();
                    m_squareInfo.Add(num, dictionary);
                }
                for (int j = 0; j < array3.Length; j++)
                {
                    string[] array4 = array3[j].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                    int num2 = int.Parse(array4[0]);
                    if (num == 0)
                    {
                        m_heros.Insert(j, num2);
                    }
                    int[] array5;
                    if (dictionary.ContainsKey(num2))
                    {
                        array5 = dictionary[num2];
                    }
                    else
                    {
                        array5 = new int[2];
                        dictionary.Add(num2, array5);
                    }
                    array5[0] = int.Parse(array4[1]);
                    array5[1] = int.Parse(array4[2]);
                }
            }
        }

        private void LoadUnit()
        {
            GetGFM().LoadUnit();
            GetGFM().SetHeroUnit(m_heros);
        }

        public Troops.ENMU_MATRIX_TYPE GetFormationType()
        {
            return m_formation_type;
        }

        private void InitFormation(string formation_info, Color unit_color)
        {
            Reset();
            ParseFormationData(formation_info);
            GetGFM().SetUnitColor(unit_color);
            LoadUnit();
        }

        public static void InitFormationS(Guardian self, string formation_info, Color unit_color)
        {
            self.InitFormation(formation_info, unit_color);
        }

        private Vector3 GetShowPosition()
        {
            return base.gameObject.transform.position;
        }

        public static Vector3 GetShowPositionS(Guardian self)
        {
            return self.GetShowPosition();
        }

        public static void SetStateS(Guardian self, Troops.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f)
        {
            self.SetState(state, current_pos, target_pos, move_speed);
        }

        public static void SetCampS(Guardian self, Troops.ENUM_FORMATION_CAMP camp)
        {
            if (self != null)
            {
                self.SetCamp(camp);
            }
        }

        private void SetCamp(Troops.ENUM_FORMATION_CAMP camp)
        {
            m_camp = camp;
            if (m_camp == Troops.ENUM_FORMATION_CAMP.Enemy)
            {
                m_curBeHitEffect = m_beHitEffect;
            }
            else
            {
                m_curBeHitEffect = m_beHitEffectEnemy;
            }
        }

        public static void SetBeAtkEffectCampS(Guardian self, Troops.ENUM_FORMATION_CAMP camp)
        {
            if (self != null)
            {
                self.SetBeAtkEffectCamp(camp);
            }
        }

        private void SetBeAtkEffectCamp(Troops.ENUM_FORMATION_CAMP camp)
        {
            if (m_camp == Troops.ENUM_FORMATION_CAMP.Enemy)
            {
                m_curBeHitEffect = m_beHitEffect;
            }
            else
            {
                m_curBeHitEffect = m_beHitEffectEnemy;
            }
        }

        public static void TriggerSkillS(Guardian self, string param)
        {
            if (self != null)
            {
                self.TriggerSkill(param);
            }
        }

        private void TriggerSkill(string param)
        {
            GetGFM().m_unitNoTex.GetComponent<CellNoTex>().PlaySkillAni();
            GetGFM().m_unitHero.GetComponent<CellBase>().PlaySpriteAniOnce(AnimationBase.ENMU_SPRITE_STATE.FIGHT);
        }

        public static void SetTargetMovableS(Guardian self, string moved)
        {
            if (self != null)
            {
                self.SetTargetMovable(moved);
            }
        }

        private void SetTargetMovable(string moved)
        {
            m_movableStr = moved;
        }

        public static void ReservedFunc1S(Guardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc1(param);
            }
        }

        private void ReservedFunc1(string param)
        {
        }

        public static void ReservedFunc2S(Guardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc2(param);
            }
        }

        private void ReservedFunc2(string param)
        {
        }

        public static void ReservedFunc3S(Guardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc3(param);
            }
        }

        private void ReservedFunc3(string param)
        {
        }

        public static void ReservedFunc4S(Guardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc4(param);
            }
        }

        private void ReservedFunc4(string param)
        {
        }

        public static void ReservedFunc5S(Guardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc5(param);
            }
        }

        private void ReservedFunc5(string param)
        {
        }

        public static void ReservedFunc6S(Guardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc6(param);
            }
        }

        private void ReservedFunc6(string param)
        {
        }

        public void SetState(Troops.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f)
        {
            bool isMoveAtk = false;
            if (state == Troops.ENMU_SQUARE_STAT.FIGHT)
            {
                Vector3 b = new Vector3(current_pos.x, 0f, current_pos.y);
                if ((base.transform.position - b).magnitude > 0.07f)
                {
                    isMoveAtk = true;
                }
            }
            ChangeState(state, current_pos, target_pos, move_speed, isMoveAtk);
            base.transform.position = new Vector3(current_pos.x, 0f, current_pos.y);
            m_start_pos = current_pos;
            m_target_pos = target_pos;
            UnboundDummys();
            Vector2 normalized = (m_target_pos - m_start_pos).normalized;
            base.transform.forward = new Vector3(normalized.x, 0f, normalized.y);
            BoundDummys();
            SetDummysDir(base.transform.forward);
            ForceUnitHeroPlace();
            if (state == Troops.ENMU_SQUARE_STAT.MOVE)
            {
                m_move_timer = 0f;
                if (move_speed == 0f)
                {
                    Debug.LogWarning("Square : invalid move_speed");
                    move_speed = 1f;
                }
                m_move_speed = move_speed;
                m_move_time = (m_target_pos - m_start_pos).magnitude / m_move_speed;
            }
            UpdateAtkEffectState(state);
            m_formation_state = state;
        }

        private void BoundDummys()
        {
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                gFM.m_dummys[i].transform.SetParent(base.transform, worldPositionStays: true);
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                gFM.m_lodDummys[j].transform.SetParent(base.transform, worldPositionStays: true);
            }
        }

        private void UnboundDummys()
        {
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                gFM.m_dummys[i].transform.SetParent(null, worldPositionStays: true);
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                gFM.m_lodDummys[j].transform.SetParent(null, worldPositionStays: true);
            }
        }

        private void SetDummysDir(Vector3 dir)
        {
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                gFM.m_dummys[i].transform.forward = dir;
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                gFM.m_lodDummys[j].transform.forward = dir;
            }
        }

        private void ForceUnitHeroPlace()
        {
            GuardianFormationMap gFM = GetGFM();
            if (gFM != null && gFM.m_unitHero != null)
            {
                gFM.m_unitHero.transform.position = gFM.m_unitHeroDummy.transform.position;
                gFM.m_unitHero.GetComponent<CellBase>().ChangeMoveState(CellBase.MOVE_STATE.STATIC);
            }
        }

        private void ChangeState(Troops.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f, bool isMoveAtk = false)
        {
            Vector2 zero = Vector2.zero;
            bool flag = false;
            switch (state)
            {
                case Troops.ENMU_SQUARE_STAT.IDLE:
                    ChangeUnitChaseSpeed(m_move_speed);
                    ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE);
                    break;
                case Troops.ENMU_SQUARE_STAT.SET_AROUND:
                    ChangeUnitChaseSpeed(m_move_speed * m_set_around_speed_factor);
                    ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE);
                    break;
                case Troops.ENMU_SQUARE_STAT.MOVE:
                    if (move_speed == 0f)
                    {
                        Debug.LogWarning("Square : invalid move_speed");
                        move_speed = 1f;
                    }
                    m_move_speed = move_speed;
                    ChangeUnitChaseSpeed(m_move_speed);
                    ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE);
                    break;
                case Troops.ENMU_SQUARE_STAT.FIGHT:
                    if (isMoveAtk || m_lastMoveAtk != isMoveAtk)
                    {
                        m_lastMoveAtk = isMoveAtk;
                        ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE, isMoveAtk);
                    }
                    else if (!m_lastMoveAtk && !isMoveAtk && m_curState != state)
                    {
                        flag = false;
                        ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE, isMoveAtk);
                    }
                    else
                    {
                        flag = true;
                    }
                    break;
                case Troops.ENMU_SQUARE_STAT.DEAD:
                    ChangeUnitMoveState(CellBase.MOVE_STATE.UNBOUND);
                    break;
            }
            if (flag)
            {
                ChangeUnitMoveState(CellBase.MOVE_STATE.NUMBER); 
            }
            SetUnitLogicalState(state);
            m_curState = state;
        }

        public void ChangeUnitMoveState(CellBase.MOVE_STATE state, bool isMoveAtk = false)
        {
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i != gFM.m_dummys.Length; i++)
            {
                CellBase unit = gFM.m_dummys[i].GetComponent<CellClone>().m_unit;
                if ((bool)unit)
                {
                    unit.ChangeMoveState(state, isMoveAtk);
                }
            }
            for (int j = 0; j != gFM.m_lodDummys.Count; j++)
            {
                CellBase unit2 = gFM.m_lodDummys[j].GetComponent<CellClone>().m_unit;
                if ((bool)unit2)
                {
                    unit2.ChangeMoveState(state, isMoveAtk);
                }
            }
            if ((bool)gFM.m_unitHero)
            {
                gFM.m_unitHero.GetComponent<CellBase>().ChangeMoveState(state, isMoveAtk);
                gFM.m_unitHero.transform.position = gFM.m_unitHeroDummy.transform.position;
            }
            if ((bool)gFM.m_unitNoTex)
            {
                gFM.m_unitNoTex.GetComponent<CellBase>().ChangeMoveState(state, isMoveAtk);
                gFM.m_unitNoTex.transform.position = gFM.m_unitNoTexDummy.transform.position;
            }
        }

        private void SetUnitLogicalState(Troops.ENMU_SQUARE_STAT formation_state)
        {
            AnimationBase.ENMU_SPRITE_STATE unitSpriteState = GetUnitSpriteState(formation_state);
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i != gFM.m_dummys.Length; i++)
            {
                CellBase unit = gFM.m_dummys[i].GetComponent<CellClone>().m_unit;
                if ((bool)unit)
                {
                    unit.SetSpriteLoigicalState(unitSpriteState);
                    if (formation_state == Troops.ENMU_SQUARE_STAT.DEAD)
                    {
                        unit.PlayDeadParticle();
                    }
                }
            }
            for (int j = 0; j != gFM.m_lodDummys.Count; j++)
            {
                CellBase unit2 = gFM.m_lodDummys[j].GetComponent<CellClone>().m_unit;
                if ((bool)unit2)
                {
                    unit2.SetSpriteLoigicalState(unitSpriteState);
                    if (formation_state == Troops.ENMU_SQUARE_STAT.DEAD)
                    {
                        unit2.PlayDeadParticle();
                    }
                }
            }
            if (formation_state == Troops.ENMU_SQUARE_STAT.DEAD)
            {
                DestroyUnit();
            }
            if (gFM.m_unitHero != null)
            {
                CellBase component = gFM.m_unitHero.GetComponent<CellBase>();
                if (unitSpriteState == AnimationBase.ENMU_SPRITE_STATE.FIGHT)
                {
                    component.SetSpriteLoigicalState(AnimationBase.ENMU_SPRITE_STATE.MOVE);
                }
                else
                {
                    component.SetSpriteLoigicalState(unitSpriteState);
                }
                if (formation_state == Troops.ENMU_SQUARE_STAT.DEAD)
                {
                    component.PlayDeadParticle();
                }
            }
            if (gFM.m_unitNoTex != null)
            {
                CellBase component2 = gFM.m_unitNoTex.GetComponent<CellBase>();
                component2.SetSpriteLoigicalState(unitSpriteState);
                if (m_formation_state == Troops.ENMU_SQUARE_STAT.DEAD)
                {
                    component2.PlayDeadParticle();
                }
            }
        }

        private void DestroyUnit()
        {
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i < gFM.m_units.Count; i++)
            {
                if (gFM.m_units[i] != null)
                {
                    CoreUtils.assetService.Destroy(gFM.m_units[i]);
                }
            }
            gFM.m_units.Clear();
            if ((bool)gFM.m_unitHero)
            {
                CoreUtils.assetService.Destroy(gFM.m_unitHero);
                gFM.m_unitHero = null;
            }
        }

        private AnimationBase.ENMU_SPRITE_STATE GetUnitSpriteState(Troops.ENMU_SQUARE_STAT formation_state)
        {
            switch (formation_state)
            {
                case Troops.ENMU_SQUARE_STAT.IDLE:
                    return AnimationBase.ENMU_SPRITE_STATE.IDLE;
                case Troops.ENMU_SQUARE_STAT.SET_AROUND:
                    return AnimationBase.ENMU_SPRITE_STATE.MOVE;
                case Troops.ENMU_SQUARE_STAT.MOVE:
                    return AnimationBase.ENMU_SPRITE_STATE.MOVE;
                case Troops.ENMU_SQUARE_STAT.FIGHT:
                    return AnimationBase.ENMU_SPRITE_STATE.FIGHT;
                default:
                    return AnimationBase.ENMU_SPRITE_STATE.IDLE;
            }
        }

        private void ChangeUnitChaseSpeed(float chase_speed)
        {
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i != gFM.m_units.Count; i++)
            {
                if (!(gFM.m_units[i] == null))
                {
                    CellBase component = gFM.m_units[i].GetComponent<CellBase>();
                    if ((bool)component)
                    {
                        component.SetMoveSpeed(chase_speed);
                    }
                }
            }
            if (gFM.m_unitHero != null)
            {
                CellBase component2 = gFM.m_unitHero.GetComponent<CellBase>();
                if ((bool)component2)
                {
                    component2.SetMoveSpeed(chase_speed);
                }
            }
            if (gFM.m_unitNoTex != null)
            {
                CellBase component3 = gFM.m_unitNoTex.GetComponent<CellBase>();
                if ((bool)component3)
                {
                    component3.SetMoveSpeed(chase_speed);
                }
            }
        }

        private void RemapingUnit()
        {
            GuardianFormationMap gFM = GetGFM();
            gFM.Remap();
        }

        public static void SetFormationInfoS(Guardian self, string formation_info)
        {
            self.SetFormationInfo(formation_info);
        }

        private void SetFormationInfo(string formation_info)
        {
            ParseFormationData(formation_info);
            GuardianFormationMap gFM = GetGFM();
            foreach (int key in m_squareInfo.Keys)
            {
                Dictionary<int, int[]> dictionary = m_squareInfo[key];
                int num = 0;
                int num2 = 0;
                foreach (KeyValuePair<int, int[]> item in dictionary)
                {
                    num += item.Value[1];
                    num2 += item.Value[0];
                }
                int num3 = gFM.UnitTotalNumByCateoory(key);
                float num4 = (float)num2 / (float)num;
                int num5 = Mathf.CeilToInt((float)num3 * num4);
                int num6 = gFM.UnitNumByCategory(key);
                if (num6 > num5)
                {
                    gFM.DieUnit(key, num6 - num5);
                }
            }
        }

        public static void InitPositionS(Guardian self, Vector2 current_pos, Vector2 target_pos)
        {
            self.InitPoisition(current_pos, target_pos);
        }

        public void InitPoisition(Vector2 current_pos, Vector2 target_pos)
        {
            base.transform.position = new Vector3(current_pos.x, 0f, current_pos.y);
            Vector2 normalized = (target_pos - current_pos).normalized;
            if (normalized != Vector2.zero)
            {
                base.transform.forward = new Vector3(normalized.x, 0f, normalized.y);
            }
        }

        private new void OnSpawn()
        {
            int frameCount = m_loadFrame;
            if (m_move_smoke_particle_str != null && !m_move_smoke_particle_str.Equals(string.Empty))
            {
                CoreUtils.assetService.Instantiate(m_move_smoke_particle_str, (@object) =>
                {
                    if ((bool)@object)
                    {
                        if (m_loadFrame != frameCount)
                        {
                            CoreUtils.assetService.Destroy(@object);
                            return;
                        }
                        m_move_smoke = (GameObject)@object;
                        m_move_smoke.transform.SetParent(base.transform);
                        m_move_smoke.transform.localPosition = Vector3.zero;
                        m_move_smoke.transform.localEulerAngles = Vector3.zero;
                    }
                });
            }
        }

        private new void OnDespawn()
        {
            if ((bool)m_move_smoke)
            {
                CoreUtils.assetService.Destroy(m_move_smoke);
                m_move_smoke = null;
            }
            Reset();
            GetGFM().Reset();
            m_curBeHitEffect = m_beHitEffect;
            DestroyUnit();
            m_lastMoveAtk = false;
        }

        private void UpdateAtkEffectState(Troops.ENMU_SQUARE_STAT state)
        {
            if (m_formation_state != Troops.ENMU_SQUARE_STAT.FIGHT && state == Troops.ENMU_SQUARE_STAT.FIGHT)
            {
                m_atk_effect_launch = true;
                m_atk_effect_next_round_interval = m_atk_effect_param.roundInterval;
                m_atk_effect_this_round_trigger = (((float)UnityEngine.Random.Range(0.0f, 1.0f) <= m_atk_effect_param.roundTrigger) ? true : false);
            }
            else if (m_formation_state == Troops.ENMU_SQUARE_STAT.FIGHT && state != Troops.ENMU_SQUARE_STAT.FIGHT)
            {
                m_atk_effect_launch = false;
            }
        }

        private void UpdateAtkEffect()
        {
            if (!m_atk_effect_launch || m_curBeHitEffect.Equals(string.Empty))
            {
                return;
            }
            float num = Time.deltaTime * 1000f;
            if (m_atk_effect_dur_time > 0f)
            {
                m_atk_effect_dur_time -= num;
                for (int num2 = m_atk_effect_array.Count - 1; num2 >= 0; num2--)
                {
                    AtkSingleEffect atkSingleEffect = m_atk_effect_array[num2];
                    atkSingleEffect.delay -= num;
                    if (atkSingleEffect.delay <= 0f)
                    {
                        CoreUtils.assetService.Instantiate(m_curBeHitEffect, (gameObject) =>
                        {
                            Vector3 zero = Vector3.zero;
                            zero.z = m_formation_radius + atkSingleEffect.dis;
                            zero.x = atkSingleEffect.width;
                            if (gameObject != null)
                            {
                                gameObject.transform.position = base.transform.TransformPoint(zero);
                                gameObject.transform.forward = base.transform.forward;
                            }
                        });
                        m_atk_effect_array.Remove(atkSingleEffect);
                    }
                }
                if (m_atk_effect_dur_time <= 0f)
                {
                    m_atk_effect_dur_time = 0f;
                    m_atk_effect_next_round_interval = m_atk_effect_param.roundInterval;
                    m_atk_effect_this_round_trigger = (((float)UnityEngine.Random.Range(0.0f, 1.0f) <= m_atk_effect_param.roundTrigger) ? true : false);
                }
                return;
            }
            m_atk_effect_next_round_interval -= num;
            if (m_atk_effect_next_round_interval <= 0f)
            {
                m_atk_effect_dur_time = 1500f;
                int num3 = 0;
                if (m_atk_effect_this_round_trigger)
                {
                    num3 = UnityEngine.Random.Range(m_atk_effect_param.roundMin, m_atk_effect_param.roundMax + 1);
                }
                for (int i = 0; i < num3; i++)
                {
                    AtkSingleEffect atkSingleEffect2 = new AtkSingleEffect();
                    atkSingleEffect2.dis = UnityEngine.Random.Range(m_atk_effect_param.disMin, m_atk_effect_param.disMax);
                    atkSingleEffect2.width = UnityEngine.Random.Range(0f - m_atk_effect_param.width * 0.5f, m_atk_effect_param.width * 0.5f);
                    atkSingleEffect2.delay = UnityEngine.Random.Range(m_atk_effect_param.singleIntervalMin, m_atk_effect_param.singleIntervalMax) * (i + 1);
                    m_atk_effect_array.Add(atkSingleEffect2);
                }
            }
        }

        private void Update()
        {
            try
            {
                if (m_formation_state == Troops.ENMU_SQUARE_STAT.MOVE && m_move_time != 0f)
                {
                    m_move_timer += Time.unscaledDeltaTime;
                    float num = m_move_timer / m_move_time;
                    if (num >= 1f)
                    {
                        base.transform.position = new Vector3(m_target_pos.x, 0f, m_target_pos.y);
                        m_move_time = 0f;
                    }
                    else
                    {
                        Vector2 vector = Vector2.Lerp(m_start_pos, m_target_pos, num);
                        base.transform.position = new Vector3(vector.x, 0f, vector.y);
                    }
                }
                UpdateAtkEffect();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void FadeIn_S(Guardian self)
        {
            self.FadeIn();
        }

        private void FadeIn()
        {
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i != gFM.m_dummys.Length; i++)
            {
                CellBase unit = gFM.m_dummys[i].GetComponent<CellClone>().m_unit;
                if ((bool)unit)
                {
                    unit.FadeIn();
                }
            }
            for (int j = 0; j != gFM.m_lodDummys.Count; j++)
            {
                CellBase unit2 = gFM.m_lodDummys[j].GetComponent<CellClone>().m_unit;
                if ((bool)unit2)
                {
                    unit2.FadeIn();
                }
            }
        }

        public static void FadeOut_S(Guardian self)
        {
            self.FadeOut();
        }

        private void FadeOut()
        {
            GuardianFormationMap gFM = GetGFM();
            for (int i = 0; i != gFM.m_dummys.Length; i++)
            {
                CellBase unit = gFM.m_dummys[i].GetComponent<CellClone>().m_unit;
                if ((bool)unit)
                {
                    unit.FadeOut();
                }
            }
            for (int j = 0; j != gFM.m_lodDummys.Count; j++)
            {
                CellBase unit2 = gFM.m_lodDummys[j].GetComponent<CellClone>().m_unit;
                if ((bool)unit2)
                {
                    unit2.FadeOut();
                }
            }
            if (gFM.m_unitNoTex != null)
            {
                CellBase component = gFM.m_unitNoTex.GetComponent<CellBase>();
                component.ChangeMoveState(CellBase.MOVE_STATE.UNBOUND);
                component.transform.position = gFM.m_unitNoTexDummy.transform.position;
                component.SetSpriteLoigicalState(AnimationBase.ENMU_SPRITE_STATE.IDLE);
                component.PlayDeadParticle();
            }
        }

        public override void UpdateLod()
        {
            if (IsLodChanged())
            {
                TroopsLodUpdateMgr.m_instance.AddUpdateFormationRequest(this);
            }
            base.UpdateLod();
        }

        public void UpdateLodNow()
        {
            GetGFM().UpdateLod(GetCurrentLodLevel(), m_formation_state, GetUnitSpriteState(m_formation_state));
        }

        public new int GetCurrentLodLevel()
        {
            return base.GetCurrentLodLevel();
        }
    }
}
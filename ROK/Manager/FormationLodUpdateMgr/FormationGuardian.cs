using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class FormationGuardian : LodBase
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

        private Formation.ENUM_FORMATION_CAMP m_camp;

        private string m_movableStr = "moved";

        public Formation.ENMU_SQUARE_STAT m_formation_state;

        private Formation.ENMU_SQUARE_TYPE m_formation_type;

        public string m_beHitEffect = string.Empty;

        public string m_beHitEffectEnemy = string.Empty;

        public string m_curBeHitEffect = string.Empty;

        public Dictionary<string, Color> m_unitColorDict = new Dictionary<string, Color>();

        public FormationGuardian.AtkEffectParam m_atk_effect_param;

        private bool m_atk_effect_launch;

        private float m_atk_effect_dur_time;

        private float m_atk_effect_next_round_interval;

        private bool m_atk_effect_this_round_trigger = true;

        private List<FormationGuardian.AtkSingleEffect> m_atk_effect_array = new List<FormationGuardian.AtkSingleEffect>();

        private GameObject m_move_smoke;

        public string m_move_smoke_particle_str;

        public int max_row_number = 5;

        private float m_formation_radius;

        private GuardianFormationMap m_gfm;

        private Dictionary<int, Dictionary<int, int[]>> m_squareInfo = new Dictionary<int, Dictionary<int, int[]>>();

        private List<int> m_heros = new List<int>();

        private GuardianFormationMap GetGFM()
        {
            if (this.m_gfm == null)
            {
                this.m_gfm = base.GetComponent<GuardianFormationMap>();
            }
            return this.m_gfm;
        }

        public static void SetRadiusS(FormationGuardian self, float r)
        {
            if (self != null)
            {
                self.SetRadius(r);
            }
        }

        public void SetRadius(float r)
        {
            this.m_formation_radius = r;
            if (this.m_move_smoke)
            {
                Vector3 localScale = this.m_move_smoke.transform.localScale;
                localScale.x = r;
                localScale.y = r;
                localScale.z = r;
                this.m_move_smoke.transform.localScale = localScale;
            }
        }

        private void Reset()
        {
        }

        private void ParseFormationData(string square_info)
        {
            string[] array = square_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
            string[] array2 = array[1].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < array.Length; i++)
            {
                string[] array3 = array[i].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                int num = int.Parse(array3[0]);
                string[] array4 = array3[1].Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<int, int[]> dictionary;
                if (this.m_squareInfo.ContainsKey(num))
                {
                    dictionary = this.m_squareInfo[num];
                }
                else
                {
                    dictionary = new Dictionary<int, int[]>();
                    this.m_squareInfo.Add(num, dictionary);
                }
                for (int j = 0; j < array4.Length; j++)
                {
                    string[] array5 = array4[j].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                    int num2 = int.Parse(array5[0]);
                    if (num == 0)
                    {
                        this.m_heros.Insert(j, num2);
                    }
                    int[] array6;
                    if (dictionary.ContainsKey(num2))
                    {
                        array6 = dictionary[num2];
                    }
                    else
                    {
                        array6 = new int[2];
                        dictionary.Add(num2, array6);
                    }
                    array6[0] = int.Parse(array5[1]);
                    array6[1] = int.Parse(array5[2]);
                }
            }
        }

        private void LoadUnit()
        {
            this.GetGFM().LoadUnit();
            this.GetGFM().SetHeroUnit(this.m_heros);
        }

        public Formation.ENMU_SQUARE_TYPE GetFormationType()
        {
            return this.m_formation_type;
        }

        private void InitFormation(string formation_info, Color unit_color, string colorString = "")
        {
            if (colorString != null && colorString != string.Empty)
            {
                this.ParseUnitColorDict(colorString);
            }
            this.Reset();
            this.ParseFormationData(formation_info);
            this.GetGFM().SetUnitColor(unit_color);
            this.GetGFM().SetFormation(this);
            this.LoadUnit();
        }

        public static void InitFormationS(FormationGuardian self, string formation_info, Color unit_color, string colorString = "")
        {
            self.InitFormation(formation_info, unit_color, colorString);
        }

        private Vector3 GetShowPosition()
        {
            return base.gameObject.transform.position;
        }

        public static Vector3 GetShowPositionS(FormationGuardian self)
        {
            return self.GetShowPosition();
        }

        public static void SetStateS(FormationGuardian self, Formation.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f)
        {
            self.SetState(state, current_pos, target_pos, move_speed);
        }

        public static void SetCampS(FormationGuardian self, Formation.ENUM_FORMATION_CAMP camp)
        {
            if (self != null)
            {
                self.SetCamp(camp);
            }
        }

        private void SetCamp(Formation.ENUM_FORMATION_CAMP camp)
        {
            this.m_camp = camp;
            if (this.m_camp == Formation.ENUM_FORMATION_CAMP.Enemy)
            {
                this.m_curBeHitEffect = this.m_beHitEffect;
            }
            else
            {
                this.m_curBeHitEffect = this.m_beHitEffectEnemy;
            }
        }

        public static void SetBeAtkEffectCampS(FormationGuardian self, Formation.ENUM_FORMATION_CAMP camp)
        {
            if (self != null)
            {
                self.SetBeAtkEffectCamp(camp);
            }
        }

        private void SetBeAtkEffectCamp(Formation.ENUM_FORMATION_CAMP camp)
        {
            if (this.m_camp == Formation.ENUM_FORMATION_CAMP.Enemy)
            {
                this.m_curBeHitEffect = this.m_beHitEffect;
            }
            else
            {
                this.m_curBeHitEffect = this.m_beHitEffectEnemy;
            }
        }

        public static void TriggerSkillS(FormationGuardian self, string param)
        {
            if (self != null)
            {
                self.TriggerSkill(param);
            }
        }

        private void TriggerSkill(string param)
        {
            this.GetGFM().m_unitNoTex.GetComponent<UnitNoTex>().PlaySkillAni();
            this.GetGFM().m_unitHero.GetComponent<UnitBase>().PlaySpriteAniOnce(TextureSheetAnimation.ENMU_SPRITE_STATE.FIGHT);
        }

        public static void SetTargetMovableS(FormationGuardian self, string moved)
        {
            if (self != null)
            {
                self.SetTargetMovable(moved);
            }
        }

        private void SetTargetMovable(string moved)
        {
            this.m_movableStr = moved;
        }

        public static void ReservedFunc1S(FormationGuardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc1(param);
            }
        }

        private void ReservedFunc1(string param)
        {
        }

        public static void ReservedFunc2S(FormationGuardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc2(param);
            }
        }

        private void ReservedFunc2(string param)
        {
        }

        public static void ReservedFunc3S(FormationGuardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc3(param);
            }
        }

        private void ReservedFunc3(string param)
        {
        }

        public static void ReservedFunc4S(FormationGuardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc4(param);
            }
        }

        private void ReservedFunc4(string param)
        {
        }

        public static void ReservedFunc5S(FormationGuardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc5(param);
            }
        }

        private void ReservedFunc5(string param)
        {
        }

        public static void ReservedFunc6S(FormationGuardian self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc6(param);
            }
        }

        private void ReservedFunc6(string param)
        {
        }

        public void SetState(Formation.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f)
        {
            bool isMoveAtk = false;
            if (state == Formation.ENMU_SQUARE_STAT.FIGHT)
            {
                Vector3 b = new Vector3(current_pos.x, 0f, current_pos.y);
                if ((base.transform.position - b).magnitude > 0.07f)
                {
                    isMoveAtk = true;
                }
            }
            this.ChangeState(state, current_pos, target_pos, move_speed, isMoveAtk);
            base.transform.position = new Vector3(current_pos.x, 0f, current_pos.y);
            this.m_start_pos = current_pos;
            this.m_target_pos = target_pos;
            this.UnboundDummys();
            Vector2 normalized = (this.m_target_pos - this.m_start_pos).normalized;
            if (normalized != Vector2.zero)
            {
                base.transform.forward = new Vector3(normalized.x, 0f, normalized.y);
            }
            this.BoundDummys();
            this.SetDummysDir(base.transform.forward);
            this.ForceUnitHeroPlace();
            if (state == Formation.ENMU_SQUARE_STAT.MOVE)
            {
                this.m_move_timer = 0f;
                if (move_speed == 0f)
                {
                    Debug.LogWarning("Square : invalid move_speed");
                    move_speed = 1f;
                }
                this.m_move_speed = move_speed;
                this.m_move_time = (this.m_target_pos - this.m_start_pos).magnitude / this.m_move_speed;
            }
            this.UpdateAtkEffectState(state);
            this.m_formation_state = state;
        }

        private void BoundDummys()
        {
            GuardianFormationMap gFM = this.GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                gFM.m_dummys[i].transform.SetParent(base.transform, true);
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                gFM.m_lodDummys[j].transform.SetParent(base.transform, true);
            }
        }

        private void UnboundDummys()
        {
            GuardianFormationMap gFM = this.GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                gFM.m_dummys[i].transform.SetParent(null, true);
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                gFM.m_lodDummys[j].transform.SetParent(null, true);
            }
        }

        private void SetDummysDir(Vector3 dir)
        {
            GuardianFormationMap gFM = this.GetGFM();
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
            GuardianFormationMap gFM = this.GetGFM();
            if (gFM != null && gFM.m_unitHero != null)
            {
                gFM.m_unitHero.transform.position = gFM.m_unitHeroDummy.transform.position;
                gFM.m_unitHero.GetComponent<UnitBase>().ChangeMoveState(UnitBase.MOVE_STATE.STATIC, false, 0f, 1f);
            }
        }

        private void ChangeState(Formation.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f, bool isMoveAtk = false)
        {
            Vector2 zero = Vector2.zero;
            switch (state)
            {
                case Formation.ENMU_SQUARE_STAT.IDLE:
                    this.ChangeUnitChaseSpeed(this.m_move_speed);
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                    break;
                case Formation.ENMU_SQUARE_STAT.MOVE:
                    if (move_speed == 0f)
                    {
                        Debug.LogWarning("Square : invalid move_speed");
                        move_speed = 1f;
                    }
                    this.m_move_speed = move_speed;
                    this.ChangeUnitChaseSpeed(this.m_move_speed);
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                    break;
                case Formation.ENMU_SQUARE_STAT.FIGHT:
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, isMoveAtk);
                    break;
                case Formation.ENMU_SQUARE_STAT.DEAD:
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.UNBOUND, false);
                    break;
                case Formation.ENMU_SQUARE_STAT.SET_AROUND:
                    this.ChangeUnitChaseSpeed(this.m_move_speed * this.m_set_around_speed_factor);
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                    break;
            }
            this.SetUnitLogicalState(state);
        }

        public void ChangeUnitMoveState(UnitBase.MOVE_STATE state, bool isMoveAtk = false)
        {
            GuardianFormationMap gFM = this.GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                UnitBase unit = gFM.m_dummys[i].GetComponent<UnitDummy>().m_unit;
                if (unit)
                {
                    unit.ChangeMoveState(state, isMoveAtk, 0f, 1f);
                }
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                UnitBase unit2 = gFM.m_lodDummys[j].GetComponent<UnitDummy>().m_unit;
                if (unit2)
                {
                    unit2.ChangeMoveState(state, isMoveAtk, 0f, 1f);
                }
            }
            if (gFM.m_unitHero)
            {
                gFM.m_unitHero.GetComponent<UnitBase>().ChangeMoveState(state, isMoveAtk, 0f, 1f);
                gFM.m_unitHero.transform.position = gFM.m_unitHeroDummy.transform.position;
            }
            if (gFM.m_unitNoTex)
            {
                gFM.m_unitNoTex.GetComponent<UnitBase>().ChangeMoveState(state, isMoveAtk, 0f, 1f);
                gFM.m_unitNoTex.transform.position = gFM.m_unitNoTexDummy.transform.position;
            }
        }

        private void SetUnitLogicalState(Formation.ENMU_SQUARE_STAT formation_state)
        {
            TextureSheetAnimation.ENMU_SPRITE_STATE unitSpriteState = this.GetUnitSpriteState(formation_state);
            GuardianFormationMap gFM = this.GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                UnitBase unit = gFM.m_dummys[i].GetComponent<UnitDummy>().m_unit;
                if (unit)
                {
                    unit.SetSpriteLoigicalState(unitSpriteState);
                    if (formation_state == Formation.ENMU_SQUARE_STAT.DEAD)
                    {
                        unit.PlayDeadParticle();
                    }
                }
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                UnitBase unit2 = gFM.m_lodDummys[j].GetComponent<UnitDummy>().m_unit;
                if (unit2)
                {
                    unit2.SetSpriteLoigicalState(unitSpriteState);
                    if (formation_state == Formation.ENMU_SQUARE_STAT.DEAD)
                    {
                        unit2.PlayDeadParticle();
                    }
                }
            }
            if (formation_state == Formation.ENMU_SQUARE_STAT.DEAD)
            {
                this.DestroyUnit();
            }
            if (gFM.m_unitHero != null)
            {
                UnitBase component = gFM.m_unitHero.GetComponent<UnitBase>();
                if (unitSpriteState == TextureSheetAnimation.ENMU_SPRITE_STATE.FIGHT)
                {
                    component.SetSpriteLoigicalState(TextureSheetAnimation.ENMU_SPRITE_STATE.MOVE);
                }
                else
                {
                    component.SetSpriteLoigicalState(unitSpriteState);
                }
                if (formation_state == Formation.ENMU_SQUARE_STAT.DEAD)
                {
                    component.PlayDeadParticle();
                }
            }
            if (gFM.m_unitNoTex != null)
            {
                UnitBase component2 = gFM.m_unitNoTex.GetComponent<UnitBase>();
                component2.SetSpriteLoigicalState(unitSpriteState);
                if (this.m_formation_state == Formation.ENMU_SQUARE_STAT.DEAD)
                {
                    component2.PlayDeadParticle();
                }
            }
        }

        private void DestroyUnit()
        {
            GuardianFormationMap gFM = this.GetGFM();
            for (int i = 0; i < gFM.m_units.Count; i++)
            {
                if (gFM.m_units[i] != null)
                {
                    CoreUtils.assetService.Destroy(gFM.m_units[i]);
                }
            }
            gFM.m_units.Clear();
            if (gFM.m_unitHero)
            {
                CoreUtils.assetService.Destroy(gFM.m_unitHero);
                gFM.m_unitHero = null;
            }
        }

        private TextureSheetAnimation.ENMU_SPRITE_STATE GetUnitSpriteState(Formation.ENMU_SQUARE_STAT formation_state)
        {
            switch (formation_state)
            {
                case Formation.ENMU_SQUARE_STAT.IDLE:
                    return TextureSheetAnimation.ENMU_SPRITE_STATE.IDLE;
                case Formation.ENMU_SQUARE_STAT.MOVE:
                    return TextureSheetAnimation.ENMU_SPRITE_STATE.MOVE;
                case Formation.ENMU_SQUARE_STAT.FIGHT:
                    return TextureSheetAnimation.ENMU_SPRITE_STATE.FIGHT;
                case Formation.ENMU_SQUARE_STAT.SET_AROUND:
                    return TextureSheetAnimation.ENMU_SPRITE_STATE.MOVE;
            }
            return TextureSheetAnimation.ENMU_SPRITE_STATE.IDLE;
        }

        private void ChangeUnitChaseSpeed(float chase_speed)
        {
            GuardianFormationMap gFM = this.GetGFM();
            for (int i = 0; i < gFM.m_units.Count; i++)
            {
                if (!(gFM.m_units[i] == null))
                {
                    UnitBase component = gFM.m_units[i].GetComponent<UnitBase>();
                    if (component)
                    {
                        component.SetMoveSpeed(chase_speed);
                    }
                }
            }
            if (gFM.m_unitHero != null)
            {
                UnitBase component2 = gFM.m_unitHero.GetComponent<UnitBase>();
                if (component2)
                {
                    component2.SetMoveSpeed(chase_speed);
                }
            }
            if (gFM.m_unitNoTex != null)
            {
                UnitBase component3 = gFM.m_unitNoTex.GetComponent<UnitBase>();
                if (component3)
                {
                    component3.SetMoveSpeed(chase_speed);
                }
            }
        }

        private void RemapingUnit()
        {
            GuardianFormationMap gFM = this.GetGFM();
            gFM.Remap();
        }

        public static void SetFormationInfoS(FormationGuardian self, string formation_info)
        {
            self.SetFormationInfo(formation_info);
        }

        private void SetFormationInfo(string formation_info)
        {
            this.ParseFormationData(formation_info);
            GuardianFormationMap gFM = this.GetGFM();
            foreach (int current in this.m_squareInfo.Keys)
            {
                Dictionary<int, int[]> dictionary = this.m_squareInfo[current];
                int num = 0;
                int num2 = 0;
                foreach (KeyValuePair<int, int[]> current2 in dictionary)
                {
                    num += current2.Value[1];
                    num2 += current2.Value[0];
                }
                int num3 = gFM.UnitTotalNumByCateoory(current);
                float num4 = (float)num2 / (float)num;
                int num5 = Mathf.CeilToInt((float)num3 * num4);
                int num6 = gFM.UnitNumByCategory(current);
                if (num6 > num5)
                {
                    gFM.DieUnit(current, num6 - num5);
                }
            }
        }

        public static void InitPositionS(FormationGuardian self, Vector2 current_pos, Vector2 target_pos)
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
            CoreUtils.assetService.Instantiate(m_move_smoke_particle_str, (gameObject) =>
            {
                if (gameObject)
                {
                    this.m_move_smoke = (GameObject)gameObject;
                    this.m_move_smoke.transform.SetParent(base.transform);
                    this.m_move_smoke.transform.localPosition = Vector3.zero;
                    this.m_move_smoke.transform.localEulerAngles = Vector3.zero;
                }
            });
        }

        private new void OnDespawn()
        {
            if (this.m_move_smoke)
            {
                CoreUtils.assetService.Destroy(this.m_move_smoke);
                this.m_move_smoke = null;
            }
            this.Reset();
            this.GetGFM().Reset();
            this.m_curBeHitEffect = this.m_beHitEffect;
            this.DestroyUnit();
        }

        private void UpdateAtkEffectState(Formation.ENMU_SQUARE_STAT state)
        {
            if (this.m_formation_state != Formation.ENMU_SQUARE_STAT.FIGHT && state == Formation.ENMU_SQUARE_STAT.FIGHT)
            {
                this.m_atk_effect_launch = true;
                this.m_atk_effect_next_round_interval = (float)this.m_atk_effect_param.roundInterval;
                this.m_atk_effect_this_round_trigger = ((float)UnityEngine.Random.Range(0, 1) <= this.m_atk_effect_param.roundTrigger);
            }
            else if (this.m_formation_state == Formation.ENMU_SQUARE_STAT.FIGHT && state != Formation.ENMU_SQUARE_STAT.FIGHT)
            {
                this.m_atk_effect_launch = false;
            }
        }

        private void UpdateAtkEffect()
        {
            if (!this.m_atk_effect_launch)
            {
                return;
            }
            float num = Time.deltaTime * 1000f;
            if (this.m_atk_effect_dur_time > 0f)
            {
                this.m_atk_effect_dur_time -= num;
                for (int i = this.m_atk_effect_array.Count - 1; i >= 0; i--)
                {
                    FormationGuardian.AtkSingleEffect atkSingleEffect = this.m_atk_effect_array[i];
                    atkSingleEffect.delay -= num;
                    if (atkSingleEffect.delay <= 0f)
                    {
                        CoreUtils.assetService.Instantiate(m_curBeHitEffect, (gameObject) =>
                        {
                            Vector3 zero = Vector3.zero;
                            zero.z = this.m_formation_radius + atkSingleEffect.dis;
                            zero.x = atkSingleEffect.width;
                            if (gameObject != null)
                            {
                                gameObject.transform.position = base.transform.TransformPoint(zero);
                                gameObject.transform.forward = base.transform.forward;
                            }
                            this.m_atk_effect_array.Remove(atkSingleEffect);
                        });
                    }
                }
                if (this.m_atk_effect_dur_time <= 0f)
                {
                    this.m_atk_effect_dur_time = 0f;
                    this.m_atk_effect_next_round_interval = (float)this.m_atk_effect_param.roundInterval;
                    this.m_atk_effect_this_round_trigger = ((float)UnityEngine.Random.Range(0, 1) <= this.m_atk_effect_param.roundTrigger);
                }
            }
            else
            {
                this.m_atk_effect_next_round_interval -= num;
                if (this.m_atk_effect_next_round_interval <= 0f)
                {
                    this.m_atk_effect_dur_time = 1500f;
                    int num2 = 0;
                    if (this.m_atk_effect_this_round_trigger)
                    {
                        num2 = UnityEngine.Random.Range(this.m_atk_effect_param.roundMin, this.m_atk_effect_param.roundMax + 1);
                    }
                    for (int j = 0; j < num2; j++)
                    {
                        FormationGuardian.AtkSingleEffect atkSingleEffect2 = new FormationGuardian.AtkSingleEffect();
                        atkSingleEffect2.dis = UnityEngine.Random.Range(this.m_atk_effect_param.disMin, this.m_atk_effect_param.disMax);
                        atkSingleEffect2.width = UnityEngine.Random.Range(-(this.m_atk_effect_param.width * 0.5f), this.m_atk_effect_param.width * 0.5f);
                        atkSingleEffect2.delay = (float)(UnityEngine.Random.Range(this.m_atk_effect_param.singleIntervalMin, this.m_atk_effect_param.singleIntervalMax) * (j + 1));
                        this.m_atk_effect_array.Add(atkSingleEffect2);
                    }
                }
            }
        }

        private void Update()
        {
            try
            {
                if (this.m_formation_state == Formation.ENMU_SQUARE_STAT.MOVE && this.m_move_time != 0f)
                {
                    this.m_move_timer += Time.unscaledDeltaTime;
                    float num = this.m_move_timer / this.m_move_time;
                    if (num >= 1f)
                    {
                        base.transform.position = new Vector3(this.m_target_pos.x, 0f, this.m_target_pos.y);
                        this.m_move_time = 0f;
                    }
                    else
                    {
                        Vector2 vector = Vector2.Lerp(this.m_start_pos, this.m_target_pos, num);
                        base.transform.position = new Vector3(vector.x, 0f, vector.y);
                    }
                }
                this.UpdateAtkEffect();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void FadeIn_S(FormationGuardian self)
        {
            self.FadeIn();
        }

        private void FadeIn()
        {
            GuardianFormationMap gFM = this.GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                UnitBase unit = gFM.m_dummys[i].GetComponent<UnitDummy>().m_unit;
                if (unit)
                {
                    unit.FadeIn();
                }
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                UnitBase unit2 = gFM.m_lodDummys[j].GetComponent<UnitDummy>().m_unit;
                if (unit2)
                {
                    unit2.FadeIn();
                }
            }
        }

        public static void FadeOut_S(FormationGuardian self)
        {
            self.FadeOut();
        }

        private void FadeOut()
        {
            GuardianFormationMap gFM = this.GetGFM();
            for (int i = 0; i < gFM.m_dummys.Length; i++)
            {
                UnitBase unit = gFM.m_dummys[i].GetComponent<UnitDummy>().m_unit;
                if (unit)
                {
                    unit.FadeOut();
                }
            }
            for (int j = 0; j < gFM.m_lodDummys.Count; j++)
            {
                UnitBase unit2 = gFM.m_lodDummys[j].GetComponent<UnitDummy>().m_unit;
                if (unit2)
                {
                    unit2.FadeOut();
                }
            }
            if (gFM.m_unitNoTex != null)
            {
                UnitBase component = gFM.m_unitNoTex.GetComponent<UnitBase>();
                component.ChangeMoveState(UnitBase.MOVE_STATE.UNBOUND, false, 0f, 1f);
                component.transform.position = gFM.m_unitNoTexDummy.transform.position;
                component.SetSpriteLoigicalState(TextureSheetAnimation.ENMU_SPRITE_STATE.IDLE);
                component.PlayDeadParticle();
            }
        }

        public override void UpdateLod()
        {
            if (base.IsLodChanged())
            {
                FormationLodUpdateMgr.m_instance.AddUpdateFormationRequest(this);
            }
            base.UpdateLod();
        }

        public void UpdateLodNow()
        {
            this.GetGFM().UpdateLod(this.GetCurrentLodLevel(), this.m_formation_state, this.GetUnitSpriteState(this.m_formation_state));
        }

        public new int GetCurrentLodLevel()
        {
            return base.GetCurrentLodLevel();
        }

        private void ParseUnitColorDict(string colorString)
        {
            this.m_unitColorDict.Clear();
            string[] array = colorString.Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                string text = array2[0];
                float num = float.Parse(array2[1]);
                float num2 = float.Parse(array2[2]);
                float num3 = float.Parse(array2[3]);
                float num4 = float.Parse(array2[4]);
                if (num != 0f || num2 != 0f || num3 != 0f || num4 != 0f)
                {
                    Color value = new Color(num, num2, num3, num4);
                    this.m_unitColorDict.Add(array2[0], value);
                }
            }
        }

        public Dictionary<string, Color> GetUnitColorDict()
        {
            return this.m_unitColorDict;
        }
    }
}
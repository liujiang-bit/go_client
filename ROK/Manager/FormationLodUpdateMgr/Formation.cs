using Skyunion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ROK
{
    public class Formation : LodBase
    {
        public enum ENMU_SQUARE_STAT
        {
            IDLE,
            MOVE,
            FIGHT,
            DEAD,
            SET_AROUND,
            NUMBER
        }

        public enum ENMU_SQUARE_TYPE
        {
            COMMON,
            BARBARIAN,
            RALLY,
            SHAMAN_GUARDIAN
        }

        public enum ENUM_SQUARE_CATEGORY
        {
            Hero,
            Infantry,
            Cavalry,
            Archery,
            Siege,
            NUMBER
        }

        public enum ENUM_FORMATION_CAMP
        {
            Mine,
            Ally,
            Enemy
        }

        public enum ENUM_HERO_PRIORITY
        {
            Prio1 = 1,
            Prio2
        }

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

        private SquareRow m_hero_square;

        public float m_square_pos_offset;

        private Dictionary<int, SquareRow> m_row_dict = new Dictionary<int, SquareRow>();

        private SquarePositionProvider m_square_position_provider = new SquarePositionProvider();

        private Vector2 m_start_pos = Vector2.zero;

        private Vector2 m_target_pos = Vector2.zero;

        private float m_move_timer;

        private float m_move_time;

        private float m_move_speed = 1f;

        public Dictionary<string, Color> m_unitColorDict = new Dictionary<string, Color>();

        private Formation.ENUM_FORMATION_CAMP m_camp;

        private string m_movableStr = "moved";

        public Formation.ENMU_SQUARE_STAT m_formation_state;

        private Formation.ENMU_SQUARE_TYPE m_formation_type;

        public string m_beHitEffect = string.Empty;

        public string m_beHitEffectEnemy = string.Empty;

        public string m_curBeHitEffect = string.Empty;

        public Formation.AtkEffectParam m_atk_effect_param;

        private bool m_atk_effect_launch;

        private float m_atk_effect_dur_time;

        private float m_atk_effect_next_round_interval;

        private bool m_atk_effect_this_round_trigger = true;

        private List<Formation.AtkSingleEffect> m_atk_effect_array = new List<Formation.AtkSingleEffect>();

        private GameObject m_move_smoke;

        public string m_move_smoke_particle_str;

        public int max_row_number = 5;

        private List<SquareRowInfo> m_lst_square_row_info = new List<SquareRowInfo>();

        private float m_formation_radius;

        private float m_minRotateAngle = 22.5f;

        private bool m_isSimpleMode;

        private Color m_formation_color = Color.red;

        private int m_atkTick;

        public static void SetRadiusS(Formation self, float r)
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
            this.m_minRotateAngle = -1.5f * this.m_formation_radius + 23f;
        }

        private void Reset()
        {
            this.m_hero_square = null;
            this.m_square_position_provider.Reset();
            this.m_row_dict.Clear();
            this.m_atkTick = 0;
        }

        public Formation.ENMU_SQUARE_TYPE GetFormationType()
        {
            return this.m_formation_type;
        }

        private void InitFormationBarbarian(string square_info, Color unit_color)
        {
            FormationBarbarian component = base.GetComponent<FormationBarbarian>();
            component.InitFormationData(square_info);
            BarbarianFormationConfig[] squareRows = component.m_squareRows;
            for (int i = 0; i < squareRows.Length; i++)
            {
                SquareRow component2 = squareRows[i].GetComponent<SquareRow>();
                component2.enabled = true;
                component2.InitRowBarbarian(string.Empty, 1, (squareRows[i].RowNum != 0) ? 1 : 0, unit_color, this);
                component2.ForceUpdateLod();
                if (i == 0)
                {
                    this.m_hero_square = component2;
                }
                this.m_row_dict.Add(i, component2);
            }
        }

        private void SwitchShowMode(string formation_info)
        {
            if (this.m_formation_type == Formation.ENMU_SQUARE_TYPE.BARBARIAN)
            {
                return;
            }
            foreach (SquareRow current in this.m_row_dict.Values)
            {
                if (current)
                {
                    CoreUtils.assetService.Destroy(current.gameObject);
                }
            }
            this.m_row_dict.Clear();
            if (this.m_isSimpleMode)
            {
                string[] array = formation_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
                if (this.m_formation_type != (Formation.ENMU_SQUARE_TYPE)int.Parse(array[0]))
                {
                    return;
                }
                string path = "row";
                CoreUtils.assetService.Instantiate(path, (GameObject gameObject) =>
                {
                    SquareRow component = gameObject.GetComponent<SquareRow>();
                    component.SetPositionProvider(this.m_square_position_provider);
                    this.m_row_dict.Add(0, component);
                    component.transform.SetParent(base.transform, false);
                    component.transform.localEulerAngles = Vector3.zero;
                    component.transform.localPosition = Vector3.zero;
                    string[] array2 = array[1].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                    component.InitRow(array2[1], 1, 0, this.m_formation_color, this);
                    component.ForceUpdateLod();
                    this.m_hero_square = component;
                });
            }
            else
            {
                this.InitFormation(formation_info, this.m_formation_color, string.Empty);
            }
        }

        private void InitFormation(string formation_info, Color unit_color, string colorString = "")
        {
            if (colorString != null && colorString != string.Empty)
            {
                this.ParseUnitColorDict(colorString);
            }
            this.m_formation_color = unit_color;
            this.Reset();
            string[] array = formation_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            this.m_formation_type = (Formation.ENMU_SQUARE_TYPE)int.Parse(array[0]);
            this.m_square_position_provider.SetSquareType(this.m_formation_type);
            if (this.m_formation_type == Formation.ENMU_SQUARE_TYPE.RALLY)
            {
                this.max_row_number = 7;
            }
            else
            {
                this.max_row_number = 5;
            }
            for (int i = 1; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                dictionary.Add(int.Parse(array2[0]), array2[1]);
            }
            if (this.m_formation_type == Formation.ENMU_SQUARE_TYPE.BARBARIAN)
            {
                this.InitFormationBarbarian(formation_info, unit_color);
                return;
            }
            Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
            Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
            foreach (int current in dictionary.Keys)
            {
                int displayUnitNumberByString = SquareRowInfo.GetDisplayUnitNumberByString(dictionary[current], current, this.m_formation_type);
                dictionary2.Add(current, SquareRowInfo.GetRawRowNumberByUnitNumber(displayUnitNumberByString, current, this.m_formation_type));
                dictionary3.Add(current, displayUnitNumberByString);
            }
            if (this.max_row_number < dictionary2.Count)
            {
                Debug.LogError("Formation : max_row_number must be greater than unit type");
                return;
            }
            Dictionary<int, int> dictionary4 = new Dictionary<int, int>();
            int num = this.max_row_number - dictionary2.Count;
            List<int> list = new List<int>(dictionary3.Keys);
            for (int j = 0; j < list.Count; j++)
            {
                this.RemoveUnitFromRawDict(dictionary3, list[j]);
                dictionary4.Add(list[j], 1);
            }
            while (num != 0)
            {
                int largestUnitTypeFromRawDict = this.GetLargestUnitTypeFromRawDict(dictionary3);
                if (largestUnitTypeFromRawDict == -1)
                {
                    break;
                }
                Dictionary<int, int> dictionary5;
                int key;
                (dictionary5 = dictionary4)[key = largestUnitTypeFromRawDict] = dictionary5[key] + 1;
                this.RemoveUnitFromRawDict(dictionary3, largestUnitTypeFromRawDict);
                num--;
            }
            float num2 = 0f;
            bool flag = true;
            float num3 = 0f;
            float num4 = 0f;
            float num5 = 0f;
            int nCount = dictionary4.Count;
            foreach (int current2 in dictionary4.Keys)
            {
                string path = "row";
                CoreUtils.assetService.Instantiate(path, (GameObject gameObject) =>
                {
                    SquareRow component = gameObject.GetComponent<SquareRow>();
                    component.SetPositionProvider(this.m_square_position_provider);
                    this.m_row_dict.Add(current2, component);
                    component.transform.SetParent(base.transform, false);
                    component.transform.localEulerAngles = Vector3.zero;
                    component.InitRow(dictionary[current2], dictionary4[current2], current2, unit_color, this);
                    float num6 = UnityGameDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(current2, this.m_formation_type);
                    float num7 = UnityGameDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(current2, this.m_formation_type);
                    float num8 = (num6 + num7) * (float)component.squareRowInfo.curRowNumber;
                    float num9 = num8 - num7 + num2;
                    num4 += num8;
                    num3 -= num9;
                    component.transform.position = base.transform.position + base.transform.forward * num3;
                    num2 = num7;
                    if (flag)
                    {
                        this.m_hero_square = component;
                        flag = false;
                    }
                    if (current2 == 4)
                    {
                        num5 += num8;
                    }

                    nCount--;
                    if (nCount == 0)
                    {
                        this.UpdateSquareRowOffset(num4);
                        if (this.m_formation_type != Formation.ENMU_SQUARE_TYPE.BARBARIAN)
                        {
                            foreach (SquareRow current3 in this.m_row_dict.Values)
                            {
                                current3.ForceUpdateLod();
                            }
                        }
                    }
                });
            }
        }

        private void UpdateSquareRowOffset(float square_size)
        {
            float d = square_size * 0.5f;
            foreach (KeyValuePair<int, SquareRow> current in this.m_row_dict)
            {
                SquareRow value = current.Value;
                value.transform.position = value.transform.position + base.transform.forward * d;
            }
        }

        private void RemoveUnitFromRawDict(Dictionary<int, int> unit_raw_dict, int unit_type)
        {
            unit_raw_dict[unit_type] -= UnityGameDatas.GetInstance().ReadUnitDisplayNumberInRow(unit_type, this.m_formation_type);
            if (unit_raw_dict[unit_type] < 0)
            {
                unit_raw_dict[unit_type] = 0;
            }
        }

        private int GetLargestUnitTypeFromRawDict(Dictionary<int, int> unit_raw_dict)
        {
            int result = -1;
            int num = 0;
            foreach (int current in unit_raw_dict.Keys)
            {
                int num2 = unit_raw_dict[current];
                if (num2 > num)
                {
                    result = current;
                    num = num2;
                }
            }
            return result;
        }

        public static void InitFormationS(Formation self, string formation_info, Color unit_color, string colorString = "")
        {
            self.InitFormation(formation_info, unit_color, colorString);
        }

        public static void SetHeroLevelUpEffectiveS(Formation self, string effectName)
        {
            if (self != null)
            {
                self.SetHeroLevelUpEffective(effectName);
            }
        }

        public void SetHeroLevelUpEffective(string effectName)
        {
            this.m_hero_square.SetEffectOnUnit(effectName);
        }

        private Vector3 GetShowPosition()
        {
            if (this.m_hero_square == null)
            {
                return base.transform.position;
            }
            return this.m_hero_square.GetShowPosition();
        }

        public static Vector3 GetShowPositionS(Formation self)
        {
            return self.GetShowPosition();
        }

        public static void SetStateS(Formation self, Formation.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f)
        {
            self.SetState(state, current_pos, target_pos, move_speed);
        }

        public static void SetCampS(Formation self, Formation.ENUM_FORMATION_CAMP camp)
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

        public static void SetBeAtkEffectCampS(Formation self, Formation.ENUM_FORMATION_CAMP camp)
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

        public static void TriggerSkillS(Formation self, string param)
        {
            if (self != null)
            {
                self.TriggerSkill(param);
            }
        }

        private void TriggerSkill(string param)
        {
            if (this.m_row_dict.ContainsKey(0))
            {
                SquareRow squareRow = this.m_row_dict[0];
                squareRow.HeroSkillAni(int.Parse(param));
            }
        }

        public static void SetTargetMovableS(Formation self, string moved)
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

        public static void ReservedFunc1S(Formation self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc1(param);
            }
        }

        private void ReservedFunc1(string param)
        {
            string[] array = param.Split(Common.DATA_DELIMITER_LEVEL_4, StringSplitOptions.RemoveEmptyEntries);
            bool flag = array[0] == "1";
            if (this.m_isSimpleMode != flag)
            {
                this.m_isSimpleMode = flag;
                float r;
                float g;
                float b;
                if (array.Length >= 5 && float.TryParse(array[2], out r) && float.TryParse(array[3], out g) && float.TryParse(array[4], out b))
                {
                    this.m_formation_color.r = r;
                    this.m_formation_color.g = g;
                    this.m_formation_color.b = b;
                }
                this.SwitchShowMode(array[1]);
            }
        }

        public static void ReservedFunc2S(Formation self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc2(param);
            }
        }

        private void ReservedFunc2(string param)
        {
        }

        public static void ReservedFunc3S(Formation self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc3(param);
            }
        }

        private void ReservedFunc3(string param)
        {
        }

        public static void ReservedFunc4S(Formation self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc4(param);
            }
        }

        private void ReservedFunc4(string param)
        {
        }

        public static void ReservedFunc5S(Formation self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc5(param);
            }
        }

        private void ReservedFunc5(string param)
        {
        }

        public static void ReservedFunc6S(Formation self, string param)
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
            float s_ticktick = Common.s_ticktick;
            Vector2 normalized = (target_pos - current_pos).normalized;
            Vector2 vector = new Vector2(base.transform.forward.x, base.transform.forward.z);
            bool isMoveAtk = false;
            Vector3 b = new Vector3(current_pos.x, 0f, current_pos.y);
            float magnitude = (base.transform.position - b).magnitude;
            if (state == Formation.ENMU_SQUARE_STAT.FIGHT)
            {
                if (magnitude > 0.2f && this.m_atkTick != 0)
                {
                    isMoveAtk = true;
                    if (move_speed * s_ticktick > magnitude)
                    {
                        if (s_ticktick < 0.9f)
                        {
                            move_speed = 0f;
                        }
                        else
                        {
                            move_speed = magnitude / s_ticktick;
                        }
                    }
                }
                else
                {
                    current_pos.x = base.transform.position.x;
                    current_pos.y = base.transform.position.z;
                    normalized = (target_pos - current_pos).normalized;
                    Vector3 to_ = new Vector3(normalized.x, 0f, normalized.y);
                    float angle = Common.GetAngle360(Vector3.forward, base.transform.forward);
                    float angle2 = Common.GetAngle360(Vector3.forward, to_);
                    int num = (int)((angle + 22.5f) / 45f % 8f);
                    int num2 = (int)((angle2 + 22.5f) / 45f % 8f);
                    if (num != num2 || Mathf.Abs(angle2 - angle) > this.m_minRotateAngle)
                    {
                        isMoveAtk = true;
                    }
                }
            }
            foreach (SquareRow current in this.m_row_dict.Values)
            {
                current.SetState(state, current_pos, target_pos, move_speed, isMoveAtk);
            }
            base.transform.position = new Vector3(current_pos.x, 0f, current_pos.y);
            this.m_start_pos = current_pos;
            this.m_target_pos = target_pos;
            base.transform.forward = new Vector3(normalized.x, 0f, normalized.y);
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
            this.TransformAtkFormation(state, false);
            this.UpdateAtkEffectState(state);
            this.m_formation_state = state;
        }

        private void TransformAtkFormation(Formation.ENMU_SQUARE_STAT state, bool isForce = false)
        {
            if (this.m_formation_state != Formation.ENMU_SQUARE_STAT.FIGHT && state == Formation.ENMU_SQUARE_STAT.FIGHT)
            {
                this.m_atkTick = 1;
                return;
            }
            if (((this.m_formation_state == Formation.ENMU_SQUARE_STAT.FIGHT && state == Formation.ENMU_SQUARE_STAT.FIGHT && this.m_atkTick == 1) || (isForce && state == Formation.ENMU_SQUARE_STAT.FIGHT)) && this.m_row_dict.Keys.Count > 1)
            {
                this.m_atkTick = 2;
                int num = 0;
                Dictionary<int, float> dictionary = new Dictionary<int, float>();
                foreach (KeyValuePair<int, SquareRow> current in this.m_row_dict)
                {
                    if (isForce)
                    {
                        current.Value.ResetAtkFormationParam();
                    }
                    if (!current.Value.CheckSquareRowIsEmpty())
                    {
                        int key = current.Key;
                        float innerFirstRowZ = current.Value.GetInnerFirstRowZ();
                        dictionary.Add(key, innerFirstRowZ);
                        if (key != 0)
                        {
                            num += current.Value.GetInnerRowNum();
                        }
                    }
                }
                List<KeyValuePair<int, float>> list = (from r in dictionary
                                                       orderby r.Value descending
                                                       select r).ToList<KeyValuePair<int, float>>();
                if (list.Count >= 2)
                {
                    float value = list[0].Value;
                    float value2 = list[1].Value;
                    float num2 = value - value2;
                    float num3 = num2;
                    if (num > 0)
                    {
                        num3 /= (float)num;
                    }
                    int num4 = 0;
                    for (int i = 1; i < list.Count; i++)
                    {
                        SquareRow squareRow = this.m_row_dict[list[i].Key];
                        if (i == 1)
                        {
                            squareRow.SetIsFirstRow(true);
                        }
                        else if (i == list.Count - 1)
                        {
                            squareRow.SetIsLastRow(true);
                        }
                        squareRow.TransformAtkFormation(true, num2 - num3 * (float)num4, num3, isForce);
                        num4 += squareRow.GetInnerRowNum();
                    }
                }
            }
            else if ((this.m_formation_state == Formation.ENMU_SQUARE_STAT.FIGHT && state != Formation.ENMU_SQUARE_STAT.FIGHT && this.m_atkTick == 2) || (isForce && state != Formation.ENMU_SQUARE_STAT.FIGHT))
            {
                foreach (SquareRow current2 in this.m_row_dict.Values)
                {
                    current2.TransformAtkFormation(false, 0f, 0f, isForce);
                }
                this.m_atkTick = 0;
            }
        }

        public void TransformAtkFormationImmediately(Formation.ENMU_SQUARE_STAT state, bool isForce = false)
        {
            if (((this.m_formation_state != Formation.ENMU_SQUARE_STAT.FIGHT && state == Formation.ENMU_SQUARE_STAT.FIGHT) || (isForce && state == Formation.ENMU_SQUARE_STAT.FIGHT)) && this.m_row_dict.Keys.Count > 1)
            {
                int num = this.m_row_dict.Keys.Count - 1;
                Dictionary<int, float> dictionary = new Dictionary<int, float>();
                foreach (KeyValuePair<int, SquareRow> current in this.m_row_dict)
                {
                    if (isForce)
                    {
                        current.Value.ResetAtkFormationParam();
                    }
                    if (!current.Value.CheckSquareRowIsEmpty())
                    {
                        int key = current.Key;
                        float innerFirstRowZ = current.Value.GetInnerFirstRowZ();
                        dictionary.Add(key, innerFirstRowZ);
                    }
                    else
                    {
                        num--;
                    }
                }
                List<KeyValuePair<int, float>> list = (from r in dictionary
                                                       orderby r.Value descending
                                                       select r).ToList<KeyValuePair<int, float>>();
                if (list.Count >= 2)
                {
                    float value = list[0].Value;
                    float value2 = list[1].Value;
                    float num2 = value - value2;
                    if (num > 0)
                    {
                        num2 /= (float)num;
                    }
                    for (int i = 1; i < list.Count; i++)
                    {
                        SquareRow squareRow = this.m_row_dict[list[i].Key];
                        if (i == 1)
                        {
                            squareRow.SetIsFirstRow(true);
                        }
                        else if (i == list.Count - 1)
                        {
                            squareRow.SetIsLastRow(true);
                        }
                    }
                }
            }
            else if ((this.m_formation_state == Formation.ENMU_SQUARE_STAT.FIGHT && state != Formation.ENMU_SQUARE_STAT.FIGHT) || (isForce && state != Formation.ENMU_SQUARE_STAT.FIGHT))
            {
                foreach (SquareRow current2 in this.m_row_dict.Values)
                {
                }
            }
        }

        public static void SetFormationInfoS(Formation self, string formation_info)
        {
            self.SetFormationInfo(formation_info);
        }

        private void SetFormationInfoBarbarian(string square_info)
        {
            FormationBarbarian component = base.GetComponent<FormationBarbarian>();
            component.SetFormationData(square_info);
        }

        private void SetFormationInfo(string formation_info)
        {
            if (this.m_formation_type == Formation.ENMU_SQUARE_TYPE.BARBARIAN)
            {
                this.SetFormationInfoBarbarian(formation_info);
                return;
            }
            if (this.m_isSimpleMode)
            {
                return;
            }
            string[] array = formation_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            this.m_square_position_provider.Reset();
            int num = 0;
            float num2 = 0f;
            bool flag = false;
            for (int i = 1; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                int num3 = int.Parse(array2[0]);
                string info_str = array2[1];
                flag |= this.m_row_dict[num3].squareRowInfo.UpdateUnitNumber(info_str);
                this.m_row_dict[num3].UpdateUnitNumber();
                num += this.m_row_dict[num3].GetRemoveUnitNum();
                if (num3 == 0)
                {
                    float num4 = UnityGameDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(0, Formation.ENMU_SQUARE_TYPE.COMMON);
                    float num5 = UnityGameDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(0, Formation.ENMU_SQUARE_TYPE.COMMON);
                    num2 = num4 + num5;
                }
                if (num3 == 4)
                {
                    float num6 = UnityGameDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(4, Formation.ENMU_SQUARE_TYPE.COMMON);
                    float num7 = UnityGameDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(4, Formation.ENMU_SQUARE_TYPE.COMMON);
                    num2 += (num6 + num7) * (float)this.m_row_dict[num3].squareRowInfo.curRowNumber;
                }
            }
            if (flag)
            {
                if (this.m_formation_type != Formation.ENMU_SQUARE_TYPE.BARBARIAN)
                {
                    float num8 = 0f;
                    float num9 = 0f;
                    float num10 = 0f;
                    foreach (SquareRow current in this.m_row_dict.Values)
                    {
                        float num11 = UnityGameDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(current.squareRowInfo.rowCategory, Formation.ENMU_SQUARE_TYPE.COMMON);
                        float num12 = UnityGameDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(current.squareRowInfo.rowCategory, Formation.ENMU_SQUARE_TYPE.COMMON);
                        float f = (float)current.squareRowInfo.curRowNumber * 0.5f;
                        float num13 = num11 * Mathf.Ceil(f) + num12 * Mathf.Floor(f);
                        float num14 = num11 * Mathf.Floor(f) + num12 * Mathf.Ceil(f);
                        float num15 = (num11 + num12) * (float)current.squareRowInfo.curRowNumber;
                        float num16 = num15 - num12 + num8;
                        num10 += num15;
                        num9 -= num16;
                        current.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                        current.transform.position = base.transform.position + base.transform.forward * num9;
                        num8 = num12;
                    }
                    this.UpdateSquareRowOffset(num10);
                }
            }
        }

        public static void InitPositionS(Formation self, Vector2 current_pos, Vector2 target_pos)
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
            CoreUtils.assetService.Instantiate(m_move_smoke_particle_str, (GameObject gameObject) =>
            {
                if (gameObject)
                {
                    this.m_move_smoke = gameObject;
                    this.m_move_smoke.transform.SetParent(base.transform);
                    this.m_move_smoke.transform.localPosition = Vector3.zero;
                    this.m_move_smoke.transform.localEulerAngles = Vector3.zero;
                }
            });
        }

        private new void OnDespawn()
        {
            if (this.m_formation_type == Formation.ENMU_SQUARE_TYPE.BARBARIAN)
            {
                foreach (SquareRow current in this.m_row_dict.Values)
                {
                    if (current)
                    {
                        current.DestroyManual();
                    }
                }
            }
            else
            {
                foreach (SquareRow current2 in this.m_row_dict.Values)
                {
                    if (current2)
                    {
                        CoreUtils.assetService.Destroy(current2.gameObject);
                    }
                }
            }
            if (this.m_move_smoke)
            {
                CoreUtils.assetService.Destroy(this.m_move_smoke);
                this.m_move_smoke = null;
            }
            this.Reset();
            this.m_curBeHitEffect = this.m_beHitEffect;
            this.m_isSimpleMode = false;
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
                    Formation.AtkSingleEffect atkSingleEffect = this.m_atk_effect_array[i];
                    atkSingleEffect.delay -= num;
                    if (atkSingleEffect.delay <= 0f)
                    {
                        CoreUtils.assetService.Instantiate(m_curBeHitEffect, (GameObject gameObject) =>
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
                        Formation.AtkSingleEffect atkSingleEffect2 = new Formation.AtkSingleEffect();
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

        public static void FadeIn_S(Formation self)
        {
            self.FadeIn();
        }

        private void FadeIn()
        {
            foreach (SquareRow current in this.m_row_dict.Values)
            {
                current.FadeIn();
            }
        }

        public static void FadeOut_S(Formation self)
        {
            self.FadeOut();
        }

        private void FadeOut()
        {
            foreach (SquareRow current in this.m_row_dict.Values)
            {
                current.FadeOut();
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
            foreach (SquareRow current in this.m_row_dict.Values)
            {
                current.ForceUpdateLod();
            }
            this.TransformAtkFormation(this.m_formation_state, true);
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
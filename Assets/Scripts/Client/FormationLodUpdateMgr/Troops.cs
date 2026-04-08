using Skyunion;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class Troops : LevelDetailBase
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

        public enum ENMU_MATRIX_TYPE
        {
            COMMON,
            BARBARIAN,//原始的
            RALLY,      //聚集地
            SHAMAN_GUARDIAN //萨满守卫者
        }

        public enum ENUM_SQUARE_CATEGORY
        {
            Hero,
            Infantry,   //步兵
            Cavalry,    //骑兵
            Archery,    //弓箭手
            Siege,      //攻城车
            NUMBER
        }

        public enum ENUM_FORMATION_CAMP
        {
            Mine,       //自己
            Ally,       //同盟
            Enemy       //敌对
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

        private MatrixRow m_hero_square;

        public float m_square_pos_offset;

        private Dictionary<int, MatrixRow> m_row_dict = new Dictionary<int, MatrixRow>();

        private MatrixPosProvider m_square_position_provider = new MatrixPosProvider();

        private Vector2 m_start_pos = Vector2.zero;

        private Vector2 m_target_pos = Vector2.zero;

        private float m_move_timer;

        private float m_move_time;

        private float m_move_speed = 1f;

        private ENUM_FORMATION_CAMP m_camp;

        private string m_movableStr = "moved";

        public ENMU_SQUARE_STAT m_formation_state;

        public ENMU_SQUARE_STAT m_formation_last_state;

        private ENMU_MATRIX_TYPE m_formation_type;

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

        private List<MatrixRowInfo> m_lst_square_row_info = new List<MatrixRowInfo>();

        private float m_formation_radius;

        private float m_minRotateAngle = 22.5f;

        private int m_atkTick;

        private int m_loadFrame;

        private Color m_unit_color = Color.blue;

        public static void SetRadiusS(Troops self, float r)
        {
            if (self != null)
            {
                self.SetRadius(r);
            }
        }

        public void SetRadius(float r)
        {
            m_formation_radius = r;
            //烟雾体积由 LodAutoScale 控制
            //烟雾体积异常变大可能是因为 SetRadius 与 LodAutoScale中OnEnable刷新scale 先后没法保证导致 
            //if ((bool)m_move_smoke)
            //{
            //    Vector3 localScale = m_move_smoke.transform.localScale;
            //    localScale.x = r;
            //    localScale.y = r;
            //    localScale.z = r;
            //    m_move_smoke.transform.localScale = localScale;
            //}
            m_minRotateAngle = -1.5f * m_formation_radius + 23f;
        }

        private void Reset()
        {
            m_hero_square = null;
            m_square_position_provider.Reset();
            m_row_dict.Clear();
            m_atkTick = 0;
            m_loadFrame = Time.frameCount;
        }

        public ENMU_MATRIX_TYPE GetFormationType()
        {
            return m_formation_type;
        }

        private void InitFormationBarbarian(string square_info, Color unit_color)
        {
            m_unit_color = unit_color;
            Barbarian component = GetComponent<Barbarian>();
            // 此处注释掉有报错， 应该是桥平原来格式有问题。
            component.InitFormationData(square_info);
            BarbarianConfig[] squareRows = component.m_squareRows;
            for (int i = 0; i < squareRows.Length; i++)
            {
                MatrixRow component2 = squareRows[i].GetComponent<MatrixRow>();
                component2.enabled = true;
                component2.InitRowBarbarian(string.Empty, 1, (squareRows[i].RowNum != 0) ? 1 : 0, m_unit_color, this);
                component2.ForceUpdateLod();
                if (i == 0)
                {
                    m_hero_square = component2;
                }
                m_row_dict.Add(i, component2);
            }
        }

        private void ChangeUnitColor(Color unit_color)
        {
            m_unit_color = unit_color;

            foreach (MatrixRow value in m_row_dict.Values)
            {
                value.ChangeUnitColor(m_unit_color);
            }
        }

        private void InitFormation(string formation_info, Color unit_color)
        {
            Reset();
            m_unit_color = unit_color;
            string[] array = formation_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            m_formation_type = (ENMU_MATRIX_TYPE)int.Parse(array[0]);
            m_square_position_provider.SetSquareType(m_formation_type);
            if (m_formation_type == ENMU_MATRIX_TYPE.RALLY)
            {
                max_row_number = 7;
            }
            else
            {
                max_row_number = 5;
            }
            if (m_formation_type == ENMU_MATRIX_TYPE.BARBARIAN)
            {
                InitFormationBarbarian(formation_info, unit_color);
                return;
            }
            for (int i = 1; i != array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                dictionary.Add(int.Parse(array2[0]), array2[1]);
            }
            Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
            Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
            foreach (int key2 in dictionary.Keys)
            {
                int displayUnitNumberByString = MatrixRowInfo.GetDisplayUnitNumberByString(dictionary[key2], key2, m_formation_type);
                dictionary2.Add(key2, MatrixRowInfo.GetRawRowNumberByUnitNumber(displayUnitNumberByString, key2, m_formation_type));
                dictionary3.Add(key2, displayUnitNumberByString);
            }
            if (max_row_number < dictionary2.Count)
            {
                Debug.LogError("Formation : max_row_number must be greater than unit type");
                return;
            }
            Dictionary<int, int> dictionary4 = new Dictionary<int, int>();
            int num = max_row_number - dictionary2.Count;
            List<int> list = new List<int>(dictionary3.Keys);
            for (int j = 0; j != list.Count; j++)
            {
                RemoveUnitFromRawDict(dictionary3, list[j]);
                dictionary4.Add(list[j], 1);
            }
            while (num != 0)
            {
                int largestUnitTypeFromRawDict = GetLargestUnitTypeFromRawDict(dictionary3);
                if (largestUnitTypeFromRawDict == -1)
                {
                    break;
                }
                Dictionary<int, int> dictionary5;
                int key;
                (dictionary5 = dictionary4)[key = largestUnitTypeFromRawDict] = dictionary5[key] + 1;
                RemoveUnitFromRawDict(dictionary3, largestUnitTypeFromRawDict);
                num--;
            }
            float num2 = 0f;
            bool flag = true;
            float num3 = 0f;
            float num4 = 0f;
            float num5 = 0f;
            int frameCount = m_loadFrame;
            int nCount = dictionary4.Keys.Count;
            foreach (int key3 in dictionary4.Keys)
            {
                string path = "row";
                int key = key3;
                CoreUtils.assetService.InstantiateNextFrame(path, (gameObject) =>
                {
                    if(m_loadFrame != frameCount)
                    {
                        CoreUtils.assetService.Destroy(gameObject);
                        return;
                    }
                    MatrixRow component = gameObject.GetComponent<MatrixRow>();
                    component.SetPositionProvider(m_square_position_provider);
                    m_row_dict.Add(key, component);
                    component.transform.SetParent(base.transform, worldPositionStays: false);
                    component.transform.localEulerAngles = Vector3.zero;
                    component.InitRow(dictionary[key], dictionary4[key], key, m_unit_color, this);
                    float num6 = CellDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(key, m_formation_type);
                    float num7 = CellDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(key, m_formation_type);
                    float num8 = (num6 + num7) * (float)component.squareRowInfo.curRowNumber;
                    float num9 = num8 - num7 + num2;
                    num4 += num8;
                    num3 -= num9;
                    component.transform.position = base.transform.position + base.transform.forward * num3;
                    num2 = num7;
                    if (flag)
                    {
                        m_hero_square = component;
                        flag = false;
                    }
                    if (key == 4)
                    {
                        num5 += num8;
                    }
                    nCount--;
                    if(nCount == 0)
                    {
                        UpdateSquareRowOffset(num4);
                        if (m_formation_type != ENMU_MATRIX_TYPE.BARBARIAN)
                        {
                            foreach (MatrixRow value in m_row_dict.Values)
                            {
                                value.ForceUpdateLod();
                            }
                        }
                    }
                });
            }
            if (m_move_smoke_particle_str != null && !m_move_smoke_particle_str.Equals(string.Empty))
            {
                CoreUtils.assetService.InstantiateNextFrame(m_move_smoke_particle_str, (@object) =>
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
                        m_move_smoke.SetActive(m_formation_state == ENMU_SQUARE_STAT.MOVE);
                    }
                });
            }
        }

        private void UpdateSquareRowOffset(float square_size)
        {
            float d = square_size * 0.5f;
            foreach (KeyValuePair<int, MatrixRow> item in m_row_dict)
            {
                MatrixRow value = item.Value;
                value.transform.position = value.transform.position + base.transform.forward * d;
            }
        }

        private void RemoveUnitFromRawDict(Dictionary<int, int> unit_raw_dict, int unit_type)
        {
            Dictionary<int, int> dictionary;
            int key;
            (dictionary = unit_raw_dict)[key = unit_type] = dictionary[key] - CellDatas.GetInstance().ReadUnitDisplayNumberInRow(unit_type, m_formation_type);
            if (unit_raw_dict[unit_type] < 0)
            {
                unit_raw_dict[unit_type] = 0;
            }
        }

        private int GetLargestUnitTypeFromRawDict(Dictionary<int, int> unit_raw_dict)
        {
            int result = -1;
            int num = 0;
            foreach (int key in unit_raw_dict.Keys)
            {
                int num2 = unit_raw_dict[key];
                if (num2 > num)
                {
                    result = key;
                    num = num2;
                }
            }
            return result;
        }

        public static void InitFormationS(Troops self, string formation_info, Color unit_color)
        {
            self.InitFormation(formation_info, unit_color);
        }

        public static void ChangeUnitColorS(Troops self, Color unit_color)
        {
            self.ChangeUnitColor(unit_color);
        }

        public static void SetHeroLevelUpEffectiveS(Troops self, string effectName)
        {
            if (self != null)
            {
                self.SetHeroLevelUpEffective(effectName);
            }
        }

        public void SetHeroLevelUpEffective(string effectName)
        {
            m_hero_square.SetEffectOnUnit(effectName);
        }

        private Vector3 GetShowPosition()
        {
            return m_hero_square.GetShowPosition();
        }

        public static Vector3 GetShowPositionS(Troops self)
        {
            return self.GetShowPosition();
        }

        public static void SetStateS(Troops self, ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f)
        {
            self.SetState(state, current_pos, target_pos, move_speed);
        }

        public static void SetCampS(Troops self, ENUM_FORMATION_CAMP camp)
        {
            if (self != null)
            {
                self.SetCamp(camp);
            }
        }

        private void SetCamp(ENUM_FORMATION_CAMP camp)
        {
            m_camp = camp;
            if (m_camp == ENUM_FORMATION_CAMP.Enemy)
            {
                m_curBeHitEffect = m_beHitEffect;
            }
            else
            {
                m_curBeHitEffect = m_beHitEffectEnemy;
            }
        }

        public static void SetBeAtkEffectCampS(Troops self, ENUM_FORMATION_CAMP camp)
        {
            if (self != null)
            {
                self.SetBeAtkEffectCamp(camp);
            }
        }

        private void SetBeAtkEffectCamp(ENUM_FORMATION_CAMP camp)
        {
            if (m_camp == ENUM_FORMATION_CAMP.Enemy)
            {
                m_curBeHitEffect = m_beHitEffect;
            }
            else
            {
                m_curBeHitEffect = m_beHitEffectEnemy;
            }
        }

        public static void TriggerSkillS(Troops self, string param, Vector3 pos)
        {
            if (self != null)
            {
                self.TriggerSkill(param, pos);
            }
        }

        private void TriggerSkill(string param, Vector3 pos)
        {
            MatrixRow squareRow = m_row_dict[0];
            squareRow.HeroSkillAni(int.Parse(param), pos);
        }

        public static void SetTargetMovableS(Troops self, string moved)
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

        public static void ReservedFunc1S(Troops self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc1(param);
            }
        }

        private void ReservedFunc1(string param)
        {
        }

        public static void ReservedFunc2S(Troops self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc2(param);
            }
        }

        private void ReservedFunc2(string param)
        {
        }

        public static void ReservedFunc3S(Troops self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc3(param);
            }
        }

        private void ReservedFunc3(string param)
        {
        }

        public static void ReservedFunc4S(Troops self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc4(param);
            }
        }

        private void ReservedFunc4(string param)
        {
        }

        public static void ReservedFunc5S(Troops self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc5(param);
            }
        }

        private void ReservedFunc5(string param)
        {
        }

        public static void ReservedFunc6S(Troops self, string param)
        {
            if (self != null)
            {
                self.ReservedFunc6(param);
            }
        }

        private void ReservedFunc6(string param)
        {
        }

        public void SetState(ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f)
        {
            // 这边的 s_ticktick 需要服务器 同步
            float s_ticktick = Common.s_ticktick;
            Vector2 normalized = (target_pos - current_pos).normalized;
            Vector3 forward = base.transform.forward;
            float x = forward.x;
            Vector3 forward2 = base.transform.forward;
            Vector2 vector = new Vector2(x, forward2.z);
            bool isMoveAtk = false;
            Vector3 b = new Vector3(current_pos.x, 0f, current_pos.y);
            float magnitude = (base.transform.position - b).magnitude;
            if (state == ENMU_SQUARE_STAT.FIGHT)
            {
                if (magnitude > 0.2f && m_atkTick != 0)
                {
                    isMoveAtk = true;
                    if (move_speed * s_ticktick > magnitude)
                    {
                        move_speed = ((!(s_ticktick < 0.9f)) ? (magnitude / s_ticktick) : 0f);
                    }
                }
                else
                {
                    Vector3 position = base.transform.position;
                    current_pos.x = position.x;
                    Vector3 position2 = base.transform.position;
                    current_pos.y = position2.z;
                    normalized = (target_pos - current_pos).normalized;
                    Vector3 to_ = new Vector3(normalized.x, 0f, normalized.y);

                    //兼容朝向归零（current_pos == target_pos）
                    if (normalized.Equals(Vector2.zero))
                    {
                        to_ = base.transform.forward;
                    }

                    float angle = Common.GetAngle360(Vector3.forward, base.transform.forward);
                    float angle2 = Common.GetAngle360(Vector3.forward, to_);
                    int num = (int)((angle + 22.5f) / 45f % 8f);
                    int num2 = (int)((angle2 + 22.5f) / 45f % 8f);
                    if (num != num2 || Mathf.Abs(angle2 - angle) > m_minRotateAngle)
                    {
                        isMoveAtk = true;
                    }
                }
            }
            foreach (MatrixRow value in m_row_dict.Values)
            {
                value.SetState(state, current_pos, target_pos, move_speed, isMoveAtk);
            }
            
            base.transform.position = new Vector3(current_pos.x, 0f, current_pos.y);
            m_start_pos = current_pos;
            m_target_pos = target_pos;            

            //防止在没有产生位移的情况下导致朝向瞬间归零
            //情况1：服务端在没有更新移动路径的情况下，下发移动相关状态
            //情况2：逻辑上的重复调用
            if (!normalized.Equals(Vector2.zero))
            {
                base.transform.forward = new Vector3(normalized.x, 0f, normalized.y); 
            }

            if (state == ENMU_SQUARE_STAT.MOVE)
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
            TransformAtkFormation(state);
            UpdateAtkEffectState(state);
            m_formation_state = state;

            if (m_formation_state == ENMU_SQUARE_STAT.MOVE)
            {
                if (m_formation_state != m_formation_last_state)
                {
                    if (m_move_smoke && !m_move_smoke.activeSelf)
                    {
                        m_move_smoke.SetActive(true);
                    }
                }
            }
            else
            {
                if (m_move_smoke && m_move_smoke.activeSelf)
                {
                    m_move_smoke.SetActive(false);
                }
            }

            m_formation_last_state = state;
        }

        private void TransformAtkFormation(ENMU_SQUARE_STAT state, bool isForce = false)
        {
            if (m_formation_state != ENMU_SQUARE_STAT.FIGHT && state == ENMU_SQUARE_STAT.FIGHT)
            {
                m_atkTick = 1;
            }
            else if (((m_formation_state == ENMU_SQUARE_STAT.FIGHT && state == ENMU_SQUARE_STAT.FIGHT && m_atkTick == 1) || (isForce && state == ENMU_SQUARE_STAT.FIGHT)) && m_row_dict.Keys.Count > 1)
            {
                m_atkTick = 2;
                int num = 0;
                Dictionary<int, float> dictionary = new Dictionary<int, float>();
                foreach (KeyValuePair<int, MatrixRow> item in m_row_dict)
                {
                    if (isForce)
                    {
                        item.Value.ResetAtkFormationParam();
                    }
                    if (!item.Value.CheckSquareRowIsEmpty())
                    {
                        int key = item.Key;
                        float innerFirstRowZ = item.Value.GetInnerFirstRowZ();
                        dictionary.Add(key, innerFirstRowZ);
                        if (key != 0)
                        {
                            num += item.Value.GetInnerRowNum();
                        }
                    }
                }
                List<KeyValuePair<int, float>> list = (from r in dictionary
                                                       orderby r.Value descending
                                                       select r).ToList();
                if (list.Count < 2)
                {
                    return;
                }
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
                    MatrixRow squareRow = m_row_dict[list[i].Key];
                    if (i == 1)
                    {
                        squareRow.SetIsFirstRow(isFirst: true);
                    }
                    else if (i == list.Count - 1)
                    {
                        squareRow.SetIsLastRow(isLastRow: true);
                    }
                    squareRow.TransformAtkFormation(isAtk: true, num2 - num3 * (float)num4, num3, isForce);
                    num4 += squareRow.GetInnerRowNum();
                }
            }
            else if ((m_formation_state == ENMU_SQUARE_STAT.FIGHT && state != ENMU_SQUARE_STAT.FIGHT && m_atkTick == 2) || (isForce && state != ENMU_SQUARE_STAT.FIGHT))
            {
                foreach (MatrixRow value3 in m_row_dict.Values)
                {
                    value3.TransformAtkFormation(isAtk: false, 0f, 0f, isForce);
                }
                m_atkTick = 0;
            }
        }

        public void TransformAtkFormationImmediately(ENMU_SQUARE_STAT state, bool isForce = false)
        {
            if (((m_formation_state != ENMU_SQUARE_STAT.FIGHT && state == ENMU_SQUARE_STAT.FIGHT) || (isForce && state == ENMU_SQUARE_STAT.FIGHT)) && m_row_dict.Keys.Count > 1)
            {
                int num = m_row_dict.Keys.Count - 1;
                Dictionary<int, float> dictionary = new Dictionary<int, float>();
                foreach (KeyValuePair<int, MatrixRow> item in m_row_dict)
                {
                    if (isForce)
                    {
                        item.Value.ResetAtkFormationParam();
                    }
                    if (!item.Value.CheckSquareRowIsEmpty())
                    {
                        int key = item.Key;
                        float innerFirstRowZ = item.Value.GetInnerFirstRowZ();
                        dictionary.Add(key, innerFirstRowZ);
                    }
                    else
                    {
                        num--;
                    }
                }
                List<KeyValuePair<int, float>> list = (from r in dictionary
                                                       orderby r.Value descending
                                                       select r).ToList();
                if (list.Count < 2)
                {
                    return;
                }
                float value = list[0].Value;
                float value2 = list[1].Value;
                float num2 = value - value2;
                if (num > 0)
                {
                    float num3 = num2 / (float)num;
                }
                for (int i = 1; i < list.Count; i++)
                {
                    MatrixRow squareRow = m_row_dict[list[i].Key];
                    if (i == 1)
                    {
                        squareRow.SetIsFirstRow(isFirst: true);
                    }
                    else if (i == list.Count - 1)
                    {
                        squareRow.SetIsLastRow(isLastRow: true);
                    }
                }
            }
            else if ((m_formation_state == ENMU_SQUARE_STAT.FIGHT && state != ENMU_SQUARE_STAT.FIGHT) || (isForce && state != ENMU_SQUARE_STAT.FIGHT))
            {
                foreach (MatrixRow value3 in m_row_dict.Values)
                {
                    MatrixRow squareRow2 = value3;
                }
            }
        }

        public static void SetFormationInfoS(Troops self, string formation_info)
        {
            self.SetFormationInfo(formation_info);
        }

        private void SetFormationInfoBarbarian(string square_info)
        {
            Barbarian component = GetComponent<Barbarian>();
            component.SetFormationData(square_info);
        }

        private void SetFormationInfo(string formation_info)
        {
            if (m_formation_type == ENMU_MATRIX_TYPE.BARBARIAN)
            {
                SetFormationInfoBarbarian(formation_info);
                return;
            }
            string[] array = formation_info.Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.RemoveEmptyEntries);
            new Dictionary<int, string>();
            m_square_position_provider.Reset();
            int num = 0;
            float num2 = 0f;
            bool flag = false;
            for (int i = 1; i != array.Length; i++)
            {
                string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                int num3 = int.Parse(array2[0]);
                string info_str = array2[1];
                flag |= m_row_dict[num3].squareRowInfo.UpdateUnitNumber(info_str);
                m_row_dict[num3].UpdateUnitNumber();
                num += m_row_dict[num3].GetRemoveUnitNum();
                if (num3 == 0)
                {
                    float num4 = CellDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(0);
                    float num5 = CellDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(0);
                    num2 = num4 + num5;
                }
                if (num3 == 4)
                {
                    float num6 = CellDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(4);
                    float num7 = CellDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(4);
                    num2 += (num6 + num7) * (float)m_row_dict[num3].squareRowInfo.curRowNumber;
                }
            }
            if (flag && m_formation_type != ENMU_MATRIX_TYPE.BARBARIAN)
            {
                float num8 = 0f;
                float num9 = 0f;
                float num10 = 0f;
                foreach (MatrixRow value in m_row_dict.Values)
                {
                    float num11 = CellDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(value.squareRowInfo.rowCategory);
                    float num12 = CellDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(value.squareRowInfo.rowCategory);
                    float f = (float)value.squareRowInfo.curRowNumber * 0.5f;
                    float num15 = num11 * Mathf.Ceil(f) + num12 * Mathf.Floor(f);
                    float num16 = num11 * Mathf.Floor(f) + num12 * Mathf.Ceil(f);
                    float num13 = (num11 + num12) * (float)value.squareRowInfo.curRowNumber;
                    float num14 = num13 - num12 + num8;
                    num10 += num13;
                    num9 -= num14;
                    value.ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE);
                    value.transform.position = base.transform.position + base.transform.forward * num9;
                    num8 = num12;
                }
                UpdateSquareRowOffset(num10);
            }
        }

        public static void InitPositionS(Troops self, Vector2 current_pos, Vector2 target_pos)
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
            
        }

        private new void OnDespawn()
        {
            if (m_formation_type == ENMU_MATRIX_TYPE.BARBARIAN)
            {
                foreach (MatrixRow value in m_row_dict.Values)
                {
                    if ((bool)value)
                    {
                        value.DestroyManual();
                    }
                }
            }
            else
            {
                foreach (MatrixRow value2 in m_row_dict.Values)
                {
                    if ((bool)value2)
                    {
                        CoreUtils.assetService.Destroy(value2.gameObject);
                    }
                }
            }
            if ((bool)m_move_smoke)
            {
                CoreUtils.assetService.Destroy(m_move_smoke);
                m_move_smoke = null;
            }
            Reset();
            m_curBeHitEffect = m_beHitEffect;
        }

        private void UpdateAtkEffectState(ENMU_SQUARE_STAT state)
        {
            if (m_formation_state != ENMU_SQUARE_STAT.FIGHT && state == ENMU_SQUARE_STAT.FIGHT)
            {
                m_atk_effect_launch = true;
                m_atk_effect_next_round_interval = m_atk_effect_param.roundInterval;
                m_atk_effect_this_round_trigger = (((float)UnityEngine.Random.Range(0.0f, 1.0f) <= m_atk_effect_param.roundTrigger) ? true : false);
            }
            else if (m_formation_state == ENMU_SQUARE_STAT.FIGHT && state != ENMU_SQUARE_STAT.FIGHT)
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
                if (m_formation_state == ENMU_SQUARE_STAT.MOVE && m_move_time != 0f)
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

        public static void FadeIn_S(Troops self)
        {
            self.FadeIn();
        }

        private void FadeIn()
        {
            foreach (MatrixRow value in m_row_dict.Values)
            {
                value.FadeIn();
            }
        }

        public static void FadeOut_S(Troops self)
        {
            self.FadeOut();
        }

        private void FadeOut()
        {
            foreach (MatrixRow value in m_row_dict.Values)
            {
                value.FadeOut();
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
            foreach (MatrixRow value in m_row_dict.Values)
            {
                value.ForceUpdateLod();
            }
            TransformAtkFormation(m_formation_state, isForce: true);
        }

        public new int GetCurrentLodLevel()
        {
            return base.GetCurrentLodLevel();
        }
    }
}
using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class MatrixRow : MonoBehaviour
    {
        private List<CellClone> m_dummy_array = new List<CellClone>();
        private List<CellClone> m_dummy_dynamic_array = new List<CellClone>();

        private MatrixRowInfo m_square_row_info;

        private MatrixPosProvider m_posProvider;

        public float m_destroy_unit_delay = 4f;

        private Color m_unit_color = Color.red;

        private float m_move_speed = 1f;

        private float m_chase_speed_factor = 2.8f;

        public float m_set_around_speed_factor = 0.9f;

        private Troops m_formation;

        public float m_row_height = 0.45f;

        public string m_unit_dummy_prefab = "unit_dummy";

        private List<float> m_sub_row_zoffset = new List<float>();

        private int m_remove_num;

        private float m_expansivity = 0.18f;

        private bool m_lastMoveAtk;

        private Vector2 m_current_pos;

        private Vector2 m_target_pos;

        private Troops.ENMU_SQUARE_STAT m_curState;

        private bool m_hasAtkFormation;

        private float m_atkOffset;

        private float m_atkOffsetStep;

        private bool m_isFirstRow;

        private bool m_isLastRow;

        private Dictionary<int, List<CellClone>> m_dummy_array_byrow;

        private int m_loadFrame;

        public MatrixRowInfo squareRowInfo
        {
            get
            {
                return m_square_row_info;
            }
            set
            {
                m_square_row_info = value;
            }
        }

        private Dictionary<int, List<CellClone>> GetUnitDummyDict()
        {
            Dictionary<int, List<CellClone>> dictionary = new Dictionary<int, List<CellClone>>();
            for (int i = 0; i != m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if (dictionary.ContainsKey(unit.unitType))
                {
                    dictionary[unit.unitType].Add(m_dummy_array[i]);
                    continue;
                }
                List<CellClone> list = new List<CellClone>();
                list.Add(m_dummy_array[i]);
                dictionary.Add(unit.unitType, list);
            }
            return dictionary;
        }

        public void SetPositionProvider(MatrixPosProvider pprovider)
        {
            m_posProvider = pprovider;
        }

        public Vector3 ProvidePosition(int cur_row, int sub_cur_row, int row_count, int cur_col, int col_count, int row_category)
        {
            if (m_posProvider == null)
            {
                m_posProvider = new MatrixPosProvider();
            }
            Vector3 unitPosition = m_posProvider.GetUnitPosition(cur_row, sub_cur_row, row_count, cur_col, col_count, row_category);
            if (!m_sub_row_zoffset.Contains(unitPosition.z))
            {
                m_sub_row_zoffset.Add(unitPosition.z);
                m_sub_row_zoffset.Sort((float x, float y) => -x.CompareTo(y));
            }
            return unitPosition;
        }

        public int GetRemoveUnitNum()
        {
            return m_remove_num;
        }

        public void UpdateUnitNumber()
        {
            if (m_square_row_info.rowCategory == 0)
            {
                return;
            }
            List<List<int>> displayUnitMap = m_square_row_info.GetDisplayUnitMap();
            Dictionary<int, List<CellClone>> unitDummyDict = GetUnitDummyDict();
            int currentLodLevel = m_formation.GetCurrentLodLevel();
            for (int i = 0; i != displayUnitMap.Count; i++)
            {
                if (currentLodLevel != 2)
                {
                    List<int> collection = new List<int>(displayUnitMap[i]);
                    for (int j = 1; j < CellDatas.GetInstance().ReadUnitLodMultiplier(m_square_row_info.rowCategory, currentLodLevel); j++)
                    {
                        displayUnitMap[i].AddRange(collection);
                    }
                    for (int k = 1; k <= CellDatas.GetInstance().ReadUnitLodAddend(m_square_row_info.rowCategory, currentLodLevel); k++)
                    {
                        displayUnitMap[i].Add(displayUnitMap[i][0]);
                    }
                }
            }
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int l = 0; l != displayUnitMap.Count; l++)
            {
                for (int m = 0; m != displayUnitMap[l].Count; m++)
                {
                    int num = displayUnitMap[l][m];
                    if (dictionary.ContainsKey(num))
                    {
                        Dictionary<int, int> dictionary2;
                        int key;
                        (dictionary2 = dictionary)[key = num] = dictionary2[key] + 1;
                    }
                    else
                    {
                        dictionary.Add(num, 1);
                    }
                }
            }
            List<CellClone> list = new List<CellClone>();
            foreach (KeyValuePair<int, int> item in dictionary)
            {
                if (unitDummyDict.ContainsKey(item.Key) && unitDummyDict[item.Key].Count > item.Value)
                {
                    list.AddRange(unitDummyDict[item.Key].GetRange(item.Value - 1, unitDummyDict[item.Key].Count - item.Value));
                }
            }
            foreach (int key2 in unitDummyDict.Keys)
            {
                if (!dictionary.ContainsKey(key2))
                {
                    list.AddRange(unitDummyDict[key2]);
                }
            }
            m_remove_num = list.Count;
            for (int n = 0; n != list.Count; n++)
            {
                list[n].m_unit.PlayDeadParticle();
                m_dummy_array.Remove(list[n]);
                CoreUtils.assetService.Destroy(list[n].m_unit.gameObject);
                CoreUtils.assetService.Destroy(list[n].gameObject);
            }
        }

        private void CloneUnitDummy(Vector3 pos, int unitType, string name, string unitPrefabName, int frameCount, Action<CellClone> callback)
        {
            if (m_formation == null)
            {
                Debug.Log("CloneUnitDummy: m_formation is null ----------- unitType = " + unitType);
            }
            int currentLodLevel = m_formation.GetCurrentLodLevel();
            CoreUtils.assetService.Instantiate(m_unit_dummy_prefab, (gameObject)=>
            {
                if (gameObject == null)
                {
                    Debug.Log("CloneUnitDummy: UnitDummy obj is nill --- unitType = " + unitType);
                }
                if(m_loadFrame != frameCount)
                {
                    CoreUtils.assetService.Destroy(gameObject);
                    return;
                }
                gameObject.name = name + "_dynamic";
                gameObject.transform.SetParent(base.transform, worldPositionStays: false);
                gameObject.transform.localPosition = pos;
                CellClone component = gameObject.GetComponent<CellClone>();
                component.UpdateInitPos();
                string text = unitPrefabName;
                if (text == string.Empty)
                {
                    text = CellDatas.GetInstance().ReadUnitPrefabPathByType(unitType);
                }
                if (text == string.Empty)
                {
                    Debug.Log("CloneUnitDummy: Unit prefab name is nill --- unitType = " + unitType);
                }

                CoreUtils.assetService.Instantiate(text, (gameObject2) =>
                {
                    if (gameObject2 == null)
                    {
                        Debug.Log("CloneUnitDummy: Unit obj is nill --- unitType = " + unitType);
                    }
                    if (m_loadFrame != frameCount)
                    {
                        CoreUtils.assetService.Destroy(gameObject);
                        CoreUtils.assetService.Destroy(gameObject2);
                        return;
                    }
                    CellBase component2 = gameObject2.GetComponent<CellBase>();
                    SpriteRenderer component3 = gameObject2.GetComponent<SpriteRenderer>();
                    component3.color = m_unit_color;
                    component.m_unit = component2;
                    component2.InitDummy(component);
                    component2.SetSpriteLoigicalState(GetUnitSpriteState(m_formation.m_formation_state));
                    component2.ChangeMoveState(CellBase.MOVE_STATE.STATIC);
                    component2.unitType = unitType;
                    gameObject2.GetComponent<AnimationBase>().SetCurLodForParticle(currentLodLevel);
                    component2.FadeIn();
                    callback?.Invoke(component);
                });
            });
        }

        public void UpdateUnitPositionBarabarian()
        {
            int currentLodLevel = m_formation.GetCurrentLodLevel();
            BarbarianConfig component = GetComponent<BarbarianConfig>();
            int frameCount = m_loadFrame;
            int nCount = component.m_UnitDummys.Length;
            for (int nIndex = 0; nIndex < component.m_UnitDummys.Length; nIndex++)
            {
                int i = nIndex;
                string unitPrefab = component.m_UnitDummys[i].unitPrefab;
                if (unitPrefab != string.Empty)
                {
                    CellClone component2 = component.m_UnitDummys[i].unitDummy.GetComponent<CellClone>();
                    CoreUtils.assetService.Instantiate(unitPrefab, (gameObject) =>
                    {
                        if (m_loadFrame != frameCount)
                        {
                            CoreUtils.assetService.Destroy(gameObject);
                            return;
                        }
                        gameObject.transform.SetParent(component2.transform, worldPositionStays: false);
                        gameObject.transform.localPosition = Vector3.zero;
                        CellBase component3 = gameObject.GetComponent<CellBase>();
                        component3.InitDummy(component2);
                        component2.ResumeInitPos();
                        component2.UpdateInitPos();
                        m_dummy_array.Add(component2);
                        SpriteRenderer component4 = gameObject.GetComponent<SpriteRenderer>();
                        if (component4 != null)
                        {
                            component4.color = m_unit_color;
                        }
                        component2.m_unit = component3;
                        component3.SetSpriteLoigicalState(GetUnitSpriteState(m_formation.m_formation_state));
                        component3.ChangeMoveState(CellBase.MOVE_STATE.STATIC);
                        component3.unitType = component.m_UnitDummys[i].unitType;
                        if (gameObject.GetComponent<AnimationBase>() != null)
                        {
                            gameObject.GetComponent<AnimationBase>().SetCurLodForParticle(currentLodLevel);
                        }
                        component3.FadeIn();
                        nCount--;
                        if (nCount == 0)
                        {
                            UpdateUnitPositionBarabarianLod(currentLodLevel, frameCount);
                        }
                    });
                    
                }
            }
        }

        private void UpdateUnitPositionBarabarianLod(int currentLodLevel, int frameCount)
        {
            if (currentLodLevel > 1 || m_square_row_info.rowCategory == 0)
            {
                return;
            }
            int nCount = m_dummy_array.Count;
            for (int j = 0; j < nCount; j++)
            {
                CellClone unitDummy = m_dummy_array[j];
                Vector3 orgPos = unitDummy.OrgPos;
                Vector3 orgPos2 = unitDummy.OrgPos;
                Vector3 orgPos3 = unitDummy.OrgPos;
                if (j % 2 == 0)
                {
                    orgPos.z += m_expansivity;
                    orgPos2.x -= m_expansivity * 1.414f;
                    orgPos2.z -= m_expansivity * 1.414f;
                    orgPos3.x += m_expansivity * 1.414f;
                    orgPos3.z -= m_expansivity * 1.414f;
                }
                else
                {
                    orgPos.z -= m_expansivity;
                    orgPos2.x -= m_expansivity * 1.414f;
                    orgPos2.z += m_expansivity * 1.414f;
                    orgPos3.x += m_expansivity * 1.414f;
                    orgPos3.z += m_expansivity * 1.414f;
                }
                unitDummy.transform.localPosition = orgPos;
                string empty = string.Empty;
                //string unitPrefabName;
                //if (unitDummy.m_unit.name.StartsWith("wolf"))
                //{
                //    unitPrefabName = "archery_t1";
                //}
                //else if (unitDummy.m_unit.name.StartsWith("bear"))
                //{
                //    unitPrefabName = "infantry_t1";
                //}
                //else
                //{
                //    string name = unitDummy.m_unit.gameObject.name;
                //    unitPrefabName = name.Replace("(Clone)", string.Empty);
                //}
                string unitPrefabName = string.Empty;
                if (unitDummy.m_unit.cloneToPrefab != string.Empty)
                {
                    unitPrefabName = unitDummy.m_unit.cloneToPrefab;
                }
                else
                {
                    string name = unitDummy.m_unit.gameObject.name;
                    unitPrefabName = name.Replace("(Clone)", string.Empty);
                }
                CloneUnitDummy(orgPos2, unitDummy.m_unit.unitType, unitDummy.gameObject.name, unitPrefabName, frameCount, (dummy) =>
                {
                    m_dummy_dynamic_array.Add(dummy);
                    m_dummy_array.Add(dummy);
                });
                CloneUnitDummy(orgPos3, unitDummy.m_unit.unitType, unitDummy.gameObject.name, unitPrefabName, frameCount, (dummy) =>
                {
                    m_dummy_dynamic_array.Add(dummy);
                    m_dummy_array.Add(dummy);
                });
            }
        }

        public void UpdateUnitPosition()
        {
            int currentLodLevel = m_formation.GetCurrentLodLevel();
            int frameCount = m_loadFrame;
            if (m_square_row_info == null)
            {
                return;
            }
            List<List<int>> displayUnitMap = m_square_row_info.GetDisplayUnitMap();
            int count = displayUnitMap.Count;
            for (int nIndex = 0; nIndex != count; nIndex++)
            {
                int i = nIndex;
                if (currentLodLevel != 2)
                {
                    List<int> collection = new List<int>(displayUnitMap[i]);
                    for (int j = 1; j < CellDatas.GetInstance().ReadUnitLodMultiplier(m_square_row_info.rowCategory, currentLodLevel); j++)
                    {
                        displayUnitMap[i].AddRange(collection);
                    }
                    for (int k = 1; k <= CellDatas.GetInstance().ReadUnitLodAddend(m_square_row_info.rowCategory, currentLodLevel); k++)
                    {
                        displayUnitMap[i].Add(displayUnitMap[i][0]);
                    }
                }
                List<List<int>> list = new List<List<int>>();
                int num = 3 - currentLodLevel;
                int count2 = displayUnitMap[i].Count;
                int num2 = 0;
                int num3 = count2 / num;
                if (m_square_row_info.rowCategory == 0)
                {
                    list.Add(displayUnitMap[i].GetRange(0, displayUnitMap[i].Count));
                }
                else
                {
                    for (int l = 0; l != num; l++)
                    {
                        List<int> list2 = new List<int>();
                        if (l % 2 == 0)
                        {
                            list2.AddRange(displayUnitMap[i].GetRange(num2, num3));
                            num2 += num3;
                        }
                        else
                        {
                            list2.AddRange(displayUnitMap[i].GetRange(num2, num3 + 1));
                            num2 += num3 + 1;
                        }
                        list.Add(list2);
                    }
                }
                for (int row = 0; row != list.Count; row++)
                {
                    int m = row;
                    int count3 = list[m].Count;
                    for (int col = 0; col != list[m].Count; col++)
                    {
                        int n = col;
                        CoreUtils.assetService.InstantiateNextFrame(m_unit_dummy_prefab, (gameObject) =>
                        {
                            if (m_loadFrame != frameCount)
                            {
                                CoreUtils.assetService.Destroy(gameObject);
                                return;
                            }
                            gameObject.transform.SetParent(base.transform, worldPositionStays: false);
                            Vector3 zero = Vector3.zero;
                            if (m_posProvider == null || m_posProvider.GetSquareType() != Troops.ENMU_MATRIX_TYPE.BARBARIAN)
                            {
                                Vector3 localPosition = ProvidePosition(i, m, num, n, count3, m_square_row_info.rowCategory);
                                gameObject.transform.localPosition = localPosition;
                                if (m_square_row_info.rowCategory == 0 && displayUnitMap[i][n] >= 1000 && displayUnitMap[i][n] < 10000)
                                    //if (m_square_row_info.rowCategory == 0 && displayUnitMap[i][n] > 10000 && displayUnitMap[i][n] < 20000)
                                {
                                    int unitPriority = m_square_row_info.GetUnitPriority(displayUnitMap[i][n]);
                                    int type = 1;
                                    if (unitPriority == 2)
                                    {
                                        type = 3;
                                    }
                                    float num4 = CellDatas.GetInstance().ReadHeroXOffset(type, m_formation.GetFormationType());
                                    float num5 = CellDatas.GetInstance().ReadHeroZOffset(type, m_formation.GetFormationType());
                                    Vector3 localPosition2 = gameObject.transform.localPosition;
                                    localPosition2.x += num4;
                                    localPosition2.z += num5;
                                    gameObject.transform.localPosition = localPosition2;
                                    if (unitPriority == 1)
                                    {
                                        CoreUtils.assetService.InstantiateNextFrame(m_unit_dummy_prefab, (gameObject2) =>
                                        {
                                            if (m_loadFrame != frameCount)
                                            {
                                                CoreUtils.assetService.Destroy(gameObject2);
                                                return;
                                            }
                                            gameObject2.transform.SetParent(base.transform, worldPositionStays: false);
                                            num4 = CellDatas.GetInstance().ReadHeroXOffset(2, m_formation.GetFormationType());
                                            num5 = CellDatas.GetInstance().ReadHeroZOffset(2, m_formation.GetFormationType());
                                            gameObject2.transform.localPosition = new Vector3(localPosition.x + num4, 0f, localPosition.z + num5);
                                            CellClone component = gameObject2.GetComponent<CellClone>();
                                            component.UpdateInitPos();
                                            m_dummy_array.Add(component);

                                            CoreUtils.assetService.InstantiateNextFrame(/*"flagman"*/"Arms1_1", (gameObject3) =>
                                            {
                                                if (m_loadFrame != frameCount)
                                                {
                                                    CoreUtils.assetService.Destroy(gameObject3);
                                                    return;
                                                }
                                                CellBase component2 = gameObject3.GetComponent<CellBase>();
                                                SpriteRenderer component3 = gameObject3.GetComponent<SpriteRenderer>();
                                                component3.color = m_unit_color;
                                                component.m_unit = component2;
                                                component2.InitDummy(component);
                                                component2.SetSpriteLoigicalState(GetUnitSpriteState(m_formation.m_formation_state));
                                                component2.ChangeMoveState(CellBase.MOVE_STATE.STATIC);
                                                component2.unitType = 9999;
                                                gameObject3.GetComponent<AnimationBase>().SetCurLodForParticle(currentLodLevel);
                                                component2.FadeIn();
                                            });
                                        });
                                    }
                                }
                            }
                            
                          //  Debug.LogError("需要加载的资源名称"+UnityGameDatas.GetInstance().ReadUnitPrefabPathByType(displayUnitMap[i][n]));
                            CoreUtils.assetService.InstantiateNextFrame(CellDatas.GetInstance().ReadUnitPrefabPathByType(displayUnitMap[i][n]), (gameObject4) =>
                            {
                                if (m_loadFrame != frameCount)
                                {
                                    CoreUtils.assetService.Destroy(gameObject4);
                                    CoreUtils.assetService.Destroy(gameObject);
                                    return;
                                }
                                CellClone component4 = gameObject.GetComponent<CellClone>();
                                component4.UpdateInitPos();
                                m_dummy_array.Add(component4);
                                CellBase component5 = gameObject4.GetComponent<CellBase>();
                                SpriteRenderer component6 = gameObject4.GetComponent<SpriteRenderer>();
                                component6.color = m_unit_color;
                                component4.m_unit = component5;
                                component5.InitDummy(component4);
                                component5.SetSpriteLoigicalState(GetUnitSpriteState(m_formation.m_formation_state));
                                component5.ChangeMoveState(CellBase.MOVE_STATE.STATIC);
                                component5.unitType = displayUnitMap[i][n];
                                gameObject4.GetComponent<AnimationBase>().SetCurLodForParticle(currentLodLevel);
                                component5.FadeIn();
                            });
                        });
                    }
                }
            }
        }

        private void DoUpdateLod()
        {
            ResetUnit();
        }

        public void ForceUpdateLod()
        {
            TransformAtkFormation(isAtk: false, 0f, 0f, immediate: true);
            ResetUnit();
        }

        private void ResetUnit()
        {
            m_sub_row_zoffset.Clear();
            for (int i = 0; i != m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if ((bool)unit)
                {
                    CoreUtils.assetService.Destroy(unit.gameObject);
                }
                if (m_formation.GetFormationType() != Troops.ENMU_MATRIX_TYPE.BARBARIAN)
                {
                    CoreUtils.assetService.Destroy(m_dummy_array[i].gameObject);
                }
            }
            m_dummy_array.Clear();
            for (int i = 0; i != m_dummy_dynamic_array.Count; i++)
            {
                CoreUtils.assetService.Destroy(m_dummy_dynamic_array[i].gameObject);
            }
            m_dummy_dynamic_array.Clear();
            m_loadFrame = Time.frameCount;
            if (m_formation.GetFormationType() == Troops.ENMU_MATRIX_TYPE.BARBARIAN)
            {
                UpdateUnitPositionBarabarian();
            }
            else
            {
                UpdateUnitPosition();
            }
        }

        private float GetUnitHorizontalDistance(int unit_number)
        {
            return CellDatas.GetInstance().ReadUnitRowWidthByCategory(m_square_row_info.rowCategory, m_formation.GetFormationType()) / (float)unit_number;
        }

        private float GetUnitVerticalDistance(int row_number)
        {
            return m_row_height / (float)row_number;
        }

        private void SetUnitLogicalState(Troops.ENMU_SQUARE_STAT formation_state)
        {
            AnimationBase.ENMU_SPRITE_STATE unitSpriteState = GetUnitSpriteState(formation_state);
            for (int i = 0; i != m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if ((bool)unit)
                {
                    unit.SetSpriteLoigicalState(unitSpriteState);
                    if (formation_state == Troops.ENMU_SQUARE_STAT.DEAD)
                    {
                        unit.PlayDeadParticle();
                    }
                }
            }
            if (formation_state == Troops.ENMU_SQUARE_STAT.DEAD)
            {
                DestroyUnit();
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

        public void ChangeUnitMoveState(CellBase.MOVE_STATE state, bool isMoveAtk = false)
        {
            for (int i = 0; i != m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if ((bool)unit)
                {
                    if (isMoveAtk)
                    {
                        unit.SetChaseMode(CellBase.CHASE_MODE.ROTATE_SPRITE);
                    }
                    else
                    {
                        unit.SetChaseMode(CellBase.CHASE_MODE.ORIGIN_SPRITE);
                    }
                    unit.ChangeMoveState(state, isMoveAtk);
                }
            }
        }

        private void ChangeUnitChaseSpeedForce(float chase_speed)
        {
            for (int i = 0; i != m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if ((bool)unit)
                {
                    unit.SetMoveSpeedForce(chase_speed);
                }
            }
        }

        private void ChangeUnitChaseSpeed(float chase_speed)
        {
            for (int i = 0; i != m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if ((bool)unit)
                {
                    unit.SetMoveSpeed(chase_speed);
                }
            }
        }

        private void DestroyUnit()
        {
            for (int num = m_dummy_array.Count - 1; num >= 0; num--)
            {
                CellBase unit = m_dummy_array[num].m_unit;
                if ((bool)unit)
                {
                    CoreUtils.assetService.Destroy(unit.gameObject);
                }
                if (m_formation.GetFormationType() != Troops.ENMU_MATRIX_TYPE.BARBARIAN)
                {
                    CoreUtils.assetService.Destroy(m_dummy_array[num].gameObject);
                }
            }
            m_dummy_array.Clear();
        }

        public void HeroSkillAni(int heroId, Vector3 pos)
        {
            if (squareRowInfo.rowCategory != 0)
            {
                return;
            }
            for (int i = 0; i < m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if (unit.unitType == heroId)
                {
                    AnimationHero component = unit.GetComponent<AnimationHero>();
                    if (component != null)
                    {
                        component.StartSkillAni(pos);
                    }
                }
            }
        }
        public void ChangeUnitColor(Color unit_color)
        {
            m_unit_color = unit_color;
            for (int i = 0; i < m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                SpriteRenderer spriteRenderer = unit.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = m_unit_color;
                }
            }
        }
        public void InitRow(string row_info_str, int row_number, int row_category, Color unit_color, Troops formation)
        {
            m_square_row_info = new MatrixRowInfo(row_info_str, row_number, row_category, formation.GetFormationType());
            m_unit_color = unit_color;
            m_formation = formation;
            m_row_height = CellDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(m_square_row_info.rowCategory, formation.GetFormationType()) + CellDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(m_square_row_info.rowCategory, formation.GetFormationType());
        }

        public void InitRowBarbarian(string row_info_str, int row_number, int row_category, Color unit_color, Troops formation)
        {
            m_square_row_info = new MatrixRowInfo(row_info_str, row_number, row_category, formation.GetFormationType());
            m_unit_color = unit_color;
            m_formation = formation;
        }

        public void SetState(Troops.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f, bool isMoveAtk = false)
        {
            Vector2 zero = Vector2.zero;
            bool flag = false;
            SetUnitLogicalState(state);
            switch (state)
            {
                case Troops.ENMU_SQUARE_STAT.IDLE:
                    ChangeUnitChaseSpeed(m_move_speed * m_chase_speed_factor);
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
                    ChangeUnitChaseSpeed(m_move_speed * m_chase_speed_factor);
                    ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE);
                    break;
                case Troops.ENMU_SQUARE_STAT.FIGHT:
                    if (isMoveAtk || m_lastMoveAtk != isMoveAtk)
                    {
                        m_lastMoveAtk = isMoveAtk;
                        if (move_speed != 0f)
                        {
                            m_move_speed = move_speed;
                        }
                        ChangeUnitChaseSpeedForce(m_move_speed);
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
            m_current_pos = current_pos;
            m_target_pos = target_pos;
            m_curState = state;
        }

        private void Update()
        {
        }

        public void DestroyManual()
        {
            OnDespawn();
        }

        private void OnDespawn()
        {
            DestroyUnit();
            base.enabled = false;
            m_hasAtkFormation = false;
            m_lastMoveAtk = false;
            m_dummy_array_byrow = null;
            ResetAtkFormationParam();
            m_isFirstRow = false;
            m_isLastRow = false;
        }

        public Vector3 GetShowPosition()
        {
            if (m_dummy_array.Count == 0)
            {
                return Vector3.zero;
            }
            return m_dummy_array[0].m_unit.transform.position;
        }

        public void SetEffectOnUnit(string effectName)
        {
            if (m_dummy_array.Count > 0)
            {
                m_dummy_array[0].m_unit.SetLevelupEffect(effectName);
            }
        }

        public void FadeIn()
        {
            for (int i = 0; i != m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if ((bool)unit)
                {
                    unit.FadeIn();
                }
            }
        }

        public void FadeOut()
        {
            for (int i = 0; i != m_dummy_array.Count; i++)
            {
                CellBase unit = m_dummy_array[i].m_unit;
                if ((bool)unit)
                {
                    unit.FadeOut();
                }
            }
        }

        public bool CheckSquareRowIsEmpty()
        {
            return m_dummy_array.Count == 0;
        }

        public float GetInnerFirstRowZ()
        {
            if (gameObject == null)
            {
                return 0f;
            }
            if (m_square_row_info.rowCategory == 0)
            {
                float num = -999f;
                for (int i = 0; i < m_dummy_array.Count; i++)
                {
                    CellClone unitDummy = m_dummy_array[i];
                    if (unitDummy != null)
                    {
                        float num2 = num;
                        Vector3 localPosition = unitDummy.transform.localPosition;
                        if (num2 < localPosition.z)
                        {
                            Vector3 localPosition2 = unitDummy.transform.localPosition;
                            num = localPosition2.z;
                        }
                    }
                }
                float num3 = num;
                Vector3 localPosition3 = base.gameObject.transform.localPosition;
                return num3 + localPosition3.z;
            }
            Dictionary<int, List<CellClone>> dummyByInnerRow = GetDummyByInnerRow();
            if (dummyByInnerRow.Values.Count == 0 || !dummyByInnerRow.ContainsKey(0))
            {
                return 0f;
            }
            List<CellClone> list = dummyByInnerRow[0];
            if (list.Count == 0)
            {
                return 0f;
            }
            CellClone unitDummy2 = list[0];
            if (unitDummy2 == null)
            {
                return 0f;
            }
            Vector3 localPosition4 = unitDummy2.gameObject.transform.localPosition;
            float z = localPosition4.z;
            Vector3 localPosition5 = base.gameObject.transform.localPosition;
            return z + localPosition5.z;
        }

        public void SetIsFirstRow(bool isFirst)
        {
            m_isFirstRow = isFirst;
        }

        public void SetIsLastRow(bool isLastRow)
        {
            m_isLastRow = isLastRow;
        }

        public void ResetAtkFormationParam()
        {
            m_hasAtkFormation = false;
            m_atkOffset = 0f;
            m_atkOffsetStep = 0f;
        }

        private int GetSubRowNum(CellClone ud)
        {
            for (int i = 0; i < m_sub_row_zoffset.Count; i++)
            {
                float z;
                if (ud.InitedOrgPos)
                {
                    Vector3 orgPos = ud.OrgPos;
                    z = orgPos.z;
                }
                else
                {
                    Vector3 localPosition = ud.transform.localPosition;
                    z = localPosition.z;
                }
                if (Mathf.Abs(m_sub_row_zoffset[i] - z) <= 0.05f)
                {
                    return i;
                }
            }
            return 0;
        }

        private Dictionary<int, List<CellClone>> GetDummyByInnerRow()
        {
            if (m_dummy_array_byrow == null)
            {
                Dictionary<int, List<CellClone>> dictionary = new Dictionary<int, List<CellClone>>();
                for (int i = 0; i < m_dummy_array.Count; i++)
                {
                    CellClone unitDummy = m_dummy_array[i];
                    int subRowNum = GetSubRowNum(unitDummy);
                    if (!dictionary.ContainsKey(subRowNum))
                    {
                        dictionary.Add(subRowNum, new List<CellClone>());
                    }
                    dictionary[subRowNum].Add(unitDummy);
                }
                m_dummy_array_byrow = dictionary;
            }
            return m_dummy_array_byrow;
        }

        public int GetInnerRowNum()
        {
            Dictionary<int, List<CellClone>> dummyByInnerRow = GetDummyByInnerRow();
            return dummyByInnerRow.Count;
        }

        public void TransformAtkFormation(bool isAtk, float zOffset, float stepLen, bool immediate = false)
        {
            if (isAtk && !m_hasAtkFormation)
            {
                m_hasAtkFormation = true;
                m_atkOffset = zOffset;
                m_atkOffsetStep = stepLen;
                if (!immediate)
                {
                    ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE);
                    ChangeUnitChaseSpeed(1.2f);
                }
                Dictionary<int, List<CellClone>> dummyByInnerRow = GetDummyByInnerRow();
                int count = dummyByInnerRow.Count;
                int num = 0;
                for (int i = 0; i < dummyByInnerRow.Count; i++)
                {
                    if (!dummyByInnerRow.ContainsKey(i))
                    {
                        Debug.LogWarning("atk: dummy_array_byrow not contains key ---- " + i);
                        continue;
                    }
                    List<CellClone> list = dummyByInnerRow[i];
                    num += list.Count;
                    int num2 = Mathf.FloorToInt((float)list.Count * 0.5f + 0.5f);
                    float num3 = m_atkOffset - m_atkOffsetStep * (float)i;
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j] != null)
                        {
                            Vector3 localPosition = list[j].transform.localPosition;
                            localPosition.z += num3;
                            if (m_isFirstRow && i == 0)
                            {
                                if (j < num2)
                                {
                                    localPosition.x -= 0.75f;
                                }
                                else
                                {
                                    localPosition.x += 0.75f;
                                }
                            }
                            list[j].transform.localPosition = localPosition;
                        }
                    }
                }
            }
            if (isAtk || !m_hasAtkFormation)
            {
                return;
            }
            m_hasAtkFormation = false;
            if (!immediate)
            {
                ChangeUnitMoveState(CellBase.MOVE_STATE.CHASE);
                ChangeUnitChaseSpeed(1f);
            }
            if (m_dummy_array_byrow == null)
            {
                m_dummy_array_byrow = GetDummyByInnerRow();
            }
            int count2 = m_dummy_array_byrow.Count;
            int num4 = 0;
            for (int k = 0; k < m_dummy_array_byrow.Count; k++)
            {
                if (!m_dummy_array_byrow.ContainsKey(k))
                {
                    Debug.LogWarning("Normal: dummy_array_byrow not contains key ---- " + k);
                    continue;
                }
                List<CellClone> list2 = m_dummy_array_byrow[k];
                num4 += list2.Count;
                int num5 = Mathf.FloorToInt((float)list2.Count * 0.5f + 0.5f);
                float num6 = m_atkOffset - m_atkOffsetStep * (float)k;
                for (int l = 0; l < list2.Count; l++)
                {
                    if (list2[l] != null)
                    {
                        Vector3 localPosition2 = list2[l].transform.localPosition;
                        localPosition2.z -= num6;
                        if (m_isFirstRow && k == 0)
                        {
                            if (l < num5)
                            {
                                localPosition2.x += 0.75f;
                            }
                            else
                            {
                                localPosition2.x -= 0.75f;
                            }
                        }
                        list2[l].transform.localPosition = localPosition2;
                    }
                }
            }
            m_dummy_array_byrow = null;
        }
    }
}
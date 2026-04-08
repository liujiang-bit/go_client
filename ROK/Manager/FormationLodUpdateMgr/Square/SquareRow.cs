using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    [Serializable]
    public class SquareRow : MonoBehaviour
    {
        private List<UnitDummy> m_dummy_array = new List<UnitDummy>();

        private SquareRowInfo m_square_row_info;

        private SquarePositionProvider m_posProvider;

        public float m_destroy_unit_delay = 4f;

        private Color m_unit_color = Color.red;

        private float m_move_speed = 1f;

        private float m_chase_speed_factor = 1f;

        public float m_set_around_speed_factor = 0.9f;

        private Formation m_formation;

        public float m_row_height = 0.45f;

        public string m_unit_dummy_prefab = "unit_dummy";

        private List<float> m_sub_row_zoffset = new List<float>();

        private int m_remove_num;

        private float m_expansivity = 0.18f;

        private bool m_lastMoveAtk;

        private Vector2 m_current_pos;

        private Vector2 m_target_pos;

        private Formation.ENMU_SQUARE_STAT m_curState;

        private bool m_hasAtkFormation;

        private float m_atkOffset;

        private float m_atkOffsetStep;

        private bool m_isFirstRow;

        private bool m_isLastRow;

        private Dictionary<int, List<UnitDummy>> m_dummy_array_byrow;

        public SquareRowInfo squareRowInfo
        {
            get
            {
                return this.m_square_row_info;
            }
            set
            {
                this.m_square_row_info = value;
            }
        }

        private Dictionary<int, List<UnitDummy>> GetUnitDummyDict()
        {
            Dictionary<int, List<UnitDummy>> dictionary = new Dictionary<int, List<UnitDummy>>();
            for (int i = 0; i < this.m_dummy_array.Count; i++)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (dictionary.ContainsKey(unit.unitType))
                {
                    dictionary[unit.unitType].Add(this.m_dummy_array[i]);
                }
                else
                {
                    List<UnitDummy> list = new List<UnitDummy>();
                    list.Add(this.m_dummy_array[i]);
                    dictionary.Add(unit.unitType, list);
                }
            }
            return dictionary;
        }

        public void SetPositionProvider(SquarePositionProvider pprovider)
        {
            this.m_posProvider = pprovider;
        }

        public Vector3 ProvidePosition(int cur_row, int sub_cur_row, int row_count, int cur_col, int col_count, int row_category)
        {
            if (this.m_posProvider == null)
            {
                this.m_posProvider = new SquarePositionProvider();
            }
            Vector3 unitPosition = this.m_posProvider.GetUnitPosition(cur_row, sub_cur_row, row_count, cur_col, col_count, row_category);
            if (!this.m_sub_row_zoffset.Contains(unitPosition.z))
            {
                this.m_sub_row_zoffset.Add(unitPosition.z);
                this.m_sub_row_zoffset.Sort((float x, float y) => -x.CompareTo(y));
            }
            return unitPosition;
        }

        public int GetRemoveUnitNum()
        {
            return this.m_remove_num;
        }

        public void UpdateUnitNumber()
        {
            if (this.m_square_row_info.rowCategory == 0)
            {
                return;
            }
            List<List<int>> displayUnitMap = this.m_square_row_info.GetDisplayUnitMap();
            Dictionary<int, List<UnitDummy>> unitDummyDict = this.GetUnitDummyDict();
            int currentLodLevel = this.m_formation.GetCurrentLodLevel();
            for (int i = 0; i < displayUnitMap.Count; i++)
            {
                if (currentLodLevel != 2)
                {
                    List<int> collection = new List<int>(displayUnitMap[i]);
                    for (int j = 1; j < UnityGameDatas.GetInstance().ReadUnitLodMultiplier(this.m_square_row_info.rowCategory, currentLodLevel); j++)
                    {
                        displayUnitMap[i].AddRange(collection);
                    }
                    for (int k = 1; k <= UnityGameDatas.GetInstance().ReadUnitLodAddend(this.m_square_row_info.rowCategory, currentLodLevel); k++)
                    {
                        displayUnitMap[i].Add(displayUnitMap[i][0]);
                    }
                }
            }
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int l = 0; l < displayUnitMap.Count; l++)
            {
                for (int m = 0; m < displayUnitMap[l].Count; m++)
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
            List<UnitDummy> list = new List<UnitDummy>();
            foreach (KeyValuePair<int, int> current in dictionary)
            {
                if (unitDummyDict.ContainsKey(current.Key))
                {
                    if (unitDummyDict[current.Key].Count > current.Value)
                    {
                        list.AddRange(unitDummyDict[current.Key].GetRange(current.Value - 1, unitDummyDict[current.Key].Count - current.Value));
                    }
                }
            }
            foreach (int current2 in unitDummyDict.Keys)
            {
                if (!dictionary.ContainsKey(current2))
                {
                    list.AddRange(unitDummyDict[current2]);
                }
            }
            this.m_remove_num = list.Count;
            for (int n = 0; n < list.Count; n++)
            {
                list[n].m_unit.PlayDeadParticle();
                this.m_dummy_array.Remove(list[n]);
                CoreUtils.assetService.Destroy(list[n].m_unit.gameObject);
                CoreUtils.assetService.Destroy(list[n].gameObject);
            }
        }

        private void CloneUnitDummy(Vector3 pos, int unitType, string name, string unitPrefabName, Action<UnitDummy> action)
        {
            if (this.m_formation == null)
            {
                Debug.Log("CloneUnitDummy: m_formation is null ----------- unitType = " + unitType);
            }
            int currentLodLevel = this.m_formation.GetCurrentLodLevel();
            CoreUtils.assetService.Instantiate(m_unit_dummy_prefab, (GameObject gameObject) =>
            {
                if (gameObject == null)
                {
                    Debug.Log("CloneUnitDummy: UnitDummy obj is nill --- unitType = " + unitType);
                }
                gameObject.name = name + "_dynamic";
                gameObject.transform.SetParent(base.transform, false);
                gameObject.transform.localPosition = pos;
                UnitDummy component = gameObject.GetComponent<UnitDummy>();
                component.UpdateInitPos();

                string text = unitPrefabName;
                if (text == string.Empty)
                {
                    text = UnityGameDatas.GetInstance().ReadUnitPrefabPathByType(unitType);
                }
                if (text == string.Empty)
                {
                    Debug.Log("CloneUnitDummy: Unit prefab name is nill --- unitType = " + unitType);
                }
                CoreUtils.assetService.Instantiate(text, (GameObject gameObject2) =>
                {
                    if (gameObject2 == null)
                    {
                        Debug.Log("CloneUnitDummy: Unit obj is nill --- unitType = " + unitType);
                    }
                    UnitBase component2 = gameObject2.GetComponent<UnitBase>();
                    SpriteRenderer component3 = gameObject2.GetComponent<SpriteRenderer>();
                    if (this.m_formation.GetFormationType() == Formation.ENMU_SQUARE_TYPE.BARBARIAN)
                    {
                        if (this.m_formation.GetUnitColorDict().ContainsKey(unitPrefabName))
                        {
                            component3.color = this.m_formation.GetUnitColorDict()[unitPrefabName];
                        }
                        else
                        {
                            component3.color = this.m_unit_color;
                        }
                    }
                    else
                    {
                        component3.color = this.m_unit_color;
                    }
                    component.m_unit = component2;
                    component2.InitDummy(component);
                    component2.SetSpriteLoigicalState(this.GetUnitSpriteState(this.m_formation.m_formation_state));
                    component2.ChangeMoveState(UnitBase.MOVE_STATE.STATIC, false, 0f, 1f);
                    component2.unitType = unitType;
                    gameObject.GetComponent<TextureSheetAnimation>().SetCurLodForParticle(currentLodLevel);
                    component2.FadeIn();
                    action?.Invoke(component);
                });
            });
        }

        public void UpdateUnitPositionBarabarian()
        {
            int currentLodLevel = this.m_formation.GetCurrentLodLevel();
            BarbarianFormationConfig component = base.GetComponent<BarbarianFormationConfig>();
            for (int i = 0; i < component.m_UnitDummys.Length; i++)
            {
                string unitPrefab = component.m_UnitDummys[i].unitPrefab;
                if (unitPrefab != string.Empty)
                {
                    UnitDummy component2 = component.m_UnitDummys[i].unitDummy.GetComponent<UnitDummy>();
                    CoreUtils.assetService.Instantiate(unitPrefab, (GameObject gameObject) =>
                    {
                        gameObject.transform.SetParent(component2.transform, false);
                        gameObject.transform.localPosition = Vector3.zero;
                        UnitBase component3 = gameObject.GetComponent<UnitBase>();
                        component3.InitDummy(component2);
                        component2.ResumeInitPos();
                        component2.UpdateInitPos();
                        this.m_dummy_array.Add(component2);
                        SpriteRenderer component4 = gameObject.GetComponent<SpriteRenderer>();
                        if (component4 != null)
                        {
                            string key = component3.name.Replace("(Clone)", string.Empty);
                            if (this.m_formation.GetUnitColorDict().ContainsKey(key))
                            {
                                component4.color = this.m_formation.GetUnitColorDict()[key];
                            }
                            else
                            {
                                component4.color = this.m_unit_color;
                            }
                        }
                        component2.m_unit = component3;
                        component3.SetSpriteLoigicalState(this.GetUnitSpriteState(this.m_formation.m_formation_state));
                        component3.ChangeMoveState(UnitBase.MOVE_STATE.STATIC, false, 0f, 1f);
                        component3.unitType = component.m_UnitDummys[i].unitType;
                        component3.cloneToPrefab = component.m_UnitDummys[i].cloneToPrefab;
                        if (gameObject.GetComponent<TextureSheetAnimation>() != null)
                        {
                            gameObject.GetComponent<TextureSheetAnimation>().SetCurLodForParticle(currentLodLevel);
                            gameObject.GetComponent<TextureSheetAnimation>().FormationState = this.m_formation.m_formation_state;
                        }
                        component3.FadeIn();
                    });
                }
            }
            if (currentLodLevel <= 1)
            {
                if (this.m_square_row_info.rowCategory == 0)
                {
                    return;
                }
                List<UnitDummy> list = new List<UnitDummy>();
                for (int j = 0; j < this.m_dummy_array.Count; j++)
                {
                    UnitDummy unitDummy = this.m_dummy_array[j];
                    Vector3 orgPos = unitDummy.OrgPos;
                    Vector3 orgPos2 = unitDummy.OrgPos;
                    Vector3 orgPos3 = unitDummy.OrgPos;
                    if (j % 2 == 0)
                    {
                        orgPos.z += this.m_expansivity;
                        orgPos2.x -= this.m_expansivity * 1.414f;
                        orgPos2.z -= this.m_expansivity * 1.414f;
                        orgPos3.x += this.m_expansivity * 1.414f;
                        orgPos3.z -= this.m_expansivity * 1.414f;
                    }
                    else
                    {
                        orgPos.z -= this.m_expansivity;
                        orgPos2.x -= this.m_expansivity * 1.414f;
                        orgPos2.z += this.m_expansivity * 1.414f;
                        orgPos3.x += this.m_expansivity * 1.414f;
                        orgPos3.z += this.m_expansivity * 1.414f;
                    }
                    unitDummy.transform.localPosition = orgPos;
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
                    CloneUnitDummy(orgPos2, unitDummy.m_unit.unitType, unitDummy.gameObject.name, unitPrefabName, (UnitDummy dummy) =>
                    {
                        this.m_dummy_array.AddRange(list);
                    });
                    CloneUnitDummy(orgPos3, unitDummy.m_unit.unitType, unitDummy.gameObject.name, unitPrefabName, (UnitDummy dummy) =>
                    {
                        this.m_dummy_array.AddRange(list);
                    });
                }
            }
        }

        public void UpdateUnitPosition()
        {
            int currentLodLevel = this.m_formation.GetCurrentLodLevel();
            if (this.m_square_row_info == null)
            {
                return;
            }
            List<List<int>> displayUnitMap = this.m_square_row_info.GetDisplayUnitMap();
            int count = displayUnitMap.Count;
            for (int i = 0; i < count; i++)
            {
                if (currentLodLevel != 2)
                {
                    List<int> collection = new List<int>(displayUnitMap[i]);
                    for (int j = 1; j < UnityGameDatas.GetInstance().ReadUnitLodMultiplier(this.m_square_row_info.rowCategory, currentLodLevel); j++)
                    {
                        displayUnitMap[i].AddRange(collection);
                    }
                    for (int k = 1; k <= UnityGameDatas.GetInstance().ReadUnitLodAddend(this.m_square_row_info.rowCategory, currentLodLevel); k++)
                    {
                        displayUnitMap[i].Add(displayUnitMap[i][0]);
                    }
                }
                List<List<int>> list = new List<List<int>>();
                int num = 3 - currentLodLevel;
                int count2 = displayUnitMap[i].Count;
                int num2 = 0;
                int num3 = count2 / num;
                if (this.m_square_row_info.rowCategory == 0)
                {
                    list.Add(displayUnitMap[i].GetRange(0, displayUnitMap[i].Count));
                }
                else
                {
                    for (int l = 0; l < num; l++)
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
                for (int m = 0; m < list.Count; m++)
                {
                    int count3 = list[m].Count;
                    for (int n = 0; n < list[m].Count; n++)
                    {
                        CoreUtils.assetService.Instantiate(m_unit_dummy_prefab, (GameObject gameObject) =>
                        {
                            gameObject.transform.SetParent(base.transform, false);
                            Vector3 localPosition = Vector3.zero;
                            if (this.m_posProvider == null || this.m_posProvider.GetSquareType() != Formation.ENMU_SQUARE_TYPE.BARBARIAN)
                            {
                                localPosition = this.ProvidePosition(i, m, num, n, count3, this.m_square_row_info.rowCategory);
                                gameObject.transform.localPosition = localPosition;
                                if (this.m_square_row_info.rowCategory == 0 && displayUnitMap[i][n] > 10000 && displayUnitMap[i][n] < 20000)
                                {
                                    int unitPriority = this.m_square_row_info.GetUnitPriority(displayUnitMap[i][n]);
                                    int type = 1;
                                    if (unitPriority == 2)
                                    {
                                        type = 3;
                                    }
                                    float num4 = UnityGameDatas.GetInstance().ReadHeroXOffset(type, this.m_formation.GetFormationType());
                                    float num5 = UnityGameDatas.GetInstance().ReadHeroZOffset(type, this.m_formation.GetFormationType());
                                    Vector3 localPosition2 = gameObject.transform.localPosition;
                                    localPosition2.x += num4;
                                    localPosition2.z += num5;
                                    gameObject.transform.localPosition = localPosition2;
                                    if (unitPriority == 1)
                                    {
                                        CoreUtils.assetService.Instantiate(m_unit_dummy_prefab, (GameObject gameObject2) =>
                                        {
                                            gameObject2.transform.SetParent(base.transform, false);
                                            num4 = UnityGameDatas.GetInstance().ReadHeroXOffset(2, this.m_formation.GetFormationType());
                                            num5 = UnityGameDatas.GetInstance().ReadHeroZOffset(2, this.m_formation.GetFormationType());
                                            gameObject2.transform.localPosition = new Vector3(localPosition.x + num4, 0f, localPosition.z + num5);
                                            UnitDummy component = gameObject2.GetComponent<UnitDummy>();
                                            component.UpdateInitPos();
                                            this.m_dummy_array.Add(component);
                                            CoreUtils.assetService.Instantiate("flagman", (GameObject gameObject3) =>
                                            {
                                                UnitBase component2 = gameObject3.GetComponent<UnitBase>();
                                                SpriteRenderer component3 = gameObject3.GetComponent<SpriteRenderer>();
                                                component3.color = this.m_unit_color;
                                                component.m_unit = component2;
                                                component2.InitDummy(component);
                                                component2.SetSpriteLoigicalState(this.GetUnitSpriteState(this.m_formation.m_formation_state));
                                                component2.ChangeMoveState(UnitBase.MOVE_STATE.STATIC, false, 0f, 1f);
                                                component2.unitType = 9999;
                                                gameObject3.GetComponent<TextureSheetAnimation>().SetCurLodForParticle(currentLodLevel);
                                                component2.FadeIn();
                                            });
                                        });
                                    }
                                }
                            }
                            UnitDummy component4 = gameObject.GetComponent<UnitDummy>();
                            component4.UpdateInitPos();
                            this.m_dummy_array.Add(component4);
                            CoreUtils.assetService.Instantiate(UnityGameDatas.GetInstance().ReadUnitPrefabPathByType(displayUnitMap[i][n]), (GameObject gameObject2) =>
                            {
                                UnitBase component5 = gameObject2.GetComponent<UnitBase>();
                                gameObject2.GetComponent<TextureSheetAnimation>().FormationState = this.m_formation.m_formation_state;
                                SpriteRenderer component6 = gameObject2.GetComponent<SpriteRenderer>();
                                component6.color = this.m_unit_color;
                                component4.m_unit = component5;
                                component5.InitDummy(component4);
                                component5.SetSpriteLoigicalState(this.GetUnitSpriteState(this.m_formation.m_formation_state));
                                component5.ChangeMoveState(UnitBase.MOVE_STATE.STATIC, false, 0f, 1f);
                                component5.unitType = displayUnitMap[i][n];
                                gameObject2.GetComponent<TextureSheetAnimation>().SetCurLodForParticle(currentLodLevel);
                                component5.FadeIn();
                            });
                        });
                    }
                }
            }
        }

        private void DoUpdateLod()
        {
            this.ResetUnit();
        }

        public void ForceUpdateLod()
        {
            this.TransformAtkFormation(false, 0f, 0f, true);
            this.ResetUnit();
        }

        private void ResetUnit()
        {
            this.m_sub_row_zoffset.Clear();
            for (int i = 0; i < this.m_dummy_array.Count; i++)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (unit)
                {
                    CoreUtils.assetService.Destroy(unit.gameObject);
                }
                if (this.m_formation.GetFormationType() != Formation.ENMU_SQUARE_TYPE.BARBARIAN)
                {
                    CoreUtils.assetService.Destroy(this.m_dummy_array[i].gameObject);
                }
            }
            this.m_dummy_array.Clear();
            if (this.m_formation.GetFormationType() == Formation.ENMU_SQUARE_TYPE.BARBARIAN)
            {
                this.UpdateUnitPositionBarabarian();
            }
            else
            {
                this.UpdateUnitPosition();
            }
        }

        private float GetUnitHorizontalDistance(int unit_number)
        {
            return UnityGameDatas.GetInstance().ReadUnitRowWidthByCategory(this.m_square_row_info.rowCategory, this.m_formation.GetFormationType()) / (float)unit_number;
        }

        private float GetUnitVerticalDistance(int row_number)
        {
            return this.m_row_height / (float)row_number;
        }

        private void SetUnitLogicalState(Formation.ENMU_SQUARE_STAT formation_state)
        {
            TextureSheetAnimation.ENMU_SPRITE_STATE unitSpriteState = this.GetUnitSpriteState(formation_state);
            for (int i = 0; i < this.m_dummy_array.Count; i++)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (unit)
                {
                    unit.SetSpriteLoigicalState(unitSpriteState);
                    if (formation_state == Formation.ENMU_SQUARE_STAT.DEAD)
                    {
                        unit.PlayDeadParticle();
                    }
                    TextureSheetAnimationHero component = unit.GetComponent<TextureSheetAnimationHero>();
                    if (component)
                    {
                        component.FormationState = formation_state;
                    }
                }
            }
            if (formation_state == Formation.ENMU_SQUARE_STAT.DEAD)
            {
                this.DestroyUnit();
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

        public void ChangeUnitMoveState(UnitBase.MOVE_STATE state, bool isMoveAtk = false)
        {
            for (int i = 0; i < this.m_dummy_array.Count; i++)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (unit)
                {
                    if (isMoveAtk)
                    {
                        unit.SetChaseMode(UnitBase.CHASE_MODE.ROTATE_SPRITE);
                    }
                    else
                    {
                        unit.SetChaseMode(UnitBase.CHASE_MODE.ORIGIN_SPRITE);
                    }
                    unit.ChangeMoveState(state, isMoveAtk, 0f, 1f);
                }
            }
        }

        private void ChangeUnitChaseSpeedForce(float chase_speed)
        {
            for (int i = 0; i < this.m_dummy_array.Count; i++)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (unit)
                {
                    unit.SetMoveSpeedForce(chase_speed);
                }
            }
        }

        private void ChangeUnitChaseSpeed(float chase_speed)
        {
            for (int i = 0; i < this.m_dummy_array.Count; i++)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (unit)
                {
                    unit.SetMoveSpeed(chase_speed);
                }
            }
        }

        private void DestroyUnit()
        {
            for (int i = this.m_dummy_array.Count - 1; i >= 0; i--)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (unit)
                {
                    CoreUtils.assetService.Destroy(unit.gameObject);
                }
                if (this.m_formation.GetFormationType() != Formation.ENMU_SQUARE_TYPE.BARBARIAN)
                {
                    CoreUtils.assetService.Destroy(this.m_dummy_array[i].gameObject);
                }
            }
            this.m_dummy_array.Clear();
        }

        public void HeroSkillAni(int heroId)
        {
            if (this.squareRowInfo.rowCategory == 0)
            {
                for (int i = 0; i < this.m_dummy_array.Count; i++)
                {
                    UnitBase unit = this.m_dummy_array[i].m_unit;
                    if (unit.unitType == heroId)
                    {
                        TextureSheetAnimationHero component = unit.GetComponent<TextureSheetAnimationHero>();
                        if (component != null)
                        {
                            component.StartSkillAni();
                        }
                    }
                }
            }
        }

        public void InitRow(string row_info_str, int row_number, int row_category, Color unit_color, Formation formation)
        {
            this.m_square_row_info = new SquareRowInfo(row_info_str, row_number, row_category, formation.GetFormationType());
            this.m_unit_color = unit_color;
            this.m_formation = formation;
            this.m_row_height = UnityGameDatas.GetInstance().ReadUnitRowForwardSpacingByCategory(this.m_square_row_info.rowCategory, formation.GetFormationType()) + UnityGameDatas.GetInstance().ReadUnitRowBackwardSpacingByCategory(this.m_square_row_info.rowCategory, formation.GetFormationType());
        }

        public void InitRowBarbarian(string row_info_str, int row_number, int row_category, Color unit_color, Formation formation)
        {
            this.m_square_row_info = new SquareRowInfo(row_info_str, row_number, row_category, formation.GetFormationType());
            this.m_unit_color = unit_color;
            this.m_formation = formation;
        }

        public void SetState(Formation.ENMU_SQUARE_STAT state, Vector2 current_pos, Vector2 target_pos, float move_speed = 2f, bool isMoveAtk = false)
        {
            Vector2 zero = Vector2.zero;
            bool flag = false;
            this.SetUnitLogicalState(state);
            switch (state)
            {
                case Formation.ENMU_SQUARE_STAT.IDLE:
                    this.ChangeUnitChaseSpeed(this.m_move_speed * this.m_chase_speed_factor);
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                    break;
                case Formation.ENMU_SQUARE_STAT.MOVE:
                    if (move_speed == 0f)
                    {
                        Debug.LogWarning("Square : invalid move_speed");
                        move_speed = 1f;
                    }
                    this.m_move_speed = move_speed;
                    this.ChangeUnitChaseSpeed(this.m_move_speed * this.m_chase_speed_factor);
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                    break;
                case Formation.ENMU_SQUARE_STAT.FIGHT:
                    if (isMoveAtk || this.m_lastMoveAtk != isMoveAtk)
                    {
                        this.m_lastMoveAtk = isMoveAtk;
                        if (move_speed != 0f)
                        {
                            this.m_move_speed = move_speed;
                        }
                        this.ChangeUnitChaseSpeedForce(this.m_move_speed);
                        this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, isMoveAtk);
                    }
                    else if (!this.m_lastMoveAtk && !isMoveAtk && this.m_curState != state)
                    {
                        flag = false;
                        this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, isMoveAtk);
                    }
                    else
                    {
                        flag = true;
                    }
                    break;
                case Formation.ENMU_SQUARE_STAT.DEAD:
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.UNBOUND, false);
                    break;
                case Formation.ENMU_SQUARE_STAT.SET_AROUND:
                    this.ChangeUnitChaseSpeed(this.m_move_speed * this.m_set_around_speed_factor);
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                    break;
            }
            if (flag)
            {
                this.ChangeUnitMoveState(UnitBase.MOVE_STATE.NUMBER, false);
            }
            this.m_current_pos = current_pos;
            this.m_target_pos = target_pos;
            this.m_curState = state;
        }

        private void Update()
        {
        }

        public void DestroyManual()
        {
            this.OnDespawn();
        }

        private void OnDespawn()
        {
            this.DestroyUnit();
            base.enabled = false;
            this.m_hasAtkFormation = false;
            this.m_lastMoveAtk = false;
            this.m_dummy_array_byrow = null;
            this.ResetAtkFormationParam();
            this.m_isFirstRow = false;
            this.m_isLastRow = false;
        }

        public Vector3 GetShowPosition()
        {
            if (this.m_dummy_array.Count == 0)
            {
                return Vector3.zero;
            }
            return this.m_dummy_array[0].m_unit.transform.position;
        }

        public void SetEffectOnUnit(string effectName)
        {
            if (this.m_dummy_array.Count > 0)
            {
                this.m_dummy_array[0].m_unit.SetLevelupEffect(effectName);
            }
        }

        public void FadeIn()
        {
            for (int i = 0; i < this.m_dummy_array.Count; i++)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (unit)
                {
                    unit.FadeIn();
                }
            }
        }

        public void FadeOut()
        {
            for (int i = 0; i < this.m_dummy_array.Count; i++)
            {
                UnitBase unit = this.m_dummy_array[i].m_unit;
                if (unit)
                {
                    unit.FadeOut();
                }
            }
        }

        public bool CheckSquareRowIsEmpty()
        {
            return this.m_dummy_array.Count == 0;
        }

        public float GetInnerFirstRowZ()
        {
            if (this.m_square_row_info.rowCategory == 0)
            {
                float num = -999f;
                for (int i = 0; i < this.m_dummy_array.Count; i++)
                {
                    UnitDummy unitDummy = this.m_dummy_array[i];
                    if (num < unitDummy.transform.localPosition.z)
                    {
                        num = unitDummy.transform.localPosition.z;
                    }
                }
                return num + base.gameObject.transform.localPosition.z;
            }
            Dictionary<int, List<UnitDummy>> dummyByInnerRow = this.GetDummyByInnerRow();
            if (dummyByInnerRow.Values.Count == 0 || !dummyByInnerRow.ContainsKey(0))
            {
                return 0f;
            }
            List<UnitDummy> list = dummyByInnerRow[0];
            if (list.Count == 0)
            {
                return 0f;
            }
            UnitDummy unitDummy2 = list[0];
            return unitDummy2.gameObject.transform.localPosition.z + base.gameObject.transform.localPosition.z;
        }

        public void SetIsFirstRow(bool isFirst)
        {
            this.m_isFirstRow = isFirst;
        }

        public void SetIsLastRow(bool isLastRow)
        {
            this.m_isLastRow = isLastRow;
        }

        public void ResetAtkFormationParam()
        {
            this.m_hasAtkFormation = false;
            this.m_atkOffset = 0f;
            this.m_atkOffsetStep = 0f;
        }

        private int GetSubRowNum(UnitDummy ud)
        {
            for (int i = 0; i < this.m_sub_row_zoffset.Count; i++)
            {
                float z;
                if (ud.InitedOrgPos)
                {
                    z = ud.OrgPos.z;
                }
                else
                {
                    z = ud.transform.localPosition.z;
                }
                if (Mathf.Abs(this.m_sub_row_zoffset[i] - z) <= 0.05f)
                {
                    return i;
                }
            }
            return 0;
        }

        private Dictionary<int, List<UnitDummy>> GetDummyByInnerRow()
        {
            if (this.m_dummy_array_byrow == null)
            {
                Dictionary<int, List<UnitDummy>> dictionary = new Dictionary<int, List<UnitDummy>>();
                for (int i = 0; i < this.m_dummy_array.Count; i++)
                {
                    UnitDummy unitDummy = this.m_dummy_array[i];
                    int subRowNum = this.GetSubRowNum(unitDummy);
                    if (!dictionary.ContainsKey(subRowNum))
                    {
                        dictionary.Add(subRowNum, new List<UnitDummy>());
                    }
                    dictionary[subRowNum].Add(unitDummy);
                }
                this.m_dummy_array_byrow = dictionary;
            }
            return this.m_dummy_array_byrow;
        }

        public int GetInnerRowNum()
        {
            Dictionary<int, List<UnitDummy>> dummyByInnerRow = this.GetDummyByInnerRow();
            return dummyByInnerRow.Count;
        }

        public void TransformAtkFormation(bool isAtk, float zOffset, float stepLen, bool immediate = false)
        {
            if (isAtk && !this.m_hasAtkFormation)
            {
                this.m_hasAtkFormation = true;
                this.m_atkOffset = zOffset;
                this.m_atkOffsetStep = stepLen;
                if (!immediate)
                {
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                    this.ChangeUnitChaseSpeed(1.2f);
                }
                Dictionary<int, List<UnitDummy>> dummyByInnerRow = this.GetDummyByInnerRow();
                int count = dummyByInnerRow.Count;
                int num = 0;
                for (int i = 0; i < dummyByInnerRow.Count; i++)
                {
                    if (!dummyByInnerRow.ContainsKey(i))
                    {
                        Debug.LogWarning("atk: dummy_array_byrow not contains key ---- " + i);
                    }
                    else
                    {
                        List<UnitDummy> list = dummyByInnerRow[i];
                        num += list.Count;
                        int num2 = Mathf.FloorToInt((float)list.Count * 0.5f + 0.5f);
                        float num3 = this.m_atkOffset - this.m_atkOffsetStep * (float)i;
                        for (int j = 0; j < list.Count; j++)
                        {
                            Vector3 localPosition = list[j].transform.localPosition;
                            localPosition.z += num3;
                            if (this.m_isFirstRow && i == 0)
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
            if (!isAtk && this.m_hasAtkFormation)
            {
                this.m_hasAtkFormation = false;
                if (!immediate)
                {
                    this.ChangeUnitMoveState(UnitBase.MOVE_STATE.CHASE, false);
                    this.ChangeUnitChaseSpeed(1f);
                }
                if (this.m_dummy_array_byrow == null)
                {
                    this.m_dummy_array_byrow = this.GetDummyByInnerRow();
                }
                int count2 = this.m_dummy_array_byrow.Count;
                int num4 = 0;
                for (int k = 0; k < this.m_dummy_array_byrow.Count; k++)
                {
                    if (!this.m_dummy_array_byrow.ContainsKey(k))
                    {
                        Debug.LogWarning("Normal: dummy_array_byrow not contains key ---- " + k);
                    }
                    else
                    {
                        List<UnitDummy> list2 = this.m_dummy_array_byrow[k];
                        num4 += list2.Count;
                        int num5 = Mathf.FloorToInt((float)list2.Count * 0.5f + 0.5f);
                        float num6 = this.m_atkOffset - this.m_atkOffsetStep * (float)k;
                        for (int l = 0; l < list2.Count; l++)
                        {
                            if (!(list2[l] == null))
                            {
                                Vector3 localPosition2 = list2[l].transform.localPosition;
                                localPosition2.z -= num6;
                                if (this.m_isFirstRow && k == 0)
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
                }
                this.m_dummy_array_byrow = null;
            }
        }
    }
}
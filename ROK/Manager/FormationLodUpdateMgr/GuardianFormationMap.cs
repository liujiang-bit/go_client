using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    [ExecuteInEditMode]
    public class GuardianFormationMap : MonoBehaviour
    {
        [Serializable]
        public class UnitDummyConfig
        {
            public string unitPrefab = string.Empty;

            public int unitType;
        }

        public GuardianFormationMap.UnitDummyConfig[] m_dummysConfig;

        public GameObject[] m_dummys;

        public List<GameObject> m_units;

        public GameObject m_unitNoTexDummy;

        public GameObject m_unitNoTex;

        public GameObject m_unitHeroDummy;

        public GameObject m_unitHero;

        public List<GameObject> m_lodDummys = new List<GameObject>();

        public List<GameObject> m_lodUnits = new List<GameObject>();

        private Color m_unitColor = Color.red;

        private FormationGuardian m_formation;

        public bool m_isDebug;

        public List<GameObject> m_cubes = new List<GameObject>();

        public void SetFormation(FormationGuardian formation)
        {
            this.m_formation = formation;
        }

        public void SetUnitColor(Color color)
        {
            this.m_unitColor = color;
        }

        public void LoadUnit()
        {
            if (this.m_units == null)
            {
                this.m_units = new List<GameObject>();
            }
            this.m_units.Clear();
            for (int i = 0; i < this.m_dummys.Length; i++)
            {
                UnitDummy component = this.m_dummys[i].GetComponent<UnitDummy>();
                CoreUtils.assetService.Instantiate(this.m_dummysConfig[i].unitPrefab, (GameObject gameObject) =>
                {
                    gameObject.transform.SetParent(component.transform, false);
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.name = this.m_dummysConfig[i].unitPrefab;
                    UnitBase component2 = gameObject.GetComponent<UnitBase>();
                    component2.SetChaseMode(UnitBase.CHASE_MODE.ORIGIN_SPRITE);
                    component.m_unit = component2;
                    component2.InitDummy(component);
                    component.ResumeInitPos();
                    component.UpdateInitPos();
                    this.m_units.Add(gameObject);
                    SpriteRenderer component3 = gameObject.GetComponent<SpriteRenderer>();
                    if (component3 != null)
                    {
                        string name = component2.name;
                        if (this.m_formation.GetUnitColorDict().ContainsKey(name))
                        {
                            component3.color = this.m_formation.GetUnitColorDict()[name];
                        }
                        else
                        {
                            component3.color = this.m_unitColor;
                        }
                    }
                    component2.FadeIn();
                });
            }
            UnitDummy component4 = this.m_unitNoTexDummy.GetComponent<UnitDummy>();
            this.m_unitNoTex.GetComponent<UnitBase>().InitDummy(component4);
            component4.ResumeInitPos();
            component4.UpdateInitPos();
            component4.m_unit = this.m_unitNoTex.GetComponent<UnitBase>();
        }

        public void SetHeroUnit(List<int> heros)
        {
            if (heros == null || heros.Count == 0 || this.m_unitHero != null)
            {
                return;
            }
            string text = UnityGameDatas.GetInstance().ReadUnitPrefabPathByType(heros[0]);
            CoreUtils.assetService.Instantiate("text", (GameObject gameObject) =>
            {
                this.m_unitHero = gameObject;
                this.m_unitHero.name = text;
                UnitBase component = this.m_unitHero.GetComponent<UnitBase>();
                UnitDummy component2 = this.m_unitHeroDummy.GetComponent<UnitDummy>();
                component.InitDummy(component2);
                component2.m_unit = component;
                component2.ResumeInitPos();
                component2.UpdateInitPos();
                SpriteRenderer component3 = this.m_unitHero.GetComponent<SpriteRenderer>();
                if (component3 != null)
                {
                    string name = component.name;
                    if (this.m_formation.GetUnitColorDict().ContainsKey(name))
                    {
                        component3.color = this.m_formation.GetUnitColorDict()[name];
                    }
                    else
                    {
                        component3.color = this.m_unitColor;
                    }
                }
            });
            this.m_unitNoTex.GetComponent<UnitNoTex>().SetBindGameObject(this.m_unitHeroDummy);
        }

        private void AddLodUnit(int lodLv, Formation.ENMU_SQUARE_STAT fst, TextureSheetAnimation.ENMU_SPRITE_STATE sst)
        {
            Vector3 position = base.transform.position;
            for (int i = 0; i < this.m_dummysConfig.Length; i++)
            {
                GuardianFormationMap.UnitDummyConfig unitDummyConfig = this.m_dummysConfig[i];
                UnitDummy component = this.m_dummys[i].GetComponent<UnitDummy>();
                CoreUtils.assetService.Instantiate("unit_dummy", (GameObject gameObject) =>
                {
                    this.m_lodDummys.Add(gameObject);
                    gameObject.transform.SetParent(base.transform, false);
                    gameObject.transform.localPosition = Vector3.MoveTowards(component.transform.localPosition, Vector3.zero, 0.7f);
                    UnitDummy component2 = gameObject.GetComponent<UnitDummy>();
                    component2.ResumeInitPos();
                    component2.UpdateInitPos();
                    CoreUtils.assetService.Instantiate(unitDummyConfig.unitPrefab, (GameObject gameObject2) =>
                    {
                        gameObject2.name = unitDummyConfig.unitPrefab;
                        this.m_lodUnits.Add(gameObject2);
                        UnitBase component3 = gameObject2.GetComponent<UnitBase>();
                        SpriteRenderer component4 = gameObject2.GetComponent<SpriteRenderer>();
                        if (component4 != null)
                        {
                            string name = component3.name;
                            if (this.m_formation.GetUnitColorDict().ContainsKey(name))
                            {
                                component4.color = this.m_formation.GetUnitColorDict()[name];
                            }
                            else
                            {
                                component4.color = this.m_unitColor;
                            }
                        }
                        component2.m_unit = component3;
                        component3.InitDummy(component2);
                        component3.SetSpriteLoigicalState(sst);
                        component3.ChangeMoveState(UnitBase.MOVE_STATE.STATIC, false, 0f, 1f);
                        TextureSheetAnimation component5 = gameObject2.GetComponent<TextureSheetAnimation>();
                        if (component5 != null)
                        {
                            component5.SetCurLodForParticle(lodLv);
                        }
                        component3.FadeIn();
                    });
                });
            }
        }

        private void RemoveLodUnit()
        {
            foreach (GameObject current in this.m_lodUnits)
            {
                if (current != null)
                {
                    CoreUtils.assetService.Destroy(current);
                }
            }
            this.m_lodUnits.Clear();
            foreach (GameObject current2 in this.m_lodDummys)
            {
                if (current2 != null)
                {
                    CoreUtils.assetService.Destroy(current2);
                }
            }
            this.m_lodDummys.Clear();
        }

        public void UpdateLod(int curLodLevel, Formation.ENMU_SQUARE_STAT fst, TextureSheetAnimation.ENMU_SPRITE_STATE sst)
        {
            if (curLodLevel >= 2)
            {
                this.RemoveLodUnit();
            }
            else
            {
                this.AddLodUnit(curLodLevel, fst, sst);
            }
        }

        private int GetClosestIdx(List<int> idxs, Vector3 pos)
        {
            if (idxs.Count == 0)
            {
                return -1;
            }
            float num = -1f;
            int num2 = -1;
            foreach (int current in idxs)
            {
                Vector3 position = this.m_dummys[current].transform.position;
                float magnitude = (position - pos).magnitude;
                if (num2 == -1 || magnitude < num)
                {
                    num = magnitude;
                    num2 = current;
                }
            }
            return num2;
        }

        public void Remap()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.m_dummys.Length; i++)
            {
                this.m_dummys[i].GetComponent<UnitDummy>().m_unit = null;
                list.Add(i);
            }
            if (list.Count == 0)
            {
                return;
            }
            List<GameObject> list2 = new List<GameObject>();
            list2.AddRange(this.m_units);
            for (int j = 0; j < this.m_units.Count; j++)
            {
                GameObject gameObject = this.m_units[j];
                int closestIdx = this.GetClosestIdx(list, gameObject.transform.position);
                UnitDummy component = this.m_dummys[closestIdx].GetComponent<UnitDummy>();
                UnitBase component2 = gameObject.GetComponent<UnitBase>();
                component.m_unit = component2;
                component2.SetDummy(component);
                list.Remove(closestIdx);
                if (base.gameObject.name.StartsWith("square_mine"))
                {
                    Debug.Log(string.Concat(new object[]
                    {
                    "remap: j = ",
                    j,
                    ", minidx = ",
                    closestIdx
                    }));
                }
            }
        }

        public int UnitTotalNumByCateoory(int category)
        {
            int num = 0;
            for (int i = 0; i < this.m_dummysConfig.Length; i++)
            {
                if (this.m_dummysConfig[i].unitType != 0)
                {
                    if ((this.m_dummysConfig[i].unitType - 1) % 4 + 1 == category)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int UnitNumByCategory(int category)
        {
            int num = 0;
            for (int i = 0; i < this.m_dummysConfig.Length; i++)
            {
                if (this.m_dummysConfig[i].unitType != 0)
                {
                    if ((this.m_dummysConfig[i].unitType - 1) % 4 + 1 == category && this.m_dummys[i].GetComponent<UnitDummy>().m_unit != null)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int DieUnit(int category, int num)
        {
            int num2 = num;
            int num3 = 0;
            while (num3 < this.m_dummysConfig.Length && num2 > 0)
            {
                if ((this.m_dummysConfig[num3].unitType - 1) % 4 + 1 == category)
                {
                    UnitDummy component = this.m_dummys[num3].GetComponent<UnitDummy>();
                    UnitBase unit = component.m_unit;
                    if (unit != null)
                    {
                        CoreUtils.assetService.Destroy(unit.gameObject);
                        unit.PlayDeadParticle();
                        component.m_unit = null;
                        num2--;
                        this.m_units[num3] = null;
                    }
                }
                num3++;
            }
            return num - num2;
        }

        public void Reset()
        {
            if (this.m_unitNoTex)
            {
                this.m_unitNoTex.transform.SetParent(this.m_unitNoTexDummy.transform, false);
                this.m_unitNoTex.transform.localPosition = Vector3.zero;
                this.m_unitNoTex.transform.localEulerAngles = Vector3.zero;
                UnitNoTex component = this.m_unitNoTex.GetComponent<UnitNoTex>();
                if (component)
                {
                    component.ClearSkillEffect();
                }
            }
        }

        private void Update()
        {
            try
            {
                this.UpdateDebug();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void UpdateDebug()
        {
            if (this.m_isDebug && this.m_cubes.Count == 0)
            {
                for (int i = 0; i < this.m_dummys.Length; i++)
                {
                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    gameObject.transform.SetParent(this.m_dummys[i].transform, false);
                    gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    gameObject.transform.localPosition = Vector3.zero;
                    this.m_cubes.Add(gameObject);
                }
            }
            else if (!this.m_isDebug && this.m_cubes.Count > 0)
            {
                for (int j = 0; j < this.m_cubes.Count; j++)
                {
                    UnityEngine.Object.DestroyImmediate(this.m_cubes[j]);
                }
                this.m_cubes.Clear();
            }
        }
    }
}
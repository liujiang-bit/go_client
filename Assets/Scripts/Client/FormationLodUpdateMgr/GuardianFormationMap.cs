using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// 地图上布阵  挂载troop/BarbarianSlayerBoss
    /// </summary>
    [ExecuteInEditMode]
    public class GuardianFormationMap : MonoBehaviour
    {
        [Serializable]
        public class UnitDummyConfig
        {
            public string unitPrefab = string.Empty;

            public int unitType;
        }

        public UnitDummyConfig[] m_dummysConfig;

        public GameObject[] m_dummys;

        public List<GameObject> m_units;

        public GameObject m_unitNoTexDummy;

        public GameObject m_unitNoTex;

        public GameObject m_unitHeroDummy;

        public GameObject m_unitHero;

        public List<GameObject> m_lodDummys = new List<GameObject>();

        public List<GameObject> m_lodUnits = new List<GameObject>();

        private Color m_unitColor = Color.red;

        public bool m_isDebug;

        public List<GameObject> m_cubes = new List<GameObject>();

        private int m_loadFrame;

        public void SetUnitColor(Color color)
        {
            m_unitColor = color;
        }

        public void LoadUnit()
        {
            if (m_units == null)
            {
                m_units = new List<GameObject>();
            }
            m_units.Clear();
            m_loadFrame = Time.frameCount;
            int frameCount = m_loadFrame;
            int nCount = m_dummys.Length;
            for (int i = 0; i < m_dummys.Length; i++)
            {
                CellClone component = m_dummys[i].GetComponent<CellClone>();
                CoreUtils.assetService.Instantiate(m_dummysConfig[i].unitPrefab, (gameObject) =>
                {
                    if(m_loadFrame != frameCount)
                    {
                        CoreUtils.assetService.Destroy(gameObject);
                        return;
                    }
                    gameObject.transform.SetParent(component.transform, worldPositionStays: false);
                    gameObject.transform.localPosition = Vector3.zero;
                    CellBase component2 = gameObject.GetComponent<CellBase>();
                    component2.SetChaseMode(CellBase.CHASE_MODE.ORIGIN_SPRITE);
                    component.m_unit = component2;
                    component2.InitDummy(component);
                    component.ResumeInitPos();
                    component.UpdateInitPos();
                    m_units.Add(gameObject);
                    SpriteRenderer component3 = gameObject.GetComponent<SpriteRenderer>();
                    if (component3 != null)
                    {
                        component3.color = m_unitColor;
                    }
                    component2.FadeIn();
                    nCount--;
                    if(nCount == 0)
                    {
                        CellClone component4 = m_unitNoTexDummy.GetComponent<CellClone>();
                        m_unitNoTex.GetComponent<CellBase>().InitDummy(component4);
                        component4.ResumeInitPos();
                        component4.UpdateInitPos();
                        component4.m_unit = m_unitNoTex.GetComponent<CellBase>();
                    }
                });
            }
        }

        public void SetHeroUnit(List<int> heros)
        {
            if (heros != null && heros.Count != 0 && !(m_unitHero != null))
            {
                int frameCount = m_loadFrame;
                string path = CellDatas.GetInstance().ReadUnitPrefabPathByType(heros[0]);
                CoreUtils.assetService.Instantiate(path, (gameObject)=>
                {
                    if (m_loadFrame != frameCount)
                    {
                        CoreUtils.assetService.Destroy(gameObject);
                        return;
                    }
                    m_unitHero = gameObject;
                    CellBase component = m_unitHero.GetComponent<CellBase>();
                    CellClone component2 = m_unitHeroDummy.GetComponent<CellClone>();
                    component.InitDummy(component2);
                    component2.m_unit = component;
                    component2.ResumeInitPos();
                    component2.UpdateInitPos();
                    m_unitNoTex.GetComponent<CellNoTex>().SetBindGameObject(m_unitHeroDummy);
                });
            }
        }

        private void AddLodUnit(int lodLv, Troops.ENMU_SQUARE_STAT fst, AnimationBase.ENMU_SPRITE_STATE sst)
        {
            Vector3 position = base.transform.position;
            int frameCount = m_loadFrame;
            int loadLoad = m_nLoadLoad;
            for (int i = 0; i < m_dummysConfig.Length; i++)
            {
                UnitDummyConfig unitDummyConfig = m_dummysConfig[i];
                CellClone component = m_dummys[i].GetComponent<CellClone>();
                CoreUtils.assetService.Instantiate("unit_dummy", (gameObject) =>
                {
                    if (m_loadFrame != frameCount)
                    {
                        CoreUtils.assetService.Destroy(gameObject);
                        return;
                    }
                    if (loadLoad != m_nLoadLoad)
                    {
                        CoreUtils.assetService.Destroy(gameObject);
                        return;
                    }
                    m_lodDummys.Add(gameObject);
                    gameObject.transform.SetParent(base.transform, worldPositionStays: false);
                    gameObject.transform.localPosition = Vector3.MoveTowards(component.transform.localPosition, Vector3.zero, 0.7f);
                    CellClone component2 = gameObject.GetComponent<CellClone>();
                    component2.ResumeInitPos();
                    component2.UpdateInitPos();
                    CoreUtils.assetService.Instantiate(unitDummyConfig.unitPrefab, (gameObject2) =>
                    {
                        if (m_loadFrame != frameCount)
                        {
                            CoreUtils.assetService.Destroy(gameObject2);
                            return;
                        }
                        if (loadLoad != m_nLoadLoad)
                        {
                            CoreUtils.assetService.Destroy(gameObject2);
                            return;
                        }
                        m_lodUnits.Add(gameObject2);
                        CellBase component3 = gameObject2.GetComponent<CellBase>();
                        SpriteRenderer component4 = gameObject2.GetComponent<SpriteRenderer>();
                        if (component4 != null)
                        {
                            component4.color = m_unitColor;
                        }
                        component2.m_unit = component3;
                        component3.InitDummy(component2);
                        component3.SetSpriteLoigicalState(sst);
                        component3.ChangeMoveState(CellBase.MOVE_STATE.STATIC);
                        AnimationBase component5 = gameObject2.GetComponent<AnimationBase>();
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
            foreach (GameObject lodUnit in m_lodUnits)
            {
                if (lodUnit != null)
                {
                    CoreUtils.assetService.Destroy(lodUnit);
                }
            }
            m_lodUnits.Clear();
            foreach (GameObject lodDummy in m_lodDummys)
            {
                if (lodDummy != null)
                {
                    CoreUtils.assetService.Destroy(lodDummy);
                }
            }
            m_lodDummys.Clear();
        }

        private int m_nLoadLoad;
        public void UpdateLod(int curLodLevel, Troops.ENMU_SQUARE_STAT fst, AnimationBase.ENMU_SPRITE_STATE sst)
        {
            m_nLoadLoad = curLodLevel;
            if (curLodLevel >= 2)
            {
                RemoveLodUnit();
            }
            else
            {
                AddLodUnit(curLodLevel, fst, sst);
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
            foreach (int idx in idxs)
            {
                Vector3 position = m_dummys[idx].transform.position;
                float magnitude = (position - pos).magnitude;
                if (num2 == -1 || magnitude < num)
                {
                    num = magnitude;
                    num2 = idx;
                }
            }
            return num2;
        }

        public void Remap()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < m_dummys.Length; i++)
            {
                m_dummys[i].GetComponent<CellClone>().m_unit = null;
                list.Add(i);
            }
            if (list.Count == 0)
            {
                return;
            }
            List<GameObject> list2 = new List<GameObject>();
            list2.AddRange(m_units);
            for (int j = 0; j < m_units.Count; j++)
            {
                GameObject gameObject = m_units[j];
                int closestIdx = GetClosestIdx(list, gameObject.transform.position);
                CellClone component = m_dummys[closestIdx].GetComponent<CellClone>();
                (component.m_unit = gameObject.GetComponent<CellBase>()).SetDummy(component);
                list.Remove(closestIdx);
                if (base.gameObject.name.StartsWith("square_mine"))
                {
                    Debug.Log("remap: j = " + j + ", minidx = " + closestIdx);
                }
            }
        }

        public int UnitTotalNumByCateoory(int category)
        {
            int num = 0;
            for (int i = 0; i < m_dummysConfig.Length; i++)
            {
                if (m_dummysConfig[i].unitType != 0 && (m_dummysConfig[i].unitType - 1) % 4 + 1 == category)
                {
                    num++;
                }
            }
            return num;
        }

        public int UnitNumByCategory(int category)
        {
            int num = 0;
            for (int i = 0; i < m_dummysConfig.Length; i++)
            {
                if (m_dummysConfig[i].unitType != 0 && (m_dummysConfig[i].unitType - 1) % 4 + 1 == category && m_dummys[i].GetComponent<CellClone>().m_unit != null)
                {
                    num++;
                }
            }
            return num;
        }

        public int DieUnit(int category, int num)
        {
            int num2 = num;
            for (int i = 0; i < m_dummysConfig.Length; i++)
            {
                if (num2 <= 0)
                {
                    break;
                }
                if ((m_dummysConfig[i].unitType - 1) % 4 + 1 == category)
                {
                    CellClone component = m_dummys[i].GetComponent<CellClone>();
                    CellBase unit = component.m_unit;
                    if (unit != null)
                    {
                        CoreUtils.assetService.Destroy(unit.gameObject);
                        unit.PlayDeadParticle();
                        component.m_unit = null;
                        num2--;
                        m_units[i] = null;
                    }
                }
            }
            return num - num2;
        }

        public void Reset()
        {
            m_loadFrame = Time.frameCount;

            if ((bool)m_unitNoTex)
            {
                m_unitNoTex.transform.SetParent(m_unitNoTexDummy.transform, worldPositionStays: false);
                m_unitNoTex.transform.localPosition = Vector3.zero;
                m_unitNoTex.transform.localEulerAngles = Vector3.zero;
                CellNoTex component = m_unitNoTex.GetComponent<CellNoTex>();
                if ((bool)component)
                {
                    component.ClearSkillEffect();
                }
            }
        }

        private void Update()
        {
            try
            {
                UpdateDebug();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void UpdateDebug()
        {
            if (m_isDebug && m_cubes.Count == 0)
            {
                for (int i = 0; i < m_dummys.Length; i++)
                {
                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    gameObject.transform.SetParent(m_dummys[i].transform, worldPositionStays: false);
                    gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    gameObject.transform.localPosition = Vector3.zero;
                    m_cubes.Add(gameObject);
                }
            }
            else if (!m_isDebug && m_cubes.Count > 0)
            {
                for (int j = 0; j < m_cubes.Count; j++)
                {
                    UnityEngine.Object.DestroyImmediate(m_cubes[j]);
                }
                m_cubes.Clear();
            }
        }
    }
}
using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class TerritoryLineMgr
    {
        private static readonly TerritoryLineMgr m_instance = new TerritoryLineMgr();

        private const string m_line_prefab_path = "troop/territory_line";

        private List<GameObject> m_territory_line_list_tactical = new List<GameObject>();

        private List<GameObject> m_territory_line_list_strategic = new List<GameObject>();

        public static TerritoryLineMgr GetInstance()
        {
            return TerritoryLineMgr.m_instance;
        }

        public void CreateLine(Vector2[] pos_array, Color color, float uvStep, float width, Material mat, bool isStrategic = false)
        {
            float smooth_distance = 2f;
            CoreUtils.assetService.LoadAssetAsync<GameObject>("territory_line_obj", (IAsset asset) =>
            {
                GameObject gameObject = asset.asset() as GameObject;
                if (isStrategic)
                {
                    gameObject.transform.parent = TerritoryMgr.territoryLineRoot;
                }
                gameObject.transform.position = Vector3.zero;
                LodTerritoryLine component = gameObject.GetComponent<LodTerritoryLine>();
                component.Init(pos_array, color, uvStep, width, smooth_distance);
                if (isStrategic)
                {
                    this.m_territory_line_list_strategic.Add(gameObject);
                }
                else
                {
                    this.m_territory_line_list_tactical.Add(gameObject);
                }
            });
        }

        public void ClearAllLine(bool clearTactical = true, bool clearStrategic = true)
        {
            if (clearTactical)
            {
                for (int i = 0; i < this.m_territory_line_list_tactical.Count; i++)
                {
                    if (this.m_territory_line_list_tactical[i].gameObject != null)
                    {
                        CoreUtils.assetService.Destroy(this.m_territory_line_list_tactical[i].gameObject);
                    }
                }
                this.m_territory_line_list_tactical.Clear();
            }
            if (clearStrategic)
            {
                for (int j = 0; j < this.m_territory_line_list_strategic.Count; j++)
                {
                    if (this.m_territory_line_list_strategic[j].gameObject != null)
                    {
                        CoreUtils.assetService.Destroy(this.m_territory_line_list_strategic[j].gameObject);
                    }
                }
                this.m_territory_line_list_strategic.Clear();
            }
        }

        public void SetTacticalLineShow(bool isShow)
        {
            for (int i = 0; i < this.m_territory_line_list_tactical.Count; i++)
            {
                if (this.m_territory_line_list_tactical[i] != null)
                {
                    this.m_territory_line_list_tactical[i].SetActive(isShow);
                }
            }
        }

        public void SetCanUpdate(bool canUpdate)
        {
            for (int i = 0; i < this.m_territory_line_list_strategic.Count; i++)
            {
                GameObject gameObject = this.m_territory_line_list_strategic[i];
                if (gameObject != null)
                {
                    LodTerritoryLine component = gameObject.GetComponent<LodTerritoryLine>();
                    component.SetCanUpdate(canUpdate);
                }
            }
        }
    }
}
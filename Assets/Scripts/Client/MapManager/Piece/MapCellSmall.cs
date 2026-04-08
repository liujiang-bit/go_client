using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class MapCellSmall : MonoBehaviour
    {
        private List<GameObject> m_gameobject_list = new List<GameObject>();

        private List<MapObjectData> m_piece_data_list = new List<MapObjectData>();

        private List<GameObject> m_grove_list = new List<GameObject>();

        private const string m_map_grove_prefab_prefix = "Grove";

        private Func<Vector2, bool> m_in_city_range_fun;

        private long m_loadQueue = 0;
        private void Start()
        {
        }

        public void SetInCityRangeFun(Func<Vector2, bool> fun)
        {
            m_in_city_range_fun = fun;
        }
        public Rect GetRect(float width)
        {
            float piece_plane_width = width;
            float num = piece_plane_width / 2f;
            return new Rect(base.transform.position.x - num, base.transform.position.z - num, piece_plane_width, piece_plane_width);
        }

        public void AddObject(List<MapObjectData> mapObjects)
        {
            m_piece_data_list.AddRange(mapObjects);
        }

        public void Refresh()
        {
            LoadObject();
        }

        private void ClearAll()
        {
            for (int i = 0; i < this.m_gameobject_list.Count; i++)
            {
                CoreUtils.assetService.Destroy(this.m_gameobject_list[i]);
            }
            this.m_gameobject_list.Clear();
            this.m_piece_data_list.Clear();
            this.m_grove_list.Clear();
            m_loadQueue++;
        }

        public void DestroySelf()
        {
            this.ClearAll();
            CoreUtils.assetService.Destroy(base.gameObject);
        }

        private void LoadObject()
        {
            m_loadQueue++;
            long currentQueue = m_loadQueue;
            for (int i = 0; i < this.m_piece_data_list.Count; i++)
            {
                MapObjectData mapObjectData = this.m_piece_data_list[i];
                if (mapObjectData.m_prefab_id.Contains("Grove"))
                {
                    CoreUtils.assetService.Instantiate(mapObjectData.m_prefab_id, (GameObject gameObject) =>
                    {
                        LoadObjectCallback(gameObject, mapObjectData, currentQueue);
                    });
                }
            }
        }

        private void LoadObjectCallback(GameObject obj, object call_back_object_data, long queueId)
        {
            if (queueId != m_loadQueue)
            {
                CoreUtils.assetService.Destroy(obj);
                return;
            }
            MapObjectData mapObjectData = (MapObjectData)call_back_object_data;
            GameObject gameObject = (GameObject)obj;
            gameObject.transform.SetParent(base.transform, true);
            gameObject.transform.position = mapObjectData.m_pos;
            if (gameObject.name.Contains("Grove"))
            {
                if(m_in_city_range_fun != null)
                {
                    Vector2 a = new Vector2(mapObjectData.m_pos.x, mapObjectData.m_pos.z);
                    if (m_in_city_range_fun(a))
                    {
                        CoreUtils.assetService.Destroy(gameObject);
                        return;
                    }

                }
                this.m_grove_list.Add(gameObject);
            }
            else if (gameObject.name.Contains("Tile_"))
            {
                gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, mapObjectData.m_rot_y, gameObject.transform.eulerAngles.z);
                LockAngle45[] componentsInChildren = gameObject.GetComponentsInChildren<LockAngle45>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].LockAngle();
                }
                if (gameObject.name.Contains("_lod2"))
                {
                    LockRotate180[] componentsInChildren2 = gameObject.GetComponentsInChildren<LockRotate180>();
                    for (int j = 0; j < componentsInChildren2.Length; j++)
                    {
                        componentsInChildren2[j].UpdateRotation(mapObjectData.m_rot_y);
                    }
                }
            }
            this.m_gameobject_list.Add(gameObject);
        }

        public void UpdateGrove(float city_pos_x, float city_pos_y, float remove_grove_distance)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < this.m_grove_list.Count; i++)
            {
                GameObject gameObject = this.m_grove_list[i];
                Vector2 a = new Vector2(gameObject.transform.position.x, gameObject.transform.position.z);
                if (Vector2.Distance(a, new Vector2(city_pos_x, city_pos_y)) <= remove_grove_distance)
                {
                    list.Add(gameObject);
                }
            }
            for (int j = 0; j < list.Count; j++)
            {
                GameObject gameObject2 = list[j];
                this.m_gameobject_list.Remove(gameObject2);
                this.m_grove_list.Remove(gameObject2);
                CoreUtils.assetService.Destroy(gameObject2);
            }
        }
    }
}
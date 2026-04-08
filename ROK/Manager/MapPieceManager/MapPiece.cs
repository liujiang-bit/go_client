using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class MapPiece : MonoBehaviour
    {
        private List<GameObject> m_gameobject_list = new List<GameObject>();

        private List<MapObjectData> m_piece_data_list = new List<MapObjectData>();

        private List<GameObject> m_grove_list = new List<GameObject>();

        private const string m_map_grove_prefab_prefix = "Grove";

        private void Start()
        {
        }

        public void Refresh()
        {
            float piece_width = MapPieceManager.GetInstance().m_piece_width;
            float num = piece_width / 2f;
            Rect rect = new Rect(base.transform.position.x - num, base.transform.position.z - num, piece_width, piece_width);
            this.m_piece_data_list = MapDataManager.GetInstance().GetDataInRange(rect);
            this.LoadObject();
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
        }

        public void DestroySelf()
        {
            this.ClearAll();
            CoreUtils.assetService.Destroy(base.gameObject);
        }

        private void LoadObject()
        {
            for (int i = 0; i < this.m_piece_data_list.Count; i++)
            {
                MapObjectData mapObjectData = this.m_piece_data_list[i];
                if (mapObjectData.m_prefab_id.Contains("Grove"))
                {
                    CoreUtils.assetService.InstantiateSlowly(mapObjectData.m_prefab_id, (GameObject gameObject) =>
                    {
                        LoadObjectCallback(gameObject, mapObjectData);
                    });
                }
            }
        }

        private void LoadObjectCallback(GameObject obj, object call_back_object_data)
        {
            if (this == null)
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
                foreach (MapCityItem current in MapCityManager.m_city_dict.Values)
                {
                    Vector2 a = new Vector2(mapObjectData.m_pos.x, mapObjectData.m_pos.z);
                    if (Vector2.Distance(a, current.m_pos) <= current.m_remove_grove_distance)
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
                TreeTweak[] componentsInChildren = gameObject.GetComponentsInChildren<TreeTweak>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].TweekTreeRot();
                }
                if (gameObject.name.Contains("_lod2"))
                {
                    AdorningTweak[] componentsInChildren2 = gameObject.GetComponentsInChildren<AdorningTweak>();
                    for (int j = 0; j < componentsInChildren2.Length; j++)
                    {
                        componentsInChildren2[j].UpdateAdorningRotation(mapObjectData.m_rot_y);
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
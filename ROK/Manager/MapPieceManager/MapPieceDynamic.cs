using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class MapPieceDynamic : MonoBehaviour
    {
        private List<MapObjectData> m_piece_data_list = new List<MapObjectData>();

        private Dictionary<Vector3, GameObject> m_tile_obj_dic = new Dictionary<Vector3, GameObject>();

        private int m_lod = -2;

        public void UpdatePiece(int lod)
        {
            if (Camera.main == null)
            {
                return;
            }
            this.m_lod = lod;
            float tile_width = MapDataManager.GetInstance().m_tile_width;
            float num = tile_width / 2f;
            Vector3[] cameraCornors = Common.GetCameraCornors(Camera.main, new Plane(Vector3.up, new Vector3(0f, 0f, 0f)));
            Rect rect = new Rect(cameraCornors[3].x - num - tile_width, cameraCornors[0].z - num - tile_width, cameraCornors[2].x - cameraCornors[3].x + 3f * tile_width, cameraCornors[2].z - cameraCornors[1].z + 3f * tile_width);
            List<Vector3> list = new List<Vector3>(this.m_tile_obj_dic.Count);
            foreach (KeyValuePair<Vector3, GameObject> current in this.m_tile_obj_dic)
            {
                Vector3 key = current.Key;
                Vector2 point = new Vector2(key.x, key.z);
                if (!Common.RectLeftBottomContains(rect, point))
                {
                    list.Add(key);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                Vector3 key2 = list[i];
                if (this.m_tile_obj_dic[key2] != null)
                {
                    CoreUtils.assetService.Destroy(this.m_tile_obj_dic[key2]);
                }
                this.m_tile_obj_dic.Remove(key2);
            }
            Dictionary<string, MapTileData> mapDataTile = MapDataManager.GetInstance().GetMapDataTile();
            foreach (MapTileData current2 in mapDataTile.Values)
            {
                List<MapObjectData> map_obj_data_list = current2.m_map_obj_data_list;
                for (int j = 0; j < map_obj_data_list.Count; j++)
                {
                    MapObjectData mapObjectData = map_obj_data_list[j];
                    Vector2 point2 = new Vector2(mapObjectData.m_pos.x, mapObjectData.m_pos.z);
                    if (!this.m_tile_obj_dic.ContainsKey(mapObjectData.m_pos) && Common.RectLeftBottomContains(rect, point2))
                    {
                        this.m_tile_obj_dic[mapObjectData.m_pos] = null;
                        CoreUtils.assetService.InstantiateSlowly(mapObjectData.m_prefab_id, (GameObject gameObject) =>
                        {
                            LoadObjectCallback(gameObject, mapObjectData);
                        });
                    }
                }
            }
        }

        private void ClearAll()
        {
            foreach (GameObject current in this.m_tile_obj_dic.Values)
            {
                if (current != null)
                {
                    CoreUtils.assetService.Destroy(current);
                }
            }
            this.m_tile_obj_dic.Clear();
        }

        public void DestroySelf()
        {
            this.ClearAll();
        }

        private void LoadObjectCallback(GameObject obj, object call_back_object_data)
        {
            MapObjectData mapObjectData = (MapObjectData)call_back_object_data;
            if (!this.m_tile_obj_dic.ContainsKey(mapObjectData.m_pos))
            {
                CoreUtils.assetService.Destroy(obj);
                return;
            }
            if (this.m_tile_obj_dic[mapObjectData.m_pos] != null)
            {
                CoreUtils.assetService.Destroy(this.m_tile_obj_dic[mapObjectData.m_pos]);
            }
            GameObject gameObject = (GameObject)obj;
            gameObject.transform.SetParent(base.transform, true);
            gameObject.transform.position = mapObjectData.m_pos;
            if (gameObject.name.Contains("Tile_"))
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
            this.m_tile_obj_dic[mapObjectData.m_pos] = gameObject;
        }
    }
}
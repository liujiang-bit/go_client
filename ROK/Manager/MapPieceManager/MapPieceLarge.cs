using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class MapPieceLarge : MonoBehaviour
    {
        private List<GameObject> m_gameobject_list = new List<GameObject>();

        private List<MapObjectData> m_piece_data_list = new List<MapObjectData>();

        private const string m_map_plane_prefab_prefix = "_TYPE_";

        private const string m_map_river_prefab_prefix = "river";

        private const string m_map_tree_prefab_prefix = "tree";

        private void Start()
        {
        }

        public void Refresh()
        {
            float piece_plane_width = MapPieceManager.GetInstance().m_piece_plane_width;
            float num = piece_plane_width / 2f;
            Rect rect = new Rect(base.transform.position.x - num, base.transform.position.z - num, piece_plane_width, piece_plane_width);
            this.m_piece_data_list.AddRange(MapDataManager.GetInstance().GetTilePlaneDataInRange(rect));
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
                if (mapObjectData.m_prefab_id.Contains("_TYPE_"))
                {
                    CoreUtils.assetService.Instantiate(mapObjectData.m_prefab_id, (GameObject gameObject) =>
                    {
                        LoadObjectCallback(gameObject, mapObjectData);
                    });
                }
                else
                {
                    CoreUtils.assetService.InstantiateSlowly(mapObjectData.m_prefab_id, (GameObject gameObject) =>
                    {
                        LoadObjectCallback(gameObject, mapObjectData);
                    });
                }
            }
        }

        private void LoadObjectCallback(UnityEngine.GameObject obj, object call_back_object_data)
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
            gameObject.transform.localScale = mapObjectData.m_scale;
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, mapObjectData.m_rot_y, gameObject.transform.eulerAngles.z);
            if (gameObject.name.Contains("river"))
            {
                if (!mapObjectData.m_river_flow_direction)
                {
                    RiverFlowController component = gameObject.GetComponent<RiverFlowController>();
                    if (component != null)
                    {
                        component.SetFlowDirection(-1f);
                    }
                }
            }
            else if (gameObject.name.Contains("tree") && mapObjectData.m_rot_y != 0f)
            {
                AdorningTweak[] componentsInChildren = gameObject.GetComponentsInChildren<AdorningTweak>();
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].UpdateAdorningRotation(mapObjectData.m_rot_y);
                }
            }
            this.m_gameobject_list.Add(gameObject);
        }
    }
}
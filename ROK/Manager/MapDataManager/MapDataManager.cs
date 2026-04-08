using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class MapDataManager
    {
        private Dictionary<string, MapTileBrief> m_dict_map_tile_brief = new Dictionary<string, MapTileBrief>();

        private Dictionary<string, MapTileData> m_dict_map_tile_sample_data_adorning = new Dictionary<string, MapTileData>();

        private Dictionary<string, MapTileData> m_dict_map_tile_sample_data_tile_plane = new Dictionary<string, MapTileData>();

        private Dictionary<string, MapTileData> m_dict_map_tile_sample_data_brief = new Dictionary<string, MapTileData>();

        public int m_server_width = 30;

        private Dictionary<string, MapTileData> m_current_map_data_adorning = new Dictionary<string, MapTileData>();

        private Dictionary<string, MapTileData> m_current_map_data_tile_plane = new Dictionary<string, MapTileData>();

        private Dictionary<string, MapTileData> m_current_map_data_tile_detail = new Dictionary<string, MapTileData>();

        private Dictionary<string, MapTileData> m_current_map_data_tile_brief = new Dictionary<string, MapTileData>();

        private Vector2 m_current_tile_center = Vector2.zero;

        public float m_tile_width = 180f;

        public int m_tile_piece_count_in_row = 3;

        private bool m_load_data_done;

        private bool m_force_update_once;

        private static readonly MapDataManager m_instance = new MapDataManager();

        public bool loadDataDone
        {
            get
            {
                return this.m_load_data_done;
            }
            set
            {
                this.m_load_data_done = value;
            }
        }

        public bool forceUpdateOnce
        {
            get
            {
                return this.m_force_update_once;
            }
            set
            {
                this.m_force_update_once = value;
            }
        }

        public static MapDataManager GetInstance()
        {
            return MapDataManager.m_instance;
        }

        public void SetMapWidth(int width)
        {
            this.m_server_width = width;
        }

        public void ClearMapTileBrief()
        {
            this.m_dict_map_tile_brief.Clear();
        }

        public void ReadMapBriefDataFromFile(string map_data_path, int server_x, int server_y, Action action = null)
        {
            CoreUtils.assetService.LoadAssetAsync<TextAsset>(map_data_path, (IAsset asset) =>
            {
                TextAsset textAsset = asset.asset() as TextAsset;
                string[] array = textAsset.text.Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array.Length; i++)
                {
                    int x = i % this.m_server_width + server_x * this.m_server_width;
                    int y = i / this.m_server_width + server_y * this.m_server_width;
                    string str = x.ToString();
                    string str2 = y.ToString();
                    string key = str + "_" + str2;
                    string text = array[i];
                    string[] array2 = text.Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
                    string[] array3 = array2[5].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                    string[] array4 = new string[MapTileBrief.m_province_name_count];
                    if (array3.Length == 1)
                    {
                        for (int j = 0; j < array4.Length; j++)
                        {
                            array4[j] = array3[0];
                        }
                    }
                    else
                    {
                        array4 = array3;
                    }
                    MapTileBrief value = new MapTileBrief(x, y, array2[0], float.Parse(array2[1]), array2[2] == "1", array2[3] == "1", array2[4] == "1", array4);
                    this.m_dict_map_tile_brief.Add(key, value);
                }
                action?.Invoke();
            });
        }

        public void ReadMapDataFromFile(string tile_data_path, Action action = null)
        {
            this.m_dict_map_tile_sample_data_tile_plane.Clear();
            this.m_dict_map_tile_sample_data_adorning.Clear();
            this.m_dict_map_tile_sample_data_brief.Clear();
            int currentLodLevel = 0;// LodCamera.instance.GetCurrentLodLevel();

            CoreUtils.assetService.LoadAssetAsync<TextAsset>(tile_data_path, (IAsset asset) =>
            {
                TextAsset textAsset = asset.asset() as TextAsset;
                string[] array = textAsset.text.Split(Common.DATA_DELIMITER_LEVEL_4, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array.Length; i++)
                {
                    string[] array2 = array[i].Split(Common.DATA_DELIMITER_LEVEL_3, StringSplitOptions.None);
                    string key = array2[0];
                    string text = array2[1];
                    string text2 = array2[2];
                    MapTileData mapTileData = new MapTileData();
                    MapTileData value = new MapTileData();
                    MapTileData mapTileData2 = new MapTileData();
                    string[] array3 = text.Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                    float num = this.m_tile_width / (float)this.m_tile_piece_count_in_row;
                    float num2 = num / 2f;
                    for (int j = 0; j < array3.Length; j++)
                    {
                        string prefab_id = array3[j];
                        Vector3 pos = new Vector3((float)(j % this.m_tile_piece_count_in_row) * num + num2, 0f, (float)(j / this.m_tile_piece_count_in_row) * num + num2);
                        float rot_y = 0f;
                        Vector3 one = Vector3.one;
                        MapObjectData item = new MapObjectData(prefab_id, pos, rot_y, one, true, currentLodLevel);
                        mapTileData2.m_map_obj_data_list.Add(item);
                    }
                    string[] array4 = text2.Split(Common.DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < array4.Length; k++)
                    {
                        string text3 = array4[k];
                        string[] array5 = text3.Split(Common.DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
                        string text4 = array5[0];
                        for (int l = 1; l < array5.Length; l++)
                        {
                            string[] array6 = array5[l].Split(Common.DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                            Vector3 pos2 = new Vector3(float.Parse(array6[0]), float.Parse(array6[1]), float.Parse(array6[2]));
                            float rot_y2 = float.Parse(array6[3]);
                            Vector3 scale = new Vector3(float.Parse(array6[4]), float.Parse(array6[5]), float.Parse(array6[6]));
                            bool river_flow_direction = true;
                            if (array6.Length == 8 && array6[7] == "-1")
                            {
                                river_flow_direction = false;
                            }
                            MapObjectData item2 = new MapObjectData(text4, pos2, rot_y2, scale, river_flow_direction, currentLodLevel);
                            if (text4.Contains("Grove"))
                            {
                                mapTileData.m_map_obj_data_list.Add(item2);
                            }
                            else
                            {
                                mapTileData2.m_map_obj_data_list.Add(item2);
                            }
                        }
                    }
                    this.m_dict_map_tile_sample_data_tile_plane.Add(key, mapTileData2);
                    this.m_dict_map_tile_sample_data_adorning.Add(key, mapTileData);
                    this.m_dict_map_tile_sample_data_brief.Add(key, value);
                }
                this.loadDataDone = true;
                action?.Invoke();
            });
        }

        public void UpdateTile(Vector2 pos)
        {
            Vector2 centerByPos = Common.GetCenterByPos(pos, MapPieceManager.GetInstance().m_piece_width);
            if (centerByPos != this.m_current_tile_center || this.forceUpdateOnce)
            {
                if (this.forceUpdateOnce)
                {
                    this.forceUpdateOnce = false;
                }
                int x = (int)(centerByPos.x / this.m_tile_width);
                int y = (int)(centerByPos.y / this.m_tile_width);
                int currentLodLevel = LodCamera.instance.GetCurrentLodLevel();
                this.ReadData(x, y, currentLodLevel);
                this.m_current_tile_center = centerByPos;
            }
        }

        private void ReadData(int x, int y, int lod)
        {
            float piece_plane_width = MapPieceManager.GetInstance().m_piece_plane_width;
            float piece_width = MapPieceManager.GetInstance().m_piece_width;
            if (lod == 0)
            {
                this.m_current_map_data_tile_detail.Clear();
                this.m_current_map_data_tile_brief.Clear();
                this.DoReadData(this.m_dict_map_tile_sample_data_adorning, ref this.m_current_map_data_adorning, x, y, piece_width, false);
                this.DoReadData(this.m_dict_map_tile_sample_data_tile_plane, ref this.m_current_map_data_tile_plane, x, y, piece_plane_width, false);
            }
            else if (lod == 1)
            {
                this.m_current_map_data_tile_detail.Clear();
                this.m_current_map_data_tile_brief.Clear();
                this.DoReadData(this.m_dict_map_tile_sample_data_adorning, ref this.m_current_map_data_adorning, x, y, piece_width, false);
                this.DoReadData(this.m_dict_map_tile_sample_data_tile_plane, ref this.m_current_map_data_tile_plane, x, y, piece_plane_width, false);
            }
            else if (lod == 2)
            {
                this.m_current_map_data_adorning.Clear();
                this.m_current_map_data_tile_brief.Clear();
                this.m_current_map_data_tile_plane.Clear();
                this.DoReadDataFromTileBrief(ref this.m_current_map_data_tile_detail, x, y, lod);
            }
            else if (lod == 3)
            {
                this.m_current_map_data_adorning.Clear();
                this.m_current_map_data_tile_brief.Clear();
                this.DoReadDataFromTileBrief(ref this.m_current_map_data_tile_detail, x, y, lod);
            }
            else if (lod == 4)
            {
                this.m_current_map_data_adorning.Clear();
                this.m_current_map_data_tile_brief.Clear();
                this.m_current_map_data_tile_plane.Clear();
                this.DoReadDataFromTileBrief(ref this.m_current_map_data_tile_detail, x, y, lod);
            }
            else if (lod == 5)
            {
                this.m_current_map_data_adorning.Clear();
                this.m_current_map_data_tile_detail.Clear();
                this.DoReadDataFromTileBrief(ref this.m_current_map_data_tile_brief, x, y, lod);
            }
        }

        private void DoReadDataFromTileBrief(ref Dictionary<string, MapTileData> result_dict, int x, int y, int lod)
        {
            string str = "lod" + lod.ToString();
            Dictionary<string, MapTileData> dictionary = new Dictionary<string, MapTileData>();
            int num = Mathf.CeilToInt(MapPieceManager.GetInstance().m_piece_width / this.m_tile_width) * 3;
            num = Mathf.CeilToInt((float)num / 2f);
            for (int i = x - num; i < x + num; i++)
            {
                for (int j = y - num; j < y + num; j++)
                {
                    string dataKey = this.GetDataKey(i, j);
                    if (this.m_dict_map_tile_brief.ContainsKey(dataKey))
                    {
                        string tile_id = this.m_dict_map_tile_brief[dataKey].m_tile_id;
                        Vector3 worldPos = this.m_dict_map_tile_brief[dataKey].GetWorldPos();
                        float tile_rot = this.m_dict_map_tile_brief[dataKey].m_tile_rot;
                        MapTileData mapTileData = new MapTileData();
                        MapObjectData item = new MapObjectData(tile_id + "_" + str, worldPos, tile_rot, Vector3.one, false, lod);
                        mapTileData.m_map_obj_data_list.Add(item);
                        dictionary.Add(dataKey, mapTileData);
                    }
                }
            }
            result_dict = dictionary;
        }

        private void DoReadData(Dictionary<string, MapTileData> sample_dict, ref Dictionary<string, MapTileData> result_dict, int x, int y, float width, bool read_half = false)
        {
            Dictionary<string, MapTileData> dictionary = new Dictionary<string, MapTileData>();
            int num = Mathf.CeilToInt(width / this.m_tile_width) * 3;
            num = Mathf.CeilToInt((float)num / 2f);
            for (int i = x - num + 1; i < x + num; i++)
            {
                for (int j = y - num + 1; j < y + num; j++)
                {
                    string dataKey = this.GetDataKey(i, j);
                    if (result_dict.ContainsKey(dataKey))
                    {
                        dictionary.Add(dataKey, result_dict[dataKey]);
                    }
                    else if (this.m_dict_map_tile_brief.ContainsKey(dataKey))
                    {
                        string tile_id = this.m_dict_map_tile_brief[dataKey].m_tile_id;
                        float tile_rot = this.m_dict_map_tile_brief[dataKey].m_tile_rot;
                        MapTileData mapTileData;
                        if (read_half)
                        {
                            mapTileData = (MapTileData)sample_dict[tile_id].DeepCloneHalf();
                        }
                        else
                        {
                            mapTileData = (MapTileData)sample_dict[tile_id].DeepClone();
                        }
                        mapTileData.GenerateMapObjectData(i, j, tile_rot, this.m_dict_map_tile_brief[dataKey].m_enable_bridge, this.m_dict_map_tile_brief[dataKey].m_river_flow_direction);
                        dictionary.Add(dataKey, mapTileData);
                    }
                }
            }
            result_dict = dictionary;
        }

        public List<MapObjectData> GetDataInRange(Rect rect)
        {
            List<MapObjectData> list = new List<MapObjectData>();
            foreach (MapTileData current in this.m_current_map_data_adorning.Values)
            {
                list.AddRange(current.GetMapObjDataInRange(rect));
            }
            foreach (MapTileData current2 in this.m_current_map_data_tile_detail.Values)
            {
                list.AddRange(current2.GetMapObjDataInRange(rect));
            }
            foreach (MapTileData current3 in this.m_current_map_data_tile_brief.Values)
            {
                list.AddRange(current3.GetMapObjDataInRange(rect));
            }
            return list;
        }

        public Dictionary<string, MapTileData> GetMapDataTile()
        {
            int currentLodLevel = LodCamera.instance.GetCurrentLodLevel();
            if (currentLodLevel == 2 || currentLodLevel == 3 || currentLodLevel == 4)
            {
                return this.m_current_map_data_tile_detail;
            }
            return this.m_current_map_data_tile_brief;
        }

        public List<MapObjectData> GetTilePlaneDataInRange(Rect rect)
        {
            List<MapObjectData> list = new List<MapObjectData>();
            foreach (MapTileData current in this.m_current_map_data_tile_plane.Values)
            {
                list.AddRange(current.GetMapObjDataInRange(rect));
            }
            return list;
        }

        private string GetDataKey(int x, int y)
        {
            return x.ToString() + "_" + y.ToString();
        }

        public static List<MapProvinceNameStruct> GetMapProvinceNameStruct_S()
        {
            return MapDataManager.m_instance.GetMapProvinceNameStruct();
        }

        private List<MapProvinceNameStruct> GetMapProvinceNameStruct()
        {
            List<MapProvinceNameStruct> list = new List<MapProvinceNameStruct>();
            foreach (MapTileBrief current in this.m_dict_map_tile_brief.Values)
            {
                if (current.m_show_province_name)
                {
                    list.Add(new MapProvinceNameStruct
                    {
                        m_pos = current.GetWorldPos(),
                        m_province_name = current.m_province_name_array[0]
                    });
                }
            }
            return list;
        }

        public static string GetMapProvinceName_S(Vector2 pos)
        {
            return MapDataManager.m_instance.GetMapProvinceName(pos);
        }

        private string GetMapProvinceName(Vector2 pos)
        {
            float num = MapDataManager.GetInstance().m_tile_width;
            string key = string.Format("{0}_{1}", Mathf.FloorToInt(pos.x / num), Mathf.FloorToInt(pos.y / num));
            MapTileBrief mapTileBrief = null;
            if (this.m_dict_map_tile_brief.TryGetValue(key, out mapTileBrief))
            {
                float num2 = num / 2f;
                int num3 = Mathf.FloorToInt(pos.x % num / num2) + Mathf.FloorToInt(pos.y % num / num2) * (int)Mathf.Sqrt((float)MapTileBrief.m_province_name_count);
                return mapTileBrief.m_province_name_array[num3];
            }
            return string.Empty;
        }
    }
}
using System;
using UnityEngine;
using Skyunion;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    public class MapManager : MonoSingleton<MapManager>
    {
        private int m_server_width = 30;

        private float m_tile_width = 180f;

        private Vector2 m_current_tile_center = Vector2.zero;

        private int m_tile_piece_count_in_row = 3;

        private bool m_load_data_done;

        private bool m_force_update_once;

        private static char[] DATA_DELIMITER_LEVEL_0 = new char[]
        {
        '\u0001'
        };

        private static char[] DATA_DELIMITER_LEVEL_1 = new char[]
        {
        '\u0002'
        };

        private static char[] DATA_DELIMITER_LEVEL_2 = new char[]
        {
        '\u0003'
        };

        private static char[] DATA_DELIMITER_LEVEL_3 = new char[]
        {
        '\u0004'
        };

        private static char[] DATA_DELIMITER_LEVEL_4 = new char[]
        {
        '\u0005'
        };

        private Dictionary<string, TileSimple> m_dict_map_tile_brief = new Dictionary<string, TileSimple>();

        private Dictionary<string, TileData> m_dict_map_tile_sample_data_adorning = new Dictionary<string, TileData>();

        private Dictionary<string, TileData> m_dict_map_tile_sample_data_tile_plane = new Dictionary<string, TileData>();

        private Dictionary<string, TileData> m_dict_map_tile_sample_data_brief = new Dictionary<string, TileData>();

        private Dictionary<string, TileData> m_current_map_data_adorning = new Dictionary<string, TileData>();

        private Dictionary<string, TileData> m_current_map_data_tile_plane = new Dictionary<string, TileData>();

        private Dictionary<string, TileData> m_current_map_data_tile_detail = new Dictionary<string, TileData>();

        private Dictionary<string, TileData> m_current_map_data_tile_brief = new Dictionary<string, TileData>();

        public void UpdatePieceWidth(float piece_width, float piece_plane_width)
        {
            if (m_piece_width != piece_width || m_piece_plane_width != piece_plane_width)
            {
                m_force_update_once = true;
            }
            this.m_piece_width = piece_width;
            this.m_piece_plane_width = piece_plane_width;
        }
        public void SetMapWidth(int width)
        {
            this.m_server_width = width;
        }
        public void ClearMapTileBrief()
        {
            this.m_dict_map_tile_brief.Clear();
        }
        public void ReadMapBriefDataFromFile2(string map_data_path, int server_x, int server_y, Action action = null)
        {
            CoreUtils.assetService.LoadAssetAsync<TextAsset>(map_data_path, (IAsset asset) =>
            {
                TextAsset textAsset = asset.asset() as TextAsset;
                var reader = new BinaryReader(new MemoryStream(textAsset.bytes));
                int count = reader.ReadInt32();
                int width = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int x = i % width + server_x * this.m_server_width;
                    int y = i / width + server_y * this.m_server_width;
                    string str = x.ToString();
                    string str2 = y.ToString();
                    string key = str + "_" + str2;
                    string text = reader.ReadString();
                    float rote_y = reader.ReadSingle();
                    bool enable_bridge = reader.ReadBoolean();
                    bool flow_river = reader.ReadBoolean();
                    bool show_province = reader.ReadBoolean();
                    string[] array4 = new string[TileSimple.m_province_name_count];
                    int province_count = reader.ReadByte();
                    if (province_count == 1)
                    {
                        var name = reader.ReadString();
                        for (int j = 0; j < array4.Length; j++)
                        {
                            array4[j] = name;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < array4.Length; j++)
                        {
                            array4[j] = reader.ReadString();
                        }
                    }
                    
                    if (!m_dict_map_tile_brief.ContainsKey(key))
                    {
                        TileSimple value = new TileSimple(x, y, text, rote_y, enable_bridge, flow_river, show_province, array4, m_tile_width);
                        this.m_dict_map_tile_brief.Add(key, value);
                    }
                }
                action?.Invoke();
            }, null);
        }

        public void ReadMapDataFromFile2(string tile_data_path, Action action = null)
        {
            this.m_dict_map_tile_sample_data_tile_plane.Clear();
            this.m_dict_map_tile_sample_data_adorning.Clear();
            this.m_dict_map_tile_sample_data_brief.Clear();
            int currentLodLevel = LevelDetailCamera.instance.GetCurrentLodLevel();

            CoreUtils.assetService.LoadAssetAsync<TextAsset>(tile_data_path, (IAsset asset) =>
            {
                TextAsset textAsset = asset.asset() as TextAsset;
                var reader = new BinaryReader(new MemoryStream(textAsset.bytes));
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    TileData mapTileData = new TileData();
                    TileData value = new TileData();
                    TileData mapTileData2 = new TileData();
                    float num = this.m_tile_width / (float)this.m_tile_piece_count_in_row;
                    float num2 = num / 2f;

                    int t_count = reader.ReadInt32();
                    for (int j = 0; j < t_count; j++)
                    {
                        string prefab_id = reader.ReadString();
                        Vector3 pos = new Vector3((float)(j % this.m_tile_piece_count_in_row) * num + num2, 0f, (float)(j / this.m_tile_piece_count_in_row) * num + num2);
                        float rot_y = reader.ReadSingle();
                        float sx = reader.ReadSingle();
                        float sy = reader.ReadSingle();
                        float sz = reader.ReadSingle();
                        Vector3 scale = new Vector3(sx, sy, sz);
                        MapObjectData item = new MapObjectData(prefab_id, pos, rot_y, scale, true, currentLodLevel);
                        mapTileData2.m_map_obj_data_list.Add(item);
                    }
                    t_count = reader.ReadInt32();
                    for (int k = 0; k < t_count; k++)
                    {
                        string text4 = reader.ReadString();
                        float x = reader.ReadSingle();
                        float y = reader.ReadSingle();
                        float z = reader.ReadSingle();
                        Vector3 pos2 = new Vector3(x+90, y, z+90);

                        float rot_y2 = reader.ReadSingle();

                        float sx = reader.ReadSingle();
                        float sy = reader.ReadSingle();
                        float sz = reader.ReadSingle();
                        Vector3 scale = new Vector3(sx, sy, sz);

                        bool river_flow_direction = reader.ReadByte() != 0;

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
                    this.m_dict_map_tile_sample_data_tile_plane.Add(key, mapTileData2);
                    this.m_dict_map_tile_sample_data_adorning.Add(key, mapTileData);
                    this.m_dict_map_tile_sample_data_brief.Add(key, value);
                }
                m_load_data_done = true;
                action?.Invoke();
            }, null);
        }

        public void ReadMapBriefDataFromFile(string map_data_path, int server_x, int server_y, Action action = null)
        {
            CoreUtils.assetService.LoadAssetAsync<TextAsset>(map_data_path, (IAsset asset) =>
            {
                TextAsset textAsset = asset.asset() as TextAsset;
                string[] array = textAsset.text.Split(DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array.Length; i++)
                {
                    int x = i % this.m_server_width + server_x * this.m_server_width;
                    int y = i / this.m_server_width + server_y * this.m_server_width;
                    string str = x.ToString();
                    string str2 = y.ToString();
                    string key = str + "_" + str2;
                    string text = array[i];
                    string[] array2 = text.Split(DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
                    string[] array3 = array2[5].Split(DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
                    string[] array4 = new string[TileSimple.m_province_name_count];
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
                    TileSimple value = new TileSimple(x, y, array2[0], float.Parse(array2[1]), array2[2] == "1", array2[3] == "1", array2[4] == "1", array4, m_tile_width);
                    this.m_dict_map_tile_brief.Add(key, value);
                }
                action?.Invoke();

                StringBuilder sb = new StringBuilder();
                sb.Append("brief");
                sb.Append("\r\n");
                foreach (var data in m_dict_map_tile_brief)
                {
                    sb.Append("\t");
                    sb.Append(data.Key);
                    sb.Append("\t");
                    sb.Append(data.Value.m_tile_id);
                    sb.Append("\t");
                    sb.Append(data.Value.m_tile_width);
                    sb.Append("\r\n bridge ");
                    sb.Append(data.Value.m_enable_bridge);
                    sb.Append("\r\n");
                }

                File.WriteAllText(Application.streamingAssetsPath + "/brief_data.txt", sb.ToString());
            }, null);
        }

        public void ReadMapDataFromFile(string tile_data_path, Action action = null)
        {
            this.m_dict_map_tile_sample_data_tile_plane.Clear();
            this.m_dict_map_tile_sample_data_adorning.Clear();
            this.m_dict_map_tile_sample_data_brief.Clear();
            int currentLodLevel = LevelDetailCamera.instance.GetCurrentLodLevel();

            CoreUtils.assetService.LoadAssetAsync<TextAsset>(tile_data_path, (IAsset asset) =>
            {
                TextAsset textAsset = asset.asset() as TextAsset;
                string[] array = textAsset.text.Split(DATA_DELIMITER_LEVEL_4, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array.Length; i++)
                {
                    string[] array2 = array[i].Split(DATA_DELIMITER_LEVEL_3, StringSplitOptions.None);
                    string key = array2[0];
                    string text = array2[1];
                    string text2 = array2[2];
                    TileData mapTileData = new TileData();
                    TileData value = new TileData();
                    TileData mapTileData2 = new TileData();
                    string[] array3 = text.Split(DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                    float num = this.m_tile_width / (float)this.m_tile_piece_count_in_row;
                    float num2 = num / 2f;
                    for (int j = 0; j < array3.Length; j++)
                    {
                        string prefab_id = array3[j];
                        Vector3 pos = new Vector3((float)(j % this.m_tile_piece_count_in_row) * num + num2, 0f, (float)(j / this.m_tile_piece_count_in_row) * num + num2);
                        float rot_y = 0f;
                        Vector3 one = Vector3.one;
                        //>
                        //prefab_id = "I_TYPE_0";
                        MapObjectData item = new MapObjectData(prefab_id, pos, rot_y, one, true, currentLodLevel);
                        mapTileData2.m_map_obj_data_list.Add(item);
                    }
                    string[] array4 = text2.Split(DATA_DELIMITER_LEVEL_2, StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < array4.Length; k++)
                    {
                        string text3 = array4[k];
                        string[] array5 = text3.Split(DATA_DELIMITER_LEVEL_1, StringSplitOptions.RemoveEmptyEntries);
                        string text4 = array5[0];
                        for (int l = 1; l < array5.Length; l++)
                        {
                            string[] array6 = array5[l].Split(DATA_DELIMITER_LEVEL_0, StringSplitOptions.RemoveEmptyEntries);
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
                                if (item2.m_prefab_id.Contains("_tree_"))
                                {
                                    item2.m_prefab_id = "Tile_31e-f890-446d-b07b-3fc2a7cf9c82_tree_0";
                                }
                                mapTileData2.m_map_obj_data_list.Add(item2);
                            }
                        }
                    }
                    this.m_dict_map_tile_sample_data_tile_plane.Add(key, mapTileData2);
                    this.m_dict_map_tile_sample_data_adorning.Add(key, mapTileData);
                    this.m_dict_map_tile_sample_data_brief.Add(key, value);
                }
                m_load_data_done = true;
                action?.Invoke();

                StringBuilder sb = new StringBuilder();
                sb.Append("tile_plane");
                foreach (var data in m_dict_map_tile_sample_data_tile_plane)
                {
                    sb.Append("\t");
                    sb.Append(data.Key);
                    sb.Append("\r\n");
                    foreach (var gamedata in data.Value.m_map_obj_data_list)
                    {
                        sb.Append("\t\t");
                        sb.Append(gamedata.m_prefab_id);
                        sb.Append("\r\n");
                    }
                }
                sb.Append("adorning");
                foreach (var data in m_dict_map_tile_sample_data_adorning)
                {
                    sb.Append("\t");
                    sb.Append(data.Key);
                    sb.Append("\r\n");
                    foreach (var gamedata in data.Value.m_map_obj_data_list)
                    {
                        sb.Append("\t\t");
                        sb.Append(gamedata.m_prefab_id);
                        sb.Append("\r\n");
                    }
                }
                sb.Append("brief");
                foreach (var data in m_dict_map_tile_sample_data_brief)
                {
                    sb.Append("\t");
                    sb.Append(data.Key);
                    sb.Append("\r\n");
                    foreach (var gamedata in data.Value.m_map_obj_data_list)
                    {
                        sb.Append("\t\t");
                        sb.Append(gamedata.m_prefab_id);
                        sb.Append("\r\n");
                    }
                }

                File.WriteAllText(Application.streamingAssetsPath + "/tile_data.txt", sb.ToString());
            }, null);
        }

        private string GetDataKey(int x, int y)
        {
            return x.ToString() + "_" + y.ToString();
        }

        private void DoReadDataFromTileBrief(ref Dictionary<string, TileData> result_dict, int x, int y, int lod)
        {
            string str = "lod" + lod.ToString();
            Dictionary<string, TileData> dictionary = new Dictionary<string, TileData>();
            int num = Mathf.CeilToInt(m_piece_width / this.m_tile_width) * 3;
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
                        TileData mapTileData = new TileData();
                        //tile_id = "Tile_00001";
                        MapObjectData item = new MapObjectData(tile_id + "_" + str, worldPos, tile_rot, Vector3.one, false, lod);
                        mapTileData.m_map_obj_data_list.Add(item);
                        dictionary.Add(dataKey, mapTileData);
                    }
                }
            }
            result_dict = dictionary;
        }

        private void DoReadData(Dictionary<string, TileData> sample_dict, ref Dictionary<string, TileData> result_dict, int x, int y, float width, bool read_half = false)
        {
            Dictionary<string, TileData> dictionary = new Dictionary<string, TileData>();
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
                        TileData mapTileData = default(TileData);
                        if (read_half)
                        {
                            mapTileData = (TileData)sample_dict[tile_id].DeepCloneHalf();
                        }
                        else
                        {
                            if (sample_dict.ContainsKey(tile_id))
                            {
                                mapTileData = (TileData)sample_dict[tile_id].DeepClone();
                            }
                            else
                            {
                                Debug.Log(tile_id);
                            }
                        }
                        mapTileData.GenerateMapObjectData(i, j, tile_rot, m_tile_width, this.m_dict_map_tile_brief[dataKey].m_enable_bridge, this.m_dict_map_tile_brief[dataKey].m_river_flow_direction);
                        dictionary.Add(dataKey, mapTileData);
                    }
                }
            }
            result_dict = dictionary;
        }

        private void ReadData(int x, int y, int lod)
        {
            float piece_plane_width = m_piece_plane_width;
            float piece_width = m_piece_width;
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

        private List<MapObjectData> GetDataInRange(Rect rect)
        {
            List<MapObjectData> list = new List<MapObjectData>();
            foreach (TileData current in this.m_current_map_data_adorning.Values)
            {
                list.AddRange(current.GetMapObjDataInRange(rect));
            }
            foreach (TileData current2 in this.m_current_map_data_tile_detail.Values)
            {
                list.AddRange(current2.GetMapObjDataInRange(rect));
            }
            foreach (TileData current3 in this.m_current_map_data_tile_brief.Values)
            {
                list.AddRange(current3.GetMapObjDataInRange(rect));
            }
            return list;
        }
        private Dictionary<string, TileData> GetMapDataTile()
        {
            int currentLodLevel = LevelDetailCamera.instance.GetCurrentLodLevel();
            if (currentLodLevel == 2 || currentLodLevel == 3 || currentLodLevel == 4)
            {
                return this.m_current_map_data_tile_detail;
            }
            return this.m_current_map_data_tile_brief;
        }

        private List<MapObjectData> GetTilePlaneDataInRange(Rect rect)
        {
            List<MapObjectData> list = new List<MapObjectData>();
            foreach (TileData current in this.m_current_map_data_tile_plane.Values)
            {
                list.AddRange(current.GetMapObjDataInRange(rect));
            }
            return list;
        }

        public List<ProvinceName> GetMapProvinceNameStruct()
        {
            List<ProvinceName> list = new List<ProvinceName>();
            foreach (TileSimple current in this.m_dict_map_tile_brief.Values)
            {
                if (current.m_show_province_name)
                {
                    list.Add(new ProvinceName
                    {
                        m_pos = current.GetWorldPos(),
                        m_province_name = current.m_province_name_array[0]
                    });
                }
            }
            return list;
        }

        public string GetMapProvinceName(Vector2 pos)
        {
            float num = m_tile_width;
            string key = string.Format("{0}_{1}", Mathf.FloorToInt(pos.x / num), Mathf.FloorToInt(pos.y / num));
            TileSimple mapTileBrief = null;
            if (this.m_dict_map_tile_brief.TryGetValue(key, out mapTileBrief))
            {
                float num2 = num / 2f;
                int num3 = Mathf.FloorToInt(pos.x % num / num2) + Mathf.FloorToInt(pos.y % num / num2) * (int)Mathf.Sqrt((float)TileSimple.m_province_name_count);
                return mapTileBrief.m_province_name_array[num3];
            }
            return string.Empty;
        }
        public int GetMapZoneLevel(Vector2 pos)
        {
            string ProvinceName = GetMapProvinceName(pos);
            int zone = 1;
            if (int.TryParse(ProvinceName, out zone))
            {
                zone = zone % 300200;
                if (zone >= 0 && zone <= 5)
                {
                    zone = 1;
                }
                else if (zone > 5 && zone <= 8)
                {
                    zone = 2;
                }
                else
                {
                    zone = 3;
                }
            }
        
            return zone;
        }
        public int GetMapZone(Vector2 pos)
        {
            string ProvinceName = GetMapProvinceName(pos);
            int zone = 1;
            if (int.TryParse(ProvinceName, out zone))
            {
                zone = zone % 300200;
                zone++;
            }

            return zone;
        }


        private Vector2 GetCenterByPos(Vector2 pos, float tile_width)
        {
            float num = tile_width / 2f;
            return new Vector2((float)((int)(pos.x / tile_width)) * tile_width + num, (float)((int)(pos.y / tile_width)) * tile_width + num);
        }

        public void UpdateTile(Vector2 pos)
        {
            Vector2 centerByPos = GetCenterByPos(pos, m_piece_width);
            if (centerByPos != this.m_current_tile_center || this.m_force_update_once)
            {
                int x = (int)(centerByPos.x / this.m_tile_width);
                int y = (int)(centerByPos.y / this.m_tile_width);
                int currentLodLevel = LevelDetailCamera.instance.GetCurrentLodLevel();
                this.ReadData(x, y, currentLodLevel);
                this.m_current_tile_center = centerByPos;
            }
        }

        private Vector2 m_current_piece_center = Vector2.zero;

        private Vector2 m_current_large_piece_center = Vector2.zero;

        private Vector2 m_current_dynamic_piece_center = Vector2.zero;

        private MapCellDynamic m_dynamic_piece;

        private Transform m_land_root;

        private float m_piece_width = 45f;

        private float m_piece_plane_width = 90f;

        private string m_land_root_path = "SceneObject/land_root";

        private Dictionary<int, MapCellSmall> m_map_piece = new Dictionary<int, MapCellSmall>();

        private Dictionary<int, MapCellLarge> m_map_piece_large = new Dictionary<int, MapCellLarge>();

        private List<int> m_to_be_remove_key = new List<int>();


        private void DoUpdatePiece(int x, int y, bool reset_all_tile)
        {
            this.m_to_be_remove_key.Clear();
            foreach (int current in this.m_map_piece.Keys)
            {
                if (reset_all_tile || (current != this.GetPieceKey(x - 1, y - 1) && current != this.GetPieceKey(x, y - 1) && current != this.GetPieceKey(x + 1, y - 1) && current != this.GetPieceKey(x - 1, y) && current != this.GetPieceKey(x, y) && current != this.GetPieceKey(x + 1, y) && current != this.GetPieceKey(x - 1, y + 1) && current != this.GetPieceKey(x, y + 1) && current != this.GetPieceKey(x + 1, y + 1)))
                {
                    this.m_to_be_remove_key.Add(current);
                }
            }
            for (int i = 0; i < this.m_to_be_remove_key.Count; i++)
            {
                int key = this.m_to_be_remove_key[i];
                if (this.m_map_piece[key] != null)
                {
                    this.m_map_piece[key].DestroySelf();
                }
                this.m_map_piece.Remove(key);
            }
            this.CreateNewPiece(x, y);
            this.CreateNewPiece(x - 1, y);
            this.CreateNewPiece(x + 1, y);
            this.CreateNewPiece(x, y + 1);
            this.CreateNewPiece(x, y - 1);
            this.CreateNewPiece(x - 1, y - 1);
            this.CreateNewPiece(x + 1, y - 1);
            this.CreateNewPiece(x - 1, y + 1);
            this.CreateNewPiece(x + 1, y + 1);
        }

        private void DoUpdateDynamicPiece(int x, int y, bool reset_all_tile, int lod)
        {
            if (this.m_dynamic_piece == null)
            {
                this.m_dynamic_piece = new GameObject($"dynamic_piece_{x}_{y}")
                {
                    transform =
                {
                    parent = this.GetLandRoot()
                }
                }.AddComponent<MapCellDynamic>();
            }
            if (reset_all_tile)
            {
                this.m_dynamic_piece.DestroySelf();
            }
            this.m_dynamic_piece.UpdatePiece(lod, m_tile_width, GetMapDataTile());
        }

        private void DoUpdateLargePiece(int x, int y, bool reset_all_tile)
        {
            List<int> list = new List<int>();
            foreach (int current in this.m_map_piece_large.Keys)
            {
                if (reset_all_tile || (current != this.GetPieceKey(x - 1, y - 1) && current != this.GetPieceKey(x, y - 1) && current != this.GetPieceKey(x + 1, y - 1) && current != this.GetPieceKey(x - 1, y) && current != this.GetPieceKey(x, y) && current != this.GetPieceKey(x + 1, y) && current != this.GetPieceKey(x - 1, y + 1) && current != this.GetPieceKey(x, y + 1) && current != this.GetPieceKey(x + 1, y + 1)))
                {
                    list.Add(current);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                int key = list[i];
                if (this.m_map_piece_large[key] != null)
                {
                    this.m_map_piece_large[key].DestroySelf();
                }
                this.m_map_piece_large.Remove(key);
            }
            this.CreateNewLargePiece(x, y);
            this.CreateNewLargePiece(x - 1, y);
            this.CreateNewLargePiece(x + 1, y);
            this.CreateNewLargePiece(x, y + 1);
            this.CreateNewLargePiece(x, y - 1);
            this.CreateNewLargePiece(x - 1, y - 1);
            this.CreateNewLargePiece(x + 1, y - 1);
            this.CreateNewLargePiece(x - 1, y + 1);
            this.CreateNewLargePiece(x + 1, y + 1);
        }

        private Transform GetLandRoot()
        {
            if (this.m_land_root == null)
            {
                this.m_land_root = GameObject.Find(this.m_land_root_path).transform;
            }
            return this.m_land_root;
        }

        private void CreateNewLargePiece(int x, int y)
        {
            int pieceKey = this.GetPieceKey(x, y);
            float num = this.m_piece_plane_width / 2f;
            if (!this.m_map_piece_large.ContainsKey(pieceKey))
            {
                GameObject gameObject = new GameObject();
                gameObject.name = $"MapPieceLarge_{x}_{y}";
                float num2 = this.m_piece_plane_width / 2f;
                gameObject.transform.position = new Vector3((float)x * this.m_piece_plane_width + num2, 0f, (float)y * this.m_piece_plane_width + num2);
                gameObject.transform.SetParent(this.GetLandRoot(), true);
                MapCellLarge mapPieceLarge = gameObject.AddComponent<MapCellLarge>();
                this.m_map_piece_large.Add(pieceKey, mapPieceLarge);

                Rect piect_rect = mapPieceLarge.GetRect(m_piece_plane_width);
                var listData = GetTilePlaneDataInRange(piect_rect);
                mapPieceLarge.AddObject(listData);
                mapPieceLarge.Refresh();
            }
        }

        private void CreateNewPiece(int x, int y)
        {
            int pieceKey = this.GetPieceKey(x, y);
            float num = this.m_piece_width / 2f;
            if (!this.m_map_piece.ContainsKey(pieceKey))
            {
                GameObject gameObject = new GameObject();
                gameObject.name = $"MapPiece_{x}_{y}";
                gameObject.transform.position = new Vector3((float)x * this.m_piece_width + num, 0f, (float)y * this.m_piece_width + num);
                gameObject.transform.SetParent(this.GetLandRoot(), true);
                MapCellSmall mapPiece = gameObject.AddComponent<MapCellSmall>();
                this.m_map_piece.Add(pieceKey, mapPiece);

                mapPiece.SetInCityRangeFun(IsInCityRange);

                Rect piect_rect = mapPiece.GetRect(m_piece_width);
                var listData = GetDataInRange(piect_rect);
                mapPiece.AddObject(listData);
                mapPiece.Refresh();
            }
        }

        private int GetPieceKey(int x, int y)
        {
            return x + 1 + (y + 1 << 8);
        }

        public void UpdatePiece(Vector2 pos)
        {
            Vector2 centerByPos = GetCenterByPos(pos, this.m_piece_plane_width);
            if (centerByPos != this.m_current_large_piece_center || this.m_force_update_once)
            {
                int x = (int)(centerByPos.x / this.m_piece_plane_width);
                int y = (int)(centerByPos.y / this.m_piece_plane_width);
                this.DoUpdateLargePiece(x, y, this.m_force_update_once);
                this.m_current_large_piece_center = centerByPos;
            }
            Vector2 centerByPos2 = GetCenterByPos(pos, this.m_piece_width);
            if (centerByPos2 != this.m_current_piece_center || this.m_force_update_once)
            {
                int x2 = (int)(centerByPos2.x / this.m_piece_width);
                int y2 = (int)(centerByPos2.y / this.m_piece_width);
                this.DoUpdatePiece(x2, y2, this.m_force_update_once);
                this.m_current_piece_center = centerByPos2;
            }
            int currentLodLevel = LevelDetailCamera.instance.GetCurrentLodLevel();
            float tile_width = m_tile_width;
            Vector2 centerByPos3 = GetCenterByPos(pos, tile_width);
            if (centerByPos3 != this.m_current_dynamic_piece_center || this.m_force_update_once)
            {
                int x3 = (int)(centerByPos3.x / tile_width);
                int y3 = (int)(centerByPos3.y / tile_width);
                this.DoUpdateDynamicPiece(x3, y3, this.m_force_update_once, currentLodLevel);
                this.m_current_dynamic_piece_center = centerByPos3;
            }
        }

        public void UpdateGrove(float city_pos_x, float city_pos_y, float remove_grove_distance)
        {
            foreach (MapCellSmall current in this.m_map_piece.Values)
            {
                current.UpdateGrove(city_pos_x, city_pos_y, remove_grove_distance);
            }
        }

        public Dictionary<string, MapTownItem> m_city_dict = new Dictionary<string, MapTownItem>();

        private bool IsInCityRange(Vector2 pos)
        {
            foreach (MapTownItem current in m_city_dict.Values)
            {
                if (Vector2.Distance(pos, current.m_pos) <= current.m_remove_grove_distance)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddCity(string city_id, float city_pos_x, float city_pos_y, float remove_grove_distance)
        {
            Vector2 pos = new Vector2(city_pos_x, city_pos_y);
            if (!m_city_dict.ContainsKey(city_id))
            {
                m_city_dict.Add(city_id, new MapTownItem(pos, remove_grove_distance));
                UpdateGrove(city_pos_x, city_pos_y, remove_grove_distance);
            }
            else
            {
                if (m_city_dict[city_id].m_pos != pos)
                {
                    RemoveCity(city_id);
                    AddCity(city_id, city_pos_x, city_pos_y, remove_grove_distance);
                }
                
            }
        }

        public void RemoveCity(string city_id)
        {
            m_city_dict.Remove(city_id);
        }
        public void ClearCiity()
        {
            m_city_dict.Clear();
        }


        private Vector2 m_current_view_center = Vector2.zero;

        private void UpdateViewCenter(Vector2 pos)
        {
            m_current_view_center = pos;
            UpdateTile(pos);
            UpdatePiece(pos);
            if (m_force_update_once)
            {
                m_force_update_once = false;
            }
        }

        public void Update()
        {
            if (m_load_data_done && !WorldCamera.Instance().isMovingToPos)
            {
                UpdateViewCenter(WorldCamera.Instance().GetViewCenter());
            }
        }
    }
}
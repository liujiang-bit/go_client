using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using System.Xml;
using Client;

namespace IG.MapEditor
{
    public class MapEditor : EditorWindow
    {
        const float k_MouseDragThreshold = 6f;
        Vector2 m_InitialMousePosition;
        Event m_CurrentEvent;
        bool m_IsDragging;
        //bool m_IsReadyForMouseDrag = false;
        TileMap m_TileMap = null;
        //private bool m_bEnabledEdit = true;
        private Plane m_mapGridPlane;
        public string[] m_EditModeNames;
        public string[] m_TileNames;
        public string[] m_TilePaths;

        private EditMode m_editMode = EditMode.None;
        private Vector2 m_tileScrollViewPos = Vector2.zero;
        private int m_nTileType = 0;
        private int m_tile_row = 40;
        private int m_tile_col = 40;
        private int m_tile_size = 180;
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

        enum EditMode
        {
            None,
            Paint,
            Rotation,
        }

        void OnEnable()
        {
            m_EditModeNames = new string[] { "None", "Paint", "Rotate" };

            var guids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/BundleAssets/Scene/Map_landform/Tile" });

            List<string> tileNameList = new List<string>();
            List<string> tilePathList = new List<string>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                tilePathList.Add(path);
                var sName = Path.GetFileNameWithoutExtension(path);
                sName = sName.Substring(5);
                tileNameList.Add(sName);
            }
            m_TileNames = tileNameList.ToArray();
            m_TilePaths = tilePathList.ToArray();

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
        SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
            if (m_TileMap == null)
            {
                m_TileMap = GameObject.FindObjectOfType<TileMap>();
                if (m_TileMap == null)
                {
                    GameObject obj = new GameObject("TileMap");
                    m_TileMap = obj.AddComponent<TileMap>();
                }
            }
            if(m_TileMap.currentTile == null)
            {
                m_nTileType = 0;
                m_TileMap.currentTile = AssetDatabase.LoadAssetAtPath<GameObject>(m_TilePaths[m_nTileType]);
            }
            m_tile_row = m_TileMap.tile_row;
            m_tile_col = m_TileMap.tile_col;
            m_tile_size = m_TileMap.tile_size;
            if (m_editMode == EditMode.None)
            {
                LockLayer(LayerMask.NameToLayer("Default"));
            }
        }
        void OnDisable()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
            UnLockLayer(LayerMask.NameToLayer("Default"));
        }
        internal static void MenuOpenWindow()
        {
            MapEditor editor = (MapEditor)EditorWindow.GetWindow(typeof(MapEditor));
        }

        bool GetMapPos(Vector2 hitPos, out int x, out int y)
        {
            x = (int)hitPos.x / m_TileMap.tile_size;
            y = (int)hitPos.y / m_TileMap.tile_size;

            if (x < 0 || x >= m_TileMap.tile_col || y < 0 || y >= m_TileMap.tile_row)
            {
                return false;
            }
            return true;
        }
        void OnSceneGUI(SceneView sceneView)
        {
            m_CurrentEvent = Event.current;
            if (m_TileMap == null || m_TileMap.transform.childCount == 0)
                return;
            //开始绘制GUI
            Handles.BeginGUI();
            EditMode mode = (EditMode)GUI.Toolbar(new Rect(0, 50, 150, 50), (int)m_editMode, m_EditModeNames);
            if(m_editMode != mode)
            {
                if(mode == EditMode.None)
                {
                    UnLockLayer(LayerMask.NameToLayer("Default"));
                }
                else
                {
                    LockLayer(LayerMask.NameToLayer("Default"));
                }
                m_editMode = mode;
            }
            int nScrollWidth = 50 * m_TileNames.Length;
            m_tileScrollViewPos = GUI.BeginScrollView(new Rect(0, 0, Screen.width, 50), m_tileScrollViewPos, new Rect(0, 100, nScrollWidth, 50));
            int nIndex = GUI.Toolbar(new Rect(0, 100, 50* m_TileNames.Length, 50), m_nTileType, m_TileNames);

            if(m_nTileType != nIndex)
            {
                m_nTileType = nIndex;
                m_TileMap.currentTile = AssetDatabase.LoadAssetAtPath<GameObject>(m_TilePaths[m_nTileType]);
            }
            GUI.EndScrollView();
            Handles.EndGUI();
            if (Tools.current == Tool.View || m_editMode == EditMode.None || m_CurrentEvent.alt)
                return;
            Tools.current = Tool.None;
            if (m_CurrentEvent.type == EventType.KeyDown)
            {
                if (m_CurrentEvent.keyCode == KeyCode.R)
                {
                    m_editMode = EditMode.Rotation;
                    LockLayer(LayerMask.NameToLayer("Default"));
                }
                else if (m_CurrentEvent.keyCode == KeyCode.N)
                {
                    m_editMode = EditMode.None;
                    UnLockLayer(LayerMask.NameToLayer("Default"));
                }
                else if (m_CurrentEvent.keyCode == KeyCode.T)
                {
                    m_editMode = EditMode.Paint;
                    LockLayer(LayerMask.NameToLayer("Default"));
                }
            }
            else if (m_CurrentEvent.type == EventType.MouseDown && m_CurrentEvent.button == 0)
            {
                Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                float enter = 0f;
                m_mapGridPlane = new Plane(Vector3.up, new Vector3(0f, m_TileMap.transform.position.y, 0f));
                if (m_mapGridPlane.Raycast(mouseRay, out enter))
                {
                    Vector3 hitPos = mouseRay.origin + mouseRay.direction * enter;
                    int x = (int)hitPos.x / m_TileMap.tile_size;
                    int y = (int)hitPos.z / m_TileMap.tile_size;
                    if (x >= 0 && x <= m_TileMap.tile_col && y >= 0 && y <= m_TileMap.tile_row)
                    {
                        if (m_editMode == EditMode.Paint)
                        {
                            m_TileMap.ReplaceTile(x, y);
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }
                        else if (m_editMode == EditMode.Rotation)
                        {
                            m_TileMap.RotateTile(x, y);
                            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                        }
                    }
                }
                m_InitialMousePosition = m_CurrentEvent.mousePosition;
            }
        }
        void OnGUI()
        {
            if(m_TileMap == null)
            {
                return;
            }
            m_TileMap.gridColor = EditorGUILayout.ColorField("GridLineColor", m_TileMap.gridColor, GUILayout.Width(200));
            GameObject prefab = EditorGUILayout.ObjectField("Current Tile", m_TileMap.currentTile, typeof(GameObject), false) as GameObject;
            if (prefab != null && prefab.name.Length == 10 && prefab.name.Substring(0, 5).Equals("Tile_"))
            {
                m_TileMap.currentTile = prefab;
            }
            if (m_TileMap.transform.childCount == 0)
            {
                m_tile_row = EditorGUILayout.IntField("row", m_tile_row);
                m_tile_col = EditorGUILayout.IntField("col", m_tile_col);
                m_tile_size = EditorGUILayout.IntField("size", m_tile_size);
                if (GUILayout.Button("Create Map"))
                {
                    m_TileMap.CreateMap(m_tile_col, m_tile_row, m_tile_size);
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
            }
            else
            {
                EditorGUILayout.LabelField($"row:{m_TileMap.tile_row}");
                EditorGUILayout.LabelField($"col:{m_TileMap.tile_col}");
                EditorGUILayout.LabelField($"size:{m_TileMap.tile_size}");
            }
            if (GUILayout.Button("SaveMapData"))
            {
                ExportMapData();
            }
            if (GUILayout.Button("Save Tile Data"))
            {
                ExportTileData();
            }
        }
        /// <summary>
        /// 切换锁定
        /// </summary>
        public void SwichLockLayer(int layer)
        {
            Tools.lockedLayers ^= 1 << layer;
        }
        /// <summary>
        /// 设置锁定
        /// </summary>
        public void LockLayer(int layer)
        {
            Debug.Log("UnLockLayer:"+LayerMask.LayerToName(layer));
            Tools.lockedLayers |= 1 << layer;
        }

        /// <summary>
        /// 取消锁定
        /// </summary>
        public void UnLockLayer(int layer)
        {
            Tools.lockedLayers &= ~(1 << layer);
        }
        /// <summary>
        /// 判断是否锁定
        /// </summary>
        public bool IsLayerLocked(int layer)
        {
            return (Tools.lockedLayers & 1 << layer) == 1 << layer;
        }

        private void ExportMapData()
        {
            var tmxPath = Application.dataPath + $"/BundleAssets/Map/Scenes/{EditorSceneManager.GetActiveScene().name}.tmx";

            List<int> m_provinceId = new List<int>();
            string[] m_provinceNames = null;
            int[] m_provinceShow = null;
            if (File.Exists(tmxPath))
            {
                XmlDocument document = new XmlDocument();
                document.Load(tmxPath);

                // 省份信息
                var objects = document.DocumentElement.SelectNodes("objectgroup/object");
                m_provinceNames = new string[objects.Count];
                m_provinceShow = new int[objects.Count];
                foreach (XmlElement provinceNode in objects)
                {
                    float x = float.Parse(provinceNode.GetAttribute("x"));
                    float y = float.Parse(provinceNode.GetAttribute("y"));
                    float width = float.Parse(provinceNode.GetAttribute("width"));
                    float height = float.Parse(provinceNode.GetAttribute("height"));
                    x += width / 2.0f;
                    y += height / 2.0f;
                    y = (m_TileMap.tile_size * m_TileMap.tile_row) - y;
                    string nameId = string.Empty;
                    int provinceId = 0;
                    var propertys = provinceNode.SelectNodes("properties/property");
                    foreach(XmlElement property in propertys)
                    {
                        if(property.GetAttribute("name").Equals("l_nameId"))
                        {
                            nameId = property.GetAttribute("value");
                        }
                        else if (property.GetAttribute("name").Equals("shengfen"))
                        {
                            provinceId = int.Parse(property.GetAttribute("value"));
                        }
                    }
                    m_provinceNames[provinceId-1] = nameId;
                    m_provinceShow[provinceId-1] = (int)(x / m_TileMap.tile_size) + (int)(y / m_TileMap.tile_size) * m_TileMap.tile_col;
                }


                var layerNode = document.DocumentElement.SelectNodes("layer/data")[0];
                string data = layerNode.InnerText;
                var rows = data.Split('\n');
                for (int i = rows.Length - 1; i >= 0; i--)
                {
                    var row = rows[i];
                    var ids = row.Split(',');
                    foreach(var id in ids)
                    {
                        if (!id.Equals(string.Empty))
                        {
                            m_provinceId.Add(int.Parse(id) - 1);
                        }
                    }
                }
            }

            var mem = new MemoryStream();
            var writer = new BinaryWriter(mem);
            writer.Write(m_TileMap.transform.childCount);

            if (m_provinceNames == null)
            {
                foreach (Transform child in m_TileMap.transform)
                {
                    var name = child.GetChild(0).name;


                    writer.Write(name);
                    writer.Write(child.transform.localRotation.eulerAngles.y);
                    writer.Write(false);
                    writer.Write(false);
                    writer.Write(false);
                    writer.Write((byte)1);
                    writer.Write("0");
                }
            }
            else
            {

                // 所有的地块
                var tileBrief = new TileSimple[m_TileMap.tile_row * m_TileMap.tile_col];

                foreach (Transform child in m_TileMap.transform)
                {
                    var name = child.GetChild(0).name;
                    var array = child.name.Split('_');
                    int x = int.Parse(array[0]);
                    int y = int.Parse(array[1]);
                    int idx = x + y * m_TileMap.tile_row;

                    int px = x * 2;
                    int py = y * 2;
                    int providIdx = px + py * m_TileMap.tile_col * 2;

                    var brief = tileBrief[idx] = new TileSimple(x, y, name, child.transform.localRotation.eulerAngles.y, false, false, false, null, m_TileMap.tile_size);
                    brief.m_province_name_array = new string[TileSimple.m_province_name_count];
                    brief.m_province_name_array[0] = m_provinceNames[m_provinceId[providIdx]];
                    brief.m_province_name_array[1] = m_provinceNames[m_provinceId[providIdx + 1]];
                    brief.m_province_name_array[2] = m_provinceNames[m_provinceId[providIdx + m_TileMap.tile_col * 2]];
                    brief.m_province_name_array[3] = m_provinceNames[m_provinceId[providIdx + m_TileMap.tile_col * 2 + 1]];
                }

                for (int i = 0; i < m_provinceShow.Length; i++)
                {
                    int nTileIdx = m_provinceShow[i];
                    tileBrief[nTileIdx].m_show_province_name = true;
                    //Debug.Log($"{tileBrief[nTileIdx].m_province_name_array[0]}\t{nTileIdx}\t{m_provinceNames[i]}\t{nTileIdx % 40 * 2}\t{nTileIdx / 40 * 2}");
                }

                foreach (var brief in tileBrief)
                {
                    writer.Write(brief.m_tile_id);
                    writer.Write(brief.m_tile_rot);
                    writer.Write(brief.m_enable_bridge);
                    writer.Write(brief.m_river_flow_direction);
                    writer.Write(brief.m_show_province_name);

                    if (brief.m_province_name_array[0].Equals(brief.m_province_name_array[1]) && brief.m_province_name_array[0].Equals(brief.m_province_name_array[2]) && brief.m_province_name_array[0].Equals(brief.m_province_name_array[3]))
                    {
                        writer.Write((byte)1);
                        writer.Write(brief.m_province_name_array[0]);
                    }
                    else
                    {
                        writer.Write((byte)brief.m_province_name_array.Length);
                        for(int i = 0; i < brief.m_province_name_array.Length; i++)
                        {
                            writer.Write(brief.m_province_name_array[i]);
                        }
                    }
                }

            }
            var savePath = Application.dataPath + $"/BundleAssets/Map/Data/{EditorSceneManager.GetActiveScene().name}_data.bytes";
            File.WriteAllBytes(savePath, mem.ToArray());
            Debug.Log(savePath);
        }
        private void ExportTileData()
        {
            var mem = new MemoryStream();
            var writer = new BinaryWriter(mem);
            writer.Write(m_TileNames.Length);
            for (int i = 0; i < m_TileNames.Length; i++)
            {
                var tile = AssetDatabase.LoadAssetAtPath<GameObject>(m_TilePaths[i]);
                writer.Write(tile.name);

                List<string> types = new List<string>();
                List<GameObject> grove = new List<GameObject>();
                foreach (Transform child in tile.transform)
                {
                    // 判断是不是预制件的跟节点，是的话才处理
                    var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                    if (preObj == child.gameObject)
                    {
                        var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                        var prefabId = Path.GetFileNameWithoutExtension(path);
                        if (child.name.Contains("_TYPE_"))
                        {
                            types.Add(prefabId);
                        }
                        else
                        {
                            grove.Add(child.gameObject);
                        }
                    }
                }
                writer.Write(types.Count);
                foreach(var id in types)
                {
                    writer.Write(id);
                }
                writer.Write(grove.Count);
                foreach (var id in grove)
                {
                    var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(id);
                    var prefabId = Path.GetFileNameWithoutExtension(path);
                    writer.Write(prefabId);
                    writer.Write(id.transform.localPosition.x);
                    writer.Write(id.transform.localPosition.y);
                    writer.Write(id.transform.localPosition.z);
                    writer.Write(id.transform.localRotation.eulerAngles.y);
                    writer.Write(id.transform.localScale.x);
                    writer.Write(id.transform.localScale.y);
                    writer.Write(id.transform.localScale.z);
                    writer.Write((byte)1);
                }
            }
            var savePath = Application.dataPath + "/BundleAssets/Map/Data/tile_data.bytes";
            File.WriteAllBytes(savePath, mem.ToArray());
            Debug.Log(savePath);
        }
    }
}
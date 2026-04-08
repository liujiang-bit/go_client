using Skyunion;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Client
{
    public class TileMapEditor
    {
        enum EditMode
        {
            None,
            Paint,
            Rotation,
        }
        private const int Walkable = 0x01;
        private const int NotWalkable = 0x02;
        private const int Jump = 0x04;
        private const int NoBuilding = 0x08;
        private const int NoWalk = 0x10;

        private static string[] m_EditModeNames;
        public static string[] m_TileNames;
        public static string[] m_TilePaths;
        private static EditMode m_editMode = EditMode.None;
        private static Vector2 m_tileScrollViewPos = Vector2.zero;
        private static int m_nTileType = 0;
        static bool m_bEnabled = false;
        static TileMapHelper m_TileMapHelper = null;
        static bool m_bIsPVP = false;
        static Scene m_currentScene;
        static string[] tileGUID;

        static float borderOutter = 150;
        static float borderInner = 30;

        [InitializeOnLoadMethod]
        static void Init()
        {
            m_EditModeNames = new string[] { "None", "Paint", "Rotate" };
            m_bEnabled = false;
            m_TileMapHelper = null;
            m_editMode = EditMode.None;
            OnSceneChange(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChange;
        }
        static void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.path.Contains("land/Scenes/"))
            {
                m_currentScene = newScene;
                OnEnabled();
            }
            else
            {
                OnDisable();
            }
        }

        /// <summary>
        /// 切换锁定
        /// </summary>
        private static void SwichLockLayer(int layer)
        {
            Tools.lockedLayers ^= 1 << layer;
        }
        /// <summary>
        /// 设置锁定
        /// </summary>
        private static void LockLayer(int layer)
        {
            Debug.Log("UnLockLayer:" + LayerMask.LayerToName(layer));
            Tools.lockedLayers |= 1 << layer;
        }

        /// <summary>
        /// 取消锁定
        /// </summary>
        private static void UnLockLayer(int layer)
        {
            Tools.lockedLayers &= ~(1 << layer);
        }
        /// <summary>
        /// 判断是否锁定
        /// </summary>
        private static bool IsLayerLocked(int layer)
        {
            return (Tools.lockedLayers & 1 << layer) == 1 << layer;
        }

        private static GameObject FindRootObjec(string name)
        {
            var gameObjects = m_currentScene.GetRootGameObjects();
            foreach (var rootObj in gameObjects)
            {
                if (rootObj.name.Equals(name))
                {
                    return rootObj;
                }
            }

            return null;
        }
        private static void OnEnabled()
        {
            if (m_bEnabled)
            {
                SceneView.duringSceneGui -= OnSceneGUI;
            }
            SceneView.duringSceneGui += OnSceneGUI;
            m_bEnabled = true;
            m_bIsPVP = m_currentScene.name.Contains("pvp");
            var tileMap = FindRootObjec("TileMap");
            if(tileMap != null)
            {
                m_TileMapHelper = tileMap.GetComponent<TileMapHelper>();
            }
            if (m_TileMapHelper == null)
            {
                GameObject obj = new GameObject("TileMap");
                m_TileMapHelper = obj.AddComponent<TileMapHelper>();
                if (m_bIsPVP)
                {
                    m_TileMapHelper.tile_row = 5;
                    m_TileMapHelper.tile_col = 5;
                }
                else
                {
                    m_TileMapHelper.tile_row = 40;
                    m_TileMapHelper.tile_col = 40;
                }
            }
            var tilePath = m_currentScene.path.Substring(0, m_currentScene.path.IndexOf("/land/")) + "/land/Tile_lod1";
            tileGUID = AssetDatabase.FindAssets("t:Prefab", new string[] { tilePath });

            List<string> tileNameList = new List<string>();
            List<string> tilePathList = new List<string>();
            foreach (var guid in tileGUID)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                tilePathList.Add(path);
                var sName = Path.GetFileNameWithoutExtension(path);
                sName = sName.Substring(5);
                tileNameList.Add(sName);
            }
            m_TileNames = tileNameList.ToArray();
            m_TilePaths = tilePathList.ToArray();

            if (m_editMode == EditMode.None)
            {
                LockLayer(LayerMask.NameToLayer("Default"));
            }

            var quad = FindRootObjec("Quad");
            if(quad != null)
            {
                GameObject.DestroyImmediate(quad);
            }
            GameObject border = FindRootObjec("Border");
            // 创建物理
            if (border == null)
            {
                border = new GameObject();
                border.name = "Border";
            }

            float fSize = m_TileMapHelper.tile_row * m_TileMapHelper.tile_size;
            float fHaldSize = fSize / 2;
            fSize -= borderInner * 2;
            if (border.transform.childCount == 0)
            {
                int collisionLayer = LayerMask.NameToLayer("Collision");
                var center = GameObject.CreatePrimitive(PrimitiveType.Quad);
                center.transform.SetParent(border.transform, true);
                center.name = "center";
                center.transform.localPosition = new Vector3(0, 0, 0);
                center.transform.localScale = new Vector3(fSize, fSize, 1);
                center.transform.localRotation = Quaternion.identity;
                center.layer = collisionLayer;
                GameObjectUtility.SetStaticEditorFlags(center, StaticEditorFlags.NavigationStatic);
                GameObjectUtility.SetNavMeshArea(center, 0);

                CreateBorder(border.transform, "Inner", (int)fSize, (int)borderInner);
                CreateBorder(border.transform, "Outter", (int)(fSize+borderInner*2), (int)borderOutter);
            }
            border.transform.position = new Vector3(fHaldSize, 0, fHaldSize);
            border.transform.eulerAngles = new Vector3(90, 0, 0);
            border.gameObject.SetActive(false);
        }
        private static void CreateBorder(Transform parent, string name, int innerSize, int borderSize)
        {
            Transform borderTrans = parent.Find(name);
            if (borderTrans == null)
            {
                var border = new GameObject();
                border.name = name;
                borderTrans = border.transform;
                borderTrans.SetParent(parent);
            }

            float fSize = innerSize;
            float fHaldSize = innerSize / 2;

            if (borderTrans.childCount == 0)
            {
                int collisionLayer = LayerMask.NameToLayer("Collision");

                var left = GameObject.CreatePrimitive(PrimitiveType.Quad);
                left.transform.SetParent(borderTrans, true);
                left.name = "left";
                left.transform.localPosition = new Vector3(-fHaldSize - borderSize / 2, 0, 0);
                left.transform.localScale = new Vector3(borderSize, fSize + borderSize * 2, 1);
                left.transform.localRotation = Quaternion.identity;
                left.layer = collisionLayer;
                GameObjectUtility.SetStaticEditorFlags(left, StaticEditorFlags.NavigationStatic);
                GameObjectUtility.SetNavMeshArea(left, 1);

                var right = GameObject.CreatePrimitive(PrimitiveType.Quad);
                right.transform.SetParent(borderTrans, true);
                right.name = "right";
                right.transform.localPosition = new Vector3(fHaldSize + borderSize / 2 , 0, 0);
                right.transform.localScale = new Vector3(borderSize, fSize + borderSize * 2, 1);
                right.transform.localRotation = Quaternion.identity;
                right.layer = collisionLayer;
                GameObjectUtility.SetStaticEditorFlags(right, StaticEditorFlags.NavigationStatic);
                GameObjectUtility.SetNavMeshArea(right, 1);

                var top = GameObject.CreatePrimitive(PrimitiveType.Quad);
                top.transform.SetParent(borderTrans, true);
                top.name = "top";
                top.transform.localPosition = new Vector3(0, fHaldSize + borderSize / 2, 0);
                top.transform.localScale = new Vector3(fSize + borderSize * 2, borderSize, 1);
                top.transform.localRotation = Quaternion.identity;
                top.layer = collisionLayer;
                GameObjectUtility.SetStaticEditorFlags(top, StaticEditorFlags.NavigationStatic);
                GameObjectUtility.SetNavMeshArea(top, 1);

                var bottom = GameObject.CreatePrimitive(PrimitiveType.Quad);
                bottom.transform.SetParent(borderTrans, true);
                bottom.name = "bottom";
                bottom.transform.localPosition = new Vector3(0, -fHaldSize - borderSize / 2, 0);
                bottom.transform.localScale = new Vector3(fSize + borderSize * 2, borderSize, 1);
                bottom.transform.localRotation = Quaternion.identity;
                bottom.layer = collisionLayer;
                GameObjectUtility.SetStaticEditorFlags(bottom, StaticEditorFlags.NavigationStatic);
                GameObjectUtility.SetNavMeshArea(bottom, 1);
            }
        }

        private static void SetBorderNavMeshArea(string name, int areaIndex)
        {
            GameObject border = FindRootObjec("Border");
            if (border == null)
                return;

            var go = border.transform.Find(name);
            if(go != null)
            {
                foreach (Transform child in go.transform)
                {
                    GameObjectUtility.SetNavMeshArea(child.gameObject, areaIndex);
                }
            }
        }

        private static void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            m_bEnabled = false;
            UnLockLayer(LayerMask.NameToLayer("Default"));
        }
        static void ClearMap()
        {
            if (UnityEditor.EditorUtility.DisplayDialog("危险操作", "是否确认清除地图", "确认", "取消"))
            {
                while (m_TileMapHelper.transform.childCount > 0)
                {
                    Object.DestroyImmediate(m_TileMapHelper.transform.GetChild(0).gameObject);
                };
            }
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
                return;


            if (Selection.objects.Length == 1 && PrefabUtility.IsPartOfPrefabAsset(Selection.activeObject))
            {
                var prefabPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (prefabPath.Contains("land/Tile_lod1/"))
                {
                    m_TileMapHelper.currentTile = Selection.activeObject as GameObject;
                }
            }

            Handles.BeginGUI();
            if (m_TileMapHelper.transform.childCount == 0)
            {
                GUILayout.BeginVertical();
                GUILayout.BeginArea(new Rect(0, 0, 300, 300));
                GameObject prefab = EditorGUILayout.ObjectField("Base Tile", m_TileMapHelper.currentTile, typeof(GameObject), false, GUILayout.Width(300)) as GameObject;
                if (m_TileMapHelper.currentTile != prefab)
                {
                    var prefabPath = AssetDatabase.GetAssetPath(prefab);
                    if (prefabPath.Contains("land/Tile_lod1/"))
                    {
                        m_TileMapHelper.currentTile = prefab;
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    }
                }

                GUILayout.BeginArea(new Rect(0, 20, 100, 300));
                if (m_TileMapHelper.currentTile != null && GUILayout.Button("CreateMap"))
                {
                    CreateMap();
                }
                GUILayout.EndArea();
                GUILayout.EndArea();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                if (m_TileMapHelper.currentTile != null && GUILayout.Button("ClearMap"))
                {
                    ClearMap();
                }
                GUILayout.Space(10);
                if (GUILayout.Button("Camera"))
                {
                    var viewPos = new Vector3(m_TileMapHelper.tile_col * m_TileMapHelper.tile_size / 2, 0, m_TileMapHelper.tile_row * m_TileMapHelper.tile_size / 2);
                    sceneView.LookAt(viewPos, Quaternion.Euler(45, 0, 0), m_TileMapHelper.tile_row * m_TileMapHelper.tile_size * 1.14f);
                }
                if (GUILayout.Button("SaveMapData"))
                {
                    ExportMapData();
                }
                if (GUILayout.Button("Save Tile Data"))
                {
                    ExportTileData();
                }

                GUILayout.Space(10);
                if (GUILayout.Button("NavMeshMode"))
                {
                    GameObject border = FindRootObjec("Border");
                    if (border != null)
                    {
                        border.SetActive(true);
                    }
                    SetBorderNavMeshArea("Inner", 1);
                    int cl = LayerMask.NameToLayer("Collision");
                    var mfs = GameObject.FindObjectsOfType<MeshFilter>();

                    int nCount = 0;
                    foreach (var mf in mfs)
                    {
                        if (mf.gameObject.layer == cl)
                        {
                            if (mf.gameObject.name.Contains("Collision"))
                            {
                                if (mf.gameObject.layer == cl)
                                {
                                    if (GameObjectUtility.GetNavMeshArea(mf.gameObject) == 4)
                                    {
                                        mf.gameObject.GetComponent<MeshRenderer>().enabled = true;
                                    }
                                    else if (GameObjectUtility.GetNavMeshArea(mf.gameObject) == 3)
                                    {
                                        mf.gameObject.GetComponent<MeshRenderer>().enabled = false;
                                    }
                                }
                            }
                        }
                        if (nCount % 1000 == 0)
                        {
                            EditorUtility.DisplayProgressBar("NavMeshMode", $"{nCount}/{mfs.Length}", nCount / mfs.Length);
                        }
                        nCount++;
                    }
                    EditorUtility.ClearProgressBar();
                }
                GUILayout.Label("Bake NavMesh First!");
                if (GUILayout.Button("ExportWalkable"))
                {
                    ExportWalkableNavMesh();
                }
                GUILayout.Space(10);
                if (GUILayout.Button("NoBuildingMode"))
                {
                    GameObject border = FindRootObjec("Border");
                    if (border != null)
                    {
                        border.SetActive(true);
                    }

                    SetBorderNavMeshArea("Inner", 3);

                    int cl = LayerMask.NameToLayer("Collision");
                    var mfs = GameObject.FindObjectsOfType<MeshFilter>();

                    int nCount = 0;
                    foreach (var mf in mfs)
                    {
                        if (mf.gameObject.layer == cl)
                        {
                            if (mf.gameObject.name.Contains("Collision"))
                            {
                                if (mf.gameObject.layer == cl)
                                {
                                    if (GameObjectUtility.GetNavMeshArea(mf.gameObject) == 4)
                                    {
                                        mf.gameObject.GetComponent<MeshRenderer>().enabled = false;
                                    }
                                    else if (GameObjectUtility.GetNavMeshArea(mf.gameObject) == 3)
                                    {
                                        mf.gameObject.GetComponent<MeshRenderer>().enabled = true;
                                    }
                                }
                            }
                        }
                        if (nCount % 1000 == 0)
                        {
                            EditorUtility.DisplayProgressBar("NavMeshMode", $"{nCount}/{mfs.Length}", nCount / mfs.Length);
                        }
                        nCount++;
                    }
                    EditorUtility.ClearProgressBar();
                }
                GUILayout.Label("Bake NavMesh First!");
                if (GUILayout.Button("ExportNoBuilding"))
                {
                    ExportNoBuildingNavMesh();
                }
                if (GUILayout.Button("ExportBuilding"))
                {
                    ExportBuildingNavMesh();
                }
                //if (GUILayout.Button("BuildNoBuildingMesh"))
                //{
                //    var meshBuildSettings = NavMesh.GetSettingsByIndex(0);
                //    //NavMeshBuildSettings meshBuildSettings2 = new NavMeshBuildSettings();
                //    //meshBuildSettings.agentTypeID = 2;
                //    //meshBuildSettings.agentRadius = 0.5f;
                //    //meshBuildSettings.agentHeight = 2;
                //    //meshBuildSettings.agentSlope = 45;
                //    //meshBuildSettings.agentClimb = 0.4f;
                //    //meshBuildSettings.minRegionArea = 2;
                //    //meshBuildSettings.overrideVoxelSize = false;
                //    //meshBuildSettings.voxelSize = 5/30.0f;
                //    //meshBuildSettings.tileSize = 256;
                //    //List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
                //    //var mfs = FindRootObjec("Border").GetComponentsInChildren<MeshFilter>();
                //    //foreach(var mf in mfs)
                //    //{
                //    //    NavMeshBuildSource source = new NavMeshBuildSource();
                //    //    source.shape = NavMeshBuildSourceShape.Mesh;
                //    //    source.sourceObject = mf.sharedMesh;
                //    //    source.transform = mf.transform.localToWorldMatrix;
                //    //    source.area = GameObjectUtility.GetNavMeshArea(mf.gameObject);
                //    //    sources.Add(source);
                //    //}
                //    //float fSize = m_TileMapHelper.tile_row * m_TileMapHelper.tile_size;
                //    //float fHaldSize = fSize / 2;
                //    //NavMeshBuilder.BuildNavMeshData(meshBuildSettings, sources, new Bounds(new Vector3(fHaldSize, 0, fHaldSize), new Vector3(fSize + (borderSize - borderInner) * 2, 8, fSize + (borderSize - borderInner) * 2)), Vector3.zero, Quaternion.identity);
                //}

                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.MaxWidth(300));
                EditMode mode = (EditMode)GUILayout.Toolbar((int)m_editMode, m_EditModeNames);
                if (m_editMode != mode)
                {
                    if (mode == EditMode.None)
                    {
                        UnLockLayer(LayerMask.NameToLayer("Default"));
                    }
                    else
                    {
                        LockLayer(LayerMask.NameToLayer("Default"));
                    }
                    m_editMode = mode;
                }
                GameObject prefab = EditorGUILayout.ObjectField("Base Tile", m_TileMapHelper.currentTile, typeof(GameObject), false) as GameObject;
                if (m_TileMapHelper.currentTile != prefab)
                {
                    var prefabPath = AssetDatabase.GetAssetPath(prefab);
                    if (prefabPath.Contains("land/Tile_lod1/"))
                    {
                        m_TileMapHelper.currentTile = prefab;
                        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                    }
                }
                GUILayout.EndVertical();


                float scale = Shader.GetGlobalFloat("_TreeScale");
                float newScale = GUILayout.HorizontalSlider(scale, 0.1f, 5, GUILayout.Width(100));
                if (newScale != scale)
                {
                    Shader.SetGlobalFloat("_TreeScale", newScale);
                }
                GUILayout.Label($"TreeScale:{newScale.ToString("f2")}");

                GUILayout.EndHorizontal();

                var m_CurrentEvent = Event.current;
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
                    var m_mapGridPlane = new Plane(Vector3.up, new Vector3(0f, m_TileMapHelper.transform.position.y, 0f));
                    if (m_mapGridPlane.Raycast(mouseRay, out enter))
                    {
                        Vector3 hitPos = mouseRay.origin + mouseRay.direction * enter;
                        int x = (int)hitPos.x / m_TileMapHelper.tile_size;
                        int y = (int)hitPos.z / m_TileMapHelper.tile_size;
                        if (x >= 0 && x <= m_TileMapHelper.tile_col && y >= 0 && y <= m_TileMapHelper.tile_row)
                        {
                            if (m_editMode == EditMode.Paint)
                            {
                                ReplaceTile(x, y);
                                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                            }
                            else if (m_editMode == EditMode.Rotation)
                            {
                                RotateTile(x, y);
                                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                            }
                        }
                    }
                }
            }
            //GUILayout.Label(sceneView.size.ToString());
            Handles.EndGUI();
            SceneView.RepaintAll();
        }
        static void ReplaceTile(int x, int y)
        {
            var tile = m_TileMapHelper.transform.Find($"{x}_{y}");
            var name = Path.GetFileNameWithoutExtension(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(tile));
            if (name.Equals(m_TileMapHelper.currentTile.name))
            {
                return;
            }
            Object.DestroyImmediate(tile.gameObject);
            var tileObj = PrefabUtility.InstantiatePrefab(m_TileMapHelper.currentTile, m_TileMapHelper.transform) as GameObject;
            tileObj.name = $"{x}_{y}";
            tileObj.transform.position = new Vector3(x * m_TileMapHelper.tile_size + m_TileMapHelper.tile_size / 2, 0, y * m_TileMapHelper.tile_size + m_TileMapHelper.tile_size / 2);

        }
        static void RotateTile(int x, int y)
        {
            var tile = m_TileMapHelper.transform.Find($"{x}_{y}");
            if (tile)
            {
                tile.transform.Rotate(Vector3.up, 90);
            }
        }

        static void CreateMap()
        {
            while(m_TileMapHelper.transform.childCount > 0)
            {
                Object.DestroyImmediate(m_TileMapHelper.transform.GetChild(0).gameObject);
            };
            var tile_row = m_TileMapHelper.tile_row;
            var tile_col = m_TileMapHelper.tile_col;
            var tile_size = m_TileMapHelper.tile_size;
            for (int i = 0; i < tile_row; i++)
            {
                for (int j = 0; j < tile_col; j++)
                {
                    var tile = PrefabUtility.InstantiatePrefab(m_TileMapHelper.currentTile, m_TileMapHelper.transform) as GameObject;
                    tile.name = $"{j}_{i}";
                    tile.transform.position = new Vector3(j * tile_size + tile_size / 2, 0, i * tile_size + tile_size / 2);
                }
            }
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        private static void ExportMapData()
        {
            string landPath = Path.GetFullPath(Path.GetDirectoryName(m_currentScene.path) + "/../");
            var tmxPath = Path.Combine(landPath, "TileMap", m_currentScene.name + ".tmx");
            Debug.Log(tmxPath);
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
                    y = (m_TileMapHelper.tile_size * m_TileMapHelper.tile_row) - y;
                    string nameId = string.Empty;
                    int provinceId = 0;
                    var propertys = provinceNode.SelectNodes("properties/property");
                    foreach (XmlElement property in propertys)
                    {
                        if (property.GetAttribute("name").Equals("l_nameId"))
                        {
                            nameId = property.GetAttribute("value");
                        }
                        else if (property.GetAttribute("name").Equals("shengfen"))
                        {
                            provinceId = int.Parse(property.GetAttribute("value"));
                        }
                    }
                    m_provinceNames[provinceId - 1] = nameId;
                    m_provinceShow[provinceId - 1] = (int)(x / m_TileMapHelper.tile_size) + (int)(y / m_TileMapHelper.tile_size) * m_TileMapHelper.tile_col;
                }


                var layerNode = document.DocumentElement.SelectNodes("layer/data")[0];
                string data = layerNode.InnerText;
                data = data.Replace("\r\n", "\n");
                var rows = data.Split('\n');
                for (int i = rows.Length - 1; i >= 0; i--)
                {
                    var row = rows[i];
                    var ids = row.Split(',');
                    foreach (var id in ids)
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
            writer.Write(m_TileMapHelper.transform.childCount);
            writer.Write(m_TileMapHelper.tile_col);

            List<KeyValuePair<int, Transform>> tiles = new List<KeyValuePair<int, Transform>>();
            foreach (Transform child in m_TileMapHelper.transform)
            {
                var values = child.name.Split('_');
                int value = int.Parse(values[0]) + int.Parse(values[1]) * m_TileMapHelper.tile_col;
                tiles.Add(new KeyValuePair<int, Transform>(value, child));
            }
            tiles.Sort((left, right) =>
            {
                return left.Key.CompareTo(right.Key);
            });

            if (m_provinceNames == null)
            {
                foreach (var elm in tiles)
                {
                    Transform child = elm.Value;
                    var name = Path.GetFileNameWithoutExtension(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child));
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
                var tileBrief = new TileSimple[m_TileMapHelper.tile_row * m_TileMapHelper.tile_col];

                foreach (var elm in tiles)
                {
                    Transform child = elm.Value;
                    var name = Path.GetFileNameWithoutExtension(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child));
                    var array = child.name.Split('_');
                    int x = int.Parse(array[0]);
                    int y = int.Parse(array[1]);
                    int idx = x + y * m_TileMapHelper.tile_row;

                    int px = x * 2;
                    int py = y * 2;
                    int providIdx = px + py * m_TileMapHelper.tile_col * 2;

                    var brief = tileBrief[idx] = new TileSimple(x, y, name, child.transform.localRotation.eulerAngles.y, false, false, false, null, m_TileMapHelper.tile_size);
                    brief.m_province_name_array = new string[TileSimple.m_province_name_count];
                    brief.m_province_name_array[0] = m_provinceNames[m_provinceId[providIdx]];
                    brief.m_province_name_array[1] = m_provinceNames[m_provinceId[providIdx + 1]];
                    brief.m_province_name_array[2] = m_provinceNames[m_provinceId[providIdx + m_TileMapHelper.tile_col * 2]];
                    brief.m_province_name_array[3] = m_provinceNames[m_provinceId[providIdx + m_TileMapHelper.tile_col * 2 + 1]];
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
                        for (int i = 0; i < brief.m_province_name_array.Length; i++)
                        {
                            writer.Write(brief.m_province_name_array[i]);
                        }
                    }
                }

            }
            var mapDataDir = landPath + "/MapData";
            if(!Directory.Exists(mapDataDir))
            {
                Directory.CreateDirectory(mapDataDir);
            }
            var savePath = mapDataDir + "/" + m_currentScene.name + "_data.bytes";
            File.WriteAllBytes(savePath, mem.ToArray());
            Debug.Log(savePath);
            AssetDatabase.Refresh();
        }
        private static void ExportTileData()
        {
            string landPath = Path.GetFullPath(Path.GetDirectoryName(m_currentScene.path) + "/../");
            var mem = new MemoryStream();
            var writer = new BinaryWriter(mem);
            writer.Write(tileGUID.Length);
            for (int i = 0; i < tileGUID.Length; i++)
            {
                var tilePath = AssetDatabase.GUIDToAssetPath(tileGUID[i]);
                var tile = AssetDatabase.LoadAssetAtPath<GameObject>(tilePath);
                writer.Write(tile.name);

                List<GameObject> types = new List<GameObject>();
                List<GameObject> grove = new List<GameObject>();
                foreach (Transform child in tile.transform)
                {
                    // 判断是不是预制件的跟节点，是的话才处理
                    var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                    if (preObj == child.gameObject)
                    {
                        if (child.name.Contains("_TYPE_"))
                        {
                            types.Add(child.gameObject);
                        }
                        else
                        {
                            grove.Add(child.gameObject);
                        }
                    }
                }
                writer.Write(types.Count);
                foreach (var id in types)
                {
                    var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(id);
                    var prefabId = Path.GetFileNameWithoutExtension(path);
                    writer.Write(prefabId);
                    writer.Write(id.transform.localEulerAngles.y);
                    writer.Write(id.transform.localScale.x);
                    writer.Write(id.transform.localScale.y);
                    writer.Write(id.transform.localScale.z);
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
            var mapDataDir = landPath + "/MapData";
            if (!Directory.Exists(mapDataDir))
            {
                Directory.CreateDirectory(mapDataDir);
            }
            var savePath = mapDataDir + "/" + "tile_data.bytes";
            File.WriteAllBytes(savePath, mem.ToArray());
            Debug.Log(savePath);
            AssetDatabase.Refresh();
        }
        public static void ExportWalkableNavMesh()
        {
            Debug.Log("ExportWalkableNavMesh");

            string landPath = Path.GetFullPath(Path.GetDirectoryName(m_currentScene.path) + "/../");
            string navmeshDir = Path.Combine(landPath, "NavMesh");
            if (!Directory.Exists(navmeshDir))
            {
                Directory.CreateDirectory(navmeshDir);
            }
            string sceneName = SceneManager.GetActiveScene().name;
            string walkablePath = Path.Combine(navmeshDir, sceneName + "_Walkable_NavMesh.obj");
            //Unity2017 API
            UnityEngine.AI.NavMeshTriangulation navMeshTriangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
            StreamWriter swWalkable = new StreamWriter(walkablePath);
            //顶点  
            for (int i = 0; i < navMeshTriangulation.vertices.Length; i++)
            {
                swWalkable.WriteLine("v  " + -navMeshTriangulation.vertices[i].x + " " + /*navMeshTriangulation.vertices[i].y*/0 + " " + navMeshTriangulation.vertices[i].z);
            }
            swWalkable.WriteLine("g navmesh");//组名称
            //索引  
            int nTrig = 0;
            for (int i = 0; i < navMeshTriangulation.indices.Length;)
            {
                //obj文件中顶点索引是从1开始
                // 可行走和桥面
                if (navMeshTriangulation.areas[nTrig] == 0 || navMeshTriangulation.areas[nTrig] == 5)
                {
                    swWalkable.WriteLine("f " + (navMeshTriangulation.indices[i] + 1) + " " + (navMeshTriangulation.indices[i + 2] + 1) + " " + (navMeshTriangulation.indices[i + 1] + 1));
                }
                i = i + 3;
                nTrig++;
            }
            swWalkable.Flush();
            swWalkable.Close();

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Debug.Log(string.Format("Verts:{0}  Tris:{1}", navMeshTriangulation.vertices.Length, navMeshTriangulation.indices.Length / 3));
            Debug.Log("ExportNavMesh Successed");
        }
        public static void ExportNoBuildingNavMesh()
        {
            Debug.Log("ExportNoBuildingNavMesh");

            string landPath = Path.GetFullPath(Path.GetDirectoryName(m_currentScene.path) + "/../");
            string navmeshDir = Path.Combine(landPath, "NavMesh");
            if (!Directory.Exists(navmeshDir))
            {
                Directory.CreateDirectory(navmeshDir);
            }
            string sceneName = SceneManager.GetActiveScene().name;
            string noBuildingPath = Path.Combine(navmeshDir, sceneName + "_NoBuilding_NavMesh.obj");
            //Unity2017 API
            UnityEngine.AI.NavMeshTriangulation navMeshTriangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
            StreamWriter swNoBuilding = new StreamWriter(noBuildingPath);
            //顶点  
            for (int i = 0; i < navMeshTriangulation.vertices.Length; i++)
            {
                swNoBuilding.WriteLine("v  " + -navMeshTriangulation.vertices[i].x + " " + /*navMeshTriangulation.vertices[i].y*/0 + " " + navMeshTriangulation.vertices[i].z);
            }
            swNoBuilding.WriteLine("g navmesh");//组名称
            //索引  
            int nTrig = 0;
            for (int i = 0; i < navMeshTriangulation.indices.Length;)
            {
                //obj文件中顶点索引是从1开始
                if (navMeshTriangulation.areas[nTrig] != 0)
                {
                    swNoBuilding.WriteLine("f " + (navMeshTriangulation.indices[i] + 1) + " " + (navMeshTriangulation.indices[i + 2] + 1) + " " + (navMeshTriangulation.indices[i + 1] + 1));
                }
                i = i + 3;
                nTrig++;
            }
            swNoBuilding.Flush();
            swNoBuilding.Close();

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Debug.Log(string.Format("Verts:{0}  Tris:{1}", navMeshTriangulation.vertices.Length, navMeshTriangulation.indices.Length / 3));
            Debug.Log("ExportNoBuildingNavMesh Successed");
        }
        public static void ExportBuildingNavMesh()
        {
            Debug.Log("ExportBuildingNavMesh");

            string landPath = Path.GetFullPath(Path.GetDirectoryName(m_currentScene.path) + "/../");
            string navmeshDir = Path.Combine(landPath, "NavMesh");
            if (!Directory.Exists(navmeshDir))
            {
                Directory.CreateDirectory(navmeshDir);
            }
            string sceneName = SceneManager.GetActiveScene().name;
            string noBuildingPath = Path.Combine(navmeshDir, sceneName + "_Building_NavMesh.obj");
            //Unity2017 API
            UnityEngine.AI.NavMeshTriangulation navMeshTriangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
            StreamWriter swNoBuilding = new StreamWriter(noBuildingPath);
            //顶点  
            for (int i = 0; i < navMeshTriangulation.vertices.Length; i++)
            {
                swNoBuilding.WriteLine("v  " + -navMeshTriangulation.vertices[i].x + " " + /*navMeshTriangulation.vertices[i].y*/0 + " " + navMeshTriangulation.vertices[i].z);
            }
            swNoBuilding.WriteLine("g navmesh");//组名称
            //索引  
            int nTrig = 0;
            for (int i = 0; i < navMeshTriangulation.indices.Length;)
            {
                //obj文件中顶点索引是从1开始
                if (navMeshTriangulation.areas[nTrig] == 0)
                {
                    swNoBuilding.WriteLine("f " + (navMeshTriangulation.indices[i] + 1) + " " + (navMeshTriangulation.indices[i + 2] + 1) + " " + (navMeshTriangulation.indices[i + 1] + 1));
                }
                i = i + 3;
                nTrig++;
            }
            swNoBuilding.Flush();
            swNoBuilding.Close();

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Debug.Log(string.Format("Verts:{0}  Tris:{1}", navMeshTriangulation.vertices.Length, navMeshTriangulation.indices.Length / 3));
            Debug.Log("ExportBuildingNavMesh Successed");
        }
    }
}
using Skyunion;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

using UnityEngine;

namespace Client
{
    public class TileEditor
    {
        static bool m_bEnabled = false;
        [InitializeOnLoadMethod]
        static void Init()
        {
            m_bEnabled = false;
            if(UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                OnPrefabStageOpend(UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage());
            }
            UnityEditor.SceneManagement.PrefabStage.prefabStageOpened += OnPrefabStageOpend;
            UnityEditor.SceneManagement.PrefabStage.prefabStageClosing += OnPrefabStageClosing;
        }
        static void OnPrefabStageOpend(UnityEditor.SceneManagement.PrefabStage stage)
        {
            if(stage.prefabAssetPath.Contains("land/Tile_lod0/"))
            {
                SceneView.duringSceneGui += OnSceneGUI;
                m_bEnabled = true;
            }
        }
        static void OnPrefabStageClosing(UnityEditor.SceneManagement.PrefabStage stage)
        {
            if (m_bEnabled)
            {
                SceneView.duringSceneGui -= OnSceneGUI;
            }
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            bool reOpenPrefab = false;
            Handles.BeginGUI();

            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            List<string> list = new List<string>();
            int nSelectedIndex = 0;
            for (int i = 0; i < prefabStage.prefabContentsRoot.transform.childCount; i++)
            {
                Transform child = prefabStage.prefabContentsRoot.transform.GetChild(i);
                list.Add(child.name);
                if (child.gameObject.activeInHierarchy)
                {
                    nSelectedIndex = i;
                }
            }

            int nIndex = EditorGUI.Popup(new Rect(5, 8, 100, 20), nSelectedIndex, list.ToArray());
            if (nIndex != nSelectedIndex)
            {
                for (int i = 0; i < prefabStage.prefabContentsRoot.transform.childCount; i++)
                {
                    Transform child = prefabStage.prefabContentsRoot.transform.GetChild(i);
                    child.gameObject.SetActive(nIndex == i);
                }
                nSelectedIndex = nIndex;

                float fileOfView = 30.0f;
                float fHeight = 14.14f;
                switch (nIndex)
                {
                    case 0:
                        {
                            fileOfView = 7.5f;
                        }
                        break;
                    case 1:
                        {
                            fHeight = 39;
                        }
                        break;
                    case 2:
                        {
                            fHeight = 77;
                        }
                        break;
                    case 3:
                        {
                            fHeight = 203;
                        }
                        break;
                    case 4:
                        {
                            fHeight = 504;
                        }
                        break;
                    case 5:
                        {
                            fHeight = 1081;
                        }
                        break;
                }
                //sceneView.LookAt(Vector3.zero, Quaternion.Euler(45, 0, 0), (fHeight + 20) * 1.414f);
                //sceneView.cameraSettings.fieldOfView = fileOfView;
            }

            GUILayout.BeginArea(new Rect(120, 8, 400, 20));
            GUILayout.BeginHorizontal();

            if (GUILayout.Button($"Build", GUILayout.MaxWidth(50)))
            {
                PrefabUtility.SaveAsPrefabAsset(prefabStage.prefabContentsRoot, prefabStage.prefabAssetPath);
                BuildLod(prefabStage.prefabAssetPath);
                reOpenPrefab = true;
            }

            float scale = Shader.GetGlobalFloat("_TreeScale");
            float newScale = GUILayout.HorizontalSlider(scale, 0.1f, 5, GUILayout.Width(100));
            if (newScale != scale)
            {
                Shader.SetGlobalFloat("_TreeScale", newScale);
            }
            GUILayout.Label($"TreeScale:{newScale.ToString("f2")}");

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            Handles.EndGUI();
            SceneView.RepaintAll();
            if (reOpenPrefab)
            {
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(prefabStage.prefabAssetPath));
            }
        }

        static Dictionary<string, Sprite> LoadSprites(string path)
        {
            var sprites = AssetDatabase.LoadAllAssetsAtPath(path);
            Dictionary<string, Sprite> dicSprites = new Dictionary<string, Sprite>();
            for (int i = 0; i < sprites.Length; i++)
            {
                Sprite sprite = sprites[i] as Sprite;
                if (sprite != null)
                {
                    dicSprites.Add(sprite.name, sprite);
                }
            }
            return dicSprites;
        }

        static GameObject CreateGrove(GameObject aGo, string meshFullPath, string prefabFullPath, bool bRevert, bool bSavePrefab = true, bool bHalf = false)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv1s = new List<Vector2>();
            List<int> triangles = new List<int>();
            List<Color> colors = new List<Color>();
            var sprites = new List<SpriteRenderer>(aGo.transform.GetComponentsInChildren<SpriteRenderer>(false));
            sprites.Sort((val1, val2) =>
            {
                var pos1 = val1.transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(0, 0, -val1.size.y / 2.0f));
                var pos2 = val2.transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(0, 0, -val2.size.y / 2.0f));

                if (pos1.z > pos2.z)
                    return -1;
                if (pos1.z < pos2.z)
                    return 1;

                return 0;
            });

            int nIndex = 0;
            for (int i = 0; i < sprites.Count; i++)
            {
                if (bHalf && (i % 2 == 1))
                {
                    continue;
                }
                var render = sprites[i];
                Matrix4x4 transformMatrix = /*aGo.transform.worldToLocalMatrix * */render.transform.localToWorldMatrix;

                var center = transformMatrix.MultiplyPoint3x4(render.sprite.bounds.center);
                var centerColor = new Color(center.x / 180.0f, center.y / 180.0f, center.z / 180.0f);
                foreach (var vert in render.sprite.vertices)
                {
                    var newPos = transformMatrix.MultiplyPoint3x4(vert);
                    vertices.Add(newPos);
                    colors.Add(centerColor);
                }
                foreach (var uv in render.sprite.uv)
                {
                    uv1s.Add(uv);
                }
                if (bRevert)
                {
                    for (int j = 0; j < render.sprite.triangles.Length; j += 3)
                    {
                        triangles.Add(render.sprite.triangles[j] + nIndex);
                        triangles.Add(render.sprite.triangles[j + 2] + nIndex);
                        triangles.Add(render.sprite.triangles[j + 1] + nIndex);
                    }
                }
                else
                {
                    for (int j = 0; j < render.sprite.triangles.Length; j += 3)
                    {
                        triangles.Add(render.sprite.triangles[j] + nIndex);
                        triangles.Add(render.sprite.triangles[j + 1] + nIndex);
                        triangles.Add(render.sprite.triangles[j + 2] + nIndex);
                    }
                }
                nIndex += render.sprite.vertices.Length;
            }

            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.uv = uv1s.ToArray();
            mesh.colors = colors.ToArray();
            mesh.subMeshCount = 1;
            mesh.SetTriangles(triangles.ToArray(), 0);

            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            // ����Mesh��Ŀ¼
            var meshDir = Path.GetDirectoryName(meshFullPath);
            if (!Directory.Exists(meshDir))
            {
                Directory.CreateDirectory(meshDir);
            }

            var prefabDir = Path.GetDirectoryName(prefabFullPath);
            if (!Directory.Exists(prefabDir))
            {
                Directory.CreateDirectory(prefabDir);
            }

            var meshPath = meshFullPath.Substring(Application.dataPath.Length - "/Assets".Length + 1);

            AssetDatabase.CreateAsset(mesh, meshPath);
            AssetDatabase.Refresh();

            // ��ȡ���Ĳ���
            var helper = aGo.GetComponentInChildren<GizmosTool>();
            Material material = helper.m_treeMaterial;

            // ����Ԥ�Ƽ�
            var grovePrefab = new GameObject(Path.GetFileNameWithoutExtension(prefabFullPath));
            grovePrefab.AddComponent<ObjectPoolItem>();
            var andorningObject = new GameObject("adorning");
            andorningObject.transform.SetParent(grovePrefab.transform);
            var mesh_filter = andorningObject.AddComponent<MeshFilter>();
            mesh_filter.mesh = mesh;
            var mesh_render = andorningObject.AddComponent<MeshRenderer>();
            mesh_render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mesh_render.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            mesh_render.sharedMaterial = material;

            Debug.Log($"Trangles:{triangles.Count}");

            if (bSavePrefab)
            {
                var grovePath = prefabFullPath.Substring(Application.dataPath.Length - "/Assets".Length + 1);
                PrefabUtility.SaveAsPrefabAssetAndConnect(grovePrefab, grovePath, InteractionMode.AutomatedAction);
            }
            return grovePrefab;
        }

        [MenuItem("Assets/LodTool/BuildLod", true, 0)]
        static bool ValidateCreateLodPrefab()
        {
            string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(assetpath) && assetpath.Contains("land/Tile_lod0/"))
            {
                return PrefabUtility.IsPartOfPrefabAsset(Selection.activeObject);
            }

            return false;
        }
        [MenuItem("Assets/LodTool/BuildLod")]
        static void BuildLod()
        {
            foreach(var obj in Selection.gameObjects)
            {
                string assetpath = AssetDatabase.GetAssetPath(obj);
                BuildLod(assetpath);
            }
        }

        public static void BuildLod(string assetpath)
        {
            GameObject tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetpath);
            GameObject aGo = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
            string baseName = aGo.name;
            if (!baseName.Contains("_lod0"))
            {
                Debug.LogWarning("base lod muset containt _lod0 in back!");
            }
            else
            {
                baseName = aGo.name.Substring(0, aGo.name.Length - 5);
            }
            PrefabUtility.UnpackPrefabInstance(aGo, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

            string landFullPath = Path.GetFullPath(Path.GetDirectoryName(assetpath) + "/../");
            string landPath = "Assets" + landFullPath.Substring(Application.dataPath.Length);
            bool bRevert = false;
            if (landPath.Contains("BundleAssetsTest"))
            {
                bRevert = true;
            }

            string autoMeshDir = Path.Combine(landFullPath, "Mesh", "AutoMake");
            if (!Directory.Exists(autoMeshDir))
                Directory.CreateDirectory(autoMeshDir);
            string autoMeshMountainDir = Path.Combine(landFullPath, "Mesh", "AutoMake/Mountain");
            if (!Directory.Exists(autoMeshMountainDir))
                Directory.CreateDirectory(autoMeshMountainDir);
            string autoTileDir = Path.Combine(landFullPath, "Tile", "AutoMake");
            if (!Directory.Exists(autoTileDir))
                Directory.CreateDirectory(autoTileDir);
            string autoTileMountainDir = Path.Combine(landFullPath, "Tile", "AutoMake/Mountain");
            if (!Directory.Exists(autoTileMountainDir))
                Directory.CreateDirectory(autoTileMountainDir);

            var lod0_go = aGo.transform.Find("Lod0");
            List<int> treeRandomList = new List<int>();
            var dynamicTree1 = lod0_go.transform.Find("Dynamic");
            if (dynamicTree1 != null)
            {
                treeRandomList = GetRandom(dynamicTree1.childCount);
            }
            lod0_go.gameObject.SetActive(true);
            // Lod1 �����߼�
            {
                var preview = aGo.transform.Find("Lod1");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod1Path = Path.Combine(landPath, "Tile_lod1") + "/" + baseName + ".prefab";
                string lod1Dir = Path.Combine(landFullPath, "Tile_lod1");
                if(!Directory.Exists(lod1Dir))
                    Directory.CreateDirectory(lod1Dir);

                var lod1_go = new GameObject("Lod1");
                lod1_go.AddComponent<ObjectPoolItem>();
                lod1_go.transform.SetParent(aGo.transform);
                // 1.���������ؿ�
                var groundTrans = lod0_go.transform.Find("Ground");
                if (groundTrans != null)
                {
                    var grounds = groundTrans.GetComponentsInChildren<Transform>();
                    foreach (Transform child in grounds)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                            tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                            var ground = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                            ground.transform.SetParent(lod1_go.transform);
                            ground.transform.position = preObj.transform.position;
                            ground.transform.rotation = preObj.transform.rotation;
                            ground.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                // 2.����
                var riverTrans = lod0_go.transform.Find("River");
                if (riverTrans != null)
                {
                    foreach (Transform child in riverTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                            tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                            var river = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                            river.transform.SetParent(lod1_go.transform);
                            river.transform.position = preObj.transform.position;
                            river.transform.rotation = preObj.transform.rotation;
                            river.transform.localScale = preObj.transform.localScale;

                            if (path.Contains("land/Mountain/"))
                            {
                                var clone = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                PrefabUtility.UnpackPrefabInstance(clone, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                                var mfs = clone.GetComponentsInChildren<MeshFilter>();
                                for (int i = 0; i < mfs.Length; i++)
                                {
                                    if (mfs[i].name.Contains("Collision"))
                                    {
                                        Object.DestroyImmediate(mfs[i].gameObject);
                                    }
                                }
                                PrefabUtility.SaveAsPrefabAsset(clone, path.Replace("land/Mountain/", "land/Tile/AutoMake/Mountain/"));
                                GameObject.DestroyImmediate(clone);
                            }
                        }
                    }
                }
                // 3.����
                var lakeTrans = lod0_go.transform.Find("Lake");
                if (lakeTrans != null)
                {
                    foreach (Transform child in lakeTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                            tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                            var lake = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                            lake.transform.SetParent(lod1_go.transform);
                            lake.transform.position = preObj.transform.position;
                            lake.transform.rotation = preObj.transform.rotation;
                            lake.transform.localScale = preObj.transform.localScale;

                            if (path.Contains("land/Mountain/"))
                            {
                                var clone = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                PrefabUtility.UnpackPrefabInstance(clone, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                                var mfs = clone.GetComponentsInChildren<MeshFilter>();
                                for (int i = 0; i < mfs.Length; i++)
                                {
                                    if (mfs[i].name.Contains("Collision"))
                                    {
                                        Object.DestroyImmediate(mfs[i].gameObject);
                                    }
                                }
                                PrefabUtility.SaveAsPrefabAsset(clone, path.Replace("land/Mountain/", "land/Tile/AutoMake/Mountain/"));
                                GameObject.DestroyImmediate(clone);
                            }
                        }
                    }
                }
                // 4.Ͽ��
                var canyonTrans = lod0_go.transform.Find("Canyon");
                if (canyonTrans != null)
                {
                    foreach (Transform child in canyonTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                            tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                            var canyon = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                            canyon.transform.SetParent(lod1_go.transform);
                            canyon.transform.position = preObj.transform.position;
                            canyon.transform.rotation = preObj.transform.rotation;
                            canyon.transform.localScale = preObj.transform.localScale;

                            if (path.Contains("land/Mountain/"))
                            {
                                var clone = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                PrefabUtility.UnpackPrefabInstance(clone, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                                var mfs = clone.GetComponentsInChildren<MeshFilter>();
                                for (int i = 0; i < mfs.Length; i++)
                                {
                                    if (mfs[i].name.Contains("Collision"))
                                    {
                                        Object.DestroyImmediate(mfs[i].gameObject);
                                    }
                                }
                                PrefabUtility.SaveAsPrefabAsset(clone, path.Replace("land/Mountain/", "land/Tile/AutoMake/Mountain/"));
                                GameObject.DestroyImmediate(clone);
                            }
                        }
                    }
                }
                // 5.ɽ��
                string[] mountainNames = { "Mountain", "Mountain_Lod1", "Mountain_Lod2", "Mountain_Lod3", "Mountain_Lod4", "Mountain_Lod5" };
                foreach (var name in mountainNames)
                {
                    var mountainTrans = lod0_go.transform.Find(name);
                    if (mountainTrans != null)
                    {
                        foreach (Transform child in mountainTrans)
                        {
                            var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                            if (preObj == child.gameObject)
                            {
                                var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                                tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                                var canyon = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                canyon.transform.SetParent(lod1_go.transform);
                                canyon.transform.position = preObj.transform.position;
                                canyon.transform.rotation = preObj.transform.rotation;
                                canyon.transform.localScale = preObj.transform.localScale;

                                if (path.Contains("land/Mountain/"))
                                {
                                    var clone = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                    PrefabUtility.UnpackPrefabInstance(clone, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                                    var mfs = clone.GetComponentsInChildren<MeshFilter>();
                                    for (int i = 0; i < mfs.Length; i++)
                                    {
                                        if (mfs[i].name.Contains("Collision"))
                                        {
                                            Object.DestroyImmediate(mfs[i].gameObject);
                                        }
                                    }
                                    PrefabUtility.SaveAsPrefabAsset(clone, path.Replace("land/Mountain/", "land/Tile/AutoMake/Mountain/"));
                                    GameObject.DestroyImmediate(clone);
                                }
                            }
                        }
                    }
                }
                // 6. ��̬��
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    foreach (Transform child in dynamicTree)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        var andorning = preObj.transform.GetChild(1);
                        var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(andorning);
                        tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                        var grove = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                        grove.transform.SetParent(lod1_go.transform);
                        grove.transform.position = andorning.transform.position;
                        grove.transform.rotation = preObj.transform.rotation;
                        grove.transform.localScale = preObj.transform.localScale;
                    }
                }
                // 7. ��̬������
                var staticTree = lod0_go.transform.Find("Static");
                if (staticTree != null)
                {
                    int nIndex = 0;
                    foreach (Transform child in staticTree)
                    {
                        string meshFullPath = Path.Combine(landFullPath, "Mesh", "AutoMake") + "/" + baseName + "_adorning_" + nIndex.ToString() + ".asset";
                        string prefabFullPath = Path.Combine(landFullPath, "Tile", "AutoMake") + "/" + baseName + "_adorning_" + nIndex.ToString() + ".prefab";
                        var grove = CreateGrove(child.gameObject, meshFullPath, prefabFullPath, bRevert);
                        grove.transform.SetParent(lod1_go.transform);
                        grove.transform.position = Vector3.zero;
                        grove.transform.localScale = Vector3.one;
                        nIndex++;
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod1_go, lod1Path, InteractionMode.AutomatedAction);
                lod1_go.gameObject.SetActive(false);
            }

            // Lod2 �����߼�
            {
                var preview = aGo.transform.Find("Lod2");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod2Path = Path.Combine(landPath, "Tile_lod2") + "/" + baseName + "_lod2.prefab";
                string lod2Dir = Path.Combine(landFullPath, "Tile_lod2");
                if (!Directory.Exists(lod2Dir))
                    Directory.CreateDirectory(lod2Dir);

                var lod2_go = new GameObject("Lod2");
                lod2_go.AddComponent<ObjectPoolItem>();
                lod2_go.transform.SetParent(aGo.transform);
                // 1.���������ؿ�
                var groundTrans = lod0_go.transform.Find("Ground");
                if (groundTrans != null)
                {
                    var grounds = groundTrans.GetComponentsInChildren<Transform>();
                    var lodHelper = groundTrans.GetComponent<LodTileMaterialHelper>();
                    foreach (Transform child in grounds)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                            tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                            var ground = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                            PrefabUtility.UnpackPrefabInstance(ground, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
                            var mr0 = ground.GetComponent<MeshRenderer>();
                            ground.transform.SetParent(lod2_go.transform);
                            ground.transform.position = preObj.transform.position;
                            ground.transform.rotation = preObj.transform.rotation;
                            ground.transform.localScale = preObj.transform.localScale;
                            if (lodHelper != null)
                            {
                                int nID = -1;
                                if (int.TryParse(preObj.transform.parent.name, out nID))
                                {
                                    if (lodHelper.lod2.Length > nID && lodHelper.lod2[nID] != null)
                                    {
                                        mr0.sharedMaterial = lodHelper.lod2[nID];
                                    }
                                }
                            }
                        }
                    }
                }
                var riverSprites = LoadSprites(landPath + "/Texture_lod3_4/land_form_river_lod3.png");
                var riverSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_sprite_ground_lod3.mat");
                // 2.����
                var riverTrans = lod0_go.transform.Find("River");
                if (riverTrans != null)
                {
                    foreach (Transform child in riverTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            render.sharedMaterial = riverSpriteMat;

                            Sprite sprite;
                            if (riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }

                            plane.transform.SetParent(lod2_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(preObj.transform.localScale.x, preObj.transform.localScale.z, 1);
                        }
                    }
                }
                // 3.����
                var lakeTrans = lod0_go.transform.Find("Lake");
                if (lakeTrans != null)
                {
                    foreach (Transform child in lakeTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            render.sharedMaterial = riverSpriteMat;

                            Sprite sprite;
                            if (riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }
                            plane.transform.SetParent(lod2_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(preObj.transform.localScale.x, preObj.transform.localScale.z, 1);
                        }
                    }
                }
                //// 4.Ͽ��
                //var canyonTrans = lod0_go.transform.Find("Canyon");
                //if (canyonTrans != null)
                //{
                //    foreach (Transform child in canyonTrans)
                //    {
                //        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                //        if (preObj == child.gameObject)
                //        {
                //            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                //            tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                //            var canyon = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                //            canyon.transform.SetParent(lod1_go.transform);
                //            canyon.transform.position = preObj.transform.position;
                //            canyon.transform.rotation = preObj.transform.rotation;
                //            canyon.transform.localScale = preObj.transform.localScale;
                //        }
                //    }
                //}
                // 5.ɽ��
                string[] mountainNames = { "Mountain", "Mountain_Lod2", "Mountain_Lod3", "Mountain_Lod4", "Mountain_Lod5" };
                foreach (var name in mountainNames)
                {
                    var mountainTrans = lod0_go.transform.Find(name);
                    if (mountainTrans != null)
                    {
                        foreach (Transform child in mountainTrans)
                        {
                            var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                            if (preObj == child.gameObject)
                            {
                                var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                                tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                                var canyon = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                canyon.transform.SetParent(lod2_go.transform);
                                canyon.transform.position = preObj.transform.position;
                                canyon.transform.rotation = preObj.transform.rotation;
                                canyon.transform.localScale = preObj.transform.localScale;
                                UnpackPrefabAndReplaceMeshLod(landPath, canyon, 2);
                            }
                        }
                    }
                }
                // 6. ��̬��
                var treeSprites = LoadSprites(landPath + "/Texture/map_tree_atlas.png");
                var treeSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_high_sprite_nomask.mat");
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    var treeList = treeRandomList.GetRange(0, 8);
                    foreach (var i in treeList)
                    {
                        Transform child = dynamicTree.GetChild(i);
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var andorningTrans = preObj.transform.Find("adorning/center");
                            if(andorningTrans == null)
                            {
                                andorningTrans = preObj.transform.Find("adorning").GetChild(0);
                            }
                            var andorning = andorningTrans.GetComponent<SpriteRenderer>();
                            var plane = new GameObject(preObj.name + "_plane_LOD2");
                            var render = plane.AddComponent<SpriteRenderer>();
                            plane.AddComponent<LockAngle45>();
                            render.sharedMaterial = treeSpriteMat;
                            render.sprite = andorning.sprite;
                            plane.transform.SetParent(lod2_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(45, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(2, 2, 2);
                        }
                    }
                }
                // 7. ��̬������
                var staticTree = lod0_go.transform.Find("Static");
                if (staticTree != null)
                {
                    int nIndex = 0;
                    foreach (Transform child in staticTree)
                    {
                        string meshFullPath = Path.Combine(landFullPath, "Mesh", "AutoMake") + "/" + baseName + "_adorning_half_" + nIndex.ToString() + ".asset";
                        string prefabFullPath = Path.Combine(landFullPath, "Tile", "AutoMake") + "/" + baseName + "_adorning_half" + nIndex.ToString() + ".prefab";
                        var grove = CreateGrove(child.gameObject, meshFullPath, prefabFullPath, bRevert, false, true);
                        grove.transform.SetParent(lod2_go.transform);
                        grove.transform.position = Vector3.zero;
                        grove.transform.localScale = Vector3.one;
                        nIndex++;
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod2_go, lod2Path, InteractionMode.AutomatedAction);
                lod2_go.gameObject.SetActive(false);
            }
            // Lod3 �����߼�
            {
                var preview = aGo.transform.Find("Lod3");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod3Path = Path.Combine(landPath, "Tile_lod3") + "/" + baseName + "_lod3.prefab";
                string lod3Dir = Path.Combine(landFullPath, "Tile_lod3");
                if (!Directory.Exists(lod3Dir))
                    Directory.CreateDirectory(lod3Dir);

                var lod3_go = new GameObject("Lod3");
                lod3_go.AddComponent<ObjectPoolItem>();
                lod3_go.transform.SetParent(aGo.transform);
                // 1.���������ؿ�
                var groundTrans = lod0_go.transform.Find("Plane");
                if (groundTrans != null)
                {
                    var mf0 = groundTrans.GetComponent<MeshFilter>();
                    var mr0 = groundTrans.GetComponent<MeshRenderer>();
                    var meshFilter = lod3_go.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh = mf0.sharedMesh;
                    var meshRender = lod3_go.AddComponent<MeshRenderer>();
                    meshRender.sharedMaterial = mr0.sharedMaterial;
                    meshRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    meshRender.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

                    var lodHelper = groundTrans.GetComponent<LodTileMaterialHelper>();
                    if(lodHelper != null && lodHelper.lod3 != null)
                    {
                        meshRender.sharedMaterial = lodHelper.lod3;
                    }
                }
                var riverSprites = LoadSprites(landPath + "/Texture_lod3_4/land_form_river_lod3.png");
                var riverSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_sprite_ground_lod3.mat");
                // 2.����
                var riverTrans = lod0_go.transform.Find("River");
                if (riverTrans != null)
                {
                    foreach (Transform child in riverTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            render.sharedMaterial = riverSpriteMat;

                            Sprite sprite;
                            if (riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }

                            plane.transform.SetParent(lod3_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(preObj.transform.localScale.x, preObj.transform.localScale.z, 1);
                        }
                    }
                }
                // 3.����
                var lakeTrans = lod0_go.transform.Find("Lake");
                if (lakeTrans != null)
                {
                    foreach (Transform child in lakeTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            render.sharedMaterial = riverSpriteMat;

                            Sprite sprite;
                            if (riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }
                            plane.transform.SetParent(lod3_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(preObj.transform.localScale.x, preObj.transform.localScale.z, 1);
                        }
                    }
                }
                // 4.ɽ��
                string[] mountainNames = { "Mountain", "Mountain_Lod3", "Mountain_Lod4", "Mountain_Lod5" };
                foreach (var name in mountainNames)
                {
                    var mountainTrans = lod0_go.transform.Find(name);
                    if (mountainTrans != null)
                    {
                        foreach (Transform child in mountainTrans)
                        {
                            var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                            if (preObj == child.gameObject)
                            {
                                var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                                tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                                var canyon = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                canyon.transform.SetParent(lod3_go.transform);
                                canyon.transform.position = preObj.transform.position;
                                canyon.transform.rotation = preObj.transform.rotation;
                                canyon.transform.localScale = preObj.transform.localScale;
                                UnpackPrefabAndReplaceMeshLod(landPath, canyon, 3);
                            }
                        }
                    }
                }
                // 5. ��̬��
                var treeSprites = LoadSprites(landPath + "/Texture/map_tree_atlas.png");
                var treeSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_high_sprite_nomask.mat");
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    var treeList = treeRandomList.GetRange(0, 4);
                    foreach (var i in treeList)
                    { 
                        Transform child = dynamicTree.GetChild(i);
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var andorningTrans = preObj.transform.Find("adorning/center");
                            if (andorningTrans == null)
                            {
                                andorningTrans = preObj.transform.Find("adorning").GetChild(0);
                            }
                            var andorning = andorningTrans.GetComponent<SpriteRenderer>();

                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            plane.AddComponent<LockAngle45>();
                            render.sharedMaterial = treeSpriteMat;
                            render.sprite = andorning.sprite;
                            plane.transform.SetParent(lod3_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(45, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(3, 3, 3);
                        }
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod3_go, lod3Path, InteractionMode.AutomatedAction);
                lod3_go.gameObject.SetActive(false);
            }

            // Lod4 �����߼�
            {
                var preview = aGo.transform.Find("Lod4");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod4Path = Path.Combine(landPath, "Tile_lod4") + "/" + baseName + "_lod4.prefab";
                string lod4Dir = Path.Combine(landFullPath, "Tile_lod4");
                if (!Directory.Exists(lod4Dir))
                    Directory.CreateDirectory(lod4Dir);

                var lod4_go = new GameObject("Lod4");
                lod4_go.AddComponent<ObjectPoolItem>();
                lod4_go.transform.SetParent(aGo.transform);
                // 1.���������ؿ�
                var groundTrans = lod0_go.transform.Find("Plane");
                if (groundTrans != null)
                {
                    var mf0 = groundTrans.GetComponent<MeshFilter>();
                    var mr0 = groundTrans.GetComponent<MeshRenderer>();
                    var meshFilter = lod4_go.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh = mf0.sharedMesh;
                    var meshRender = lod4_go.AddComponent<MeshRenderer>();
                    meshRender.sharedMaterial = mr0.sharedMaterial;
                    meshRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    meshRender.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

                    var lodHelper = groundTrans.GetComponent<LodTileMaterialHelper>();
                    if (lodHelper != null && lodHelper.lod4 != null)
                    {
                        meshRender.sharedMaterial = lodHelper.lod4;
                    }
                }
                var riverSprites = LoadSprites(landPath + "/Texture_lod3_4/land_form_river_lod3.png");
                var riverSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_sprite_ground_lod3.mat");
                // 2.����
                var riverTrans = lod0_go.transform.Find("River");
                if (riverTrans != null)
                {
                    foreach (Transform child in riverTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            render.sharedMaterial = riverSpriteMat;

                            Sprite sprite;
                            if (riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }

                            plane.transform.SetParent(lod4_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(preObj.transform.localScale.x, preObj.transform.localScale.z, 1);
                        }
                    }
                }
                // 3.����
                var lakeTrans = lod0_go.transform.Find("Lake");
                if (lakeTrans != null)
                {
                    foreach (Transform child in lakeTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            render.sharedMaterial = riverSpriteMat;

                            Sprite sprite;
                            if (riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }
                            plane.transform.SetParent(lod4_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(preObj.transform.localScale.x, preObj.transform.localScale.z, 1);
                        }
                    }
                }
                // 4.ʡ��ɽ��
                string[] mountainNames = { "Mountain", "Mountain_Lod4", "Mountain_Lod5" };
                foreach (var name in mountainNames)
                {
                    var mountainTrans = lod0_go.transform.Find(name);
                    if (mountainTrans != null)
                    {
                        foreach (Transform child in mountainTrans)
                        {
                            var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                            if (preObj == child.gameObject)
                            {
                                var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                                tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                                var canyon = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                canyon.transform.SetParent(lod4_go.transform);
                                canyon.transform.position = preObj.transform.position;
                                canyon.transform.rotation = preObj.transform.rotation;
                                canyon.transform.localScale = preObj.transform.localScale;
                                UnpackPrefabAndReplaceMeshLod(landPath, canyon, 4);
                            }
                        }
                    }
                }
                // 5. ��̬��
                var treeSprites = LoadSprites(landPath + "/Texture/map_tree_atlas.png");
                var treeSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_high_sprite_nomask.mat");
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    var treeList = treeRandomList.GetRange(0, 2);
                    foreach (var i in treeList)
                    { 
                        Transform child = dynamicTree.GetChild(i);
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var andorningTrans = preObj.transform.Find("adorning/center");
                            if (andorningTrans == null)
                            {
                                andorningTrans = preObj.transform.Find("adorning").GetChild(0);
                            }
                            var andorning = andorningTrans.GetComponent<SpriteRenderer>();

                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            plane.AddComponent<LockAngle45>();
                            render.sharedMaterial = treeSpriteMat;
                            render.sprite = andorning.sprite;
                            plane.transform.SetParent(lod4_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(45, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(5, 5, 5);
                        }
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod4_go, lod4Path, InteractionMode.AutomatedAction);
                lod4_go.gameObject.SetActive(false);
            }

            // Lod5 �����߼�
            {
                var preview = aGo.transform.Find("Lod5");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod5Path = Path.Combine(landPath, "Tile_lod5") + "/" + baseName + "_lod5.prefab";
                string lod5Dir = Path.Combine(landFullPath, "Tile_lod5");
                if (!Directory.Exists(lod5Dir))
                    Directory.CreateDirectory(lod5Dir);

                var lod5_go = new GameObject("Lod5");
                lod5_go.AddComponent<ObjectPoolItem>();
                lod5_go.transform.SetParent(aGo.transform);
                // 1.���������ؿ�
                var groundTrans = lod0_go.transform.Find("Plane");
                if (groundTrans != null)
                {
                    var mf0 = groundTrans.GetComponent<MeshFilter>();
                    var mr0 = groundTrans.GetComponent<MeshRenderer>();
                    var meshFilter = lod5_go.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh = mf0.sharedMesh;
                    var meshRender = lod5_go.AddComponent<MeshRenderer>();
                    meshRender.sharedMaterial = mr0.sharedMaterial;
                    meshRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    meshRender.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

                    var lodHelper = groundTrans.GetComponent<LodTileMaterialHelper>();
                    if (lodHelper != null && lodHelper.lod5 != null)
                    {
                        meshRender.sharedMaterial = lodHelper.lod5;
                    }
                }
                var riverSprites = LoadSprites(landPath + "/Texture_lod3_4/land_form_river_lod3.png");
                var riverSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_sprite_ground_lod3.mat");
                // 2.����
                var riverTrans = lod0_go.transform.Find("River");
                if (riverTrans != null)
                {
                    foreach (Transform child in riverTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            render.sharedMaterial = riverSpriteMat;

                            Sprite sprite;
                            if (riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }

                            plane.transform.SetParent(lod5_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(preObj.transform.localScale.x, preObj.transform.localScale.z, 1);
                        }
                    }
                }
                // 3.����
                var lakeTrans = lod0_go.transform.Find("Lake");
                if (lakeTrans != null)
                {
                    foreach (Transform child in lakeTrans)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            render.sharedMaterial = riverSpriteMat;

                            Sprite sprite;
                            if (riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }
                            plane.transform.SetParent(lod5_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(preObj.transform.localScale.x, preObj.transform.localScale.z, 1);
                        }
                    }
                }
                // 4.ʡ��ɽ��
                string[] mountainNames = { "Mountain", "Mountain_Lod5" };
                foreach (var name in mountainNames)
                {
                    var mountainTrans = lod0_go.transform.Find(name);
                    if (mountainTrans != null)
                    {
                        foreach (Transform child in mountainTrans)
                        {
                            var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                            if (preObj == child.gameObject)
                            {
                                var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
                                tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                                var canyon = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
                                canyon.transform.SetParent(lod5_go.transform);
                                canyon.transform.position = preObj.transform.position;
                                canyon.transform.rotation = preObj.transform.rotation;
                                canyon.transform.localScale = preObj.transform.localScale;
                                UnpackPrefabAndReplaceMeshLod(landPath, canyon, 5);
                            }
                        }
                    }
                }
                // 5. ��̬��
                var treeSprites = LoadSprites(landPath + "/Texture/map_tree_atlas.png");
                var treeSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_high_sprite_nomask.mat");
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    var treeList = treeRandomList.GetRange(0, 1);
                    foreach (var i in treeList)
                    {
                        Transform child = dynamicTree.GetChild(i);
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var andorningTrans = preObj.transform.Find("adorning/center");
                            if (andorningTrans == null)
                            {
                                andorningTrans = preObj.transform.Find("adorning").GetChild(0);
                            }
                            var andorning = andorningTrans.GetComponent<SpriteRenderer>();

                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            plane.AddComponent<LockAngle45>();
                            render.sharedMaterial = treeSpriteMat;
                            render.sprite = andorning.sprite;
                            plane.transform.SetParent(lod5_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(45, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(10, 10, 10);
                        }
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod5_go, lod5Path, InteractionMode.AutomatedAction);
            }
            lod0_go.gameObject.SetActive(false);
            PrefabUtility.SaveAsPrefabAssetAndConnect(aGo, assetpath, InteractionMode.AutomatedAction);
            Object.DestroyImmediate(aGo);
        }
        static List<int> GetRandom(int total)
        {
            List<int> randomList = new List<int>();
            for(int i = 0; i < total; i++)
            {
                randomList.Insert(Random.Range(0, randomList.Count), i);
            }
            return randomList;
        }
        static void UnpackPrefabAndReplaceMeshLod(string landRoot, GameObject mountainGo, int nLodLevel)
        {
            PrefabUtility.UnpackPrefabInstance(mountainGo, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            var meshFilters = mountainGo.GetComponentsInChildren<MeshFilter>();

            for(int i = 0; i < meshFilters.Length; i++)
            {
                if (meshFilters[i].gameObject.name.Contains("Collision"))
                {
                    if (nLodLevel > 1)
                    {
                        GameObject.DestroyImmediate(meshFilters[i].gameObject);
                        continue;
                    }
                }
                else
                {
                    var lodMeshHelper = meshFilters[i].gameObject.GetComponent<LodMeshHelper>();                    
                    if (lodMeshHelper != null)
                    {
                        if (lodMeshHelper.weight.Length > nLodLevel && lodMeshHelper.keepBorder.Length > nLodLevel)
                        {
                            if (lodMeshHelper.weight[nLodLevel] == 0)
                            {
                                GameObject.DestroyImmediate(meshFilters[i].gameObject);
                                continue;
                            }
                            else
                            {
                                meshFilters[i].sharedMesh = LoadOrCreateLodMeshFiliter(landRoot, meshFilters[i], nLodLevel, lodMeshHelper.weight[nLodLevel], lodMeshHelper.keepBorder[nLodLevel]);
                            }
                        }
                        Object.Destroy(lodMeshHelper);
                    }
                    else
                    {
                        if (meshFilters[i].gameObject.name.Contains("edge"))
                        {
                            if (nLodLevel > 3)
                            {
                                GameObject.DestroyImmediate(meshFilters[i].gameObject);
                            }
                            continue;
                        }
                        float[] aWeight = { 1.0f, 1.0f, 0.15f, 0.12f, 0.06f, 0.02f };
                        bool[] bKeepBorder = { true, true, true, true, false, false };
                        meshFilters[i].sharedMesh = LoadOrCreateLodMeshFiliter(landRoot, meshFilters[i], nLodLevel, aWeight[nLodLevel], bKeepBorder[nLodLevel]);
                    }
                }
            }
        }
        static Mesh LoadOrCreateLodMeshFiliter(string landRoot, MeshFilter meshFilter, int nLodLevel, float weight, bool keepBorder)
        {
            var meshPath = Path.Combine(landRoot, "Mesh/AutoMake/Mountain", meshFilter.sharedMesh.name + $"_lod{nLodLevel}.asset");

            if (!File.Exists(meshPath))
            {
                var ms = meshFilter.gameObject.AddComponent<MeshSimplify>();

                ms.m_fVertexAmount = weight;
                Debug.Log($"VertexCount:{ms.m_fVertexAmount} Src:{meshFilter.sharedMesh.vertexCount} Lod:{nLodLevel}");
                ms.m_bLockBorder = keepBorder;
                ms.ComputeData(false);
                ms.ComputeMesh(false);
                ms.AssignSimplifiedMesh(false);
                AssetDatabase.CreateAsset(ms.m_simplifiedMesh, meshPath);
                Mesh mesh = ms.m_simplifiedMesh;
                Object.DestroyImmediate(ms);
                return mesh;
            }
            else
            {
                return AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            }

            //if (!File.Exists(meshPath))
            //{







            //    //int tCount = mesh.triangles.Length;
            //    //float nMinWeigth = 22.0f / tCount;
            //    //if(mesh.isReadable)
            //    //{
            //    //    mesh.UploadMeshData(false);
            //    //}
            //    //mesh = mesh.MakeLODMesh( Mathf.Max(nMinWeigth, aWeight[nLodLevel]), false);


            //    //AssetDatabase.CreateAsset(mesh, meshPath);
            //    //return mesh;
            //}
            //return AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
        }
    }

}
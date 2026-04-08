using Skyunion;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    public class LodEditor
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            //SceneView.duringSceneGui += OnSceneGUI;
        }
        static void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage == null)
                return;            
            var prefabPath = prefabStage.prefabAssetPath;
            if(prefabPath.Contains("land/Tile_lod0/"))
            {
                OnTileGUI(sceneView);
            }
            else if (prefabPath.Contains("land/Grove/"))
            {
                OnGroveGUI(sceneView);
            }
        }
        static void OnTileGUI(SceneView sceneView)
        {
            bool reOpenPrefab = false;
            Handles.BeginGUI();

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            List<string> list = new List<string>();
            int nSelectedIndex = 0;
            for(int i = 0; i < prefabStage.prefabContentsRoot.transform.childCount; i++)
            {
                Transform child = prefabStage.prefabContentsRoot.transform.GetChild(i);
                list.Add(child.name);
                if(child.gameObject.activeInHierarchy)
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
                sceneView.LookAt(Vector3.zero, Quaternion.Euler(45, 0, 0), (fHeight+20) * 1.414f);
                sceneView.cameraSettings.fieldOfView = fileOfView;
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
            if(reOpenPrefab)
            {
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(prefabStage.prefabAssetPath));
            }
        }
        static void OnGroveGUI(SceneView sceneView)
        {
            bool reOpenPrefab = false;
            Handles.BeginGUI();

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            GUILayout.BeginArea(new Rect(0, 0, 400, 50));
            GUILayout.BeginHorizontal();


            if (GUILayout.Button($"Build", GUILayout.MaxWidth(50)))
            {
                PrefabUtility.SaveAsPrefabAsset(prefabStage.prefabContentsRoot, prefabStage.prefabAssetPath);
                BuildDynamicGrove(prefabStage.prefabAssetPath);
                reOpenPrefab = true;
            }
            var adorning = prefabStage.prefabContentsRoot.transform.Find("adorning");
            var grove = prefabStage.prefabContentsRoot.transform.Find(prefabStage.prefabContentsRoot.name + "_adorning");
            if (adorning!=null && GUILayout.Button($"Switch", GUILayout.MaxWidth(50)))
            {
                bool bAdorning = adorning.gameObject.activeInHierarchy;
                adorning?.gameObject.SetActive(!bAdorning);
                grove?.gameObject.SetActive(bAdorning);
            }
            
            if(grove != null)
            {
                float scale = Shader.GetGlobalFloat("_TreeScale");
                float newScale = GUILayout.HorizontalSlider(scale, 0.1f, 5, GUILayout.Width(100));
                if (newScale != scale)
                {
                    Shader.SetGlobalFloat("_TreeScale", newScale);
                }
                GUILayout.Label($"TreeScale:{newScale.ToString("f2")}");
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            //var adorning = prefabStage.prefabContentsRoot.transform.Find("adorning");
            //var grove = prefabStage.prefabContentsRoot.transform.Find(prefabStage.prefabContentsRoot.name+ "_adorning");
            //bool bAdorning = true;
            //if (adorning != null)
            //{
            //    bAdorning = adorning.gameObject.activeInHierarchy;
            //}
            //if (grove != null)
            //{
            //    if (GUI.Toggle(new Rect(5 + 50, 8, 100, 20), bAdorning, $"PrewView"))
            //    {
            //        adorning?.gameObject.SetActive(false);
            //        grove?.gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        adorning?.gameObject.SetActive(true);
            //        grove?.gameObject.SetActive(false);
            //    }
            //}

            Handles.EndGUI();
            SceneView.RepaintAll();
            if (reOpenPrefab)
            {
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(prefabStage.prefabAssetPath));
            }
        }

        static void BuildLod(string assetpath)
        {
            GameObject tempPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetpath);
            GameObject aGo = PrefabUtility.InstantiatePrefab(tempPrefab) as GameObject;
            PrefabUtility.UnpackPrefabInstance(aGo, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            string baseName = aGo.name.Substring(0, aGo.name.Length-5);

            string landFullPath = Path.GetFullPath(Path.GetDirectoryName(assetpath) + "/../");
            string landPath = "Assets" + landFullPath.Substring(Application.dataPath.Length);

            var lod0_go = aGo.transform.Find("Lod0");
            lod0_go.gameObject.SetActive(true);
            // Lod1 生成逻辑
            {
                var preview = aGo.transform.Find("Lod1");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod1Path = Path.Combine(landPath, "Tile_lod1") + "/" + baseName + "_lod1.prefab";

                var lod1_go = new GameObject("Lod1");
                lod1_go.transform.SetParent(aGo.transform);
                // 1.处理基础地块
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
                // 2.河流
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
                        }
                    }
                }
                // 3.湖泊
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
                        }
                    }
                }
                // 4.峡谷
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
                        }
                    }
                }
                // 5.山脉
                var mountainTrans = lod0_go.transform.Find("Mountain");
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
                        }
                    }
                }
                // 6. 动态树
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
                // 5. 静态树生成
                var staticTree = lod0_go.transform.Find("Static");
                if (staticTree != null)
                {
                    int nIndex = 0;
                    foreach (Transform child in staticTree)
                    {
                        string meshFullPath = Path.Combine(landFullPath, "Mesh", "AutoMake") + "/" + baseName + "_adorning_" + nIndex.ToString() + ".asset";
                        string prefabFullPath = Path.Combine(landFullPath, "Tile", "AutoMake") + "/" + baseName + "_adorning_" + nIndex.ToString() + ".prefab";
                        var grove = CreateGrove(child.gameObject, meshFullPath, prefabFullPath);
                        grove.transform.SetParent(lod1_go.transform);
                        grove.transform.position = Vector3.zero;
                        grove.transform.localScale = Vector3.one;
                        nIndex++;
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod1_go, lod1Path, InteractionMode.AutomatedAction);
                lod1_go.gameObject.SetActive(false);
            }

            // Lod2 生成逻辑
            {
                var preview = aGo.transform.Find("Lod2");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod2Path = Path.Combine(landPath, "Tile_lod2") + "/" + baseName + "_lod2.prefab";

                var lod2_go = new GameObject("Lod2");
                lod2_go.transform.SetParent(aGo.transform);
                // 1.处理基础地块
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
                            ground.transform.SetParent(lod2_go.transform);
                            ground.transform.position = preObj.transform.position;
                            ground.transform.rotation = preObj.transform.rotation;
                            ground.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                var riverSprites = LoadSprites(landPath + "/Texture_lod3_4/land_form_lod3.png");
                var riverSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_sprite_ground_lod3.mat");
                // 2.河流
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
                            plane.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                // 3.湖泊
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
                            if(riverSprites.TryGetValue(preObj.name, out sprite))
                            {
                                render.sprite = sprite;
                            }
                            plane.transform.SetParent(lod2_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(90, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                //// 4.峡谷
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
                //// 5.山脉
                //var mountainTrans = lod0_go.transform.Find("Mountain");
                //if (mountainTrans != null)
                //{
                //    foreach (Transform child in mountainTrans)
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
                // 6. 动态树
                var treeSprites = LoadSprites(landPath + "/Texture/map_tree_atlas.png");
                var treeSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_high_sprite_nomask.mat");
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    foreach (Transform child in dynamicTree)
                    {
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var andorning = preObj.transform.Find("adorning/center").GetComponent<SpriteRenderer>();

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
                // 5. 静态树生成
                var staticTree = lod0_go.transform.Find("Static");
                if (staticTree != null)
                {
                    int nIndex = 0;
                    foreach (Transform child in staticTree)
                    {
                        string meshFullPath = Path.Combine(landFullPath, "Mesh", "AutoMake") + "/" + baseName + "_adorning_half_" + nIndex.ToString() + ".asset";
                        string prefabFullPath = Path.Combine(landFullPath, "Tile", "AutoMake") + "/" + baseName + "_adorning_half" + nIndex.ToString() + ".prefab";
                        var grove = CreateGrove(child.gameObject, meshFullPath, prefabFullPath, false, true);
                        grove.transform.SetParent(lod2_go.transform);
                        grove.transform.position = Vector3.zero;
                        grove.transform.localScale = Vector3.one;
                        nIndex++;
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod2_go, lod2Path, InteractionMode.AutomatedAction);
                lod2_go.gameObject.SetActive(false);
            }
            // Lod3 生成逻辑
            {
                var preview = aGo.transform.Find("Lod3");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod3Path = Path.Combine(landPath, "Tile_lod3") + "/" + baseName + "_lod3.prefab";

                var lod3_go = new GameObject("Lod3");
                lod3_go.transform.SetParent(aGo.transform);
                // 1.处理基础地块
                var groundTrans = lod0_go.transform.Find("Plane");
                if (groundTrans != null)
                {
                    var mf0 = groundTrans.GetComponent<MeshFilter>();
                    var mr0 = groundTrans.GetComponent<MeshRenderer>();
                    var meshFilter = lod3_go.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh  = mf0.sharedMesh;
                    var meshRender = lod3_go.AddComponent<MeshRenderer>();
                    meshRender.sharedMaterial = mr0.sharedMaterial;
                    meshRender.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    meshRender.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                }
                var riverSprites = LoadSprites(landPath + "/Texture_lod3_4/land_form_lod3.png");
                var riverSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_sprite_ground_lod3.mat");
                // 2.河流
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
                            plane.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                // 3.湖泊
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
                            plane.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                // 6. 动态树
                var treeSprites = LoadSprites(landPath + "/Texture/map_tree_atlas.png");
                var treeSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_high_sprite_nomask.mat");
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    for (int i = 0; i < dynamicTree.childCount; i++)
                    {
                        Transform child = dynamicTree.GetChild(i);
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var andorning = preObj.transform.Find("adorning/center").GetComponent<SpriteRenderer>();

                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            plane.AddComponent<LockAngle45>();
                            render.sharedMaterial = treeSpriteMat;
                            render.sprite = andorning.sprite;
                            plane.transform.SetParent(lod3_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(45, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(2, 2, 2);
                        }
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod3_go, lod3Path, InteractionMode.AutomatedAction);
                lod3_go.gameObject.SetActive(false);
            }

            // Lod4 生成逻辑
            {
                var preview = aGo.transform.Find("Lod4");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod4Path = Path.Combine(landPath, "Tile_lod4") + "/" + baseName + "_lod4.prefab";

                var lod4_go = new GameObject("Lod4");
                lod4_go.transform.SetParent(aGo.transform);
                // 1.处理基础地块
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
                }
                var riverSprites = LoadSprites(landPath + "/Texture_lod3_4/land_form_lod3.png");
                var riverSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_sprite_ground_lod3.mat");
                // 2.河流
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
                            plane.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                // 3.湖泊
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
                            plane.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                // 6. 动态树
                var treeSprites = LoadSprites(landPath + "/Texture/map_tree_atlas.png");
                var treeSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_high_sprite_nomask.mat");
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    for (int i = 0; i < dynamicTree.childCount; i++)
                    {
                        Transform child = dynamicTree.GetChild(i);
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var andorning = preObj.transform.Find("adorning/center").GetComponent<SpriteRenderer>();

                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            plane.AddComponent<LockAngle45>();
                            render.sharedMaterial = treeSpriteMat;
                            render.sprite = andorning.sprite;
                            plane.transform.SetParent(lod4_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(45, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(2, 2, 2);
                        }
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod4_go, lod4Path, InteractionMode.AutomatedAction);
                lod4_go.gameObject.SetActive(false);
            }

            // Lod5 生成逻辑
            {
                var preview = aGo.transform.Find("Lod5");
                if (preview != null)
                {
                    Object.DestroyImmediate(preview.gameObject);
                }
                string lod5Path = Path.Combine(landPath, "Tile_lod5") + "/" + baseName + "_lod5.prefab";

                var lod5_go = new GameObject("Lod5");
                lod5_go.transform.SetParent(aGo.transform);
                // 1.处理基础地块
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
                }
                var riverSprites = LoadSprites(landPath + "/Texture_lod3_4/land_form_lod3.png");
                var riverSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_sprite_ground_lod3.mat");
                // 2.河流
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
                            plane.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                // 3.湖泊
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
                            plane.transform.localScale = preObj.transform.localScale;
                        }
                    }
                }
                // 6. 动态树
                var treeSprites = LoadSprites(landPath + "/Texture/map_tree_atlas.png");
                var treeSpriteMat = AssetDatabase.LoadAssetAtPath<Material>(landPath + "/Material/map_high_sprite_nomask.mat");
                var dynamicTree = lod0_go.transform.Find("Dynamic");
                if (dynamicTree != null)
                {
                    for (int i = 0; i < dynamicTree.childCount; i++)
                    {
                        Transform child = dynamicTree.GetChild(i);
                        var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
                        if (preObj == child.gameObject)
                        {
                            var andorning = preObj.transform.Find("adorning/center").GetComponent<SpriteRenderer>();

                            var plane = new GameObject(preObj.name + "_plane_LOD2");

                            var render = plane.AddComponent<SpriteRenderer>();
                            plane.AddComponent<LockAngle45>();
                            render.sharedMaterial = treeSpriteMat;
                            render.sprite = andorning.sprite;
                            plane.transform.SetParent(lod5_go.transform);
                            plane.transform.position = preObj.transform.position;
                            plane.transform.Rotate(new Vector3(45, preObj.transform.eulerAngles.y, 0));
                            plane.transform.localScale = new Vector3(2, 2, 2);
                        }
                    }
                }
                PrefabUtility.SaveAsPrefabAssetAndConnect(lod5_go, lod5Path, InteractionMode.AutomatedAction);
            }
            lod0_go.gameObject.SetActive(false);
            PrefabUtility.SaveAsPrefabAssetAndConnect(aGo, assetpath, InteractionMode.AutomatedAction);
            Object.DestroyImmediate(aGo);
        }

        static Dictionary<string, Sprite> LoadSprites(string path)
        {
            var sprites = AssetDatabase.LoadAllAssetsAtPath(path);
            Dictionary<string, Sprite> dicSprites = new Dictionary<string, Sprite>();
            for (int i = 1; i < sprites.Length; i++)
            {
                Sprite sprite = sprites[i] as Sprite;
                dicSprites.Add(sprite.name, sprite);
            }
            return dicSprites;
        }

        static GameObject CreateGrove(GameObject aGo, string meshFullPath, string prefabFullPath, bool bSavePrefab = true, bool bHalf = false)
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
                if(bHalf && (i%2==1))
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
                for (int j = 0; j < render.sprite.triangles.Length; j += 3)
                {
                    triangles.Add(render.sprite.triangles[j] + nIndex);
                    triangles.Add(render.sprite.triangles[j + 2] + nIndex);
                    triangles.Add(render.sprite.triangles[j + 1] + nIndex);
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

            // 创建Mesh的目录
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

            // 获取树的材质
            var helper = aGo.GetComponentInChildren<GizmosTool>();
            Material material = helper.m_treeMaterial;

            // 创建预制件
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

        static void BuildDynamicGrove(string assetpath)
        {
            GameObject tempEffectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetpath);
            GameObject aGo = PrefabUtility.InstantiatePrefab(tempEffectPrefab) as GameObject;
            PrefabUtility.UnpackPrefabInstance(aGo, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            var adorningTran = aGo.transform.Find("adorning");
            adorningTran?.gameObject.SetActive(true);

            string landPath = Path.GetFullPath(Path.GetDirectoryName(assetpath) + "/../");
            string meshFullPath = Path.Combine(landPath, "Mesh", "AutoMake") + "/" + aGo.name + "_adorning.asset";
            string prefabFullPath = Path.Combine(landPath, "Tile") + "/" + aGo.name + "_adorning.prefab";
            var grovePrefab = CreateGrove(aGo, meshFullPath, prefabFullPath);
            // 删掉原来的树
            adorningTran?.gameObject.SetActive(false);
            while (true)
            {
                var preview = aGo.transform.Find(grovePrefab.name);
                if (preview == null)
                    break;
                Object.DestroyImmediate(preview.gameObject);
            }
            grovePrefab.transform.SetParent(aGo.transform);
            PrefabUtility.SaveAsPrefabAssetAndConnect(aGo, assetpath, InteractionMode.AutomatedAction);
            Object.DestroyImmediate(aGo);
        }

        #region "Tile Lod 的生成"
        [MenuItem("Assets/LodTool/BuildTile(1-5)", true, 0)]
        static bool ValidateBuildTile()
        {
            if (Selection.objects.Length != 1)
                return false;

            string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(assetpath) && Path.GetFileName(assetpath).Contains("Tile-") && Path.GetDirectoryName(assetpath).Contains("land\\Tile_lod0"))
            {
                return PrefabUtility.IsPartOfPrefabAsset(Selection.activeObject);
            }

            return false;
        }
        #endregion
        #region "动态树的生成"
        [MenuItem("Assets/LodTool/BuildGrove", true, 0)]
        static bool ValidateBuildGrove()
        {
            if (Selection.objects.Length != 1)
                return false;

            string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(assetpath) && Path.GetFileName(assetpath).Contains("Grove") && Path.GetDirectoryName(assetpath).Contains("land\\Grove"))
            {
                return PrefabUtility.IsPartOfPrefabAsset(Selection.activeObject);
            }

            return false;
        }
        [MenuItem("Assets/LodTool/BuildGrove")]
        static void BuildGrove()
        {
            string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
        }
        #endregion
    }
}
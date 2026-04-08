using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using Skyunion;

namespace Client
{
    public class MapEditorMenus : MonoBehaviour
    {
        [MenuItem("Tools/OpenClientEditor", false)]
        public static void OpenEditorWindow()
        {
            ClientEditor.MenuOpenWindow();
        }

        private const int Walkable = 0x01;
        private const int NotWalkable = 0x02;
        private const int Jump = 0x04;
        private const int NoBuilding = 0x08;
        private const int NoWalk = 0x10;

        [MenuItem("Tools/导出可行走面")]
        public static void ExportWalkableNavMesh()
        {
            Debug.Log("Export NavMesh");
            int flag = Walkable | NoBuilding;
            //新建文件  
            string sceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
            string savePath = Application.dataPath + "/" + sceneName + "_Walkable_NavMesh.obj";


            Debug.Log("Export NavMesh");

            //Unity2017 API
            UnityEngine.AI.NavMeshTriangulation navMeshTriangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();

            StreamWriter sw = new StreamWriter(savePath);

            //顶点  
            for (int i = 0; i < navMeshTriangulation.vertices.Length; i++)
            {
                sw.WriteLine("v  " + -navMeshTriangulation.vertices[i].x + " " + /*navMeshTriangulation.vertices[i].y*/0 + " " + navMeshTriangulation.vertices[i].z);
            }

            sw.WriteLine("g navmesh");//组名称

            //索引  
            int nTrig = 0;
            for (int i = 0; i < navMeshTriangulation.indices.Length;)
            {
                //obj文件中顶点索引是从1开始
                if (navMeshTriangulation.areas[nTrig] == 0)
                {
                    sw.WriteLine("f " + (navMeshTriangulation.indices[i] + 1) + " " + (navMeshTriangulation.indices[i + 2] + 1) + " " + (navMeshTriangulation.indices[i + 1] + 1));
                }
                i = i + 3;
                nTrig++;
            }

            sw.Flush();
            sw.Close();

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Debug.Log(string.Format("Verts:{0}  Tris:{1}", navMeshTriangulation.vertices.Length, navMeshTriangulation.indices.Length / 3));
            Debug.Log(savePath);
            Debug.Log("ExportNavMesh Success");
        }
        [MenuItem("Tools/导出不可建筑面")]
        public static void ExportNoBuildingNavMesh()
        {
            Debug.Log("Export NavMesh");
            int flag = NotWalkable | NoBuilding;
            //新建文件  
            string sceneName = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name;
            string savePath = Application.dataPath + "/" + sceneName + "_NoBuilding_NavMesh.obj";


            Debug.Log("Export NavMesh");

            //Unity2017 API
            UnityEngine.AI.NavMeshTriangulation navMeshTriangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();

            StreamWriter sw = new StreamWriter(savePath);

            //顶点  
            for (int i = 0; i < navMeshTriangulation.vertices.Length; i++)
            {
                sw.WriteLine("v  " + -navMeshTriangulation.vertices[i].x + " " + /*navMeshTriangulation.vertices[i].y*/0 + " " + navMeshTriangulation.vertices[i].z);
            }

            sw.WriteLine("g navmesh");//组名称

            //索引  
            int nTrig = 0;
            for (int i = 0; i < navMeshTriangulation.indices.Length;)
            {
                //obj文件中顶点索引是从1开始
                if (navMeshTriangulation.areas[nTrig] != 0)
                {
                    sw.WriteLine("f " + (navMeshTriangulation.indices[i] + 1) + " " + (navMeshTriangulation.indices[i + 2] + 1) + " " + (navMeshTriangulation.indices[i + 1] + 1));
                }
                i = i + 3;
                nTrig++;
            }

            sw.Flush();
            sw.Close();

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Debug.Log(string.Format("Verts:{0}  Tris:{1}", navMeshTriangulation.vertices.Length, navMeshTriangulation.indices.Length / 3));
            Debug.Log(savePath);
            Debug.Log("ExportNavMesh Success");
        }
        public static void ExportNavMesh(int flag, string savePath)
        {
        }

        //static void CombineTree(bool half)
        //{
        //    var aGo = Selection.activeGameObject;

        //    List<Vector3> vertices = new List<Vector3>();
        //    List<Vector2> uv1s = new List<Vector2>();
        //    List<int> triangles = new List<int>();
        //    List<Color> colors = new List<Color>();


        //    var sprites = new List<SpriteRenderer>(aGo.transform.GetComponentsInChildren<SpriteRenderer>(false));
        //    sprites.Sort((val1, val2) =>
        //    {
        //        var pos1 = val1.transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(0, 0, -val1.size.y / 2.0f));
        //        var pos2 = val2.transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(0, 0, -val2.size.y / 2.0f));

        //        if (pos1.z > pos2.z)
        //            return -1;
        //        if (pos1.z < pos2.z)
        //            return 1;

        //        return 0;
        //    });

        //    int nIndex = 0;
        //    for(int i = 0; i < sprites.Count; i++)
        //    {
        //        if(half==true && i%2==1)
        //        {
        //            continue;
        //        }
        //        var render = sprites[i];
        //        Matrix4x4 transformMatrix = aGo.transform.worldToLocalMatrix * render.transform.localToWorldMatrix;

        //        var center = transformMatrix.MultiplyPoint3x4(render.sprite.bounds.center);
        //        var centerColor = new Color(center.x / 180.0f, center.y / 180.0f, center.z / 180.0f);
        //        foreach (var vert in render.sprite.vertices)
        //        {
        //            var newPos = transformMatrix.MultiplyPoint3x4(vert);
        //            vertices.Add(newPos);
        //            colors.Add(centerColor);
        //        }
        //        foreach (var uv in render.sprite.uv)
        //        {
        //            uv1s.Add(uv);
        //        }
        //        for (int j = 0; j < render.sprite.triangles.Length; j+=3)
        //        {
        //            triangles.Add(render.sprite.triangles[j] + nIndex);
        //            triangles.Add(render.sprite.triangles[j+1] + nIndex);
        //            triangles.Add(render.sprite.triangles[j+2] + nIndex);
        //        }
        //        nIndex += render.sprite.vertices.Length;
        //    }


        //    var mesh = new Mesh();
        //    mesh.vertices = vertices.ToArray();
        //    mesh.uv = uv1s.ToArray();
        //    mesh.colors = colors.ToArray();
        //    mesh.subMeshCount = 1;
        //    mesh.SetTriangles(triangles.ToArray(), 0);

        //    mesh.RecalculateTangents();
        //    mesh.RecalculateBounds();

        //    string meshPath = AssetDatabase.GenerateUniqueAssetPath("Assets" + "/" + aGo.name + "_part" + ".asset");
        //    AssetDatabase.CreateAsset(mesh, meshPath);
        //    Debug.Log($"Trangles:{triangles.Count}");
        //}

        //[MenuItem("Tools/CombineTree/Full")]
        //static void CombineTreeFull()
        //{
        //    CombineTree(false);
        //}

        //[MenuItem("Tools/CombineTree/Full", true, 0)]
        //static bool ValidateCombineTreeFull()
        //{
        //    return Selection.transforms.Length == 1;
        //}

        //[MenuItem("Tools/CombineTree/Half")]
        //static void CombineTreeHalf()
        //{
        //    CombineTree(true);
        //}

        //[MenuItem("Tools/CombineTree/Half", true, 0)]
        //static bool ValidateCombineTreeHalf()
        //{
        //    return Selection.transforms.Length == 1;
        //}


        //[MenuItem("Assets/LodTool/CreateLod2-5Prefab", true, 0)]
        //static bool ValidateCreateLodPrefab()
        //{
        //    string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
        //    if (!string.IsNullOrEmpty(assetpath) && Path.GetDirectoryName(assetpath).Equals("Assets\\BundleAssets\\Scene\\Map_landform\\Tile"))
        //    {
        //        return PrefabUtility.IsPartOfPrefabAsset(Selection.activeObject);
        //    }

        //    return false;
        //}

        //[MenuItem("Assets/LodTool/CreateLod2-5Prefab")]
        //static void CreateLodPrefab()
        //{
        //    string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
        //    CreateLod2Prefab(assetpath, 2);
        //    CreateLod2Prefab(assetpath, 3);
        //    CreateLod2Prefab(assetpath, 4);
        //    CreateLod2Prefab(assetpath, 5);
        //}

        //static void CreateLod2Prefab(string assetpath, int lod)
        //{
        //    string prefabName = Path.GetFileNameWithoutExtension(assetpath);
        //    string lodPath = Path.Combine($"Assets/BundleAssets/Scene/Map_landform/Tile_lod{lod}", prefabName + $"_lod{lod}.prefab");
        //    // 不存在才创建
        //    if (!File.Exists(lodPath))
        //    {
        //        var goLod2 = new GameObject(Path.GetFileNameWithoutExtension(lodPath));
        //        var golod0 = PrefabUtility.LoadPrefabContents(assetpath);
        //        goLod2.transform.localPosition = golod0.transform.localPosition;

        //        foreach (Transform child in golod0.transform)
        //        {
        //            // 判断是不是预制件的跟节点，是的话才处理
        //            var preObj = PrefabUtility.GetNearestPrefabInstanceRoot(child);
        //            if (preObj == child.gameObject)
        //            {
        //                var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(child);
        //                var prefabId = Path.GetFileNameWithoutExtension(path);
        //                // 基础地表需要保留
        //                if (child.name.Contains("_TYPE_"))
        //                {
        //                    // 大于lod2的会合并
        //                    if (lod > 2)
        //                        continue;
        //                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        //                    var obj = PrefabUtility.InstantiatePrefab(prefab, goLod2.transform) as GameObject;
        //                    obj.transform.localPosition = preObj.transform.localPosition;
        //                }
        //                // 山脉也需要保留
        //                else if (child.name.Contains("mountain"))
        //                {
        //                    string mountainPath = Path.Combine("Assets/BundleAssets/Scene/Map_landform/AutoMake/", prefabId + $"_lod{lod}.prefab");
        //                    // 如果没有生成过需要生成一份
        //                    if (!File.Exists(mountainPath))
        //                    {
        //                        var mountainRoot = new GameObject(Path.GetFileNameWithoutExtension(mountainPath));
        //                        foreach (Transform mountain in child.transform)
        //                        {
        //                            // 碰撞的需要过滤掉
        //                            if (mountain.name.Contains("collider"))
        //                                continue;

        //                            var newMountain = GameObject.Instantiate<GameObject>(mountain.gameObject, mountainRoot.transform);
        //                            newMountain.transform.localPosition = mountain.localPosition;
        //                            newMountain.transform.localScale = mountain.localScale;
        //                            newMountain.transform.localRotation = mountain.localRotation;

        //                            // 生成lod的mesh

        //                            var mf = mountain.GetComponent<MeshFilter>();
        //                            var meshPath = Path.Combine("Assets/BundleAssets/Scene/Map_landform/AutoMake/", mf.sharedMesh.name + $"_lod{lod}.asset");
        //                            if (!File.Exists(meshPath))
        //                            {
        //                                var mesh = mf.sharedMesh;
        //                                int tCount = mesh.GetTriangleCount();
        //                                var saveMeshPath = AssetDatabase.GenerateUniqueAssetPath("Assets" + "/BundleAssets/Scene/Map_landform/AutoMake/" + mf.sharedMesh.name + $"_lod{2}.asset");
        //                                mesh = mesh.MakeLODMesh(tCount / 200, false);
        //                                AssetDatabase.CreateAsset(mesh, saveMeshPath);
        //                                AssetDatabase.Refresh();
        //                            }
        //                            var newMeshLod2 = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
        //                            newMountain.GetComponent<MeshFilter>().sharedMesh = newMeshLod2;
        //                        }
        //                        PrefabUtility.SaveAsPrefabAsset(mountainRoot, mountainPath);
        //                        DestroyImmediate(mountainRoot);
        //                        AssetDatabase.Refresh();
        //                    }
        //                    // 树丛 需要替换成图片
        //                    else if (child.name.Contains("Grove"))
        //                    {
        //                        Debug.LogError("not support grove");
        //                    }
        //                    // 河流 需要替换成图片
        //                    else if (child.name.Contains("river"))
        //                    {
        //                        Debug.LogError("not support river");
        //                    }
        //                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(mountainPath);
        //                    if (prefab != null)
        //                    {
        //                        var obj = PrefabUtility.InstantiatePrefab(prefab, goLod2.transform) as GameObject;
        //                        obj.transform.localPosition = preObj.transform.localPosition;
        //                        obj.transform.localScale = preObj.transform.localScale;
        //                        obj.transform.localRotation = preObj.transform.localRotation;
        //                    }
        //                }
        //            }
        //        }

        //        // 大于2的地表只贴一个
        //        if (lod > 2)
        //        {
        //            var mf = goLod2.AddComponent<MeshFilter>();
        //            mf.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>("Assets/BundleAssets/Map/Env/Plane/Plane180_180.FBX");
        //            var render = goLod2.AddComponent<MeshRenderer>();
        //            render.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/BundleAssets/Map/Env/Ground/I_TYPE_Sand_02_lod3.mat");
        //            render.receiveShadows = false;
        //            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        //            render.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        //            render.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        //        }
        //        goLod2.AddComponent<ObjectPoolItem>();
        //        PrefabUtility.SaveAsPrefabAsset(goLod2, lodPath);
        //        DestroyImmediate(goLod2);
        //        PrefabUtility.UnloadPrefabContents(golod0);
        //    }
        //}


        //[MenuItem("Examples/Instantiate Selected")]
        //static void InstantiatePrefab()
        //{
        //    Selection.activeObject = PrefabUtility.InstantiatePrefab(Selection.activeObject as GameObject);
        //}

        //[MenuItem("Examples/Instantiate Selected", true)]
        //static bool ValidateInstantiatePrefab()
        //{
        //    GameObject go = Selection.activeObject as GameObject;
        //    if (go == null)
        //        return false;

        //    return PrefabUtility.IsPartOfPrefabAsset(go);
        //}

        [MenuItem("Assets/TextureTool/ExportPng", true, 0)]
        static bool ValidateCreateLodPrefab()
        {
            string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(assetpath) )
            {
                return Selection.activeObject is RenderTexture;
            }

            return false;
        }

        [MenuItem("Assets/TextureTool/ExportPng")]
        static void CreateLodPrefab()
        {
            string assetpath = AssetDatabase.GetAssetPath(Selection.activeObject);
            SaveRenderToPng(Selection.activeObject as RenderTexture, Path.GetDirectoryName(assetpath), Path.GetFileNameWithoutExtension(assetpath));
            AssetDatabase.Refresh();

        }
        static public Texture2D SaveRenderToPng(RenderTexture renderT, string folderName, string name)
        {
            int width = renderT.width;
            int height = renderT.height;
            Texture2D tex2d = new Texture2D(width, height, TextureFormat.ARGB32, false);
            RenderTexture.active = renderT;
            tex2d.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex2d.Apply();

            byte[] b = tex2d.EncodeToPNG();
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);
            FileStream file = File.Open(folderName + "/" + name + ".png", FileMode.Create);
            BinaryWriter writer = new BinaryWriter(file);
            writer.Write(b);
            file.Close();
            return tex2d;
        }
        [MenuItem("Tools/打印非BoxCollider")]
        public static void PrintfCollision()
        {
            List<GameObject> prefabs = new List<GameObject>();
            var resourcesPath = Application.dataPath;
            var absolutePaths = System.IO.Directory.GetFiles(resourcesPath, "*.prefab", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < absolutePaths.Length; i++)
            {
                EditorUtility.DisplayProgressBar("获取预制体……", "获取预制体中……", (float)i / absolutePaths.Length);

                string path = "Assets" + absolutePaths[i].Remove(0, resourcesPath.Length);
                path = path.Replace("\\", "/");
                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                if (prefab != null)
                {
                    var colliders = prefab.GetComponentsInChildren<Collider>();
                    foreach(var col in colliders)
                    {
                        if (col as BoxCollider == null)
                        {
                            Debug.Log(col.GetType().ToString()+":" + path);
                        }
                    }
                }
                else
                    Debug.Log("预制体不存在！ " + path);
            }
            if (absolutePaths.Length > 0)
            {
                EditorUtility.ClearProgressBar();
            }
        }

        [MenuItem("Tools/检测错误粒子")]
        static void BuildLod()
        {
            List<GameObject> prefabs = new List<GameObject>();
            var resourcesPath = Application.dataPath;
            var absolutePaths = System.IO.Directory.GetFiles(resourcesPath, "*.prefab", System.IO.SearchOption.AllDirectories);
            for (int i = 0; i < absolutePaths.Length; i++)
            {
                EditorUtility.DisplayProgressBar("获取预制体……", "获取预制体中……", (float)i / absolutePaths.Length);

                string path = "Assets" + absolutePaths[i].Remove(0, resourcesPath.Length);
                path = path.Replace("\\", "/");
                GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                if (prefab != null)
                {
                    var particles = prefab.GetComponentsInChildren<ParticleSystem>();
                    foreach (var ps in particles)
                    {
                        if (ps.shape.enabled != true) continue;

                        switch (ps.shape.shapeType)
                        {
                            case ParticleSystemShapeType.Mesh:
                                if (ps.shape.mesh == null)
                                {
                                    Debug.Log($"ShapeType - Mesh : mesh null !!! path:{path} name:{ps.name}");
                                }
                                else
                                {
                                    if (ps.shape.mesh.isReadable != true)
                                    {
                                        Debug.Log($"ShapeType - Mesh : mesh read/write off !!! path:{path} name:{ps.name}");
                                    }
                                }
                                break;
                            case ParticleSystemShapeType.MeshRenderer:
                                if (ps.shape.meshRenderer == null)
                                {
                                    Debug.Log($"ShapeType - MeshRenderer : mesh null !!! path:{path} name:{ps.name}");                                    
                                }
                                else
                                {
                                    if (ps.shape.meshRenderer.gameObject != null)
                                    {
                                        var mf = ps.shape.meshRenderer.gameObject.GetComponent<MeshFilter>();
                                        if (mf != null &&
                                            mf.sharedMesh != null &&
                                            mf.sharedMesh.isReadable != true)
                                        {
                                            Debug.Log($"ShapeType - MeshRenderer : mesh read/write off !!! path:{path} name:{ps.name}");
                                        }
                                    }
                                }
                                break;
                            case ParticleSystemShapeType.SkinnedMeshRenderer:
                                if (ps.shape.skinnedMeshRenderer == null)
                                {
                                    Debug.Log($"ShapeType - SkinnedMeshRenderer : mesh null !!! path:{path} name:{ps.name}");
                                }
                                else
                                {
                                    if (ps.shape.skinnedMeshRenderer.sharedMesh != null &&
                                        ps.shape.skinnedMeshRenderer.sharedMesh.isReadable != true)
                                    {
                                        Debug.Log($"ShapeType - SkinnedMeshRenderer : mesh read/write off !!! path:{path} name:{ps.name}");
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            if (absolutePaths.Length > 0)
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}
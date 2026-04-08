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
    public class DynamicTreeEditor
    {
        static bool m_bEnabled = false;
        [InitializeOnLoadMethod]
        static void Init()
        {
            m_bEnabled = false;
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                OnPrefabStageOpend(PrefabStageUtility.GetCurrentPrefabStage());
            }
            PrefabStage.prefabStageOpened += OnPrefabStageOpend;
            PrefabStage.prefabStageClosing += OnPrefabStageClosing;
        }
        static void OnPrefabStageOpend(PrefabStage stage)
        {
            if(stage.prefabAssetPath.Contains("land/Grove/"))
            {
                SceneView.duringSceneGui += OnSceneGUI;
                m_bEnabled = true;
            }
        }
        static void OnPrefabStageClosing(PrefabStage stage)
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
            if (adorning != null && GUILayout.Button($"Switch", GUILayout.MaxWidth(50)))
            {
                bool bAdorning = adorning.gameObject.activeInHierarchy;
                adorning?.gameObject.SetActive(!bAdorning);
                grove?.gameObject.SetActive(bAdorning);
            }

            if (grove != null)
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

            Handles.EndGUI();
            SceneView.RepaintAll();
            if (reOpenPrefab)
            {
                AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(prefabStage.prefabAssetPath));
            }
        }
        private static void BuildDynamicGrove(string assetpath)
        {
            GameObject tempEffectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetpath);
            GameObject aGo = PrefabUtility.InstantiatePrefab(tempEffectPrefab) as GameObject;
            PrefabUtility.UnpackPrefabInstance(aGo, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            var adorningTran = aGo.transform.Find("adorning");
            adorningTran?.gameObject.SetActive(true);

            string landPath = Path.GetFullPath(Path.GetDirectoryName(assetpath) + "/../");
            string meshFullPath = Path.Combine(landPath, "Mesh", "AutoMake") + "/" + aGo.name + "_adorning.asset";
            string prefabFullPath = Path.Combine(landPath, "Tile") + "/" + aGo.name + "_adorning.prefab";
            bool bRevert = false;
            if(landPath.Contains("BundleAssetsTest"))
            {
                bRevert = true;
            }
            var grovePrefab = CreateGrove(aGo, meshFullPath, prefabFullPath, bRevert);
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

        public static GameObject CreateGrove(GameObject aGo, string meshFullPath, string prefabFullPath, bool bRevert = false, bool bSavePrefab = true, bool bHalf = false)
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

    }
}
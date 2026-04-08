using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.U2D;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using Object = UnityEngine.Object;

#if UNITY_EDITOR

namespace FindReferencesSpace
{
    public class EditorFindReferences
    {
        const string _menuName = "Assets/Find References In Project %&e";

        [MenuItem(_menuName, false, 25)]
        static private void Find()
        {
            DateTime cachedTime = DateTime.Now;

            EditorSettings.serializationMode = SerializationMode.ForceText;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            Object[] selectObjs = Selection.objects;
            Dictionary<string, AssetReferencesData> assetReferenceDict = new Dictionary<string, AssetReferencesData>();

            /*
            for (int i = 0; i < objs.Length; i++)
            {
                Object obj = objs[i];
                string guid;
                long localId;
    
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objs[i], out guid, out localId);
    
                bool isSubAsset = AssetDatabase.IsSubAsset(obj);
                Debug.Log($"obj:[{objs[i]}]   guid:[{guid}]  localId:[{localId}]  isSubAsset:[{isSubAsset}]");
            }*/

            for (int i = 0; i < selectObjs.Length; i++)
            {
                Object obj = selectObjs[i];
                bool isSubAsset = AssetDatabase.IsSubAsset(obj);

                string guid;
                long localId;
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(selectObjs[i], out guid, out localId);
                if (isSubAsset)
                {
                    AssetReferencesData assetData = new AssetSubReferencesData(obj, guid, localId);
                    assetReferenceDict.Add(guid + localId, assetData);
                }
                else
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                    List<string> fixedAssetsGUID = new List<string>();
                    if (FileTool.IsDirectory(assetPath))
                    {
                        List<string> tmpList = FileTool.GetFiles(assetPath, "*.*");
                        foreach (var tmpDict in tmpList)
                        {
                            if (FileTool.IsDirectory(tmpDict) == false)
                            {
                                fixedAssetsGUID.Add(AssetDatabase.AssetPathToGUID(tmpDict));
                            }
                        }
                    }
                    else
                    {
                        fixedAssetsGUID.Add(AssetDatabase.AssetPathToGUID(assetPath));
                    }

                    foreach (var tmpValue in fixedAssetsGUID)
                    {
                        if (string.IsNullOrEmpty(tmpValue) == false && !assetReferenceDict.ContainsKey(tmpValue))
                        {
                            assetReferenceDict.Add(tmpValue, new AssetReferencesData(tmpValue));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(path))
            {
                FindReferencesEditorWindow.Open(assetReferenceDict);
            }
        }


        [MenuItem(_menuName, true)]
        static private bool VFind()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return (!string.IsNullOrEmpty(path));
        }
    }


    public enum FindReferencesFileDataType
    {
        Asset,
        Prefab,
        Unity,
        Material,
    }

    public class FindReferencesFileData
    {
        public string fullPath;
        public string assetPath;
        public string extension;
        public Object assetObj;
        public FindReferencesFileDataType dataType;

        public bool foldout;
        public List<Object> RefChilds = new List<Object>();

        public Object targetAssetObj; // 图片资源
        public string targetGUID;
        public string targetPath;

        public FindReferencesFileData(string fullPath, string assetPath)
        {
            this.fullPath = fullPath;
            this.assetPath = assetPath;

            this.extension = System.IO.Path.GetExtension(fullPath);
            this.assetObj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
        }

        public void FindRefChilds()
        {
            switch (extension)
            {
                case ".prefab":
                    FindRefChildsInGameObject();
                    break;
                case ".unity":
                    FindRefChildsInScene();
                    break;
            }
        }

        void FindRefChildsInGameObject()
        {
            RefChilds.Clear();
            GameObject obj = (GameObject) assetObj;

            Image[] images = obj.GetComponentsInChildren<Image>(true);
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i].sprite != null)
                {
                    string spritePath = AssetDatabase.GetAssetPath(images[i].sprite);
                    if (spritePath.Equals(targetPath))
                    {
                        RefChilds.Add(images[i]);
                    }
                }
            }
        }

        void FindRefChildsInScene()
        {
            RefChilds.Clear();
            Debug.Log("scene    target:[" + assetObj);
        }
    }

    public class AssetSubReferencesData : AssetReferencesData
    {
        public long localId;

        public AssetSubReferencesData(Object obj, string guid, long localId) : base(guid)
        {
            this.assetName = obj.name;
            this.assetObj = obj;
            this.localId = localId;
            this.canDel = false;
        }
    }

// 资源引用的数据
    public class AssetReferencesData
    {
        public string guid;
        public string assetPath;
        public string assetExtension;
        public Object assetObj;
        public string assetName;

        public bool isShow;
        public bool canDel = false;
        public bool needDel;
        public Vector2 vec2 = Vector2.zero;
        public List<FindReferencesFileData> fileDatas = new List<FindReferencesFileData>();

        public AssetReferencesData(string guid)
        {
            this.guid = guid;
            this.assetPath = AssetDatabase.GUIDToAssetPath(guid);
            this.assetExtension = System.IO.Path.GetExtension(assetPath);
            this.assetObj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            this.assetName = System.IO.Path.GetFileName(assetPath);
//        Debug.Log("AssetReferencesData  assets   assetExtension:[" + this.assetExtension + "]   assetPath:[" + this.assetPath + "   guid:[" + this.guid + "]");
        }

        public void AddFileData(FindReferencesFileData data)
        {
            data.targetAssetObj = this.assetObj;
            data.targetPath = this.assetPath;
            data.targetGUID = this.guid;
            data.FindRefChilds();
            fileDatas.Add(data);
        }

        public void ClearFileData()
        {
            fileDatas.Clear();
        }

        public void DelAsset()
        {
            AssetDatabase.DeleteAsset(this.assetPath);
        }
    }

    public partial class FindReferencesEditorWindow : EditorWindow
    {
        string searchPathContent = "Assets/BundleAssets/UI/UIPrefabs";
        List<string> searchPath = new List<string>();
        Dictionary<string, AssetReferencesData> assetReferenceDict;
        Vector2 vec2;
        bool isSearchFinish = false;

        protected string[] titles = new string[] {"工程内资源"};
        int curTitle = 0;

        public static void Open(Dictionary<string, AssetReferencesData> assetRerenceDict)
        {
            FindReferencesEditorWindow window = GetWindow<FindReferencesEditorWindow>();
            window.assetReferenceDict = assetRerenceDict;
            window.isSearchFinish = false;
            window.searchPath.Clear();
            window.Show();
        }

        // 搜索引用
        void SearchUpdate(string[] searchDir)
        {
            var withoutExtensions = new List<string>()
                {".prefab", ".unity", ".mat", ".asset", ".spriteatlas", ".overrideController"};

            List<string> fileList = new List<string>();
            for (int i = 0; i < searchDir.Length; i++)
            {
                string path = searchDir[i];
                if (string.IsNullOrEmpty(path) || !FileTool.IsDirectory(path))
                {
                    continue;
                }

                string[] tmp = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(s => withoutExtensions.Contains(Path.GetExtension(s))).ToArray();
                fileList.AddRange(tmp);
            }

            string[] files = fileList.ToArray();

            if (files.Length <= 0)
            {
                isSearchFinish = true;
            }
            else
            {
                int startIndex = 0;
                DateTime cachedTime = DateTime.Now;
                EditorApplication.update = delegate()
                {
                    string file = files[startIndex];
                    bool isCancel =
                        EditorUtility.DisplayCancelableProgressBar("匹配资源中", file,
                            (float) startIndex / (float) files.Length);
                    var text = File.ReadAllText(file);

                    foreach (var dict in assetReferenceDict)
                    {
                        string guid = dict.Key;
                        AssetSubReferencesData assetSubData = dict.Value as AssetSubReferencesData;
                        if (assetSubData != null)
                        {
                            if (Regex.IsMatch(text, assetSubData.guid) &&
                                Regex.IsMatch(text, assetSubData.localId.ToString()))
                            {
                                FindReferencesFileData tempData =
                                    new FindReferencesFileData(file, GetRelativeAssetsPath(file));
                                dict.Value.AddFileData(tempData);
                            }
                        }
                        else
                        {
                            if (Regex.IsMatch(text, guid))
                            {
                                FindReferencesFileData tempData =
                                    new FindReferencesFileData(file, GetRelativeAssetsPath(file));
                                dict.Value.AddFileData(tempData);
                            }
                        }
                    }

                    startIndex++;
                    if (isCancel || startIndex >= files.Length)
                    {
                        isSearchFinish = true;
                        EditorUtility.ClearProgressBar();
                        EditorApplication.update = null;
                        startIndex = 0;

                        TimeSpan ts = DateTime.Now - cachedTime;
                        Debug.Log(" 本次匹配数据  花了: " + (ts.TotalMilliseconds * 1.0f / 1000) + " 秒才缓过来");
                    }
                };
            }
        }

        static private string GetRelativeAssetsPath(string path)
        {
            return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "")
                       .Replace('\\', '/');
        }

        void SetReferenceDict(Dictionary<string, AssetReferencesData> assetRerenceDict)
        {
            this.assetReferenceDict = assetRerenceDict;
        }

        private void OnGUI()
        {
            if (assetReferenceDict == null)
                return;

            GUI.color = Color.red;
            if (GUILayout.Button("重置路径"))
            {
                isSearchFinish = false;
                searchPathContent = "";
            }

            GUI.color = Color.white;
            GUILayout.Space(20);

            GUILayout.BeginHorizontal("box");
            for (int i = 0; i < titles.Length; i++)
            {
                GUI.color = Color.white;
                if (curTitle == i)
                {
                    GUI.color = Color.green;
                }

                if (GUILayout.Button(titles[i]))
                {
                    curTitle = i;
                }

                GUI.color = Color.white;
            }

            GUILayout.EndHorizontal();

            switch (curTitle)
            {
                case 0:
                    if (isSearchFinish == false)
                    {
                        DrawSearchDir();
                    }
                    else
                    {
                        DrawReferenceDatas();
                    }

                    break;
            }
        }

        /// <summary>
        /// 清空无引用资源 (暂时用不到)
        /// </summary>
        void ClearNoRefAssets()
        {
            foreach (var assetReferencesData in assetReferenceDict)
            {
                if (assetReferencesData.Value.fileDatas.Count <= 0)
                {
                    assetReferencesData.Value.needDel = true;
                }
            }

            DealNeedDelAssets();
        }
        
        /// <summary>
        /// 删除需要删除的资源(暂时用不到)
        /// </summary>
        void DealNeedDelAssets()
        {
            List<string> delAssets = new List<string>();
            foreach (var dict in assetReferenceDict)
            {
                AssetReferencesData assetData = dict.Value;
                if (assetData.needDel)
                {
                    assetData.DelAsset();
                    delAssets.Add(dict.Key);
                }
            }

            for (int i = 0; i < delAssets.Count; i++)
            {
                assetReferenceDict.Remove(delAssets[i]);
            }
        }

        // 绘制搜索路径
        void DrawSearchDir()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            vec2 = GUILayout.BeginScrollView(vec2);
            foreach (var assetReferencesData in assetReferenceDict)
            {
                AssetReferencesData assetData = assetReferencesData.Value;
                GUILayout.Button(assetData.assetPath);
            }

            GUILayout.EndScrollView();

            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            searchPathContent = HopeEditorUtils.DrawPath("搜索路径", searchPathContent);
            searchPath.Clear();
            searchPath.AddRange(searchPathContent.Split('\n'));

            GUI.color = Color.green;
            if (GUILayout.Button("搜索"))
            {
                if (string.IsNullOrEmpty(searchPathContent))
                {
                    searchPath.Add(Application.dataPath);
                    Debug.LogWarning("没有搜索路径，默认当前整个项目");
                }

                foreach (var dict in assetReferenceDict)
                    dict.Value.ClearFileData(); // 清空之前引用的数据

                SearchUpdate(searchPath.ToArray());
            }

            GUI.color = Color.white;

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        void DrawReferenceDatas()
        {
            AssetReferencesData curAssetData = null;
            bool needDelAsset = false;

            GUILayout.BeginHorizontal();

            vec2 = GUILayout.BeginScrollView(vec2);
            GUILayout.BeginVertical(GUILayout.MaxWidth(250));

            foreach (var assetReferencesData in assetReferenceDict)
            {
                GUILayout.BeginHorizontal("box");

                AssetReferencesData assetData = assetReferencesData.Value;
                if (assetData.isShow)
                    GUI.color = Color.green;

                if (GUILayout.Button(assetData.assetName))
                {
                    bool oldValue = assetData.isShow;
                    SetAllAssetDataNotShow();
                    assetData.isShow = !oldValue;
                }

                GUI.color = Color.white;

                if (assetData.isShow)
                {
                    curAssetData = assetData;
                }

                if (assetData.canDel)
                {
                    assetData.needDel = EditorGUILayout.Toggle(assetData.needDel);
                    GUI.color = Color.red;
                    if (GUILayout.Button("删除", GUILayout.Width(35)))
                    {
                        needDelAsset = true;

                        break;
                    }
                }

                GUI.color = Color.white;

                GUILayout.EndHorizontal();
            }

            if (needDelAsset)
            {
                DealNeedDelAssets();
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            if (curAssetData != null)
                DrawAssetData(curAssetData);

            GUILayout.EndHorizontal();
        }


        void DrawAssetData(AssetReferencesData assetData)
        {
            assetData.vec2 = GUILayout.BeginScrollView(assetData.vec2);

            DragAssetObject(assetData.assetExtension, assetData.assetObj, GUILayout.Width(100));
            GUILayout.Space(20);

            for (int i = 0; i < assetData.fileDatas.Count; i++)
            {
                FindReferencesFileData tmp = assetData.fileDatas[i];
                DragAssetObject(tmp);
            }


            GUILayout.EndScrollView();
        }

        void SetAllAssetDataNotShow()
        {
            foreach (var assetReferencesData in assetReferenceDict)
            {
                assetReferencesData.Value.isShow = false;
            }
        }

        void DragAssetObject(FindReferencesFileData data)
        {
            string extension = data.extension;
            Object obj = data.assetObj;
            if (obj == null)
            {
                return;
            }

            switch (extension)
            {
                case ".prefab":
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField("", obj, typeof(GameObject), true);
                    if (data.RefChilds.Count > 0)
                    {
                        GUILayout.BeginVertical();

                        data.foldout = EditorGUILayout.Foldout(data.foldout, obj.name);
                        if (data.foldout)
                        {
                            for (int i = 0; i < data.RefChilds.Count; i++)
                            {
                                if (data.RefChilds[i] != null)
                                {
                                    GUILayout.BeginHorizontal("box");
                                    EditorGUILayout.TextField(data.RefChilds[i].name);
                                    EditorGUILayout.ObjectField(data.RefChilds[i], typeof(GameObject));
                                    GUILayout.EndHorizontal();
                                }
                            }
                        }

                        GUILayout.EndVertical();
                    }

                    GUILayout.EndHorizontal();
                    break;
                default:
                    EditorGUILayout.ObjectField("", obj, typeof(Object), true);
                    break;
            }
        }

        void DragAssetObject(string extension, Object obj, params GUILayoutOption[] options)
        {
            switch (extension)
            {
                default:
                    EditorGUILayout.ObjectField("", obj, typeof(Object), true, options);
                    break;
            }
        }
    }
}


#endif
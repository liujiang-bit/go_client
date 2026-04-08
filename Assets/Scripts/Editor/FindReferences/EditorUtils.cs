using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace FindReferencesSpace
{
    public class HopeEditorUtils
    {
        static Color defaultcolor = GUI.color;

        public static GUIStyle RichTextStyle
        {
            get
            {
                if (_style == null)
                {
                    _style = new GUIStyle("IN Label");
                    _style.richText = true;
                    _style.padding = new RectOffset(0, 0, 0, 0);
                }

                return _style;
            }
        }

        static GUIStyle _style;

        /// <summary>
        /// 获取场景实例对象的资源路径
        /// </summary>
        /// <param name="instanceObject"></param>
        /// <returns></returns>
        public static string GetAssetPath(GameObject instanceObject)
        {
            UnityEngine.Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(instanceObject);
            string path = AssetDatabase.GetAssetPath(parentObject);
            if (path.IndexOf("Resources") > -1)
            {
                path = path.Substring(path.IndexOf("Resources") + 10);
            }

            path = path.Replace(".prefab", "");
            return path;
        }

        public static void SetColorBegin(Color c)
        {
            defaultcolor = GUI.color;
            GUI.color = c;
        }

        public static void SetColorEnd()
        {
            GUI.color = defaultcolor;
        }

        // 判断是否是文件夹
        public static bool IsDirectory(string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            return string.IsNullOrEmpty(extension);
        }

        public static string DrawPath(string label, string targetPath)
        {
            var rect = EditorGUILayout.GetControlRect(true, 14 * targetPath.Split('\n').Length, GUILayout.Width(500));

            targetPath = EditorGUI.TextField(rect, label, targetPath);
            if ((Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragExited) &&
                rect.Contains(Event.current.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (Event.current.type == EventType.DragUpdated)
                {
                    return targetPath;
                }
                else if (!targetPath.Contains(DragAndDrop.paths[0]) && DragAndDrop.paths != null &&
                         DragAndDrop.paths.Length > 0)
                {
                    if (targetPath.Length > 0)
                        targetPath += "\n";
                    targetPath += DragAndDrop.paths[0];
                }
            }

            return targetPath;
        }

        public static void DisplayProgressBar(string[] files, System.Action<int> actionCallback,
            System.Action finishCallback = null, string title = "匹配资源中")
        {
            int startIndex = 0;
            DateTime cachedTime = DateTime.Now;
            EditorApplication.update = delegate()
            {
                if (files.Length <= 0)
                {
                    EditorApplication.update = null;
                    finishCallback?.Invoke();
                    return;
                }

                string file = files[startIndex];
                bool isCancel =
                    EditorUtility.DisplayCancelableProgressBar(title, file, (float) startIndex / (float) files.Length);

                actionCallback(startIndex);

                startIndex++;
                if (isCancel || startIndex >= files.Length)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    startIndex = 0;

                    TimeSpan ts = DateTime.Now - cachedTime;
                    Debug.Log(" 本次  花了: " + (ts.TotalMilliseconds * 1.0f / 1000) + " 秒，unity才缓过来");

                    finishCallback?.Invoke();
                }
            };
        }

        public static string GetObjectGUID(UnityEngine.Object obj)
        {
            string tmpPath = AssetDatabase.GetAssetPath(obj);
            string tmpGUID = AssetDatabase.AssetPathToGUID(tmpPath);
            return tmpGUID;
        }

        public static string GetObjectPath(UnityEngine.Object obj)
        {
            string tmpPath = AssetDatabase.GetAssetPath(obj);
            return tmpPath;
        }
    }
}

#endif
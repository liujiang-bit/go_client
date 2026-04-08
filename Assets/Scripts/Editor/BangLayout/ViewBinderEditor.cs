//
// Author:  Johance
//
using GameFramework;
using Skyunion;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ViewBinder))]
//[CanEditMultipleObjects]
public class BangLayoutEditor : Editor
{
    private BangLayoutCompment _layout;

    private Vector2Int GetGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetMainGameView.Invoke(null, null);
        var gameView = (UnityEditor.EditorWindow)Res;
        var prop = gameView.GetType().GetProperty("currentGameViewSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var gvsize = prop.GetValue(gameView, new object[0] { });
        var gvSizeType = gvsize.GetType();

        var debug_h = (int)gvSizeType.GetProperty("height", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0] { });
        var debug_w = (int)gvSizeType.GetProperty("width", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(gvsize, new object[0] { });

        return new Vector2Int(debug_w, debug_h);
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var viewBinder = target as ViewBinder;
        RectTransform gui = viewBinder.GetComponent<RectTransform>();
        if (gui == null)
            return;

        BangLayoutCompment layout = gui.GetComponent<BangLayoutCompment>();
        int bangLayout = 0;
        if (layout)
        {
            bangLayout = layout.BangLayout;
        }
        
        if (GetGameViewSize().Equals(new Vector2Int(2436, 1125)))
        {
            GUILayout.Label("刘海屏适配");
        }
        else
        {
            GUILayout.Label("刘海屏适配(必须设置分辨率为 2436*1125)");
            return;
        }
        var uiRoot = gui.GetComponentInParent<CanvasScaler>();
        bool bShowBang = false;
        if (uiRoot != null)
        {
            string strFrameName = "IphoneXFrame";
            Transform ipxFrame = uiRoot.transform.Find(strFrameName);            
            bool bValue = ipxFrame != null;
            bShowBang = bValue;
            if (GUILayout.Toggle(bValue, "IphoneX边框预览") != bValue)
            {
                if (bValue)
                {
                    DestroyImmediate(ipxFrame.gameObject);
                }
                else
                {
                    GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/BundleAssets/UI/Bang/IphoneXFrame.prefab");
                    if (asset != null)
                    {
                        GameObject iphoneXFrame = Instantiate(asset, uiRoot.transform) as GameObject;
                        iphoneXFrame.name = strFrameName;
                    }
                }
            }
        }

        if (!bShowBang)
            return;

        foreach (BangLayoutCompment.BangLayoutStyle item in Enum.GetValues(typeof(BangLayoutCompment.BangLayoutStyle)))
        {
            if ((int)item > 0)
            {
                FieldInfo field = item.GetType().GetField(item.ToString());
                object[] objs = field.GetCustomAttributes(typeof(BangLayoutStyleEditor), false);    //获取描述属性
                if (objs != null && objs.Length > 0)
                {
                    BangLayoutStyleEditor styleAttribute = (BangLayoutStyleEditor)objs[0];
                    if(gui.GetComponent(styleAttribute.CompmentType) != null)
                    {
                        bool bValue = (bangLayout & (int)item) > 0;
                        if (GUILayout.Toggle(bValue, styleAttribute.Name) != bValue)
                        {
                            if (layout == null)
                            {
                                layout = gui.gameObject.AddComponent<BangLayoutCompment>();
                            }
                            layout.SetStyle(item, !bValue);
                            //if (layout.BangLayout == 0)
                            //{
                            //    DestroyImmediate(layout, true);
                            //    break;
                            //}
                        }
                    }
                }
            }
        }
    }
}
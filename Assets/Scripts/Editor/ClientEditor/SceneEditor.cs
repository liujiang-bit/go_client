using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Client
{
    public class SceneEditor : EditorWindow
    {
        private Vector3 angle;
        
        [MenuItem("Tools/Scene/设置scene角度")]
        static void OpenWnd()
        {
            GetWindow<SceneEditor>().Show();
        }

        private void OnGUI()
        {
            angle = EditorGUILayout.Vector3Field("角度", angle);
            if (GUILayout.Button("设置scene角度"))
            {
                SceneView.lastActiveSceneView.rotation = Quaternion.Euler (angle.x, angle.y, angle.z);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;
using Skyunion;

[CustomEditor(typeof(PolygonImageMask))]
public class PolygonImageMaskEditor : PolygonImageEditor
{
    PolygonImageMask imageMask;
    protected override void OnEnable()
    {
        base.OnEnable();
        imageMask = (PolygonImageMask)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var property = serializedObject.FindProperty("maskSprite");
        EditorGUILayout.PropertyField(property);
        serializedObject.ApplyModifiedProperties();
    }
}

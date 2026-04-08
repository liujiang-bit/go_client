using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.UI;

[CustomEditor(typeof(CartoonImage))]
public class CartoonImageEditor : ImageEditor
{
    private CartoonImage image;

    private SerializedProperty Sprites;

    private SerializedProperty Fps;

    private SerializedProperty AlwaysSetNativeSize;


    private SerializedProperty Switch;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(Sprites,EditorGUIUtility.TrTextContent("序列帧图片"),true);
        EditorGUILayout.PropertyField(Fps, EditorGUIUtility.TrTextContent("帧/秒"));
        EditorGUILayout.PropertyField(Switch, EditorGUIUtility.TrTextContent("自动播放"));
        EditorGUILayout.PropertyField(AlwaysSetNativeSize, EditorGUIUtility.TrTextContent("动态设置大小"));

        serializedObject.ApplyModifiedProperties();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        image = (CartoonImage)target;

        Sprites = serializedObject.FindProperty("m_sprites");
        Fps = serializedObject.FindProperty("m_fps");
        Switch = serializedObject.FindProperty("m_switch");
        AlwaysSetNativeSize = serializedObject.FindProperty("m_alwaysSetNativeSize");
    }
}

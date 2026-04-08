using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CustomEditor(typeof(PolygonImage))]
public class PolygonImageEditor : ImageEditor
{
    private PolygonImage polygonImage;

    GUIContent m_editOverrideSpriteContent;
    SerializedProperty m_editOverrideSprite;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_editOverrideSprite, m_editOverrideSpriteContent);
        if (EditorGUI.EndChangeCheck())
        {
            var newSprite = m_editOverrideSprite.objectReferenceValue as Sprite;
            if (newSprite)
            {
                polygonImage.m_fakeSprite = newSprite;
                polygonImage.SetAllDirty();
            }
            else
            {
                polygonImage.m_fakeSprite = null;
                polygonImage.SetAllDirty();
            }
            EditorUtility.SetDirty(polygonImage);
        }
        serializedObject.ApplyModifiedProperties();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        polygonImage = (PolygonImage)target;

        m_editOverrideSpriteContent = EditorGUIUtility.TrTextContent("仅编辑器可见的图片");
        m_editOverrideSprite = serializedObject.FindProperty("m_fakeSprite");
    }
}

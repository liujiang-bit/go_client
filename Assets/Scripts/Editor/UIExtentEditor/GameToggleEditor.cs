using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CustomEditor(typeof(GameToggle))]
public class GameToggleEditor : ToggleEditor
{
    private GameToggle gameToggle;

    SerializedProperty AutoPlaySound;
    SerializedProperty SoundAssetName;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(gameToggle != null)
        {
            AutoPlaySound = serializedObject.FindProperty("AutoPlaySound");
            SoundAssetName = serializedObject.FindProperty("SoundAssetName");
            serializedObject.Update();

            AutoPlaySound.boolValue =  GUILayout.Toggle(gameToggle.AutoPlaySound,"自动播放通用按钮音效");
            SoundAssetName.stringValue = EditorGUILayout.TextField("通用按钮音效资源", gameToggle.SoundAssetName);

            serializedObject.ApplyModifiedProperties();
        }

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        gameToggle = (GameToggle)target;
    }
}

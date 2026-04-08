//
// Author:  Johance
//
using Client;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(AnimationBase), true)]
public class AnimationBaseEditor : Editor
{
    private Texture2D [] states;
    public override void OnInspectorGUI()
    {
        var Target = target as AnimationBase;
        if(Target.m_state != null && Target.m_state.Length > 0)
        {
            if(states == null || states.Length != Target.m_state.Length)
            {
                states = new Texture2D[Target.m_state.Length];
            }

            for(int i = 0; i < Target.m_state.Length; i++)
            {
                if(states[i] != null)
                {
                    states[i] = EditorGUILayout.ObjectField(states[i].name, GetTextureFromState(i), typeof(Texture2D), false) as Texture2D;
                }
                else
                {
                    states[i] = EditorGUILayout.ObjectField(i.ToString(), GetTextureFromState(i), typeof(Texture2D), false) as Texture2D;
                }
            }
            if (GUILayout.Button("Reload Sprite"))
            {
                for(int i = 0; i < Target.m_state.Length; i++)
                {
                    UpdateSprites(i, Target.m_state[i]);
                }
                EditorUtility.SetDirty(target);
            }
        }
        base.OnInspectorGUI();
    }

    Texture2D GetTextureFromState(int nState)
    {
        var Target = target as AnimationBase;
        if (states[nState] == null && Target.m_state != null && Target.m_state.Length > nState)
        {
            var dir = Target.m_state[nState].m_direction;
            if (dir != null && dir.Length > 0)
            {
                var sprites = dir[0].m_sprite_array;
                if(sprites != null && sprites.Length > 0)
                {
                    var sprite = sprites[0];
                    if(sprite != null)
                    {
                        return sprite.texture;
                    }
                }
            }
            return null;
        }
        return states[nState];
    }
    void UpdateSprites(int nState, SpriteStateArray spriteStateGroup)
    {
        if(states[nState] == null)
        {
            spriteStateGroup.m_direction = null;
            return;
        }
        var path = AssetDatabase.GetAssetPath(states[nState]);
        var sprites = AssetDatabase.LoadAllAssetsAtPath(path);
        Dictionary<string, Sprite> dics = new Dictionary<string, Sprite>();
        foreach(var obj in sprites)
        {
            var sprite = obj as Sprite;
            if(sprite != null)
            {
                dics.Add(sprite.name, sprite);
            }
        }

        var prefix = states[nState].name;
        //Target.m_state[nState] = new SpriteStateGroup();
        spriteStateGroup.m_direction = new SpriteArray[5];
        if(spriteStateGroup.m_update_rate == 0.0f)
        {
            spriteStateGroup.m_update_rate = 0.035f;
        }
        for(int i = 0; i < 5; i++)
        {
            List<Sprite> dirs = new List<Sprite>();
            for(int j = 0; j < dics.Count; j++)
            {
                Sprite sprite;
                if(dics.TryGetValue($"{prefix}_{i+1}_{j.ToString("00")}", out sprite))
                {
                    dirs.Add(sprite);
                }
                else
                {
                    break;
                }
            }
            spriteStateGroup.m_direction[i] = new SpriteArray();
            spriteStateGroup.m_direction[i].m_sprite_array = dirs.ToArray();
        }
    }
}
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

[CustomEditor(typeof(GizmosTool))]
public class GizmosToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var Target = target as GizmosTool;
        Target.m_initDensity = EditorGUILayout.Slider("Densiity", Target.m_initDensity, 0, 1);
        Target.m_treeVolumePerSquareMeter = EditorGUILayout.IntSlider("treeVolume PerSquareMeter", Target.m_treeVolumePerSquareMeter, 0, 10);
        Target.m_prohibitRadius = EditorGUILayout.Slider("Prohibit Radius", Target.m_prohibitRadius, 0, 10);

        if (GUILayout.Button("Refresh"))
        {
            while(Target.transform.childCount > 0)
            {
                DestroyImmediate(Target.transform.GetChild(0).gameObject);
            }

            Target.ClearTreePoints();

            List<Vector3> tree = new List<Vector3>();
            var line = Target.GetComponent<LineRenderer>();
            if (line != null)
            {
                Mesh mesh = new Mesh();
                line.BakeMesh(mesh, true);

                for (int i = 0; i < mesh.triangles.Length; i++)
                {
                    var pos = mesh.vertices[mesh.triangles[i]];
                    pos.y = 0;
                    tree.Add(pos);
                }
            }
            else
            {
                Target.transform.position = Vector3.zero;
                Target.transform.parent.position = Vector3.zero;
                tree.Add(new Vector3(-4, 0, -4));
                tree.Add(new Vector3(4, 0, 4));
                tree.Add(new Vector3(4, 0, -4));

                tree.Add(new Vector3(-4, 0, -4));
                tree.Add(new Vector3(-4, 0, 4));
                tree.Add(new Vector3(4, 0, 4));
            }

            Target.SetData(tree);
            Target.SetDensity(Target.m_initDensity);
            Target.SetTrees(Target.m_treeVolumePerSquareMeter, Target.m_prohibitRadius);
            var trees = Target.GetTreesPoint();
            Target.AddTreePoints(trees);

            for (int i = 0; i < trees.Count; i++)
            {
                GameObject treeSprite = new GameObject(i.ToString());
                treeSprite.transform.SetParent(Target.transform);
                var sprite = treeSprite.AddComponent<SpriteRenderer>();
                sprite.sprite = Target.m_treeSprites[UnityEngine.Random.Range(0, Target.m_treeSprites.Count)];
                sprite.transform.position = trees[i];
                sprite.transform.eulerAngles = new Vector3(45f, 0f, 0f);
                float scale = UnityEngine.Random.Range(Target.m_treeMinScale, Target.m_treeMaxScale);
                sprite.transform.localScale = new Vector3(scale, scale, scale);
            }

            if (line == null)
            {
                float fDistance = 10.0f;
                Transform centerObject = null;
                foreach (Transform child in Target.transform)
                {
                    float dst = Vector3.Distance(child.position, Vector3.zero);
                    if (dst < fDistance)
                    {
                        fDistance = dst;
                        centerObject = child;
                    }
                }
                if (centerObject != null)
                {
                    centerObject.gameObject.name = "center";
                    float scale = UnityEngine.Random.Range(Target.m_treeMaxScale, Target.m_treeMaxScale);
                    centerObject.transform.localScale = new Vector3(scale, scale, scale);
                    centerObject.transform.position = new Vector3(0, 0, 0);
                }
            }

                Debug.Log($"Tree Number:{trees.Count}");
            EditorUtility.SetDirty(Target);
        }
    }
}
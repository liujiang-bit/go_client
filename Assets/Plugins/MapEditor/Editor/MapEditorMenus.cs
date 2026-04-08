using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace IG.MapEditor
{
    public static class MapEditorMenus
    {
        [MenuItem("Tools/OpenMapEditor", false)]
        public static void OpenEditorWindow()
        {
            MapEditor.MenuOpenWindow();
        }
    }
}
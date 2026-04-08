using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Skyunion
{
    class CustomBuildProcessor : IPostBuildPlayerScriptDLLs
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostBuildPlayerScriptDLLs(BuildReport report)
        {
            var hotfixDllTargetPath = Path.Combine(Application.streamingAssetsPath, "Hotfix.dll.bytes");
            var hotfixPdbTargetPath = Path.Combine(Application.streamingAssetsPath, "Hotfix.pdb.bytes");
            var mode = (HotfixMode)EditorPrefs.GetInt("HofixService_HofixMode", (int)HotfixMode.NativeCode);
            if (BuildPipeline.isBuildingPlayer)
            {
                if (File.Exists(hotfixDllTargetPath))
                {
                    File.Delete(hotfixDllTargetPath);
                }
                if (File.Exists(hotfixPdbTargetPath))
                {
                    File.Delete(hotfixPdbTargetPath);
                }
                if (mode == HotfixMode.ILRT)
                {
                    string hotfixDllSourcePath = Path.Combine(System.Environment.CurrentDirectory, "Library/PlayerScriptAssemblies/Hotfix.dll");
                    File.Copy(hotfixDllSourcePath, hotfixDllTargetPath, true);
                    CoreUtils.EncryptFile(hotfixDllTargetPath);
                    Debug.Log($"Hotfix Copy To :\n{hotfixDllTargetPath}");
                }
            }
        }
    }
}
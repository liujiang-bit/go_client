using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Skyunion;

public static class ILRuntimeCLRBinding
{
    [MenuItem("Skyunion/ILRuntime/Generate CLR Binding Code by Analysis")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        var hotfixDllSourcePath = Path.Combine(System.Environment.CurrentDirectory, "Library/ScriptAssemblies/Hotfix.dll");
        using (System.IO.FileStream fs = new System.IO.FileStream(hotfixDllSourcePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            domain.LoadAssembly(fs);

            //Crossbind Adapter is needed to generate the correct binding code
            ILRTBind.ILRTBind.Init(domain);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, "Assets/Scripts/ILRTBind/Generated/AutoType");
        }

        AssetDatabase.Refresh();

    }
    /*[MenuItem("Skyunion/ILRuntime/Delegate拷贝")]
    public static void CopyDelegate()
    {
        var text = HotfixCodeGen.GetRefreshDelegate();
        TextEditor te = new TextEditor();
        te.text = text;
        te.SelectAll();
        te.Copy();
        Debug.Log("已经赋值到粘贴板");
    }*/
}

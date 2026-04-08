using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using UnityEngine;




public class lbsdiff
{
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
	 [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
     public static extern int bsdiff2(string old_file, string new_file, string patch_file);
	 [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
     public static extern int bspatch2(string old_file, string new_file, string patch_file);
#else
    private const string libname = "libbsdiff";
    [DllImport(libname, EntryPoint = "bsdiff2", CallingConvention = CallingConvention.Cdecl)]
    public static extern int bsdiff2(string old_file, string new_file, string patch_file);
    [DllImport(libname, EntryPoint = "bspatch2", CallingConvention = CallingConvention.Cdecl)]
    public static extern int bspatch2(string old_file, string new_file, string patch_file);
#endif
}

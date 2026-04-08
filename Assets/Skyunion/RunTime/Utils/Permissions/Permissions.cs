/* ==============================================================================
 * 功能描述：UnityPlayer Android 适配器
 * 创 建 者：Johance
 * 邮箱：421465201@qq.com
 * 创建日期：2019.5.21
 * ==============================================================================*/
using System;
using System.Collections;
using UnityEngine;
// We need this one for importing our IOS functions
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Skyunion;

public class Permissions
{
#if UNITY_ANDROID && !UNITY_EDITOR
    public class PermissionActivityListener : AndroidJavaProxy
    {
        public delegate void Listener1(int requestCode, String[] permissions, int[] grantResults);
        public PermissionActivityListener(Listener1 callback)
            : base("com.unity.androidplugin.PermissionActivityListener")
        {
            m_callback1 = callback;
        }

        void onRequestPermissionsResult(int requestCode, string [] permissions, int [] grantResults)
        {
            if (m_callback1 != null)
            {
                Debug.Log("onRequestPermissionsResult");
                m_callback1(requestCode, permissions, grantResults);
            }
        }
        public static Listener1 m_callback1;
    }

    private static int REQUEST_PERMISSION_CODE = 100001;
    public static void requestPermissions(string[] permissions, PermissionActivityListener.Listener1 listener)
    {
        var javaClass = new AndroidJavaClass("com.unity.androidplugin.PermissionActivity");
        javaClass.CallStatic("requestPermissions", permissions, REQUEST_PERMISSION_CODE, new PermissionActivityListener(listener));
    }

    public static bool IsPermissionsGranted(int requestCode, string[] permissions, int[] grantResults)
    {
        if (requestCode == REQUEST_PERMISSION_CODE)
        {
            bool isAllGranted = true;
            for (int i = 0; i < grantResults.Length; i++)
            {
                if (grantResults[i] != 0)
                {
                    isAllGranted = false;
                    break;
                }
            }
            return isAllGranted;
        }
        return false;
    }
    public static void GotoSetting(String message)
    {
        var javaClass = new AndroidJavaClass("com.unity.androidplugin.PermissionActivity");
        javaClass.CallStatic("GotoSetting", message);
    }
#endif
}
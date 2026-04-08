package com.unity.androidplugin;

public interface PermissionActivityListener
{
    public void onRequestPermissionsResult( int requestCode, String[] permissions, int[] grantResults );
}
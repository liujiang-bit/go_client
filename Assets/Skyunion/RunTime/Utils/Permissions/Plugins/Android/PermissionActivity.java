package com.unity.androidplugin;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.Settings;
import androidx.annotation.RequiresApi;
import androidx.core.app.ActivityCompat;
import androidx.fragment.app.FragmentActivity;

import com.unity3d.player.UnityPlayer;

public class PermissionActivity extends FragmentActivity {
	
	public static final String TAG = "PermissionActivity";
	@RequiresApi(api = Build.VERSION_CODES.M)
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		//requestPermissions(m_Permissions, 0);
		if(m_Permissions==null){
			return;
		}
		ActivityCompat.requestPermissions(PermissionActivity.this, m_Permissions, 0);
	}
	private  static PermissionActivityListener mPermissionActivityListener;
	private static String[] m_Permissions = null;
	public void setActivityListener(PermissionActivityListener listener)
	{
		mPermissionActivityListener = listener;
	}
	public static void requestPermissions(String[] permissions, int requestCode, PermissionActivityListener listener)
	{
		mPermissionActivityListener = listener;
		m_Permissions = permissions;
		Intent intent = new Intent();
		intent.setClass(UnityPlayer.currentActivity, PermissionActivity.class);
		UnityPlayer.currentActivity.startActivityForResult(intent, requestCode);
	}
	@Override
	public void onRequestPermissionsResult( int requestCode, String[] permissions, int[] grantResults )
	{
		if(mPermissionActivityListener != null)
		{
			mPermissionActivityListener.onRequestPermissionsResult(requestCode, permissions, grantResults);
		}
		finish();
	}
	public static void GotoSetting(String message)
	{
		AlertDialog.Builder builder = new AlertDialog.Builder(UnityPlayer.currentActivity);
		builder.setMessage(message);
		builder.setPositiveButton("Go", new DialogInterface.OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				Intent intent = new Intent();
				intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
				intent.addCategory(Intent.CATEGORY_DEFAULT);
				intent.setData(Uri.parse("package:" + UnityPlayer.currentActivity.getPackageName()));
				intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
				intent.addFlags(Intent.FLAG_ACTIVITY_NO_HISTORY);
				intent.addFlags(Intent.FLAG_ACTIVITY_EXCLUDE_FROM_RECENTS);
				UnityPlayer.currentActivity.startActivity(intent);
			}
		});
		builder.setNegativeButton("Cancel", null);
		builder.show();
	}
}

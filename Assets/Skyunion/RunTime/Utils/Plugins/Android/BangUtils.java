package com.unity.androidplugin;
import android.app.Activity;
import android.content.Context;
import android.content.res.Configuration;
import android.os.Build;
import android.provider.Settings;
import android.util.Log;
import android.view.DisplayCutout;
import android.view.Surface;
import android.view.View;
import android.view.WindowInsets;

import java.lang.reflect.Method;

public class BangUtils
{
	public static boolean hasNotchAtHuawei(Context context) {
		boolean ret = false;
		try {
			ClassLoader classLoader = context.getClassLoader();
			Class HwNotchSizeUtil = classLoader.loadClass("com.huawei.android.util.HwNotchSizeUtil");
			Method get = HwNotchSizeUtil.getMethod("hasNotchInScreen");
			ret = (boolean) get.invoke(HwNotchSizeUtil);
		} catch (ClassNotFoundException e) {
//			Log.d("Notch", "hasNotchAtHuawei ClassNotFoundException");
		} catch (NoSuchMethodException e) {
//			Log.d("Notch", "hasNotchAtHuawei NoSuchMethodException");
		} catch (Exception e) {
//			Log.d("Notch", "hasNotchAtHuawei Exception");
		} finally {
			return ret;
		}
	}
    public static final String DISPLAY_NOTCH_STATUS = "display_notch_status";
	//获取刘海尺寸：width、height
    //int[0]值为刘海宽度 int[1]值为刘海高度
	// <meta-data android:name="android.notch_support" android:value="true"/>
    public static int[] getNotchSizeAtHuawei(Context context) {
        int[] ret = new int[]{0, 0};
        try {
            ClassLoader cl = context.getClassLoader();
            Class HwNotchSizeUtil = cl.loadClass("com.huawei.android.util.HwNotchSizeUtil");
            Method get = HwNotchSizeUtil.getMethod("getNotchSize");
            ret = (int[]) get.invoke(HwNotchSizeUtil);

            int mIsNotchSwitchOpen = Settings.Secure.getInt(context.getContentResolver(),DISPLAY_NOTCH_STATUS, 0);
            if(mIsNotchSwitchOpen == 1)
            {
                ret[0] = 0;
                ret[1] = 0;
            }

            //Log.d("getNotchSizeAtHuawei", String.format("%d, %d", ret[0], ret[1]));

        } catch (ClassNotFoundException e) {
//            Log.d("Notch", "getNotchSizeAtHuawei ClassNotFoundException");
        } catch (NoSuchMethodException e) {
//            Log.d("Notch", "getNotchSizeAtHuawei NoSuchMethodException");
        } catch (Exception e) {
//            Log.d("Notch", "getNotchSizeAtHuawei Exception");
        } finally {
            return ret;
        }
    }
    public static final int VIVO_NOTCH = 0x00000020;//是否有刘海
    public static final int VIVO_FILLET = 0x00000008;//是否有圆角

    public static boolean hasNotchAtVivo(Context context) {
        boolean ret = false;
        try {
            ClassLoader classLoader = context.getClassLoader();
            Class FtFeature = classLoader.loadClass("android.util.FtFeature");
            Method method = FtFeature.getMethod("isFeatureSupport", int.class);
            ret = (boolean) method.invoke(FtFeature, VIVO_NOTCH);
        } catch (ClassNotFoundException e) {
//            Log.d("Notch", "hasNotchAtVivo ClassNotFoundException");
        } catch (NoSuchMethodException e) {
//            Log.d("Notch", "hasNotchAtVivo NoSuchMethodException");
        } catch (Exception e) {
//            Log.d("Notch", "hasNotchAtVivo Exception");
        } finally {
            return ret;
        }
    }
    public static boolean hasNotchAtOPPO(Context context) {
        return context.getPackageManager().hasSystemFeature("com.oppo.feature.screen.heteromorphism");
    }
    public static boolean hasMIUINotchInScreen() {
        try {
            Class<?> clz = Class.forName("android.os.SystemProperties");
            Method get = clz.getMethod("getInt", String.class, String.class);
            return (int) get.invoke(clz, "ro.miui.notch", -1) == 1;
        } catch (Exception e) {
            //e.printStackTrace();
//            Log.d("Notch", "hasMIUINotchInScreen Exception");
        }
        return false;
    }
    public static int getMIUINotchSize(Context context) {
        int resourceId = context.getResources().getIdentifier("notch_height", "dimen", "android");
        if (resourceId > 0) {
            return context.getResources().getDimensionPixelSize(resourceId);
        }
        return 0;
    }

    public static String getSafeAreaInsets(Activity activity)
	{
        Context context = activity;
	    String strRet = "0, 0, 0, 0, 0";

        /*if (android.os.Build.VERSION.SDK_INT >= Build.VERSION_CODES.P) {
            View decorView = activity.getWindow().getDecorView();
            WindowInsets windowInsets = null;
            windowInsets = decorView.getRootWindowInsets();
            if (windowInsets != null) {
                DisplayCutout displayCutout = windowInsets.getDisplayCutout();
                strRet = String.format("%d, %d, %d, %d, 1", displayCutout.getSafeInsetLeft(), displayCutout.getSafeInsetRight(), displayCutout.getSafeInsetTop(), displayCutout.getSafeInsetBottom());
                //return strRet;
            }
        }*/

        int screenRotation = activity.getWindowManager().getDefaultDisplay().getRotation();
		// 华为判断
        try {
            if (hasNotchAtHuawei(context)) {
                Log.d("SafeArea", "判断华为");
                int[] area = getNotchSizeAtHuawei(context);
				if(screenRotation == Surface.ROTATION_0) {
                    strRet = String.format("0, 0, %d, 0, 1", area[1]);
                }
                else if(screenRotation == Surface.ROTATION_180) {
                    strRet = String.format("0, 0, 0, %d, 1", area[1]);
                }
                else if(screenRotation == Surface.ROTATION_90) {
                    strRet = String.format("%d, 0, 0, 0, 1", area[1]);
                }
                else if(screenRotation == Surface.ROTATION_270) {
                    strRet = String.format("0, %d, 0, 0, 1", area[1]);
                }
            }
            // vivo判断
            else if (hasNotchAtVivo(context)) {
                Log.d("SafeArea", "判断vivo");
                if(screenRotation == Surface.ROTATION_0) {
                    strRet = String.format("0, 0, %d, 0, 1", 27);
                }
                else if(screenRotation == Surface.ROTATION_180) {
                    strRet = String.format("0, 0, 0, %d, 1", 27);
                }
                else if(screenRotation == Surface.ROTATION_90) {
                    strRet = String.format("%d, 0, 0, 0, 1", 27);
                }
                else if(screenRotation == Surface.ROTATION_270) {
                    strRet = String.format("0, %d, 0, 0, 1", 27);
                }
            }
            // opple 判断
            else if (hasNotchAtOPPO(context)) {
                Log.d("SafeArea", "判断opple");
                if(screenRotation == Surface.ROTATION_0) {
                    strRet = String.format("0, 0, %d, 0, 1", 80);
                }
                else if(screenRotation == Surface.ROTATION_180) {
                    strRet = String.format("0, 0, 0, %d, 1", 80);
                }
                else if(screenRotation == Surface.ROTATION_90) {
                    strRet = String.format("%d, 0, 0, 0, 1", 80);
                }
                else if(screenRotation == Surface.ROTATION_270) {
                    strRet = String.format("0, %d, 0, 0, 1", 80);
                }
            }
            else if (hasMIUINotchInScreen()) {
                Log.d("SafeArea", "判断小米");
                int nTop = getMIUINotchSize(context);
                if(screenRotation == Surface.ROTATION_0) {
                    strRet = String.format("0, 0, %d, 0, 1", nTop);
                }
                else if(screenRotation == Surface.ROTATION_180) {
                    strRet = String.format("0, 0, 0, %d, 1", nTop);
                }
                else if(screenRotation == Surface.ROTATION_90) {
                    strRet = String.format("%d, 0, 0, 0, 1", nTop);
                }
                else if(screenRotation == Surface.ROTATION_270) {
                    strRet = String.format("0, %d, 0, 0, 1", nTop);
                }
            }
        }
        catch(Throwable albe){}

        //Log.d("SafeArea", strRet);
		return strRet;
	}
}

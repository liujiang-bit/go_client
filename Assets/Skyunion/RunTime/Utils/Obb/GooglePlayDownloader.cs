using UnityEngine;
using System;

public interface IGooglePlayObbDownloader
{
    string PublicKey { get; set; }

    string GetExpansionFilePath();
    string GetMainOBBPath();
    string GetPatchOBBPath();
    void FetchOBB();
}

public class GooglePlayObbDownloadManager
{
    private static AndroidJavaClass m_AndroidOSBuildClass = new AndroidJavaClass("android.os.Build");
    private static IGooglePlayObbDownloader m_Instance;

    public static IGooglePlayObbDownloader GetGooglePlayObbDownloader()
    {
        if (m_Instance != null)
            return m_Instance;

        if (!IsDownloaderAvailable())
            return null;

        m_Instance = new GooglePlayObbDownloader();
        return m_Instance;
    }

    public static bool IsDownloaderAvailable()
    {
        return m_AndroidOSBuildClass.GetRawClass() != IntPtr.Zero;
    }
}


/*  Demo code
using UnityEngine;
using System.Collections;

public class DownloadObbExample : MonoBehaviour
{
    private IGooglePlayObbDownloader m_obbDownloader;
    void Start()
    {
        m_obbDownloader = GooglePlayObbDownloadManager.GetGooglePlayObbDownloader();
        m_obbDownloader.PublicKey = ""; // YOUR PUBLIC KEY HERE
    }	

    void OnGUI()
    {
        if (!GooglePlayObbDownloadManager.IsDownloaderAvailable())
        {
            GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Use GooglePlayDownloader only on Android device!");
            return;
        }
        
        string expPath = m_obbDownloader.GetExpansionFilePath();
        if (expPath == null)
        {
            GUI.Label(new Rect(10, 10, Screen.width-10, 20), "External storage is not available!");
        }
        else
        {
            var mainPath = m_obbDownloader.GetMainOBBPath();
            var patchPath = m_obbDownloader.GetPatchOBBPath();
            
            GUI.Label(new Rect(10, 10, Screen.width-10, 20), "Main = ..."  + ( mainPath == null ? " NOT AVAILABLE" :  mainPath.Substring(expPath.Length)));
            GUI.Label(new Rect(10, 25, Screen.width-10, 20), "Patch = ..." + (patchPath == null ? " NOT AVAILABLE" : patchPath.Substring(expPath.Length)));
            if (mainPath == null || patchPath == null)
                if (GUI.Button(new Rect(10, 100, 100, 100), "Fetch OBBs"))
                    m_obbDownloader.FetchOBB();
        }

    }
}
*/

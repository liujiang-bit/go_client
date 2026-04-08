using UnityEditor; 
[InitializeOnLoad]
public class GlobalConfig
{
    static GlobalConfig()
    {
        PlayerSettings.Android.keystorePass = "skyunion";
        PlayerSettings.Android.keyaliasName = "oc";
        PlayerSettings.Android.keyaliasPass = "skyunion";
    }
}
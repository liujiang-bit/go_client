using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Skyunion
{
    public class VersionUtil
    {
        public static long GetVersionNumber()
        {
            var version = Application.version;
            var versions = version.Split('.');
            int v1 = versions.Length > 0 ? int.Parse(versions[0]) : 0;
            int v2 = versions.Length > 1 ? int.Parse(versions[1]) : 0;
            int v3 = versions.Length > 2 ? int.Parse(versions[2]) : 0;
            int v4 = HotfixNumber;

            return v4 + v3 * 100 + v2 * 10000 + v1 * 1000000;
        }

        public static string GetVersionStr()
        {
            return GetVersionStr(GetVersionNumber());
        }

        public static string GetVersionStr(long versionNumber)
        {
            long number = versionNumber;

            long v1 = number / 1000000;
            long v2 = number % 1000000 / 10000;
            long v3 = number % 10000 / 100;
            long v4 = number % 100;

            return $"{v1}.{v2}.{v3}.{v4}";
        }

        public static long GetVersionNumber(string version)
        {
            var versions = version.Split('.');
            int v1 = versions.Length > 0 ? int.Parse(versions[0]) : 0;
            int v2 = versions.Length > 1 ? int.Parse(versions[1]) : 0;
            int v3 = versions.Length > 2 ? int.Parse(versions[2]) : 0;
            int v4 = versions.Length > 3 ? int.Parse(versions[3]) : 0;

            return v4 + v3 * 100 + v2 * 10000 + v1 * 1000000;
        }

        public static readonly string HotfixVersionPath = $"{Application.persistentDataPath}/{Application.version}";
        public static readonly string HotFixKey = $"HotFixVersion_{Application.version}";
        public static string HotfixRuntimePath
        {
            get
            {
                return $"{HotfixVersionPath}/{HotfixNumber}";
            }
        }

        public static string HotfixAddressablesPath
        {
            get
            {
                return Path.Combine(HotfixRuntimePath, Addressables.StreamingAssetsSubFolder);
            }
        }
        public static bool HasHotfix
        {
            get
            {
                return HotfixNumber != 0;
            }
        }
        public static int HotfixNumber
        {
            set
            {
                PlayerPrefs.SetInt(HotFixKey, value);
                PlayerPrefs.Save();
            }
            get
            {
                return PlayerPrefs.GetInt(HotFixKey, 0);
            }
        }
        public static string GetPlatform()
        {
            return "" + PlatformMappingService.GetPlatform();
        }
    }
}
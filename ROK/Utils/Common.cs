using Skyunion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ROK
{
    public static class Common
    {
        private class ColliderCache
        {
            public GameObject obj;

            public Collider[] colliders;

            public static ArrayList listofdebugout = new ArrayList();
        }

        private static long currentTimeMillis_19700101000000 = -999L;

        private static Dictionary<string, Common.ColliderCache> s_colliders = new Dictionary<string, Common.ColliderCache>();

        private static bool m_enable_log = false;

        public static float m_previous_lod_Distance = 0f;

        public static char[] DATA_DELIMITER_LEVEL_0 = new char[]
        {
        '\u0001'
        };

        public static char[] DATA_DELIMITER_LEVEL_1 = new char[]
        {
        '\u0002'
        };

        public static char[] DATA_DELIMITER_LEVEL_2 = new char[]
        {
        '\u0003'
        };

        public static char[] DATA_DELIMITER_LEVEL_3 = new char[]
        {
        '\u0004'
        };

        public static char[] DATA_DELIMITER_LEVEL_4 = new char[]
        {
        '\u0005'
        };

        private static float m_smooth_angle = 10f;

        public static float s_ticktick = 1f;

        public static RenderTexture rt = null;

        public static long currentTimeMillis()
        {
            if (Common.currentTimeMillis_19700101000000 == -999L)
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                Common.currentTimeMillis_19700101000000 = dateTime.Ticks / 10000L;
            }
            return DateTime.Now.Ticks / 10000L - Common.currentTimeMillis_19700101000000;
        }

        public static float GetAngle360(Vector3 from_, Vector3 to_)
        {
            if (to_.x >= from_.x)
            {
                return Vector3.Angle(from_, to_);
            }
            return 360f - Vector3.Angle(from_, to_);
        }

        public static float GetAngle360(Vector2 from_, Vector2 to_)
        {
            if (to_.x >= from_.x)
            {
                return Vector2.Angle(from_, to_);
            }
            return 360f - Vector2.Angle(from_, to_);
        }

        public static float GetLodDistance()
        {
            if (Camera.main)
            {
                return WorldCameraImpl.GetInstance().getCurrentCameraDxf();
            }
            return 0f;
        }

        public static float GetCameraFov()
        {
            if (Camera.main)
            {
                return WorldCameraImpl.GetInstance().getCurrentCameraFov();
            }
            return 0f;
        }

        public static float GetCameraDist()
        {
            if (Camera.main)
            {
                return WorldCameraImpl.GetInstance().getCurrentCameraDist();
            }
            return 0f;
        }

        public static float GetPreviousLodDistance()
        {
            return Common.m_previous_lod_Distance;
        }

        public static Vector3[] GetCameraCornors(Camera camera, Plane plane)
        {
            Ray[] array = new Ray[]
            {
            camera.ViewportPointToRay(new Vector3(0f, 0f, 0f)),
            camera.ViewportPointToRay(new Vector3(1f, 0f, 0f)),
            camera.ViewportPointToRay(new Vector3(1f, 1f, 0f)),
            camera.ViewportPointToRay(new Vector3(0f, 1f, 0f))
            };
            Vector3[] array2 = new Vector3[4];
            for (int i = 0; i < 4; i++)
            {
                Ray ray = array[i];
                float d = 0f;
                Vector3 vector = Vector3.zero;
                if (plane.Raycast(ray, out d))
                {
                    vector = ray.origin + ray.direction * d;
                }
                else
                {
                    Ray ray2 = new Ray(ray.origin, -ray.direction);
                    if (plane.Raycast(ray2, out d))
                    {
                        vector = ray2.origin + ray2.direction * d;
                    }
                }
                array2[i] = vector;
            }
            return array2;
        }

        public static Transform FindInParent(Transform trans, string name)
        {
            if (!(trans != null))
            {
                return null;
            }
            if (trans.name == name)
            {
                return trans;
            }
            return Common.FindInParent(trans.parent, name);
        }

        public static void PrintDictionary(Dictionary<int, int> dict)
        {
            foreach (KeyValuePair<int, int> current in dict)
            {
                UnityEngine.Debug.LogError("Key = " + current.Key.ToString() + ", Value = " + current.Value.ToString());
            }
        }

        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Sin(radian), Mathf.Cos(radian));
        }

        public static Vector2 DegreeToVector2(float degree)
        {
            return Common.RadianToVector2(degree * 0.0174532924f);
        }

        public static void SetGameObjectPos(GameObject obj, Vector3 pos)
        {
            obj.transform.position = pos;
        }

        public static Vector3 GetGameObjectLocalPos(GameObject obj)
        {
            return obj.transform.localPosition;
        }

        public static void SetGameObjectLocalPos(GameObject obj, Vector3 pos)
        {
            obj.transform.localPosition = pos;
        }

        public static Vector3 GetGameObjectLocalScale(GameObject obj)
        {
            return obj.transform.localScale;
        }

        public static void SetGameObjectLocalScale(GameObject obj, Vector3 scale)
        {
            obj.transform.localScale = scale;
        }

        public static Vector3 GetGameObjectPos(GameObject obj)
        {
            return obj.transform.position;
        }

        public static void SetComponentPos(Behaviour comp, Vector3 pos)
        {
            comp.transform.position = pos;
        }

        public static Vector3 GetComponentPos(Behaviour comp)
        {
            return comp.transform.position;
        }

        public static Vector3 GetGameObjectLocalEulerAngles(GameObject obj)
        {
            return obj.transform.localEulerAngles;
        }

        public static void SetGameObjectLocalEulerAngles(GameObject obj, Vector3 euler)
        {
            obj.transform.localEulerAngles = euler;
        }

        public static void MySqlLog(string log)
        {
        }

        public static GameObject FindGameObjectChildObj(GameObject obj, string pathName)
        {
            Transform transform = obj.transform.Find(pathName);
            if (transform != null)
            {
                return transform.gameObject;
            }
            return null;
        }

        public static void DestroyAllChildObj(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                CoreUtils.assetService.Destroy(obj.transform.GetChild(i).gameObject);
            }
        }

        public static void GetScreenSize(out int w, out int h)
        {
            w = Screen.width;
            h = Screen.height;
        }

        public static void SetRendererColor(Renderer renderer, string propertyName, Color color)
        {
            renderer.material.SetColor(propertyName, color);
        }

        public static float RealtimeSinceStartup()
        {
            return Time.realtimeSinceStartup;
        }

        public static void GetRectTransformSizeDelta(RectTransform trans, out float x, out float y)
        {
            Vector2 sizeDelta = trans.sizeDelta;
            x = sizeDelta.x;
            y = sizeDelta.y;
        }

        public static void UnpackVector2(Vector2 vec, out float x, out float y)
        {
            x = vec.x;
            y = vec.y;
        }

        public static void UnpackVector3(Vector3 vec, out float x, out float y, out float z)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public static void UnpackVector4(Vector4 vec, out float x, out float y, out float z, out float w)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
            w = vec.w;
        }

        public static Vector3 CalcRayEnterPos(Ray ray, float enter)
        {
            return ray.origin + ray.direction * enter;
        }

        public static Vector3 WorldToViewportPoint(Camera camera, Vector3 pos)
        {
            return camera.WorldToViewportPoint(pos);
        }

        public static bool IsInViewPort(Camera camera, Vector3 pos, float border)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            Vector3 vector = camera.WorldToViewportPoint(pos);
            return vector.x > -border && vector.x < 1f + border && vector.y > -border && vector.y < 1f + border;
        }

        public static bool IsInViewPort2D(Camera camera, float x, float y, float border)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            Vector3 vector = camera.WorldToViewportPoint(new Vector3(x, 0f, y));
            return vector.x > -border && vector.x < 1f + border && vector.y > -border && vector.y < 1f + border;
        }

        public static bool IsInViewPort2DS(Camera camera, float x, float y)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            Vector3 vector = camera.WorldToViewportPoint(new Vector3(x, 0f, y));
            return vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f;
        }

        public static GameObject[] GetChildrenByNamePrefix(GameObject gameObj, string namePrefix)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < gameObj.transform.childCount; i++)
            {
                Transform child = gameObj.transform.GetChild(i);
                if (child.gameObject.name.StartsWith(namePrefix))
                {
                    list.Add(child.gameObject);
                }
            }
            return list.ToArray();
        }

        public static GameObject GetChildrenByName(GameObject gameObj, string name)
        {
            for (int i = 0; i < gameObj.transform.childCount; i++)
            {
                Transform child = gameObj.transform.GetChild(i);
                if (child.gameObject.name == name)
                {
                    return child.gameObject;
                }
            }
            return null;
        }

        public static Vector3 GetColliderWorldPos(GameObject gameObj)
        {
            BoxCollider component = gameObj.GetComponent<BoxCollider>();
            if (component)
            {
                return gameObj.transform.TransformPoint(component.center);
            }
            return Vector3.zero;
        }

        public static float GetRealtimeSinceStartup()
        {
            return Time.realtimeSinceStartup;
        }

        public static float GetUnscaledDeltaTime()
        {
            return Time.unscaledDeltaTime;
        }

        public static Color RGBAToColor(double rgba)
        {
            return Common.RGBAToColor((uint)rgba);
        }

        public static Color RGBAToColor(uint rgba)
        {
            return new Color(((rgba & 4278190080u) >> 24) / 255f, ((rgba & 16711680u) >> 16) / 255f, ((rgba & 65280u) >> 8) / 255f, (rgba & 255u) / 255f);
        }

        public static Color RGBToColor(double rgb)
        {
            return Common.RGBToColor((uint)rgb);
        }

        public static Color RGBToColor(uint rgb)
        {
            return new Color(((rgb & 16711680u) >> 16) / 255f, ((rgb & 65280u) >> 8) / 255f, (rgb & 255u) / 255f, 1f);
        }

        public static void ColorToRGBA(Color color, out uint r, out uint g, out uint b, out uint a)
        {
            r = (uint)(color.r * 255f);
            g = (uint)(color.g * 255f);
            b = (uint)(color.b * 255f);
            a = (uint)(color.a * 255f);
        }

        public static double ToNumber(string text)
        {
            double result = 0.0;
            if (!double.TryParse(text, out result))
            {
                result = double.NaN;
            }
            return result;
        }

        public static bool IsNaN(double value)
        {
            return double.IsNaN(value);
        }

        public static bool IsNaN(float value)
        {
            return float.IsNaN(value);
        }

        public static bool CastLuaBool(object b)
        {
            return b != null && (b.GetType() != typeof(bool) || (bool)b);
        }

        public static Vector2 GetCenterByPos(Vector2 pos, float tile_width)
        {
            float num = tile_width / 2f;
            return new Vector2((float)((int)(pos.x / tile_width)) * tile_width + num, (float)((int)(pos.y / tile_width)) * tile_width + num);
        }

        public static bool RectLeftBottomContains(Rect rect, Vector2 point)
        {
            return point.y >= rect.yMin && point.y < rect.yMax && point.x >= rect.xMin && point.x < rect.xMax;
        }

        public static Vector2[] SmoothTroopLine(Vector2[] path, float smooth_distance = 0.5f)
        {
            if (path.Length >= 3)
            {
                List<Vector2> list = new List<Vector2>();
                list.Add(path[0]);
                for (int i = 1; i < path.Length - 1; i++)
                {
                    Vector2 vector = path[i] - path[i - 1];
                    Vector2 vector2 = path[i + 1] - path[i];
                    if (Vector2.Angle(vector, vector2) >= Common.m_smooth_angle && vector.magnitude > smooth_distance * 2f && vector2.magnitude > smooth_distance * 2f)
                    {
                        vector = vector.normalized;
                        vector2 = vector2.normalized;
                        Vector2 vector3 = path[i] - vector * smooth_distance;
                        Vector2 vector4 = path[i] + vector2 * smooth_distance;
                        list.Add(vector3);
                        list.Add(((vector3 + vector4) / 2f + path[i]) / 2f);
                        list.Add(vector4);
                    }
                    else
                    {
                        list.Add(path[i]);
                    }
                }
                list.Add(path[path.Length - 1]);
                return list.ToArray();
            }
            return path;
        }

        public static Vector2[] SmoothLine(Vector2[] path, float smooth_distance, int iterate = 0, int iterated_times = 0)
        {
            if (path.Length < 3)
            {
                return path;
            }
            List<Vector2> list = new List<Vector2>();
            list.Add(path[0]);
            for (int i = 1; i < path.Length - 1; i++)
            {
                Vector2 a = path[i] - path[i - 1];
                Vector2 a2 = path[i + 1] - path[i];
                if (a.magnitude > smooth_distance * 2f && a2.magnitude > smooth_distance * 2f)
                {
                    a = a.normalized;
                    a2 = a2.normalized;
                    Vector2 item = path[i] - a * smooth_distance;
                    Vector2 item2 = path[i] + a2 * smooth_distance;
                    list.Add(item);
                    list.Add(item2);
                }
                else
                {
                    list.Add(path[i]);
                }
            }
            list.Add(path[path.Length - 1]);
            if (iterate == 0)
            {
                return list.ToArray();
            }
            iterated_times++;
            return Common.SmoothLine(list.ToArray(), smooth_distance / (float)(iterated_times + 1), iterate - 1, 0);
        }

        [Conditional("ENABLE_PROFILING")]
        public static void BeginSample(string tag)
        {
        }

        [Conditional("ENABLE_PROFILING")]
        public static void EndSample()
        {
        }

        public static string GetGameObjectPath(GameObject go)
        {
            if (go == null)
            {
                return string.Empty;
            }
            string text = go.name;
            Transform parent = go.transform.parent;
            while (parent != null)
            {
                text = parent.name + "/" + text;
                parent = parent.parent;
            }
            return text;
        }

        public static void EditorWriteResourceImage(Color[] greens, int w, int h, string mapName)
        {
        }

        public static string RemoveEmoji(string msg, string replaceStr)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return string.Empty;
            }
            return Regex.Replace(msg, "\\uD83D[\\uDC00-\\uDFFF]|\\uD83C[\\uDC00-\\uDFFF]|\\uFFFD", replaceStr);
        }

        public static string RemoveEmojiV2(string msg, string replaceStr, int subLength)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return string.Empty;
            }
            string text = Regex.Replace(msg, "(?:\\uD83D(?:[\\uDC76\\uDC66\\uDC67](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC68(?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:\\u2695\\uFE0F?|\\uD83C[\\uDF93\\uDFEB\\uDF3E\\uDF73\\uDFED\\uDFA4\\uDFA8]|\\u2696\\uFE0F?|\\uD83D[\\uDD27\\uDCBC\\uDD2C\\uDCBB\\uDE80\\uDE92]|\\u2708\\uFE0F?|\\uD83E[\\uDDB0-\\uDDB3]))?)|\\u200D(?:\\u2695\\uFE0F?|\\uD83C[\\uDF93\\uDFEB\\uDF3E\\uDF73\\uDFED\\uDFA4\\uDFA8]|\\u2696\\uFE0F?|\\uD83D(?:\\uDC69\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|\\uDC68\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|[\\uDD27\\uDCBC\\uDD2C\\uDCBB\\uDE80\\uDE92])|\\u2708\\uFE0F?|\\uD83E[\\uDDB0-\\uDDB3]|\\u2764(?:\\uFE0F\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D\\uDC68|\\uDC68)|\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D\\uDC68|\\uDC68)))))?|\\uDC69(?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:\\u2695\\uFE0F?|\\uD83C[\\uDF93\\uDFEB\\uDF3E\\uDF73\\uDFED\\uDFA4\\uDFA8]|\\u2696\\uFE0F?|\\uD83D[\\uDD27\\uDCBC\\uDD2C\\uDCBB\\uDE80\\uDE92]|\\u2708\\uFE0F?|\\uD83E[\\uDDB0-\\uDDB3]))?)|\\u200D(?:\\u2695\\uFE0F?|\\uD83C[\\uDF93\\uDFEB\\uDF3E\\uDF73\\uDFED\\uDFA4\\uDFA8]|\\u2696\\uFE0F?|\\uD83D(?:\\uDC69\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|[\\uDD27\\uDCBC\\uDD2C\\uDCBB\\uDE80\\uDE92])|\\u2708\\uFE0F?|\\uD83E[\\uDDB0-\\uDDB3]|\\u2764(?:\\uFE0F\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]|[\\uDC68\\uDC69])|\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]|[\\uDC68\\uDC69])))))?|[\\uDC74\\uDC75](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC6E(?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|\\uDD75(?:(?:\\uFE0F(?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?|\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDC82\\uDC77](?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|\\uDC78(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC73(?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|\\uDC72(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC71(?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDC70\\uDC7C](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE4D\\uDE4E\\uDE45\\uDE46\\uDC81\\uDE4B\\uDE47\\uDC86\\uDC87\\uDEB6](?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDC83\\uDD7A](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC6F(?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?|[\\uDEC0\\uDECC](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD74(?:(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F))?|\\uDDE3\\uFE0F?|[\\uDEA3\\uDEB4\\uDEB5](?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDCAA\\uDC48\\uDC49\\uDC46\\uDD95\\uDC47\\uDD96](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD90(?:(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F))?|[\\uDC4C-\\uDC4E\\uDC4A\\uDC4B\\uDC4F\\uDC50\\uDE4C\\uDE4F\\uDC85\\uDC42\\uDC43](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC41(?:(?:\\uFE0F(?:\\u200D\\uD83D\\uDDE8\\uFE0F?)?|\\u200D\\uD83D\\uDDE8\\uFE0F?))?|[\\uDDE8\\uDDEF\\uDD73\\uDD76\\uDECD\\uDC3F\\uDD4A\\uDD77\\uDD78\\uDDFA\\uDEE3\\uDEE4\\uDEE2\\uDEF3\\uDEE5\\uDEE9\\uDEF0\\uDECE\\uDD70\\uDD79\\uDDBC\\uDDA5\\uDDA8\\uDDB1\\uDDB2\\uDCFD\\uDD6F\\uDDDE\\uDDF3\\uDD8B\\uDD8A\\uDD8C\\uDD8D\\uDDC2\\uDDD2\\uDDD3\\uDD87\\uDDC3\\uDDC4\\uDDD1\\uDDDD\\uDEE0\\uDDE1\\uDEE1\\uDDDC\\uDECF\\uDECB\\uDD49]\\uFE0F?|[\\uDE00-\\uDE06\\uDE09-\\uDE0B\\uDE0E\\uDE0D\\uDE18\\uDE17\\uDE19\\uDE1A\\uDE42\\uDE10\\uDE11\\uDE36\\uDE44\\uDE0F\\uDE23\\uDE25\\uDE2E\\uDE2F\\uDE2A\\uDE2B\\uDE34\\uDE0C\\uDE1B-\\uDE1D\\uDE12-\\uDE15\\uDE43\\uDE32\\uDE41\\uDE16\\uDE1E\\uDE1F\\uDE24\\uDE22\\uDE2D\\uDE26-\\uDE29\\uDE2C\\uDE30\\uDE31\\uDE33\\uDE35\\uDE21\\uDE20\\uDE37\\uDE07\\uDE08\\uDC7F\\uDC79\\uDC7A\\uDC80\\uDC7B\\uDC7D\\uDC7E\\uDCA9\\uDE3A\\uDE38\\uDE39\\uDE3B-\\uDE3D\\uDE40\\uDE3F\\uDE3E\\uDE48-\\uDE4A\\uDC64\\uDC65\\uDC6B-\\uDC6D\\uDC8F\\uDC91\\uDC6A\\uDC63\\uDC40\\uDC45\\uDC44\\uDC8B\\uDC98\\uDC93-\\uDC97\\uDC99-\\uDC9C\\uDDA4\\uDC9D-\\uDC9F\\uDC8C\\uDCA4\\uDCA2\\uDCA3\\uDCA5\\uDCA6\\uDCA8\\uDCAB-\\uDCAD\\uDC53-\\uDC62\\uDC51\\uDC52\\uDCFF\\uDC84\\uDC8D\\uDC8E\\uDC35\\uDC12\\uDC36\\uDC15\\uDC29\\uDC3A\\uDC31\\uDC08\\uDC2F\\uDC05\\uDC06\\uDC34\\uDC0E\\uDC2E\\uDC02-\\uDC04\\uDC37\\uDC16\\uDC17\\uDC3D\\uDC0F\\uDC11\\uDC10\\uDC2A\\uDC2B\\uDC18\\uDC2D\\uDC01\\uDC00\\uDC39\\uDC30\\uDC07\\uDC3B\\uDC28\\uDC3C\\uDC3E\\uDC14\\uDC13\\uDC23-\\uDC27\\uDC38\\uDC0A\\uDC22\\uDC0D\\uDC32\\uDC09\\uDC33\\uDC0B\\uDC2C\\uDC1F-\\uDC21\\uDC19\\uDC1A\\uDC0C\\uDC1B-\\uDC1E\\uDC90\\uDCAE\\uDD2A\\uDDFE\\uDDFB\\uDC92\\uDDFC\\uDDFD\\uDD4C\\uDD4D\\uDD4B\\uDC88\\uDE82-\\uDE8A\\uDE9D\\uDE9E\\uDE8B-\\uDE8E\\uDE90-\\uDE9C\\uDEB2\\uDEF4\\uDEF9\\uDEF5\\uDE8F\\uDEA8\\uDEA5\\uDEA6\\uDED1\\uDEA7\\uDEF6\\uDEA4\\uDEA2\\uDEEB\\uDEEC\\uDCBA\\uDE81\\uDE9F-\\uDEA1\\uDE80\\uDEF8\\uDD5B\\uDD67\\uDD50\\uDD5C\\uDD51\\uDD5D\\uDD52\\uDD5E\\uDD53\\uDD5F\\uDD54\\uDD60\\uDD55\\uDD61\\uDD56\\uDD62\\uDD57\\uDD63\\uDD58\\uDD64\\uDD59\\uDD65\\uDD5A\\uDD66\\uDD25\\uDCA7\\uDEF7\\uDD2E\\uDD07-\\uDD0A\\uDCE2\\uDCE3\\uDCEF\\uDD14\\uDD15\\uDCFB\\uDCF1\\uDCF2\\uDCDE-\\uDCE0\\uDD0B\\uDD0C\\uDCBB\\uDCBD-\\uDCC0\\uDCFA\\uDCF7-\\uDCF9\\uDCFC\\uDD0D\\uDD0E\\uDCA1\\uDD26\\uDCD4-\\uDCDA\\uDCD3\\uDCD2\\uDCC3\\uDCDC\\uDCC4\\uDCF0\\uDCD1\\uDD16\\uDCB0\\uDCB4-\\uDCB8\\uDCB3\\uDCB9\\uDCB1\\uDCB2\\uDCE7-\\uDCE9\\uDCE4-\\uDCE6\\uDCEB\\uDCEA\\uDCEC-\\uDCEE\\uDCDD\\uDCBC\\uDCC1\\uDCC2\\uDCC5-\\uDCD0\\uDD12\\uDD13\\uDD0F-\\uDD11\\uDD28\\uDD2B\\uDD27\\uDD29\\uDD17\\uDD2C\\uDD2D\\uDCE1\\uDC89\\uDC8A\\uDEAA\\uDEBD\\uDEBF\\uDEC1\\uDED2\\uDEAC\\uDDFF\\uDEAE\\uDEB0\\uDEB9-\\uDEBC\\uDEBE\\uDEC2-\\uDEC5\\uDEB8\\uDEAB\\uDEB3\\uDEAD\\uDEAF\\uDEB1\\uDEB7\\uDCF5\\uDD1E\\uDD03\\uDD04\\uDD19-\\uDD1D\\uDED0\\uDD4E\\uDD2F\\uDD00-\\uDD02\\uDD3C\\uDD3D\\uDD05\\uDD06\\uDCF6\\uDCF3\\uDCF4\\uDD31\\uDCDB\\uDD30\\uDD1F\\uDCAF\\uDD20-\\uDD24\\uDD36-\\uDD3B\\uDCA0\\uDD18\\uDD32-\\uDD35\\uDEA9])|\\uD83E(?:[\\uDDD2\\uDDD1\\uDDD3](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDB8\\uDDB9](?:\\u200D(?:[\\u2640\\u2642]\\uFE0F?))?|[\\uDD34\\uDDD5\\uDDD4\\uDD35\\uDD30\\uDD31\\uDD36](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDD9-\\uDDDD](?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2640\\u2642]\\uFE0F?))?)|\\u200D(?:[\\u2640\\u2642]\\uFE0F?)))?|[\\uDDDE\\uDDDF](?:\\u200D(?:[\\u2640\\u2642]\\uFE0F?))?|[\\uDD26\\uDD37](?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDDD6-\\uDDD8](?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2640\\u2642]\\uFE0F?))?)|\\u200D(?:[\\u2640\\u2642]\\uFE0F?)))?|\\uDD38(?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|\\uDD3C(?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?|[\\uDD3D\\uDD3E\\uDD39](?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDD33\\uDDB5\\uDDB6\\uDD1E\\uDD18\\uDD19\\uDD1B\\uDD1C\\uDD1A\\uDD1F\\uDD32](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD23\\uDD70\\uDD17\\uDD29\\uDD14\\uDD28\\uDD10\\uDD24\\uDD11\\uDD2F\\uDD75\\uDD76\\uDD2A\\uDD2C\\uDD12\\uDD15\\uDD22\\uDD2E\\uDD27\\uDD20\\uDD21\\uDD73\\uDD74\\uDD7A\\uDD25\\uDD2B\\uDD2D\\uDDD0\\uDD13\\uDD16\\uDD3A\\uDD1D\\uDDB0-\\uDDB3\\uDDE0\\uDDB4\\uDDB7\\uDDE1\\uDD7D\\uDD7C\\uDDE3-\\uDDE6\\uDD7E\\uDD7F\\uDDE2\\uDD8D\\uDD8A\\uDD9D\\uDD81\\uDD84\\uDD93\\uDD8C\\uDD99\\uDD92\\uDD8F\\uDD9B\\uDD94\\uDD87\\uDD98\\uDDA1\\uDD83\\uDD85\\uDD86\\uDDA2\\uDD89\\uDD9A\\uDD9C\\uDD8E\\uDD95\\uDD96\\uDD88\\uDD80\\uDD9E\\uDD90\\uDD91\\uDD8B\\uDD97\\uDD82\\uDD9F\\uDDA0\\uDD40\\uDD6D\\uDD5D\\uDD65\\uDD51\\uDD54\\uDD55\\uDD52\\uDD6C\\uDD66\\uDD5C\\uDD50\\uDD56\\uDD68\\uDD6F\\uDD5E\\uDDC0\\uDD69\\uDD53\\uDD6A\\uDD59\\uDD5A\\uDD58\\uDD63\\uDD57\\uDDC2\\uDD6B\\uDD6E\\uDD5F-\\uDD61\\uDDC1\\uDD67\\uDD5B\\uDD42\\uDD43\\uDD64\\uDD62\\uDD44\\uDDED\\uDDF1\\uDDF3\\uDDE8\\uDDE7\\uDD47-\\uDD49\\uDD4E\\uDD4F\\uDD4D\\uDD4A\\uDD4B\\uDD45\\uDD4C\\uDDFF\\uDDE9\\uDDF8\\uDD41\\uDDEE\\uDDFE\\uDDF0\\uDDF2\\uDDEA-\\uDDEC\\uDDEF\\uDDF4-\\uDDF7\\uDDF9-\\uDDFD])|[\\u263A\\u2639\\u2620]\\uFE0F?|\\uD83C(?:\\uDF85(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDFC3(?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDFC7\\uDFC2](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDFCC(?:(?:\\uFE0F(?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?|\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDFC4\\uDFCA](?:(?:\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|\\uDFCB(?:(?:\\uFE0F(?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?|\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\uDFCE\\uDFCD\\uDFF5\\uDF36\\uDF7D\\uDFD4-\\uDFD6\\uDFDC-\\uDFDF\\uDFDB\\uDFD7\\uDFD8\\uDFDA\\uDFD9\\uDF21\\uDF24-\\uDF2C\\uDF97\\uDF9F\\uDF96\\uDF99-\\uDF9B\\uDF9E\\uDFF7\\uDD70\\uDD71\\uDD7E\\uDD7F\\uDE02\\uDE37]\\uFE0F?|\\uDFF4(?:(?:\\u200D\\u2620\\uFE0F?|\\uDB40\\uDC67\\uDB40\\uDC62\\uDB40(?:\\uDC65\\uDB40\\uDC6E\\uDB40\\uDC67\\uDB40\\uDC7F|\\uDC73\\uDB40\\uDC63\\uDB40\\uDC74\\uDB40\\uDC7F|\\uDC77\\uDB40\\uDC6C\\uDB40\\uDC73\\uDB40\\uDC7F)))?|\\uDFF3(?:(?:\\uFE0F(?:\\u200D\\uD83C\\uDF08)?|\\u200D\\uD83C\\uDF08))?|\\uDDE6\\uD83C[\\uDDE8-\\uDDEC\\uDDEE\\uDDF1\\uDDF2\\uDDF4\\uDDF6-\\uDDFA\\uDDFC\\uDDFD\\uDDFF]|\\uDDE7\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEF\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9\\uDDFB\\uDDFC\\uDDFE\\uDDFF]|\\uDDE8\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDEE\\uDDF0-\\uDDF5\\uDDF7\\uDDFA-\\uDDFF]|\\uDDE9\\uD83C[\\uDDEA\\uDDEC\\uDDEF\\uDDF0\\uDDF2\\uDDF4\\uDDFF]|\\uDDEA\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDED\\uDDF7-\\uDDFA]|\\uDDEB\\uD83C[\\uDDEE-\\uDDF0\\uDDF2\\uDDF4\\uDDF7]|\\uDDEC\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEE\\uDDF1-\\uDDF3\\uDDF5-\\uDDFA\\uDDFC\\uDDFE]|\\uDDED\\uD83C[\\uDDF0\\uDDF2\\uDDF3\\uDDF7\\uDDF9\\uDDFA]|\\uDDEE\\uD83C[\\uDDE8-\\uDDEA\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9]|\\uDDEF\\uD83C[\\uDDEA\\uDDF2\\uDDF4\\uDDF5]|\\uDDF0\\uD83C[\\uDDEA\\uDDEC-\\uDDEE\\uDDF2\\uDDF3\\uDDF5\\uDDF7\\uDDFC\\uDDFE\\uDDFF]|\\uDDF1\\uD83C[\\uDDE6-\\uDDE8\\uDDEE\\uDDF0\\uDDF7-\\uDDFB\\uDDFE]|\\uDDF2\\uD83C[\\uDDE6\\uDDE8-\\uDDED\\uDDF0-\\uDDFF]|\\uDDF3\\uD83C[\\uDDE6\\uDDE8\\uDDEA-\\uDDEC\\uDDEE\\uDDF1\\uDDF4\\uDDF5\\uDDF7\\uDDFA\\uDDFF]|\\uDDF4\\uD83C\\uDDF2|\\uDDF5\\uD83C[\\uDDE6\\uDDEA-\\uDDED\\uDDF0-\\uDDF3\\uDDF7-\\uDDF9\\uDDFC\\uDDFE]|\\uDDF6\\uD83C\\uDDE6|\\uDDF7\\uD83C[\\uDDEA\\uDDF4\\uDDF8\\uDDFA\\uDDFC]|\\uDDF8\\uD83C[\\uDDE6-\\uDDEA\\uDDEC-\\uDDF4\\uDDF7-\\uDDF9\\uDDFB\\uDDFD-\\uDDFF]|\\uDDF9\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDED\\uDDEF-\\uDDF4\\uDDF7\\uDDF9\\uDDFB\\uDDFC\\uDDFF]|\\uDDFA\\uD83C[\\uDDE6\\uDDEC\\uDDF2\\uDDF3\\uDDF8\\uDDFE\\uDDFF]|\\uDDFB\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDEE\\uDDF3\\uDDFA]|\\uDDFC\\uD83C[\\uDDEB\\uDDF8]|\\uDDFD\\uD83C\\uDDF0|\\uDDFE\\uD83C[\\uDDEA\\uDDF9]|\\uDDFF\\uD83C[\\uDDE6\\uDDF2\\uDDFC]|[\\uDFFB-\\uDFFF\\uDF92\\uDFA9\\uDF93\\uDF38-\\uDF3C\\uDF37\\uDF31-\\uDF35\\uDF3E-\\uDF43\\uDF47-\\uDF53\\uDF45\\uDF46\\uDF3D\\uDF44\\uDF30\\uDF5E\\uDF56\\uDF57\\uDF54\\uDF5F\\uDF55\\uDF2D-\\uDF2F\\uDF73\\uDF72\\uDF7F\\uDF71\\uDF58-\\uDF5D\\uDF60\\uDF62-\\uDF65\\uDF61\\uDF66-\\uDF6A\\uDF82\\uDF70\\uDF6B-\\uDF6F\\uDF7C\\uDF75\\uDF76\\uDF7E\\uDF77-\\uDF7B\\uDF74\\uDFFA\\uDF0D-\\uDF10\\uDF0B\\uDFE0-\\uDFE6\\uDFE8-\\uDFED\\uDFEF\\uDFF0\\uDF01\\uDF03-\\uDF07\\uDF09\\uDF0C\\uDFA0-\\uDFA2\\uDFAA\\uDF11-\\uDF20\\uDF00\\uDF08\\uDF02\\uDF0A\\uDF83\\uDF84\\uDF86-\\uDF8B\\uDF8D-\\uDF91\\uDF80\\uDF81\\uDFAB\\uDFC6\\uDFC5\\uDFC0\\uDFD0\\uDFC8\\uDFC9\\uDFBE\\uDFB3\\uDFCF\\uDFD1-\\uDFD3\\uDFF8\\uDFA3\\uDFBD\\uDFBF\\uDFAF\\uDFB1\\uDFAE\\uDFB0\\uDFB2\\uDCCF\\uDC04\\uDFB4\\uDFAD\\uDFA8\\uDFBC\\uDFB5\\uDFB6\\uDFA4\\uDFA7\\uDFB7-\\uDFBB\\uDFA5\\uDFAC\\uDFEE\\uDFF9\\uDFE7\\uDFA6\\uDD8E\\uDD91-\\uDD9A\\uDE01\\uDE36\\uDE2F\\uDE50\\uDE39\\uDE1A\\uDE32\\uDE51\\uDE38\\uDE34\\uDE33\\uDE3A\\uDE35\\uDFC1\\uDF8C])|\\u26F7\\uFE0F?|\\u26F9(?:(?:\\uFE0F(?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?|\\uD83C(?:[\\uDFFB-\\uDFFF](?:\\u200D(?:[\\u2642\\u2640]\\uFE0F?))?)|\\u200D(?:[\\u2642\\u2640]\\uFE0F?)))?|[\\u261D\\u270C](?:(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F))?|[\\u270B\\u270A](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\u270D(?:(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F))?|[\\u2764\\u2763\\u26D1\\u2618\\u26F0\\u26E9\\u2668\\u26F4\\u2708\\u23F1\\u23F2\\u2600\\u2601\\u26C8\\u2602\\u26F1\\u2744\\u2603\\u2604\\u26F8\\u2660\\u2665\\u2666\\u2663\\u260E\\u2328\\u2709\\u270F\\u2712\\u2702\\u26CF\\u2692\\u2694\\u2699\\u2696\\u26D3\\u2697\\u26B0\\u26B1\\u26A0\\u2622\\u2623\\u2B06\\u2197\\u27A1\\u2198\\u2B07\\u2199\\u2B05\\u2196\\u2195\\u2194\\u21A9\\u21AA\\u2934\\u2935\\u269B\\u267E\\u2721\\u2638\\u262F\\u271D\\u2626\\u262A\\u262E\\u25B6\\u23ED\\u23EF\\u25C0\\u23EE\\u23F8-\\u23FA\\u23CF\\u2640\\u2642\\u2695\\u267B\\u269C\\u2611\\u2714\\u2716\\u303D\\u2733\\u2734\\u2747\\u203C\\u2049\\u3030\\u00A9\\u00AE\\u2122]\\uFE0F?|[\\u0023\\u002A\\u0030-\\u0039](?:\\uFE0F\\u20E3|\\u20E3)|[\\u2139\\u24C2\\u3297\\u3299\\u25AA\\u25AB\\u25FB\\u25FC]\\uFE0F?|[\\u2615\\u26EA\\u26F2\\u26FA\\u26FD\\u2693\\u26F5\\u231B\\u23F3\\u231A\\u23F0\\u2B50\\u26C5\\u2614\\u26A1\\u26C4\\u2728\\u26BD\\u26BE\\u26F3\\u267F\\u26D4\\u2648-\\u2653\\u26CE\\u23E9-\\u23EC\\u2B55\\u2705\\u274C\\u274E\\u2795-\\u2797\\u27B0\\u27BF\\u2753-\\u2755\\u2757\\u25FD\\u25FE\\u2B1B\\u2B1C\\u26AA\\u26AB])", replaceStr);
            if (subLength != -1)
            {
                int num = subLength;
                if (num > text.Length)
                {
                    num = text.Length;
                }
                text = text.Substring(0, num);
            }
            return text;
        }

        public static void SetParent(GameObject obj, GameObject parentObj)
        {
            obj.transform.SetParent(parentObj.transform);
            ObjectPoolItem component = obj.GetComponent<ObjectPoolItem>();
            if (component != null)
            {
                component.SearchParent();
            }
        }

        public static void AdaptPos(GameObject obj, float v)
        {
            if (obj == null)
            {
                return;
            }
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                Transform child = obj.transform.GetChild(i);
                Vector2 anchoredPosition = child.GetComponent<RectTransform>().anchoredPosition;
                child.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredPosition.x + v, anchoredPosition.y);
            }
        }

        public static string GetAppVer()
        {
            return Application.version;
        }

        public static Sprite ScreenShot(Camera camera, Vector2 pos, Vector2 wh, int mode)
        {
            Rect source = new Rect(pos.x, pos.y, wh.x, wh.y);
            Common.rt = new RenderTexture((int)source.width, (int)source.height, 32);
            camera.targetTexture = Common.rt;
            camera.Render();
            RenderTexture.active = Common.rt;
            TextureFormat format = TextureFormat.RGB24;
            Texture2D texture2D = new Texture2D((int)source.width, (int)source.height, format, false);
            texture2D.ReadPixels(source, 0, 0);
            texture2D.Apply();
            Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, wh.x, wh.y), Vector2.zero);
            camera.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(Common.rt);
            sprite.name = "Screenshot_" + Mathf.FloorToInt(Time.fixedUnscaledTime * 1000f);
            return sprite;
        }

        public static void RemovePreDlcFolder(string curresnum, string targetfolder)
        {
            if (targetfolder == string.Empty)
            {
                return;
            }
            if (curresnum == string.Empty)
            {
                return;
            }
            int num = int.Parse(curresnum);
            if (!Directory.Exists(targetfolder))
            {
                return;
            }
            string[] directories = Directory.GetDirectories(targetfolder);
            string[] array = directories;
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                int num2 = text.LastIndexOf("/");
                string text2 = text.Remove(0, num2 + 1);
                bool flag = Regex.IsMatch(text2, "^\\d*[.]?\\d*$");
                if (flag)
                {
                    int num3 = int.Parse(text2);
                    if (num >= num3)
                    {
                        Directory.Delete(text, true);
                    }
                }
            }
        }

        private static void Update()
        {
            s_ticktick -= Time.fixedUnscaledDeltaTime;
        }

        public static void SyncTickS(float TickInterval)
        {
            s_ticktick = TickInterval * 0.001f;
        }
    }
}
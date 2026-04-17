using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Skyunion;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public static class ClientUtils
    {
        #region 服务

        #endregion

        #region 管理器

        static LevelDetailManager _lodManager;

        public static LevelDetailManager lodManager
        {
            get { return _lodManager ?? (_lodManager = LevelDetailManager.Instance()); }
        }

        static MapManager _mapManager;

        public static MapManager mapManager
        {
            get { return _mapManager ?? (_mapManager = MapManager.Instance()); }
        }

        static HUDManager _hudManager;

        public static HUDManager hudManager
        {
            get { return _hudManager ?? (_hudManager = HUDManager.Instance()); }
        }

        static RssAnimationManager _rssAnimationManager;

        public static RssAnimationManager rssAnimationManager
        {
            get
            {
                return _rssAnimationManager ??
                       (_rssAnimationManager = PluginManager.Instance().FindModule<RssAnimationManager>()) ??
                       new RssAnimationManager();
            }
        }

        static LightManager _lightingManager;

        public static LightManager lightingManager
        {
            get { return _lightingManager ?? (_lightingManager = LightManager.Instance()); }
        }

        static WeatherManager _weatherMgr;

        public static WeatherManager weatherMgr
        {
            get { return _weatherMgr ?? (_weatherMgr = WeatherManager.Instance()); }
        }

        #endregion

        public static void ClearCore()
        {
            _weatherMgr = null;
            _lightingManager = null;
            _rssAnimationManager = null;
            _hudManager = null;
            _mapManager = null;
            //_lodManager = null;
        }

        #region 辅助函数

        public static Vector2 GetCenterByPos(Vector2 pos, float tile_width)
        {
            float num = tile_width / 2f;
            return new Vector2((float) ((int) (pos.x / tile_width)) * tile_width + num,
                (float) ((int) (pos.y / tile_width)) * tile_width + num);
        }

        public static bool RectLeftBottomContains(Rect rect, Vector2 point)
        {
            return point.y >= rect.yMin && point.y < rect.yMax && point.x >= rect.xMin && point.x < rect.xMax;
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

        //打印对象数据
        public static void Print(object obj,string name="")
        {
            if (obj != null)
            {
                Debug.Log(name+"  "+LitJson.JsonMapper.ToJson(obj));
            }
        }

        //资源预加载
        public static void PreLoadRes(GameObject viewObj, List<string> prefabNames,
            Action<Dictionary<string, GameObject>> loadFinishCallback)
        {
            GameObject tempObj = viewObj;
            Dictionary<string, GameObject> assetDic = new Dictionary<string, GameObject>();
            int total = prefabNames.Count;
            int count = 0;
            for (int i = 0; i < total; i++)
            {
                CoreUtils.assetService.LoadAssetAsync<GameObject>(prefabNames[i], (asset) =>
                {
                    if (tempObj == null)
                    {
                        assetDic.Clear();
                        return;
                    }

                    GameObject obj = asset.asset() as GameObject;
                    if (obj != null)
                    {
                        assetDic[obj.name] = obj;
                    }
                    else
                    {
                        Debug.LogError(string.Format("asset load fail :{0}", asset.ToString()));
                    }

                    count = count + 1;
                    if (count >= total)
                    {
                        loadFinishCallback(assetDic);
                    }
                }, viewObj);
            }
        }

        //资源预加载
        public static void PreLoadRes(GameObject viewObj, List<string> prefabNames,
            Action<Dictionary<string, IAsset>> loadFinishCallback)
        {
            GameObject tempObj = viewObj;
            Dictionary<string, IAsset> assetDic = new Dictionary<string, IAsset>();
            int total = prefabNames.Count;
            if (total < 1)
            {
                loadFinishCallback(assetDic);
                return;
            }

            int count = 0;
            for (int i = 0; i < total; i++)
            {
                CoreUtils.assetService.LoadAssetAsync<GameObject>(prefabNames[i], (asset) =>
                {
                    if (tempObj == null || asset.asset() == null)
                    {
                        Debug.LogErrorFormat("load prefab fail:{0}", asset.assetName());
                        assetDic.Clear();
                        return;
                    }

                    assetDic[asset.asset().name] = asset;
                    count = count + 1;
                    if (count >= total)
                    {
                        loadFinishCallback(assetDic);
                    }
                }, viewObj);
            }
        }

        #endregion


        /// <summary>
        /// 替换皮肤和动作
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="skeletonDataAsset"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        private static SkeletonGraphic ChangeSkeletonGraphic(SkeletonGraphic graphic,
            SkeletonDataAsset skeletonDataAsset, Skin skin = null)
        {
            if (graphic == null || !graphic.gameObject)
            {
                return graphic;
            }

            if (graphic.SkeletonDataAsset!=null)
            {
                graphic.skeletonDataAsset.Clear();
            }

            graphic.skeletonDataAsset = skeletonDataAsset;
            SkeletonData data = skeletonDataAsset.GetSkeletonData(true);

            if (data == null)
            {
                //                for (int i = 0; i < skeletonDataAsset.atlasAssets.Length; i++) {
                //                    string reloadAtlasPath = AssetDatabase.GetAssetPath(skeletonDataAsset.atlasAssets[i]);
                //                    skeletonDataAsset.atlasAssets[i] = (AtlasAsset)AssetDatabase.LoadAssetAtPath(reloadAtlasPath, typeof(AtlasAsset));
                //                }

                data = skeletonDataAsset.GetSkeletonData(true);
            }

            skin = skin ?? data.DefaultSkin ?? data.Skins.Items[0];


            graphic.Initialize(true);
            if (skin != null) graphic.Skeleton.SetSkin(skin);


            graphic.initialSkinName = skin.Name;

            //graphic.startingAnimation = "idle";
            graphic.Skeleton.UpdateWorldTransform();
            graphic.UpdateMesh();

            return graphic;
        }

        /// <summary>
        /// 替换皮肤和动作
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="skeletonDataAsset"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        private static SkeletonAnimation ChangeSkeletonAnimation(SkeletonAnimation skeleton,
            SkeletonDataAsset skeletonDataAsset, Skin skin = null)
        {
            if (skeleton == null || !skeleton.gameObject)
            {
                return skeleton;
            }

            skeleton.skeletonDataAsset = skeletonDataAsset;
            SkeletonData data = skeletonDataAsset.GetSkeletonData(true);

            if (data == null)
            {
                data = skeletonDataAsset.GetSkeletonData(true);
            }

            skin = skin ?? data.DefaultSkin ?? data.Skins.Items[0];


            skeleton.Initialize(true);
            if (skin != null) skeleton.Skeleton.SetSkin(skin);


            skeleton.initialSkinName = skin.Name;

            skeleton.Skeleton.UpdateWorldTransform();

            return skeleton;
        }

        /// <summary>
        /// 加载Sprite  图片名称[Sprite名称]
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetname"></param>
        public static void LoadSprite(PolygonImage image, string assetname, bool isNativeSize = false,
            Action Callback = null)
        {
            if (string.IsNullOrEmpty(assetname))
            {
                return;
            }

            if (!string.IsNullOrEmpty(image.assetName) && image.assetName == assetname)
            {
                if (Callback != null)
                {
                    Callback();
                }
                return;
            }
            
            image.assetName = assetname;
            CoreUtils.assetService.LoadAssetAsync<Sprite>(assetname, (asset) =>
            {
                if (asset == null)
                {
                    Debug.Log("找不到资源" + assetname);
                }

                if (image == null)
                {
                    Debug.LogWarning("物件已经被销毁了" + assetname);
                    return;
                }

                var dat = asset.asset();
                if (dat != null && dat is Sprite)
                {
                    if (image.assetName != assetname)
                    {
                        return;
                    }

                    image.sprite = dat as Sprite;
                    if (isNativeSize)
                    {
                        image.SetNativeSize();
                    }
                }
                else
                {
                    if (dat != null)
                    {
                        Debug.Log("找不到资源2" + assetname + "\t" + dat.ToString());
                    }
                    else
                    {
                        Debug.Log("找不到资源3" + assetname);
                    }
                }

                if (Callback != null)
                {
                    Callback();
                }
            }, image.gameObject);
        }

        /// <summary>
        /// 加载Sprite  图片名称[Sprite名称]
        /// </summary>
        /// <param name="image"></param>
        /// <param name="assetname"></param>
        public static void LoadSprite(SpriteRenderer spriteRenderer, string assetname,Action Callback = null)
        {
            CoreUtils.assetService.LoadAssetAsync<Sprite>(assetname, (asset) =>
            {
                if (asset == null)
                {
                    Debug.Log("找不到资源" + assetname);
                }

                if (spriteRenderer == null)
                {
                    Debug.LogWarning("物件已经被销毁了" + assetname);
                    return;
                }

                var dat = asset.asset();
                if (dat != null && dat is Sprite)
                {
                    Callback?.Invoke();
                    spriteRenderer.sprite = dat as Sprite;
                }
                else
                {
                    Debug.Log("找不到资源2" + assetname);
                }
            }, spriteRenderer.gameObject);
        }

        /// <summary>
        /// 替换 加载spine资源
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="assetname"></param>
        public static void LoadSpine(SkeletonGraphic graphic, string assetname, Action Callback = null)
        {
            CoreUtils.assetService.LoadAssetAsync<SkeletonDataAsset>(assetname, (asset) =>
            {
                if (asset == null)
                {
                    Debug.Log("找不到资源" + assetname);
                }

                var dat = asset.asset();
                if (dat != null && dat is SkeletonDataAsset)
                {
                    var data = dat as SkeletonDataAsset;

                    ChangeSkeletonGraphic(graphic, data);
                }
                else
                {
                    Debug.Log("找不到资源2" + assetname);
                }

                if (Callback != null)
                {
                    Callback();
                }
            }, graphic.gameObject);
        }

        /// <summary>
        /// 替换 加载spine资源
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="assetname"></param>
        public static void LoadSpine(SkeletonAnimation skeleton, string assetname, Action Callback = null)
        {
            CoreUtils.assetService.LoadAssetAsync<SkeletonDataAsset>(assetname, (asset) =>
            {
                if (asset == null)
                {
                    Debug.Log("找不到资源" + assetname);
                }

                var dat = asset.asset();
                if (dat != null && dat is SkeletonDataAsset)
                {
                    var data = dat as SkeletonDataAsset;

                    ChangeSkeletonAnimation(skeleton, data);
                }
                else
                {
                    Debug.Log("找不到资源2" + assetname);
                }

                if (Callback != null)
                {
                    Callback();
                }
            }, skeleton.gameObject);
        }

        //播放其他动画
        public static void PlaySpine(SkeletonGraphic graphic, string initSkinName,string amname,bool isLoop = false )
        {
            graphic.initialSkinName = initSkinName;


            bool isChangeAnimation = false;

            if (graphic.startingAnimation!= initSkinName)
            {
                isChangeAnimation = true;
                graphic.startingAnimation = "";
            }
            
            graphic.Initialize(true);

            if (isChangeAnimation)
            {
                graphic.AnimationState.SetAnimation(0, amname, isLoop);
            }
            graphic.SetMaterialDirty();
        }


        private static string[] mFormats = new[]
            {"{0}", "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{10}"};

        /// <summary>
        /// 策划配置的字符串 加 参数List<int>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string SafeFormat(string str, List<int> arr)
        {
            try
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    if (i < mFormats.Length)
                    {
                        str = str.Replace(mFormats[i], arr[i].ToString());
                    }
                }

                return str;
            }
            catch (Exception e)
            {
                Debug.Log(arr.Count + "格式化错误" + str + e.ToString());
            }

            return str;
        }


        /// <summary>
        /// 设置文本字体颜色#FFFFFFF
        /// </summary>
        /// <param name="text"></param>
        /// <param name="rgb"></param>
        public static void TextSetColor(Text text, string rgb)
        {
            Color c;
            ColorUtility.TryParseHtmlString(rgb, out c);
            text.color = c;
        }

        public static void TextSetColor(Text text, Color c)
        {
            text.color = c;
        }

        public static Color StringToHtmlColor(string rgb)
        {
            Color c = Color.clear;
            ColorUtility.TryParseHtmlString(rgb, out c);
            return c;
        }

        /// <summary>
        /// hex rgba转换到color
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HexRGBAToColor(string hex)
        {
            byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte cc = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            float a = cc / 255f;
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// hex rgb转换到color
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static Color HexRGBToColor(string hex)
        {
            byte br = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte bg = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte bb = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            float r = br / 255f;
            float g = bg / 255f;
            float b = bb / 255f;
            return new Color(r, g, b, 1.0f);
        }

        public static void ImageSetColor(PolygonImage image, string rgb)
        {
            Color c;
            ColorUtility.TryParseHtmlString(rgb, out c);
            image.color = c;
        }
        
        public static void ImageSetColor(SpriteRenderer image, string rgb)
        {
            Color c;
            ColorUtility.TryParseHtmlString(rgb, out c);
            image.color = c;
        }


        public static void ShowChild<T>(Transform node, int nCount) where T : Behaviour
        {
            for (int i = 0; i < node.childCount; i++)
            {
                var child = node.GetChild(i).GetComponent<T>();
                child.enabled = i < nCount;
            }
        }

        private static string[] units = new string[] {"", "K", "M", "G", "T", "P", "E", "Z", "Y", "B", "N", "D"};

        /// <summary>
        /// 格式化货币单位
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string CurrencyFormat(long number)
        {
            long mod = 1000;
            int i = 0;
            decimal tmpNum = number;
            while (tmpNum >= mod)
            {
                tmpNum /= mod;
                i++;
            }

            if (units.Length > i)
            {
                if (i == 0)
                {
                    return string.Format("{0:N0}", number);
                }
                else
                {
                    int Decimal = decimal.ToInt32(Math.Floor((tmpNum - (int)tmpNum) * 10));
                    return $"{(int)tmpNum}.{Decimal}{units[i]}"; //保留小数点一位+单位
                }
            }
            else
            {
                Debug.Log("number格式化错误:" + number);
                return number.ToString();
            }
        }

        private static System.TimeSpan m_formatTimeSpan;

        /// <summary>
        /// 倒计时格式化
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatCountDown(int num)
        {
            string str = "";
            m_formatTimeSpan = new TimeSpan(0, 0, num);
            if (m_formatTimeSpan.Days > 0)
            {
                str = string.Format(LanguageUtils.getText(128004), m_formatTimeSpan.Days);
                str = string.Format("{0} {1:D2}:{2:D2}:{3:D2}", str, m_formatTimeSpan.Hours, m_formatTimeSpan.Minutes,
                    m_formatTimeSpan.Seconds);
            }
            else
            {
                str = string.Format("{0:D2}:{1:D2}:{2:D2}", m_formatTimeSpan.Hours, m_formatTimeSpan.Minutes,
                    m_formatTimeSpan.Seconds);
            }

            return str;
        }

        /// <summary>
        /// 千分位添加逗号
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatComma(Int64 num)
        {
            return num.ToString("N0");
        }

        public static int NumberSymbol(this float n)
        {
            if (n < 0)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// 开服天数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int TimeSpanDays(DateTime departure, DateTime arrival)
        {
            int days = 0;
            DateTime start = departure.AddHours(-departure.Hour).AddMinutes(-departure.Minute).AddSeconds(-departure.Second);
            DateTime end = arrival.AddHours(-arrival.Hour).AddMinutes(-arrival.Minute).AddSeconds(-arrival.Second);
            TimeSpan timespan = end - start;
            return timespan.Days + 1;
        }
        
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatTime(int num)
        {
            string str = "";
            TimeSpan timeSpan = new TimeSpan(0, 0, num);
            if (timeSpan.Seconds > 0)
            {
                str = LanguageUtils.getTextFormat(300028, timeSpan.Seconds);
            }

            if (timeSpan.Minutes > 0)
            {
                str = string.Format("{0}{1}", LanguageUtils.getTextFormat(180610, timeSpan.Minutes), str);
            }

            if (timeSpan.Hours > 0)
            {
                str = string.Format("{0}{1}", LanguageUtils.getTextFormat(180611, timeSpan.Hours), str);
            }

            if (timeSpan.Days > 0)
            {
                str = string.Format("{0}{1}", string.Format(LanguageUtils.getText(180612), timeSpan.Days), str);
            }

            return str;
        }

        /// <summary>
        /// 持续时间小等于60秒的显示为X秒，持续时间大于60秒的显示为X分钟(取整)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatTimeCityBuff(int num)
        {
            string str = "";
            TimeSpan timeSpan = new TimeSpan(0, 0, num);

            if (timeSpan.TotalSeconds <= 60)
            {
                str = LanguageUtils.getTextFormat(300028, timeSpan.TotalSeconds);
            }
            else 
            {
                str = LanguageUtils.getTextFormat(180610, timeSpan.TotalMinutes);
            }
            return str;
        }


        public static string FormatTimeSplit(int num)
        {
            num = Mathf.Max(0, num);
            TimeSpan timeSpan = new TimeSpan(0, 0, num);

            string ft = timeSpan.ToString(LanguageUtils.getText(300236));
            if (timeSpan.Days>0)
            {
                return LanguageUtils.getTextFormat(300138, timeSpan.Days,ft);
            }
            
            return timeSpan.ToString(LanguageUtils.getText(300236));
        }


        public static string FormatTimeTroop(int num)
        {
            string str = string.Empty;
            TimeSpan timeSpan = new TimeSpan(0, 0, num);
            str = timeSpan.ToString("hh\\:mm\\:ss");

            return str;
        }

        /// <summary>
        /// 升级界面的格式化时间
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string FormatTimeUpgrad(int num,bool isFull= false)
        {
            string str = "";
            TimeSpan timeSpan = new TimeSpan(0, 0, num);
            if (timeSpan.Seconds > 0 )
            {
                str = LanguageUtils.getTextFormat(300028, timeSpan.Seconds);
            }

            if (timeSpan.Minutes > 0)
            {
                if (timeSpan.Seconds > 0)
                {
                    str = LanguageUtils.getTextFormat(180613, timeSpan.Minutes, timeSpan.Seconds);
                }
                else
                {
                    str = LanguageUtils.getTextFormat(180610, timeSpan.Minutes);
                }
            }

            if (timeSpan.Hours > 0)
            {
                if (timeSpan.Minutes > 0)
                {
                    str = LanguageUtils.getTextFormat(180614, timeSpan.Hours, timeSpan.Minutes);
                }
                else
                {
                    str = LanguageUtils.getTextFormat(180611, timeSpan.Hours);
                }
            }

            if (timeSpan.Days > 0)
            {
                if (timeSpan.Hours > 0)
                {
                    str = LanguageUtils.getTextFormat(180615, timeSpan.Days, timeSpan.Hours);
                }
                else
                {
                    str = LanguageUtils.getTextFormat(180612, timeSpan.Days);
                }
            }

            if (isFull)
            {
                str = LanguageUtils.getTextFormat(100715, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
//                str = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }

            return str;
        }


        /// <summary>
        /// 将百分比转换成小数
        /// </summary>
        /// <param name="perc">百分比值，可纯为数值，或都加上%号的表示，
        /// 如：65|65%</param>
        /// <returns></returns>
        public static decimal PerctangleToDecimal(string perc)
        {
            try
            {
                decimal newPerc = perc.Replace("%", "").Replace("+", "").ConvertToDecimal();
                if (newPerc == 0)
                {
                    return 0;
                }
                else
                {
                    return newPerc / 100;
                }
            }
            catch
            {
                return 1;
            }
        }


        public static void SetPro(float value, Slider slider)
        {
            slider.value = value;
            string img = string.Empty;
            if (value>=1f)
            {
                img = "ui_common[pb_com_1000_4]";
            }
            else
            {
                img = "ui_common[pb_com_1000_2]";
            }
            
            CoreUtils.assetService.LoadAssetAsync<Sprite>(img, (sp) =>
            {
                if (slider != null)
                {
                    slider.fillRect.GetComponent<PolygonImage>().sprite = sp.asset() as Sprite;
                }
            }, slider.gameObject);
        }

        public static decimal ConvertToDecimal(this string value)
        {
            decimal num;
            if (decimal.TryParse(value, out num))
                return num;
            return 0;
        }

        public static string FormatDecimal(decimal data, int dotafer = 1)
        {
            System.Globalization.NumberFormatInfo provider = new System.Globalization.NumberFormatInfo();
            provider.PercentDecimalDigits = dotafer; //小数点保留几位数. 
            provider.PercentPositivePattern = 1; //百分号出现在何处. 
            return data.ToString("P", provider).Replace("%","");
        }

        public static void PlayUIAnimation(Animator animator, string strName)
        {
            //string strArbName = strName;
            //if(LanguageUtils.IsArabic())
            //{
            //    strArbName += "_arb";
            //    var controller = animator.runtimeAnimatorController as AnimatorOverrideController;
            //    AnimatorStateMachine stateMachine = controller.layers[0].stateMachine;
            //    string[] animatorState = new string[stateMachine.states.Length];
            //    for (int i = 0; i < stateMachine.states.Length; i++)
            //    {
            //        if (strArbName == stateMachine.states[i].state.name)
            //        {
            //            animator.Play(strArbName);
            //            return;
            //        }
            //    }
            //}
            animator.Play(strName);
        }
        
        public static float GetAnimationLength(Animator animator, string animationName)
        {
            if (animator == null) return 0;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Equals(animationName))
                {
                    return clip.length;
                }
            }
            return 0;
        }

        public static float GetAnimationLength(Animator animator, int index)
        {
            if (animator == null) return 0;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            if (clips == null || clips.Length <= index) return 0;
            return clips[index].length;
        }

        public static void UIAddEffect(string effectName, Transform targetPart, Action<GameObject> callBack,bool autoremove = false,float removeTime=3f)
        {
            CoreUtils.assetService.Instantiate(effectName, (effectGO) =>
            {
                effectGO.name = effectName;
                effectGO.SetActive(true);
                if(targetPart!=null)
                {
                    effectGO.transform.SetParent(targetPart);
                }
                effectGO.transform.localScale = Vector3.one;
                effectGO.transform.localPosition = Vector3.zero;

                callBack?.Invoke(effectGO);

                if (autoremove)
                {
                    Timer.Register(removeTime, () =>
                    {
                        if (effectGO)
                        {
                            CoreUtils.assetService.Destroy(effectGO);
                        }
                    });
                }
               
            });
        }

        public static void AddEffect(string effectName, Vector3 postion, Action<GameObject> callBack)
        {
            CoreUtils.assetService.Instantiate(effectName, (effectGO) =>
            {
                effectGO.name = effectName;
                effectGO.SetActive(true);
                effectGO.transform.position = postion;
                effectGO.transform.localScale = Vector3.one;

                callBack?.Invoke(effectGO);
            });
        }
        public static GameObject GetEffect(string effectName, Transform targetPart)
        {
            if (targetPart == null)
            {
                return null;
            }

            Transform effectGO = targetPart.Find(effectName);
            if (effectGO == null)
            {
                return null;
            }

            return effectGO.gameObject;
        }
        /// <summary>
        /// 查找子节点
        /// </summary>
        public static Transform FindDeepChild(GameObject _target, string _childName)
        {
            Transform resultTrs = null;
            resultTrs = _target.transform.Find(_childName);
            if (resultTrs == null)
            {
                foreach (Transform trs in _target.transform)
                {
                    resultTrs = FindDeepChild(trs.gameObject, _childName);
                    if (resultTrs != null)
                        return resultTrs;
                }
            }

            return resultTrs;
        }

        static List<Transform> trans_list = new List<Transform>();

        /// <summary>
        /// 查找子节点（多个）
        /// </summary>
        public static List<Transform> FindDeepMulChild(GameObject _target, string _childName)
        {
            trans_list.Clear();
            Transform[] list = _target.GetComponentsInChildren<Transform>(true);
            foreach (Transform tran in list)
            {
                //遍历当前物体及其所有子物体       
                if (tran.name == _childName)
                {
                    trans_list.Add(tran);
                }
            }

            return trans_list;
        }


        /// <summary>
        /// 线段与圆的交点
        /// </summary>
        /// <param name="ptStart">线段起点</param>
        /// <param name="ptEnd">线段终点</param>
        /// <param name="ptCenter">圆心坐标</param>
        /// <param name="Radius2">圆半径平方</param>
        /// <param name="ptInter1">交点1(若不存在返回65536)</param>
        /// <param name="ptInter2">交点2(若不存在返回65536)</param>
        public static bool LineInterCircle(Vector2 ptStart, Vector2 ptEnd, Vector2 ptCenter, double Radius2,
            ref Vector2 ptInter1, ref Vector2 ptInter2)
        {
            ptInter1.x = ptInter2.x = 65536.0f;
            ptInter2.y = ptInter2.y = 65536.0f;
            float fDis = (float) Math.Sqrt((ptEnd.x - ptStart.x) * (ptEnd.x - ptStart.x) +
                                           (ptEnd.y - ptStart.y) * (ptEnd.y - ptStart.y));
            Vector2 d = new Vector2();
            d.x = (ptEnd.x - ptStart.x) / fDis;
            d.y = (ptEnd.y - ptStart.y) / fDis;
            Vector2 E = new Vector2();
            E.x = ptCenter.x - ptStart.x;
            E.y = ptCenter.y - ptStart.y;
            float a = E.x * d.x + E.y * d.y;
            float a2 = a * a;
            float e2 = E.x * E.x + E.y * E.y;
            if ((Radius2 - e2 + a2) < 0)
            {
                return false;
            }
            else
            {
                float f = (float) Math.Sqrt(Radius2 - e2 + a2);
                float t = a - f;
                if (((t - 0.0) > -0.00001) && (t - fDis) < 0.00001)
                {
                    ptInter1.x = ptStart.x + t * d.x;
                    ptInter1.y = ptStart.y + t * d.y;
                }

                t = a + f;
                if (((t - 0.0) > -0.00001) && (t - fDis) < 0.00001)
                {
                    ptInter2.x = ptStart.x + t * d.x;
                    ptInter2.y = ptStart.y + t * d.y;
                }

                return true;
            }
        }
        public static bool DestroyAllChild(this Transform ransform)
        {
            int childCount = ransform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                CoreUtils.assetService.Destroy(ransform.GetChild(i).gameObject);
            }
            return childCount != 0;
        }

        public static void UIReLayout(GameButton btn)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(btn.GetComponent<RectTransform>());
        }
        
        public static void UIReLayout(GameObject btn)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(btn.GetComponent<RectTransform>());
        }

        public static int SubStringGetTotalIndex(string str)
        {            
            int currIndex = 0;
            int i = 0;
            int lastCount = 1;

            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
            do
            {
                lastCount = SubStringGetByteCount(byteArray, i);
                i = i + lastCount;
                currIndex = currIndex + 1;
            } while (lastCount > 0);
            return currIndex - 1;
        }

        public static int SubStringGetByteCount(byte[] byteArray, int index)
        {
            int byteCount = 1;
            if (byteArray == null)
            {
                byteCount = 0;
            }
            else
            {
                int length = byteArray.Length;
                if (length == 0 || index >= length)
                {
                    byteCount = 0;
                    return byteCount;
                }
                int currByte = byteArray[index];                
                if (currByte <= 0)
                {
                    byteCount = 0;
                }
                else
                {
                    if (currByte > 0 && currByte <= 127)
                    {
                        byteCount = 1;
                    }
                    else
                    {
                        if (currByte >= 192 && currByte <= 223)
                        {
                            byteCount = 2;
                        }
                        else
                        {
                            if (currByte >= 224 && currByte <= 239)
                            {
                                byteCount = 3;
                            }
                            else
                            {
                                if (currByte >= 240 && currByte <= 247)
                                {
                                    byteCount = 4;
                                }
                            }
                        }

                    }
                }
            }
            return byteCount;
        }

        public static string SubStringUTF8(string str, int startIndex, int endIndex = 0)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (startIndex < 0)
            {
                startIndex = SubStringGetTotalIndex(str) + startIndex + 1;
            }
            if (endIndex < 0)
            {
                endIndex = SubStringGetTotalIndex(str) + endIndex + 1;
            }
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
            if (endIndex == 0)
            {
                int sIndex = SubStringGetTrueIndex(byteArray, startIndex);
                int length = byteArray.Length - sIndex;
                return System.Text.Encoding.UTF8.GetString(byteArray, sIndex, length);
                //return str.Substring(SubStringGetTrueIndex(str, startIndex));
            }
            else
            {
                int sIndex = SubStringGetTrueIndex(byteArray, startIndex);
                int eIndex = SubStringGetTrueIndex(byteArray, endIndex + 1);
                int length = eIndex - sIndex;
                if (length >= 0)
                {
                    return System.Text.Encoding.UTF8.GetString(byteArray, sIndex, length);
                } else
                {
                    return "";
                }
                //return str.Substring(SubStringGetTrueIndex(str, startIndex), 
                //                     SubStringGetTrueIndex(str, endIndex+1)-1);
            }
        }

        public static int SubStringGetTrueIndex(byte[] byteArray, int index)
        {
            int currIndex = 0;
            int i = 0;
            int lastCount = 1;
            do
            {
                lastCount = SubStringGetByteCount(byteArray, i);
                i = i + lastCount;
                currIndex = currIndex + 1;
            } while (index > currIndex);

            return i - lastCount;
        }

        public static int SubStringGetTrueIndex(string str, int index)
        {
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(str);
            int currIndex = 0;
            int i = 0;
            int lastCount = 1;
            do
            {
                lastCount = SubStringGetByteCount(byteArray, i);
                i = i + lastCount;
                currIndex = currIndex + 1;
            } while (index > currIndex);

            return i - lastCount;
        }

        public static string FormatF2(float value)
        {
            return ((int)(value * 100) / 100.00f).ToString("f2");
        }

        public static void TranslatorSDK(LanguageText text)
        {
            // SDK 翻译功能已移除，直接保留原文
            // 后续可接入 Google Translate API 或其他翻译服务
        }

        //文本超出省略号格式化
        public static void FormatBeyondText(LanguageText targetText, string content)
        {
            float showWidth = targetText.GetComponent<RectTransform>().sizeDelta.x;
            string formatText = content;
            int len = SubStringGetTotalIndex(formatText);
            targetText.text = formatText;
            float curWidth = targetText.preferredWidth;
            while (showWidth < curWidth)
            {
                len = len - 3;
                formatText = SubStringUTF8(formatText, 1, len);
                targetText.text = formatText;
                curWidth = targetText.preferredWidth;
                if (curWidth <= showWidth)
                {
                    if (showWidth - curWidth < 15)
                    {
                        formatText = SubStringUTF8(formatText, 1, len - 3);
                    }
                    formatText = LanguageUtils.getTextFormat(300250, formatText);
                    break;
                }
            }
            targetText.text = formatText;
        }
        
        //清理<>与其包含的文本
        public static string ClearRichText(string txt)
        {
            Regex regex = new Regex("<.*?>", RegexOptions.Compiled);
            return regex.Replace(txt, string.Empty);
        }
        
        
        //立即获取ContentSizeFitter的区域
        public static Vector2 GetPreferredSize(ContentSizeFitter obj)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent <RectTransform>());
            return new Vector2(HandleSelfFittingAlongAxis(0,obj), HandleSelfFittingAlongAxis(1,obj));
        }
        //获取宽和高
        private static float HandleSelfFittingAlongAxis(int axis,ContentSizeFitter obj)
        {
            ContentSizeFitter.FitMode fitting = (axis == 0 ? obj.horizontalFit : obj.verticalFit);
            if (fitting == ContentSizeFitter.FitMode.MinSize)
            {
                return LayoutUtility.GetMinSize(obj.GetComponent<RectTransform>(), axis);
            }
            else
            {
                return LayoutUtility.GetPreferredSize(obj.GetComponent<RectTransform>(), axis);
            }
        }
    }
}
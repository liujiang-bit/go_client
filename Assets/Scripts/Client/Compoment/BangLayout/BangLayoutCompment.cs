//
// Author:  Johance
//
using GameFramework;
using Skyunion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BangLayoutCompment : MonoBehaviour {
    public enum BangLayoutStyle
    {
        BangLayoutStyle_None = 0,
        [BangLayoutStyleEditor(typeof(RectTransform), "开启预览")]
        BangLayoutStyle_Preview = 0x01,
        [BangLayoutStyleEditor(typeof(RectTransform), "上")]
        BangLayoutStyle_Top = 0x02,
        [BangLayoutStyleEditor(typeof(RectTransform), "下")]
        BangLayoutStyle_Bottom = 0x04,
        [BangLayoutStyleEditor(typeof(RectTransform), "左")]
        BangLayoutStyle_Left = 0x08,
        [BangLayoutStyleEditor(typeof(RectTransform), "右")]
        BangLayoutStyle_Right = 0x10,
        [BangLayoutStyleEditor(typeof(RectTransform), "左图片")]
        BangLayoutStyle_LeftImage = 0x20,
        [BangLayoutStyleEditor(typeof(RectTransform), "右图片")]
        BangLayoutStyle_RightImage = 0x40,
    }
    private RectTransform gui;
    [HideInInspector]
    [SerializeField]
    protected int _bang_Layout = 0;
    public int BangLayout
    {
        get
        {
            return _bang_Layout;
        }
        set
        {
            int oldValue = _bang_Layout;
            _bang_Layout = value;
            OnBangLayoutPropertyChanged(oldValue, value);
        }
    }

    // 先使用ipx的安全区域换算过来的  132 132 0 63 1.5倍
    static private Vector4 _DefaultSafeAreaInsets = new Vector4(88, 88, 0, 42);
    static public Vector4 DefaultSafeAreaInsets { get { return _DefaultSafeAreaInsets; } }
    public Vector4 SafeAreaInsets = DefaultSafeAreaInsets;

    [SerializeField]
    private GameObject marginTemplate = null;

    private RectTransform marginPanel = null;
    private RectTransform marginLeft = null;
    private RectTransform marginRight = null;

    public void SetStyle(BangLayoutStyle style, bool bEnable)
    {
        if (style == BangLayoutStyle.BangLayoutStyle_Preview || GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
        {
            UpdateBangLayout(style);
        }
        if (bEnable)
            BangLayout = BangLayout | (int)style;
        else
            BangLayout = BangLayout & ~((int)style);

        //// 属性如果一样就不刷新UI
        //if (GetStyle(style) == bEnable)
        //    return;

        //bool bUpdateLayout = GetStyle(BangLayoutStyle.BangLayoutStyle_Preview);
        //if (style == BangLayoutStyle.BangLayoutStyle_Preview)
        //{
        //    bUpdateLayout = bUpdateLayout != bEnable;
        //}
        //if (bUpdateLayout)
        //{
        //    UpdateBangLayout(style);
        //}
        //if (bEnable)
        //    BangLayout = BangLayout | (int)style;
        //else
        //    BangLayout = BangLayout & ~((int)style);
    }

    public bool GetStyle(BangLayoutStyle style)
    {
        return (BangLayout & (int)style) != 0;
    }

    internal virtual void OnBangLayoutPropertyChanged(int oldValue, int newValue)
    {
    }

    protected virtual bool UpdateBangLayout(BangLayoutStyle style)
    {
        if (gui == null)
        {
            gui = GetComponent<RectTransform>();
        }
        if (gui == null)
        {
            return false;
        }
        switch (style)
        {
            case BangLayoutStyle.BangLayoutStyle_Preview:
                {
                    if (marginTemplate != null)
                    {
                        if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                        {
                            if (marginPanel)
                            {
                                DestroyImmediate(marginPanel.gameObject);
                                marginLeft = null;
                                marginRight = null;
                                marginPanel = null;
                            }
                        }
                        else
                        {
                            if (marginPanel == null)
                            {
                                marginPanel = Instantiate<GameObject>(marginTemplate, gui).GetComponent<RectTransform>();
                                marginPanel.gameObject.name = "BangMargin";
                                marginLeft = marginPanel.transform.Find("Left").GetComponent<RectTransform>();                                
                                marginRight = marginPanel.transform.Find("Right").GetComponent<RectTransform>();
                            }
                        }
                    }
                    else
                    {
                        if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                        {
                            if (marginPanel)
                            {
                                marginPanel.gameObject.SetActive(true);
                                marginLeft = null;
                                marginRight = null;
                                marginPanel = null;
                            }
                        }
                        else
                        {
                            if (marginPanel != null)
                            {
                                marginPanel.gameObject.name = "BangMargin";
                                marginLeft = marginPanel.transform.Find("Left").GetComponent<RectTransform>();
                                marginRight = marginPanel.transform.Find("Right").GetComponent<RectTransform>();
                            }
                        }
                    }

                    foreach (BangLayoutStyle item in Enum.GetValues(typeof(BangLayoutStyle)))
                    {
                        if((int)item > 1 && GetStyle(item))
                        {
                            UpdateBangLayout(item);
                        }
                    }
                }
                return false;
            case BangLayoutStyle.BangLayoutStyle_Top:
                {
                    if (SafeAreaInsets.z < 0.0001)
                        break;
                    Vector2 max = gui.offsetMax;
                    if(GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                    {
                        if(GetStyle(style))
                        {
                            max.y += SafeAreaInsets.z;
                        }
                        else
                        {
                            max.y -= SafeAreaInsets.z;
                        }
                        gui.offsetMax = max;
                    }
                    else
                    {
                        if (GetStyle(style))
                        {
                            max.y -= SafeAreaInsets.z;
                        }
                        else
                        {
                            max.y += SafeAreaInsets.z;
                        }
                        gui.offsetMax = max;
                    }
                }
                return true;
            case BangLayoutStyle.BangLayoutStyle_Right:
                {
                    if (SafeAreaInsets.y < 0.0001)
                        break;
                    Vector2 max = gui.offsetMax;
                    if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                    {
                        if (GetStyle(style))
                        {
                            max.x += SafeAreaInsets.y;
                        }
                        else
                        {
                            max.x -= SafeAreaInsets.y;
                        }
                        gui.offsetMax = max;
                    }
                    else
                    {
                        if (GetStyle(style))
                        {
                            max.x -= SafeAreaInsets.y;
                        }
                        else
                        {
                            max.x += SafeAreaInsets.y;
                        }
                        gui.offsetMax = max;
                    }
                }
                return true;
            case BangLayoutStyle.BangLayoutStyle_Bottom:
                {
                    if (SafeAreaInsets.w < 0.0001)
                        break;
                    Vector2 min = gui.offsetMin;
                    if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                    {
                        if (GetStyle(style))
                        {
                            min.y -= SafeAreaInsets.w;
                        }
                        else
                        {
                            min.y += SafeAreaInsets.w;
                        }
                    }
                    else
                    {
                        if (GetStyle(style))
                        {
                            min.y += SafeAreaInsets.w;
                        }
                        else
                        {
                            min.y -= SafeAreaInsets.w;
                        }
                    }
                    gui.offsetMin = min;

                    if(marginPanel != null)
                    {
                        min = marginPanel.offsetMin;
                        if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                        {
                            if (GetStyle(style))
                            {
                                min.y += SafeAreaInsets.w;
                            }
                            else
                            {
                                min.y -= SafeAreaInsets.w;
                            }
                        }
                        else
                        {
                            if (GetStyle(style))
                            {
                                min.y -= SafeAreaInsets.w;
                            }
                            else
                            {
                                min.y += SafeAreaInsets.w;
                            }
                        }
                        marginPanel.offsetMin = min;
                    }

                }
                return true;
            case BangLayoutStyle.BangLayoutStyle_Left:
                {
                    if (SafeAreaInsets.x < 0.0001)
                        break;
                    Vector2 min = gui.offsetMin;
                    if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                    {
                        if (GetStyle(style))
                        {
                            min.x -= SafeAreaInsets.x;
                        }
                        else
                        {
                            min.x += SafeAreaInsets.x;
                        }
                    }
                    else
                    {
                        if (GetStyle(style))
                        {
                            min.x += SafeAreaInsets.x;
                        }
                        else
                        {
                            min.x -= SafeAreaInsets.x;
                        }
                    }
                    gui.offsetMin = min;
                }
                return true;
            case BangLayoutStyle.BangLayoutStyle_LeftImage:
                {
                    if (SafeAreaInsets.x < 0.0001 || marginLeft == null)
                        break;
                    Vector2 sizeDelta = marginLeft.sizeDelta;
                    if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                    {
                        if (GetStyle(style))
                        {
                            sizeDelta.x = 0;
                        }
                        else
                        {
                            sizeDelta.x = SafeAreaInsets.x;
                        }
                    }
                    else
                    {
                        if (GetStyle(style))
                        {
                            sizeDelta.x = SafeAreaInsets.x;
                        }
                        else
                        {
                            sizeDelta.x = 0;
                        }
                    }
                    marginLeft.sizeDelta = sizeDelta;
                }
                return true;
            case BangLayoutStyle.BangLayoutStyle_RightImage:
                {
                    if (SafeAreaInsets.y < 0.0001 || marginRight == null)
                        break;
                    Vector2 sizeDelta = marginRight.sizeDelta;
                    if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                    {
                        if (GetStyle(style))
                        {
                            sizeDelta.x = 0;
                        }
                        else
                        {
                            sizeDelta.x = SafeAreaInsets.y;
                        }
                    }
                    else
                    {
                        if (GetStyle(style))
                        {
                            sizeDelta.x = SafeAreaInsets.y;
                        }
                        else
                        {
                            sizeDelta.x = 0;
                        }
                    }
                    marginRight.sizeDelta = sizeDelta;
                }
                return true;
        }
        return false;
    }

    private ScreenOrientation _screenOrientation = ScreenOrientation.Unknown;
    private RectTransform _uistrench = null;
    private Vector2 _uiStrenchPos;
    // Use this for initialization
    void Awake () {
        gui = GetComponent<RectTransform>();
        if (Application.isPlaying)
        {
            Debug.Log($"SafeArea:{ getSafeAreaInset()}");
            if (BangLayout > 1)
            {
                // 建议策划都不要使用预览
                if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                {
                    SetStyle(BangLayoutStyle.BangLayoutStyle_Preview, false);
                }
                SafeAreaInsets = getSafeAreaInset() * getScreenScale();
                bool isBang = !getSafeAreaInset().Equals(Vector4.zero);
                if(isBang)
                {
                    EnableBang();
                }
                else
                {
                    enabled = false;
                }
                // 建议策划都不要使用预览
                //if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview) != isBang)
                //{
                //    if (GetStyle(BangLayoutStyle.BangLayoutStyle_Preview))
                //    {
                //        SetStyle(BangLayoutStyle.BangLayoutStyle_Preview, false);
                //    }
                //    if (isBang)
                //        SafeAreaInsets =
                //        bool isBangUI = GetStyle(BangLayoutStyle.BangLayoutStyle_Preview);
                //    if (isBangUI != true)
                //    {
                //        SetStyle(BangLayoutStyle.BangLayoutStyle_Preview, !isBangUI);
                //    }
                //}
             }
        }
    }
    private static Vector4 getSafeAreaInset()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return Vector4.zero;
#endif
        return CoreUtils.getSafeAreaInset();
//#if UNITY_EDITOR
//        if (Screen.width == 2436 && Screen.height == 1125)
//        {
//            return new Vector4(88, 88, 0, 42);
//        }
//        return Vector4.zero;
//#else
//        return new Vector4(Screen.safeArea.xMin, Screen.width - Screen.safeArea.xMax, Screen.height - Screen.safeArea.yMax, Screen.safeArea.yMin);
//#endif
    }
    private static float getScreenScale()
    {
        return CoreUtils.getScreenScale();
    }

    private void EnableBang()
    {
        var uiStrench = GetComponentInChildren<NbUIStrench>();
        if (uiStrench)
        {
            _uistrench = uiStrench.GetComponent<RectTransform>();
            _uiStrenchPos = _uistrench.anchoredPosition;
        }
        _screenOrientation = Screen.orientation;

        SafeAreaInsets = getSafeAreaInset() * getScreenScale();

        SetStyle(BangLayoutStyle.BangLayoutStyle_Preview, true);
        UpdateUIStrench();
    }
    static public BangLayoutCompment BeginAddFeacture(GameObject ui)
    {
        var safeAreaInsets = getSafeAreaInset() * getScreenScale();
        bool isBang = !safeAreaInsets.Equals(Vector4.zero);
        if (isBang == false || ui.GetComponent<BangLayoutCompment>() != null)
        {
            return null;
        }
        BangLayoutCompment layout = ui.AddComponent<BangLayoutCompment>();
        return layout;
    }

    public void AddFeature(GameObject margin)
    {
        if (margin == null)
            return;

        if (gui == null)
        {
            gui = GetComponent<RectTransform>();
        }

        margin.transform.parent = gui;
        marginPanel = margin.GetComponent<RectTransform>();
        marginPanel.transform.parent = gui;
        marginPanel.gameObject.name = "BangMargin";
        marginLeft = marginPanel.transform.Find("Left").GetComponent<RectTransform>();
        marginRight = marginPanel.transform.Find("Right").GetComponent<RectTransform>();
    }

    public void EndAddFeacture()
    {
        EnableBang();
    }

    // Update is called once per frame
    void Update () {
        //if (Input.GetKeyUp(KeyCode.LeftArrow))
        //{
        //    Plateform.Instance.setSafeAreaInsets("88, 0, 0, 42, 1");
        //    bFalse = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.RightArrow))
        //{
        //    Plateform.Instance.setSafeAreaInsets("0, 88, 42, 0, 1");
        //    bFalse = true;
        //}
        if (_screenOrientation == ScreenOrientation.Unknown)
            return;

        if(_screenOrientation != Screen.orientation)
        {
            SetStyle(BangLayoutStyle.BangLayoutStyle_Preview, false);
            SafeAreaInsets = getSafeAreaInset() * getScreenScale();
            _screenOrientation = Screen.orientation;
            SetStyle(BangLayoutStyle.BangLayoutStyle_Preview, true);
            UpdateUIStrench();
        }
	}

    void UpdateUIStrench()
    {
        if(_uistrench != null)
        {
            var newPos = _uiStrenchPos;
            newPos.x = 0;
            if (GetStyle(BangLayoutStyle.BangLayoutStyle_Left))
            {
                newPos.x -= SafeAreaInsets.x * _uistrench.pivot.x;
            }
            if (GetStyle(BangLayoutStyle.BangLayoutStyle_Right))
            {
                newPos.x += SafeAreaInsets.y * _uistrench.pivot.x;
            }
            if (GetStyle(BangLayoutStyle.BangLayoutStyle_Top))
            {
                newPos.y += SafeAreaInsets.z * _uistrench.pivot.y;
            }
            if (GetStyle(BangLayoutStyle.BangLayoutStyle_Bottom))
            {
                newPos.y -= SafeAreaInsets.w * _uistrench.pivot.y;
            }
            _uistrench.anchoredPosition = newPos;
        }
    }
}

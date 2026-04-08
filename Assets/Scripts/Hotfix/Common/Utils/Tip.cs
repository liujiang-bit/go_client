using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Skyunion;
using System;
using Data;

namespace Game
{
    public class Tip
    {
        public enum TipStyle
        {
            Top,
            Middle,
            City,
            AllianceHelp,
            Other,
        }
        
        public enum AllianceHelpType
        {
            Self,
            Other
        }

        public class OtherAssetData
        {
            public string otherAssetName;
            public Action<GameObject> openCallback;
            public float showTimer;

            public OtherAssetData(string otherAssetName , Action<GameObject> callback,float showTimer = 0.5f)
            {
                this.otherAssetName = otherAssetName;
                openCallback = callback;
                this.showTimer = showTimer;
            }
        }
        //单次播放持续时间
        public static float ShowTime = 2f;
        //连续播放的持续时间
        public static float QuickShowTime = 1f;

        private static bool m_isInitTime;

        public string text;
        public TipStyle style;
        public GameObject go;
        public OtherAssetData otherAssetData;

        public AllianceHelpType HelpType;

        public static Tip CreateTip(string text, TipStyle style = TipStyle.Top)
        {
            InitTime();
            Tip tip = new Tip();
            tip.text = text;
            tip.style = style;
            return tip;
        }

        public static Tip CreateTip(int languageID, TipStyle style = TipStyle.Top)
        {
            InitTime();
            Tip tip = new Tip();
            tip.text = LanguageUtils.getText(languageID);
            tip.style = style;
            return tip;
        }

        public static Tip CreateTip(int languageID, params object[] args)
        {
            InitTime();
            Tip tip = new Tip();
            tip.text = string.Format(LanguageUtils.getText(languageID),args);
            return tip;
        }
        public static Tip CreateTip(OtherAssetData otherAssetData)
        {
            InitTime();
            Tip tip = new Tip();
            tip.otherAssetData = otherAssetData;
            tip.SetStyle(TipStyle.Other);
            return tip;
        }

        public Tip SetStyle(TipStyle style)
        {
            this.style = style;
            return this;
        }

        public Tip SetHelpType(AllianceHelpType type)
        {
            this.HelpType = type;
            return this; 
        }

        private static void InitTime()
        {
            if (!m_isInitTime)
            {
                ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                QuickShowTime = config.upTipAnimTimeOn;
                ShowTime = config.upTipKeepLife;
                m_isInitTime = true;
            }
        }

        public Tip Show()
        {
            TipManager.Instance.Enqueue(this);
            return this;
        }

    }

    public class TipManager : Hotfix.TSingleton<TipManager>
    {
        private Dictionary<string, GameObject> m_tipAssets = new Dictionary<string, GameObject>();
        private Queue<Tip> m_topTips = new Queue<Tip>();
        private bool isDequeueTop = false;
        private Queue<Tip> m_midTips = new Queue<Tip>();
        private bool isDequeueMid = false;
        private Queue<Tip> m_cityTips = new Queue<Tip>();
        private bool isDequeueCity = false;
        private Queue<Tip> m_otherTips = new Queue<Tip>();
        private bool isDequeueOther = false;
        private Transform tipLayer;

        private Timer m_lastTipTimer;
        private Tip m_showingTip;   //表现中tip 
        private Tip m_replaceingTip;//替换中tip

        private Tip m_playMidTip;
        private Timer m_playMidTimer;

        public void Enqueue(Tip tip)
        {
            switch (tip.style)
            {
                case Tip.TipStyle.Top:
                    m_topTips.Enqueue(tip);
                    InitAsset(RS.Tip_Up, OnTopAssetLoad);
                    break;
                case Tip.TipStyle.AllianceHelp:
                    m_topTips.Enqueue(tip);
                    InitAsset(RS.Tip_AllianceHelp, OnTopAssetLoad);
                    break;
                case Tip.TipStyle.Middle:
                    m_midTips.Enqueue(tip);
                    InitAsset(RS.Tip_Mid, OnMidAssetLoad);
                    break;
                case Tip.TipStyle.City:
                    m_cityTips.Enqueue(tip);
                    InitAsset(RS.Tip_City, OnCityAssetLoad);
                    break;
                case Tip.TipStyle.Other:
                    m_otherTips.Enqueue(tip);
                    InitAsset(tip.otherAssetData.otherAssetName, OnOtherAssetLoad);
                    break;
                default: break;
            }
        }

        private void OnTopAssetLoad()
        {
            if (isDequeueTop)
            {
                return;
            }
            if (m_replaceingTip != null)
            {
                return;
            }
            TopDequeue();
        }

        private void OnMidAssetLoad()
        {
            //if (!isDequeueMid)
            //{
            //    MidDequeue();
            //}
            MidDequeue();
        }
        private void OnCityAssetLoad()
        {
            if (!isDequeueCity)
            {
                CityDequeue();
            }
        }

        private void OnOtherAssetLoad()
        {
            if (!isDequeueOther)
            {
                OtherDequeue();
            }
        }

        private void TopDequeue()
        {
            if (m_topTips.Count > 0)
            {
                Tip tip = m_topTips.Dequeue();
                GameObject go = null;
                if (tip.style == Tip.TipStyle.AllianceHelp)
                {
                    if (!m_tipAssets.ContainsKey(RS.Tip_AllianceHelp))
                    {
                        Debug.LogErrorFormat("TopDequeue not find:{0}", RS.Tip_AllianceHelp);
                        return;
                    }
                    go = CoreUtils.assetService.Instantiate(m_tipAssets[RS.Tip_AllianceHelp]);
                    UI_Common_HelpTipsView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_HelpTipsView>(go);
                    tipView.m_lbl_message_LanguageText.text = tip.text;

                    if (tip.HelpType == Tip.AllianceHelpType.Other)
                    {
                        ClientUtils.LoadSprite(tipView.m_img_icon_PolygonImage, RS.Tip_HelpOther);
                    }
                    else
                    {
                        ClientUtils.LoadSprite(tipView.m_img_icon_PolygonImage, RS.Tip_AskForHelp);
                    }
                }
                else
                {
                    if (!m_tipAssets.ContainsKey(RS.Tip_Up))
                    {
                        Debug.LogErrorFormat("TopDequeue not find:{0}", RS.Tip_Up);
                        return;
                    }
                    go = CoreUtils.assetService.Instantiate(m_tipAssets[RS.Tip_Up]);
                    UI_Common_UpTipsView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_UpTipsView>(go);
                    tipView.m_lbl_message_LanguageText.text = tip.text;
                    tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta = new Vector2(tipView.m_lbl_message_LanguageText.preferredWidth + 50, tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta.y);
                }
                isDequeueTop = true;
                go.transform.SetParent(tipLayer);
                go.transform.localScale = Vector3.one;
                tip.go = go;
                CancelWaitTimer();
                ReplaceTopTip();
                m_showingTip = tip;
                Timer.Register(Tip.QuickShowTime, () =>
                {
                    OnQueueComplete2();
                });
            }
        }

        private void RemovePlayMidTip()
        {
            if (m_playMidTip != null)
            {
                if (m_playMidTip.go != null)
                {
                    GameObject.DestroyImmediate(m_playMidTip.go);
                }
                m_playMidTip = null;
            }
            if (m_playMidTimer != null)
            {
                m_playMidTimer.Cancel();
                m_playMidTimer = null;
            }
        }

        private void MidDequeue()
        {
            if (m_midTips.Count > 0)
            {
                RemovePlayMidTip();

                Tip tip = m_midTips.Dequeue();
                GameObject go = CoreUtils.assetService.Instantiate(m_tipAssets[RS.Tip_Mid]);
                go.transform.SetParent(tipLayer);
                go.transform.localScale = Vector3.one;
                tip.go = go;
                UI_Common_MidTipsView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_MidTipsView>(go);
                tipView.m_lbl_message_LanguageText.text = tip.text;
                tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta = new Vector2(tipView.m_lbl_message_LanguageText.preferredWidth + 50, tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta.y);

                m_playMidTip = tip;
                m_playMidTimer = Timer.Register(Tip.QuickShowTime, () =>
                {
                    OnQueueCompleteMid(tip);
                });
            }
        }

        private void OnQueueCompleteMid(Tip tip)
        {
            if (tip.go != null)
            {
                AutoPlayAndDestroyTip component = tip.go.GetComponent<AutoPlayAndDestroyTip>();
                if (component != null)
                {
                    component.PlayEndAni();
                }
                else
                {
                    GameObject.DestroyImmediate(tip.go);
                }
            }
        }

        private GameObject m_cacheCityTipGo;
        private Timer m_cacheTimer;
        private void CityDequeue()
        {
            if (CoreUtils.uiManager.ExistUI(UI.s_Loading))
            {
                if (m_cityTips.Count > 0)
                {
                    m_cityTips.Dequeue();
                }

                return;
            }
            
            if (m_cityTips.Count > 0)
            {
                isDequeueCity = true;
                Tip tip = m_cityTips.Peek();

                GameObject go = m_cacheCityTipGo;
                if (go == null)
                {
                    go = CoreUtils.assetService.Instantiate(m_tipAssets[RS.Tip_City]);
                    go.transform.SetParent(tipLayer);
                    go.transform.localScale = Vector3.one;
                    m_cacheCityTipGo = go;
                }
                tip.go = go;
                UI_Common_MidTipsView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_MidTipsView>(go);
                tipView.m_lbl_message_LanguageText.text = tip.text;
                tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta = new Vector2(tipView.m_lbl_message_LanguageText.preferredWidth + 50, tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta.y);
                
                m_cityTips.Dequeue();
                
                isDequeueCity = false;
                
                if (m_cacheTimer != null)
                {
                    m_cacheTimer.Cancel();
                    m_cacheTimer = null;
                }
                
                m_cacheTimer = Timer.Register(Tip.QuickShowTime, () =>
                {
                        AutoPlayAndDestroyTip component = m_cacheCityTipGo.GetComponent<AutoPlayAndDestroyTip>();
                        if (component != null)
                        {
                            component.PlayEndAni();
                        }
                        m_cacheCityTipGo = null;
                        m_cacheTimer = null;
                });
            }
        }


        private void OtherDequeue()
        {
            if (m_otherTips.Count > 0)
            {
                isDequeueOther = true;
                Tip tip = m_otherTips.Peek();
                GameObject go = CoreUtils.assetService.Instantiate(m_tipAssets[tip.otherAssetData.otherAssetName]);
                go.transform.SetParent(tipLayer);
                go.transform.localScale = Vector3.one;
                tip.go = go;
                tip.otherAssetData.openCallback?.Invoke(go);
                // UI_Common_MidTipsView tipView = MonoHelper.AddHotFixViewComponent<UI_Common_MidTipsView>(go);
                // tipView.m_lbl_message_LanguageText.text = tip.text;
                // tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta = new Vector2(tipView.m_lbl_message_LanguageText.preferredWidth + 50, tipView.m_img_bg_PolygonImage.rectTransform.sizeDelta.y);
                Timer.Register(tip.otherAssetData.showTimer, () =>
                {
                    OnQueueComplete(m_otherTips, ref isDequeueOther, OtherDequeue);
                });
            }
        }

        private void OnQueueComplete(Queue<Tip> queue, ref bool working, Action nextMove)
        {
            if (queue.Count > 0)
            {
                Tip tip = queue.Dequeue();
                //播放完毕自动销毁
                if (tip.go != null)
                {
                    AutoPlayAndDestroyTip component = tip.go.GetComponent<AutoPlayAndDestroyTip>();
                    if (component != null)
                    {
                        component.PlayEndAni();
                    }
                    else
                    {
                        GameObject.DestroyImmediate(tip.go);
                    }
                }
                working = false;
                nextMove?.Invoke();
            }
        }
        
        private void OnQueueCompleteCity(Queue<Tip> queue, ref bool working, Action nextMove)
        {
            if (queue.Count > 0)
            {
                Tip tip = queue.Dequeue();
                working = false;
                nextMove?.Invoke();
            }
        }

        private void ReplaceTopTip()
        {
            if (m_showingTip!=null && m_showingTip.go != null)
            {
                m_replaceingTip = m_showingTip;

                AutoPlayAndDestroyTip component = m_replaceingTip.go.GetComponent<AutoPlayAndDestroyTip>();
                if (component != null)
                {
                    float length = component.PlayChangeAni();
                    Timer.Register(length, () => {

                        m_replaceingTip = null;
                        OnTopAssetLoad();
                    });
                }
                else
                {
                    GameObject.DestroyImmediate(m_replaceingTip.go);
                    m_replaceingTip = null;
                }
            }
            m_showingTip = null;
        }

        private void OnQueueComplete2()
        {
            isDequeueTop = false;
            int count = m_topTips.Count;
            if (m_topTips.Count > 0) //如果有待显示的tip 且替换tip不存在 则顶替当前tip
            {
                if (m_replaceingTip == null)
                {
                    ReplaceTopTip();
                    TopDequeue();
                }
            }
            else //只有一个tip 则需要等待一段时间再移除
            {
                CancelWaitTimer();
                m_lastTipTimer = Timer.Register(Tip.ShowTime, () =>
                {
                    m_lastTipTimer = null;

                    if (m_showingTip !=null && m_showingTip.go != null)
                    {
                        AutoPlayAndDestroyTip component = m_showingTip.go.GetComponent<AutoPlayAndDestroyTip>();
                        if (component != null)
                        {
                            component.PlayEndAni();
                        }
                        else
                        {
                            GameObject.DestroyImmediate(m_showingTip.go);
                        }
                    }
                    m_showingTip = null;

                    OnTopAssetLoad();
                });
            }
        }

        private void CancelWaitTimer()
        {
            if (m_lastTipTimer != null)
            {
                m_lastTipTimer.Cancel();
                m_lastTipTimer = null;
            }
        }

        private void InitAsset(string assetName,Action callBack)
        {
            if(tipLayer==null)
            {
                tipLayer = CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer).Find("pl_tip");
            }
            if(!m_tipAssets.ContainsKey(assetName))
            {
                CoreUtils.assetService.LoadAssetAsync<GameObject>(assetName, (asset) =>
                {
                    m_tipAssets[assetName] = asset.asset() as GameObject;
                    callBack();
                });
            }
            else
            {
                callBack();
            }
        }

        public void Clear()
        {
            RemovePlayMidTip();
        }
    }
}

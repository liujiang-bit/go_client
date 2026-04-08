using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Skyunion;
using UnityEngine.UI;

namespace Client
{

    public class RssAniGroup
    {
        public List<RssAniItem> m_rssAniItems;

        public float m_step1TotalClock = 1f;

        public float m_step2TotalClock = 0.5f;

        public float m_step1VT = 1f;
        public Vector3 LaunchAngle = Vector3.zero;

        public float m_tClock;

        public int m_passFrame = 1;

        public float m_step1Dist = 200f;

        public bool m_is2D = true;

        public Vector3 m_scaleSpeed = Vector3.zero;

        public float m_randomDirectFactor = 1f;

        public Vector3 m_startPos;

        public Vector3 m_targetPos;

        public Vector3 m_targetUIPos;

        public Action m_startReachCallback;

        public Action m_beforeDestroyCallback;
        public bool m_destroy = true;

        public Action m_resAniDoneCallback;

        public Action m_stepOneDoneCallback;

        private RssAniGroup(Vector3 startPos, Vector3 targetUIPos)
        {
            m_rssAniItems = new List<RssAniItem>();
            this.m_is2D = true;
            this.m_startPos = startPos;
            this.m_targetUIPos = targetUIPos;
            this.m_targetPos = targetUIPos;
        }

        public static RssAniGroup Register(Vector3 startPos, Vector3 targetUIPos)
        {
            RssAniGroup group = new RssAniGroup(startPos, targetUIPos);

            return group;
        }

        public static RssAniGroup Register()
        {
            RssAniGroup group = new RssAniGroup(Vector3.zero, Vector3.zero);

            return group;
        }

        public RssAniGroup SetStartPos(Transform worldObj)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, 
WorldCamera.Instance().GetCamera().WorldToScreenPoint(worldObj.position), CoreUtils.uiManager.GetUICamera(), out pos);
            return SetStartPos(pos);
        }

        public RssAniGroup SetStartPos(RectTransform uiObj)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(uiObj.position), CoreUtils.uiManager.GetUICamera(), out pos);
            return SetStartPos(pos);
        }

        public RssAniGroup SetStartPos(Vector3 pos)
        {
            this.m_startPos = pos;
            return this;
        }

        public RssAniGroup SetEndPos(Transform worldObj)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, worldObj.position, CoreUtils.uiManager.GetUICamera(), out pos);
            return SetEndPos(pos);
        }

        public RssAniGroup SetEndPos(RectTransform uiObj)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(uiObj.position), CoreUtils.uiManager.GetUICamera(), out pos);
            return SetEndPos(pos);
        }

        public RssAniGroup SetEndPos(Vector3 pos)
        {
            this.m_targetUIPos = pos;
            this.m_targetPos = pos;
            return this;
        }

        /// <summary>
        /// 设置飘飞物体
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="scale"></param>
        /// <param name="count"></param>
        /// <param name="randomDirection"></param>
        /// <returns></returns>
        public RssAniGroup FlyItem<T>(string assetName,Vector3 scale,int count = 1,bool randomDirection = true) where T:UnityEngine.Object
        {
            CoreUtils.assetService.LoadAssetAsync<T>(assetName,(asset)=>
            {
                UnityEngine.Object go = asset.asset() as UnityEngine.Object;
                if(go is Sprite)
                {
                    Sprite sprite = go as Sprite;
                    GameObject tmpObj = new GameObject();
                    tmpObj.AddComponent<Image>().sprite = sprite;
                    FlyItem(tmpObj,scale,count, randomDirection);
                    asset.Attack(tmpObj);
                }
                else if(go is GameObject)
                {
                    FlyItem(go as GameObject, scale, count, randomDirection);
                    asset.Attack(go);
                }

            }, null);
            return this;
        }

        public RssAniGroup FlyItem(UnityEngine.GameObject go, Vector3 scale, int count = 1,bool randomDirection = true)
        {
            Canvas tipLayer = CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer).GetComponent<Canvas>();
            for (int i = 0; i < count; i++)
            {
                RssAniItem rssAniItem = new RssAniItem();
                rssAniItem.m_spos = this.m_startPos;
                rssAniItem.m_step = 1;
                rssAniItem.m_startFrame = (float)(i + 1) * 0.015f;
                rssAniItem.m_dir = GenerateDirect(this.m_randomDirectFactor);
                float num = Vector3.Magnitude(rssAniItem.m_dir);
                rssAniItem.m_a = (0f - num) / (this.m_step1TotalClock * this.m_step1TotalClock);
                rssAniItem.m_v0 = this.m_step1Dist * num / this.m_step1TotalClock;
                GameObject tmpObj = go;
                tmpObj.transform.SetParent(CoreUtils.uiManager.GetUILayer((int)UILayer.TipLayer));
                if (randomDirection)
                {
                    tmpObj.transform.eulerAngles = new Vector3(0, 0, UnityEngine.Random.Range(0, 360));
                }
                tmpObj.transform.localScale = scale;
                SpriteRenderer sr = tmpObj.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = tipLayer.sortingOrder + i * 50;
                }
                rssAniItem.m_gameObject = tmpObj;
                if (this.m_is2D)
                {
                    rssAniItem.m_gameObject.GetComponent<RectTransform>().anchoredPosition = this.m_startPos;
                }
                else
                {
                    rssAniItem.m_gameObject.transform.position = Vector3.zero;
                    rssAniItem.m_gameObject.transform.eulerAngles = new Vector3(45f, 0f, 0f);
                    rssAniItem.m_baseScale = rssAniItem.m_gameObject.transform.localScale;
                }
                this.m_rssAniItems.Add(rssAniItem);
            }
            ClientUtils.rssAnimationManager.AddAnimaitonGroup(this);
            return this;
        }

        public RssAniGroup SetAniTime(float step1Time, float step2Time)
        {
            this.m_step1TotalClock = step1Time;
            this.m_step2TotalClock = step2Time;
            return this;
        }

        public RssAniGroup SetScale2D(float start, float end)
        {
            float num = (end - start) / this.m_step2TotalClock;
            this.m_scaleSpeed = new Vector3(num, num, 0f);
            return this;
        }

        public RssAniGroup SetAniStep1VTAndDist(float vt, float dist)
        {
            this.m_step1VT = vt;
            this.m_step1Dist = dist;
            return this;
        }
        public RssAniGroup SetLaunchAngle(Vector3 launchAngle)
        {
            this.LaunchAngle = launchAngle;
            return this;
        }
        public RssAniGroup SetAniDoneCallBack(Action callBack)
        {
            this.m_resAniDoneCallback = callBack;
            return this;
        }

        public RssAniGroup SetStepOneCallBack(Action callBack)
        {
            this.m_stepOneDoneCallback = callBack;
            return this;
        }

        public RssAniGroup SetStartReachCallback(Action callBack)
        {
            this.m_startReachCallback = callBack;
            return this;
        }
        public RssAniGroup SetDestroy(bool destroy)
        {
                this.m_destroy = destroy;
            return this;
        }

        /// <summary>
        /// 需要手动在需要的UI层级Instantiate
        /// </summary>
        /// <param name="gameObjs"></param>
        /// <returns></returns>
        public RssAniGroup SetAniGameObjects(GameObject[] gameObjs)
        {
            for(int i = 0; i < gameObjs.Length; i++)
            {
                RssAniItem rssAniItem = new RssAniItem();
                rssAniItem.m_spos = this.m_startPos;
                rssAniItem.m_step = 1;
                rssAniItem.m_startFrame = (float)(i + 1) * 0.015f;
                rssAniItem.m_dir = GenerateDirect(this.m_randomDirectFactor);
                float num = Vector3.Magnitude(rssAniItem.m_dir);
                rssAniItem.m_a = (0f - num) / (this.m_step1TotalClock * this.m_step1TotalClock);
                rssAniItem.m_v0 = this.m_step1Dist * num / this.m_step1TotalClock;
                rssAniItem.m_gameObject = gameObjs[i];
                if (this.m_is2D)
                {
                    rssAniItem.m_gameObject.GetComponent<RectTransform>().anchoredPosition = this.m_startPos;
                }
                else
                {
                    rssAniItem.m_gameObject.transform.position = Vector3.zero;
                    rssAniItem.m_gameObject.transform.eulerAngles = new Vector3(45f, 0f, 0f);
                    rssAniItem.m_baseScale = rssAniItem.m_gameObject.transform.localScale;
                }
                this.m_rssAniItems.Add(rssAniItem);
            }
            ClientUtils.rssAnimationManager.AddAnimaitonGroup(this);
            return this;
        }

        public void ClearData()
        {
            for (int i = 0; i < m_rssAniItems.Count; i++)
            {
                RssAniItem rssAniItem = m_rssAniItems[i];
                if (rssAniItem != null && rssAniItem.m_gameObject != null)
                {
                    CoreUtils.assetService.Destroy(rssAniItem.m_gameObject);
                }
            }
            m_rssAniItems.Clear();
        }

        private Vector3 GenerateDirect(float factor)
        {
            if ( this.LaunchAngle == Vector3.zero)
            {
                Vector3 zero = Vector3.zero;
                zero.x = UnityEngine.Random.Range(0.2f * factor, 0.5f * factor) * (float)((UnityEngine.Random.Range(-1f, 1f) > 0f) ? 1 : (-1));
                zero.y = UnityEngine.Random.Range(0.2f * factor, 0.5f * factor);
                zero.z = UnityEngine.Random.Range(0.2f * factor, 0.5f * factor) * (float)((UnityEngine.Random.Range(-1f, 1f) > 0f) ? 1 : (-1));
                return zero;
            }
            else
            {
                return this.LaunchAngle;
            }
        }

        private void UpdateRssItem(RssAniItem rssItem)
        {
            if (rssItem != null && rssItem.m_step == 2)
            {
                Vector3 b = rssItem.m_spos = ((!m_is2D) ? rssItem.m_gameObject.transform.position : ((Vector3)rssItem.m_gameObject.GetComponent<RectTransform>().anchoredPosition));
                rssItem.m_dir = m_targetPos - b;
                float num = Vector3.Magnitude(rssItem.m_dir);
                float num2 = m_tClock - rssItem.m_timeShift;
                float num3 = rssItem.m_step2LeftClock - num2;
                if (rssItem.m_timeShift < m_step1TotalClock)
                {
                    rssItem.m_v0 = m_step1VT;
                    num3 = rssItem.m_step2LeftClock;
                }
                else
                {
                    rssItem.m_v0 += rssItem.m_a * num2;
                }
                rssItem.m_a = (num - rssItem.m_v0 * num3) / (num3 * num3);
                rssItem.m_timeShift = m_tClock;
                rssItem.m_step2LeftClock = num3;
            }
        }

        public void Update(float dt)
        {
            m_tClock += dt;
            m_passFrame++;
            bool flag = true;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            for (int i = 0; i < m_rssAniItems.Count; i++)
            {
                RssAniItem rssAniItem = m_rssAniItems[i];
                if (rssAniItem.m_step == 1)
                {                    
                    if (!flag4 && rssAniItem.m_timeShift < 0f && rssAniItem.m_startFrame <= m_tClock)
                    {
                        rssAniItem.m_timeShift = m_tClock;
                        flag4 = true;
                        Animation component = rssAniItem.m_gameObject.GetComponent<Animation>();
                        if (component != null)
                        {
                            component.Play();
                        }
                    }
                    float num = -1f;
                    if (rssAniItem.m_timeShift > 0f)
                    {
                        float num2 = m_tClock - rssAniItem.m_timeShift;
                        float d = rssAniItem.m_v0 * num2 + rssAniItem.m_a * num2 * num2;
                        Vector3 a = rssAniItem.m_dir.normalized * d;
                        Vector3 vector = a + rssAniItem.m_spos;
                        num = rssAniItem.m_v0 + rssAniItem.m_a * num2;
                        if (m_is2D)
                        {
                            rssAniItem.m_gameObject.GetComponent<RectTransform>().anchoredPosition = vector;
                        }
                        else
                        {
                            rssAniItem.m_gameObject.transform.position = vector;
                        }
                    }
                    if ((num > 0f && num < m_step1VT) || m_tClock - rssAniItem.m_timeShift > m_step1TotalClock)
                    {
                        rssAniItem.m_step = 2;
                        rssAniItem.m_step2LeftClock = m_step2TotalClock;
                        this.m_stepOneDoneCallback?.Invoke();
                        UpdateRssItem(rssAniItem);
                    }
                    flag = false;
                }
                else if (rssAniItem.m_step == 2)
                {
                    float a2 = m_tClock - rssAniItem.m_timeShift;
                    a2 = Mathf.Min(a2, rssAniItem.m_step2LeftClock);
                    float a3 = 1f - Mathf.Max(0f, a2 / rssAniItem.m_step2LeftClock - 0.9f) * 10f;
                    float d2 = rssAniItem.m_a * a2 * a2 + rssAniItem.m_v0 * a2;
                    Vector3 a4 = rssAniItem.m_dir.normalized * d2;
                    Vector3 vector2 = a4 + rssAniItem.m_spos;
                    if (m_is2D)
                    {
                        RectTransform component2 = rssAniItem.m_gameObject.GetComponent<RectTransform>();
                        component2.anchoredPosition = vector2;
                        if (m_scaleSpeed != Vector3.zero)
                        {
                            component2.localScale += m_scaleSpeed * dt;
                        }
                    }
                    else
                    {
                        rssAniItem.m_gameObject.transform.position = vector2;
                        PolygonImage componentInChildren = rssAniItem.m_gameObject.GetComponentInChildren<PolygonImage>();
                        if (componentInChildren != null)
                        {
                            Color color = componentInChildren.color;
                            color.a = a3;
                            componentInChildren.color = color;
                        }
                    }
                    if (a2 >= rssAniItem.m_step2LeftClock)
                    {
                        rssAniItem.m_step = 3;
                        if (m_destroy)
                        {
                            rssAniItem.m_gameObject.SetActive(value: false);
                        }
                        flag2 = true;
                    }
                    flag = false;
                }
                else
                {
                    flag3 = true;
                }
            }
            if (flag)
            {
                RssAniDone();
            }
            else if (!flag3 && flag2)
            {
                m_startReachCallback?.Invoke();
            }
        }

        private void RssAniDone()
        {
            m_beforeDestroyCallback?.Invoke();
            m_tClock = 0f;
            m_passFrame = 0;
            if (m_destroy)
            {
                foreach (RssAniItem rssAniItem in m_rssAniItems)
                {
                    if (rssAniItem.m_gameObject != null)
                    {
                        CoreUtils.assetService.Destroy(rssAniItem.m_gameObject);
                    }
                }
            }
            m_rssAniItems.Clear();
            m_resAniDoneCallback?.Invoke();
            ClientUtils.rssAnimationManager.RemoveAnimationGroup(this);
        }
    }
}


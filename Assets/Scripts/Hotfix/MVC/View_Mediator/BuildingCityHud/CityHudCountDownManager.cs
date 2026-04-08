// =============================================================================== 
// Author              :    xzl
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    CityHudCountDownManager 城市建筑倒计时刷新显示管理
// Copyright IGG All rights reserved.
// ===============================================================================
using System;
using System.Collections.Generic;
using Client;
using Skyunion;
using SprotoType;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CityHudTimerItem
    {
        public LanguageText TextDesc;
        public Animator DescAnimator;
        public LanguageText TextTime;
        public Animator TimeAnimator;
        public GameSlider Slider;
        public GameObject SliderObject;
        public QueueInfo QueueData;
        public bool IsChangeShow;
        public Action SecondCallback; //每秒回调
        public Action<Int64> EndCallback;
        public int Status;
        public int HashCode;
        public int TimeLanguageId;
    }

    public class CityHudCountDownManager : Hotfix.TSingleton<CityHudCountDownManager>
    {
        private Dictionary<Int64, CityHudTimerItem> m_keyMap = new Dictionary<Int64, CityHudTimerItem>();
        private List<CityHudTimerItem> m_itemList = new List<CityHudTimerItem>();

        //界面里面定时器
        private Dictionary<Int64, CityHudTimerItem> m_keyUiMap = new Dictionary<Int64, CityHudTimerItem>();
        private List<CityHudTimerItem> m_itemUiList = new List<CityHudTimerItem>();

        private Timer m_timer;
        private bool m_isShowTime;

        public void AddQueue(LanguageText desc, LanguageText time, GameSlider sd, QueueInfo queue, Action<Int64> callback)
        {
            int hashCode = sd.GetHashCode();
            if (!m_keyMap.ContainsKey(hashCode))
            {
                CityHudTimerItem item = new CityHudTimerItem();
                m_keyMap[hashCode] = item;
                m_itemList.Add(item);
            }
            m_keyMap[hashCode].TextDesc = desc;
            m_keyMap[hashCode].TextTime = time;
            m_keyMap[hashCode].Slider = sd;
            m_keyMap[hashCode].SliderObject = sd.gameObject;
            m_keyMap[hashCode].QueueData = queue;
            m_keyMap[hashCode].EndCallback = callback;
            m_keyMap[hashCode].HashCode = hashCode;
            if (desc != null)
            {
                m_keyMap[hashCode].DescAnimator = desc.GetComponent<Animator>();
            }
            if (time != null)
            {
                m_keyMap[hashCode].TimeAnimator = time.GetComponent<Animator>();
            }
            if (queue.armyType > 0) //训练
            {
                m_keyMap[hashCode].IsChangeShow = true;
                m_keyMap[hashCode].Status = (int)EnumQueueType.Training;
            }
            else if (queue.technologyType > 0) //研究
            {
                m_keyMap[hashCode].IsChangeShow = true;
                m_keyMap[hashCode].Status = (int)EnumQueueType.Studying;
            }
            else if (queue.treatmentSoldiers != null && queue.treatmentSoldiers.Count > 0) //治疗
            {
                m_keyMap[hashCode].IsChangeShow = true;
                m_keyMap[hashCode].Status = (int)EnumQueueType.Treatmenting;
            }
            else //建筑升级
            {
                m_keyMap[hashCode].IsChangeShow = false;
                m_keyMap[hashCode].Status = (int)EnumQueueType.Upgradeing;
            }
            SyncRefresh(m_keyMap[hashCode]);
            StartCountDown();
        }

        //public void RemoveQueue(Int64 buildingIndex)
        //{
        //    for (int i = 0; i < m_itemList.Count; i++)
        //    {
        //        if (m_itemList[i].QueueData!=null && m_itemList[i].QueueData.buildingIndex == buildingIndex)
        //        {
        //            m_keyMap.Remove(m_itemList[i].HashCode);
        //            m_itemList.RemoveAt(i);
        //        }
        //    }
        //}

        //public void RemoveQueue(int status)
        //{
        //    for (int i = 0; i < m_itemList.Count; i++)
        //    {
        //        if (m_itemList[i].Status == status)
        //        {
        //            if (m_itemList[i].QueueData != null)
        //            {
        //                m_keyMap.Remove(m_itemList[i].HashCode);
        //            }
        //            m_itemList.RemoveAt(i);
        //        }
        //    }
        //}

        public void RemoveUiQueue(int hashCode)
        {
            if (m_keyUiMap.ContainsKey(hashCode))
            {
                m_keyUiMap.Remove(hashCode);
                for (int i = 0; i < m_itemUiList.Count; i++)
                {
                    if (m_itemUiList[i].HashCode == hashCode)
                    {
                        m_itemUiList.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        public void AddUiQueue(LanguageText desc, LanguageText time, GameSlider sd, QueueInfo queue, Action secondCallback, Action<Int64> callback, int timeLanguageid = 0)
        {
            int hashCode = sd.GetHashCode();
            if (!m_keyUiMap.ContainsKey(hashCode))
            {
                CityHudTimerItem item = new CityHudTimerItem();
                m_keyUiMap[hashCode] = item;
                m_itemUiList.Add(item);
            }
            m_keyUiMap[hashCode].TextDesc = desc;
            m_keyUiMap[hashCode].TextTime = time;
            m_keyUiMap[hashCode].Slider = sd;
            m_keyUiMap[hashCode].SliderObject = sd.gameObject;
            m_keyUiMap[hashCode].QueueData = queue;
            m_keyUiMap[hashCode].SecondCallback = secondCallback;
            m_keyUiMap[hashCode].EndCallback = callback;
            m_keyUiMap[hashCode].HashCode = hashCode;
            m_keyUiMap[hashCode].TimeLanguageId = timeLanguageid;
            if (queue.armyType > 0) //训练
            {
                m_keyUiMap[hashCode].Status = (int)EnumQueueType.Training;
            }
            else if (queue.technologyType > 0) //研究
            {
                m_keyUiMap[hashCode].Status = (int)EnumQueueType.Studying;
            }
            else if (queue.treatmentSoldiers != null && queue.treatmentSoldiers.Count > 0) //治疗
            {
                m_keyUiMap[hashCode].Status = (int)EnumQueueType.Treatmenting;
            }
            else //建筑升级
            {
                m_keyUiMap[hashCode].Status = (int)EnumQueueType.Upgradeing;
            }
            SyncRefresh(m_keyUiMap[hashCode]);
            StartCountDown();
        }

        private void UpdateCountDown()
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();

            //界面里面进度更新
            for (int i = 0; i < m_itemUiList.Count; i++)
            {
                if (m_itemUiList[i].SliderObject == null)
                {
                    m_keyUiMap.Remove(m_itemUiList[i].HashCode);
                    m_itemUiList.RemoveAt(i);
                }
                else
                {
                    Int64 totalTime = m_itemUiList[i].QueueData.firstFinishTime - m_itemUiList[i].QueueData.beginTime;
                    totalTime = (totalTime == 0) ? 1 : totalTime;
                    Int64 speedTime = m_itemUiList[i].QueueData.firstFinishTime - m_itemUiList[i].QueueData.finishTime;
                    Int64 costTime = serverTime - m_itemUiList[i].QueueData.beginTime+ speedTime;
                    costTime = (costTime < 0) ? 0 : costTime;
                    Int64 residueTime = totalTime - costTime;
                    residueTime = (residueTime < 0) ? 0 : residueTime;
                    float pro = (float)costTime / totalTime;
                    if (m_itemUiList[i].TimeLanguageId > 0)
                    {
                        m_itemUiList[i].TextTime.text = LanguageUtils.getTextFormat(m_itemUiList[i].TimeLanguageId, ClientUtils.FormatCountDown((int)residueTime));
                    }
                    else
                    {
                        m_itemUiList[i].TextTime.text = ClientUtils.FormatCountDown((int)residueTime);
                    }
                    m_itemUiList[i].Slider.value = pro;

                    if (m_itemUiList[i].SecondCallback != null)
                    {
                        m_itemUiList[i].SecondCallback();
                    }

                    if (m_itemUiList[i].QueueData.finishTime <= serverTime)
                    {
                        if (m_itemUiList[i].EndCallback != null)
                        {
                            m_itemUiList[i].EndCallback(m_itemUiList[i].QueueData.buildingIndex);
                        }
                        m_keyUiMap.Remove(m_itemUiList[i].HashCode);
                        m_itemUiList.RemoveAt(i);
                    }
                }
            }

            //城市建筑hud进度更新
            bool isChangeShow = ((serverTime % 5) == 0) ? true : false;
            if (isChangeShow)
            {
                m_isShowTime = !m_isShowTime;
            }
            //m_isShowTime = true;
            for (int i = 0; i < m_itemList.Count;i++)
            {
                if (m_itemList[i].SliderObject == null)
                {
                    m_keyMap.Remove(m_itemList[i].HashCode);
                    m_itemList.RemoveAt(i);
                }else
                {
                    Int64 totalTime = m_itemList[i].QueueData.firstFinishTime - m_itemList[i].QueueData.beginTime;
                    totalTime = (totalTime == 0) ? 1 : totalTime;
                    Int64 speedTime = m_itemList[i].QueueData.firstFinishTime - m_itemList[i].QueueData.finishTime;
                    Int64 costTime = serverTime - m_itemList[i].QueueData.beginTime+ speedTime;
                    costTime = (costTime < 0) ? 0 : costTime;
                    Int64 residueTime = totalTime - costTime;
                    float pro = (float)costTime / totalTime;
                    m_itemList[i].TextTime.text = ClientUtils.FormatCountDown((int)residueTime);
                    m_itemList[i].Slider.value = pro;

                    if (m_itemList[i].QueueData.finishTime <= serverTime)
                    {
                        if (m_itemList[i].EndCallback != null)
                        {
                            m_itemList[i].EndCallback(m_itemList[i].QueueData.buildingIndex);
                        }
                        m_keyMap.Remove(m_itemList[i].HashCode);
                        m_itemList.RemoveAt(i);
                    }
                    else
                    {
                        if (m_itemList[i].IsChangeShow)
                        {
                            if (isChangeShow)
                            {
                                if (m_itemList[i].TextDesc != null)
                                {
                                    m_itemList[i].TextDesc.gameObject.SetActive(!m_isShowTime);
                                }
                                m_itemList[i].TextTime.gameObject.SetActive(m_isShowTime);
                                if (m_isShowTime)
                                {
                                    if (m_itemList[i].TimeAnimator != null)
                                    {
                                        m_itemList[i].TimeAnimator.Play("Show");
                                    }
                                }
                                else
                                {
                                    if (m_itemList[i].DescAnimator != null)
                                    {
                                        m_itemList[i].DescAnimator.Play("Show");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (m_itemList.Count == 0 && m_itemUiList.Count == 0)
            {
                PauseCountDown();
            }
        }

        public void StartCountDown()
        {
            if (m_timer == null)
            {
                m_timer = Timer.Register(1.0f, UpdateCountDown, null, true, true);
            }
            else
            {
                if (m_timer.isPaused)
                {
                    m_timer.Resume();
                }
            }
        }

        //首次添加 先同步刷新一下 保持显示一致
        private void SyncRefresh(CityHudTimerItem item)
        {
            Int64 serverTime = ServerTimeModule.Instance.GetServerTime();
            Int64 totalTime = item.QueueData.firstFinishTime - item.QueueData.beginTime;
            totalTime = (totalTime == 0) ? 1 : totalTime;
            Int64 speedTime = item.QueueData.firstFinishTime - item.QueueData.finishTime;
            Int64 costTime = serverTime - item.QueueData.beginTime + speedTime;
            Int64 residueTime = totalTime - costTime;
            residueTime = (residueTime < 0) ? 0 : residueTime;
            float pro = (float)costTime / totalTime;

            if (item.TimeLanguageId > 0)
            {
                item.TextTime.text = LanguageUtils.getTextFormat(item.TimeLanguageId, ClientUtils.FormatCountDown((int)residueTime));
            }
            else
            {
                item.TextTime.text = ClientUtils.FormatCountDown((int)residueTime);
            }
            item.Slider.value = pro;

            if (item.TextDesc != null)
            {
                item.TextDesc.gameObject.SetActive(!m_isShowTime);
            }
            if (item.IsChangeShow)
            {
                item.TextTime.gameObject.SetActive(m_isShowTime);
            }
        }

        public bool IsShowTime()
        {
            return m_isShowTime;
        }

        private void PauseCountDown()
        {
            if (m_timer != null)
            {
                m_timer.Pause();
            }
        }

        private void CancelCountDown()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
        }
    }
}


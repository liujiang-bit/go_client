using Skyunion;
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Client;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using ArabicSupport;

namespace Game
{
    public class PopupParams
    {
        public int radius = 80;
        public int left_offset = 5;
        public int right_offset = 5;
        public int top_offset = 50;
        public int bottom_offset = 50;
        public bool adjust_pivot = false;
        public float full_height = 80;
    }
    public class UIHelper
    {
        /// <summary>
        /// 参数direc 1左2右 3上4下
        /// </summary>
        public static void CalcPopupPos2(RectTransform viewRect, RectTransform relativeRect,
            RectTransform targetRect,Vector3 worldPos,GameObject arrowLeft,GameObject arrowRight,
            GameObject arrowTop,GameObject arrowBottom,
            float radius, int direc = 2, bool isUseUiCamera = false)
        {

            arrowLeft.SetActive(false);
            arrowRight.SetActive(false);
            arrowTop.SetActive(false);
            arrowBottom.SetActive(false);

            //屏幕坐标转界面局部坐标
            Vector2 localPos;
            Vector3 screenPos;
            if (isUseUiCamera)//ui节点世界坐标转屏幕坐标
            {
                screenPos = RectTransformUtility.WorldToScreenPoint(CoreUtils.uiManager.GetUICamera(), worldPos);
            }
            else//场景3d世界坐标转屏幕坐标
            {
                screenPos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(), worldPos);
            }
            RectTransformUtility.ScreenPointToLocalPointInRectangle(relativeRect,
                                                                    screenPos,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            float screenHeight = viewRect.rect.height;
            float screenWidth = viewRect.rect.width;

            var rect = targetRect.rect;

            float diffNum = radius;

            if (direc>2)
            {
                if (direc == 3) //显示在上方
                {
                    if ((localPos.y + rect.height / 2 + diffNum) > screenHeight)
                    {
                        direc = 4;
                    }
                }
                else //显示在下方
                {
                    if (localPos.y < (rect.height / 2 + diffNum))
                    {
                        direc = 3;
                    }
                }

                //tip显示在上方
                if (direc == 3)
                {
                    arrowBottom.gameObject.SetActive(true);
                    localPos.y = localPos.y + rect.height / 2 + diffNum;
                    if (localPos.x > (screenWidth - rect.width / 2))
                    {
                        //坐标太靠近右边界了 需要向左偏移
                        float offset = localPos.x - (screenWidth - rect.width / 2);
                        arrowBottom.transform.localPosition = new Vector2(offset, arrowBottom.transform.localPosition.y);
                        localPos.x = (screenWidth - rect.width / 2);
                    }
                    else if (localPos.x < (rect.width / 2))
                    {
                        //坐标太靠近左边界了 需要向右偏移
                        float offset = localPos.x - rect.width / 2;
                        arrowBottom.transform.localPosition = new Vector2(offset, arrowBottom.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }
                }

                //tip显示在下方 
                if (direc == 4)
                {
                    arrowTop.gameObject.SetActive(true);
                    localPos.y = localPos.y - rect.height / 2 - diffNum;
                    if (localPos.x > (screenWidth - rect.width / 2))
                    {
                        //坐标太靠近右边界了 需要向左偏移
                        float offset = localPos.x - (screenWidth - rect.width / 2);
                        arrowTop.transform.localPosition = new Vector2(offset, arrowTop.transform.localPosition.y);
                        localPos.x = (screenWidth - rect.width / 2);
                    }
                    else if (localPos.x < (rect.width / 2))
                    {
                        //坐标太靠近左边界了 需要向右偏移
                        float offset = localPos.x - rect.width / 2;
                        arrowTop.transform.localPosition = new Vector2(offset, arrowTop.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }
                }
            }
            else
            {                
                if (localPos.x < screenWidth / 2)
                {
                    //tip显示在右方
                    arrowLeft.gameObject.SetActive(true);
                    localPos.x = localPos.x + rect.width / 2 + diffNum;
                    if (localPos.y > (screenHeight - rect.height / 2))
                    {
                        //坐标太靠近上边界 需要向下偏移
                        float offset = localPos.y - (screenHeight - rect.height / 2);
                        arrowLeft.transform.localPosition = new Vector2(arrowLeft.transform.localPosition.x, offset);
                        localPos.y = (screenHeight - rect.height / 2);
                    }
                    else if (localPos.y < (rect.height / 2))
                    {
                        //坐标太靠近下边界 需要向下偏移
                        float offset = localPos.y - rect.height / 2;
                        arrowLeft.transform.localPosition = new Vector2(arrowLeft.transform.localPosition.x, offset);
                        localPos.y = rect.height / 2;
                    }
                }
                else if (localPos.x >= screenWidth / 2)                
                {
                    //tip显示在左方
                    arrowRight.gameObject.SetActive(true);
                    localPos.x = localPos.x - rect.width / 2 - diffNum;
                    if (localPos.y > (screenHeight - rect.height / 2))
                    {
                        //坐标太靠近上边界 需要向下偏移
                        float offset = localPos.y - (screenHeight - rect.height / 2);
                        arrowRight.transform.localPosition = new Vector2(arrowRight.transform.localPosition.x, offset);
                        localPos.y = (screenHeight - rect.height / 2);
                    }
                    else if (localPos.y < (rect.height / 2))
                    {
                        //坐标太靠近下边界 需要向下偏移
                        float offset = localPos.y - rect.height / 2;
                        arrowRight.transform.localPosition = new Vector2(arrowRight.transform.localPosition.x, offset);
                        localPos.y = rect.height / 2;
                    }

                }
            }
            targetRect.transform.localPosition = localPos;
        }


        /// <summary>
        /// 弹窗界面坐标自适应
        /// </summary>
        /// <param name="rssObj"></param>
        /// <param name="view"></param>
        /// <param name="content"></param>
        /// <param name="arrowLeft"></param>
        /// <param name="arrowRight"></param>
        /// <param name="arrowTop"></param>
        /// <param name="arrowBottom"></param>
        /// <param name="radius"></param>
        public static void SelfAdaptPopViewPos(GameObject rssObj, RectTransform viewRect, RectTransform viewPos, RectTransform content, GameObject arrowLeft, GameObject arrowRight, GameObject arrowTop, GameObject arrowBottom)
        {
            if (rssObj == null)
            {
                return;
            }

            //屏幕坐标转界面局部坐标
            Vector2 localPos;
            Vector3 pos = RectTransformUtility.WorldToScreenPoint(WorldCamera.Instance().GetCamera(),
                rssObj.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewPos,
                pos,
                CoreUtils.uiManager.GetUICamera(),
                out localPos);

            var rect = content.rect;

            float diffNum = 50f;

            // 左
            if (localPos.x < viewRect.rect.width / 2)
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    localPos.y = localPos.y - (rect.height / 2) - diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        arrowTop.transform.localPosition = new Vector2(offset,
                            arrowTop.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }

                    arrowTop.gameObject.SetActive(true);
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x < rect.width / 2)
                    {
                        float offset = localPos.x - rect.width / 2;
                        arrowBottom.transform.localPosition = new Vector2(offset,
                            arrowBottom.transform.localPosition.y);
                        localPos.x = rect.width / 2;
                    }

                    arrowBottom.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x + (rect.width / 2) + diffNum;
                    arrowLeft.gameObject.SetActive(true);
                }
            }
            // 右
            else
            {
                // 下方
                if (localPos.y > (viewRect.rect.height - rect.height / 2))
                {
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.y - (viewRect.rect.height - rect.height / 2);
                        arrowRight.transform.localPosition = new Vector2(
                            arrowRight.transform.localPosition.x,
                            offset);
                        arrowRight.gameObject.SetActive(true);

                        localPos.x = localPos.x - (rect.width / 2) - diffNum;
                        localPos.y = viewRect.rect.height - rect.height / 2;
                    }
                    else
                    {
                        localPos.y = localPos.y - (rect.height / 2) - diffNum;
                        arrowTop.gameObject.SetActive(true);
                    }
                }
                // 上方
                else if (localPos.y < (rect.height / 2))
                {
                    localPos.y = localPos.y + (rect.height / 2) + diffNum;
                    if (localPos.x > (viewRect.rect.width - rect.width / 2))
                    {
                        float offset = localPos.x - (viewRect.rect.width - rect.width / 2);
                        arrowBottom.transform.localPosition = new Vector2(offset,
                            arrowBottom.transform.localPosition.y);
                        localPos.x = (viewRect.rect.width - rect.width / 2);
                    }

                    arrowBottom.gameObject.SetActive(true);
                }
                else
                {
                    localPos.x = localPos.x - (rect.width / 2) - diffNum;
                    arrowRight.gameObject.SetActive(true);
                }
            }

            content.transform.localPosition = localPos;
        }


        /// <summary>
        /// 弹窗界面坐标自适应（会根据控件边缘和pivot调整，控件pivot和isHorizen决定了弹窗的方向）
        /// </summary>
        /// <param name="rssObj">点击的物体</param>
        /// <param name="viewRect">content的父节点</param>
        /// <param name="content">需要自适应的控件</param>
        /// <param name="arrowLeft"></param>
        /// <param name="arrowRight"></param>
        /// <param name="arrowTop"></param>
        /// <param name="arrowBottom"></param>
        /// <param name="radius"></param>
        /// /// <param name="width">自定义控件的宽</param>
        /// /// /// <param name="height">自定义控件的高</param>
        /// /// <param name="isHorizen">弹窗方向</param>
        public static void SelfAdaptPopViewPos(GameObject rssObj, RectTransform viewRect, RectTransform content,
            GameObject arrowLeft, GameObject arrowRight, GameObject arrowTop, GameObject arrowBottom, int radius = 80,
            bool isHorizen = true,float width = 0,float height = 0)
        {
            Vector2 pos = Vector2.zero;
            Vector2 screenPoint = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(rssObj.transform.position);
                
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, screenPoint,
                CoreUtils.uiManager.GetUICamera(), out pos);

            //相对于旧位置的偏移
            Vector3 deltOffect = pos - content.anchoredPosition;

            RectTransform canvasRect = CoreUtils.uiManager.GetCanvas().transform as RectTransform;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, screenPos, CoreUtils.uiManager.GetUICamera(), out Vector2 pos);
            Vector3[] corners = new Vector3[4];
            content.GetWorldCorners(corners);

            //控件的左下角和右上角
            Vector3 zeroPoint = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[0]) + deltOffect;
            Vector3 onePoint = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[2]) + deltOffect;
            Vector2 offset = new Vector2();

            arrowLeft.SetActive(false);
            arrowRight.SetActive(false);
            arrowTop.SetActive(false);
            arrowBottom.SetActive(false);

            float rectWidth = width.Equals(0) ? content.rect.width : width;
            float rectHeight = height.Equals(0) ? content.rect.height : height;
            
            //水平弹出
            if (isHorizen)
            {
                //右边溢出
                if (onePoint.x > Screen.width)
                {
                    arrowRight.SetActive(true);
                    offset -= new Vector2(rectWidth + radius, 0);
                }
                //左边溢出
                else if (zeroPoint.x < 0)
                {
                    arrowLeft.SetActive(true);
                    offset += new Vector2(rectWidth + radius, 0);
                }
                else
                {
                    if (content.pivot.x >= 0.5)
                    {
                        arrowRight.SetActive(true);
                    }
                    else
                    {
                        arrowLeft.SetActive(true);
                    }
                }
                
                //对于非直接销毁的弹窗（关闭隐藏）要重置箭头的位置
                arrowLeft.GetComponent<RectTransform>().anchoredPosition=  new Vector2(1.6f,0f);
                arrowRight.GetComponent<RectTransform>().anchoredPosition=  new Vector2(-1.6f,0f);

                Vector2 offsetY = Vector2.zero;
                Vector2 vectHeight = new Vector2(0,rectHeight/2);;
                //纵向溢出直接加减溢出部分
                //下方溢出
                if (zeroPoint.y < 0)
                {
                    offsetY = new Vector2(0, zeroPoint.y * (Screen.height/canvasRect.rect.height));
                    offset -= offsetY;
                    arrowLeft.GetComponent<RectTransform>().anchoredPosition += offsetY + vectHeight;;
                    arrowRight.GetComponent<RectTransform>().anchoredPosition += offsetY + vectHeight;;
                }
                //上方溢出
                else if (onePoint.y > Screen.height)
                {
                    offsetY = new Vector2(0, (Screen.height - onePoint.y) * (Screen.height/canvasRect.rect.height));
                    offset += offsetY;
                    arrowLeft.GetComponent<RectTransform>().anchoredPosition -= offsetY - vectHeight;
                    arrowRight.GetComponent<RectTransform>().anchoredPosition -= offsetY - vectHeight;
                }
                content.anchoredPosition = pos + offset;
            }
            else
            {
                if (onePoint.y > Screen.height)
                {
                    arrowTop.SetActive(true);
                    offset -= new Vector2(0, rectHeight + radius);
                }
                else if (zeroPoint.y < 0)
                {
                    arrowBottom.SetActive(true);
                    offset += new Vector2(0, rectHeight + radius);
                }
                else
                {
                    if (content.pivot.y >= 0.5)
                    {
                        arrowTop.SetActive(true);
                    }
                    else
                    {
                        arrowBottom.SetActive(true);
                    }
                }
                
                arrowTop.GetComponent<RectTransform>().anchoredPosition=  new Vector2(0f,-1.6f);
                arrowBottom.GetComponent<RectTransform>().anchoredPosition=  new Vector2(0,1.6f);
                
                Vector2 offsetX = Vector2.zero;
                Vector2 vectWidth = new Vector2(rectWidth/2,0);
                if (zeroPoint.x < 0)
                {
                    offsetX = new Vector2(zeroPoint.x * (Screen.width/canvasRect.rect.width), 0);
                    offset -= offsetX;
                    arrowTop.GetComponent<RectTransform>().anchoredPosition += offsetX + vectWidth;
                    arrowBottom.GetComponent<RectTransform>().anchoredPosition += offsetX + vectWidth;
                }
                else if (onePoint.x > Screen.width)
                {
                    offsetX = new Vector2((Screen.width - onePoint.x) * (Screen.width/canvasRect.rect.width), 0);
                    offset += offsetX;
                    arrowTop.GetComponent<RectTransform>().anchoredPosition -= offsetX - vectWidth;;
                    arrowBottom.GetComponent<RectTransform>().anchoredPosition -= offsetX - vectWidth;;
                }
                content.anchoredPosition = pos + offset;
            }
        }
        
        public static void CalcPopupPos(Vector2 pos, RectTransform popup, GameObject arrowLeft, GameObject arrowRight, GameObject arrowTop, GameObject arrowBottom, int radius = 80)
        {
            RectTransform canvasRect = CoreUtils.uiManager.GetCanvas().transform as RectTransform;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, screenPos, CoreUtils.uiManager.GetUICamera(), out Vector2 pos);
            Vector3[] corners = new Vector3[4];
            popup.GetWorldCorners(corners);

            Vector3 zeroPoint = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[0]);
            Vector3 onePoint = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[2]);
            Vector2 offset = new Vector2();

            arrowLeft.SetActive(false);
            arrowRight.SetActive(false);
            arrowTop.SetActive(false);
            arrowBottom.SetActive(false);


            float screenWidth = canvasRect.rect.width;
            float screenHeight = canvasRect.rect.height;
            bool horizen = (Mathf.Abs((pos.y / screenHeight)) / Mathf.Abs((pos.x / screenWidth))) < 1;
            pos -= new Vector2(canvasRect.rect.width / 2, canvasRect.rect.height / 2);
            if (horizen)
            {
                if (pos.x > 0)
                {
                    arrowRight.SetActive(true);
                    offset -= new Vector2(popup.rect.width / 2 + radius, 0);
                }
                else
                {
                    arrowLeft.SetActive(true);
                    offset += new Vector2(popup.rect.width / 2 + radius, 0);
                }
                if (zeroPoint.y < 0)
                {
                    offset -= new Vector2(0, zeroPoint.y * (canvasRect.rect.height / Screen.height));
                }
                else if (onePoint.y > Screen.height)
                {
                    offset += new Vector2(0, (Screen.height - onePoint.y) * (canvasRect.rect.height / Screen.height));
                }
                popup.anchoredPosition = pos + offset;
            }
            else
            {
                if (pos.y > 0)
                {
                    arrowTop.SetActive(true);
                    offset -= new Vector2(0, popup.rect.height / 2 + radius);
                }
                else
                {
                    arrowBottom.SetActive(true);
                    offset += new Vector2(0, popup.rect.height / 2 + radius);
                }
                if (zeroPoint.x < 0)
                {
                    offset -= new Vector2(zeroPoint.x * (canvasRect.rect.width / Screen.width), 0);
                }
                else if (onePoint.x > Screen.width)
                {
                    offset += new Vector2((Screen.width - onePoint.x) * (canvasRect.rect.width / Screen.width), 0);
                }
                popup.anchoredPosition = pos + offset;
            }
        }

        #region 时间文本相关

        public static string GetHMSCounterDown(long endTime)
        {
            var remainTime = endTime - ServerTimeModule.Instance.GetServerTime();
            if (remainTime < 0)
            {
                Debug.LogWarning("倒计时小于0");
                return null;
            }

            var hour = remainTime / 3600;
            var min = remainTime % 3600 / 60;
            var sec = remainTime % 60;

            return LanguageUtils.getTextFormat(300031, hour.ToString("00"), min.ToString("00"), sec.ToString("00"));
        }

        public static string GetDHMSCounterDown(long endTime)
        {
            var remainTime = endTime - ServerTimeModule.Instance.GetServerTime();
            if (remainTime < 0)
            {
                Debug.LogWarning("倒计时小于0");
                return null;
            }

            var day = remainTime / 3600 / 24;
            remainTime = remainTime - 3600 * 24 * day;
            var hour = remainTime / 3600;
            remainTime = remainTime - 3600 * hour;
            var min = remainTime / 60;
            remainTime = remainTime - 60 * min;
            var sec = remainTime;

            return LanguageUtils.getTextFormat(300090, day.ToString("00"), hour.ToString("00"), min.ToString("00"), sec.ToString("00"));
        }

        public static string GetFixedDHMSCounterDown(long endTime)
        {
            var remainTime = endTime - ServerTimeModule.Instance.GetServerTime();
            if (remainTime < 0)
            {
                Debug.LogWarning("倒计时小于0");
                return "";
            }

            var day = remainTime / 3600 / 24;
            if (day >= 1)
                return GetDHMSCounterDown(endTime);
            else
                return GetHMSCounterDown(endTime);
        }

        public static string GetServerLongTimeFormat(long timeStamp)
        {
            DateTime dt = ServerTimeModule.Instance.ConvertToDateTime(timeStamp);

            return LanguageUtils.getTextFormat(300002, dt.Year, GetTimeFormat(dt.Month), GetTimeFormat(dt.Day), GetTimeFormat(dt.Hour), GetTimeFormat(dt.Minute));
        }

        public static string GetServerShortTimeFormat(long timeStamp)
        {
            DateTime dt = ServerTimeModule.Instance.ConvertToDateTime(timeStamp);

            return LanguageUtils.getTextFormat(300031, GetTimeFormat(dt.Hour), GetTimeFormat(dt.Minute), GetTimeFormat(dt.Second));
        }

        //时分秒
        public static string GetShortTimeFormat(long timeStamp)
        {
            DateTime dt = ServerTimeModule.Instance.ConvertToDateTime(timeStamp);

            return LanguageUtils.getTextFormat(300031, GetTimeFormat(dt.Hour), GetTimeFormat(dt.Minute), GetTimeFormat(dt.Second));
        }

        public static string GetTimeFormat(int num)
        {
            return num < 10 ? string.Concat("0", num) : num.ToString();
        }

        public static string FormatEmailSendTime(long sendTime)
        {
            long timeStamp = ServerTimeModule.Instance.GetServerTime() - sendTime;
            if (timeStamp < 3600)
            {
                long num = timeStamp / 60;
                return LanguageUtils.getTextFormat(570085, (num == 0) ? 1 : num);
            }
            if (timeStamp < 86400)
            {
                return LanguageUtils.getTextFormat(570086, timeStamp / 3600);
            }
            if (timeStamp < 86400*3)
            {
                return LanguageUtils.getTextFormat(570087, timeStamp / 86400);
            }
            DateTime currDate = ServerTimeModule.Instance.GetCurrServerDateTime();
            DateTime sendDate = ServerTimeModule.Instance.ConverToServerDateTime(sendTime);
            if (currDate.Year != sendDate.Year)
            {
                return sendDate.ToString("yyyy/MM/dd");
            } else
            {
                return sendDate.ToString("MM/dd");
            }
        }

        public static string GetDateTimeFormat(long times)
        {
            return LanguageUtils.getTextFormat(100717, GetServerLongTimeFormat(times));
        }

        public static string GetAstTimeFormat(long times)
        {
            DateTime dt = ServerTimeModule.Instance.ConverToServerDateTime(times);
            string str = LanguageUtils.getTextFormat(300002, dt.Year, GetTimeFormat(dt.Month), GetTimeFormat(dt.Day), GetTimeFormat(dt.Hour), GetTimeFormat(dt.Minute));
            return LanguageUtils.getTextFormat(100717, str);
        }

        #endregion

        #region 道具奖励组

        /// <summary>
        /// 刷新道具奖励组
        /// </summary>
        /// <param name="itemPackage"></param>
        /// <param name="m_itemViewList"></param>
        /// <param name="getItem"></param>
        /// <param name="sourceType"></param>
        public static void RefreshItemPackage(int itemPackage, List<UI_Model_Item_SubView> m_itemViewList, UI_Model_Item_SubView originItemSubView, int sourceType = 3)
        {
            RewardGroupProxy rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            
            List<RewardGroupData> groupDataList = rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int count = groupDataList.Count;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    if (i >= m_itemViewList.Count)
                    {
                        GameObject obj = CoreUtils.assetService.Instantiate(originItemSubView.gameObject);
                        obj.transform.SetParent(originItemSubView.gameObject.transform.parent);
                        obj.transform.localPosition = Vector3.one;
                        obj.transform.localScale = Vector3.one;
                        m_itemViewList.Add(new UI_Model_Item_SubView(obj.GetComponent<RectTransform>()));
                    }
                
                    UI_Model_Item_SubView item = m_itemViewList[i];
                    if (i < count)
                    {
                        item.gameObject.SetActive(true);
                        item.RefreshByGroup(groupDataList[i], sourceType);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                }
                catch (Exception e)
                {
                    CoreUtils.logService.Warn($"刷新 奖励组错误  {e}");   
                }
            }
        }


        #endregion

        #region 飘飞特效

        //宝箱飘飞特效
        public static void FlyBoxRewardEffect(Transform reward, SprotoType.RewardInfo rewardInfo)
        {
            RewardGroupProxy proxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> groupDatas = proxy.GetRewardDataByRewardInfo(rewardInfo);
            RectTransform rect = reward.GetComponent<RectTransform>();
            if (groupDatas == null || groupDatas.Count <= 0)
            {
                CoreUtils.logService.Warn($"[FlyRewardEffect] rewardGroupDatas 等于空！");
                return;
            }
            StringBuilder rewardItemName = new StringBuilder();
            foreach (var data in groupDatas)
            {
                if (rewardItemName.Length > 0)
                {
                    rewardItemName.Append(" ");
                }
                rewardItemName.Append(LanguageUtils.getTextFormat(200030, LanguageUtils.getText(data.name),
                    ClientUtils.FormatComma(data.number)));
            }
            Tip.CreateTip(LanguageUtils.getTextFormat(300237, rewardItemName.ToString())).Show();
            foreach (var groupData in groupDatas)
            {
                FlyRewardEffect(rect,groupData);
            }
        }
        
        public static void FlyRewardEffect(Transform rewards, SprotoType.RewardInfo rewardInfo)
        {
            RewardGroupProxy proxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> groupDatas = proxy.GetRewardDataByRewardInfo(rewardInfo);
            FlyRewardEffect(rewards.GetComponents<RectTransform>(), groupDatas);
        }

        public static void FlyRewardEffect(RectTransform[] trs, SprotoType.RewardInfo rewardInfo)
        {
            RewardGroupProxy proxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> groupDatas = proxy.GetRewardDataByRewardInfo(rewardInfo);
            FlyRewardEffect(trs, groupDatas);
        }

        public static void FlyRewardEffect(Transform rewards, List<RewardGroupData> rewardGroupDataList)
        {
            FlyRewardEffect(rewards.GetComponents<RectTransform>(), rewardGroupDataList);
        }
        
        public static void FlyRewardEffect(RectTransform[] rewards, List<RewardGroupData> rewardGroupDataList)
        {
            if (rewards != null && rewards.Length >= rewardGroupDataList.Count)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.TipRewardGroup, rewardGroupDataList);
                for (int i = 0; i < rewardGroupDataList.Count; i++)
                {
                    FlyRewardEffect(rewards[i],rewardGroupDataList[i]);
                }
            }
            else
            {
                if (rewards == null)
                {
                    CoreUtils.logService.Warn($"[FlyRewardEffect] rewards 等于空！");
                }
                
                CoreUtils.logService.Warn($"[FlyRewardEffect]  rewards长度:[{rewards.Length}]与rewardGroupDataList长度:[{rewardGroupDataList.Count}]不符！  请检查！   ");
            }
        }

        private static void FlyRewardEffect(RectTransform reward, RewardGroupData rewardGroupData)
        {
            if (reward != null && rewardGroupData != null)
            {
                switch ((EnumRewardType)rewardGroupData.RewardType)
                {
                    case EnumRewardType.Currency:
                    {
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyUICurrency(rewardGroupData.CurrencyData.ID, (int)rewardGroupData.number, reward.transform.position);
                    }
                        break;
                    case EnumRewardType.Soldier:
                    {
                        //飘飞特效
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyPowerUpEffect(reward.gameObject, reward, Vector3.one);
                    }
                        break;
                    case EnumRewardType.Item:
                    {
                        GlobalEffectMediator mt = AppFacade.GetInstance().RetrieveMediator(GlobalEffectMediator.NameMediator) as GlobalEffectMediator;
                        mt.FlyItemEffect(rewardGroupData.ItemData.ID, (int)rewardGroupData.number, reward);
                    }
                        break;
                }
            }
            else
            {
                if (reward == null)
                {
                    CoreUtils.logService.Warn($"[FlyRewardEffect]  reward为空！  请检查！   ");
                }

                if (rewardGroupData == null)
                {
                    CoreUtils.logService.Warn($"[FlyRewardEffect]  rewardGroupData为空！  请检查！   ");
                }
            }
        }

        #endregion
        
        //获取动画长度
        public static float GetLengthByName(Animator animator, string name)
        {
            float length = 0;
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach(AnimationClip clip in clips)
            {
                if(clip.name.Equals(name))
                {
                    length = clip.length;
                    break;
                }
            }
            return length;
        }
        
        //宝石消费提醒
        public static Alert DenarCostRemain(long price,Action confirmCallback)
        {
            Alert alert = null;
       GeneralSettingProxy generalSettingProxy = AppFacade.GetInstance().RetrieveProxy(GeneralSettingProxy.ProxyNAME) as GeneralSettingProxy;
            bool isRemind = generalSettingProxy.GetGeneralSettingByID((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
            if (isRemind)
            {
                CurrencyProxy currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
                string currencyiconId = currencyProxy.GeticonIdByType(104);
                Data.SettingPersonalityDefine settingPersonalityDefine = generalSettingProxy.GetSettingPersonalityDefine((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                if (settingPersonalityDefine != null)
                {
                    string s_remind = settingPersonalityDefine.resetTiem == 0 ? LanguageUtils.getText(300071) : LanguageUtils.getText(300294);
                    string str = LanguageUtils.getTextFormat(300072, price);
                    alert =  Alert.CreateAlert(str, LanguageUtils.getText(300075)).
                        SetLeftButton().
                        SetRightButton().
                        SetCurrencyRemind((isBool) =>
                        {
                            if (isBool)
                            {
                                generalSettingProxy.CloseGeneralSettingItem((int)EnumSetttingPersonType.DiamondUsageConfirmationa);
                            }
                            confirmCallback.Invoke();
                        },
                            (int)price, s_remind, currencyiconId).
                        Show();
                }
            }
            else
            {
                confirmCallback.Invoke();
            }
            return alert;
        }
        
        //举报
        public static void Reporting(long iggID,string nickName,string reportingContent,string noticeStr,UnityAction okCallback = null,UnityAction cancelCallback = null)
        {
            Alert.CreateAlert(noticeStr, LanguageUtils.getText(730341)).
                SetLeftButton(cancelCallback).
                SetRightButton(() =>
                {
                    IGGInGameReporting.shareInstance().ReportComplain(iggID.ToString(),nickName,reportingContent,
                        (bool bSussessed, IGGInGameReporting.WebRequestReturn requestReturn) =>
                        {
                            if (bSussessed &&requestReturn.error.code == 0)
                            {
                                Tip.CreateTip(LanguageUtils.getText(750037)).Show();
                            }
                        });
                    okCallback?.Invoke();
                }).
                Show();
        }
        private static readonly Regex s_HrefRegex = new Regex("<href=([^>\\n\\s]+)>(.*?)(</href>)");
        private static readonly Regex s_ColorContentRegex = new Regex("<color=(.*?)>(.*?)(</color>)");
        //多余的文字用省略号代替
        public static int SetHrefTextWithEllipsis(Text text,string message,string ellipsis)
        {
            int totalLength = 0;
            //string characters = message;
            //{
            //    var match = s_ColorContentRegex.Match(message);
            //    if (match.Success)
            //    {
            //        characters = match.Groups[2].Value;
            //    }
            //}

            //{
            //    var match2 = s_HrefRegex.Match(message);
            //    if (match2.Success)
            //    {
            //        string temp = message.Substring(match2.Length);
            //        characters += temp;
            //    }
            //}
            //int ignoreCharLength = 38;

            //Font myFont = text.font; 
            //RectTransform rectTransform = text.GetComponent<RectTransform>();
            //var width = rectTransform.rect.width;
            //myFont.RequestCharactersInTexture(characters, text.fontSize, text.fontStyle);
            //CharacterInfo characterInfo = new CharacterInfo();

            //char[] arr = characters.ToCharArray();
            //int characterCountVisible = 0;
            //foreach (char c in arr)
            //{
            //    myFont.GetCharacterInfo(c, out characterInfo, text.fontSize);

            //    totalLength += characterInfo.advance;
            //    if (totalLength > width)
            //    {
            //        break;
            //    }
            //    characterCountVisible++;
            //}
            //string updatedText = message;
            //if (characters.Length > characterCountVisible)
            //{
            //    // 阿拉伯语中的省略号在左边
            //    if ((!LanguageUtils.IsArabic()) && !ArabicFixer.IsRtl(message))
            //    {
            //        updatedText = message.Substring(0, characterCountVisible + ignoreCharLength - ellipsis.Length);
            //    }
            //    else if ((!LanguageUtils.IsArabic()) && ArabicFixer.IsRtl(message))
            //    {
            //        updatedText = message.Substring(message.Length - (characterCountVisible + ignoreCharLength - ellipsis.Length), characterCountVisible + ignoreCharLength - ellipsis.Length);
            //    }
            //    else if (LanguageUtils.IsArabic() && ArabicFixer.IsRtl(message))
            //    {
            //        updatedText = message.Substring(0, characterCountVisible + ignoreCharLength - ellipsis.Length);
            //    }
            //    else if (LanguageUtils.IsArabic() && !ArabicFixer.IsRtl(message))
            //    {
            //        updatedText = message.Substring(message.Length - (characterCountVisible + ignoreCharLength - ellipsis.Length), characterCountVisible + ignoreCharLength - ellipsis.Length);
            //    }

            //    updatedText = LanguageUtils.getTextFormat(300250, updatedText);

            //}
            // text.text = updatedText;
            text.text = text.text = message; ;
            return totalLength;
        }

        //多余的文字用省略号代替
        public static int SetTextWithEllipsis(Text text, string message, string ellipsis)
        {
            int totalLength = 0;
            Font myFont = text.font;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            var width = rectTransform.rect.width;
            myFont.RequestCharactersInTexture(message, text.fontSize, text.fontStyle);
            CharacterInfo characterInfo = new CharacterInfo();

            char[] arr = message.ToCharArray();
            int characterCountVisible = 0;
            foreach (char c in arr)
            {
                myFont.GetCharacterInfo(c, out characterInfo, text.fontSize);

                totalLength += characterInfo.advance;
                if (totalLength > width)
                {
                    break;
                }
                characterCountVisible++;
            }
            
            string updatedText = message;
            if (message.Length > characterCountVisible)
            {
                updatedText = message.Substring(0, characterCountVisible - ellipsis.Length);
                //阿拉伯语中的省略号在左边
                //if (LanguageUtils.IsArabic())
                //{
                //    updatedText = string.Format("{0}{1}", ellipsis, updatedText);
                //}
                //else
                //{
                //    updatedText += ellipsis;
                //}
                updatedText = LanguageUtils.getTextFormat(300250, updatedText);
            }
            text.text = updatedText;

            return totalLength;
        }

        public static string NumerBeyondFormat(int num)
        {
            return (num > 99) ? "99+" : num.ToString();
        }

        public static void AlignRightWhenIsArabic(LanguageText text,float width)
        {
            int alignDir = 0;
            switch (text.alignment)
            {
                case TextAnchor.LowerCenter:
                case TextAnchor.MiddleCenter:
                case TextAnchor.UpperCenter:
                    alignDir = 0;
                    break;
                case TextAnchor.MiddleLeft:
                case TextAnchor.LowerLeft:
                case TextAnchor.UpperLeft:
                    alignDir = -1;
                    break;
                default:
                    alignDir = 1;
                    break;
            }

            if (text.isArabicText)
            {
                if (alignDir == -1)
                {
                    text.alignment = text.alignment+2;
                }
            }
            else
            {
                if (alignDir == 1)
                {
                    text.alignment = text.alignment-2;
                }
            }
            text.rectTransform.sizeDelta = new Vector2(width,text.rectTransform.sizeDelta.y);
        }
    }
}


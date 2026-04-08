// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年7月30日
// Update Time         :    2020年7月30日
// Class Description   :    UI_Pop_IconOnEquip_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Pop_IconOnEquip_SubView : UI_SubView
    {
        private Timer m_produceTimer;
        private Timer m_produceInfoAnimationTimer;
        private float m_produceInfoAnimationLength;
        protected override void BindEvent()
        {
            base.BindEvent();

            m_produceInfoAnimationLength = UIHelper.GetLengthByName(m_lbl_desc_Animator, "UA_PbDescChange_Time");
        }

        #region 材料领取

        public void SetCollectMaterialInfo(QueueInfo materialQueue, UnityAction clickCallback=null)
        {
            m_pl_offset.gameObject.SetActive(false);
            m_pl_offset1.gameObject.SetActive(false);
            m_pl_get.gameObject.SetActive(true);

            m_btn_get_GameButton.onClick.RemoveAllListeners();
            m_btn_get_GameButton.onClick.AddListener(() => { clickCallback?.Invoke(); });


            RefreshCompletedItems(materialQueue);
            ClearProduceTimer();

            if (materialQueue!=null && materialQueue.produceItems != null && materialQueue.produceItems.Count > 0)
            {
                m_pl_item.gameObject.SetActive(true);
                long costTime = materialQueue.finishTime - materialQueue.beginTime;
                long finishTime = materialQueue.finishTime;
                long curTime = ServerTimeModule.Instance.GetServerTime();
                var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>((int)materialQueue.produceItems[0].itemId);
                if (itemCfg != null)
                {
                    //防止客户端与服务端时间误差导致时间显示过长
                    if (finishTime - curTime > config.equipMaterialMakeTime)
                    {
                        finishTime = curTime + config.equipMaterialMakeTime;
                    }

                    if (costTime <= 0 || finishTime < curTime)
                    {
                        m_pl_item.gameObject.SetActive(false);
                        return;
                    }

                    m_img_get.Refresh(itemCfg, "");
                    m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(200030, LanguageUtils.getText(itemCfg.l_nameID), materialQueue.produceItems[0].itemNum);
                    UpdateProduceTime(costTime, finishTime);
                    ExchangeShowProduceTimeAndDescription();
                    m_produceInfoAnimationTimer = Timer.Register(m_produceInfoAnimationLength, ExchangeShowProduceTimeAndDescription, null, true);
                    m_produceTimer = Timer.Register(1, () => { UpdateProduceTime(costTime, finishTime); }, null, true);
                }
            }
            else
            {
                m_pl_item.gameObject.SetActive(false);
            }
        }

        private void RefreshCompletedItems(QueueInfo materialQueue)
        {
            if (materialQueue==null || !materialQueue.HasCompleteItems || materialQueue.completeItems == null ||
                materialQueue.completeItems.Count <= 0)
            {
                m_pl_completeItems_PolygonImage.gameObject.SetActive(false);
                m_btn_get_GameButton.gameObject.SetActive(false);
                return;
            }
            m_pl_completeItems_PolygonImage.gameObject.SetActive(true);
            m_btn_get_GameButton.gameObject.SetActive(true);

            List<UI_Model_Item_SubView> items = new List<UI_Model_Item_SubView>() {m_UI_Model_Item};
            var itemCount = materialQueue.completeItems.Count;
            for (int i = 1; i < m_pl_itemList_HorizontalLayoutGroup.transform.childCount; i++)
            {
                if (i < itemCount)
                {
                    var item = new UI_Model_Item_SubView(m_pl_itemList_HorizontalLayoutGroup.transform.GetChild(i)
                        .GetComponent<RectTransform>());
                    items.Add(item);
                    m_pl_itemList_HorizontalLayoutGroup.gameObject.SetActive(true);
                }
                else
                {
                    m_pl_itemList_HorizontalLayoutGroup.gameObject.SetActive(false);
                }

            }

            for (int i = 0; i < itemCount; i++)
            {
                if (items.Count <= i)
                {
                    var itemObj = CoreUtils.assetService.Instantiate(m_UI_Model_Item.gameObject);
                    itemObj.transform.SetParent(m_pl_itemList_HorizontalLayoutGroup.transform);
                    itemObj.transform.localScale = m_UI_Model_Item.m_root_RectTransform.localScale;
                    var item = new UI_Model_Item_SubView(itemObj.transform.GetComponent<RectTransform>());
                    items.Add(item);
                }
                
                var itemCfg =
                    CoreUtils.dataService.QueryRecord<ItemDefine>((int) materialQueue.completeItems[i].itemId);
                items[i].m_img_quality_PolygonImage.assetName = null;
                items[i].m_img_icon_PolygonImage.assetName = null;
                items[i].Refresh(itemCfg, "");
            }

            Vector2 size = m_pl_Size_PolygonImage.rectTransform.sizeDelta;
            m_pl_Size_PolygonImage.rectTransform.sizeDelta = new Vector2(78 + 10 * (itemCount - 1),size.y);
        }

        private bool isShowTime = true;
        private void ExchangeShowProduceTimeAndDescription()
        {
            if (m_root_RectTransform == null)
            {
                ClearProduceTimer();
                return;
            }
            m_lbl_time_Animator.gameObject.SetActive(isShowTime);
            m_lbl_desc_LanguageText.gameObject.SetActive(!isShowTime);
            if (isShowTime)
            {
                isShowTime = false;
                m_lbl_time_Animator.Play("Show",-1,0);
            }
            else
            {
                isShowTime = true;
                m_lbl_desc_Animator.Play("Show",-1,0);
            }
        }

        private void UpdateProduceTime(long costTime, long finishTime)
        {
            if (m_root_RectTransform == null)
            {
                ClearProduceTimer();
                return;
            }
            long leftTime;
            long currentTime = ServerTimeModule.Instance.GetServerTime();
            leftTime = finishTime - currentTime;
            if (leftTime < 0)
            {
                ClearProduceTimer();
                return;
            }
            m_lbl_time_LanguageText.text = ClientUtils.FormatCountDown((int) leftTime);
            m_pb_rogressBar_GameSlider.value = (float) (costTime - leftTime) / costTime;
        }

        private void ClearProduceTimer()
        {
            if (m_produceTimer != null)
            {
                m_produceTimer.Cancel();
                m_produceTimer = null;
            }

            if (m_produceInfoAnimationTimer != null)
            {
                m_produceInfoAnimationTimer.Cancel();
                m_produceInfoAnimationTimer = null;
            }
        }
        #endregion

        #region 锻造图标

        public void SetCanForgeInfo()
        {
            m_pl_offset.gameObject.SetActive(true);
            m_pl_offset1.gameObject.SetActive(false);
            
            m_btn_click_GameButton.onClick.RemoveAllListeners();
            m_btn_click_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_Equip);
            });
        }
        #endregion

        #region 生产图标

        public void SetCanProduceInfo()
        {
            m_pl_offset.gameObject.SetActive(false);
            m_pl_offset1.gameObject.SetActive(true);
            
            m_btn_click1_GameButton.onClick.RemoveAllListeners();
            m_btn_click1_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_Material);
            });
            m_lbl_languageText_LanguageText.text = LanguageUtils.getText(182089);
        }

        #endregion
        
        #region 图纸合成

        public void SetCanMixDrawingMaterial()
        {
            m_pl_offset.gameObject.SetActive(false);
            m_pl_offset1.gameObject.SetActive(true);
            
            m_btn_click1_GameButton.onClick.RemoveAllListeners();
            m_btn_click1_GameButton.onClick.AddListener(() =>
            {
                CoreUtils.uiManager.ShowUI(UI.s_Material,null,MaterialPageType.Mix);
            });
            m_lbl_languageText_LanguageText.text = LanguageUtils.getText(182090);
        }

        #endregion
    }
}
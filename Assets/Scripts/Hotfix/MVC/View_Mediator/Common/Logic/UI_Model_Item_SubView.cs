// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月17日
// Update Time         :    2020年2月17日
// Class Description   :    UI_Model_Item_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using UnityEngine.Events;
using Data;
using System;

namespace Game {
    public partial class UI_Model_Item_SubView : UI_SubView
    {
        public int ItemData1;
        public object ItemData2;
        public Action<UI_Model_Item_SubView> BtnClickListener;

        private Vector3 m_zeroVec = Vector3.zero;
        private Vector3 m_oneVec = Vector3.one;

        private GrayChildrens _makeChildrenGray;
        private GrayChildrens m_makeChildrenGray
        {
            get
            {
                if (_makeChildrenGray == null)
                {
                    _makeChildrenGray = gameObject.GetComponent<GrayChildrens>();
                }

                if (_makeChildrenGray == null)
                {
                    _makeChildrenGray = gameObject.AddComponent<GrayChildrens>();
                }
                
                return _makeChildrenGray;
            }
        }

        
        public void AddBtnListener()
        {
            m_btn_animButton_GameButton.onClick.AddListener(ClickCallback);
        }

        public void AddBtnListener(UnityAction callback)
        {
            m_btn_animButton_GameButton.onClick.AddListener(callback);
        }

        public void ClickCallback()
        {
            if (BtnClickListener != null)
            {
                BtnClickListener(this);
            }
        }

        public void RemoveBtnAllListener()
        {
            m_btn_animButton_GameButton.onClick.RemoveAllListeners();
        }

        public void SetSelectImgActive(bool isShow)
        {
            if (isShow)
            {
                m_img_select_PolygonImage.transform.localScale = m_oneVec;
            }
            else
            {
                m_img_select_PolygonImage.transform.localScale = m_zeroVec;
            }
        }


        public void SetGuildGift(string imageName)
        {
            m_img_quality_PolygonImage.sprite = null;
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, imageName);
            m_lbl_count_LanguageText.text = "";
            m_img_select_PolygonImage.transform.localScale = m_zeroVec;
            m_pl_desc_bg_PolygonImage.transform.localScale = m_zeroVec;
        }

        public void Refresh(ItemDefine itemDefine, string overlay, bool isSelect = false, bool isRegisterListener = false,bool isShowRedDot=false, Action callback =null)
        {
            if (itemDefine == null)
            {
                return;
            }
            //设置品质图片
            ClientUtils.LoadSprite(m_img_quality_PolygonImage, GetQualityImg(itemDefine.quality));
            //设置icon
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, itemDefine.itemIcon, false, callback);

            if (itemDefine.l_topID < 1)
            {
                m_pl_desc_bg_PolygonImage.transform.localScale = m_zeroVec;
            }
            else
            {
                m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
                m_lbl_desc_LanguageText.text = string.Format(LanguageUtils.getText(itemDefine.l_topID), ClientUtils.FormatComma(itemDefine.topData));
            }

            m_lbl_count_LanguageText.text = overlay;

            if (isSelect)
            {
                m_img_select_PolygonImage.transform.localScale = m_oneVec;
            }
            else
            {
                m_img_select_PolygonImage.transform.localScale = m_zeroVec;
            }

            if (isRegisterListener)
            {
                ItemData2 = itemDefine;
                RemoveBtnAllListener();
                AddBtnListener(()=> {
                    ItemDefine itemDefine1 = ItemData2 as ItemDefine;
                    float height = m_img_icon_PolygonImage.GetComponent<RectTransform>().rect.height/2;
                    HelpTip.CreateTip(LanguageUtils.getText(itemDefine1.l_nameID), m_root_RectTransform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(height).Show();
                });
            }
            
            m_img_redpoint_PolygonImage.gameObject.SetActive(isShowRedDot);
        }
        public void Refresh(ItemDefine itemDefine, Int64 overlay, bool isSelect = false, bool isRegisterListener = false,bool isShowRedDot=false, Action callback = null)
        {
            string overlayStr = "";
            if (overlay >= 1)
            {
                overlayStr = ClientUtils.FormatComma(overlay);
            }
            Refresh(itemDefine,overlayStr,isSelect,isRegisterListener,isShowRedDot, callback);
            
        }

        public void Refresh(ItemDefine itemDefine, bool isSelect = false, bool isShowRedDot = false, Action callback = null)
        {
            if (itemDefine == null)
            {
                return;
            }

            //设置icon
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, itemDefine.itemIcon);

            ItemData2 = itemDefine;

            RemoveBtnAllListener();

            AddBtnListener(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
        }

        public void Refresh(ArmsDefine armsDefine, Int64 overlay, bool isSelect = false)
        {
            //设置品质图片
            ClientUtils.LoadSprite(m_img_quality_PolygonImage, GetQualityImg(2));
            //设置icon
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, armsDefine.icon);

            m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
            m_lbl_desc_LanguageText.text = LanguageUtils.getText(armsDefine.l_armsID);
            m_lbl_count_LanguageText.gameObject.SetActive(true);
            if (overlay < 1)
            {
                m_lbl_count_LanguageText.text = "";
            }
            else
            {
                m_lbl_count_LanguageText.text = ClientUtils.FormatComma(overlay);
            }

            if (isSelect)
            {
                m_img_select_PolygonImage.transform.localScale = m_oneVec;
            }
            else
            {
                m_img_select_PolygonImage.transform.localScale = m_zeroVec;
            }
        }
        public void Refresh(CurrencyDefine currencyDefine, Int64 overlay, bool isSelect = false)
        {
            //设置品质图片
            ClientUtils.LoadSprite(m_img_quality_PolygonImage, GetQualityImg(2));
            //设置icon
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, currencyDefine.iconID);

            m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
            m_lbl_desc_LanguageText.text = LanguageUtils.getText(currencyDefine.l_desID);
            m_lbl_count_LanguageText.gameObject.SetActive(true);
            if (overlay < 1)
            {
                m_lbl_count_LanguageText.text = "";
            }
            else
            {
                m_lbl_count_LanguageText.text = ClientUtils.FormatComma(overlay);
            }

            if (isSelect)
            {
                m_img_select_PolygonImage.transform.localScale = m_oneVec;
            }
            else
            {
                m_img_select_PolygonImage.transform.localScale = m_zeroVec;
            }
        }
        /// <summary>
        ///  任务奖励显示用
        /// </summary>
        /// <param name="currencyDefine"></param>
        /// <param name="overlay"></param>
        /// <param name="isSelect"></param>
        public void Refresh(CurrencyDefine currencyDefine)
        {
            m_img_quality_PolygonImage.gameObject.SetActive(false);
            //设置icon
            ClientUtils.LoadSprite(m_img_icon_PolygonImage, currencyDefine.iconID);

            m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
            m_lbl_desc_LanguageText.text = "";
            m_lbl_count_LanguageText.text = "";
            m_img_select_PolygonImage.transform.localScale = m_zeroVec;
            m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// 奖励组显示 界面来源sourceType： 1活动日历 2达标+排行类型 排行奖励
        /// </summary>
        public void RefreshByGroup(RewardGroupData rewardGroupData, int sourceType = 1)
        {
            ItemData1 = sourceType;
            ItemData2 = rewardGroupData;
            switch ((EnumRewardType)rewardGroupData.RewardType)
            {
                case EnumRewardType.Currency:
                    if (sourceType == 3)
                    {
                        Refresh(rewardGroupData.CurrencyData.currencyDefine, rewardGroupData.number, false);
                    }
                    else
                    {
                        Refresh(rewardGroupData.CurrencyData.currencyDefine, 0, false);
                    }
                    if (sourceType == 1)
                    {
                        m_img_select_PolygonImage.gameObject.SetActive(false);
                        m_img_quality_PolygonImage.gameObject.SetActive(false);
                        m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
                    }
                    break;
                case EnumRewardType.Soldier:
                    if (sourceType == 3)
                    {
                        Refresh(rewardGroupData.SoldierData.armsDefine, rewardGroupData.number, false);
                    }
                    else
                    {
                        Refresh(rewardGroupData.SoldierData.armsDefine, 0, false);
                    }
                    if (sourceType == 1)
                    {
                        m_img_select_PolygonImage.gameObject.SetActive(false);
                        m_img_quality_PolygonImage.gameObject.SetActive(false);
                        m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
                    }
                    break;
                case EnumRewardType.Item:
                    if (sourceType == 3)
                    {
                        Refresh(rewardGroupData.ItemData.itemDefine, rewardGroupData.number, false);
                    }
                    else
                    {
                        Refresh(rewardGroupData.ItemData.itemDefine, 0, false);
                    }
                    break;
                default:
                    Debug.LogError("not find type");
                    break;
            }
            m_btn_animButton_GameButton.onClick.RemoveAllListeners();
            m_btn_animButton_GameButton.onClick.AddListener(RewardGroupBtnAddEvent);
        }

        private void RewardGroupBtnAddEvent()
        {
            float offset = m_img_icon_PolygonImage.GetComponent<RectTransform>().sizeDelta.y / 4;
            if (ItemData1 == 2)
            {
                offset = 10;
            }
            RewardGroupData rewardGroupData = ItemData2 as RewardGroupData;
            if (rewardGroupData == null)
            {
                return;
            }
            switch ((EnumRewardType)rewardGroupData.RewardType)
            {
                case EnumRewardType.Currency:

                    HelpTip.CreateTip(LanguageUtils.getText(rewardGroupData.CurrencyData.currencyDefine.l_desID),
                                      m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                    break;
                case EnumRewardType.Soldier:
                    HelpTip.CreateTip(LanguageUtils.getText(rewardGroupData.SoldierData.armsDefine.l_armsID),
                                      m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                    break;
                case EnumRewardType.Item:
                    HelpTipsDefine tipDefine = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(5000);
                    string str = LanguageUtils.getTextFormat(tipDefine.l_typeID,
                                                             LanguageUtils.getText(rewardGroupData.ItemData.l_nameID),
                                                             rewardGroupData.ItemData.descFormat);
                    HelpTip.CreateTip(str, m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                    break;
                default:
                    Debug.LogError("not find type");
                    break;
            }
        }

        /// <summary>
        /// 任务奖励显示用
        /// </summary>
        /// <param name="currencyDefine"></param>
        /// <param name="overlay"></param>
        /// <param name="isSelect"></param>
        public void Refresh(RewardGroupData rewardGroupData)
        {
            switch ((EnumRewardType)rewardGroupData.RewardType)
            {
                case EnumRewardType.Currency:
                    m_img_quality_PolygonImage.gameObject.SetActive(false);
                    if (m_img_icon_PolygonImage.assetName != rewardGroupData.CurrencyData.iconID)
                    {
                        ClientUtils.LoadSprite(m_img_icon_PolygonImage, rewardGroupData.CurrencyData.iconID);
                    }
                    m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
                    m_lbl_desc_LanguageText.text = "";
                    m_lbl_count_LanguageText.text = "";
                    m_img_select_PolygonImage.transform.localScale = m_zeroVec;
                    break;
                case EnumRewardType.Soldier:
                    m_img_quality_PolygonImage.gameObject.SetActive(false);
                    if (m_img_icon_PolygonImage.assetName != rewardGroupData.SoldierData.icon)
                    {
                        ClientUtils.LoadSprite(m_img_icon_PolygonImage, rewardGroupData.SoldierData.icon);
                    }
                    m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
                    m_lbl_desc_LanguageText.text = "";
                    m_lbl_count_LanguageText.text = "";
                    m_img_select_PolygonImage.transform.localScale = m_zeroVec;
                    break;
                case EnumRewardType.Item:
                    m_img_quality_PolygonImage.gameObject.SetActive(true);
                    string qualityImg = GetQualityImg(rewardGroupData.ItemData.quality);
                    if (m_img_quality_PolygonImage.assetName != qualityImg)
                    {
                        ClientUtils.LoadSprite(m_img_quality_PolygonImage, qualityImg);
                    }
                    if (m_img_icon_PolygonImage.assetName != rewardGroupData.ItemData.itemIcon)
                    {
                        ClientUtils.LoadSprite(m_img_icon_PolygonImage, rewardGroupData.ItemData.itemIcon);
                    }
                    m_pl_desc_bg_PolygonImage.transform.localScale = m_oneVec;
                    m_lbl_desc_LanguageText.text = "";
                    m_lbl_count_LanguageText.text = "";
                    m_img_select_PolygonImage.transform.localScale = m_zeroVec;
                    break;
                default:
                    Debug.LogError("not find type");
                    break; 
            }

            m_pl_desc_bg_PolygonImage.gameObject.SetActive(false);
            AddClickEvent(rewardGroupData);
        }
        private void AddClickEvent(RewardGroupData itemPackageShow)
        {
            switch ((EnumRewardType)itemPackageShow.RewardType)
            {
                case EnumRewardType.Currency:
                    m_btn_animButton_GameButton.enabled = false;
                    break;
                case EnumRewardType.Soldier:
                    {
                        ArmsDefine armsDefine = itemPackageShow.SoldierData.armsDefine;
                        m_btn_animButton_GameButton.enabled = true;
                        m_btn_animButton_GameButton.onClick.RemoveAllListeners();
                        float offset = m_img_icon_PolygonImage.GetComponent<RectTransform>().sizeDelta.y / 4;
                        m_btn_animButton_GameButton.onClick.AddListener(() =>
                        {
                            HelpTip.CreateTip(LanguageUtils.getText(armsDefine.l_armsID), m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                        });
                    }
                    break;
                case EnumRewardType.Item:
                    {
                        m_btn_animButton_GameButton.enabled = true;
                        m_btn_animButton_GameButton.onClick.RemoveAllListeners();
                        float offset = m_img_icon_PolygonImage.GetComponent<RectTransform>().sizeDelta.y / 4;
                        m_btn_animButton_GameButton.onClick.AddListener(() =>
                        {
                            HelpTip.CreateTip(LanguageUtils.getText(itemPackageShow.ItemData.l_nameID), m_img_icon_PolygonImage.transform).SetStyle(HelpTipData.Style.arrowDown).SetOffset(offset).Show();
                        });
                    }
                    break;
            }

        }

        public string GetQualityImg(int quality)
        {
            quality = quality - 1;
            return RS.ItemQualityBg[quality];
        }
        
        public void SetGray(bool isGray)
        {
            if (isGray)
            {
                m_makeChildrenGray?.Gray();
            }
            else
            {
                m_makeChildrenGray?.Normal();
            }
        }
        
    }
}
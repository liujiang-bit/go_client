// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月14日
// Update Time         :    2020年4月14日
// Class Description   :    UI_Item_TavernSingleReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using SprotoType;
using Data;
using System;
using System.Collections.Generic;

namespace Game
{
    public partial class UI_Item_TavernSingleReward_SubView : UI_SubView
    {
        private GameObject m_effectObject;
        private GameObject m_bottomEffect;
        private HeroProxy m_heroProxy;
        private BagProxy m_bagProxy;

        private int m_effectQuality;

        private float m_itemShowTime = 0.39f;
        private float m_bottomEffectShowTime = 0.13f;

        private Dictionary<string, GameObject> m_dicAsset;

        private BoxRewardInfo m_boxInfo;
        private Action m_callback;

        private bool m_isInit;

        public void RemoveEffect()
        {
            if (m_effectObject != null)
            {
                CoreUtils.assetService.Destroy(m_effectObject);
                m_effectObject = null;
            }
            if (m_bottomEffect != null)
            {
                CoreUtils.assetService.Destroy(m_bottomEffect);
                m_bottomEffect = null;
            }
        }

        public void SetEffectVisible(bool isShow)
        {
            if (m_bottomEffect != null)
            {
                m_bottomEffect.gameObject.SetActive(isShow);
            }
        }

        private int GetHeroQuality(int quality)
        {
            if (quality <= 2)
            {
                return 0;
            }
            return quality - 2;
        }

        private int GetItemQuality(int quality)
        {
            if (quality <= 2)
            {
                return 0;
            }
            return quality - 2;
        }

        public void RefreshItem(BoxRewardInfo boxInfo, Action callback)
        {
            if (!m_isInit)
            {
                m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                m_isInit = true;
            }
            m_boxInfo = boxInfo;
            m_callback = callback;
            //判断光效颜色
            int quality = -1;
            if (boxInfo.HeroInfo != null)
            {
                HeroDefine define = CoreUtils.dataService.QueryRecord<HeroDefine>((int)boxInfo.HeroInfo.heroId);
                quality = GetHeroQuality(define.rare);
            }
            else
            {
                ItemDefine define = CoreUtils.dataService.QueryRecord<ItemDefine>((int)boxInfo.ItemInfo.itemId);
                quality = GetItemQuality(define.quality);
            }
            if (quality >= RS.TavernBoxShowEffect.Length)
            {
                Debug.LogErrorFormat("quality 超出范围：{0}", quality);
                quality = RS.TavernBoxShowEffect.Length - 1;
            }
            m_effectQuality = quality;

            List<string> prefabList = new List<string>();
            prefabList.Add(RS.TavernBoxShowEffect[quality]);
            prefabList.Add(RS.TavernBoxBottomEffect[quality]);
            ClientUtils.PreLoadRes(gameObject, prefabList, OnLoadFinish);
        }

        private void OnLoadFinish(Dictionary<string, GameObject> asset)
        {
            if (gameObject == null)
            {
                return;
            }
            m_dicAsset = asset;

            //添加结算特效
            GameObject effectGO = CoreUtils.assetService.Instantiate(m_dicAsset[RS.TavernBoxShowEffect[m_effectQuality]]);
            effectGO.SetActive(true);
            effectGO.transform.SetParent(m_pl_effect);
            effectGO.transform.localPosition = Vector3.zero;
            effectGO.transform.localScale = Vector3.one;
            m_effectObject = effectGO;

            m_root_RectTransform.gameObject.SetActive(true);
            m_pl_CaptainName.gameObject.SetActive(false);
            m_pl_ItemName.gameObject.SetActive(false);

            if (m_effectQuality == 3)
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiSummonEpic);
            }
            else if (m_effectQuality == 2)
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiSummonRare);
            }
            else
            {
                CoreUtils.audioService.PlayOneShot(RS.SoundUiSummonNormal);
            }

            //显示item
            Timer.Register(m_itemShowTime, () => {
                if (gameObject == null)
                {
                    return;
                }
                if (m_boxInfo.HeroInfo != null)
                {
                    ShowHero(m_boxInfo.HeroInfo);
                }
                else
                {
                    ShowItem(m_boxInfo.ItemInfo);
                }
            });
            //添加底部特效
            Timer.Register(m_bottomEffectShowTime, () => {
                if (gameObject == null)
                {
                    return;
                }
                GameObject effectGO2 = CoreUtils.assetService.Instantiate(m_dicAsset[RS.TavernBoxBottomEffect[m_effectQuality]]);
                effectGO2.SetActive(true);
                effectGO2.transform.SetParent(m_pl_effect);
                effectGO2.transform.localPosition = Vector3.zero;
                effectGO2.transform.localScale = Vector3.one;
                m_bottomEffect = effectGO2;
            });
        }

        public void ShowItem(RewardItem info)
        {
            m_root_RectTransform.gameObject.SetActive(true);
            m_pl_CaptainName.gameObject.SetActive(false);
            m_pl_ItemName.gameObject.SetActive(true);

            ItemDefine define = CoreUtils.dataService.QueryRecord<ItemDefine>((int)info.itemId);
            if (define != null)
            {
                m_lbl_ItemName_LanguageText.text = LanguageUtils.getText(define.l_nameID);
                m_lbl_item_count_LanguageText.text = "";

                m_UI_Item_Bag.m_UI_Model_Item.gameObject.SetActive(false);
                m_UI_Item_Bag.m_UI_Model_Item.Refresh(define, info.itemNum, false, false, false, ()=> {
                    m_UI_Item_Bag.m_UI_Model_Item.gameObject.SetActive(true);
                });
                m_UI_Item_Bag.m_UI_Model_Item.m_lbl_count_LanguageText.gameObject.SetActive(true);

                ConfigDefine configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                if (configDefine.boxRewardType.Contains(define.subType))
                {
                    ItemHeroDefine itemHero = CoreUtils.dataService.QueryRecord<ItemHeroDefine>(define.ID);
                    if (itemHero != null)
                    {
                        HeroDefine define2 = CoreUtils.dataService.QueryRecord<HeroDefine>(itemHero.heroID);
                        if (define2 != null)
                        {
                            Int64 ownCount = m_bagProxy.GetItemNum((int)info.itemId);
                            string str = "";
                            HeroProxy.Hero heroData = m_heroProxy.GetHeroByID(itemHero.heroID);
                            if (heroData == null || heroData.data == null) //未拥有英雄
                            {
                                str = LanguageUtils.getTextFormat(760023, ClientUtils.FormatComma(ownCount), define2.getItemNum);
                            }
                            else //已拥有英雄
                            {
                                str = LanguageUtils.getTextFormat(760024, ClientUtils.FormatComma(ownCount));
                            }
                            m_lbl_item_count_LanguageText.text = str;
                        }
                    }
                }
            }
            if (m_callback != null)
            {
                m_callback();
            }
        }

        public void ShowHero(Heros heroInfo)
        {
            CaptainSummonParam param = new CaptainSummonParam();
            param.Source = 1;
            param.HeroId = (int)heroInfo.heroId;
            param.Callback = () => {
                RefreshHero(heroInfo, m_callback);
            };
            param.IsNew = (heroInfo.isNew == 1);
            CoreUtils.uiManager.ShowUI(UI.s_captainSummon, null, param);
        }

        public void RefreshHero(Heros heroInfo, Action callback)
        {
            m_root_RectTransform.gameObject.SetActive(true);
            m_pl_CaptainName.gameObject.SetActive(true);
            m_pl_ItemName.gameObject.SetActive(false);

            HeroDefine define = CoreUtils.dataService.QueryRecord<HeroDefine>((int)heroInfo.heroId);
            if (define != null)
            {
                m_lbl_capName_LanguageText.text = LanguageUtils.getText(define.l_nameID);
                CivilizationDefine define2 = CoreUtils.dataService.QueryRecord<CivilizationDefine>(define.civilization);
                if (define2 != null)
                {
                    m_lbl_civi_LanguageText.text = LanguageUtils.getText(define2.l_addNameID);
                }

                m_pl_spine_SkeletonGraphic.gameObject.SetActive(false);
                ClientUtils.LoadSpine(m_pl_spine_SkeletonGraphic, define.heroModel, () => {
                    if (m_pl_spine_SkeletonGraphic.gameObject == null)
                    {
                        return;
                    }
                    m_pl_spine_SkeletonGraphic.gameObject.SetActive(true);
                    if (callback != null)
                    {
                        callback();
                    }
                });
            }
        }
    }
}
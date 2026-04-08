// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Item_CaptainList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System;
using System.Collections.Generic;

namespace Game {
    public class CaptainListData
    {
        //标题
        public string title;
        public HeroProxy.Hero selectHero;
        public HeroProxy.Hero ignoreHero;//要忽略的主将
        public HeroProxy.SortType type;
        public bool ignoreOut;//忽略出城的
        public bool TopSelectHero;//默认选中的英雄居首
        public Dictionary<string, GameObject> m_assetDic;
        //点击关闭的回调
        public Action<HeroProxy.Hero> CloseCallback;
    }
    public partial class UI_Item_CaptainList_SubView : UI_SubView
    {
        private List<List<HeroProxy.Hero>> heroList;
        private HeroProxy m_HeroProxy;
        private TroopProxy m_TroopProxy;
        private CaptainListData data;
        private HeroProxy.Hero m_hero;
        private List<HeroProxy.Hero> m_ownHeros;
        private List<HeroProxy.Hero> m_summonHeros;
        private List<HeroProxy.Hero> m_noSummomHeros;
        private HeroProxy.SortType m_sortType;
        private int listCont = 0;
        public void SetData(CaptainListData data)
        {
            m_HeroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_btn_arr_GameButton.onClick.AddListener(() =>
            {
                m_UI_Pop_arrType_ArabLayoutCompment.gameObject.SetActive(true);
                m_UI_Item_CaptionArrType_star.SetSelected(
                    m_sortType == HeroProxy.SortType.Star);
                m_UI_Item_CaptionArrType_power.SetSelected(
                    m_sortType == HeroProxy.SortType.Power);
                m_UI_Item_CaptionArrType_level.SetSelected(
                    m_sortType == HeroProxy.SortType.Level);
                m_UI_Item_CaptionArrType_Quality.SetSelected(
                    m_sortType == HeroProxy.SortType.Rare);
            });

            m_UI_Item_CaptionArrType_star.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Star);
            });
            m_UI_Item_CaptionArrType_level.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Level);
            });
            m_UI_Item_CaptionArrType_power.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Power);
            });
            m_UI_Item_CaptionArrType_Quality.AddClickEvent(() =>
            {
                HeroSortRefresh(HeroProxy.SortType.Rare);
            });


            this.m_UI_Item_CaptainPartline.m_lbl_text_LanguageText.text = data.title;
            this.data = data;
            heroList = new List<List<HeroProxy.Hero>>();
            SortHeroByType(data.type);
            RefreshHeroTitleSortText(data.type);
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = ListHeroItemByIndex;
            m_sv_captainHead_ListView.SetInitData(data.m_assetDic, funcTab);
            m_sv_captainHead_ListView.FillContent(heroList.Count);
            m_lbl_count_LanguageText.text = listCont.ToString("N0");
        }

        private void InitUI()
        {
            m_UI_Item_CaptainPartline.gameObject.SetActive(false);

        }
        private void HeroSortRefresh(HeroProxy.SortType type)
        {
            heroList = new List<List<HeroProxy.Hero>>();
            SortHeroByType(type);
            RefreshHeroTitleSortText(type);

            m_sv_captainHead_ListView.FillContent(heroList.Count);
        }
        //英雄数据排序
        private void SortHeroByType(HeroProxy.SortType type )
        {
            m_HeroProxy.GetHerosBySort(out m_ownHeros, out m_summonHeros, out m_noSummomHeros, type);

            List<HeroProxy.Hero> list1 = new List<HeroProxy.Hero>();
            for (int i = 0; i < m_ownHeros.Count; i++)
            {
                if (m_ownHeros[i] == data.ignoreHero)
                {
                    continue;
                }
                if (data.ignoreOut)
                {
                    if (!m_TroopProxy.IsWarByHero(m_ownHeros[i].config.ID))
                    {
                        if (data.selectHero!=null&& m_ownHeros[i] == data.selectHero)
                        {
                            list1.Insert(0,m_ownHeros[i]);
                        }
                        else
                        {
                            list1.Add(m_ownHeros[i]);
                        }
                    }
                }
                else
                {
                    if (data.selectHero != null && m_ownHeros[i] == data.selectHero)
                    {
                        list1.Insert(0, m_ownHeros[i]);
                    }
                    else
                    {
                        list1.Add(m_ownHeros[i]);
                    }
                    list1.Add(m_ownHeros[i]);
                }
            }
            listCont = list1.Count;
            for (int i = 0; i < list1.Count; i += 2)
            {
                var item2 = new List<HeroProxy.Hero>();
                item2.Add(list1[i]);
                if (i + 1 < list1.Count)
                {
                    item2.Add(list1[i + 1]);
                }
                heroList.Add(item2);
            }
        }

        private void RefreshHeroTitleSortText(HeroProxy.SortType type)
        {
            m_UI_Pop_arrType_ArabLayoutCompment.gameObject.SetActive(false);
            m_sortType = type;
            switch (type)
            {
                case HeroProxy.SortType.Rare:
                    m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200026);

                    break;
                case HeroProxy.SortType.Star:
                    m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200027);
                    break;
                case HeroProxy.SortType.Level:
                    m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200028);
                    break;
                case HeroProxy.SortType.Power:
                    m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(300005);
                    break;
            }
        }

        private void ListHeroItemByIndex(ListView.ListItem listItem)
        {
            UI_LC_Captain_SubView subView = null;
            List<HeroProxy.Hero> heros = heroList[listItem.index];
            if (listItem.data != null)
            {
                subView = listItem.data as UI_LC_Captain_SubView;

            }
            else
            {
                subView = new UI_LC_Captain_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
            }
            subView.m_UI_Item_CaptainHead1.m_btn_select_GameButton.onClick.RemoveAllListeners();
            subView.m_UI_Item_CaptainHead2.m_btn_select_GameButton.onClick.RemoveAllListeners();
            subView.m_UI_Item_CaptainHead1.m_btn_select_GameButton.onClick.AddListener(()=> {
                if (heros[0] == data.selectHero)
                {
                    return;
                }
                data.CloseCallback?.Invoke(heros[0]); });
            if (heros.Count > 1)
            {
                subView.m_UI_Item_CaptainHead2.m_btn_select_GameButton.onClick.AddListener(() => {
                    if (heros[1] == data.selectHero)
                    {
                        return;
                    }
                    data.CloseCallback?.Invoke(heros[1]);
                });
            }
            subView.SetHero(heros);
            subView.SelecteHero(data.selectHero);
        }
    }
}
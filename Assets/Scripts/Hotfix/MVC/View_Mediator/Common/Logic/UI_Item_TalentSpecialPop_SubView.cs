// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月26日
// Update Time         :    2020年5月26日
// Class Description   :    UI_Item_TalentSpecialPop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_TalentSpecialPop_SubView : UI_SubView
    {
        private List<HeroTalentMasteryDefine> m_heroTalentMasteryDefineList = new List<HeroTalentMasteryDefine>();
        private int m_talentPoint;
        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_closeButton_GameButton.onClick.AddListener(OnClickClose);
        }

        public void SetInfo(HeroTalentDefine heroTalentDefine,int talentPoint,int curLevel)
        {
            m_talentPoint = talentPoint;
            if (heroTalentDefine != null)
            {
                m_heroTalentMasteryDefineList.Clear();
                var masteryDefines = CoreUtils.dataService.QueryRecords<Data.HeroTalentMasteryDefine>();
                foreach (var mastery in masteryDefines)
                {
                    if (mastery.group == heroTalentDefine.masteryGroupID)
                    {
                        m_heroTalentMasteryDefineList.Add(mastery);
                    }
                }
                m_heroTalentMasteryDefineList.Sort((HeroTalentMasteryDefine a, HeroTalentMasteryDefine b) =>
                {
                    return a.level.CompareTo(b.level);
                });
            }

            m_UI_Item_TalentSpecialPop_Animator.gameObject.SetActive(true);
            m_btn_closeButton_GameButton.gameObject.SetActive(true);
            m_pl_pos.gameObject.SetActive(true);

            m_lbl_title_LanguageText.text =
                LanguageUtils.getTextFormat(175018, LanguageUtils.getText(m_heroTalentMasteryDefineList[0].name), curLevel);

            m_lbl_mes_LanguageText.text = LanguageUtils.getText(175019);
            
            ClientUtils.PreLoadRes(gameObject, m_sv_talentList_ListView.ItemPrefabDataList, LoadTalentItemFinish);
        }
        
        private void LoadTalentItemFinish(Dictionary<string, GameObject> dic)
        {
            m_sv_talentList_ListView.Clear();
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.GetItemSize = GetItemSize;
            functab.ItemEnter = InitTalentListItem;
            m_sv_talentList_ListView.SetInitData(dic, functab);
            m_sv_talentList_ListView.FillContent(m_heroTalentMasteryDefineList.Count);
            for (int i = 0; i < m_heroTalentMasteryDefineList.Count;i++)
            {
                m_sv_talentList_ListView.RefreshItem(i);
            }

        }
        
        private void InitTalentListItem(ListView.ListItem item)
        {
            UI_Item_TalentSpecialText_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_TalentSpecialText_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_TalentSpecialText_SubView;
            }
            if (subView == null) return;
            if (item.index >= m_heroTalentMasteryDefineList.Count) return;
            subView.Init(m_heroTalentMasteryDefineList[item.index],m_talentPoint);
        }

        private float GetItemSize(ListView.ListItem item)
        {
            UI_Item_TalentSpecialText_SubView subView = null;
            if (item.data == null)
            {
                return 0;
            }
            else
            {
                subView = item.data as UI_Item_TalentSpecialText_SubView;
            }

            float height = LayoutUtility.GetPreferredHeight(subView.m_lbl_message_LanguageText.rectTransform);
            subView.m_UI_Item_TalentSpecialText.sizeDelta = new Vector2(subView.m_UI_Item_TalentSpecialText.sizeDelta.x,height);
            return height;
        }

        private void OnClickClose()
        {
            m_UI_Item_TalentSpecialPop_Animator.gameObject.SetActive(false);
            m_btn_closeButton_GameButton.gameObject.SetActive(false);
            m_pl_pos.gameObject.SetActive(false);
        }
    }
}
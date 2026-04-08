// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月27日
// Update Time         :    2020年5月27日
// Class Description   :    UI_Item_TalentTop_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;

namespace Game {
    public partial class UI_Item_TalentTop_SubView : UI_SubView
    {
        private HeroProxy.Hero m_Hero;
        private HeroTalentDefine m_heroTalentDefine;
        private List<HeroTalentMasteryDefine> m_heroTalentMasteryDefineList = new List<HeroTalentMasteryDefine>();
        private List<GameObject> m_StarList = new List<GameObject>();
        private RectTransform m_parentRoot;
        private UI_Item_TalentSpecialPop_SubView m_PopView;
        private int m_activePoint;
        private int m_curLevel;
        
        protected override void BindEvent()
        {
            m_pl_mes_GameButton.onClick.AddListener(OnClickEvent);
        }

        public void InitData(HeroProxy.Hero heroInfo,HeroTalentDefine talentDefine,RectTransform parentRoot,UI_Item_TalentSpecialPop_SubView popView)
        {
            m_Hero = heroInfo;
            m_parentRoot = parentRoot;
            m_PopView = popView;
            if (talentDefine != null)
            {
                m_heroTalentDefine = talentDefine;
                var masteryDefines = CoreUtils.dataService.QueryRecords<Data.HeroTalentMasteryDefine>();
                m_heroTalentMasteryDefineList.Clear();
                m_StarList.Clear();
                foreach (var mastery in masteryDefines)
                {
                    if (mastery.group == m_heroTalentDefine.masteryGroupID)
                    {
                        m_heroTalentMasteryDefineList.Add(mastery);
                    }
                }
                m_heroTalentMasteryDefineList.Sort((HeroTalentMasteryDefine a, HeroTalentMasteryDefine b) =>
                    {
                        return a.level.CompareTo(b.level);
                    });

                m_StarList.Add(m_img_light1_PolygonImage.gameObject);
                m_StarList.Add(m_img_light2_PolygonImage.gameObject);
                m_StarList.Add(m_img_light3_PolygonImage.gameObject);
                m_StarList.Add(m_img_light4_PolygonImage.gameObject);
                m_StarList.Add(m_img_light5_PolygonImage.gameObject);
            }
        }

        public void Refresh(int index)
        {
            if (m_heroTalentDefine == null) return;
            var heroTypeDefine = CoreUtils.dataService.QueryRecord<Data.HeroTalentTypeDefine>(m_heroTalentDefine.type);
            if (heroTypeDefine != null)
            {
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, heroTypeDefine.icon);
                m_lbl_name_LanguageText.text = LanguageUtils.getText(m_heroTalentDefine.l_talentID);
            }

            int curPoint = m_Hero != null ? m_Hero.GetTalentMasteryPointByIndex(index,int.Parse(m_heroTalentDefine.gainTree)) : 0;
            m_activePoint = curPoint;
            for (int i = 0; i < m_StarList.Count; i++)
            {
                if (m_heroTalentMasteryDefineList.Count > i && m_heroTalentMasteryDefineList[i].needTalentPoint <= curPoint)
                {
                    m_StarList[i].SetActive(true);
                    if (m_curLevel < m_heroTalentMasteryDefineList[i].level)
                    {
                        m_curLevel = m_heroTalentMasteryDefineList[i].level;
                    }
                }
                else
                {
                    m_StarList[i].SetActive(false);
                }
            }
        }
        
        public void OnClickEvent()
        {
            if (m_parentRoot != null && m_PopView != null)
            {
                /*Vector2 arrpos = Vector2.zero;
                //Debug.LogError("positon: " + m_UI_Item_TalentSkill.position.ToString() + " screenPoint: " + CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(m_UI_Item_TalentSkill.position).ToString());
                
                RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parentRoot,
                    CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(m_UI_Item_TalentTop.position),
                    CoreUtils.uiManager.GetUICamera(), out arrpos);
                //Debug.LogError("lastPosition: " + arrpos.ToString());
                UIHelper.CalcPopupPos(arrpos, m_PopView.m_pl_pos.gameObject.transform as RectTransform,
                    m_PopView.m_img_arrowSideL_PolygonImage.gameObject, m_PopView.m_img_arrowSideR_PolygonImage.gameObject,
                    m_PopView.m_img_arrowSideTop_PolygonImage.gameObject, m_PopView.m_img_arrowSideButtom_PolygonImage.gameObject);*/
                

                m_PopView.SetInfo(m_heroTalentDefine,m_activePoint,m_curLevel);
                Rect rect = m_PopView.m_img_bg_PolygonImage.GetComponent<RectTransform>().rect;
                UIHelper.SelfAdaptPopViewPos(m_UI_Item_TalentTop.gameObject, m_parentRoot,
                    m_PopView.m_pl_pos.gameObject.transform as RectTransform,
                    m_PopView.m_img_arrowSideL_PolygonImage.gameObject,
                    m_PopView.m_img_arrowSideR_PolygonImage.gameObject,
                    m_PopView.m_img_arrowSideTop_PolygonImage.gameObject,
                    m_PopView.m_img_arrowSideButtom_PolygonImage.gameObject,80,false,rect.width,rect.height);
            }
        }
    }
}
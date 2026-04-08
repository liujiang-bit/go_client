// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月25日
// Update Time         :    2020年5月25日
// Class Description   :    UI_Item_TalentSkill_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using SprotoType;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_TalentSkill_SubView : UI_SubView
    {
        private HeroTalentGainTreeDefine m_heroTalentTreeDefine;
        private RectTransform m_parentRoot;
        private UI_Item_TalentSkillPop_SubView m_PopView;
        private HeroProxy.Hero m_Hero;
        private int m_TabIndex;
        private enum State
        {
            Normal = 0,//可激活
            Gray = 1,//待激活，灰色
            Light = 2,//已激活
            Invalid = 3,//不可激活
        }

        private State m_State = State.Gray;
        
        
        protected override void BindEvent()
        {
            m_pl_mes_GameButton.onClick.AddListener(OnClickEvent);
        }
        
        public void Init(HeroProxy.Hero heroInfo,int index,HeroTalentGainTreeDefine heroTalentTreeDefine,RectTransform parentRoot,UI_Item_TalentSkillPop_SubView popView)
        {
            m_TabIndex = index;
            m_Hero = heroInfo;
            m_parentRoot = parentRoot;
            m_PopView = popView;
            if (heroTalentTreeDefine != null)
            {
                m_heroTalentTreeDefine = heroTalentTreeDefine;
                ClientUtils.LoadSprite(m_img_icon_PolygonImage, heroTalentTreeDefine.icon);
                GrayChildrens grayCom = m_pl_mes_GameButton.gameObject.GetComponent<GrayChildrens>();
                grayCom.Normal();
                int curLevel = m_Hero.GetLevelByIndex(index);
                var talentTrees = m_Hero.GetTalentTreesByIndex(index);
                if (curLevel >= heroTalentTreeDefine.level)
                {
                    if (talentTrees.talentTree.Contains(heroTalentTreeDefine.ID))
                    {
                        m_State = State.Light;
                        m_img_cover_PolygonImage.gameObject.SetActive(false);
                        m_img_choose_PolygonImage.gameObject.SetActive(true);
                        m_img_icon_PolygonImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        m_State = State.Invalid;
                        m_img_cover_PolygonImage.gameObject.SetActive(true);
                        m_img_choose_PolygonImage.gameObject.SetActive(false);
                        m_img_icon_PolygonImage.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (heroInfo.GetCurPageRemainPoint(m_TabIndex) > 0 && heroTalentTreeDefine.level == (curLevel + 1))
                    {
                        m_State = State.Normal;
                        m_img_cover_PolygonImage.gameObject.SetActive(false);
                        m_img_choose_PolygonImage.gameObject.SetActive(false);
                        m_img_icon_PolygonImage.gameObject.SetActive(true);

                    }
                    else
                    {
                        m_State = State.Gray;
                        m_img_cover_PolygonImage.gameObject.SetActive(false);
                        m_img_choose_PolygonImage.gameObject.SetActive(false);
                        m_img_icon_PolygonImage.gameObject.SetActive(true);
                        grayCom.Gray();
                    }
                }
            }
            
        }

        public void OnClickEvent()
        {
            if (m_parentRoot != null && m_PopView != null)
            {
                m_PopView.SetInfo(m_heroTalentTreeDefine,(int)m_State, () =>
                {
                    Hero_TalentUp.request request = new Hero_TalentUp.request();
                    request.heroId = m_Hero.data.heroId;
                    request.id = m_heroTalentTreeDefine.ID;
                    request.index = m_TabIndex;
                    AppFacade.GetInstance().SendSproto(request);

                    CoreUtils.audioService.PlayOneShot(RS.SoundUiTalentAddPoint);
                });

                if (LanguageUtils.IsArabic())
                {
                    m_PopView.m_pl_pos.gameObject.GetComponent<RectTransform>().pivot = new Vector2(0,0.5f);
                }
                else
                {
                    m_PopView.m_pl_pos.gameObject.GetComponent<RectTransform>().pivot = new Vector2(1,0.5f);
                }

                Rect rect = m_PopView.m_img_bg_PolygonImage.GetComponent<RectTransform>().rect;
                UIHelper.SelfAdaptPopViewPos(m_UI_Item_TalentSkill.gameObject, m_parentRoot,
                    m_PopView.m_pl_pos.gameObject.transform as RectTransform,
                    m_PopView.m_img_arrowSideL_PolygonImage.gameObject,
                    m_PopView.m_img_arrowSideR_PolygonImage.gameObject,
                    m_PopView.m_img_arrowSideTop_PolygonImage.gameObject,
                    m_PopView.m_img_arrowSideButtom_PolygonImage.gameObject,80,true,rect.width,rect.height);

            }
        }
    }
}
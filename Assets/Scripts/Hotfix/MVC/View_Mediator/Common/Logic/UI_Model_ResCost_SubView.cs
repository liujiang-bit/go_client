// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年1月3日
// Update Time         :    2020年1月3日
// Class Description   :    UI_Model_ResCost_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using System;
using Data;

namespace Game {
    public partial class UI_Model_ResCost_SubView : UI_SubView
    {
        public ResCostModel ResCostModel;
        private bool m_isInit;

        public Dictionary<int, Int64> ResCostData = new Dictionary<int, Int64>();

        public bool UpdateResCost(Int64 food, Int64 wood, Int64 stone, Int64 gold)
        {
            if (!m_isInit)
            {
                ResCostModel = new ResCostModel();
                ResCostModel.SetTransform(m_UI_Model_ArmyTrainRes_1.gameObject.transform,
                                            m_UI_Model_ArmyTrainRes_2.gameObject.transform,
                                            m_UI_Model_ArmyTrainRes_3.gameObject.transform,
                                            m_UI_Model_ArmyTrainRes_4.gameObject.transform);
                ResCostModel.SetText(m_UI_Model_ArmyTrainRes_1.m_lbl_resCost_num_LanguageText,
                    m_UI_Model_ArmyTrainRes_2.m_lbl_resCost_num_LanguageText,
                    m_UI_Model_ArmyTrainRes_3.m_lbl_resCost_num_LanguageText,
                    m_UI_Model_ArmyTrainRes_4.m_lbl_resCost_num_LanguageText);

                m_UI_Model_ArmyTrainRes_1.m_btn_area_GameButton.onClick.AddListener(ClickFood);
                m_UI_Model_ArmyTrainRes_2.m_btn_area_GameButton.onClick.AddListener(ClickWood);
                m_UI_Model_ArmyTrainRes_3.m_btn_area_GameButton.onClick.AddListener(ClickStone);
                m_UI_Model_ArmyTrainRes_4.m_btn_area_GameButton.onClick.AddListener(ClickGold);

                m_isInit = true;
            }


            return ResCostModel.UpdateResCost(food, wood, stone, gold); 
        }


        private bool SetText(LanguageText text, int num, bool isEnough)
        {
            text.text =  ClientUtils.CurrencyFormat(num);

            if (isEnough)
            {
                text.color = Color.white;
            }
            else
            {
                text.color = Color.red;
            }

            return isEnough;
        }

        public void ClickStorage()
        {
            CoreUtils.uiManager.ShowUI(UI.s_AllianceDepot);
        }


        public bool UpdateGuildResCost(AllianceStudyDefine studyDefine,Dictionary<long,GuildCurrencyInfoEntity> currencyInfo)
        {
            if (!m_isInit)
            {
                m_UI_Model_ArmyTrainRes_1.m_btn_area_GameButton.onClick.AddListener(ClickStorage);
                m_UI_Model_ArmyTrainRes_2.m_btn_area_GameButton.onClick.AddListener(ClickStorage);
                m_UI_Model_ArmyTrainRes_3.m_btn_area_GameButton.onClick.AddListener(ClickStorage);
                m_UI_Model_ArmyTrainRes_4.m_btn_area_GameButton.onClick.AddListener(ClickStorage);
                m_UI_Model_ArmyTrainRes_5.m_btn_area_GameButton.onClick.AddListener(ClickStorage);
                m_isInit = true;
            }
            
            bool isNeedFood = studyDefine.needFood > 0;
            bool isNeedWood = studyDefine.needWood > 0;
            bool isNeedStone = studyDefine.needStone> 0;
            bool isNeedGold = studyDefine.needGold > 0;
            bool isNeedLeaguePoints = studyDefine.needLeaguePoints > 0;
            
           m_UI_Model_ArmyTrainRes_1.gameObject.SetActive(isNeedFood);
           m_UI_Model_ArmyTrainRes_2.gameObject.SetActive(isNeedWood);
           m_UI_Model_ArmyTrainRes_3.gameObject.SetActive(isNeedStone);
           m_UI_Model_ArmyTrainRes_4.gameObject.SetActive(isNeedGold);
           m_UI_Model_ArmyTrainRes_5.gameObject.SetActive(isNeedLeaguePoints);

           bool isNeed = true;

          
           if (isNeedFood)
           {
               var tb = SetText(m_UI_Model_ArmyTrainRes_1.m_lbl_resCost_num_LanguageText,studyDefine.needFood,studyDefine.needFood<= currencyInfo[(long)EnumCurrencyType.allianceFood].num);
               if (tb==false && isNeed)
               {
                   isNeed = false;
               }
           }
           if (isNeedWood)
           {
               var tb = SetText(m_UI_Model_ArmyTrainRes_2.m_lbl_resCost_num_LanguageText,studyDefine.needWood,studyDefine.needWood<= currencyInfo[(long)EnumCurrencyType.allianceWood].num);
               if (tb==false && isNeed)
               {
                   isNeed = false;
               }
           }
           if (isNeedStone)
           {
               var tb = SetText(m_UI_Model_ArmyTrainRes_3.m_lbl_resCost_num_LanguageText,studyDefine.needStone,studyDefine.needStone<= currencyInfo[(long)EnumCurrencyType.allianceStone].num);
               if (tb==false && isNeed)
               {
                   isNeed = false;
               }
           }
           if (isNeedGold)
           {
               var tb = SetText(m_UI_Model_ArmyTrainRes_4.m_lbl_resCost_num_LanguageText,studyDefine.needGold,studyDefine.needGold<= currencyInfo[(long)EnumCurrencyType.allianceGold].num);
               if (tb==false && isNeed)
               {
                   isNeed = false;
               }
           }
           
           if ( isNeedLeaguePoints)
           {
               
               var tb = SetText(m_UI_Model_ArmyTrainRes_5.m_lbl_resCost_num_LanguageText,studyDefine.needLeaguePoints,studyDefine.needLeaguePoints<= currencyInfo[(long)EnumCurrencyType.leaguePoints].num);

               if (tb==false && isNeed)
               {
                   isNeed = false;
               }
           }


           return isNeed;
        }

        private void ClickFood()
        {
            long[] m_rss = new long[4];
            m_rss[0] = ResCostModel.ResCostData[(int)EnumCurrencyType.food];
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, m_rss);
        }

        private void ClickWood()
        {
            long[] m_rss = new long[4];
            m_rss[1] = ResCostModel.ResCostData[(int)EnumCurrencyType.wood];
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, m_rss);
        }

        private void ClickStone()
        {
            long[] m_rss = new long[4];
            m_rss[2] = ResCostModel.ResCostData[(int)EnumCurrencyType.stone];
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, m_rss);
        }

        private void ClickGold()
        {
            long[] m_rss = new long[4];
            m_rss[3] = ResCostModel.ResCostData[(int)EnumCurrencyType.gold];
            CoreUtils.uiManager.ShowUI(UI.s_AddRes, null, m_rss);
        }
    }
}
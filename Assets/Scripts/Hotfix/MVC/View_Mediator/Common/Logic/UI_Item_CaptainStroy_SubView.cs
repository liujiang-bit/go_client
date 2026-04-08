// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    UI_Item_CaptainStroy_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using Client;
using Skyunion;
using System;
using UnityEngine;

namespace Game {
    public partial class UI_Item_CaptainStroy_SubView : UI_SubView
    {
        private HeroProxy.Hero m_Hero;
        public void setHero(HeroProxy.Hero hero)
        {
            m_Hero = hero;
            var heroInfo = hero.config;

            m_lbl_name_LanguageText.text = LanguageUtils.getText(heroInfo.l_nameID);
            m_lbl_title_LanguageText.text = LanguageUtils.getText(heroInfo.l_appellationID);

            if (hero.data == null)
            {
                m_lbl_summonData_LanguageText.text = LanguageUtils.getText(145015);
                m_lbl_killData_LanguageText.text = "0";
                m_lbl_killNpcData_LanguageText.text = "0";
            }
            else
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                DateTime TranslateDate = startTime.AddSeconds(m_Hero.data.summonTime);
                m_lbl_summonData_LanguageText.text = TranslateDate.ToString("yyyy-MM-dd HH:mm:ss");
                Debug.Log(m_Hero.data.summonTime);
                m_lbl_killData_LanguageText.text = hero.data.soldierKillNum.ToString();
                m_lbl_killNpcData_LanguageText.text = hero.data.savageKillNum.ToString();
            }

            //Color c;
            //var civilizationInfo = CoreUtils.dataService.QueryRecord<Data.CivilizationDefine>((int)heroInfo.civilization);
            //if (civilizationInfo != null)
            //{
            //    ClientUtils.LoadSprite(m_img_img_PolygonImage, civilizationInfo.civilizationMark);
            //    //ColorUtility.TryParseHtmlString(civilizationInfo.markColour, out c);
            //    //m_img_img_PolygonImage.color = c;
            //    m_img_img_PolygonImage.enabled = true;
            //}
            //else
            //{
            //    m_img_img_PolygonImage.enabled = false;
            //}

            while (m_pl_starlvs_GridLayoutGroup.transform.childCount < 6)
            {
                UnityEngine.Object.Instantiate(m_UI_Model_CaptainStar.gameObject, m_pl_starlvs_GridLayoutGroup.transform);
            }
            for (int i = 0; i < m_pl_starlvs_GridLayoutGroup.transform.childCount; i++)
            {
                new UI_Model_CaptainStar_SubView(m_pl_starlvs_GridLayoutGroup.transform.GetChild(i).GetComponent<RectTransform>()).setHight(hero.star > i);
            }

            var config = CoreUtils.dataService.QueryRecord<Data.ConfigDefine>((int)0);
            m_lbl_qualityData_LanguageText.text = LanguageUtils.getText(heroInfo.rare + config.rareLanguage - 1);
            Color c;
            ColorUtility.TryParseHtmlString(RS.HeroQualityColor[heroInfo.rare - 1], out c);
            m_lbl_qualityData_LanguageText.color = c;
            m_lbl_stroy_LanguageText.text = LanguageUtils.getText(heroInfo.l_desID);
        }
        public void Open()
        {
            gameObject.SetActive(true);
            var anim = gameObject.GetComponent<Animator>();
            if (!anim)
                return;
            if (LanguageUtils.IsArabic())
            {
                ClientUtils.PlayUIAnimation(anim, "OpenArb");
            }
            else
            {
                ClientUtils.PlayUIAnimation(anim, "OpenNoArb");
            }
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
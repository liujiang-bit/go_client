// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月22日
// Update Time         :    2020年5月22日
// Class Description   :    UI_Item_EquipAtt_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using PureMVC.Core;

namespace Game {
    public partial class UI_Item_EquipAtt_SubView : UI_SubView
    {
        private List<UI_Model_EquipAtt_SubView> m_equipAttLst = new List<UI_Model_EquipAtt_SubView>();
        private List<UI_Item_EquipComposeAtt_SubView> m_composeAttLst = new List<UI_Item_EquipComposeAtt_SubView>();
        private RectTransform m_equipAttRect;
        private HeroProxy m_heroProxy;
        protected override void BindEvent()
        {
            base.BindEvent();
            m_equipAttLst.Add(m_UI_Model_EquipAtt);
            m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
        }

        public void SetForgeItemInfo(ForgeEquipItemInfo itemInfo)
        {
            SetInfo(itemInfo.EquipID);
        }

        public void SetEquipItemInfo(EquipItemInfo itemInfo,bool isCaptainMain = false)
        {

            if (itemInfo.HeroID == 0)
                SetInfo(itemInfo.ItemID,itemInfo.Exclusive,itemInfo.HeroID,0,true,isCaptainMain);
            else
            {
                var hero = m_heroProxy.GetHeroByID(itemInfo.HeroID);
                var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(itemInfo.ItemID);
                int composeNum = GetComposeNum(hero,equipCfg.compose);
                SetInfo(itemInfo.ItemID,itemInfo.Exclusive,itemInfo.HeroID,composeNum,true,isCaptainMain);
            }
            if (itemInfo.Exclusive == 0)
            {
                m_pl_talent_Image.gameObject.SetActive(false);
            }
            else
            {
                m_pl_talent_Image.gameObject.SetActive(true);
            }
        }

        private void SetInfo(int itemId, int exclusive = 0, long hero = 0, int composeNum = 0,bool isCountCompose = true,bool isCaptainMain = false)
        {
            var itemCfg = CoreUtils.dataService.QueryRecord<ItemDefine>(itemId);
            var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(itemId);
            m_lbl_name_LanguageText.text = LanguageUtils.getText(itemCfg.l_nameID);

            m_lbl_name_LanguageText.color = GetQualityColor(itemCfg.quality);
                    
            m_lbl_lv_LanguageText.text = LanguageUtils.getTextFormat(182067, equipCfg.useLevel);
            
            //装备属性
            
            float plus = -1;
            if (exclusive != 0 && hero != 0)
            {
                var heroCfg = CoreUtils.dataService.QueryRecord<HeroDefine>((int)hero);
                foreach (var talent in heroCfg.talent)
                {
                    var talentCfg = CoreUtils.dataService.QueryRecord<HeroTalentDefine>(talent);
                    if (talentCfg.type == exclusive)
                    {
                        var config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                        plus = config.equipTalentPromote;
                        break;
                    }
                }
            }
            DisableEquipAttItem();
            for (int i = 0; i < equipCfg.att.Count && i <equipCfg.attAdd.Count; i++)
            {
                if (m_equipAttLst.Count <= i)
                {
                    var equipAttItemObj = GameObject.Instantiate(m_UI_Model_EquipAtt.gameObject,m_pl_equipatt_GridLayoutGroup.transform);
                    var equipAttItem = new UI_Model_EquipAtt_SubView(equipAttItemObj.GetComponent<RectTransform>());
                    m_equipAttLst.Add(equipAttItem);
                }
                m_equipAttLst[i].gameObject.SetActive(true);
                m_equipAttLst[i].SetAttInfo(equipCfg.att[i], equipCfg.attAdd[i],plus);
            }

            if (m_equipAttRect == null)
            {
                m_equipAttRect = m_pl_equipatt_GridLayoutGroup.GetComponent<RectTransform>();
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_equipAttRect);

            SetAttSize();


            //专属天赋
            if (exclusive==0)
            {
                m_lbl_title2_LanguageText.text = LanguageUtils.getText(182069);
                m_lbl_talent_LanguageText.text = LanguageUtils.getText(182076);
                m_img_icon_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                var talentCfg = CoreUtils.dataService.QueryRecord<HeroTalentTypeDefine>(exclusive);
                
                m_lbl_title2_LanguageText.text = LanguageUtils.getTextFormat(182079,LanguageUtils.getText(talentCfg.l_talentID));
                m_img_icon_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(m_img_icon_PolygonImage,talentCfg.equipIcon);
                m_lbl_talent_LanguageText.text = LanguageUtils.getTextFormat(182080,LanguageUtils.getText(talentCfg.l_talentID));
                if (hero > 0 && plus < 0)
                {
                    m_lbl_talent_LanguageText.color = Color.gray;
                }
                else
                {
                    if (isCaptainMain)
                        m_lbl_talent_LanguageText.color = new Color(37/255.0f,39/255.0f,39/255.0f);
                    else
                    {
                        m_lbl_talent_LanguageText.color = Color.white;
                    }
                }
            }
            SetTalentSize();
            
            //套装效果
            m_UI_Item_EquipComposeAtt_LanguageText.gameObject.SetActive(false);
            if (equipCfg.compose != 0)
            {
                var composeCfg = CoreUtils.dataService.QueryRecord<EquipComposeDefine>(equipCfg.compose);

                m_pl_compose_Image.gameObject.SetActive(true);
                DisableComposeAttItem();
                int index = 0;
                int maxComposeEquipNum = 0;
                if (composeCfg.compose2 != null && composeCfg.compose2.Count>0)
                {
                    maxComposeEquipNum = 2;
                    SetComposeAtt(ref index, composeCfg.compose2, composeCfg.compose2Add, 182072, composeNum >= 2 || !isCountCompose);
                }
                if (composeCfg.compose4 != null&& composeCfg.compose4.Count>0)
                {
                    maxComposeEquipNum = 4;
                    SetComposeAtt(ref index, composeCfg.compose4, composeCfg.compose4Add, 182073, composeNum >= 4 || !isCountCompose);
                }
                if (composeCfg.compose6 != null&& composeCfg.compose6.Count>0)
                {
                    maxComposeEquipNum = 6;
                    SetComposeAtt(ref index, composeCfg.compose6, composeCfg.compose6Add, 182074, composeNum >= 6 || !isCountCompose);
                }
                if (composeCfg.compose8 != null&& composeCfg.compose8.Count>0)
                {
                    maxComposeEquipNum = 8;
                    SetComposeAtt(ref index, composeCfg.compose8, composeCfg.compose8Add, 182075, composeNum >= 8 || !isCountCompose);
                }
                if (maxComposeEquipNum == 0)
                {
                    m_pl_compose_Image.gameObject.SetActive(false);
                }
                else
                {
                    m_UI_Item_EquipComposeAtt_LanguageText.gameObject.SetActive(true);
                    m_UI_Item_EquipComposeAtt_LanguageText.text = isCountCompose?LanguageUtils.getTextFormat(182071,
                        LanguageUtils.getText(composeCfg.l_nameID), composeNum, maxComposeEquipNum):LanguageUtils.getText(composeCfg.l_nameID);
                }
            }
            else
            {
                m_pl_compose_Image.gameObject.SetActive(false);
            }
        }

        public int GetComposeNum(HeroProxy.Hero hero, int compose)
        {
            if (hero == null || hero.data == null ) return 0;
            
            int composeNum = 0;
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            
            List<int> heroEquipIndex = new List<int>();
            if (hero.data.head > 0) heroEquipIndex.Add((int)hero.data.head);
            if (hero.data.breastPlate > 0) heroEquipIndex.Add((int)hero.data.breastPlate);
            if (hero.data.weapon > 0) heroEquipIndex.Add((int)hero.data.weapon);
            if (hero.data.gloves > 0) heroEquipIndex.Add((int)hero.data.gloves);
            if (hero.data.pants > 0) heroEquipIndex.Add((int)hero.data.pants);
            if (hero.data.accessories1 > 0) heroEquipIndex.Add((int)hero.data.accessories1);
            if (hero.data.accessories2 > 0) heroEquipIndex.Add((int)hero.data.accessories2);
            if (hero.data.shoes > 0) heroEquipIndex.Add((int)hero.data.shoes);

            foreach (var equipIndex in heroEquipIndex)
            {
                var equipInfo = bagProxy.GetEquipItemInfo(equipIndex);
                if (equipInfo == null)
                {
                    continue;
                }
                var equipCfg = CoreUtils.dataService.QueryRecord<EquipDefine>(equipInfo.ItemID);
                if (equipCfg.compose == compose)
                    composeNum++;
            }

            return composeNum;
        }

        public void SetComposeAtt(ref int index, List<int> atts, List<int> adds, int languageID,bool isActive)
        {
            for (int i = 0; i < atts.Count; i++)
            {
                var attrCfg = CoreUtils.dataService.QueryRecord<EquipAttDefine>(atts[i]);
                if (attrCfg.l_nameID==0)
                {
                    continue;
                }
                if (m_composeAttLst.Count <= index)
                {
                    var composeAttItemObj = GameObject.Instantiate(m_UI_Item_EquipComposeAtt_LanguageText.gameObject,m_pl_composeAt_GridLayoutGroup.transform);
                    var composeAttItem = new UI_Item_EquipComposeAtt_SubView(composeAttItemObj.GetComponent<RectTransform>());
                    m_composeAttLst.Add(composeAttItem);
                }
                m_composeAttLst[index].SetText(LanguageUtils.getTextFormat(languageID,LanguageUtils.getTextFormat(attrCfg.l_nameID),adds[i]),isActive);
                m_composeAttLst[index].gameObject.SetActive(true);
                index++;
            }

            return ;
        }

        private void DisableEquipAttItem()
        {
            foreach (var attSubView in m_equipAttLst)
            {
                attSubView.gameObject.SetActive(false);
            }
        }

        private void SetAttSize()
        {
            int num = 0;
            foreach (var attSubView in m_equipAttLst)
            {
                if (attSubView.gameObject.activeSelf)
                {
                    num++;
                }
            }
            float ySize = m_equipAttLst[0].gameObject.GetComponent<RectTransform>().sizeDelta.y;
            m_pl_att_Image.rectTransform.sizeDelta = new Vector2(m_pl_att_Image.rectTransform.sizeDelta.x,70+ySize*(num-1));
        }

        private void SetTalentSize()
        {
            m_pl_talent_Image.rectTransform.sizeDelta = new Vector2(m_pl_talent_Image.rectTransform.sizeDelta.x,m_lbl_title2_LanguageText.preferredHeight+m_lbl_talent_LanguageText.preferredHeight);
        }

        private void DisableComposeAttItem()
        {
            foreach (var attSubView in m_composeAttLst)
            {
                attSubView.gameObject.SetActive(false);
            }
        }
        
        private Color GetQualityColor(int quality)
        {
            switch (quality)
            {
                case 1:
                    return new Color((float)136.0/255,(float)136.0/255,(float)136.0/255);
                case 2:
                    return new Color((float)42.0/255,(float)145.0/255,(float)10.0/255);
                case 3:
                    return new Color((float)104.0/255,(float)126.0/255,(float)237.0/255);
                case 4:
                    return new Color((float)126.0/255,(float)84.0/255,(float)218.0/255);
                case 5:
                    return new Color((float)185.0/255,(float)134.0/255,(float)17.0/255);
                default:
                    return Color.white;
            }
        }

        public float GetHeight()
        {
            float height = 0;
            if (m_pl_att_Image.gameObject.activeSelf)
            {
                height = height + m_pl_att_Image.gameObject.GetComponent<RectTransform>().rect.height;
            }
            if (m_pl_talent_Image.gameObject.activeSelf)
            {
                height = height + m_pl_talent_Image.gameObject.GetComponent<RectTransform>().rect.height;
            }
            if (m_pl_compose_ArabLayoutCompment.gameObject.activeSelf)
            {
                height = height + m_pl_compose_Image.gameObject.GetComponent<RectTransform>().rect.height;
            }
            return height;
        }

    }
}
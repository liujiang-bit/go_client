// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月4日
// Update Time         :    2020年6月4日
// Class Description   :    UI_Item_CaptainEquipUse_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Item_CaptainEquipUse_SubView : UI_SubView
    {
        private int m_gridType; // 0:身上的格子 1:列表里的格子
        
        private string m_EquipKey;
        private int m_EquipType;
        private int m_index;
        private HeroProxy.Hero m_heroInfo;

        private EquipItemInfo m_EquipInfo;

        protected override void BindEvent()
        {
            m_btn_btn_GameButton.onClick.AddListener(OnEquipClickEvent);
        }

        public void InitEquipForHero(string equipKey,HeroProxy.Hero hero)
        {
            m_gridType = 0;
            m_EquipKey = equipKey;
            m_heroInfo = hero;
            m_EquipType = int.Parse(m_EquipKey.Split('_')[0]);
            m_index = int.Parse(m_EquipKey.Split('_')[1]);
            Refresh();
            RefreshRedPoint();
        }

        public void InitEquipForList(EquipItemInfo equipInfo,HeroProxy.Hero heroInfo)
        {
            m_gridType = 1;
            m_EquipInfo = equipInfo;
            m_heroInfo = heroInfo;
            Refresh();
            RefreshRedPoint();
        }

        //缩略图显示
        public void InitEquipForThumbnail(string equipKey,HeroProxy.Hero hero)
        {
            m_gridType = 2;
            m_EquipKey = equipKey;
            m_heroInfo = hero;
            m_EquipType = int.Parse(m_EquipKey.Split('_')[0]);
            m_index = int.Parse(m_EquipKey.Split('_')[1]);
            Refresh();
            //RefreshRedPoint();
        }

        public void Refresh()
        {
            if (m_gridType == 0)
            {
                if (m_heroInfo != null)
                {
                    var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                    int heroEquip = m_heroInfo.GetHeroEquipByType(m_index);
                    if (heroEquip > 0)
                    {
                        var equipInfo = bagProxy.GetEquipItemInfo(heroEquip);
                        var itemData = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(equipInfo.ItemID);
                        SetQualityColor(itemData.quality);
                        SetEquipIcon(false,itemData.itemIcon,false);
                        SetTalentIcon(equipInfo.Exclusive);
                        
                        m_img_key_PolygonImage.gameObject.SetActive(false);
                        m_img_add_PolygonImage.gameObject.SetActive(false);

                    }
                    else
                    {
                        m_img_color_PolygonImage.gameObject.SetActive(false);
                        int index = m_index;
                        if (m_index == 7)
                        {
                            index = 9;
                        }
                        else if (m_index == 8)
                        {
                            index = 7;
                        }

                        SetEquipIcon(true,"",true);
                        m_img_talent_PolygonImage.gameObject.SetActive(false);
                        m_img_key_PolygonImage.gameObject.SetActive(false);

                        var equipList = bagProxy.GetEquipItemsBySubType(m_EquipType);
                        var usableEquipList = bagProxy.GetUsableEquipItemsBySubType(m_EquipType);

                        if (equipList.Count == 0)
                        {
                            m_img_add_PolygonImage.gameObject.SetActive(false);
                        }
                        else 
                        {
                            GrayChildrens makeGray =
                                m_img_add_PolygonImage.gameObject.GetComponent<GrayChildrens>();
                            if (makeGray == null)
                            {
                                makeGray = m_img_add_PolygonImage.gameObject.AddComponent<GrayChildrens>();
                            }
                            
                            if (usableEquipList.Count == 0)
                            {
                                m_img_add_PolygonImage.gameObject.SetActive(true);
                                makeGray.Gray();
                            }
                            else
                            {
                                m_img_add_PolygonImage.gameObject.SetActive(true);
                                makeGray.Normal();
                            }
                        
                        }
                    }
                }
            }
            else if (m_gridType == 1)
            {
                if (m_EquipInfo != null)
                {
                    var itemData = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(m_EquipInfo.ItemID);
                    var equipData = CoreUtils.dataService.QueryRecord<Data.EquipDefine>(m_EquipInfo.ItemID);
                    SetQualityColor(itemData.quality);
                    SetEquipIcon(false,itemData.itemIcon,false);
                    SetTalentIcon(m_EquipInfo.Exclusive);

                    if (m_EquipInfo.HeroID > 0)
                    {
                        var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                        m_UI_Model_CaptainHead.gameObject.SetActive(true);   
                        m_UI_Model_CaptainHead.SetHero(heroProxy.GetHeroByID(m_EquipInfo.HeroID ));
                    }
                    else
                    {
                        m_UI_Model_CaptainHead.gameObject.SetActive(false);   
                    }

                    if (m_heroInfo != null)
                    {
                        if (equipData.useLevel > m_heroInfo.level)
                        {
                            m_img_key_PolygonImage.gameObject.SetActive(true);
                        }
                        else
                        {
                            m_img_key_PolygonImage.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        m_img_key_PolygonImage.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (m_heroInfo != null)
                {
                    var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                    int heroEquip = m_heroInfo.GetHeroEquipByType(m_index);
                    if (heroEquip > 0 && m_heroInfo.data != null)
                    {
                        var equipInfo = bagProxy.GetEquipItemInfo(heroEquip);
                        var itemData = CoreUtils.dataService.QueryRecord<Data.ItemDefine>(equipInfo.ItemID);
                        SetBgColor(true,itemData.quality);
                        m_img_key_PolygonImage.gameObject.SetActive(false);
                        m_img_add_PolygonImage.gameObject.SetActive(false);

                    }
                    else
                    {
                        SetBgColor(false);
                    }
                }
            }
        }
        
        public void RefreshRedPoint()
        {
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            if (m_gridType == 0)
            {
                int redPointCount = bagProxy.GetNewEquipCountBySubtype(m_EquipType);
                if (redPointCount > 0)
                    m_UI_Common_Redpoint.ShowRedPoint(redPointCount);
                else
                    m_UI_Common_Redpoint.HideRedPoint();
            }
            else
            {
                if (m_EquipInfo != null && bagProxy.isNewItem(m_EquipInfo.ItemIndex))
                    m_UI_Common_Redpoint.m_img_redpoint_PolygonImage.gameObject.SetActive(true);
                else
                    m_UI_Common_Redpoint.HideRedPoint();
            }
        }

        private void SetTalentIcon(int talentIndex)
        {
            if (talentIndex > 0)
            {
                m_img_talent_PolygonImage.gameObject.SetActive(true);
                var talentData = CoreUtils.dataService.QueryRecord<Data.HeroTalentTypeDefine>(talentIndex);
                ClientUtils.LoadSprite(m_img_talent_PolygonImage,talentData.equipIcon);

                bool isEqual = false;
                foreach (var talentId in m_heroInfo.config.talent)
                {
                    var config = CoreUtils.dataService.QueryRecord<Data.HeroTalentDefine>(talentId);
                    if (config.type == talentIndex)
                    {
                        isEqual = true;
                        break;
                    }
                }

                GrayChildrens makeGray =
                    m_img_talent_PolygonImage.gameObject.GetComponent<GrayChildrens>();
                            
                if (isEqual)
                {
                    if (makeGray != null)
                    {
                        makeGray.Normal();
                    }
                }
                else
                {
                    if (makeGray == null)
                    {
                        makeGray = m_img_talent_PolygonImage.gameObject.AddComponent<GrayChildrens>();
                    }
                    makeGray.Gray();
                }

            }
            else
            {
                m_img_talent_PolygonImage.gameObject.SetActive(false);
            }
        }

        private void SetQualityColor(int quality)
        {
            m_img_color_PolygonImage.gameObject.SetActive(true);
            m_img_color_PolygonImage.color = GetQualityColor(quality);
        }

        private void SetEquipIcon(bool isNull,string iconPath, bool isGray)
        {
            if (isNull)
            {
                m_img_set_PolygonImage.gameObject.SetActive(true);
                m_img_equip_PolygonImage.gameObject.SetActive(false);
            }
            else
            {
                m_img_set_PolygonImage.gameObject.SetActive(false);
                m_img_equip_PolygonImage.gameObject.SetActive(true);
                ClientUtils.LoadSprite(m_img_equip_PolygonImage,iconPath);
                m_img_set_PolygonImage.color = isGray ? new Color(0.3f, 0.3f, 0.3f, 1f) : Color.white;
            }
            
        }

        private void SetBgColor(bool isEquip,int quality = 0)
        {
            if (isEquip)
            {
                ClientUtils.LoadSprite(m_img_color_PolygonImage,$"ui_hero[img_hero_equiprare{quality}]");
                m_img_color_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                m_img_color_PolygonImage.gameObject.SetActive(false);
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

        public void Normal()
        {
            m_img_selected_PolygonImage.gameObject.SetActive(false);
            SetTransparent(false);
        }

        public void Selected(bool isSelected)
        {
            if (isSelected)
            {
                m_img_selected_PolygonImage.gameObject.SetActive(true);
                SetTransparent(false);
            }
            else
            {
                m_img_selected_PolygonImage.gameObject.SetActive(false);
                SetTransparent(true);
            }
        }

        public void SetTransparent(bool isTransparent)
        {
            float alpha = 1;
            if (isTransparent)
            {
                alpha = 0.3f;
            }

            PolygonImage[] imgList = gameObject.transform.GetComponentsInChildren<PolygonImage>();
            foreach (var img in imgList)
            {
                var oriColor = img.color;
                img.color = new Color(oriColor.r,oriColor.g,oriColor.b,alpha);
            }
        }

        private void OnEquipClickEvent()
        {
            if (m_gridType == 0)
            {
                AppFacade.GetInstance().SendNotification(CmdConstant.ClickHeroEquip, m_EquipKey);
            }
            else
            {
                if (m_EquipInfo == null) return;
                var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
                if (bagProxy.isNewItem(m_EquipInfo.ItemIndex))
                {
                    m_UI_Common_Redpoint.HideRedPoint();
                    bagProxy.SetLocalItemToOld(m_EquipInfo.ItemIndex);
                    AppFacade.GetInstance().SendNotification(CmdConstant.RefreshEquipRedPoint);
                }
                AppFacade.GetInstance().SendNotification(CmdConstant.ClickHeroEquipItem, m_EquipInfo);
            }
        }
    }
}
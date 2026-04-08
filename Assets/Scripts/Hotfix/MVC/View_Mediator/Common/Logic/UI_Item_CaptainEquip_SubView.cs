// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年6月4日
// Update Time         :    2020年6月4日
// Class Description   :    UI_Item_CaptainEquip_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.Events;

namespace Game {
    public partial class UI_Item_CaptainEquip_SubView : UI_SubView
    {

        private HeroProxy.Hero m_Hero;
        private Dictionary<string,UI_Item_CaptainEquipUse_SubView> m_equipObjs = new Dictionary<string, UI_Item_CaptainEquipUse_SubView>();

        private bool m_isInited = false;

        private string m_curEquipKey;
        private EquipItemInfo m_curEquipInfo;
        
        private List<EquipItemInfo> m_equipList = new List<EquipItemInfo>();
        private int m_curListCount;

        private int LISTWIDTH = 838;

        private bool m_isListInited = false;
        private Dictionary<string, GameObject> m_dic = new Dictionary<string, GameObject>();
        
        protected override void BindEvent()
        {
            m_btn_title_GameButton.onClick.AddListener(OnShowTileClick);
            
            m_btn_drop.AddClickEvent(OnTakeOffEquipEvent);
            m_btn_change.AddClickEvent(OnReplaceEquipEvent);
            m_btn_use.AddClickEvent(OnWearEquipEvent);
            
            m_btn_closeButton_GameButton.onClick.AddListener(() =>
            {
                HideEquipList();
            });
            
            m_btn_closeButton1_GameButton.onClick.AddListener(() =>
            {
                HideEquipInfo();
            });
            
            m_btn_closeButton2_GameButton.onClick.AddListener(() =>
            {
                HideListEquipInfo();
            });
            
            
            SubViewManager.Instance.AddListener(new string[] {
                CmdConstant.ClickHeroEquip,
                CmdConstant.ClickHeroEquipItem,
                CmdConstant.RefreshEquipRedPoint,
                Hero_HeroWearEquip.TagName,
                Hero_TakeOffEquip.TagName
            },this.gameObject, OnNotification);
        }
        
        private void OnNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ClickHeroEquip:
                    string equipKey = notification.Body.ToString();
                    OnEquipClickEvent(equipKey);
                    break;
                case CmdConstant.ClickHeroEquipItem:
                    var equipInfo = notification.Body as EquipItemInfo;
                    OnListEquipClickEvent(equipInfo);
                    break;
                case Hero_HeroWearEquip.TagName:
                    var responseWear = notification.Body as Hero_HeroWearEquip.response;
                    if (responseWear.result)
                    {
                        HideListEquipInfo();
                        HideEquipList();
                        Refresh();
                    }
                    break;
                case Hero_TakeOffEquip.TagName:
                    var responseTakeOff = notification.Body as Hero_TakeOffEquip.response;
                    if (responseTakeOff.result)
                    {
                        HideEquipInfo();
                        Refresh();
                    }

                    break;
                case CmdConstant.RefreshEquipRedPoint:
                    Refresh(false,true);
                    break;
                default:
                    break;
            }
        }
        
        public void AddCharListener(UnityAction call)
        {
            m_btn_char_GameButton.onClick.AddListener(call);
        }

        private void Init()
        {
            m_isInited = true;
            var heroEquipTypes =  CoreUtils.dataService.QueryRecord<ConfigDefine>(0).heroEquipType;
            m_equipObjs.Clear();
            m_equipObjs.Add(heroEquipTypes[0] + "_1",m_UI_Item_EquipUse.m_UI_Equipslo1);
            m_equipObjs.Add(heroEquipTypes[1] + "_2",m_UI_Item_EquipUse.m_UI_Equipslo2);
            m_equipObjs.Add(heroEquipTypes[2] + "_3",m_UI_Item_EquipUse.m_UI_Equipslo3);
            m_equipObjs.Add(heroEquipTypes[3] + "_4",m_UI_Item_EquipUse.m_UI_Equipslo4);
            m_equipObjs.Add(heroEquipTypes[4] + "_5",m_UI_Item_EquipUse.m_UI_Equipslo5);
            m_equipObjs.Add(heroEquipTypes[5] + "_6",m_UI_Item_EquipUse.m_UI_Equipslo6);
            m_equipObjs.Add(heroEquipTypes[6] + "_7",m_UI_Item_EquipUse.m_UI_Equipslo7);
            m_equipObjs.Add(heroEquipTypes[7] + "_8",m_UI_Item_EquipUse.m_UI_Equipslo8);
            
            ClientUtils.PreLoadRes(gameObject, m_sv_equip_list_ListView.ItemPrefabDataList, LoadSupplyItemFinish);
        }
        
        private void LoadSupplyItemFinish(Dictionary<string, GameObject> dic)
        {
            m_dic = dic;
        }
        
        private void InitSupplyListItem(ListView.ListItem item)
        {
            UI_Item_CaptainEquipList_SubView subView = null;
            if (item.data == null)
            {
                subView = new UI_Item_CaptainEquipList_SubView(item.go.GetComponent<RectTransform>());
                item.data = subView;
            }
            else
            {
                subView = item.data as UI_Item_CaptainEquipList_SubView;
            }
            if (subView == null) return;
            if (item.index >= m_curListCount) return;
            
            int index = int.Parse(m_curEquipKey.Split('_')[1]);
            if (index == 1 || index == 8)
            {
                int listIndex = item.index * 3;
                var info1 = listIndex < m_equipList.Count ? m_equipList[listIndex] : null;
                listIndex++;
                var info2 = listIndex < m_equipList.Count ? m_equipList[listIndex] : null;
                listIndex++;
                var info3 = listIndex < m_equipList.Count ? m_equipList[listIndex] : null;
                subView.InitEquipItemInfo(info1,info2,info3,m_Hero);
            }
            else
            {
                if (item.index == 0)
                {
                    var info2 = 0 < m_equipList.Count ? m_equipList[0] : null;
                    var info3 = 1 < m_equipList.Count ? m_equipList[1] : null;
                    subView.InitEquipItemInfo(null,info2,info3,m_Hero);
                }
                else
                {
                    int listIndex = item.index * 3 - 1;

                    var info1 = listIndex < m_equipList.Count ? m_equipList[listIndex] : null;
                    listIndex++;

                    var info2 = listIndex < m_equipList.Count ? m_equipList[listIndex] : null;
                    listIndex++;

                    var info3 = listIndex < m_equipList.Count ? m_equipList[listIndex] : null;
                    subView.InitEquipItemInfo(info1,info2,info3,m_Hero);
                }
            }

        }

        public void SetHero(HeroProxy.Hero hero)
        {
            m_Hero = hero;
            var heroInfo = hero.config;

            if (!m_isInited)
            {
                Init();
            }

            m_lbl_heroname_LanguageText.text = LanguageUtils.getText(heroInfo.l_nameID);
            m_lbl_title_LanguageText.text = LanguageUtils.getText(heroInfo.l_appellationID);

            //var civilizationInfo = CoreUtils.dataService.QueryRecord<Data.CivilizationDefine>((int)heroInfo.civilization);
            //if (civilizationInfo != null)
            //{
            //    //ClientUtils.LoadSprite(m_img_titleimg_PolygonImage, civilizationInfo.civilizationMark);
            //    //Color c;
            //    //ColorUtility.TryParseHtmlString(civilizationInfo.markColour, out c);
            //    //m_img_titleimg_PolygonImage.color = c;
            //    //m_img_titleimg_PolygonImage.enabled = true;
            //}
            //else
            //{
            //    m_img_titleimg_PolygonImage.enabled = false;
            //}

            m_UI_Model_CaptainTalent1.SetTalentId(heroInfo.talent[0]);
            m_UI_Model_CaptainTalent2.SetTalentId(heroInfo.talent[1]);
            m_UI_Model_CaptainTalent3.SetTalentId(heroInfo.talent[2]);

            m_UI_Model_CaptainStar1.setHight(hero.star > 0);
            m_UI_Model_CaptainStar2.setHight(hero.star > 1);
            m_UI_Model_CaptainStar3.setHight(hero.star > 2);
            m_UI_Model_CaptainStar4.setHight(hero.star > 3);
            m_UI_Model_CaptainStar5.setHight(hero.star > 4);
            m_UI_Model_CaptainStar6.setHight(hero.star > 5);

            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;

            foreach (var keyValue in m_equipObjs)
            {
                keyValue.Value.InitEquipForHero(keyValue.Key,m_Hero);
            }
            
        }

        private void Refresh(bool isRefreshInfo = true,bool isRefreshRedPoint= true )
        {
            foreach (var keyValue in m_equipObjs)
            {
                if (isRefreshInfo)
                    keyValue.Value.Refresh();
                if (isRefreshRedPoint)
                    keyValue.Value.RefreshRedPoint();
            }
        }

        private void ShowListEquipInfo(EquipItemInfo equipInfo)
        {
            m_curEquipInfo = equipInfo;
            if (equipInfo == null) return;
            var itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(equipInfo.ItemID);
            var equipDefine = CoreUtils.dataService.QueryRecord<EquipDefine>(equipInfo.ItemID);
            m_pl_mask2_ViewBinder.gameObject.SetActive(true);
            
            m_UI_Model_Equip2.Init(equipInfo);
            m_lbl_name2_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
            m_lbl_name2_LanguageText.color = GetQualityColor(itemDefine.quality);
            m_lbl_lv2_LanguageText.text = LanguageUtils.getTextFormat(182067, equipDefine.useLevel);
            
            m_UI_Item_EquipAtt2.SetEquipItemInfo(equipInfo,true);
            
            var makeChildrenGray = m_btn_use.gameObject.GetComponent<GrayChildrens>();
            if (makeChildrenGray == null)
                makeChildrenGray = m_btn_use.gameObject.AddComponent<GrayChildrens>();
            
            if (equipDefine.useLevel > m_Hero.data.level)
            {
                makeChildrenGray.Gray();
            }
            else
            {
                makeChildrenGray.Normal();
            }

            if (m_Hero.data == null || (equipInfo.HeroID > 0 && equipInfo.HeroID == m_Hero.data.heroId))
            {
                m_btn_use.gameObject.SetActive(false);
            }
            else
            {
                m_btn_use.gameObject.SetActive(true);
            }

        }

        private void ShowBodyEquipInfo(string equipKey)
        {
            var clickEquip = m_equipObjs[equipKey];

            if (clickEquip == null) return;
            
            int equipType = int.Parse(equipKey.Split('_')[0]);
            int index = int.Parse(equipKey.Split('_')[1]);
            
            int heroEquip = m_Hero.GetHeroEquipByType(index);
            if (heroEquip <= 0) return;
            
            m_curEquipKey = equipKey;

            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            var heroEquipInfo = bagProxy.GetEquipItemInfo(heroEquip);
            var itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(heroEquipInfo.ItemID);
            var equipDefine = CoreUtils.dataService.QueryRecord<EquipDefine>(heroEquipInfo.ItemID);
            
            foreach (var keyValue in m_equipObjs)
            {
                if (keyValue.Key.Equals(equipKey))
                {
                    keyValue.Value.Selected(true);
                }
                else
                {
                    keyValue.Value.Selected(false);
                }
            }
            
            m_UI_Model_Equip1.Init(heroEquipInfo);
            m_lbl_name1_LanguageText.text = LanguageUtils.getText(itemDefine.l_nameID);
            m_lbl_name1_LanguageText.color = GetQualityColor(itemDefine.quality);
            m_lbl_lv1_LanguageText.text = LanguageUtils.getTextFormat(182067, equipDefine.useLevel);
            
            m_UI_Item_EquipAtt1.SetEquipItemInfo(heroEquipInfo,true);

            m_pl_mask1.gameObject.SetActive(true);
            
            if (LanguageUtils.IsArabic())
            {
                m_pl_equip_mes1_CanvasGroup.gameObject.GetComponent<RectTransform>().pivot = new Vector2(0,0.7f);
            }
            else
            {
                m_pl_equip_mes1_CanvasGroup.gameObject.GetComponent<RectTransform>().pivot = new Vector2(1,0.7f);
            }

            Vector2 arrpos = Vector2.zero;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_pl_mask1,
                CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(clickEquip.gameObject.transform.position),
                CoreUtils.uiManager.GetUICamera(), out arrpos);
            m_pl_equip_mes1_CanvasGroup.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(arrpos.x, arrpos.y);

            Vector3[] corners = new Vector3[4];
            m_pl_equip_mes1_CanvasGroup.gameObject.GetComponent<RectTransform>().GetWorldCorners(corners);

            Vector3 zeroPoint = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[0]);
            Vector3 onePoint = CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(corners[2]);

            RectTransform canvasRect = CoreUtils.uiManager.GetCanvas().transform as RectTransform;
 
            float screenHeight = canvasRect.rect.height;
            Vector2 offset = new Vector2();
            
            if (zeroPoint.y < 0)
            {
                offset -= new Vector2(0, zeroPoint.y * (screenHeight / Screen.height));
            }
            else if (onePoint.y > Screen.height)
            {
                offset += new Vector2(0, (Screen.height - onePoint.y) * (screenHeight / Screen.height));
            }
            m_pl_equip_mes1_CanvasGroup.gameObject.GetComponent<RectTransform>().anchoredPosition = arrpos + offset;

            

        }

        private void ShowEquipList(string equipKey)
        {
            m_pl_mask.gameObject.SetActive(true);
            if (!m_isListInited)
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = InitSupplyListItem;
                m_sv_equip_list_ListView.SetInitData(m_dic, functab);
                m_isListInited = true;
            }

            var clickEquip = m_equipObjs[equipKey];

            if (clickEquip == null) return;
            
            int equipType = int.Parse(equipKey.Split('_')[0]);
            int index = int.Parse(equipKey.Split('_')[1]);

            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            
            m_equipList.Clear();
            m_equipList = bagProxy.GetEquipItemsBySubType(equipType);

            if (m_equipList.Count <= 0)
            {
                Tip.CreateTip(166113,Tip.TipStyle.Middle).Show();
            }
            else
            {
                
                m_curEquipKey = equipKey;
                
                foreach (var keyValue in m_equipObjs)
                {
                    if (keyValue.Key.Equals(equipKey))
                    {
                        keyValue.Value.Selected(true);
                    }
                    else
                    {
                        keyValue.Value.Selected(false);
                    }
                }
                
                AppFacade.GetInstance().SendNotification(CmdConstant.HeroListVisible,false);
            
                Vector2 arrpos = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(m_pl_equip_list_ArabLayoutCompment.transform.GetComponent<RectTransform>(), CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(clickEquip.gameObject.transform.position), CoreUtils.uiManager.GetUICamera(), out arrpos);
            
                Vector2 pos = Vector2.zero;
            
                if (LanguageUtils.IsArabic())
                {
                    if (index == 1)
                    {
                        pos = new Vector2(arrpos.x + 10,arrpos.y - 80);
                    }
                    else if (index == 8)
                    {
                        pos = new Vector2(arrpos.x + 10,arrpos.y + 80);
                    }
                    else
                    {
                        pos = new Vector2(arrpos.x - 60,arrpos.y);
                    }
                }
                else
                {
                    if (index == 1)
                    {
                        pos = new Vector2(arrpos.x - 10,arrpos.y - 80);
                    }
                    else if (index == 8)
                    {
                        pos = new Vector2(arrpos.x - 10,arrpos.y + 80);
                    }
                    else
                    {
                        pos = new Vector2(arrpos.x + 60,arrpos.y);
                    }
                }
            
                m_sv_equip_list_ListView.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
                m_equipList.Sort((EquipItemInfo a, EquipItemInfo b) =>
                {
                    if (a.ItemID == b.ItemID)
                    {
                        return 0.CompareTo((int)a.HeroID);
                    }
                    else
                    {
                        return b.ItemID.CompareTo(a.ItemID);
                    }
                });
                
                m_curListCount = 0;
                if (index == 1 || index == 8)
                {
                    m_curListCount = (int)Mathf.Ceil((float) m_equipList.Count / 3.0f);
                }
                else
                {
                    m_curListCount = (int)Mathf.Ceil((float) (m_equipList.Count - 2) / 3.0f) + 1;
                }

                m_sv_equip_list_ListView.FillContent(m_curListCount);
                var m_c_equip_list = m_c_equip_list_ArabLayoutCompment.GetComponent<RectTransform>();
                if (LanguageUtils.IsArabic())
                {
                    m_c_equip_list.anchorMin = new Vector2(0,0.5f);
                    m_c_equip_list.anchorMax = new Vector2(0,0.5f);
                    m_c_equip_list.pivot = new Vector2(0,0.5f);
                }

                var scrollView = m_sv_equip_list_ListView.GetComponent<ScrollView>();
                var listRectTransform = m_sv_equip_list_ListView.GetComponent<RectTransform>();
                var rect = listRectTransform.rect;
                if (m_c_equip_list.rect.width < LISTWIDTH)
                {
                    listRectTransform.sizeDelta = new Vector2(m_c_equip_list.rect.width,rect.height);
                    if (scrollView != null)
                    {
                        scrollView.enabled = false;
                    }
                }
                else
                {
                    listRectTransform.sizeDelta = new Vector2(LISTWIDTH,rect.height);
                    if (scrollView != null)
                    {
                        scrollView.enabled = true;
                    }
                }
                m_sv_equip_list_ListView.ForceRefresh();
            }
            
        }

        private void HideListEquipInfo()
        {
            if (m_pl_mask2_ViewBinder.gameObject.activeSelf)
            {
                m_pl_mask2_ViewBinder.gameObject.SetActive(false);
                m_curEquipInfo = null;
            }
        }

        private void HideEquipInfo()
        {
            if (m_pl_mask1.gameObject.activeSelf)
            {
                m_pl_mask1.gameObject.SetActive(false);
                foreach (var keyValue in m_equipObjs)
                {
                    keyValue.Value.Normal();

                }

                m_curEquipKey = String.Empty;
            }
        }

        private void HideEquipList()
        {
            if (m_pl_mask.gameObject.activeSelf)
            {
                m_pl_mask.gameObject.SetActive(false); 
                m_sv_equip_list_ListView.FillContent(0);
                foreach (var keyValue in m_equipObjs)
                {
                    keyValue.Value.Normal();

                }

                m_curEquipKey = String.Empty;
                AppFacade.GetInstance().SendNotification(CmdConstant.HeroListVisible, true);
            }
        }

        private void OnShowTileClick()
        {
            var civilizationInfo = CoreUtils.dataService.QueryRecord<Data.CivilizationDefine>((int)m_Hero.config.civilization);
            var data1 = LanguageUtils.getText(civilizationInfo.l_civilizationID);
            HelpTip.CreateTip(data1, m_btn_title_GameButton.transform ).SetStyle(HelpTipData.Style.arrowUp).Show();
        }

        private void OnEquipClickEvent(string equipKey)
        {
            var clickEquip = m_equipObjs[equipKey];

            if (clickEquip == null) return;
            
            int equipType = int.Parse(equipKey.Split('_')[0]);
            int index = int.Parse(equipKey.Split('_')[1]);
            
            int heroEquip = m_Hero.GetHeroEquipByType(index);

            if ( heroEquip > 0)
            {
                ShowBodyEquipInfo(equipKey);
            }
            else
            {
                ShowEquipList(equipKey);
            }
        }

        private void OnListEquipClickEvent(EquipItemInfo equipInfo)
        {
            ShowListEquipInfo(equipInfo);
        }

        private void OnWearEquipEvent()
        {
            if (m_curEquipInfo == null || string.IsNullOrEmpty(m_curEquipKey)) return;
            
            var equipData = CoreUtils.dataService.QueryRecord<Data.EquipDefine>(m_curEquipInfo.ItemID);
            if (equipData.useLevel > m_Hero.data.level)
            {
                Tip.CreateTip(166111,equipData.useLevel).Show();
                return;
            }
            
            var troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            bool isWarByHero = troopProxy.IsWarByHero((int)m_Hero.data.heroId);
            if (isWarByHero)
            {
                Tip.CreateTip(182047).Show();
                return;
            }

            int equipType = int.Parse(m_curEquipKey.Split('_')[0]);
            int index = int.Parse(m_curEquipKey.Split('_')[1]);
            if (m_curEquipInfo.HeroID > 0)
            {
                var heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
                var heroInfo = heroProxy.GetHeroByID(m_curEquipInfo.HeroID);
                
                Alert.CreateAlert(LanguageUtils.getTextFormat(166112,LanguageUtils.getText(heroInfo.config.l_nameID))).SetLeftButton().SetRightButton(() =>
                {
                    SendWearEquip((int)m_Hero.data.heroId,(int)m_curEquipInfo.ItemIndex,index);
                }).Show();
            }
            else
            {
                SendWearEquip((int)m_Hero.data.heroId,(int)m_curEquipInfo.ItemIndex,index);
            }
        }

        private void OnTakeOffEquipEvent()
        {
            if (string.IsNullOrEmpty(m_curEquipKey)) return;
            int equipType = int.Parse(m_curEquipKey.Split('_')[0]);
            int index = int.Parse(m_curEquipKey.Split('_')[1]);

            int equipIndex = m_Hero.GetHeroEquipByType(index);
            if (equipIndex > 0)
            {
                var troopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
                bool isWarByHero = troopProxy.IsWarByHero((int) m_Hero.data.heroId);
                if (isWarByHero)
                {
                    Tip.CreateTip(182047).Show();
                    return;
                }

                SendTakeOffEquip((int)m_Hero.data.heroId,index);
            }

        }

        private void OnReplaceEquipEvent()
        {
            string equipkey = m_curEquipKey;
            HideEquipInfo();
            ShowEquipList(equipkey);
        }

        private void SendWearEquip(int heroId, int itemIndex, int equipIndex)
        {
            Hero_HeroWearEquip.request request = new Hero_HeroWearEquip.request()
            {
                heroId = heroId,
                itemIndex = itemIndex,
                equipIndex = equipIndex
            };
            AppFacade.GetInstance().SendSproto(request);
        }

        private void SendTakeOffEquip(int heroId,int equipIndex)
        {
            Hero_TakeOffEquip.request request = new Hero_TakeOffEquip.request()
            {
                heroId = heroId,
                equipIndex = equipIndex
            };
            AppFacade.GetInstance().SendSproto(request);
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